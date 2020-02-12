VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3195
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4680
   LinkTopic       =   "Form1"
   ScaleHeight     =   3195
   ScaleWidth      =   4680
   StartUpPosition =   3  'Windows Default
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Type POINT2D

    X As Single
    Y As Single

End Type

Private Type VECTOR2D

    X As Single
    Y As Single

End Type

Private Type TIME_TYPE

    Time As Single
    Initial As Single
    Current As Single
    New_Time As Single
    Delta As Single
    Accumulator As Single

End Type

Private Type SPRITE2D

    Force As VECTOR2D
    Velocity As VECTOR2D
    Position As POINT2D
    Acceleration As POINT2D
    Mass As Single
    Time As TIME_TYPE
    

End Type

Private Declare Function QueryPerformanceCounter Lib "Kernel32" (lpPerformanceCount As Currency) As Long
Private Declare Function QueryPerformanceFrequency Lib "Kernel32" (lpPerformanceCount As Currency) As Long

Private Const dt As Single = 0.01
Private Const GRAVITY As Single = 9.80665
Private Const SCALAR As Single = 50

Private Sprite As SPRITE2D

Private Center As POINT2D

Private Running As Boolean

Private Ticks_Per_Second As Currency
Private Start_Time As Currency

Private Function Get_Elapsed_Time() As Single
    
    Dim Last_Time As Currency
    
    Dim Current_Time As Currency

    QueryPerformanceCounter Current_Time
    
    Get_Elapsed_Time = (Current_Time - Last_Time) / Ticks_Per_Second
    
    QueryPerformanceCounter Last_Time
    
End Function

Private Function Lock_FPS(ByVal Target_FPS As Byte) As Single

    If Target_FPS = 0 Then Target_FPS = 1

    Static Last_Time As Currency

    Dim Current_Time As Currency

    Dim FPS As Single

    Last_Time = Start_Time - 1
    
    Do

        QueryPerformanceCounter Current_Time
    
        If (Current_Time - Last_Time) <> 0 Then FPS = Ticks_Per_Second / (Current_Time - Last_Time)
    
        Lock_FPS = FPS
    
    Loop While (FPS > Target_FPS)
    
    Last_Time = Start_Time - 1
    
End Function

Private Sub Form_Activate()
    
    ScaleMode = 3
    DrawWidth = 10
    AutoRedraw = True
    
    QueryPerformanceFrequency Ticks_Per_Second
    QueryPerformanceCounter Start_Time
    
    With Sprite
    
        .Time.Initial = Get_Elapsed_Time
        
        .Mass = 100
        
        .Force.X = 0
        .Force.Y = .Mass * -GRAVITY
        
        .Velocity.X = 0
        .Velocity.Y = 0
        
        .Position.X = Me.ScaleWidth / 2
        .Position.Y = Me.ScaleHeight / 2
        
        .Acceleration.X = .Force.X / .Mass
        .Acceleration.Y = .Force.Y / .Mass
    
    End With
    
    Center.Y = Me.Height / 2
    
    Running = True
    
    Do While Running = True
    
        DoEvents
        
        Me.Cls
        
        Lock_FPS 60
        
        With Sprite.Time
    
            .New_Time = Get_Elapsed_Time - .Initial
            .Delta = .New_Time - .Current
            .Current = .New_Time
            
            If (.Delta > 0.25) Then .Delta = 0.25
            
            .Accumulator = .Accumulator + .Delta
            
            While (.Accumulator >= dt)
            
                Sprite.Position.X = Sprite.Position.X + (Sprite.Velocity.X * dt) * SCALAR
                Sprite.Velocity.X = Sprite.Velocity.X + Sprite.Acceleration.X * dt
           
                Sprite.Position.Y = Sprite.Position.Y + (Sprite.Velocity.Y * dt) * SCALAR
                Sprite.Velocity.Y = Sprite.Velocity.Y - Sprite.Acceleration.Y * dt
           
                .Time = .Time + dt
           
                .Accumulator = .Accumulator - dt
            
            Wend
        
        End With

        PSet (Sprite.Position.X, Sprite.Position.Y)
        
        QueryPerformanceCounter Start_Time
        
    Loop

End Sub

Private Sub Form_Unload(Cancel As Integer)

    Running = False
    Unload Me
    End

End Sub
