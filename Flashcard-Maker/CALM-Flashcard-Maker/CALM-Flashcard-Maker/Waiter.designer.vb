'
' Created by SharpDevelop.
' User: Alfred
' Date: 20/08/2018
' Time: 17:40
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class Waiter
	Inherits System.Windows.Forms.Form
	
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	Private components As System.ComponentModel.IContainer
	
	''' <summary>
	''' Disposes resources used by the form.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub
	
	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Waiter))
        Me.lbltxt = New System.Windows.Forms.Label()
        Me.pgb = New System.Windows.Forms.ProgressBar()
        Me.BUTSTOP = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lbltxt
        '
        Me.lbltxt.Location = New System.Drawing.Point(1, 1)
        Me.lbltxt.Name = "lbltxt"
        Me.lbltxt.Size = New System.Drawing.Size(382, 25)
        Me.lbltxt.TabIndex = 0
        Me.lbltxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pgb
        '
        Me.pgb.Location = New System.Drawing.Point(1, 29)
        Me.pgb.Name = "pgb"
        Me.pgb.Size = New System.Drawing.Size(343, 30)
        Me.pgb.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pgb.TabIndex = 1
        '
        'BUTSTOP
        '
        Me.BUTSTOP.Location = New System.Drawing.Point(350, 29)
        Me.BUTSTOP.Name = "BUTSTOP"
        Me.BUTSTOP.Size = New System.Drawing.Size(33, 30)
        Me.BUTSTOP.TabIndex = 2
        Me.BUTSTOP.Text = "X"
        Me.BUTSTOP.UseVisualStyleBackColor = True
        '
        'Waiter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 61)
        Me.ControlBox = False
        Me.Controls.Add(Me.BUTSTOP)
        Me.Controls.Add(Me.pgb)
        Me.Controls.Add(Me.lbltxt)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(400, 100)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 100)
        Me.Name = "Waiter"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Please Wait..."
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents pgb As System.Windows.Forms.ProgressBar
    Public WithEvents lbltxt As System.Windows.Forms.Label
    Public WithEvents BUTSTOP As System.Windows.Forms.Button
End Class
