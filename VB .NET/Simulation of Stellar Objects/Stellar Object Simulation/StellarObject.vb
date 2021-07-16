'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.Drawing.Drawing2D

Public Class StellarObject

    Protected objectUniverse As Universe
    Protected objectUniverseMatrix As Drawing2D.Matrix
    Protected objectInverseUniverseMatrix As Drawing2D.Matrix
    Protected objectUniverseOffsetX As Double
    Protected objectUniverseOffsetY As Double

    Protected objectSize As Integer
    Protected objectRadius As Integer
    Protected objectMass As Double
    Protected objectType As Integer
    Protected objectColor As Color
    Protected objectBorderWidth As Integer

    Protected objectCenterOfMass As PointFD

    Protected objectVelX As Double
    Protected objectVelY As Double

    Protected objectAccX As Double
    Protected objectAccY As Double

    Protected objectMerged As Boolean

    'Properties for tunneling effect.
    Protected objectTransitionDirection As String
    Protected objectDuplicatePointRight As PointF
    Protected objectDuplicatePointLeft As PointF
    Protected objectDuplicatePointTop As PointF
    Protected objectDuplicatePointBottom As PointF

    'Misc. properties.
    Protected objectListItem As ListViewItem 'Dummy item to store label properties (forecolor, font etc.)

    Protected objectMerging As Boolean 'Is the object currently merging?
    Protected objectSelected As Boolean 'Is object selected?

    Protected objectTrajectoryPoints As New List(Of PointF)
    Protected objectMaxTrajectoryPoints As Integer
    Protected objectTrajRelativeDist As New List(Of PointF)

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

    Public Property Size() As Integer
        Get
            Return objectSize
        End Get
        Set(ByVal Value As Integer)
            objectSize = Value
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
    Public Property Radius() As Integer
        Get
            Return objectRadius
        End Get
        Set(ByVal Value As Integer)
            objectRadius = Value
        End Set
    End Property
    Public Property Type() As Integer
        Get
            Return objectType
        End Get
        Set(ByVal Value As Integer)
            objectType = Value
        End Set
    End Property
    Public Property Color() As Color
        Get
            Return objectColor
        End Get
        Set(ByVal Color As Color)
            objectColor = Color
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

    Public Property IsMerged() As Boolean
        Get
            Return objectMerged
        End Get
        Set(ByVal Status As Boolean)
            objectMerged = Status
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
    Public Property IsSelected() As Boolean
        Get
            Return objectSelected
        End Get
        Set(ByVal Status As Boolean)
            objectSelected = Status
        End Set
    End Property

    Public Property TrajectoryPoints() As List(Of PointF)
        Get
            Return objectTrajectoryPoints
        End Get
        Set(value As List(Of PointF))
            objectTrajectoryPoints = value
        End Set
    End Property

    'Misc properties.
    Public Overridable ReadOnly Property isVisibleX() As Boolean
        Get
            Return objectCenterOfMass.X >= objectUniverseOffsetX - objectRadius And
                   objectCenterOfMass.X <= objectUniverse.getWidth + objectRadius 'If inside the X visible area at all.
        End Get
    End Property
    Public Overridable ReadOnly Property isVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY - objectRadius And
                   objectCenterOfMass.Y <= objectUniverse.getHeight + objectRadius 'If inside the Y visible area at all.
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
                   objectCenterOfMass.X <= objectUniverse.getWidth 'If it's half or more is inside X visible area.
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

    Public ReadOnly Property isStar() As Boolean
        Get
            Return [GetType]() = GetType(Star) 'Is the object a star?
        End Get
    End Property
    Public ReadOnly Property isPlanet() As Boolean
        Get
            Return [GetType]() = GetType(Planet) 'Is the object a planet?
        End Get
    End Property

    'Misc subs.
    Friend Sub AddVelX(ByVal AccX As Double)
        objectVelX += AccX
    End Sub
    Friend Sub AddVelY(ByVal AccY As Double)
        objectVelY += AccY
    End Sub

    Friend Overridable Sub applyAcceleration(ByVal objList As List(Of StellarObject), ByVal gravityConstant As Double)

        Dim delta As Double = 1 / 100 'Catalyst.
        Dim distanceMultiplier = objectUniverse.getDistanceMultiplier()

        'Reset accelerations.
        objectAccX = 0
        objectAccY = 0

        For Each obj In objList.FindAll(Function(o) Not o.Equals(Me))

            If obj.IsMerged Then Continue For

            'Don't let planets interact with one another.
            If obj.isPlanet And Me.isPlanet Then Continue For

            'Distance vector of two bodies.
            Dim distanceVector As New PointFD(objectCenterOfMass.X - obj.CenterOfMass.X,
                                             objectCenterOfMass.Y - obj.CenterOfMass.Y)
            Dim distanceLength As Double = Math.Sqrt(distanceVector.X ^ 2 + distanceVector.Y ^ 2)  'Get distance(magnitude).

            'Surface of objects collide. Don't merge planets with planets yet.
            If (obj.isStar Or Me.isStar) And distanceLength <= Radius + obj.Radius Then

                ' delta = 1 / 1000 'Slow down.

                'Merge objects.
                If distanceLength <= Radius Or distanceLength <= obj.Radius Then

                    obj.IsMerging = True
                    IsMerging = True

                    delta = 1

                    Dim newStarMass As Double = 0
                    Dim newStarRadius As Integer = 0

                    newStarMass = objectMass + obj.Mass

                    'Calculate new velocity.
                    Dim newVelX As Double = Math.Round((objectVelX * objectMass * delta + obj.VelX * obj.Mass * delta) / newStarMass, 10)
                    Dim newVelY As Double = Math.Round((objectVelY * objectMass * delta + obj.VelY * obj.Mass * delta) / newStarMass, 10)

                    'New type is the sum of the two object types.
                    Dim newobjectType = objectType + obj.Type

                    'New color is the sum of their halves.
                    Dim newcolor As Color = Color.FromArgb(objectColor.R / 2 + obj.Color.R / 2,
                                                                objectColor.G / 2 + obj.Color.G / 2,
                                                                objectColor.B / 2 + obj.Color.B / 2)

                    'I'm the eater.
                    If objectRadius >= obj.Radius Then

                        obj.IsMerged = True

                        newStarRadius = objectRadius + obj.Radius / 4 'New radius is the radius of the eater + a 4th the radius of the eaten object.

                        'Initialize new merged Star.
                        CType(Me, Star).Init(objectUniverse, objectUniverseMatrix, CenterOfMass, newStarRadius, objectBorderWidth, newcolor, newobjectType,
                             New PointFD(newVelX, newVelY))

                        'Selected merged object, if the smaller one was selected.
                        If obj.IsSelected Then
                            objectSelected = obj.IsSelected
                        End If

                        objectMass = newStarMass

                    Else 'I'm being eaten.
                        IsMerged = True

                        newStarRadius = obj.Radius + objectRadius / 4 'New radius is the radius of eater + a 4th the radius of the eaten object.

                        'Initialize new merged Star.
                        CType(obj, Star).Init(objectUniverse, objectUniverseMatrix, obj.CenterOfMass, newStarRadius, objectBorderWidth, newcolor, newobjectType,
                                  New PointFD(newVelX, newVelY))

                        'Selected merged object, if the smaller one was selected.
                        If objectSelected Then
                            obj.IsSelected = objectSelected
                        End If

                        obj.Mass = newStarMass

                    End If

                    obj.IsMerging = False
                    IsMerging = False

                    delta = 1 / 100 'Reset catalyst.
                    Continue For

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

            'We calculated the accelaration that is applied to this object (Me), from the other object (obj).
            'So when the other object starts calculating it's applied accelaration from this object (Me), it must produce the same results.

            'Due to Threads not running simultaneously, the other object will not produce the same result, because this object (Me)
            'has already moved a little, because it calculated it's applied accelaration, FIRST.

            'So when we calculate this object's (Me) applied accelaration, we can apply the opposite(-) accelaration to the other planet.

            currentAccX = objectMass * (-dx) * delta
            currentAccY = objectMass * (-dy) * delta

            Dim oldacc As Double = obj.AccX

            obj.AccX += currentAccX
            obj.AccY += currentAccY

            obj.AddVelX(currentAccX)
            obj.AddVelY(currentAccY)
        Next

        'Dim newpos As New PointFD(objectVelX, objectVelY)
        'Move body.
        'Move(0, "", objectCenterOfMass.X + newpos.X, objectCenterOfMass.Y + newpos.Y)

    End Sub
    Friend Overridable Sub Move(ByVal stepCount As Integer, ByVal Direction As String, Optional ByVal dX As Double = 0, Optional ByVal dY As Double = 0)

    End Sub
    Friend Overridable Sub CheckForBounce()

    End Sub

    Friend Sub AddTrajectoryPoint(ByVal point As PointF)

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

    Friend Sub DrawTrajectory(ByVal universeGraphics As Graphics, ByVal planetPen As Pen)

        Dim maxPoints As Integer = objectUniverse.MaxTrajectoryPoints 'Get maximum number of points to use for the trajectory.
        Dim addRelativeDist As Boolean = False

        'If new value is given, reset list.
        If maxPoints <> objectMaxTrajectoryPoints And objectTrajectoryPoints.Count > 0 Then

            ClearTrajectory()
            ClearRelativeDistances()
            objectMaxTrajectoryPoints = maxPoints

        End If

        'Draw trajectory if said so.
        If objectUniverse.drawTraj Then

            Try
                Dim singlePoint As New PointF(objectCenterOfMass.X, objectCenterOfMass.Y)

                If objectUniverse.RelativeTraj And objectTrajectoryPoints.Count > 0 And objectTrajRelativeDist.Count = 0 Then
                    ClearTrajectory()
                ElseIf Not objectUniverse.RelativeTraj And objectTrajRelativeDist.Count > 0 Then
                    ClearTrajectory()
                    ClearRelativeDistances()
                End If

                'If there are any trajectory points.
                If objectTrajectoryPoints.Count > 0 Then

                    Dim lastPoint As PointF = objectTrajectoryPoints.Last

                    'Don't change trajectory when dragging.
                    If Not objectUniverse.isDragged Then

                        'If center of mass is not the same, create new trajectory point.
                        If Not singlePoint.Equals(objectTrajectoryPoints.Last) Then

                            addRelativeDist = True

                            'Recycle points.
                            If objectTrajectoryPoints.Count = maxPoints Then

                                objectTrajectoryPoints.RemoveAt(0)

                                If objectUniverse.RelativeTraj And objectTrajRelativeDist.Count > 0 Then
                                    objectTrajRelativeDist.RemoveAt(0)
                                End If

                            End If

                            objectTrajectoryPoints.Add(singlePoint)

                            'Check for violent changes in trajectory, usually because of tunneling.
                            'In that case, reset all points and start from scratch.
                            If Math.Abs(objectCenterOfMass.X - lastPoint.X) > 100 Or Math.Abs(objectCenterOfMass.Y - lastPoint.Y) > 100 Then

                                ClearTrajectory()
                                ClearRelativeDistances()
                                objectTrajectoryPoints.Add(singlePoint) 'Add first point again.

                            End If
                        Else
                            addRelativeDist = False 'Add relative distances only when we add trajectory points.
                        End If

                    End If

                Else
                    addRelativeDist = True
                    objectTrajectoryPoints.Add(singlePoint) 'Add first point.
                End If

                'Create path.
                Dim graphicsPath As New Drawing2D.GraphicsPath

                'Draw relative trajectory, if said so.
                If objectUniverse.RelativeTraj Or objectUniverse.drawBothTraj Then

                    If addRelativeDist Then

                        Dim distance As PointF

                        'Get distance from selected/followed object.
                        Dim selectedObject As StellarObject = objectUniverse.Objects.Find(Function(o) o.IsSelected)
                        distance = New PointF(singlePoint.X - selectedObject.CenterOfMass.X, singlePoint.Y - selectedObject.CenterOfMass.Y)

                        objectTrajRelativeDist.Add(distance)

                        Dim i As Integer
                        Dim realTrajectory() As PointF = objectTrajectoryPoints.ToArray

                        'Change point to match original point.
                        'NOTE: For Each loops don't change the elements in the group as they are getting local copies of the objects.
                        For i = 0 To realTrajectory.Count - 1
                            realTrajectory(i) = New PointF(selectedObject.CenterOfMass.X + objectTrajRelativeDist(i).X, selectedObject.CenterOfMass.Y + objectTrajRelativeDist(i).Y)
                        Next

                        graphicsPath.AddLines(realTrajectory.ToArray)
                    End If

                    'Draw original trajectory also, if both are set to be drawn.
                    If objectUniverse.drawBothTraj Then

                        Dim secGraphicsPath As New GraphicsPath
                        secGraphicsPath.AddLines(objectTrajectoryPoints.ToArray)

                        'Draw trajectory.
                        universeGraphics.DrawPath(planetPen, secGraphicsPath)

                    End If

                Else
                    graphicsPath.AddLines(objectTrajectoryPoints.ToArray)
                End If

                'Draw trajectory.
                universeGraphics.DrawPath(planetPen, graphicsPath)

            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try

        End If

    End Sub


End Class
