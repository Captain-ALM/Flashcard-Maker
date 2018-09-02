Imports captainalm.util.preference
Imports captainalm.workerpumper
Imports System.Drawing.Printing

Public NotInheritable Class GlobalOptions
    Private formClosingDone As Boolean = False
    Private formClosedDone As Boolean = False
    Private wp As WorkerPump = Nothing
    Private ue As Boolean = False
    'Should not construct externally.
    Sub New(Optional ByRef workerp As WorkerPump = Nothing)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If workerp IsNot Nothing Then
            wp = workerp
            ue = True
        Else
            ue = False
        End If
    End Sub

    Friend WithEvents FontDialogNoSize As New FontDialogNoSize(True, False)

    Private Sub GlobalOptions_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not formClosedDone Then
            wp.addEvent(New WorkerEvent(Me, EventTypes.Closed, e))
            formClosedDone = True
        End If
    End Sub

    Private Sub GlobalOptions_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not formClosingDone Then
            If Me.Visible Then
                'If close button pressed
                e.Cancel = True
                Me.Hide()
            End If
            wp.addEvent(New WorkerEvent(Me, EventTypes.Closing, e))
            Me.OnFormClosed(New FormClosedEventArgs(e.CloseReason))
            formClosingDone = True
        End If
    End Sub

#Region "closeOverride"
    Public Shadows Sub Close()
        Me.Hide()
        Me.OnFormClosing(New FormClosingEventArgs(CloseReason.UserClosing, False))
    End Sub
