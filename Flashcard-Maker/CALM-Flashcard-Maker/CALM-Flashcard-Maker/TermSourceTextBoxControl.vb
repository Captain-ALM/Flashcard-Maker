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
    Public Overrides ReadOnly Property InternalControl As Control
        Get
            Return TextboxInternal
        End Get
    End Property
    Public Overrides Function Duplicate() As TermSourceBaseControl
        Dim sc As TermSourceTextBoxControl = New TermSourceTextBoxControl(DeepCopyHelper.deepCopy(Of TextTerm)(_term), _col, _row) With {.SelectionColor = _scol}
        Return sc
    End Function
End Class
