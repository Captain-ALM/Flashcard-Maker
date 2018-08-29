Imports System.Threading
Imports System.Windows.Forms

Public Class WorkerPump
    Implements IDisposable

    Private formInstanceRegistry As New List(Of Form)
    Private workerQueue As New Queue(Of WorkerEvent)
    Private workerStates As New Dictionary(Of WorkerEvent, Boolean)
    Private pump As Boolean = False
    Private parsers As New List(Of IEventParser)
    Private wThread As Thread = Nothing

    Public Sub New()
        wThread = New Thread(New ThreadStart(AddressOf workerRunner))
        wThread.IsBackground = True
    End Sub

    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return disp
        End Get
    End Property

    Public ReadOnly Property Disposing As Boolean
        Get
            Return disping
        End Get
    End Property

    Public Sub addFormInstance(f As Form)
        formInstanceRegistry.Add(f)
    End Sub

    Public Sub startPump()
        pump = True
        wThread.Start()
    End Sub

    Public Function pumping() As Boolean
        Return pump Or wThread.IsAlive
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

    Public Sub addEvent(es As Object, et As EventType, ed As EventArgs)
        Dim ev As New WorkerEvent(es, et, ed)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub

    Public Sub addEvent(es As Object, sp As Object, et As EventType, ed As EventArgs)
        Dim evpl As New List(Of Object)
        evpl.Add(sp)
        Dim ev As New WorkerEvent(es, evpl, et, ed)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub

    Public Sub addEvent(so As Object, sops As List(Of Object), et As EventType, ed As EventArgs)
        Dim ev As New WorkerEvent(so, sops, et, ed)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub

    Public Sub addParser(p As IEventParser)
        parsers.Add(p)
    End Sub

    Public Function showForm(Of t As Form)(Optional index As Integer = 0, Optional owner As Form = Nothing) As Boolean
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

    Public Function removeForm(Of t As Form)(Optional index As Integer = 0) As Boolean
        Dim ci As Integer = 0
        If index < 0 Then Return False
        For Each cf As Form In formInstanceRegistry
            If canCastForm(Of t)(cf) Then
                If ci = index Then
                    If cf.Visible Then
                        Return False
                    Else
                        Return formInstanceRegistry.Remove(cf)
                    End If
                Else
                    ci = ci + 1
                End If
            End If
        Next
        Return False
    End Function

    Public Function removeForms(Of t As Form)() As Boolean
        Dim toret As Boolean = True
        Dim cnt As Integer = 0
        For Each cf As Form In formInstanceRegistry
            If canCastForm(Of t)(cf) Then
                If cf.Visible Then
                    toret = toret And False
                Else
                    toret = toret And formInstanceRegistry.Remove(cf)
                End If
                cnt += 1
            End If
        Next
        If cnt < 1 Then toret = False
        Return toret
    End Function

    Public Function removeParser(Of t As IEventParser)(Optional index As Integer = 0) As Boolean
        Dim frm As Form = Nothing
        Dim ci As Integer = 0
        If index < 0 Then Return False
        For Each cf As IEventParser In parsers
            If canCastParser(Of t)(cf) Then
                If ci = index Then
                    Return parsers.Remove(cf)
                End If
            Else
                ci = ci + 1
            End If
        Next
        Return False
    End Function

    Public Function removeParsers(Of t As IEventParser)() As Boolean
        Dim toret As Boolean = True
        Dim cnt As Integer = 0
        For Each cf As IEventParser In parsers
            If canCastParser(Of t)(cf) Then
                toret = toret And parsers.Remove(cf)
                cnt += 1
            End If
        Next
        If cnt < 1 Then toret = False
        Return toret
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

    Private Function castParser(Of t As IEventParser)(f As IEventParser) As t
        Try
            Dim nf As t = f
            Return nf
        Catch ex As InvalidCastException
            Return Nothing
        End Try
    End Function

    Private Function canCastParser(Of t As IEventParser)(f As IEventParser) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
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

    Private Sub workerRunner()
        Try
            While pump
                Try
                    Thread.Sleep(100)
                    While workerQueue.Count > 0
                        Dim ev As WorkerEvent = workerQueue.Dequeue()
                        If Not ev.EventSource.parentObjs.Contains(ev.EventSource.sourceObj) Then
                            If canCastObject(Of Control)(ev.EventSource.sourceObj) Then
                                Dim c As Control = castObject(Of Control)(ev.EventSource.sourceObj)
                                c.Invoke(Sub() c.Enabled = False)
                            End If
                        End If
                        Dim en As Boolean = parseEvents(ev)
                        If Not ev.EventSource.parentObjs.Contains(ev.EventSource.sourceObj) And en Then
                            If canCastObject(Of Control)(ev.EventSource.sourceObj) Then
                                Dim c As Control = castObject(Of Control)(ev.EventSource.sourceObj)
                                c.Invoke(Sub() c.Enabled = True)
                            End If
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
        Catch ex As ThreadAbortException
            Throw ex
        Catch ex As Exception
        End Try
        pump = False
    End Sub

    Function parseEvents(ev As WorkerEvent) As Boolean
        Dim toret As Boolean = True
        For Each parser As IEventParser In parsers
            toret = toret And parser.Parse(ev)
        Next
        Return toret
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean = False ' To detect redundant calls
    Private disping As Boolean = False
    Private disp As Boolean = False

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Me.disping = True
                workerQueue.Clear()
                workerStates.Clear()
                For Each frm As Form In formInstanceRegistry
                    If Not frm.IsDisposed And Not frm.Disposing Then
                        frm.Dispose()
                    End If
                Next
                formInstanceRegistry.Clear()
                For Each par As IEventParser In parsers
                    If canCastObject(Of IDisposable)(par) Then
                        Dim disppar As IDisposable = castObject(Of IDisposable)(par)
                        disppar.Dispose()
                    End If
                Next
                parsers.Clear()
            End If

            ' t.o.d.o. free unmanaged resources (unmanaged objects) and override Finalize() below.
            wThread = Nothing
            pump = Nothing
            workerQueue = Nothing
            workerStates = Nothing
            formInstanceRegistry = Nothing
            parsers = Nothing
            Me.disping = False
            Me.disp = True
        End If
        Me.disposedValue = True
    End Sub

    't.o.d.o. override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        If pumping() Then Throw New InvalidOperationException("Stop the workerpump before disposing.")
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

