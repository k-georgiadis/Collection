'MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.ComponentModel
Imports System.Configuration
Imports System.Runtime.InteropServices
Imports System.Threading
Imports StellarObjectSimulation.StellarObject

'FIXED BUG: Bottom right tunneling doesn't create transition for top-left corner. Bottom left does though. Something I missed?
'The duplicate points are not the center of the ellipse but it's top-left corner. That is because of how the ellipse is drawn.
'It seems I had forgotten to add the "2 * star.GetRadius" part in the condition.

'FIXED BUG: Fix star labels when merged. They hide but the program keeps checking them. We need an extra variable to check whether their label has been disposed.
'The hidden label status was misconfigured.

'FIXED BUG: When applying acceleration from planets to stars and vice-versa, sometimes the collection was modified (created a new object).
'Acceleration is now calculated by only one thread.

'FIXED BUG: Sometimes when painting planets fast, the labels are mispositioned. Index is off by one step.
'While applying accelerations, sometimes a merge occurs while creating an object label.
'Therefore, while finding how many objects are merged, after a few lines of code it changes because we are still applying acceleration and a merge occured.
'This happens because when the object count grows larger the applyAcceleration method takes a while. So by the time it checks if we are creating a label, it's too late.
'We fix it by correcting the label location while creating the label.


