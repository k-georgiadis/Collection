'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Public Class Planet
    Inherits StellarObject
    Private planetVertices As New List(Of PointF)
    Private planetUniverseMatrix As Drawing2D.Matrix

    Private planetTrajectoryPoints As New List(Of PointF)
    Private planetLastTrajectoryPointIndex As Integer
    Private planetMaxTrajectoryPoints As Integer
    Private planetTrajectoryTransformed As Boolean


    Public Property TrajectoryPoints() As List(Of PointF)
        Get
            Return planetTrajectoryPoints
        End Get
        Set(value As List(Of PointF))
            planetTrajectoryPoints = value
        End Set
    End Property
    Public Property trajectoryTransformed() As Boolean
        Get
            Return planetTrajectoryTransformed
        End Get
        Set(value As Boolean)
            planetTrajectoryTransformed = value
        End Set
    End Property

    Public ReadOnly Property GetVertices() As List(Of PointF)
        Get
            Return planetVertices
        End Get
    End Property
    Public ReadOnly Property GetBorderWidth() As Integer
        Get
            Return objectBorderWidth
        End Get
    End Property

    Public ReadOnly Property GetHalfSize() As Integer
        Get
            Return objectSize / 2
        End Get
    End Property
    Public ReadOnly Property isFullyVisibleX() As Boolean
        Get
            Return objectCenterOfMass.X >= objectUniverseOffsetX + GetHalfSize + objectBorderWidth And
                   objectCenterOfMass.X <= objectUniverse.getWidth - GetHalfSize - objectBorderWidth 'If fully inside the X visible area.
        End Get
    End Property
    Public ReadOnly Property isFullyVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY + GetHalfSize + objectBorderWidth And
                   objectCenterOfMass.Y <= objectUniverse.getHeight - GetHalfSize - objectBorderWidth 'If fully inside the Y visible area.
        End Get
    End Property
    Public ReadOnly Property isFullyVisible() As Boolean
        Get
            Return isFullyVisibleX() And isFullyVisibleY() 'If fully visible inside the X,Y visible area.
        End Get
    End Property

    ' Allow friend access to the empty constructor.
    Friend Sub New()
    End Sub
    Friend Sub Init(ByVal pUniverse As Universe, ByVal pUniverseMatrix As Drawing2D.Matrix, ByVal pLocation As PointF, ByVal pOriginLocation As PointF,
                    ByVal pSize As Integer, ByVal pBorderWidth As Integer, ByVal pColor As Color,
                    Optional ByVal pVelX As Double = 0, Optional ByVal pVelY As Double = 0
                    )

        objectUniverse = pUniverse 'Set universe the object is in.
        objectUniverseMatrix = pUniverseMatrix 'Set object's universe matrix.

        'Transform original point.
        Dim points() As PointF = {pOriginLocation}
        Dim inverseMatrix = objectUniverseMatrix.Clone
        inverseMatrix.Invert()
        inverseMatrix.TransformPoints(points)

        objectOriginPoint = points(0) 'Get absolute point for drawing the planet.

        'Set offset.
        objectUniverseOffsetX = objectUniverse.OffsetX
        objectUniverseOffsetY = objectUniverse.OffsetY

        objectCenterOfMass = New PointF(pLocation.X, pLocation.Y) 'Init center of mass.
        objectSize = pSize 'Init object size.
        objectRadius = Math.Sqrt(2 * ((objectSize / 2) ^ 2)) + objectBorderWidth 'Init object radius. Yes the planet is square.
        objectMass = 5.972 * objectSize * 10000 'Init object mass. 5,972e24 = Earth mass

        objectBorderWidth = pBorderWidth 'Init object border width.
        objectColor = pColor 'Init object color.

        'Add vertices to planet.
        planetVertices.Add(New PointF(pOriginLocation.X - objectSize / 2, pOriginLocation.Y - objectSize / 2))
        planetVertices.Add(New PointF(pOriginLocation.X + objectSize / 2, pOriginLocation.Y - objectSize / 2))
        planetVertices.Add(New PointF(pOriginLocation.X + objectSize / 2, pOriginLocation.Y + objectSize / 2))
        planetVertices.Add(New PointF(pOriginLocation.X - objectSize / 2, pOriginLocation.Y + objectSize / 2))

        objectTransitionDirection = "" 'Init transition direction. No direction.

        'Init duplicate points.
        objectDuplicatePointRight = New PointF(0, 0)
        objectDuplicatePointLeft = New PointF(0, 0)
        objectDuplicatePointTop = New PointF(0, 0)
        objectDuplicatePointBottom = New PointF(0, 0)

        objectLabel = New Label 'Init object label.
        objectLabel.ForeColor = objectColor 'Set label color.
        objectLabelHidden = False 'Init state of object label.

        'Initiliaze accelerations.
        'Here we can give extra boosts to our created objects. Don't forget to account for the distance multiplier.
        objectVelX = pVelX / objectUniverse.getDistanceMultiplier 'Init X Velocity.
        objectVelY = pVelY / objectUniverse.getDistanceMultiplier 'Init Y Velocity.
        objectAccX = 0 'Init Acceleration X.
        objectAccY = 0 'Init Acceleration Y.

        planetMaxTrajectoryPoints = objectUniverse.getMaxTrajPoints() 'Init max number of trajectory points.
        planetLastTrajectoryPointIndex = -1 'Set starting position of index.

        objectSelected = False 'Clear selection flag.

    End Sub

    Friend Sub applyAcceleration(ByVal starList As List(Of Star), ByVal gravityConstant As Double)

        Dim delta As Double = 1 / 100
        Dim distanceMultiplier = objectUniverse.getDistanceMultiplier()

        'Reset accelerations.
        objectAccX = 0
        objectAccY = 0

        'Calculate acceleration from stars.
        For Each star In starList.FindAll(Function(s) s.IsMerged = False And Not s.Equals(Me))

            'Distance vector of two bodies.
            Dim distanceVector As New PointF(objectCenterOfMass.X - star.CenterOfMass.X, objectCenterOfMass.Y - star.CenterOfMass.Y)
            Dim distanceLength As Double = Math.Sqrt(distanceVector.X ^ 2 + distanceVector.Y ^ 2)  'Get distance(magnitude).

            If distanceLength < Math.Sqrt(2 * ((objectSize / 2) ^ 2)) + star.Radius Then

                ' delta = 1 / 100

                If distanceLength < star.Radius - 3 Then

                    objectMerged = True
                    IsSelected = False 'Clear selection flag.

                    Exit Sub

                End If

            End If

            'Multiply distance to simulate "normal" distances of stellar objects in the universe.
            distanceVector.X *= distanceMultiplier
            distanceVector.Y *= distanceMultiplier

            distanceLength = Math.Sqrt(distanceVector.X ^ 2 + distanceVector.Y ^ 2)  'Get distance(magnitude).

            'Calculate gravity force. Check below for a guide to the physics.
            'http://www.cs.princeton.edu/courses/archive/fall03/cs126/assignments/nbody.html

            'force = gravityConstant * (star.GetMass * planetMass) / Math.Pow(distanceLength, 2)
            'F = G * (M * m) / d ^ 2
            'Fx = F * cos(f) = F * dx / d => .... => Ax = M * dx / d^3
            'Fy = F * sin(f) = F * dy / d => .... => Ay = M * dy / d^3

            Dim inv_d As Double = 1.0 / (distanceLength * distanceLength * distanceLength)

            'Precalculate force component (1/r^2) * direction (dx/r) = dx / r^3
            Dim dx As Double = (star.CenterOfMass.X - objectCenterOfMass.X)
            Dim dy As Double = (star.CenterOfMass.Y - objectCenterOfMass.Y)

            dx *= inv_d
            dy *= inv_d

            'Calculate accelerations for both bodies and apply them to the velocities.
            Dim currentAccX As Double = star.Mass() * dx * delta
            Dim currentAccY As Double = star.Mass() * dy * delta

            objectAccX += currentAccX
            objectAccY += currentAccY

            objectVelX += currentAccX
            objectVelY += currentAccY

            'We calculated the accelaration that is applied to this planet(Me), from the other planet(iteration).
            'So when the other planet starts calculating it's applied accelaration from this planet(Me), it must produce the same results.

            'Due to Threads not running simultaneously, the other planet will not produce the same result, because this planet(Me)
            'has already moved a little, because it calculated it's applied accelaration, FIRST.

            'So when we calculate this planet's (Me) applied accelaration, we can apply the equal but opposite(-) accelaration to the other planet.

            currentAccX = objectMass * (-dx) * delta
            currentAccY = objectMass * (-dy) * delta

            star.AccX += currentAccX
            star.AccY += currentAccY

            star.AddVelX(currentAccX)
            star.AddVelY(currentAccY)
        Next

        'Dim newpos As New PointF(planetVelX, planetVelY)
        'Move body.
        'Move(0, "", planetCenterOfMass.X + newpos.X - planetSize / 2, planetCenterOfMass.Y + newpos.Y - planetSize / 2) 'Give new coordinates of first vertex.

    End Sub

    Friend Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0)

        Dim currentVertex As New PointF

        If Direction = "" Then

            'If dX < 0 Then
            '    dX = planetUniverse.getWidth - 2 * planetUniverseOffsetX
            'End If
            'If dY < 0 Then
            '    dY = planetUniverse.getHeight - 2 * planetUniverseOffsetY
            'End If

            currentVertex.X = dX
            currentVertex.Y = dY

            planetVertices(0) = currentVertex 'Update first vertex.

            'Update the rest of the vertices.
            For i = 1 To planetVertices.Count - 1

                currentVertex = planetVertices(i)

                Select Case i
                    Case 1
                        currentVertex.X = planetVertices(0).X + objectSize
                        currentVertex.Y = planetVertices(0).Y
                    Case 2
                        currentVertex.X = planetVertices(0).X + objectSize
                        currentVertex.Y = planetVertices(0).Y + objectSize
                    Case 3
                        currentVertex.X = planetVertices(0).X
                        currentVertex.Y = planetVertices(0).Y + objectSize
                End Select

                planetVertices(i) = currentVertex 'Save new vertex.

            Next
        Else

            For i = 0 To planetVertices.Count - 1

                currentVertex = planetVertices(i) 'Get vertex.

                'Change coordinates.
                If Direction.Contains("u") Then
                    currentVertex.Y = planetVertices(i).Y - stepCount 'Move up.
                End If
                If Direction.Contains("d") Then
                    currentVertex.Y = planetVertices(i).Y + stepCount 'Move down.
                End If
                If Direction.Contains("l") Then
                    currentVertex.X = planetVertices(i).X - stepCount 'Move left.
                End If
                If Direction.Contains("r") Then
                    currentVertex.X = planetVertices(i).X + stepCount 'Move right.
                End If

                planetVertices(i) = currentVertex 'Save new vertex.
            Next

        End If

        objectCenterOfMass = New PointF(planetVertices(0).X + objectSize / 2, planetVertices(0).Y + objectSize / 2) 'New center of mass.

    End Sub
    Friend Sub CheckForBounce()

        'Bouncing (Left, Right & Top, Bottom).
        If objectCenterOfMass.X <= objectUniverseOffsetX + GetHalfSize + objectBorderWidth - 1 Or
           objectCenterOfMass.X >= objectUniverse.getWidth - GetHalfSize - objectBorderWidth + 1 Then
            'Change X direction.
            objectVelX = -objectVelX 'Opposite direction.

            If objectCenterOfMass.X <= objectUniverseOffsetX + GetHalfSize + objectBorderWidth - 1 Then
                Move(0, "", objectUniverseOffsetX + objectBorderWidth - 1, planetVertices(0).Y)
            Else
                Move(0, "", objectUniverse.getWidth - objectSize - objectBorderWidth + 1, planetVertices(0).Y)
            End If

        ElseIf objectCenterOfMass.Y <= objectUniverseOffsetY + GetHalfSize + objectBorderWidth - 1 Or
               objectCenterOfMass.Y >= objectUniverse.getHeight - GetHalfSize - objectBorderWidth + 1 Then
            'Change Y direction.
            objectVelY = -objectVelY 'Opposite direction.

            If objectCenterOfMass.Y <= objectUniverseOffsetY + GetHalfSize + objectBorderWidth - 1 Then
                Move(0, "", objectCenterOfMass.X, objectUniverseOffsetY + objectBorderWidth - 1)
            Else
                Move(0, "", objectCenterOfMass.X, objectUniverse.getHeight - objectSize - objectBorderWidth + 1)
            End If

            'Console.Write("im going places" + Math.Sign(planetVelY).ToString + planetCenterOfMass.ToString + vbNewLine)
        End If

    End Sub

    Friend Sub AddTrajectoryPoint(ByVal point As PointF)

        planetTrajectoryPoints.Add(point)

    End Sub
    Friend Sub ClearTrajectory()

        planetTrajectoryPoints.RemoveRange(0, planetTrajectoryPoints.Count)

    End Sub

    Friend Overrides Sub Paint(ByVal universeGraphics As Graphics)

        Dim planetPen As New Pen(objectUniverse.getPen.Brush, objectUniverse.getPen.Width)

        'Paint lines(sides) between each pair of vertices.
        For i = 0 To planetVertices.Count - 1

            If i = planetVertices.Count - 1 Then
                universeGraphics.DrawLine(planetPen, planetVertices.Item(i), planetVertices.Item(0)) 'Draw last line.
            Else
                universeGraphics.DrawLine(planetPen, planetVertices.Item(i), planetVertices.Item(i + 1)) 'Draw line.
            End If

        Next

        Dim tempWidth As Integer = planetPen.Width
        planetPen.Width = 1

        'Paint selection rectangle, if selected.
        If IsSelected Then

            Dim tempColor As Color = planetPen.Color 'Save color.

            planetPen.Color = Color.White 'Set to white for maximum contrast.
            universeGraphics.DrawRectangle(planetPen, planetVertices.Item(0).X - 2, planetVertices.Item(0).Y - 2, Size + 4, Size + 4)
            planetPen.Color = tempColor 'Restore original color.

        End If

        Dim maxPoints As Integer = objectUniverse.getMaxTrajPoints() 'Get maximum number of points to use for the trajectory.

        'If new value is given, reset list.
        If maxPoints <> planetMaxTrajectoryPoints And planetTrajectoryPoints.Count > 0 Then
            ClearTrajectory() 'Remove all.
            planetMaxTrajectoryPoints = maxPoints 'Set new value.
        End If

        'Draw trajectory if said so.
        If objectUniverse.drawTraj Then

            'Transform the trajectory point.
            Dim transformedTrajectoryPoint() As PointF = {objectOriginPoint}
            UniverseMatrix.TransformPoints(transformedTrajectoryPoint)

            'If there are any trajectory points.
            If planetTrajectoryPoints.Count > 0 Then
                'If center of mass is not the same, create new trajectory point.
                'If Not transformedTrajectoryPoint(0).Equals(planetTrajectoryPoints.Last) Then
                '    'If we reached the maximum amount of points we can use, replace points from the start.
                '    If planetTrajectoryPoints.Count = maxPoints Then

                '        'If index is at the end, start from the head again.
                '        If planetLastTrajectoryPointIndex = maxPoints - 1 Then
                '            planetLastTrajectoryPointIndex = 0 'Set index to the head of the list.
                '        Else
                '            planetLastTrajectoryPointIndex += 1 'Increment index.
                '        End If
                '        'planetTrajectoryPoints.Item(planetLastTrajectoryPointIndex) = transformedTrajectoryPoint(0) 'Replace that point with the new one.
                '    Else
                '        planetTrajectoryPoints.Add(transformedTrajectoryPoint(0))
                '        planetLastTrajectoryPointIndex += 1 'Increment index.
                '    End If
                'End If

                Dim previousPoint As PointF = planetTrajectoryPoints.First 'Init with first point.

                'But if we 're full and we are recycling the points, init with the last.
                If planetTrajectoryPoints.Count = maxPoints Then
                    previousPoint = planetTrajectoryPoints.Last
                End If

                Dim beginTailing As Boolean = False 'Have we finished drawing the recycled points? Time to start drawing the tail.

                For Each point In planetTrajectoryPoints

                    'If we reached the old points (tail), stop and start creating the tail.
                    'If Not beginTailing And planetTrajectoryPoints.IndexOf(point) > planetLastTrajectoryPointIndex Then
                    '    previousPoint = point 'Reset previous point with current one.
                    '    beginTailing = True 'Raise flag as to not enter again.
                    'End If

                    'Check for violent changes in trajectory, usually because of tunneling.
                    'The value is random, it could be 50, 100 etc. as long it's smaller than the universe width/height so we can detect tunneling.
                    If Math.Abs(point.X - previousPoint.X) < 1120 And Math.Abs(point.Y - previousPoint.Y) < 1120 Then
                        'planetGraphics.DrawLine(planetPen, point, previousPoint)
                        objectUniverse.getImage.SetPixel(point.X, point.Y, Me.Color)
                    End If
                    previousPoint = point
                Next
            Else
                planetTrajectoryPoints.Add(transformedTrajectoryPoint(0)) 'Add first point.
                planetLastTrajectoryPointIndex = 0 'Reset index.
            End If

        End If

        planetPen.Width = tempWidth 'Restore original width.

    End Sub


End Class