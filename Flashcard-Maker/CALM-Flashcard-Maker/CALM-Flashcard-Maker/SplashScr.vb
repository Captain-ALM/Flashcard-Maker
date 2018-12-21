Public NotInheritable Class SplashScr
    Public wasactive As Boolean = False
    'Should not construct externaly
    Sub New()
        Me.SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub SplashScr_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If My.Application.Info.Title <> "" Then
            Me.Text = My.Application.Info.Title
        Else
            Me.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If

        Version.BackColor = Color.FromArgb(150, Color.WhiteSmoke)
        Copyright.BackColor = Color.FromArgb(150, Color.WhiteSmoke)

        Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
        Copyright.Text = My.Application.Info.Copyright

        Me.BackColor = Color.Gainsboro
    End Sub

    Private Sub SplashScr_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        wasactive = True
    End Sub

    Private Sub SplashScr_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        wasactive = False
    End Sub

    Public Sub CloseForm()
        Me.Invoke(Sub() Me.Close())
    End Sub
End Class
