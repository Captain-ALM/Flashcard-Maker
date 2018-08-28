Public NotInheritable Class GlobalOptions

End Class

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
End Class