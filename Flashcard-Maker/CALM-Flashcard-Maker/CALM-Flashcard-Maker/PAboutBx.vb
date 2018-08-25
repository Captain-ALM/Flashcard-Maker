Class PAboutBx
    Implements IEventParser

    Public Function Parse(ev As WorkerEvent) As Boolean Implements IEventParser.Parse
        Dim toret As Boolean = False
        If canCastForm(Of AboutBx)(ev.FromForm) Then
            Dim frm As AboutBx = castForm(Of AboutBx)(ev.FromForm)
            If ev.FromControl Is frm.OKButton Then
                If ev.EventType = EventType.Click Then
                    ev.FromForm.Invoke(Sub() ev.FromForm.Close())
                    toret = False
                End If
            ElseIf ev.FromControl Is frm Then
                If ev.EventType = EventType.Load Then
                    ev.FromForm.Invoke(Sub() frm.whenLoad())
                    toret = False
                ElseIf ev.EventType = EventType.Closed Then
                    ev.FromForm.Invoke(Sub() frm.whenClosed())
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
End Class
