Public Class TermSourceComboBoxControl
    Protected _term As TermSource = Nothing
    Public Event SelectedIndexChanged(index As Integer)
    Public Sub New(items As String(), Optional sindex As Integer = 0)
        _term = New EmptyTerm()
        InitializeComponent()
        ComboBoxInternal.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBoxInternal.Items.Clear()
        ComboBoxInternal.Items.AddRange(items)
        If sindex > 0 And sindex < ComboBoxInternal.Items.Count Then ComboBoxInternal.SelectedIndex = sindex
    End Sub
    Public Overrides Sub setTerm(term As TermSource)
        _term = term
    End Sub
    Public Overrides Function getTerm() As TermSource
        Return _term
    End Function
    Public ReadOnly Property ComboBox As ComboBox
        Get
            Return ComboBoxInternal
        End Get
    End Property
    Private Sub ComboBoxInternal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxInternal.SelectedIndexChanged
        RaiseEvent SelectedIndexChanged(ComboBoxInternal.SelectedIndex)
    End Sub
End Class


