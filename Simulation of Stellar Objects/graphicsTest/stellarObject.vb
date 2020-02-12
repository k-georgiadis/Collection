Public Class StellarObject

    Protected objectUniverse As Universe
    Protected objectUniverseOffsetX As Double
    Protected objectUniverseOffsetY As Double

    Protected objectSize As Integer
    Protected objectRadius As Integer
    Protected objectMass As Double
    Protected objectType As Integer
    Protected objectColor As Color
    Protected objectBorderWidth As Integer

    Protected objectCenterOfMass As PointF

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

    Property Universe() As Universe
        Get
            Return objectUniverse
        End Get
        Set(ByVal Universe As Universe)
            objectUniverse = Universe
        End Set
    End Property
    Property UniverseOffsetX() As Double
        Get
            Return objectUniverseOffsetX
        End Get
        Set(ByVal Offset As Double)
            objectUniverseOffsetX = Offset
        End Set
    End Property
    Property UniverseOffsetY() As Double
        Get
            Return objectUniverseOffsetY
        End Get
        Set(ByVal Offset As Double)
            objectUniverseOffsetY = Offset
        End Set
    End Property

    Property Mass() As Double
        Get
            Return objectMass
        End Get
        Set(ByVal Value As Double)
            objectMass = Value
        End Set
    End Property
    Property Size() As Integer
        Get
            Return objectSize
        End Get
        Set(ByVal Value As Integer)
            objectSize = Value
        End Set
    End Property
    Property Radius() As Integer
        Get
            Return objectRadius
        End Get
        Set(ByVal Value As Integer)
            objectRadius = Value
        End Set
    End Property
    Property Type() As Integer
        Get
            Return objectType
        End Get
        Set(ByVal Value As Integer)
            objectType = Value
        End Set
    End Property
    Property Color() As Color
        Get
            Return objectColor
        End Get
        Set(ByVal Color As Color)
            objectColor = Color
        End Set
    End Property
    Property BorderWidth() As Integer
        Get
            Return objectBorderWidth
        End Get
        Set(ByVal Value As Integer)
            objectBorderWidth = Value
        End Set
    End Property

    Property CenterOfMass() As PointF
        Get
            Return objectCenterOfMass
        End Get
        Set(ByVal Point As PointF)
            objectCenterOfMass = Point
        End Set
    End Property

    Property VelX() As Double
        Get
            Return objectVelX
        End Get
        Set(ByVal Value As Double)
            objectVelX = Value
        End Set
    End Property
    Property VelY() As Double
        Get
            Return objectVelY
        End Get
        Set(ByVal Value As Double)
            objectVelY = Value
        End Set
    End Property

    Property AccX As Double
        Get
            Return objectAccX
        End Get
        Set(value As Double)
            objectAccX = value
        End Set
    End Property
    Property AccY As Double
        Get
            Return objectAccY
        End Get
        Set(value As Double)
            objectAccY = value
        End Set
    End Property

    Property IsMerged() As Boolean
        Get
            Return objectMerged
        End Get
        Set(ByVal Status As Boolean)
            objectMerged = Status
        End Set
    End Property

    Property TransitionDirection() As String
        Get
            Return objectTransitionDirection
        End Get
        Set(ByVal Direction As String)
            objectTransitionDirection = Direction
        End Set
    End Property
    Property DuplicatePointRight() As PointF
        Get
            Return objectDuplicatePointRight
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointRight = Point
        End Set
    End Property
    Property DuplicatePointLeft() As PointF
        Get
            Return objectDuplicatePointLeft
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointLeft = Point
        End Set
    End Property
    Property DuplicatePointTop() As PointF
        Get
            Return objectDuplicatePointTop
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointTop = Point
        End Set
    End Property
    Property DuplicatePointBottom() As PointF
        Get
            Return objectDuplicatePointBottom
        End Get
        Set(ByVal Point As PointF)
            objectDuplicatePointBottom = Point
        End Set
    End Property

    Property Label() As Label
        Get
            Return objectLabel
        End Get
        Set(ByVal Label As Label)
            objectLabel = Label
        End Set
    End Property
    Property IsLabelHidden() As Boolean
        Get
            Return objectLabelHidden
        End Get
        Set(ByVal Status As Boolean)
            objectLabelHidden = Status
        End Set
    End Property

    Property IsMerging() As Boolean
        Get
            Return objectMerging
        End Get
        Set(ByVal Status As Boolean)
            objectMerging = Status
        End Set
    End Property
    Property IsOutOfBounds() As Boolean
        Get
            Return objectOutOfBounds
        End Get
        Set(ByVal Status As Boolean)
            objectOutOfBounds = Status
        End Set
    End Property
    Property IsSelected() As Boolean
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
                   objectCenterOfMass.X <= objectUniverse.getRightBottomBoundary().X + objectRadius 'If inside the X visible area at all.
        End Get
    End Property
    Public ReadOnly Property isVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY - objectRadius And
                   objectCenterOfMass.Y <= objectUniverse.getRightBottomBoundary().Y + objectRadius 'If inside the Y visible area at all.
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
                   objectCenterOfMass.X <= objectUniverse.getRightBottomBoundary().X 'If it's half or more is inside X visible area.
        End Get
    End Property
    Public ReadOnly Property isPartialVisibleY() As Boolean
        Get
            Return objectCenterOfMass.Y >= objectUniverseOffsetY And
                   objectCenterOfMass.Y <= objectUniverse.getRightBottomBoundary().Y 'If it's half or more is inside Y visible area.
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

    Friend Overridable Sub Paint()

    End Sub

End Class
