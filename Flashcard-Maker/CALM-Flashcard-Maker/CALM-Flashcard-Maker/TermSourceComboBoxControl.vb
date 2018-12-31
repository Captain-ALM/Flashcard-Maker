Public Class TermSourceComboBoxControl
    Protected _term As TermSource = Nothing
    Protected _itms As String() = Nothing
    Public Event SelectedIndexChanged(source As Object, e As TermSourceComboBoxControlSelectedIndexChangedEventArgs)
    Public Sub New(items As String(), c As Integer, r As Integer, Optional sindex As Integer = 0)
        MyBase.New(c, r)
        _term = New EmptyTerm()
        InitializeComponent()
        ComboBoxInternal.DropDownStyle = ComboBoxStyle.DropDownList
        ComboBoxInternal.Items.Clear()
        ComboBoxInternal.Items.AddRange(items)
        _itms = items
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
        RaiseEvent SelectedIndexChanged(Me, New TermSourceComboBoxControlSelectedIndexChangedEventArgs(Column, Row, ComboBoxInternal.SelectedIndex))
    End Sub
    Public Overrides ReadOnly Property InternalControl As Control
        Get
            Return ComboBoxInternal
        End Get
    End Property
    Public Overrides Function Duplicate() As TermSourceBaseControl
        Dim sc As TermSourceComboBoxControl = New TermSourceComboBoxControl(DeepCopyHelper.deepCopy(Of String())(_itms), _col, _row) With {.SelectionColor = _scol}
        Return sc
    End Function
End Class

Public Class TermSourceComboBoxControlSelectedIndexChangedEventArgs
    Inherits TermSourceControlEventArgs
    Protected _index As Integer
    Public Sub New(col As Integer, row As Integer, sindex As Integer)
        MyBase.New(col, row)
        sindex = _index
    End Sub
    Public Overridable ReadOnly Property SelectedIndex As Integer
        Get
            Return _index
        End Get
    End Property
End Class


