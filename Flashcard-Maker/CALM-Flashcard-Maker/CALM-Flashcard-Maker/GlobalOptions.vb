Public NotInheritable Class GlobalOptions
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Friend WithEvents FontDialogNoSize As New FontDialogNoSize(True, False)
End Class

<Serializable>
Public NotInheritable Class GlobalPreferences
    Inherits Preferences

    Public Sub New()
        MyBase.New("GlobalPreferences")
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("EnableSizeLimit"))
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

    <Serializable>
    Public Enum RegisterMode As Integer
        NotRegistered = 0
        Registered = 1
        RegisteredAndDefault = 2
    End Enum
End Class