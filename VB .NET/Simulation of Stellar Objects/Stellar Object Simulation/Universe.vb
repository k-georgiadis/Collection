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
	Private eyeSize As Double = 0.01 'Eye size of observer. Light bending more degrees than that gets lensed for the observer.

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
					'Dim arcLength As Double = theta * obj.VisualSize
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
				Dim distanceVector As New PointFD(bh.CenterOfMass.X - obj.CenterOfMass.X, bh.CenterOfMass.Y - obj.CenterOfMass.Y)
				Dim distanceLength As Double = Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y * distanceVector.Y)

				'The photon sphere is at a distance of 1.5 times the radius of the event horizon.
				'Closer, we fall into the center after a few orbits. Further, we spiral out into space.
				Dim photonSphereDistance As Double = 1.5 * bh.VisualSize

				'Draw photon sphere.
				Dim myPen As New Drawing.Pen(Color.Red, 1)
				universeGraphics.DrawEllipse(myPen, Convert.ToSingle(bh.CenterOfMass.X - photonSphereDistance), Convert.ToSingle(bh.CenterOfMass.Y - photonSphereDistance),
													Convert.ToSingle(photonSphereDistance * 2), Convert.ToSingle(photonSphereDistance * 2))
				universeGraphics.FillEllipse(Brushes.Black, Convert.ToSingle(bh.CenterOfMass.X - photonSphereDistance), Convert.ToSingle(bh.CenterOfMass.Y - photonSphereDistance),
													Convert.ToSingle(photonSphereDistance * 2), Convert.ToSingle(photonSphereDistance * 2))

				'The lensing effect starts and ends before reaching the photon sphere.
				'How much before depends on the deflection angle size. If it's bigger than our "eye" then the lensing starts.
				Dim dividend As Double = 4 * gravityConstant * (100000 * bh.Mass)
				Dim lensingRadius As Double = dividend / (C * C * eyeSize * DistanceMultiplier) 'r = (4GM/c^2) * (1/θ)

				'Check if the object passed the lensing radius.
				If distanceLength <= lensingRadius + obj.VisualSize Then

					obj.IsLensed = True

					'Draw lensing sphere.
					myPen.Color = Color.Yellow
					universeGraphics.DrawEllipse(myPen, Convert.ToSingle(bh.CenterOfMass.X - lensingRadius), Convert.ToSingle(bh.CenterOfMass.Y - lensingRadius),
														Convert.ToSingle(lensingRadius * 2), Convert.ToSingle(lensingRadius * 2))
					'universeGraphics.FillEllipse(Brushes.Black, Convert.ToSingle(bh.CenterOfMass.X - lensingRadius), Convert.ToSingle(bh.CenterOfMass.Y - lensingRadius),
					'											Convert.ToSingle(lensingRadius * 2), Convert.ToSingle(lensingRadius * 2))

					'Calculate angle between distance vectors.
					Dim v1 As New Vector(distanceVector.X, distanceVector.Y)
					Dim v2 As New Vector(distanceVector.X, 0)
					Dim vectorAngle = Vector.AngleBetween(v2, v1) * Math.PI / 180 'In radians.

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
					Dim lineIntersectPointsC2 As PointF() = LineIntersectionPointsWithCircle(lineEquationParams(0), lineEquationParams(1), c2, obj.VisualSize)
					Dim lineIntersectPointsC1 As PointF() = LineIntersectionPointsWithCircle(lineEquationParams(0), lineEquationParams(1), c1, lensingRadius)

					'If there is an intersection, show it.
					If lineIntersectPointsC2 IsNot Nothing Then

						'Find point on stellar object that is closest to the singularity.
						Dim closestPoint As PointF = lineIntersectPointsC2.ToList.Find(
															Function(p)
																Return Math.Round(Math.Sqrt((p.X - c1.X) * (p.X - c1.X) +
																				 (p.Y - c1.Y) * (p.Y - c1.Y))) <= Math.Round(distanceLength - obj.VisualSize)
															End Function
														)
						universeGraphics.DrawLine(New Pen(Color.Red), New PointF(obj.CenterOfMass.X, obj.CenterOfMass.Y), closestPoint)

						'Find point on stellar object that is farthest to the singularity.
						Dim farthestPoint As PointF = lineIntersectPointsC2.ToList.Find(Function(p) p.X <> closestPoint.X)

						'Calculate and get deflection point.
						GetDeflectionPoint(New Vector3D(closestPoint.X, closestPoint.Y, obj.CenterOfMass.Z), bh, distanceLength - obj.VisualSize)

						'Add lensor to the list, if not already added. Otherwise, reset points.
						If Not obj.lensorSingularityList.Contains(bh) Then
							obj.lensorSingularityList.Add(bh)
						Else
							obj.lensingPoints.RemoveRange(0, obj.lensingPoints.Count)
						End If

						'We bend the stellar object and then we catch the light that comes in the opposite side of the singularity.

						'Because of the drawing limitations in .NET we must get the two (2) points on the line that passes through the center and is
						'parallel to our intersection. Then we bent those points to simulater the "bending" of the stellar object.

						'We use trigonometry to calculate the parallel intersection points/vectors.
						Dim xOffset As Double = Math.Sin(vectorAngle) * obj.VisualSize 'A = sin(a) * R
						Dim yOffset As Double = Math.Cos(vectorAngle) * obj.VisualSize 'A = cos(a) * R
						Dim parallelIntersectionPoint1 As New PointF(obj.CenterOfMass.X + xOffset, obj.CenterOfMass.Y - yOffset)
						Dim parallelIntersectionPoint2 As New PointF(obj.CenterOfMass.X - xOffset, obj.CenterOfMass.Y + yOffset)
						Dim parallelVector1 As Vector3D
						Dim parallelVector2 As Vector3D

						If intersectPoints Is Nothing Then
							parallelVector1 = New Vector3D(parallelIntersectionPoint1.X, parallelIntersectionPoint1.Y, obj.CenterOfMass.Z)
							parallelVector2 = New Vector3D(parallelIntersectionPoint2.X, parallelIntersectionPoint2.Y, obj.CenterOfMass.Z)
						Else
							parallelVector1 = New Vector3D(intersectPoints(0).X, intersectPoints(0).Y, obj.CenterOfMass.Z)
							parallelVector2 = New Vector3D(intersectPoints(1).X, intersectPoints(1).Y, obj.CenterOfMass.Z)
						End If

						universeGraphics.DrawLine(New Pen(Color.LightBlue), parallelIntersectionPoint1, parallelIntersectionPoint2)

						'Calculate and get deflection points.
						Dim lensedParallelIntersectionPoint1 = GetDeflectionPoint(parallelVector1, bh, parallelVector1.DistanceFromPointF(c1))
						Dim lensedParallelIntersectionPoint2 = GetDeflectionPoint(parallelVector2, bh, parallelVector2.DistanceFromPointF(c1))

						'Get line equations from the singularity center and the intersection points.
						'Dim extendedLineEquationParams1 As Double() = LineEquationFromTwoPoints(c1, intersectPoints(0))
						'Dim extendedLineEquationParams2 As Double() = LineEquationFromTwoPoints(c1, intersectPoints(1))

						''Now get the farthest intersection points with the stellar object.
						'Dim extendedLineIntersectPoint1 As PointF = LineIntersectionPointsWithCircle(extendedLineEquationParams1(0), extendedLineEquationParams1(1), c2,
						'																			 obj.VisualSize).ToList.Find(
						'									Function(p)
						'										Return Math.Sqrt((p.X - c1.X) * (p.X - c1.X) +
						'														 (p.Y - c1.Y) * (p.Y - c1.Y)) - lensingRadius > 0.1
						'									End Function
						'								)
						'Dim extendedLineIntersectPoint2 As PointF = LineIntersectionPointsWithCircle(extendedLineEquationParams2(0), extendedLineEquationParams2(1), c2,
						'																			 obj.VisualSize).ToList.Find(
						'									Function(p)
						'										Return Math.Sqrt((p.X - c1.X) * (p.X - c1.X) +
						'														 (p.Y - c1.Y) * (p.Y - c1.Y)) - lensingRadius > 0.1
						'									End Function
						'								)
						'universeGraphics.DrawLine(New Pen(Color.LightBlue), extendedLineIntersectPoint1, extendedLineIntersectPoint2)

						Dim intersectingEllipseRadius As Double = obj.VisualSize + lensingRadius - distanceLength
						Dim radiusCircleWithBentPoints As Double = obj.VisualSize - intersectingEllipseRadius * 0.2
						Dim bentIntersectPoints As PointF() = LineIntersectionPointsWithCircle(lineEquationParams(0), lineEquationParams(1), c2,
																							   Math.Abs(radiusCircleWithBentPoints))
						Dim bentClosestPoint As PointF

						'Find point on lensing circle that is closest to the stellar object.
						'Dim closestPointOnSingularity As PointF = lineIntersectPointsC1.ToList.Find(
						'												Function(p)
						'													Return Math.Round(Math.Sqrt((p.X - c2.X) * (p.X - c2.X) +
						'																	 (p.Y - c2.Y) * (p.Y - c2.Y))) <= distanceLength
						'												End Function
						'											)
						If bentIntersectPoints IsNot Nothing Then

							'Find point on circle containing the bent points that is closest to the singularity.
							bentClosestPoint = bentIntersectPoints.ToList.Find(
															Function(p)
																Dim v As New Vector3D(p.X, p.Y, 0)
																Return v.DistanceFromPointF(c1) <= distanceLength
															End Function
														)
							'If we reached the center, continue bending behind it.
							If radiusCircleWithBentPoints < 0 Then
								bentClosestPoint = bentIntersectPoints.ToList.Find(Function(p) p.X <> bentClosestPoint.X)
							End If

						Else
							bentClosestPoint = c1
						End If

						'Find parallel points to the intersection ones on the oppposite side of the circle.
						'We do this by faking a singularity on the opposite side.
						'Then follow the same procedure to find the intersection points as we did for the original one.
						Dim fakeSingularityCenter As New PointF(obj.CenterOfMass.X - distanceVector.X, obj.CenterOfMass.Y - distanceVector.Y)
						Dim fakeIntersectPoints As PointF() = CircleIntersectionPoints(distanceLength, lensingRadius, obj.VisualSize, fakeSingularityCenter, c2)

						'Add points to create the bent closed curve.
						obj.lensingPoints.Add(parallelIntersectionPoint1)
						'obj.lensingPoints.Add(fakeIntersectPoints(0))
						'obj.lensingPoints.Add(parallelIntersectionPoint1)
						'obj.lensingPoints.Add(lensedParallelIntersectionPoint1)
						obj.lensingPoints.Add(bentClosestPoint)
						'obj.lensingPoints.Add(lensedParallelIntersectionPoint2)
						'obj.lensingPoints.Add(parallelIntersectionPoint2)
						'obj.lensingPoints.Add(fakeIntersectPoints(1))
						obj.lensingPoints.Add(parallelIntersectionPoint2)
						obj.lensingPoints.Add(farthestPoint)

						'universeGraphics.DrawLine(New Pen(Color.Cyan), bentClosestPoint, closestPoint)
						If intersectPoints IsNot Nothing Then
							universeGraphics.DrawLine(New Pen(Color.LightBlue), intersectPoints(0), intersectPoints(1))
						End If
						If fakeIntersectPoints IsNot Nothing Then
							universeGraphics.DrawLine(New Pen(Color.LightBlue), fakeIntersectPoints(0), fakeIntersectPoints(1))
						End If

						universeGraphics.DrawString(intersectingEllipseRadius.ToString, obj.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 70)
						universeGraphics.DrawString(radiusCircleWithBentPoints.ToString, obj.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 80)

					End If

					'Dim arcLength As Double = thetaZX * obj.VisualSize
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

				Else

					'If we were lensed before by this singularity but not anymore, remove lensing data from lists.
					If obj.lensorSingularityList.Contains(bh) Then

						obj.lensingPoints.RemoveAt(obj.lensorSingularityList.IndexOf(bh))
						obj.lensorSingularityList.Remove(bh)

						'If no lensors left, clear flag and reset lensing points list.
						If obj.lensorSingularityList.Count = 0 Then

							obj.IsLensed = False
							obj.lensingPoints.RemoveRange(0, obj.lensingPoints.Count)

						End If

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

		If intersectPoints Is Nothing Then
			Return Nothing
		End If

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

			Return {intersectP1, intersectP2}

		Else
			Return Nothing
		End If

	End Function

	'This function returns the slope (m) and the constant (b) of a line using two (2) points.
	'The parameters passed are the two (2) points.

	Private Function LineEquationFromTwoPoints(ByVal p1 As PointF, ByVal p2 As PointF) As Double()

		'y = m * x + b

		'   b = y1 - m * x1
		' - b = y2 - m * x2
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
			Return Nothing
		End If

	End Function

	'This function returns the intersection point (if any) between two (2) lines.
	'The parameters passed are the line slopes and constants for both lines.

	Private Function LineIntersectionPointsWithLine(m1, b1, m2, b2) As PointF

		'If lines intersect, then y1 = y2 = y and x1 = x2 = x
		'y = m1 * x + b1
		'y = m2 * x + b2

		'If lines intersect.
		If m1 - m2 > 0 Then

			Dim x As Double = (b2 - b1) / (m1 - m2)
			Dim y As Double = m1 * x + b1

			Return New PointF(x, y)

		Else
			Console.WriteLine("LineIntersectionPointsWithLine: Lines do not intersect.")
			Return New PointF(0, 0)
		End If

	End Function



	''' <summary>
	''' This function calculates the y coordinate of a point (x,y) that resides on a circle with center (0,0) and radius r.
	''' The parameters passed are the known x coordinate of the point, the center of the circle and it's radius.
	''' </summary>
	''' <param name="x"></param>
	''' <param name="r"></param>
	''' <returns>The Y coordinate.</returns>
	Private Function GetYCoordinateFromCircle(ByVal x As Double, ByVal r As Double) As Double

		'Circle equation.
		'( x - h )^2 + ( y - k )^2 = r^2

		'For center (0,0):
		'x^2 + y^2 = r^2

		'=>
		'y = sqrt(r^2 - x^2 )

		Return Math.Sqrt(r * r - x * x)

	End Function


	''' <summary>
	''' This function calculates the deflected vector of a source vector that is deflected due to the gravity of the stellar object lens.
	''' The parameters passed are the source vector, the lens stellar object and the distance between the source vector and lens center.
	''' </summary>
	''' <param name="source">The source point to calculate it's new deflected coordinates.</param>
	''' <param name="lens">The center point of the lens.</param>
	''' <returns>The deflected point.</returns>
	Private Function GetDeflectionPoint(ByVal source As Vector3D, ByVal lens As StellarObject, ByVal distanceLength As Double) As PointF

		'Find distance from the center of the singularity.
		Dim distX As Double = source.X - lens.CenterOfMass.X
		Dim distY As Double = source.Y - lens.CenterOfMass.Y
		Dim distZ As Double = source.Z - lens.CenterOfMass.Z

		Dim dividend As Double = 4 * gravityConstant * (100000 * lens.Mass)
		Dim lensingRadius As Double = dividend / (C * C * eyeSize * DistanceMultiplier) 'r = (4GM/c^2) * (1/θ)

		'Calculate deflection angles.
		Dim theta As Double = dividend / (C * C * distanceLength * DistanceMultiplier) 'θ = (4GM/c2) * (1/r)
		'Dim thetaZX As Double = dividend / (C * C * distX * DistanceMultiplier)
		'Dim thetaZY As Double = dividend / (C * C * distY * DistanceMultiplier)

		Dim phi As Double = (Math.Sqrt(5) + 1) / 2 'Φ
		theta *= 7 * lens.Mass / SolarMass

		'If the distance is very close, we set the angles at zero (0) to avoid extreme values.
		'If Math.Abs(distX) < 1 Then
		'	thetaZX = 0
		'End If
		'If Math.Abs(distY) < 1 Then
		'	thetaZY = 0
		'End If
		'The reason we see the extreme angles is because our "eye" is the size of the entire universe plane.
		'That means that no matter how much the light bends, we will be able to see it.
		'Normally, an observer in the real universe would never catch these extreme angles as we are very tiny and too far.
		'IDEA: Maybe set the size of our "eye" to a much smaller one so we avoid seeing extreme lensing?

		'Find distance of deflection point from source using trigonometry.
		Dim deflectedDistance As Double = Math.Abs(distZ / Math.Cos(theta))
		Dim deflectedDistanceXY As Double = Math.Abs(deflectedDistance * Math.Sin(theta))

		'Get line equation from center and source.
		Dim lineEquationParams As Double() = LineEquationFromTwoPoints(source.ToPointF, lens.CenterOfMass.ToPointF)

		'Now find the intersection points with circle with center the lens center and radius the total deflection distance from the lens center in the XY plane.
		Dim lineIntersectPointsLens As PointF() = LineIntersectionPointsWithCircle(lineEquationParams(0), lineEquationParams(1),
																				   lens.CenterOfMass.ToPointF, deflectedDistanceXY + distanceLength)

		'Find point that is closest to the source.
		Dim deflectionPoint As PointF = lineIntersectPointsLens.ToList.Find(
															Function(p)
																Return Math.Round(source.DistanceFromPointF(p)) <= Math.Round(deflectedDistanceXY + distanceLength)
															End Function
														)

		'Now check if the deflection angle is steep enough that causes the light to pass close enough to the lens (<= lensingRadius) so that it bends again.
		'We need the deflection line intersection points in the lensing circle in the ZX axis plane.
		Dim deflectedLineParams As Double() = LineEquationFromTwoPoints(New PointF(lens.CenterOfMass.Z, source.X), New PointF(source.Z, deflectionPoint.X))
		Dim lensRadiusLineZ As Double() = LineEquationFromTwoPoints(New PointF(lens.CenterOfMass.Z, lens.CenterOfMass.X), New PointF(lens.CenterOfMass.Z + lensingRadius, lens.CenterOfMass.X))
		Dim newDeflectionPoint As PointF = LineIntersectionPointsWithLine(deflectedLineParams(0), deflectedLineParams(1), lensRadiusLineZ(0), lensRadiusLineZ(1))

		If Not newDeflectionPoint.Equals(New PointF(0, 0)) AndAlso Math.Abs(newDeflectionPoint.X - lens.CenterOfMass.Z) <= lensingRadius Then

			'If Math.Abs(newDeflectionPoint.X - lens.CenterOfMass.Z) <= 1.5 * lens.VisualSize Then
			'	universeGraphics.DrawLine(New Pen(Color.Green), source.ToPointF, New PointF(lens.CenterOfMass.X, lens.CenterOfMass.Y))

			'End If
		End If

		universeGraphics.DrawString(theta.ToString, lens.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 50)
		universeGraphics.DrawString((theta * 180 / Math.PI).ToString, lens.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 60)
		'universeGraphics.DrawString(thetaZY.ToString, lens.ListItem.Font, Brushes.White, universeOffsetX + 10, universeOffsetY + 60)
		universeGraphics.DrawLine(New Pen(Color.Green), source.ToPointF, deflectionPoint)

		Return deflectionPoint

	End Function

End Class