#End Region

    Private Sub GlobalOptions_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Me.DialogResult = Windows.Forms.DialogResult.None
        Me.Enabled = False
        formClosingDone = False
        formClosedDone = False
        wp.addEvent(New WorkerEvent(Me, EventTypes.Shown, e))
    End Sub

    Private Sub chkbxesl_CheckedChanged(sender As Object, e As EventArgs) Handles chkbxesl.CheckedChanged
        chkbxesl.Enabled = False
        If chkbxesl.Checked Then
            Label1.Enabled = True
            Label2.Enabled = True
            nudmin.Enabled = True
            nudmax.Enabled = True
        Else
            Label1.Enabled = False
            Label2.Enabled = False
            nudmin.Enabled = False
            nudmax.Enabled = False
        End If
        wp.addEvent(Of GlobalOptions)(chkbxesl, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub nudmin_ValueChanged(sender As Object, e As EventArgs) Handles nudmin.ValueChanged
        nudmin.Enabled = False
        nudmax.Minimum = nudmin.Value
        nudrfs.Minimum = nudmin.Value
        wp.addEvent(Of GlobalOptions)(nudmin, Me, EventTypes.ValueChanged, e)
    End Sub

    Private Sub nudmax_ValueChanged(sender As Object, e As EventArgs) Handles nudmax.ValueChanged
        nudmax.Enabled = False
        nudmax.Maximum = nudmax.Value
        nudrfs.Maximum = nudmax.Value
        wp.addEvent(Of GlobalOptions)(nudmax, Me, EventTypes.ValueChanged, e)
    End Sub

    Private Sub chkbxetem_CheckedChanged(sender As Object, e As EventArgs) Handles chkbxetem.CheckedChanged
        chkbxetem.Enabled = False
        wp.addEvent(Of GlobalOptions)(chkbxetem, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub cbxps_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxps.SelectedIndexChanged
        cbxps.Enabled = False
        wp.addEvent(Of GlobalOptions)(cbxps, Me, EventTypes.SelectedIndexChanged, e)
    End Sub

    Private Sub nudcw_ValueChanged(sender As Object, e As EventArgs) Handles nudcw.ValueChanged
        nudcw.Enabled = False
        wp.addEvent(Of GlobalOptions)(nudcw, Me, EventTypes.ValueChanged, e)
    End Sub

    Private Sub nudch_ValueChanged(sender As Object, e As EventArgs) Handles nudch.ValueChanged
        nudch.Enabled = False
        wp.addEvent(Of GlobalOptions)(nudch, Me, EventTypes.ValueChanged, e)
    End Sub

    Private Sub butcfs_Click(sender As Object, e As EventArgs) Handles butcfs.Click
        butcfs.Enabled = False
        Dim r As DialogResult = FontDialogNoSize.ShowDialog(Me)
        If r = Windows.Forms.DialogResult.OK Then
            Dim lst As New List(Of Object)
            lst.Add(butcfs)
            lst.Add(Me)
            wp.addEvent(FontDialogNoSize, lst, EventTypes.DialogClosed, New FontDialogSuccessEventArgs(FontDialogNoSize.FontValue, FontDialogNoSize.ColorValue))
        End If
        wp.addEvent(Of GlobalOptions)(FontDialogNoSize, Me, EventTypes.Click, e)
        butcfs.Enabled = True
    End Sub

    Private Sub butcfc_Click(sender As Object, e As EventArgs) Handles butcfc.Click
        butcfc.Enabled = False
        Dim r As DialogResult = ColorDialog1.ShowDialog(Me)
        If r = Windows.Forms.DialogResult.OK Then
            Dim lst As New List(Of Object)
            lst.Add(butcfc)
            lst.Add(Me)
            wp.addEvent(ColorDialog1, lst, EventTypes.DialogClosed, New ColorDialogSuccessEventArgs(ColorDialog1.Color))
        End If
        wp.addEvent(Of GlobalOptions)(ColorDialog1, Me, EventTypes.Click, e)
        butcfc.Enabled = True
    End Sub

    Private Sub rbutcpc_CheckedChanged(sender As Object, e As EventArgs) Handles rbutcpc.CheckedChanged
        rbutcpc.Enabled = False
        If rbutcpc.Checked Then
            Label9.Enabled = True
            nudtc.Enabled = True
            Label10.Enabled = False
            nudrfs.Enabled = False
        Else
            Label9.Enabled = False
            nudtc.Enabled = False
            Label10.Enabled = True
            nudrfs.Enabled = True
        End If
        wp.addEvent(Of GlobalOptions)(rbutcpc, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub rbututcprfs_CheckedChanged(sender As Object, e As EventArgs) Handles rbututcprfs.CheckedChanged
        rbututcprfs.Enabled = False
        If rbututcprfs.Checked Then
            Label9.Enabled = False
            nudtc.Enabled = False
            Label10.Enabled = True
            nudrfs.Enabled = True
        Else
            Label9.Enabled = True
            nudtc.Enabled = True
            Label10.Enabled = False
            nudrfs.Enabled = False
        End If
        wp.addEvent(Of GlobalOptions)(rbututcprfs, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub chkbxasw_CheckedChanged(sender As Object, e As EventArgs) Handles chkbxasw.CheckedChanged
        chkbxasw.Enabled = False
        wp.addEvent(Of GlobalOptions)(chkbxasw, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub nudtc_ValueChanged(sender As Object, e As EventArgs) Handles nudtc.ValueChanged
        nudtc.Enabled = False
        wp.addEvent(Of GlobalOptions)(nudtc, Me, EventTypes.ValueChanged, e)
    End Sub

    Private Sub nudrfs_ValueChanged(sender As Object, e As EventArgs) Handles nudrfs.ValueChanged
        nudrfs.Enabled = False
        wp.addEvent(Of GlobalOptions)(nudrfs, Me, EventTypes.ValueChanged, e)
    End Sub

    Private Sub rbutfcpradp_CheckedChanged(sender As Object, e As EventArgs) Handles rbutfcpradp.CheckedChanged
        rbutfcpradp.Enabled = False
        wp.addEvent(Of GlobalOptions)(rbutfcpradp, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub rbutfcpraop_CheckedChanged(sender As Object, e As EventArgs) Handles rbutfcpraop.CheckedChanged
        rbutfcpraop.Enabled = False
        wp.addEvent(Of GlobalOptions)(rbutfcpraop, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub rbutfcppnr_CheckedChanged(sender As Object, e As EventArgs) Handles rbutfcppnr.CheckedChanged
        rbutfcppnr.Enabled = False
        wp.addEvent(Of GlobalOptions)(rbutfcppnr, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub rbutcalmfcmpradp_CheckedChanged(sender As Object, e As EventArgs) Handles rbutcalmfcmpradp.CheckedChanged
        rbutcalmfcmpradp.Enabled = False
        wp.addEvent(Of GlobalOptions)(rbutcalmfcmpradp, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub rbutcalmfcmpraop_CheckedChanged(sender As Object, e As EventArgs) Handles rbutcalmfcmpraop.CheckedChanged
        rbutcalmfcmpraop.Enabled = False
        wp.addEvent(Of GlobalOptions)(rbutcalmfcmpraop, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub rbutcalmfcmppnr_CheckedChanged(sender As Object, e As EventArgs) Handles rbutcalmfcmppnr.CheckedChanged
        rbutcalmfcmppnr.Enabled = False
        wp.addEvent(Of GlobalOptions)(rbutcalmfcmppnr, Me, EventTypes.CheckedChanged, e)
    End Sub

    Private Sub butok_Click(sender As Object, e As EventArgs) Handles butok.Click
        Me.Close()
    End Sub
End Class

Public Class FontDialogSuccessEventArgs
    Inherits EventArgs

    Private _color As Color
    Private _font As Font

    Public Sub New(f As Font, c As Color)
        _font = f
        _color = c
    End Sub

    Public ReadOnly Property Font As Font
        Get
            Return _font
        End Get
    End Property

    Public ReadOnly Property Color As Color
        Get
            Return _color
        End Get
    End Property
End Class

Public Class ColorDialogSuccessEventArgs
    Inherits EventArgs

    Private _color As Color

    Public Sub New(c As Color)
        _color = c
    End Sub

    Public ReadOnly Property Color As Color
        Get
            Return _color
        End Get
    End Property
End Class

<Serializable>
Public NotInheritable Class GlobalPreferences
    Inherits Preferences

    Public Sub New()
        MyBase.New("GlobalPreferences")
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("EnableFontSizeLimit"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("MinumumFontSize"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("MaximumFontSize"))
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("EnableThreadErrorMessages"))
    End Sub

    Public Overrides Sub addPreference(Of t As IPreference)(pref As t)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub removePreference(name As String, Optional index As Integer = 0)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub setPreference(pref As Object)
        Throw New InvalidOperationException()
    End Sub
End Class

<Serializable>
Public NotInheritable Class FileAssociations
    Inherits Preferences

    Public Sub New()
        MyBase.New("FileAssociations")
        MyBase.addPreference(Of IPreference(Of ApplicationRegisterMode))(New Preference(Of ApplicationRegisterMode)("ApplicationRegistered"))
        MyBase.addPreference(Of IPreference(Of RegisterMode))(New Preference(Of RegisterMode)(".fcp"))
        MyBase.addPreference(Of IPreference(Of RegisterMode))(New Preference(Of RegisterMode)(".calmfcmp"))
    End Sub

    Public Overrides Sub addPreference(Of t As IPreference)(pref As t)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub removePreference(name As String, Optional index As Integer = 0)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub setPreference(pref As Object)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Function getPreference() As Object
        Throw New InvalidOperationException()
    End Function

    Public Function getPreferencesFromRegistry() As Boolean
        'TODO: fill in Registry Code
        Return False
    End Function

    Public Function setPreferencesToRegistry() As Boolean
        'TODO: fill in Registry Code
        Return False
    End Function

    <Serializable>
    Public Enum RegisterMode As Integer
        NotRegistered = 0
        Registered = 1
        RegisteredAndDefault = 2
    End Enum

    <Serializable>
    Public Enum ApplicationRegisterMode As Integer
        NotRegistered = 0
        UserRegistered = 1
        LocalMachineRegistered = 2
    End Enum
End Class

<Serializable>
Public NotInheritable Class GlobalPreferenceSet
    Inherits Preferences

    Public Sub New()
        MyBase.New("GlobalOptions")
        MyBase.addPreference(Of IPreference)(New GlobalPreferences())
        MyBase.addPreference(Of IPreference)(New ProjectPreferences())
        MyBase.addPreference(Of IPreference)(New FileAssociations())
    End Sub

    Public Overrides Sub addPreference(Of t As IPreference)(pref As t)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub removePreference(name As String, Optional index As Integer = 0)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub setPreference(pref As Object)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Function getPreference() As Object
        Throw New InvalidOperationException()
    End Function

    Public Overrides Sub loadPreference(data As String)
        MyBase.loadPreference(data)
        Me.getPreference(Of FileAssociations)("FileAssociations").getPreferencesFromRegistry()
    End Sub

    Public Overrides Function savePreference() As String
        Dim lst As New List(Of String)
        For Each pref As IPreference In prefs
            If pref.getName() <> "FileAssociations" Then
                lst.Add(BinarySerializer.serialize(pref.getName()) & ":" & BinarySerializer.serialize(pref.savePreference()))
            End If
        Next
        Return BinarySerializer.serialize(lst)
    End Function
End Class