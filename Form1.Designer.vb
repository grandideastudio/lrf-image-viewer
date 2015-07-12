<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.lblPFC = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblRangeIn = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.lblRangeCm = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.pnlBounds = New System.Windows.Forms.Panel()
        Me.pnlUpper = New System.Windows.Forms.Panel()
        Me.pnlLower = New System.Windows.Forms.Panel()
        Me.LabelYLower = New System.Windows.Forms.Label()
        Me.vUDUpper = New System.Windows.Forms.NumericUpDown()
        Me.LabelULower = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.LabelVUpper = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.LabelYUpper = New System.Windows.Forms.Label()
        Me.vUDLower = New System.Windows.Forms.NumericUpDown()
        Me.LabelVLower = New System.Windows.Forms.Label()
        Me.uUDUpper = New System.Windows.Forms.NumericUpDown()
        Me.yUDLower = New System.Windows.Forms.NumericUpDown()
        Me.uUDLower = New System.Windows.Forms.NumericUpDown()
        Me.yUDUpper = New System.Windows.Forms.NumericUpDown()
        Me.LabelUUpper = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.radTrackBounds = New System.Windows.Forms.RadioButton()
        Me.radTrackCentroid = New System.Windows.Forms.RadioButton()
        Me.chkBlob = New System.Windows.Forms.CheckBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.radSaveBitmap = New System.Windows.Forms.RadioButton()
        Me.radSaveRaw = New System.Windows.Forms.RadioButton()
        Me.txtDataReceived = New System.Windows.Forms.RichTextBox()
        Me.lblResY = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.lblResX = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.radFrameROIProc = New System.Windows.Forms.RadioButton()
        Me.radFrameFull = New System.Windows.Forms.RadioButton()
        Me.radFrameROI = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.boxBaud = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnGrab = New System.Windows.Forms.Button()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.btnConnect = New System.Windows.Forms.Button()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.txtDataToSend = New System.Windows.Forms.TextBox()
        Me.cbbCOMPorts = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlBitmap = New System.Windows.Forms.Panel()
        Me.pboxBitmap = New System.Windows.Forms.PictureBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtMessage = New System.Windows.Forms.RichTextBox()
        Me.txtSlope = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtIntercept = New System.Windows.Forms.TextBox()
        Me.UDBlobMass = New System.Windows.Forms.NumericUpDown()
        Me.LabelBlobMass = New System.Windows.Forms.Label()
        Me.pnlBounds.SuspendLayout()
        CType(Me.vUDUpper, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.vUDLower, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uUDUpper, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.yUDLower, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.uUDLower, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.yUDUpper, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.pnlBitmap.SuspendLayout()
        CType(Me.pboxBitmap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.UDBlobMass, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblPFC
        '
        Me.lblPFC.BackColor = System.Drawing.Color.Gainsboro
        Me.lblPFC.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblPFC.Location = New System.Drawing.Point(648, 561)
        Me.lblPFC.Name = "lblPFC"
        Me.lblPFC.Size = New System.Drawing.Size(30, 18)
        Me.lblPFC.TabIndex = 125
        Me.lblPFC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(615, 564)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(27, 13)
        Me.Label25.TabIndex = 124
        Me.Label25.Text = "PFC"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(796, 564)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(21, 13)
        Me.Label2.TabIndex = 123
        Me.Label2.Text = "cm"
        '
        'lblRangeIn
        '
        Me.lblRangeIn.BackColor = System.Drawing.Color.Gainsboro
        Me.lblRangeIn.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRangeIn.Location = New System.Drawing.Point(736, 589)
        Me.lblRangeIn.Name = "lblRangeIn"
        Me.lblRangeIn.Size = New System.Drawing.Size(57, 18)
        Me.lblRangeIn.TabIndex = 122
        Me.lblRangeIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(796, 592)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(38, 13)
        Me.Label18.TabIndex = 121
        Me.Label18.Text = "inches"
        '
        'lblRangeCm
        '
        Me.lblRangeCm.BackColor = System.Drawing.Color.Gainsboro
        Me.lblRangeCm.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblRangeCm.Location = New System.Drawing.Point(736, 561)
        Me.lblRangeCm.Name = "lblRangeCm"
        Me.lblRangeCm.Size = New System.Drawing.Size(57, 18)
        Me.lblRangeCm.TabIndex = 120
        Me.lblRangeCm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(692, 563)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(39, 13)
        Me.Label17.TabIndex = 119
        Me.Label17.Text = "Range"
        '
        'pnlBounds
        '
        Me.pnlBounds.Controls.Add(Me.pnlUpper)
        Me.pnlBounds.Controls.Add(Me.pnlLower)
        Me.pnlBounds.Controls.Add(Me.LabelYLower)
        Me.pnlBounds.Controls.Add(Me.vUDUpper)
        Me.pnlBounds.Controls.Add(Me.LabelULower)
        Me.pnlBounds.Controls.Add(Me.Label15)
        Me.pnlBounds.Controls.Add(Me.LabelVUpper)
        Me.pnlBounds.Controls.Add(Me.Label9)
        Me.pnlBounds.Controls.Add(Me.LabelYUpper)
        Me.pnlBounds.Controls.Add(Me.vUDLower)
        Me.pnlBounds.Controls.Add(Me.LabelVLower)
        Me.pnlBounds.Controls.Add(Me.uUDUpper)
        Me.pnlBounds.Controls.Add(Me.yUDLower)
        Me.pnlBounds.Controls.Add(Me.uUDLower)
        Me.pnlBounds.Controls.Add(Me.yUDUpper)
        Me.pnlBounds.Controls.Add(Me.LabelUUpper)
        Me.pnlBounds.Enabled = False
        Me.pnlBounds.Location = New System.Drawing.Point(483, 488)
        Me.pnlBounds.Name = "pnlBounds"
        Me.pnlBounds.Size = New System.Drawing.Size(310, 59)
        Me.pnlBounds.TabIndex = 118
        '
        'pnlUpper
        '
        Me.pnlUpper.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlUpper.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlUpper.Location = New System.Drawing.Point(271, 6)
        Me.pnlUpper.Name = "pnlUpper"
        Me.pnlUpper.Size = New System.Drawing.Size(35, 20)
        Me.pnlUpper.TabIndex = 61
        '
        'pnlLower
        '
        Me.pnlLower.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlLower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pnlLower.Location = New System.Drawing.Point(271, 35)
        Me.pnlLower.Name = "pnlLower"
        Me.pnlLower.Size = New System.Drawing.Size(35, 20)
        Me.pnlLower.TabIndex = 61
        '
        'LabelYLower
        '
        Me.LabelYLower.AutoSize = True
        Me.LabelYLower.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelYLower.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelYLower.Location = New System.Drawing.Point(58, 38)
        Me.LabelYLower.Name = "LabelYLower"
        Me.LabelYLower.Size = New System.Drawing.Size(14, 13)
        Me.LabelYLower.TabIndex = 0
        Me.LabelYLower.Text = "Y"
        Me.LabelYLower.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'vUDUpper
        '
        Me.vUDUpper.Location = New System.Drawing.Point(214, 6)
        Me.vUDUpper.Maximum = New Decimal(New Integer() {123, 0, 0, 0})
        Me.vUDUpper.Name = "vUDUpper"
        Me.vUDUpper.Size = New System.Drawing.Size(46, 20)
        Me.vUDUpper.TabIndex = 22
        Me.vUDUpper.Value = New Decimal(New Integer() {123, 0, 0, 0})
        '
        'LabelULower
        '
        Me.LabelULower.AutoSize = True
        Me.LabelULower.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelULower.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelULower.Location = New System.Drawing.Point(126, 38)
        Me.LabelULower.Name = "LabelULower"
        Me.LabelULower.Size = New System.Drawing.Size(15, 13)
        Me.LabelULower.TabIndex = 1
        Me.LabelULower.Text = "U"
        Me.LabelULower.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(13, 37)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(36, 13)
        Me.Label15.TabIndex = 74
        Me.Label15.Text = "Lower"
        '
        'LabelVUpper
        '
        Me.LabelVUpper.AutoSize = True
        Me.LabelVUpper.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelVUpper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelVUpper.Location = New System.Drawing.Point(198, 9)
        Me.LabelVUpper.Name = "LabelVUpper"
        Me.LabelVUpper.Size = New System.Drawing.Size(14, 13)
        Me.LabelVUpper.TabIndex = 2
        Me.LabelVUpper.Text = "V"
        Me.LabelVUpper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(13, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 13)
        Me.Label9.TabIndex = 73
        Me.Label9.Text = "Upper"
        '
        'LabelYUpper
        '
        Me.LabelYUpper.AutoSize = True
        Me.LabelYUpper.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelYUpper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelYUpper.Location = New System.Drawing.Point(58, 9)
        Me.LabelYUpper.Name = "LabelYUpper"
        Me.LabelYUpper.Size = New System.Drawing.Size(14, 13)
        Me.LabelYUpper.TabIndex = 0
        Me.LabelYUpper.Text = "Y"
        Me.LabelYUpper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'vUDLower
        '
        Me.vUDLower.Location = New System.Drawing.Point(214, 35)
        Me.vUDLower.Maximum = New Decimal(New Integer() {123, 0, 0, 0})
        Me.vUDLower.Name = "vUDLower"
        Me.vUDLower.Size = New System.Drawing.Size(46, 20)
        Me.vUDLower.TabIndex = 26
        '
        'LabelVLower
        '
        Me.LabelVLower.AutoSize = True
        Me.LabelVLower.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelVLower.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelVLower.Location = New System.Drawing.Point(198, 38)
        Me.LabelVLower.Name = "LabelVLower"
        Me.LabelVLower.Size = New System.Drawing.Size(14, 13)
        Me.LabelVLower.TabIndex = 2
        Me.LabelVLower.Text = "V"
        Me.LabelVLower.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'uUDUpper
        '
        Me.uUDUpper.Location = New System.Drawing.Point(143, 6)
        Me.uUDUpper.Maximum = New Decimal(New Integer() {87, 0, 0, 0})
        Me.uUDUpper.Name = "uUDUpper"
        Me.uUDUpper.Size = New System.Drawing.Size(46, 20)
        Me.uUDUpper.TabIndex = 21
        Me.uUDUpper.Value = New Decimal(New Integer() {87, 0, 0, 0})
        '
        'yUDLower
        '
        Me.yUDLower.Location = New System.Drawing.Point(75, 35)
        Me.yUDLower.Name = "yUDLower"
        Me.yUDLower.Size = New System.Drawing.Size(46, 20)
        Me.yUDLower.TabIndex = 24
        Me.yUDLower.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'uUDLower
        '
        Me.uUDLower.Location = New System.Drawing.Point(143, 35)
        Me.uUDLower.Maximum = New Decimal(New Integer() {87, 0, 0, 0})
        Me.uUDLower.Name = "uUDLower"
        Me.uUDLower.Size = New System.Drawing.Size(46, 20)
        Me.uUDLower.TabIndex = 25
        '
        'yUDUpper
        '
        Me.yUDUpper.Location = New System.Drawing.Point(75, 6)
        Me.yUDUpper.Name = "yUDUpper"
        Me.yUDUpper.Size = New System.Drawing.Size(46, 20)
        Me.yUDUpper.TabIndex = 20
        Me.yUDUpper.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'LabelUUpper
        '
        Me.LabelUUpper.AutoSize = True
        Me.LabelUUpper.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelUUpper.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelUUpper.Location = New System.Drawing.Point(126, 9)
        Me.LabelUUpper.Name = "LabelUUpper"
        Me.LabelUUpper.Size = New System.Drawing.Size(15, 13)
        Me.LabelUUpper.TabIndex = 1
        Me.LabelUUpper.Text = "U"
        Me.LabelUUpper.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.radTrackBounds)
        Me.Panel1.Controls.Add(Me.radTrackCentroid)
        Me.Panel1.Location = New System.Drawing.Point(558, 460)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(136, 26)
        Me.Panel1.TabIndex = 16
        '
        'radTrackBounds
        '
        Me.radTrackBounds.AutoSize = True
        Me.radTrackBounds.Enabled = False
        Me.radTrackBounds.Location = New System.Drawing.Point(73, 3)
        Me.radTrackBounds.Name = "radTrackBounds"
        Me.radTrackBounds.Size = New System.Drawing.Size(61, 17)
        Me.radTrackBounds.TabIndex = 18
        Me.radTrackBounds.TabStop = True
        Me.radTrackBounds.Text = "Bounds"
        Me.radTrackBounds.UseVisualStyleBackColor = True
        '
        'radTrackCentroid
        '
        Me.radTrackCentroid.AutoSize = True
        Me.radTrackCentroid.Checked = True
        Me.radTrackCentroid.Enabled = False
        Me.radTrackCentroid.Location = New System.Drawing.Point(3, 3)
        Me.radTrackCentroid.Name = "radTrackCentroid"
        Me.radTrackCentroid.Size = New System.Drawing.Size(64, 17)
        Me.radTrackCentroid.TabIndex = 1417
        Me.radTrackCentroid.TabStop = True
        Me.radTrackCentroid.Text = "Centroid"
        Me.radTrackCentroid.UseVisualStyleBackColor = True
        '
        'chkBlob
        '
        Me.chkBlob.AutoSize = True
        Me.chkBlob.Enabled = False
        Me.chkBlob.Location = New System.Drawing.Point(457, 465)
        Me.chkBlob.Name = "chkBlob"
        Me.chkBlob.Size = New System.Drawing.Size(96, 17)
        Me.chkBlob.TabIndex = 9
        Me.chkBlob.Text = "Blob Detection"
        Me.chkBlob.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.radSaveBitmap)
        Me.Panel3.Controls.Add(Me.radSaveRaw)
        Me.Panel3.Location = New System.Drawing.Point(558, 421)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(117, 26)
        Me.Panel3.TabIndex = 13
        '
        'radSaveBitmap
        '
        Me.radSaveBitmap.AutoSize = True
        Me.radSaveBitmap.Checked = True
        Me.radSaveBitmap.Enabled = False
        Me.radSaveBitmap.Location = New System.Drawing.Point(3, 3)
        Me.radSaveBitmap.Name = "radSaveBitmap"
        Me.radSaveBitmap.Size = New System.Drawing.Size(57, 17)
        Me.radSaveBitmap.TabIndex = 14
        Me.radSaveBitmap.TabStop = True
        Me.radSaveBitmap.Text = "Bitmap"
        Me.radSaveBitmap.UseVisualStyleBackColor = True
        '
        'radSaveRaw
        '
        Me.radSaveRaw.AutoSize = True
        Me.radSaveRaw.Enabled = False
        Me.radSaveRaw.Location = New System.Drawing.Point(64, 3)
        Me.radSaveRaw.Name = "radSaveRaw"
        Me.radSaveRaw.Size = New System.Drawing.Size(47, 17)
        Me.radSaveRaw.TabIndex = 15
        Me.radSaveRaw.TabStop = True
        Me.radSaveRaw.Text = "Raw"
        Me.radSaveRaw.UseVisualStyleBackColor = True
        '
        'txtDataReceived
        '
        Me.txtDataReceived.BackColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(243, Byte), Integer))
        Me.txtDataReceived.DetectUrls = False
        Me.txtDataReceived.Enabled = False
        Me.txtDataReceived.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDataReceived.ForeColor = System.Drawing.Color.Cornsilk
        Me.txtDataReceived.Location = New System.Drawing.Point(28, 354)
        Me.txtDataReceived.Name = "txtDataReceived"
        Me.txtDataReceived.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.txtDataReceived.Size = New System.Drawing.Size(368, 218)
        Me.txtDataReceived.TabIndex = 96
        Me.txtDataReceived.TabStop = False
        Me.txtDataReceived.Text = ""
        '
        'lblResY
        '
        Me.lblResY.Location = New System.Drawing.Point(796, 421)
        Me.lblResY.Name = "lblResY"
        Me.lblResY.Size = New System.Drawing.Size(26, 22)
        Me.lblResY.TabIndex = 104
        Me.lblResY.Text = "999"
        Me.lblResY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(787, 425)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(12, 13)
        Me.Label20.TabIndex = 103
        Me.Label20.Text = "x"
        '
        'lblResX
        '
        Me.lblResX.Location = New System.Drawing.Point(765, 421)
        Me.lblResX.Name = "lblResX"
        Me.lblResX.Size = New System.Drawing.Size(26, 22)
        Me.lblResX.TabIndex = 102
        Me.lblResX.Text = "999"
        Me.lblResX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(706, 426)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(63, 13)
        Me.Label19.TabIndex = 101
        Me.Label19.Text = "Resolution: "
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(454, 421)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(98, 23)
        Me.btnSave.TabIndex = 8
        Me.btnSave.Text = "Save Image"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.radFrameROIProc)
        Me.Panel4.Controls.Add(Me.radFrameFull)
        Me.Panel4.Controls.Add(Me.radFrameROI)
        Me.Panel4.Location = New System.Drawing.Point(558, 382)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(226, 27)
        Me.Panel4.TabIndex = 10
        '
        'radFrameROIProc
        '
        Me.radFrameROIProc.AutoSize = True
        Me.radFrameROIProc.Enabled = False
        Me.radFrameROIProc.Location = New System.Drawing.Point(126, 3)
        Me.radFrameROIProc.Name = "radFrameROIProc"
        Me.radFrameROIProc.Size = New System.Drawing.Size(97, 17)
        Me.radFrameROIProc.TabIndex = 133
        Me.radFrameROIProc.Text = "ROI Processed"
        Me.radFrameROIProc.UseVisualStyleBackColor = True
        '
        'radFrameFull
        '
        Me.radFrameFull.AutoSize = True
        Me.radFrameFull.Checked = True
        Me.radFrameFull.Enabled = False
        Me.radFrameFull.Location = New System.Drawing.Point(3, 3)
        Me.radFrameFull.Name = "radFrameFull"
        Me.radFrameFull.Size = New System.Drawing.Size(73, 17)
        Me.radFrameFull.TabIndex = 11
        Me.radFrameFull.TabStop = True
        Me.radFrameFull.Text = "Full Frame"
        Me.radFrameFull.UseVisualStyleBackColor = True
        '
        'radFrameROI
        '
        Me.radFrameROI.AutoSize = True
        Me.radFrameROI.Enabled = False
        Me.radFrameROI.Location = New System.Drawing.Point(78, 3)
        Me.radFrameROI.Name = "radFrameROI"
        Me.radFrameROI.Size = New System.Drawing.Size(44, 17)
        Me.radFrameROI.TabIndex = 12
        Me.radFrameROI.Text = "ROI"
        Me.radFrameROI.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(134, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 13)
        Me.Label4.TabIndex = 98
        Me.Label4.Text = "Baud Rate"
        '
        'boxBaud
        '
        Me.boxBaud.FormattingEnabled = True
        Me.boxBaud.Location = New System.Drawing.Point(127, 37)
        Me.boxBaud.Name = "boxBaud"
        Me.boxBaud.Size = New System.Drawing.Size(80, 21)
        Me.boxBaud.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(146, 334)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(130, 16)
        Me.Label3.TabIndex = 97
        Me.Label3.Text = "Terminal Console"
        '
        'btnGrab
        '
        Me.btnGrab.Enabled = False
        Me.btnGrab.Location = New System.Drawing.Point(454, 382)
        Me.btnGrab.Name = "btnGrab"
        Me.btnGrab.Size = New System.Drawing.Size(98, 23)
        Me.btnGrab.TabIndex = 7
        Me.btnGrab.Text = "Grab Image"
        Me.btnGrab.UseVisualStyleBackColor = True
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Enabled = False
        Me.btnDisconnect.Location = New System.Drawing.Point(320, 36)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(75, 23)
        Me.btnDisconnect.TabIndex = 4
        Me.btnDisconnect.Text = "Disconnect"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'btnConnect
        '
        Me.btnConnect.Location = New System.Drawing.Point(226, 36)
        Me.btnConnect.Name = "btnConnect"
        Me.btnConnect.Size = New System.Drawing.Size(75, 23)
        Me.btnConnect.TabIndex = 0
        Me.btnConnect.Text = "Connect"
        Me.btnConnect.UseVisualStyleBackColor = True
        '
        'btnSend
        '
        Me.btnSend.Enabled = False
        Me.btnSend.Location = New System.Drawing.Point(321, 586)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(75, 23)
        Me.btnSend.TabIndex = 6
        Me.btnSend.Text = "Send"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'txtDataToSend
        '
        Me.txtDataToSend.BackColor = System.Drawing.Color.Cornsilk
        Me.txtDataToSend.Enabled = False
        Me.txtDataToSend.Location = New System.Drawing.Point(28, 587)
        Me.txtDataToSend.Multiline = True
        Me.txtDataToSend.Name = "txtDataToSend"
        Me.txtDataToSend.Size = New System.Drawing.Size(279, 20)
        Me.txtDataToSend.TabIndex = 5
        '
        'cbbCOMPorts
        '
        Me.cbbCOMPorts.FormattingEnabled = True
        Me.cbbCOMPorts.Location = New System.Drawing.Point(28, 37)
        Me.cbbCOMPorts.Name = "cbbCOMPorts"
        Me.cbbCOMPorts.Size = New System.Drawing.Size(80, 21)
        Me.cbbCOMPorts.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(34, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 94
        Me.Label1.Text = "COM Port"
        '
        'pnlBitmap
        '
        Me.pnlBitmap.BackColor = System.Drawing.Color.Transparent
        Me.pnlBitmap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.pnlBitmap.Controls.Add(Me.pboxBitmap)
        Me.pnlBitmap.Location = New System.Drawing.Point(450, 32)
        Me.pnlBitmap.Name = "pnlBitmap"
        Me.pnlBitmap.Size = New System.Drawing.Size(410, 330)
        Me.pnlBitmap.TabIndex = 100
        '
        'pboxBitmap
        '
        Me.pboxBitmap.BackColor = System.Drawing.Color.Gainsboro
        Me.pboxBitmap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.pboxBitmap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pboxBitmap.ErrorImage = Nothing
        Me.pboxBitmap.InitialImage = Nothing
        Me.pboxBitmap.Location = New System.Drawing.Point(3, 3)
        Me.pboxBitmap.Name = "pboxBitmap"
        Me.pboxBitmap.Size = New System.Drawing.Size(400, 320)
        Me.pboxBitmap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pboxBitmap.TabIndex = 0
        Me.pboxBitmap.TabStop = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(146, 88)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(135, 16)
        Me.Label24.TabIndex = 127
        Me.Label24.Text = "System Messages"
        '
        'txtMessage
        '
        Me.txtMessage.BackColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(163, Byte), Integer))
        Me.txtMessage.DetectUrls = False
        Me.txtMessage.Enabled = False
        Me.txtMessage.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMessage.ForeColor = System.Drawing.Color.Cornsilk
        Me.txtMessage.Location = New System.Drawing.Point(28, 107)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.txtMessage.Size = New System.Drawing.Size(368, 211)
        Me.txtMessage.TabIndex = 128
        Me.txtMessage.TabStop = False
        Me.txtMessage.Text = ""
        '
        'txtSlope
        '
        Me.txtSlope.Location = New System.Drawing.Point(510, 561)
        Me.txtSlope.Name = "txtSlope"
        Me.txtSlope.Size = New System.Drawing.Size(94, 20)
        Me.txtSlope.TabIndex = 129
        Me.txtSlope.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(470, 563)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(34, 13)
        Me.Label10.TabIndex = 130
        Me.Label10.Text = "Slope"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(455, 592)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(49, 13)
        Me.Label14.TabIndex = 132
        Me.Label14.Text = "Intercept"
        '
        'txtIntercept
        '
        Me.txtIntercept.Location = New System.Drawing.Point(510, 590)
        Me.txtIntercept.Name = "txtIntercept"
        Me.txtIntercept.Size = New System.Drawing.Size(94, 20)
        Me.txtIntercept.TabIndex = 131
        Me.txtIntercept.TabStop = False
        '
        'UDBlobMass
        '
        Me.UDBlobMass.Enabled = False
        Me.UDBlobMass.Location = New System.Drawing.Point(808, 463)
        Me.UDBlobMass.Maximum = New Decimal(New Integer() {255, 0, 0, 0})
        Me.UDBlobMass.Name = "UDBlobMass"
        Me.UDBlobMass.Size = New System.Drawing.Size(46, 20)
        Me.UDBlobMass.TabIndex = 133
        Me.UDBlobMass.Value = New Decimal(New Integer() {16, 0, 0, 0})
        '
        'LabelBlobMass
        '
        Me.LabelBlobMass.AutoSize = True
        Me.LabelBlobMass.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.LabelBlobMass.Enabled = False
        Me.LabelBlobMass.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelBlobMass.Location = New System.Drawing.Point(706, 466)
        Me.LabelBlobMass.Name = "LabelBlobMass"
        Me.LabelBlobMass.Size = New System.Drawing.Size(100, 13)
        Me.LabelBlobMass.TabIndex = 134
        Me.LabelBlobMass.Text = "Minimum Blob Mass"
        Me.LabelBlobMass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.ClientSize = New System.Drawing.Size(888, 635)
        Me.Controls.Add(Me.LabelBlobMass)
        Me.Controls.Add(Me.UDBlobMass)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.txtIntercept)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtSlope)
        Me.Controls.Add(Me.txtMessage)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.lblPFC)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblRangeIn)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.lblRangeCm)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.pnlBounds)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.chkBlob)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.txtDataReceived)
        Me.Controls.Add(Me.lblResY)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.lblResX)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.boxBaud)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnGrab)
        Me.Controls.Add(Me.btnDisconnect)
        Me.Controls.Add(Me.btnConnect)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.txtDataToSend)
        Me.Controls.Add(Me.cbbCOMPorts)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.pnlBitmap)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "Parallax Laser Range Finder Image Viewer"
        Me.pnlBounds.ResumeLayout(False)
        Me.pnlBounds.PerformLayout()
        CType(Me.vUDUpper, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.vUDLower, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uUDUpper, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.yUDLower, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.uUDLower, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.yUDUpper, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.pnlBitmap.ResumeLayout(False)
        CType(Me.pboxBitmap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.UDBlobMass, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPFC As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblRangeIn As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents lblRangeCm As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents pnlBounds As System.Windows.Forms.Panel
    Friend WithEvents pnlUpper As System.Windows.Forms.Panel
    Friend WithEvents pnlLower As System.Windows.Forms.Panel
    Friend WithEvents LabelYLower As System.Windows.Forms.Label
    Friend WithEvents vUDUpper As System.Windows.Forms.NumericUpDown
    Friend WithEvents LabelULower As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents LabelVUpper As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents LabelYUpper As System.Windows.Forms.Label
    Friend WithEvents vUDLower As System.Windows.Forms.NumericUpDown
    Friend WithEvents LabelVLower As System.Windows.Forms.Label
    Friend WithEvents uUDUpper As System.Windows.Forms.NumericUpDown
    Friend WithEvents yUDLower As System.Windows.Forms.NumericUpDown
    Friend WithEvents uUDLower As System.Windows.Forms.NumericUpDown
    Friend WithEvents yUDUpper As System.Windows.Forms.NumericUpDown
    Friend WithEvents LabelUUpper As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents radTrackBounds As System.Windows.Forms.RadioButton
    Friend WithEvents radTrackCentroid As System.Windows.Forms.RadioButton
    Friend WithEvents chkBlob As System.Windows.Forms.CheckBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents radSaveBitmap As System.Windows.Forms.RadioButton
    Friend WithEvents radSaveRaw As System.Windows.Forms.RadioButton
    Friend WithEvents txtDataReceived As System.Windows.Forms.RichTextBox
    Friend WithEvents lblResY As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lblResX As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents radFrameFull As System.Windows.Forms.RadioButton
    Friend WithEvents radFrameROI As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents boxBaud As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnGrab As System.Windows.Forms.Button
    Friend WithEvents btnDisconnect As System.Windows.Forms.Button
    Friend WithEvents btnConnect As System.Windows.Forms.Button
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents txtDataToSend As System.Windows.Forms.TextBox
    Friend WithEvents cbbCOMPorts As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pnlBitmap As System.Windows.Forms.Panel
    Friend WithEvents pboxBitmap As System.Windows.Forms.PictureBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtMessage As System.Windows.Forms.RichTextBox
    Friend WithEvents txtSlope As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtIntercept As System.Windows.Forms.TextBox
    Friend WithEvents radFrameROIProc As System.Windows.Forms.RadioButton
    Friend WithEvents UDBlobMass As System.Windows.Forms.NumericUpDown
    Friend WithEvents LabelBlobMass As System.Windows.Forms.Label

End Class
