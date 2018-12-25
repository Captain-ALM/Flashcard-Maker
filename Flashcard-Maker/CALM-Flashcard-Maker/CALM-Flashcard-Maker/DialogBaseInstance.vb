Public Class DialogBaseInstance
    Inherits CommonDialog

    Public Overrides Sub Reset()
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Function RunDialog(hwndOwner As IntPtr) As Boolean
        Throw New NotImplementedException()
    End Function
End Class