Public Class TermSourceButtonControl
    Protected _term As TermSource = Nothing
    Public Event ButtonClicked(ByRef term As TermSource)
    Public Sub New(t As TermSource)
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
        RaiseEvent ButtonClicked(_term)
        ButtonInternal.Image = _term.getImage(ButtonInternal.Width, ButtonInternal.Height)
        OnTermModified()
    End Sub
End Class