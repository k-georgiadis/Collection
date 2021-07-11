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

    'Clip offset width/height because when zooming it changes. It's X/Y coordinate isn't its length anymore.
    'Dim clipOffsetWidth As Integer = Math.Abs(clipOffsetX - left_top_boundary(0).X)
    'Dim clipOffsetHeight As Integer = Math.Abs(clipOffsetY - left_top_boundary(0).Y)

    Dim mousePoint As New PointF
    Dim absoluteMousePoint As New PointF

    Dim zoomValue As Double = 1.0 'Default zoom value.
    Dim zoomStep As Double = 0.05 'Default zoom step.

    Dim threadList As New List(Of Threading.Thread)

    Dim objectLabelList As New List(Of Label)
    Dim labelSpace As Integer = 20
    Dim moveLabels As Boolean = False

    Dim dragPoint As New PointF
    Dim dragStart As Boolean = False
    Dim oldOffset As New PointF(0, 0)

    Dim ctrlKeyDown As Boolean = False
    Dim mouseIsDown As Boolean = False
    Dim counter As Integer = 0

    Dim selectedObject As StellarObject = Nothing
    Dim hoverObject As StellarObject = Nothing
    Dim hover As Boolean = False

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
        myUniverse.Init(formGraphics, myPen, universeWidth, universeHeight, formDefaultWidth, formDefaultHeight, New PointFD(clipOffsetX, clipOffsetY),
                        generalTraj.Checked, numTraj.Value, collisionBounce.Checked)
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
            If UniversePaused Then Continue While

            myUniverse.Live(StarArrayInUseFlag, PlanetArrayInUseFlag) 'Start calculating accelarations and move objects.
            frameCounter += 1 'Count frames.

            If tickValue > 0 Then
                Thread.Sleep(tickValue) 'Delay.
            End If

        End While

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

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Try

            For Each obj In objList.FindAll(Function(o) o.IsMerged = False And o.isVisible)

                obj.Paint(universeGraphics, zoomValue)

                'Tunneling.
                If obj.isStar And collisionTunnel.Checked And Not dragStart Then

                    Star_CheckForLeftTunneling(obj, obj.CenterOfMass, universeGraphics)   'Left Wall.
                    Star_CheckForRightTunneling(obj, obj.CenterOfMass, universeGraphics)  'Right Wall.
                    Star_CheckForTopTunneling(obj, obj.CenterOfMass, universeGraphics)    'Top Wall.
                    Star_CheckForBottomTunneling(obj, obj.CenterOfMass, universeGraphics) 'Bottom Wall.

                ElseIf collisionTunnel.Checked And Not dragStart Then

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
    Private Delegate Sub FrameDelegate()
    Private Sub Frame(ByVal universeGraphics As Graphics)

        While 1
            onFrame = True

            'Don't paint objects while zooming/moving to avoid "object in use" exceptions.
            If zooming Or dragging Or StarArrayInUseFlag Or PlanetArrayInUseFlag Then Continue While

            'Update world.
            Try
                If canvasClear.Checked Then
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

        For Each label In objectLabelList
            label.Parent = Nothing
        Next
        objectLabelList.RemoveRange(0, objectLabelList.Count)

        lblInfoVel.Visible = False
        lblInfoAcc.Visible = False

        numPlanetParamXVel.Value = 0
        numPlanetParamYVel.Value = 0
        numStarParamXVel.Value = 0
        numStarParamYVel.Value = 0
        numTimeTick.Value = 1

        myUniverse.Stars.RemoveRange(0, myUniverse.Stars.Count)
        myUniverse.Planets.RemoveRange(0, myUniverse.Planets.Count)

        canvasClear.Checked = True
        modePlanet.Checked = True
        collisionTunnel.Checked = True

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
        newstar.Init(myUniverse, myUniverseMatrix, data(0), starRadius, starBorderWidth, myBrush.Color, starType, data(1).X, data(1).Y)

        myUniverse.AddStar(newstar)
        AddObjStatsLabel(newstar)

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
    Private Sub createplanet(ByVal data() As PointFD)

        Dim newplanet As New Planet

        While applyingPlanetAcceleration Or paintingPlanets Or PlanetArrayInUseFlag  'Wait until planet list is free.

        End While

        PlanetArrayInUseFlag = True

        'Initialize and add new planet to planet World.
        newplanet.Init(myUniverse, myUniverseMatrix, data(0), planetSize, planetBorderWidth, planetColor, data(1).X, data(1).Y)

        myUniverse.AddPlanet(newplanet)
        AddObjStatsLabel(newplanet)

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

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Timer events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub numTimeTick_ValueChanged(sender As Object, e As EventArgs) Handles numTimeTick.ValueChanged

        tickValue = numTimeTick.Value

    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If mouseIsDown Then
            counter = counter + 1 'Start counting.
        End If

        UpdateLabels()

    End Sub
    Private Sub UpdateLabels()

        Dim objList As New List(Of StellarObject)
        objList.AddRange(myUniverse.Objects)

        If objList.Count > 0 Then

            Dim index As Integer = 0

            For Each obj In objList

                If obj.IsLabelHidden Then
                    Continue For
                End If

                UpdateObjStats(obj, index)
                index += 1

            Next

            boxObjectList.Update()

        End If

    End Sub

    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Stats label events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Delegate Sub AddObjStatsLabelDelegate(ByVal obj As StellarObject)
    Private Sub AddObjStatsLabel(ByVal obj As StellarObject)

        If boxObjectList.InvokeRequired Then
            Try
                panelObjectList.Invoke(New AddObjStatsLabelDelegate(AddressOf AddObjStatsLabel), New Object() {obj})
            Catch ex As Exception
                End
            End Try
        Else
            Dim firstPlanetLabelIndex As Integer = objectLabelList.FindIndex(0, Function(l) l.Text.Contains("Planet") = True)
            Dim lastObjectIndex As Integer = myUniverse.Objects.FindAll(Function(o) o.IsMerged = False).Count - 1

            If obj.isStar And firstPlanetLabelIndex >= 0 Then
                lastObjectIndex = firstPlanetLabelIndex 'Replace index with proper one for stars.
            End If

            Dim objectLocation As New Label
            Dim brightness As Single = obj.Label.ForeColor.GetBrightness

            'Set positions for locations.
            objectLocation.Parent = panelObjectList
            objectLocation.Location = New Point(labelSpace / 2, lastObjectIndex * labelSpace)
            objectLocation.Font = New Font("Calibri", 11.25)
            objectLocation.AutoSize = True
            objectLocation.ForeColor = obj.Label.ForeColor

            If brightness > 0.4 Then
                objectLocation.BackColor = Color.Black
            Else
                objectLocation.BackColor = Color.White
            End If

            'Set label status.
            obj.IsLabelHidden = False 'Not hidden.

            'Add label to list.
            'Add star label behind planet labels.
            If obj.isStar Then

                'In case no planets are added yet.
                If firstPlanetLabelIndex < 0 Then
                    objectLabelList.Add(objectLocation) 'Just add them at the end as we normally would.
                Else
                    moveLabels = True 'Signal main thread to move the planet labels one step down.
                    objectLabelList.Insert(firstPlanetLabelIndex, objectLocation) 'Insert label.
                End If
            Else
                objectLabelList.Add(objectLocation)  'Planet labels go at the end, after star labels.
            End If

            'Move label below.
            If objectLabelList.Count > 1 And moveLabels = False Then
                If objectLocation.Location.Y = objectLabelList(objectLabelList.Count - 2).Location.Y Then
                    objectLocation.Location = New Point(objectLocation.Location.X, objectLocation.Location.Y + labelSpace)
                End If
            End If

            'Show label.
            objectLocation.Show()

        End If
    End Sub
    Private Delegate Sub UpdateObjStatsDelegate(ByVal star As Star, ByRef index As Integer)
    Private Sub UpdateObjStats(ByVal obj As StellarObject, ByRef index As Integer)

        If objectLabelList.Count <= 0 Then
            Exit Sub
        End If

        If selectedObject IsNot Nothing Then
            UpdateTxtInfo(selectedObject) 'Update info for selected object.
        Else
            If hover Then
                UpdateTxtInfo(hoverObject)
            Else 'Show nothing.
                hoverObject = Nothing 'Empty object.
                UpdateTxtInfo(hoverObject) 'Clear fields.
            End If
        End If

        'Move all planet labels one step down if flag is raised.
        If moveLabels Then
            Dim list = objectLabelList.FindAll(Function(l) l.Text.Contains("Planet"))
            For Each item In list
                item.Top = item.Top + labelSpace
            Next
            moveLabels = False
        End If

        If Not obj.IsMerged And index < objectLabelList.Count Then

            'Get object labels.
            Dim objectLocation As Label = objectLabelList(index)

            If objectLocation.InvokeRequired Then
                Try
                    objectLocation.Invoke(New UpdateObjStatsDelegate(AddressOf UpdateObjStats), New Object() {obj, index})
                Catch ex As Exception
                    End
                End Try
            Else
                'Create some helpful strings. Round points to two (2) decimals.
                Dim objLocX As String = Math.Round(obj.CenterOfMass.X, 2).ToString
                Dim objLocY As String = Math.Round(obj.CenterOfMass.Y, 2).ToString
                Dim objType As String = ""
                If obj.isStar Then objType = "Star" Else objType = "Planet" 'Get type of object.

                'Set new colors if star label color changed.
                If objectLocation.ForeColor <> obj.Label.ForeColor Then
                    objectLocation.ForeColor = obj.Label.ForeColor
                End If

                'Update location.
                '-------------------------------------------------------------------------------------------------------------
                Dim typeIndex As Integer = index + 1
                If objType.Equals("Planet") Then 'Give different indexes for planets. Star 1 - Planet 1 / instead of / Star 1 - Planet 2
                    typeIndex -= myUniverse.Stars.Where(Function(s) s.IsMerged = False).Count
                End If
                objectLocation.Text = objType + " " + typeIndex.ToString + ":   " + objLocX.ToString + ";  " + objLocY.ToString 'X location.
                '-------------------------------------------------------------------------------------------------------------

            End If
        ElseIf Not obj.IsLabelHidden() Then

            'Move all labels below the hidden label one step up.
            For Each label In objectLabelList.GetRange(index + 1, objectLabelList.Count - index - 1)
                label.Location = New Point(label.Location.X, label.Location.Y - labelSpace)
            Next

            obj.IsLabelHidden = True 'Set hidden label flag.
            obj.Label = Nothing 'Clear label of object. With this, we now know that the object has no labels.

            'Dispose labels.
            objectLabelList(index).Parent = Nothing
            objectLabelList(index).Dispose()

            'Remove hidden label.
            objectLabelList.RemoveAt(index)

            'Adjust index.
            index = index - 1
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


            Dim points() As PointF = {mousePoint}
            myInverseUniverseMatrix.TransformPoints(points)

            mousePoint = points(0)

            'Check always with absolute point.
            'If mouse out of boundaries, do nothing.
            If absoluteMousePoint.X >= imageWidth Or absoluteMousePoint.Y >= imageHeight Or
               absoluteMousePoint.X < defaultClipOffset Or absoluteMousePoint.Y < defaultClipOffset Then

                lblCoordsMouse.Text = "Out Of Bounds"
                lblCoordsAbs.Text = "Out Of Bounds"
                Exit Sub
            Else
                'Debug.
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
        If oldOffset <> New PointF(clipOffsetX, clipOffsetY) And dragStart Then
            Exit Sub
        End If

        If creationMode.Equals("p") And Not hover Then

            Dim planetHalfSize As Double = planetSize / 2

            'If planet size exceeds boundaries, don't create anything.
            If mousePoint.X + planetHalfSize + planetBorderWidth > myUniverse.getWidth Or
                mousePoint.Y + planetHalfSize + planetBorderWidth > myUniverse.getHeight Or
                mousePoint.X - planetHalfSize - planetBorderWidth < clipOffsetX - 1 Or
                mousePoint.Y - planetHalfSize - planetBorderWidth < clipOffsetY - 1 Then 'left_top_boundary(0).Y

                Exit Sub

            End If

            threadList.Add(New Thread(AddressOf createplanet)) 'Each planet is handled by a different thread.
            threadList.Last.Start(New PointFD() {New PointFD(mousePoint.X, mousePoint.Y), New PointFD(numPlanetParamXVel.Value, numPlanetParamYVel.Value),
                                  New PointFD(absoluteMousePoint.X, absoluteMousePoint.Y)}) 'Start thread.

        ElseIf creationMode.Equals("s") And Not hover Then

            'If star size exceeds boundaries.
            If mousePoint.X + starRadius + planetBorderWidth > myUniverse.getWidth Or
                mousePoint.Y + starRadius + planetBorderWidth > myUniverse.getHeight Or
                mousePoint.X - starRadius - planetBorderWidth < clipOffsetX - 1 Or
                mousePoint.Y - starRadius - planetBorderWidth < clipOffsetY - 1 Then 'left_top_boundary(0).Y

                Exit Sub

            End If

            threadList.Add(New Thread(AddressOf createstar)) 'Each star is handled by a different thread.
            threadList.Last.Start(New PointFD() {New PointFD(mousePoint.X, mousePoint.Y), New PointFD(numStarParamXVel.Value, numStarParamYVel.Value),
                                  New PointFD(absoluteMousePoint.X, absoluteMousePoint.Y)}) 'Start thread.

        Else
            ObjectSelected() 'We selected an object.
        End If

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

            ElseIf e.Delta = -120 And zoomValue > -10 Then

                zoomValue -= zoomStep 'Decrement by one step. Zoom OUT.
                zoomValue = Math.Round(zoomValue, 5) 'Round value in case of trailing 0-9.
                zoomUniverse(0)

            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        zooming = False

    End Sub
    Private Sub blockForm_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown

        oldOffset = New PointF(clipOffsetX, clipOffsetY)
        mouseIsDown = True
        dragStart = True
        dragPoint = absoluteMousePoint 'Save point where dragging begun.

    End Sub
    Private Sub blockForm_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp

        mouseIsDown = False
        dragStart = False

        If UniversePausedForDragging = True Then
            UniversePaused = False
            UniversePausedForDragging = False
        End If

        myUniverse.SetDragStatus(dragStart)
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

        If Not ctrlKeyDown And mouseIsDown And dragStart And dragPoint <> absoluteMousePoint And counter > 5 And Not onFrame Then

            dragging = True

            If UniversePaused = False Then
                UniversePaused = True
                UniversePausedForDragging = True
            End If

            counter = 5 'The maximum speed we can draw objects. Lower means slower.
            myUniverse.SetDragStatus(dragStart)

            'Move universe.
            Try
                moveUniverse(dragPoint, absoluteMousePoint)
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

            'Update origin point.
            dragPoint = absoluteMousePoint

            dragging = False

        End If

    End Sub
    Private Sub ObjectHover()

        Dim objList As New List(Of StellarObject)
        objList.AddRange(myUniverse.Objects.FindAll(Function(o)
                                                        Return o.IsMerged = False And o.isVisible = True
                                                    End Function))
        hover = False 'Reset flag.

        For Each obj In objList

            If obj.isStar Then

                'Calculate if the point is inside the ellipse area.
                Dim ellipseEquation As Double = (mousePoint.X - obj.CenterOfMass.X) ^ 2 / (obj.Radius ^ 2) + (mousePoint.Y - obj.CenterOfMass.Y) ^ 2 / (obj.Radius ^ 2)

                If Math.Round(ellipseEquation, 2) <= 1 Then

                    hover = True 'Set flag.
                    hoverObject = obj
                    Exit For

                End If

            Else
                Dim planet As Planet = CType(obj, Planet) 'Cast object as planet.
                Dim rec As New RectangleF(New PointF(planet.GetVertices(0).X, planet.GetVertices(0).Y), New Size(planet.Size + planet.GetBorderWidth, planet.Size + planet.GetBorderWidth))

                If rec.Contains(mousePoint) Then
                    hoverObject = obj
                    hover = True 'Set flag.
                    Exit For
                End If

            End If

            'This selects merged objects.
            If obj.IsSelected Then
                selectedObject = obj
            End If

        Next

        'If we haven't selected anything.
        If selectedObject Is Nothing Then

            'Update text fields with info of hovering object.
            If hover Then
                UpdateTxtInfo(hoverObject)
            Else 'Show nothing.
                hoverObject = Nothing 'Empty object.
                UpdateTxtInfo(hoverObject) 'Clear fields.
            End If

        ElseIf selectedObject.IsMerged Then

            selectedObject = Nothing 'Empty object.
            UpdateTxtInfo(selectedObject) 'Clear fields.

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

                If Not selectedObject.Equals(hoverObject) Then 'If clicked on another object.
                    selectedObject.IsSelected = False
                Else 'If clicked on self, deselect and exit sub.

                    selectedObject.IsSelected = False
                    selectedObject = Nothing
                    Exit Sub

                End If

            End If

            selectedObject = hoverObject 'Set new selected object.
            selectedObject.IsSelected = True

        End If

    End Sub
    Private Delegate Sub UpdateTxtInfoDelegate(ByVal obj As StellarObject)
    Private Sub UpdateTxtInfo(ByVal obj As StellarObject)

        If boxObjectInfo.InvokeRequired Then
            Try
                boxObjectInfo.Invoke(New UpdateTxtInfoDelegate(AddressOf UpdateTxtInfo), New Object() {obj})
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        Else
            If obj IsNot Nothing Then

                'Round points to two (2) decimals.
                Dim objLocX As String = Math.Round(obj.CenterOfMass.X, 2).ToString
                Dim objLocY As String = Math.Round(obj.CenterOfMass.Y, 2).ToString
                Dim objAccX As String = obj.AccX.ToString
                Dim objAccY As String = obj.AccY.ToString
                Dim objVelX As String = obj.VelX.ToString
                Dim objVelY As String = obj.VelY.ToString

                txtInfoLoc.Text = objLocX + " - " + objLocY
                txtInfoMass.Text = obj.Mass.ToString

                If obj.isStar Then
                    txtInfoSize.Text = obj.Radius.ToString
                Else
                    txtInfoSize.Text = obj.Size.ToString
                End If

                txtInfoAcc.Text = objAccX + " - " + objAccY
                txtInfoVel.Text = objVelX + " - " + objVelY
            Else
                txtInfoLoc.Clear()
                txtInfoMass.Clear()
                txtInfoSize.Clear()
                txtInfoAcc.Clear()
                txtInfoVel.Clear()
            End If
        End If

    End Sub
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------
    'Options events.
    '----------------------------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------------------------

    Private Sub planetMode_CheckedChanged(sender As Object, e As EventArgs) Handles modePlanet.CheckedChanged

        If modePlanet.Checked Then
            creationMode = "p"
            boxPlanetParam.Visible = True
            boxPlanetParam.BringToFront()
        Else
            boxPlanetParam.Visible = False
        End If

    End Sub
    Private Sub starMode_CheckedChanged(sender As Object, e As EventArgs) Handles modeStar.CheckedChanged

        If modeStar.Checked Then
            creationMode = "s"
            boxStarParam.Visible = True
            boxStarParam.BringToFront()
        Else
            boxStarParam.Visible = False
        End If

    End Sub
    Private Sub modeNothing_CheckedChanged(sender As Object, e As EventArgs) Handles modeNothing.CheckedChanged

        If modeNothing.Checked Then

            If creationMode.Equals("s") Then
                boxStarParam.Enabled = False
                boxStarParam.Visible = True
            Else
                boxPlanetParam.Enabled = False
                boxPlanetParam.Visible = True
            End If
            creationMode = ""
        Else
            boxStarParam.Enabled = True
            boxPlanetParam.Enabled = True
        End If

    End Sub
    Private Sub showTraj_CheckedChanged(sender As Object, e As EventArgs) Handles generalTraj.CheckedChanged

        myUniverse.SetTrajStatus(generalTraj.Checked)

        If Not generalTraj.Checked Then
            numTraj.Visible = False
            myUniverse.Planets.ForEach(Sub(p) p.ClearTrajectory())
        Else
            numTraj.Visible = True
        End If

    End Sub
    Private Sub pointNumber_ValueChanged(sender As Object, e As EventArgs) Handles numTraj.ValueChanged

        myUniverse.SetTrajMaxPoints(numTraj.Value)

    End Sub
    Private Sub bounceMode_CheckedChanged(sender As Object, e As EventArgs) Handles collisionBounce.CheckedChanged

        myUniverse.SetBounceStatus(collisionBounce.Checked)

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
        myUniverse.Stars.Last.Init(myUniverse, defaultUniverseMatrix, pos, starRadius, starBorderWidth, myBrush.Color, starType, 0, -5)
        AddObjStatsLabel(myUniverse.Stars.Last)
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
        myUniverse.Stars.Last.Init(myUniverse, defaultUniverseMatrix, pos3, starRadius, starBorderWidth, myBrush.Color, starType, 0, 5)
        AddObjStatsLabel(myUniverse.Stars.Last)

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
            myUniverse.ResizeUniverse(universeGraphics.ClipBounds.Location, points(0), myUniverseMatrix)

            Dim s As New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) 'Init empty mouse event.
            Me.OnMouseMove(s) 'Update UI points.

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

            'Apply transformation to graphics.
            universeGraphics.Transform = newMatrix.Clone

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
            myUniverse.ResizeUniverse(New PointF(clipOffsetX, clipOffsetY), New PointF(universeWidth, universeHeight), myUniverseMatrix)

            Dim s As New MouseEventArgs(MouseButtons.None, 0, 0, 0, 0) 'Init empty mouse event.
            Me.OnMouseMove(s) 'Update UI points.

        End If

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
