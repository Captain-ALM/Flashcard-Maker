'
' Created by SharpDevelop.
' User: Alfred
' Date: 20/08/2018
' Time: 17:40
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
Imports System.Threading
Imports captainalm.util.preference

''' <summary>
''' This provides a Waiter Class.
''' </summary>
''' <remarks></remarks>
Partial Public Class Waiter
    ''' <summary>
    ''' This is the delegate used in waiter threads.
    ''' </summary>
    ''' <param name="passedWaiter">The instance of waiter.</param>
    ''' <remarks></remarks>
    Public Delegate Sub WaiterRunnable(ByRef passedWaiter As Waiter)

    Private torun As WaiterRunnable = Nothing
    Private _thread As Thread = Nothing
    Private canceled As Boolean = False
    ''' <summary>
    ''' Creates a new instance.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        ' The Me.InitializeComponent call is required for Windows Forms designer support.
        Me.InitializeComponent()
    End Sub
    ''' <summary>
    ''' Creates a new instance with the instance of the delegate to run.
    ''' </summary>
    ''' <param name="tr">The delegate to run as the WaiterRunnable Delegate.</param>
    ''' <remarks></remarks>
    Public Sub New(tr As WaiterRunnable)
        ' The Me.InitializeComponent call is required for Windows Forms designer support.
        Me.InitializeComponent()
        torun = tr
    End Sub

    Private Sub Waiter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        canceled = False
        If torun IsNot Nothing Then
            _thread = New Thread(New ThreadStart(Sub()
                                                     Try
                                                         torun.Invoke(Me)
                                                         Me.Invoke(Sub() Me.Close())
                                                     Catch ex As Exception
                                                         If globalops.getPreference(Of GlobalPreferences)("GlobalPreferences").getPreference(Of IPreference(Of Boolean))("EnableThreadErrorMessages").getPreference() Then
                                                             Dim frm As New UnhandledExceptionViewer(True, True, False, ex)
                                                             Dim r As System.Windows.Forms.DialogResult = frm.ShowDialog()
                                                             If Not frm.Disposing And Not frm.IsDisposed Then
                                                                 frm.Dispose()
                                                             End If
                                                             frm = Nothing
                                                         End If
                                                     End Try
                                                 End Sub))
        End If
    End Sub

    Private Sub Waiter_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        If _thread IsNot Nothing Then
            _thread.IsBackground = True
            _thread.Start()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub BUTSTOP_Click(sender As Object, e As EventArgs) Handles BUTSTOP.Click
        BUTSTOP.Enabled = False
        canceled = True
    End Sub
    ''' <summary>
    ''' Gets the cancel state.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property wasCanceled As Boolean
        Get
            Return canceled
        End Get
    End Property
    ''' <summary>
    ''' Resets the Cancel Button and Cancel State.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub resetCancel()
        canceled = False
        BUTSTOP.Enabled = True
    End Sub
    ''' <summary>
    ''' Sets the cancel state to true.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub cancel()
        BUTSTOP.Enabled = False
        canceled = True
    End Sub
End Class
