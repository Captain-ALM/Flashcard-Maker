Imports captainalm.util.regedit
Imports Microsoft.Win32

Public Class RegEdit
    Implements IRegedit

    Public Function getValue(keyName As String, valueName As String) As Object Implements IRegedit.getValue
        Return Registry.GetValue(keyName, valueName, Nothing)
    End Function

    Public Sub setValue(keyName As String, valueName As String, value As Object) Implements IRegedit.setValue
        Registry.SetValue(keyName, valueName, value)
    End Sub

    Public Sub setValue(keyName As String, valueName As String, value As Object, valueKind As RegistryValueKind) Implements IRegedit.setValue
        Registry.SetValue(keyName, valueName, value, valueKind)
    End Sub

    Public Function getValue(keyName As String, valueName As String, defaultValue As Object) As Object Implements IRegedit.getValue
        Return Registry.GetValue(keyName, valueName, defaultValue)
    End Function
End Class
