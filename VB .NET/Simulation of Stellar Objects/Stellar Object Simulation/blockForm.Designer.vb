'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class blockForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(blockForm))
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Stars", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Planets", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("BlackHoles", System.Windows.Forms.HorizontalAlignment.Left)
        Me.lblCoordsMouse = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblTimeTick = New System.Windows.Forms.Label()
        Me.numTimeTick = New System.Windows.Forms.NumericUpDown()
        Me.planetslbl = New System.Windows.Forms.Label()
        Me.chkCanvasClear = New System.Windows.Forms.CheckBox()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.boxParam = New System.Windows.Forms.GroupBox()
        Me.lblParamZPos = New System.Windows.Forms.Label()
        Me.numParamZPos = New System.Windows.Forms.NumericUpDown()
        Me.lblParamZVel = New System.Windows.Forms.Label()
        Me.numParamZVel = New System.Windows.Forms.NumericUpDown()
        Me.lblParamMass = New System.Windows.Forms.Label()
        Me.numParamMass = New System.Windows.Forms.NumericUpDown()
        Me.lblParamYVel = New System.Windows.Forms.Label()
        Me.numParamYVel = New System.Windows.Forms.NumericUpDown()
        Me.lblParamXVel = New System.Windows.Forms.Label()
        Me.numParamXVel = New System.Windows.Forms.NumericUpDown()
        Me.boxTimeParam = New System.Windows.Forms.GroupBox()
        Me.cbTickSpeed = New System.Windows.Forms.ComboBox()
        Me.boxMode = New System.Windows.Forms.GroupBox()
        Me.chkbNoCreation = New System.Windows.Forms.CheckBox()
        Me.cbCreateType = New System.Windows.Forms.ComboBox()
        Me.boxGeneral = New System.Windows.Forms.GroupBox()
        Me.chkRealisticSim = New System.Windows.Forms.CheckBox()
        Me.numTraj = New System.Windows.Forms.NumericUpDown()
        Me.chkDrawTrajectories = New System.Windows.Forms.CheckBox()
        Me.chkFollowSelected = New System.Windows.Forms.CheckBox()
        Me.boxCollision = New System.Windows.Forms.GroupBox()
        Me.radCollisionNone = New System.Windows.Forms.RadioButton()
        Me.radCollisionBounce = New System.Windows.Forms.RadioButton()
        Me.radCollisionTunnel = New System.Windows.Forms.RadioButton()
        Me.trajTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnSelectTool = New System.Windows.Forms.Button()
        Me.imgListActionBtn = New System.Windows.Forms.ImageList(Me.components)
        Me.btnZoomTool = New System.Windows.Forms.Button()
        Me.btnZoomAreaTool = New System.Windows.Forms.Button()
        Me.btnNavReturn = New System.Windows.Forms.Button()
        Me.imgListNavBtn = New System.Windows.Forms.ImageList(Me.components)
        Me.boxCoords = New System.Windows.Forms.GroupBox()
        Me.lblCoordsAbs = New System.Windows.Forms.Label()
        Me.boxObjectInfo = New System.Windows.Forms.GroupBox()
        Me.objectListView = New System.Windows.Forms.ListView()
        Me.NameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LocationXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LocationYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LocationZHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationZHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityZHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MassHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SizeHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnPauseUniverse = New System.Windows.Forms.Button()
        Me.pnlActionLeft = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.lblStatusMessage = New System.Windows.Forms.Label()
        Me.boxSelParam = New System.Windows.Forms.GroupBox()
        Me.chkHideInfo = New System.Windows.Forms.CheckBox()
        Me.boxTraj = New System.Windows.Forms.GroupBox()
        Me.radBothTraj = New System.Windows.Forms.RadioButton()
        Me.radRelativeTraj = New System.Windows.Forms.RadioButton()
        Me.radRealTraj = New System.Windows.Forms.RadioButton()
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.pnlActionRight = New System.Windows.Forms.FlowLayoutPanel()
        Me.boxMinimap = New System.Windows.Forms.GroupBox()
        Me.pnlCosmos = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblCosmosTitle = New System.Windows.Forms.Label()
        Me.btnSection1 = New System.Windows.Forms.Button()
        Me.btnSection2 = New System.Windows.Forms.Button()
        Me.btnSection3 = New System.Windows.Forms.Button()
        Me.btnSection4 = New System.Windows.Forms.Button()
        Me.btnSection5 = New System.Windows.Forms.Button()
        Me.btnSection6 = New System.Windows.Forms.Button()
        Me.btnSection7 = New System.Windows.Forms.Button()
        Me.btnSection8 = New System.Windows.Forms.Button()
        Me.btnSection9 = New System.Windows.Forms.Button()
        Me.picMinimap = New System.Windows.Forms.PictureBox()
        Me.picUniverse = New System.Windows.Forms.PictureBox()
        CType(Me.numTimeTick, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxParam.SuspendLayout()
        CType(Me.numParamZPos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numParamZVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numParamMass, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numParamYVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numParamXVel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxTimeParam.SuspendLayout()
        Me.boxMode.SuspendLayout()
        Me.boxGeneral.SuspendLayout()
        CType(Me.numTraj, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxCollision.SuspendLayout()
        Me.boxCoords.SuspendLayout()
        Me.boxObjectInfo.SuspendLayout()
        Me.pnlActionLeft.SuspendLayout()
        Me.boxSelParam.SuspendLayout()
        Me.boxTraj.SuspendLayout()
        Me.pnlActionRight.SuspendLayout()
        Me.boxMinimap.SuspendLayout()
        Me.pnlCosmos.SuspendLayout()
        CType(Me.picMinimap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picUniverse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCoordsMouse
        '
        Me.lblCoordsMouse.AutoSize = True
        Me.lblCoordsMouse.CausesValidation = False
        Me.lblCoordsMouse.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblCoordsMouse.Location = New System.Drawing.Point(8, 31)
        Me.lblCoordsMouse.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCoordsMouse.Name = "lblCoordsMouse"
        Me.lblCoordsMouse.Size = New System.Drawing.Size(0, 29)
        Me.lblCoordsMouse.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'lblTimeTick
        '
        Me.lblTimeTick.AutoSize = True
        Me.lblTimeTick.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblTimeTick.Location = New System.Drawing.Point(8, 37)
        Me.lblTimeTick.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTimeTick.Name = "lblTimeTick"
        Me.lblTimeTick.Size = New System.Drawing.Size(62, 23)
        Me.lblTimeTick.TabIndex = 8
        Me.lblTimeTick.Text = "Speed:"
        '
        'numTimeTick
        '
        Me.numTimeTick.Enabled = False
        Me.numTimeTick.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numTimeTick.Location = New System.Drawing.Point(198, 34)
        Me.numTimeTick.Margin = New System.Windows.Forms.Padding(4)
        Me.numTimeTick.Maximum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.numTimeTick.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numTimeTick.Name = "numTimeTick"
        Me.numTimeTick.Size = New System.Drawing.Size(98, 30)
        Me.numTimeTick.TabIndex = 7
        Me.numTimeTick.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numTimeTick.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'planetslbl
        '
        Me.planetslbl.AutoSize = True
        Me.planetslbl.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.planetslbl.Location = New System.Drawing.Point(1476, 652)
        Me.planetslbl.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.planetslbl.Name = "planetslbl"
        Me.planetslbl.Size = New System.Drawing.Size(0, 23)
        Me.planetslbl.TabIndex = 10
        '
        'chkCanvasClear
        '
        Me.chkCanvasClear.AutoSize = True
        Me.chkCanvasClear.Checked = True
        Me.chkCanvasClear.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCanvasClear.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkCanvasClear.Location = New System.Drawing.Point(12, 31)
        Me.chkCanvasClear.Margin = New System.Windows.Forms.Padding(4)
        Me.chkCanvasClear.Name = "chkCanvasClear"
        Me.chkCanvasClear.Size = New System.Drawing.Size(128, 27)
        Me.chkCanvasClear.TabIndex = 11
        Me.chkCanvasClear.Text = "Clear canvas"
        Me.chkCanvasClear.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        Me.btnReset.BackColor = System.Drawing.Color.Black
        Me.btnReset.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia
        Me.btnReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue
        Me.btnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReset.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnReset.ForeColor = System.Drawing.Color.Yellow
        Me.btnReset.Location = New System.Drawing.Point(451, 10)
        Me.btnReset.Margin = New System.Windows.Forms.Padding(4)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(160, 38)
        Me.btnReset.TabIndex = 12
        Me.btnReset.Text = "Reset"
        Me.btnReset.UseVisualStyleBackColor = False
        '
        'boxParam
        '
        Me.boxParam.Controls.Add(Me.lblParamZPos)
        Me.boxParam.Controls.Add(Me.numParamZPos)
        Me.boxParam.Controls.Add(Me.lblParamZVel)
        Me.boxParam.Controls.Add(Me.numParamZVel)
        Me.boxParam.Controls.Add(Me.lblParamMass)
        Me.boxParam.Controls.Add(Me.numParamMass)
        Me.boxParam.Controls.Add(Me.lblParamYVel)
        Me.boxParam.Controls.Add(Me.numParamYVel)
        Me.boxParam.Controls.Add(Me.lblParamXVel)
        Me.boxParam.Controls.Add(Me.numParamXVel)
        Me.boxParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxParam.Location = New System.Drawing.Point(1480, 11)
        Me.boxParam.Margin = New System.Windows.Forms.Padding(4)
        Me.boxParam.Name = "boxParam"
        Me.boxParam.Padding = New System.Windows.Forms.Padding(4)
        Me.boxParam.Size = New System.Drawing.Size(304, 222)
        Me.boxParam.TabIndex = 14
        Me.boxParam.TabStop = False
        Me.boxParam.Text = "Creation Parameters"
        '
        'lblParamZPos
        '
        Me.lblParamZPos.AutoSize = True
        Me.lblParamZPos.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblParamZPos.Location = New System.Drawing.Point(8, 148)
        Me.lblParamZPos.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblParamZPos.Name = "lblParamZPos"
        Me.lblParamZPos.Size = New System.Drawing.Size(91, 23)
        Me.lblParamZPos.TabIndex = 16
        Me.lblParamZPos.Text = "Z Position:"
        '
        'numParamZPos
        '
        Me.numParamZPos.DecimalPlaces = 2
        Me.numParamZPos.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numParamZPos.Location = New System.Drawing.Point(124, 145)
        Me.numParamZPos.Margin = New System.Windows.Forms.Padding(4)
        Me.numParamZPos.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.numParamZPos.Minimum = New Decimal(New Integer() {1000000, 0, 0, -2147483648})
        Me.numParamZPos.Name = "numParamZPos"
        Me.numParamZPos.Size = New System.Drawing.Size(172, 30)
        Me.numParamZPos.TabIndex = 15
        Me.numParamZPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.trajTooltip.SetToolTip(Me.numParamZPos, "1 : 1 Million km")
        '
        'lblParamZVel
        '
        Me.lblParamZVel.AutoSize = True
        Me.lblParamZVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblParamZVel.Location = New System.Drawing.Point(8, 110)
        Me.lblParamZVel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblParamZVel.Name = "lblParamZVel"
        Me.lblParamZVel.Size = New System.Drawing.Size(88, 23)
        Me.lblParamZVel.TabIndex = 14
        Me.lblParamZVel.Text = "Z Velocity:"
        '
        'numParamZVel
        '
        Me.numParamZVel.DecimalPlaces = 2
        Me.numParamZVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numParamZVel.Location = New System.Drawing.Point(124, 107)
        Me.numParamZVel.Margin = New System.Windows.Forms.Padding(4)
        Me.numParamZVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numParamZVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numParamZVel.Name = "numParamZVel"
        Me.numParamZVel.Size = New System.Drawing.Size(172, 30)
        Me.numParamZVel.TabIndex = 13
        Me.numParamZVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblParamMass
        '
        Me.lblParamMass.AutoSize = True
        Me.lblParamMass.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblParamMass.Location = New System.Drawing.Point(8, 186)
        Me.lblParamMass.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblParamMass.Name = "lblParamMass"
        Me.lblParamMass.Size = New System.Drawing.Size(56, 23)
        Me.lblParamMass.TabIndex = 12
        Me.lblParamMass.Text = "Mass:"
        '
        'numParamMass
        '
        Me.numParamMass.DecimalPlaces = 2
        Me.numParamMass.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numParamMass.Location = New System.Drawing.Point(124, 183)
        Me.numParamMass.Margin = New System.Windows.Forms.Padding(4)
        Me.numParamMass.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numParamMass.Name = "numParamMass"
        Me.numParamMass.Size = New System.Drawing.Size(172, 30)
        Me.numParamMass.TabIndex = 11
        Me.numParamMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.trajTooltip.SetToolTip(Me.numParamMass, "Solar masses for Stars, Earth masses for Planets.")
        Me.numParamMass.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblParamYVel
        '
        Me.lblParamYVel.AutoSize = True
        Me.lblParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblParamYVel.Location = New System.Drawing.Point(8, 71)
        Me.lblParamYVel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblParamYVel.Name = "lblParamYVel"
        Me.lblParamYVel.Size = New System.Drawing.Size(88, 23)
        Me.lblParamYVel.TabIndex = 10
        Me.lblParamYVel.Text = "Y Velocity:"
        '
        'numParamYVel
        '
        Me.numParamYVel.DecimalPlaces = 2
        Me.numParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numParamYVel.Location = New System.Drawing.Point(124, 69)
        Me.numParamYVel.Margin = New System.Windows.Forms.Padding(4)
        Me.numParamYVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numParamYVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numParamYVel.Name = "numParamYVel"
        Me.numParamYVel.Size = New System.Drawing.Size(172, 30)
        Me.numParamYVel.TabIndex = 9
        Me.numParamYVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblParamXVel
        '
        Me.lblParamXVel.AutoSize = True
        Me.lblParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblParamXVel.Location = New System.Drawing.Point(8, 33)
        Me.lblParamXVel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblParamXVel.Name = "lblParamXVel"
        Me.lblParamXVel.Size = New System.Drawing.Size(89, 23)
        Me.lblParamXVel.TabIndex = 5
        Me.lblParamXVel.Text = "X Velocity:"
        '
        'numParamXVel
        '
        Me.numParamXVel.DecimalPlaces = 2
        Me.numParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numParamXVel.Location = New System.Drawing.Point(124, 31)
        Me.numParamXVel.Margin = New System.Windows.Forms.Padding(4)
        Me.numParamXVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numParamXVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numParamXVel.Name = "numParamXVel"
        Me.numParamXVel.Size = New System.Drawing.Size(172, 30)
        Me.numParamXVel.TabIndex = 4
        Me.numParamXVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'boxTimeParam
        '
        Me.boxTimeParam.Controls.Add(Me.cbTickSpeed)
        Me.boxTimeParam.Controls.Add(Me.numTimeTick)
        Me.boxTimeParam.Controls.Add(Me.lblTimeTick)
        Me.boxTimeParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxTimeParam.Location = New System.Drawing.Point(1480, 542)
        Me.boxTimeParam.Margin = New System.Windows.Forms.Padding(4)
        Me.boxTimeParam.Name = "boxTimeParam"
        Me.boxTimeParam.Padding = New System.Windows.Forms.Padding(4)
        Me.boxTimeParam.Size = New System.Drawing.Size(304, 75)
        Me.boxTimeParam.TabIndex = 14
        Me.boxTimeParam.TabStop = False
        Me.boxTimeParam.Text = "Universe Tick Speed"
        '
        'cbTickSpeed
        '
        Me.cbTickSpeed.FormattingEnabled = True
        Me.cbTickSpeed.Items.AddRange(New Object() {"Custom", "Fast", "Faster", "Fastest"})
        Me.cbTickSpeed.Location = New System.Drawing.Point(77, 34)
        Me.cbTickSpeed.Name = "cbTickSpeed"
        Me.cbTickSpeed.Size = New System.Drawing.Size(114, 31)
        Me.cbTickSpeed.TabIndex = 9
        '
        'boxMode
        '
        Me.boxMode.Controls.Add(Me.chkbNoCreation)
        Me.boxMode.Controls.Add(Me.cbCreateType)
        Me.boxMode.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxMode.Location = New System.Drawing.Point(1480, 240)
        Me.boxMode.Margin = New System.Windows.Forms.Padding(4)
        Me.boxMode.Name = "boxMode"
        Me.boxMode.Padding = New System.Windows.Forms.Padding(4)
        Me.boxMode.Size = New System.Drawing.Size(304, 69)
        Me.boxMode.TabIndex = 15
        Me.boxMode.TabStop = False
        Me.boxMode.Text = "Creation Mode"
        '
        'chkbNoCreation
        '
        Me.chkbNoCreation.AutoSize = True
        Me.chkbNoCreation.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkbNoCreation.Location = New System.Drawing.Point(215, 32)
        Me.chkbNoCreation.Name = "chkbNoCreation"
        Me.chkbNoCreation.Size = New System.Drawing.Size(73, 27)
        Me.chkbNoCreation.TabIndex = 23
        Me.chkbNoCreation.Text = "None"
        Me.chkbNoCreation.UseVisualStyleBackColor = True
        '
        'cbCreateType
        '
        Me.cbCreateType.FormattingEnabled = True
        Me.cbCreateType.Items.AddRange(New Object() {"Planet", "Star", "Black hole"})
        Me.cbCreateType.Location = New System.Drawing.Point(12, 30)
        Me.cbCreateType.Name = "cbCreateType"
        Me.cbCreateType.Size = New System.Drawing.Size(197, 31)
        Me.cbCreateType.TabIndex = 22
        '
        'boxGeneral
        '
        Me.boxGeneral.Controls.Add(Me.chkRealisticSim)
        Me.boxGeneral.Controls.Add(Me.numTraj)
        Me.boxGeneral.Controls.Add(Me.chkDrawTrajectories)
        Me.boxGeneral.Controls.Add(Me.chkCanvasClear)
        Me.boxGeneral.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxGeneral.Location = New System.Drawing.Point(1480, 625)
        Me.boxGeneral.Margin = New System.Windows.Forms.Padding(4)
        Me.boxGeneral.Name = "boxGeneral"
        Me.boxGeneral.Padding = New System.Windows.Forms.Padding(4)
        Me.boxGeneral.Size = New System.Drawing.Size(304, 270)
        Me.boxGeneral.TabIndex = 16
        Me.boxGeneral.TabStop = False
        Me.boxGeneral.Text = "Generic Parameters"
        '
        'chkRealisticSim
        '
        Me.chkRealisticSim.AutoSize = True
        Me.chkRealisticSim.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkRealisticSim.Location = New System.Drawing.Point(12, 65)
        Me.chkRealisticSim.Margin = New System.Windows.Forms.Padding(4)
        Me.chkRealisticSim.Name = "chkRealisticSim"
        Me.chkRealisticSim.Size = New System.Drawing.Size(181, 27)
        Me.chkRealisticSim.TabIndex = 13
        Me.chkRealisticSim.Text = "Realistic Simulation"
        Me.chkRealisticSim.UseVisualStyleBackColor = True
        '
        'numTraj
        '
        Me.numTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numTraj.Location = New System.Drawing.Point(175, 98)
        Me.numTraj.Margin = New System.Windows.Forms.Padding(4)
        Me.numTraj.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.numTraj.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.numTraj.Name = "numTraj"
        Me.numTraj.Size = New System.Drawing.Size(121, 30)
        Me.numTraj.TabIndex = 9
        Me.numTraj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numTraj.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'chkDrawTrajectories
        '
        Me.chkDrawTrajectories.AutoSize = True
        Me.chkDrawTrajectories.Checked = True
        Me.chkDrawTrajectories.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDrawTrajectories.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkDrawTrajectories.Location = New System.Drawing.Point(12, 100)
        Me.chkDrawTrajectories.Margin = New System.Windows.Forms.Padding(4)
        Me.chkDrawTrajectories.Name = "chkDrawTrajectories"
        Me.chkDrawTrajectories.Size = New System.Drawing.Size(144, 27)
        Me.chkDrawTrajectories.TabIndex = 12
        Me.chkDrawTrajectories.Text = "Trajectory Size"
        Me.chkDrawTrajectories.UseVisualStyleBackColor = True
        '
        'chkFollowSelected
        '
        Me.chkFollowSelected.AutoSize = True
        Me.chkFollowSelected.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkFollowSelected.Location = New System.Drawing.Point(12, 31)
        Me.chkFollowSelected.Margin = New System.Windows.Forms.Padding(4)
        Me.chkFollowSelected.Name = "chkFollowSelected"
        Me.chkFollowSelected.Size = New System.Drawing.Size(83, 27)
        Me.chkFollowSelected.TabIndex = 13
        Me.chkFollowSelected.Text = "Follow"
        Me.chkFollowSelected.UseVisualStyleBackColor = True
        '
        'boxCollision
        '
        Me.boxCollision.Controls.Add(Me.radCollisionNone)
        Me.boxCollision.Controls.Add(Me.radCollisionBounce)
        Me.boxCollision.Controls.Add(Me.radCollisionTunnel)
        Me.boxCollision.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxCollision.Location = New System.Drawing.Point(1480, 316)
        Me.boxCollision.Margin = New System.Windows.Forms.Padding(4)
        Me.boxCollision.Name = "boxCollision"
        Me.boxCollision.Padding = New System.Windows.Forms.Padding(4)
        Me.boxCollision.Size = New System.Drawing.Size(304, 69)
        Me.boxCollision.TabIndex = 17
        Me.boxCollision.TabStop = False
        Me.boxCollision.Text = "Collision"
        '
        'radCollisionNone
        '
        Me.radCollisionNone.AutoSize = True
        Me.radCollisionNone.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radCollisionNone.Location = New System.Drawing.Point(215, 31)
        Me.radCollisionNone.Margin = New System.Windows.Forms.Padding(4)
        Me.radCollisionNone.Name = "radCollisionNone"
        Me.radCollisionNone.Size = New System.Drawing.Size(72, 27)
        Me.radCollisionNone.TabIndex = 20
        Me.radCollisionNone.Text = "None"
        Me.radCollisionNone.UseVisualStyleBackColor = True
        '
        'radCollisionBounce
        '
        Me.radCollisionBounce.AutoSize = True
        Me.radCollisionBounce.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radCollisionBounce.Location = New System.Drawing.Point(109, 31)
        Me.radCollisionBounce.Margin = New System.Windows.Forms.Padding(4)
        Me.radCollisionBounce.Name = "radCollisionBounce"
        Me.radCollisionBounce.Size = New System.Drawing.Size(88, 27)
        Me.radCollisionBounce.TabIndex = 19
        Me.radCollisionBounce.Text = "Bounce"
        Me.radCollisionBounce.UseVisualStyleBackColor = True
        '
        'radCollisionTunnel
        '
        Me.radCollisionTunnel.AutoSize = True
        Me.radCollisionTunnel.Checked = True
        Me.radCollisionTunnel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radCollisionTunnel.Location = New System.Drawing.Point(12, 31)
        Me.radCollisionTunnel.Margin = New System.Windows.Forms.Padding(4)
        Me.radCollisionTunnel.Name = "radCollisionTunnel"
        Me.radCollisionTunnel.Size = New System.Drawing.Size(82, 27)
        Me.radCollisionTunnel.TabIndex = 18
        Me.radCollisionTunnel.TabStop = True
        Me.radCollisionTunnel.Text = "Tunnel"
        Me.radCollisionTunnel.UseVisualStyleBackColor = True
        '
        'trajTooltip
        '
        Me.trajTooltip.AutoPopDelay = 10000
        Me.trajTooltip.InitialDelay = 500
        Me.trajTooltip.IsBalloon = True
        Me.trajTooltip.ReshowDelay = 100
        '
        'btnSelectTool
        '
        Me.btnSelectTool.BackColor = System.Drawing.Color.Black
        Me.btnSelectTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnSelectTool.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia
        Me.btnSelectTool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue
        Me.btnSelectTool.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnSelectTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelectTool.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSelectTool.ForeColor = System.Drawing.Color.Yellow
        Me.btnSelectTool.ImageAlign = System.Drawing.ContentAlignment.TopRight
        Me.btnSelectTool.ImageKey = "select2.png"
        Me.btnSelectTool.ImageList = Me.imgListActionBtn
        Me.btnSelectTool.Location = New System.Drawing.Point(717, 10)
        Me.btnSelectTool.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSelectTool.Name = "btnSelectTool"
        Me.btnSelectTool.Size = New System.Drawing.Size(51, 38)
        Me.btnSelectTool.TabIndex = 41
        Me.trajTooltip.SetToolTip(Me.btnSelectTool, "Selection Tool (s)")
        Me.btnSelectTool.UseVisualStyleBackColor = False
        '
        'imgListActionBtn
        '
        Me.imgListActionBtn.ImageStream = CType(resources.GetObject("imgListActionBtn.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListActionBtn.TransparentColor = System.Drawing.Color.Transparent
        Me.imgListActionBtn.Images.SetKeyName(0, "return.png")
        Me.imgListActionBtn.Images.SetKeyName(1, "select.png")
        Me.imgListActionBtn.Images.SetKeyName(2, "select2.png")
        Me.imgListActionBtn.Images.SetKeyName(3, "magnify.png")
        Me.imgListActionBtn.Images.SetKeyName(4, "magnify_scan.png")
        '
        'btnZoomTool
        '
        Me.btnZoomTool.BackColor = System.Drawing.Color.Black
        Me.btnZoomTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnZoomTool.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia
        Me.btnZoomTool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue
        Me.btnZoomTool.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnZoomTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnZoomTool.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnZoomTool.ForeColor = System.Drawing.Color.Yellow
        Me.btnZoomTool.ImageKey = "magnify_scan.png"
        Me.btnZoomTool.ImageList = Me.imgListActionBtn
        Me.btnZoomTool.Location = New System.Drawing.Point(599, 10)
        Me.btnZoomTool.Margin = New System.Windows.Forms.Padding(4)
        Me.btnZoomTool.Name = "btnZoomTool"
        Me.btnZoomTool.Size = New System.Drawing.Size(51, 38)
        Me.btnZoomTool.TabIndex = 42
        Me.trajTooltip.SetToolTip(Me.btnZoomTool, "Zooming Tool (z). Zoom into the Cosmos. Mousewheel zooming is for zooming into th" &
        "e image and not the Cosmos itself.")
        Me.btnZoomTool.UseVisualStyleBackColor = False
        Me.btnZoomTool.Visible = False
        '
        'btnZoomAreaTool
        '
        Me.btnZoomAreaTool.BackColor = System.Drawing.Color.Black
        Me.btnZoomAreaTool.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnZoomAreaTool.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia
        Me.btnZoomAreaTool.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue
        Me.btnZoomAreaTool.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnZoomAreaTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnZoomAreaTool.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnZoomAreaTool.ForeColor = System.Drawing.Color.Yellow
        Me.btnZoomAreaTool.ImageKey = "magnify.png"
        Me.btnZoomAreaTool.ImageList = Me.imgListActionBtn
        Me.btnZoomAreaTool.Location = New System.Drawing.Point(658, 10)
        Me.btnZoomAreaTool.Margin = New System.Windows.Forms.Padding(4)
        Me.btnZoomAreaTool.Name = "btnZoomAreaTool"
        Me.btnZoomAreaTool.Size = New System.Drawing.Size(51, 38)
        Me.btnZoomAreaTool.TabIndex = 43
        Me.trajTooltip.SetToolTip(Me.btnZoomAreaTool, "Area Zooming Tool (a). Zoom to a selected area of the Cosmos.")
        Me.btnZoomAreaTool.UseVisualStyleBackColor = False
        Me.btnZoomAreaTool.Visible = False
        '
        'btnNavReturn
        '
        Me.btnNavReturn.BackColor = System.Drawing.Color.Black
        Me.btnNavReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnNavReturn.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnNavReturn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnNavReturn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
        Me.btnNavReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNavReturn.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnNavReturn.ForeColor = System.Drawing.Color.Yellow
        Me.btnNavReturn.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNavReturn.ImageKey = "return.png"
        Me.btnNavReturn.ImageList = Me.imgListNavBtn
        Me.btnNavReturn.Location = New System.Drawing.Point(17, 35)
        Me.btnNavReturn.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNavReturn.Name = "btnNavReturn"
        Me.btnNavReturn.Size = New System.Drawing.Size(31, 28)
        Me.btnNavReturn.TabIndex = 42
        Me.btnNavReturn.UseVisualStyleBackColor = False
        '
        'imgListNavBtn
        '
        Me.imgListNavBtn.ImageStream = CType(resources.GetObject("imgListNavBtn.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgListNavBtn.TransparentColor = System.Drawing.Color.Transparent
        Me.imgListNavBtn.Images.SetKeyName(0, "return.png")
        '
        'boxCoords
        '
        Me.boxCoords.Controls.Add(Me.lblCoordsAbs)
        Me.boxCoords.Controls.Add(Me.lblCoordsMouse)
        Me.boxCoords.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxCoords.Location = New System.Drawing.Point(1205, 625)
        Me.boxCoords.Margin = New System.Windows.Forms.Padding(4)
        Me.boxCoords.Name = "boxCoords"
        Me.boxCoords.Padding = New System.Windows.Forms.Padding(4)
        Me.boxCoords.Size = New System.Drawing.Size(263, 106)
        Me.boxCoords.TabIndex = 23
        Me.boxCoords.TabStop = False
        Me.boxCoords.Text = "Cursor Pos.(Real/Absolute)"
        '
        'lblCoordsAbs
        '
        Me.lblCoordsAbs.AutoSize = True
        Me.lblCoordsAbs.CausesValidation = False
        Me.lblCoordsAbs.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblCoordsAbs.Location = New System.Drawing.Point(8, 67)
        Me.lblCoordsAbs.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCoordsAbs.Name = "lblCoordsAbs"
        Me.lblCoordsAbs.Size = New System.Drawing.Size(0, 29)
        Me.lblCoordsAbs.TabIndex = 1
        '
        'boxObjectInfo
        '
        Me.boxObjectInfo.Controls.Add(Me.objectListView)
        Me.boxObjectInfo.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxObjectInfo.Location = New System.Drawing.Point(67, 625)
        Me.boxObjectInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.boxObjectInfo.Name = "boxObjectInfo"
        Me.boxObjectInfo.Padding = New System.Windows.Forms.Padding(4)
        Me.boxObjectInfo.Size = New System.Drawing.Size(807, 270)
        Me.boxObjectInfo.TabIndex = 36
        Me.boxObjectInfo.TabStop = False
        Me.boxObjectInfo.Text = "Object List"
        '
        'objectListView
        '
        Me.objectListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NameHeader, Me.LocationXHeader, Me.LocationYHeader, Me.LocationZHeader, Me.AccelerationHeader, Me.AccelerationXHeader, Me.AccelerationYHeader, Me.AccelerationZHeader, Me.VelocityHeader, Me.VelocityXHeader, Me.VelocityYHeader, Me.VelocityZHeader, Me.MassHeader, Me.SizeHeader})
        Me.objectListView.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ListViewGroup1.Header = "Stars"
        ListViewGroup1.Name = "Stars"
        ListViewGroup2.Header = "Planets"
        ListViewGroup2.Name = "Planets"
        ListViewGroup3.Header = "BlackHoles"
        ListViewGroup3.Name = "BlackHoles"
        Me.objectListView.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3})
        Me.objectListView.HideSelection = False
        Me.objectListView.Location = New System.Drawing.Point(8, 27)
        Me.objectListView.Margin = New System.Windows.Forms.Padding(4)
        Me.objectListView.MultiSelect = False
        Me.objectListView.Name = "objectListView"
        Me.objectListView.Size = New System.Drawing.Size(791, 234)
        Me.objectListView.TabIndex = 42
        Me.objectListView.UseCompatibleStateImageBehavior = False
        Me.objectListView.View = System.Windows.Forms.View.Details
        '
        'NameHeader
        '
        Me.NameHeader.Text = "Name"
        Me.NameHeader.Width = 86
        '
        'LocationXHeader
        '
        Me.LocationXHeader.Text = "Location X"
        Me.LocationXHeader.Width = 120
        '
        'LocationYHeader
        '
        Me.LocationYHeader.Text = "Location Y"
        Me.LocationYHeader.Width = 120
        '
        'LocationZHeader
        '
        Me.LocationZHeader.Text = "Location Z"
        Me.LocationZHeader.Width = 120
        '
        'AccelerationHeader
        '
        Me.AccelerationHeader.Text = "Acceleration"
        Me.AccelerationHeader.Width = 120
        '
        'AccelerationXHeader
        '
        Me.AccelerationXHeader.Text = "Acceleration X"
        Me.AccelerationXHeader.Width = 120
        '
        'AccelerationYHeader
        '
        Me.AccelerationYHeader.Text = "Acceleration Y"
        Me.AccelerationYHeader.Width = 120
        '
        'AccelerationZHeader
        '
        Me.AccelerationZHeader.Text = "Acceleration Z"
        Me.AccelerationZHeader.Width = 120
        '
        'VelocityHeader
        '
        Me.VelocityHeader.Text = "Velocity"
        Me.VelocityHeader.Width = 120
        '
        'VelocityXHeader
        '
        Me.VelocityXHeader.Text = "Velocity X"
        Me.VelocityXHeader.Width = 120
        '
        'VelocityYHeader
        '
        Me.VelocityYHeader.Text = "Velocity Y"
        Me.VelocityYHeader.Width = 120
        '
        'VelocityZHeader
        '
        Me.VelocityZHeader.Text = "Velocity Z"
        Me.VelocityZHeader.Width = 120
        '
        'MassHeader
        '
        Me.MassHeader.Text = "Mass"
        Me.MassHeader.Width = 120
        '
        'SizeHeader
        '
        Me.SizeHeader.Text = "Size"
        Me.SizeHeader.Width = 120
        '
        'btnPauseUniverse
        '
        Me.btnPauseUniverse.BackColor = System.Drawing.Color.Black
        Me.btnPauseUniverse.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia
        Me.btnPauseUniverse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue
        Me.btnPauseUniverse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnPauseUniverse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPauseUniverse.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnPauseUniverse.ForeColor = System.Drawing.Color.Yellow
        Me.btnPauseUniverse.Location = New System.Drawing.Point(11, 10)
        Me.btnPauseUniverse.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPauseUniverse.Name = "btnPauseUniverse"
        Me.btnPauseUniverse.Size = New System.Drawing.Size(96, 38)
        Me.btnPauseUniverse.TabIndex = 38
        Me.btnPauseUniverse.Text = "Pause"
        Me.btnPauseUniverse.UseVisualStyleBackColor = False
        '
        'pnlActionLeft
        '
        Me.pnlActionLeft.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.pnlActionLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlActionLeft.Controls.Add(Me.btnPauseUniverse)
        Me.pnlActionLeft.Controls.Add(Me.btnSave)
        Me.pnlActionLeft.Controls.Add(Me.btnLoad)
        Me.pnlActionLeft.Controls.Add(Me.btnReset)
        Me.pnlActionLeft.Location = New System.Drawing.Point(67, 0)
        Me.pnlActionLeft.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlActionLeft.Name = "pnlActionLeft"
        Me.pnlActionLeft.Padding = New System.Windows.Forms.Padding(7, 6, 0, 0)
        Me.pnlActionLeft.Size = New System.Drawing.Size(626, 60)
        Me.pnlActionLeft.TabIndex = 39
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.Black
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia
        Me.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue
        Me.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSave.ForeColor = System.Drawing.Color.Yellow
        Me.btnSave.Location = New System.Drawing.Point(115, 10)
        Me.btnSave.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(160, 38)
        Me.btnSave.TabIndex = 40
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.BackColor = System.Drawing.Color.Black
        Me.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.Fuchsia
        Me.btnLoad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.SlateBlue
        Me.btnLoad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnLoad.ForeColor = System.Drawing.Color.Yellow
        Me.btnLoad.Location = New System.Drawing.Point(283, 10)
        Me.btnLoad.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(160, 38)
        Me.btnLoad.TabIndex = 39
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'lblStatusMessage
        '
        Me.lblStatusMessage.AutoSize = True
        Me.lblStatusMessage.BackColor = System.Drawing.Color.Transparent
        Me.lblStatusMessage.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.lblStatusMessage.ForeColor = System.Drawing.Color.White
        Me.lblStatusMessage.Location = New System.Drawing.Point(77, 69)
        Me.lblStatusMessage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblStatusMessage.Name = "lblStatusMessage"
        Me.lblStatusMessage.Size = New System.Drawing.Size(0, 23)
        Me.lblStatusMessage.TabIndex = 41
        '
        'boxSelParam
        '
        Me.boxSelParam.Controls.Add(Me.chkHideInfo)
        Me.boxSelParam.Controls.Add(Me.chkFollowSelected)
        Me.boxSelParam.Enabled = False
        Me.boxSelParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxSelParam.Location = New System.Drawing.Point(1480, 469)
        Me.boxSelParam.Margin = New System.Windows.Forms.Padding(4)
        Me.boxSelParam.Name = "boxSelParam"
        Me.boxSelParam.Padding = New System.Windows.Forms.Padding(4)
        Me.boxSelParam.Size = New System.Drawing.Size(304, 65)
        Me.boxSelParam.TabIndex = 15
        Me.boxSelParam.TabStop = False
        Me.boxSelParam.Text = "Selection Parameters"
        '
        'chkHideInfo
        '
        Me.chkHideInfo.AutoSize = True
        Me.chkHideInfo.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkHideInfo.Location = New System.Drawing.Point(107, 31)
        Me.chkHideInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.chkHideInfo.Name = "chkHideInfo"
        Me.chkHideInfo.Size = New System.Drawing.Size(122, 27)
        Me.chkHideInfo.TabIndex = 14
        Me.chkHideInfo.Text = "Hide tooltip"
        Me.chkHideInfo.UseVisualStyleBackColor = True
        '
        'boxTraj
        '
        Me.boxTraj.Controls.Add(Me.radBothTraj)
        Me.boxTraj.Controls.Add(Me.radRelativeTraj)
        Me.boxTraj.Controls.Add(Me.radRealTraj)
        Me.boxTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxTraj.Location = New System.Drawing.Point(1480, 393)
        Me.boxTraj.Margin = New System.Windows.Forms.Padding(4)
        Me.boxTraj.Name = "boxTraj"
        Me.boxTraj.Padding = New System.Windows.Forms.Padding(4)
        Me.boxTraj.Size = New System.Drawing.Size(304, 69)
        Me.boxTraj.TabIndex = 21
        Me.boxTraj.TabStop = False
        Me.boxTraj.Text = "Trajectory / Orbit Parameters"
        '
        'radBothTraj
        '
        Me.radBothTraj.AutoSize = True
        Me.radBothTraj.Enabled = False
        Me.radBothTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radBothTraj.Location = New System.Drawing.Point(215, 31)
        Me.radBothTraj.Margin = New System.Windows.Forms.Padding(4)
        Me.radBothTraj.Name = "radBothTraj"
        Me.radBothTraj.Size = New System.Drawing.Size(67, 27)
        Me.radBothTraj.TabIndex = 20
        Me.radBothTraj.Text = "Both"
        Me.radBothTraj.UseVisualStyleBackColor = True
        '
        'radRelativeTraj
        '
        Me.radRelativeTraj.AutoSize = True
        Me.radRelativeTraj.Enabled = False
        Me.radRelativeTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radRelativeTraj.Location = New System.Drawing.Point(107, 31)
        Me.radRelativeTraj.Margin = New System.Windows.Forms.Padding(4)
        Me.radRelativeTraj.Name = "radRelativeTraj"
        Me.radRelativeTraj.Size = New System.Drawing.Size(92, 27)
        Me.radRelativeTraj.TabIndex = 19
        Me.radRelativeTraj.Text = "Relative"
        Me.radRelativeTraj.UseVisualStyleBackColor = True
        '
        'radRealTraj
        '
        Me.radRealTraj.AutoSize = True
        Me.radRealTraj.Checked = True
        Me.radRealTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radRealTraj.Location = New System.Drawing.Point(12, 31)
        Me.radRealTraj.Margin = New System.Windows.Forms.Padding(4)
        Me.radRealTraj.Name = "radRealTraj"
        Me.radRealTraj.Size = New System.Drawing.Size(63, 27)
        Me.radRealTraj.TabIndex = 18
        Me.radRealTraj.TabStop = True
        Me.radRealTraj.Text = "Real"
        Me.radRealTraj.UseVisualStyleBackColor = True
        '
        'pnlActionRight
        '
        Me.pnlActionRight.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.pnlActionRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlActionRight.Controls.Add(Me.btnSelectTool)
        Me.pnlActionRight.Controls.Add(Me.btnZoomAreaTool)
        Me.pnlActionRight.Controls.Add(Me.btnZoomTool)
        Me.pnlActionRight.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.pnlActionRight.Location = New System.Drawing.Point(687, 0)
        Me.pnlActionRight.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlActionRight.Name = "pnlActionRight"
        Me.pnlActionRight.Padding = New System.Windows.Forms.Padding(7, 6, 0, 0)
        Me.pnlActionRight.Size = New System.Drawing.Size(781, 60)
        Me.pnlActionRight.TabIndex = 42
        '
        'boxMinimap
        '
        Me.boxMinimap.Controls.Add(Me.btnNavReturn)
        Me.boxMinimap.Controls.Add(Me.pnlCosmos)
        Me.boxMinimap.Controls.Add(Me.picMinimap)
        Me.boxMinimap.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxMinimap.Location = New System.Drawing.Point(882, 625)
        Me.boxMinimap.Margin = New System.Windows.Forms.Padding(4)
        Me.boxMinimap.Name = "boxMinimap"
        Me.boxMinimap.Padding = New System.Windows.Forms.Padding(4)
        Me.boxMinimap.Size = New System.Drawing.Size(315, 270)
        Me.boxMinimap.TabIndex = 24
        Me.boxMinimap.TabStop = False
        Me.boxMinimap.Text = "Minimap"
        '
        'pnlCosmos
        '
        Me.pnlCosmos.BackColor = System.Drawing.Color.Black
        Me.pnlCosmos.Controls.Add(Me.lblCosmosTitle)
        Me.pnlCosmos.Controls.Add(Me.btnSection1)
        Me.pnlCosmos.Controls.Add(Me.btnSection2)
        Me.pnlCosmos.Controls.Add(Me.btnSection3)
        Me.pnlCosmos.Controls.Add(Me.btnSection4)
        Me.pnlCosmos.Controls.Add(Me.btnSection5)
        Me.pnlCosmos.Controls.Add(Me.btnSection6)
        Me.pnlCosmos.Controls.Add(Me.btnSection7)
        Me.pnlCosmos.Controls.Add(Me.btnSection8)
        Me.pnlCosmos.Controls.Add(Me.btnSection9)
        Me.pnlCosmos.Location = New System.Drawing.Point(19, 27)
        Me.pnlCosmos.Margin = New System.Windows.Forms.Padding(4)
        Me.pnlCosmos.Name = "pnlCosmos"
        Me.pnlCosmos.Padding = New System.Windows.Forms.Padding(7, 6, 0, 0)
        Me.pnlCosmos.Size = New System.Drawing.Size(272, 221)
        Me.pnlCosmos.TabIndex = 43
        Me.pnlCosmos.Visible = False
        '
        'lblCosmosTitle
        '
        Me.lblCosmosTitle.BackColor = System.Drawing.Color.Black
        Me.lblCosmosTitle.ForeColor = System.Drawing.Color.Yellow
        Me.lblCosmosTitle.Location = New System.Drawing.Point(10, 6)
        Me.lblCosmosTitle.Name = "lblCosmosTitle"
        Me.lblCosmosTitle.Size = New System.Drawing.Size(250, 29)
        Me.lblCosmosTitle.TabIndex = 43
        Me.lblCosmosTitle.Text = "The Cosmos"
        Me.lblCosmosTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnSection1
        '
        Me.btnSection1.BackColor = System.Drawing.Color.Black
        Me.btnSection1.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection1.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection1.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection1.Location = New System.Drawing.Point(11, 39)
        Me.btnSection1.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection1.Name = "btnSection1"
        Me.btnSection1.Size = New System.Drawing.Size(78, 55)
        Me.btnSection1.TabIndex = 38
        Me.btnSection1.Text = "1"
        Me.btnSection1.UseVisualStyleBackColor = False
        '
        'btnSection2
        '
        Me.btnSection2.BackColor = System.Drawing.Color.Black
        Me.btnSection2.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection2.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection2.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection2.Location = New System.Drawing.Point(97, 39)
        Me.btnSection2.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection2.Name = "btnSection2"
        Me.btnSection2.Size = New System.Drawing.Size(78, 55)
        Me.btnSection2.TabIndex = 39
        Me.btnSection2.Text = "2"
        Me.btnSection2.UseVisualStyleBackColor = False
        '
        'btnSection3
        '
        Me.btnSection3.BackColor = System.Drawing.Color.Black
        Me.btnSection3.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection3.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection3.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection3.Location = New System.Drawing.Point(183, 39)
        Me.btnSection3.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection3.Name = "btnSection3"
        Me.btnSection3.Size = New System.Drawing.Size(78, 55)
        Me.btnSection3.TabIndex = 40
        Me.btnSection3.Text = "3"
        Me.btnSection3.UseVisualStyleBackColor = False
        '
        'btnSection4
        '
        Me.btnSection4.BackColor = System.Drawing.Color.Black
        Me.btnSection4.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection4.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection4.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection4.Location = New System.Drawing.Point(11, 102)
        Me.btnSection4.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection4.Name = "btnSection4"
        Me.btnSection4.Size = New System.Drawing.Size(78, 55)
        Me.btnSection4.TabIndex = 41
        Me.btnSection4.Text = "4"
        Me.btnSection4.UseVisualStyleBackColor = False
        '
        'btnSection5
        '
        Me.btnSection5.BackColor = System.Drawing.Color.Black
        Me.btnSection5.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection5.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection5.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection5.Location = New System.Drawing.Point(97, 102)
        Me.btnSection5.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection5.Name = "btnSection5"
        Me.btnSection5.Size = New System.Drawing.Size(78, 55)
        Me.btnSection5.TabIndex = 42
        Me.btnSection5.Text = "5"
        Me.btnSection5.UseVisualStyleBackColor = False
        '
        'btnSection6
        '
        Me.btnSection6.BackColor = System.Drawing.Color.Black
        Me.btnSection6.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection6.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection6.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection6.Location = New System.Drawing.Point(183, 102)
        Me.btnSection6.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection6.Name = "btnSection6"
        Me.btnSection6.Size = New System.Drawing.Size(78, 55)
        Me.btnSection6.TabIndex = 43
        Me.btnSection6.Text = "6"
        Me.btnSection6.UseVisualStyleBackColor = False
        '
        'btnSection7
        '
        Me.btnSection7.BackColor = System.Drawing.Color.Black
        Me.btnSection7.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection7.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection7.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection7.Location = New System.Drawing.Point(11, 165)
        Me.btnSection7.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection7.Name = "btnSection7"
        Me.btnSection7.Size = New System.Drawing.Size(78, 55)
        Me.btnSection7.TabIndex = 44
        Me.btnSection7.Text = "7"
        Me.btnSection7.UseVisualStyleBackColor = False
        '
        'btnSection8
        '
        Me.btnSection8.BackColor = System.Drawing.Color.Black
        Me.btnSection8.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection8.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection8.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection8.Location = New System.Drawing.Point(97, 165)
        Me.btnSection8.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection8.Name = "btnSection8"
        Me.btnSection8.Size = New System.Drawing.Size(78, 55)
        Me.btnSection8.TabIndex = 45
        Me.btnSection8.Text = "8"
        Me.btnSection8.UseVisualStyleBackColor = False
        '
        'btnSection9
        '
        Me.btnSection9.BackColor = System.Drawing.Color.Black
        Me.btnSection9.FlatAppearance.BorderColor = System.Drawing.Color.Lime
        Me.btnSection9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSection9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OrangeRed
        Me.btnSection9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSection9.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.btnSection9.ForeColor = System.Drawing.Color.Yellow
        Me.btnSection9.Location = New System.Drawing.Point(183, 165)
        Me.btnSection9.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSection9.Name = "btnSection9"
        Me.btnSection9.Size = New System.Drawing.Size(78, 55)
        Me.btnSection9.TabIndex = 46
        Me.btnSection9.Text = "9"
        Me.btnSection9.UseVisualStyleBackColor = False
        '
        'picMinimap
        '
        Me.picMinimap.BackColor = System.Drawing.Color.Black
        Me.picMinimap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picMinimap.Location = New System.Drawing.Point(7, 27)
        Me.picMinimap.Name = "picMinimap"
        Me.picMinimap.Size = New System.Drawing.Size(296, 233)
        Me.picMinimap.TabIndex = 2
        Me.picMinimap.TabStop = False
        '
        'picUniverse
        '
        Me.picUniverse.BackColor = System.Drawing.Color.Black
        Me.picUniverse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picUniverse.Location = New System.Drawing.Point(67, 62)
        Me.picUniverse.Margin = New System.Windows.Forms.Padding(4)
        Me.picUniverse.Name = "picUniverse"
        Me.picUniverse.Size = New System.Drawing.Size(1401, 555)
        Me.picUniverse.TabIndex = 40
        Me.picUniverse.TabStop = False
        '
        'blockForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.Silver
        Me.ClientSize = New System.Drawing.Size(1793, 902)
        Me.Controls.Add(Me.pnlActionRight)
        Me.Controls.Add(Me.boxTraj)
        Me.Controls.Add(Me.boxSelParam)
        Me.Controls.Add(Me.lblStatusMessage)
        Me.Controls.Add(Me.pnlActionLeft)
        Me.Controls.Add(Me.boxObjectInfo)
        Me.Controls.Add(Me.boxCoords)
        Me.Controls.Add(Me.boxCollision)
        Me.Controls.Add(Me.boxGeneral)
        Me.Controls.Add(Me.boxMode)
        Me.Controls.Add(Me.boxTimeParam)
        Me.Controls.Add(Me.planetslbl)
        Me.Controls.Add(Me.boxParam)
        Me.Controls.Add(Me.picUniverse)
        Me.Controls.Add(Me.boxMinimap)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1811, 779)
        Me.Name = "blockForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Block Universe"
        CType(Me.numTimeTick, System.ComponentModel.ISupportInitialize).EndInit()
        Me.boxParam.ResumeLayout(False)
        Me.boxParam.PerformLayout()
        CType(Me.numParamZPos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numParamZVel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numParamMass, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numParamYVel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numParamXVel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.boxTimeParam.ResumeLayout(False)
        Me.boxTimeParam.PerformLayout()
        Me.boxMode.ResumeLayout(False)
        Me.boxMode.PerformLayout()
        Me.boxGeneral.ResumeLayout(False)
        Me.boxGeneral.PerformLayout()
        CType(Me.numTraj, System.ComponentModel.ISupportInitialize).EndInit()
        Me.boxCollision.ResumeLayout(False)
        Me.boxCollision.PerformLayout()
        Me.boxCoords.ResumeLayout(False)
        Me.boxCoords.PerformLayout()
        Me.boxObjectInfo.ResumeLayout(False)
        Me.pnlActionLeft.ResumeLayout(False)
        Me.boxSelParam.ResumeLayout(False)
        Me.boxSelParam.PerformLayout()
        Me.boxTraj.ResumeLayout(False)
        Me.boxTraj.PerformLayout()
        Me.pnlActionRight.ResumeLayout(False)
        Me.boxMinimap.ResumeLayout(False)
        Me.pnlCosmos.ResumeLayout(False)
        CType(Me.picMinimap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picUniverse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCoordsMouse As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lblTimeTick As Label
    Private WithEvents numTimeTick As NumericUpDown
    Friend WithEvents planetslbl As Label
    Friend WithEvents chkCanvasClear As CheckBox
    Friend WithEvents btnReset As Button
    Friend WithEvents boxTimeParam As GroupBox
    Friend WithEvents boxMode As GroupBox
    Friend WithEvents boxParam As GroupBox
    Friend WithEvents lblParamYVel As Label
    Friend WithEvents numParamYVel As NumericUpDown
    Friend WithEvents lblParamXVel As Label
    Friend WithEvents numParamXVel As NumericUpDown
    Friend WithEvents boxGeneral As GroupBox
    Friend WithEvents boxCollision As GroupBox
    Friend WithEvents radCollisionBounce As RadioButton
    Friend WithEvents radCollisionTunnel As RadioButton
    Friend WithEvents chkDrawTrajectories As CheckBox
    Private WithEvents numTraj As NumericUpDown
    Friend WithEvents trajTooltip As ToolTip
    Friend WithEvents radCollisionNone As RadioButton
    Friend WithEvents boxCoords As GroupBox
    Friend WithEvents lblCoordsAbs As Label
    Friend WithEvents boxObjectInfo As GroupBox
    Friend WithEvents btnPauseUniverse As Button
    Friend WithEvents pnlActionLeft As FlowLayoutPanel
    Friend WithEvents picUniverse As PictureBox
    Friend WithEvents lblStatusMessage As Label
    Friend WithEvents chkFollowSelected As CheckBox
    Friend WithEvents objectListView As ListView
    Friend WithEvents NameHeader As ColumnHeader
    Friend WithEvents LocationXHeader As ColumnHeader
    Friend WithEvents AccelerationHeader As ColumnHeader
    Friend WithEvents VelocityHeader As ColumnHeader
    Friend WithEvents MassHeader As ColumnHeader
    Friend WithEvents SizeHeader As ColumnHeader
    Friend WithEvents LocationYHeader As ColumnHeader
    Friend WithEvents AccelerationXHeader As ColumnHeader
    Friend WithEvents VelocityXHeader As ColumnHeader
    Friend WithEvents AccelerationYHeader As ColumnHeader
    Friend WithEvents VelocityYHeader As ColumnHeader
    Friend WithEvents boxSelParam As GroupBox
    Friend WithEvents boxTraj As GroupBox
    Friend WithEvents radRelativeTraj As RadioButton
    Friend WithEvents radRealTraj As RadioButton
    Friend WithEvents radBothTraj As RadioButton
    Friend WithEvents chkHideInfo As CheckBox
    Friend WithEvents lblParamMass As Label
    Friend WithEvents numParamMass As NumericUpDown
    Friend WithEvents chkRealisticSim As CheckBox
    Friend WithEvents btnLoad As Button
    Friend WithEvents btnSave As Button
    Friend WithEvents openFileDialog As OpenFileDialog
    Friend WithEvents lblParamZVel As Label
    Friend WithEvents numParamZVel As NumericUpDown
    Friend WithEvents lblParamZPos As Label
    Friend WithEvents numParamZPos As NumericUpDown
    Friend WithEvents LocationZHeader As ColumnHeader
    Friend WithEvents AccelerationZHeader As ColumnHeader
    Friend WithEvents VelocityZHeader As ColumnHeader
    Friend WithEvents imgListActionBtn As ImageList
    Friend WithEvents pnlActionRight As FlowLayoutPanel
    Friend WithEvents btnSelectTool As Button
    Friend WithEvents boxMinimap As GroupBox
    Friend WithEvents picMinimap As PictureBox
    Friend WithEvents pnlCosmos As FlowLayoutPanel
    Friend WithEvents btnSection1 As Button
    Friend WithEvents btnSection2 As Button
    Friend WithEvents btnSection3 As Button
    Friend WithEvents btnSection4 As Button
    Friend WithEvents btnSection5 As Button
    Friend WithEvents btnSection6 As Button
    Friend WithEvents btnSection7 As Button
    Friend WithEvents btnSection8 As Button
    Friend WithEvents btnSection9 As Button
    Friend WithEvents lblCosmosTitle As Label
    Friend WithEvents btnNavReturn As Button
    Friend WithEvents imgListNavBtn As ImageList
    Friend WithEvents btnZoomTool As Button
    Friend WithEvents btnZoomAreaTool As Button
    Friend WithEvents cbTickSpeed As ComboBox
    Friend WithEvents cbCreateType As ComboBox
    Friend WithEvents chkbNoCreation As CheckBox
End Class
