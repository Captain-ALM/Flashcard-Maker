﻿Imports captainalm.util.preference

Public Class TermEditorTable
    Private _project As Project = Nothing
    Private cmbxarr As String() = New String() {"", "Text"}
    Private col0 As New List(Of TermSourceBaseControl)
    Private col1 As New List(Of TermSourceBaseControl)
    Private _selected As New List(Of TermSourceBaseControl)
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
        refreshTerms()
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
    Public Sub addRow(at As Integer)
        If Not projectCheck() Then Return
        If at > _project.dataCount - 1 Then Return
        _project.addData(New TermSet(Of TermSource, TermSource)(New EmptyTerm(), New EmptyTerm()))
        For cnt As Integer = TableLayoutPanelInternal.RowCount - 1 To at - 1 Step -1
            _project.data(cnt) = _project.data(cnt - 1)
            col0(cnt - 1).Row += 1
            col1(cnt - 1).Row += 1
            TableLayoutPanelInternal.SetRow(col0(cnt - 1), col0(cnt - 1).Row)
            TableLayoutPanelInternal.SetRow(col1(cnt - 1), col1(cnt - 1).Row)
        Next
        _project.data(at) = New TermSet(Of TermSource, TermSource)(New EmptyTerm(), New EmptyTerm())
        Dim ctrl0 As New TermSourceComboBoxControl(cmbxarr, 0, at) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        Dim ctrl1 As New TermSourceComboBoxControl(cmbxarr, 1, at) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl0.TermModified, AddressOf onTermModified
        AddHandler ctrl0.SelectedIndexChanged, AddressOf onSIChanged
        AddHandler ctrl0.ControlSelected, AddressOf onTermSelected
        AddHandler ctrl0.ControlDeselected, AddressOf onTermDeselected
        col0.Add(ctrl0)
        AddHandler ctrl1.TermModified, AddressOf onTermModified
        AddHandler ctrl1.SelectedIndexChanged, AddressOf onSIChanged
        AddHandler ctrl1.ControlSelected, AddressOf onTermSelected
        AddHandler ctrl1.ControlDeselected, AddressOf onTermDeselected
        col1.Add(ctrl1)
        TableLayoutPanelInternal.Controls.Add(ctrl0, 0, at)
        TableLayoutPanelInternal.Controls.Add(ctrl1, 1, at)
        ctrl0.Enabled = True
        ctrl0.Visible = True
        ctrl1.Enabled = True
        ctrl1.Visible = True
    End Sub
    Public Sub removeRow(at As Integer)
        If Not projectCheck() Then Return
        If at > _project.dataCount - 1 Then Return
        _project.removeData(at)
        Dim ctrl0 As TermSourceBaseControl = col0(at)
        Dim ctrl1 As TermSourceBaseControl = col1(at)
        col0.RemoveAt(at)
        col1.RemoveAt(at)
        TableLayoutPanelInternal.Controls.Remove(ctrl0)
        TableLayoutPanelInternal.Controls.Remove(ctrl1)
        killControl(ctrl0)
        killControl(ctrl1)
        For cnt As Integer = at To _project.dataCount Step 1
            col0(cnt).Row -= 1
            col1(cnt).Row -= 1
            TableLayoutPanelInternal.SetRow(col0(cnt), col0(cnt).Row)
            TableLayoutPanelInternal.SetRow(col1(cnt), col1(cnt).Row)
        Next
        TableLayoutPanelInternal.RowCount -= 1
        TableLayoutPanelInternal.RowStyles.RemoveAt(TableLayoutPanelInternal.RowStyles.Count - 1)
        For i As Integer = 0 To TableLayoutPanelInternal.RowCount - 1 Step 1
            TableLayoutPanelInternal.RowStyles(i) = New RowStyle(SizeType.Percent, 100 / (_project.dataCount + 1))
        Next
    End Sub
    Public Sub moveRowUp(row As Integer)
        If Not projectCheck() Then Return
        If row < 1 Then Return
        If row > _project.dataCount - 1 Then Return
        Dim ctrl0 As TermSourceBaseControl = col0(row)
        Dim ctrl1 As TermSourceBaseControl = col1(row)
        Dim ctrl2 As TermSourceBaseControl = col0(row - 1)
        Dim ctrl3 As TermSourceBaseControl = col1(row - 1)
        Dim d0 As TermSet(Of TermSource, TermSource) = _project.data(row)
        Dim d1 As TermSet(Of TermSource, TermSource) = _project.data(row - 1)
        ctrl0.Row -= 1
        ctrl1.Row -= 1
        ctrl2.Row += 1
        ctrl3.Row += 1
        _project.data(row - 1) = d0
        _project.data(row) = d1
        TableLayoutPanelInternal.SetRow(ctrl0, ctrl0.Row)
        TableLayoutPanelInternal.SetRow(ctrl1, ctrl1.Row)
        TableLayoutPanelInternal.SetRow(ctrl2, ctrl2.Row)
        TableLayoutPanelInternal.SetRow(ctrl3, ctrl3.Row)
    End Sub
    Public Sub moveRowDown(row As Integer)
        If Not projectCheck() Then Return
        If row > _project.dataCount - 2 Then Return
        Dim ctrl0 As TermSourceBaseControl = col0(row)
        Dim ctrl1 As TermSourceBaseControl = col1(row)
        Dim ctrl2 As TermSourceBaseControl = col0(row + 1)
        Dim ctrl3 As TermSourceBaseControl = col1(row + 1)
        Dim d0 As TermSet(Of TermSource, TermSource) = _project.data(row)
        Dim d1 As TermSet(Of TermSource, TermSource) = _project.data(row + 1)
        ctrl0.Row += 1
        ctrl1.Row += 1
        ctrl2.Row -= 1
        ctrl3.Row -= 1
        _project.data(row + 1) = d0
        _project.data(row) = d1
        TableLayoutPanelInternal.SetRow(ctrl0, ctrl0.Row)
        TableLayoutPanelInternal.SetRow(ctrl1, ctrl1.Row)
        TableLayoutPanelInternal.SetRow(ctrl2, ctrl2.Row)
        TableLayoutPanelInternal.SetRow(ctrl3, ctrl3.Row)
    End Sub
    Public Sub saveTerms()
        If Not projectCheck() Then Return
        For cnt As Integer = 0 To _project.dataCount - 1 Step 1
            Dim e1 As TermSourceBaseControl = col0(cnt)
            Dim e2 As TermSourceBaseControl = col1(cnt)
            _project.data(cnt).Term1 = e1.Term
            _project.data(cnt).Term2 = e2.Term
            cnt += 1
        Next cnt
    End Sub
    Private Sub unTerms()
        If Not projectCheck() Then Return
        TableLayoutPanelInternal.Controls.Clear()
        TableLayoutPanelInternal.ColumnStyles.Clear()
        TableLayoutPanelInternal.RowStyles.Clear()
        TableLayoutPanelInternal.ColumnCount = 2
        TableLayoutPanelInternal.RowCount = 1
        For Each e As TermSourceBaseControl In col0
            killControl(e)
        Next
        For Each e As TermSourceBaseControl In col1
            killControl(e)
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
                AddHandler ctrl.ControlSelected, AddressOf onTermSelected
                AddHandler ctrl.ControlDeselected, AddressOf onTermDeselected
                col0.Add(ctrl)
            ElseIf s.Term1.termSourceType = "EmptyTerm" Then
                Dim ctrl As New TermSourceComboBoxControl(cmbxarr, 0, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                AddHandler ctrl.SelectedIndexChanged, AddressOf onSIChanged
                AddHandler ctrl.ControlSelected, AddressOf onTermSelected
                AddHandler ctrl.ControlDeselected, AddressOf onTermDeselected
                col0.Add(ctrl)
            End If
            If s.Term2.termSourceType = "TextTerm" Then
                Dim ctrl As New TermSourceTextBoxControl(s.Term2, 1, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                AddHandler ctrl.ControlSelected, AddressOf onTermSelected
                AddHandler ctrl.ControlDeselected, AddressOf onTermDeselected
                col1.Add(ctrl)
            ElseIf s.Term2.termSourceType = "EmptyTerm" Then
                Dim ctrl As New TermSourceComboBoxControl(cmbxarr, 1, cv) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                AddHandler ctrl.SelectedIndexChanged, AddressOf onSIChanged
                AddHandler ctrl.ControlSelected, AddressOf onTermSelected
                AddHandler ctrl.ControlDeselected, AddressOf onTermDeselected
                col1.Add(ctrl)
            End If
        Next cv
        Dim ctrl0 As New TermSourceComboBoxControl(cmbxarr, 0, TableLayoutPanelInternal.RowCount - 1) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl0.TermModified, AddressOf onTermModified
        AddHandler ctrl0.SelectedIndexChanged, AddressOf onSIChanged
        AddHandler ctrl0.ControlSelected, AddressOf onTermSelected
        AddHandler ctrl0.ControlDeselected, AddressOf onTermDeselected
        col0.Add(ctrl0)
        Dim ctrl1 As New TermSourceComboBoxControl(cmbxarr, 1, TableLayoutPanelInternal.RowCount - 1) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl1.TermModified, AddressOf onTermModified
        AddHandler ctrl1.SelectedIndexChanged, AddressOf onSIChanged
        AddHandler ctrl1.ControlSelected, AddressOf onTermSelected
        AddHandler ctrl1.ControlDeselected, AddressOf onTermDeselected
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
    Private Sub onTermSelected(sender As Object, e As TermSourceControlEventArgs)
        If canCastObject(Of TermSourceBaseControl)(sender) Then
            Dim c As TermSourceBaseControl = castObject(Of TermSourceBaseControl)(sender)
            If Not _selected.Contains(c) And c.Selected Then _selected.Add(c)
        End If
    End Sub
    Private Sub onTermDeselected(sender As Object, e As TermSourceControlEventArgs)
        If canCastObject(Of TermSourceBaseControl)(sender) Then
            Dim c As TermSourceBaseControl = castObject(Of TermSourceBaseControl)(sender)
            If _selected.Contains(c) And Not c.Selected Then _selected.Remove(c)
        End If
    End Sub
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
                killControl(ctrl)
            End If
            If e.SelectedIndex = 1 Then
                Dim ts As New TextTerm("", _project.Settings.getPreference(Of IPreference(Of Font))("Font").getPreference(), _project.Settings.getPreference(Of IPreference(Of Color))("Color").getPreference(), globalops.getPreference(Of IPreference(Of GlobalPreferences))("GlobalPreferences").getPreference().getPreference(Of IPreference(Of Integer))("MinumumFontSize").getPreference(), _project.Settings.getPreference(Of IPreference(Of Boolean))("CanSplitWords").getPreference())
                ctrl = New TermSourceTextBoxControl(ts, e.Column, e.Row) With {.Parent = TableLayoutPanelInternal, .Dock = DockStyle.Fill}
                AddHandler ctrl.TermModified, AddressOf onTermModified
                AddHandler ctrl.ControlSelected, AddressOf onTermSelected
                AddHandler ctrl.ControlDeselected, AddressOf onTermDeselected
                TableLayoutPanelInternal.Controls.Add(ctrl, e.Column, e.Row)
                ctrl.Enabled = True
                ctrl.Visible = True
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
                addNewToBottom()
            End If
        End If
    End Sub
    Private Sub addNewToBottom()
        Dim ctrl0 As New TermSourceComboBoxControl(cmbxarr, 0, TableLayoutPanelInternal.RowCount) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl0.TermModified, AddressOf onTermModified
        AddHandler ctrl0.SelectedIndexChanged, AddressOf onSIChanged
        AddHandler ctrl0.ControlSelected, AddressOf onTermSelected
        AddHandler ctrl0.ControlDeselected, AddressOf onTermDeselected
        col0.Add(ctrl0)
        Dim ctrl1 As New TermSourceComboBoxControl(cmbxarr, 1, TableLayoutPanelInternal.RowCount) With {.Dock = DockStyle.Fill, .Parent = TableLayoutPanelInternal}
        AddHandler ctrl1.TermModified, AddressOf onTermModified
        AddHandler ctrl1.SelectedIndexChanged, AddressOf onSIChanged
        AddHandler ctrl1.ControlSelected, AddressOf onTermSelected
        AddHandler ctrl1.ControlDeselected, AddressOf onTermDeselected
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
    End Sub
    Private Sub killControl(ByRef ctrl As TermSourceBaseControl)
        ctrl.Parent = Nothing
        If canCastObject(Of TermSourceComboBoxControl)(ctrl) Then
            Dim _ctrl As TermSourceComboBoxControl = castObject(Of TermSourceComboBoxControl)(ctrl)
            RemoveHandler _ctrl.SelectedIndexChanged, AddressOf onSIChanged
        End If
        RemoveHandler ctrl.TermModified, AddressOf onTermModified
        RemoveHandler ctrl.ControlSelected, AddressOf onTermSelected
        RemoveHandler ctrl.ControlDeselected, AddressOf onTermDeselected
        ctrl.Dispose()
        ctrl = Nothing
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
