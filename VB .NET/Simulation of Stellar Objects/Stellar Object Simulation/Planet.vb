'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Public Class Planet
    Inherits StellarObject
    Private planetVertices As New List(Of PointFD)
    Private planetUniverseMatrix As Drawing2D.Matrix

    Public ReadOnly Property GetVertices() As List(Of PointFD)
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
    Public Overrides ReadOnly Property isVisibleX() As Boolean
        Get
            Return objectCenterOfMass.X >= objectUniverseOffsetX - GetHalfSize - objectBorderWidth And
                   objectCenterOfMass.X <= objectUniverse.getWidth + GetHalfSize + objectBorderWidth 'If fully inside the X visible area.
        End Get
    End Property
    Public Overrides ReadOnly Property isVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY - GetHalfSize - objectBorderWidth And
                   objectCenterOfMass.Y <= objectUniverse.getHeight + GetHalfSize + objectBorderWidth 'If fully inside the Y visible area.
        End Get
    End Property

    ' Allow friend access to the empty constructor.
    Friend Sub New()
    End Sub
    Friend Sub Init(ByVal pUniverse As Universe, ByVal pUniverseMatrix As Drawing2D.Matrix, ByVal pLocation As PointFD,
                    ByVal pSize As Integer, ByVal pBorderWidth As Integer, ByVal pColor As Color,
                    Optional ByVal pVelX As Double = 0, Optional ByVal pVelY As Double = 0
                    )

        objectUniverse = pUniverse 'Set universe the object is in.
        objectUniverseMatrix = pUniverseMatrix 'Set object's universe matrix.

        objectInverseUniverseMatrix = objectUniverseMatrix.Clone 'Save inverted matrix.
        objectInverseUniverseMatrix.Invert()

        'Set offset.
        objectUniverseOffsetX = objectUniverse.OffsetX
        objectUniverseOffsetY = objectUniverse.OffsetY

        objectCenterOfMass = New PointFD(pLocation.X, pLocation.Y) 'Init center of mass.
        objectSize = pSize 'Init object size.
        objectRadius = Math.Sqrt(2 * ((objectSize / 2) ^ 2)) + objectBorderWidth 'Init object radius. Yes the planet is square.
        objectMass = 5.972 * objectSize * 10000 'Init object mass. 5,972e24 = Earth mass

        objectBorderWidth = pBorderWidth 'Init object border width.
        objectColor = pColor 'Init object color.

        'Add vertices to planet.
        planetVertices.Add(New PointFD(pLocation.X - GetHalfSize, pLocation.Y - GetHalfSize))
        'planetVertices.Add(New PointFD(pLocation.X + objectSize / 2, pLocation.Y - objectSize / 2))
        'planetVertices.Add(New PointFD(pLocation.X + objectSize / 2, pLocation.Y + objectSize / 2))
        'planetVertices.Add(New PointFD(pLocation.X - objectSize / 2, pLocation.Y + objectSize / 2))

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

        objectMaxTrajectoryPoints = objectUniverse.getMaxTrajPoints() 'Init max number of trajectory points.
        objectLastTrajectoryPointIndex = -1 'Set starting position of index.

        objectSelected = False 'Clear selection flag.

    End Sub

    'Friend Sub applyAcceleration(ByVal starList As List(Of Star), ByVal gravityConstant As Double)

    'End Sub

    Friend Overrides Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0)

        Dim currentVertex As New PointFD

        If Direction = "" Then

            currentVertex.X = dX - GetHalfSize
            currentVertex.Y = dY - GetHalfSize

            planetVertices(0) = currentVertex 'Update vertex.

        Else
            currentVertex = planetVertices(0) 'Get vertex.

            'Change coordinates.
            If Direction.Contains("u") Then
                currentVertex.Y = planetVertices(0).Y - stepCount 'Move up.
            End If
            If Direction.Contains("d") Then
                currentVertex.Y = planetVertices(0).Y + stepCount 'Move down.
            End If
            If Direction.Contains("l") Then
                currentVertex.X = planetVertices(0).X - stepCount 'Move left.
            End If
            If Direction.Contains("r") Then
                currentVertex.X = planetVertices(0).X + stepCount 'Move right.
            End If

            planetVertices(0) = currentVertex 'Save new vertex.

        End If

        objectCenterOfMass = New PointFD(planetVertices(0).X + GetHalfSize, planetVertices(0).Y + GetHalfSize) 'New center of mass.

    End Sub
    Friend Overrides Sub CheckForBounce()

        'Bouncing (Left, Right & Top, Bottom).
        If objectCenterOfMass.X <= objectUniverseOffsetX + GetHalfSize + objectBorderWidth - 1 Or
           objectCenterOfMass.X >= objectUniverse.getWidth - GetHalfSize - objectBorderWidth + 1 Then
            'Change X direction.
            objectVelX = -objectVelX 'Opposite direction.

            If objectCenterOfMass.X <= objectUniverseOffsetX + GetHalfSize + objectBorderWidth - 1 Then
                Move(0, "", objectUniverseOffsetX + GetHalfSize + objectBorderWidth - 1, objectCenterOfMass.Y)
            Else
                Move(0, "", objectUniverse.getWidth - GetHalfSize - objectBorderWidth + 1, objectCenterOfMass.Y)
            End If

        ElseIf objectCenterOfMass.Y <= objectUniverseOffsetY + GetHalfSize + objectBorderWidth - 1 Or
               objectCenterOfMass.Y >= objectUniverse.getHeight - GetHalfSize - objectBorderWidth + 1 Then
            'Change Y direction.
            objectVelY = -objectVelY 'Opposite direction.

            If objectCenterOfMass.Y <= objectUniverseOffsetY + GetHalfSize + objectBorderWidth - 1 Then
                Move(0, "", objectCenterOfMass.X, objectUniverseOffsetY + GetHalfSize + objectBorderWidth - 1)
            Else
                Move(0, "", objectCenterOfMass.X, objectUniverse.getHeight - GetHalfSize - objectBorderWidth + 1)
            End If

            'Console.Write("im going places" + Math.Sign(planetVelY).ToString + planetCenterOfMass.ToString + vbNewLine)
        End If

    End Sub

    Friend Overrides Sub Paint(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim planetPen As New Pen(objectUniverse.getPen.Brush, objectUniverse.getPen.Width)
        Dim pointSingle As New PointF(planetVertices.Item(0).X, planetVertices.Item(0).Y)

        'Paint lines(sides) between each pair of vertices.
        'For i = 0 To planetVertices.Count - 1

        '    If i = planetVertices.Count - 1 Then
        '        universeGraphics.DrawLine(planetPen, New Point(planetVertices.Item(i).X, planetVertices.Item(i).Y), New Point(planetVertices.Item(0).X, planetVertices.Item(0).Y)) 'Draw last line.
        '    Else
        '        universeGraphics.DrawLine(planetPen, New Point(planetVertices.Item(i).X, planetVertices.Item(i).Y), New Point(planetVertices.Item(i + 1).X, planetVertices.Item(i + 1).Y)) 'Draw line.
        '    End If

        'Next

        'Draw rectangle instead of lines.
        universeGraphics.DrawRectangle(planetPen, pointSingle.X, pointSingle.Y, Size, Size)

        Dim tempWidth As Integer = planetPen.Width
        planetPen.Width = 1

        'Paint selection rectangle, if selected.
        If IsSelected Then

            Dim tempColor As Color = planetPen.Color 'Save color.

            If zoomValue >= 1 Then
                zoomValue = 1
            Else
                zoomValue = (2 - zoomValue) ^ 3
            End If

            Dim zoomSingle As Single = CType(zoomValue, Single)

            planetPen.Color = Color.White 'Set to white for maximum contrast.
            universeGraphics.DrawRectangle(planetPen, pointSingle.X - 2 * zoomSingle, pointSingle.Y - 2 * zoomSingle, Size + 4 * zoomSingle, Size + 4 * zoomSingle)
            planetPen.Color = tempColor 'Restore original color.

        End If

        Dim maxPoints As Integer = objectUniverse.getMaxTrajPoints() 'Get maximum number of points to use for the trajectory.

        'If new value is given, reset list.
        If maxPoints <> objectMaxTrajectoryPoints And objectTrajectoryPoints.Count > 0 Then
            ClearTrajectory() 'Remove all.
            objectMaxTrajectoryPoints = maxPoints 'Set new value.
        End If

        'Draw trajectory if said so.
        If objectUniverse.drawTraj Then

            'Transform the trajectory point.
            Dim transformedTrajectoryPoint() As PointF = {New Point(objectCenterOfMass.X, objectCenterOfMass.Y)}
            UniverseMatrix.TransformPoints(transformedTrajectoryPoint)

            'If there are any trajectory points.
            If objectTrajectoryPoints.Count > 0 Then
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

                Dim previousPoint As PointF = objectTrajectoryPoints.First 'Init with first point.

                'But if we 're full and we are recycling the points, init with the last.
                If objectTrajectoryPoints.Count = maxPoints Then
                    previousPoint = objectTrajectoryPoints.Last
                End If

                Dim beginTailing As Boolean = False 'Have we finished drawing the recycled points? Time to start drawing the tail.

                For Each point In objectTrajectoryPoints

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
                objectTrajectoryPoints.Add(transformedTrajectoryPoint(0)) 'Add first point.
                objectLastTrajectoryPointIndex = 0 'Reset index.
            End If

        End If

        planetPen.Width = tempWidth 'Restore original width.

    End Sub


End Class