Public Class blockForm

    Dim rand As New Random

    Dim starBorderWidth As Integer = 1
    Dim planetBorderWidth As Integer = 2

    Dim planetColor As Color = Color.Yellow

    Dim myPen As New Pen(planetColor, planetBorderWidth)
    Dim myBrush As SolidBrush = New SolidBrush(Color.AliceBlue)

    Dim formDefaultWidth As Integer
    Dim formDefaultHeight As Integer

    Dim starRadius As Integer = 20
    Dim starSolarMass As Double = 1 'Multiplier of Solar mass.
    Dim planetSize As Integer = 10
    Dim planetEarthMass As Double = 1 'Multiplier of Earth mass.

    Dim myUniverse As New Universe
    Dim myUniverseMatrix As New Drawing2D.Matrix
    Dim myInverseUniverseMatrix As New Drawing2D.Matrix

    Dim myMinimap As New Minimap
    Dim currentCosmosSection As Byte = 1 'Default section.
    Dim navigatingMinimap As Boolean = False 'Are we navigating in the Minimap UI?

    Dim defaultUniverseMatrix As New Drawing2D.Matrix
    Dim defaultUniverseWidth As Double = 1100
    Dim defaultUniverseHeight As Double = 500

    Dim universeWidth As Double = defaultUniverseWidth
    Dim universeHeight As Double = defaultUniverseHeight
    Dim imageWidth As Integer = universeWidth
    Dim imageHeight As Integer = universeHeight

    Dim defaultClipOffset As Double = 50
    Dim clipOffsetX As Double = defaultClipOffset
    Dim clipOffsetY As Double = defaultClipOffset
    Dim totalDragOffset As New PointF(0, 0)

    'Thread for each object. They get destroyed as soon as the object is fully created.
    Dim threadList As New List(Of Threading.Thread)

    Dim mousePoint As New PointF
    Dim minimapPoint As New PointF
    Dim absoluteMousePoint As New PointF

    Dim zoomValue As Double = 1.0 'Default zoom value.
    Dim zoomStep As Double = 0.05 'Default zoom step.
    Dim zAxisMultiplier As Integer = 1 'Multiplier for initial Z position.
    Dim cameraZOffset As Double = 0 'The offset of the camera in the Z axis.

    'We need these to correctly display labels and for accurate hover detections.
    Dim scaledStarBWidth As Double = starBorderWidth
    Dim scaledPlanetBWidth As Double = planetBorderWidth

    Dim dragPoint As New PointF
    Dim dragStart As Boolean = False
    Dim resetOffset As Boolean = False
    Dim minimapDragging As Boolean = False

    Dim ctrlKeyDown As Boolean = False
    Dim mouseIsDown As Boolean = False
    Dim counter As Integer = 0

    Dim selectedObject As StellarObject = Nothing
    Dim followingObject As Boolean = False
    Dim relativeTrajectories As Boolean = False
    Dim hoverObject As StellarObject = Nothing
    Dim hover As Boolean = False
    Dim hoverLabel As New Label
    Dim hoverLabelOverflow As String = ""
    Dim hideHoverInfo As Boolean = False

    Dim formLoaded As Boolean = False

    'Tool settings.
    Dim isSelecting As Boolean = False
    Dim selectionDist As New PointF(0, 0)
    Dim selectionRectangle As New RectangleF(0, 0, 0, 0)
    Dim realSelectionRectangle As New RectangleF(0, 0, 0, 0)

    'Creation mode variables.
    Dim creationMode As String

    'Flags for threads.
    Dim UniversePaused As Boolean = False
    Dim UniversePausedForDragging As Boolean = False
    Dim dragging As Boolean = False
    Dim zooming As Boolean = False
    Dim onLive As Boolean = False
    Dim onFrame As Boolean = False
    Dim StarArrayInUseFlag As Boolean = False
    Dim PlanetArrayInUseFlag As Boolean = False
    Dim paintingStars As Boolean = False
    Dim paintingPlanets As Boolean = False

    'Timers and threads.
    Dim debug_stopwatch As New Stopwatch()

    Dim tickValue As Integer
    Dim tickModeValues() As Integer = {10000000, 100000, 0, -1} 'Fast, Faster, Fastest.
    Dim tickModeSelectedValue As Integer = -1 'The selected tick mode value.
    Dim updateCounter As Integer = 0
    Dim frameCounter As Integer = 0
    Dim FPS As Integer = 0
    Dim fpsTimer As New Timers.Timer(1000) '1 second.

    Dim mainThread As New Thread(AddressOf UniverseLive)
    Dim paintThread As New Thread(AddressOf Frame)

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Starting/Loading world events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub blockForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        picUniverse.Dispose() 'Destroy dummy universe.

        trajTooltip.SetToolTip(numTraj, "The number of points an object uses for its trajectory. Minimum 100 - Maximum 999999." & vbCrLf &
                                             "CPU power must be considered before applying large values.")

        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        Me.UpdateStyles()

        formDefaultWidth = Me.Width
        formDefaultHeight = Me.Height

        tickValue = numTimeTick.Value
        cbCreateType.SelectedItem = cbCreateType.Items(0)
        cbTickSpeed.SelectedItem = cbTickSpeed.Items(0)

        'Init universe/world for the first time.
        InitWorld(False)

        'Start main thread for calculating accelerations and movements.
        mainThread.Start()
        'paintThread.Start()

    End Sub

    Private Sub InitWorld(ByVal reset As Boolean)

        'Is simulation realistic? If not, we tone down the calculations to make it faster but more inaccurate.
        myUniverse.isRealistic = chkRealisticSim.Checked

        'Init planet universe.
        myUniverse.Init(myPen, universeWidth, universeHeight, formDefaultWidth, formDefaultHeight,
                        New PointFD(clipOffsetX, clipOffsetY), numTraj.Value)

        'Set default transformation matrix.
        defaultUniverseMatrix = myUniverse.defUniverseMatrix

        'Load minimap.
        myMinimap.Init(boxMinimap, picMinimap, pnlCosmos, New List(Of Button)({btnSection1, btnSection2, btnSection3,
                                                                   btnSection4, btnSection5, btnSection6,
                                                                   btnSection7, btnSection8, btnSection9}),
                       btnNavReturn)

        formLoaded = True
        Timer1.Enabled = True

        'Init fps timer if first time.
        If Not reset Then
            AddHandler fpsTimer.Elapsed, New Timers.ElapsedEventHandler(AddressOf ResetFPS)
            fpsTimer.AutoReset = True
        End If

        'Start timer.
        fpsTimer.Enabled = True

    End Sub
    Private Sub UniverseLive()

        Dim cycles As Integer = 0

        'startstars()

        'So basically the idea is this:
        'We calculate the gravities applied to each object from all other objects.
        'Then we move the objects. That way we eliminate the distance "errors" that occured by moving the object as soon as its accelaration was calculated.
        'Because we used threads for each object, they weren't simultaneous so by the time an object tried to calculate it's gravity from another object,
        'that object had already moved (because we already calculated its accelaration) and therefore the new object would get a new accelaration NOT EQUAL to the accelaration
        'that the other object had been applied to by this object.

        'Now we calculate the forces for ALL objects and THEN we move them.

        While 1

            'Just paint the universe when paused.
            If UniversePaused Then
                Thread.Sleep(1) 'Wait 1ms to catch mouse events before drawing.
                Frame()
                Continue While
            End If

            'Wait for painting to complete.
            If paintingStars Or paintingPlanets Then Continue While

            onLive = 1

            'Start calculating accelarations and move objects.
            myUniverse.Live(StarArrayInUseFlag, PlanetArrayInUseFlag)
            frameCounter += 1

            onLive = 0
            myUniverse.FrameRate = FPS

            'Paint universe.
            Frame()

            'Tick delay.
            If tickModeSelectedValue < 0 Then
                cycles = 0
                Thread.Sleep(tickValue)
            Else
                While cycles < tickModeSelectedValue
                    cycles += 1
                End While
                cycles = 0
                'Thread.Sleep(1) 'Delay 1 ms.
            End If

        End While

    End Sub

    Private Sub startstars()

        Dim posSun As New PointFD(550 - 1, 250 - 1) 'Sun.
        Dim posMercury As New PointFD(posSun.X - 69.8169, 250 - 1) 'Mercury at aphelion.
        Dim posVenus As New PointFD(posSun.X - 108.94, 250 - 1) 'Venus at aphelion.
        Dim posEarth As New PointFD(posSun.X - 152.1, 250 - 1) 'Earth at aphelion.
        Dim posMars As New PointFD(posSun.X - 249.2, 250 - 1) 'Mars at aphelion.
        Dim posJupiter As New PointFD(posSun.X - 817, 250 - 1) 'Jupiter at aphelion.
        Dim posSaturn As New PointFD(posSun.X - 1348, 250 - 1) 'Saturn at aphelion.
        Dim posUranus As New PointFD(posSun.X - 3003.625, 250 - 1) 'Uranus at aphelion.
        Dim posNeptune As New PointFD(posSun.X - 4545.671, 250 - 1) 'Neptune at aphelion.
        Dim posPluto As New PointFD(posSun.X - 7304.326, 250 - 1) 'Pluto at aphelion.

        'Set new random color.
        myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Add Sun.
        threadList.Add(New Thread(AddressOf createstar))
        threadList.Last.Start(New Object() {posSun, New PointFD(0, 0)})
        Thread.Sleep(20)

        'Add Mercury.
        threadList.Add(New Thread(AddressOf createplanet))
        threadList.Last.Start(New Object() {posMercury, New PointFD(0, 38.86), 0.0553}) 'Speed at aphelion and mass.
        Thread.Sleep(20)

        'Add Venus.
        threadList.Add(New Thread(AddressOf createplanet))
        threadList.Last.Start(New Object() {posVenus, New PointFD(0, 34.78), 0.815})
        Thread.Sleep(20)

        'Add Earth.
        threadList.Add(New Thread(AddressOf createplanet))
        threadList.Last.Start(New Object() {posEarth, New PointFD(0, 29.29), 1})
        Thread.Sleep(20)

        'Add Mars.
        threadList.Add(New Thread(AddressOf createplanet))
        threadList.Last.Start(New Object() {posMars, New PointFD(0, 22.0), 0.107})
        Thread.Sleep(20)

        ''Add Jupiter.
        'threadList.Add(New Thread(AddressOf createplanet))
        'threadList.Last.Start(New Object() {posJupiter, New PointFD(0, 317.83)})
        'Thread.Sleep(20)

        ''Add Saturn.
        'threadList.Add(New Thread(AddressOf createplanet))
        'threadList.Last.Start(New Object() {posSaturn, New PointFD(0, 9.09)})
        'Thread.Sleep(20)

        ''Add Uranus.
        'threadList.Add(New Thread(AddressOf createplanet))
        'threadList.Last.Start(New Object() {posUranus, New PointFD(0, 6.49)})
        'Thread.Sleep(20)

        ''Add Neptune.
        'threadList.Add(New Thread(AddressOf createplanet))
        'threadList.Last.Start(New Object() {posNeptune, New PointFD(0, 5.37)})
        'Thread.Sleep(20)

        ''Add Pluto.
        'threadList.Add(New Thread(AddressOf createplanet))
        'threadList.Last.Start(New Object() {posPluto, New PointFD(0, 3.71)})
        'Thread.Sleep(20)

        'Set new random color.
        'myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Add and initialize new star to universe.
        'myUniverse.AddStar(New Star)
        'myUniverse.Stars.Last.Init(myUniverse, pos2, starRadius, starBorderWidth, myBrush.Color, starType, 30, 0)
        'AddObjStatsLabel(myUniverse.Stars.Last)
        'Set new random color.
        'myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Add and initialize new star to universe.
        'myUniverse.AddStar(New Star)
        'myUniverse.Stars.Last.Init(myUniverse, defaultUniverseMatrix, pos3, starRadius, starBorderWidth, myBrush.Color, starType, New PointFD(0, -5))
        'AddListItem(myUniverse.Stars.Last, myUniverse.Objects)

        'Set new random color.
        'myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Add and initialize new star to universe.
        'myUniverse.AddStar(New Star)
        'myUniverse.Stars.Last.Init(myUniverse, pos4, starRadius, starBorderWidth, myBrush.Color, starType, -30, 0)
        'AddObjStatsLabel(myUniverse.Stars.Last)
        'While myUniverse.getStars.Count > 0
        '    For Each star In myUniverse.getStars
        '        If star.IsMerged = False Then
        '            star.applyAcceleration(myUniverse.getStars, myUniverse.getplanets, myUniverse.getGravityConstant)
        '            Thread.Sleep(universeTick.Value)
        '        End If
        '    Next
        'End While

    End Sub
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Paint events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        e.Graphics.DrawImage(myUniverse.getImage, New Point(0, 0)) 'Draw universe.
        e.Graphics.DrawString(FPS.ToString, Me.Font, Brushes.White, defaultUniverseWidth - defaultClipOffset, defaultClipOffset + 10) 'Draw FPS.
    End Sub
    Private Sub PaintUniverseObjects(ByVal universeGraphics As Graphics)

        paintingStars = True
        paintingPlanets = True
        'debug_stopwatch = Stopwatch.StartNew()

        Dim objList As New List(Of StellarObject)


        Try
            objList.AddRange(myUniverse.Objects.OrderBy(Function(o) o.CenterOfMass.Z))

            '' Create pen.
            'Dim blackPen As New Pen(Color.White, 3)

            '' Create rectangle to bound ellipse.
            'Dim rect As New Rectangle(120, 120, 100, 100)

            '' Create start and sweep angles on ellipse.
            'Dim startAngle As Single = 0
            'Dim sweepAngle As Single = 1

            '' Draw arc to screen.
            'universeGraphics.DrawArc(blackPen, rect, startAngle, sweepAngle)

            'Check if we 're hovering above stellar object.
            ObjectUnderMouse(objList)

            'If the offset is changed due to zooming/dragging, set the new offset for all stellar objects and check if they are still visible.
            If resetOffset Then

                myUniverse.ResetAllOffsets(objList) 'Start resetting.
                resetOffset = False

            End If

            For Each obj In objList.FindAll(Function(o) o.IsMerged = False And (o.isVisible Or o.IsSelected) And
                                                        o.CosmosSection = currentCosmosSection)

                obj.Paint(universeGraphics, zoomValue)

                'Selected object is always drawn, even if it's not visible.
                If obj.IsSelected Then

                    'Draw selection shape, if necessary.
                    obj.PaintSelectionShape(universeGraphics, zoomValue)

                    'Follow object, if said so.
                    FollowObject(followingObject)

                    'Hide label, if said so.
                    If hideHoverInfo Then
                        Me.Invoke(New hideHoverLabelDelegate(AddressOf hideHoverLabel))
                    Else
                        Me.Invoke(New showHoverLabelDelegate(AddressOf showHoverLabel), New Object() {obj})
                    End If

                ElseIf selectedObject Is Nothing And hoverObject IsNot Nothing Then
                    Me.Invoke(New showHoverLabelDelegate(AddressOf showHoverLabel), New Object() {hoverObject})
                End If

                'Tunneling.
                If radCollisionTunnel.Checked And Not dragStart Then
                    If obj.Type = StellarObjectType.Planet Then
                        Planet_CheckForLeftTunneling(obj, obj.CenterOfMass, universeGraphics)
                        Planet_CheckForRightTunneling(obj, obj.CenterOfMass, universeGraphics)
                        Planet_CheckForTopTunneling(obj, obj.CenterOfMass, universeGraphics)
                        Planet_CheckForBottomTunneling(obj, obj.CenterOfMass, universeGraphics)
                    ElseIf obj.type = StellarObjectType.Star Then
                        Star_CheckForLeftTunneling(obj, obj.CenterOfMass, universeGraphics)
                        Star_CheckForRightTunneling(obj, obj.CenterOfMass, universeGraphics)
                        Star_CheckForTopTunneling(obj, obj.CenterOfMass, universeGraphics)
                        Star_CheckForBottomTunneling(obj, obj.CenterOfMass, universeGraphics)
                    End If
                End If

            Next

            'Gravitational lensing for black holes.
            'myUniverse.GravityLensEffect(objList)
            myUniverse.SemiRealGravityLensEffect(objList)

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        'debug_stopwatch.Stop()
        'Console.WriteLine("  Painted stars in: " + debug_stopwatch.ElapsedMilliseconds.ToString)
        paintingStars = False
        paintingPlanets = False

    End Sub
    Private Sub picMinimap_Paint(sender As Object, e As PaintEventArgs) Handles picMinimap.Paint
        Try
            e.Graphics.DrawImage(myMinimap.getImage, New Point(0, 0)) 'Draw minimap.
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Private Sub Frame()

        Dim universeGraphics As Graphics = myUniverse.getGraphics
        Dim minimapGraphics As Graphics = myMinimap.getGraphics

        onFrame = True

        'While 1

        'Don't paint objects while zooming/moving/creating to avoid "object in use" exceptions.
        If zooming Or dragging Or StarArrayInUseFlag Or PlanetArrayInUseFlag Then
            Exit Sub
        End If

        'Update world.
        Try
            'Clear universe, if said so.
            If chkCanvasClear.Checked Then
                universeGraphics.Clear(Color.Black)
            End If
            minimapGraphics.Clear(Color.Black) 'Always clear minimap.

            'Draw objects.
            PaintUniverseObjects(universeGraphics)

            'Draw selection rectangle, if selecting.
            If isSelecting Then

                realSelectionRectangle = selectionRectangle 'Our actual selection rectangle.

                'If the direction is not right-bottom, we must move location of the rectangle.
                If selectionDist.X < 0 Then
                    realSelectionRectangle.X = selectionRectangle.Location.X + selectionDist.X
                End If
                If selectionDist.Y < 0 Then
                    realSelectionRectangle.Y = selectionRectangle.Location.Y + selectionDist.Y
                End If

                myUniverse.getGraphics.DrawRectangle(myPen, realSelectionRectangle.Location.X,
                                                            realSelectionRectangle.Location.Y,
                                                            realSelectionRectangle.Width, realSelectionRectangle.Height)

                'Reset selection if mouse is released.
                If Not mouseIsDown Then
                    selectionRectangle.Location = New Point(0, 0)
                    selectionRectangle.Size = New SizeF(0, 0)
                    realSelectionRectangle = selectionRectangle
                End If

            End If

            'Update minimap when not navigating.
            If Not navigatingMinimap Then
                myMinimap.Update(universeGraphics.Transform.Clone, myUniverse.Objects.
                                                                   FindAll(Function(o) o.CosmosSection = currentCosmosSection), 'o.isVisible
                                 New Double() {-myUniverse.SectionWidth / 2, -myUniverse.SectionHeight / 2,
                                                myUniverse.SectionWidth, myUniverse.SectionHeight},
                                 New Double() {clipOffsetX, clipOffsetY, universeWidth, universeHeight})
            End If

            'Update images.
            Control_Invalidate(Me, picUniverse.Region)
            Control_Invalidate(picMinimap, picMinimap.Region)

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        onFrame = False

        'Wait a little to avoid flickering
        'Thread.Sleep(15)

        'End While

    End Sub
    Private Sub ResetFPS()

        FPS = frameCounter 'Set total frames per second.
        frameCounter = 0 'Reset fps counter.

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Reset events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub ResetUniverse()

        UniversePaused = False
        btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)
        Timer1.Enabled = False

        For Each thread In threadList
            thread.Abort()
        Next
        threadList.RemoveRange(0, threadList.Count)

        numParamXVel.Value = 0
        numParamYVel.Value = 0
        numTimeTick.Value = 1

        objectListView.Items.Clear()

        myUniverse.Stars.RemoveRange(0, myUniverse.Stars.Count)
        myUniverse.Planets.RemoveRange(0, myUniverse.Planets.Count)

        chkbNoCreation.Checked = False
        radCollisionTunnel.Checked = True
        radRealTraj.Checked = True
        chkFollowSelected.Checked = False
        chkHideInfo.Checked = False
        chkCanvasClear.Checked = True
        chkDrawTrajectories.Checked = True
        numTraj.Value = 100

        'Reset universe matrices.
        myUniverse.getGraphics.ResetTransform()
        myUniverseMatrix = defaultUniverseMatrix.Clone
        myInverseUniverseMatrix = myUniverseMatrix.Clone
        myInverseUniverseMatrix.Invert()

        'Reset universe size.
        universeWidth = defaultUniverseWidth
        universeHeight = defaultUniverseHeight

        'Reset clip offset.
        clipOffsetX = defaultClipOffset
        clipOffsetY = defaultClipOffset

        zoomValue = 1.0 'Reset zoom level.
        totalDragOffset = New PointF(0, 0) 'Reset total drag.

        InitWorld(True) 'Init world again.

        UniversePaused = True
        btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Tunneling events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub Star_CheckForLeftTunneling(ByVal star As Star, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        Dim radius As Integer = star.Radius

        'Left tunneling.
        If center.X - radius < clipOffsetX And Not star.TransitionDirection.Contains("r") Then

            'Add transition direction.
            If Not star.TransitionDirection.Contains("l") Then
                star.TransitionDirection = star.TransitionDirection + "l"
            End If

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(universeWidth - radius + center.X - clipOffsetX, center.Y - radius)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            star.DuplicatePointLeft = duplicatePoint 'Set star duplicate point.

        End If

        'Move the star, but just once.
        If center.X < clipOffsetX Then
            star.Move(0, "", center.X + universeWidth - clipOffsetX, center.Y)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If center.X > universeWidth - radius And star.TransitionDirection.Contains("l") Then

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint As New PointF(clipOffsetX - radius - universeWidth + center.X, center.Y - radius)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            star.DuplicatePointLeft = duplicatePoint 'Set star duplicate point.

        ElseIf star.isVisibleX Then 'If inside X visible area.

            If star.TransitionDirection.Contains("l") Then
                star.TransitionDirection = star.TransitionDirection.Remove(star.TransitionDirection.IndexOf("l"), 1) 'Delete transition direction.
            End If

        End If

    End Sub
    Private Sub Star_CheckForRightTunneling(ByVal star As Star, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        Dim radius As Single = star.Radius

        'Right tunneling.
        If center.X + radius > universeWidth And Not star.TransitionDirection.Contains("l") Then

            'Add transition direction.
            If Not star.TransitionDirection.Contains("r") Then
                star.TransitionDirection = star.TransitionDirection + "r"
            End If

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(clipOffsetX - radius + center.X - universeWidth, center.Y - radius)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            star.DuplicatePointRight = duplicatePoint 'Set star duplicate point.
        End If

        'Move the star, but just once.
        If center.X > universeWidth Then
            star.Move(0, "", center.X - universeWidth + clipOffsetX, center.Y)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If center.X < clipOffsetX + radius And star.TransitionDirection.Contains("r") Then

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint As New PointF(universeWidth - radius - clipOffsetX + center.X, center.Y - radius)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            star.DuplicatePointRight = duplicatePoint 'Set star duplicate point.

        ElseIf star.isVisibleX Then 'If inside X visible area.

            If star.TransitionDirection.Contains("r") Then
                star.TransitionDirection = star.TransitionDirection.Remove(star.TransitionDirection.IndexOf("r"), 1) 'Delete transition direction.
            End If

        End If


    End Sub
    Private Sub Star_CheckForTopTunneling(ByVal star As Star, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        Dim radius As Integer = star.Radius

        'Top tunneling.
        If center.Y - radius < clipOffsetY And Not star.TransitionDirection.Contains("b") Then

            'Add transition direction.
            If Not star.TransitionDirection.Contains("t") Then
                star.TransitionDirection = star.TransitionDirection + "t"
            End If

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint = New Point(center.X - radius, universeHeight + center.Y - clipOffsetY - radius)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            'If the star is also tunneling to the left, draw a second duplicate in the bottom-right corner.
            If center.X - radius < clipOffsetX Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New Point(universeWidth + duplicatePoint.X - clipOffsetX, duplicatePoint.Y)

                'Create second duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)

            ElseIf center.X + radius > universeWidth Then 'Else if the star is also tunneling to the right, draw a second duplicate in the bottom-left corner.

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New Point(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)

                'Create second duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)

            End If

            star.DuplicatePointTop = duplicatePoint 'Set star duplicate point.

        End If

        'Move the star, but just once.
        If center.Y < clipOffsetY Then
            star.Move(0, "", center.X, center.Y + universeHeight - clipOffsetY)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If center.Y > universeHeight - radius And star.TransitionDirection.Contains("t") Then

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint = New Point(center.X - radius, center.Y - universeHeight + clipOffsetY - radius)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            'If the star is also tunneling to the left, draw a second duplicate in the top-right corner.
            If center.X - radius < clipOffsetX Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New Point(universeWidth + duplicatePoint.X - clipOffsetX, duplicatePoint.Y)

                'Create second duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)

            ElseIf center.X + radius > universeWidth Then  'Else if the star is also tunneling to the right, draw a second duplicate in the top-left corner.

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New Point(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)

                'Create second duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)

            End If

            star.DuplicatePointTop = duplicatePoint 'Set star duplicate point.

        ElseIf star.isVisibleY Then 'If inside Y visible area.

            If star.TransitionDirection.Contains("t") Then
                star.TransitionDirection = star.TransitionDirection.Remove(star.TransitionDirection.IndexOf("t"), 1) 'Delete transition direction.
            End If

        End If

    End Sub
    Private Sub Star_CheckForBottomTunneling(ByVal star As Star, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        Dim radius As Integer = star.Radius

        'Bottom tunneling.
        If center.Y + radius > universeHeight And Not star.TransitionDirection.Contains("t") Then

            'Add transition direction.
            If Not star.TransitionDirection.Contains("b") Then
                star.TransitionDirection = star.TransitionDirection + "b"
            End If

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(center.X - radius, clipOffsetY - radius + center.Y - universeHeight)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            'If the star is also tunneling to the left, draw a second duplicate in the top-right corner.
            If center.X - radius < clipOffsetX Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(universeWidth + duplicatePoint.X - clipOffsetX, duplicatePoint.Y)

                'Create second duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)

            ElseIf center.X + radius > universeWidth Then 'Else if the star is also tunneling to the right, draw a second duplicate in the top-left corner.

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)

                'Create second duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)

            End If

            star.DuplicatePointBottom = duplicatePoint 'Set star duplicate point.

        End If

        'Move the star, but just once.
        If center.Y > universeHeight Then
            star.Move(0, "", center.X, center.Y - universeHeight + clipOffsetY)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If center.Y < clipOffsetY + radius And star.TransitionDirection.Contains("b") Then

            Dim newPen As New Pen(star.PaintColor)

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(center.X - radius, universeHeight - radius + center.Y - clipOffsetY)

            'Create duplicate ellipse for transition purposes.
            universeGraphics.DrawEllipse(newPen, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)
            universeGraphics.FillEllipse(newPen.Brush, duplicatePoint.X, duplicatePoint.Y, radius * 2, radius * 2)

            'If the duplicate is also tunneling to the left, draw a second duplicate in the bottom-right corner.
            If center.X - radius < clipOffsetX Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(universeWidth + duplicatePoint.X - clipOffsetX, duplicatePoint.Y)

                'Create duplicate of duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)

            ElseIf center.X + radius > universeWidth Then 'Else if the star is also tunneling to the right, draw a second duplicate in the bottom-left corner.

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)

                'Create duplicate of duplicate ellipse for corner transition purposes.
                universeGraphics.DrawEllipse(newPen, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
                universeGraphics.FillEllipse(newPen.Brush, secondDuplicatePoint.X, secondDuplicatePoint.Y, radius * 2, radius * 2)
            End If

            star.DuplicatePointBottom = duplicatePoint 'Set star duplicate point.

        ElseIf star.isVisibleY Then 'If inside Y visible area.

            If star.TransitionDirection.Contains("b") Then
                star.TransitionDirection = star.TransitionDirection.Remove(star.TransitionDirection.IndexOf("b"), 1) 'Delete transition direction.
            End If

        End If

    End Sub

    Private Sub Planet_CheckForLeftTunneling(ByVal planet As Planet, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        'Remember to add the border width in all of the planet the calculations.

        Dim planetHalfWidth As Double = planet.GetHalfSize

        'Left tunneling.
        If planet.Vertices(0).X < clipOffsetX + planetBorderWidth And Not planet.TransitionDirection.Contains("r") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("l") Then
                planet.TransitionDirection = planet.TransitionDirection + "l"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.Vertices(0).X + universeWidth - clipOffsetX, planet.Vertices(0).Y)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            planet.DuplicatePointLeft = duplicatePoint 'Set planet duplicate point.
        End If

        'Move the planet, but just once.
        If center.X < clipOffsetX Then
            planet.Move(0, "", center.X + universeWidth - clipOffsetX, center.Y)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If planet.Vertices(0).X > universeWidth - planetBorderWidth - planetSize And planet.TransitionDirection.Contains("l") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint As PointF = New PointF(planet.Vertices(0).X - universeWidth + clipOffsetX, planet.Vertices(0).Y)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            planet.DuplicatePointLeft = duplicatePoint 'Set planet duplicate point.

        ElseIf planet.isVisibleX Then 'If inside X visible area.

            If planet.TransitionDirection.Contains("l") Then
                planet.TransitionDirection = planet.TransitionDirection.Remove(planet.TransitionDirection.IndexOf("l"), 1) 'Delete transition direction.
            End If

        End If

    End Sub
    Private Sub Planet_CheckForRightTunneling(ByVal planet As Planet, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        Dim planetHalfWidth As Double = planet.GetHalfSize

        'Right tunneling.
        If planet.Vertices(0).X > universeWidth - planetBorderWidth - planetSize And Not planet.TransitionDirection.Contains("l") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("r") Then
                planet.TransitionDirection = planet.TransitionDirection + "r"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.Vertices(0).X - universeWidth + clipOffsetX, planet.Vertices(0).Y)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            planet.DuplicatePointRight = duplicatePoint 'Set planet duplicate point.
        End If

        'Move the planet, but just once.
        If center.X > universeWidth Then
            planet.Move(0, "", center.X - universeWidth + clipOffsetX, center.Y)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If planet.Vertices(0).X < clipOffsetX + planetBorderWidth And planet.TransitionDirection.Contains("r") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(planet.Vertices(0).X + universeWidth - clipOffsetX, planet.Vertices(0).Y)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics)  'Draw duplicate planet.

            planet.DuplicatePointRight = duplicatePoint 'Set planet duplicate point.

        ElseIf planet.isVisibleX Then 'If inside X visible area.

            If planet.TransitionDirection.Contains("r") Then
                planet.TransitionDirection = planet.TransitionDirection.Remove(planet.TransitionDirection.IndexOf("r"), 1) 'Delete transition direction.
            End If

        End If


    End Sub
    Private Sub Planet_CheckForTopTunneling(ByVal planet As Planet, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        Dim planetHalfWidth As Double = planet.GetHalfSize

        'Top tunneling.
        If planet.Vertices(0).Y < clipOffsetY + planetBorderWidth And Not planet.TransitionDirection.Contains("b") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("t") Then
                planet.TransitionDirection = planet.TransitionDirection + "t"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.Vertices(0).X, planet.Vertices(0).Y + universeHeight - clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the bottom-right corner.
            If planet.Vertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            ElseIf planet.Vertices(0).X > universeWidth - planetBorderWidth - planetSize Then 'Else if the planet is also tunneling to the right, draw a second duplicate in the bottom-left corner.

                'Create duplicate of duplicate planet for corner transition purposes.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            End If

            planet.DuplicatePointTop = duplicatePoint 'Set planet duplicate point.
        End If

        'Move the planet, but just once.
        If center.Y < clipOffsetY Then
            planet.Move(0, "", center.X, center.Y + universeHeight - clipOffsetY)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If planet.Vertices(0).Y > universeHeight - planetBorderWidth - planetSize And planet.TransitionDirection.Contains("t") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(planet.Vertices(0).X, planet.Vertices(0).Y - universeHeight + clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the top-right corner.
            If planet.Vertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(planet.Vertices(0).X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            ElseIf planet.Vertices(0).X > universeWidth - planetBorderWidth - planetSize Then  'Else if the planet is also tunneling to the right, draw a second duplicate in the top-left corner.

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics)

            End If

            planet.DuplicatePointTop = duplicatePoint 'Set planet duplicate point.

        ElseIf planet.isVisibleY Then 'If inside Y visible area.

            If planet.TransitionDirection.Contains("t") Then
                planet.TransitionDirection = planet.TransitionDirection.Remove(planet.TransitionDirection.IndexOf("t"), 1) 'Delete transition direction.
            End If
        End If
    End Sub
    Private Sub Planet_CheckForBottomTunneling(ByVal planet As Planet, ByVal center As PointFD, ByVal universeGraphics As Graphics)

        Dim planetHalfWidth As Double = planet.GetHalfSize

        'Bottom tunneling.
        If planet.Vertices(0).Y > universeHeight - planetBorderWidth - planetSize And Not planet.TransitionDirection.Contains("t") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("b") Then
                planet.TransitionDirection = planet.TransitionDirection + "b"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.Vertices(0).X, planet.Vertices(0).Y - universeHeight + clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the bottom-right corner.
            If planet.Vertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            ElseIf planet.Vertices(0).X > universeWidth - planetBorderWidth - planetSize Then 'Else if the planet is going through the bottom-right corner, draw duplicate on top-right corner.

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            End If

            planet.DuplicatePointBottom = duplicatePoint 'Set planet duplicate point.
        End If

        'Move the planet, but just once.
        If center.Y > universeHeight Then
            planet.Move(0, "", center.X, center.Y - universeHeight + clipOffsetY)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If planet.Vertices(0).Y < clipOffsetY + planetBorderWidth And planet.TransitionDirection.Contains("b") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(planet.Vertices(0).X, planet.Vertices(0).Y + universeHeight - clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the top-right corner.
            If planet.Vertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics)

            ElseIf planet.Vertices(0).X > universeWidth - planetBorderWidth - planetSize Then 'Else if the planet is also tunneling to the right, draw a second duplicate in the top-left corner.

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X - universeWidth + clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics)

            End If

            planet.DuplicatePointBottom = duplicatePoint 'Set planet duplicate point.

        ElseIf planet.isVisibleY Then 'If inside Y visible area.

            If planet.TransitionDirection.Contains("b") Then
                planet.TransitionDirection = planet.TransitionDirection.Remove(planet.TransitionDirection.IndexOf("b"), 1) 'Delete transition direction.
            End If

        End If

    End Sub
    Private Sub DrawDuplicatePlanet(ByVal startingVertex As PointF, ByVal planetPen As Pen, ByVal universeGraphics As Graphics)

        'Draw rectangle instead of lines.
        universeGraphics.DrawRectangle(planetPen, startingVertex.X, startingVertex.Y, planetSize, planetSize)

        'Dim duplicatePlanetVertices As New List(Of PointF)

        ''Initialize vertices for square.
        'duplicatePlanetVertices.Add(startingVertex)
        'duplicatePlanetVertices.Add(New PointF(startingVertex.X + planetSize, startingVertex.Y))
        'duplicatePlanetVertices.Add(New PointF(startingVertex.X + planetSize, startingVertex.Y + planetSize))
        'duplicatePlanetVertices.Add(New PointF(startingVertex.X, startingVertex.Y + planetSize))

        ''Draw square.
        'For i = 0 To duplicatePlanetVertices.Count - 1
        '    If i = duplicatePlanetVertices.Count - 1 Then
        '        universeGraphics.DrawLine(planePen, duplicatePlanetVertices(i), duplicatePlanetVertices(0)) 'Draw last line.
        '    Else
        '        universeGraphics.DrawLine(planePen, duplicatePlanetVertices(i), duplicatePlanetVertices(i + 1)) 'Draw line.
        '    End If
        'Next

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Object Creation events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub CreateStar(ByVal data() As Object)

        Dim newstar As New Star

        While paintingStars Or StarArrayInUseFlag 'Wait until star list is free.

        End While

        StarArrayInUseFlag = True

        Dim solarMass As Double

        If data.Count < 3 Then
            solarMass = starSolarMass
        Else
            solarMass = data(2)
        End If

        'Set new random color.
        myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))
        'myBrush = New SolidBrush(Color.FromArgb(220, 253, 219))

        'Initialize and add new star to the universe.
        newstar.Init(myUniverse, myUniverseMatrix, data(0), starRadius, solarMass, starBorderWidth, myBrush.Color, data(1))
        newstar.CosmosSection = currentCosmosSection

        myUniverse.AddStar(newstar)
        AddListItem(newstar, myUniverse.Objects)

        StarArrayInUseFlag = False

        'While not merged. Calculate acceleration from stars.
        'While (Not newstar.ismerged)

        '    Dim cloneList As New List(Of Star)
        '    cloneList = myUniverse.getStars.ToArray().ToList

        '    newstar.applyAcceleration(cloneList, myUniverse.getplanets, myUniverse.getGravityConstant)
        '    If bounceMode.Checked Then
        '        newstar.CheckForBounce()
        '    End If

        '    Thread.Sleep(universeTick.Value)
        'End While

        'If it's merged remove thread from list and terminate.
        threadList.Remove(Thread.CurrentThread)
        Thread.CurrentThread.Abort()

    End Sub
    Private Sub CreatePlanet(ByVal data() As Object)

        Dim newplanet As New Planet

        While paintingPlanets Or PlanetArrayInUseFlag  'Wait until planet list is free.

        End While

        PlanetArrayInUseFlag = True

        Dim earthMass As Double

        If data.Count < 3 Then
            earthMass = planetEarthMass
        Else
            earthMass = data(2)
        End If

        'Initialize and add new planet to planet World.
        newplanet.Init(myUniverse, myUniverseMatrix, data(0), planetSize, earthMass, planetBorderWidth, planetColor, data(1))
        newplanet.CosmosSection = currentCosmosSection

        myUniverse.AddPlanet(newplanet)
        AddListItem(newplanet, myUniverse.Objects)

        PlanetArrayInUseFlag = False

        'While not merged. Calculate acceleration from stars.
        'While (Not newplanet.ismerged)

        '    newplanet.applyAcceleration(myUniverse.getStars, myUniverse.getplanets, myUniverse.getGravityConstant)
        '    If bounceMode.Checked Then
        '        newplanet.CheckForBounce()
        '    End If

        '    Thread.Sleep(universeTick.Value)
        'End While

        'If it's merged remove thread from list and terminate.
        threadList.Remove(Thread.CurrentThread)
        Thread.CurrentThread.Abort()

    End Sub
    Private Sub CreateBlackHole(ByVal data() As Object)

        Dim newBlackHole As New Singularity

        While paintingStars Or StarArrayInUseFlag  'Wait until painting is done.

        End While

        StarArrayInUseFlag = True

        Dim starMass As Double

        If data.Count < 3 Then
            starMass = starSolarMass
        Else
            starMass = data(2)
        End If

        'Initialize and add new planet to planet World.
        newBlackHole.Init(myUniverse, myUniverseMatrix, data(0), starRadius, starMass, data(1))
        newBlackHole.CosmosSection = currentCosmosSection

        myUniverse.AddBlackHole(newBlackHole)
        AddListItem(newBlackHole, myUniverse.Objects)

        StarArrayInUseFlag = False

        'While not merged. Calculate acceleration from stars.
        'While (Not newplanet.ismerged)

        '    newplanet.applyAcceleration(myUniverse.getStars, myUniverse.getplanets, myUniverse.getGravityConstant)
        '    If bounceMode.Checked Then
        '        newplanet.CheckForBounce()
        '    End If

        '    Thread.Sleep(universeTick.Value)
        'End While

        'If it's merged remove thread from list and terminate.
        threadList.Remove(Thread.CurrentThread)
        Thread.CurrentThread.Abort()

    End Sub
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Timer events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        'Start counting.
        If mouseIsDown And Not isSelecting Then
            counter += 1
        Else
            'Update coordinates while moving.
            If followingObject Then
                Dim s As New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) 'Init empty mouse event.
                Me.OnMouseMove(s) 'Update UI points.
            End If

            updateCounter += 1

            'Wait until we finished calculating the forces or at least 3ms.
            If updateCounter >= 3 Then

                If Not UniversePaused And Not onLive Then
                    UpdateLabels()
                End If

                If Not onLive Then
                    updateCounter = 0
                End If
            End If

        End If

    End Sub
    Private Sub UpdateLabels()

        Dim objList As New List(Of StellarObject)
        objList.AddRange(myUniverse.Objects.FindAll(Function(o) o.CosmosSection = currentCosmosSection))

        If objList.Count > 0 Then

            Dim removedListItems As New List(Of Integer) 'List of list item indexes to be removed.

            For Each item In objectListView.Items

                'Get stellar object of list item.
                Dim obj As StellarObject = objList.Find(Function(o) o.ListItem.Equals(item))

                'If stellar object is not found, that means it got merged. Remove item from list later.
                If obj Is Nothing Then
                    removedListItems.Add(objectListView.Items.IndexOf(item))
                    Continue For
                End If

                UpdateListItem(obj, item)

            Next

            'Start in reverse order to keep the indexes intact.
            removedListItems.Reverse()

            'Remove merged items.
            For Each item In removedListItems
                objectListView.Items.RemoveAt(item)
            Next

        End If

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Stats label events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Delegate Sub AddListItemDelegate(ByVal obj As StellarObject, ByVal objList As List(Of StellarObject))
    Private Sub AddListItem(ByVal obj As StellarObject, ByVal objList As List(Of StellarObject))

        If objectListView.InvokeRequired Then
            Try
                objectListView.Invoke(New AddListItemDelegate(AddressOf AddListItem), New Object() {obj, objList})
            Catch ex As Exception
                End
            End Try
        Else

            Dim objectItem As ListViewItem = obj.ListItem
            Dim brightness As Single = obj.ListItem.BackColor.GetBrightness
            objectItem.BackColor = obj.ListItem.BackColor

            If brightness > 0.4 AndAlso Not obj.Type.Equals(StellarObjectType.BlackHole) Then
                objectItem.ForeColor = Color.Black
            Else
                objectItem.ForeColor = Color.White
            End If

            'Get group to assign the object to.
            Dim group As String
            Dim name As String

            group = obj.Type.ToString + "s"
            name = obj.Type.ToString
            'group = "Default"
            'name = ""

            'Get index of item.
            Dim index As Integer = objectListView.Groups(group).Items.Count
            objectItem.Name = name + index.ToString
            objectItem.Text = objectItem.Name

            'Now add sub-items, one for each statistic (location, acceleration, etc.) we want to display.
            'The name is displayed in the first (0) item.
            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemLocX"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemLocY"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemLocZ"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemAcc"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemAccX"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemAccY"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemAccZ"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemVel"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemVelX"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemVelY"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemVelZ"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemMass"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(objectItem.SubItems.Count - 1).Name = "objItemSize"

            objectItem.Group = objectListView.Groups.Item(group)

            obj.ListItem = objectItem 'Update item in the object only once.
            objectListView.Items.Add(objectItem)  'Insert item in the list.

        End If

    End Sub
    Private Delegate Sub UpdateListItemDelegate(ByVal star As Star, ByVal listItem As ListViewItem)
    Private Sub UpdateListItem(ByVal obj As StellarObject, ByVal objectItem As ListViewItem)

        If objectItem IsNot Nothing Then

            'Convert statistics to strings.
            Dim objLocX As String = Math.Round(obj.CenterOfMass.X, 5).ToString
            Dim objLocY As String = Math.Round(obj.CenterOfMass.Y, 5).ToString
            Dim objLocZ As String = Math.Round(obj.CenterOfMass.Z, 5).ToString
            Dim objAccX As String = obj.AccX.ToString
            Dim objAccY As String = obj.AccY.ToString
            Dim objAccZ As String = obj.AccZ.ToString
            Dim objVelX As String = obj.VelX.ToString
            Dim objVelY As String = obj.VelY.ToString
            Dim objVelZ As String = obj.VelZ.ToString
            Dim objSize As String

            'Add/remove selection suffix.
            If obj.IsSelected And Not objectItem.Text.Contains(" (s)") Then
                objectItem.Text += " (s)"
            ElseIf Not obj.IsSelected And objectItem.Text.Contains(" (s)") Then
                objectItem.Text = objectItem.Text.Remove(objectItem.Text.IndexOf(" (s)"), 4)
            End If

            If obj.Type = StellarObjectType.Planet Then
                objSize = obj.Size.ToString
            Else
                objSize = obj.Radius.ToString
            End If

            Dim brightness As Single = obj.ListItem.BackColor.GetBrightness

            If brightness > 0.4 AndAlso Not obj.Type.Equals(StellarObjectType.BlackHole) Then
                objectItem.ForeColor = Color.Black
            Else
                objectItem.ForeColor = Color.White
            End If

            'Update stats.
            '-------------------------------------------------------------------------------------------------------------
            objectListView.SuspendLayout()

            If objectItem.SubItems.Item("objItemLocX").Text <> objLocX Then
                objectItem.SubItems.Item("objItemLocX").Text = objLocX
            End If
            If objectItem.SubItems.Item("objItemLocY").Text <> objLocY Then
                objectItem.SubItems.Item("objItemLocY").Text = objLocY
            End If
            If objectItem.SubItems.Item("objItemLocZ").Text <> objLocZ Then
                objectItem.SubItems.Item("objItemLocZ").Text = objLocZ
            End If

            If objectItem.SubItems.Item("objItemAcc").Text <> Math.Sqrt(obj.AccX * obj.AccX + obj.AccY * obj.AccY + obj.AccZ * obj.AccZ).ToString Then
                objectItem.SubItems.Item("objItemAcc").Text = Math.Sqrt(obj.AccX * obj.AccX + obj.AccY * obj.AccY + obj.AccZ * obj.AccZ)
            End If
            If objectItem.SubItems.Item("objItemAccX").Text <> objAccX Then
                objectItem.SubItems.Item("objItemAccX").Text = objAccX
            End If
            If objectItem.SubItems.Item("objItemAccY").Text <> objAccY Then
                objectItem.SubItems.Item("objItemAccY").Text = objAccY
            End If
            If objectItem.SubItems.Item("objItemAccZ").Text <> objAccZ Then
                objectItem.SubItems.Item("objItemAccZ").Text = objAccZ
            End If

            If objectItem.SubItems.Item("objItemVel").Text <> Math.Sqrt(obj.VelX * obj.VelX + obj.VelY * obj.VelY + obj.VelZ * obj.VelZ).ToString Then
                objectItem.SubItems.Item("objItemVel").Text = Math.Sqrt(obj.VelX * obj.VelX + obj.VelY * obj.VelY + obj.VelZ * obj.VelZ)
            End If
            If objectItem.SubItems.Item("objItemVelX").Text <> objVelX Then
                objectItem.SubItems.Item("objItemVelX").Text = objVelX
            End If
            If objectItem.SubItems.Item("objItemVelY").Text <> objVelY Then
                objectItem.SubItems.Item("objItemVelY").Text = objVelY
            End If
            If objectItem.SubItems.Item("objItemVelZ").Text <> objVelZ Then
                objectItem.SubItems.Item("objItemVelZ").Text = objVelZ
            End If

            If objectItem.SubItems.Item("objItemMass").Text <> obj.Mass.ToString Then
                objectItem.SubItems.Item("objItemMass").Text = obj.Mass.ToString
            End If
            If objectItem.SubItems.Item("objItemSize").Text <> objSize Then
                objectItem.SubItems.Item("objItemSize").Text = objSize
            End If

            objectListView.ResumeLayout()
            '-------------------------------------------------------------------------------------------------------------

        End If

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Form/click/resize/keydown events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub blockForm_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove

        'Catch movements only on main form and not file dialogs etc.
        If formLoaded And sender Is ActiveForm Then

            mousePoint = PointToClient(MousePosition)
            absoluteMousePoint = mousePoint 'Save absolute point before scaling it.

            'Check always with absolute point.
            'If mouse out of boundaries, do nothing.
            If absoluteMousePoint.X >= imageWidth Or absoluteMousePoint.Y >= imageHeight Or
               absoluteMousePoint.X < defaultClipOffset Or absoluteMousePoint.Y < defaultClipOffset Then

                'lblCoordsMouse.Text = "Out Of Bounds"
                'lblCoordsAbs.Text = "Out Of Bounds"

                'Don't stop selection if cursor is out of bounds.
                If isSelecting Then
                    SetSelectionRectangle()
                End If

                Exit Sub
            Else
                Dim points() As PointF = {mousePoint}

                Try
                    myInverseUniverseMatrix.TransformPoints(points)
                Catch ex As Exception
                    Console.WriteLine(ex.ToString)
                End Try

                mousePoint = points(0)
                mousePoint.X = Math.Round(mousePoint.X, 2)
                mousePoint.Y = Math.Round(mousePoint.Y, 2)

                'Make sure these have "CausesValidation = False" to avoid image flickering when moving the mouse.
                lblCoordsMouse.Text = mousePoint.ToString
                lblCoordsAbs.Text = absoluteMousePoint.ToString

                Dim bh As Singularity = myUniverse.Objects.FindLast(Function(x) x.Type = StellarObjectType.BlackHole)
                If bh IsNot Nothing Then
                    Dim newCOM As New PointFD(mousePoint.X, mousePoint.Y, bh.CenterOfMass.Z)
                    bh.CenterOfMass = newCOM
                End If

            End If

            If Not isSelecting Then
                CheckPlanetContinuousMode(e) 'Check for planet continuous mode creation.
                CheckDragging() 'Check for universe dragging.
            Else
                SetSelectionRectangle()
            End If
            ObjectHover() 'Check for object hover.

        End If

    End Sub
    Private Sub picMinimap_MouseMove(sender As Object, e As MouseEventArgs) Handles picMinimap.MouseMove

        mousePoint = picMinimap.PointToClient(MousePosition)

        'Convert minimap coordinates to the corresponding section coordinates.
        Dim x As Double = (Math.Abs(mousePoint.X * (myUniverse.SectionWidth + 1))) /
                                    (myMinimap.ImageWidth) - myUniverse.SectionWidth / 2
        Dim y As Double = (Math.Abs(mousePoint.Y * (myUniverse.SectionHeight + 1))) /
                                    (myMinimap.ImageHeight) - myUniverse.SectionHeight / 2

        'Save new point.
        minimapPoint = New PointF(x, y)

        lblCoordsMouse.Text = minimapPoint.ToString
        lblCoordsAbs.Text = mousePoint.ToString

        'Enable dragging on minimap.
        If mouseIsDown And picMinimap.ClientRectangle.Contains(e.Location) Then

            If UniversePaused = False Then
                UniversePaused = True
                UniversePausedForDragging = True
            End If

            minimapDragging = True
            picMinimap_Click(sender, e)


        End If

    End Sub
    Private Sub blockForm_Click(sender As Object, e As EventArgs) Handles Me.Click

        'Do nothing until offset changes when dragging.
        If dragStart Then
            Exit Sub
        End If


        If creationMode.Equals("p") And Not hover Then

            'Make sure this code is updated from the planet class.
            Dim VisualSize As Double = planetSize + (zAxisMultiplier * numParamZPos.Value) / planetSize
            If VisualSize < 4 Then
                VisualSize = 4
            End If

            'If planet size exceeds boundaries, don't create anything.
            If mousePoint.X + VisualSize / 2 + planetBorderWidth / 2 > universeWidth Or
                mousePoint.Y + VisualSize / 2 + planetBorderWidth / 2 > universeHeight Or
                mousePoint.X - VisualSize / 2 - planetBorderWidth / 2 < clipOffsetX - 1 Or
                mousePoint.Y - VisualSize / 2 - planetBorderWidth / 2 < clipOffsetY - 1 Then

                Exit Sub
            End If

            threadList.Add(New Thread(AddressOf createplanet)) 'Each planet is handled by a different thread.
            threadList.Last.Start(New Object() {New PointFD(mousePoint.X, mousePoint.Y, zAxisMultiplier * numParamZPos.Value),
                                  New PointFD(numParamXVel.Value, numParamYVel.Value, numParamZVel.Value), numParamMass.Value}) 'Start thread.

        ElseIf creationMode.Equals("s") And Not hover Then

            'Make sure this code is updated from the star class.
            Dim VisualSize As Double = starRadius + (zAxisMultiplier * numParamZPos.Value) / starRadius
            If VisualSize < 4 Then
                VisualSize = 4
            End If

            'If star size exceeds boundaries.
            If mousePoint.X + VisualSize / 2 + starBorderWidth / 2 > universeWidth Or
                mousePoint.Y + VisualSize / 2 + starBorderWidth / 2 > universeHeight Or
                mousePoint.X - VisualSize / 2 - starBorderWidth / 2 < clipOffsetX Or
                mousePoint.Y - VisualSize / 2 - starBorderWidth / 2 < clipOffsetY Then

                Exit Sub
            End If

            threadList.Add(New Thread(AddressOf createstar)) 'Each star is handled by a different thread.
            threadList.Last.Start(New Object() {New PointFD(mousePoint.X, mousePoint.Y, zAxisMultiplier * numParamZPos.Value),
                                  New PointFD(numParamXVel.Value, numParamYVel.Value, numParamZVel.Value), numParamMass.Value}) 'Start thread.

        ElseIf creationMode.Equals("b") And Not hover Then

            'Make sure this code is updated from the star class.
            Dim VisualSize As Double = starRadius + (zAxisMultiplier * numParamZPos.Value) / starRadius
            If VisualSize < 4 Then
                VisualSize = 4
            End If

            'If star size exceeds boundaries.
            If mousePoint.X + VisualSize / 2 > universeWidth Or
                mousePoint.Y + VisualSize / 2 > universeHeight Or
                mousePoint.X - VisualSize / 2 < clipOffsetX Or
                mousePoint.Y - VisualSize / 2 < clipOffsetY Then

                Exit Sub
            End If

            threadList.Add(New Thread(AddressOf CreateBlackHole)) 'Each star is handled by a different thread.
            threadList.Last.Start(New Object() {New PointFD(mousePoint.X, mousePoint.Y, zAxisMultiplier * numParamZPos.Value),
                                  New PointFD(numParamXVel.Value, numParamYVel.Value, numParamZVel.Value), numParamMass.Value}) 'Start thread.

        ElseIf selectionRectangle.Location.IsEmpty Then
            CheckObjectSelection()
        End If

        picUniverse.Focus()

    End Sub
    Private Sub picMinimap_Click(sender As Object, e As EventArgs) Handles picMinimap.Click

        Dim universeCenter As New PointF((universeWidth + clipOffsetX) / 2 - 1, (universeHeight + clipOffsetY) / 2 - 1)

        'Center camera around clicked point on minimap.
        moveUniverse(minimapPoint, universeCenter, True)

    End Sub
    Private Sub blockForm_MouseWheel(sender As Object, e As MouseEventArgs) Handles Me.MouseWheel

        'If mouse out of boundaries, do nothing.
        If absoluteMousePoint.X >= imageWidth Or absoluteMousePoint.Y >= imageHeight Or
           absoluteMousePoint.X < defaultClipOffset Or absoluteMousePoint.Y < defaultClipOffset Or
           onFrame Then

            Exit Sub

        End If

        zooming = True

        'Zoom IN/OUT. 

        'Because each zoom is applied to the previous zoomed matrix we need to adjust the new zoom value to match what it would be,
        'if it followed the step. If we want to zoom in with a step of 0.25, then it would be like this:
        ' 1 -> 1.25 -> 1.5 -> ..... But because the zoom is applied to the already zoomed matrix it becomes like this:
        ' 1 -> 1.25 -> 1.875 -> ..... As you can see, it multiplied 1.25 x 1.5 = 1.875, that's not how it would go based on the previous statement.
        'So we figure out what values need to be multiplied with the current zoomed matrix to match the next appropriate step.
        'We need: 1.25 * X = 1.5 => X = 1.5/1.25 => newValue = appropriateValue / previousZoomValue
        Try
            If e.Delta = 120 And zoomValue < 10.0 Then

                zoomValue += zoomStep 'Increment by one step. Zoom IN.
                zoomValue = Math.Round(zoomValue, 5) 'Round value in case of trailing 0-9.
                zoomUniverse(1)

                Dim s As New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) 'Init empty mouse event.
                Me.OnMouseMove(s) 'Update UI points.

            ElseIf e.Delta = -120 And zoomValue > -10 Then

                zoomValue -= zoomStep 'Decrement by one step. Zoom OUT.
                zoomValue = Math.Round(zoomValue, 5) 'Round value in case of trailing 0-9.
                zoomUniverse(0)

                Dim s As New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) 'Init empty mouse event.
                Me.OnMouseMove(s) 'Update UI points.

            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        zooming = False

    End Sub
    Private Sub blockForm_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        counter = 0
        mouseIsDown = True
        dragPoint = absoluteMousePoint 'Save point where dragging begun.

    End Sub
    Private Sub picMinimap_MouseDown(sender As Object, e As MouseEventArgs) Handles picMinimap.MouseDown

        mouseIsDown = True

    End Sub
    Private Sub blockForm_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp

        mouseIsDown = False
        dragStart = False

        If UniversePausedForDragging = True Then
            UniversePaused = False
            UniversePausedForDragging = False
        End If

        myUniverse.isDragged = dragStart
        counter = 0 'Reset counter.

    End Sub
    Private Sub picMinimap_MouseUp(sender As Object, e As MouseEventArgs) Handles picMinimap.MouseUp

        mouseIsDown = False
        minimapDragging = False

        If UniversePausedForDragging = True Then
            UniversePaused = False
            UniversePausedForDragging = False
        End If

    End Sub
    Private Sub blockForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.ControlKey Then
            ctrlKeyDown = True
        End If

    End Sub
    Private Sub blockForm_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp

        If e.KeyCode = Keys.ControlKey Then
            ctrlKeyDown = False
        End If

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Functions used with mouse events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub CheckPlanetContinuousMode(e As MouseEventArgs)

        If ctrlKeyDown And mouseIsDown And creationMode = "p" And counter > 2 Then

            dragStart = False
            Me.OnClick(e)
            counter = 0

        End If

    End Sub
    Private Sub CheckDragging()

        Dim dragOffset As PointF = New PointF(Math.Abs(dragPoint.X - absoluteMousePoint.X), Math.Abs(dragPoint.Y - absoluteMousePoint.Y))

        'If the mouse is moved for a fixed distance and not on the minimap, start dragging universe.
        If mouseIsDown And Not minimapDragging And (dragOffset.X > 20 Or dragOffset.Y > 20) Then
            dragStart = True
        End If

        If Not ctrlKeyDown And dragStart And counter > 5 And Not onFrame And Not followingObject Then

            dragging = True

            If UniversePaused = False Then
                UniversePaused = True
                UniversePausedForDragging = True
            End If

            counter = 5 'The maximum speed we can draw objects. Lower means slower.
            myUniverse.isDragged = dragStart

            'Move universe.
            Try
                moveUniverse(dragPoint, absoluteMousePoint, False)

                Dim s As New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) 'Init empty mouse event.
                Me.OnMouseMove(s) 'Update UI points.

            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

            'Update origin point.
            dragPoint = absoluteMousePoint

            dragging = False

        End If

    End Sub
    Private Sub ObjectHover()

        'If we haven't selected anything.
        If selectedObject Is Nothing Then

            'Show info if hovering object.
            If hover Then
                showHoverLabel(hoverObject)
            Else
                hoverLabel.Visible = False
                hoverObject = Nothing 'Empty object.
            End If
        End If

    End Sub
    Private Sub CheckObjectSelection()

        'If clicked on nothing, clear selection.
        If Not hover And selectedObject IsNot Nothing Then
            ClearSelection()
        Else
            If selectedObject IsNot Nothing Then

                If selectedObject.Equals(hoverObject) Then 'If clicked on self, clear selection.
                    ClearSelection()
                    Me.Focus()
                    Exit Sub
                End If

                'De-select previous object.
                selectedObject.IsSelected = False

            End If

            'Check if we clicked on a hovered object.
            If hover And hoverObject IsNot Nothing Then
                SelectObject(hoverObject)
            End If

        End If

    End Sub
    Private Sub ObjectUnderMouse(ByVal objList As List(Of StellarObject))

        hover = False 'Reset flag.

        'De-select selected object just in case we selected nothing with the tool.
        If selectedObject IsNot Nothing And Not selectionRectangle.Location.IsEmpty And Not mouseIsDown Then
            CheckObjectSelection()
        End If

        'Reverse the list to select the closest object on the Z axis and not the farthest.
        objList.Reverse()

        For Each obj In objList

            If obj.Type = StellarObjectType.Planet Then

                Dim planet As Planet = CType(obj, Planet) 'Cast object as planet.

                Dim borderWidth As Double
                If zoomValue > 1 Then
                    borderWidth = planet.GetBorderWidth / zoomValue
                ElseIf zoomValue < 1 Then
                    borderWidth = planet.GetBorderWidth * zoomValue
                End If

                Dim rec As New RectangleF(New PointF(planet.Vertices(0).X - borderWidth, planet.Vertices(0).Y - borderWidth),
                                          New SizeF(obj.VisualSize + 2 * borderWidth,
                                                    obj.VisualSize + 2 * borderWidth))

                'Check if we' re selecting.
                If isSelecting And Not mouseIsDown And Not selectionRectangle.Location.IsEmpty Then

                    'Select object if it's inside the selection rectangle.
                    If realSelectionRectangle.IntersectsWith(rec) Then
                        SelectObject(objList.Item(objList.IndexOf(obj)))
                        Exit For
                    End If

                End If

                'Check if cursor is inside the rectangle.
                If rec.Contains(mousePoint) Then
                    hoverObject = objList.Item(objList.IndexOf(obj)) 'Get reference to original item not the local copy.
                    hover = True 'Set flag.
                    Exit For
                End If

            Else
                Dim borderWidth As Double
                If zoomValue > 1 Then
                    borderWidth = starBorderWidth / zoomValue
                ElseIf zoomValue < 1 Then
                    borderWidth = starBorderWidth * zoomValue
                End If

                'Calculate if the point is inside the ellipse area.
                Dim ellipseEquation As Double = (mousePoint.X - obj.CenterOfMass.X) ^ 2 /
                                                (obj.VisualSize + borderWidth) ^ 2 +
                                                (mousePoint.Y - obj.CenterOfMass.Y) ^ 2 /
                                                (obj.VisualSize + borderWidth) ^ 2

                'Check if we' re selecting.
                If isSelecting And Not mouseIsDown And Not selectionRectangle.Location.IsEmpty Then

                    'Select object if it intersects with the selection rectangle.
                    If RectangleIntersectCircle(realSelectionRectangle, New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y), obj.VisualSize) Then
                        SelectObject(objList.Item(objList.IndexOf(obj)))
                        Exit For
                    End If

                End If

                'The number of digits to round to, makes a noticeable difference.
                If Math.Round(ellipseEquation, 2) <= 1.0 Then

                    hoverObject = objList.Item(objList.IndexOf(obj)) 'Get reference to original item not the local copy.
                    hover = True 'Set flag.
                    Exit For

                End If

            End If

        Next

        objList.Reverse() 'Restore it.

    End Sub
    Private Sub SelectObject(ByVal obj As StellarObject)

        'Select object.
        selectedObject = obj
        selectedObject.IsSelected = True

        'Enable group box and trajectory options.
        Control_Enabled(boxSelParam, True)

        'Check if we already have the option checked.
        If chkFollowSelected.Checked Then

            followingObject = True
            Control_Enabled(radRelativeTraj, True)
            Control_Enabled(radBothTraj, True)

        End If

        'Reset trajectories if followed object changed and we 're drawing relative trajectories.
        If followingObject And radRealTraj.Checked = False Then
            For Each obj In myUniverse.Objects
                obj.ClearTrajectory()
                obj.ClearRelativeDistances()
            Next
        End If

    End Sub
    Private Sub ClearSelection()

        If selectedObject IsNot Nothing Then
            selectedObject.IsSelected = False
        End If
        selectedObject = Nothing
        followingObject = False

        RadioButton_Checked(radRealTraj, True) 'If nothing is selected/followed, we can only draw real trajectories.
        Control_Enabled(boxSelParam, False)
        Control_Enabled(radRelativeTraj, False)
        Control_Enabled(radBothTraj, False)

    End Sub

    Private Sub FollowObject(ByVal checked As Boolean)

        If followingObject And selectedObject IsNot Nothing Then

            Dim objCenter As New PointF(selectedObject.CenterOfMass.X, selectedObject.CenterOfMass.Y)
            Dim universeDefaultCenter As New PointF((defaultUniverseWidth + defaultClipOffset) / 2 - 1, (defaultUniverseHeight + defaultClipOffset) / 2 - 1)

            'Transforming the point to the original center seems to be the best choice.
            Dim points() As PointF = {objCenter}
            myUniverseMatrix.TransformPoints(points)
            objCenter = points(0)

            'If object has not moved, don't waste resources.
            'This also prevents from infinitely moving because of rounding errors.
            If Math.Abs(objCenter.X - universeDefaultCenter.X) < 0.4 And Math.Abs(objCenter.Y - universeDefaultCenter.Y) < 0.4 Then
                Exit Sub
            End If

            'Center around object and follow it.
            moveUniverse(objCenter, universeDefaultCenter, False)
        End If

    End Sub

    Private Delegate Sub showHoverLabelDelegate(ByVal obj As StellarObject)
    Private Sub showHoverLabel(ByVal obj As StellarObject)

        If obj IsNot Nothing Then

            Dim objTopRight() As PointF

            Dim objLocX As String = Math.Round(obj.CenterOfMass.X, 2).ToString
            Dim objLocY As String = Math.Round(obj.CenterOfMass.Y, 2).ToString
            Dim objLocZ As String = Math.Round(obj.CenterOfMass.Z, 2).ToString
            Dim objAccX As String = Math.Round(obj.AccX, 4).ToString
            Dim objAccY As String = Math.Round(obj.AccY, 4).ToString
            Dim objAccZ As String = Math.Round(obj.AccZ, 4).ToString
            Dim objVelX As String = Math.Round(obj.VelX, 4).ToString
            Dim objVelY As String = Math.Round(obj.VelY, 4).ToString
            Dim objVelZ As String = Math.Round(obj.VelZ, 4).ToString

            If objAccX = "0" And obj.AccX <> 0 Then
                objAccX = "~0"
            End If
            If objAccY = "0" And obj.AccY <> 0 Then
                objAccY = "~0"
            End If
            If objAccZ = "0" And obj.AccZ <> 0 Then
                objAccZ = "~0"
            End If
            If objVelX = "0" And obj.VelX <> 0 Then
                objVelX = "~0"
            End If
            If objVelY = "0" And obj.VelY <> 0 Then
                objVelY = "~0"
            End If
            If objVelZ = "0" And obj.VelZ <> 0 Then
                objVelZ = "~0"
            End If

            'Create label once.
            If hoverLabel.Parent Is Nothing Then

                hoverLabel.Size = New Size(105, 46)
                hoverLabel.AutoSize = True
                hoverLabel.BorderStyle = BorderStyle.FixedSingle
                hoverLabel.ForeColor = Color.White
                hoverLabel.BackColor = Color.FromArgb(64, 64, 64)
                hoverLabel.Font = New Font("Calibri", 9)
                hoverLabel.Parent = Me
                AddHandler hoverLabel.MouseMove, AddressOf hoverLabelMouseMove
                hoverLabel.Show()

            End If

            'Insert data.
            hoverLabel.Text = "L: " + objLocX + " | " + objLocY + " | " + objLocZ + vbCrLf +
                              "A: " + objAccX + " | " + objAccY + " | " + objAccZ + vbCrLf +
                              "V: " + objVelX + " | " + objVelY + " | " + objVelZ

            Dim visualSize As Double = obj.VisualSize

            'This required some math to figure out.
            Dim emptySpace As Double = visualSize - visualSize / Math.Sqrt(2) 'R - R/sqrt(2)

            'Position label next to the object.
            If obj.Type = StellarObjectType.Planet Then
                visualSize = obj.VisualSize / 2
                objTopRight = {New PointF(obj.CenterOfMass.X + visualSize + obj.BorderWidth / 2,
                                          obj.CenterOfMass.Y - visualSize - obj.BorderWidth / 2)}
            Else
                objTopRight = {New PointF(obj.CenterOfMass.X + visualSize - emptySpace + obj.BorderWidth / 2,
                                          obj.CenterOfMass.Y - visualSize + emptySpace - obj.BorderWidth / 2)}
            End If

            Dim selectionDist As Double = 0 'Extra distance added because of the selection shape.

            'Adjust the label so it shows directly upon the selection shape and not the actual object.
            'This is pure ME.
            If obj.IsSelected Then

                Dim zoom As Double = zoomValue
                If zoom >= 1 Then
                    zoom = 1
                Else
                    zoom = (2 - zoom) ^ 3
                End If

                'The selection shape pen width is one (1) pixel.
                selectionDist = 1 / 2 + 2 * Convert.ToSingle(zoom)

                objTopRight(0).X += selectionDist
                objTopRight(0).Y -= selectionDist

            End If

            'We need to transform the point to the original matrix to perform checks.
            myUniverseMatrix.TransformPoints(objTopRight)

            'Check for overflows.
            HoverLabelCheckHorizontalOverflow(obj, objTopRight, visualSize, selectionDist)
            HoverLabelCheckTopOverflow(obj, objTopRight, visualSize, selectionDist)

            hoverLabel.Location = New Point(objTopRight(0).X, objTopRight(0).Y - hoverLabel.Height)
            hoverLabel.Visible = True
            hoverLabel.BringToFront()

        End If

    End Sub
    Private Sub HoverLabelCheckHorizontalOverflow(ByVal obj As StellarObject, ByRef objTopRight As PointF(), ByVal visualSize As Double,
                                             ByVal selectionDist As Double)

        'We use this when multiple overflows are happening, thus the checks and calculations must be performed differently.
        Dim sign As Double = 1.0

        If hoverLabelOverflow.Contains("t") Then
            sign = -1
        End If

        'This required some math to figure out.
        Dim emptySpace As Double = visualSize - visualSize / Math.Sqrt(2) 'R - R/sqrt(2)
        Dim clipDist As Double

        'Check if label doesn't fit in the right side. If so, put it on the left side.
        If objTopRight(0).X + hoverLabel.Width > defaultUniverseWidth Then

            clipDist = objTopRight(0).X + hoverLabel.Width - defaultUniverseWidth

            If Not hoverLabelOverflow.Contains("r") Then
                hoverLabelOverflow += "r"
            End If

            'Move to opposite side.
            objTopRight(0).X -= hoverLabel.Width
            sign = -1

            myInverseUniverseMatrix.TransformPoints(objTopRight)
            If obj.Type = StellarObjectType.Star Then
                objTopRight(0).X -= 2 * (visualSize - emptySpace + selectionDist)
            Else
                objTopRight(0).X -= 2 * (visualSize + selectionDist) + obj.BorderWidth
            End If
            myUniverseMatrix.TransformPoints(objTopRight)

            'If the label doesn't fit to either side, show it on the center.
            If objTopRight(0).X < defaultClipOffset Then

                myInverseUniverseMatrix.TransformPoints(objTopRight)
                objTopRight(0).X = obj.CenterOfMass.X
                myUniverseMatrix.TransformPoints(objTopRight)

            ElseIf objTopRight(0).X + hoverLabel.Width > defaultUniverseWidth Then 'We got a right overflow again. Start rolling.

                clipDist = objTopRight(0).X + hoverLabel.Width - defaultUniverseWidth

                'Check if we' re having a bottom overflow as well. If so, stick it on the corner.
                'This condition must apply when the object is in the bottom-right corner.
                If obj.CenterOfMass.Y > obj.Universe.getHeight And objTopRight(0).Y + clipDist > defaultUniverseHeight Then
                    objTopRight(0).X = defaultUniverseWidth - hoverLabel.Width
                    objTopRight(0).Y = defaultUniverseHeight
                Else

                    If obj.Type = StellarObjectType.Star Then

                        'Move label.
                        objTopRight(0).X += clipDist * sign
                        objTopRight(0).Y += clipDist

                        myInverseUniverseMatrix.TransformPoints(objTopRight)

                        'Stay on center, if beyond it.
                        If objTopRight(0).Y > obj.CenterOfMass.Y Then

                            objTopRight(0).Y = obj.CenterOfMass.Y
                            myUniverseMatrix.TransformPoints(objTopRight)
                            objTopRight(0).X = defaultUniverseWidth - hoverLabel.Width 'Stick to wall.
                        Else
                            myUniverseMatrix.TransformPoints(objTopRight)
                        End If
                    Else
                        objTopRight(0).X = defaultUniverseWidth - hoverLabel.Width 'Stick to wall.
                    End If

                End If

            ElseIf objTopRight(0).Y > defaultUniverseHeight Then 'We got a bottom overflow. Start rolling.

                clipDist = objTopRight(0).Y - defaultUniverseHeight

                'Check if we' re having a right overflow as well. If so, stick it on the corner.
                'This condition must apply when the object is in the bottom-right corner.
                If obj.CenterOfMass.X > obj.Universe.getWidth And
                   objTopRight(0).X + hoverLabel.Width + clipDist > defaultUniverseWidth Then

                    objTopRight(0).X = defaultUniverseWidth - hoverLabel.Width
                    objTopRight(0).Y = defaultUniverseHeight
                Else

                    If obj.Type = StellarObjectType.Star Then

                        'Move label.
                        objTopRight(0).X += clipDist
                        objTopRight(0).Y += clipDist * sign

                        myInverseUniverseMatrix.TransformPoints(objTopRight)

                        'Stay on center, if beyond it.
                        If objTopRight(0).X + hoverLabel.Width > obj.CenterOfMass.X Then

                            objTopRight(0).X = obj.CenterOfMass.X - hoverLabel.Width
                            myUniverseMatrix.TransformPoints(objTopRight)
                            objTopRight(0).Y = defaultUniverseHeight 'Stick to wall.
                        Else
                            myUniverseMatrix.TransformPoints(objTopRight)
                        End If
                    Else
                        objTopRight(0).Y = defaultUniverseHeight 'Stick to wall.
                    End If

                End If
            End If

        ElseIf objTopRight(0).Y > defaultUniverseHeight Then 'We got a bottom overflow. Start rolling.

            If obj.Type = StellarObjectType.Star Then

                clipDist = objTopRight(0).Y - defaultUniverseHeight

                'Check if we' re having a right overflow as well. If so, stick it on the corner.
                If obj.CenterOfMass.X > obj.Universe.getWidth And
                 objTopRight(0).X + hoverLabel.Width + clipDist > defaultUniverseWidth Then

                    objTopRight(0).X = defaultUniverseWidth - hoverLabel.Width
                    objTopRight(0).Y = defaultUniverseHeight
                Else
                    'Move label.
                    objTopRight(0).X += clipDist
                    objTopRight(0).Y += clipDist * sign

                    myInverseUniverseMatrix.TransformPoints(objTopRight)

                    'Stay on center, if beyond it.
                    If objTopRight(0).X + hoverLabel.Width > obj.CenterOfMass.X Then

                        objTopRight(0).X = obj.CenterOfMass.X - hoverLabel.Width
                        myUniverseMatrix.TransformPoints(objTopRight)
                        objTopRight(0).Y = defaultUniverseHeight 'Stick to wall.
                    Else
                        myUniverseMatrix.TransformPoints(objTopRight)
                    End If

                End If
            Else
                objTopRight(0).Y = defaultUniverseHeight 'Stick to wall.
            End If

        ElseIf objTopRight(0).X < defaultClipOffset Then 'We got a left overflow. Start rolling.

            ''Delete right overflow direction.
            'If hoverLabelOverflow.Contains("r") Then
            '    hoverLabelOverflow = hoverLabelOverflow.Remove(hoverLabelOverflow.IndexOf("r"), 1)
            'End If

            ''Add new overflow direction.
            'If Not hoverLabelOverflow.Contains("l") Then
            '    hoverLabelOverflow += "l"
            'End If

            'Check if we' re having a bottom overflow as well. If so, stick it on the corner.
            If obj.CenterOfMass.Y > obj.Universe.getHeight And objTopRight(0).Y + clipDist > defaultUniverseHeight Then
                objTopRight(0).X = defaultClipOffset
                objTopRight(0).Y = defaultUniverseHeight
            Else

                If obj.Type = StellarObjectType.Star Then

                    clipDist = defaultClipOffset - objTopRight(0).X

                    If obj.CenterOfMass.Y > obj.Universe.getHeight And objTopRight(0).Y + clipDist > defaultUniverseHeight Then
                        objTopRight(0).X = defaultClipOffset
                        objTopRight(0).Y = defaultUniverseHeight
                    Else
                        'Move label.
                        objTopRight(0).X += clipDist
                        objTopRight(0).Y += clipDist * sign

                        myInverseUniverseMatrix.TransformPoints(objTopRight)

                        'Stay on center, if beyond it.
                        If objTopRight(0).Y > obj.CenterOfMass.Y Then

                            objTopRight(0).Y = obj.CenterOfMass.Y
                            myUniverseMatrix.TransformPoints(objTopRight)
                            objTopRight(0).X = defaultClipOffset 'Stick to wall.

                        Else
                            myUniverseMatrix.TransformPoints(objTopRight)

                            'Check if label still doesn't fit. If so, stop where you are.
                            If objTopRight(0).X + hoverLabel.Width > defaultUniverseWidth Then
                                objTopRight(0).X = defaultUniverseWidth - hoverLabel.Width
                            End If

                        End If
                    End If
                Else
                    objTopRight(0).X = defaultClipOffset 'Stick to wall.
                End If

            End If

        ElseIf objTopRight(0).Y > defaultUniverseHeight Then 'We got a bottom overflow. Start rolling.

            If obj.Type = StellarObjectType.Star Then

                clipDist = objTopRight(0).Y - defaultUniverseHeight

                'Check if we' re having a left overflow as well. If so, stick it on the corner.
                'This condition must apply when the object is in the bottom-left corner.
                If obj.CenterOfMass.X < obj.UniverseOffsetX And objTopRight(0).X - clipDist < defaultClipOffset Then
                    objTopRight(0).X = defaultClipOffset
                    objTopRight(0).Y = defaultUniverseHeight
                Else
                    'Move label.
                    objTopRight(0).X -= clipDist * sign
                    objTopRight(0).Y -= clipDist * sign

                    myInverseUniverseMatrix.TransformPoints(objTopRight)

                    'Stay on center, if beyond it.
                    If objTopRight(0).X < obj.CenterOfMass.X Then

                        objTopRight(0).X = obj.CenterOfMass.X
                        myUniverseMatrix.TransformPoints(objTopRight)
                        objTopRight(0).Y = defaultUniverseHeight 'Stick to wall.

                    Else
                        myUniverseMatrix.TransformPoints(objTopRight)
                    End If

                End If
            Else
                objTopRight(0).Y = defaultUniverseHeight 'Stick to wall.
            End If

        Else
            If hoverLabelOverflow.Contains("r") Then
                hoverLabelOverflow = hoverLabelOverflow.Remove(hoverLabelOverflow.IndexOf("r"), 1) 'Delete overflow direction.
            End If
        End If

    End Sub
    Private Sub HoverLabelCheckTopOverflow(ByVal obj As StellarObject, ByRef objTopRight As PointF(), ByVal visualSize As Double,
                                           ByVal selectionDist As Double)

        'We use this when multiple overflows are happening, thus the checks and calculations must be performed differently.
        Dim sign As Integer = 1

        If hoverLabelOverflow.Contains("r") Then
            sign = -1
        End If

        'This required some math to figure out.
        Dim emptySpace As Double = visualSize - visualSize / Math.Sqrt(2) 'R - R/sqrt(2)

        If objTopRight(0).Y - hoverLabel.Height < defaultClipOffset Then

            Dim clipDist As Double 'How much we need to move the label?
            'Dim wallType As Double 'Which wall (top/bottom) we need to stick to?

            'wallType = defaultClipOffset
            'clipDist = wallType - (objTopRight(0).Y - hoverLabel.Height)
            'wallType += hoverLabel.Height

            'Move to bottom side.
            objTopRight(0).Y += hoverLabel.Height

            'There is a lot of point transformations but I can't help it.
            myInverseUniverseMatrix.TransformPoints(objTopRight)
            If obj.Type = StellarObjectType.Star Then
                objTopRight(0).Y += 2 * (visualSize - emptySpace + selectionDist)
            Else
                objTopRight(0).Y += 2 * (visualSize + selectionDist) + obj.BorderWidth
            End If
            myUniverseMatrix.TransformPoints(objTopRight)

            'If the label doesn't fit to either side, show it on the center.
            If hoverLabelOverflow.Contains("t") And objTopRight(0).Y > defaultUniverseHeight Then

                myInverseUniverseMatrix.TransformPoints(objTopRight)
                objTopRight(0).Y = obj.CenterOfMass.Y
                myUniverseMatrix.TransformPoints(objTopRight)

            ElseIf objTopRight(0).X + hoverLabel.Width > defaultUniverseWidth Then
                'We are on the bot-left corner and we're having a right overflow.
                'Start moving the label.

                clipDist = objTopRight(0).X + hoverLabel.Width - defaultUniverseWidth
                objTopRight(0).Y += clipDist * sign
                objTopRight(0).X += clipDist * sign

                'Stay on center, if beyond it.
                myInverseUniverseMatrix.TransformPoints(objTopRight)
                If objTopRight(0).Y - hoverLabel.Height < obj.CenterOfMass.Y Then
                    objTopRight(0).Y = obj.CenterOfMass.Y + hoverLabel.Height
                End If
                myUniverseMatrix.TransformPoints(objTopRight)

                'Here we have a top overflow while moving the label. Stick it on the corner.
                If objTopRight(0).Y - hoverLabel.Height < defaultClipOffset Then
                    objTopRight(0).Y = defaultClipOffset + hoverLabel.Height
                End If

            ElseIf objTopRight(0).Y - hoverLabel.Height < defaultClipOffset Then
                'We are on the bot-right corner and we're having a top overflow again.
                'Start moving the label.

                clipDist = defaultClipOffset - (objTopRight(0).Y - hoverLabel.Height)
                objTopRight(0).X -= clipDist * sign
                objTopRight(0).Y += clipDist

                'Stay on center, if we go beyond it.
                myInverseUniverseMatrix.TransformPoints(objTopRight)
                If objTopRight(0).X * sign < obj.CenterOfMass.X * sign Then
                    objTopRight(0).X = obj.CenterOfMass.X
                End If
                myUniverseMatrix.TransformPoints(objTopRight)

                'Here we have a right or left overflow while moving the label. Stick it on the corner.
                If objTopRight(0).X + hoverLabel.Width > defaultUniverseWidth Then
                    objTopRight(0).X = defaultUniverseWidth - hoverLabel.Width
                ElseIf objTopRight(0).X < defaultClipOffset Then
                    objTopRight(0).X = defaultClipOffset
                End If

            ElseIf objTopRight(0).X < defaultClipOffset Then
                'We are on the bot-right corner and we're having a left overflow.
                'Start moving the label.

                clipDist = defaultClipOffset - objTopRight(0).X
                objTopRight(0).Y -= clipDist * sign
                objTopRight(0).X += clipDist * sign

                'Stay on center, if beyond it.
                myInverseUniverseMatrix.TransformPoints(objTopRight)
                If objTopRight(0).Y - hoverLabel.Height < obj.CenterOfMass.Y Then
                    objTopRight(0).Y = obj.CenterOfMass.Y + hoverLabel.Height
                End If
                myUniverseMatrix.TransformPoints(objTopRight)

                'Here we have a top overflow while moving the label. Stick it on the corner.
                If objTopRight(0).Y - hoverLabel.Height < defaultClipOffset Then
                    objTopRight(0).Y = defaultClipOffset + hoverLabel.Height
                End If

            End If
            'Add overflow direction.
            If Not hoverLabelOverflow.Contains("t") Then
                hoverLabelOverflow += "t"
            End If
        Else
            If hoverLabelOverflow.Contains("t") Then
                hoverLabelOverflow = hoverLabelOverflow.Remove(hoverLabelOverflow.IndexOf("t"), 1) 'Delete overflow direction.
            End If
        End If

    End Sub
    Private Delegate Sub hideHoverLabelDelegate()
    Private Sub hideHoverLabel()

        hoverLabel.Visible = False

    End Sub
    Private Sub hoverLabelMouseMove()

        'We need this so we can catch mouse movements above objects that are behind labels.
        Me.OnMouseMove(New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0))

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Action functions.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    Private Sub SetSelectionRectangle()

        If mouseIsDown Then

            'Remember to transform the points.
            Dim points() As PointF = {absoluteMousePoint, dragPoint}
            myInverseUniverseMatrix.TransformPoints(points)

            'We also use the sign of the X,Y distance to figure out the direction of the cursor.
            selectionDist = New PointF(points(0).X - points(1).X, points(0).Y - points(1).Y)

            If selectionDist.IsEmpty Then
                Exit Sub
            End If

            'Set location and selection size.
            selectionRectangle.Width = Math.Abs(selectionDist.X)
            selectionRectangle.Height = Math.Abs(selectionDist.Y)

            'Set starting point once.
            If selectionRectangle.Location.IsEmpty Then
                selectionRectangle.Location = points(1)
            End If

        End If

    End Sub

    Private Function RectangleIntersectCircle(ByVal rec As RectangleF, ByVal center As PointF, ByVal radius As Double) As Boolean

        'Do the basic check first.
        If rec.Contains(center) Then
            Return True
        Else
            'Get the square of the circle.
            Dim circleSquare As New RectangleF(center.X - radius, center.Y - radius, 2 * radius, 2 * radius)

            'If we intersect with the circle square, we need to check if we are actually inside the circle and not on the square's edges.
            If rec.IntersectsWith(circleSquare) Then

                'Rectangle vertices.
                Dim a As PointF = rec.Location
                Dim b As New PointF(rec.Location.X + rec.Width, rec.Location.Y)
                Dim c As New PointF(rec.Location.X + rec.Width, rec.Location.Y + rec.Height)
                Dim d As New PointF(rec.Location.X, rec.Location.Y + rec.Height)

                Dim intersectCount As Integer = 0
                Dim intersectVertex As New PointF

                'Get all the rectangles for the edges.
                Dim topLeftEdge As New RectangleF(circleSquare.X, circleSquare.Y, radius, radius)
                Dim topRightEdge As New RectangleF(center.X, circleSquare.Y, radius, radius)
                Dim botRightEdge As New RectangleF(center.X, center.Y, radius, radius)
                Dim botLeftEdge As New RectangleF(circleSquare.X, center.Y, radius, radius)

                If rec.IntersectsWith(topLeftEdge) Then
                    intersectCount += 1
                    intersectVertex = c
                End If
                If rec.IntersectsWith(topRightEdge) Then
                    intersectCount += 1
                    intersectVertex = d
                End If
                If rec.IntersectsWith(botRightEdge) Then
                    intersectCount += 1
                    intersectVertex = a
                End If
                If rec.IntersectsWith(botLeftEdge) Then
                    intersectCount += 1
                    intersectVertex = b
                End If

                'If we are intersecting with at least two (2) edges, that means we are definitely intersecting with the circle.
                If intersectCount > 1 Then
                    Return True
                Else

                    'If the distance between the vertex and the center is shorter than the radius that means we 're inside the circle.
                    Dim dist As Double = Math.Sqrt((center.X - intersectVertex.X) * (center.X - intersectVertex.X) +
                                                       (center.Y - intersectVertex.Y) * (center.Y - intersectVertex.Y))
                    If dist <= radius Then
                        Return True
                    End If

                End If

            End If

        End If

        Return False

    End Function

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Delegate functions for form controls.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    Private Sub Control_Invalidate(ByVal ctrl As Control, ByVal region As Region)
        If ctrl.InvokeRequired Then
            ctrl.Invoke(Sub() ctrl.Invalidate(region))
            Return
        End If
        ctrl.Invalidate(region)
    End Sub
    Private Sub Control_Enabled(ByVal ctrl As Control, ByVal bool As Boolean)
        If ctrl.InvokeRequired Then
            ctrl.Invoke(Sub() ctrl.Enabled = bool)
            Return
        End If
        ctrl.Enabled = bool
    End Sub
    Private Sub CheckBox_Checked(ByVal ctrl As CheckBox, ByVal bool As Boolean)
        If ctrl.InvokeRequired Then
            ctrl.Invoke(Sub() ctrl.Checked = bool)
            Return
        End If
        ctrl.Checked = bool
    End Sub
    Private Sub RadioButton_Checked(ByVal ctrl As RadioButton, ByVal bool As Boolean)
        If ctrl.InvokeRequired Then
            ctrl.Invoke(Sub() ctrl.Checked = bool)
            Return
        End If
        ctrl.Checked = bool
    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Options events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub cbCreateType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbCreateType.SelectedIndexChanged

        creationMode = cbCreateType.SelectedItem.ToString.ToLower.Chars(0)

    End Sub

    Private Sub chkbNoCreation_CheckedChanged(sender As Object, e As EventArgs) Handles chkbNoCreation.CheckedChanged
        If chkbNoCreation.Checked Then
            creationMode = ""
            cbCreateType.Enabled = False
        Else
            cbCreateType.Enabled = True
        creationMode = cbCreateType.SelectedItem.ToString.ToLower.Chars(0)
        End If
    End Sub

    Private Sub radCollisionBounce_CheckedChanged(sender As Object, e As EventArgs) Handles radCollisionBounce.CheckedChanged
        myUniverse.Bounce = radCollisionBounce.Checked
    End Sub

    Private Sub radRelativeTraj_CheckedChanged(sender As Object, e As EventArgs) Handles radRelativeTraj.CheckedChanged

        relativeTrajectories = radRelativeTraj.Checked
        myUniverse.RelativeTraj = relativeTrajectories

    End Sub
    Private Sub radBothTraj_CheckedChanged(sender As Object, e As EventArgs) Handles radBothTraj.CheckedChanged

        myUniverse.drawBothTraj = radBothTraj.Checked
        relativeTrajectories = radBothTraj.Checked
        myUniverse.RelativeTraj = relativeTrajectories

    End Sub

    Private Sub chkFollowSelected_CheckedChanged(sender As Object, e As EventArgs) Handles chkFollowSelected.CheckedChanged

        followingObject = chkFollowSelected.Checked
        radRelativeTraj.Enabled = followingObject
        radBothTraj.Enabled = followingObject

        If followingObject Then
            FollowObject(followingObject)
        Else
            radRealTraj.Checked = True
        End If

    End Sub
    Private Sub chkHideHover_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideInfo.CheckedChanged

        hideHoverInfo = chkHideInfo.Checked

    End Sub

    Private Sub numTimeTick_ValueChanged(sender As Object, e As EventArgs) Handles numTimeTick.ValueChanged

        tickValue = Convert.ToInt32(numTimeTick.Value)
        myUniverse.TickRate = tickValue

    End Sub

    Private Sub chkRealisticSim_CheckedChanged(sender As Object, e As EventArgs) Handles chkRealisticSim.CheckedChanged

        myUniverse.isRealistic = chkRealisticSim.Checked
        'ResetUniverse()

        'If myUniverse.isRealistic Then
        '    myUniverse.DistanceMultiplier = 1000000000.0 '1 pixel = 1 million km = 1 billion meters.
        'Else
        '    myUniverse.DistanceMultiplier = 1000.0 '1 pixel = 1 km = 1000 meters.
        'End If

    End Sub
    Private Sub chkDrawTrajectories_CheckedChanged(sender As Object, e As EventArgs) Handles chkDrawTrajectories.CheckedChanged

        myUniverse.DrawTraj = chkDrawTrajectories.Checked
        numTraj.Visible = chkDrawTrajectories.Checked
        boxTraj.Enabled = chkDrawTrajectories.Checked

        If Not chkDrawTrajectories.Checked Then
            myUniverse.Planets.ForEach(Sub(p) p.ClearTrajectory())
        End If

    End Sub
    Private Sub numTraj_ValueChanged(sender As Object, e As EventArgs) Handles numTraj.ValueChanged
        myUniverse.MaxTrajectoryPoints = numTraj.Value
    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Action menu button functions.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub btnPauseUniverse_Click(sender As Object, e As EventArgs) Handles btnPauseUniverse.Click

        If UniversePaused Then
            btnPauseUniverse.Text = "Pause"
            lblStatusMessage.Text = ""
            UniversePaused = False
            myUniverse.isPaused = False
        Else
            btnPauseUniverse.Text = "Resume"
            lblStatusMessage.Text = "Paused"
            UniversePaused = True
            myUniverse.isPaused = True
        End If

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        'Pause universe while saving.
        If Not UniversePaused Then
            btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)
        End If

        'Create binary file.
        'FIX HOUR WHEN MIDNIGHT
        Dim binFile = IO.File.Create(My.Computer.FileSystem.CurrentDirectory +
                                        "\sos_save" +
                                        DateTime.Now.Day.ToString + DateTime.Now.Month.ToString + DateTime.Now.Year.ToString +
                                        "_" + DateTime.Now.Hour.ToString + DateTime.Now.Minute.ToString + DateTime.Now.Second.ToString +
                                        ".bin")

        Dim universeData As New List(Of Byte)
        Dim tempMatrix As Drawing2D.Matrix = myUniverse.getGraphics.Transform.Clone

        'Convert transformation data to binary form.x
        universeData.AddRange(BitConverter.GetBytes(tempMatrix.Elements.Count))
        For Each elem In tempMatrix.Elements
            universeData.AddRange(BitConverter.GetBytes(elem))
        Next
        'Convert default transformation data to binary form.
        universeData.AddRange(BitConverter.GetBytes(defaultUniverseMatrix.Elements.Count))
        For Each elem In defaultUniverseMatrix.Elements
            universeData.AddRange(BitConverter.GetBytes(elem))
        Next

        Dim cosmosSection() As Byte = {currentCosmosSection}  'Save as one byte.

        'Convert universe data in binary form.
        universeData.AddRange(
                BitConverter.GetBytes(clipOffsetX).Concat(
                BitConverter.GetBytes(clipOffsetY).Concat(
                BitConverter.GetBytes(universeWidth).Concat(
                BitConverter.GetBytes(universeHeight).Concat(
                cosmosSection.Concat(
                BitConverter.GetBytes(zoomValue).Concat(
                BitConverter.GetBytes(radCollisionTunnel.Checked).Concat(
                BitConverter.GetBytes(myUniverse.Bounce).Concat(
                BitConverter.GetBytes(myUniverse.DrawTraj).Concat(
                BitConverter.GetBytes(myUniverse.RelativeTraj).Concat(
                BitConverter.GetBytes(myUniverse.drawBothTraj).Concat(
                BitConverter.GetBytes(myUniverse.MaxTrajectoryPoints).Concat(
                BitConverter.GetBytes(chkFollowSelected.Checked).Concat(
                BitConverter.GetBytes(chkHideInfo.Checked).Concat(
                BitConverter.GetBytes(myUniverse.TickRate).Concat(
                BitConverter.GetBytes(chkCanvasClear.Checked).Concat(
                BitConverter.GetBytes(myUniverse.isRealistic)))))))))))))))))
            )

        'Write object data.
        universeData.AddRange(BitConverter.GetBytes(myUniverse.Objects.Count))
        For Each obj In myUniverse.Objects

            Dim size As New Integer
            If obj.Type = StellarObjectType.Star Then
                size = obj.Radius
            Else
                size = obj.Size
            End If

            cosmosSection = {obj.CosmosSection}  'Char types in .NET are two (2) bytes because of Unicode.

            'Convert and merge object binary data to one array.
            universeData.AddRange(
                BitConverter.GetBytes(obj.Type).Concat(
                BitConverter.GetBytes(obj.Mass).Concat(
                BitConverter.GetBytes(size).Concat(
                BitConverter.GetBytes(obj.PaintColor.ToArgb).Concat(
                BitConverter.GetBytes(obj.CenterOfMass.X).Concat(
                BitConverter.GetBytes(obj.CenterOfMass.Y).Concat(
                BitConverter.GetBytes(obj.CenterOfMass.Z).Concat(
                BitConverter.GetBytes(obj.VelX).Concat(
                BitConverter.GetBytes(obj.VelY).Concat(
                BitConverter.GetBytes(obj.VelZ).Concat(
                cosmosSection.Concat(
                BitConverter.GetBytes(obj.IsMerged).Concat(
                BitConverter.GetBytes(obj.IsSelected)))))))))))))
            )

            'Convert trajectory points to binary form, if any.
            universeData.AddRange(BitConverter.GetBytes(obj.TrajectoryPoints.Count))
            If obj.TrajectoryPoints.Count Then
                For Each trajPoint In obj.TrajectoryPoints
                    universeData.AddRange(BitConverter.GetBytes(trajPoint.X).Concat(
                                          BitConverter.GetBytes(trajPoint.Y).Concat(
                                          BitConverter.GetBytes(trajPoint.Z))))
                Next
            End If

        Next

        'Write binary data to file.
        binFile.Write(universeData.ToArray, 0, universeData.Count)
        binFile.Close()

        'Unpause universe.
        btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click

        Dim frameRate As Integer = myUniverse.FrameRate 'Save framerate because after the pause it becomes zero (0) and we need it.
        Dim wasPaused As Boolean = False

        'Pause universe while loading.
        If Not UniversePaused Then
            btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)
            wasPaused = True
        End If

        Dim byteArray(7) As Byte
        Dim count As Integer
        Dim elemList As New List(Of Single)

        'Show dialog.
        Dim res As DialogResult = openFileDialog.ShowDialog()

        If openFileDialog.FileName = "" Or res <> DialogResult.OK Then
            If wasPaused Then
                btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)
            End If
            Exit Sub
        End If

        'Stop and remove all working threads, if any.
        For Each thread In threadList
            thread.Abort()
        Next
        threadList.RemoveRange(0, threadList.Count)

        'Remove current trajectories.
        myUniverse.Planets.ForEach(Sub(p)
                                       p.ClearTrajectory()
                                       p.ClearRelativeDistances()
                                   End Sub)

        'Clear any selected objects.
        ClearSelection()

        'Open binary file.
        Dim binFile = IO.File.OpenRead(openFileDialog.FileName)

        'Get universe transformation data by reading the count and then the elements one by one.
        binFile.Read(byteArray, 0, 4)
        count = BitConverter.ToUInt32(byteArray, 0)

        'Probably a wrong/corrupted file, exit.
        If count = 0 Then
            btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)
            Exit Sub
        End If

        While count
            binFile.Read(byteArray, 0, 4)
            elemList.Add(BitConverter.ToSingle(byteArray, 0))
            count -= 1
        End While

        'Load new universe transformation.
        myUniverse.getGraphics.Transform = New Drawing2D.Matrix(elemList(0), elemList(1), elemList(2), elemList(3), elemList(4), elemList(5))
        myUniverseMatrix = myUniverse.getGraphics.Transform.Clone
        myInverseUniverseMatrix = myUniverseMatrix.Clone
        myInverseUniverseMatrix.Invert()
        elemList.RemoveRange(0, elemList.Count)

        binFile.Read(byteArray, 0, 4)
        count = BitConverter.ToUInt32(byteArray, 0)

        While count
            binFile.Read(byteArray, 0, 4)
            elemList.Add(BitConverter.ToSingle(byteArray, 0))
            count -= 1
        End While

        'Load new default universe transformation.
        defaultUniverseMatrix = New Drawing2D.Matrix(elemList(0), elemList(1), elemList(2), elemList(3), elemList(4), elemList(5))

        'Load universe data.
        binFile.Read(byteArray, 0, 8)
        clipOffsetX = BitConverter.ToDouble(byteArray, 0)
        binFile.Read(byteArray, 0, 8)
        clipOffsetY = BitConverter.ToDouble(byteArray, 0)
        binFile.Read(byteArray, 0, 8)
        universeWidth = BitConverter.ToDouble(byteArray, 0)
        binFile.Read(byteArray, 0, 8)
        universeHeight = BitConverter.ToDouble(byteArray, 0)

        'Reset section button style.
        Me.Controls.Find("btnSection" + currentCosmosSection.ToString, True).First.BackColor = Color.Black

        'Get current cosmos section.
        binFile.Read(byteArray, 0, 1)
        currentCosmosSection = byteArray(0)

        'Reset UI, if necessary.
        If navigatingMinimap Then
            btnNavReturn_Click(btnNavReturn, e)
        End If

        'Get zoom value.
        binFile.Read(byteArray, 0, 8)
        zoomValue = BitConverter.ToDouble(byteArray, 0)
        UpdateObjectWidths()

        'Resize universe.
        myUniverse.ResizeUniverse(New PointF(clipOffsetX, clipOffsetY), New PointF(universeWidth, universeHeight), myUniverse.getGraphics.Transform)

        'Load collision settings.
        binFile.Read(byteArray, 0, 1)
        radCollisionTunnel.Checked = BitConverter.ToBoolean(byteArray, 0)
        binFile.Read(byteArray, 0, 1)
        radCollisionBounce.Checked = BitConverter.ToBoolean(byteArray, 0)

        If radCollisionTunnel.Checked = False And myUniverse.Bounce = False Then
            radCollisionNone.Checked = True
        End If

        'Load trajectory settings.
        binFile.Read(byteArray, 0, 1)
        chkDrawTrajectories.Checked = BitConverter.ToBoolean(byteArray, 0)
        binFile.Read(byteArray, 0, 1)
        radRelativeTraj.Checked = BitConverter.ToBoolean(byteArray, 0)
        binFile.Read(byteArray, 0, 1)
        radBothTraj.Checked = BitConverter.ToBoolean(byteArray, 0)
        binFile.Read(byteArray, 0, 4)
        numTraj.Value = BitConverter.ToUInt32(byteArray, 0)

        If radBothTraj.Checked = False And myUniverse.RelativeTraj = False Then
            radRealTraj.Checked = True
        End If

        'Load selection settings.
        binFile.Read(byteArray, 0, 1)
        Dim follow As Boolean = BitConverter.ToBoolean(byteArray, 0) 'We need to find the selected object first, if any.
        binFile.Read(byteArray, 0, 1)
        chkHideInfo.Checked = BitConverter.ToBoolean(byteArray, 0)

        'Load tick rate.
        binFile.Read(byteArray, 0, 4)
        numTimeTick.Value = BitConverter.ToUInt32(byteArray, 0)
        myUniverse.FrameRate = frameRate 'Set last saved frame rate value.

        'Load generic settings.
        binFile.Read(byteArray, 0, 1)
        chkCanvasClear.Checked = BitConverter.ToBoolean(byteArray, 0)
        binFile.Read(byteArray, 0, 1)
        chkRealisticSim.Checked = BitConverter.ToBoolean(byteArray, 0)

        'Remove current objects.
        objectListView.Items.Clear()
        myUniverse.Stars.RemoveRange(0, myUniverse.Stars.Count)
        myUniverse.Planets.RemoveRange(0, myUniverse.Planets.Count)

        'Load new objects.
        binFile.Read(byteArray, 0, 4)
        count = BitConverter.ToUInt32(byteArray, 0)

        While count

            Dim tempObj As New StellarObject

            'Get stellar object type (star, planet, ...).
            binFile.Read(byteArray, 0, 1)
            Dim objType As StellarObjectType = byteArray(0)

            'Convert it to the appropriate type.
            If objType = StellarObjectType.Star Then
                tempObj = New Star
            Else
                tempObj = New Planet
            End If

            'Read basic object data (mass, size, color).
            binFile.Read(byteArray, 0, 8)
            tempObj.Mass = BitConverter.ToDouble(byteArray, 0)
            binFile.Read(byteArray, 0, 4)
            If objType = StellarObjectType.Planet Then
                tempObj.Size = BitConverter.ToUInt32(byteArray, 0)
            Else
                tempObj.Radius = BitConverter.ToUInt32(byteArray, 0)
            End If
            binFile.Read(byteArray, 0, 4)
            tempObj.PaintColor = Color.FromArgb(BitConverter.ToInt32(byteArray, 0))

            Dim center As New PointFD

            'Get center of mass.
            binFile.Read(byteArray, 0, 8)
            center.X = BitConverter.ToDouble(byteArray, 0)
            binFile.Read(byteArray, 0, 8)
            center.Y = BitConverter.ToDouble(byteArray, 0)
            binFile.Read(byteArray, 0, 8)
            center.Z = BitConverter.ToDouble(byteArray, 0)

            'Get velocity.
            binFile.Read(byteArray, 0, 8)
            tempObj.VelX = BitConverter.ToDouble(byteArray, 0)
            binFile.Read(byteArray, 0, 8)
            tempObj.VelY = BitConverter.ToDouble(byteArray, 0)
            binFile.Read(byteArray, 0, 8)
            tempObj.VelZ = BitConverter.ToDouble(byteArray, 0)

            'Get cosmos section.
            binFile.Read(byteArray, 0, 1)
            tempObj.CosmosSection = byteArray(0)

            'Get some flags.
            binFile.Read(byteArray, 0, 1)
            Dim isMerged = BitConverter.ToBoolean(byteArray, 0)
            binFile.Read(byteArray, 0, 1)
            Dim isSelected As Boolean = BitConverter.ToBoolean(byteArray, 0)

            Dim mass As Double = tempObj.Mass 'Use this to restore the original mass after initialization.

            'Initialize and add new stellar object to the universe.
            If objType = StellarObjectType.Star Then
                CType(tempObj, Star).Init(myUniverse, myUniverseMatrix, center, tempObj.Radius, tempObj.Mass,
                         starBorderWidth, tempObj.PaintColor, Nothing, New PointFD(tempObj.VelX, tempObj.VelY))

                tempObj.Mass = mass
                myUniverse.AddStar(tempObj)
            Else
                CType(tempObj, Planet).Init(myUniverse, myUniverseMatrix, center, tempObj.Size, tempObj.Mass,
                         planetBorderWidth, tempObj.PaintColor, Nothing, New PointFD(tempObj.VelX, tempObj.VelY))

                tempObj.Mass = mass
                myUniverse.AddPlanet(tempObj)
            End If

            'Add it to our local list.
            AddListItem(tempObj, myUniverse.Objects)

            'After initialization the flags are cleared. We need to set them again, if necessary.
            If isSelected Then
                SelectObject(tempObj)
            End If
            chkFollowSelected.Checked = follow 'Set check status after the selection.
            If isMerged Then
                tempObj.IsMerged = True
            End If

            'Load trajectories, if any.
            binFile.Read(byteArray, 0, 4)
            Dim trajCount As Integer = BitConverter.ToUInt32(byteArray, 0)

            'Update object's local copy.
            tempObj.MaxTrajectoryPoints = trajCount

            While trajCount

                Dim trajPoint As New PointFD()

                binFile.Read(byteArray, 0, 8)
                trajPoint.X = BitConverter.ToDouble(byteArray, 0)
                binFile.Read(byteArray, 0, 8)
                trajPoint.Y = BitConverter.ToDouble(byteArray, 0)
                binFile.Read(byteArray, 0, 8)
                trajPoint.Y = BitConverter.ToDouble(byteArray, 0)

                tempObj.AddTrajectoryPoint(trajPoint)
                trajCount -= 1

            End While

            count -= 1

        End While

        'Unpause universe.
        btnPauseUniverse_Click(btnPauseUniverse, New EventArgs)

    End Sub

    Private Sub resetBtn_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ResetUniverse()
    End Sub

    Private Sub btnSelectTool_Click(sender As Object, e As EventArgs) Handles btnSelectTool.Click

        isSelecting = Not isSelecting 'Toggle selection mode.

        'Pause universe.

        If isSelecting Then
            btnSelectTool.FlatAppearance.BorderColor = Color.Red
            chkbNoCreation.Checked = True
        Else
            btnSelectTool.FlatAppearance.BorderColor = Color.Fuchsia
        End If

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Zooming/moving functions.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub zoomUniverse(ByVal zoomIn As Boolean)

        If formLoaded Then

            Dim zoomPoint As PointF = absoluteMousePoint
            Dim newZoomPoint As PointF = absoluteMousePoint

            Dim overflowDirection() As Boolean = {False, False, False, False}
            Dim universeMatrix As Drawing2D.Matrix = myUniverseMatrix.Clone

            'Zoom universe.
            ZoomUniverseOnPoint(zoomPoint, universeMatrix, zoomIn)

            'Update universe graphics.
            UpdateUniverseGraphics(universeMatrix)

            'Out of bounds checks.
            'Adjust zooming when close to sides or corners.
            'The order of the checks also deals with situations when the are only two (2) directions left (corners).
            If universeWidth > myUniverse.SectionWidth / 2 Then
                overflowDirection(0) = True
                newZoomPoint = New PointF(defaultUniverseWidth - 1, newZoomPoint.Y)
            End If
            If clipOffsetX < -myUniverse.SectionWidth / 2 Then
                overflowDirection(1) = True
                newZoomPoint = New PointF(defaultClipOffset, newZoomPoint.Y)
            End If
            If universeHeight > myUniverse.SectionHeight / 2 Then
                overflowDirection(2) = True
                newZoomPoint = New PointF(newZoomPoint.X, defaultUniverseHeight - 1)
            End If
            If clipOffsetY < -myUniverse.SectionHeight / 2 Then
                overflowDirection(3) = True
                newZoomPoint = New PointF(newZoomPoint.X, defaultClipOffset)
            End If

            'Check when there is only one direction left. In that case we take the middle point.
            If (overflowDirection(3) And overflowDirection(0) And overflowDirection(2)) Or       ' ]
               (overflowDirection(2) And overflowDirection(1) And overflowDirection(3)) Then     ' [
                newZoomPoint.Y = defaultUniverseHeight / 2 - 1
            ElseIf (overflowDirection(0) And overflowDirection(2) And overflowDirection(1)) Or   ' ˄
                   (overflowDirection(1) And overflowDirection(3) And overflowDirection(0)) Then ' ˅
                newZoomPoint.X = defaultUniverseWidth / 2 - 1
            End If

            Dim revert As Boolean = False

            'If there is no room for zooming out, undo zooming.
            If (overflowDirection(0) And overflowDirection(1)) Or
                overflowDirection(0) And overflowDirection(1) And overflowDirection(2) And overflowDirection(3) Then

                'Restore zoom value, since we revert the zooming.
                If zoomIn Then
                    zoomValue -= zoomStep
                Else
                    zoomValue += zoomStep
                End If
                zoomValue = Math.Round(zoomValue, 5)

                'Revert zooming.
                ZoomUniverseOnPoint(zoomPoint, universeMatrix, Not zoomIn)
                UpdateUniverseGraphics(universeMatrix)

            ElseIf overflowDirection.Contains(True) Then
                revert = True
            End If

            'Revert zooming and redo it, if said so.
            If revert Then

                'Revert zooming.
                ZoomUniverseOnPoint(zoomPoint, universeMatrix, Not zoomIn)
                UpdateUniverseGraphics(universeMatrix)

                'Redo it properly.
                ZoomUniverseOnPoint(newZoomPoint, universeMatrix, zoomIn)
                UpdateUniverseGraphics(universeMatrix)

            End If

            'Give the signal to the universe to update its objects. (new clipOffset locations etc.)
            resetOffset = True

            'Give the signal to the universe to update its objects. (new clipOffset locations etc.)
            myUniverse.ResizeUniverse(New PointF(clipOffsetX, clipOffsetY), New PointF(universeWidth, universeHeight), myUniverseMatrix)

            'Update object's widths based on new zoom value.
            UpdateObjectWidths()
        End If

    End Sub

    Private Sub ZoomUniverseOnPoint(ByVal zoomPoint As PointF, ByVal newMatrix As Drawing2D.Matrix, ByVal zoomIn As Boolean)

        'If an object is selected, make its center the zoom origin point.
        If selectedObject IsNot Nothing Then

            'We need the center of the object relative to the universe image.
            Dim relativeCenter() As PointF = {New Point(selectedObject.CenterOfMass.X, selectedObject.CenterOfMass.Y)}

            newMatrix.TransformPoints(relativeCenter)
            zoomPoint = relativeCenter(0)

        End If

        'Apply transformations. Move -> Scale -> Move back
        newMatrix.Translate(-zoomPoint.X, -zoomPoint.Y, Drawing2D.MatrixOrder.Append) 'Translate to opposite original point.

        'Scale.
        If zoomIn Then
            newMatrix.Scale(1 + zoomStep, 1 + zoomStep, Drawing2D.MatrixOrder.Append)
        Else
            newMatrix.Scale(1 / (1 + zoomStep), 1 / (1 + zoomStep), Drawing2D.MatrixOrder.Append) 'Scale.
        End If

        'Because sometimes the above scaling method leaves trailing nines (9) and zeros (0), we manually set it to the correct zoom value.
        'After the scaling of course. This changes the width and height of the Clip rectangle by whatever the rounding error was pretty miniscule.
        'cloneMatrix = New Drawing2D.Matrix(zoomValue, 0, 0, zoomValue, cloneMatrix.OffsetX, cloneMatrix.OffsetY) 'Set new matrix with proper scale.
        newMatrix.Translate(zoomPoint.X, zoomPoint.Y, Drawing2D.MatrixOrder.Append) 'Translate it to original point.

    End Sub

    Private Sub UpdateObjectWidths()

        If zoomValue > 1 Then
            scaledStarBWidth = starBorderWidth / zoomValue
            scaledPlanetBWidth = planetBorderWidth / zoomValue
        ElseIf zoomValue < 1 Then
            scaledStarBWidth = starBorderWidth * zoomValue
            scaledPlanetBWidth = planetBorderWidth * zoomValue
        End If

    End Sub

    Private Sub moveUniverse(ByVal originPoint As PointF, ByVal endPoint As PointF, ByVal fromMinimap As Boolean)

        If formLoaded Then

            Dim newMatrix As Drawing2D.Matrix
            Try
                newMatrix = myUniverseMatrix.Clone
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
                Exit Sub
            End Try

            Dim offsetX As Double = endPoint.X - originPoint.X
            Dim offsetY As Double = endPoint.Y - originPoint.Y

            'Out of bounds checks.
            'Different checks are used depending on the method used (dragging the universe/clicking on minimap).
            If fromMinimap Then

                'Stick to wall.
                If originPoint.X + (universeWidth - clipOffsetX) / 2 > myUniverse.SectionWidth / 2 Then
                    originPoint.X = myUniverse.SectionWidth / 2 - (universeWidth - clipOffsetX) / 2 - 1
                End If
                If originPoint.X - (universeWidth - clipOffsetX) / 2 + 1 < -myUniverse.SectionWidth / 2 Then
                    originPoint.X = -myUniverse.SectionWidth / 2 + (universeWidth - clipOffsetX) / 2 - 1
                End If
                If originPoint.Y + (universeHeight - clipOffsetY) / 2 > myUniverse.SectionHeight / 2 Then
                    originPoint.Y = myUniverse.SectionHeight / 2 - (universeHeight - clipOffsetY) / 2 - 1
                End If
                If originPoint.Y - (universeHeight - clipOffsetY) / 2 + 1 < -myUniverse.SectionHeight / 2 Then
                    originPoint.Y = -myUniverse.SectionHeight / 2 + (universeHeight - clipOffsetY) / 2 - 1
                End If

                'Re-calculate offsets, in case they changed.
                offsetX = endPoint.X - originPoint.X
                offsetY = endPoint.Y - originPoint.Y

                'Calculate new offset due to zooming.
                newMatrix = defaultUniverseMatrix.Clone
                For i = 1 To Math.Abs(Math.Round(zoomValue - 1, 5)) / zoomStep
                    If zoomValue > 1 Then
                        newMatrix.Scale(1 + zoomStep, 1 + zoomStep, Drawing2D.MatrixOrder.Append)
                    Else
                        newMatrix.Scale(1 / (1 + zoomStep), 1 / (1 + zoomStep), Drawing2D.MatrixOrder.Append) 'Scale.
                    End If
                Next

                'Transform offsets.
                Dim points() As PointF = {New PointF(offsetX, offsetY)}
                newMatrix.TransformPoints(points)

                'Get new offsets.
                offsetX = points(0).X
                offsetY = points(0).Y

                'Restore matrix.
                Try
                    newMatrix = myUniverseMatrix.Clone
                Catch ex As Exception
                    Console.WriteLine(ex.ToString)
                End Try

            Else
                Dim overflowDirection() As Boolean = {False, False, False, False}

                'Transform mouse points.
                Dim points() As PointF = {originPoint, endPoint}
                myInverseUniverseMatrix.TransformPoints(points)

                'Get new offsets.
                Dim newOffsetX As Double = points(1).X - points(0).X
                Dim newOffsetY As Double = points(1).Y - points(0).Y

                If newOffsetX < 0 And universeWidth - newOffsetX > myUniverse.SectionWidth / 2 Then
                    overflowDirection(0) = True
                End If
                If newOffsetX > 0 And clipOffsetX - newOffsetX < -myUniverse.SectionWidth / 2 Then
                    overflowDirection(1) = True
                End If
                If newOffsetY < 0 And universeHeight - newOffsetY > myUniverse.SectionHeight / 2 Then
                    overflowDirection(2) = True
                End If
                If newOffsetY > 0 And clipOffsetY - newOffsetX < -myUniverse.SectionHeight / 2 Then
                    overflowDirection(3) = True
                End If

                'Make movement smooth when close to boundaries.
                If offsetY <> 0 And (overflowDirection(0) Or overflowDirection(1)) And
                                Not (overflowDirection(2) Or overflowDirection(3)) Then 'Right, Left with Bottom/Top.
                    offsetX = 0

                ElseIf offsetX <> 0 And (overflowDirection(2) Or overflowDirection(3)) And
                                    Not (overflowDirection(0) Or overflowDirection(1)) Then 'Bottom, Top with Right/Left.
                    offsetY = 0

                ElseIf overflowDirection.Contains(True) Then
                    Exit Sub 'Do nothing if there is nowhere to go.
                End If

            End If

            'Move universe based on offset.
            newMatrix.Translate(offsetX, offsetY, Drawing2D.MatrixOrder.Append)

            'Update universe graphics.
            UpdateUniverseGraphics(newMatrix)

            'Give the signal to the universe to update its objects. (new clipOffset locations etc.)
            resetOffset = True

            myUniverse.ResizeUniverse(New PointF(clipOffsetX, clipOffsetY), New PointF(universeWidth, universeHeight), myUniverseMatrix)

        End If

    End Sub
    Private Sub UpdateUniverseGraphics(ByVal newMatrix As Drawing2D.Matrix)

        Dim universeGraphics As Graphics = myUniverse.getGraphics

        Try
            'Apply transformation to graphics.
            universeGraphics.Transform = newMatrix.Clone

            'Update clip coordinates.
            clipOffsetX = universeGraphics.ClipBounds.X
            clipOffsetY = universeGraphics.ClipBounds.Y

            'Transform original width and height for our new transformation.
            Dim points() As PointF = {New PointF(defaultUniverseWidth - 1, defaultUniverseHeight - 1)}
            myInverseUniverseMatrix = newMatrix.Clone
            myInverseUniverseMatrix.Invert()
            myInverseUniverseMatrix.TransformPoints(points)

            'Update universe size.
            universeWidth = points(0).X
            universeHeight = points(0).Y

            'Update local vars.
            myUniverseMatrix = newMatrix.Clone
            'totalDragOffset.X += offsetX
            'totalDragOffset.Y += offsetY

        Catch ex As InvalidOperationException 'This "Object in use" exception happens because mouse events are asynchronous.
            Console.WriteLine(ex.ToString)
            Console.WriteLine("Retrying...")
            UpdateUniverseGraphics(newMatrix)
        End Try

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Minimap navigation button functions.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub btnNavReturn_Click(sender As Object, e As EventArgs) Handles btnNavReturn.Click

        btnNavReturn.Visible = False
        pnlCosmos.Visible = True
        navigatingMinimap = True

        Me.Controls.Find("btnSection" + currentCosmosSection.ToString, True).First.BackColor = Color.Red

    End Sub

    'One function for all section buttons.
    Private Sub btnSection_Click(sender As Object, e As EventArgs) Handles btnSection1.Click, btnSection2.Click, btnSection3.Click,
                btnSection4.Click, btnSection5.Click, btnSection6.Click, btnSection7.Click, btnSection8.Click, btnSection9.Click


        Me.Controls.Find("btnSection" + currentCosmosSection.ToString, True).First.BackColor = Color.Black

        'Get new cosmos section.
        Dim newCosmosSection = CInt(CType(sender, Button).Name.Last.ToString)

        If newCosmosSection <> currentCosmosSection Then

            'Set new section.
            currentCosmosSection = newCosmosSection

            'Remove all list items.
            objectListView.Items.Clear()

            'Add new ones.
            For Each obj In myUniverse.Objects.FindAll(Function(o) o.CosmosSection = newCosmosSection)
                objectListView.Items.Add(obj.ListItem)
            Next

            'Hide hover label if it was visible before.
            hoverLabel.Visible = False

        End If

        btnNavReturn.Visible = True
        pnlCosmos.Visible = False
        navigatingMinimap = False
        Me.Controls.Find("btnSection" + currentCosmosSection.ToString, True).First.BackColor = Color.Red

    End Sub

    Private Sub cbTickSpeed_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbTickSpeed.SelectedIndexChanged

        Dim modes(4) As Object
        cbTickSpeed.Items.CopyTo(modes, 0)

        numTimeTick.Enabled = False

        If cbTickSpeed.SelectedItem = modes.ToList.Find(Function(x) x.ToString = "Fast") Then
            tickModeSelectedValue = tickModeValues(0)
        ElseIf cbTickSpeed.SelectedItem = modes.ToList.Find(Function(x) x.ToString = "Faster") Then
            tickModeSelectedValue = tickModeValues(1)
        ElseIf cbTickSpeed.SelectedItem = modes.ToList.Find(Function(x) x.ToString = "Fastest") Then
            tickModeSelectedValue = tickModeValues(2)
        ElseIf cbTickSpeed.SelectedItem = modes.ToList.Find(Function(x) x.ToString = "Custom") Then
            numTimeTick.Enabled = True
            tickModeSelectedValue = tickModeValues(3)
        End If

    End Sub


