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
                   objectCenterOfMass.X <= objectUniverse.getRightBottomBoundary().X - objectRadius 'If fully inside the X visible area.
        End Get
    End Property
    Public ReadOnly Property isFullyVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY + objectRadius And
                   objectCenterOfMass.Y <= objectUniverse.getRightBottomBoundary().Y - objectRadius 'If fully inside the Y visible area.
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
                    ByVal sLocation As PointF, ByVal sRadius As Double,
                    ByVal sBorderWidth As Integer, ByVal sColor As Color, ByVal sType As Integer,
                    Optional ByVal sVelX As Double = 0, Optional ByVal sVelY As Double = 0)

        objectUniverse = sUniverse 'Set universe the object is in.
        objectUniverseMatrix = pUniverseMatrix 'Set object's universe matrix.

        'Set offset.
        objectUniverseOffsetX = objectUniverse.OffsetX
        objectUniverseOffsetY = objectUniverse.OffsetY

        objectCenterOfMass = New PointF(sLocation.X, sLocation.Y) 'Init center of mass.
        objectRadius = sRadius 'Init object radius.

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
        objectOutOfBounds = False 'Init "out of bounds" flag.

    End Sub

    Friend Sub applyAcceleration(ByVal objectList As List(Of StellarObject), ByVal gravityConstant As Double)

        Dim delta As Double = 1 / 100 'Catalyst.
        Dim distanceMultiplier = objectUniverse.getDistanceMultiplier()

        'Reset accelerations.
        objectAccX = 0
        objectAccY = 0

        For Each obj In objectList.FindAll(Function(o) Not o.IsMerged And Not o.Equals(Me))

            'Distance vector of two bodies.
            Dim distanceVector As New PointF(objectCenterOfMass.X - obj.CenterOfMass.X,
                                             objectCenterOfMass.Y - obj.CenterOfMass.Y)
            Dim distanceLength As Double = Math.Sqrt(distanceVector.X ^ 2 + distanceVector.Y ^ 2)  'Get distance(magnitude).

            'Surface of object collide. Slow down.
            If distanceLength <= Radius + obj.Radius Then

                ' delta = 1 / 1000

                'Merge stars.
                If distanceLength <= Radius Or distanceLength <= obj.Radius Then

                    obj.IsMerging = True 'Set flag for merging.
                    IsMerging = True 'Set flag for merging.
                    delta = 1

                    Dim newCenterOfMass As New PointF 'Calculate point after we find out who ate who.
                    Dim newStarMass As Double = 0 'Let's find out first, who has the biggest mass.
                    Dim newStarRadius As Integer = 0 'Let's find out first, who has the biggest radius.

                    newStarMass = objectMass + obj.Mass

                    'Calculate new velocity.
                    Dim newVelX As Double = Math.Round((objectVelX * objectMass * delta + obj.VelX * obj.Mass * delta) / newStarMass, 10)
                    Dim newVelY As Double = Math.Round((objectVelY * objectMass * delta + obj.VelY * obj.Mass * delta) / newStarMass, 10)

                    Dim newobjectType = objectType + obj.Type 'New type is the sum of the two object types.
                    Dim newcolor As Color = Color.FromArgb(objectColor.R / 2 + obj.Color.R / 2,
                                                                objectColor.G / 2 + obj.Color.G / 2,
                                                                objectColor.B / 2 + obj.Color.B / 2)
                    If objectRadius >= obj.Radius Then

                        obj.IsMerged = True 'Set merge flag for the eaten object.
                        obj.IsSelected = False 'Clear selection flag.

                        newCenterOfMass = New PointF(objectCenterOfMass.X, objectCenterOfMass.Y) 'Set center of mass on eater.
                        newStarRadius = objectRadius + obj.Radius / 4 'New radius is the radius of the eater + a 4th the radius of the eaten object.

                        'Initialize new merged Star.
                        Init(objectUniverse, objectUniverseMatrix, newCenterOfMass, newStarRadius, objectBorderWidth, newcolor, newobjectType,
                             newVelX, newVelY)

                        objectMass = newStarMass 'Set new merged mass.
                        objectLabel.ForeColor = newcolor 'Set new label color.

                    Else
                        IsMerged = True 'Set merge flag for the eaten object.
                        IsSelected = False 'Clear selection flag.

                        newCenterOfMass = New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y) 'Set center of mass on eaten object.
                        newStarRadius = obj.Radius + objectRadius / 4 'New radius is the radius of eater + a 4th the radius of the eaten object.

                        'Initialize new merged Star.
                        CType(obj, Star).Init(objectUniverse, objectUniverseMatrix, newCenterOfMass, newStarRadius, objectBorderWidth, newcolor, newobjectType,
                                  newVelX, newVelY)

                        obj.Mass = newStarMass 'Set new merged mass.
                        obj.Label.ForeColor = newcolor 'Set new label color.

                    End If

                    obj.IsMerging = False 'Clear flag.
                    IsMerging = False 'Clear flag.
                    Exit For
                End If
            End If

            'Multiply distance to simulate "normal" distances of stellar objects in the universe.
            distanceVector.X *= distanceMultiplier
            distanceVector.Y *= distanceMultiplier

            distanceLength = Math.Sqrt(distanceVector.X ^ 2 + distanceVector.Y ^ 2)  'Get new distance(magnitude).

            'Calculate gravity force. Check below for a guide to the physics.
            'http://www.cs.princeton.edu/courses/archive/fall03/cs126/assignments/nbody.html

            'force = gravityConstant * (star.GetMass * planetMass) / Math.Pow(distanceLength, 2)
            'F = G * (M * m) / d ^ 2
            'Fx = F * cos(f) = F * dx / d => .... => Ax = M * dx / d^3
            'Fy = F * sin(f) = F * dy / d => .... => Ay = M * dy / d^3

            Dim inv_d As Double = 1.0 / (distanceLength * distanceLength * distanceLength)

            'Precalculate force component (1/r^2) * direction (dx/r) = dx / r^3
            Dim dx As Double = (obj.CenterOfMass.X - objectCenterOfMass.X)
            Dim dy As Double = (obj.CenterOfMass.Y - objectCenterOfMass.Y)

            dx *= inv_d
            dy *= inv_d

            'Calculate accelerations for both bodies and apply them to the velocities.
            Dim currentAccX As Double = obj.Mass() * dx * delta
            Dim currentAccY As Double = obj.Mass() * dy * delta

            objectAccX += currentAccX
            objectAccY += currentAccY

            objectVelX += currentAccX
            objectVelY += currentAccY

            'We calculated the accelaration that is applied to this planet(Me), from the other planet(iteration).
            'So when the other planet starts calculating it's applied accelaration from this planet(Me), it must produce the same results.

            'Due to Threads not running simultaneously, the other planet will not produce the same result, because this planet(Me)
            'has already moved a little, because it calculated it's applied accelaration, FIRST.

            'So when we calculate this planet's (Me) applied accelaration, we can apply the opposite(-) accelaration to the other planet.

            currentAccX = objectMass * (-dx) * delta
            currentAccY = objectMass * (-dy) * delta

            obj.AccX += currentAccX
            obj.AccY += currentAccY

            obj.AddVelX(currentAccX)
            obj.AddVelY(currentAccY)
        Next

        'Dim newpos As New PointF(objectVelX, objectVelY)
        'Move body.
        'Move(0, "", objectCenterOfMass.X + newpos.X, objectCenterOfMass.Y + newpos.Y)

    End Sub

    Friend Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0)

        Dim currentVertex As New PointF

        If Direction = "" Then
            objectCenterOfMass = New PointF(dX, dY)
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
    Friend Overrides Sub Paint()

        Dim universeGraphics As Graphics = objectUniverse.getGraphics
        Dim starPen As Pen = objectUniverse.getPen

        Dim tempColor As Color = objectUniverse.getPen.Color
        Dim tempWidth As Integer = starPen.Width

        starPen.Color = objectColor 'Set color of star.
        starPen.Width = objectBorderWidth 'Set star border width.

        'Paint ellipse.
        universeGraphics.DrawEllipse(starPen, objectCenterOfMass.X - objectRadius, objectCenterOfMass.Y - objectRadius, 2 * objectRadius, 2 * objectRadius)
        'Fill star.
        universeGraphics.FillEllipse(starPen.Brush, objectCenterOfMass.X - objectRadius, objectCenterOfMass.Y - objectRadius, 2 * objectRadius, 2 * objectRadius)

        'Paint selection ellipse, if selected.
        If IsSelected Then
            starPen.Color = Color.White 'Set to white for maximum contrast.
            universeGraphics.DrawEllipse(starPen, objectCenterOfMass.X - objectRadius - 2, objectCenterOfMass.Y - objectRadius - 2, 2 * (objectRadius + 2), 2 * (objectRadius + 2))
        End If

        starPen.Color = tempColor  'Restore color.
        starPen.Width = tempWidth 'Restore width.
    End Sub
    Friend Sub CheckForBounce()

        'Bouncing (Left, Right & Top, Bottom).
        If objectCenterOfMass.X <= objectUniverseOffsetX + objectRadius Or
           objectCenterOfMass.X >= objectUniverse.getRightBottomBoundary().X - objectRadius Then
            'Change X direction.
            objectVelX = -objectVelX 'Opposite direction.

            If objectVelX = 0 Then
                If objectCenterOfMass.X <= objectUniverseOffsetX + objectRadius Then
                    Move(0, "", objectUniverseOffsetX + objectRadius, objectCenterOfMass.Y)
                Else
                    Move(0, "", objectUniverse.getRightBottomBoundary().X - objectRadius, objectCenterOfMass.Y)
                End If
            End If
        ElseIf objectCenterOfMass.Y <= objectUniverseOffsetY + objectRadius Or
               objectCenterOfMass.Y >= objectUniverse.getRightBottomBoundary().Y - objectRadius Then
            'Change Ydirection.
            objectVelY = -objectVelY 'Opposite direction.

            If objectVelY = 0 Then
                If objectCenterOfMass.Y <= objectUniverseOffsetY + objectRadius Then
                    Move(0, "", objectCenterOfMass.X, objectUniverseOffsetY + objectRadius)
                Else
                    Move(0, "", objectCenterOfMass.X, objectUniverse.getRightBottomBoundary().Y - objectRadius)
                End If
            End If
        End If

    End Sub

End Class
