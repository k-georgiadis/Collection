'MIT License

Imports System.Windows

Friend Structure Vector3D

    Public X As Double
    Public Y As Double
    Public Z As Double

    Friend Sub New(ByVal _x As Double, ByVal _y As Double, ByVal _z As Double)
        X = _x
        Y = _y
        Z = _z
    End Sub

    Friend Function Length() As Double

        Return Math.Sqrt(X * X + Y * Y + Z * Z)

    End Function
    Friend Function ToPointF() As PointF
        Return New PointF(X, Y)
    End Function

    Friend Function DistanceFromPointF(ByVal target As PointF) As Double

        Return Math.Sqrt((X - target.X) * (X - target.X) + (Y - target.Y) * (Y - target.Y))

    End Function

End Structure
