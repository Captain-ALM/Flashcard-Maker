<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TermEditorTable
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TableLayoutPanelInternal = New System.Windows.Forms.TableLayoutPanel()
        Me.SuspendLayout()
        '
        'TableLayoutPanelInternal
        '
        Me.TableLayoutPanelInternal.ColumnCount = 2
        Me.TableLayoutPanelInternal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelInternal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelInternal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelInternal.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelInternal.Name = "TableLayoutPanelInternal"
        Me.TableLayoutPanelInternal.RowCount = 1
        Me.TableLayoutPanelInternal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelInternal.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelInternal.Size = New System.Drawing.Size(150, 150)
        Me.TableLayoutPanelInternal.TabIndex = 0
        '
        'TermEditorTable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanelInternal)
        Me.Name = "TermEditorTable"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanelInternal As System.Windows.Forms.TableLayoutPanel

End Class
