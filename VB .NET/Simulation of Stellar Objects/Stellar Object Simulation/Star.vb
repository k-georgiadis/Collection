''MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Public Class Star
    Inherits StellarObject

    'In detail, the minimum mass for hydrogen fusion in a manner that is
    'capable of sustaining a star in equilibrium against gravitational contraction, is about 0.075M, +-0.002M.
    Private Const M As Double = 19890000000.0 ' = 1.989E+10 'This is my edited mass. The real Solar Mass = 1.989E+30

    Public ReadOnly Property isFullyVisibleX() As Boolean
        Get
            Return objectCenterOfMass.X >= objectUniverseOffsetX + objectRadius And
                   objectCenterOfMass.X <= objectUniverse.getWidth - objectRadius 'If fully inside the X visible area.
        End Get
    End Property
    Public ReadOnly Property isFullyVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY + objectRadius And
                   objectCenterOfMass.Y <= objectUniverse.getHeight - objectRadius 'If fully inside the Y visible area.
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
                    ByVal sLocation As PointFD, ByVal sRadius As Double,
                    ByVal sBorderWidth As Integer, ByVal sColor As Color, ByVal sType As Integer,
                    Optional ByVal sVelX As Double = 0, Optional ByVal sVelY As Double = 0)

        objectUniverse = sUniverse 'Set universe the object is in.
        objectUniverseMatrix = pUniverseMatrix 'Set object's universe matrix.

        objectInverseUniverseMatrix = objectUniverseMatrix.Clone 'Save inverted matrix.
        objectInverseUniverseMatrix.Invert()

        'Set offset.
        objectUniverseOffsetX = objectUniverse.OffsetX
        objectUniverseOffsetY = objectUniverse.OffsetY

        objectCenterOfMass = sLocation 'Init center of mass.
        objectRadius = sRadius 'Init object radius.
        objectSize = objectRadius 'Use radius as size for stars.

        objectBorderWidth = sBorderWidth 'Init object pen border width.
        objectColor = sColor 'Init object color.
        objectType = sType 'Init object type.

        'Init object mass. Minimum mass for object. The " * sType" part is my addition so I can make objects with different masses.
        objectMass = 0.075 * M * sType

        objectTransitionDirection = "" 'Init transition direction. No direction.

        'Init duplicate points.
        objectDuplicatePointRight = New PointF(0, 0)
        objectDuplicatePointLeft = New PointF(0, 0)
        objectDuplicatePointTop = New PointF(0, 0)
        objectDuplicatePointBottom = New PointF(0, 0)

        objectLabel = New Label 'Init object label.
        objectLabel.ForeColor = objectColor 'Set label color.
        objectLabelHidden = False 'Init state of object label.

        'Initiliaze velocities and accelerations.
        'Here we can give extra boosts to our created objects.
        'Don't forget to account for the distance multiplier ONLY when creating new objects.
        If IsMerging Then
            objectVelX = sVelX 'Init X Velocity.
            objectVelY = sVelY 'Init Y Velocity.
        Else
            objectVelX = sVelX / objectUniverse.getDistanceMultiplier 'Init X Velocity.
            objectVelY = sVelY / objectUniverse.getDistanceMultiplier 'Init Y Velocity.
            objectSelected = False 'Init selection flag.
        End If

        objectAccX = 0 'Init Acceleration X.
        objectAccY = 0 'Init Acceleration Y.
        objectMerged = False 'Init state of object.

        objectMerging = False 'Init merging status flag.

    End Sub

    'Friend Sub applyAcceleration(ByVal objectList As List(Of StellarObject), ByVal gravityConstant As Double)

    'End Sub

    Friend Overrides Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0)

        Dim currentVertex As New PointFD

        If Direction = "" Then
            objectCenterOfMass = New PointFD(dX, dY)
        Else
            Select Case Direction
                Case "u"
                    objectCenterOfMass.Y = objectCenterOfMass.Y - stepCount 'Move up.
                Case "d"
                    objectCenterOfMass.Y = objectCenterOfMass.Y + stepCount 'Move down.
                Case "l"
                    objectCenterOfMass.X = objectCenterOfMass.X - stepCount 'Move left.
                Case "r"
                    objectCenterOfMass.X = objectCenterOfMass.X + stepCount 'Move right.
            End Select
        End If

    End Sub
    Friend Overrides Sub CheckForBounce()

        'Bouncing (Left, Right & Top, Bottom).
        If objectCenterOfMass.X <= objectUniverseOffsetX + objectRadius Or
           objectCenterOfMass.X >= objectUniverse.getWidth - objectRadius Then

            'Change X direction.
            objectVelX = -objectVelX 'Opposite direction.

            If objectVelX = 0 Then
                If objectCenterOfMass.X <= objectUniverseOffsetX + objectRadius Then
                    Move(0, "", objectUniverseOffsetX + objectRadius, objectCenterOfMass.Y)
                Else
                    Move(0, "", objectUniverse.getWidth - objectRadius, objectCenterOfMass.Y)
                End If
            End If
        ElseIf objectCenterOfMass.Y <= objectUniverseOffsetY + objectRadius Or
               objectCenterOfMass.Y >= objectUniverse.getHeight - objectRadius Then

            'Change Ydirection.
            objectVelY = -objectVelY 'Opposite direction.

            If objectVelY = 0 Then
                If objectCenterOfMass.Y <= objectUniverseOffsetY + objectRadius Then
                    Move(0, "", objectCenterOfMass.X, objectUniverseOffsetY + objectRadius)
                Else
                    Move(0, "", objectCenterOfMass.X, objectUniverse.getHeight - objectRadius)
                End If
            End If
        End If

    End Sub

    Friend Overrides Sub Paint(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

        Dim starPen As Pen = objectUniverse.getPen
        Dim tempColor As Color = objectUniverse.getPen.Color
        Dim tempWidth As Integer = starPen.Width

        starPen.Color = objectColor 'Set color of star.
        starPen.Width = objectBorderWidth 'Set star border width.

        Dim centerOfMass As New PointF(objectCenterOfMass.X, objectCenterOfMass.Y)

        'Paint ellipse.
        universeGraphics.DrawEllipse(starPen, centerOfMass.X - objectRadius, centerOfMass.Y - objectRadius, 2 * objectRadius, 2 * objectRadius)
        'Fill star.
        universeGraphics.FillEllipse(starPen.Brush, centerOfMass.X - objectRadius, centerOfMass.Y - objectRadius, 2 * objectRadius, 2 * objectRadius)

        'Paint selection ellipse, if selected.
        If IsSelected Then

            If zoomValue >= 1 Then
                zoomValue = 1
            Else
                zoomValue = (2 - zoomValue) ^ 3
            End If

            starPen.Color = Color.White 'Set to white for maximum contrast.
            universeGraphics.DrawEllipse(starPen, centerOfMass.X - objectRadius - 2 * CType(zoomValue, Single), centerOfMass.Y - objectRadius - 2 * CType(zoomValue, Single),
                                         2 * (objectRadius + 2 * CType(zoomValue, Single)), 2 * (objectRadius + 2 * CType(zoomValue, Single)))
        End If

        starPen.Color = tempColor  'Restore color.
        starPen.Width = tempWidth 'Restore width.

    End Sub

End Class
