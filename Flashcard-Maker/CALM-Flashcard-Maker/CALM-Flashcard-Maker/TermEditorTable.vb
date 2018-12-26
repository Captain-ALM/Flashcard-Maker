Public Class TermEditorTable
    Private _project As Project = Nothing
    Private cmbxarr As String() = New String() {"", "Text"}
    Private col0 As New List(Of TermSourceBaseControl)
    Private col1 As New List(Of TermSourceBaseControl)
    Private ppr As Integer = 200
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(ByRef project As Project)
        _project = project
        InitializeComponent()
        refreshTerms()
    End Sub
    Public ReadOnly Property Project As Project
        Get
            Return _project
        End Get
    End Property
    Public Sub setProject(ByRef project As Project)
        _project = project
    End Sub
    Public Property PixelsPerRow As Integer
        Get
            Return ppr
        End Get
        Set(value As Integer)
            If value < 1 Then Return
            ppr = value
        End Set
    End Property
    Private Sub unTerms()
        If Not projectCheck() Then Return
        TableLayoutPanelInternal.Controls.Clear()
        TableLayoutPanelInternal.ColumnStyles.Clear()
        TableLayoutPanelInternal.RowStyles.Clear()
        TableLayoutPanelInternal.ColumnCount = 2
        TableLayoutPanelInternal.RowCount = 1
        For Each e As TermSourceBaseControl In col0
            If canCastObject(Of TermSourceComboBoxControl)(e) Then
                Dim ctrl As TermSourceComboBoxControl = castObject(Of TermSourceComboBoxControl)(e)
                RemoveHandler ctrl.SelectedIndexChanged, AddressOf onSIChanged
            End If
            RemoveHandler e.TermModified, AddressOf onTermModified
            e.Dispose()
        Next
        For Each e As TermSourceBaseControl In col1
            If canCastObject(Of TermSourceComboBoxControl)(e) Then
                Dim ctrl As TermSourceComboBoxControl = castObject(Of TermSourceComboBoxControl)(e)
                RemoveHandler ctrl.SelectedIndexChanged, AddressOf onSIChanged
            End If
            RemoveHandler e.TermModified, AddressOf onTermModified
            e.Dispose()
        Next
        col0.Clear()
        col1.Clear()
    End Sub
    Private Sub doTerms()
        If Not projectCheck() Then Return
        TableLayoutPanelInternal.ColumnCount = 2
        While TableLayoutPanelInternal.ColumnStyles.Count < 2
            TableLayoutPanelInternal.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        End While
        TableLayoutPanelInternal.RowCount = _project.dataCount + 1
        While TableLayoutPanelInternal.RowStyles.Count < _project.dataCount + 1
            TableLayoutPanelInternal.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / (_project.dataCount + 1)))
        End While
        Me.MinimumSize = New Size(0, TableLayoutPanelInternal.RowCount * ppr)
        Me.Height = TableLayoutPanelInternal.RowCount * ppr
        Dim cv As Integer = 0
        For cv = 0 To _project.dataCount - 1 Step 1
            Dim s As TermSet(Of TermSource, TermSource) = _project.data(cv)
            If s.Term1.termSourceType = "TextTerm" Then
                Dim ctrl As New TermSourceTextBoxControl(s.Term1, 0, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                col0.Add(ctrl)
            ElseIf s.Term1.termSourceType = "EmptyTerm" Then
                Dim ctrl As New TermSourceComboBoxControl(cmbxarr, 0, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                AddHandler ctrl.SelectedIndexChanged, AddressOf onSIChanged
                col0.Add(ctrl)
            End If
            If s.Term2.termSourceType = "TextTerm" Then
                Dim ctrl As New TermSourceTextBoxControl(s.Term2, 1, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                col1.Add(ctrl)
            ElseIf s.Term2.termSourceType = "EmptyTerm" Then
                Dim ctrl As New TermSourceComboBoxControl(cmbxarr, 1, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                AddHandler ctrl.SelectedIndexChanged, AddressOf onSIChanged
                col1.Add(ctrl)
            End If
        Next cv
        Dim ctrl0 As New TermSourceComboBoxControl(cmbxarr, 0, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl0.TermModified, AddressOf onTermModified
        AddHandler ctrl0.SelectedIndexChanged, AddressOf onSIChanged
        col0.Add(ctrl0)
        Dim ctrl1 As New TermSourceComboBoxControl(cmbxarr, 1, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl1.TermModified, AddressOf onTermModified
        AddHandler ctrl1.SelectedIndexChanged, AddressOf onSIChanged
        col1.Add(ctrl1)
        cv = 0
        For Each e As TermSourceBaseControl In col0
            e.Parent = TableLayoutPanelInternal
            TableLayoutPanelInternal.Controls.Add(e, 0, cv)
            e.Visible = True
            cv += 1
        Next
        cv = 0
        For Each e As TermSourceBaseControl In col1
            e.Parent = TableLayoutPanelInternal
            TableLayoutPanelInternal.Controls.Add(e, 1, cv)
            e.Visible = True
            cv += 1
        Next
    End Sub
    Public Sub refreshTerms()
        If Not projectCheck() Then Return
        unTerms()
        doTerms()
    End Sub
    Private Function projectCheck()
        If _project Is Nothing Then Return False Else Return True
    End Function
    Private Sub onTermModified(sender As Object, e As TermSourceControlEventArgs)

    End Sub
    Private Sub onSIChanged(sender As Object, e As TermSourceComboBoxControlSelectedIndexChangedEventArgs)

    End Sub
    Private Function castObject(Of t)(f As Object) As t
        Try
            Dim nf As t = f
            Return nf
        Catch ex As InvalidCastException
            Return Nothing
        End Try
    End Function
    Private Function canCastObject(Of t)(f As Object) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function
End Class
