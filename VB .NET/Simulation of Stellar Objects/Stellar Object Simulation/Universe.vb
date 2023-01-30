'MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


Imports System.Drawing.Drawing2D

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

    Friend Sub AddPlanet(ByVal newplanet As Planet)
        planetList.Add(newplanet)
    End Sub
    Friend Sub AddStar(ByVal newstar As Star)
        starList.Add(newstar)
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

End Class