End Class


'Friend Class Vector
'    Public x As Double
'    Public y As Double

'    Public Sub New(vx As Double, vy As Double)
'        x = vx
'        y = vy
'    End Sub
'    Public Sub setTo(vx As Double, vy As Double)
'        x = vx
'        y = vy
'    End Sub
'    Public Sub copyFrom(v As Vector)
'        x = v.x
'        y = v.y
'    End Sub
'    Public Function dotProduct(v As Vector) As Double
'        Return x * v.x + y * v.y
'    End Function
'    Public Function crossProduct(v As Vector) As Double
'        Return x * v.y - y * v.x
'    End Function
'    Public Function Plus(v As Vector) As Vector
'        Return New Vector(x + v.x, y + v.y)
'    End Function
'    Public Function plusEquals(v As Vector) As Vector
'        x += v.x
'        y += v.y
'        Return Me
'    End Function
'    Public Function Minus(v As Vector) As Vector
'        Return New Vector(x - v.x, y - v.y)
'    End Function
'    Public Function minusEquals(v As Vector) As Vector
'        x -= v.x
'        y -= v.y
'        Return Me
'    End Function
'    Public Function Multiply(s As Double) As Vector
'        Return New Vector(x * s, y * s)
'    End Function
'    Public Function multEquals(s As Double) As Vector
'        x *= s
'        y *= s
'        Return Me
'    End Function
'    Public Function Times(v As Vector) As Vector
'        Return New Vector(x * v.x, y * v.y)
'    End Function
'    Public Function divEquals(s As Double) As Vector
'        If s = 0 Then
'            s = 0.0001
'        End If
'        x /= s
'        y /= s
'        Return Me
'    End Function
'    Public Function getMagnitude() As Double
'        Return Math.Sqrt(x * x + y * y)
'    End Function
'    Public Function getDistance(v As Vector) As Double
'        Dim delta As Vector = Me.Minus(v)
'        Return delta.getMagnitude()
'    End Function
'    Public Function Normalize() As Vector
'        Dim m As Double = getMagnitude()
'        If m = 0 Then
'            m = 0.0001
'        End If
'        Return Multiply(1 / m)
'    End Function
'    Public Overrides Function toString() As String
'        Return (x + " : " + y)
'    End Function
'End Class
''
''
''
'If planet.CenterOfMass.X < objectCenterOfMass.X Then
'    VelX -= Math.Abs(force * Math.Cos(theta))
'Else
'    VelX += Math.Round(force * Math.Cos(theta), 4)
'End If

