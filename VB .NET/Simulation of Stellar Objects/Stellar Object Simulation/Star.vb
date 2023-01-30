''MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.ComponentModel
Imports System.Runtime.Remoting

Public Class Star
    Inherits StellarObject


    Public ReadOnly Property isFullyVisibleX() As Boolean
        Get
            Return CenterOfMass.X >= UniverseOffsetX + VisualSize + BorderWidth / 2 And
                   CenterOfMass.X <= Universe.getWidth - VisualSize - BorderWidth / 2 'If fully inside the X visible area.
        End Get
    End Property
    Public ReadOnly Property isFullyVisibleY() As Boolean
        Get
            Return CenterOfMass.Y >= UniverseOffsetY + VisualSize + BorderWidth / 2 And
                   CenterOfMass.Y <= Universe.getHeight - VisualSize - BorderWidth / 2 'If fully inside the Y visible area.
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
    Friend Sub Init(ByVal sUniverse As Universe, ByVal pUniverseMatrix As Drawing2D.Matrix,
                    ByVal sLocation As PointFD, ByVal sRadius As Double, ByVal starMass As Double,
                    ByVal sBorderWidth As Integer, ByVal sColor As Color,
                    ByVal sVelInit As PointFD, Optional ByVal sVel As PointFD = Nothing)

        Universe = sUniverse 'Set universe the object is in.
        UniverseMatrix = pUniverseMatrix 'Set object's universe matrix.

        InverseUniverseMatrix = UniverseMatrix.Clone 'Save inverted matrix.
        InverseUniverseMatrix.Invert()

        'Set offset.
        UniverseOffsetX = Universe.OffsetX
        UniverseOffsetY = Universe.OffsetY

        CenterOfMass = sLocation 'Init center of mass.
        Radius = sRadius 'Init object radius.
        VisualSize = sRadius + CenterOfMass.Z / sRadius 'Init object visual size.

        'Set minimum size as to not disappear entirely.
        If VisualSize < 4 Then
            VisualSize = 4
        End If

        BorderWidth = sBorderWidth 'Init object pen border width.
        Color = sColor 'Init object color.

        'Init object mass.
        Mass = Universe.SolarMass * starMass '0.075 * M '* sType
        'If Universe.isRealistic Then
        '    Mass = M * sSolarMass '0.075 * M '* sType
        'Else
        '    Mass = 0.075 * 19890000000.0 * sSolarMass
        'End If

        TransitionDirection = "" 'Init transition direction. No direction.

        'Init duplicate points.
        DuplicatePointRight = New PointF(0, 0)
        DuplicatePointLeft = New PointF(0, 0)
        DuplicatePointTop = New PointF(0, 0)
        DuplicatePointBottom = New PointF(0, 0)

        If sVelInit.Equals(CType(Nothing, PointFD)) And sVel.Equals(CType(Nothing, PointFD)) Then
            sVel = New PointFD(0, 0, 0)
        End If

        'Init object label only when not merging.
        If Not IsMerging Then

            ListItem = New ListViewItem

            'Here we can give extra boosts to our created objects.
            If Not sVelInit.Equals(CType(Nothing, PointFD)) Then

                If Universe.isRealistic Then
                    sVelInit.X *= 1000  'kpf = kilometers per frame.
                    sVelInit.Y *= 1000
                    sVelInit.Z *= 1000
                Else
                    sVelInit.X *= Universe.DistanceMultiplier  'ppf = pixels per frame.
                    sVelInit.Y *= Universe.DistanceMultiplier
                    sVelInit.Z *= Universe.DistanceMultiplier
                End If

                'Set new velocity.
                sVel = sVelInit

            End If
        End If

        ListItem.ForeColor = Color 'Set label color.

        AccX = 0
        AccY = 0
        AccZ = 0

        'Init velocity. Here we can give extra boosts to our created objects.
        VelX = sVel.X
        VelY = sVel.Y
        VelZ = sVel.Z

        IsSelected = False 'Init selection flag.
        IsMerged = False 'Init state of object.
        IsMerging = False 'Init merging status flag.

    End Sub

    'Friend Sub applyAcceleration(ByVal objectList As List(Of StellarObject), ByVal gravityConstant As Double)

    'End Sub

    Friend Overrides Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0, Optional ByVal dZ As Double = 0)

        'Update visual size.
        VisualSize = Radius + CenterOfMass.Z / Radius

        'Set minimum size as to not disappear entirely.
        If VisualSize < 4 Then
            VisualSize = 4
        End If

        'The center must be updated after the visual size, otherwise the object will start glitching when getting bigger/smaller.
        Dim currentVertex As New PointFD

        If Direction = "" Then
            CenterOfMass = New PointFD(dX, dY, dZ)
        Else
            Select Case Direction
                Case "u"
                    CenterOfMass = New PointFD(CenterOfMass.X, CenterOfMass.Y - stepCount)'Move up.
                Case "d"
                    CenterOfMass = New PointFD(CenterOfMass.X, CenterOfMass.Y + stepCount) 'Move down.
                Case "l"
                    CenterOfMass = New PointFD(CenterOfMass.X - stepCount, CenterOfMass.Y) 'Move left.
                Case "r"
                    CenterOfMass = New PointFD(CenterOfMass.X + stepCount, CenterOfMass.Y)  'Move right.
            End Select
        End If


    End Sub
    Friend Overrides Sub CheckForBounce()

        'Bouncing (Left, Right & Top, Bottom).
        If CenterOfMass.X <= UniverseOffsetX + Radius Or
           CenterOfMass.X >= Universe.getWidth - Radius Then

            'Change X direction.
            VelX = -VelX 'Opposite direction.

            If VelX = 0 Then
                If CenterOfMass.X <= UniverseOffsetX + Radius Then
                    Move(0, "", UniverseOffsetX + Radius, CenterOfMass.Y)
                Else
                    Move(0, "", Universe.getWidth - Radius, CenterOfMass.Y)
                End If
            End If
        ElseIf CenterOfMass.Y <= UniverseOffsetY + Radius Or
               CenterOfMass.Y >= Universe.getHeight - Radius Then

            'Change Ydirection.
            VelY = -VelY 'Opposite direction.

            If VelY = 0 Then
                If CenterOfMass.Y <= UniverseOffsetY + Radius Then
                    Move(0, "", CenterOfMass.X, UniverseOffsetY + Radius)
                Else
                    Move(0, "", CenterOfMass.X, Universe.getHeight - Radius)
                End If
            End If
        End If

    End Sub

    Friend Overrides Sub Paint(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim starPen As Pen = Universe.getPen
        Dim tempColor As Color = Universe.getPen.Color
        Dim tempWidth As Integer = starPen.Width

        starPen.Color = Color 'Set color of star.
        starPen.Width = BorderWidth 'Set star border width.

        Dim newCenterOfMass As New PointF(CenterOfMass.X, CenterOfMass.Y)

        'Paint ellipse.
        universeGraphics.DrawEllipse(starPen,
                                     newCenterOfMass.X - CType(VisualSize, Single),
                                     newCenterOfMass.Y - CType(VisualSize, Single),
                                     2 * CType(VisualSize, Single), 2 * CType(VisualSize, Single))
        'Fill star.
        universeGraphics.FillEllipse(starPen.Brush,
                                     newCenterOfMass.X - CType(VisualSize, Single),
                                     newCenterOfMass.Y - CType(VisualSize, Single), 2 * CType(VisualSize, Single),
                                     2 * CType(VisualSize, Single))
        starPen.Color = tempColor
        starPen.Width = tempWidth

    End Sub
    Friend Overrides Sub PaintSelectionShape(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim starPen As Pen = Universe.getPen
        Dim tempColor As Color = Universe.getPen.Color
        Dim tempWidth As Integer = starPen.Width

        starPen.Color = Color 'Set color of star.
        starPen.Width = BorderWidth 'Set star border width.

        Dim newCenterOfMass As New PointF(CenterOfMass.X, CenterOfMass.Y)

        If zoomValue >= 1 Then
            zoomValue = 1
        Else
            zoomValue = (2 - zoomValue) ^ 3
        End If

        Dim zoomSingle As Single = Convert.ToSingle(zoomValue)

        starPen.Color = Color.White 'Set to white for maximum contrast.
        universeGraphics.DrawEllipse(starPen,
                                    newCenterOfMass.X - Convert.ToSingle(VisualSize) - Convert.ToSingle(BorderWidth / 2) - 2 * zoomSingle,
                                    newCenterOfMass.Y - Convert.ToSingle(VisualSize) - Convert.ToSingle(BorderWidth / 2) - 2 * zoomSingle,
                                    2 * Convert.ToSingle(VisualSize) + BorderWidth + 4 * zoomSingle,
                                    2 * Convert.ToSingle(VisualSize) + BorderWidth + 4 * zoomSingle)


        starPen.Color = tempColor
        starPen.Width = tempWidth

    End Sub
End Class
