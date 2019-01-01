Imports captainalm.workerpumper

Public Class MainProgram
    Implements IWorkerPumpReceiver

    Private formClosingDone As Boolean = False
    Private formClosedDone As Boolean = False
    Private wp As WorkerPump = Nothing
    Private ue As Boolean = False
    Friend project As Project = Nothing
    Friend project_path As String = ""

    'Private tcshpo As TableControlSizeHooker = Nothing

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
        'tcshpo = New TableControlSizeHooker(Me, TableScrollablepo1, TableLayoutPanelpo1) With {.ResizeDelay = 2500, .MinumumSize = New Size(552, 274)}
        'tcshpo.hookEvents()
        'tcshpo.forceCheckAndUpdate()
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
        If ue Then
            NewProjectToolStripMenuItem.Enabled = False
            wp.addEvent(NewProjectToolStripMenuItem, New List(Of Object())(New Object() {FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub OpenProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenProjectToolStripMenuItem.Click
        If ue Then
            OpenProjectToolStripMenuItem.Enabled = False
            wp.addEvent(OpenProjectToolStripMenuItem, New List(Of Object())(New Object() {FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ImportProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportProjectToolStripMenuItem.Click
        If ue Then
            ImportProjectToolStripMenuItem.Enabled = False
            wp.addEvent(ImportProjectToolStripMenuItem, New List(Of Object())(New Object() {FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub SaveProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectToolStripMenuItem.Click
        If ue Then
            SaveProjectToolStripMenuItem.Enabled = False
            wp.addEvent(SaveProjectToolStripMenuItem, New List(Of Object())(New Object() {FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub SaveAsProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveAsProjectToolStripMenuItem.Click
        If ue Then
            SaveAsProjectToolStripMenuItem.Enabled = False
            wp.addEvent(SaveAsProjectToolStripMenuItem, New List(Of Object())(New Object() {FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub CloseProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseProjectToolStripMenuItem.Click
        If ue Then
            CloseProjectToolStripMenuItem.Enabled = False
            wp.addEvent(CloseProjectToolStripMenuItem, New List(Of Object())(New Object() {FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
#Region "InternalExportOptions-0.0-Only"
    Private Sub PrintProjectToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintProjectToolStripMenuItem.Click
        If ue Then
            PrintProjectToolStripMenuItem.Enabled = False
            wp.addEvent(PrintProjectToolStripMenuItem, New List(Of Object())(New Object() {ExportProjectToolStripMenuItem, FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub SaveProjectImagesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveProjectImagesToolStripMenuItem.Click
        If ue Then
            SaveProjectImagesToolStripMenuItem.Enabled = False
            wp.addEvent(SaveProjectImagesToolStripMenuItem, New List(Of Object())(New Object() {ExportProjectToolStripMenuItem, FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
#End Region
    Private Sub CloseProgramToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseProgramToolStripMenuItem.Click
        If ue Then
            CloseProgramToolStripMenuItem.Enabled = False
            wp.addEvent(CloseProgramToolStripMenuItem, New List(Of Object())(New Object() {FileToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub UndoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UndoToolStripMenuItem.Click
        If ue Then
            UndoToolStripMenuItem.Enabled = False
            wp.addEvent(UndoToolStripMenuItem, New List(Of Object())(New Object() {EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub RedoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RedoToolStripMenuItem.Click
        If ue Then
            RedoToolStripMenuItem.Enabled = False
            wp.addEvent(RedoToolStripMenuItem, New List(Of Object())(New Object() {EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        If ue Then
            CutToolStripMenuItem.Enabled = False
            wp.addEvent(CutToolStripMenuItem, New List(Of Object())(New Object() {EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        If ue Then
            CopyToolStripMenuItem.Enabled = False
            wp.addEvent(CopyToolStripMenuItem, New List(Of Object())(New Object() {EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        If ue Then
            PasteToolStripMenuItem.Enabled = False
            wp.addEvent(PasteToolStripMenuItem, New List(Of Object())(New Object() {EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If ue Then
            DeleteToolStripMenuItem.Enabled = False
            wp.addEvent(DeleteToolStripMenuItem, New List(Of Object())(New Object() {EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        If ue Then
            SelectAllToolStripMenuItem.Enabled = False
            wp.addEvent(SelectAllToolStripMenuItem, New List(Of Object())(New Object() {EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ImportDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportDataToolStripMenuItem.Click
        If ue Then
            ImportDataToolStripMenuItem.Enabled = False
            wp.addEvent(ImportDataToolStripMenuItem, New List(Of Object())(New Object() {DataEditorToolStripMenuItem, EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ExportDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportDataToolStripMenuItem.Click
        If ue Then
            ExportDataToolStripMenuItem.Enabled = False
            wp.addEvent(ExportDataToolStripMenuItem, New List(Of Object())(New Object() {DataEditorToolStripMenuItem, EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub AddRowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddRowToolStripMenuItem.Click
        If ue Then
            AddRowToolStripMenuItem.Enabled = False
            wp.addEvent(AddRowToolStripMenuItem, New List(Of Object())(New Object() {DataEditorToolStripMenuItem, EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub RemoveRowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveRowToolStripMenuItem.Click
        If ue Then
            RemoveRowToolStripMenuItem.Enabled = False
            wp.addEvent(RemoveRowToolStripMenuItem, New List(Of Object())(New Object() {DataEditorToolStripMenuItem, EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub MoveRowUpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveRowUpToolStripMenuItem.Click
        If ue Then
            MoveRowUpToolStripMenuItem.Enabled = False
            wp.addEvent(MoveRowUpToolStripMenuItem, New List(Of Object())(New Object() {DataEditorToolStripMenuItem, EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub MoveRowDownToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MoveRowDownToolStripMenuItem.Click
        If ue Then
            MoveRowDownToolStripMenuItem.Enabled = False
            wp.addEvent(MoveRowDownToolStripMenuItem, New List(Of Object())(New Object() {DataEditorToolStripMenuItem, EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ClearCellToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearCellToolStripMenuItem.Click
        If ue Then
            ClearCellToolStripMenuItem.Enabled = False
            wp.addEvent(ClearCellToolStripMenuItem, New List(Of Object())(New Object() {DataEditorToolStripMenuItem, EditToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub StartTabToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartTabToolStripMenuItem.Click
        If ue Then
            StartTabToolStripMenuItem.Enabled = False
            wp.addEvent(StartTabToolStripMenuItem, New List(Of Object())(New Object() {ViewToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ProjectOptionsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ProjectOptionsToolStripMenuItem1.Click
        If ue Then
            ProjectOptionsToolStripMenuItem.Enabled = False
            wp.addEvent(ProjectOptionsToolStripMenuItem, New List(Of Object())(New Object() {ViewToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ProjectDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectDataToolStripMenuItem.Click
        If ue Then
            ProjectDataToolStripMenuItem.Enabled = False
            wp.addEvent(ProjectDataToolStripMenuItem, New List(Of Object())(New Object() {ViewToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ProjectViewerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectViewerToolStripMenuItem.Click
        If ue Then
            ProjectViewerToolStripMenuItem.Enabled = False
            wp.addEvent(ProjectViewerToolStripMenuItem, New List(Of Object())(New Object() {ViewToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ProjectExportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectExportToolStripMenuItem.Click
        If ue Then
            ProjectExportToolStripMenuItem.Enabled = False
            wp.addEvent(ProjectExportToolStripMenuItem, New List(Of Object())(New Object() {ViewToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub GlobalOptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GlobalOptionsToolStripMenuItem.Click
        If ue Then
            GlobalOptionsToolStripMenuItem.Enabled = False
            wp.addEvent(GlobalOptionsToolStripMenuItem, New List(Of Object())(New Object() {SettingsToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ProjectOptionsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProjectOptionsToolStripMenuItem.Click
        If ue Then
            ProjectOptionsToolStripMenuItem.Enabled = False
            wp.addEvent(ProjectOptionsToolStripMenuItem, New List(Of Object())(New Object() {SettingsToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ProgramHelpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProgramHelpToolStripMenuItem.Click
        If ue Then
            ProgramHelpToolStripMenuItem.Enabled = False
            wp.addEvent(ProgramHelpToolStripMenuItem, New List(Of Object())(New Object() {HelpToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ReportAnIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportAnIssueToolStripMenuItem.Click
        If ue Then
            ReportAnIssueToolStripMenuItem.Enabled = False
            wp.addEvent(ReportAnIssueToolStripMenuItem, New List(Of Object())(New Object() {HelpToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        If ue Then
            AboutToolStripMenuItem.Enabled = False
            wp.addEvent(AboutToolStripMenuItem, New List(Of Object())(New Object() {HelpToolStripMenuItem, MenuStrip1, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ImportDataToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ImportDataToolStripMenuItem1.Click
        If ue Then
            ImportDataToolStripMenuItem1.Enabled = False
            wp.addEvent(ImportDataToolStripMenuItem1, New List(Of Object())(New Object() {DataManagementToolStripMenuItem, ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ExportDataToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ExportDataToolStripMenuItem1.Click
        If ue Then
            ExportDataToolStripMenuItem1.Enabled = False
            wp.addEvent(ExportDataToolStripMenuItem1, New List(Of Object())(New Object() {DataManagementToolStripMenuItem, ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub AddRowToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles AddRowToolStripMenuItem1.Click
        If ue Then
            AddRowToolStripMenuItem1.Enabled = False
            wp.addEvent(AddRowToolStripMenuItem1, New List(Of Object())(New Object() {RowManagementToolStripMenuItem, ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub RemoveRowToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RemoveRowToolStripMenuItem1.Click
        If ue Then
            RemoveRowToolStripMenuItem1.Enabled = False
            wp.addEvent(RemoveRowToolStripMenuItem1, New List(Of Object())(New Object() {RowManagementToolStripMenuItem, ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub MoveRowUpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MoveRowUpToolStripMenuItem1.Click
        If ue Then
            MoveRowUpToolStripMenuItem1.Enabled = False
            wp.addEvent(MoveRowUpToolStripMenuItem1, New List(Of Object())(New Object() {RowManagementToolStripMenuItem, ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub MoveRowDownToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MoveRowDownToolStripMenuItem1.Click
        If ue Then
            MoveRowDownToolStripMenuItem1.Enabled = False
            wp.addEvent(MoveRowDownToolStripMenuItem1, New List(Of Object())(New Object() {RowManagementToolStripMenuItem, ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub ClearCellToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ClearCellToolStripMenuItem1.Click
        If ue Then
            ClearCellToolStripMenuItem1.Enabled = False
            wp.addEvent(ClearCellToolStripMenuItem1, New List(Of Object())(New Object() {ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub CutToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem1.Click
        If ue Then
            CutToolStripMenuItem1.Enabled = False
            wp.addEvent(CutToolStripMenuItem1, New List(Of Object())(New Object() {ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub CopyToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem1.Click
        If ue Then
            CopyToolStripMenuItem1.Enabled = False
            wp.addEvent(CopyToolStripMenuItem1, New List(Of Object())(New Object() {ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub PasteToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem1.Click
        If ue Then
            PasteToolStripMenuItem1.Enabled = False
            wp.addEvent(PasteToolStripMenuItem1, New List(Of Object())(New Object() {ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub
    Private Sub SelectAllToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem1.Click
        If ue Then
            SelectAllToolStripMenuItem1.Enabled = False
            wp.addEvent(SelectAllToolStripMenuItem1, New List(Of Object())(New Object() {ContextMenuStripde, Me}), EventTypes.Click, e)
        End If
    End Sub

    Public Property WorkerPump As WorkerPump Implements IWorkerPumpReceiver.WorkerPump
        Get
            Return wp
        End Get
        Set(value As WorkerPump)
            If value IsNot Nothing Then
                wp = value
                ue = True
            End If
        End Set
    End Property
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
    Private ssize As Boolean = False
    Private slockssize As New Object()
    Private ssizep As Boolean = False
    Private slockssizep As New Object()
    Private delay As Integer = 1000
    Private t_del As Threading.Thread = Nothing

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
    Private Sub d_thread()
        While True
            If ShouldSize And ShouldSizeParent Then Form.Invoke(Sub() Me.forceCheckAndUpdate())
            Threading.Thread.Sleep(delay)
            ShouldSize = True
            ShouldSizeParent = True
        End While
    End Sub
    Public Property ResizeDelay As Integer
        Get
            Return delay
        End Get
        Set(value As Integer)
            delay = value
        End Set
    End Property
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
    Private Property ShouldSize As Boolean
        Get
            Return ssize
        End Get
        Set(value As Boolean)
            SyncLock slockssize
                ssize = value
            End SyncLock
        End Set
    End Property
    Private Property ShouldSizeParent As Boolean
        Get
            Return ssizep
        End Get
        Set(value As Boolean)
            SyncLock slockssizep
                ssizep = value
            End SyncLock
        End Set
    End Property
    Public Sub hookEvents()
        AddHandler _parent.Layout, AddressOf onResizeParent
        AddHandler _control.Layout, AddressOf onResize
        If t_del Is Nothing Then
            t_del = New Threading.Thread(AddressOf d_thread) With {.IsBackground = True}
            t_del.Start()
        End If
    End Sub
    Public Sub unhookEvents()
        RemoveHandler _parent.Layout, AddressOf onResizeParent
        RemoveHandler _control.Layout, AddressOf onResize
        If t_del IsNot Nothing Then If t_del.IsAlive Then t_del.Abort()
        If t_del.IsAlive Then t_del.Join(delay)
        t_del = Nothing
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
        ShouldSize = True
        ShouldSizeParent = True
        onResizeParent(Nothing, New LayoutEventArgs(Nothing, Nothing))
        onResize(Nothing, New LayoutEventArgs(Nothing, Nothing))
    End Sub
    Private Sub onResizeParent(sender As Object, e As LayoutEventArgs)
        If sizingp Then Return
        If Not ShouldSizeParent Then Return
        sizingp = True
        ShouldSizeParent = False
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
        If Not ShouldSize Then Return
        sizing = True
        ShouldSize = False
        _form.SuspendLayout()
        If hasMin() Then
            _control.MinimumSize = New Size(0, 0)
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
            _control.MaximumSize = New Size(0, 0)
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