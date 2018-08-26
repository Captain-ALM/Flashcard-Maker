Imports System.IO
Imports System.Reflection
Imports System.Threading

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
    Private sc As SplashScr = Nothing
    Private wa As Boolean = True

    Public Sub main()
        init()
        runtime()
        shutdown()
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

        WorkerPump.addFormInstance(New AboutBx())

        'Close the splash form and thread.
        sc.CloseForm()
        If t.IsAlive Then t.Join(2500)
        If t.IsAlive Then t.Abort()
        If Not sc.IsDisposed And Not sc.Disposing Then
            sc.Dispose()
        End If
    End Sub

    Public Sub runtime()
        WorkerPump.startPump()
        WorkerPump.stopPump()
    End Sub

    Public Sub shutdown()
        If WorkerPump.pumping() Then WorkerPump.stopPumpForce()
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

Enum ProgramSwitchMode As Integer
    None = 0
    Open = 2
    Import = 3
    Export = 4
    [New] = 1
End Enum
