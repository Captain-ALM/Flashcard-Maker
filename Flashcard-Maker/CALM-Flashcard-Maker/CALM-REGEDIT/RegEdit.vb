Imports captainalm.util.regedit
Imports Microsoft.Win32

Public Class RegEdit
    Implements IRegedit

    Public Function getValue(keyName As String, valueName As String) As Object Implements IRegedit.getValue
        Try
            Return Registry.GetValue(keyName, valueName, EmptyRegistryValue.[New])
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub setValue(keyName As String, valueName As String, value As Object) Implements IRegedit.setValue
        Try
            Registry.SetValue(keyName, valueName, value)
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
        End Try
    End Sub

    Public Sub setValue(keyName As String, valueName As String, value As Object, valueKind As RegistryValueKind) Implements IRegedit.setValue
        Try
            Registry.SetValue(keyName, valueName, value, valueKind)
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
        End Try
    End Sub

    Public Function getValue(keyName As String, valueName As String, defaultValue As Object) As Object Implements IRegedit.getValue
        Try
            Return Registry.GetValue(keyName, valueName, defaultValue)
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Class

Public Class EmptyRegistryValue
    Public Sub New()
    End Sub
    Public Shared Function [New]() As EmptyRegistryValue
        Return New EmptyRegistryValue
    End Function
End Class
