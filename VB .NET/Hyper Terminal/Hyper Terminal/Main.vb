'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class Main

    Const SPM_PAGESIZE As Integer = 128
    Const FLASH_OK As Byte = &H1A
    Const FLASH_NOK As Byte = &H1E

    Dim listen As Integer = 0
    Dim checkPort As Integer = 0
    Dim ListenOver As Integer = 0
    Dim HexData As String = 0
    Dim appendChar As String = vbCrLf

    Dim com As New IO.Ports.SerialPort

    Dim flashStatus As String
    Dim mcuResponse As String = ""
    Dim flashThread As New Thread(AddressOf BeginFlash)

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
        hexTooltip.SetToolTip(byteMode, "Examples : FF, 0a, c0, 0001, 10AA")

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
        ElseIf My.Settings.Item("usrMOD") = "flash" Then
            flashMode.Checked = True
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
            If flashStatus = "failed" OrElse flashStatus = "complete" Then
                transmitData.Enabled = True
            End If
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
            AddFormattedText(outputTextBox, "[" + Date.Now + "]", Color.DarkRed, FontStyle.Bold)
            AddFormattedText(outputTextBox, vbCrLf + "----------------------------------------------", Color.Black, FontStyle.Bold)

        Else
            Try
                openPort() 'Open port.

                AddFormattedText(outputTextBox, vbCrLf + "----------------------------------------------" + vbCrLf, Color.Black, FontStyle.Bold)
                AddFormattedText(outputTextBox, "Status: ", Color.Black, FontStyle.Bold)
                AddFormattedText(outputTextBox, portList.SelectedItem.ToString, Color.Blue, FontStyle.Bold)
                AddFormattedText(outputTextBox, " - Open - ", Color.DarkGreen, FontStyle.Bold)
                AddFormattedText(outputTextBox, "[" + Date.Now + "]", Color.DarkGreen, FontStyle.Bold)
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
            ElseIf byteMode.Checked Then
                Send_Data(inputData.Text, "byte")
            ElseIf flashMode.Checked Then
                Send_Data(binaryFilePath.Text, "flash")
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

                Dim byteList As New List(Of Byte)

                Dim matches As MatchCollection = Regex.Matches(data, ".{2}")
                For Each match In matches
                    byteList.Add(Byte.Parse(match.ToString, Globalization.NumberStyles.HexNumber))
                Next
                com.Write(byteList.ToArray, 0, byteList.Count)

                'If data.Length = 1 Then
                'data = "0" + data 'Add the leading zero(0) if we didn't send full byte.
                'data = "0x" + data.ToUpper

            ElseIf type = "flash" Then

                transmitData.Enabled = False

                'Begin FLASH process.
                If Not flashThread.IsAlive OrElse flashThread.ThreadState = ThreadState.Aborted Then
                    flashThread = New Thread(AddressOf BeginFlash)
                    flashThread.Start(data)
                End If
                flashStatus = "init"

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

    'This Sub gets called automatically, when the COM port receives data.
    Private Sub COM_DataReceived(ByVal sender As Object, ByVal e As IO.Ports.SerialDataReceivedEventArgs)

        Dim str As String = com.ReadExisting

        'Print as string or as HEX bytes.
        If stringMode.Checked = True Then
            AddFormattedText(outputTextBox, str, Color.Purple, FontStyle.Bold)
        ElseIf byteMode.Checked Then
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
        ElseIf flashMode.Checked Then

            'Save response.
            If mcuResponse = String.Empty Then
                mcuResponse += str
            End If

            'If flashStatus = "complete" Then
            '    Dim byte_val As Byte
            '    Dim HexData As String

            '    For Each c As Char In mcuResponse

            '        byte_val = Convert.ToByte(c)
            '        HexData = Convert.ToString(byte_val, 16)

            '        'Add the leading zero(0) if we didn't get a full byte.
            '        If HexData.Length = 1 Then
            '            HexData = "0" + HexData
            '        End If

            '        'Add newline if necessary.
            '        If c = Chr(10) Then
            '            HexData += vbNewLine
            '        End If
            '        AddFormattedText(outputTextBox, HexData.ToString.ToUpper, Color.Purple, FontStyle.Bold)
            '    Next

            'End If

        End If

    End Sub

    Private Sub closeApplication_Click(sender As Object, e As EventArgs) Handles closeApplication.Click
        End
    End Sub

    Private Sub byteMode_CheckedChanged(sender As Object, e As EventArgs) Handles byteMode.CheckedChanged

        If byteMode.Checked = True Then

            'inputData.MaxLength = 2 'For Hex numbers.
            inputData.MaxLength = 2147483647

            sendCapsCheck.Enabled = False
            sendCapsCheck.Checked = False
            appendList.Enabled = True

            My.Settings.Item("usrMOD") = "byte"
            My.Settings.Save()

        Else
            inputData.MaxLength = 2147483647 'Default value.
            sendCapsCheck.Enabled = True
        End If

    End Sub

    Private Sub flashMode_CheckedChanged(sender As Object, e As EventArgs) Handles flashMode.CheckedChanged

        If flashMode.Checked = True Then

            inputData.MaxLength = 2147483647

            sendCapsCheck.Enabled = False
            sendCapsCheck.Checked = False
            appendList.Enabled = False
            appendList.SelectedIndex = 3
            inputData.Enabled = False
            transmitData.Text = "FLASH"
            binaryFilePath.Visible = True
            browseButton.Visible = True

            My.Settings.Item("usrMOD") = "flash"
            My.Settings.Save()

        Else
            sendCapsCheck.Enabled = True
            appendList.Enabled = True
            inputData.Enabled = True
            transmitData.Text = "Send"
            binaryFilePath.Visible = False
            browseButton.Visible = False
        End If

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

    Private Sub browseButton_Click(sender As Object, e As EventArgs) Handles browseButton.Click

        openFileDialog.ShowDialog()
        binaryFilePath.Text = openFileDialog.FileName

    End Sub

    'OPTIONS

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

    'FLASHING

    Private Sub BeginFlash(ByVal data As String)

        If flashStatus = "init" Then

                mcuResponse = ""

            Try
                AddFormattedText(outputTextBox, vbCrLf + "************************************************************" + vbCrLf, Color.Black, FontStyle.Bold)
                AddFormattedText(outputTextBox, "[" + Date.Now + "] - Starting operation - [ FLASH ]" + vbCrLf, Color.DarkGreen, FontStyle.Bold)

                If data = String.Empty Then
                    Throw New Exception("Empty file path")
                End If

                Dim binaryData As Byte() = File.ReadAllBytes(data)

                AddFormattedText(outputTextBox, "[" + Date.Now + "] - BIN loaded - [ " + openFileDialog.SafeFileName + " ]" + vbCrLf, Color.DarkGreen, FontStyle.Bold)

                'Send signal to prepare for flashing.
                com.Write("x88")

                    'Send file size. (max 2 bytes)
                    com.Write(New Byte() {(binaryData.Count And &HFF00) >> 8, (binaryData.Count And &HFF)}, 0, 2)

                'Wait until we get a response or we time out.
                Dim timer As New Stopwatch
                timer.Restart()
                While mcuResponse = String.Empty

                    If timer.ElapsedMilliseconds >= 1000 Then
                        Exit While
                    End If

                End While
                timer.Stop()

                'Did we get the right response?
                If mcuResponse = Chr(FLASH_OK) Then

                    flashStatus = "start"

                    AddFormattedText(outputTextBox, "[" + Date.Now + "] - MCU OK -" + vbCrLf, Color.DarkGreen, FontStyle.Bold)
                    AddFormattedText(outputTextBox, "[" + Date.Now + "] - FLASHING -" + vbCrLf, Color.DarkGreen, FontStyle.Bold)

                    Dim buffer As List(Of Byte)
                    Dim crcByte As Byte

                    'Flash binary file to MCU in SPM_PAGESIZE chunks.
                    For i = 0 To binaryData.Count - 1 Step SPM_PAGESIZE

                        flashStatus = "send"
                        mcuResponse = ""

                        'If we 're on last page and we have spare bytes, replace them with 0xFF.
                        If i + SPM_PAGESIZE > binaryData.Count Then

                            buffer = binaryData.ToList.GetRange(i, binaryData.Count - i)
                            buffer.AddRange(Enumerable.Repeat(Of Byte)(&HFF, (i + SPM_PAGESIZE) - binaryData.Count))

                        Else
                            buffer = binaryData.ToList.GetRange(i, SPM_PAGESIZE)
                        End If

                        'Generate and insert CRC byte.
                        crcByte = GenerateCRC(buffer.ToArray, SPM_PAGESIZE)
                        buffer.Add(crcByte)

                        'Send page plus CRC byte.
                        com.Write(buffer.ToArray, 0, buffer.Count)

                        'Wait until we get a response or we time out.
                        timer.Restart()
                        While mcuResponse = String.Empty

                            If timer.ElapsedMilliseconds >= 1000 Then
                                Exit While
                            End If

                        End While
                        timer.Stop()

                        'Check response.
                        Select Case mcuResponse

                            Case Chr(FLASH_OK) 'CRC OK.
                                AddFormattedText(outputTextBox, "[" + Date.Now + "] - PAGE " + Convert.ToInt32((i / SPM_PAGESIZE)).ToString + " OK -" + vbCrLf, Color.DarkSlateGray, FontStyle.Bold)
                                Continue For

                            Case Chr(FLASH_NOK) 'CRC NOK.
                                Throw New Exception("BAD checksum")

                            Case Else
                                Throw New Exception("MCU timed out")

                        End Select

                    Next
                    mcuResponse = ""
                    flashStatus = "complete"
                    AddFormattedText(outputTextBox, "[" + Date.Now + "] - FLASH COMPLETE - ", Color.DarkGreen, FontStyle.Bold)

                Else
                    AddFormattedText(outputTextBox, "[" + Date.Now + "] - Error: MCU failed to respond - ", Color.Red, FontStyle.Bold)
                End If

            Catch ex As IOException
                flashStatus = "failed"
                AddFormattedText(outputTextBox, "[" + Date.Now + "] - Error: Failed to read BIN file - " + ex.Message, Color.Red, FontStyle.Bold)
            Catch ex As Exception
                flashStatus = "failed"
                AddFormattedText(outputTextBox, "[" + Date.Now + "] - Error: " + ex.Message + " - ", Color.Red, FontStyle.Bold)
            Finally

                AddFormattedText(outputTextBox, vbCrLf + "************************************************************" + vbCrLf, Color.Black, FontStyle.Bold)
                mcuResponse = String.Empty

            End Try

        End If

        flashThread.Abort()

    End Sub

    Private Function GenerateCRC(ByVal data As Byte(), ByVal len As Integer)

        Dim crc As Byte = 0

        For i = 0 To len - 1

            crc = crc Xor data(i)

            For j = 1 To 8
                If (crc And &H80) Then
                    crc = (crc << 1) Xor &H31
                Else
                    crc <<= 1
                End If
            Next

        Next

        Return crc

    End Function

End Class
