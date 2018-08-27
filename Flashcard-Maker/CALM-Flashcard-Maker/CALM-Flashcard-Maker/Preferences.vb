Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Runtime.Serialization

Public Class Preferences
    Implements IPreference(Of List(Of IPreference(Of Object)))

    Protected nom As String = ""
    Protected prefs As New List(Of IPreference(Of Object))

    Sub New()
        nom = ""
        prefs = New List(Of IPreference(Of Object))
    End Sub

    Public Sub New(name As String)
        nom = name
        prefs = New List(Of IPreference(Of Object))
    End Sub

    Public Overridable Function getName() As String Implements IPreference(Of List(Of IPreference(Of Object))).getName
        Return nom
    End Function

    Public Overridable Sub loadPreference(data As String) Implements IPreference(Of List(Of IPreference(Of Object))).loadPreference
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

    Protected Overridable Function getPreferenceIndex(name As String, Optional index As Integer = 0) As Integer
        Dim cnt As Integer = 0
        Dim indx As Integer = 0
        For Each pref As IPreference(Of Object) In prefs
            If pref.getName() = name Then
                If cnt = index Then Return indx Else cnt += 1
            End If
            indx += 1
        Next
        Return -1
    End Function

    Public Overridable Function savePreference() As String Implements IPreference(Of List(Of IPreference(Of Object))).savePreference
        Dim lst As New List(Of String)
        For Each pref As IPreference(Of Object) In prefs
            lst.Add(ser.serialize(pref.getName()) & ":" & ser.serialize(pref.savePreference()))
        Next
        Return ser.serialize(lst)
    End Function

    Public Overridable Sub setName(name As String) Implements IPreference(Of List(Of IPreference(Of Object))).setName
        nom = name
    End Sub

    Public Overridable Function getPreference() As List(Of IPreference(Of Object)) Implements IPreference(Of List(Of IPreference(Of Object))).getPreference
        Throw New InvalidOperationException("Get preference object using getPreference<IPreference<Object> t>(String name, [Optional] Integer index = 0)")
    End Function

    Public Overridable Function getPreference(Of t As IPreference(Of Object))(name As String, Optional index As Integer = 0) As t
        Dim indx As Integer = getPreferenceIndex(name, index)
        If indx < 0 Then Return Nothing
        Return prefs(indx)
    End Function

    Public Overridable Sub setPreference(pref As List(Of IPreference(Of Object))) Implements IPreference(Of List(Of IPreference(Of Object))).setPreference
        Throw New InvalidOperationException("Set preference object using setPreference<IPreference<Object> t>(t pref, [Optional] Integer index = 0)")
    End Sub

    Public Overridable Sub setPreference(Of t As IPreference(Of Object))(pref As t, Optional index As Integer = 0)
        Dim indx As Integer = getPreferenceIndex(pref.getName(), index)
        If indx < 0 Then Return
        prefs(indx) = pref
    End Sub

    Protected Class ser
        Private Shared formatter As New BinaryFormatter()
        Public Shared Function serialize(obj As Object) As String
            Try
                Dim ms As New MemoryStream()
                formatter.Serialize(ms, obj)
                Dim toreturn As String = Convert.ToBase64String(ms.ToArray)
                ms.Dispose()
                ms = Nothing
                Return toreturn
            Catch ex As SerializationException
                Return ""
            End Try
        End Function
        Public Shared Function deserialize(ser As String) As Object
            Try
                Dim ms As New MemoryStream(Convert.FromBase64String(ser))
                Dim toreturn As Object = formatter.Deserialize(ms)
                ms.Dispose()
                ms = Nothing
                Return toreturn
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

    Public Overridable Function getName() As String Implements IPreference(Of t).getName
        Return nom
    End Function

    Public Overridable Function getPreference() As t Implements IPreference(Of t).getPreference
        Return val
    End Function

    Public Overridable Sub loadPreference(data As String) Implements IPreference(Of t).loadPreference
        val = ser.deserialize(data)
    End Sub

    Public Overridable Function savePreference() As String Implements IPreference(Of t).savePreference
        Return ser.serialize(val)
    End Function

    Public Overridable Sub setName(name As String) Implements IPreference(Of t).setName
        nom = name
    End Sub

    Public Overridable Sub setPreference(pref As t) Implements IPreference(Of t).setPreference
        val = pref
    End Sub

    Protected Class ser
        Private Shared formatter As New BinaryFormatter()
        Public Shared Function serialize(obj As Object) As String
            Try
                Dim ms As New MemoryStream()
                formatter.Serialize(ms, obj)
                Dim toreturn As String = Convert.ToBase64String(ms.ToArray)
                ms.Dispose()
                ms = Nothing
                Return toreturn
            Catch ex As SerializationException
                Return ""
            End Try
        End Function
        Public Shared Function deserialize(ser As String) As Object
            Try
                Dim ms As New MemoryStream(Convert.FromBase64String(ser))
                Dim toreturn As Object = formatter.Deserialize(ms)
                ms.Dispose()
                ms = Nothing
                Return toreturn
            Catch ex As SerializationException
                Return Nothing
            End Try
        End Function
    End Class
End Class

Public Interface IPreference(Of t)
    Sub setPreference(pref As t)
    Function getPreference() As t
    Function savePreference() As String
    Sub loadPreference(data As String)
    Function getName() As String
    Sub setName(name As String)
End Interface
