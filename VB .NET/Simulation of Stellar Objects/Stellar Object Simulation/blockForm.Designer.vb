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
        Dim ListViewGroup5 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Stars", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup6 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Planets", System.Windows.Forms.HorizontalAlignment.Left)
        Me.lblCoordsMouse = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblTimeTick = New System.Windows.Forms.Label()
        Me.numTimeTick = New System.Windows.Forms.NumericUpDown()
        Me.planetslbl = New System.Windows.Forms.Label()
        Me.chkCanvasClear = New System.Windows.Forms.CheckBox()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.boxParam = New System.Windows.Forms.GroupBox()
        Me.lblParamYVel = New System.Windows.Forms.Label()
        Me.numParamYVel = New System.Windows.Forms.NumericUpDown()
        Me.lblParamXVel = New System.Windows.Forms.Label()
        Me.numParamXVel = New System.Windows.Forms.NumericUpDown()
        Me.boxTimeParam = New System.Windows.Forms.GroupBox()
        Me.boxMode = New System.Windows.Forms.GroupBox()
        Me.radModeNothing = New System.Windows.Forms.RadioButton()
        Me.radModeStar = New System.Windows.Forms.RadioButton()
        Me.radModePlanet = New System.Windows.Forms.RadioButton()
        Me.boxGeneral = New System.Windows.Forms.GroupBox()
        Me.numTraj = New System.Windows.Forms.NumericUpDown()
        Me.chkDrawTrajectories = New System.Windows.Forms.CheckBox()
        Me.chkFollowSelected = New System.Windows.Forms.CheckBox()
        Me.boxCollision = New System.Windows.Forms.GroupBox()
        Me.radCollisionNone = New System.Windows.Forms.RadioButton()
        Me.radCollisionBounce = New System.Windows.Forms.RadioButton()
        Me.radCollisionTunnel = New System.Windows.Forms.RadioButton()
        Me.trajTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.boxCoords = New System.Windows.Forms.GroupBox()
        Me.lblCoordsAbs = New System.Windows.Forms.Label()
        Me.boxObjectInfo = New System.Windows.Forms.GroupBox()
        Me.objectListView = New System.Windows.Forms.ListView()
        Me.NameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LocationXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LocationYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MassHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SizeHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnPauseUniverse = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.dummyUniverse = New System.Windows.Forms.PictureBox()
        Me.lblStatusMessage = New System.Windows.Forms.Label()
        Me.boxSelParam = New System.Windows.Forms.GroupBox()
        Me.chkHideInfo = New System.Windows.Forms.CheckBox()
        Me.boxTraj = New System.Windows.Forms.GroupBox()
        Me.radBothTraj = New System.Windows.Forms.RadioButton()
        Me.radRelativeTraj = New System.Windows.Forms.RadioButton()
        Me.radRealTraj = New System.Windows.Forms.RadioButton()
        CType(Me.numTimeTick, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxParam.SuspendLayout()
        CType(Me.numParamYVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numParamXVel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxTimeParam.SuspendLayout()
        Me.boxMode.SuspendLayout()
        Me.boxGeneral.SuspendLayout()
        CType(Me.numTraj, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxCollision.SuspendLayout()
        Me.boxCoords.SuspendLayout()
        Me.boxObjectInfo.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.dummyUniverse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxSelParam.SuspendLayout()
        Me.boxTraj.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCoordsMouse
        '
        Me.lblCoordsMouse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCoordsMouse.AutoSize = True
        Me.lblCoordsMouse.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblCoordsMouse.Location = New System.Drawing.Point(15, 23)
        Me.lblCoordsMouse.Name = "lblCoordsMouse"
        Me.lblCoordsMouse.Size = New System.Drawing.Size(0, 23)
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
        Me.lblTimeTick.Location = New System.Drawing.Point(6, 30)
        Me.lblTimeTick.Name = "lblTimeTick"
        Me.lblTimeTick.Size = New System.Drawing.Size(45, 18)
        Me.lblTimeTick.TabIndex = 8
        Me.lblTimeTick.Text = "Tick  ="
        '
        'numTimeTick
        '
        Me.numTimeTick.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numTimeTick.Location = New System.Drawing.Point(56, 28)
        Me.numTimeTick.Maximum = New Decimal(New Integer() {1500, 0, 0, 0})
        Me.numTimeTick.Name = "numTimeTick"
        Me.numTimeTick.Size = New System.Drawing.Size(166, 26)
        Me.numTimeTick.TabIndex = 7
        Me.numTimeTick.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numTimeTick.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'planetslbl
        '
        Me.planetslbl.AutoSize = True
        Me.planetslbl.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.planetslbl.Location = New System.Drawing.Point(1107, 530)
        Me.planetslbl.Name = "planetslbl"
        Me.planetslbl.Size = New System.Drawing.Size(0, 18)
        Me.planetslbl.TabIndex = 10
        '
        'chkCanvasClear
        '
        Me.chkCanvasClear.AutoSize = True
        Me.chkCanvasClear.Checked = True
        Me.chkCanvasClear.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkCanvasClear.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkCanvasClear.Location = New System.Drawing.Point(9, 25)
        Me.chkCanvasClear.Name = "chkCanvasClear"
        Me.chkCanvasClear.Size = New System.Drawing.Size(103, 22)
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
        Me.btnReset.Location = New System.Drawing.Point(86, 8)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(120, 31)
        Me.btnReset.TabIndex = 12
        Me.btnReset.Text = "Reset"
        Me.btnReset.UseVisualStyleBackColor = False
        '
        'boxParam
        '
        Me.boxParam.Controls.Add(Me.lblParamYVel)
        Me.boxParam.Controls.Add(Me.numParamYVel)
        Me.boxParam.Controls.Add(Me.lblParamXVel)
        Me.boxParam.Controls.Add(Me.numParamXVel)
        Me.boxParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxParam.Location = New System.Drawing.Point(1110, 50)
        Me.boxParam.Name = "boxParam"
        Me.boxParam.Size = New System.Drawing.Size(228, 92)
        Me.boxParam.TabIndex = 14
        Me.boxParam.TabStop = False
        Me.boxParam.Text = "Creation Parameters"
        '
        'lblParamYVel
        '
        Me.lblParamYVel.AutoSize = True
        Me.lblParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblParamYVel.Location = New System.Drawing.Point(6, 62)
        Me.lblParamYVel.Name = "lblParamYVel"
        Me.lblParamYVel.Size = New System.Drawing.Size(81, 18)
        Me.lblParamYVel.TabIndex = 10
        Me.lblParamYVel.Text = "Y Velocity  ="
        '
        'numParamYVel
        '
        Me.numParamYVel.DecimalPlaces = 2
        Me.numParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numParamYVel.Location = New System.Drawing.Point(93, 60)
        Me.numParamYVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numParamYVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numParamYVel.Name = "numParamYVel"
        Me.numParamYVel.Size = New System.Drawing.Size(129, 26)
        Me.numParamYVel.TabIndex = 9
        Me.numParamYVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblParamXVel
        '
        Me.lblParamXVel.AutoSize = True
        Me.lblParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblParamXVel.Location = New System.Drawing.Point(6, 30)
        Me.lblParamXVel.Name = "lblParamXVel"
        Me.lblParamXVel.Size = New System.Drawing.Size(82, 18)
        Me.lblParamXVel.TabIndex = 5
        Me.lblParamXVel.Text = "X Velocity  ="
        '
        'numParamXVel
        '
        Me.numParamXVel.DecimalPlaces = 2
        Me.numParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numParamXVel.Location = New System.Drawing.Point(93, 28)
        Me.numParamXVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numParamXVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numParamXVel.Name = "numParamXVel"
        Me.numParamXVel.Size = New System.Drawing.Size(129, 26)
        Me.numParamXVel.TabIndex = 4
        Me.numParamXVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'boxTimeParam
        '
        Me.boxTimeParam.Controls.Add(Me.numTimeTick)
        Me.boxTimeParam.Controls.Add(Me.lblTimeTick)
        Me.boxTimeParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxTimeParam.Location = New System.Drawing.Point(1110, 420)
        Me.boxTimeParam.Name = "boxTimeParam"
        Me.boxTimeParam.Size = New System.Drawing.Size(228, 81)
        Me.boxTimeParam.TabIndex = 14
        Me.boxTimeParam.TabStop = False
        Me.boxTimeParam.Text = "Universe Tick"
        '
        'boxMode
        '
        Me.boxMode.Controls.Add(Me.radModeNothing)
        Me.boxMode.Controls.Add(Me.radModeStar)
        Me.boxMode.Controls.Add(Me.radModePlanet)
        Me.boxMode.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxMode.Location = New System.Drawing.Point(1110, 148)
        Me.boxMode.Name = "boxMode"
        Me.boxMode.Size = New System.Drawing.Size(228, 56)
        Me.boxMode.TabIndex = 15
        Me.boxMode.TabStop = False
        Me.boxMode.Text = "Creation Mode"
        '
        'radModeNothing
        '
        Me.radModeNothing.AutoSize = True
        Me.radModeNothing.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radModeNothing.Location = New System.Drawing.Point(161, 25)
        Me.radModeNothing.Name = "radModeNothing"
        Me.radModeNothing.Size = New System.Drawing.Size(60, 22)
        Me.radModeNothing.TabIndex = 21
        Me.radModeNothing.Text = "None"
        Me.radModeNothing.UseVisualStyleBackColor = True
        '
        'radModeStar
        '
        Me.radModeStar.AutoSize = True
        Me.radModeStar.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radModeStar.Location = New System.Drawing.Point(93, 25)
        Me.radModeStar.Name = "radModeStar"
        Me.radModeStar.Size = New System.Drawing.Size(50, 22)
        Me.radModeStar.TabIndex = 17
        Me.radModeStar.Text = "Star"
        Me.radModeStar.UseVisualStyleBackColor = True
        '
        'radModePlanet
        '
        Me.radModePlanet.AutoSize = True
        Me.radModePlanet.Checked = True
        Me.radModePlanet.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radModePlanet.Location = New System.Drawing.Point(9, 25)
        Me.radModePlanet.Name = "radModePlanet"
        Me.radModePlanet.Size = New System.Drawing.Size(66, 22)
        Me.radModePlanet.TabIndex = 16
        Me.radModePlanet.TabStop = True
        Me.radModePlanet.Text = "Planet"
        Me.trajTooltip.SetToolTip(Me.radModePlanet, "Hold CTRL for continuous mode.")
        Me.radModePlanet.UseVisualStyleBackColor = True
        '
        'boxGeneral
        '
        Me.boxGeneral.Controls.Add(Me.numTraj)
        Me.boxGeneral.Controls.Add(Me.chkDrawTrajectories)
        Me.boxGeneral.Controls.Add(Me.chkCanvasClear)
        Me.boxGeneral.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxGeneral.Location = New System.Drawing.Point(1110, 507)
        Me.boxGeneral.Name = "boxGeneral"
        Me.boxGeneral.Size = New System.Drawing.Size(228, 219)
        Me.boxGeneral.TabIndex = 16
        Me.boxGeneral.TabStop = False
        Me.boxGeneral.Text = "Generic Parameters"
        '
        'numTraj
        '
        Me.numTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numTraj.Location = New System.Drawing.Point(148, 52)
        Me.numTraj.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.numTraj.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.numTraj.Name = "numTraj"
        Me.numTraj.Size = New System.Drawing.Size(74, 26)
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
        Me.chkDrawTrajectories.Location = New System.Drawing.Point(9, 53)
        Me.chkDrawTrajectories.Name = "chkDrawTrajectories"
        Me.chkDrawTrajectories.Size = New System.Drawing.Size(99, 22)
        Me.chkDrawTrajectories.TabIndex = 12
        Me.chkDrawTrajectories.Text = "Trajectories"
        Me.chkDrawTrajectories.UseVisualStyleBackColor = True
        '
        'chkFollowSelected
        '
        Me.chkFollowSelected.AutoSize = True
        Me.chkFollowSelected.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkFollowSelected.Location = New System.Drawing.Point(9, 25)
        Me.chkFollowSelected.Name = "chkFollowSelected"
        Me.chkFollowSelected.Size = New System.Drawing.Size(69, 22)
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
        Me.boxCollision.Location = New System.Drawing.Point(1110, 210)
        Me.boxCollision.Name = "boxCollision"
        Me.boxCollision.Size = New System.Drawing.Size(228, 56)
        Me.boxCollision.TabIndex = 17
        Me.boxCollision.TabStop = False
        Me.boxCollision.Text = "Collision"
        '
        'radCollisionNone
        '
        Me.radCollisionNone.AutoSize = True
        Me.radCollisionNone.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radCollisionNone.Location = New System.Drawing.Point(161, 25)
        Me.radCollisionNone.Name = "radCollisionNone"
        Me.radCollisionNone.Size = New System.Drawing.Size(60, 22)
        Me.radCollisionNone.TabIndex = 20
        Me.radCollisionNone.Text = "None"
        Me.radCollisionNone.UseVisualStyleBackColor = True
        '
        'radCollisionBounce
        '
        Me.radCollisionBounce.AutoSize = True
        Me.radCollisionBounce.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radCollisionBounce.Location = New System.Drawing.Point(82, 25)
        Me.radCollisionBounce.Name = "radCollisionBounce"
        Me.radCollisionBounce.Size = New System.Drawing.Size(72, 22)
        Me.radCollisionBounce.TabIndex = 19
        Me.radCollisionBounce.Text = "Bounce"
        Me.radCollisionBounce.UseVisualStyleBackColor = True
        '
        'radCollisionTunnel
        '
        Me.radCollisionTunnel.AutoSize = True
        Me.radCollisionTunnel.Checked = True
        Me.radCollisionTunnel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radCollisionTunnel.Location = New System.Drawing.Point(9, 25)
        Me.radCollisionTunnel.Name = "radCollisionTunnel"
        Me.radCollisionTunnel.Size = New System.Drawing.Size(68, 22)
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
        'boxCoords
        '
        Me.boxCoords.Controls.Add(Me.lblCoordsAbs)
        Me.boxCoords.Controls.Add(Me.lblCoordsMouse)
        Me.boxCoords.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxCoords.Location = New System.Drawing.Point(865, 507)
        Me.boxCoords.Name = "boxCoords"
        Me.boxCoords.Size = New System.Drawing.Size(236, 219)
        Me.boxCoords.TabIndex = 23
        Me.boxCoords.TabStop = False
        Me.boxCoords.Text = "Coordinates (Real/Absolute)"
        '
        'lblCoordsAbs
        '
        Me.lblCoordsAbs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCoordsAbs.AutoSize = True
        Me.lblCoordsAbs.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblCoordsAbs.Location = New System.Drawing.Point(15, 52)
        Me.lblCoordsAbs.Name = "lblCoordsAbs"
        Me.lblCoordsAbs.Size = New System.Drawing.Size(0, 23)
        Me.lblCoordsAbs.TabIndex = 1
        '
        'boxObjectInfo
        '
        Me.boxObjectInfo.Controls.Add(Me.objectListView)
        Me.boxObjectInfo.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxObjectInfo.Location = New System.Drawing.Point(50, 507)
        Me.boxObjectInfo.Name = "boxObjectInfo"
        Me.boxObjectInfo.Size = New System.Drawing.Size(809, 219)
        Me.boxObjectInfo.TabIndex = 36
        Me.boxObjectInfo.TabStop = False
        Me.boxObjectInfo.Text = "Object List"
        '
        'objectListView
        '
        Me.objectListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NameHeader, Me.LocationXHeader, Me.LocationYHeader, Me.AccelerationHeader, Me.VelocityHeader, Me.AccelerationXHeader, Me.VelocityXHeader, Me.AccelerationYHeader, Me.VelocityYHeader, Me.MassHeader, Me.SizeHeader})
        Me.objectListView.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ListViewGroup5.Header = "Stars"
        ListViewGroup5.Name = "Stars"
        ListViewGroup6.Header = "Planets"
        ListViewGroup6.Name = "Planets"
        ListViewGroup6.Tag = ""
        Me.objectListView.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup5, ListViewGroup6})
        Me.objectListView.Location = New System.Drawing.Point(6, 22)
        Me.objectListView.Name = "objectListView"
        Me.objectListView.Size = New System.Drawing.Size(797, 191)
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
        Me.LocationXHeader.Width = 123
        '
        'LocationYHeader
        '
        Me.LocationYHeader.Text = "Location Y"
        Me.LocationYHeader.Width = 108
        '
        'AccelerationHeader
        '
        Me.AccelerationHeader.Text = "Acceleration"
        Me.AccelerationHeader.Width = 123
        '
        'VelocityHeader
        '
        Me.VelocityHeader.Text = "Velocity"
        Me.VelocityHeader.Width = 113
        '
        'AccelerationXHeader
        '
        Me.AccelerationXHeader.Text = "Acceleration X"
        Me.AccelerationXHeader.Width = 131
        '
        'VelocityXHeader
        '
        Me.VelocityXHeader.Text = "Velocity X"
        Me.VelocityXHeader.Width = 117
        '
        'AccelerationYHeader
        '
        Me.AccelerationYHeader.Text = "Acceleration Y"
        Me.AccelerationYHeader.Width = 124
        '
        'VelocityYHeader
        '
        Me.VelocityYHeader.Text = "Velocity Y"
        Me.VelocityYHeader.Width = 141
        '
        'MassHeader
        '
        Me.MassHeader.Text = "Mass"
        Me.MassHeader.Width = 105
        '
        'SizeHeader
        '
        Me.SizeHeader.Text = "Size"
        Me.SizeHeader.Width = 112
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
        Me.btnPauseUniverse.Location = New System.Drawing.Point(8, 8)
        Me.btnPauseUniverse.Name = "btnPauseUniverse"
        Me.btnPauseUniverse.Size = New System.Drawing.Size(72, 31)
        Me.btnPauseUniverse.TabIndex = 38
        Me.btnPauseUniverse.Text = "Pause"
        Me.btnPauseUniverse.UseVisualStyleBackColor = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(113, Byte), Integer))
        Me.FlowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FlowLayoutPanel1.Controls.Add(Me.btnPauseUniverse)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnReset)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(50, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Padding = New System.Windows.Forms.Padding(5, 5, 0, 0)
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(1051, 49)
        Me.FlowLayoutPanel1.TabIndex = 39
        '
        'dummyUniverse
        '
        Me.dummyUniverse.BackColor = System.Drawing.Color.Black
        Me.dummyUniverse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.dummyUniverse.Location = New System.Drawing.Point(50, 50)
        Me.dummyUniverse.Name = "dummyUniverse"
        Me.dummyUniverse.Size = New System.Drawing.Size(1051, 451)
        Me.dummyUniverse.TabIndex = 40
        Me.dummyUniverse.TabStop = False
        '
        'lblStatusMessage
        '
        Me.lblStatusMessage.AutoSize = True
        Me.lblStatusMessage.BackColor = System.Drawing.Color.Transparent
        Me.lblStatusMessage.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.lblStatusMessage.ForeColor = System.Drawing.Color.White
        Me.lblStatusMessage.Location = New System.Drawing.Point(58, 56)
        Me.lblStatusMessage.Name = "lblStatusMessage"
        Me.lblStatusMessage.Size = New System.Drawing.Size(0, 18)
        Me.lblStatusMessage.TabIndex = 41
        '
        'boxSelParam
        '
        Me.boxSelParam.Controls.Add(Me.chkHideInfo)
        Me.boxSelParam.Controls.Add(Me.chkFollowSelected)
        Me.boxSelParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxSelParam.Location = New System.Drawing.Point(1110, 334)
        Me.boxSelParam.Name = "boxSelParam"
        Me.boxSelParam.Size = New System.Drawing.Size(228, 80)
        Me.boxSelParam.TabIndex = 15
        Me.boxSelParam.TabStop = False
        Me.boxSelParam.Text = "Selection Parameters"
        '
        'chkHideInfo
        '
        Me.chkHideInfo.AutoSize = True
        Me.chkHideInfo.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.chkHideInfo.Location = New System.Drawing.Point(9, 53)
        Me.chkHideInfo.Name = "chkHideInfo"
        Me.chkHideInfo.Size = New System.Drawing.Size(100, 22)
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
        Me.boxTraj.Location = New System.Drawing.Point(1110, 272)
        Me.boxTraj.Name = "boxTraj"
        Me.boxTraj.Size = New System.Drawing.Size(228, 56)
        Me.boxTraj.TabIndex = 21
        Me.boxTraj.TabStop = False
        Me.boxTraj.Text = "Trajectory / Orbit Parameters"
        '
        'radBothTraj
        '
        Me.radBothTraj.AutoSize = True
        Me.radBothTraj.Enabled = False
        Me.radBothTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radBothTraj.Location = New System.Drawing.Point(161, 25)
        Me.radBothTraj.Name = "radBothTraj"
        Me.radBothTraj.Size = New System.Drawing.Size(55, 22)
        Me.radBothTraj.TabIndex = 20
        Me.radBothTraj.Text = "Both"
        Me.radBothTraj.UseVisualStyleBackColor = True
        '
        'radRelativeTraj
        '
        Me.radRelativeTraj.AutoSize = True
        Me.radRelativeTraj.Enabled = False
        Me.radRelativeTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radRelativeTraj.Location = New System.Drawing.Point(80, 25)
        Me.radRelativeTraj.Name = "radRelativeTraj"
        Me.radRelativeTraj.Size = New System.Drawing.Size(76, 22)
        Me.radRelativeTraj.TabIndex = 19
        Me.radRelativeTraj.Text = "Relative"
        Me.radRelativeTraj.UseVisualStyleBackColor = True
        '
        'radRealTraj
        '
        Me.radRealTraj.AutoSize = True
        Me.radRealTraj.Checked = True
        Me.radRealTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radRealTraj.Location = New System.Drawing.Point(9, 25)
        Me.radRealTraj.Name = "radRealTraj"
        Me.radRealTraj.Size = New System.Drawing.Size(53, 22)
        Me.radRealTraj.TabIndex = 18
        Me.radRealTraj.TabStop = True
        Me.radRealTraj.Text = "Real"
        Me.radRealTraj.UseVisualStyleBackColor = True
        '
        'blockForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1347, 733)
        Me.Controls.Add(Me.boxTraj)
        Me.Controls.Add(Me.boxSelParam)
        Me.Controls.Add(Me.lblStatusMessage)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.boxObjectInfo)
        Me.Controls.Add(Me.boxCoords)
        Me.Controls.Add(Me.boxCollision)
        Me.Controls.Add(Me.boxGeneral)
        Me.Controls.Add(Me.boxMode)
        Me.Controls.Add(Me.boxTimeParam)
        Me.Controls.Add(Me.planetslbl)
        Me.Controls.Add(Me.boxParam)
        Me.Controls.Add(Me.dummyUniverse)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1363, 642)
        Me.Name = "blockForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Block Universe"
        CType(Me.numTimeTick, System.ComponentModel.ISupportInitialize).EndInit()
        Me.boxParam.ResumeLayout(False)
        Me.boxParam.PerformLayout()
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
        Me.FlowLayoutPanel1.ResumeLayout(False)
        CType(Me.dummyUniverse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.boxSelParam.ResumeLayout(False)
        Me.boxSelParam.PerformLayout()
        Me.boxTraj.ResumeLayout(False)
        Me.boxTraj.PerformLayout()
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
    Friend WithEvents radModeStar As RadioButton
    Friend WithEvents radModePlanet As RadioButton
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
    Friend WithEvents radModeNothing As RadioButton
    Friend WithEvents btnPauseUniverse As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents dummyUniverse As PictureBox
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
End Class
