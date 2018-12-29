Imports captainalm.workerpumper

Public Class MainProgram
    Private formClosingDone As Boolean = False
    Private formClosedDone As Boolean = False
    Private wp As WorkerPump = Nothing
    Private ue As Boolean = False
    Friend project As Project = Nothing
    Friend project_path As String = ""

    'Should not construct externally.
    Sub New(Optional ByRef workerp As WorkerPump = Nothing)
        ' This call is required by the designer.
        InitializeComponent()

        If workerp IsNot Nothing Then
            wp = workerp
            ue = True
        Else
            ue = False
        End If
    End Sub

#Region "closeOverride"
    Public Shadows Sub Close()
        Me.Hide()
        Me.OnFormClosing(New FormClosingEventArgs(CloseReason.UserClosing, False))
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
#End Region

    Private Sub MainProgram_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FontDialogNoSize1.FontDialog.ShowColor = False
        FontDialogNoSize1.FontDialog.ShowEffects = True
        If ue Then wp.addEvent(New WorkerEvent(Me, EventTypes.Load, e))
    End Sub

    Private Sub MainProgram_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not formClosingDone Then
            If project IsNot Nothing Then
                If Not Me.Visible Then Me.Show()
                Dim r As MsgBoxResult = MsgBox("Do you want to save the project?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question, "CALM Flashcard Maker")
                If project_path = "" Then
                    If r = MsgBoxResult.Yes Then
                        SaveFileDialog1.FileName = ""
                        Dim r2 As DialogResult = SaveFileDialog1.ShowDialog(Me)
                        project_path = SaveFileDialog1.FileName
                        If r2 = Windows.Forms.DialogResult.OK Then
                            IO.File.WriteAllText(project_path, BinarySerializer.serialize(Of Project)(project))
                            If ue Then wp.addEvent(SaveFileDialog1, New List(Of Object)(New Object() {Me}), EventTypes.DialogClosed, New FileDialogSuccessEventArgs(SaveFileDialog1.FileName, FileDialogMode.Save))
                        End If
                    ElseIf r = MsgBoxResult.Cancel Then
                        Return
                    End If
                Else
                    If r = MsgBoxResult.Yes Then
                        IO.File.WriteAllText(project_path, BinarySerializer.serialize(Of Project)(project))
                    ElseIf r = MsgBoxResult.Cancel Then
                        Return
                    End If
                End If
            End If
            If Me.Visible Then
                'If close button pressed
                e.Cancel = True
                Me.Hide()
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
            If ue Then wp.addEvent(New WorkerEvent(Me, EventTypes.Closing, e))
            Me.OnFormClosed(New FormClosedEventArgs(e.CloseReason))
            formClosingDone = True
        End If
    End Sub

    Private Sub MainProgram_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not formClosedDone Then
            If ue Then wp.addEvent(New WorkerEvent(Me, EventTypes.Closed, e))
            formClosedDone = True
        End If
    End Sub

    Private Sub MainProgram_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.DialogResult = Windows.Forms.DialogResult.None
        formClosingDone = False
        formClosedDone = False
        If ue Then wp.addEvent(Me, EventTypes.Shown, e)
    End Sub

    Private Sub butstn_Click(sender As Object, e As EventArgs) Handles butstn.Click
        If ue Then
            butstn.Enabled = False
            project_path = ""
            project = New Project("UNTITLED")
            wp.addEvent(butstn, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butstq_Click(sender As Object, e As EventArgs) Handles butstq.Click
        If ue Then
            butstq.Enabled = False
            wp.addEvent(butstq, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
        Me.Close()
    End Sub

    Private Sub butsta_Click(sender As Object, e As EventArgs) Handles butsta.Click
        If ue Then
            butsta.Enabled = False
            wp.addEvent(butsta, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butsth_Click(sender As Object, e As EventArgs) Handles butsth.Click
        If ue Then
            butsth.Enabled = False
            wp.addEvent(butsth, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butstc_Click(sender As Object, e As EventArgs) Handles butstc.Click
        If ue Then
            butstc.Enabled = False
            Dim r As MsgBoxResult = MsgBox("Do you want to save the project?", MsgBoxStyle.YesNoCancel + MsgBoxStyle.Question, "CALM Flashcard Maker")
            If project_path = "" Then
                If r = MsgBoxResult.Yes Then
                    SaveFileDialog1.FileName = ""
                    Dim r2 As DialogResult = SaveFileDialog1.ShowDialog(Me)
                    project_path = SaveFileDialog1.FileName
                    If r2 = Windows.Forms.DialogResult.OK Then
                        IO.File.WriteAllText(project_path, BinarySerializer.serialize(Of Project)(project))
                        If ue Then wp.addEvent(SaveFileDialog1, New List(Of Object)(New Object() {butstc, Me}), EventTypes.DialogClosed, New FileDialogSuccessEventArgs(SaveFileDialog1.FileName, FileDialogMode.Save))
                    End If
                    project = Nothing
                    project_path = ""
                ElseIf r = MsgBoxResult.Cancel Then
                    Return
                End If
            Else
                If r = MsgBoxResult.Yes Then
                    IO.File.WriteAllText(project_path, BinarySerializer.serialize(Of Project)(project))
                    project = Nothing
                    project_path = ""
                ElseIf r = MsgBoxResult.Cancel Then
                    Return
                End If
            End If
            wp.addEvent(butstc, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butsto_Click(sender As Object, e As EventArgs) Handles butsto.Click
        If ue Then
            butsto.Enabled = False
            OpenFileDialog1.FileName = ""
            Dim r As DialogResult = OpenFileDialog1.ShowDialog(Me)
            If r = Windows.Forms.DialogResult.OK Then
                project = BinarySerializer.deserialize(Of Project)(IO.File.ReadAllText(OpenFileDialog1.FileName))
                If project IsNot Nothing Then
                    project_path = OpenFileDialog1.FileName
                End If
                If ue Then wp.addEvent(OpenFileDialog1, New List(Of Object)(New Object() {butsto, Me}), EventTypes.DialogClosed, New FileDialogSuccessEventArgs(OpenFileDialog1.FileName, FileDialogMode.Open))
            End If
            wp.addEvent(butsto, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butsti_Click(sender As Object, e As EventArgs) Handles butsti.Click
        If ue Then
            butsti.Enabled = False
            OpenFileDialog1.FileName = ""
            Dim r As DialogResult = OpenFileDialog1.ShowDialog(Me)
            If r = Windows.Forms.DialogResult.OK Then
                project = BinarySerializer.deserialize(Of Project)(IO.File.ReadAllText(OpenFileDialog1.FileName))
                project_path = ""
                If ue Then wp.addEvent(OpenFileDialog1, New List(Of Object)(New Object() {butsti, Me}), EventTypes.DialogClosed, New FileDialogSuccessEventArgs(OpenFileDialog1.FileName, FileDialogMode.Open))
            End If
            wp.addEvent(butsti, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butrender_Click(sender As Object, e As EventArgs) Handles butrender.Click
        If ue Then
            butrender.Enabled = False
            wp.addEvent(butrender, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butpdid_Click(sender As Object, e As EventArgs) Handles butpdid.Click
        If ue Then
            butpdid.Enabled = False
            wp.addEvent(butpdid, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butpded_Click(sender As Object, e As EventArgs) Handles butpded.Click
        If ue Then
            butpded.Enabled = False
            wp.addEvent(butpded, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub
    
    Private Sub TermEditorTablepd1_KeyDown(sender As Object, e As KeyEventArgs) Handles TermEditorTablepd1.KeyDown
        If ue Then
            TermEditorTablepd1.Enabled = False
            wp.addEvent(TermEditorTablepd1, New List(Of Object)(New Object() {Me}), EventTypes.KeyDown, e)
        End If
    End Sub

    Private Sub nudpvzoom_ValueChanged(sender As Object, e As EventArgs) Handles nudpvzoom.ValueChanged
        If ue Then
            nudpvzoom.Enabled = False
            wp.addEvent(nudpvzoom, New List(Of Object)(New Object() {Me}), EventTypes.ValueChanged, e)
        End If
    End Sub

    Private Sub butpvforward_Click(sender As Object, e As EventArgs) Handles butpvforward.Click
        If ue Then
            butpvforward.Enabled = False
            wp.addEvent(butpvforward, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub butpvback_Click(sender As Object, e As EventArgs) Handles butpvback.Click
        If ue Then
            butpvback.Enabled = False
            wp.addEvent(butpvback, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub cmbxpeec_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbxpeec.SelectedIndexChanged
        If ue Then
            cmbxpeec.Enabled = False
            wp.addEvent(cmbxpeec, New List(Of Object)(New Object() {Me}), EventTypes.SelectedIndexChanged, e)
        End If
    End Sub

    Private Sub butpee_Click(sender As Object, e As EventArgs) Handles butpee.Click
        If ue Then
            butpee.Enabled = False
            wp.addEvent(butpee, New List(Of Object)(New Object() {Me}), EventTypes.Click, e)
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If ue Then
            TabControl1.Enabled = False
            wp.addEvent(TabControl1, New List(Of Object)(New Object() {Me}), EventTypes.SelectedIndexChanged, e)
        End If
    End Sub

    Private Sub cbxpops_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxpops.SelectedIndexChanged
        If ue Then
            cbxpops.Enabled = False
            wp.addEvent(cbxpops, New List(Of Object)(New Object() {Me}), EventTypes.SelectedIndexChanged, e)
        End If
    End Sub
    Private Sub nudporfs_ValueChanged(sender As Object, e As EventArgs) Handles nudporfs.ValueChanged
        If ue Then
            nudporfs.Enabled = False
            wp.addEvent(nudporfs, New List(Of Object)(New Object() {Me}), EventTypes.ValueChanged, e)
        End If
    End Sub
    Private Sub nudpotc_ValueChanged(sender As Object, e As EventArgs) Handles nudpotc.ValueChanged
        If ue Then
            nudpotc.Enabled = False
            wp.addEvent(nudpotc, New List(Of Object)(New Object() {Me}), EventTypes.ValueChanged, e)
        End If
    End Sub
    Private Sub rbutpoutcprfs_CheckedChanged(sender As Object, e As EventArgs) Handles rbutpoutcprfs.CheckedChanged
        If ue Then rbutpoutcprfs.Enabled = False
        If rbutpoutcprfs.Checked Then
            lblpotc.Enabled = False
            nudpotc.Enabled = False
            lblporfs.Enabled = True
            nudporfs.Enabled = True
        Else
            lblpotc.Enabled = True
            nudpotc.Enabled = True
            lblporfs.Enabled = False
            nudporfs.Enabled = False
        End If
        If ue Then wp.addEvent(rbutpoutcprfs, New List(Of Object)(New Object() {Me}), EventTypes.CheckedChanged, e)
    End Sub
    Private Sub rbutpocpc_CheckedChanged(sender As Object, e As EventArgs) Handles rbutpocpc.CheckedChanged
        If ue Then rbutpocpc.Enabled = False
        If rbutpocpc.Checked Then
            lblpotc.Enabled = True
            nudpotc.Enabled = True
            lblporfs.Enabled = False
            nudporfs.Enabled = False
        Else
            lblpotc.Enabled = False
            nudpotc.Enabled = False
            lblporfs.Enabled = True
            nudporfs.Enabled = True
        End If
        If ue Then wp.addEvent(rbutpocpc, New List(Of Object)(New Object() {Me}), EventTypes.CheckedChanged, e)
    End Sub
    Private Sub nudpocw_ValueChanged(sender As Object, e As EventArgs) Handles nudpocw.ValueChanged
        If ue Then
            nudpocw.Enabled = False
            wp.addEvent(nudpocw, New List(Of Object)(New Object() {Me}), EventTypes.ValueChanged, e)
        End If
    End Sub
    Private Sub nudpoch_ValueChanged(sender As Object, e As EventArgs) Handles nudpoch.ValueChanged
        If ue Then
            nudpoch.Enabled = False
            wp.addEvent(nudpoch, New List(Of Object)(New Object() {Me}), EventTypes.ValueChanged, e)
        End If
    End Sub
    Private Sub butpocfs_Click(sender As Object, e As EventArgs) Handles butpocfs.Click
        If ue Then butpocfs.Enabled = False
        Dim r As DialogResult = FontDialogNoSize1.ShowDialog(Me)
        If r = Windows.Forms.DialogResult.OK Then
            Dim lst As New List(Of Object)
            lst.Add(butpocfs)
            lst.Add(Me)
            If ue Then wp.addEvent(FontDialogNoSize1, lst, EventTypes.DialogClosed, New FontDialogSuccessEventArgs(FontDialogNoSize1.FontValue, FontDialogNoSize1.ColorValue))
        End If
        If ue Then wp.addEvent(butpocfs, Me, EventTypes.Click, e)
        If ue Then butpocfs.Enabled = True
    End Sub
    Private Sub butpocfc_Click(sender As Object, e As EventArgs) Handles butpocfc.Click
        If ue Then butpocfc.Enabled = False
        Dim r As DialogResult = ColorDialog1.ShowDialog(Me)
        If r = Windows.Forms.DialogResult.OK Then
            Dim lst As New List(Of Object)
            lst.Add(butpocfc)
            lst.Add(Me)
            If ue Then wp.addEvent(ColorDialog1, lst, EventTypes.DialogClosed, New ColorDialogSuccessEventArgs(ColorDialog1.Color))
        End If
        If ue Then wp.addEvent(butpocfc, Me, EventTypes.Click, e)
        If ue Then butpocfc.Enabled = True
    End Sub
    Private Sub chkbxpoasw_CheckedChanged(sender As Object, e As EventArgs) Handles chkbxpoasw.CheckedChanged
        If ue Then
            chkbxpoasw.Enabled = False
            wp.addEvent(chkbxpoasw, New List(Of Object)(New Object() {Me}), EventTypes.CheckedChanged, e)
        End If
    End Sub
    Private Sub NewProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewProjectToolStripMenuItem.Click

    End Sub
    Private Sub OpenProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenProjectToolStripMenuItem.Click

    End Sub
    Private Sub ImportProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportProjectToolStripMenuItem.Click

    End Sub
    Private Sub SaveProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectToolStripMenuItem.Click

    End Sub
    Private Sub SaveAsProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsProjectToolStripMenuItem.Click

    End Sub
    Private Sub CloseProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseProjectToolStripMenuItem.Click

    End Sub
#Region "InternalExportOptions-0.0-Only"
    Private Sub PrintProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintProjectToolStripMenuItem.Click

    End Sub
    Private Sub SaveProjectImagesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectImagesToolStripMenuItem.Click

    End Sub
#End Region
    Private Sub CloseProgramToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseProgramToolStripMenuItem.Click

    End Sub
    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click

    End Sub
    Private Sub RedoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoToolStripMenuItem.Click

    End Sub
    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click

    End Sub
    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click

    End Sub
    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click

    End Sub
    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click

    End Sub
    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click

    End Sub
    Private Sub ImportDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportDataToolStripMenuItem.Click

    End Sub
    Private Sub ExportDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportDataToolStripMenuItem.Click

    End Sub
    Private Sub AddRowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddRowToolStripMenuItem.Click

    End Sub
    Private Sub RemoveRowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveRowToolStripMenuItem.Click

    End Sub
    Private Sub MoveRowUpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveRowUpToolStripMenuItem.Click

    End Sub
    Private Sub MoveRowDownToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveRowDownToolStripMenuItem.Click

    End Sub
    Private Sub ClearCellToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearCellToolStripMenuItem.Click

    End Sub
    Private Sub StartTabToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartTabToolStripMenuItem.Click

    End Sub
    Private Sub ProjectOptionsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ProjectOptionsToolStripMenuItem1.Click

    End Sub
    Private Sub ProjectDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectDataToolStripMenuItem.Click

    End Sub
    Private Sub ProjectViewerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectViewerToolStripMenuItem.Click

    End Sub
    Private Sub ProjectExportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectExportToolStripMenuItem.Click

    End Sub
    Private Sub GlobalOptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GlobalOptionsToolStripMenuItem.Click

    End Sub
    Private Sub ProjectOptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectOptionsToolStripMenuItem.Click

    End Sub
    Private Sub ProgramHelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProgramHelpToolStripMenuItem.Click

    End Sub
    Private Sub ReportAnIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportAnIssueToolStripMenuItem.Click

    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click

    End Sub
    Private Sub ImportDataToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ImportDataToolStripMenuItem1.Click

    End Sub
    Private Sub ExportDataToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ExportDataToolStripMenuItem1.Click

    End Sub
    Private Sub AddRowToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddRowToolStripMenuItem1.Click

    End Sub
    Private Sub RemoveRowToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RemoveRowToolStripMenuItem1.Click

    End Sub
    Private Sub MoveRowUpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MoveRowUpToolStripMenuItem1.Click

    End Sub
    Private Sub MoveRowDownToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MoveRowDownToolStripMenuItem1.Click

    End Sub
    Private Sub ClearCellToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ClearCellToolStripMenuItem1.Click

    End Sub
End Class

Public Class FileDialogSuccessEventArgs
    Inherits EventArgs

    Private _filep As String
    Private _mode As FileDialogMode = FileDialogMode.None

    Public Sub New(p As String, m As FileDialogMode)
        _filep = p
        _mode = m
    End Sub

    Public ReadOnly Property FilePath As String
        Get
            Return _filep
        End Get
    End Property

    Public ReadOnly Property DialogMode As FileDialogMode
        Get
            Return _mode
        End Get
    End Property
End Class

Public Enum FileDialogMode
    None = 0
    Open = 1
    Save = 2
End Enum

Public Class TableControlSizeHooker
    Private _form As Form = Nothing
    Private _parent As TableLayoutPanel = Nothing
    Private _control As Control = Nothing
    Private minsize As Size = Nothing
    Private maxsize As Size = Nothing
    Private sizing As Boolean = False
    Private sizingp As Boolean = False
    Public Sub New(f As Form, p As TableLayoutPanel, Optional he As Boolean = False)
        _parent = p
        _control = _parent.Container.Components(0)
        _form = f
        If he Then hookEvents()
    End Sub
    Public Sub New(f As Form, p As TableLayoutPanel, c As Control, Optional he As Boolean = False)
        _parent = p
        If _parent.Contains(c) Then _control = c Else _control = _parent.Container.Components(0)
        _form = f
        If he Then hookEvents()
    End Sub
    Public Sub New(f As Form, p As TableLayoutPanel, c As Control, mins As Size, maxs As Size, Optional he As Boolean = False)
        _parent = p
        If _parent.Contains(c) Then _control = c Else _control = _parent.Container.Components(0)
        minsize = mins
        maxsize = maxs
        _form = f
        If he Then hookEvents()
    End Sub
    Public ReadOnly Property ParentControl As TableLayoutPanel
        Get
            Return _parent
        End Get
    End Property
    Public ReadOnly Property Control As Control
        Get
            Return _control
        End Get
    End Property
    Public ReadOnly Property Form As Form
        Get
            Return _form
        End Get
    End Property
    Public Property MinumumSize As Size
        Get
            Return minsize
        End Get
        Set(value As Size)
            minsize = value
        End Set
    End Property
    Public Property MaximumSize As Size
        Get
            Return maxsize
        End Get
        Set(value As Size)
            maxsize = value
        End Set
    End Property
    Public Sub hookEvents()
        AddHandler _parent.Layout, AddressOf onResizeParent
        AddHandler _control.Layout, AddressOf onResize
    End Sub
    Public Sub unhookEvents()
        RemoveHandler _parent.Layout, AddressOf onResizeParent
        RemoveHandler _control.Layout, AddressOf onResize
    End Sub
    Private Function hasMin() As Boolean
        If minsize.IsEmpty Or minsize.Height < 1 Or minsize.Width < 1 Then Return False Else Return True
    End Function
    Private Function hasMax() As Boolean
        If maxsize.IsEmpty Or maxsize.Height < 1 Or maxsize.Width < 1 Then Return False Else Return True
    End Function
    Private Function width(ctrl As Control, cctrl As Control) As Integer
        Return ctrl.Width - (cctrl.Margin.Right + cctrl.Margin.Left + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth)
    End Function
    Private Function height(ctrl As Control, cctrl As Control) As Integer
        Return ctrl.Height - (cctrl.Margin.Top + cctrl.Margin.Bottom + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight)
    End Function
    Public Sub forceCheckAndUpdate()
        onResize(Nothing, New LayoutEventArgs(Nothing, Nothing))
        onResizeParent(Nothing, New LayoutEventArgs(Nothing, Nothing))
    End Sub
    Private Sub onResizeParent(sender As Object, e As LayoutEventArgs)
        If sizingp Then Return
        sizingp = True
        _form.SuspendLayout()
        If hasMin() Then
            If width(_parent, _control) >= minsize.Width Or height(_parent, _control) >= minsize.Height Then
                _control.Dock = DockStyle.Fill
                _parent.Padding = New Padding(_parent.Padding.Left, _parent.Padding.Top, System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight)
                _parent.AutoScroll = False
                _parent.AutoScroll = True
            End If
        End If
        If hasMax() Then
            If width(_parent, _control) <= minsize.Width Or height(_parent, _control) <= minsize.Height Then
                _control.Dock = DockStyle.Fill
            End If
        End If
        _form.ResumeLayout(False)
        _form.PerformLayout()
        sizingp = False
    End Sub
    Private Sub onResize(sender As Object, e As LayoutEventArgs)
        If sizing Then Return
        sizing = True
        _form.SuspendLayout()
        If hasMin() Then
            If _control.Width + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth < minsize.Width Or _control.Height + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight < minsize.Height Then
                _control.Dock = DockStyle.Top
                _control.Width = minsize.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth
                _control.Height = minsize.Height - System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight
                _parent.Padding = New Padding(_parent.Padding.Left, _parent.Padding.Top, System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight)
                _parent.AutoScroll = False
                _parent.AutoScroll = True
            End If
        End If
        If hasMax() Then
            If _control.Width > maxsize.Width Or _control.Height > maxsize.Height Then
                _control.Dock = DockStyle.Top
                _control.Width = maxsize.Width
                _control.Height = maxsize.Height
            End If
        End If
        _form.ResumeLayout(False)
        _form.PerformLayout()
        sizing = False
    End Sub
End Class