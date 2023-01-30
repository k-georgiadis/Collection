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
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Stars", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Planets", System.Windows.Forms.HorizontalAlignment.Left)
        Me.lblCoordsMouse = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblInfoVel = New System.Windows.Forms.Label()
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
        Me.lblInfoAcc = New System.Windows.Forms.Label()
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
        Me.txtInfoLocX = New System.Windows.Forms.Label()
        Me.lblInfoLoc = New System.Windows.Forms.Label()
        Me.txtInfoSize = New System.Windows.Forms.Label()
        Me.txtInfoMass = New System.Windows.Forms.Label()
        Me.txtInfoVelX = New System.Windows.Forms.Label()
        Me.lblInfoMass = New System.Windows.Forms.Label()
        Me.lblInfoSize = New System.Windows.Forms.Label()
        Me.txtInfoAccX = New System.Windows.Forms.Label()
        Me.boxObjectList = New System.Windows.Forms.GroupBox()
        Me.panelObjectList = New System.Windows.Forms.Panel()
        Me.boxObjectInfo = New System.Windows.Forms.GroupBox()
        Me.txtInfoLocY = New System.Windows.Forms.Label()
        Me.txtInfoVelY = New System.Windows.Forms.Label()
        Me.txtInfoAccY = New System.Windows.Forms.Label()
        Me.btnPauseUniverse = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.dummyUniverse = New System.Windows.Forms.PictureBox()
        Me.lblStatusMessage = New System.Windows.Forms.Label()
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.Name = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Location = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Acceleration = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Velocity = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Mass = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Size = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
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
        Me.boxObjectList.SuspendLayout()
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
        Me.lblCoordsMouse.Location = New System.Drawing.Point(16, 22)
        Me.lblCoordsMouse.Name = "lblCoordsMouse"
        Me.lblCoordsMouse.Size = New System.Drawing.Size(0, 23)
        Me.lblCoordsMouse.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Interval = 1
        '
        'lblInfoVel
        '
        Me.lblInfoVel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfoVel.AutoSize = True
        Me.lblInfoVel.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblInfoVel.Location = New System.Drawing.Point(26, 84)
        Me.lblInfoVel.Name = "lblInfoVel"
        Me.lblInfoVel.Size = New System.Drawing.Size(93, 18)
        Me.lblInfoVel.TabIndex = 3
        Me.lblInfoVel.Text = "Velocity (X, Y)"
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
        Me.boxTimeParam.Location = New System.Drawing.Point(1110, 398)
        Me.boxTimeParam.Name = "boxTimeParam"
        Me.boxTimeParam.Size = New System.Drawing.Size(228, 63)
        Me.boxTimeParam.TabIndex = 14
        Me.boxTimeParam.TabStop = False
        Me.boxTimeParam.Text = "Time Parameters"
        '
        'lblInfoAcc
        '
        Me.lblInfoAcc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfoAcc.AutoSize = True
        Me.lblInfoAcc.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblInfoAcc.Location = New System.Drawing.Point(0, 54)
        Me.lblInfoAcc.Name = "lblInfoAcc"
        Me.lblInfoAcc.Size = New System.Drawing.Size(120, 18)
        Me.lblInfoAcc.TabIndex = 4
        Me.lblInfoAcc.Text = "Acceleration (X, Y)"
        '
        'boxMode
        '
        Me.boxMode.Controls.Add(Me.modeNothing)
        Me.boxMode.Controls.Add(Me.modeStar)
        Me.boxMode.Controls.Add(Me.modePlanet)
        Me.boxMode.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxMode.Location = New System.Drawing.Point(608, 505)
        Me.boxMode.Name = "boxMode"
        Me.boxMode.Size = New System.Drawing.Size(128, 115)
        Me.boxMode.TabIndex = 15
        Me.boxMode.TabStop = False
        Me.boxMode.Text = "Creation Mode"
        '
        'modeNothing
        '
        Me.modeNothing.AutoSize = True
        Me.modeNothing.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.modeNothing.Location = New System.Drawing.Point(6, 55)
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
        Me.modeStar.Location = New System.Drawing.Point(72, 26)
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
        Me.boxGeneral.Location = New System.Drawing.Point(896, 505)
        Me.boxGeneral.Name = "boxGeneral"
        Me.boxGeneral.Size = New System.Drawing.Size(191, 115)
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
        Me.numTraj.Location = New System.Drawing.Point(111, 79)
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
        Me.generalTraj.Location = New System.Drawing.Point(6, 80)
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
        Me.boxCollision.Location = New System.Drawing.Point(742, 505)
        Me.boxCollision.Name = "boxCollision"
        Me.boxCollision.Size = New System.Drawing.Size(148, 115)
        Me.boxCollision.TabIndex = 17
        Me.boxCollision.TabStop = False
        Me.boxCollision.Text = "Collision"
        '
        'collisionNone
        '
        Me.collisionNone.AutoSize = True
        Me.collisionNone.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.collisionNone.Location = New System.Drawing.Point(6, 54)
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
        Me.collisionBounce.Location = New System.Drawing.Point(72, 26)
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
        Me.Label1.Location = New System.Drawing.Point(1221, 464)
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
        Me.boxCoords.Location = New System.Drawing.Point(1110, 505)
        Me.boxCoords.Name = "boxCoords"
        Me.boxCoords.Size = New System.Drawing.Size(228, 87)
        Me.boxCoords.TabIndex = 23
        Me.boxCoords.TabStop = False
        Me.boxCoords.Text = "Coordinates (Real/Absolute)"
        '
        'lblCoordsAbs
        '
        Me.lblCoordsAbs.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCoordsAbs.AutoSize = True
        Me.lblCoordsAbs.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblCoordsAbs.Location = New System.Drawing.Point(16, 51)
        Me.lblCoordsAbs.Name = "lblCoordsAbs"
        Me.lblCoordsAbs.Size = New System.Drawing.Size(0, 23)
        Me.lblCoordsAbs.TabIndex = 1
        '
        'txtInfoLocX
        '
        Me.txtInfoLocX.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoLocX.Location = New System.Drawing.Point(125, 21)
        Me.txtInfoLocX.Name = "txtInfoLocX"
        Me.txtInfoLocX.Size = New System.Drawing.Size(145, 26)
        Me.txtInfoLocX.TabIndex = 33
        '
        'lblInfoLoc
        '
        Me.lblInfoLoc.AutoSize = True
        Me.lblInfoLoc.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblInfoLoc.Location = New System.Drawing.Point(25, 27)
        Me.lblInfoLoc.Name = "lblInfoLoc"
        Me.lblInfoLoc.Size = New System.Drawing.Size(94, 18)
        Me.lblInfoLoc.TabIndex = 31
        Me.lblInfoLoc.Text = "Location (X, Y)"
        '
        'txtInfoSize
        '
        Me.txtInfoSize.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoSize.Location = New System.Drawing.Point(476, 54)
        Me.txtInfoSize.Name = "txtInfoSize"
        Me.txtInfoSize.Size = New System.Drawing.Size(108, 26)
        Me.txtInfoSize.TabIndex = 32
        '
        'txtInfoMass
        '
        Me.txtInfoMass.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoMass.Location = New System.Drawing.Point(476, 21)
        Me.txtInfoMass.Name = "txtInfoMass"
        Me.txtInfoMass.Size = New System.Drawing.Size(108, 26)
        Me.txtInfoMass.TabIndex = 35
        '
        'txtInfoVelX
        '
        Me.txtInfoVelX.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoVelX.Location = New System.Drawing.Point(125, 83)
        Me.txtInfoVelX.Name = "txtInfoVelX"
        Me.txtInfoVelX.Size = New System.Drawing.Size(145, 26)
        Me.txtInfoVelX.TabIndex = 28
        '
        'lblInfoMass
        '
        Me.lblInfoMass.AutoSize = True
        Me.lblInfoMass.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblInfoMass.Location = New System.Drawing.Point(431, 27)
        Me.lblInfoMass.Name = "lblInfoMass"
        Me.lblInfoMass.Size = New System.Drawing.Size(39, 18)
        Me.lblInfoMass.TabIndex = 30
        Me.lblInfoMass.Text = "Mass"
        '
        'lblInfoSize
        '
        Me.lblInfoSize.AutoSize = True
        Me.lblInfoSize.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(161, Byte))
        Me.lblInfoSize.Location = New System.Drawing.Point(437, 57)
        Me.lblInfoSize.Name = "lblInfoSize"
        Me.lblInfoSize.Size = New System.Drawing.Size(33, 18)
        Me.lblInfoSize.TabIndex = 34
        Me.lblInfoSize.Text = "Size"
        '
        'txtInfoAccX
        '
        Me.txtInfoAccX.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoAccX.Location = New System.Drawing.Point(125, 52)
        Me.txtInfoAccX.Name = "txtInfoAccX"
        Me.txtInfoAccX.Size = New System.Drawing.Size(145, 26)
        Me.txtInfoAccX.TabIndex = 29
        '
        'boxObjectList
        '
        Me.boxObjectList.Controls.Add(Me.panelObjectList)
        Me.boxObjectList.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxObjectList.Location = New System.Drawing.Point(1110, 148)
        Me.boxObjectList.Name = "boxObjectList"
        Me.boxObjectList.Size = New System.Drawing.Size(228, 244)
        Me.boxObjectList.TabIndex = 5
        Me.boxObjectList.TabStop = False
        Me.boxObjectList.Text = "Object List"
        '
        'panelObjectList
        '
        Me.panelObjectList.AutoScroll = True
        Me.panelObjectList.BackColor = System.Drawing.Color.Transparent
        Me.panelObjectList.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.panelObjectList.Location = New System.Drawing.Point(0, 22)
        Me.panelObjectList.Name = "panelObjectList"
        Me.panelObjectList.Size = New System.Drawing.Size(228, 222)
        Me.panelObjectList.TabIndex = 37
        '
        'boxObjectInfo
        '
        Me.boxObjectInfo.Controls.Add(Me.txtInfoLocY)
        Me.boxObjectInfo.Controls.Add(Me.txtInfoVelY)
        Me.boxObjectInfo.Controls.Add(Me.txtInfoAccY)
        Me.boxObjectInfo.Controls.Add(Me.txtInfoSize)
        Me.boxObjectInfo.Controls.Add(Me.txtInfoLocX)
        Me.boxObjectInfo.Controls.Add(Me.txtInfoVelX)
        Me.boxObjectInfo.Controls.Add(Me.lblInfoMass)
        Me.boxObjectInfo.Controls.Add(Me.lblInfoSize)
        Me.boxObjectInfo.Controls.Add(Me.lblInfoVel)
        Me.boxObjectInfo.Controls.Add(Me.txtInfoAccX)
        Me.boxObjectInfo.Controls.Add(Me.lblInfoLoc)
        Me.boxObjectInfo.Controls.Add(Me.lblInfoAcc)
        Me.boxObjectInfo.Controls.Add(Me.txtInfoMass)
        Me.boxObjectInfo.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.boxObjectInfo.Location = New System.Drawing.Point(12, 505)
        Me.boxObjectInfo.Name = "boxObjectInfo"
        Me.boxObjectInfo.Size = New System.Drawing.Size(590, 115)
        Me.boxObjectInfo.TabIndex = 36
        Me.boxObjectInfo.TabStop = False
        Me.boxObjectInfo.Text = "Object Info"
        '
        'txtInfoLocY
        '
        Me.txtInfoLocY.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoLocY.Location = New System.Drawing.Point(276, 21)
        Me.txtInfoLocY.Name = "txtInfoLocY"
        Me.txtInfoLocY.Size = New System.Drawing.Size(145, 26)
        Me.txtInfoLocY.TabIndex = 38
        '
        'txtInfoVelY
        '
        Me.txtInfoVelY.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoVelY.Location = New System.Drawing.Point(276, 83)
        Me.txtInfoVelY.Name = "txtInfoVelY"
        Me.txtInfoVelY.Size = New System.Drawing.Size(145, 26)
        Me.txtInfoVelY.TabIndex = 36
        '
        'txtInfoAccY
        '
        Me.txtInfoAccY.Font = New System.Drawing.Font("Calibri", 11.25!)
        Me.txtInfoAccY.Location = New System.Drawing.Point(276, 52)
        Me.txtInfoAccY.Name = "txtInfoAccY"
        Me.txtInfoAccY.Size = New System.Drawing.Size(145, 26)
        Me.txtInfoAccY.TabIndex = 37
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
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Name, Me.Location, Me.Acceleration, Me.Velocity, Me.Mass, Me.Size})
        ListViewGroup1.Header = "Stars"
        ListViewGroup1.Name = "Stars"
        ListViewGroup2.Header = "Planets"
        ListViewGroup2.Name = "Planets"
        ListViewGroup2.Tag = ""
        Me.ListView1.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2})
        Me.ListView1.Location = New System.Drawing.Point(254, 238)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(583, 176)
        Me.ListView1.TabIndex = 42
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'Name
        '
        Me.Name.Text = "Name"
        Me.Name.Width = 86
        '
        'Location
        '
        Me.Location.Text = "Location"
        Me.Location.Width = 123
        '
        'Acceleration
        '
        Me.Acceleration.Text = "Acceleration"
        Me.Acceleration.Width = 115
        '
        'Velocity
        '
        Me.Velocity.Text = "Velocity"
        Me.Velocity.Width = 113
        '
        'Mass
        '
        Me.Mass.Text = "Mass"
        Me.Mass.Width = 69
        '
        'Size
        '
        Me.Size.Text = "Size"
        Me.Size.Width = 72
        '
        'blockForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1347, 626)
        Me.Controls.Add(Me.ListView1)
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
        Me.Controls.Add(Me.boxObjectList)
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
        Me.boxObjectList.ResumeLayout(False)
        Me.boxObjectInfo.ResumeLayout(False)
        Me.boxObjectInfo.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        CType(Me.dummyUniverse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCoordsMouse As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lblInfoVel As Label
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
    Friend WithEvents lblInfoAcc As Label
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
    Friend WithEvents boxObjectList As GroupBox
    Friend WithEvents txtInfoLocX As Label
    Friend WithEvents txtInfoSize As Label
    Friend WithEvents lblInfoLoc As Label
    Friend WithEvents lblInfoMass As Label
    Friend WithEvents txtInfoAccX As Label
    Friend WithEvents txtInfoVelX As Label
    Friend WithEvents txtInfoMass As Label
    Friend WithEvents lblInfoSize As Label
    Friend WithEvents boxObjectInfo As GroupBox
    Friend WithEvents panelObjectList As Panel
    Friend WithEvents modeNothing As RadioButton
    Friend WithEvents btnPauseUniverse As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents dummyUniverse As PictureBox
    Friend WithEvents lblStatusMessage As Label
    Friend WithEvents txtInfoLocY As Label
    Friend WithEvents txtInfoVelY As Label
    Friend WithEvents txtInfoAccY As Label
    Friend WithEvents followSelected As CheckBox
    Friend WithEvents ListView1 As ListView
    Friend WithEvents Name As ColumnHeader
    Friend WithEvents Location As ColumnHeader
    Friend WithEvents Acceleration As ColumnHeader
    Friend WithEvents Velocity As ColumnHeader
    Friend WithEvents Mass As ColumnHeader
    Friend WithEvents Size As ColumnHeader
End Class
