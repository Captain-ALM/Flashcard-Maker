Public NotInheritable Class AboutBx
    Private ct As Boolean = False

    Private Sub AboutBx_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addEvent(New WorkerEvent(Me, EventType.Load, e))
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        addEvent(New WorkerEvent(Me, OKButton, EventType.Click, e))
    End Sub

    Private Sub AboutBx_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        whenClosed()
        addEvent(New WorkerEvent(Me, EventType.Closed, e))
    End Sub

    Public Sub whenClosed()
        Me.OKButton.Enabled = True
    End Sub

    Private Sub AboutBx_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not ct Then
            If e.CloseReason = CloseReason.UserClosing Then
                e.Cancel = True
                Me.Hide()
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
            addEvent(New WorkerEvent(Me, EventType.Closing, e))
            ct = True
        End If
    End Sub

#Region "closeOverride"
    Public Shadows Sub Close()
        Dim cea As New FormClosingEventArgs(CloseReason.None, False)
        'Me.OnFormClosing(cea)
        If Not cea.Cancel Then
            Me.Hide()
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub
#End Region

    Private Sub AboutBx_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.DialogResult = Windows.Forms.DialogResult.None
        ct = False
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = "Description: " & vbCrLf & My.Application.Info.Description & vbCrLf & description
        Me.TextBox1.Text = "License: " & vbCrLf & license
        addEvent(Me, EventType.Shown, e)
    End Sub
End Class
