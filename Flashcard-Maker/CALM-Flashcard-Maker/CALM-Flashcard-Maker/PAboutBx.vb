Class PAboutBx
    Implements IEventParser

    Public Function Parse(ev As WorkerEvent) As Boolean Implements IEventParser.Parse
        Dim toret As Boolean = False
        If canCastForm(Of AboutBx)(ev.FromForm) Then
            Dim frm As AboutBx = castForm(Of AboutBx)(ev.FromForm)
            If ev.FromControl Is frm.OKButton Then
                If ev.EventType = EventType.Click Then
                    frm.Invoke(Sub() frm.Close())
                    toret = False
                End If
            End If
        End If
        Return False
    End Function

    Private Function castForm(Of t As Form)(f As Form) As t
        Try
            Dim nf As t = f
            Return nf
        Catch ex As InvalidCastException
            Return Nothing
        End Try
    End Function

    Private Function canCastForm(Of t As Form)(f As Form) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function

    Private Function castEd(Of t As EventArgs)(f As EventArgs) As t
        Try
            Dim nf As t = f
            Return nf
        Catch ex As InvalidCastException
            Return Nothing
        End Try
    End Function

    Private Function canCastEd(Of t As EventArgs)(f As EventArgs) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function
End Class
