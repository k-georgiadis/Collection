Public Class Map

    Private _filename As String
    Private _fullname As String
    Private _CD As String
    Private _MinPlayers As String
    Private _MaxPlayers As String
    Private _Description As String
    Private _Name As String
    Private _Author As String
    Private _EnforceMaxPlayers As String
    Private _Size As String
    Private _LocalSize As String
    Private _PreviewSize As String
    Private _GameModes As String
    Private _Waypoints(7) As String 'Array of waypoints.

    ' Example below:

    ' CD = 0,1,2
    ' MinPlayers = 2
    ' MaxPlayers = 2
    ' Name = Test Map (8)
    ' Description =[8] Test Map
    ' Author = DaBoss
    ' EnforceMaxPlayers = False
    ' Size = 0,0,200,195
    ' LocalSize = 5,5,190,182
    ' PreviewSize = 800,393
    ' GameModes = Fan - made
    ' Waypoint0 = 23212
    ' Waypoint1 = 13252

    'Getters.

    Public Function filename() As String
        Return _filename
    End Function
    Public Function fullname() As String
        Return _fullname
    End Function
    Public Function CD() As String
        Return _CD
    End Function
    Public Function MinPlayers() As String
        Return _MinPlayers
    End Function
    Public Function MaxPlayers() As String
        Return _MaxPlayers
    End Function
    Public Function Name() As String
        Return _Name
    End Function
    Public Function Description() As String
        Return _Description
    End Function
    Public Function Author() As String
        Return _Author
    End Function
    Public Function EnforceMaxPlayers() As String
        Return _EnforceMaxPlayers
    End Function
    Public Function Size() As String
        Return _Size
    End Function
    Public Function LocalSize() As String
        Return _LocalSize
    End Function
    Public Function PreviewSize() As String
        Return _PreviewSize
    End Function
    Public Function GameModes() As String
        Return _GameModes
    End Function
    Public Function Waypoints() As String()
        Return _Waypoints
    End Function

    'Setters.
    Public Sub filename(ByVal new_filename As String)
        _filename = new_filename
    End Sub
    Public Sub fullname(ByVal new_fullname As String)
        _fullname = new_fullname
    End Sub
    Public Sub CD(ByVal new_CD As String)
        _CD = new_CD
    End Sub
    Public Sub MinPlayers(ByVal new_MinPlayers As String)
        _MinPlayers = new_MinPlayers
    End Sub
    Public Sub MaxPlayers(ByVal new_MaxPlayers As String)
        _MaxPlayers = new_MaxPlayers
    End Sub
    Public Sub Name(ByVal new_Name As String)

        'I have no idea how Rampastring generates the filenames.
        'For now, I just remove a parenthesis string which I assume it contains the max players.
        Dim playersArea As Integer = new_Name.IndexOf("(")

        If playersArea >= 0 Then
            _Name = new_Name.Remove(playersArea, new_Name.IndexOf(")") - playersArea + 1).Trim
        Else
            _Name = new_Name.Trim
        End If


    End Sub
    Public Sub Description(ByVal new_Description As String)
        _Description = new_Description
    End Sub
    Public Sub Author(ByVal new_Author As String)
        _Author = new_Author
    End Sub
    Public Sub EnforceMaxPlayers(ByVal new_EnforceMaxPlayers As String)
        _EnforceMaxPlayers = new_EnforceMaxPlayers
    End Sub
    Public Sub Size(ByVal new_Size As String)
        _Size = new_Size
    End Sub
    Public Sub LocalSize(ByVal new_LocalSize As String)
        _LocalSize = new_LocalSize
    End Sub
    Public Sub PreviewSize(ByVal new_PreviewSize As String)
        _PreviewSize = new_PreviewSize
    End Sub
    Public Sub GameModes(ByVal new_GameModes As String)
        _GameModes = Char.ToUpper(new_GameModes(0)) & new_GameModes.Substring(1) 'First letter uppercase.
    End Sub

    Public Sub Waypoints(ByVal map_text As String, ByVal getMapParamValue As Func(Of String, String, String))

        Dim counter As Integer = 0
        Dim waypointSection As String = map_text.Substring(map_text.IndexOf("[Waypoints]")) 'Find waypoint section.

        If _MaxPlayers IsNot String.Empty Then

            While counter < CInt(_MaxPlayers)
                _Waypoints(counter) = New String("Waypoint" + counter.ToString + "=" + getMapParamValue(counter.ToString, waypointSection)) 'Get and save waypoint.
                counter += 1
            End While

        Else

            Dim w_end As String = waypointSection.IndexOf(vbCrLf + vbCrLf)
            Dim w_index As Integer = waypointSection.IndexOf(counter.ToString + "=")

            While w_index > -1 And w_index < w_end And counter < 8

                _Waypoints(counter) = New String("Waypoint" + counter.ToString + "=" + getMapParamValue(counter.ToString, waypointSection)) 'Get and save waypoint.
                counter += 1
                w_index = waypointSection.IndexOf(counter.ToString + "=")

            End While

        End If

    End Sub
    Public Sub Waypoint(ByVal new_Waypoint_Index As Integer, ByVal new_Waypoint As String)
        _Waypoints(new_Waypoint_Index) = new_Waypoint
    End Sub

End Class
