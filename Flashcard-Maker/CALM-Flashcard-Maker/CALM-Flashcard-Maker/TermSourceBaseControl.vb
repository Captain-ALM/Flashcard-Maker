Public Class TermSourceBaseControl
    Protected _col As Integer = 0
    Protected _row As Integer = 0
    Public Event TermModified(sender As Object, e As TermSourceControlEventArgs)
    Public Sub New()
        InitializeComponent()
        Me.Enabled = False
    End Sub
    Public Sub New(c As Integer, r As Integer)
        InitializeComponent()
        _col = c
        _row = r
    End Sub
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
    Protected Overridable Sub OnTermModified(sender As Object, e As TermSourceControlEventArgs)
        RaiseEvent TermModified(sender, e)
    End Sub
    Public Overridable Property Row As Integer
        Get
            Return _row
        End Get
        Set(value As Integer)
            _row = value
        End Set
    End Property
    Public Overridable Property Column As Integer
        Get
            Return _col
        End Get
        Set(value As Integer)
            _col = value
        End Set
    End Property
End Class

Public Class TermSourceControlEventArgs
    Inherits EventArgs
    Protected _row As Integer = 0
    Protected _col As Integer = 0
    Protected _ht As Boolean = False
    Protected _term As TermSource = Nothing
    Public Sub New(col As Integer, row As Integer)
        _row = row
        _col = col
        _ht = False
    End Sub
    Public Sub New(col As Integer, row As Integer, t As TermSource)
        _row = row
        _col = col
        _term = t
        _ht = True
    End Sub
    Public ReadOnly Property Row As Integer
        Get
            Return _row
        End Get
    End Property
    Public ReadOnly Property Column As Integer
        Get
            Return _col
        End Get
    End Property
    Public ReadOnly Property HasTerm As Boolean
        Get
            Return _ht
        End Get
    End Property
    Public ReadOnly Property Term As TermSource
        Get
            Return _term
        End Get
    End Property
End Class