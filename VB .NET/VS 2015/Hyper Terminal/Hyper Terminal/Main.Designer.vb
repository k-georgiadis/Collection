<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.closeApplication = New System.Windows.Forms.Button()
        Me.transmitData = New System.Windows.Forms.Button()
        Me.outputTextBox = New System.Windows.Forms.RichTextBox()
        Me.inputData = New System.Windows.Forms.RichTextBox()
        Me.serialportlbl = New System.Windows.Forms.Label()
        Me.portList = New System.Windows.Forms.ComboBox()
        Me.rateList = New System.Windows.Forms.ComboBox()
        Me.baudratelbl = New System.Windows.Forms.Label()
        Me.databitsList = New System.Windows.Forms.ComboBox()
        Me.databitslbl = New System.Windows.Forms.Label()
        Me.stopbitsList = New System.Windows.Forms.ComboBox()
        Me.stopbitslbl = New System.Windows.Forms.Label()
        Me.parityList = New System.Windows.Forms.ComboBox()
        Me.paritylbl = New System.Windows.Forms.Label()
        Me.optionsGB = New System.Windows.Forms.GroupBox()
        Me.usePort = New System.Windows.Forms.Button()
        Me.stringMode = New System.Windows.Forms.RadioButton()
        Me.byteMode = New System.Windows.Forms.RadioButton()
        Me.modeSel = New System.Windows.Forms.GroupBox()
        Me.hexTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.echoCheck = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.sendCapsCheck = New System.Windows.Forms.CheckBox()
        Me.clearInputCheck = New System.Windows.Forms.CheckBox()
        Me.appendLbl = New System.Windows.Forms.Label()
        Me.appendList = New System.Windows.Forms.ComboBox()
        Me.portListTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.optionsGB.SuspendLayout()
        Me.modeSel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'closeApplication
        '
        Me.closeApplication.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.closeApplication.Location = New System.Drawing.Point(649, 415)
        Me.closeApplication.Name = "closeApplication"
        Me.closeApplication.Size = New System.Drawing.Size(63, 25)
        Me.closeApplication.TabIndex = 7
        Me.closeApplication.Text = "Exit"
        Me.closeApplication.UseVisualStyleBackColor = True
        '
        'transmitData
        '
        Me.transmitData.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.transmitData.Location = New System.Drawing.Point(12, 415)
        Me.transmitData.Name = "transmitData"
        Me.transmitData.Size = New System.Drawing.Size(96, 25)
        Me.transmitData.TabIndex = 1
        Me.transmitData.Text = "Send"
        Me.transmitData.UseVisualStyleBackColor = True
        '
        'outputTextBox
        '
        Me.outputTextBox.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.outputTextBox.Location = New System.Drawing.Point(12, 12)
        Me.outputTextBox.Name = "outputTextBox"
        Me.outputTextBox.ReadOnly = True
        Me.outputTextBox.Size = New System.Drawing.Size(513, 305)
        Me.outputTextBox.TabIndex = 2
        Me.outputTextBox.Text = ""
        '
        'inputData
        '
        Me.inputData.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.inputData.Location = New System.Drawing.Point(12, 325)
        Me.inputData.Name = "inputData"
        Me.inputData.Size = New System.Drawing.Size(513, 83)
        Me.inputData.TabIndex = 6
        Me.inputData.Text = ""
        '
        'serialportlbl
        '
        Me.serialportlbl.AutoSize = True
        Me.serialportlbl.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.serialportlbl.Location = New System.Drawing.Point(6, 29)
        Me.serialportlbl.Name = "serialportlbl"
        Me.serialportlbl.Size = New System.Drawing.Size(91, 15)
        Me.serialportlbl.TabIndex = 9
        Me.serialportlbl.Text = "Serial Port:"
        '
        'portList
        '
        Me.portList.Cursor = System.Windows.Forms.Cursors.Default
        Me.portList.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.portList.FormattingEnabled = True
        Me.portList.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.portList.Location = New System.Drawing.Point(103, 25)
        Me.portList.Name = "portList"
        Me.portList.Size = New System.Drawing.Size(72, 23)
        Me.portList.TabIndex = 10
        '
        'rateList
        '
        Me.rateList.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.rateList.FormattingEnabled = True
        Me.rateList.Items.AddRange(New Object() {"2400", "4800", "9600", "19200", "28800", "38400", "57600", "76800", "115200", "230400", "250000"})
        Me.rateList.Location = New System.Drawing.Point(103, 56)
        Me.rateList.Name = "rateList"
        Me.rateList.Size = New System.Drawing.Size(72, 23)
        Me.rateList.TabIndex = 12
        '
        'baudratelbl
        '
        Me.baudratelbl.AutoSize = True
        Me.baudratelbl.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.baudratelbl.Location = New System.Drawing.Point(6, 60)
        Me.baudratelbl.Name = "baudratelbl"
        Me.baudratelbl.Size = New System.Drawing.Size(77, 15)
        Me.baudratelbl.TabIndex = 11
        Me.baudratelbl.Text = "Baud Rate:"
        '
        'databitsList
        '
        Me.databitsList.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.databitsList.FormattingEnabled = True
        Me.databitsList.Items.AddRange(New Object() {"5", "6", "7", "8", "9"})
        Me.databitsList.Location = New System.Drawing.Point(103, 86)
        Me.databitsList.Name = "databitsList"
        Me.databitsList.Size = New System.Drawing.Size(72, 23)
        Me.databitsList.TabIndex = 14
        '
        'databitslbl
        '
        Me.databitslbl.AutoSize = True
        Me.databitslbl.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.databitslbl.Location = New System.Drawing.Point(6, 90)
        Me.databitslbl.Name = "databitslbl"
        Me.databitslbl.Size = New System.Drawing.Size(77, 15)
        Me.databitslbl.TabIndex = 13
        Me.databitslbl.Text = "Data bits:"
        '
        'stopbitsList
        '
        Me.stopbitsList.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.stopbitsList.FormattingEnabled = True
        Me.stopbitsList.Location = New System.Drawing.Point(103, 116)
        Me.stopbitsList.Name = "stopbitsList"
        Me.stopbitsList.Size = New System.Drawing.Size(72, 23)
        Me.stopbitsList.TabIndex = 16
        '
        'stopbitslbl
        '
        Me.stopbitslbl.AutoSize = True
        Me.stopbitslbl.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.stopbitslbl.Location = New System.Drawing.Point(6, 120)
        Me.stopbitslbl.Name = "stopbitslbl"
        Me.stopbitslbl.Size = New System.Drawing.Size(77, 15)
        Me.stopbitslbl.TabIndex = 15
        Me.stopbitslbl.Text = "Stop bits:"
        '
        'parityList
        '
        Me.parityList.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.parityList.FormattingEnabled = True
        Me.parityList.Location = New System.Drawing.Point(103, 146)
        Me.parityList.Name = "parityList"
        Me.parityList.Size = New System.Drawing.Size(72, 23)
        Me.parityList.TabIndex = 18
        '
        'paritylbl
        '
        Me.paritylbl.AutoSize = True
        Me.paritylbl.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.paritylbl.Location = New System.Drawing.Point(6, 150)
        Me.paritylbl.Name = "paritylbl"
        Me.paritylbl.Size = New System.Drawing.Size(56, 15)
        Me.paritylbl.TabIndex = 17
        Me.paritylbl.Text = "Parity:"
        '
        'optionsGB
        '
        Me.optionsGB.Controls.Add(Me.serialportlbl)
        Me.optionsGB.Controls.Add(Me.parityList)
        Me.optionsGB.Controls.Add(Me.paritylbl)
        Me.optionsGB.Controls.Add(Me.portList)
        Me.optionsGB.Controls.Add(Me.rateList)
        Me.optionsGB.Controls.Add(Me.stopbitsList)
        Me.optionsGB.Controls.Add(Me.stopbitslbl)
        Me.optionsGB.Controls.Add(Me.baudratelbl)
        Me.optionsGB.Controls.Add(Me.databitsList)
        Me.optionsGB.Controls.Add(Me.databitslbl)
        Me.optionsGB.Font = New System.Drawing.Font("Courier New", 9.0!)
        Me.optionsGB.Location = New System.Drawing.Point(531, 12)
        Me.optionsGB.Name = "optionsGB"
        Me.optionsGB.Size = New System.Drawing.Size(182, 199)
        Me.optionsGB.TabIndex = 19
        Me.optionsGB.TabStop = False
        Me.optionsGB.Text = "Options"
        '
        'usePort
        '
        Me.usePort.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.usePort.Location = New System.Drawing.Point(531, 415)
        Me.usePort.Name = "usePort"
        Me.usePort.Size = New System.Drawing.Size(112, 25)
        Me.usePort.TabIndex = 20
        Me.usePort.Text = "Open Port"
        Me.usePort.UseVisualStyleBackColor = True
        '
        'stringMode
        '
        Me.stringMode.AutoSize = True
        Me.stringMode.Checked = True
        Me.stringMode.Location = New System.Drawing.Point(9, 19)
        Me.stringMode.Name = "stringMode"
        Me.stringMode.Size = New System.Drawing.Size(67, 19)
        Me.stringMode.TabIndex = 21
        Me.stringMode.TabStop = True
        Me.stringMode.Text = "String"
        Me.stringMode.UseVisualStyleBackColor = True
        '
        'byteMode
        '
        Me.byteMode.AutoSize = True
        Me.byteMode.Location = New System.Drawing.Point(103, 19)
        Me.byteMode.Name = "byteMode"
        Me.byteMode.Size = New System.Drawing.Size(46, 19)
        Me.byteMode.TabIndex = 22
        Me.byteMode.Text = "Hex"
        Me.byteMode.UseVisualStyleBackColor = True
        '
        'modeSel
        '
        Me.modeSel.Controls.Add(Me.stringMode)
        Me.modeSel.Controls.Add(Me.byteMode)
        Me.modeSel.Font = New System.Drawing.Font("Courier New", 9.0!)
        Me.modeSel.Location = New System.Drawing.Point(531, 217)
        Me.modeSel.Name = "modeSel"
        Me.modeSel.Size = New System.Drawing.Size(182, 49)
        Me.modeSel.TabIndex = 23
        Me.modeSel.TabStop = False
        Me.modeSel.Text = "Mode"
        '
        'hexTooltip
        '
        Me.hexTooltip.IsBalloon = True
        '
        'echoCheck
        '
        Me.echoCheck.AutoSize = True
        Me.echoCheck.Font = New System.Drawing.Font("Courier New", 9.0!)
        Me.echoCheck.Location = New System.Drawing.Point(9, 52)
        Me.echoCheck.Name = "echoCheck"
        Me.echoCheck.Size = New System.Drawing.Size(96, 19)
        Me.echoCheck.TabIndex = 24
        Me.echoCheck.Text = "Local Echo"
        Me.echoCheck.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.sendCapsCheck)
        Me.GroupBox1.Controls.Add(Me.clearInputCheck)
        Me.GroupBox1.Controls.Add(Me.appendLbl)
        Me.GroupBox1.Controls.Add(Me.appendList)
        Me.GroupBox1.Controls.Add(Me.echoCheck)
        Me.GroupBox1.Font = New System.Drawing.Font("Courier New", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(531, 273)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(181, 135)
        Me.GroupBox1.TabIndex = 25
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Trasmit Options"
        '
        'sendCapsCheck
        '
        Me.sendCapsCheck.AutoSize = True
        Me.sendCapsCheck.Font = New System.Drawing.Font("Courier New", 9.0!)
        Me.sendCapsCheck.Location = New System.Drawing.Point(9, 79)
        Me.sendCapsCheck.Name = "sendCapsCheck"
        Me.sendCapsCheck.Size = New System.Drawing.Size(89, 19)
        Me.sendCapsCheck.TabIndex = 27
        Me.sendCapsCheck.Text = "Send Caps"
        Me.sendCapsCheck.UseVisualStyleBackColor = True
        '
        'clearInputCheck
        '
        Me.clearInputCheck.AutoSize = True
        Me.clearInputCheck.Font = New System.Drawing.Font("Courier New", 9.0!)
        Me.clearInputCheck.Location = New System.Drawing.Point(9, 106)
        Me.clearInputCheck.Name = "clearInputCheck"
        Me.clearInputCheck.Size = New System.Drawing.Size(159, 19)
        Me.clearInputCheck.TabIndex = 26
        Me.clearInputCheck.Text = "Clear input on Send"
        Me.clearInputCheck.UseVisualStyleBackColor = True
        '
        'appendLbl
        '
        Me.appendLbl.AutoSize = True
        Me.appendLbl.Location = New System.Drawing.Point(6, 29)
        Me.appendLbl.Name = "appendLbl"
        Me.appendLbl.Size = New System.Drawing.Size(63, 15)
        Me.appendLbl.TabIndex = 25
        Me.appendLbl.Text = "Append :"
        '
        'appendList
        '
        Me.appendList.Cursor = System.Windows.Forms.Cursors.Default
        Me.appendList.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.appendList.FormattingEnabled = True
        Me.appendList.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.appendList.Location = New System.Drawing.Point(103, 26)
        Me.appendList.Name = "appendList"
        Me.appendList.Size = New System.Drawing.Size(72, 23)
        Me.appendList.TabIndex = 19
        '
        'portListTooltip
        '
        Me.portListTooltip.IsBalloon = True
        '
        'Main
        '
        Me.AcceptButton = Me.transmitData
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(724, 446)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.modeSel)
        Me.Controls.Add(Me.usePort)
        Me.Controls.Add(Me.optionsGB)
        Me.Controls.Add(Me.inputData)
        Me.Controls.Add(Me.outputTextBox)
        Me.Controls.Add(Me.transmitData)
        Me.Controls.Add(Me.closeApplication)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Hyper Terminal"
        Me.optionsGB.ResumeLayout(False)
        Me.optionsGB.PerformLayout()
        Me.modeSel.ResumeLayout(False)
        Me.modeSel.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents closeApplication As Button
    Friend WithEvents transmitData As Button
    Friend WithEvents outputTextBox As RichTextBox
    Friend WithEvents inputData As RichTextBox
    Friend WithEvents serialportlbl As Label
    Friend WithEvents portList As ComboBox
    Friend WithEvents rateList As ComboBox
    Friend WithEvents baudratelbl As Label
    Friend WithEvents databitsList As ComboBox
    Friend WithEvents databitslbl As Label
    Friend WithEvents stopbitsList As ComboBox
    Friend WithEvents stopbitslbl As Label
    Friend WithEvents parityList As ComboBox
    Friend WithEvents paritylbl As Label
    Friend WithEvents optionsGB As GroupBox
    Friend WithEvents usePort As Button
    Friend WithEvents stringMode As RadioButton
    Friend WithEvents byteMode As RadioButton
    Friend WithEvents modeSel As GroupBox
    Friend WithEvents hexTooltip As ToolTip
    Friend WithEvents echoCheck As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents appendLbl As Label
    Friend WithEvents appendList As ComboBox
    Friend WithEvents clearInputCheck As CheckBox
    Friend WithEvents sendCapsCheck As CheckBox
    Friend WithEvents portListTooltip As ToolTip
End Class
