Imports System.IO
Imports System.Reflection
Imports System.Threading
Imports captainalm.workerpumper
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports captainalm.util.preference
Imports System.Drawing.Printing

''' <summary>
''' The main static class - internal.
''' </summary>
''' <remarks></remarks>
Module Main
    Public args As String() = Environment.GetCommandLineArgs()
    Public targetFile As String = ""
    Public exportOption As Integer = 0
    Public programSwitch As ProgramSwitchMode = ProgramSwitchMode.None
    Public Const quote As Char = """c"
    Public description As String = ""
    Public license As String = ""
    Public programAssembly As Assembly = Assembly.GetEntryAssembly()
    Public programPath As String = programAssembly.Location
    Public execdir As String = Path.GetDirectoryName(programPath)
    Public worker As WorkerPump = Nothing
    Public globalops As New GlobalPreferenceSet
    Private sc As SplashScr = Nothing
    Private wa As Boolean = True

    Public Sub main()
        Try
            init()
            Try
                runtime()
            Catch ex As Exception
                Dim frm As New UnhandledExceptionViewer(False, True, True, ex)
                Dim r As System.Windows.Forms.DialogResult = frm.ShowDialog()
                If Not frm.Disposing And Not frm.IsDisposed Then
                    frm.Dispose()
                End If
                frm = Nothing
            End Try
            shutdown()
        Catch ex As Exception
            Dim frm As New UnhandledExceptionViewer(False, True, True, ex)
            Dim r As System.Windows.Forms.DialogResult = frm.ShowDialog()
            If Not frm.Disposing And Not frm.IsDisposed Then
                frm.Dispose()
            End If
            frm = Nothing
        End Try
        Environment.Exit(0)
    End Sub

    Public Sub init()
        Application.EnableVisualStyles()
        'Splash Thread Start
        sc = New SplashScr()
        Dim t As Thread = New Thread(New ThreadStart(AddressOf splashRunner))
        t.IsBackground = True
        t.Start()

        Thread.Sleep(1500)

        'Decode passed args Flashcard-Maker.exe <TargetFile> <switch> <exportnumber(export switch only)>
        decodeArgs()

        Dim succededslod As Boolean = False
        Try
            If File.Exists(execdir & "\settings.ser") Then
                Dim str As String = File.ReadAllText(execdir & "\settings.ser")
                If str = "" Then
                    Throw New IOException("File Invalid!")
                End If
                globalops.loadPreference(str)
            Else
                Throw New IOException("File Does Not Exist!")
            End If
            succededslod = True
        Catch ex As InvalidCastException
            succededslod = False
        Catch ex As ArgumentNullException
            succededslod = False
        Catch ex As IOException
            succededslod = False
        End Try

        If Not succededslod Then
            Dim gp As GlobalPreferences = globalops.getPreference(Of GlobalPreferences)("GlobalPreferences")
            gp.getPreference(Of IPreference(Of Boolean))("EnableFontSizeLimit").setPreference(True)
            gp.getPreference(Of IPreference(Of Integer))("MinumumFontSize").setPreference(4)
            gp.getPreference(Of IPreference(Of Integer))("MaximumFontSize").setPreference(1638)
            gp.getPreference(Of IPreference(Of Boolean))("EnableThreadErrorMessages").setPreference(True)
            Dim gpp As ProjectPreferences = globalops.getPreference(Of ProjectPreferences)("GlobalProjectPreferences")
            gpp.getPreference(Of IPreference(Of PaperKind))("PageSize").setPreference(PaperKind.A4)
            gpp.getPreference(Of IPreference(Of Integer))("CardWidth").setPreference(10)
            gpp.getPreference(Of IPreference(Of Integer))("CardHeight").setPreference(10)
            gpp.getPreference(Of IPreference(Of Font))("Font").setPreference(New Font("Consolas", 8.25, FontStyle.Regular))
            gpp.getPreference(Of IPreference(Of Color))("Color").setPreference(Color.Black)
            gpp.getPreference(Of IPreference(Of Boolean))("SetTermCountPerCard").setPreference(True)
            gpp.getPreference(Of IPreference(Of Integer))("TermCount").setPreference(1)
            gpp.getPreference(Of IPreference(Of Boolean))("SetTermCountPerRecommenedFontSize").setPreference(False)
            gpp.getPreference(Of IPreference(Of Integer))("RecommenedFontSize").setPreference(4)
            gpp.getPreference(Of IPreference(Of Boolean))("CanSplitWords").setPreference(True)
            Dim fap As FileAssociations = globalops.getPreference(Of FileAssociations)("FileAssociations")
            Dim s As Boolean = fap.getPreferencesFromRegistry()
            If Not s Then
                Try
                    Throw New Exception("File Associations Could Not Be Loaded")
                Catch ex As Exception
                    Dim frm As New UnhandledExceptionViewer(True, False, True, ex)
                    Dim r As System.Windows.Forms.DialogResult = frm.ShowDialog()
                    If Not frm.Disposing And Not frm.IsDisposed Then
                        frm.Dispose()
                    End If
                    frm = Nothing
                    If r = DialogResult.Abort Then
                        Environment.Exit(1)
                    End If
                End Try
            End If
            Try
                File.WriteAllText(execdir & "\settings.ser", globalops.savePreference())
            Catch ex As IOException
                Dim frm As New UnhandledExceptionViewer(True, False, True, ex)
                Dim r As System.Windows.Forms.DialogResult = frm.ShowDialog()
                If Not frm.Disposing And Not frm.IsDisposed Then
                    frm.Dispose()
                End If
                frm = Nothing
                If r = DialogResult.Abort Then
                    Environment.Exit(1)
                End If
            End Try
        End If

        Try
            If File.Exists(execdir & "\license.txt") Then
                license = File.ReadAllText(execdir & "\license.txt")
            End If
            If license = "" Then
                license = My.Resources.LICENSE
            End If
        Catch ex As IOException
        End Try
        Try
            If File.Exists(execdir & "\description.txt") Then
                license = File.ReadAllText(execdir & "\description.txt")
            End If
        Catch ex As IOException
        End Try

        worker = New WorkerPump()
        worker.addFormInstance(New AboutBx(worker))

        'Close the splash form and thread.
        sc.CloseForm()
        If t.IsAlive Then t.Join(2500)
        If t.IsAlive Then t.Abort()
        If Not sc.IsDisposed And Not sc.Disposing Then
            sc.Dispose()
        End If
    End Sub

    Public Sub runtime()
        worker.startPump()
        worker.stopPump()
    End Sub

    Public Sub shutdown()
        If worker.pumping() Then worker.stopPumpForce()
    End Sub

    Sub decodeArgs()
        For i As Integer = 1 To args.Length - 1 Step 1
            Dim carg As String = args(i)
            Dim switch As Boolean = carg.StartsWith("-") And carg.Length > 1
            Dim hasq As Boolean = carg.StartsWith(quote) And carg.EndsWith(quote) And carg.Length > 2
            Dim dat As String = ""
            If switch Then
                dat = carg.Substring(1, carg.Length - 1)
            ElseIf hasq Then
                dat = carg.Substring(1, carg.Length - 2)
            Else
                dat = carg
            End If
            If i = 1 Then
                If Not switch Then
                    targetFile = dat
                End If
            End If
            If i = 2 Then
                If switch Then
                    If dat.ToLower = "o" Or dat.ToLower = "open" Then
                        programSwitch = ProgramSwitchMode.Open
                    ElseIf dat.ToLower = "i" Or dat.ToLower = "import" Then
                        programSwitch = ProgramSwitchMode.Import
                    ElseIf dat.ToLower = "e" Or dat.ToLower = "export" Then
                        programSwitch = ProgramSwitchMode.Export
                    ElseIf dat.ToLower = "n" Or dat.ToLower = "new" Then
                        programSwitch = ProgramSwitchMode.New
                    End If
                End If
            End If
            If i = 3 Then
                If programSwitch = ProgramSwitchMode.Export And Not switch And Not hasq Then
                    If convertStringToInteger(dat) > 0 Then
                        exportOption = convertStringToInteger(dat)
                    End If
                End If
            End If
        Next
    End Sub

    Function convertStringToInteger(str As String) As Integer
        Dim toret As Integer = Integer.MinValue
        Try
            toret = Integer.Parse(str)
        Catch ex As InvalidCastException
            toret = Integer.MinValue
        Catch ex As ArgumentException
            toret = Integer.MinValue
        Catch ex As OverflowException
            toret = Integer.MinValue
        End Try
        Return toret
    End Function

    Sub splashRunner()
        If sc IsNot Nothing Then
            sc.ShowDialog()
        End If
        wa = sc.wasactive
    End Sub
End Module
''' <summary>
''' The program switch mode - internal.
''' </summary>
''' <remarks></remarks>
Enum ProgramSwitchMode As Integer
    None = 0
    Open = 2
    Import = 3
    Export = 4
    [New] = 1
End Enum

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