'If planet.CenterOfMass.Y > objectCenterOfMass.Y Then
'    VelY += force * Math.Sin(theta)
'Else
'    VelY += Math.Abs(Math.Round(force * Math.Sin(theta), 4))
'End If

'Dim distance As Integer = Math.Sqrt(Math.Pow(myUniverse.getplanets.Last.CenterOfMass.X - myUniverse.getplanets(0).CenterOfMass.X, 2) +
'                          Math.Pow(myUniverse.getplanets.Last.CenterOfMass.Y - myUniverse.getplanets(0).CenterOfMass.Y, 2)) ' |x1-x2| + |y1-y2|

'Dim distance1 As Integer = Math.Sqrt(Math.Pow(myUniverse.Gets.Last.CenterOfMass.X - myUniverse.getplanets(0).CenterOfMass.X, 2) +
'                            Math.Pow(myUniverse.Gets.Last.CenterOfMass.Y - myUniverse.getplanets(0).CenterOfMass.Y, 2)) ' |x1-x2| + |y1-y2|

'Dim distance2 As Integer = Math.Sqrt(Math.Pow(myUniverse.Gets.Last.CenterOfMass.X - myUniverse.getplanets(1).CenterOfMass.X, 2) +
'                           Math.Pow(myUniverse.Gets.Last.CenterOfMass.Y - myUniverse.getplanets(1).CenterOfMass.Y, 2)) ' |x1-x2| + |y1-y2|

