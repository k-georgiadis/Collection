'MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Public Class Planet
    Inherits StellarObject

    Private planetVertices As New List(Of PointFD)

    Public Property Vertices As List(Of PointFD)
        Get
            Return planetVertices
        End Get
        Set(value As List(Of PointFD))
            planetVertices = value
        End Set
    End Property

    Public ReadOnly Property GetBorderWidth() As Integer
        Get
            Return BorderWidth
        End Get
    End Property
    Public ReadOnly Property GetHalfSize() As Double
        Get
            Return Size / 2
        End Get
    End Property
    Public ReadOnly Property GetHalfVisualSize() As Double
        Get
            Return VisualSize / 2
        End Get
    End Property

    Public ReadOnly Property isFullyVisibleX() As Boolean
        Get
            Return CenterOfMass.X >= UniverseOffsetX + GetHalfVisualSize + BorderWidth / 2 And
                   CenterOfMass.X <= Universe.getWidth - GetHalfVisualSize - BorderWidth / 2 'If fully inside the X visible area.
        End Get
    End Property
    Public ReadOnly Property isFullyVisibleY() As Boolean
        Get
            Return CenterOfMass.Y >= UniverseOffsetY + GetHalfVisualSize + BorderWidth / 2 And
                   CenterOfMass.Y <= Universe.getHeight - GetHalfVisualSize - BorderWidth / 2 'If fully inside the Y visible area.
        End Get
    End Property
    Public ReadOnly Property isFullyVisible() As Boolean
        Get
            Return isFullyVisibleX() And isFullyVisibleY() 'If fully visible inside the X,Y visible area.
        End Get
    End Property
    Public Overrides ReadOnly Property isVisibleX() As Boolean
        Get
            Return CenterOfMass.X >= UniverseOffsetX - GetHalfVisualSize - BorderWidth / 2 And
                   CenterOfMass.X <= Universe.getWidth + GetHalfVisualSize + BorderWidth / 2 'If inside the X visible area at all.
        End Get
    End Property
    Public Overrides ReadOnly Property isVisibleY() As Boolean
        Get
            Return CenterOfMass.Y >= UniverseOffsetY - GetHalfVisualSize - BorderWidth / 2 And
                   CenterOfMass.Y <= Universe.getHeight + GetHalfVisualSize + BorderWidth / 2 'If inside the Y visible area at all.
        End Get
    End Property

    ' Allow friend access to the empty constructor.
    Friend Sub New()
    End Sub
    Friend Sub Init(ByVal pUniverse As Universe, ByVal pUniverseMatrix As Drawing2D.Matrix, ByVal pLocation As PointFD,
                    ByVal pSize As Double, ByVal pEarthMass As Double, pBorderWidth As Integer, ByVal pColor As Color,
                     ByVal pVelInit As PointFD, Optional ByVal pVel As PointFD = Nothing)

        Universe = pUniverse
        UniverseMatrix = pUniverseMatrix

        InverseUniverseMatrix = UniverseMatrix.Clone
        InverseUniverseMatrix.Invert()

        'Set offset.
        UniverseOffsetX = Universe.OffsetX
        UniverseOffsetY = Universe.OffsetY

        Type = StellarObjectType.Planet
        CenterOfMass = pLocation
        Size = pSize
        VisualSize = pSize + CenterOfMass.Z / pSize

        'Set minimum size as to not disappear entirely.
        If VisualSize < 0 Then
            VisualSize = 4
        End If

        Radius = Math.Sqrt(2 * ((pSize / 2) ^ 2)) + BorderWidth 'Init object radius. Yes, the planet is square.

        'Init object mass. 5,972E+24 = Earth mass
        Mass = 5.972E+24 * pEarthMass
        'If Universe.isRealistic Then
        '    Mass = 5.972E+24 * pEarthMass
        'Else
        '    Mass = 5972000.0 * pEarthMass
        'End If

        BorderWidth = pBorderWidth 'Init object border width.
        PaintColor = pColor 'Init object color.

        'Add vertices to planet.
        planetVertices.Add(New PointFD(pLocation.X - GetHalfVisualSize, pLocation.Y - GetHalfVisualSize,
                                       pLocation.Z - GetHalfVisualSize))
        'PlanetVertices.Add(New PointFD(pLocation.X + objectSize / 2, pLocation.Y - objectSize / 2))
        'PlanetVertices.Add(New PointFD(pLocation.X + objectSize / 2, pLocation.Y + objectSize / 2))
        'PlanetVertices.Add(New PointFD(pLocation.X - objectSize / 2, pLocation.Y + objectSize / 2))

        TransitionDirection = "" 'Init transition direction. No direction.

        'Init duplicate points.
        DuplicatePointRight = New PointF(0, 0)
        DuplicatePointLeft = New PointF(0, 0)
        DuplicatePointTop = New PointF(0, 0)
        DuplicatePointBottom = New PointF(0, 0)

        If pVelInit.Equals(CType(Nothing, PointFD)) And pVel.Equals(CType(Nothing, PointFD)) Then
            pVel = New PointFD(0, 0, 0)
        End If

        'Init object label only when not merging.
        If Not IsMerging Then

            ListItem = New ListViewItem

            'Here we can give extra boosts to our created objects.
            If Not pVelInit.Equals(CType(Nothing, PointFD)) Then

                If Universe.isRealistic Then
                    pVelInit.X *= 1000 'kpf = kilometers per frame.
                    pVelInit.Y *= 1000
                    pVelInit.Z *= 1000
                Else
                    pVelInit.X *= Universe.DistanceMultiplier  'ppf = pixels per frame.
                    pVelInit.Y *= Universe.DistanceMultiplier
                    pVelInit.Z *= Universe.DistanceMultiplier
                End If

                'Set new velocity.
                pVel = pVelInit

            End If
        End If

        ListItem.BackColor = PaintColor 'Set label color.

        AccX = 0
        AccY = 0
        AccZ = 0

        'Set velocity.
        VelX = pVel.X
        VelY = pVel.Y
        VelZ = pVel.Z

        IsSelected = False 'Init selection flag.
        IsMerged = False 'Init state of object.
        IsMerging = False 'Init merging status flag.

        MaxTrajectoryPoints = Universe.MaxTrajectoryPoints 'Init max number of trajectory points.

    End Sub

    'Friend Sub applyAcceleration(ByVal starList As List(Of Star), ByVal gravityConstant As Double)

    'End Sub

    Friend Overrides Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0, Optional ByVal dZ As Double = 0)

        Dim currentVertex As New PointFD

        If Direction = "" Then

            'Calculate new visual size.
            Dim newVisualSize = Size + CenterOfMass.Z / Size

            'Update visual size and starting vertex.
            If Not VisualSize.Equals(newVisualSize) Then

                'Set minimum visual size as to not disappear completely.
                If newVisualSize < 4 Then
                    newVisualSize = 4
                    VisualSize = 4
                End If

                'Grow rectangle uniformly.
                Dim offset As Double = (newVisualSize - VisualSize) / 2

                currentVertex.X = dX - GetHalfVisualSize - offset
                currentVertex.Y = dY - GetHalfVisualSize - offset
                currentVertex.Z = dZ - GetHalfVisualSize - offset

                'Update visual size.
                VisualSize = newVisualSize

            Else
                currentVertex.X = dX - GetHalfVisualSize
                currentVertex.Y = dY - GetHalfVisualSize
                currentVertex.Z = dZ - GetHalfVisualSize
            End If

        Else
            currentVertex = Vertices(0) 'Get starting vertex.

            'Change coordinates.
            Select Case Direction
                Case "u"
                    currentVertex.Y = Vertices(0).Y - stepCount 'Move up.
                Case "d"
                    currentVertex.Y = Vertices(0).Y + stepCount 'Move down.
                Case "l"
                    currentVertex.X = Vertices(0).X - stepCount 'Move left.
                Case "r"
                    currentVertex.X = Vertices(0).X + stepCount  'Move right.
            End Select


        End If

        Vertices(0) = currentVertex 'Update starting vertex.
        CenterOfMass = New PointFD(dX, dY, dZ) 'New center of mass.

    End Sub
    Friend Overrides Sub CheckForBounce()

        'Dim delta As Integer = 0

        'Bouncing (Left, Right & Top, Bottom).
        If Not isFullyVisibleX() Then

            'Change X direction.
            VelX = -VelX 'Opposite direction.

            'Bounce back when velocity is not zero.
            'If VelX <> 0 Then
            '    delta = 1
            'End If

            'If CenterOfMass.X < UniverseOffsetX + GetHalfVisualSize + BorderWidth / 2 Then
            '    Move(0, "", UniverseOffsetX + GetHalfVisualSize + BorderWidth / 2 + delta, CenterOfMass.Y, CenterOfMass.Z)
            'Else
            '    Move(0, "", Universe.getWidth - GetHalfVisualSize - BorderWidth / 2 - delta, CenterOfMass.Y, CenterOfMass.Z)
            'End If

        ElseIf Not isFullyVisibleY() Then

            'Change Y direction.
            VelY = -VelY 'Opposite direction.

            'Bounce back when velocity is not zero.
            'If VelY <> 0 Then
            '    delta = 1
            'End If

            'If CenterOfMass.Y < UniverseOffsetY + GetHalfVisualSize + BorderWidth / 2 Then
            '    Move(0, "", CenterOfMass.X, UniverseOffsetY + GetHalfVisualSize + BorderWidth / 2 + delta, CenterOfMass.Z)
            'Else
            '    Move(0, "", CenterOfMass.X, Universe.getHeight - GetHalfVisualSize - BorderWidth / 2 - delta, CenterOfMass.Z)
            'End If

            'Console.Write("im going places" + Math.Sign(planetVelY).ToString + planetCenterOfMass.ToString + vbNewLine)
        End If

    End Sub

    Friend Overrides Sub Paint(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim planetPen As New Pen(Universe.getPen.Brush, Universe.getPen.Width)
        Dim pointSingle As New PointF(Vertices.Item(0).X, Vertices.Item(0).Y)

        'Paint lines(sides) between each pair of vertices.
        'For i = 0 To planetVertices.Count - 1

        '    If i = planetVertices.Count - 1 Then
        '        universeGraphics.DrawLine(planetPen, New Point(planetVertices.Item(i).X, planetVertices.Item(i).Y), New Point(planetVertices.Item(0).X, planetVertices.Item(0).Y)) 'Draw last line.
        '    Else
        '        universeGraphics.DrawLine(planetPen, New Point(planetVertices.Item(i).X, planetVertices.Item(i).Y), New Point(planetVertices.Item(i + 1).X, planetVertices.Item(i + 1).Y)) 'Draw line.
        '    End If

        'Next

        'Draw rectangle instead of lines.
        universeGraphics.DrawRectangle(planetPen, pointSingle.X, pointSingle.Y,
                                       Convert.ToSingle(VisualSize), Convert.ToSingle(VisualSize))

        'Draw trajectory path.
        DrawTrajectory(universeGraphics, planetPen)

    End Sub
    Friend Overrides Sub PaintSelectionShape(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim planetPen As New Pen(Universe.getPen.Brush, Universe.getPen.Width)
        Dim pointSingle As New PointF(Vertices.Item(0).X, Vertices.Item(0).Y)

        Dim tempColor As Color = planetPen.Color
        Dim tempWidth As Integer = planetPen.Width
        planetPen.Width = 1

        If zoomValue >= 1 Then
            zoomValue = 1
        Else
            zoomValue = (2 - zoomValue) ^ 3
        End If

        Dim zoomSingle As Single = Convert.ToSingle(zoomValue)

        planetPen.Color = Color.White 'Set to white for maximum contrast.
        universeGraphics.DrawRectangle(planetPen,
                                            pointSingle.X - Convert.ToSingle(BorderWidth / 2) - 2 * zoomSingle,
                                            pointSingle.Y - Convert.ToSingle(BorderWidth / 2) - 2 * zoomSingle,
                                            Convert.ToSingle(VisualSize) + BorderWidth + 4 * zoomSingle,
                                            Convert.ToSingle(VisualSize) + BorderWidth + 4 * zoomSingle)
        planetPen.Color = tempColor
        planetPen.Width = tempWidth

    End Sub
End Class