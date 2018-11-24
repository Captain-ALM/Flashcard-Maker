Imports captainalm.util.preference
Imports captainalm.util.regedit

<Serializable>
Public NotInheritable Class FileAssociations
    Inherits Preferences

    <NonSerialized>
    Private Regedit As IRegedit = Nothing

    Public Sub New()
        MyBase.New("FileAssociations")
        MyBase.addPreference(Of IPreference(Of ApplicationRegisterMode))(New Preference(Of ApplicationRegisterMode)("ApplicationRegistered"))
        MyBase.addPreference(Of IPreference(Of RegisterMode))(New Preference(Of RegisterMode)(".fcp"))
        MyBase.addPreference(Of IPreference(Of RegisterMode))(New Preference(Of RegisterMode)(".calmfcmp"))
        Regedit = New RegEdit()
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

    Public Function getPreferencesFromRegistry() As Boolean
        MyBase.getPreference(Of IPreference(Of ApplicationRegisterMode))("ApplicationRegistered").setPreference(getApplicationRegistered())
        If MyBase.getPreference(Of IPreference(Of ApplicationRegisterMode))("ApplicationRegistered").getPreference() = ApplicationRegisterMode.UserRegistered Then
            Dim ko1 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp", "")
            If ko1 IsNot Nothing Then
                If canCastObject(Of String)(ko1) Then
                    Dim ko1str As String = castObject(Of String)(ko1)
                    If ko1str <> "cfcmkr_project" Then
                        Dim ko2 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp\OpenWithProgids", "cfcmkr_project")
                        If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.Registered)
                        Else
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.NotRegistered)
                        End If
                    Else
                        MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.Registered)
                    End If
                Else
                    MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.NotRegistered)
                End If
            Else
                MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.NotRegistered)
            End If
            Dim ko3 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp", "")
            If ko3 IsNot Nothing Then
                If canCastObject(Of String)(ko3) Then
                    Dim ko1str As String = castObject(Of String)(ko3)
                    If ko1str <> "cfcmkr_project" Then
                        Dim ko4 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp\OpenWithProgids", "cfcmkr_project")
                        If ko4 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko4)) Then
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.Registered)
                        Else
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.NotRegistered)
                        End If
                    Else
                        MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.Registered)
                    End If
                Else
                    MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.NotRegistered)
                End If
            Else
                MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.NotRegistered)
            End If
            Return True
        ElseIf MyBase.getPreference(Of IPreference(Of ApplicationRegisterMode))("ApplicationRegistered").getPreference() = ApplicationRegisterMode.LocalMachineRegistered Then
            Dim ko1 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp", "")
            If ko1 IsNot Nothing Then
                If canCastObject(Of String)(ko1) Then
                    Dim ko1str As String = castObject(Of String)(ko1)
                    If ko1str <> "cfcmkr_project" Then
                        Dim ko2 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp\OpenWithProgids", "cfcmkr_project")
                        If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.Registered)
                        Else
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.NotRegistered)
                        End If
                    Else
                        MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.Registered)
                    End If
                Else
                    MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.NotRegistered)
                End If
            Else
                MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").setPreference(RegisterMode.NotRegistered)
            End If
            Dim ko3 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp", "")
            If ko3 IsNot Nothing Then
                If canCastObject(Of String)(ko3) Then
                    Dim ko1str As String = castObject(Of String)(ko3)
                    If ko1str <> "cfcmkr_project" Then
                        Dim ko4 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp\OpenWithProgids", "cfcmkr_project")
                        If ko4 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko4)) Then
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.Registered)
                        Else
                            MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.NotRegistered)
                        End If
                    Else
                        MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.Registered)
                    End If
                Else
                    MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.NotRegistered)
                End If
            Else
                MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").setPreference(RegisterMode.NotRegistered)
            End If
            Return True
        End If
        Return False
    End Function

    Private slockgar As New Object()

    Private Function getApplicationRegistered() As ApplicationRegisterMode
        SyncLock slockgar
            Dim user As Boolean = False
            Dim machine As Boolean = False
            Dim cuap As Object = Regedit.getValue("HKEY_CURRENT_USER\\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cfcmkr.exe", "")
            Dim lmap As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\cfcmkr.exe", "")
            Dim cua As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\Applications\cfcmkr.exe", "")
            Dim lma As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\Applications\cfcmkr.exe", "")
            Dim cur As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\cfcmkr_project", "")
            Dim lmr As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\cfcmkr_project", "")
            If cua IsNot Nothing And cuap IsNot Nothing And cur IsNot Nothing Then
                user = True
            End If
            If lma IsNot Nothing And lmap IsNot Nothing And lmr IsNot Nothing Then
                machine = True
            End If
            If machine Then Return ApplicationRegisterMode.LocalMachineRegistered
            If user Then Return ApplicationRegisterMode.UserRegistered
        End SyncLock
        Return ApplicationRegisterMode.NotRegistered
    End Function

    Public Function setPreferencesToRegistry() As Boolean
        MyBase.getPreference(Of IPreference(Of ApplicationRegisterMode))("ApplicationRegistered").setPreference(getApplicationRegistered())
        If MyBase.getPreference(Of IPreference(Of ApplicationRegisterMode))("ApplicationRegistered").getPreference() = ApplicationRegisterMode.UserRegistered Then
            If MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").getPreference() = RegisterMode.NotRegistered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp\OpenWithProgids", "cfcmkr_project")
                        If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.fcp\OpenWithProgids", True)
                                sk.DeleteValue("cfcmkr_project")
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko1str = "cfcmkr_project" Then
                            Regedit.setValue("HKEY_CURRENT_USER\\Software\Classes\.fcp", "", "", Microsoft.Win32.RegistryValueKind.String)
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").getPreference() = RegisterMode.Registered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp\OpenWithProgids", "cfcmkr_project")
                        If ko2 Is Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.fcp", True)
                                sk.CreateSubKey("OpenWithProgids").Close()
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko2 IsNot Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.fcp\OpenWithProgids", True)
                                sk.SetValue("cfcmkr_project", "", Microsoft.Win32.RegistryValueKind.String)
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").getPreference() = RegisterMode.RegisteredAndDefault Then
                Dim ko1 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        If ko1str <> "" Then
                            Dim ko2 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.fcp\OpenWithProgids", ko1str)
                            If ko2 Is Nothing Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.fcp", True)
                                    sk.CreateSubKey("OpenWithProgids").Close()
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                            If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.fcp\OpenWithProgids", True)
                                    sk.SetValue(ko1str, "", Microsoft.Win32.RegistryValueKind.String)
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                        Regedit.setValue("HKEY_CURRENT_USER\\Software\Classes\.fcp", "", "cfcmkr_project", Microsoft.Win32.RegistryValueKind.String)
                    End If
                End If
            End If
            If MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").getPreference() = RegisterMode.NotRegistered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp\OpenWithProgids", "cfcmkr_project")
                        If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.calmfcmp\OpenWithProgids", True)
                                sk.DeleteValue("cfcmkr_project")
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko1str = "cfcmkr_project" Then
                            Regedit.setValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp", "", "", Microsoft.Win32.RegistryValueKind.String)
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").getPreference() = RegisterMode.Registered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp\OpenWithProgids", "cfcmkr_project")
                        If ko2 Is Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.calmfcmp", True)
                                sk.CreateSubKey("OpenWithProgids").Close()
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko2 IsNot Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.calmfcmp\OpenWithProgids", True)
                                sk.SetValue("cfcmkr_project", "", Microsoft.Win32.RegistryValueKind.String)
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").getPreference() = RegisterMode.RegisteredAndDefault Then
                Dim ko1 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        If ko1str <> "" Then
                            Dim ko2 As Object = Regedit.getValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp\OpenWithProgids", ko1str)
                            If ko2 Is Nothing Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.calmfcmp", True)
                                    sk.CreateSubKey("OpenWithProgids").Close()
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                            If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\Classes\.calmfcmp\OpenWithProgids", True)
                                    sk.SetValue(ko1str, "", Microsoft.Win32.RegistryValueKind.String)
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                        Regedit.setValue("HKEY_CURRENT_USER\\Software\Classes\.calmfcmp", "", "cfcmkr_project", Microsoft.Win32.RegistryValueKind.String)
                    End If
                End If
            End If
        ElseIf MyBase.getPreference(Of IPreference(Of ApplicationRegisterMode))("ApplicationRegistered").getPreference() = ApplicationRegisterMode.LocalMachineRegistered Then
            If MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").getPreference() = RegisterMode.NotRegistered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp\OpenWithProgids", "cfcmkr_project")
                        If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.fcp\OpenWithProgids", True)
                                sk.DeleteValue("cfcmkr_project")
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko1str = "cfcmkr_project" Then
                            Regedit.setValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp", "", "", Microsoft.Win32.RegistryValueKind.String)
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").getPreference() = RegisterMode.Registered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp\OpenWithProgids", "cfcmkr_project")
                        If ko2 Is Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.fcp", True)
                                sk.CreateSubKey("OpenWithProgids").Close()
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko2 IsNot Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.fcp\OpenWithProgids", True)
                                sk.SetValue("cfcmkr_project", "", Microsoft.Win32.RegistryValueKind.String)
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".fcp").getPreference() = RegisterMode.RegisteredAndDefault Then
                Dim ko1 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        If ko1str <> "" Then
                            Dim ko2 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp\OpenWithProgids", ko1str)
                            If ko2 Is Nothing Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.fcp", True)
                                    sk.CreateSubKey("OpenWithProgids").Close()
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                            If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.fcp\OpenWithProgids", True)
                                    sk.SetValue(ko1str, "", Microsoft.Win32.RegistryValueKind.String)
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                        Regedit.setValue("HKEY_LOCAL_MACHINE\\Software\Classes\.fcp", "", "cfcmkr_project", Microsoft.Win32.RegistryValueKind.String)
                    End If
                End If
            End If
            If MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").getPreference() = RegisterMode.NotRegistered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp\OpenWithProgids", "cfcmkr_project")
                        If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.calmfcmp\OpenWithProgids", True)
                                sk.DeleteValue("cfcmkr_project")
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko1str = "cfcmkr_project" Then
                            Regedit.setValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp", "", "", Microsoft.Win32.RegistryValueKind.String)
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").getPreference() = RegisterMode.Registered Then
                Dim ko1 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        Dim ko2 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp\OpenWithProgids", "cfcmkr_project")
                        If ko2 Is Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.calmfcmp", True)
                                sk.CreateSubKey("OpenWithProgids").Close()
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                        If ko2 IsNot Nothing Then
                            Try
                                Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.calmfcmp\OpenWithProgids", True)
                                sk.SetValue("cfcmkr_project", "", Microsoft.Win32.RegistryValueKind.String)
                                sk.Flush()
                                sk.Close()
                            Catch ex As Threading.ThreadAbortException
                                Throw ex
                            Catch ex As Exception
                            End Try
                        End If
                    End If
                End If
            ElseIf MyBase.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").getPreference() = RegisterMode.RegisteredAndDefault Then
                Dim ko1 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp", "")
                If ko1 IsNot Nothing Then
                    If canCastObject(Of String)(ko1) Then
                        Dim ko1str As String = castObject(Of String)(ko1)
                        If ko1str <> "" Then
                            Dim ko2 As Object = Regedit.getValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp\OpenWithProgids", ko1str)
                            If ko2 Is Nothing Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.calmfcmp", True)
                                    sk.CreateSubKey("OpenWithProgids").Close()
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                            If ko2 IsNot Nothing And Not (canCastObject(Of EmptyRegistryValue)(ko2)) Then
                                Try
                                    Dim sk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Classes\.calmfcmp\OpenWithProgids", True)
                                    sk.SetValue(ko1str, "", Microsoft.Win32.RegistryValueKind.String)
                                    sk.Flush()
                                    sk.Close()
                                Catch ex As Threading.ThreadAbortException
                                    Throw ex
                                Catch ex As Exception
                                End Try
                            End If
                        End If
                        Regedit.setValue("HKEY_LOCAL_MACHINE\\Software\Classes\.calmfcmp", "", "cfcmkr_project", Microsoft.Win32.RegistryValueKind.String)
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Private Function castObject(Of t)(f As Object) As t
        Try
            Dim nf As t = f
            Return nf
        Catch ex As InvalidCastException
            Return Nothing
        End Try
    End Function

    Private Function canCastObject(Of t)(f As Object) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function

    <Serializable>
    Public Enum RegisterMode As Integer
        NotRegistered = 0
        Registered = 1
        RegisteredAndDefault = 2
    End Enum

    <Serializable>
    Public Enum ApplicationRegisterMode As Integer
        NotRegistered = 0
        UserRegistered = 1
        LocalMachineRegistered = 2
    End Enum
End Class