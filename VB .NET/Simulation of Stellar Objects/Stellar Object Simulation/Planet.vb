'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Public Class Planet
    Inherits StellarObject
    Private planetVertices As New List(Of PointFD)

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
                     Optional ByVal pVel As PointFD = Nothing)

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

        objectListItem = New ListViewItem 'Init object label.
        objectListItem.ForeColor = objectColor 'Set label color.

        'Initiliaze accelerations.
        'Here we can give extra boosts to our created objects. Don't forget to account for the distance multiplier.
        objectVelX = pVel.X / objectUniverse.getDistanceMultiplier 'Init X Velocity.
        objectVelY = pVel.Y / objectUniverse.getDistanceMultiplier 'Init Y Velocity.
        objectAccX = 0 'Init Acceleration X.
        objectAccY = 0 'Init Acceleration Y.

        objectMaxTrajectoryPoints = objectUniverse.MaxTrajectoryPoints 'Init max number of trajectory points.

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

        DrawTrajectory(universeGraphics, planetPen)
        planetPen.Width = tempWidth 'Restore original width.

    End Sub


End Class