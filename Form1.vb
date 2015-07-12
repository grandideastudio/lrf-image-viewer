Public Class Form1
    ' CONSTANTS
    ' Frame buffer resolutions
    ' Full frame
    Private Const _FB_X_FULL As Integer = 160   ' 8 bits/pixel greyscale, only using the Y/luma component
    Private Const _FB_Y_FULL As Integer = 128

    ' ROI (Region-of-Interest), used for actual laser range finding operations
    ' FW 1.x (Original)
    Private Const _FB_X_ROI As Integer = 640    ' 16 bits/pixel YUV422 color
    Private Const _FB_Y_ROI As Integer = 16

    ' FW 2.x (Optimized)
    Private Const _FB_X_ROI_2 As Integer = 320  ' 8 bits/pixel greyscale, only using the Y/luma component
    Private Const _FB_Y_ROI_2 As Integer = 16

    ' Range finding (optional, since this can be done directly on the LRF module, as well)
    Private Const H_CM As Double = 7.8                  ' Distance between centerpoints of camera and laser, fixed based on PCB layout (cm)
    Private Const ANGLE_MIN As Double = 0.030699015     ' Minimum allowable angle (radians) = arctan(h/D) (corresponds to D = 254 cm)

    ' Image processing/blob finding
    Private Const _SUM_THRESHOLD As Integer = 3     ' Threshold that column sum must be above in order to be considered part of the blob
    Private Const _MAX_BLOBS As Integer = 6         ' Maximum number of blobs to detect within the frame


    ' GLOBAL VARIABLES
    Dim WithEvents serialPort As New IO.Ports.SerialPort
    Public fg_flag As Byte      ' set to 1 when a frame grab is in progress
    Public fw_ver As Byte       ' major firmware version (1, 2, ...)
    Public myStr As String      ' global string used to store input from serial port
    Public bmp As Bitmap        ' bitmap object to store frame image
    ' in VB.NET, arrays are initialized with the UpperBound of the array (number of elements - 1)
    Public buf((_FB_X_FULL * _FB_Y_FULL) - 1) As Byte       ' serial port input buffer in byte form (number of bytes in frame should be <= total number of elements)
    Public yuv444((_FB_X_ROI * _FB_Y_ROI) - 1) As YUV       ' buffer for color processing 


    ' Image processing/blob finding
    Structure blobStruct
        Public left As Integer                      ' X coordinate of left side (beginning) of detected blob
        Public right As Integer                     ' X coordinate of right side (end) of detected blob
        Public mass As Integer                      ' Mass of blob (sum of all valid pixels within the blob)
        Public centroid As Integer                  ' Centroid (center of mass) of blob
    End Structure

    Public fb_bool(_FB_Y_ROI, _FB_X_ROI) As Integer ' Detection map: 1 = pixel is within our desired color bounds, 0 = otherwise
    Public roi(_FB_X_ROI) As Integer                ' Array of detected pixels per X coordinate
    Public blob(_MAX_BLOBS) As blobStruct


    ' Functions/objects
    Private Sub Form1_Load( _
       ByVal sender As System.Object, _
       ByVal e As System.EventArgs) _
       Handles MyBase.Load

        For i As Integer = 0 To _
           My.Computer.Ports.SerialPortNames.Count - 1
            cbbCOMPorts.Items.Add( _
               My.Computer.Ports.SerialPortNames(i))
        Next
        cbbCOMPorts.SelectedIndex = 0

        ' Fill available baud rate speeds into selection box
        boxBaud.Items.Add("9600")
        boxBaud.Items.Add("19200")
        boxBaud.Items.Add("38400")
        boxBaud.Items.Add("57600")
        boxBaud.Items.Add("115200")
        boxBaud.SelectedIndex = 4           ' Select 115.2kbps as default
        btnDisconnect.Enabled = False
        AcceptButton = btnSend              ' Assign Send button to be the form's primary button when Enter is pressed 

        fg_flag = 0
        fw_ver = 0
    End Sub


    Private Sub DataReceived( _
       ByVal sender As Object, _
       ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) _
           Handles serialPort.DataReceived  ' is any data received from the serial port?

        If (fg_flag = 0) Then  ' if we're not in the middle of grabbing a frame...
            txtDataReceived.Invoke(New  _
             myDelegate(AddressOf updateTextBox), _
            New Object() {})    ' ...then print the received text to the text box
        End If

    End Sub

    'Used to Send Messages to the control and will be used with a request for the current line number in the textbox control.
    'From http://www.vbforfree.com/tag/sb_bottom/
    Private Declare Function SendMessage Lib "user32.dll" Alias "SendMessageA" (ByVal winHandle As Int32, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As Int32) As Int32

    'Constants used for the SendMessage API call function.
    Private Const EM_SCROLL = &HB5
    Private Const SB_BOTTOM = 7
    Private Const SB_TOP = 6

    Public Delegate Sub myDelegate()
    Public Sub updateTextBox()
        With txtDataReceived
            .SelectionColor = Color.White
            .AppendText(serialPort.ReadExisting)  ' read all existing bytes from the serial port buffer and print to screen
        End With
        SendMessage(txtDataReceived.Handle.ToInt32, EM_SCROLL, SB_BOTTOM, 0)
    End Sub


    Private Sub frame_grab() ' grab a single frame from the LRF module and display it on the screen
        '    Try
        Dim i, ix, iy As Integer
        Dim val As Integer

        txtMessage.AppendText(Convert.ToString("Grabbing frame..."))
        txtMessage.Update() ' force update of text box

        fg_flag = 1                   ' set flag
        serialPort.DiscardInBuffer()  ' clear any contents of serial input buffer

        If radFrameFull.Checked Then    ' Full frame (greyscale)
            serialPort.Write("G")       ' send command to LRF

            myStr = serialPort.ReadTo("END" & vbCr & ":")  ' read contents of serial port buffer (into a String) until the entire frame has been sent
            ' convert String into an array of bytes
            buf = System.Text.Encoding.GetEncoding(1252).GetBytes(myStr)   ' 1252 is the default codepage on US Windows

            ' fill in bitmap with pixel data
            ' 8 bits/pixel, only using the Y/luma component
            For iy = 0 To (_FB_Y_FULL - 1)        ' for each row of Y
                For ix = 0 To (_FB_X_FULL - 1)       ' for each column of X
                    i = (iy * _FB_X_FULL) + ix         ' calculate index into array
                    val = buf(i)
                    bmp.SetPixel(ix, iy, Color.FromArgb(val, val, val))
                Next
            Next
        ElseIf radFrameROI.Checked Or radFrameROIProc.Checked Then   ' ROI (color for FW 1.x, greyscale for FW 2.x)
            If radFrameROI.Checked Then     ' ROI (region of interest), single frame with no processing
                serialPort.Write("C")           ' send command to LRF
            Else                            ' ROI (region of interest), single frame with processing (double frame grab with laser off/on and background subtracted)
                serialPort.Write("P")           ' send command to LRF
            End If

            myStr = serialPort.ReadTo("END" & vbCr & ":")  ' read contents of serial port buffer (into a String) until the entire frame has been sent
            ' convert String into an array of bytes
            buf = System.Text.Encoding.GetEncoding(1252).GetBytes(myStr)   ' 1252 is the default codepage on US Windows

            If (fw_ver = 1) Then  ' Original
                ' 16 bits/pixel YUV422
                ' ColorHelper object and conversion routines from http://www.codeproject.com/KB/recipes/colorspace1.aspx
                ' Y must be in [0, 1]
                ' U must be in [-0.436, +0.436]
                ' V must be in [-0.615, +0.615]

                ' unpack YUV422 (16 bits/pixel) into YUV444 (24 bits/pixel, 1 byte per Y/U/V) and store in our yuv444 array
                ' code based lightly on etheora_422to444() function (http://svn.xiph.org/branches/etheora-0.1.1/src/etheora.c)

                ' first, copy Y values
                ' Y/luma component is unique for every pixel of the packed YUV422 format
                For i = 0 To (buf.Length - 1) Step 2
                    yuv444(i / 2).Y = Convert.ToDouble(buf(i) / 255.0)             ' Normalize value as required by the YUV structure
                Next

                ' then, upsample CbCr/UV (horizontal upconversion by a factor of 2)
                Dim idx As Integer
                For i = 1 To buf.Length Step 4
                    val = buf(i)
                    'val = 102            ' constant value for greyscale (for testing)
                    idx = i / 2
                    yuv444(idx).U = -0.436 + (Convert.ToDouble(val / 235.0))      ' Normalize value as required by the YUV structure
                    yuv444(idx + 1).U = -0.436 + (Convert.ToDouble(val / 235.0))  ' Make odd column in U/Cb plane equal to the even one
                Next

                For i = 3 To buf.Length Step 4
                    val = buf(i)
                    'val = 144            ' constant value for greyscale (for testing)
                    idx = (i - 2) / 2
                    yuv444(idx).V = -0.615 + (Convert.ToDouble(val / 235.0))      ' Normalize value as required by the YUV structure
                    yuv444(idx + 1).V = -0.615 + (Convert.ToDouble(val / 235.0))  ' Make odd column in V/Cr plane equal to the even one
                Next

                ' fill in bitmap with pixel data
                For iy = 0 To (_FB_Y_ROI - 1)        ' for each row of Y
                    For ix = 0 To (_FB_X_ROI - 1)       ' for each column of X
                        i = (iy * _FB_X_ROI) + ix         ' calculate index into array
                        bmp.SetPixel(ix, iy, ColorHelper.YUVtoColor(yuv444(i)))
                    Next
                Next
            ElseIf (fw_ver = 2) Then ' Optimized
                ' 8 bits/pixel, only using the Y/luma component
                ' fill in bitmap with pixel data
                For iy = 0 To (_FB_Y_ROI_2 - 1)        ' for each row of Y
                    For ix = 0 To (_FB_X_ROI_2 - 1)       ' for each column of X
                        i = (iy * _FB_X_ROI_2) + ix         ' calculate index into array
                        val = buf(i)
                        bmp.SetPixel(ix, iy, Color.FromArgb(val, val, val))
                    Next
                Next
            End If
        End If

        fg_flag = 0         ' frame grab is done, so clear the flag
        txtMessage.AppendText(Convert.ToString("DONE!") & vbCr)
        txtMessage.ScrollToCaret()

        If radFrameROIProc.Checked And chkBlob.Checked Then   ' if user wants to see tracking data w/ blob detection
            blob_detection()                                    ' find the blobs within the frame
        Else                                                  ' otherwise, clear any information from the text boxes
            lblPFC.Text = String.Empty
            lblRangeCm.Text = String.Empty
            lblRangeIn.Text = String.Empty
        End If

        pnlBitmap.Refresh()         ' force painting of the panel to update the image

        btnSave.Enabled = True      ' enable save button now that we have a bitmap available
        radSaveBitmap.Enabled = True
        radSaveRaw.Enabled = True
    End Sub


    Private Sub blob_detection()  ' locate blob(s) within the frame
        Try
            Dim ix, iy As Integer
            Dim val As Integer

            If (fw_ver = 1) Then  ' Original
                ' initialize variables
                Dim upper_bound, lower_bound As YUV     ' tracking parameters, pixel must be within these bounds in order to be considered
                lower_bound.Y = Convert.ToDouble(yUDLower.Value / 100.0)               ' normalize values as required by the YUV structure
                lower_bound.U = -0.436 + (Convert.ToDouble(uUDLower.Value / 100.0))
                lower_bound.V = -0.615 + (Convert.ToDouble(vUDLower.Value / 100.0))
                upper_bound.Y = Convert.ToDouble(yUDUpper.Value / 100.0)
                upper_bound.U = -0.436 + (Convert.ToDouble(uUDUpper.Value / 100.0))
                upper_bound.V = -0.615 + (Convert.ToDouble(vUDUpper.Value / 100.0))

                ' threshold each pixel based on the lower and upper color bounds
                For iy = 0 To (_FB_Y_ROI - 1)           ' for each row of Y
                    For ix = 0 To (_FB_X_ROI - 1)          ' for each column of X 
                        val = (iy * _FB_X_ROI) + ix                                                 ' get index of current pixel
                        If ((yuv444(val) >= lower_bound) And (yuv444(val) <= upper_bound)) Then     ' if pixel's YUV components are within the lower and upper color bounds...
                            bmp.SetPixel(ix, iy, Color.White)                                       ' for easier visualization/testing, convert each pixel to pure black and white
                            fb_bool(iy, ix) = 1                                                     ' store result in a binary array, 1 = pixel is within bounds, 0 = otherwise
                        Else
                            bmp.SetPixel(ix, iy, Color.Black)
                            fb_bool(iy, ix) = 0
                        End If
                    Next
                Next
            ElseIf (fw_ver = 2) Then ' Optimized
                ' initialize variables
                Dim upper_bound, lower_bound As Byte        ' tracking parameters, pixel must be within these bounds in order to be considered
                lower_bound = yUDLower.Value
                upper_bound = yUDUpper.Value

                ' threshold each pixel based on the lower and upper greyscale bounds
                For iy = 0 To (_FB_Y_ROI_2 - 1)           ' for each row of Y
                    For ix = 0 To (_FB_X_ROI_2 - 1)          ' for each column of X 
                        val = (iy * _FB_X_ROI_2) + ix                                                 ' get index of current pixel
                        If ((buf(val) >= lower_bound) And (buf(val) <= upper_bound)) Then           ' if pixel's Y component is within the lower and upper bounds...
                            bmp.SetPixel(ix, iy, Color.White)                                       ' for easier visualization/testing, convert each pixel to pure black and white
                            fb_bool(iy, ix) = 1                                                     ' store result in a binary array, 1 = pixel is within bounds, 0 = otherwise
                        Else
                            bmp.SetPixel(ix, iy, Color.Black)
                            fb_bool(iy, ix) = 0
                        End If
                    Next
                Next
            End If

            ' column sum
            ' keep a sum of detected pixels per column of the image
            ' basically creating a 1-D array/histogram instead of a 2-D
            If (fw_ver = 1) Then ' Original
                For ix = 0 To (_FB_X_ROI / 2 - 1)      ' for each column X within our region-of-interest (we only care about the left side of the frame - anything on the right is not our laser)
                    roi(ix) = 0                          ' clear count
                    For iy = 0 To (_FB_Y_ROI - 1)        ' for each row Y
                        roi(ix) += fb_bool(iy, ix)         ' add all of the detected pixels within the column
                    Next
                Next
            ElseIf (fw_ver = 2) Then ' Optimized
                For ix = 0 To (_FB_X_ROI_2 - 1)      ' for each column X within our region-of-interest (we only care about the left side of the frame - anything on the right is not our laser)
                    roi(ix) = 0                        ' clear count
                    For iy = 0 To (_FB_Y_ROI_2 - 1)    ' for each row Y
                        roi(ix) += fb_bool(iy, ix)       ' add all of the detected pixels within the column
                    Next
                Next
            End If

            ' find the blobs
            ' search through the 1-D array to find the blobs and determine their start and end coordinates
            Dim found_blob As Boolean = False   ' flag set while there is a blob currently being processed
            Dim num_blobs As Integer = 0        ' number of detected blobs
            Dim roi_counter As Integer

            If (fw_ver = 1) Then ' Original
                roi_counter = _FB_X_ROI / 2
            ElseIf (fw_ver = 2) Then ' Optimized
                roi_counter = _FB_X_ROI_2
            End If
            For ix = 0 To (roi_counter - 1)     ' for each column X of our region-of-interest                
                If ((roi(ix) > _SUM_THRESHOLD) And found_blob = False) Then     ' we've found the beginning of a blob
                    num_blobs += 1                                                  ' increment blob count
                    If (num_blobs > _MAX_BLOBS) Then                                ' limit maximum number of blobs detected in a single frame
                        num_blobs = _MAX_BLOBS
                        Exit For ' stop searching for blobs in frame if we've reached our limit
                    End If

                    blob(num_blobs - 1).left = ix       ' save the location
                    found_blob = True
                ElseIf ((roi(ix) <= _SUM_THRESHOLD) And found_blob = True) Then ' we've found the end of the blob
                    blob(num_blobs - 1).right = ix - 1  ' save the location
                    found_blob = False
                End If
            Next

            If num_blobs = 0 Then   ' if no blobs detected...
                txtMessage.AppendText(Convert.ToString("No blobs detected!") & vbCr)
                txtMessage.ScrollToCaret()

                lblPFC.Text = String.Empty          ' clear any information from the text boxes
                lblRangeCm.Text = String.Empty
                lblRangeIn.Text = String.Empty
            Else  ' if one or more blobs exist...
                txtMessage.AppendText(Convert.ToString("Blobs detected: ") & Convert.ToString(num_blobs) & vbCr)
                txtMessage.ScrollToCaret()

                ' calculate centroid and mass for each detected blob
                For ix = 0 To (num_blobs - 1)
                    Dim temp_sum As Integer = 0
                    blob(ix).mass = 0               ' clear count

                    For iy = 0 To (blob(ix).right - blob(ix).left)  ' for each column X within the blob
                        val = roi(blob(ix).left + iy)                   ' get the current column sum
                        blob(ix).mass += val                            ' add column sum to the total mass of the blob
                        temp_sum += (iy + 1) * val                      ' weighted sum of all positive pixels in the blob
                    Next

                    blob(ix).centroid = (temp_sum / blob(ix).mass) + blob(ix).left - 1               ' calculate centroid (e.g., where in the blob the weight is centered)
                    'blob(ix).centroid = ((blob(ix).right - blob(ix).left) / 2) + blob(ix).left      ' simpler centroid calculation, assumes blob is well balanced (equal mass on both sides of the mean)

                    ' display results
                    txtMessage.AppendText(Convert.ToString(ix) & Convert.ToString(": L = ") & Convert.ToString(blob(ix).left))
                    txtMessage.AppendText(Convert.ToString(" R = ") & Convert.ToString(blob(ix).right))
                    txtMessage.AppendText(Convert.ToString(" Mass = ") & Convert.ToString(blob(ix).mass))
                    txtMessage.AppendText(Convert.ToString(" Centroid = ") & Convert.ToString(blob(ix).centroid) & vbCr)
                    txtMessage.ScrollToCaret()
                Next

                ' determine and highlight the blob with the largest mass (this is likely our laser spot)
                ' if there are two blobs with the same mass, the first occurrence remains the maximum/primary
                Dim max As Integer = 0          ' index into the blob array and points to the largest mass
                If (fw_ver = 1) Then ' Original
                    found_blob = True ' unused variable for original firmware
                    For ix = 0 To (num_blobs - 1)
                        If (blob(ix).mass > blob(max).mass) Then
                            max = ix
                        End If
                    Next
                ElseIf (fw_ver = 2) Then ' Optimized
                    found_blob = False
                    For ix = 0 To (num_blobs - 1)
                        If (blob(ix).mass > UDBlobMass.Value) Then    ' if blob is greater than minimum threshold mass (otherwise ignore it)
                            If (found_blob = False Or (blob(ix).mass > blob(max).mass)) Then
                                max = ix
                                found_blob = True
                            End If
                        End If
                    Next
                End If

                If (found_blob = False) Then
                    txtMessage.AppendText(Convert.ToString("Primary blob: None") & vbCr)
                Else
                    txtMessage.AppendText(Convert.ToString("Primary blob: ") & Convert.ToString(max) & vbCr)
                End If
                txtMessage.ScrollToCaret()

                Dim roi_max As Integer
                If (fw_ver = 1) Then ' Original
                    roi_max = _FB_Y_ROI
                ElseIf (fw_ver = 2) Then ' Optimized
                    roi_max = _FB_Y_ROI_2
                End If

                If radTrackBounds.Checked Then           ' draw vertical lines to bound the blob
                    For iy = 0 To (roi_max - 1)        ' for each row of Y
                        bmp.SetPixel(blob(max).left - 1, iy, Color.Red)     ' left line
                        bmp.SetPixel(blob(max).right - 1, iy, Color.Red)    ' right line
                    Next
                ElseIf radTrackCentroid.Checked Then     ' draw vertical line to intersect centroid
                    For iy = 0 To (roi_max - 1)        ' for each row of Y
                        bmp.SetPixel(blob(max).centroid, iy, Color.Red)
                    Next
                End If

                get_range(blob(max).centroid)   ' now that we have the centroid of the primary blob, calculate the range
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            StreamStop()
        End Try
    End Sub


    Private Sub get_range(ByVal centroid_x As Integer)  ' calculate distance from LRF module to target object
        Try
            Dim pixels_from_center As Integer
            Dim range, angle As Double
            Dim str As String

            Dim slope, intercept As Double  ' Best-fit slope-intercept linear equation values
            Dim pfc_min As Integer          ' Minimum allowable pixels_from_center value

            If (fw_ver = 1) Then ' Original
                pixels_from_center = Math.Abs((_FB_X_ROI / 2) - centroid_x)  ' calculate the number of pixels from center of frame
            ElseIf (fw_ver = 2) Then ' Optimized
                pixels_from_center = Math.Abs(_FB_X_ROI_2 - centroid_x)  ' calculate the number of pixels from center of frame
            End If

            lblPFC.Text = Convert.ToString(pixels_from_center)

            ' Grab SLOPE and INTERCEPT values from the text boxes
            ' These are specific for each unit based on manufacturing & assembly tolerances
            ' and can be calculated by taking a number of measurements from known distances and recording resultant pfc values
            slope = Val(txtSlope.Text())                ' we're not doing any bounds checking here, so hopefully the values are properly entered
            intercept = Val(txtIntercept.Text())
            'pfc_min = (ANGLE_MIN - intercept) / slope
            pfc_min = 0

            ' calculate range in cm based on centroid of primary blob
            ' D = h / tan(theta)
            If (pixels_from_center >= pfc_min) Then     ' if the pfc value is greater than our minimum (e.g., less than our maximum defined distance)
                ' use a best-fit slope-intercept linear equation (based on calibration measurements) to convert pixel offset (pfc) to angle
                angle = (slope * pixels_from_center) + intercept

                range = H_CM / Math.Tan(angle)
                str = Convert.ToString(range)
                lblRangeCm.Text = str.Substring(0, 5)  ' limit string length

                range = range / 2.54    ' convert to inches
                str = Convert.ToString(range)
                lblRangeIn.Text = str.Substring(0, 4)  ' limit string length
            Else
                lblRangeCm.Text = String.Empty
                lblRangeIn.Text = String.Empty
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            StreamStop()
        End Try
    End Sub


    Private Sub btnGrab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrab.Click
        Try
            frame_grab()  ' grab a frame

        Catch ex As Exception
            MsgBox(ex.Message)
            StreamStop()
        End Try
    End Sub


    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click ' save current bitmap image
        Try
            Dim myFilename As String
            Dim saveFileDialog1 As New SaveFileDialog()

            saveFileDialog1.FilterIndex = 1
            saveFileDialog1.RestoreDirectory = True

            If radSaveBitmap.Checked Then         ' BITMAP
                saveFileDialog1.Filter = "Bitmap (*.bmp)|*.bmp|All files (*.*)|*.*"

                If saveFileDialog1.ShowDialog() = DialogResult.OK Then  ' pop up the Save File dialog box and let the user select a file
                    myFilename = saveFileDialog1.FileName()             ' grab the name of the file
                    If (myFilename IsNot Nothing) Then
                        bmp.Save(myFilename, System.Drawing.Imaging.ImageFormat.Bmp)
                    End If
                End If
            ElseIf radSaveRaw.Checked Then        ' RAW/BINARY
                saveFileDialog1.Filter = "Raw/Binary (*.raw)|*.raw|All files (*.*)|*.*"

                If saveFileDialog1.ShowDialog() = DialogResult.OK Then  ' pop up the Save File dialog box and let the user select a file
                    myFilename = saveFileDialog1.FileName()             ' grab the name of the file
                    If (myFilename IsNot Nothing) Then
                        Dim oFileStream As System.IO.FileStream
                        oFileStream = New System.IO.FileStream(myFilename, System.IO.FileMode.Create)
                        oFileStream.Write(buf, 0, buf.Length)
                        oFileStream.Close()
                    End If
                End If
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            StreamStop()
        End Try
    End Sub


    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click     ' send data to serial port
        Try
            If txtDataToSend.TextLength > 0 Then
                serialPort.Write(txtDataToSend.Text)    ' write contents of text field to serial port
            Else
                serialPort.Write(vbCr)
            End If
            With txtDataReceived
                .SelectionColor = Color.Cornsilk
                .AppendText(txtDataToSend.Text)         ' echo the text to the text box
            End With
            SendMessage(txtDataReceived.Handle.ToInt32, EM_SCROLL, SB_BOTTOM, 0)
            txtDataToSend.Text = String.Empty           ' clear buffer

        Catch ex As Exception
            MsgBox(ex.Message)
            StreamStop()
        End Try
    End Sub

    Private Sub btnConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConnect.Click   ' open serial port (don't let anything else on the form work until we're successfully connected)

        If serialPort.IsOpen Then
            serialPort.Close()
        End If
        Try
            With serialPort
                '.PortName = "COM13" ' hard-code COM port for development purposes
                .PortName = cbbCOMPorts.Text
                .BaudRate = Val(boxBaud.SelectedItem().ToString())
                .Parity = IO.Ports.Parity.None
                .DataBits = 8
                .StopBits = IO.Ports.StopBits.One
                .Encoding = System.Text.Encoding.GetEncoding(1252)   '1252 is the default codepage on US Windows (English)
                .ReadBufferSize = 32768  ' Make sure input buffer is larger than we expect to receive at any given time
                .ReadTimeout = 2000 ' default setting in ms
            End With

            serialPort.Open()
            serialPort.DiscardInBuffer()
            serialPort.DiscardOutBuffer()

            serialPort.Write("U")           ' Upon power-up, the LRF waits for a "U" character to be sent in order to automatically detect baud rate
            txtMessage.AppendText(serialPort.ReadTo(vbCr & ":"))  ' Print any responses from the LRF module, such as start-up errors or warnings
            txtMessage.AppendText(vbCr)
            txtMessage.ScrollToCaret()
            ' If we don't receive the ":" within the default ReadTimeout, we'll get a TimeoutException

            ' Once we know the correct COM port is open and we can properly communicate with the LRF, 
            ' set the read timeout in ms depending on the selected baud rate (long enough to receive an entire frame + some additional overhead)
            Select Case serialPort.BaudRate
                Case 9600
                    serialPort.ReadTimeout = 30000
                Case 19200
                    serialPort.ReadTimeout = 18000
                Case 38400
                    serialPort.ReadTimeout = 12000
                Case 57600
                    serialPort.ReadTimeout = 8000
                Case 115200
                    serialPort.ReadTimeout = 5000
            End Select

            ' Read version and calibration information from LRF module
            serialPort.Write("V")           ' Get version information from LRF
            serialPort.ReadTo("FW = ")      ' Wait for the LRF to send firmware version
            fw_ver = serialPort.ReadByte - &H30 ' Set major version number as decimal value (used for code to support different versions)

            ' Read calibration and parameter values and place into the text boxes
            serialPort.ReadTo("SLOPE = ")           ' Wait for the LRF to send value
            txtSlope.Text = serialPort.ReadTo(" ")
            serialPort.ReadTo("INT = ")             ' Wait for the LRF to send value
            txtIntercept.Text = serialPort.ReadTo(" ")

            If (fw_ver = 1) Then ' Original
                yUDLower.Maximum = 100
                yUDUpper.Maximum = 100
            ElseIf (fw_ver = 2) Then ' Optimized
                yUDLower.Maximum = 255
                yUDUpper.Maximum = 255
                serialPort.ReadTo("LOWER_BOUND = ")     ' Wait for the LRF to send value
                yUDLower.Value = CDec(Val(serialPort.ReadTo(vbCr)))
                serialPort.ReadTo("UPPER_BOUND = ")     ' Wait for the LRF to send value
                yUDUpper.Value = CDec(Val(serialPort.ReadTo(vbCr)))
                serialPort.ReadTo("BLOB_MASS_THRESHOLD = ")   ' Wait for the LRF to send value
                UDBlobMass.Value = CDec(Val(serialPort.ReadTo(vbCr)))
            End If

            serialPort.ReadTo(":")          ' Wait for the LRF to send a ":" indicating that it is ready to receive the next command
            ' If we don't receive the ":" within the default ReadTimeout, we'll get a TimeoutException

            ' Set default state of objects
            cbbCOMPorts.Enabled = False
            boxBaud.Enabled = False
            btnConnect.Enabled = False
            btnDisconnect.Enabled = True
            btnGrab.Enabled = True
            btnSave.Enabled = False
            btnSend.Enabled = True
            radFrameFull.Enabled = True
            radFrameROI.Enabled = True
            radFrameROIProc.Enabled = True
            radSaveBitmap.Enabled = False
            radSaveRaw.Enabled = False
            txtDataToSend.Enabled = True
            txtDataReceived.Enabled = True
            txtMessage.Enabled = True

            If (radFrameROIProc.Checked) Then
                chkBlob.Enabled = True        ' enable all color tracking options
                radTrackBounds.Enabled = True
                radTrackCentroid.Enabled = True
                If (fw_ver = 1) Then ' Original
                    uUDUpper.Enabled = True
                    uUDLower.Enabled = True
                    vUDUpper.Enabled = True
                    vUDLower.Enabled = True
                    UDBlobMass.Enabled = False
                    LabelUUpper.Enabled = True
                    LabelVUpper.Enabled = True
                    LabelULower.Enabled = True
                    LabelVLower.Enabled = True
                    LabelBlobMass.Enabled = False
                ElseIf (fw_ver = 2) Then ' Optimized
                    ' disable U/V bound controls, since we're now only using greyscale values
                    uUDUpper.Enabled = False
                    uUDLower.Enabled = False
                    vUDUpper.Enabled = False
                    vUDLower.Enabled = False
                    UDBlobMass.Enabled = True
                    LabelUUpper.Enabled = False
                    LabelVUpper.Enabled = False
                    LabelULower.Enabled = False
                    LabelVLower.Enabled = False
                    LabelBlobMass.Enabled = True
                End If
                pnlBounds.Enabled = True
                txtSlope.Enabled = True
                txtIntercept.Enabled = True
            End If

            ' Set default color panels
            Dim rgb_u, rgb_l As RGB
            If (fw_ver = 1) Then  ' Original
                rgb_l = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDLower.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDLower.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDLower.Value / 100.0))))
                rgb_u = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDUpper.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDUpper.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDUpper.Value / 100.0))))
                pnlLower.BackColor = Color.FromArgb(rgb_l.Red, rgb_l.Green, rgb_l.Blue)
                pnlUpper.BackColor = Color.FromArgb(rgb_u.Red, rgb_u.Green, rgb_u.Blue)
            ElseIf (fw_ver = 2) Then ' Optimized
                pnlLower.BackColor = Color.FromArgb(yUDLower.Value, yUDLower.Value, yUDLower.Value)
                pnlUpper.BackColor = Color.FromArgb(yUDUpper.Value, yUDUpper.Value, yUDUpper.Value)
            End If

            ' set correct resolution of bitmap and text labels based on currently selected radio button
            If (radFrameFull.Checked) Then
                bmp = New Bitmap(_FB_X_FULL, _FB_Y_FULL)
                lblResX.Text = CStr(_FB_X_FULL)
                lblResY.Text = CStr(_FB_Y_FULL)
            ElseIf (radFrameROI.Checked Or radFrameROIProc.Checked) Then
                If (fw_ver = 1) Then
                    bmp = New Bitmap(_FB_X_ROI, _FB_Y_ROI)
                    lblResX.Text = CStr(_FB_X_ROI)
                    lblResY.Text = CStr(_FB_Y_ROI)
                ElseIf (fw_ver = 2) Then
                    bmp = New Bitmap(_FB_X_ROI_2, _FB_Y_ROI_2)
                    lblResX.Text = CStr(_FB_X_ROI_2)
                    lblResY.Text = CStr(_FB_Y_ROI_2)
                End If
            End If

            txtMessage.AppendText(serialPort.PortName & Convert.ToString(" connected." & vbCr))  ' update screen with information
            txtMessage.ScrollToCaret()
            txtDataToSend.Focus()

        Catch ex As Exception
            MsgBox(ex.Message)
            StreamStop()
        End Try
    End Sub


    Private Sub btnDisconnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisconnect.Click   ' close serial port
        Try
            serialPort.Close()

            txtMessage.AppendText(serialPort.PortName & Convert.ToString(" disconnected." & vbCr))  ' update screen with information
            txtMessage.ScrollToCaret()

            cbbCOMPorts.Enabled = True
            boxBaud.Enabled = True
            btnConnect.Enabled = True
            btnDisconnect.Enabled = False
            btnGrab.Enabled = False
            btnSave.Enabled = False
            btnSend.Enabled = False
            radFrameFull.Enabled = False
            radFrameROI.Enabled = False
            radFrameROIProc.Enabled = False
            radSaveBitmap.Enabled = False
            radSaveRaw.Enabled = False
            radTrackBounds.Enabled = False
            radTrackCentroid.Enabled = False
            txtDataToSend.Enabled = False
            txtDataReceived.Enabled = False
            txtMessage.Enabled = False

            chkBlob.Enabled = False             ' color tracking options
            pnlBounds.Enabled = False
            txtSlope.Enabled = False
            txtIntercept.Enabled = False
            UDBlobMass.Enabled = False
            LabelBlobMass.Enabled = False

            lblPFC.Text = String.Empty          ' clear text boxes
            lblRangeCm.Text = String.Empty
            lblRangeIn.Text = String.Empty

        Catch ex As Exception
            MsgBox(ex.Message)
            StreamStop()
        End Try
    End Sub


    Private Sub StreamStop() ' stop and clear everything after error/exception
        Try
            serialPort.DiscardInBuffer()
            serialPort.DiscardOutBuffer()
            fg_flag = 0
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub pnlBitmap_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pnlBitmap.Paint
        pboxBitmap.Image = bmp     ' draw bitmap into picturebox
    End Sub


    Private Sub chkTrack_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBlob.CheckStateChanged
        If chkBlob.Checked And serialPort.IsOpen Then
            ' enable color tracking options
            radTrackBounds.Enabled = True
            radTrackCentroid.Enabled = True
            If (fw_ver = 1) Then ' Original
                uUDUpper.Enabled = True
                uUDLower.Enabled = True
                vUDUpper.Enabled = True
                vUDLower.Enabled = True
                UDBlobMass.Enabled = False
                LabelUUpper.Enabled = True
                LabelVUpper.Enabled = True
                LabelULower.Enabled = True
                LabelVLower.Enabled = True
                LabelBlobMass.Enabled = False
            ElseIf (fw_ver = 2) Then ' Optimized
                ' disable U/V bound controls, since we're now only using greyscale values
                uUDUpper.Enabled = False
                uUDLower.Enabled = False
                vUDUpper.Enabled = False
                vUDLower.Enabled = False
                UDBlobMass.Enabled = True
                LabelUUpper.Enabled = False
                LabelVUpper.Enabled = False
                LabelULower.Enabled = False
                LabelVLower.Enabled = False
                LabelBlobMass.Enabled = True
            End If
            pnlBounds.Enabled = True
            txtSlope.Enabled = True
            txtIntercept.Enabled = True
        Else
            ' disable color tracking options
            radTrackBounds.Enabled = False
            radTrackCentroid.Enabled = False
            pnlBounds.Enabled = False
            txtSlope.Enabled = False
            txtIntercept.Enabled = False
            UDBlobMass.Enabled = False
            LabelBlobMass.Enabled = False
        End If
    End Sub


    Private Sub radFrameFull_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFrameFull.CheckedChanged
        bmp = New Bitmap(_FB_X_FULL, _FB_Y_FULL)  ' change resolution of bitmap 

        lblResX.Text = CStr(_FB_X_FULL)    ' fill in textbox
        lblResY.Text = CStr(_FB_Y_FULL)

        If serialPort.IsOpen Then
            btnSave.Enabled = False           ' disable save button because a new bitmap has just been formed, so there's nothing to save yet
            radSaveBitmap.Enabled = False
            radSaveRaw.Enabled = False

            chkBlob.Enabled = False           ' disable all color tracking options, since we only do that for color
            radTrackBounds.Enabled = False
            radTrackCentroid.Enabled = False
            pnlBounds.Enabled = False
            txtSlope.Enabled = False
            txtIntercept.Enabled = False
            UDBlobMass.Enabled = False
            LabelBlobMass.Enabled = False
        End If
    End Sub


    Private Sub radFrameROI_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFrameROI.CheckedChanged
        If (fw_ver = 1) Then  ' Original
            bmp = New Bitmap(_FB_X_ROI, _FB_Y_ROI)   ' change resolution of bitmap

            lblResX.Text = CStr(_FB_X_ROI)    ' fill in textbox
            lblResY.Text = CStr(_FB_Y_ROI)
        ElseIf (fw_ver = 2) Then ' Optimized
            bmp = New Bitmap(_FB_X_ROI_2, _FB_Y_ROI_2)   ' change resolution of bitmap

            lblResX.Text = CStr(_FB_X_ROI_2)    ' fill in textbox
            lblResY.Text = CStr(_FB_Y_ROI_2)
        End If

        If serialPort.IsOpen Then
            btnSave.Enabled = False           ' disable save button because a new bitmap has just been formed, so there's nothing to save yet
            radSaveBitmap.Enabled = False
            radSaveRaw.Enabled = False

            chkBlob.Enabled = False           ' disable all color tracking options, since we only do that for color
            radTrackBounds.Enabled = False
            radTrackCentroid.Enabled = False
            pnlBounds.Enabled = False
            txtSlope.Enabled = False
            txtIntercept.Enabled = False
            UDBlobMass.Enabled = False
            LabelBlobMass.Enabled = False
        End If
    End Sub


    Private Sub radFrameROIProc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFrameROIProc.CheckedChanged
        If (fw_ver = 1) Then  ' Original
            bmp = New Bitmap(_FB_X_ROI, _FB_Y_ROI)   ' change resolution of bitmap

            lblResX.Text = CStr(_FB_X_ROI)    ' fill in textbox
            lblResY.Text = CStr(_FB_Y_ROI)
        ElseIf (fw_ver = 2) Then ' Optimized
            bmp = New Bitmap(_FB_X_ROI_2, _FB_Y_ROI_2)   ' change resolution of bitmap

            lblResX.Text = CStr(_FB_X_ROI_2)    ' fill in textbox
            lblResY.Text = CStr(_FB_Y_ROI_2)
        End If

        If serialPort.IsOpen Then
            btnSave.Enabled = False       ' disable save button because a new bitmap has just been formed, so there's nothing to save yet
            radSaveBitmap.Enabled = False
            radSaveRaw.Enabled = False

            chkBlob.Enabled = True        ' enable all color tracking options
            radTrackBounds.Enabled = True
            radTrackCentroid.Enabled = True
            If (fw_ver = 1) Then ' Original
                uUDUpper.Enabled = True
                uUDLower.Enabled = True
                vUDUpper.Enabled = True
                vUDLower.Enabled = True
                UDBlobMass.Enabled = False
                LabelUUpper.Enabled = True
                LabelVUpper.Enabled = True
                LabelULower.Enabled = True
                LabelVLower.Enabled = True
                LabelBlobMass.Enabled = False
            ElseIf (fw_ver = 2) Then ' Optimized
                ' disable U/V bound controls, since we're now only using greyscale values
                uUDUpper.Enabled = False
                uUDLower.Enabled = False
                vUDUpper.Enabled = False
                vUDLower.Enabled = False
                UDBlobMass.Enabled = True
                LabelUUpper.Enabled = False
                LabelVUpper.Enabled = False
                LabelULower.Enabled = False
                LabelVLower.Enabled = False
                LabelBlobMass.Enabled = True
            End If
            pnlBounds.Enabled = True
            txtSlope.Enabled = True
            txtIntercept.Enabled = True
        End If
    End Sub

    Private Sub YUVLowerValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles yUDLower.ValueChanged, uUDLower.ValueChanged, vUDLower.ValueChanged
        Dim rgb_u, rgb_l As RGB

        ' Set bound limits
        ' Lower bound values cannot be greater than upper bound values
        If (yUDLower.Value > yUDUpper.Value) Then yUDUpper.Value = yUDLower.Value
        If (uUDLower.Value > uUDUpper.Value) Then uUDUpper.Value = uUDLower.Value
        If (vUDLower.Value > vUDUpper.Value) Then vUDUpper.Value = vUDLower.Value

        If (fw_ver = 1) Then  ' Original
            rgb_l = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDLower.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDLower.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDLower.Value / 100.0))))
            rgb_u = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDUpper.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDUpper.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDUpper.Value / 100.0))))
            pnlLower.BackColor = Color.FromArgb(rgb_l.Red, rgb_l.Green, rgb_l.Blue)
            pnlUpper.BackColor = Color.FromArgb(rgb_u.Red, rgb_u.Green, rgb_u.Blue)
        ElseIf (fw_ver = 2) Then ' Optimized
            pnlLower.BackColor = Color.FromArgb(yUDLower.Value, yUDLower.Value, yUDLower.Value)
            pnlUpper.BackColor = Color.FromArgb(yUDUpper.Value, yUDUpper.Value, yUDUpper.Value)
        End If
    End Sub

    Private Sub YUVUpperValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles yUDUpper.ValueChanged, uUDUpper.ValueChanged, vUDUpper.ValueChanged
        Dim rgb_u, rgb_l As RGB

        ' Set bound limits
        ' Upper bound values cannot be less than lower bound values
        If (yUDUpper.Value < yUDLower.Value) Then yUDLower.Value = yUDUpper.Value
        If (uUDUpper.Value < uUDLower.Value) Then uUDLower.Value = uUDUpper.Value
        If (vUDUpper.Value < vUDLower.Value) Then vUDLower.Value = vUDUpper.Value

        If (fw_ver = 1) Then  ' Original
            rgb_l = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDLower.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDLower.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDLower.Value / 100.0))))
            rgb_u = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDUpper.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDUpper.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDUpper.Value / 100.0))))
            pnlLower.BackColor = Color.FromArgb(rgb_l.Red, rgb_l.Green, rgb_l.Blue)
            pnlUpper.BackColor = Color.FromArgb(rgb_u.Red, rgb_u.Green, rgb_u.Blue)
        ElseIf (fw_ver = 2) Then ' Optimized
            pnlLower.BackColor = Color.FromArgb(yUDLower.Value, yUDLower.Value, yUDLower.Value)
            pnlUpper.BackColor = Color.FromArgb(yUDUpper.Value, yUDUpper.Value, yUDUpper.Value)
        End If
    End Sub
End Class