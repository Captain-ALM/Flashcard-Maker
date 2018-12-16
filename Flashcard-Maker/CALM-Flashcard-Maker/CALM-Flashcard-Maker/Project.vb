Imports captainalm.util.preference
Imports System.Drawing.Printing

<Serializable>
Public NotInheritable Class Project
    Private pset As ProjectPreferences = Nothing
    Private pbit As New List(Of Pair(Of Bitmap, Bitmap))
    Private ppg As New List(Of Bitmap)
    Private pterm As New List(Of TermSet(Of TermSource, TermSource))
    Private pcard As New List(Of Card)
    Private pnom As String = ""
    Private Const pver As Integer = 1

    Public Sub New(name As String)
        pnom = name
        pset = New ProjectPreferences(name)
    End Sub

    'TODO: FILL IN PROJECT CODE.
End Class

<Serializable>
Public NotInheritable Class ProjectPreferences
    Inherits Preferences

    Public Sub New()
        Me.New("Global")
    End Sub

    Public Sub New(name As String)
        MyBase.New(name & "ProjectPreferences")
        MyBase.addPreference(Of IPreference(Of PaperKind))(New Preference(Of PaperKind)("PageSize"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("CardWidth"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("CardHeight"))
        MyBase.addPreference(Of IPreference(Of Font))(New Preference(Of Font)("Font"))
        MyBase.addPreference(Of IPreference(Of Color))(New Preference(Of Color)("Color"))
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("SetTermCountPerCard"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("TermCount"))
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("SetTermCountPerRecommenedFontSize"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("RecommenedFontSize"))
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("CanSplitWords"))
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
Public Class Card
    Private terms As New List(Of TermSet(Of TermSource, TermSource))
    'TODO: FILL IN CARD CODE.
End Class
