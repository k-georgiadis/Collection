'MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.Drawing.Drawing2D

Public Class StellarObject

    Private objectUniverse As Universe
    Private objectUniverseMatrix As Drawing2D.Matrix
    Private objectInverseUniverseMatrix As Drawing2D.Matrix
    Private objectUniverseOffsetX As Double
    Private objectUniverseOffsetY As Double

    Private objectType As StellarObjectType
    Private objectSize As Integer
    Private objectRadius As Integer
    Private objectVisualSize As Double 'Used for Z dimension.
    Private objectMass As Double
    Private objectColor As Color
    Private objectGlow As Color
    Private objectBorderWidth As Integer

    Private objectCenterOfMass As PointFD
    Private objectCosmosSection As Byte

    Private objectVelX As Double
    Private objectVelY As Double
    Private objectVelZ As Double

    Private objectAccX As Double
    Private objectAccY As Double
    Private objectAccZ As Double

    'Properties for tunneling effect.
    Private objectTransitionDirection As String
    Private objectDuplicatePointRight As PointF
    Private objectDuplicatePointLeft As PointF
    Private objectDuplicatePointTop As PointF
    Private objectDuplicatePointBottom As PointF

    'Misc. properties.
    Private objectListItem As ListViewItem 'Dummy item to store label properties (forecolor, font etc.)

    Private objectMerging As Boolean 'Is the object currently merging?
    Private objectMerged As Boolean
    Private objectSelected As Boolean 'Is object selected?

    'Trajectory system.
    Private objectTrajectoryPoints As New List(Of PointFD)
    Private objectMaxTrajectoryPoints As Integer
    Private objectTrajRelativeDist As New List(Of PointF)

    'Lensing system.
    Private lensPoints As New List(Of PointF)
    Private lensorObjects As New List(Of Singularity)
    Private lensed As Boolean

    Enum StellarObjectType As Byte
        Planet
        Star
        BlackHole
    End Enum

    Public Property Universe() As Universe
        Get
            Return objectUniverse
        End Get
        Set(ByVal Universe As Universe)
            objectUniverse = Universe
        End Set
    End Property
    Public Property UniverseMatrix() As Drawing2D.Matrix
        Get
            Return objectUniverseMatrix
        End Get
        Set(ByVal Matrix As Drawing2D.Matrix)
            objectUniverseMatrix = Matrix
        End Set
    End Property
    Public Property InverseUniverseMatrix As Drawing2D.Matrix
        Get
            Return objectInverseUniverseMatrix
        End Get
        Set(ByVal Matrix As Drawing2D.Matrix)
            objectInverseUniverseMatrix = Matrix
        End Set
    End Property
    Public Property UniverseOffsetX() As Double
        Get
            Return objectUniverseOffsetX
        End Get
        Set(ByVal Offset As Double)
            objectUniverseOffsetX = Offset
        End Set
    End Property
    Public Property UniverseOffsetY() As Double
        Get
            Return objectUniverseOffsetY
        End Get
        Set(ByVal Offset As Double)
            objectUniverseOffsetY = Offset
        End Set
    End Property

    Public Property Type As StellarObjectType
        Get
            Return objectType
        End Get
        Set(value As StellarObjectType)
            objectType = value
        End Set
    End Property
    Public Property Size() As Integer
        Get
            Return objectSize
        End Get
        Set(ByVal Value As Integer)
            objectSize = Value
        End Set
    End Property

    ''' <summary>
    ''' The actual radius of the stellar object.
    ''' This is not the radius that is used to draw the stellar object.
    ''' </summary>
    ''' <returns></returns>
    Public Property Radius() As Integer
        Get
            Return objectRadius
        End Get
        Set(ByVal Value As Integer)
            objectRadius = Value
        End Set
    End Property

    ''' <summary>
    ''' Radius used to draw the stellar object to simulate depth (Z dimension).
    ''' </summary>
    ''' <returns></returns>
    Public Property VisualSize As Double
        Get
            Return objectVisualSize
        End Get
        Set(value As Double)
            objectVisualSize = value
        End Set
    End Property
    Public Property Mass() As Double
        Get
            Return objectMass
        End Get
        Set(ByVal Value As Double)
            objectMass = Value
        End Set
    End Property
    Public Property PaintColor() As Color
        Get
            Return objectColor
        End Get
        Set(ByVal Color As Color)
            objectColor = Color
        End Set
    End Property
    Public Property GlowColor As Color
        Get
            Return objectGlow
        End Get
        Set(value As Color)
            objectGlow = value
        End Set
    End Property

    Public Property BorderWidth() As Integer
        Get
            Return objectBorderWidth
        End Get
        Set(ByVal Value As Integer)
            objectBorderWidth = Value
        End Set
    End Property

    Public Property CenterOfMass() As PointFD
        Get
            Return objectCenterOfMass
        End Get
        Set(ByVal Point As PointFD)
            objectCenterOfMass = Point
        End Set
    End Property
    Public Property CosmosSection As Byte
        Get
            Return objectCosmosSection
        End Get
        Set(value As Byte)
            objectCosmosSection = value
        End Set
    End Property

    Public Property VelX() As Double
        Get
            Return objectVelX
        End Get
        Set(ByVal Value As Double)
            objectVelX = Value
        End Set
    End Property
    Public Property VelY() As Double
        Get
            Return objectVelY
        End Get
        Set(ByVal Value As Double)
            objectVelY = Value
        End Set
    End Property
    Public Property VelZ() As Double
        Get
            Return objectVelZ
        End Get
        Set(ByVal Value As Double)
            objectVelZ = Value
        End Set
    End Property

    Public Property AccX As Double
        Get
            Return objectAccX
        End Get
        Set(value As Double)
            objectAccX = value
        End Set
    End Property
    Public Property AccY As Double
        Get
            Return objectAccY
        End Get
        Set(value As Double)
            objectAccY = value
        End Set
    End Property
    Public Property AccZ As Double
        Get
            Return objectAccZ
        End Get
        Set(value As Double)
            objectAccZ = value
        End Set
    End Property
    Public Property TransitionDirection() As String
        Get
            Return objectTransitionDirection
        End Get
        Set(ByVal Direction As String)
            objectTransitionDirection = Direction
        End Set
    End Property
    Public Property DuplicatePointRight() As PointF
        Get
            Return objectDuplicatePointRight
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointRight = Point
        End Set
    End Property
    Public Property DuplicatePointLeft() As PointF
        Get
            Return objectDuplicatePointLeft
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointLeft = Point
        End Set
    End Property
    Public Property DuplicatePointTop() As PointF
        Get
            Return objectDuplicatePointTop
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointTop = Point
        End Set
    End Property
    Public Property DuplicatePointBottom() As PointF
        Get
            Return objectDuplicatePointBottom
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointBottom = Point
        End Set
    End Property

    Public Property ListItem() As ListViewItem
        Get
            Return objectListItem
        End Get
        Set(ByVal Item As ListViewItem)
            objectListItem = Item
        End Set
    End Property

    Public Property IsMerging() As Boolean
        Get
            Return objectMerging
        End Get
        Set(ByVal Status As Boolean)
            objectMerging = Status
        End Set
    End Property
    Public Property IsMerged() As Boolean
        Get
            Return objectMerged
        End Get
        Set(ByVal Status As Boolean)
            objectMerged = Status
        End Set
    End Property
    Public Property IsSelected() As Boolean
        Get
            Return objectSelected
        End Get
        Set(ByVal Status As Boolean)
            objectSelected = Status
        End Set
    End Property

    Public Property TrajectoryPoints() As List(Of PointFD)
        Get
            Return objectTrajectoryPoints
        End Get
        Set(value As List(Of PointFD))
            objectTrajectoryPoints = value
        End Set
    End Property
    Public Property MaxTrajectoryPoints As Integer
        Get
            Return objectMaxTrajectoryPoints
        End Get
        Set(value As Integer)
            objectMaxTrajectoryPoints = value
        End Set
    End Property

    Public Property lensingPoints As List(Of PointF)
        Get
            Return lensPoints
        End Get
        Set(value As List(Of PointF))
            lensPoints = value
        End Set
    End Property
    Public Property IsLensed As Boolean
        Get
            Return lensed
        End Get
        Set(value As Boolean)
            lensed = value
        End Set
    End Property

    Public Property lensorSingularityList As List(Of Singularity)
        Get
            Return lensorObjects
        End Get
        Set(value As List(Of Singularity))
            lensorObjects = value
        End Set
    End Property

    'Misc properties.

    Public Overridable ReadOnly Property isVisibleX() As Boolean
        Get
            Return objectCenterOfMass.X >= objectUniverseOffsetX - VisualSize - BorderWidth / 2 And
                   objectCenterOfMass.X <= objectUniverse.getWidth + VisualSize + BorderWidth / 2 'If inside the X visible area at all.
        End Get
    End Property
    Public Overridable ReadOnly Property isVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY - VisualSize - BorderWidth / 2 And
                   objectCenterOfMass.Y <= objectUniverse.getHeight + VisualSize + BorderWidth / 2 'If inside the Y visible area at all.
        End Get
    End Property
    Public Overridable ReadOnly Property isVisible() As Boolean
        Get
            Return isVisibleX() And isVisibleY() 'If inside the X,Y visible area.
        End Get
    End Property
    Public ReadOnly Property isPartialVisibleX() As Boolean
        Get
            Return objectCenterOfMass.X >= objectUniverseOffsetX And
                   objectCenterOfMass.X <= objectUniverse.getWidth 'If its half or more is inside X visible area.
        End Get
    End Property
    Public ReadOnly Property isPartialVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY And
                   objectCenterOfMass.Y <= objectUniverse.getHeight 'If it's half or more is inside Y visible area.
        End Get
    End Property
    Public ReadOnly Property isPartialVisible() As Boolean
        Get
            Return isPartialVisibleX() And isPartialVisibleY() 'If it's half or more inside the X,Y visible area.
        End Get
    End Property
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


    'Misc subs/funcs.
    Friend Sub AddVelX(ByVal AccX As Double)
        objectVelX += AccX
    End Sub
    Friend Sub AddVelY(ByVal AccY As Double)
        objectVelY += AccY
    End Sub
    Friend Sub AddVelZ(ByVal AccZ As Double)
        objectVelZ += AccZ
    End Sub

    Public Function ComparePointsByAtan2(ByVal p1 As PointF, ByVal p2 As PointF) As Integer

        'Converting Cartesian to polar coordinates.
        Dim theta1 As Double = Math.Atan2(p1.Y - CenterOfMass.Y, p1.X - CenterOfMass.X)
        Dim theta2 As Double = Math.Atan2(p2.Y - CenterOfMass.Y, p2.X - CenterOfMass.X)

        If p1.Equals(Nothing) Then
            If p2.Equals(Nothing) Then
                ' If p1 is Nothing and p2 is Nothing, they're
                ' equal. 
                Return 0
            Else
                ' If p1 is Nothing and p2 is not Nothing, p2
                ' is greater. 
                Return -1
            End If
        Else
            ' If p1 is not Nothing...
            '
            If p2.Equals(Nothing) Then
                ' ...and p2 is Nothing, p1 is greater.
                Return -1
            Else
                ' ...and p2 is not Nothing, compare the point coordinates.

                If theta1 = theta2 Then

                    Return 0 'Both points are equal.

                ElseIf theta1 < theta2 Then

                    ' p2 is closer to the top-left corner, so p2 wins.
                    Return -1

                Else

                    Return 1

                End If

            End If
        End If

    End Function

    Public Function CompareTest(ByVal p1 As Integer, ByVal p2 As Integer) As Integer


        If p1.Equals(Nothing) Then
            If p2.Equals(Nothing) Then
                ' If p1 is Nothing and p2 is Nothing, they're
                ' equal. 
                Return 0
            Else
                ' If p1 is Nothing and p2 is not Nothing, p2
                ' is greater. 
                Return -1
            End If
        Else
            ' If p1 is not Nothing...
            '
            If p2.Equals(Nothing) Then
                ' ...and p2 is Nothing, p1 is greater.
                Return -1
            Else
                ' ...and p2 is not Nothing, compare the point coordinates.

                If p1 = p2 Then

                    Return 0 'Both points are equal.

                ElseIf p1 < p2 Then

                    ' p2 is closer to the top-left corner, so p2 wins.
                    Return -1

                Else

                    Return 1

                End If

            End If
        End If

    End Function


    Friend Overridable Sub applyAcceleration(ByVal objList As List(Of StellarObject), ByVal gravityConstant As Double)

        Dim delta As Double = 1 'Catalyst.
        Dim distanceMultiplier As Long = objectUniverse.DistanceMultiplier()

        'GOTTA GO FAST.
        If Not objectUniverse.isRealistic Then

            delta = 10000000.0

        End If

        'Skip previous objects since we already calculated their forces (on this object too).
        Dim index As Integer = objList.IndexOf(Me)

        For Each obj In objList.ToArray.Skip(index + 1)

            If obj.IsMerged Then Continue For

            'Distance vector of two bodies.
            Dim distanceVector As New PointFD(obj.CenterOfMass.X - objectCenterOfMass.X,
                                              obj.CenterOfMass.Y - objectCenterOfMass.Y,
                                              obj.CenterOfMass.Z - objectCenterOfMass.Z)

            Dim distanceLength As Double = Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y * distanceVector.Y +
                                                     distanceVector.Z * distanceVector.Z)  'Get distance(magnitude).

            'Surface of objects collide.
            If distanceLength <= VisualSize + obj.VisualSize Then

                ' delta = 1 / 1000 'Slow down.

                'Merge objects.
                If distanceLength <= VisualSize Or distanceLength <= obj.VisualSize Then

                    obj.IsMerging = True
                    IsMerging = True

                    Dim newMass As Double
                    Dim newObjectRadius As Integer

                    newMass = objectMass + obj.Mass

                    'Calculate new velocity.
                    Dim newVelX As Double = (objectVelX * objectMass + obj.VelX * obj.Mass) / newMass
                    Dim newVelY As Double = (objectVelY * objectMass + obj.VelY * obj.Mass) / newMass
                    Dim newVelZ As Double = (objectVelZ * objectMass + obj.VelZ * obj.Mass) / newMass

                    'New color is the sum of their halves.
                    Dim newObjectcolor As Color = Color.FromArgb(objectColor.R / 2 + obj.PaintColor.R / 2,
                                                                 objectColor.G / 2 + obj.PaintColor.G / 2,
                                                                 objectColor.B / 2 + obj.PaintColor.B / 2)
                    'I'm the eater.
                    If objectMass >= obj.Mass Then

                        Dim selected As Boolean = objectSelected 'We need to re-select the eater if it was selected before the merging.
                        obj.IsMerged = True

                        'Initialize new merged object.
                        If Not Type = StellarObjectType.Planet Then

                            newObjectRadius = objectRadius + obj.Radius / 4

                            CType(Me, Star).Init(objectUniverse, objectUniverseMatrix, CenterOfMass, newObjectRadius, 1,
                                                 objectBorderWidth, newObjectcolor, Nothing, New PointFD(newVelX, newVelY, newVelZ))

                        Else
                            'This will no longer be needed when the planets are ellipses and not rectangles.
                            Dim newPlanetSize As Integer = objectSize + obj.Size / 4

                            'Check if the mass is big enough to ignite a star.
                            If obj.Type = StellarObjectType.Planet And newMass >= Universe.MinFusionMass Then
                                CType(Me, Star).Init(objectUniverse, objectUniverseMatrix, CenterOfMass, newPlanetSize, 1,
                                                     objectBorderWidth, newObjectcolor, Nothing, New PointFD(newVelX, newVelY, newVelZ))
                            Else
                                CType(Me, Planet).Init(objectUniverse, objectUniverseMatrix, CenterOfMass, newPlanetSize, 1,
                                                       objectBorderWidth, newObjectcolor, Nothing, New PointFD(newVelX, newVelY, newVelZ))
                            End If

                        End If

                        'Select merged object, if either of them was selected.
                        If obj.IsSelected Or selected Then
                            objectSelected = True
                        End If

                        objectMass = newMass

                    Else 'I'm being eaten.

                        Dim selected As Boolean = obj.IsSelected 'We need to re-select the eaten object if it was selected before the merging.
                        IsMerged = True

                        'Initialize new merged object.
                        If Not Type = StellarObjectType.Planet Then

                            newObjectRadius = obj.Radius + objectRadius / 4

                            CType(obj, Star).Init(objectUniverse, objectUniverseMatrix, obj.CenterOfMass, newObjectRadius, 1,
                                                  objectBorderWidth, newObjectcolor, Nothing, New PointFD(newVelX, newVelY, newVelZ))

                        Else
                            'This will no longer be needed when the planets are real planets and not rectangles.
                            Dim newPlanetSize As Integer = obj.Size + objectSize / 4

                            'Check if the mass is big enough to ignite a star.
                            If obj.Type = StellarObjectType.Planet And newMass >= Universe.MinFusionMass Then
                                CType(obj, Star).Init(objectUniverse, objectUniverseMatrix, obj.CenterOfMass, newPlanetSize, 1,
                                                      objectBorderWidth, newObjectcolor, Nothing, New PointFD(newVelX, newVelY, newVelZ))
                            Else
                                CType(obj, Planet).Init(objectUniverse, objectUniverseMatrix, obj.CenterOfMass, newPlanetSize, 1,
                                                        objectBorderWidth, newObjectcolor, Nothing, New PointFD(newVelX, newVelY, newVelZ))
                            End If
                        End If

                        'Select merged object, if either of them was selected.
                        If objectSelected Or selected Then
                            obj.IsSelected = True
                        End If

                        obj.Mass = newMass

                    End If

                    obj.IsMerging = False
                    IsMerging = False

                    Continue For

                End If
            End If

            'Simulate "normal" distances of stellar objects in the universe.
            distanceVector.X *= distanceMultiplier
            distanceVector.Y *= distanceMultiplier
            distanceVector.Z *= distanceMultiplier

            distanceLength = Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y * distanceVector.Y +
                                       distanceVector.Z * distanceVector.Z)  'Get new distance(magnitude).

            'Calculate gravity force. Check below for a guide to the physics.
            'http://www.cs.princeton.edu/courses/archive/fall03/cs126/assignments/nbody.html

            'force = gravityConstant * (star.GetMass * planetMass) / Math.Pow(distanceLength, 2)
            'F = G * (M * m) / d ^ 2
            'Fx = F * cos(f) = F * dx / d => .... => Ax = G * M * dx / d^3
            'Fy = F * sin(f) = F * dy / d => .... => Ay = G * M * dy / d^3

            Dim inv_d As Double = 1.0 / (distanceLength * distanceLength * distanceLength)

            'Precalculate force component (1/r^2) * direction (dx/r) = dx / r^3
            distanceVector.X *= inv_d 'dx / d ^ 3
            distanceVector.Y *= inv_d 'dy / d ^ 3
            distanceVector.Z *= inv_d 'dy / d ^ 3

            'Calculate accelerations for both bodies and apply them to the velocities.
            Dim currentAccX As Double = gravityConstant * obj.Mass() * distanceVector.X * delta
            Dim currentAccY As Double = gravityConstant * obj.Mass() * distanceVector.Y * delta
            Dim currentAccZ As Double = gravityConstant * obj.Mass() * distanceVector.Z * delta

            'If Universe.isRealistic Then
            '    currentAccX *= gravityConstant
            '    currentAccY *= gravityConstant
            'End If

            objectAccX += currentAccX
            objectAccY += currentAccY
            objectAccZ += currentAccZ

            objectVelX += currentAccX
            objectVelY += currentAccY
            objectVelZ += currentAccZ

            'When we calculate this object's (Me) applied accelaration, we can apply the opposite(-) accelaration to the other object.
            'That's why we skip the previous objects, because we already calculated their forces on this object.

            currentAccX = gravityConstant * objectMass * (-distanceVector.X) * delta
            currentAccY = gravityConstant * objectMass * (-distanceVector.Y) * delta
            currentAccZ = gravityConstant * objectMass * (-distanceVector.Z) * delta

            'If Universe.isRealistic Then
            '    currentAccX *= gravityConstant
            '    currentAccY *= gravityConstant
            'End If

            obj.AccX += currentAccX
            obj.AccY += currentAccY
            obj.AccZ += currentAccZ

            obj.AddVelX(currentAccX)
            obj.AddVelY(currentAccY)
            obj.AddVelZ(currentAccZ)

        Next

    End Sub
    Friend Overridable Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0, Optional ByVal dZ As Double = 0)

    End Sub
    Friend Overridable Sub CheckForBounce()

    End Sub

    Friend Sub AddTrajectoryPoint(ByVal point As PointFD)

        objectTrajectoryPoints.Add(point)

    End Sub
    Friend Sub ClearTrajectory()

        objectTrajectoryPoints.RemoveRange(0, objectTrajectoryPoints.Count)

    End Sub
    Friend Sub ClearRelativeDistances()

        objectTrajRelativeDist.RemoveRange(0, objectTrajRelativeDist.Count)

    End Sub

    Friend Overridable Sub Paint(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

    End Sub
    Friend Overridable Sub PaintSelectionShape(ByVal universeGraphics As Graphics, ByVal zoomValue As Double)

    End Sub

    Friend Sub DrawTrajectory(ByVal universeGraphics As Graphics, ByVal planetPen As Pen)

        Dim maxPoints As Integer = objectUniverse.MaxTrajectoryPoints 'Get maximum number of points to use for the trajectory.
        Dim addRelativeDist As Boolean = False

        'If new value is given, reset list.
        If maxPoints <> MaxTrajectoryPoints And objectTrajectoryPoints.Count > 0 Then

            ClearTrajectory()
            ClearRelativeDistances()
            MaxTrajectoryPoints = maxPoints

        End If

        'Draw trajectory if said so.
        If objectUniverse.drawTraj Then

            Try
                'We don't need the Z dimension for now.
                Dim centerPoint As New PointFD(objectCenterOfMass.X, objectCenterOfMass.Y)

                If objectUniverse.RelativeTraj And objectTrajectoryPoints.Count > 0 And objectTrajRelativeDist.Count = 0 Then
                    ClearTrajectory()
                ElseIf Not objectUniverse.RelativeTraj And objectTrajRelativeDist.Count > 0 Then
                    'ClearTrajectory()
                    ClearRelativeDistances()
                End If

                'If there are any trajectory points.
                If objectTrajectoryPoints.Count > 0 Then

                    Dim lastPoint As PointFD = objectTrajectoryPoints.Last

                    'Don't change trajectory when dragging.
                    If Not objectUniverse.isDragged Then

                        Dim newPoint As New PointFD(Math.Round(objectTrajectoryPoints.Last.X, 2),
                                                    Math.Round(objectTrajectoryPoints.Last.Y, 2))
                        centerPoint.X = Math.Round(centerPoint.X, 2)
                        centerPoint.Y = Math.Round(centerPoint.Y, 2)

                        'If center of mass is not the same, create new trajectory point.
                        If Not centerPoint.Equals(newPoint) Then

                            addRelativeDist = True

                            'Recycle points.
                            If objectTrajectoryPoints.Count = maxPoints Then

                                objectTrajectoryPoints.RemoveAt(0)

                                If objectUniverse.RelativeTraj And objectTrajRelativeDist.Count > 0 Then
                                    objectTrajRelativeDist.RemoveAt(0)
                                End If

                            End If

                            objectTrajectoryPoints.Add(centerPoint)

                            'Check for violent changes in trajectory, usually because of tunneling.
                            'In that case, reset all points and start from scratch.
                            If Math.Abs(objectCenterOfMass.X - lastPoint.X) > 100 Or Math.Abs(objectCenterOfMass.Y - lastPoint.Y) > 100 Then

                                ClearTrajectory()
                                ClearRelativeDistances()
                                objectTrajectoryPoints.Add(centerPoint) 'Add first point again.

                            End If
                        Else
                            addRelativeDist = False 'Add relative distances only when we add trajectory points.
                        End If

                    End If

                Else
                    addRelativeDist = True
                    objectTrajectoryPoints.Add(centerPoint) 'Add first point.
                End If

                'Convert PointFD to PointF.
                Dim singlePointList As New List(Of PointF)
                For Each point In objectTrajectoryPoints
                    singlePointList.Add(New PointF(point.X, point.Y))
                Next

                'Create path.
                Dim graphicsPath As New Drawing2D.GraphicsPath

                'Draw relative trajectory, if said so.
                If objectUniverse.RelativeTraj Or objectUniverse.drawBothTraj Then

                    If addRelativeDist Or Universe.isPaused Then

                        Dim distance As PointF

                        'Get distance from selected/followed object.
                        Dim selectedObject As StellarObject = objectUniverse.Objects.Find(Function(o) o.IsSelected)
                        If selectedObject Is Nothing Then
                            Exit Sub
                        End If
                        distance = New PointF(centerPoint.X - selectedObject.CenterOfMass.X, centerPoint.Y - selectedObject.CenterOfMass.Y)

                        objectTrajRelativeDist.Add(distance)

                        Dim i As Integer
                        Dim realTrajectory() As PointF = singlePointList.ToArray

                        'Change point to match original point.
                        'NOTE: For Each loops don't change the elements in the group as they are getting local copies of the objects.
                        For i = 0 To realTrajectory.Count - 1
                            realTrajectory(i) = New PointF(selectedObject.CenterOfMass.X + objectTrajRelativeDist(i).X,
                                                           selectedObject.CenterOfMass.Y + objectTrajRelativeDist(i).Y)
                        Next

                        graphicsPath.AddLines(realTrajectory.ToArray)
                    End If

                    'Draw original trajectory also, if both are set to be drawn.
                    If objectUniverse.drawBothTraj Then

                        Dim secGraphicsPath As New GraphicsPath
                        secGraphicsPath.AddLines(singlePointList.ToArray)

                        'Draw trajectory.
                        universeGraphics.DrawPath(planetPen, secGraphicsPath)

                    End If

                Else
                    'Add points to path.
                    graphicsPath.AddLines(singlePointList.ToArray)
                End If

                'Draw trajectory.
                universeGraphics.DrawPath(planetPen, graphicsPath)

            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

        End If

    End Sub

End Class