'Dim Radius As Double
'Radius = 0
'Dim endcircle As New Point(myUniverse.getplanets(1).CenterOfMass.X, myUniverse.getplanets(1).CenterOfMass.Y)
'If CheckBox1.Checked Then
'    Radius = 100
'    theta = theta + 0.01
'    phi = phi + 0.01
'    If theta > 2 * Math.PI Then
'        theta = 0
'    End If
'    If phi > 2 * Math.PI Then
'        phi = 0
'    End If
'    'myUniverse.getplanets(0).Move(1, "u")
'    'myUniverse.getplanets(1).Move(1, "r")
'    ' myUniverse.getplanets(1).Move(1, "r")
'    'myUniverse.getplanets(1).Move(1, "r")

'    'myUniverse.getplanets(0).Move(0, "", myUniverse.Gets.Last.CenterOfMass.X - 15 - (Radius * Math.Cos(phi)),
'    'myUniverse.Gets.Last.CenterOfMass.Y - 15 - (Radius * Math.Sin(phi)))
'    ' myUniverse.getplanets(1).Move(0, "", myUniverse.Gets.Last.CenterOfMass.X - 15 - (Radius * Math.Cos(theta)),
'    'myUniverse.Gets.Last.CenterOfMass.Y - 15 - (Radius * Math.Sin(theta)))

