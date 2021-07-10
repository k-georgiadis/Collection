'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Public Class StellarObject

    Protected objectUniverse As Universe
    Protected objectUniverseMatrix As Drawing2D.Matrix
    Protected objectUniverseOffsetX As Double
    Protected objectUniverseOffsetY As Double

    Protected objectSize As Integer
    Protected objectRadius As Integer
    Protected objectMass As Double
    Protected objectType As Integer
    Protected objectColor As Color
    Protected objectBorderWidth As Integer

    Protected objectCenterOfMass As PointF
    Protected objectOriginPoint As PointF 'Used as the original point without any offsets, for drawing the object.

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
    Protected objectLabel As Label 'Dummy label to store label properties (forecolor, font etc.)
    Protected objectLabelHidden As Boolean 'Is label hidden?

    Protected objectMerging As Boolean 'Is the object currently merging?
    Protected objectOutOfBounds As Boolean 'Is the object out of bounds due to zooming?
    Protected objectSelected As Boolean 'Is object selected?

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

    Public Property Mass() As Double
        Get
            Return objectMass
        End Get
        Set(ByVal Value As Double)
            objectMass = Value
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

    Public Property CenterOfMass() As PointF
        Get
            Return objectCenterOfMass
        End Get
        Set(ByVal Point As PointF)
            objectCenterOfMass = Point
        End Set
    End Property

    Public Property OriginPoint As PointF
        Get
            Return objectOriginPoint
        End Get
        Set(value As PointF)
            objectOriginPoint = value
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

    Public Property Label() As Label
        Get
            Return objectLabel
        End Get
        Set(ByVal Label As Label)
            objectLabel = Label
        End Set
    End Property

    Public Property IsLabelHidden() As Boolean
        Get
            Return objectLabelHidden
        End Get
        Set(ByVal Status As Boolean)
            objectLabelHidden = Status
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

    Public Property IsOutOfBounds() As Boolean
        Get
            Return objectOutOfBounds
        End Get
        Set(ByVal Status As Boolean)
            objectOutOfBounds = Status
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

    'Misc properties.
    Public ReadOnly Property isVisibleX() As Boolean
        Get
            Return objectCenterOfMass.X >= objectUniverseOffsetX - objectRadius And
                   objectCenterOfMass.X <= objectUniverse.getWidth + objectRadius 'If inside the X visible area at all.
        End Get
    End Property
    Public ReadOnly Property isVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY - objectRadius And
                   objectCenterOfMass.Y <= objectUniverse.getHeight + objectRadius 'If inside the Y visible area at all.
        End Get
    End Property
    Public ReadOnly Property isVisible() As Boolean
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

    Friend Overridable Sub Paint(ByVal universeGraphics As Graphics)

    End Sub

End Class
