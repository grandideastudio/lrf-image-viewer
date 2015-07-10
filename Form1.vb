Public Class Form1
    ' CONSTANTS
    ' Frame buffer resolutions
    Private Const _FB_X_GRY As Integer = 160  ' 8 bits/pixel greyscale, only using the Y/luma component
    Private Const _FB_Y_GRY As Integer = 128
    Private Const _FB_X_CLR As Integer = 640  ' 16 bits/pixel YUV422 color, used for actual laser range finding operations
    Private Const _FB_Y_CLR As Integer = 16

    ' Range finding (optional, since this can be done directly on the LRF module, as well)
    Private Const H_CM As Double = 7.8                  ' Distance between centerpoints of camera and laser, fixed based on PCB layout (cm)
    Private Const ANGLE_MIN As Double = 0.030699015     ' Minimum allowable angle (radians) = arctan(h/D) (corresponds to D = 254 cm)

    ' Image processing/blob finding
    Private Const _SUM_THRESHOLD As Integer = 3     ' Threshold that column sum must be above in order to be considered part of the blob
    Private Const _MAX_BLOBS As Integer = 6         ' Maximum number of blobs to detect within the frame
    Private Const _ROI_X As Integer = _FB_X_CLR / 2


    ' GLOBAL VARIABLES
    Dim WithEvents serialPort As New IO.Ports.SerialPort
    Public fg_flag As Byte      ' set to 1 when a frame grab is in progress
    Public myStr As String      ' global string used to store input from serial port
    Public buf((_FB_X_GRY * _FB_Y_GRY) - 1) As Byte        ' serial port input buffer in byte form (total number of elements should always match number of bytes in frame)
    Public yuv444((_FB_X_CLR * _FB_Y_CLR) - 1) As YUV
    Public bmp As Bitmap        ' bitmap object to store frame image

    ' Image processing/blob finding
    Structure blobStruct
        Public left As Integer                      ' X coordinate of left side (beginning) of detected blob
        Public right As Integer                     ' X coordinate of right side (end) of detected blob
        Public mass As Integer                      ' Mass of blob (sum of all valid pixels within the blob)
        Public centroid As Integer                  ' Centroid (center of mass) of blob
    End Structure

    Public fb_bool(_FB_Y_CLR, _FB_X_CLR) As Integer ' Detection map: 1 = pixel is within our desired color bounds, 0 = otherwise
    Public roi(_ROI_X) As Integer                   ' Array of detected pixels per X coordinate
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
        boxBaud.Items.Add("115200")         ' Fill available baud rate speeds into selection box (LRF defaults to 115.2k, so force the program to use it)
        boxBaud.SelectedIndex = 0
        btnDisconnect.Enabled = False
        AcceptButton = btnSend              ' Assign Send button to be the form's primary button when Enter is pressed 

        fg_flag = 0
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

    Public Delegate Sub myDelegate()
    Public Sub updateTextBox()
        With txtDataReceived
            .SelectionColor = Color.White
            .AppendText(serialPort.ReadExisting)  ' read all existing bytes from the serial port buffer and print to screen
            .ScrollToCaret()
        End With
    End Sub


    Private Sub frame_grab() ' grab a single frame from the LRF module and display it on the screen
        '    Try
        Dim i, ix, iy As Integer
        Dim val As Integer

        txtMessage.AppendText(Convert.ToString("Grabbing frame..."))
        txtMessage.Update() ' force update of text box

        fg_flag = 1                   ' set flag
        serialPort.DiscardInBuffer()  ' clear any contents of serial input buffer

        If radFrameGrey.Checked Then    ' GREYSCALE
            serialPort.Write("G")       ' send command to LRF

            myStr = serialPort.ReadTo("END" & vbCr & ":")  ' read contents of serial port buffer until the entire frame has been sent

            Array.Clear(buf, 0, buf.Length)       ' clear the contents of the byte array before copying in data from the new frame
            For i = 0 To (myStr.Length - 1)       ' convert String received by serialPort.ReadTo command into a byte array 
                buf(i) = CByte(Asc(myStr(i)))
            Next

            ' fill in bitmap with pixel data
            ' 8 bits/pixel, only using the Y/luma component
            For iy = 0 To (_FB_Y_GRY - 1)        ' for each row of Y
                For ix = 0 To (_FB_X_GRY - 1)       ' for each column of X
                    i = (iy * _FB_X_GRY) + ix         ' calculate index into array
                    val = buf(i)
                    bmp.SetPixel(ix, iy, Color.FromArgb(val, val, val))
                Next
            Next
        ElseIf radFrameColor.Checked Then   ' COLOR
            If chkBlob.Checked Then         ' if color tracking checkbox is checked, then retrieved processed frame (double frame grab with laser off/on and background subtracted)
                serialPort.Write("P")           ' send command to LRF
            Else                            ' otherwise, just grab a normal color frame with no processing
                serialPort.Write("C")           ' send command to LRF
            End If

            myStr = serialPort.ReadTo("END" & vbCr & ":")  ' read contents of serial port buffer until the entire frame has been sent

            Array.Clear(buf, 0, buf.Length)       ' clear the contents of the byte array before copying in data from the new frame
            For i = 0 To (myStr.Length - 1)     ' convert String received by serialPort.ReadTo command into a byte array 
                buf(i) = CByte(Asc(myStr(i)))
            Next

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
                'val = buf(i) - 16
                'yuv444(i / 2).Y = Convert.ToDouble(val / 220.0)               ' Normalize value as required by the YUV structure
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
            For iy = 0 To (_FB_Y_CLR - 1)        ' for each row of Y
                For ix = 0 To (_FB_X_CLR - 1)       ' for each column of X
                    i = (iy * _FB_X_CLR) + ix         ' calculate index into array
                    bmp.SetPixel(ix, iy, ColorHelper.YUVtoColor(yuv444(i)))
                Next
            Next
        End If

        fg_flag = 0         ' frame grab is done, so clear the flag
        txtMessage.AppendText(Convert.ToString("DONE!") & vbCr)
        txtMessage.ScrollToCaret()

        If radFrameColor.Checked And chkBlob.Checked Then   ' if user wants to see tracking data
            blob_detection()                                    ' find the blobs within the frame
        Else                                                ' otherwise, clear any information from the text boxes
            lblPFC.Text = String.Empty
            lblRangeCm.Text = String.Empty
            lblRangeIn.Text = String.Empty
        End If

        pnlBitmap.Refresh()         ' force painting of the panel to update the image

        btnSave.Enabled = True      ' enable save button now that we have a bitmap available
        radSaveBitmap.Enabled = True
        radSaveRaw.Enabled = True

        '       Catch ex As Exception
        '          MsgBox(ex.Message)
        '         StreamStop()
        '    End Try
    End Sub


    Private Sub blob_detection()  ' locate blob(s) within the frame
        Try
            Dim ix, iy As Integer
            Dim val As Integer
            Dim upper_bound, lower_bound As YUV     ' tracking parameters, pixel must be within these bounds in order to be considered

            ' initialize variables
            lower_bound.Y = Convert.ToDouble(yUDLower.Value / 100.0)               ' normalize values as required by the YUV structure
            lower_bound.U = -0.436 + (Convert.ToDouble(uUDLower.Value / 100.0))
            lower_bound.V = -0.615 + (Convert.ToDouble(vUDLower.Value / 100.0))
            upper_bound.Y = Convert.ToDouble(yUDUpper.Value / 100.0)
            upper_bound.U = -0.436 + (Convert.ToDouble(uUDUpper.Value / 100.0))
            upper_bound.V = -0.615 + (Convert.ToDouble(vUDUpper.Value / 100.0))

            ' huge thanks to zoz for his image processing expertise!
            ' threshold each pixel based on the lower and upper color bounds
            For iy = 0 To (_FB_Y_CLR - 1)           ' for each row of Y
                For ix = 0 To (_FB_X_CLR - 1)          ' for each column of X 
                    val = (iy * _FB_X_CLR) + ix                                                 ' get index of current pixel
                    If ((yuv444(val) >= lower_bound) And (yuv444(val) <= upper_bound)) Then     ' if pixel's YUV components are within the lower and upper color bounds...
                        bmp.SetPixel(ix, iy, Color.White)                                       ' for easier visualization/testing, convert each pixel to pure black and white
                        fb_bool(iy, ix) = 1                                                     ' store result in a binary array, 1 = pixel is within bounds, 0 = otherwise
                    Else
                        bmp.SetPixel(ix, iy, Color.Black)
                        fb_bool(iy, ix) = 0
                    End If
                Next
            Next

            ' column sum
            ' keep a sum of detected pixels per column of the image
            ' basically creating a 1-D array/histogram instead of a 2-D
            For ix = 0 To (_ROI_X - 1)      ' for each column X within our region-of-interest (we only care about the left side of the frame - anything on the right is not our laser)
                roi(ix) = 0                     ' clear count
                For iy = 0 To (_FB_Y_CLR - 1)   ' for each row Y
                    roi(ix) += fb_bool(iy, ix)      ' add all of the detected pixels within the column
                Next
            Next

            ' find the blobs
            ' search through the 1-D array to find the blobs and determine their start and end coordinates
            Dim found_blob As Boolean = False   ' flag set while there is a blob currently being processed
            Dim num_blobs As Integer = 0        ' number of detected blobs

            For ix = 0 To (_ROI_X - 1)     ' for each column X of our region-of-interest                
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
                For ix = 0 To (num_blobs - 1)
                    If (blob(ix).mass > blob(max).mass) Then
                        max = ix
                    End If
                Next
                txtMessage.AppendText(Convert.ToString("Primary blob: ") & Convert.ToString(max) & vbCr)
                txtMessage.ScrollToCaret()

                If radTrackBounds.Checked Then           ' draw vertical lines to bound the blob
                    For iy = 0 To (_FB_Y_CLR - 1)        ' for each row of Y
                        bmp.SetPixel(blob(max).left - 1, iy, Color.Red)     ' left line
                        bmp.SetPixel(blob(max).right - 1, iy, Color.Red)    ' right line
                    Next
                ElseIf radTrackCentroid.Checked Then     ' draw vertical line to intersect centroid
                    For iy = 0 To (_FB_Y_CLR - 1)        ' for each row of Y
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

            pixels_from_center = Math.Abs((_FB_X_CLR / 2) - centroid_x)  ' calculate the number of pixels from center of frame
            lblPFC.Text = Convert.ToString(pixels_from_center)

            ' Grab SLOPE and INTERCEPT values from the text boxes
            ' These are specific for each unit based on manufacturing & assembly tolerances
            ' and can be calculated by taking a number of measurements from known distances and recording resultant pfc values
            slope = Val(txtSlope.Text())                ' we're not doing any bounds checking here, so hopefully the user entered in the values properly
            intercept = Val(txtIntercept.Text())
            pfc_min = (ANGLE_MIN - intercept) / slope

            ' calculate range in cm based on centroid of primary blob
            ' D = h / tan(theta)
            If (pixels_from_center >= PFC_MIN) Then     ' if the pfc value is greater than our minimum (e.g., less than our maximum defined distance)
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
                .ScrollToCaret()
            End With
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
                '.PortName = "COM11" ' hard-code COM port for development purposes
                .PortName = cbbCOMPorts.Text
                .BaudRate = Val(boxBaud.SelectedItem().ToString())
                .Parity = IO.Ports.Parity.None
                .DataBits = 8
                .StopBits = IO.Ports.StopBits.One
                .ReadTimeout = 5000 ' ms
                .Encoding = System.Text.Encoding.Default
                .ReadBufferSize = 32768  ' Make sure input buffer is large
            End With

            serialPort.Open()
            serialPort.DiscardInBuffer()
            serialPort.DiscardOutBuffer()

            btnConnect.Enabled = False
            btnDisconnect.Enabled = True
            btnGrab.Enabled = True
            btnSave.Enabled = False
            btnSend.Enabled = True
            radFrameGrey.Enabled = True
            radFrameColor.Enabled = True
            radSaveBitmap.Enabled = False
            radSaveRaw.Enabled = False
            txtDataToSend.Enabled = True
            txtDataReceived.Enabled = True
            txtMessage.Enabled = True

            If (radFrameColor.Checked) Then
                chkBlob.Enabled = True        ' enable all color tracking options
                radTrackBounds.Enabled = True
                radTrackCentroid.Enabled = True
                pnlBounds.Enabled = True
                txtSlope.Enabled = True
                txtIntercept.Enabled = True
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

            btnConnect.Enabled = True
            btnDisconnect.Enabled = False
            btnGrab.Enabled = False
            btnSave.Enabled = False
            btnSend.Enabled = False
            radFrameGrey.Enabled = False
            radFrameColor.Enabled = False
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
        End If
    End Sub


    Private Sub radFrameGrey_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFrameGrey.CheckedChanged
        bmp = New Bitmap(_FB_X_GRY, _FB_Y_GRY)  ' change resolution of bitmap 

        lblResX.Text = CStr(_FB_X_GRY)    ' fill in textbox
        lblResY.Text = CStr(_FB_Y_GRY)

        btnSave.Enabled = False           ' disable save button because a new bitmap has just been formed, so there's nothing to save yet
        radSaveBitmap.Enabled = False
        radSaveRaw.Enabled = False

        chkBlob.Enabled = False           ' disable all color tracking options, since we only do that for color
        radTrackBounds.Enabled = False
        radTrackCentroid.Enabled = False
        pnlBounds.Enabled = False
        txtSlope.Enabled = False
        txtIntercept.Enabled = False
    End Sub


    Private Sub radFrameColor_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radFrameColor.CheckedChanged
        bmp = New Bitmap(_FB_X_CLR, _FB_Y_CLR)   ' change resolution of bitmap

        lblResX.Text = CStr(_FB_X_CLR)    ' fill in textbox
        lblResY.Text = CStr(_FB_Y_CLR)

        If serialPort.IsOpen Then
            btnSave.Enabled = False       ' disable save button because a new bitmap has just been formed, so there's nothing to save yet
            radSaveBitmap.Enabled = False
            radSaveRaw.Enabled = False

            chkBlob.Enabled = True        ' enable all color tracking options
            radTrackBounds.Enabled = True
            radTrackCentroid.Enabled = True
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

        rgb_l = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDLower.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDLower.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDLower.Value / 100.0))))
        pnlLower.BackColor = Color.FromArgb(rgb_l.Red, rgb_l.Green, rgb_l.Blue)

        rgb_u = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDUpper.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDUpper.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDUpper.Value / 100.0))))
        pnlUpper.BackColor = Color.FromArgb(rgb_u.Red, rgb_u.Green, rgb_u.Blue)
    End Sub

    Private Sub YUVUpperValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles yUDUpper.ValueChanged, uUDUpper.ValueChanged, vUDUpper.ValueChanged
        Dim rgb_u, rgb_l As RGB

        ' Set bound limits
        ' Upper bound values cannot be less than lower bound values
        If (yUDUpper.Value < yUDLower.Value) Then yUDLower.Value = yUDUpper.Value
        If (uUDUpper.Value < uUDLower.Value) Then uUDLower.Value = uUDUpper.Value
        If (vUDUpper.Value < vUDLower.Value) Then vUDLower.Value = vUDUpper.Value

        rgb_l = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDLower.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDLower.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDLower.Value / 100.0))))
        pnlLower.BackColor = Color.FromArgb(rgb_l.Red, rgb_l.Green, rgb_l.Blue)

        rgb_u = ColorHelper.YUVtoRGB(Convert.ToDouble(yUDUpper.Value / 100.0), (-0.436 + (Convert.ToDouble(uUDUpper.Value / 100.0))), (-0.615 + (Convert.ToDouble(vUDUpper.Value / 100.0))))
        pnlUpper.BackColor = Color.FromArgb(rgb_u.Red, rgb_u.Green, rgb_u.Blue)
    End Sub

End Class