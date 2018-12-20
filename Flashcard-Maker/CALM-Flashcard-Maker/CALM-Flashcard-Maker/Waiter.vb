Imports System.Threading
Imports captainalm.util.preference

''' <summary>
''' This provides a Waiter Class.
''' </summary>
''' <remarks></remarks>
Partial Public Class Waiter
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
        callOnForm(Sub() BUTSTOP.Enabled = True)
    End Sub
    ''' <summary>
    ''' Sets the cancel state to true.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub cancel()
        callOnForm(Sub() BUTSTOP.Enabled = False)
        canceled = True
    End Sub
    ''' <summary>
    ''' Sets the text on the waiting form.
    ''' </summary>
    ''' <param name="txt">The text to set the label to.</param>
    ''' <remarks></remarks>
    Public Sub setText(txt As String)
        callOnForm(Sub() lbltxt.Text = txt)
    End Sub
    ''' <summary>
    ''' Gets the text on the waiting form.
    ''' </summary>
    ''' <returns>The text on the waiting form.</returns>
    ''' <remarks></remarks>
    Public Function getText() As String
        Return callOnForm(Function() As String
                              Return lbltxt.Text
                          End Function)
    End Function
    ''' <summary>
    ''' Gets or Sets the title of the form.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property title As String
        Get
            Return callOnForm(Function() As String
                                  Return Me.Text
                              End Function)
        End Get
        Set(value As String)
            callOnForm(Sub() Me.Text = value)
        End Set
    End Property
    ''' <summary>
    ''' Calls a delegate on the form thread.
    ''' </summary>
    ''' <param name="del">The delegate to call.</param>
    ''' <param name="args">The arguments for the delegate.</param>
    ''' <returns>The return value of the delegate.</returns>
    ''' <remarks></remarks>
    Public Function callOnForm(del As [Delegate], ParamArray args As Object()) As Object
        If Me.InvokeRequired Then Return Me.Invoke(Sub() callOnForm(del, args)) Else Return del.DynamicInvoke(args)
    End Function
End Class
''' <summary>
''' This is the delegate used in waiter threads.
''' </summary>
''' <param name="passedWaiter">The instance of waiter.</param>
''' <remarks></remarks>
Public Delegate Sub WaiterRunnable(ByRef passedWaiter As Waiter)
