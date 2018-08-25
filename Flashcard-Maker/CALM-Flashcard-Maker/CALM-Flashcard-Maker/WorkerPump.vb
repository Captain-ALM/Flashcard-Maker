Imports System.Threading

Module WorkerPump
    Private formInstanceRegistry As New List(Of Form)
    Private workerQueue As New Queue(Of WorkerEvent)
    Private workerStates As New Dictionary(Of WorkerEvent, Boolean)
    Private pump As Boolean = False
    Private parsers As New List(Of IEventParser)
    Private wThread As Thread = Nothing

    Public Sub addFormInstance(f As Form)
        formInstanceRegistry.Add(f)
    End Sub

    Public Sub startPump()
        wThread = New Thread(New ThreadStart(AddressOf workerRunner))
        wThread.IsBackground = True
        pump = True
        wThread.Start()
    End Sub

    Public Function pumping() As Boolean
        Return pump And wThread.IsAlive
    End Function

    Public Function stopPump() As Boolean
        pump = False
        Return Not wThread.IsAlive
    End Function

    Public Sub stopPumpForce()
        pump = False
        If wThread.IsAlive Then wThread.Join(5000)
        If wThread.IsAlive Then wThread.Abort()
    End Sub

    Public Sub addEvent(ev As WorkerEvent)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub

    Public Sub addParser(p As IEventParser)
        parsers.Add(p)
    End Sub

    Public Function showForm(Of t As Form)(Optional index As Integer = 0, Optional owner As Form = Nothing) As Boolean
        Dim frm As Form = Nothing
        Dim ci As Integer = 0
        If index < 0 Then Return False
        For Each cf As Form In formInstanceRegistry
            If canCastForm(Of t)(cf) Then
                If ci = index Then
                    If owner IsNot Nothing Then
                        cf.ShowDialog(owner)
                    Else
                        cf.ShowDialog()
                    End If
                    Return True
                Else
                    ci = ci + 1
                End If
            End If
        Next
        Return False
    End Function

    Private Function castForm(Of t As Form)(f As Form) As t
        Try
            Dim nf As t = f
            Return nf
        Catch ex As InvalidCastException
            Return Nothing
        End Try
    End Function

    Private Function canCastForm(Of t As Form)(f As Form) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function

    Private Sub workerRunner()
        While pump
            Try
                Thread.Sleep(100)
                While workerQueue.Count > 0
                    Dim ev As WorkerEvent = workerQueue.Dequeue()
                    If ev.FromForm IsNot ev.FromControl Then
                        ev.FromControl.Invoke(Sub() ev.FromControl.Enabled = False)
                    End If
                    Dim en As Boolean = parseEvents(ev)
                    If ev.FromForm IsNot ev.FromControl And en Then
                        ev.FromControl.Invoke(Sub() ev.FromControl.Enabled = True)
                    End If
                    If workerStates.ContainsKey(ev) Then
                        workerStates.Remove(ev)
                    End If
                    Thread.Sleep(100)
                End While
            Catch ex As ThreadAbortException
                Throw ex
            Catch ex As Exception
            End Try
        End While
    End Sub

    Function parseEvents(ev As WorkerEvent) As Boolean
        Dim toret As Boolean = True
        For Each parser As IEventParser In parsers
            toret = toret And parser.Parse(ev)
        Next
        Return toret
    End Function
End Module

Interface IEventParser
    Function Parse(ev As WorkerEvent) As Boolean
End Interface

Structure WorkerEvent
    Public FromForm As Form
    Public FromControl As Control
    Public EventType As EventType
    Public EventData As EventArgs
    Public Sub New(ff As Form, et As EventType, ed As EventArgs)
        FromForm = ff
        FromControl = ff
        EventType = et
        EventData = ed
    End Sub
    Public Sub New(ff As Form, fc As Control, et As EventType, ed As EventArgs)
        FromForm = ff
        FromControl = fc
        EventType = et
        EventData = ed
    End Sub
End Structure

Enum EventType As Integer
    None = 0
    Click = 1
    Load = 2
    Shown = 3
    Closing = 4
    Closed = 5
End Enum
