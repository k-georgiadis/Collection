'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

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

    Private universeWidth As Integer
    Private universeHeight As Integer

    Private visibleWidth As Integer
    Private visibleHeight As Integer

    Private planetList As New List(Of Planet)
    Private starList As New List(Of Star)

    Private canBounce As Boolean

    Private drawTrajectories As Boolean
    Private relativeTrajectories As Boolean
    Private bothTrajectories As Boolean
    Private maxTrajPoints As Integer

    Private universeDragged As Boolean

    Private Const gravityConstant As Double = 6.674 * 10 ^ -11
    Private Const distanceMultiplier As Integer = 1000 'Multiply each pixel by this number.
    Private Const M As Double = 19890000000.0 ' = 1.989E+10 'This is my edited mass. The real Solar Mass = 1.989E+30

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

    Public Property isDragged As Boolean
        Get
            Return universeDragged
        End Get
        Set(value As Boolean)
            universeDragged = value
        End Set
    End Property

    Public ReadOnly Property getGravityConstant() As Double
        Get
            Return gravityConstant
        End Get
    End Property
    Public ReadOnly Property getDistanceMultiplier() As Integer
        Get
            Return distanceMultiplier
        End Get

    End Property


    ' Allow friend access to the empty constructor.
    Friend Sub New()
    End Sub

    Friend Sub Init(ByVal bGraphics As Graphics, ByVal defaultUniverseMatrix As Matrix, ByVal bPen As Pen, ByVal uWidth As Integer, ByVal uHeight As Integer,
                    ByVal fWidth As Integer, ByVal fHeight As Integer, ByVal offset As PointFD,
                    ByVal _maxTrajPoints As Integer)
        universePen = bPen
        universeOffsetX = offset.X
        universeOffsetY = offset.Y
        defUniverseMatrix = defaultUniverseMatrix

        universeWidth = uWidth
        universeHeight = uHeight

        'Set current default form values.
        defaultFormHeight = fWidth
        defaultFormHeight = fHeight

        bmpImage = New Bitmap(universeWidth, universeHeight) 'Create image. Take in account the DPI factor too.

        universeGraphics = Graphics.FromImage(bmpImage)
        universeGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        universeGraphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        'Save transformation so we don't run into access violations.
        universeMatrix = universeGraphics.Transform.Clone()
        universeMatrix.Invert()

        Dim visibleBounds As RectangleF = bmpImage.GetBounds(universeGraphics.PageUnit)
        visibleWidth = visibleBounds.Width
        visibleHeight = visibleBounds.Height

        universeGraphics.Clip = New Region(New RectangleF(visibleBounds.Left + universeOffsetX, visibleBounds.Top + universeOffsetY,
                                                          visibleWidth, visibleHeight))

        canBounce = False

        drawTrajectories = True
        bothTrajectories = False
        maxTrajPoints = _maxTrajPoints 'Max number of points used for each trajectory.

        universeDragged = False 'Flag to check if the universe is being dragged.

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

        While StarArrayInUseFlag Or PlanetArrayInUseFlag

        End While

        StarArrayInUseFlag = True
        PlanetArrayInUseFlag = True

        'Make local copies to avoid "object in use" exceptions, as much as possible.
        Dim objList As List(Of StellarObject) = Objects()

        Try

            'First, calculate acceleration from all objects.
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

                'Move object.
                obj.Move(0, "", obj.CenterOfMass.X + obj.VelX, obj.CenterOfMass.Y + obj.VelY)

                If canBounce And Not universeDragged And obj.isVisible Then
                    obj.CheckForBounce()
                End If

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

