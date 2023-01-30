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
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Stars", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup4 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Planets", System.Windows.Forms.HorizontalAlignment.Left)
        Me.lblCoordsMouse = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.numPlanetParamXVel = New System.Windows.Forms.NumericUpDown()
        Me.lblPlanetParamXVel = New System.Windows.Forms.Label()
        Me.lblTimeTick = New System.Windows.Forms.Label()
        Me.numTimeTick = New System.Windows.Forms.NumericUpDown()
        Me.planetslbl = New System.Windows.Forms.Label()
        Me.canvasClear = New System.Windows.Forms.CheckBox()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.boxPlanetParam = New System.Windows.Forms.GroupBox()
        Me.lblPlanetParamYVel = New System.Windows.Forms.Label()
        Me.numPlanetParamYVel = New System.Windows.Forms.NumericUpDown()
        Me.boxStarParam = New System.Windows.Forms.GroupBox()
        Me.lblStarParamYVel = New System.Windows.Forms.Label()
        Me.numStarParamYVel = New System.Windows.Forms.NumericUpDown()
        Me.lblStarParamXVel = New System.Windows.Forms.Label()
        Me.numStarParamXVel = New System.Windows.Forms.NumericUpDown()
        Me.boxTimeParam = New System.Windows.Forms.GroupBox()
        Me.boxMode = New System.Windows.Forms.GroupBox()
        Me.modeNothing = New System.Windows.Forms.RadioButton()
        Me.modeStar = New System.Windows.Forms.RadioButton()
        Me.modePlanet = New System.Windows.Forms.RadioButton()
        Me.boxGeneral = New System.Windows.Forms.GroupBox()
        Me.followSelected = New System.Windows.Forms.CheckBox()
        Me.numTraj = New System.Windows.Forms.NumericUpDown()
        Me.generalTraj = New System.Windows.Forms.CheckBox()
        Me.boxCollision = New System.Windows.Forms.GroupBox()
        Me.collisionNone = New System.Windows.Forms.RadioButton()
        Me.collisionBounce = New System.Windows.Forms.RadioButton()
        Me.collisionTunnel = New System.Windows.Forms.RadioButton()
        Me.trajTooltip = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.boxCoords = New System.Windows.Forms.GroupBox()
        Me.lblCoordsAbs = New System.Windows.Forms.Label()
        Me.boxObjectInfo = New System.Windows.Forms.GroupBox()
        Me.btnPauseUniverse = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.dummyUniverse = New System.Windows.Forms.PictureBox()
        Me.lblStatusMessage = New System.Windows.Forms.Label()
        Me.objectListView = New System.Windows.Forms.ListView()
        Me.NameHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LocationXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.MassHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SizeHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LocationYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AccelerationYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityXHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.VelocityYHeader = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        CType(Me.numPlanetParamXVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTimeTick, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxPlanetParam.SuspendLayout()
        CType(Me.numPlanetParamYVel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxStarParam.SuspendLayout()
        CType(Me.numStarParamYVel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numStarParamXVel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxTimeParam.SuspendLayout()
        Me.boxMode.SuspendLayout()
        Me.boxGeneral.SuspendLayout()
        CType(Me.numTraj, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.boxCollision.SuspendLayout()
        Me.boxCoords.SuspendLayout()
        Me.boxObjectInfo.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.dummyUniverse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblCoordsMouse
        '
        Me.lblCoordsMouse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCoordsMouse.AutoSize = True
        Me.lblCoordsMouse.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblCoordsMouse.Location = New System.Drawing.Point(24, 22)
        Me.lblCoordsMouse.Name = "lblCoordsMouse"
        Me.lblCoordsMouse.Size = New System.Drawing.Size(0, 23)
        Me.lblCoordsMouse.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'numPlanetParamXVel
        '
        Me.numPlanetParamXVel.DecimalPlaces = 2
        Me.numPlanetParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numPlanetParamXVel.Location = New System.Drawing.Point(93, 28)
        Me.numPlanetParamXVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numPlanetParamXVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numPlanetParamXVel.Name = "numPlanetParamXVel"
        Me.numPlanetParamXVel.Size = New System.Drawing.Size(129, 26)
        Me.numPlanetParamXVel.TabIndex = 4
        Me.numPlanetParamXVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblPlanetParamXVel
        '
        Me.lblPlanetParamXVel.AutoSize = True
        Me.lblPlanetParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblPlanetParamXVel.Location = New System.Drawing.Point(5, 32)
        Me.lblPlanetParamXVel.Name = "lblPlanetParamXVel"
        Me.lblPlanetParamXVel.Size = New System.Drawing.Size(82, 18)
        Me.lblPlanetParamXVel.TabIndex = 5
        Me.lblPlanetParamXVel.Text = "X Velocity  ="
        '
        'lblTimeTick
        '
        Me.lblTimeTick.AutoSize = True
        Me.lblTimeTick.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblTimeTick.Location = New System.Drawing.Point(5, 32)
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
        'canvasClear
        '
        Me.canvasClear.AutoSize = True
        Me.canvasClear.Checked = True
        Me.canvasClear.CheckState = System.Windows.Forms.CheckState.Checked
        Me.canvasClear.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.canvasClear.Location = New System.Drawing.Point(6, 26)
        Me.canvasClear.Name = "canvasClear"
        Me.canvasClear.Size = New System.Drawing.Size(103, 22)
        Me.canvasClear.TabIndex = 11
        Me.canvasClear.Text = "Clear canvas"
        Me.canvasClear.UseVisualStyleBackColor = True
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
        'boxPlanetParam
        '
        Me.boxPlanetParam.Controls.Add(Me.lblPlanetParamYVel)
        Me.boxPlanetParam.Controls.Add(Me.numPlanetParamYVel)
        Me.boxPlanetParam.Controls.Add(Me.lblPlanetParamXVel)
        Me.boxPlanetParam.Controls.Add(Me.numPlanetParamXVel)
        Me.boxPlanetParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxPlanetParam.Location = New System.Drawing.Point(1110, 50)
        Me.boxPlanetParam.Name = "boxPlanetParam"
        Me.boxPlanetParam.Size = New System.Drawing.Size(228, 92)
        Me.boxPlanetParam.TabIndex = 13
        Me.boxPlanetParam.TabStop = False
        Me.boxPlanetParam.Text = "Planet Creation Parameters"
        '
        'lblPlanetParamYVel
        '
        Me.lblPlanetParamYVel.AutoSize = True
        Me.lblPlanetParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblPlanetParamYVel.Location = New System.Drawing.Point(6, 64)
        Me.lblPlanetParamYVel.Name = "lblPlanetParamYVel"
        Me.lblPlanetParamYVel.Size = New System.Drawing.Size(81, 18)
        Me.lblPlanetParamYVel.TabIndex = 10
        Me.lblPlanetParamYVel.Text = "Y Velocity  ="
        '
        'numPlanetParamYVel
        '
        Me.numPlanetParamYVel.DecimalPlaces = 2
        Me.numPlanetParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numPlanetParamYVel.Location = New System.Drawing.Point(93, 60)
        Me.numPlanetParamYVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numPlanetParamYVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numPlanetParamYVel.Name = "numPlanetParamYVel"
        Me.numPlanetParamYVel.Size = New System.Drawing.Size(129, 26)
        Me.numPlanetParamYVel.TabIndex = 9
        Me.numPlanetParamYVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'boxStarParam
        '
        Me.boxStarParam.Controls.Add(Me.lblStarParamYVel)
        Me.boxStarParam.Controls.Add(Me.numStarParamYVel)
        Me.boxStarParam.Controls.Add(Me.lblStarParamXVel)
        Me.boxStarParam.Controls.Add(Me.numStarParamXVel)
        Me.boxStarParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxStarParam.Location = New System.Drawing.Point(1110, 50)
        Me.boxStarParam.Name = "boxStarParam"
        Me.boxStarParam.Size = New System.Drawing.Size(228, 92)
        Me.boxStarParam.TabIndex = 14
        Me.boxStarParam.TabStop = False
        Me.boxStarParam.Text = "Star Creation Parameters"
        Me.boxStarParam.Visible = False
        '
        'lblStarParamYVel
        '
        Me.lblStarParamYVel.AutoSize = True
        Me.lblStarParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblStarParamYVel.Location = New System.Drawing.Point(6, 64)
        Me.lblStarParamYVel.Name = "lblStarParamYVel"
        Me.lblStarParamYVel.Size = New System.Drawing.Size(81, 18)
        Me.lblStarParamYVel.TabIndex = 10
        Me.lblStarParamYVel.Text = "Y Velocity  ="
        '
        'numStarParamYVel
        '
        Me.numStarParamYVel.DecimalPlaces = 2
        Me.numStarParamYVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numStarParamYVel.Location = New System.Drawing.Point(93, 60)
        Me.numStarParamYVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numStarParamYVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numStarParamYVel.Name = "numStarParamYVel"
        Me.numStarParamYVel.Size = New System.Drawing.Size(129, 26)
        Me.numStarParamYVel.TabIndex = 9
        Me.numStarParamYVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblStarParamXVel
        '
        Me.lblStarParamXVel.AutoSize = True
        Me.lblStarParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblStarParamXVel.Location = New System.Drawing.Point(5, 32)
        Me.lblStarParamXVel.Name = "lblStarParamXVel"
        Me.lblStarParamXVel.Size = New System.Drawing.Size(82, 18)
        Me.lblStarParamXVel.TabIndex = 5
        Me.lblStarParamXVel.Text = "X Velocity  ="
        '
        'numStarParamXVel
        '
        Me.numStarParamXVel.DecimalPlaces = 2
        Me.numStarParamXVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numStarParamXVel.Location = New System.Drawing.Point(93, 28)
        Me.numStarParamXVel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.numStarParamXVel.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.numStarParamXVel.Name = "numStarParamXVel"
        Me.numStarParamXVel.Size = New System.Drawing.Size(129, 26)
        Me.numStarParamXVel.TabIndex = 4
        Me.numStarParamXVel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'boxTimeParam
        '
        Me.boxTimeParam.Controls.Add(Me.numTimeTick)
        Me.boxTimeParam.Controls.Add(Me.lblTimeTick)
        Me.boxTimeParam.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxTimeParam.Location = New System.Drawing.Point(1110, 380)
        Me.boxTimeParam.Name = "boxTimeParam"
        Me.boxTimeParam.Size = New System.Drawing.Size(228, 64)
        Me.boxTimeParam.TabIndex = 14
        Me.boxTimeParam.TabStop = False
        Me.boxTimeParam.Text = "Time Parameters"
        '
        'boxMode
        '
        Me.boxMode.Controls.Add(Me.modeNothing)
        Me.boxMode.Controls.Add(Me.modeStar)
        Me.boxMode.Controls.Add(Me.modePlanet)
        Me.boxMode.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxMode.Location = New System.Drawing.Point(1118, 148)
        Me.boxMode.Name = "boxMode"
        Me.boxMode.Size = New System.Drawing.Size(220, 110)
        Me.boxMode.TabIndex = 15
        Me.boxMode.TabStop = False
        Me.boxMode.Text = "Creation Mode"
        '
        'modeNothing
        '
        Me.modeNothing.AutoSize = True
        Me.modeNothing.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.modeNothing.Location = New System.Drawing.Point(6, 82)
        Me.modeNothing.Name = "modeNothing"
        Me.modeNothing.Size = New System.Drawing.Size(60, 22)
        Me.modeNothing.TabIndex = 21
        Me.modeNothing.Text = "None"
        Me.modeNothing.UseVisualStyleBackColor = True
        '
        'modeStar
        '
        Me.modeStar.AutoSize = True
        Me.modeStar.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.modeStar.Location = New System.Drawing.Point(6, 54)
        Me.modeStar.Name = "modeStar"
        Me.modeStar.Size = New System.Drawing.Size(50, 22)
        Me.modeStar.TabIndex = 17
        Me.modeStar.Text = "Star"
        Me.modeStar.UseVisualStyleBackColor = True
        '
        'modePlanet
        '
        Me.modePlanet.AutoSize = True
        Me.modePlanet.Checked = True
        Me.modePlanet.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.modePlanet.Location = New System.Drawing.Point(6, 26)
        Me.modePlanet.Name = "modePlanet"
        Me.modePlanet.Size = New System.Drawing.Size(66, 22)
        Me.modePlanet.TabIndex = 16
        Me.modePlanet.TabStop = True
        Me.modePlanet.Text = "Planet"
        Me.trajTooltip.SetToolTip(Me.modePlanet, "Hold CTRL for continuous mode.")
        Me.modePlanet.UseVisualStyleBackColor = True
        '
        'boxGeneral
        '
        Me.boxGeneral.Controls.Add(Me.followSelected)
        Me.boxGeneral.Controls.Add(Me.numTraj)
        Me.boxGeneral.Controls.Add(Me.generalTraj)
        Me.boxGeneral.Controls.Add(Me.canvasClear)
        Me.boxGeneral.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxGeneral.Location = New System.Drawing.Point(1110, 450)
        Me.boxGeneral.Name = "boxGeneral"
        Me.boxGeneral.Size = New System.Drawing.Size(222, 164)
        Me.boxGeneral.TabIndex = 16
        Me.boxGeneral.TabStop = False
        Me.boxGeneral.Text = "General Options"
        '
        'followSelected
        '
        Me.followSelected.AutoSize = True
        Me.followSelected.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.followSelected.Location = New System.Drawing.Point(6, 53)
        Me.followSelected.Name = "followSelected"
        Me.followSelected.Size = New System.Drawing.Size(167, 22)
        Me.followSelected.TabIndex = 13
        Me.followSelected.Text = "Follow selected object"
        Me.followSelected.UseVisualStyleBackColor = True
        '
        'numTraj
        '
        Me.numTraj.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.numTraj.Location = New System.Drawing.Point(111, 78)
        Me.numTraj.Maximum = New Decimal(New Integer() {999999, 0, 0, 0})
        Me.numTraj.Minimum = New Decimal(New Integer() {100, 0, 0, 0})
        Me.numTraj.Name = "numTraj"
        Me.numTraj.Size = New System.Drawing.Size(74, 26)
        Me.numTraj.TabIndex = 9
        Me.numTraj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.numTraj.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'generalTraj
        '
        Me.generalTraj.AutoSize = True
        Me.generalTraj.Checked = True
        Me.generalTraj.CheckState = System.Windows.Forms.CheckState.Checked
        Me.generalTraj.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.generalTraj.Location = New System.Drawing.Point(6, 79)
        Me.generalTraj.Name = "generalTraj"
        Me.generalTraj.Size = New System.Drawing.Size(99, 22)
        Me.generalTraj.TabIndex = 12
        Me.generalTraj.Text = "Trajectories"
        Me.generalTraj.UseVisualStyleBackColor = True
        '
        'boxCollision
        '
        Me.boxCollision.Controls.Add(Me.collisionNone)
        Me.boxCollision.Controls.Add(Me.collisionBounce)
        Me.boxCollision.Controls.Add(Me.collisionTunnel)
        Me.boxCollision.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxCollision.Location = New System.Drawing.Point(1124, 264)
        Me.boxCollision.Name = "boxCollision"
        Me.boxCollision.Size = New System.Drawing.Size(214, 110)
        Me.boxCollision.TabIndex = 17
        Me.boxCollision.TabStop = False
        Me.boxCollision.Text = "Collision"
        '
        'collisionNone
        '
        Me.collisionNone.AutoSize = True
        Me.collisionNone.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.collisionNone.Location = New System.Drawing.Point(6, 82)
        Me.collisionNone.Name = "collisionNone"
        Me.collisionNone.Size = New System.Drawing.Size(60, 22)
        Me.collisionNone.TabIndex = 20
        Me.collisionNone.Text = "None"
        Me.collisionNone.UseVisualStyleBackColor = True
        '
        'collisionBounce
        '
        Me.collisionBounce.AutoSize = True
        Me.collisionBounce.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.collisionBounce.Location = New System.Drawing.Point(6, 54)
        Me.collisionBounce.Name = "collisionBounce"
        Me.collisionBounce.Size = New System.Drawing.Size(72, 22)
        Me.collisionBounce.TabIndex = 19
        Me.collisionBounce.Text = "Bounce"
        Me.collisionBounce.UseVisualStyleBackColor = True
        '
        'collisionTunnel
        '
        Me.collisionTunnel.AutoSize = True
        Me.collisionTunnel.Checked = True
        Me.collisionTunnel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.collisionTunnel.Location = New System.Drawing.Point(6, 26)
        Me.collisionTunnel.Name = "collisionTunnel"
        Me.collisionTunnel.Size = New System.Drawing.Size(68, 22)
        Me.collisionTunnel.TabIndex = 18
        Me.collisionTunnel.TabStop = True
        Me.collisionTunnel.Text = "Tunnel"
        Me.collisionTunnel.UseVisualStyleBackColor = True
        '
        'trajTooltip
        '
        Me.trajTooltip.AutoPopDelay = 10000
        Me.trajTooltip.InitialDelay = 500
        Me.trajTooltip.IsBalloon = True
        Me.trajTooltip.ReshowDelay = 100
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(689, 539)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(114, 33)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "A: 5.3434 , 5.343434 L: 300,4 -- 241,4"
        Me.trajTooltip.SetToolTip(Me.Label1, "Sample tooltip for objects.")
        '
        'boxCoords
        '
        Me.boxCoords.Controls.Add(Me.lblCoordsAbs)
        Me.boxCoords.Controls.Add(Me.lblCoordsMouse)
        Me.boxCoords.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.boxCoords.Location = New System.Drawing.Point(865, 527)
        Me.boxCoords.Name = "boxCoords"
        Me.boxCoords.Size = New System.Drawing.Size(236, 87)
        Me.boxCoords.TabIndex = 23
        Me.boxCoords.TabStop = False
        Me.boxCoords.Text = "Coordinates (Real/Absolute)"
        '
        'lblCoordsAbs
        '
        Me.lblCoordsAbs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCoordsAbs.AutoSize = True
        Me.lblCoordsAbs.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblCoordsAbs.Location = New System.Drawing.Point(24, 51)
        Me.lblCoordsAbs.Name = "lblCoordsAbs"
        Me.lblCoordsAbs.Size = New System.Drawing.Size(0, 23)
        Me.lblCoordsAbs.TabIndex = 1
        '
        'boxObjectInfo
        '
        Me.boxObjectInfo.Controls.Add(Me.objectListView)
        Me.boxObjectInfo.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxObjectInfo.Location = New System.Drawing.Point(12, 505)
        Me.boxObjectInfo.Name = "boxObjectInfo"
        Me.boxObjectInfo.Size = New System.Drawing.Size(590, 115)
        Me.boxObjectInfo.TabIndex = 36
        Me.boxObjectInfo.TabStop = False
        Me.boxObjectInfo.Text = "Object List"
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
        'objectListView
        '
        Me.objectListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.NameHeader, Me.LocationXHeader, Me.LocationYHeader, Me.AccelerationHeader, Me.VelocityHeader, Me.AccelerationXHeader, Me.VelocityXHeader, Me.AccelerationYHeader, Me.VelocityYHeader, Me.MassHeader, Me.SizeHeader})
        ListViewGroup3.Header = "Stars"
        ListViewGroup3.Name = "Stars"
        ListViewGroup4.Header = "Planets"
        ListViewGroup4.Name = "Planets"
        ListViewGroup4.Tag = ""
        Me.objectListView.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup3, ListViewGroup4})
        Me.objectListView.Location = New System.Drawing.Point(6, 22)
        Me.objectListView.Name = "objectListView"
        Me.objectListView.Size = New System.Drawing.Size(578, 87)
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
        'LocationYHeader
        '
        Me.LocationYHeader.Text = "Location Y"
        Me.LocationYHeader.Width = 108
        '
        'AccelerationXHeader
        '
        Me.AccelerationXHeader.Text = "Acceleration X"
        Me.AccelerationXHeader.Width = 131
        '
        'AccelerationYHeader
        '
        Me.AccelerationYHeader.Text = "Acceleration Y"
        Me.AccelerationYHeader.Width = 124
        '
        'VelocityXHeader
        '
        Me.VelocityXHeader.Text = "Velocity X"
        Me.VelocityXHeader.Width = 117
        '
        'VelocityYHeader
        '
        Me.VelocityYHeader.Text = "Velocity Y"
        Me.VelocityYHeader.Width = 141
        '
        'blockForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1347, 626)
        Me.Controls.Add(Me.lblStatusMessage)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Controls.Add(Me.boxObjectInfo)
        Me.Controls.Add(Me.boxCoords)
        Me.Controls.Add(Me.boxCollision)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.boxGeneral)
        Me.Controls.Add(Me.boxMode)
        Me.Controls.Add(Me.boxTimeParam)
        Me.Controls.Add(Me.planetslbl)
        Me.Controls.Add(Me.boxStarParam)
        Me.Controls.Add(Me.boxPlanetParam)
        Me.Controls.Add(Me.dummyUniverse)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(1363, 642)
        Me.Name = "blockForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Block Universe"
        CType(Me.numPlanetParamXVel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTimeTick, System.ComponentModel.ISupportInitialize).EndInit()
        Me.boxPlanetParam.ResumeLayout(False)
        Me.boxPlanetParam.PerformLayout()
        CType(Me.numPlanetParamYVel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.boxStarParam.ResumeLayout(False)
        Me.boxStarParam.PerformLayout()
        CType(Me.numStarParamYVel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numStarParamXVel, System.ComponentModel.ISupportInitialize).EndInit()
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
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCoordsMouse As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents numPlanetParamXVel As NumericUpDown
    Friend WithEvents lblPlanetParamXVel As Label
    Friend WithEvents lblTimeTick As Label
    Private WithEvents numTimeTick As NumericUpDown
    Friend WithEvents planetslbl As Label
    Friend WithEvents canvasClear As CheckBox
    Friend WithEvents btnReset As Button
    Friend WithEvents boxPlanetParam As GroupBox
    Friend WithEvents lblPlanetParamYVel As Label
    Friend WithEvents numPlanetParamYVel As NumericUpDown
    Friend WithEvents boxTimeParam As GroupBox
    Friend WithEvents boxMode As GroupBox
    Friend WithEvents modeStar As RadioButton
    Friend WithEvents modePlanet As RadioButton
    Friend WithEvents boxStarParam As GroupBox
    Friend WithEvents lblStarParamYVel As Label
    Friend WithEvents numStarParamYVel As NumericUpDown
    Friend WithEvents lblStarParamXVel As Label
    Friend WithEvents numStarParamXVel As NumericUpDown
    Friend WithEvents boxGeneral As GroupBox
    Friend WithEvents boxCollision As GroupBox
    Friend WithEvents collisionBounce As RadioButton
    Friend WithEvents collisionTunnel As RadioButton
    Friend WithEvents generalTraj As CheckBox
    Private WithEvents numTraj As NumericUpDown
    Friend WithEvents trajTooltip As ToolTip
    Friend WithEvents Label1 As Label
    Friend WithEvents collisionNone As RadioButton
    Friend WithEvents boxCoords As GroupBox
    Friend WithEvents lblCoordsAbs As Label
    Friend WithEvents boxObjectInfo As GroupBox
    Friend WithEvents modeNothing As RadioButton
    Friend WithEvents btnPauseUniverse As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents dummyUniverse As PictureBox
    Friend WithEvents lblStatusMessage As Label
    Friend WithEvents followSelected As CheckBox
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
End Class
