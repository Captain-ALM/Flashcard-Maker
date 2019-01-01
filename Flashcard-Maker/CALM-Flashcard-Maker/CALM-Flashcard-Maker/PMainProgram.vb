Imports captainalm.workerpumper
Imports System.ComponentModel

Public Class PMainProgram
    Implements IEventParser
    Private wp As WorkerPump = Nothing

    Public Function Parse(ev As WorkerEvent) As Boolean Implements IEventParser.Parse
        Dim ceventtype As EventType = ev.EventType
        Dim ceventsource As Object = ev.EventSource.sourceObj
        Dim ceventsourceparents As New List(Of Object)(reverseArray(Of Object)(ev.EventSource.parentObjs.ToArray))
        If canCastObject(Of MainProgram)(ceventsource) Then
            Dim frm As MainProgram = castObject(Of MainProgram)(ceventsource)
            If ceventtype = EventTypes.Load Then

            ElseIf ceventtype = EventTypes.Shown Then

            ElseIf ceventtype = EventTypes.Closed Then

            End If
        Else
            If ceventsourceparents IsNot Nothing Then
                If ceventsourceparents.Count > 0 Then
                    If canCastObject(Of MainProgram)(ceventsourceparents(0)) Then
                        Dim frm As MainProgram = castObject(Of MainProgram)(ceventsourceparents(0))
                        If canCastObject(Of Component)(ceventsource) Then
                            Dim ctrl As Component = castObject(Of Component)(ceventsource)

                        End If
                    End If
                End If
            End If
        End If
        Return True
    End Function

    Private Function reverseArray(Of t)(arr As t()) As t()
        Dim os As New Stack(Of t)(arr)
        Dim ol As New List(Of t)
        While os.Count > 0
            ol.Add(os.Pop())
        End While
        Return ol.ToArray
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

    Private Function getValueFromControl(Of t)(ctrl As Control, del As [Delegate]) As t
        Return ctrl.Invoke(del)
    End Function

    Public Property WorkerPump As WorkerPump Implements IWorkerPumpReceiver.WorkerPump
        Get
            Return wp
        End Get
        Set(value As WorkerPump)
            wp = value
        End Set
    End Property
End Class
