<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TermSourceTextBoxControl
    'Inherits System.Windows.Forms.UserControl
    Inherits captainalm.FlashCardMaker.TermSourceBaseControl

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
        Me.TextboxInternal = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'TextboxInternal
        '
        Me.TextboxInternal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TextboxInternal.Location = New System.Drawing.Point(0, 0)
        Me.TextboxInternal.MaxLength = 2147483646
        Me.TextboxInternal.Multiline = True
        Me.TextboxInternal.Name = "TextboxInternal"
        Me.TextboxInternal.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextboxInternal.Size = New System.Drawing.Size(0, 0)
        Me.TextboxInternal.TabIndex = 0
        Me.TextboxInternal.WordWrap = False
        '
        'TermSourceTextBoxControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TextboxInternal)
        Me.Name = "TermSourceTextBoxControl"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextboxInternal As System.Windows.Forms.TextBox

End Class