Public Interface IEventParser
    Function Parse(ev As WorkerEvent) As Boolean
End Interface

Public Class WorkerEvent
    Public EventSource As Source
    Public EventType As EventType
    Public EventData As EventArgs

    Public Sub New(so As Object, et As EventType, ed As EventArgs)
        EventSource = New Source(so)
        EventType = et
        EventData = ed
    End Sub

    Public Sub New(so As Object, sop As List(Of Object), et As EventType, ed As EventArgs)
        EventSource = New Source(so, sop)
        EventType = et
        EventData = ed
    End Sub

    Public Structure Source
        Public sourceObj As Object
        Public parentObjs As List(Of Object)
        Sub New(so As Object, Optional pos As List(Of Object) = Nothing)
            sourceObj = so
            parentObjs = pos
            If parentObjs Is Nothing Then
                parentObjs = New List(Of Object)
            End If
        End Sub
    End Structure
End Class

Public Class EventType
    Private _data As String = ""
    Public Sub New()
        _data = "none"
    End Sub
    Public Sub New(data As String)
        _data = data.ToLower
    End Sub
    Public Function getEvent() As String
        Return _data
    End Function
    Shared Widening Operator CType(str As String) As EventType
        Return New EventType(str)
    End Operator
    Shared Widening Operator CType(et As EventType) As String
        Return et.getEvent()
    End Operator
    Shared Operator =(v1 As EventType, v2 As EventType) As Boolean
        Return v1.getEvent() = v2.getEvent()
    End Operator
    Shared Operator <>(v1 As EventType, v2 As EventType) As Boolean
        Return v1.getEvent() <> v2.getEvent()
    End Operator
End Class

Public Class EventTypes
    Public Shared ReadOnly None As New EventType("None")
    Public Shared ReadOnly Click As New EventType("Click")
    Public Shared ReadOnly Load As New EventType("Load")
    Public Shared ReadOnly Shown As New EventType("Shown")
    Public Shared ReadOnly Closing As New EventType("Closing")
    Public Shared ReadOnly Closed As New EventType("Closed")
End Class
