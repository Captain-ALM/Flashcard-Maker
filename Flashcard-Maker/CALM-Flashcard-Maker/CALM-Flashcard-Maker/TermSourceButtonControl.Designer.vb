<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TermSourceButtonControl
    Inherits captainalm.FlashCardMaker.TermSourceBaseControl
    'Inherits System.Windows.Forms.UserControl

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
        Me.ButtonInternal = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ButtonInternal
        '
        Me.ButtonInternal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ButtonInternal.Location = New System.Drawing.Point(0, 0)
        Me.ButtonInternal.Name = "ButtonInternal"
        Me.ButtonInternal.Size = New System.Drawing.Size(0, 0)
        Me.ButtonInternal.TabIndex = 0
        Me.ButtonInternal.UseVisualStyleBackColor = True
        '
        'TermSourceButtonControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ButtonInternal)
        Me.Name = "TermSourceButtonControl"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonInternal As System.Windows.Forms.Button

End Class
