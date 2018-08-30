Imports Microsoft.Win32

''' <summary>
''' Provides an Interface for registry Editing Classes
''' </summary>
''' <remarks></remarks>
Public Interface IRegedit
    Sub setValue(keyName As String, valueName As String, value As Object)
    Sub setValue(keyName As String, valueName As String, value As Object, valueKind As RegistryValueKind)
    Function getValue(keyName As String, valueName As String) As Object
    Function getValue(keyName As String, valueName As String, defaultValue As Object) As Object
End Interface
