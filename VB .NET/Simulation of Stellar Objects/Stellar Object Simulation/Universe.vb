'MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.Drawing.Drawing2D
Imports System.Runtime.Remoting
Imports System.Windows
Imports StellarObjectSimulation.StellarObject

Public Class Universe

    Private universeGraphics As Graphics
    Private universePen As Pen
    Private bmpImage As Bitmap

    Private defaultFormWidth As Integer
    Private defaultFormHeight As Integer

    Private universeOffsetX As Double
    Private universeOffsetY As Double
    Private universeMatrix As Drawing2D.Matrix
    Private defaultUniverseMatrix As Drawing2D.Matrix

    'In reality it's + 1 pixel, since we like round numbers [-X/2, +X/2].
    Private Const cosmosSectionWidth As Double = 50000
    Private Const cosmosSectionHeight As Double = 50000
    Private Const cosmosSectionDepth As Double = 50000

    Private Const maxCosmosSections As Integer = 9

    Private universeWidth As Integer
    Private universeHeight As Integer

    Private visibleWidth As Integer
    Private visibleHeight As Integer

    Private planetList As New List(Of Planet)
    Private starList As New List(Of Star)
    Private singularityList As New List(Of Singularity)

    Private universeTickRate As Integer
    Private universeFrameRate As Integer

    Private canBounce As Boolean

    Private drawTrajectories As Boolean
    Private relativeTrajectories As Boolean
    Private bothTrajectories As Boolean
    Private maxTrajPoints As Integer

    Private universeRealistic As Boolean
    Private universeDragged As Boolean
    Private universePaused As Boolean

    'In detail, the minimum mass for hydrogen fusion in a manner that is
    'capable of sustaining a star in equilibrium against gravitational contraction, is about 0.075M, +-0.002M.
    Private Const universeSolarMass As Double = 1.989E+30 ' Mass of the sun.
    Private Const universeMinFusionMass As Double = 0.075 * universeSolarMass

    Private Const C As Double = 300000000 'Speed of light.
    Private Const gravityConstant As Double = 0.00000000006674
    Private universeDistanceDelta As Double = 1000000000.0 'Default value. 1 pixel = 1 million km = 1 billion meters.

    Public Property DefFormWidth As Integer
        Get
            Return defaultFormWidth
        End Get
        Set(value As Integer)
            defaultFormWidth = value
        End Set
    End Property
    Public Property DefFormHeight As Integer
        Get
            Return defaultFormHeight
        End Get
        Set(value As Integer)
            defaultFormHeight = value
        End Set
    End Property

    Public Property OffsetX() As Double
        Get
            Return universeOffsetX
        End Get
        Set(ByVal Offset As Double)
            universeOffsetX = Offset
        End Set
    End Property
    Public Property OffsetY() As Double
        Get
            Return universeOffsetY
        End Get
        Set(ByVal Offset As Double)
            universeOffsetY = Offset
        End Set
    End Property
    Public Property defUniverseMatrix As Matrix
        Get
            Return defaultUniverseMatrix
        End Get
        Set(value As Matrix)
            defaultUniverseMatrix = value
        End Set
    End Property


    Public ReadOnly Property getGraphics() As Graphics
        Get
            Return universeGraphics
        End Get
    End Property
    Public ReadOnly Property getPen() As Pen
        Get
            Return universePen
        End Get
    End Property
    Public ReadOnly Property getImage() As Bitmap
        Get
            Return bmpImage
        End Get
    End Property
    Public ReadOnly Property getWidth() As Integer
        Get
            Return universeWidth
        End Get
    End Property
    Public ReadOnly Property getHeight() As Integer
        Get
            Return universeHeight
        End Get
    End Property

    Public ReadOnly Property Planets() As List(Of Planet)
        Get
            Return planetList
        End Get
    End Property
    Public ReadOnly Property Stars() As List(Of Star)
        Get
            Return starList
        End Get
    End Property
    Public ReadOnly Property Objects() As List(Of StellarObject)
        Get
            Dim objectList As New List(Of StellarObject)

            Try
                objectList.AddRange(starList)
                objectList.AddRange(planetList)
                objectList.AddRange(singularityList)
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
            Return objectList
        End Get
    End Property

    Public Property Bounce As Boolean
        Get
            Return canBounce
        End Get
        Set(value As Boolean)
            canBounce = value
        End Set
    End Property

    Public Property DrawTraj As Boolean
        Get
            Return drawTrajectories
        End Get
        Set(value As Boolean)
            drawTrajectories = value
        End Set
    End Property
    Public Property RelativeTraj As Boolean
        Get
            Return relativeTrajectories
        End Get
        Set(value As Boolean)
            relativeTrajectories = value
        End Set
    End Property
    Public Property drawBothTraj As Boolean
        Get
            Return bothTrajectories
        End Get
        Set(value As Boolean)
            bothTrajectories = value
        End Set
    End Property
    Public Property MaxTrajectoryPoints As Integer
        Get
            Return maxTrajPoints
        End Get
        Set(value As Integer)
            maxTrajPoints = value
        End Set
    End Property

    Public Property isRealistic As Boolean
        Get
            Return universeRealistic
        End Get
        Set(value As Boolean)
            universeRealistic = value
        End Set
    End Property
    Public Property isDragged As Boolean
        Get
            Return universeDragged
        End Get
        Set(value As Boolean)
            universeDragged = value
        End Set
    End Property
    Public Property isPaused As Boolean
        Get
            Return universePaused
        End Get
        Set(value As Boolean)
            universePaused = value
        End Set
    End Property

    Public ReadOnly Property getGravityConstant() As Double
        Get
            Return gravityConstant
        End Get
    End Property
    Public Property DistanceMultiplier() As Double
        Get
            Return universeDistanceDelta
        End Get
        Set(value As Double)
            universeDistanceDelta = value
        End Set
    End Property

    Public Property TickRate As Integer
        Get
            Return universeTickRate
        End Get
        Set(value As Integer)
            universeTickRate = value
        End Set
    End Property
    Public Property FrameRate As Integer
        Get
            Return universeFrameRate
        End Get
        Set(value As Integer)
            universeFrameRate = value
        End Set
    End Property

    Public Shared ReadOnly Property SolarMass As Double
        Get
            Return universeSolarMass
        End Get
    End Property

    Public Shared ReadOnly Property MinFusionMass As Double
        Get
            Return universeMinFusionMass
        End Get
    End Property

    Public ReadOnly Property SectionWidth As Double
        Get
            Return cosmosSectionWidth
        End Get
    End Property

    Public ReadOnly Property SectionHeight As Double
        Get
            Return cosmosSectionHeight
        End Get
    End Property

    Public ReadOnly Property SectionDepth As Double
        Get
            Return cosmosSectionDepth
        End Get
    End Property

    Public Shared ReadOnly Property getC As Double
        Get
            Return C
        End Get
    End Property

    ' Allow friend access to the empty constructor.
    Friend Sub New()
    End Sub

    Friend Sub Init(ByVal bPen As Pen, ByVal uWidth As Integer, ByVal uHeight As Integer,
                    ByVal fWidth As Integer, ByVal fHeight As Integer, ByVal offset As PointFD,
                    ByVal _maxTrajPoints As Integer)

        universePen = bPen
        universeOffsetX = offset.X
        universeOffsetY = offset.Y

        universeWidth = uWidth
        universeHeight = uHeight

        'Set current default form values.
        defaultFormHeight = fWidth
        defaultFormHeight = fHeight

        'Create one time only.
        If bmpImage Is Nothing Then

            bmpImage = New Bitmap(universeWidth, universeHeight) 'Create image. Take in account the DPI factor too.

            universeGraphics = Graphics.FromImage(bmpImage)
            universeGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            universeGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            'Save transformation so we don't run into access violations.
            universeMatrix = universeGraphics.Transform.Clone
            defUniverseMatrix = universeMatrix.Clone

            Dim visibleBounds As RectangleF = bmpImage.GetBounds(universeGraphics.PageUnit)
            visibleWidth = visibleBounds.Width
            visibleHeight = visibleBounds.Height

            universeGraphics.Clip = New Region(New RectangleF(visibleBounds.Left + universeOffsetX,
                                                              visibleBounds.Top + universeOffsetY, visibleWidth, visibleHeight))
        End If

        universeGraphics.Clear(Color.Black)
        canBounce = False

        drawTrajectories = True
        bothTrajectories = False
        maxTrajPoints = _maxTrajPoints 'Max number of points used for each trajectory.

        universeDragged = False 'Flag to check if the universe is being dragged.
        universePaused = False

    End Sub
    Friend Sub Live(ByRef StarArrayInUseFlag As Boolean, ByRef PlanetArrayInUseFlag As Boolean)

        applyAcceleration(StarArrayInUseFlag, PlanetArrayInUseFlag)

    End Sub

    Friend Sub AddPlanet(ByVal newPlanet As Planet)
        planetList.Add(newPlanet)
    End Sub
    Friend Sub AddStar(ByVal newStar As Star)
        starList.Add(newStar)
    End Sub
    Friend Sub AddBlackHole(ByVal newBlackHole As Singularity)
        singularityList.Add(newBlackHole)
    End Sub

    Friend Sub applyAcceleration(ByRef StarArrayInUseFlag As Boolean, ByRef PlanetArrayInUseFlag As Boolean)

        If StarArrayInUseFlag Or PlanetArrayInUseFlag Then
            Exit Sub
        End If

        StarArrayInUseFlag = True
        PlanetArrayInUseFlag = True

        Try
            'Calculate acceleration from all objects in the SAME cosmos section.
            For i = 1 To maxCosmosSections

                Dim section As Integer = i

                'Make local copies to avoid "object in use" exceptions, as much as possible.
                Dim objList As New List(Of StellarObject)
                objList.AddRange(Objects.FindAll(Function(o) o.CosmosSection = section))

                'First, reset their acceleration.
                For Each obj In objList
                    If Not obj.IsMerged Then
                        obj.AccX = 0
                        obj.AccY = 0
                        obj.AccZ = 0
                    End If
                Next

                For Each obj In objList

                    If obj.IsMerged Then
                        Continue For
                    End If

                    obj.applyAcceleration(objList, gravityConstant)
                Next

                'Remove merged objects.
                objList.RemoveAll(Function(o) o.IsMerged)
                planetList.RemoveAll(Function(o) o.IsMerged)
                starList.RemoveAll(Function(o) o.IsMerged)

                'Now move them.
                For Each obj In objList

                    'Move object. Remember to account for the distance multiplier.
                    obj.Move(0, "", obj.CenterOfMass.X + obj.VelX / DistanceMultiplier,
                                    obj.CenterOfMass.Y + obj.VelY / DistanceMultiplier,
                                    obj.CenterOfMass.Z + obj.VelZ / DistanceMultiplier)

                    If canBounce And Not universeDragged And obj.isVisible Then
                        obj.CheckForBounce()
                    End If

                Next
            Next
        Catch ex As Exception
            Console.Write(ex.ToString)
        End Try

        StarArrayInUseFlag = False
        PlanetArrayInUseFlag = False

    End Sub

    Friend Sub ResizeUniverse(ByVal clipOffset As PointF, ByVal universeSize As PointF, ByVal matrix As Drawing2D.Matrix)

        'Update local values.
        universeMatrix = matrix
        universeOffsetX = clipOffset.X
        universeOffsetY = clipOffset.Y
        universeWidth = universeSize.X
        universeHeight = universeSize.Y

    End Sub
    Friend Sub ResetAllOffsets(ByVal objList As List(Of StellarObject))

        For Each obj In objList

            'Set new offset.
            obj.UniverseOffsetX = universeOffsetX
            obj.UniverseOffsetY = universeOffsetY

            'Update universe matrices for the object.
            obj.UniverseMatrix = universeMatrix.Clone
            obj.InverseUniverseMatrix = universeMatrix.Clone
            obj.InverseUniverseMatrix.Invert()
        Next

    End Sub

    Friend Sub GravityLensEffect(ByVal objList As List(Of StellarObject))

        For Each bh In objList.FindAll(Function(x) x.Type = StellarObjectType.BlackHole)

            'Assuming we already sorted the list by Z in ascending order.
            'Take only the stellar objects behind the black hole.
            For Each obj In objList.FindAll(Function(x) x.CenterOfMass.Z < bh.CenterOfMass.Z)

                '2D distance vector of two bodies.
                Dim distanceVector As New PointFD(bh.CenterOfMass.X - obj.CenterOfMass.X,
                                                  bh.CenterOfMass.Y - obj.CenterOfMass.Y)
                Dim distanceLength As Double = Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y * distanceVector.Y)

                'The lensing effect starts a little further before the event horizon.
                Dim lensingRadius As Double = bh.VisualSize + bh.VisualSize / 2
                Dim surfaceDist As Double = lensingRadius + obj.VisualSize
                Dim objOffset As Double = distanceLength - surfaceDist

                'Check if the object passed the lensing radius.
                If objOffset <= 0 Then

                    Dim starPen As New Pen(obj.GlowColor)

                    'Increase border width as the object region that overlaps the black hole increases.
                    starPen.Width = Math.Abs(objOffset) / distanceLength + 2 * (obj.VisualSize / obj.Radius)

                    'Create rectangle to bound ellipse.
                    Dim rect As New RectangleF(bh.CenterOfMass.X - lensingRadius,
                                               bh.CenterOfMass.Y - lensingRadius,
                                               2 * lensingRadius, 2 * lensingRadius)

                    'Calculate angle between distance vectors.
                    Dim v1 As New System.Windows.Vector(distanceVector.X, distanceVector.Y)
                    Dim v2 As New System.Windows.Vector(distanceVector.X, 0)
                    Dim a = System.Windows.Vector.AngleBetween(v2, v1)


                    Dim startAngle As Single
                    Dim sweepAngle As Single

                    'Adjust angle depending on the side the object comes from.
                    If distanceVector.X > 0 Then
                        startAngle = 180 + a 'Left side.
                    Else
                        startAngle = 360 + a 'Right side.
                    End If

                    'Get intersecting common chord length.
                    Dim chordLength As Double = CommonChordLength(lensingRadius, obj.VisualSize, distanceLength)

                    'Calculate arc angle and then it's length.
                    Dim theta As Double = 2 * Math.Asin(chordLength / (2 * obj.VisualSize))
                    Dim arcLength As Double = theta * obj.VisualSize
                    'startAngle -= (theta * 180 / Math.PI) / 2
                    'sweepAngle = arcLength

                    'If half the object is behind the singularity, the chord length will start decreasing since it's reached the maximum value.
                    'We make sure the angles keep increasing, by keeping the chord length at max value.
                    If distanceLength < bh.VisualSize Then
                        chordLength = 2 * obj.VisualSize
                    End If

                    'draw lines
                    'Dim orthgonalSideLen As Double = Math.Sqrt(obj.VisualSize * obj.VisualSize - (chordLength / 2) * (chordLength / 2))
                    'universeGraphics.DrawLine(New Pen(Color.White),
                    '                          New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y),
                    '                          New PointF(obj.CenterOfMass.X + orthgonalSideLen, obj.CenterOfMass.Y + chordLength / 2))
                    'universeGraphics.DrawLine(New Pen(Color.White),
                    '                          New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y),
                    '                          New PointF(obj.CenterOfMass.X + orthgonalSideLen, obj.CenterOfMass.Y - chordLength / 2))

                    'Calculate arc start angle and length.

                    'The below settings create a somewhat pseudo-lensing effect.
                    'I call it Linear Gravitational Lensing.
                    'It basically wraps the object along the lensing radius.
                    'The real gravitational lensing works differently.

                    'Add offset so we wrap around the lensing radius.
                    startAngle += (objOffset * 180) / surfaceDist

                    'With just these settings, we create the arc that is covered by the lensing radius only.
                    startAngle -= chordLength
                    sweepAngle = 2 * chordLength

                    'Add offset so we wrap around the lensing radius.
                    sweepAngle += 2 * (Math.Abs(objOffset) * 180) / surfaceDist

                    If sweepAngle > 360 Then

                        sweepAngle = 360 'That's enough.

                        Dim ringRadius As Double = lensingRadius + starPen.Width
                        Dim ringArea As Double = Math.PI * (ringRadius * ringRadius - lensingRadius * lensingRadius)
                        Dim objArea As Double = Math.PI * (obj.VisualSize * obj.VisualSize)

                        'Make sure the ring is not bigger than the object itself.
                        'We need to find out what's the pen width value so that both areas are equal.
                        If ringArea > objArea Then
                            'W = sqrt(R^2 + r^2) - R,   where R = ring outer radius, r = object radius
                            starPen.Width = Math.Sqrt((lensingRadius * lensingRadius) + (obj.VisualSize * obj.VisualSize)) - lensingRadius
                        End If

                    End If

                    ' Draw arc to screen.
                    universeGraphics.DrawArc(starPen, rect, startAngle, sweepAngle)

                    'universeGraphics.DrawString(chordLength.ToString, obj.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 50)
                End If

            Next

        Next

    End Sub

    Friend Sub RealGravityLensEffect(ByVal objList As List(Of StellarObject))

        For Each bh In objList.FindAll(Function(x) x.Type = StellarObjectType.BlackHole)

            'Assuming we already sorted the list by Z in ascending order.
            'Take only the stellar objects behind the black hole.
            For Each obj In objList.FindAll(Function(x) x.CenterOfMass.Z < bh.CenterOfMass.Z)

                '2D distance vector of two bodies.
                Dim distanceVector As New PointFD(bh.CenterOfMass.X - obj.CenterOfMass.X,
                                                  bh.CenterOfMass.Y - obj.CenterOfMass.Y)
                Dim distanceLength As Double = Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y * distanceVector.Y)

                'The lensing effect starts a little further before the event horizon.
                Dim lensingRadius As Double = 5 * bh.VisualSize + bh.VisualSize / 2
                Dim surfaceDist As Double = lensingRadius + obj.VisualSize
                Dim objOffset As Double = surfaceDist - distanceLength

                'Check if the object passed the lensing radius.
                If objOffset >= 0 Then

                    'Calculate angle between distance vectors.
                    Dim v1 As New Vector(distanceVector.X, distanceVector.Y)
                    Dim v2 As New Vector(distanceVector.X, 0)
                    Dim a = Vector.AngleBetween(v2, v1)

                    'Get intersecting common chord length.
                    Dim chordLength As Double = CommonChordLength(lensingRadius, obj.VisualSize, distanceLength)

                    'Find middle point of intersection.
                    Dim c1 As New PointF(bh.CenterOfMass.X, bh.CenterOfMass.Y)
                    Dim c2 As New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y)
                    Dim middlePoint As PointF = GetMiddleChordPoint(distanceLength, lensingRadius, obj.VisualSize, c1, c2)
                    Dim intersectPoints As PointF() = CircleIntersectionPoints(distanceLength, lensingRadius, obj.VisualSize, c1, c2)

                    If middlePoint.Equals(New PointF(0, 0)) Then
                        middlePoint = c1
                    End If

                    'Get line equation from the two (2) centers of the intersecting circles and their intersection points.
                    Dim lineEquationParams As Double() = LineEquationFromTwoPoints(c2, middlePoint)
                    Dim lineIntersectPoints As PointF() = LineIntersectionPointsWithCircle(lineEquationParams(0), lineEquationParams(1), c2, obj.VisualSize)

                    Dim thetaXY As Double = 0
                    Dim thetaZX As Double = 0
                    Dim thetaZY As Double = 0

                    'If there is an intersection, show it.
                    If Not lineIntersectPoints.Contains(New PointF(0, 0)) Then

                        'Find point on stellar object that is closest to the singularity.
                        Dim closestPoint As PointF = lineIntersectPoints.ToList.Find(
                                                            Function(p)
                                                                Return Math.Sqrt((p.X - c1.X) * (p.X - c1.X) +
                                                                                 (p.Y - c1.Y) * (p.Y - c1.Y)) < lensingRadius
                                                            End Function
                                                        )
                        universeGraphics.DrawLine(New Pen(Color.Red),
                                                  New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y),
                                                  closestPoint)

                        'Find distance from the center of the singularity.
                        Dim distX As Double = closestPoint.X - bh.CenterOfMass.X
                        Dim distY As Double = closestPoint.Y - bh.CenterOfMass.Y
                        Dim distZ As Double = (bh.CenterOfMass.Z - obj.CenterOfMass.Z) * DistanceMultiplier

                        'Constant so we can reuse it.
                        Dim dividend As Double = 4 * gravityConstant * (100000 * bh.Mass) '100000

                        'Calculate deflection angle in both ZX and ZY planes.
                        'θ = (4GM/c2)(1/r)
                        thetaXY = dividend / (C * C * Math.Round(distanceLength - obj.VisualSize, 1) * DistanceMultiplier)

                        'Find angles on ZX and ZY planes.
                        If Math.Round(distX, 1) Then
                            thetaZX = dividend / (C * C * Math.Abs(distanceLength - obj.VisualSize +
                                                                   lensingRadius - Math.Abs(distY)) * DistanceMultiplier)
                            If Math.Round(distY, 1) Then
                                thetaZX *= 500 * 1 / distanceLength
                            End If
                        End If
                        If Math.Round(distY, 1) Then
                            thetaZY = dividend / (C * C * Math.Abs(distanceLength - obj.VisualSize +
                                                                   lensingRadius - Math.Abs(distX)) * DistanceMultiplier)
                            If Math.Round(distX, 1) Then
                                thetaZY *= 500 * 1 / distanceLength
                            End If
                        End If

                        'Find distance of deflection point from original one using trigonometry.
                        Dim deflectionDist As Double = Math.Tan(thetaXY) * Math.Abs(distZ) 'y = Tan(θ) * z

                        'Now find the distance from the focal point using the Pythagorean theorem.
                        Dim focalDist As Decimal = Math.Sqrt(deflectionDist * deflectionDist + distZ * distZ)

                        'Find deflection point coordinates by using the vectors for each plane.
                        Dim sourceVector As New Vector3D(0, 0, Math.Abs(distZ))
                        Dim deflectedVectorZY As New Vector
                        Dim deflectedVectorZX As New Vector

                        'We know that the X coordinate of each plane corresponds to the 3D source Z coordinate.
                        'We need only calculate the possible Y values based on equation for finding the angle between two (2) vectors.
                        deflectedVectorZX.X = sourceVector.Z
                        deflectedVectorZY.X = sourceVector.Z

                        Dim cosZX As Double = Math.Cos(thetaZX)
                        Dim cosZY As Double = Math.Cos(thetaZY)

                        'Calculate deflection point coordinates.
                        'In the ZX plane the Y coordinate corresponds to the 3D source X coordinate.
                        deflectedVectorZX.Y = Math.Sign(distX) * Math.Tan(thetaZX) * deflectedVectorZX.X / DistanceMultiplier

                        deflectedVectorZY.Y = Math.Sign(distY) * Math.Tan(thetaZY) * deflectedVectorZX.X / DistanceMultiplier

                        universeGraphics.DrawString(thetaZX.ToString, obj.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 50)
                        universeGraphics.DrawString(thetaZY.ToString, obj.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 60)
                        universeGraphics.DrawLine(New Pen(Color.Green),
                                                 closestPoint,
                                                 New PointF(closestPoint.X + deflectedVectorZX.Y, closestPoint.Y + deflectedVectorZY.Y))
                    End If

                    Dim arcLength As Double = thetaZX * obj.VisualSize

                    'Dim orthgonalSideLen As Double = Math.Sqrt(obj.VisualSize * obj.VisualSize - (chordLength / 2) * (chordLength / 2))
                    If Not intersectPoints.Contains(New PointF(0, 0)) Then
                        universeGraphics.DrawLine(New Pen(Color.White),
                                            New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y),
                                            intersectPoints(0))
                        universeGraphics.DrawLine(New Pen(Color.White),
                                              New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y),
                                              intersectPoints(1))
                        universeGraphics.DrawLine(New Pen(Color.White),
                                              New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y),
                                              middlePoint)
                    End If

                End If

            Next

        Next

    End Sub

    'This functions calculates the length of the common chord between two (2) intersecting circles/ellipses.
    'It takes as parameters the radii of the circles and their distance between their centers.

    Private Function CommonChordLength(ByVal r1 As Double, ByVal r2 As Double, ByVal dist As Double) As Double

        '     sqrt[(d^2 - (r1^2 - r2^2)) * ((r1^2 - r2^2) - d^2)]
        ' x = ---------------------------------------------------
        '                            d

        Dim distSQR = dist * dist
        Dim radiusDiffSQR As Double = (r1 - r2) * (r1 - r2)
        Dim radiusSumSQR As Double = (r1 + r2) * (r1 + r2)

        Return Math.Sqrt(Math.Abs((distSQR - radiusDiffSQR) * (radiusSumSQR - distSQR))) / dist

    End Function

    'This function returns the middle point of the chord between the two (2) intersecting points of two (2) circles.
    'The parameters passed are the distance between the centers of the cirles and their radii.

    Private Function GetMiddleChordPoint(ByVal dist As Double, ByVal r1 As Double, ByVal r2 As Double, ByVal c1 As PointF, ByVal c2 As PointF) As PointF

        Dim middlePoint As PointF
        Dim intersectPoints As PointF() = CircleIntersectionPoints(dist, r1, r2, c1, c2)

        middlePoint.X = (intersectPoints(0).X + intersectPoints(1).X) / 2
        middlePoint.Y = (intersectPoints(0).Y + intersectPoints(1).Y) / 2

        Return middlePoint

    End Function

    'This function returns the two (2) intersecting points of two (2) circles.
    'The parameters passed are the distance between the centers of the cirles and their radii.
    'The equations used are based on this site: http://ambrnet.com/TrigoCalc/Circles2/circle2intersection/CircleCircleIntersection.htm

    Private Function CircleIntersectionPoints(ByVal dist As Double, ByVal r1 As Double, ByVal r2 As Double,
                                              ByVal c1 As PointF, ByVal c2 As PointF) As PointF()

        Dim intersectP1 As PointF
        Dim intersectP2 As PointF
        Dim half_perim As Double = (r1 + r2 + dist) / 2
        Dim area As Double = Math.Sqrt(half_perim * (half_perim - r1) * (half_perim - r2) * (half_perim - dist)) 'Heron's formula.

        If area >= 0 Then

            'Perfome the calculations only once.
            Dim part1 As Double = (c1.X + c2.X) / 2 + (c2.X - c1.X) * (r1 * r1 - r2 * r2) / (2 * dist * dist)
            Dim part2 As Double = area * 2 * (c1.Y - c2.Y) / (dist * dist)

            intersectP1.X = part1 + part2
            intersectP2.X = part1 - part2

            'Now for the Y coordinate.
            part1 = (c1.Y + c2.Y) / 2 + (c2.Y - c1.Y) * (r1 * r1 - r2 * r2) / (2 * dist * dist)
            part2 = area * 2 * (c1.X - c2.X) / (dist * dist)

            intersectP1.Y = part1 - part2
            intersectP2.Y = part1 + part2

        End If

        Return {intersectP1, intersectP2}

    End Function
    'This function returns the slope (m) and the constant (b) of a line using two (2) points.
    'The parameters passed are the two (2) points.

    Private Function LineEquationFromTwoPoints(ByVal p1 As PointF, ByVal p2 As PointF) As Double()

        'y = mx + b

        '   b = y1 - mx1
        ' - b = y2 - mx2
        '=> a = (y2 - y1) / (x2 - x1)

        Dim m As Double = (p2.Y - p1.Y) / (p2.X - p1.X)
        Dim b As Double = p1.Y - m * p1.X

        Return {m, b}

    End Function

    'This function returns the intersection points (if any) of a line with a circle.
    'The parameters passed are the line slope, constant, center of the circle and it's radius.

    Private Function LineIntersectionPointsWithCircle(m, b, center, r) As PointF()

        'Solve quadratic formula after calculating the constants.
        'qx^2 + wx + e = 0
        Dim q As Double = m * m + 1
        Dim w As Double = -2 * center.x + 2 * m * (b - center.y)
        Dim e As Double = center.x * center.x + (b - center.y) * (b - center.y) - r * r

        Dim D As Double = w * w - 4 * q * e

        'If there are possible solutions.
        If D >= 0 Then

            Dim x1 As Single = (-w + Math.Sqrt(D)) / (2 * q)
            Dim x2 As Single = (-w - Math.Sqrt(D)) / (2 * q)

            'Plug x in the line equation to find y.
            Dim y1 As Single = m * x1 + b
            Dim y2 As Single = m * x2 + b

            Return {New PointF(x1, y1), New PointF(x2, y2)}

        Else
            Console.WriteLine("LineIntersectionPointsWithCircle: No solutions for quadratic equation.")
            Return {New PointF(0, 0)}
        End If

    End Function

End Class

