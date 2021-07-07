'MIT License

'Copyright (c) 2021 Kosmas Georgiadis

'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports System.Configuration
Imports System.Xml

Public Class mainForm

    Private Sub mainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Load settings, if any.
        txtMapFolder.Text = My.Settings.mapFolder
        txtINI.Text = My.Settings.iniPath
        txtRendererPath.Text = My.Settings.exePath

        btnBrowseMapsToolTip.SetToolTip(btnBrowseMaps, "Directory that contains "".map"" or "".mpr"" files.")

    End Sub

    Private Sub btnBrowseMaps_Click(sender As Object, e As EventArgs) Handles btnBrowseMaps.Click

        'Show dialog.
        folderBrowser.ShowDialog()

        'Update textbox.
        txtMapFolder.Text = folderBrowser.SelectedPath

    End Sub
    Private Sub btnBrowseINI_Click(sender As Object, e As EventArgs) Handles btnBrowseINI.Click

        'Show dialog.
        iniBrowser.ShowDialog()

        'Update textbox.
        txtINI.Text = iniBrowser.FileName

    End Sub
    Private Sub btnBrowseRenderer_Click(sender As Object, e As EventArgs) Handles btnBrowseRenderer.Click

        'Show dialog.
        rendererBrowser.ShowDialog()

        'Update textbox.
        txtRendererPath.Text = rendererBrowser.FileName

    End Sub

    Private Sub btnInsertMaps_Click(sender As Object, e As EventArgs) Handles btnInsertMaps.Click

        'Save paths historty.
        My.Settings.mapFolder = txtMapFolder.Text
        My.Settings.iniPath = txtINI.Text
        My.Settings.exePath = txtRendererPath.Text

        'Start.
        insertMaps()

    End Sub

    Private Sub insertMaps()

        Dim mapFolder As String = txtMapFolder.Text
        Dim iniPath = txtINI.Text
        Dim iniFolder = iniPath.Replace(iniBrowser.SafeFileName, "")
        Dim iniName = iniBrowser.SafeFileName.Replace(".ini", "") 'No extension.

        logInfo("-------------------------------------------------------")
        logInfo("Begin")
        logInfo("-------------------------------------------------------")

        'Parse folder for maps.
        Dim mapObjects As List(Of Map) = getMaps(mapFolder)

        'Sort maps based on max players.
        Dim sortedMaps As List(Of Map) = mapObjects.OrderBy(Function(x) x.MaxPlayers).ToList()

        'Read INI file.
        Dim iniText As String = IO.File.ReadAllText(iniPath)

        'Create specific UTF8 encoder without BOM.
        'TS Client crashes when trying to read files with BOM.
        Dim UTF8_NoBOM As New Text.UTF8Encoding(False)

        'Create backup file.
        Dim iniBackup As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(iniFolder + iniName + "_backup.ini", False, UTF8_NoBOM)
        iniBackup.Write(iniText) 'Save backup.
        iniBackup.Close() 'Close backup.

        Dim counter As Integer = 0 'Init map counter.
        Dim lastEntryIndex As Integer = getLastMapEntry(iniText, counter) 'Get last map entry index.

        'Skip last entry value and get next available position.
        Dim nextFreePosition As Integer = iniText.IndexOf(vbCrLf, lastEntryIndex) + 2

        logInfo(vbNewLine, False)

        'Process sorted maps.
        For Each map In sortedMaps

            Dim entry As String = "MAPS\" + map.GameModes.ToUpper + "\"
            Dim prettyName As String = map.filename.ToUpper()

            ''Remove parenthesis string, if any.
            'Dim parStart As Integer = map.filename.IndexOf("(")
            'Dim parEnd As Integer = map.filename.IndexOf(")")

            'If parStart > -1 And parEnd > -1 Then
            '    entry = map.filename.Remove(parStart, parEnd - parStart + 1).ToUpper
            'ElseIf parStart > -1 Then
            '    entry += prettyName.Replace("(", "_")
            'ElseIf parEnd > -1 Then
            '    entry += prettyName.Replace(")", "_")
            'Else
            '    entry += prettyName
            'End If

            entry += prettyName

            Dim newEntry As String = counter.ToString + "=" + entry + vbCrLf 'Entry to be added.

            'Check if map is already set in the INI file, if so skip.
            If iniText.Contains(entry) Then
                logInfo("-- " + map.filename + " -> OK")
            Else
                'Write new entry below the last entry.
                iniText = iniText.Insert(nextFreePosition, newEntry)

                'Append map parameters to end of file.
                iniText = iniText.Insert(iniText.LastIndexOf(vbCrLf),
                    vbCrLf + vbCrLf + "[" + entry + "]" + vbCrLf +
                    "CD=" + map.CD + vbCrLf +
                    "MinPlayers=" + map.MinPlayers + vbCrLf +
                    "MaxPlayers=" + map.MaxPlayers + vbCrLf +
                    "Description=" + map.Description + vbCrLf +
                    "Author=" + map.Author + vbCrLf +
                    "EnforceMaxPlayers=" + map.EnforceMaxPlayers + vbCrLf +
                    "Size=" + map.Size + vbCrLf +
                    "LocalSize=" + map.LocalSize + vbCrLf +
                    "PreviewSize=" + map.PreviewSize + vbCrLf +
                    "GameModes=" + map.GameModes + vbCrLf +
                    String.Join(vbCrLf, map.Waypoints, 0, CInt(map.MaxPlayers)))

                'Set new available position.
                nextFreePosition += newEntry.Count

                'Log entry.
                logInfo("-- " + map.filename + " -> Inserted")

                counter += 1
            End If
        Next

        logInfo(vbNewLine, False)
        logInfo("Saving changes...")
        logInfo(vbNewLine, False)

        'Open ini file.
        Dim ini As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(iniPath, False, UTF8_NoBOM)

        ini.Write(iniText) 'Replace old INI file with new one.
        ini.Close()

        logInfo("-------------------------------------------------------")
        logInfo("Finished")
        logInfo("-------------------------------------------------------")

    End Sub

    Private Function getLastMapEntry(ByVal iniText As String, ByRef counter As Integer) As Integer

        Dim lastEntryIndex As Integer = -1 'Last entry index.

        'Check if any entry exists. If so, continue until we find the next available entry number.
        While 1

            'Find entry, if any.
            Dim tempIndex As Integer = iniText.IndexOf(counter.ToString + "=")

            If tempIndex >= 0 Then
                lastEntryIndex = tempIndex 'Save index.
                counter += 1 'Next entry number to check.
            Else
                Exit While
            End If
        End While

        Return lastEntryIndex

    End Function

    Private Function getMaps(ByVal mapPath As String) As List(Of Map)

        Dim mapObjects As New List(Of Map) 'Map objects are saved here.

        'Get ".map" and ".mpr" files.
        Dim maps As List(Of String) = IO.Directory.GetFiles(mapPath, "*.map", IO.SearchOption.AllDirectories).ToList
        maps.AddRange(IO.Directory.GetFiles(mapPath, "*.mpr", IO.SearchOption.AllDirectories).ToList)

        'Process map files.
        For Each mapName In maps

            'Get map name index.
            Dim mapNameStart As Integer = mapName.LastIndexOf("\") + 1

            'Replace underscores with spaces.
            Dim prettyMapName As String = mapName.Substring(mapNameStart).Replace("_", " ")

            'Get file type index.
            Dim type As Integer = prettyMapName.IndexOf(".map")
            If type < 0 Then type = prettyMapName.IndexOf(".mpr")

            'Remove map file extension.
            If type >= 0 Then
                prettyMapName = prettyMapName.ToLower.Remove(type)
            Else
                type = prettyMapName.Length - 1
            End If

            'Read map.
            Dim mapText As String = IO.File.ReadAllText(mapName)

            'Create map object.
            '-----------------------------------------------------------------------------------------------
            Dim map As New Map

            map.filename(prettyMapName.Replace(" ", "_").Replace("-", "_"))
            map.fullname(mapName)

            map.CD("0,1,2")
            map.MinPlayers(getMapParamValue("MinPlayer", mapText))
            map.MaxPlayers(getMapParamValue("MaxPlayer", mapText))
            map.Name(getMapParamValue("Name", mapText.Substring(mapText.IndexOf("[Basic]")))) 'Not used for now.
            map.Description("[" + map.MaxPlayers + "] " + StrConv(prettyMapName, VbStrConv.ProperCase)) 'Convert first letter of each word to uppercase.

            'We don't know the author. User must fill it himself.
            map.Author("Unknown")

            map.EnforceMaxPlayers(getMapParamValue("EnforceMaxPlayers", mapText, "False"))
            map.Size(getMapParamValue("Size", mapText.Substring(mapText.IndexOf("[Map]"))))
            map.LocalSize(getMapParamValue("LocalSize", mapText))

            Dim image As Bitmap
            Try

                image = Bitmap.FromFile(mapName.Substring(0, mapNameStart + type) + ".png") 'Get ".png" file.
                map.PreviewSize(image.Width.ToString + "," + image.Height.ToString)
                image.Dispose()

            Catch ex As IO.FileNotFoundException

                Try
                    image = Bitmap.FromFile(mapName.Substring(0, mapNameStart + type) + ".jpg") 'Try to get ".jpg" file.
                    map.PreviewSize(image.Width.ToString + "," + image.Height.ToString)
                    image.Dispose()

                Catch exc As IO.FileNotFoundException

                    'If preview not found, act accordingly.
                    If Not useRendererBox.Checked Then
                        logError("Preview not found for " + mapName)
                        Continue For
                    Else
                        logInfo("Preview not found for " + mapName + ". Generating...")

                        Dim customOutput() As String = {"", ""}
                        renderMap(mapName, customOutput) 'Render map and create preview on the spot.

                        If customOutput(0).Equals("") Then
                            logError("Preview not generated for " + mapName)
                            Continue For
                        End If

                        'Get preview.
                        Try
                            Dim imagePath As String = mapName.Substring(0, mapNameStart) + map.filename + customOutput(0)
                            image = Bitmap.FromFile(imagePath)

                            Dim newImage = New Bitmap(image, New Size(800, 400)) 'Resize image to TS Client's valid dimensions.
                            image.Dispose() 'Close original file.

                            newImage.Save(imagePath, Imaging.ImageFormat.Png) 'Save image.
                            map.PreviewSize(newImage.Width.ToString + "," + newImage.Height.ToString)

                            newImage.Dispose()

                        Catch excep As IO.FileNotFoundException
                            logError("Preview not found for " + mapName)
                            Continue For
                        Catch excep As Exception
                            logError(ex.Message.ToString)
                            Continue For
                        End Try

                    End If
                Catch exc As Exception
                    logError(ex.Message.ToString)
                    Continue For
                End Try
            Catch ex As Exception
                logError(ex.Message.ToString)
                Continue For
            End Try

            'Set proper game mode depending on residing dir.
            If (mapName.Contains("\Tiberian Sun\")) Then
                map.GameModes("Default")
            ElseIf (mapName.Contains("\Firestorm\")) Then
                map.GameModes("Default")
            ElseIf (mapName.Contains("\Custom\")) Then
                map.GameModes("Custom")
            ElseIf (mapName.Contains("\Fan-made\")) Then
                map.GameModes("Fan-made")
            End If

            map.Waypoints(mapText, AddressOf getMapParamValue)
            '-----------------------------------------------------------------------------------------------

            'Rename map file.
            Dim newName As String = map.filename + ".map"

            If mapName.Substring(0, mapNameStart) + newName <> mapName Then
                My.Computer.FileSystem.RenameFile(mapName, newName)
            End If

            mapObjects.Add(map)
        Next

        'Log info.
        logInfo("-------------------------------------------------------")
        logInfo("Found " + mapObjects.Count.ToString + " maps")
        logInfo("-------------------------------------------------------")

        Return mapObjects

    End Function
    Private Function getMapParamValue(ByVal parameter As String, ByVal mapText As String, Optional ByVal default_value As String = "") As String

        Dim paramValue As String
        Dim iniValue As String = parameter + "=" 'Example: "MinPlayers="

        'Find index of parameter.
        Dim index As Integer = mapText.IndexOf(iniValue)

        If index >= 0 Then

            paramValue = mapText.Substring(index + iniValue.Count) 'Skip parameter name and get actual value.
            paramValue = paramValue.Substring(0, paramValue.IndexOf(vbLf)).Trim() 'Isolate value by finding first newline and stopping there.

        Else
            Return default_value
        End If

        Return paramValue

    End Function

    Private Sub logError(ByVal text As String, Optional newline As Boolean = True)

        formatText(Color.Red)

        If newline Then
            text += vbNewLine
        End If

        txtLog.AppendText("Error: " + text)
        txtLog.ScrollToCaret()

    End Sub
    Private Sub logInfo(ByVal text As String, Optional newline As Boolean = True)

        If newline Then
            text += vbNewLine
        End If

        txtLog.AppendText(text)
        txtLog.ScrollToCaret()

    End Sub
    Private Sub formatText(ByVal clr As Color, Optional ByVal style As FontStyle = FontStyle.Regular)
        With txtLog
            .Select(.TextLength, 0)
            .SelectionFont = New Font(.SelectionFont, style)
            .SelectionColor = clr
        End With
    End Sub

    Private Sub renderMap(ByVal mapName As String, ByRef output() As String)

        'Get renderer ".exe" file.
        Dim renderer As New ProcessStartInfo()
        renderer.FileName = txtRendererPath.Text
        renderer.WorkingDirectory = renderer.FileName.Substring(0, renderer.FileName.LastIndexOf("\"))

        Try
            'Get last used settings from GUI.
            Dim rendererSettings As New ConfigXmlDocument
            rendererSettings.Load(renderer.WorkingDirectory + "\gui_settings.xml")

            renderer.Arguments = "-i """ + mapName + """" 'Add map path parameter.
            ReadRendererXMLSettings(rendererSettings.DocumentElement.FirstChild.FirstChild, renderer.Arguments, mapName, output) 'Read rest of settings for Renderer's GUI XML settings file.

            Process.Start(renderer).WaitForExit(30 * 1000) 'Wait at most 30 seconds to render the map.

        Catch ex As ConfigurationErrorsException

            logError(ex.BareMessage)
            logError(ex.Filename)

            'If an error occured while trying to read external settings, use default ones.
            renderer.Arguments = "-i """ + mapName + """ -p -M ""modconfig.xml"""

            Process.Start(renderer).WaitForExit(30 * 1000) 'Wait at most 30 seconds to render the map.

        Catch ex As Exception
            logError(ex.Message)
        End Try

    End Sub
    Private Sub ReadRendererXMLSettings(ByVal config As XmlNode, ByRef settings As String, ByVal mapName As String, ByRef info() As String)

        Dim nodeList As New List(Of XmlNode)

        'Get all nodes and save them to a list where we can search them.
        For Each node In config.SelectNodes("//setting")
            nodeList.Add(node)
        Next

        'PNG option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("outputpng")).InnerXml.Equals("True") Then

            settings += " -p"
            info(0) = ".png" 'We need to know this.

            'Add compression.
            Dim compression As String = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("outputpngq")).InnerXml

            If Not compression.Equals("") Then
                settings += " -c " + compression
            End If
        End If

        'Get file type index.
        Dim type As Integer = mapName.IndexOf(".map")
        If type < 0 Then type = mapName.IndexOf(".mpr")

        'Automatic/custom output filename option.

        'OVERRIDE IT SINCE THE FILENAMES OF THE MAP AND THE PREVIEW MUST BE THE SAME.

        'If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("outputauto")).InnerXml.Equals("False") Then

        '    Dim custom As String = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("customfilename")).InnerXml

        '    If Not custom.Equals("") Then
        '        settings += " -o """ + custom + """"
        '        info(1) = custom 'Save output name for later.
        '    End If
        'End If

        settings += " -o """ + mapName.Remove(type) + """"

        'JPG option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("outputjpg")).InnerXml.ToString.Equals("True") Then

            settings += " -j"
            info(0) = ".jpg" 'We need to know this.

            'Add quality.
            Dim quality As String = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("outputjpgq")).InnerXml

            If Not quality.Equals("") Then
                settings += " -q " + quality
            End If
        End If

        'Special mod config option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("modconfig")).InnerXml.Equals("True") Then

            Dim modFile As String = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("modconfigfile")).InnerXml

            If Not modFile.Equals("") Then
                settings += " -M """ + modFile + """"
            End If
        Else

            Dim mixDir As String = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("mixdir")).InnerXml

            If Not mixDir.Equals("") Then
                settings += " -m """ + mixDir + """"
            End If
        End If

        'Engine rules option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("engineauto")).InnerXml.Equals("False") Then

            Dim engRules As String = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("engineyr")).InnerXml

            If engRules.Equals("False") Then
                engRules = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("enginera2")).InnerXml

                If engRules.Equals("False") Then
                    engRules = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("enginets")).InnerXml

                    If engRules.Equals("False") Then
                        settings += " -T" 'FS
                    Else
                        settings += " -t" 'TS
                    End If
                Else
                    settings += " -y" 'RA2
                End If
            Else
                settings += " -Y" 'YR
            End If

        End If

        'Emphasize ore/gems option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("emphore")).InnerXml.Equals("True") Then
            settings += " -r"
        End If

        'Squared start positions option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("squaredpos")).InnerXml.Equals("True") Then
            settings += " -S"
        End If

        'Tiled start positions option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("tiledpos")).InnerXml.Equals("True") Then
            settings += " -s"
        End If

        'Size mode option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("autosize")).InnerXml.Equals("False") Then
            If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("localsize")).InnerXml.Equals("True") Then
                settings += " -f"
            ElseIf nodeList.Find(Function(x) x.Attributes("name").Value.Equals("fullsize")).InnerXml.Equals("True") Then
                settings += " -F"
            End If
        End If

        'Thumbnail injection option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("injectthumb")).InnerXml.Equals("True") Then
            If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("omitsquarespreview")).InnerXml.Equals("True") Then
                settings += " -K"
            Else
                settings += " -k"
            End If
        End If

        'Output thumbnail option.
        If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("outputthumb")).InnerXml.Equals("True") Then

            settings += " -z"

            If nodeList.Find(Function(x) x.Attributes("name").Value.Equals("thumbpreserveaspect")).InnerXml.Equals("True") Then
                settings += " +"
            Else
                settings += " "
            End If

            'Get thumbnail dimensions.
            Dim dimensions As String = nodeList.Find(Function(x) x.Attributes("name").Value.Equals("thumbdimensions")).InnerXml

            'Markers type option is not currently saved in the XML file (v2.0.8.8713.0).
            Dim markers As String = "bittah" 'config.Item("XXXX").Value.ToString

            settings += "(" + dimensions + ")"
            settings += " --preview-markers-" + markers

        End If

        'Voxel rendering mode option is apparently not used (v2.0.8.8713.0).
        'Maybe the renderer itself uses a default option and just doesn't get it from the GUI.
        '......

    End Sub

    Private Sub useRendererBox_CheckedChanged(sender As Object, e As EventArgs) Handles useRendererBox.CheckedChanged

        If useRendererBox.Checked Then
            txtRendererPath.Enabled = True
            btnBrowseRenderer.Enabled = True
        Else
            txtRendererPath.Enabled = False
            btnBrowseRenderer.Enabled = False
        End If

    End Sub

End Class