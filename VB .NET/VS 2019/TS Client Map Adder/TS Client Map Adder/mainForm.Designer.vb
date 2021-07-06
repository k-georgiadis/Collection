<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class mainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(mainForm))
        Me.txtMapFolder = New System.Windows.Forms.TextBox()
        Me.folderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.mapslbl = New System.Windows.Forms.Label()
        Me.inilbl = New System.Windows.Forms.Label()
        Me.txtINI = New System.Windows.Forms.TextBox()
        Me.groupOptions = New System.Windows.Forms.GroupBox()
        Me.useRendererBox = New System.Windows.Forms.CheckBox()
        Me.btnBrowseRenderer = New System.Windows.Forms.Button()
        Me.rendererlbl = New System.Windows.Forms.Label()
        Me.txtRendererPath = New System.Windows.Forms.TextBox()
        Me.btnBrowseINI = New System.Windows.Forms.Button()
        Me.btnBrowseMaps = New System.Windows.Forms.Button()
        Me.txtLog = New System.Windows.Forms.RichTextBox()
        Me.btnInsertMaps = New System.Windows.Forms.Button()
        Me.iniBrowser = New System.Windows.Forms.OpenFileDialog()
        Me.btnBrowseMapsToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.rendererBrowser = New System.Windows.Forms.OpenFileDialog()
        Me.groupOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtMapFolder
        '
        Me.txtMapFolder.Location = New System.Drawing.Point(77, 26)
        Me.txtMapFolder.Name = "txtMapFolder"
        Me.txtMapFolder.Size = New System.Drawing.Size(248, 20)
        Me.txtMapFolder.TabIndex = 0
        '
        'mapslbl
        '
        Me.mapslbl.AutoSize = True
        Me.mapslbl.Location = New System.Drawing.Point(6, 29)
        Me.mapslbl.Name = "mapslbl"
        Me.mapslbl.Size = New System.Drawing.Size(36, 13)
        Me.mapslbl.TabIndex = 1
        Me.mapslbl.Text = "Maps:"
        '
        'inilbl
        '
        Me.inilbl.AutoSize = True
        Me.inilbl.Location = New System.Drawing.Point(6, 56)
        Me.inilbl.Name = "inilbl"
        Me.inilbl.Size = New System.Drawing.Size(65, 13)
        Me.inilbl.TabIndex = 3
        Me.inilbl.Text = "MPMaps.ini:"
        '
        'txtINI
        '
        Me.txtINI.Location = New System.Drawing.Point(77, 53)
        Me.txtINI.Name = "txtINI"
        Me.txtINI.Size = New System.Drawing.Size(248, 20)
        Me.txtINI.TabIndex = 2
        '
        'groupOptions
        '
        Me.groupOptions.Controls.Add(Me.useRendererBox)
        Me.groupOptions.Controls.Add(Me.btnBrowseRenderer)
        Me.groupOptions.Controls.Add(Me.rendererlbl)
        Me.groupOptions.Controls.Add(Me.txtRendererPath)
        Me.groupOptions.Controls.Add(Me.btnBrowseINI)
        Me.groupOptions.Controls.Add(Me.btnBrowseMaps)
        Me.groupOptions.Controls.Add(Me.mapslbl)
        Me.groupOptions.Controls.Add(Me.inilbl)
        Me.groupOptions.Controls.Add(Me.txtMapFolder)
        Me.groupOptions.Controls.Add(Me.txtINI)
        Me.groupOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.groupOptions.Location = New System.Drawing.Point(12, 12)
        Me.groupOptions.Name = "groupOptions"
        Me.groupOptions.Size = New System.Drawing.Size(420, 131)
        Me.groupOptions.TabIndex = 4
        Me.groupOptions.TabStop = False
        Me.groupOptions.Text = "TS Client Options"
        '
        'useRendererBox
        '
        Me.useRendererBox.AutoSize = True
        Me.useRendererBox.Location = New System.Drawing.Point(9, 106)
        Me.useRendererBox.Name = "useRendererBox"
        Me.useRendererBox.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.useRendererBox.Size = New System.Drawing.Size(188, 17)
        Me.useRendererBox.TabIndex = 9
        Me.useRendererBox.Text = "Render new previews if not found."
        Me.useRendererBox.UseVisualStyleBackColor = True
        '
        'btnBrowseRenderer
        '
        Me.btnBrowseRenderer.Enabled = False
        Me.btnBrowseRenderer.Location = New System.Drawing.Point(331, 78)
        Me.btnBrowseRenderer.Name = "btnBrowseRenderer"
        Me.btnBrowseRenderer.Size = New System.Drawing.Size(83, 23)
        Me.btnBrowseRenderer.TabIndex = 8
        Me.btnBrowseRenderer.Text = "Browse"
        Me.btnBrowseRenderer.UseVisualStyleBackColor = True
        '
        'rendererlbl
        '
        Me.rendererlbl.AutoSize = True
        Me.rendererlbl.Location = New System.Drawing.Point(6, 83)
        Me.rendererlbl.Name = "rendererlbl"
        Me.rendererlbl.Size = New System.Drawing.Size(54, 13)
        Me.rendererlbl.TabIndex = 7
        Me.rendererlbl.Text = "Renderer:"
        '
        'txtRendererPath
        '
        Me.txtRendererPath.Enabled = False
        Me.txtRendererPath.Location = New System.Drawing.Point(77, 80)
        Me.txtRendererPath.Name = "txtRendererPath"
        Me.txtRendererPath.Size = New System.Drawing.Size(248, 20)
        Me.txtRendererPath.TabIndex = 6
        '
        'btnBrowseINI
        '
        Me.btnBrowseINI.Location = New System.Drawing.Point(331, 51)
        Me.btnBrowseINI.Name = "btnBrowseINI"
        Me.btnBrowseINI.Size = New System.Drawing.Size(83, 23)
        Me.btnBrowseINI.TabIndex = 5
        Me.btnBrowseINI.Text = "Browse"
        Me.btnBrowseINI.UseVisualStyleBackColor = True
        '
        'btnBrowseMaps
        '
        Me.btnBrowseMaps.Location = New System.Drawing.Point(331, 24)
        Me.btnBrowseMaps.Name = "btnBrowseMaps"
        Me.btnBrowseMaps.Size = New System.Drawing.Size(83, 23)
        Me.btnBrowseMaps.TabIndex = 4
        Me.btnBrowseMaps.Text = "Browse"
        Me.btnBrowseMaps.UseVisualStyleBackColor = True
        '
        'txtLog
        '
        Me.txtLog.Location = New System.Drawing.Point(12, 149)
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ReadOnly = True
        Me.txtLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(420, 219)
        Me.txtLog.TabIndex = 5
        Me.txtLog.Text = ""
        '
        'btnInsertMaps
        '
        Me.btnInsertMaps.Location = New System.Drawing.Point(12, 374)
        Me.btnInsertMaps.Name = "btnInsertMaps"
        Me.btnInsertMaps.Size = New System.Drawing.Size(420, 25)
        Me.btnInsertMaps.TabIndex = 6
        Me.btnInsertMaps.Text = "Insert Maps"
        Me.btnInsertMaps.UseVisualStyleBackColor = True
        '
        'iniBrowser
        '
        Me.iniBrowser.FileName = "MPMaps.ini"
        '
        'rendererBrowser
        '
        Me.rendererBrowser.DefaultExt = "exe"
        Me.rendererBrowser.FileName = "CNCMaps.Renderer.exe"
        Me.rendererBrowser.Filter = "Executable files (*.exe)|"
        '
        'mainForm
        '
        Me.AcceptButton = Me.btnInsertMaps
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(438, 403)
        Me.Controls.Add(Me.btnInsertMaps)
        Me.Controls.Add(Me.txtLog)
        Me.Controls.Add(Me.groupOptions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "mainForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Tiberian Sun Client - Map Adder"
        Me.groupOptions.ResumeLayout(False)
        Me.groupOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents txtMapFolder As System.Windows.Forms.TextBox
    Friend WithEvents folderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents mapslbl As System.Windows.Forms.Label
    Friend WithEvents inilbl As System.Windows.Forms.Label
    Friend WithEvents txtINI As System.Windows.Forms.TextBox
    Friend WithEvents groupOptions As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowseINI As System.Windows.Forms.Button
    Friend WithEvents btnBrowseMaps As System.Windows.Forms.Button
    Friend WithEvents txtLog As System.Windows.Forms.RichTextBox
    Friend WithEvents btnInsertMaps As System.Windows.Forms.Button
    Friend WithEvents iniBrowser As System.Windows.Forms.OpenFileDialog
    Friend WithEvents btnBrowseMapsToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents useRendererBox As System.Windows.Forms.CheckBox
    Friend WithEvents btnBrowseRenderer As System.Windows.Forms.Button
    Friend WithEvents rendererlbl As System.Windows.Forms.Label
    Friend WithEvents txtRendererPath As System.Windows.Forms.TextBox
    Friend WithEvents rendererBrowser As System.Windows.Forms.OpenFileDialog
End Class
