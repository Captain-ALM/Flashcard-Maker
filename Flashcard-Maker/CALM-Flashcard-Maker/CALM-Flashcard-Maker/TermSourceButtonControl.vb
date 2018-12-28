Public Class TermSourceButtonControl
    Protected _term As TermSource = Nothing
    Public Event ButtonClicked(source As Object, e As TermSourceButtonControlClickEventArgs)
    Public Sub New(t As TermSource, c As Integer, r As Integer)
        MyBase.New(c, r)
        _term = t
        InitializeComponent()
        ButtonInternal.Image = _term.getImage(ButtonInternal.Width, ButtonInternal.Height)
    End Sub
    Public Overrides Sub setTerm(term As TermSource)
        _term = term
        ButtonInternal.Image = _term.getImage(ButtonInternal.Width, ButtonInternal.Height)
    End Sub
    Public Overrides Function getTerm() As TermSource
        Return _term
    End Function
    Public ReadOnly Property Button As Button
        Get
            Return ButtonInternal
        End Get
    End Property
    Private Sub ButtonInternal_Click(sender As Object, e As EventArgs) Handles ButtonInternal.Click
        RaiseEvent ButtonClicked(Me, New TermSourceButtonControlClickEventArgs(Column, Row, _term))
        ButtonInternal.Image = _term.getImage(ButtonInternal.Width, ButtonInternal.Height)
        OnTermModified(Me, New TermSourceControlEventArgs(Column, Row, _term))
    End Sub
    Public Overrides ReadOnly Property InternalControl As Control
        Get
            Return ButtonInternal
        End Get
    End Property
End Class

Public Class TermSourceButtonControlClickEventArgs
    Inherits TermSourceControlEventArgs
    Public Sub New(col As Integer, row As Integer, ByRef t As TermSource)
        MyBase.New(col, row)
        _term = t
        _ht = True
    End Sub
End Class