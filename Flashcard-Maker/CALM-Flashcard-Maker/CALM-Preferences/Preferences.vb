Imports System.Runtime.Serialization.Formatters.Binary
Imports System.IO
Imports System.Runtime.Serialization
''' <summary>
''' This Class Provides a preferences holder.
''' </summary>
''' <remarks></remarks>
<Serializable>
Public Class Preferences
    Implements IPreference

    Protected nom As String = ""
    Protected prefs As New List(Of IPreference)
    ''' <summary>
    ''' Creates a new instance of Preferences with no name.
    ''' </summary>
    ''' <remarks></remarks>
    Sub New()
        nom = ""
        prefs = New List(Of IPreference)
    End Sub
    ''' <summary>
    ''' Creates a new instance of Preferences.
    ''' <para name="name">The name of the preferences.</para>
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(name As String)
        nom = name
        prefs = New List(Of IPreference)
    End Sub
    ''' <summary>
    ''' Returns the name of the Preferences Object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function getName() As String Implements IPreference.getName
        Return nom
    End Function
    ''' <summary>
    ''' Loads preferences into the Preferences object via a string.
    ''' </summary>
    ''' <param name="data">The string data.</param>
    ''' <remarks></remarks>
    Public Overridable Sub loadPreference(data As String) Implements IPreference.loadPreference
        Dim lst As List(Of String) = BinarySerializer.deserialize(data)
        Dim nr As New Dictionary(Of String, Integer)
        For Each pref As String In lst
            Dim dat As String() = pref.Split(":")
            If dat.Length = 2 Then
                Dim pname As String = BinarySerializer.deserialize(dat(0))
                Dim pdat As String = BinarySerializer.deserialize(dat(1))
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
    ''' <summary>
    ''' Saves a Preferences object to a string.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function savePreference() As String Implements IPreference.savePreference
        Dim lst As New List(Of String)
        For Each pref As IPreference In prefs
            lst.Add(BinarySerializer.serialize(pref.getName()) & ":" & BinarySerializer.serialize(pref.savePreference()))
        Next
        Return BinarySerializer.serialize(lst)
    End Function
    ''' <summary>
    ''' Sets the name of the Preferences object.
    ''' </summary>
    ''' <param name="name">The object name.</param>
    ''' <remarks></remarks>
    Public Overridable Sub setName(name As String) Implements IPreference.setName
        nom = name
    End Sub
    ''' <summary>
    ''' Returns the stored preferences in the Preferences Object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function getPreference() As Object Implements IPreference.getPreference
        Return prefs
    End Function
    ''' <summary>
    ''' Gets a preference of a specified type and name.
    ''' </summary>
    ''' <typeparam name="t">The type of preference.</typeparam>
    ''' <param name="name">The preference name.</param>
    ''' <param name="index">The index of the preference of that name and type.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function getPreference(Of t As IPreference)(name As String, Optional index As Integer = 0) As t
        Dim indx As Integer = getPreferenceIndex(name, index)
        If indx < 0 Then Return Nothing
        Return prefs(indx)
    End Function
    ''' <summary>
    ''' Sets the stored preferences in the Preferences Object.
    ''' </summary>
    ''' <param name="pref"></param>
    ''' <remarks></remarks>
    Public Overridable Sub setPreference(pref As Object) Implements IPreference.setPreference
        prefs = pref
    End Sub
    ''' <summary>
    ''' Sets a preference of the specified type using its object.
    ''' </summary>
    ''' <typeparam name="t">The type of preference to set.</typeparam>
    ''' <param name="pref">The preference object to use.</param>
    ''' <param name="index">The index for that preference name and type.</param>
    ''' <remarks></remarks>
    Public Overridable Sub setPreference(Of t As IPreference)(pref As t, Optional index As Integer = 0)
        Dim indx As Integer = getPreferenceIndex(pref.getName(), index)
        If indx < 0 Then Return
        prefs(indx) = pref
    End Sub
    ''' <summary>
    ''' Adds a preference to the Preferences object.
    ''' </summary>
    ''' <typeparam name="t">The type of preference.</typeparam>
    ''' <param name="pref">The preference object.</param>
    ''' <remarks></remarks>
    Public Overridable Sub addPreference(Of t As IPreference)(pref As t)
        prefs.Add(pref)
    End Sub
    ''' <summary>
    ''' Removes a preference from the preferences object.
    ''' </summary>
    ''' <param name="name">The name of the preference.</param>
    ''' <param name="index">The index of the name.</param>
    ''' <remarks></remarks>
    Public Overridable Sub removePreference(name As String, Optional index As Integer = 0)
        Dim indx As Integer = getPreferenceIndex(name, index)
        If indx < 0 Then Return
        prefs.RemoveAt(indx)
    End Sub
End Class
''' <summary>
''' Internal Binary Serializer Helper.
''' </summary>
''' <remarks></remarks>
NotInheritable Class BinarySerializer
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
        Catch ex As ArgumentNullException
            Return ""
        Catch ex As FormatException
            Return ""
        Catch ex As IOException
            Return ""
        Catch ex As SerializationException
            Return ""
        End Try
    End Function
    Public Shared Function serialize(Of t)(obj As t) As String
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
        Catch ex As ArgumentNullException
            Return ""
        Catch ex As FormatException
            Return ""
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
                toreturn = formatter.Deserialize(ms)
                ms.Dispose()
                ms = Nothing
            End SyncLock
            Return toreturn
        Catch ex As ArgumentNullException
            Return Nothing
        Catch ex As FormatException
            Return Nothing
        Catch ex As IOException
            Return Nothing
        Catch ex As SerializationException
            Return Nothing
        End Try
    End Function
    Public Shared Function deserialize(Of t)(ser As String) As t
        Try
            Dim toreturn As t = Nothing
            SyncLock slock
                Dim ms As New MemoryStream(Convert.FromBase64String(ser))
                toreturn = formatter.Deserialize(ms)
                ms.Dispose()
                ms = Nothing
            End SyncLock
            Return toreturn
        Catch ex As ArgumentNullException
            Return Nothing
        Catch ex As FormatException
            Return Nothing
        Catch ex As IOException
            Return Nothing
        Catch ex As SerializationException
            Return Nothing
        End Try
    End Function
End Class
''' <summary>
''' This class is a preference holding a certain value type.
''' </summary>
''' <typeparam name="t">The type this Preference(Of t) holds.</typeparam>
''' <remarks></remarks>
<Serializable>
Public Class Preference(Of t)
    Implements IPreference(Of t)

    Protected val As t = Nothing
    Protected nom As String = ""
    ''' <summary>
    ''' Creates a new instance of Preference(Of t) with no name.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        val = Nothing
        nom = ""
    End Sub
    ''' <summary>
    ''' Creates a new instance of Preference(Of t).
    ''' <para name="name">The name of the Preference.</para>
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(name As String)
        val = Nothing
        nom = name
    End Sub
    ''' <summary>
    ''' Returns the name of the preference.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function getName() As String Implements IPreference.getName
        Return nom
    End Function
    ''' <summary>
    ''' Returns the Object Value of the Value of the Preference.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function getPreferenceAsObject() As Object Implements IPreference.getPreference
        Return val
    End Function
    ''' <summary>
    ''' Loads the value of the preference from a string.
    ''' </summary>
    ''' <param name="data">The data string.</param>
    ''' <remarks></remarks>
    Public Overridable Sub loadPreference(data As String) Implements IPreference.loadPreference
        val = BinarySerializer.deserialize(data)
    End Sub
    ''' <summary>
    ''' Saves the value of the preference to a string.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function savePreference() As String Implements IPreference.savePreference
        Return BinarySerializer.serialize(val)
    End Function
    ''' <summary>
    ''' Sets the name of the preference.
    ''' </summary>
    ''' <param name="name">The name of the preference.</param>
    ''' <remarks></remarks>
    Public Overridable Sub setName(name As String) Implements IPreference.setName
        nom = name
    End Sub
    ''' <summary>
    ''' Sets the value of the preference as an Object.
    ''' </summary>
    ''' <param name="pref">The Object to have the new preference value.</param>
    ''' <remarks></remarks>
    Public Overridable Sub setPreferenceAsObject(pref As Object) Implements IPreference.setPreference
        val = pref
    End Sub
    ''' <summary>
    ''' Gets the preference as the class generic type.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function getPreference() As t Implements IPreference(Of t).getPreference
        Return val
    End Function
    ''' <summary>
    ''' Sets the preference as the class generic type.
    ''' </summary>
    ''' <param name="pref"></param>
    ''' <remarks></remarks>
    Public Overloads Sub setPreference(pref As t) Implements IPreference(Of t).setPreference
        val = pref
    End Sub
End Class
''' <summary>
''' Provides an Interface for preferences.
''' </summary>
''' <remarks></remarks>
Public Interface IPreference
    ''' <summary>
    ''' Sets the value of the preference as an object.
    ''' </summary>
    ''' <param name="pref">The new Object value of the preference.</param>
    ''' <remarks></remarks>
    Sub setPreference(pref As Object)
    ''' <summary>
    ''' Gets the object value of the preference.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getPreference() As Object
    ''' <summary>
    ''' Saves the preference as a string value.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function savePreference() As String
    ''' <summary>
    ''' Loads a preference from a string value.
    ''' </summary>
    ''' <param name="data">The string data.</param>
    ''' <remarks></remarks>
    Sub loadPreference(data As String)
    ''' <summary>
    ''' Gets the name of the preference.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getName() As String
    ''' <summary>
    ''' Sets the name of the preference.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <remarks></remarks>
    Sub setName(name As String)
End Interface
''' <summary>
''' Provides an Interface for Generic Preferences.
''' </summary>
''' <typeparam name="t">The generic value of the preference.</typeparam>
''' <remarks></remarks>
Public Interface IPreference(Of t)
    Inherits IPreference
    ''' <summary>
    ''' Sets the preference with an object of its Generic value.
    ''' </summary>
    ''' <param name="pref">The new preference value.</param>
    ''' <remarks></remarks>
    Overloads Sub setPreference(pref As t)
    ''' <summary>
    ''' Gets the preference as its generic value type.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Overloads Function getPreference() As t
End Interface
