''MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.Drawing.Drawing2D

Public Class Star
    Inherits StellarObject

    ' Allow friend access to the empty constructor.
    Friend Sub New()

    End Sub
    Friend Sub Init(ByVal sUniverse As Universe, ByVal pUniverseMatrix As Drawing2D.Matrix,
                    ByVal sLocation As PointFD, ByVal sRadius As Double, ByVal starMass As Double,
                    ByVal sBorderWidth As Integer, ByVal sColor As Color,
                    ByVal sVelInit As PointFD, Optional ByVal sVel As PointFD = Nothing)

        Universe = sUniverse
        UniverseMatrix = pUniverseMatrix

        InverseUniverseMatrix = UniverseMatrix.Clone
        InverseUniverseMatrix.Invert()

        'Set offset.
        UniverseOffsetX = Universe.OffsetX
        UniverseOffsetY = Universe.OffsetY

        Type = StellarObjectType.Star
        CenterOfMass = sLocation
        Radius = sRadius
        VisualSize = sRadius + CenterOfMass.Z / sRadius

        'Set minimum size as to not disappear entirely.
        If VisualSize < 4 Then
            VisualSize = 4
        End If

        BorderWidth = sBorderWidth
        PaintColor = sColor
        GlowColor = sColor

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

        lensingPoints.RemoveRange(0, lensingPoints.Count)
        ListItem.BackColor = PaintColor 'Set label color.

        AccX = 0
        AccY = 0
        AccZ = 0

        'Init velocity. Here we can give extra boosts to our created objects.
        VelX = sVel.X
        VelY = sVel.Y
        VelZ = sVel.Z

        IsSelected = False
        IsMerged = False
        IsMerging = False
        IsLensed = False

    End Sub

    'Friend Sub applyAcceleration(ByVal objectList As List(Of StellarObject), ByVal gravityConstant As Double)

    'End Sub

    Friend Overrides Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0, Optional ByVal dZ As Double = 0)

        'Calculate angular diameter.
        'θ = 206265 (57.3 for degrees) * (Actual diameter / Distance)
        'arc length = s = Rθ
        'Dim ad As Double = Radius * 57.3 * (2 * Radius / d)
        'Dim distZ0 As Double = Radius * 57.3 * (2 * Radius / ad)

        'Update visual size.
        VisualSize = Radius + CenterOfMass.Z / Radius

        'Set minimum visual size as to not disappear completely.
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

        'Dim delta As Integer = 0

        'Bouncing (Left, Right & Top, Bottom).
        If Not isFullyVisibleX() Then

            'Change X direction.
            VelX = -VelX 'Opposite direction.

            'Bounce back when velocity is not zero.
            'If VelY <> 0 Then
            '    delta = 1
            'End If

            'If CenterOfMass.X < UniverseOffsetX + VisualSize + BorderWidth / 2 Then
            '    Move(0, "", UniverseOffsetX + VisualSize + BorderWidth / 2 + delta, CenterOfMass.Y, CenterOfMass.Z)
            'Else
            '    Move(0, "", Universe.getWidth - VisualSize - BorderWidth / 2 - delta, CenterOfMass.Y, CenterOfMass.Z)
            'End If

        ElseIf Not isFullyVisibleY() Then

            'Change Y direction.
            VelY = -VelY 'Opposite direction.

            'Bounce back when velocity is not zero.
            'If VelY <> 0 Then
            '    delta = 1
            'End If

            'If CenterOfMass.Y < UniverseOffsetY + VisualSize + BorderWidth / 2 Then
            '    Move(0, "", CenterOfMass.X, UniverseOffsetY + VisualSize + BorderWidth / 2 + delta, CenterOfMass.Z)
            'Else
            '    Move(0, "", CenterOfMass.X, Universe.getHeight - VisualSize - BorderWidth / 2 - delta, CenterOfMass.Z)
            'End If

            'Console.Write("im going places" + Math.Sign(planetVelY).ToString + planetCenterOfMass.ToString + vbNewLine)
        End If

    End Sub

    Friend Overrides Sub Paint(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim starPen As Pen = Universe.getPen
        Dim tempColor As Color = Universe.getPen.Color
        Dim tempWidth As Integer = starPen.Width

        starPen.Color = PaintColor 'Get color of star.
        starPen.Width = BorderWidth 'Get star border width.

        Dim newCenterOfMass As New PointF(CenterOfMass.X, CenterOfMass.Y)

        'Create glow effect for the star.
        If Not PaintColor.Equals(Color.White) Then
            GlowEffect()
            starPen.Color = GlowColor
        End If

        'Paint star.
        universeGraphics.DrawEllipse(starPen,
                                         newCenterOfMass.X - Convert.ToSingle(VisualSize),
                                         newCenterOfMass.Y - Convert.ToSingle(VisualSize),
                                         2 * Convert.ToSingle(VisualSize), 2 * Convert.ToSingle(VisualSize))

        'Pain curve, if lensed.
        If IsLensed Then

            Dim lensPointList As New List(Of PointF)

            'Add four (4) standard points for drawing the star when lensed.
            'lensPointList.Add(New PointF(CenterOfMass.X, CenterOfMass.Y - Convert.ToSingle(VisualSize)))
            'lensPointList.Add(New PointF(CenterOfMass.X + Convert.ToSingle(VisualSize), CenterOfMass.Y))
            'lensPointList.Add(New PointF(CenterOfMass.X, CenterOfMass.Y + Convert.ToSingle(VisualSize)))
            'lensPointList.Add(New PointF(CenterOfMass.X - Convert.ToSingle(VisualSize), CenterOfMass.Y))

            'Add rest of lensing points.
            lensPointList.AddRange(lensingPoints)

            'Sort the points so we can draw the curve the right way.
            'lensPointList.Sort(AddressOf ComparePointsByAtan2)

            'universeGraphics.DrawClosedCurve(starPen, lensingPoints.ToArray, 0.8275, FillMode.Alternate)
            universeGraphics.DrawClosedCurve(starPen, lensPointList.ToArray, 0.8275, FillMode.Alternate)

        Else
            'Fill star.
            universeGraphics.FillEllipse(starPen.Brush,
                                     newCenterOfMass.X - Convert.ToSingle(VisualSize),
                                     newCenterOfMass.Y - Convert.ToSingle(VisualSize),
                                     2 * Convert.ToSingle(VisualSize), 2 * Convert.ToSingle(VisualSize))
        End If

        starPen.Color = tempColor
        starPen.Width = tempWidth

    End Sub
    Friend Overrides Sub PaintSelectionShape(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim starPen As Pen = Universe.getPen
        Dim tempColor As Color = Universe.getPen.Color
        Dim tempWidth As Integer = starPen.Width

        starPen.Color = PaintColor 'Set color of star.
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

    Private Sub GlowEffect()

        Static glowStatus As Integer = 1 '1 = glow, -1 = darken
        Static offset() As Integer = {0, 0, 0}
        Dim offsetIndex As Integer = -1

        'Select available channel.
        If offset(1) = 0 AndAlso PaintColor.R + offset(0) < 256 Then
            offsetIndex = 0
            GlowColor = Color.FromArgb(PaintColor.R + offset(0), PaintColor.G + offset(1), PaintColor.B + offset(2))

            If GlowColor.R = 255 Then
                offsetIndex = 1
            End If

        ElseIf offset(2) = 0 AndAlso PaintColor.G + offset(1) < 256 Then
            offsetIndex = 1
            GlowColor = Color.FromArgb(255, PaintColor.G + offset(1), PaintColor.B + offset(2))

            If GlowColor.G = 255 Then
                offsetIndex = 2
            End If

        ElseIf PaintColor.B + offset(2) < 256 Then
            offsetIndex = 2
            GlowColor = Color.FromArgb(255, 255, PaintColor.B + offset(2))

            If GlowColor.B = 255 Then
                offsetIndex = -1
            End If

        End If

        If offsetIndex >= 0 Then

            'Reverse glow effect if we reached the glow target.
            If offset.Sum = 40 Then
                glowStatus = -1
            ElseIf offset.Sum = 0 AndAlso glowStatus = -1 Then 'Start over.
                glowStatus = 1
                Exit Sub
            End If

        ElseIf offsetIndex = -1 Then 'Maximum glow (white light) reached. Find out which channel was last incremented.

            If PaintColor.B = 255 AndAlso GlowColor.B <> 255 Then
                offsetIndex = 2
            ElseIf PaintColor.G = 255 AndAlso GlowColor.G <> 255 Then
                offsetIndex = 1
            Else
                offsetIndex = 0
            End If

            'Begin darkening.
            glowStatus = -1

        End If

        'Don't go below zero. Move to next channel.
        If glowStatus = -1 And offset(offsetIndex) = 0 Then
            offsetIndex -= 1 'The offset here is at least 1.
        End If

        'Adjust offset.
        offset(offsetIndex) += 1 * glowStatus

    End Sub

End Class
