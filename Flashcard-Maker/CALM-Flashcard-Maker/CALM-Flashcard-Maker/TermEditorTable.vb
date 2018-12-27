Imports captainalm.util.preference

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
            e.Parent = Nothing
            If canCastObject(Of TermSourceComboBoxControl)(e) Then
                Dim ctrl As TermSourceComboBoxControl = castObject(Of TermSourceComboBoxControl)(e)
                RemoveHandler ctrl.SelectedIndexChanged, AddressOf onSIChanged
            End If
            RemoveHandler e.TermModified, AddressOf onTermModified
            e.Dispose()
        Next
        For Each e As TermSourceBaseControl In col1
            e.Parent = Nothing
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
        Dim ctrl0 As New TermSourceComboBoxControl(cmbxarr, 0, TableLayoutPanelInternal.RowCount - 1) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl0.TermModified, AddressOf onTermModified
        AddHandler ctrl0.SelectedIndexChanged, AddressOf onSIChanged
        col0.Add(ctrl0)
        Dim ctrl1 As New TermSourceComboBoxControl(cmbxarr, 1, TableLayoutPanelInternal.RowCount - 1) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl1.TermModified, AddressOf onTermModified
        AddHandler ctrl1.SelectedIndexChanged, AddressOf onSIChanged
        col1.Add(ctrl1)
        cv = 0
        For Each e As TermSourceBaseControl In col0
            e.Parent = TableLayoutPanelInternal
            TableLayoutPanelInternal.Controls.Add(e, 0, cv)
            e.Visible = True
            e.Enabled = True
            cv += 1
        Next
        cv = 0
        For Each e As TermSourceBaseControl In col1
            e.Parent = TableLayoutPanelInternal
            TableLayoutPanelInternal.Controls.Add(e, 1, cv)
            e.Visible = True
            e.Enabled = True
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
        If e.Column = 0 Then
            _project.data(e.Row).Term1 = col0(e.Row).Term
        ElseIf e.Column = 1 Then
            _project.data(e.Row).Term2 = col1(e.Row).Term
        End If
    End Sub
    Private Sub onSIChanged(sender As Object, e As TermSourceComboBoxControlSelectedIndexChangedEventArgs)
        If e.SelectedIndex > 0 Then
            If canCastObject(Of Control)(sender) Then
                Dim c As Control = castObject(Of Control)(sender)
                c.Enabled = False
            End If
            Dim ctrl As TermSourceBaseControl = Nothing
            If e.Column = 0 Then
                ctrl = col0(e.Row)
            ElseIf e.Column = 1 Then
                ctrl = col1(e.Row)
            End If
            If ctrl IsNot Nothing Then
                TableLayoutPanelInternal.Controls.Remove(ctrl)
                ctrl.Parent = Nothing
                If canCastObject(Of TermSourceComboBoxControl)(ctrl) Then
                    Dim ctrl2 As TermSourceComboBoxControl = castObject(Of TermSourceComboBoxControl)(ctrl)
                    RemoveHandler ctrl2.SelectedIndexChanged, AddressOf onSIChanged
                End If
                RemoveHandler ctrl.TermModified, AddressOf onTermModified
                ctrl.Dispose()
                ctrl = Nothing
            End If
            If e.SelectedIndex = 1 Then
                Dim ts As New TextTerm("", _project.Settings.getPreference(Of IPreference(Of Font))("Font").getPreference(), _project.Settings.getPreference(Of IPreference(Of Color))("Color").getPreference(), globalops.getPreference(Of IPreference(Of GlobalPreferences))("GlobalPreferences").getPreference().getPreference(Of IPreference(Of Integer))("MinumumFontSize").getPreference(), _project.Settings.getPreference(Of IPreference(Of Boolean))("CanSplitWords").getPreference())
                ctrl = New TermSourceTextBoxControl(ts, e.Column, e.Row) With {.Parent = TableLayoutPanelInternal, .Dock = DockStyle.Fill}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                TableLayoutPanelInternal.Controls.Add(ctrl, e.Column, e.Row)
            End If
            If ctrl IsNot Nothing Then
                If e.Column = 0 Then
                    If e.Row = TableLayoutPanelInternal.RowCount - 1 Then _project.addData(New TermSet(Of TermSource, TermSource)(ctrl.getTerm(), New EmptyTerm())) Else _project.data(e.Row).Term1 = ctrl.getTerm()
                    col0(e.Row) = ctrl
                ElseIf e.Column = 1 Then
                    If e.Row = TableLayoutPanelInternal.RowCount - 1 Then _project.addData(New TermSet(Of TermSource, TermSource)(New EmptyTerm(), ctrl.getTerm())) Else _project.data(e.Row).Term2 = ctrl.getTerm()
                    col1(e.Row) = ctrl
                End If
            End If
            If e.Row = TableLayoutPanelInternal.RowCount - 1 Then
                Dim ctrl0 As New TermSourceComboBoxControl(cmbxarr, 0, TableLayoutPanelInternal.RowCount) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl0.TermModified, AddressOf onTermModified
                AddHandler ctrl0.SelectedIndexChanged, AddressOf onSIChanged
                col0.Add(ctrl0)
                Dim ctrl1 As New TermSourceComboBoxControl(cmbxarr, 1, TableLayoutPanelInternal.RowCount) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl1.TermModified, AddressOf onTermModified
                AddHandler ctrl1.SelectedIndexChanged, AddressOf onSIChanged
                col1.Add(ctrl1)
                TableLayoutPanelInternal.RowCount += 1
                TableLayoutPanelInternal.RowStyles.Add(New RowStyle(SizeType.Percent, 100 / (_project.dataCount + 1)))
                For i As Integer = 0 To TableLayoutPanelInternal.RowCount - 1 Step 1
                    TableLayoutPanelInternal.RowStyles(i) = New RowStyle(SizeType.Percent, 100 / (_project.dataCount + 1))
                Next
                ctrl0.Parent = TableLayoutPanelInternal
                TableLayoutPanelInternal.Controls.Add(ctrl0, 0, TableLayoutPanelInternal.RowCount - 1)
                ctrl0.Visible = True
                ctrl0.Enabled = True
                ctrl1.Parent = TableLayoutPanelInternal
                TableLayoutPanelInternal.Controls.Add(ctrl1, 1, TableLayoutPanelInternal.RowCount - 1)
                ctrl1.Visible = True
                ctrl1.Enabled = True
            End If
        End If
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
