Public Class TermSourceBaseControl
    Public Event TermModified()
    Public Overridable Function getTerm() As TermSource
        Throw New NotImplementedException()
    End Function
    Public Overridable Sub setTerm(term As TermSource)
        Throw New NotImplementedException()
    End Sub
    Public Overridable Property Term As TermSource
        Get
            Return getTerm()
        End Get
        Set(value As TermSource)
            setTerm(value)
        End Set
    End Property
    Protected Overridable Sub OnTermModified()
        RaiseEvent TermModified()
    End Sub
End Class