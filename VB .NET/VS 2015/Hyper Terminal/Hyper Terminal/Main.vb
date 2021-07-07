'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Public Class Main

    Dim listen As Integer = 0
    Dim checkPort As Integer = 0
    Dim ListenOver As Integer = 0
    Dim HexData As String = 0
    Dim appendChar As String = vbCrLf

    Dim com As New IO.Ports.SerialPort

    Private Sub Test_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitControls()
    End Sub

    Private Sub InitControls()

        'Green color for opening port.
        usePort.ForeColor = Color.DarkGreen

        'Get all available COM ports.
        For Each sp As String In My.Computer.Ports.SerialPortNames
            portList.Items.Add(sp)
        Next

        If portList.Items.Count Then
            portList.SelectedItem = portList.Items(0)
        End If

        'Get parity values.
        For Each par As String In [Enum].GetNames(GetType(IO.Ports.Parity))
            parityList.Items.Add(par)
        Next

        'Get stop bit values.
        For Each bit As String In [Enum].GetNames(GetType(IO.Ports.StopBits))
            stopbitsList.Items.Add(bit)
        Next

        'Fill Append list.
        appendList.Items.Add("CR")
        appendList.Items.Add("LF")
        appendList.Items.Add("CR-LF")
        appendList.Items.Add("Nothing")

        'Get saved settings, if any.
        GetSettings()

        'Disable buttons.
        transmitData.Enabled = False

        'Assign Tooltip.
        hexTooltip.SetToolTip(byteMode, "Examples : FF, 0a, c0, D, 0, 9")
    End Sub

    Private Sub GetSettings()

        'Get saved settings, if changed.

        If My.Settings.Item("usrBR") <> "" Then
            rateList.SelectedItem = My.Settings.Item("usrBR")
        Else
            rateList.SelectedIndex = 3 'Baud Rate = 19200
        End If

        If My.Settings.Item("usrDB") <> "" Then
            databitsList.SelectedItem = My.Settings.Item("usrDB")
        Else
            databitsList.SelectedIndex = 3 'Data bits = 8
        End If

        If My.Settings.Item("usrSB") <> "" Then
            stopbitsList.SelectedItem = My.Settings.Item("usrSB")
        Else
            stopbitsList.SelectedIndex = 1 'Stop bits = 1
        End If

        If My.Settings.Item("usrPAR") <> "" Then
            parityList.SelectedItem = My.Settings.Item("usrPAR")
        Else
            parityList.SelectedIndex = 0 'Parity = None
        End If

        If My.Settings.Item("usrMOD") = "byte" Then
            byteMode.Checked = True
        End If

        If My.Settings.Item("usrAPP") <> "" Then
            appendList.SelectedItem = My.Settings.Item("usrAPP")
        Else
            appendList.SelectedIndex = 2 'Append CR-LF.
        End If

        If My.Settings.Item("usrLE") = True Then
            echoCheck.Checked = True
        End If

        If My.Settings.Item("usrSC") = True Then
            sendCapsCheck.Checked = True
        End If

        If My.Settings.Item("usrCI") = True Then
            clearInputCheck.Checked = True
        End If

    End Sub

    Private Delegate Sub AddFormattedTextDelegate(ByVal RTC As RichTextBox, ByVal text As String, ByVal col As Color, ByVal style As FontStyle)

    Private Sub AddFormattedText(ByVal RTC As RichTextBox, ByVal text As String, ByVal col As Color, ByVal style As FontStyle)
        If RTC.InvokeRequired Then
            RTC.Invoke(New AddFormattedTextDelegate(AddressOf AddFormattedText), New Object() {RTC, text, col, style})
        Else
            With RTC
                .Select(.TextLength, 0)
                .SelectionFont = New Font(.SelectionFont, style)
                .SelectionColor = col
                .AppendText(text)
                .ScrollToCaret()
            End With
        End If
    End Sub

    Private Sub usePort_Click(sender As Object, e As EventArgs) Handles usePort.Click

        If portList.SelectedItem Is Nothing Then
            AddFormattedText(outputTextBox, vbCrLf + "Error: COM port not selected!" + vbCrLf, Color.DarkRed, FontStyle.Bold)

        ElseIf com.IsOpen = True Then

            closePort() 'Close port.

            AddFormattedText(outputTextBox, vbCrLf + "----------------------------------------------" + vbCrLf, Color.Black, FontStyle.Bold)
            AddFormattedText(outputTextBox, "Status: ", Color.Black, FontStyle.Bold)
            AddFormattedText(outputTextBox, portList.SelectedItem.ToString, Color.Blue, FontStyle.Bold)
            AddFormattedText(outputTextBox, " - Closed - ", Color.DarkRed, FontStyle.Bold)
            AddFormattedText(outputTextBox, "[" + DateTime.Now + "]", Color.DarkRed, FontStyle.Bold)
            AddFormattedText(outputTextBox, vbCrLf + "----------------------------------------------", Color.Black, FontStyle.Bold)

        Else
            Try
                openPort() 'Open port.

                AddFormattedText(outputTextBox, vbCrLf + "----------------------------------------------" + vbCrLf, Color.Black, FontStyle.Bold)
                AddFormattedText(outputTextBox, "Status: ", Color.Black, FontStyle.Bold)
                AddFormattedText(outputTextBox, portList.SelectedItem.ToString, Color.Blue, FontStyle.Bold)
                AddFormattedText(outputTextBox, " - Open - ", Color.DarkGreen, FontStyle.Bold)
                AddFormattedText(outputTextBox, "[" + DateTime.Now + "]", Color.DarkGreen, FontStyle.Bold)
                AddFormattedText(outputTextBox, vbCrLf + "----------------------------------------------" + vbCrLf, Color.Black, FontStyle.Bold)

            Catch ex As Exception
                AddFormattedText(outputTextBox, vbCrLf + "Error: " + ex.Message + vbCrLf, Color.DarkRed, FontStyle.Bold)
            End Try
        End If

    End Sub

    Private Sub closePort()

        'UI changes.
        portList.Enabled = True
        portListTooltip.SetToolTip(serialportlbl, "")
        transmitData.Enabled = False
        usePort.Text = "Open Port"
        usePort.ForeColor = Color.DarkGreen
        inputData.Focus()

        com.Close() 'Close port.

    End Sub

    Private Sub openPort()

        'Open port.
        com = My.Computer.Ports.OpenSerialPort(portList.SelectedItem)

        'Set com settings.
        com.BaudRate = Integer.Parse(rateList.SelectedItem)
        com.StopBits = DirectCast([Enum].Parse(GetType(IO.Ports.StopBits), stopbitsList.SelectedItem), IO.Ports.StopBits)
        com.Parity = DirectCast([Enum].Parse(GetType(IO.Ports.Parity), parityList.SelectedItem), IO.Ports.Parity)

        'Add handler for received data.
        AddHandler com.DataReceived, AddressOf COM_DataReceived

        'UI changes.
        portList.Enabled = False
        portListTooltip.SetToolTip(serialportlbl, "Close the opened port first!")
        transmitData.Enabled = True
        usePort.Text = "Close Port"
        usePort.ForeColor = Color.DarkRed
        inputData.Focus()

    End Sub

    Private Sub transmitData_Click(sender As Object, e As EventArgs) Handles transmitData.Click

        Try
            If stringMode.Checked = True Then
                Send_Data(inputData.Text, "string")
            Else
                Send_Data(inputData.Text, "byte")
            End If
        Catch ex As Exception
            AddFormattedText(outputTextBox, ex.Message + vbCrLf, Color.DarkRed, FontStyle.Bold)
        End Try
    End Sub

    Private Sub Send_Data(ByVal data As String, ByVal type As String)

        'Send string or byte to the serial port.
        If com.IsOpen Then

            If type = "string" Then

                'Send data capitalized, if needed.
                If sendCapsCheck.CheckState = CheckState.Checked Then
                    data = data.ToUpper()
                End If

                'Send data with the appendChar.
                com.Write(data + appendChar + vbNullChar)

            ElseIf type = "byte" Then

                com.Write(New Byte() {Convert.ToInt16(data, 16)}, 0, 1)

                'If data.Length = 1 Then
                'data = "0" + data 'Add the leading zero(0) if we didn't send full byte.

                'data = "0x" + data.ToUpper

            End If

            'Add echo.
            If echoCheck.Checked = True Then
                AddFormattedText(outputTextBox, data, Color.Blue, FontStyle.Regular)
            End If

            'Clear text if needed.
            If clearInputCheck.CheckState = CheckState.Checked Then
                inputData.Clear()
            End If

            inputData.Focus()
        End If

    End Sub

    Private Sub COM_DataReceived(ByVal sender As Object, ByVal e As IO.Ports.SerialDataReceivedEventArgs)

        'This Sub gets called automatically, when the COM port receives data.
        Dim str As String = com.ReadExisting

        'Print as string or as HEX bytes.
        If stringMode.Checked = True Then
            AddFormattedText(outputTextBox, str, Color.Purple, FontStyle.Bold)
        Else
            Dim data As New List(Of Byte)
            Dim byte_val As Byte
            Dim HexData As String

            For Each c As Char In str

                byte_val = Convert.ToByte(c)
                HexData = Convert.ToString(byte_val, 16)

                'Add the leading zero(0) if we didn't get a full byte.
                If HexData.Length = 1 Then
                    HexData = "0" + HexData
                End If

                'Add newline if necessary.
                If c = Chr(10) Then
                    HexData += vbNewLine
                End If

                'AddFormattedText(outputTextBox, "0x" + HexData.ToString.ToUpper, Color.Purple, FontStyle.Bold)
                AddFormattedText(outputTextBox, HexData.ToString.ToUpper, Color.Purple, FontStyle.Bold)
            Next

        End If
    End Sub

    Private Sub closeApplication_Click(sender As Object, e As EventArgs) Handles closeApplication.Click
        End
    End Sub

    Private Sub byteMode_CheckedChanged(sender As Object, e As EventArgs) Handles byteMode.CheckedChanged

        If byteMode.Checked = True Then
            inputData.MaxLength = 2 'For Hex numbers.
            sendCapsCheck.Enabled = False
            sendCapsCheck.Checked = False

            My.Settings.Item("usrMOD") = "byte"
        Else
            inputData.MaxLength = 2147483647 'Default value.
            sendCapsCheck.Enabled = True

            My.Settings.Item("usrMOD") = "string"
        End If

        My.Settings.Save()
    End Sub

    Private Sub appendList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles appendList.SelectedIndexChanged

        My.Settings.Item("usrAPP") = appendList.SelectedItem
        My.Settings.Save()

        If appendList.SelectedIndex = 0 Then
            appendChar = vbCr
        ElseIf appendList.SelectedIndex = 1 Then
            appendChar = vbLf
        ElseIf appendList.SelectedIndex = 2 Then
            appendChar = vbCrLf
        ElseIf appendList.SelectedIndex = 3 Then
            appendChar = ""
        End If

    End Sub

    Private Sub rateList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rateList.SelectedIndexChanged

        My.Settings.Item("usrBR") = rateList.SelectedItem
        My.Settings.Save()

    End Sub

    Private Sub databitsList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles databitsList.SelectedIndexChanged

        My.Settings.Item("usrDB") = databitsList.SelectedItem
        My.Settings.Save()

    End Sub

    Private Sub stopbitsList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles stopbitsList.SelectedIndexChanged

        My.Settings.Item("usrSB") = stopbitsList.SelectedItem
        My.Settings.Save()

    End Sub

    Private Sub parityList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles parityList.SelectedIndexChanged

        My.Settings.Item("usrPAR") = parityList.SelectedItem
        My.Settings.Save()

    End Sub

    Private Sub echoCheck_CheckedChanged(sender As Object, e As EventArgs) Handles echoCheck.CheckedChanged

        My.Settings.Item("usrLE") = CBool(echoCheck.CheckState)
        My.Settings.Save()

    End Sub

    Private Sub sendCapsCheck_CheckedChanged(sender As Object, e As EventArgs) Handles sendCapsCheck.CheckedChanged

        My.Settings.Item("usrSC") = CBool(sendCapsCheck.CheckState)
        My.Settings.Save()

    End Sub

    Private Sub clearInputCheck_CheckedChanged(sender As Object, e As EventArgs) Handles clearInputCheck.CheckedChanged

        My.Settings.Item("usrCI") = CBool(clearInputCheck.CheckState)
        My.Settings.Save()

    End Sub
End Class
