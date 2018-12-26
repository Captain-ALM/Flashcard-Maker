Public Class TermSourceTextBoxControl
    Protected _term As TextTerm = Nothing
    Public Sub New(t As TextTerm, c As Integer, r As Integer)
        MyBase.New(c, r)
        _term = t
        InitializeComponent()
        TextboxInternal.Text = _term.Text
    End Sub
    Public Overrides Sub setTerm(term As TermSource)
        _term = term
        TextboxInternal.Text = _term.Text
    End Sub
    Public Overrides Function getTerm() As TermSource
        Return _term
    End Function
    Private Sub TextboxInternal_TextChanged(sender As Object, e As EventArgs) Handles TextboxInternal.TextChanged
        _term.Text = TextboxInternal.Text
        OnTermModified(Me, New TermSourceControlEventArgs(Column, Row, _term))
    End Sub
    Public ReadOnly Property Textbox As TextBox
        Get
            Return TextboxInternal
        End Get
    End Property
End Class
