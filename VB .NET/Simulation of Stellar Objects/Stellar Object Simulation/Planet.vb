'MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Imports System.ComponentModel

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
    Public ReadOnly Property GetHalfSize() As Integer
        Get
            Return Size / 2
        End Get
    End Property
    Public ReadOnly Property GetHalfVisualSize() As Integer
        Get
            Return VisualSize / 2
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

        Universe = pUniverse 'Set universe the object is in.
        UniverseMatrix = pUniverseMatrix 'Set object's universe matrix.

        InverseUniverseMatrix = UniverseMatrix.Clone 'Save inverted matrix.
        InverseUniverseMatrix.Invert()

        'Set offset.
        UniverseOffsetX = Universe.OffsetX
        UniverseOffsetY = Universe.OffsetY

        CenterOfMass = pLocation 'Init center of mass.
        Size = pSize 'Init object actual size.
        VisualSize = pSize + CenterOfMass.Z / pSize 'Init object visual size.

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
        Color = pColor 'Init object color.

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

        ListItem.ForeColor = Color 'Set label color.

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

            currentVertex.X = dX - GetHalfVisualSize
            currentVertex.Y = dY - GetHalfVisualSize
            currentVertex.Z = dZ - GetHalfVisualSize

            Vertices(0) = currentVertex 'Update vertex.

        Else
            currentVertex = Vertices(0) 'Get vertex.

            'Change coordinates.
            If Direction.Contains("u") Then
                currentVertex.Y = Vertices(0).Y - stepCount 'Move up.
            End If
            If Direction.Contains("d") Then
                currentVertex.Y = Vertices(0).Y + stepCount 'Move down.
            End If
            If Direction.Contains("l") Then
                currentVertex.X = Vertices(0).X - stepCount 'Move left.
            End If
            If Direction.Contains("r") Then
                currentVertex.X = Vertices(0).X + stepCount 'Move right.
            End If

            Vertices(0) = currentVertex 'Save new vertex.

        End If

        'Update visual size.
        VisualSize = Size + CenterOfMass.Z / Size

        'Set minimum size as to not disappear entirely.
        If VisualSize < 4 Then
            VisualSize = 4
        End If

        'The center must be updated after the visual size, otherwise the object will start glitching when getting bigger/smaller.
        CenterOfMass = New PointFD(dX, dY, dZ) 'New center of mass.

    End Sub
    Friend Overrides Sub CheckForBounce()

        'Bouncing (Left, Right & Top, Bottom).
        If CenterOfMass.X <= UniverseOffsetX + GetHalfSize + BorderWidth - 1 Or
           CenterOfMass.X >= Universe.getWidth - GetHalfSize - BorderWidth + 1 Then

            'Change X direction.
            VelX = -VelX 'Opposite direction.

            If CenterOfMass.X <= UniverseOffsetX + GetHalfSize + BorderWidth - 1 Then
                Move(0, "", UniverseOffsetX + GetHalfSize + BorderWidth - 1, CenterOfMass.Y)
            Else
                Move(0, "", Universe.getWidth - GetHalfSize - BorderWidth + 1, CenterOfMass.Y)
            End If

        ElseIf CenterOfMass.Y <= UniverseOffsetY + GetHalfSize + BorderWidth - 1 Or
               CenterOfMass.Y >= Universe.getHeight - GetHalfSize - BorderWidth + 1 Then

            'Change Y direction.
            VelY = -VelY 'Opposite direction.

            If CenterOfMass.Y <= UniverseOffsetY + GetHalfSize + BorderWidth - 1 Then
                Move(0, "", CenterOfMass.X, UniverseOffsetY + GetHalfSize + BorderWidth - 1)
            Else
                Move(0, "", CenterOfMass.X, Universe.getHeight - GetHalfSize - BorderWidth + 1)
            End If

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
                                       CType(VisualSize, Single), CType(VisualSize, Single))

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