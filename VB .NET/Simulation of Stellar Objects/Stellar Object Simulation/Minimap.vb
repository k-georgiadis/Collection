'MIT License

'Copyright (c) 2023 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.Runtime.CompilerServices

Public Class Minimap

    Private minimapBox As GroupBox
    Private minimapPicBox As PictureBox
    Private cosmosPanel As FlowLayoutPanel
    Private cosmosSectionBtnList As List(Of Button)
    Private cosmosNavReturn As Button

    Private minimapGraphics As Graphics
    Private minimapPen As Pen
    Private minimapImage As Bitmap
    Private minimapImageWidth As Double
    Private minimapImageHeight As Double
    Private minimapMatrix As Drawing2D.Matrix
    Private minimapLocation As PointF

    Private currentSection As Integer

    Friend Sub New()
    End Sub

    Public Property getGraphics As Graphics
        Get
            Return minimapGraphics
        End Get
        Set(value As Graphics)
            minimapGraphics = value
        End Set
    End Property
    Public Property getImage As Bitmap
        Get
            Return minimapImage
        End Get
        Set(value As Bitmap)
            minimapImage = value
        End Set
    End Property

    Public Property ImageWidth As Double
        Get
            Return minimapImageWidth
        End Get
        Set(value As Double)
            minimapImageWidth = value
        End Set
    End Property

    Public Property ImageHeight As Double
        Get
            Return minimapImageHeight
        End Get
        Set(value As Double)
            minimapImageHeight = value
        End Set
    End Property

    Public Property getPicBox As PictureBox
        Get
            Return minimapPicBox
        End Get
        Set(value As PictureBox)
            minimapPicBox = value
        End Set
    End Property

    Public Property getLocation As PointF
        Get
            Return minimapLocation
        End Get
        Set(value As PointF)
            minimapLocation = value
        End Set
    End Property
    Friend Sub Init(minimapBox As GroupBox, minimapPicBox As PictureBox, cosmosPanel As FlowLayoutPanel, cosmosSectionBtnList As List(Of Button), cosmosNavReturn As Button)

        If minimapBox Is Nothing Then
            Throw New ArgumentNullException(NameOf(minimapBox))
        End If

        If minimapPicBox Is Nothing Then
            Throw New ArgumentNullException(NameOf(minimapPicBox))
        End If

        If cosmosPanel Is Nothing Then
            Throw New ArgumentNullException(NameOf(cosmosPanel))
        End If

        If cosmosSectionBtnList Is Nothing Then
            Throw New ArgumentNullException(NameOf(cosmosSectionBtnList))
        End If

        If cosmosNavReturn Is Nothing Then
            Throw New ArgumentNullException(NameOf(cosmosNavReturn))
        End If

        Me.minimapBox = minimapBox
        Me.minimapPicBox = minimapPicBox
        Me.cosmosPanel = cosmosPanel
        Me.cosmosSectionBtnList = cosmosSectionBtnList
        Me.cosmosNavReturn = cosmosNavReturn

        Me.cosmosPanel.Visible = False
        Me.cosmosNavReturn.Visible = True
        currentSection = 1 'Default cosmos section.
        minimapLocation = New PointF(minimapBox.Left + minimapPicBox.Left, minimapBox.Top + minimapPicBox.Top)

        Load() 'Initialize graphics.

    End Sub
    Friend Sub Load()

        If minimapGraphics Is Nothing Then

            If minimapPicBox Is Nothing Then
                minimapImage = New Bitmap(296, 233) 'Default size.
            Else
                'Use a few pixels less because they are used by the picture box.
                minimapImage = New Bitmap(minimapPicBox.Width, minimapPicBox.Height)

                'We adjust the image size because the client rectangle is different in size.
                'Remember that we use a PictureBox element to display the image, unlike the Universe which is directly painted on top of the form.
                Dim unusedPixels As Integer = minimapPicBox.Width - minimapPicBox.ClientSize.Width
                minimapImageWidth = minimapImage.Width - unusedPixels
                minimapImageHeight = minimapImage.Height - unusedPixels
            End If

            minimapGraphics = Graphics.FromImage(minimapImage)
            minimapGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            minimapGraphics.SmoothingMode = Drawing2D.SmoothingMode.None
            minimapGraphics.Clear(Color.Black)

            minimapPen = New Pen(Color.Yellow)
            minimapPen.Width = 1

            'Save transformation so we don't run into access violations.
            minimapMatrix = minimapGraphics.Transform.Clone

        End If

    End Sub
    Friend Sub Update(ByVal universeMatrix As Drawing2D.Matrix, ByVal objList As List(Of StellarObject),
                      ByVal universeSizeData() As Double, ByVal visibleUniverseSizeData() As Double)

        'First draw the objects.
        For Each obj In objList

            'Convert size to local size.
            Dim sizeX As Double = (obj.VisualSize * minimapImage.Width) / universeSizeData(2)
            Dim sizeY As Double = (obj.VisualSize * minimapImage.Height) / universeSizeData(3)

            'Don't disappear completely.
            If sizeX < 1 Then
                sizeX = 1
            End If
            If sizeY < 1 Then
                sizeY = 1
            End If

            'These calculations are for displaying only the current view in the minimap, along with the visible objects.
            'Dim x As Double = (Math.Abs(obj.CenterOfMass.X - universeSizeData(0)) *
            '                        (minimapImage.Width - 2)) / (universeSizeData(2) - universeSizeData(0)) - sizeX / 2
            'Dim y As Double = (Math.Abs(obj.CenterOfMass.Y - universeSizeData(1)) *
            '                        (minimapImage.Height - 2)) / (universeSizeData(3) - universeSizeData(1)) - sizeX / 2

            'These calculations are for displaying the entire cosmos section in the minimap including all objects.
            Dim x As Double = (Math.Abs(obj.CenterOfMass.X + universeSizeData(2) / 2) * (minimapImage.Width - 1)) /
                              universeSizeData(2) - sizeX / 2
            Dim y As Double = (Math.Abs(obj.CenterOfMass.Y + universeSizeData(3) / 2) * (minimapImage.Height - 1)) /
                              universeSizeData(3) - sizeX / 2

            minimapPen.Color = obj.Color

            'If obj.isStar Then
            '    minimapGraphics.DrawEllipse(minimapPen, New RectangleF(x, y, sizeX, sizeY))
            'Else
            'Using RectangleF makes the shape miss a few pixels, sometimes.
            'Because of that, we lose a few pixels in total size due to integer rounding.
            'An easy, yet hacky, solution is to subtract a fixed value of pixels from the minimap size while doing the calculations.
            'Dim rects() As RectangleF = {New RectangleF(x, y, sizeX, sizeY)}
            minimapGraphics.DrawRectangle(minimapPen, New Rectangle(x, y, sizeX, sizeY))
            minimapGraphics.FillRectangle(minimapPen.Brush, New Rectangle(x, y, sizeX, sizeY))

            'End If

        Next

        'Now the minimap camera.
        DrawMinimapCamera(universeSizeData, visibleUniverseSizeData)

    End Sub
    Private Sub DrawMinimapCamera(ByVal universeSizeData() As Double, ByVal visibleUniverseSizeData() As Double)

        Dim clipOffsetX As Double = visibleUniverseSizeData(0)
        Dim clipOffsetY As Double = visibleUniverseSizeData(1)
        Dim universeRight As Double = visibleUniverseSizeData(2)
        Dim universeBottom As Double = visibleUniverseSizeData(3)
        Dim cosmosWidth As Double = universeSizeData(2)
        Dim cosmosHeight As Double = universeSizeData(3)

        Dim x As Double = (Math.Abs(clipOffsetX + cosmosWidth / 2) * (minimapImageWidth)) / (cosmosWidth + 1)
        Dim y As Double = (Math.Abs(clipOffsetY + cosmosHeight / 2) * (minimapImageHeight)) / (cosmosHeight + 1)
        Dim width As Double = (Math.Abs(universeRight + cosmosWidth / 2) * (minimapImageWidth)) / (cosmosWidth + 1) - x
        Dim height As Double = (Math.Abs(universeBottom + cosmosHeight / 2) * (minimapImageHeight)) / (cosmosHeight + 1) - y

        'The border itself hides elements so we need to adjust the width.
        If x + width > minimapImageWidth - 1 Then
            width -= 1
        Else
            width += 1
        End If
        If y + height > minimapImageHeight - 1 Then
            height -= 1
        Else
            height += 1
        End If


        minimapPen.Color = Color.White
        minimapGraphics.DrawRectangle(minimapPen, New Rectangle(x, y, width, height))

    End Sub

End Class
