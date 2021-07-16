'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.Threading

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
    Dim formGraphics As Graphics

    Dim formDefaultWidth As Integer
    Dim formDefaultHeight As Integer

    Dim starRadius As Integer = 20
    Dim starType As Integer = 2 'Multiplier of mass. Basically we create stars with different masses.
    Dim planetSize As Integer = 10

    Dim myUniverse As New Universe
    Dim myUniverseMatrix As New Drawing2D.Matrix
    Dim myInverseUniverseMatrix As New Drawing2D.Matrix

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
    Dim absoluteMousePoint As New PointF

    Dim zoomValue As Double = 1.0 'Default zoom value.
    Dim zoomStep As Double = 0.05 'Default zoom step.

    Dim dragPoint As New PointF
    Dim dragStart As Boolean = False
    Dim resetOffset As Boolean = False

    Dim ctrlKeyDown As Boolean = False
    Dim mouseIsDown As Boolean = False
    Dim counter As Integer = 0

    Dim selectedObject As StellarObject = Nothing
    Dim followingObject As Boolean = False
    Dim relativeTrajectories As Boolean = False
    Dim hoverObject As StellarObject = Nothing
    Dim hover As Boolean = False
    Dim hoverLabel As New Label
    Dim hideHoverInfo As Boolean = False

    Dim formLoaded As Boolean = False

    'Creation mode variables.
    Dim creationMode As String

    'Flags for threads.
    Dim UniversePaused As Boolean = False
    Dim UniversePausedForDragging As Boolean = False
    Dim dragging As Boolean = False
    Dim zooming As Boolean = False
    Dim onFrame As Boolean = False
    Dim StarArrayInUseFlag As Boolean = False
    Dim PlanetArrayInUseFlag As Boolean = False
    Dim paintingStars As Boolean = False
    Dim paintingPlanets As Boolean = False
    Dim applyingstarVelocity As Boolean = False
    Dim applyingPlanetAcceleration As Boolean = False

    'Timers and threads.
    Dim debug_stopwatch As New Stopwatch()

    Dim tickValue As Integer
    Dim updateCounter As Integer = 0
    Dim frameCounter As Integer = 0
    Dim FPS As Integer = 0
    Dim fpsTimer As New Timers.Timer(1000) '1 second.

    Dim mainThread As New Thread(AddressOf UniverseLive)
    Dim paintThread As New Thread(AddressOf Frame)

    '----------------------------------------------------------------------------------------------------------------------
    'Starting/Loading world events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub blockForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dummyUniverse.Dispose() 'Destroy dummy universe.

        trajTooltip.SetToolTip(numTraj, "The number of points an object uses for its trajectory. Minimum 100 - Maximum 999999." & vbCrLf &
                                             "CPU power must be considered before applying large values.")

        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        Me.UpdateStyles()

        'Graphics for universe.
        formGraphics = Me.CreateGraphics()

        formDefaultWidth = Me.Width
        formDefaultHeight = Me.Height

        tickValue = numTimeTick.Value

        'Init universe/world for the first time.
        InitWorld(False)

        'Start main thread for calculating accelerations and movements.
        mainThread.Start()
        paintThread.Start(myUniverse.getGraphics)

    End Sub

    Private Sub InitWorld(ByVal reset As Boolean)

        'threadList.Add(New Thread(AddressOf startstars)) 'All stars are handled by a single thread.
        ' threadList.Last.Start()

        ' threadList.Add(New Thread(AddressOf createplanet)) 'Each planet is handled by a different thread.
        ' threadList.Last.Start(New PointFD(750, 350)) 'Start thread.

        'Init planet universe.
        myUniverse.Init(formGraphics, defaultUniverseMatrix, myPen, universeWidth, universeHeight, formDefaultWidth, formDefaultHeight, New PointFD(clipOffsetX, clipOffsetY), numTraj.Value)

        myUniverse.getGraphics.Clear(Color.Black)

        'Set default transformation matrix.
        defaultUniverseMatrix = myUniverse.getGraphics.Transform

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

        'startstars()

        'So basically the idea is this:
        'We calculate the gravities applied to each object from all other objects.
        'Then we move the objects. That way we eliminate the distance "errors" that occured by moving the object as soon as its accelaration was calculated.
        'Because we used threads for each object, they weren't simultaneous so by the time an object tried to calculate it's gravity from another object,
        'that object had already moved (because we already calculated its accelaration) and therefore the new object would get a new accelaration NOT EQUAL to the accelaration
        'that the other object had been applied to by this object.
        'Now we calculate the accelerations for ALL stars and THEN we move them.

        While 1

            'Stop universe if paused.
            If UniversePaused Or paintingStars Or paintingPlanets Then Continue While

            'Start calculating accelarations and move objects.
            myUniverse.Live(StarArrayInUseFlag, PlanetArrayInUseFlag)
            frameCounter += 1

            If tickValue > 0 Then
                Thread.Sleep(tickValue) 'Delay.
            End If

        End While

    End Sub
    Private Sub btnPauseUniverse_Click(sender As Object, e As EventArgs) Handles btnPauseUniverse.Click

        If UniversePaused Then
            btnPauseUniverse.Text = "Pause"
            lblStatusMessage.Text = ""
            UniversePaused = False
        Else
            btnPauseUniverse.Text = "Resume"
            lblStatusMessage.Text = "Paused"
            UniversePaused = True
        End If

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
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
            objList.AddRange(myUniverse.Objects)

            'Check if we 're hovering above stellar object.
            ObjectUnderMouse(objList)

            'If the offset is changed due to zooming/dragging, set the new offset for all stellar objects and check if they are still visible.
            If resetOffset Then

                myUniverse.ResetAllOffsets(objList) 'Start resetting.
                resetOffset = False

            End If

            For Each obj In objList.FindAll(Function(o) o.IsMerged = False And o.isVisible)

                obj.Paint(universeGraphics, zoomValue)

                If obj.IsSelected Then

                    'Follow object, if said so.
                    followObject(followingObject)

                    'Hide label, if said show.
                    If hideHoverInfo Then
                        Me.Invoke(New hideHoverLabelDelegate(AddressOf hideHoverLabel))
                    Else
                        Me.Invoke(New showHoverLabelDelegate(AddressOf showHoverLabel), New Object() {obj})
                    End If

                ElseIf selectedObject Is Nothing And hoverObject IsNot Nothing Then
                        Me.Invoke(New showHoverLabelDelegate(AddressOf showHoverLabel), New Object() {hoverObject})
                End If

                'Tunneling.
                If obj.isStar And radCollisionTunnel.Checked And Not dragStart Then

                    Star_CheckForLeftTunneling(obj, obj.CenterOfMass, universeGraphics)   'Left Wall.
                    Star_CheckForRightTunneling(obj, obj.CenterOfMass, universeGraphics)  'Right Wall.
                    Star_CheckForTopTunneling(obj, obj.CenterOfMass, universeGraphics)    'Top Wall.
                    Star_CheckForBottomTunneling(obj, obj.CenterOfMass, universeGraphics) 'Bottom Wall.

                ElseIf radCollisionTunnel.Checked And Not dragStart Then

                    Planet_CheckForLeftTunneling(obj, obj.CenterOfMass, universeGraphics)
                    Planet_CheckForRightTunneling(obj, obj.CenterOfMass, universeGraphics)
                    Planet_CheckForTopTunneling(obj, obj.CenterOfMass, universeGraphics)
                    Planet_CheckForBottomTunneling(obj, obj.CenterOfMass, universeGraphics)
                End If

            Next

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        'debug_stopwatch.Stop()
        'Console.WriteLine("  Painted stars in: " + debug_stopwatch.ElapsedMilliseconds.ToString)
        paintingStars = False
        paintingPlanets = False

    End Sub
    Private Sub Frame(ByVal universeGraphics As Graphics)

        While 1
            onFrame = True

            'Don't paint objects while zooming/moving to avoid "object in use" exceptions.
            If zooming Or dragging Or StarArrayInUseFlag Or PlanetArrayInUseFlag Then Continue While

            'Update world.
            Try
                If chkCanvasClear.Checked Then
                    universeGraphics.Clear(Color.Black) 'Clear image.
                End If

                'Draw objects.
                PaintUniverseObjects(universeGraphics)
                Me.Invoke(New InvalidateImageDelegate(AddressOf InvalidateImage))

            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

            onFrame = False
            Thread.Sleep(15) 'Delay 15 ms because 1ms is too fast and it causes flickering.

        End While

    End Sub
    Private Sub ResetFPS()

        FPS = frameCounter 'Set total frames per second.
        frameCounter = 0 'Reset fps counter.

    End Sub
    Private Delegate Sub InvalidateImageDelegate()
    Private Sub InvalidateImage()
        Me.Invalidate(New Region(myUniverse.getImage.GetBounds(GraphicsUnit.Pixel)))
    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Reset events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub resetBtn_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ResetUniverse()
    End Sub
    Private Sub ResetUniverse()

        Timer1.Enabled = False

        For Each thread In threadList
            thread.Abort()
        Next
        threadList.RemoveRange(0, threadList.Count)

        numParamXVel.Value = 0
        numParamYVel.Value = 0
        numTimeTick.Value = 1

        myUniverse.Stars.RemoveRange(0, myUniverse.Stars.Count)
        myUniverse.Planets.RemoveRange(0, myUniverse.Planets.Count)

        chkCanvasClear.Checked = True
        radModePlanet.Checked = True
        radCollisionTunnel.Checked = True

        'Reset clip offset.
        clipOffsetX = defaultClipOffset
        clipOffsetY = defaultClipOffset
        'clipOffsetWidth = defaultClipOffset
        'clipOffsetHeight = defaultClipOffset

        zoomValue = 1.0 'Reset zoom level.

        InitWorld(True) 'Init world again.

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
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

            Dim newPen As New Pen(star.Color)

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

            Dim newPen As New Pen(star.Color)

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

            Dim newPen As New Pen(star.Color)

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

            Dim newPen As New Pen(star.Color)

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

            Dim newPen As New Pen(star.Color)

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

            Dim newPen As New Pen(star.Color)

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

            Dim newPen As New Pen(star.Color)

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

            Dim newPen As New Pen(star.Color)

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
        If planet.GetVertices(0).X < clipOffsetX + planetBorderWidth And Not planet.TransitionDirection.Contains("r") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("l") Then
                planet.TransitionDirection = planet.TransitionDirection + "l"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.GetVertices(0).X + universeWidth - clipOffsetX, planet.GetVertices(0).Y)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            planet.DuplicatePointLeft = duplicatePoint 'Set planet duplicate point.
        End If

        'Move the planet, but just once.
        If center.X < clipOffsetX Then
            planet.Move(0, "", center.X + universeWidth - clipOffsetX, center.Y)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If planet.GetVertices(0).X > universeWidth - planetBorderWidth - planetSize And planet.TransitionDirection.Contains("l") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint As PointF = New PointF(planet.GetVertices(0).X - universeWidth + clipOffsetX, planet.GetVertices(0).Y)
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
        If planet.GetVertices(0).X > universeWidth - planetBorderWidth - planetSize And Not planet.TransitionDirection.Contains("l") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("r") Then
                planet.TransitionDirection = planet.TransitionDirection + "r"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.GetVertices(0).X - universeWidth + clipOffsetX, planet.GetVertices(0).Y)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            planet.DuplicatePointRight = duplicatePoint 'Set planet duplicate point.
        End If

        'Move the planet, but just once.
        If center.X > universeWidth Then
            planet.Move(0, "", center.X - universeWidth + clipOffsetX, center.Y)
        End If

        'Center of Mass moved. Draw the duplicate ellipse in it's position to preserve the transition.
        If planet.GetVertices(0).X < clipOffsetX + planetBorderWidth And planet.TransitionDirection.Contains("r") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(planet.GetVertices(0).X + universeWidth - clipOffsetX, planet.GetVertices(0).Y)
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
        If planet.GetVertices(0).Y < clipOffsetY + planetBorderWidth And Not planet.TransitionDirection.Contains("b") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("t") Then
                planet.TransitionDirection = planet.TransitionDirection + "t"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.GetVertices(0).X, planet.GetVertices(0).Y + universeHeight - clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the bottom-right corner.
            If planet.GetVertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            ElseIf planet.GetVertices(0).X > universeWidth - planetBorderWidth - planetSize Then 'Else if the planet is also tunneling to the right, draw a second duplicate in the bottom-left corner.

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
        If planet.GetVertices(0).Y > universeHeight - planetBorderWidth - planetSize And planet.TransitionDirection.Contains("t") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(planet.GetVertices(0).X, planet.GetVertices(0).Y - universeHeight + clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the top-right corner.
            If planet.GetVertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(planet.GetVertices(0).X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            ElseIf planet.GetVertices(0).X > universeWidth - planetBorderWidth - planetSize Then  'Else if the planet is also tunneling to the right, draw a second duplicate in the top-left corner.

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
        If planet.GetVertices(0).Y > universeHeight - planetBorderWidth - planetSize And Not planet.TransitionDirection.Contains("t") Then

            'Add transition direction.
            If Not planet.TransitionDirection.Contains("b") Then
                planet.TransitionDirection = planet.TransitionDirection + "b"
            End If

            'Create duplicate planet for transition purposes.
            Dim duplicatePoint = New PointF(planet.GetVertices(0).X, planet.GetVertices(0).Y - universeHeight + clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the bottom-right corner.
            If planet.GetVertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate of duplicate planet.

            ElseIf planet.GetVertices(0).X > universeWidth - planetBorderWidth - planetSize Then 'Else if the planet is going through the bottom-right corner, draw duplicate on top-right corner.

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
        If planet.GetVertices(0).Y < clipOffsetY + planetBorderWidth And planet.TransitionDirection.Contains("b") Then

            'Calculate duplicate origin point.
            Dim duplicatePoint = New PointF(planet.GetVertices(0).X, planet.GetVertices(0).Y + universeHeight - clipOffsetY)
            DrawDuplicatePlanet(duplicatePoint, planet.Universe.getPen, universeGraphics) 'Draw duplicate planet.

            'If the planet is also tunneling to the left, draw a second duplicate in the top-right corner.
            If planet.GetVertices(0).X < clipOffsetX + planetBorderWidth Then

                'Calculate second duplicate origin point.
                Dim secondDuplicatePoint As New PointF(duplicatePoint.X + universeWidth - clipOffsetX, duplicatePoint.Y)
                DrawDuplicatePlanet(secondDuplicatePoint, planet.Universe.getPen, universeGraphics)

            ElseIf planet.GetVertices(0).X > universeWidth - planetBorderWidth - planetSize Then 'Else if the planet is also tunneling to the right, draw a second duplicate in the top-left corner.

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
    '----------------------------------------------------------------------------------------------------------------------
    'Object Creation events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub createstar(ByVal data() As PointFD)

        Dim newstar As New Star

        While applyingstarVelocity Or paintingStars Or StarArrayInUseFlag 'Wait until star list is free.

        End While

        StarArrayInUseFlag = True

        'Set new random color.
        myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Initialize and add new star to the universe.
        newstar.Init(myUniverse, myUniverseMatrix, data(0), starRadius, starBorderWidth, myBrush.Color, starType, data(1))

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
        'threadList.Remove(Thread.CurrentThread)
        'Thread.CurrentThread.

    End Sub
    Private Sub createplanet(ByVal data() As PointFD)

        Dim newplanet As New Planet

        While applyingPlanetAcceleration Or paintingPlanets Or PlanetArrayInUseFlag  'Wait until planet list is free.

        End While

        PlanetArrayInUseFlag = True

        'Initialize and add new planet to planet World.
        newplanet.Init(myUniverse, myUniverseMatrix, data(0), planetSize, planetBorderWidth, planetColor, data(1))

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
        'threadList.Remove(Thread.CurrentThread)
        'Thread.CurrentThread.Abort()

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Timer events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        'Start counting.
        If mouseIsDown Then
            counter += 1
        Else
            updateCounter += 1

            'Update coordinates while moving.
            If followingObject Then
                Dim s As New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) 'Init empty mouse event.
                Me.OnMouseMove(s) 'Update UI points.
            End If

            If updateCounter >= 3 Then

                If Not UniversePaused Then
                    UpdateLabels()
                End If

                updateCounter = 0
            End If
        End If

    End Sub
    Private Sub UpdateLabels()

        Dim objList As New List(Of StellarObject)
        objList.AddRange(myUniverse.Objects)

        If objList.Count > 0 Then

            Dim removedListItems As New List(Of Integer) 'List of list item indexes to be removed.

            For Each item In objectListView.Items

                'Get stellar object of list item.
                Dim obj As StellarObject = objList.Find(Function(o)
                                                            Return o.ListItem.Equals(item)

                                                        End Function)

                'If stellar object is not found, that means it got merged. Remove item from list later.
                If obj Is Nothing Then
                    removedListItems.Add(objectListView.Items.IndexOf(item))
                    Continue For
                End If

                UpdateListItem(obj, item)

                'This updates our local var to the new selected object, if the previous one merged.
                If obj.IsSelected Then
                    selectedObject = obj
                End If

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
            Dim brightness As Single = obj.ListItem.ForeColor.GetBrightness
            objectItem.ForeColor = obj.ListItem.ForeColor

            If brightness > 0.4 Then
                objectItem.BackColor = Color.Black
            Else
                objectItem.BackColor = Color.White
            End If

            'Get group to assign the object to.
            Dim group As String
            Dim name As String

            If obj.isStar Then
                group = "Stars"
                name = "Star"
            ElseIf obj.isPlanet Then
                group = "Planets"
                name = "Planet"
            Else
                group = "Default"
                name = ""
            End If

            'Get index of item.
            Dim index As Integer = objectListView.Groups(group).Items.Count
            objectItem.Name = name + index.ToString
            objectItem.Text = objectItem.Name

            'Now add sub-items, one for each statistic (location, acceleration, etc.) we want to display. The name is displayed in the first (0) item.
            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(1).Name = "objItemLocX"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(2).Name = "objItemLocY"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(3).Name = "objItemAcc"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(4).Name = "objItemAccX"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(5).Name = "objItemAccY"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(6).Name = "objItemVel"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(7).Name = "objItemVelX"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(8).Name = "objItemVelY"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(9).Name = "objItemMass"

            objectItem.SubItems.Add("")
            objectItem.SubItems.Item(10).Name = "objItemSize"

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
            Dim objAccX As String = obj.AccX.ToString
            Dim objAccY As String = obj.AccY.ToString
            Dim objVelX As String = obj.VelX.ToString
            Dim objVelY As String = obj.VelY.ToString
            Dim objSize As String

            If obj.isStar Then
                objSize = obj.Radius.ToString
            ElseIf obj.isPlanet Then
                objSize = obj.Size.ToString
            Else
                objSize = obj.Radius.ToString
            End If

            Dim brightness As Single = obj.ListItem.ForeColor.GetBrightness

            If brightness > 0.4 Then
                objectItem.BackColor = Color.Black
            Else
                objectItem.BackColor = Color.White
            End If

            'Update stats.
            '-------------------------------------------------------------------------------------------------------------
            If objectItem.SubItems.Item("objItemLocX").Text <> objLocX Then
                objectItem.SubItems.Item("objItemLocX").Text = objLocX
            End If
            If objectItem.SubItems.Item("objItemLocY").Text = objLocY <> objLocY Then
                objectItem.SubItems.Item("objItemLocY").Text = objLocY
            End If

            If objectItem.SubItems.Item("objItemAcc").Text <> (obj.AccX + obj.AccY).ToString Then
                objectItem.SubItems.Item("objItemAcc").Text = obj.AccX + obj.AccY
            End If
            If objectItem.SubItems.Item("objItemAccX").Text <> objAccX Then
                objectItem.SubItems.Item("objItemAccX").Text = objAccX
            End If
            If objectItem.SubItems.Item("objItemAccY").Text <> objAccY Then
                objectItem.SubItems.Item("objItemAccY").Text = objAccY
            End If

            If objectItem.SubItems.Item("objItemVel").Text <> (obj.VelX + obj.VelY).ToString Then
                objectItem.SubItems.Item("objItemVel").Text = obj.VelX + obj.VelY
            End If
            If objectItem.SubItems.Item("objItemVelX").Text <> objVelX Then
                objectItem.SubItems.Item("objItemVelX").Text = objVelX
            End If
            If objectItem.SubItems.Item("objItemVelY").Text <> objVelY Then
                objectItem.SubItems.Item("objItemVelY").Text = objVelY
            End If

            If objectItem.SubItems.Item("objItemMass").Text <> obj.Mass.ToString Then
                objectItem.SubItems.Item("objItemMass").Text = obj.Mass.ToString
            End If
            If objectItem.SubItems.Item("objItemSize").Text <> objSize Then
                objectItem.SubItems.Item("objItemSize").Text = objSize
            End If
            '-------------------------------------------------------------------------------------------------------------

        End If

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Form/click/resize/keydown events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub blockForm_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove

        If formLoaded Then

            mousePoint = PointToClient(MousePosition)
            absoluteMousePoint = mousePoint 'Save absolute point before scaling it.

            'Check always with absolute point.
            'If mouse out of boundaries, do nothing.
            If absoluteMousePoint.X >= imageWidth Or absoluteMousePoint.Y >= imageHeight Or
               absoluteMousePoint.X < defaultClipOffset Or absoluteMousePoint.Y < defaultClipOffset Then

                lblCoordsMouse.Text = "Out Of Bounds"
                lblCoordsAbs.Text = "Out Of Bounds"
                Exit Sub
            Else
                Dim points() As PointF = {mousePoint}

                Try
                    myInverseUniverseMatrix.TransformPoints(points)
                Catch ex As Exception
                    Console.WriteLine(ex.ToString)
                End Try

                mousePoint = points(0)

                lblCoordsMouse.Text = mousePoint.ToString
                lblCoordsAbs.Text = absoluteMousePoint.ToString
            End If

            CheckPlanetContinuousMode(e) 'Check for planet continuous mode creation.
            CheckDragging() 'Check for universe dragging.
            ObjectHover() 'Check for object hover.

        End If

    End Sub
    Private Sub blockForm_Click(sender As Object, e As EventArgs) Handles Me.Click

        'Do nothing until offset changes when dragging.
        If dragStart Then
            Exit Sub
        End If

        If creationMode.Equals("p") And Not hover Then

            Dim planetHalfSize As Double = planetSize / 2

            'If planet size exceeds boundaries, don't create anything.
            If mousePoint.X + planetHalfSize + planetBorderWidth > myUniverse.getWidth Or
                mousePoint.Y + planetHalfSize + planetBorderWidth > myUniverse.getHeight Or
                mousePoint.X - planetHalfSize - planetBorderWidth < clipOffsetX - 1 Or
                mousePoint.Y - planetHalfSize - planetBorderWidth < clipOffsetY - 1 Then

                Exit Sub

            End If

            threadList.Add(New Thread(AddressOf createplanet)) 'Each planet is handled by a different thread.
            threadList.Last.Start(New PointFD() {New PointFD(mousePoint.X, mousePoint.Y), New PointFD(numParamXVel.Value, numParamYVel.Value),
                                  New PointFD(absoluteMousePoint.X, absoluteMousePoint.Y)}) 'Start thread.

        ElseIf creationMode.Equals("s") And Not hover Then

            'If star size exceeds boundaries.
            If mousePoint.X + starRadius + planetBorderWidth > myUniverse.getWidth Or
                mousePoint.Y + starRadius + planetBorderWidth > myUniverse.getHeight Or
                mousePoint.X - starRadius - planetBorderWidth < clipOffsetX - 1 Or
                mousePoint.Y - starRadius - planetBorderWidth < clipOffsetY - 1 Then

                Exit Sub

            End If

            threadList.Add(New Thread(AddressOf createstar)) 'Each star is handled by a different thread.
            threadList.Last.Start(New PointFD() {New PointFD(mousePoint.X, mousePoint.Y), New PointFD(numParamXVel.Value, numParamYVel.Value),
                                  New PointFD(absoluteMousePoint.X, absoluteMousePoint.Y)}) 'Start thread.

        Else
            ObjectSelected() 'We selected an object.
        End If

        dummyUniverse.Focus()

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
        'So we figure out what values need to multiplied with the current zoomed matrix to match the next appropriate step.
        'We need: 1.25 * X = 1.5 --> X = 1.5/1.25 --> newValue = appropriateValue / previousZoomValue
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

    Private Sub CheckPlanetContinuousMode(e As MouseEventArgs)

        If ctrlKeyDown And mouseIsDown And creationMode = "p" And counter > 2 Then

            dragStart = False
            Me.OnClick(e)
            counter = 0

        End If

    End Sub
    Private Sub CheckDragging()

        'How much to move mouse before starting the drag.
        Dim dragOffset As PointF = New PointF(Math.Abs(dragPoint.X - absoluteMousePoint.X), Math.Abs(dragPoint.Y - absoluteMousePoint.Y))

        If mouseIsDown And (dragOffset.X > 20 Or dragOffset.Y > 20) Then
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
                moveUniverse(dragPoint, absoluteMousePoint)

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
    Private Sub ObjectSelected()

        'If clicked on nothing, clear selection.
        If Not hover Then

            If selectedObject IsNot Nothing Then
                selectedObject.IsSelected = False
                selectedObject = Nothing
            End If

        Else

            If selectedObject IsNot Nothing Then

                selectedObject.IsSelected = False

                If selectedObject.Equals(hoverObject) Then 'If clicked on self, clear selection.
                    selectedObject = Nothing
                    Me.Focus()
                    Exit Sub
                End If

            End If

            selectedObject = hoverObject 'Set new selected object.
            selectedObject.IsSelected = True

            'Reset trajectories if followed object changed.
            If followingObject Then
                For Each obj In myUniverse.Objects
                    obj.ClearTrajectory()
                    obj.ClearRelativeDistances()
                Next
            End If

        End If

    End Sub
    Private Sub ObjectUnderMouse(ByVal objList As List(Of StellarObject))

        hover = False 'Reset flag.

        For Each obj In objList

            If obj.isStar Then

                'Calculate if the point is inside the ellipse area.
                Dim ellipseEquation As Double = (mousePoint.X - obj.CenterOfMass.X) ^ 2 / (obj.Radius ^ 2) + (mousePoint.Y - obj.CenterOfMass.Y) ^ 2 / (obj.Radius ^ 2)

                If Math.Round(ellipseEquation, 2) <= 1 Then

                    hoverObject = objList.Item(objList.IndexOf(obj)) 'Get reference to original item not the local copy.
                    hover = True 'Set flag.
                    Exit For

                End If

            ElseIf obj.isPlanet Then
                Dim planet As Planet = CType(obj, Planet) 'Cast object as planet.
                Dim rec As New RectangleF(New PointF(planet.GetVertices(0).X, planet.GetVertices(0).Y), New Size(planet.Size + planet.GetBorderWidth, planet.Size + planet.GetBorderWidth))

                If rec.Contains(mousePoint) Then
                    hoverObject = objList.Item(objList.IndexOf(obj)) 'Get reference to original item not the local copy.
                    hover = True 'Set flag.
                    Exit For
                End If

            End If

        Next

    End Sub

    Private Delegate Sub showHoverLabelDelegate(ByVal obj As StellarObject)
    Private Sub showHoverLabel(ByVal obj As StellarObject)

        If obj IsNot Nothing Then

            Dim objLocX As String = Math.Round(obj.CenterOfMass.X, 2).ToString
            Dim objLocY As String = Math.Round(obj.CenterOfMass.Y, 2).ToString
            Dim objAccX As String = Math.Round(obj.AccX, 4).ToString
            Dim objAccY As String = Math.Round(obj.AccY, 4).ToString
            Dim objVelX As String = Math.Round(obj.VelX, 4).ToString
            Dim objVelY As String = Math.Round(obj.VelY, 4).ToString

            If objAccX = "0" And obj.AccX <> 0 Then
                objAccX = "~0"
            End If
            If objAccY = "0" And obj.AccY <> 0 Then
                objAccY = "~0"
            End If
            If objVelX = "0" And obj.VelX <> 0 Then
                objVelX = "~0"
            End If
            If objVelY = "0" And obj.VelY <> 0 Then
                objVelY = "~0"
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

            Dim objCenter() As PointF = {New PointF(obj.CenterOfMass.X + obj.Radius, obj.CenterOfMass.Y - obj.Radius)}
            obj.UniverseMatrix.TransformPoints(objCenter)

            'Set label position under the mouse.
            hoverLabel.Text = "L: " + objLocX + " - " + objLocY + vbCrLf + "A: " + objAccX + " - " + objAccY + vbCrLf + "V: " + objVelX + " - " + objVelY
            hoverLabel.Location = New Point(objCenter(0).X, objCenter(0).Y - hoverLabel.Size.Height)
            hoverLabel.Visible = True
            hoverLabel.BringToFront()

        End If

    End Sub
    Private Delegate Sub hideHoverLabelDelegate()
    Private Sub hideHoverLabel()

        hoverLabel.Visible = False

    End Sub
    Private Sub hoverLabelMouseMove()

        'We need this so we can catch mouse movemenets above objects that are behind labels.
        Me.OnMouseMove(New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0))

    End Sub
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Options events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub radModePlanet_CheckedChanged(sender As Object, e As EventArgs) Handles radModePlanet.CheckedChanged

        If radModePlanet.Checked Then
            creationMode = "p"
        End If

    End Sub
    Private Sub radModeStar_CheckedChanged(sender As Object, e As EventArgs) Handles radModeStar.CheckedChanged

        If radModeStar.Checked Then
            creationMode = "s"
        End If

    End Sub
    Private Sub radModeNothing_CheckedChanged(sender As Object, e As EventArgs) Handles radModeNothing.CheckedChanged

        If radModeNothing.Checked Then
            creationMode = ""
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

        If followingObject = False Then
            radRealTraj.Checked = True
        End If

        followObject(followingObject)

    End Sub
    Private Sub chkHideHover_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideInfo.CheckedChanged

        hideHoverInfo = chkHideInfo.Checked

    End Sub

    Private Sub numTimeTick_ValueChanged(sender As Object, e As EventArgs) Handles numTimeTick.ValueChanged

        tickValue = numTimeTick.Value

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

    Private Sub startstars()

        Dim pos As New PointFD(450, 250)
        Dim pos2 As New PointFD(550, 150)
        Dim pos3 As New PointFD(650, 250)
        Dim pos4 As New PointFD(550, 350)

        'Set new random color.
        myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Add and initialize new star to universe.
        myUniverse.AddStar(New Star)
        myUniverse.Stars.Last.Init(myUniverse, defaultUniverseMatrix, pos, starRadius, starBorderWidth, myBrush.Color, starType, New PointFD(0, -5))
        AddListItem(myUniverse.Stars.Last, myUniverse.Objects)
        'Set new random color.
        myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Add and initialize new star to universe.
        'myUniverse.AddStar(New Star)
        'myUniverse.Stars.Last.Init(myUniverse, pos2, starRadius, starBorderWidth, myBrush.Color, starType, 30, 0)
        'AddObjStatsLabel(myUniverse.Stars.Last)
        'Set new random color.
        myBrush = New SolidBrush(Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))

        'Add and initialize new star to universe.
        myUniverse.AddStar(New Star)
        myUniverse.Stars.Last.Init(myUniverse, defaultUniverseMatrix, pos3, starRadius, starBorderWidth, myBrush.Color, starType, New PointFD(0, -5))
        AddListItem(myUniverse.Stars.Last, myUniverse.Objects)

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
    Private Sub followObject(ByVal checked As Boolean)

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
            moveUniverse(objCenter, universeDefaultCenter)
        End If

    End Sub

    Private Sub zoomUniverse(ByVal zoomIn As Boolean)

        If formLoaded And Not onFrame Then

            Dim zoomPoint As PointF = absoluteMousePoint
            Dim universeMatrix As Drawing2D.Matrix = myUniverseMatrix.Clone

            'If an object is selected, make its center the zoom origin point.
            If selectedObject IsNot Nothing Then

                'We need the center of the object relative to the universe image.
                Dim relativeCenter() As PointF = {New Point(selectedObject.CenterOfMass.X, selectedObject.CenterOfMass.Y)}

                universeMatrix.TransformPoints(relativeCenter)
                zoomPoint = relativeCenter(0)

            End If

            'Apply transformations. Move -> Scale -> Move back
            universeMatrix.Translate(-zoomPoint.X, -zoomPoint.Y, Drawing2D.MatrixOrder.Append) 'Translate to opposite original point.

            'Scale.
            If zoomIn Then
                universeMatrix.Scale(1 + zoomStep, 1 + zoomStep, Drawing2D.MatrixOrder.Append)
            Else
                universeMatrix.Scale(1 / (1 + zoomStep), 1 / (1 + zoomStep), Drawing2D.MatrixOrder.Append) 'Scale.
            End If

            'Because sometimes the above scaling method leaves trailing nines (9) and zeros (0), we manually set it to the correct zoom value.
            'After the scaling of course. This changes the width and height of the Clip rectangle by whatever the rounding error was (pretty miniscule).
            'cloneMatrix = New Drawing2D.Matrix(zoomValue, 0, 0, zoomValue, cloneMatrix.OffsetX, cloneMatrix.OffsetY) 'Set new matrix with proper scale.
            universeMatrix.Translate(zoomPoint.X, zoomPoint.Y, Drawing2D.MatrixOrder.Append) 'Translate it to original point.

            'Access graphics object only once to avoid "object in use" exceptions.
            Dim universeGraphics As Graphics = myUniverse.getGraphics

            'Apply transformation to graphics.
            myUniverseMatrix = universeMatrix.Clone
            universeGraphics.Transform = myUniverseMatrix

            'Transform original width and height so we can use them as limits to our new transformation.
            Dim points() As PointF = {New PointF(defaultUniverseWidth - 1, defaultUniverseHeight - 1)}
            myInverseUniverseMatrix = universeMatrix.Clone
            myInverseUniverseMatrix.Invert()
            myInverseUniverseMatrix.TransformPoints(points)

            'Update clip coordinates. Notice the clip limits don't need to be transformed since they already are.
            clipOffsetX = universeGraphics.ClipBounds.X
            clipOffsetY = universeGraphics.ClipBounds.Y
            universeWidth = points(0).X
            universeHeight = points(0).Y

            'Give the signal to the universe to update its objects. (new clipOffset locations etc.)
            resetOffset = True

            'Give the signal to the universe to update its objects. (new clipOffset locations etc.)
            myUniverse.ResizeUniverse(universeGraphics.ClipBounds.Location, points(0), myUniverseMatrix)

        End If

    End Sub
    Private Sub moveUniverse(ByVal originPoint As PointF, ByVal endPoint As PointF)

        If formLoaded Then

            Dim offsetX As Double = endPoint.X - originPoint.X
            Dim offsetY As Double = endPoint.Y - originPoint.Y

            Dim newMatrix As Drawing2D.Matrix = myUniverseMatrix.Clone

            'Move universe based on offset.
            newMatrix.Translate(offsetX, offsetY, Drawing2D.MatrixOrder.Append)

            'Access graphics object only once to avoid "object in use" exceptions.
            Dim universeGraphics As Graphics = myUniverse.getGraphics

            Try
                'Apply transformation to graphics.
                universeGraphics.Transform = newMatrix.Clone

            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try


            'Update local vars.
            myUniverseMatrix = newMatrix.Clone
            totalDragOffset.X += offsetX
            totalDragOffset.Y += offsetY

            'Transform original width and height so we can use them as limits to our new transformation.
            Dim points() As PointF = {New PointF(defaultUniverseWidth - 1, defaultUniverseHeight - 1)}
            myInverseUniverseMatrix = newMatrix.Clone
            myInverseUniverseMatrix.Invert()
            myInverseUniverseMatrix.TransformPoints(points)

            'Update clip coordinates.
            clipOffsetX = universeGraphics.ClipBounds.X
            clipOffsetY = universeGraphics.ClipBounds.Y
            universeWidth = points(0).X
            universeHeight = points(0).Y

            'Give the signal to the universe to update its objects. (new clipOffset locations etc.)
            resetOffset = True

            myUniverse.ResizeUniverse(New PointF(clipOffsetX, clipOffsetY), New PointF(universeWidth, universeHeight), myUniverseMatrix)

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
