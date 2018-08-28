<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GlobalOptions
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GlobalOptions))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.tcgo = New System.Windows.Forms.TabControl()
        Me.TabPageGeneralOptions = New System.Windows.Forms.TabPage()
        Me.TabPageFileAssociations = New System.Windows.Forms.TabPage()
        Me.butok = New System.Windows.Forms.Button()
        Me.butcancel = New System.Windows.Forms.Button()
        Me.TabPageDefaultProjectOptions = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.chkbxesl = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudmin = New System.Windows.Forms.NumericUpDown()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudmax = New System.Windows.Forms.NumericUpDown()
        Me.chkbxetem = New System.Windows.Forms.CheckBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tcgo.SuspendLayout()
        Me.TabPageGeneralOptions.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        CType(Me.nudmin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel4.SuspendLayout()
        CType(Me.nudmax, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.tcgo, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.butok, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.butcancel, 1, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(584, 361)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'tcgo
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.tcgo, 2)
        Me.tcgo.Controls.Add(Me.TabPageGeneralOptions)
        Me.tcgo.Controls.Add(Me.TabPageDefaultProjectOptions)
        Me.tcgo.Controls.Add(Me.TabPageFileAssociations)
        Me.tcgo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcgo.Location = New System.Drawing.Point(3, 3)
        Me.tcgo.Name = "tcgo"
        Me.tcgo.SelectedIndex = 0
        Me.tcgo.Size = New System.Drawing.Size(578, 300)
        Me.tcgo.TabIndex = 0
        '
        'TabPageGeneralOptions
        '
        Me.TabPageGeneralOptions.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPageGeneralOptions.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGeneralOptions.Name = "TabPageGeneralOptions"
        Me.TabPageGeneralOptions.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageGeneralOptions.Size = New System.Drawing.Size(570, 274)
        Me.TabPageGeneralOptions.TabIndex = 0
        Me.TabPageGeneralOptions.Text = "General Options"
        Me.TabPageGeneralOptions.UseVisualStyleBackColor = True
        '
        'TabPageFileAssociations
        '
        Me.TabPageFileAssociations.Location = New System.Drawing.Point(4, 22)
        Me.TabPageFileAssociations.Name = "TabPageFileAssociations"
        Me.TabPageFileAssociations.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageFileAssociations.Size = New System.Drawing.Size(570, 274)
        Me.TabPageFileAssociations.TabIndex = 1
        Me.TabPageFileAssociations.Text = "File Associations"
        Me.TabPageFileAssociations.UseVisualStyleBackColor = True
        '
        'butok
        '
        Me.butok.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butok.Location = New System.Drawing.Point(3, 309)
        Me.butok.Name = "butok"
        Me.butok.Size = New System.Drawing.Size(286, 49)
        Me.butok.TabIndex = 1
        Me.butok.Text = "Ok"
        Me.butok.UseVisualStyleBackColor = True
        '
        'butcancel
        '
        Me.butcancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.butcancel.Location = New System.Drawing.Point(295, 309)
        Me.butcancel.Name = "butcancel"
        Me.butcancel.Size = New System.Drawing.Size(286, 49)
        Me.butcancel.TabIndex = 2
        Me.butcancel.Text = "Cancel"
        Me.butcancel.UseVisualStyleBackColor = True
        '
        'TabPageDefaultProjectOptions
        '
        Me.TabPageDefaultProjectOptions.Location = New System.Drawing.Point(4, 22)
        Me.TabPageDefaultProjectOptions.Name = "TabPageDefaultProjectOptions"
        Me.TabPageDefaultProjectOptions.Size = New System.Drawing.Size(570, 274)
        Me.TabPageDefaultProjectOptions.TabIndex = 2
        Me.TabPageDefaultProjectOptions.Text = "Default Project Options"
        Me.TabPageDefaultProjectOptions.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.chkbxetem, 0, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel4, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.chkbxesl, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel3, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(564, 268)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'chkbxesl
        '
        Me.chkbxesl.AutoSize = True
        Me.chkbxesl.Checked = True
        Me.chkbxesl.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxesl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkbxesl.Location = New System.Drawing.Point(3, 3)
        Me.chkbxesl.Name = "chkbxesl"
        Me.chkbxesl.Size = New System.Drawing.Size(276, 61)
        Me.chkbxesl.TabIndex = 0
        Me.chkbxesl.Text = "Enable Size Limit"
        Me.chkbxesl.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 2
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.nudmin, 1, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 70)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(276, 61)
        Me.TableLayoutPanel3.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(132, 61)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Minumum Size:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'nudmin
        '
        Me.nudmin.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudmin.Location = New System.Drawing.Point(141, 20)
        Me.nudmin.Maximum = New Decimal(New Integer() {1638, 0, 0, 0})
        Me.nudmin.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudmin.Name = "nudmin"
        Me.nudmin.Size = New System.Drawing.Size(132, 20)
        Me.nudmin.TabIndex = 1
        Me.nudmin.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.Label2, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.nudmax, 1, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(285, 70)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(276, 61)
        Me.TableLayoutPanel4.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(132, 61)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Maximum Size:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'nudmax
        '
        Me.nudmax.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.nudmax.Location = New System.Drawing.Point(141, 20)
        Me.nudmax.Maximum = New Decimal(New Integer() {1638, 0, 0, 0})
        Me.nudmax.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudmax.Name = "nudmax"
        Me.nudmax.Size = New System.Drawing.Size(132, 20)
        Me.nudmax.TabIndex = 1
        Me.nudmax.Value = New Decimal(New Integer() {1638, 0, 0, 0})
        '
        'chkbxetem
        '
        Me.chkbxetem.AutoSize = True
        Me.chkbxetem.Checked = True
        Me.chkbxetem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxetem.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkbxetem.Location = New System.Drawing.Point(3, 137)
        Me.chkbxetem.Name = "chkbxetem"
        Me.chkbxetem.Size = New System.Drawing.Size(276, 61)
        Me.chkbxetem.TabIndex = 3
        Me.chkbxetem.Text = "Enable Thread Error Messages"
        Me.chkbxetem.UseVisualStyleBackColor = True
        '
        'GlobalOptions
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 361)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(600, 400)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "GlobalOptions"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Global Options"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.tcgo.ResumeLayout(False)
        Me.TabPageGeneralOptions.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        CType(Me.nudmin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        CType(Me.nudmax, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tcgo As System.Windows.Forms.TabControl
    Friend WithEvents TabPageGeneralOptions As System.Windows.Forms.TabPage
    Friend WithEvents TabPageFileAssociations As System.Windows.Forms.TabPage
    Friend WithEvents butok As System.Windows.Forms.Button
    Friend WithEvents butcancel As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TabPageDefaultProjectOptions As System.Windows.Forms.TabPage
    Friend WithEvents chkbxesl As System.Windows.Forms.CheckBox
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudmin As System.Windows.Forms.NumericUpDown
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudmax As System.Windows.Forms.NumericUpDown
    Friend WithEvents chkbxetem As System.Windows.Forms.CheckBox
End Class