'    endcircle = New Point(myUniverse.getplanets(1).CenterOfMass.X, myUniverse.getplanets(1).CenterOfMass.Y)

'    'myUniverse.getGraphics.DrawLine(myUniverse.getPen, New point(myUniverse.Gets.Last.CenterOfMass.X, myUniverse.Gets.Last.CenterOfMass.Y), endcircle)
'End If

'myUniverse.getGraphics.TranslateTransform(190, 200)
'myUniverse.getGraphics.RotateTransform(angle)
'myUniverse.getGraphics.DrawLine(myPen, -200, 0, 200, 0)

'Dim Array As Array = myUniverse.getGraphics.Transform.Elements
'For i = 0 To Array.Length - 1
'    Me.Text = Me.Text + Array(i).ToString + " "
'Next
'myUniverse.getGraphics.ResetTransform()
'angle += 1
'myUniverse.getGraphics.DrawImage(myUniverse.getImage, New Point(imgOffset, imgOffset))
'mouseCoordslbl.Text = mousePoint.ToString + myUniverse.Gets.Last.GetGravityForce.ToString + " " + (angleX * 180 / Math.PI).ToString + " theta= " + (angleY * 180 / Math.PI).ToString + " " + " d = " + distance.ToString +
' " d1 = " + distance1.ToString + " d2 = " + distance2.ToString


'Friend Class
'star.objectVelX += -objectVelX
'star.objectVelY += -objectVelY

'star.starListGravityKnown.Add(Me) 'Add this star(Me) to the list of known gravities.

'Dim pos As New PointFD(star.objectVelX, star.objectVelY)

''Move body.
'star.Move(0, "", star.CenterOfMass.X + pos.X, star.CenterOfMass.Y + pos.Y)
