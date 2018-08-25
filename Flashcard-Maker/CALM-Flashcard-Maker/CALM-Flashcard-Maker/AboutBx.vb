Public NotInheritable Class AboutBx

    Private Sub AboutBx_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        addEvent(New WorkerEvent(Me, EventType.Load, e))
    End Sub

    Public Sub whenLoad()
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
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        addEvent(New WorkerEvent(Me, OKButton, EventType.Click, e))
    End Sub

    Private Sub AboutBx_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        addEvent(New WorkerEvent(Me, EventType.Closed, e))
    End Sub

    Public Sub whenClosed()
        Me.OKButton.Enabled = True
    End Sub
End Class
