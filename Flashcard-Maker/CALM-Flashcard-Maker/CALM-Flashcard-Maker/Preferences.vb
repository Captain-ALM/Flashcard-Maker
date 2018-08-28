Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Runtime.Serialization

Public Class Preferences
    Implements IPreference


    Protected nom As String = ""
    Protected prefs As New List(Of IPreference)

    Sub New()
        nom = ""
        prefs = New List(Of IPreference)
    End Sub

    Public Sub New(name As String)
        nom = name
        prefs = New List(Of IPreference)
    End Sub

    Public Overridable Function getName() As String Implements IPreference.getName
        Return nom
    End Function

    Public Overridable Sub loadPreference(data As String) Implements IPreference.loadPreference
        Dim lst As List(Of String) = ser.deserialize(data)
        Dim nr As New Dictionary(Of String, Integer)
        For Each pref As String In lst
            Dim dat As String() = pref.Split(":")
            If dat.Length = 2 Then
                Dim pname As String = ser.deserialize(dat(0))
                Dim pdat As String = ser.deserialize(dat(1))
                Dim i As Integer = -1
                If nr.ContainsKey(pname) Then
                    i = getPreferenceIndex(pname, nr(pname))
                Else
                    i = getPreferenceIndex(pname)
                End If
                If i > -1 Then
                    prefs(i).loadPreference(pdat)
                End If
                If nr.ContainsKey(pname) Then
                    nr(pname) += 1
                Else
                    nr.Add(pname, 1)
                End If
            End If
        Next
    End Sub

    Protected slockGetPreferenceIndex As New Object()

    Protected Overridable Function getPreferenceIndex(name As String, Optional index As Integer = 0) As Integer
        Dim toret As Integer = -1
        SyncLock slockGetPreferenceIndex
            Dim cnt As Integer = 0
            Dim indx As Integer = 0
            For Each pref As IPreference In prefs
                If pref.getName() = name Then
                    If cnt = index Then toret = indx Else cnt += 1
                End If
                indx += 1
            Next
        End SyncLock
        Return toret
    End Function

    Public Overridable Function savePreference() As String Implements IPreference.savePreference
        Dim lst As New List(Of String)
        For Each pref As IPreference In prefs
            lst.Add(ser.serialize(pref.getName()) & ":" & ser.serialize(pref.savePreference()))
        Next
        Return ser.serialize(lst)
    End Function

    Public Overridable Sub setName(name As String) Implements IPreference.setName
        nom = name
    End Sub

    Public Overridable Function getPreference() As Object Implements IPreference.getPreference
        Return prefs
    End Function

    Public Overridable Function getPreference(Of t As IPreference)(name As String, Optional index As Integer = 0) As t
        Dim indx As Integer = getPreferenceIndex(name, index)
        If indx < 0 Then Return Nothing
        Return prefs(indx)
    End Function

    Public Overridable Sub setPreference(pref As Object) Implements IPreference.setPreference
        prefs = pref
    End Sub

    Public Overridable Sub setPreference(Of t As IPreference)(pref As t, Optional index As Integer = 0)
        Dim indx As Integer = getPreferenceIndex(pref.getName(), index)
        If indx < 0 Then Return
        prefs(indx) = pref
    End Sub

    Public Overridable Sub addPreference(Of t As IPreference)(pref As t)
        prefs.Add(pref)
    End Sub

    Public Overridable Sub removePreference(name As String, Optional index As Integer = 0)
        Dim indx As Integer = getPreferenceIndex(name, index)
        If indx < 0 Then Return
        prefs.RemoveAt(indx)
    End Sub

    Protected Class ser
        Private Shared formatter As New BinaryFormatter()
        Private Shared slock As New Object()
        Public Shared Function serialize(obj As Object) As String
            Try
                Dim toreturn As String = ""
                SyncLock slock
                    Dim ms As New MemoryStream()
                    formatter.Serialize(ms, obj)
                    toreturn = Convert.ToBase64String(ms.ToArray)
                    ms.Dispose()
                    ms = Nothing
                End SyncLock
                Return toreturn
            Catch ex As IOException
                Return ""
            Catch ex As SerializationException
                Return ""
            End Try
        End Function
        Public Shared Function deserialize(ser As String) As Object
            Try
                Dim toreturn As Object = Nothing
                SyncLock slock
                    Dim ms As New MemoryStream(Convert.FromBase64String(ser))
                    formatter.Deserialize(ms)
                    ms.Dispose()
                    ms = Nothing
                End SyncLock
                Return toreturn
            Catch ex As IOException
                Return Nothing
            Catch ex As SerializationException
                Return Nothing
            End Try
        End Function
    End Class
End Class

Public Class Preference(Of t)
    Implements IPreference(Of t)

    Protected val As t = Nothing
    Protected nom As String = ""

    Public Sub New()
        val = Nothing
        nom = ""
    End Sub

    Public Sub New(name As String)
        val = Nothing
        nom = name
    End Sub

    Public Overridable Function getName() As String Implements IPreference.getName
        Return nom
    End Function

    Public Overridable Function getPreferenceAsObject() As Object Implements IPreference.getPreference
        Return val
    End Function

    Public Overridable Sub loadPreference(data As String) Implements IPreference.loadPreference
        val = ser.deserialize(data)
    End Sub

    Public Overridable Function savePreference() As String Implements IPreference.savePreference
        Return ser.serialize(val)
    End Function

    Public Overridable Sub setName(name As String) Implements IPreference.setName
        nom = name
    End Sub

    Public Overridable Sub setPreferenceAsObject(pref As Object) Implements IPreference.setPreference
        val = pref
    End Sub

    Protected Class ser
        Private Shared formatter As New BinaryFormatter()
        Private Shared slock As New Object()
        Public Shared Function serialize(obj As Object) As String
            Try
                Dim toreturn As String = ""
                SyncLock slock
                    Dim ms As New MemoryStream()
                    formatter.Serialize(ms, obj)
                    toreturn = Convert.ToBase64String(ms.ToArray)
                    ms.Dispose()
                    ms = Nothing
                End SyncLock
                Return toreturn
            Catch ex As IOException
                Return ""
            Catch ex As SerializationException
                Return ""
            End Try
        End Function
        Public Shared Function deserialize(ser As String) As Object
            Try
                Dim toreturn As Object = Nothing
                SyncLock slock
                    Dim ms As New MemoryStream(Convert.FromBase64String(ser))
                    formatter.Deserialize(ms)
                    ms.Dispose()
                    ms = Nothing
                End SyncLock
                Return toreturn
            Catch ex As IOException
                Return Nothing
            Catch ex As SerializationException
                Return Nothing
            End Try
        End Function
    End Class

    Public Overloads Function getPreference() As t Implements IPreference(Of t).getPreference

    End Function

    Public Overloads Sub setPreference(pref As t) Implements IPreference(Of t).setPreference

    End Sub
End Class

Public Interface IPreference
    Sub setPreference(pref As Object)
    Function getPreference() As Object
    Function savePreference() As String
    Sub loadPreference(data As String)
    Function getName() As String
    Sub setName(name As String)
End Interface

Public Interface IPreference(Of t)
    Inherits IPreference
    Overloads Sub setPreference(pref As t)
    Overloads Function getPreference() As t
End Interface
