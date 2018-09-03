Imports captainalm.util.preference
Imports captainalm.util.regedit

<Serializable>
Public NotInheritable Class FileAssociations
    Inherits Preferences

    <NonSerialized>
    Private Regedit As IRegedit = Nothing

    Public Sub New()
        MyBase.New("FileAssociations")
        MyBase.addPreference(Of IPreference(Of ApplicationRegisterMode))(New Preference(Of ApplicationRegisterMode)("ApplicationRegistered"))
        MyBase.addPreference(Of IPreference(Of RegisterMode))(New Preference(Of RegisterMode)(".fcp"))
        MyBase.addPreference(Of IPreference(Of RegisterMode))(New Preference(Of RegisterMode)(".calmfcmp"))
        Regedit = New RegEdit()
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
        'TODO: fill in Registry Get Code
        Return False
    End Function

    Private Function getApplicationRegistered() As ApplicationRegisterMode
        Dim user As Boolean = False
        Dim machine As Boolean = False
        Dim cuap As Object = Regedit.getValue("HKEY_CURRENT_USER\\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cfcmkr.exe", "")
        Dim lmap As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cfcmkr.exe", "")
        Dim cua As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\Applications\cfcmkr.exe", "")
        Dim lma As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\Applications\cfcmkr.exe", "")
        Return ApplicationRegisterMode.NotRegistered
    End Function

    Public Function setPreferencesToRegistry() As Boolean
        'TODO: fill in Registry Set Code
        Return False
    End Function

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