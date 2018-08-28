'
' Created by SharpDevelop.
' User: Alfred
' Date: 20/08/2018
' Time: 17:40
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System.Threading
Partial Public Class Waiter
    Public Delegate Sub WaiterRunnable(ByRef passedWaiter As Waiter)

    Private torun As WaiterRunnable = Nothing
    Private _thread As Thread = Nothing
    Private canceled As Boolean = False

    Public Sub New()
        ' The Me.InitializeComponent call is required for Windows Forms designer support.
        Me.InitializeComponent()

        '
        ' TODO : Add constructor code after InitializeComponents
        '
    End Sub

    Public Sub New(tr As WaiterRunnable)
        ' The Me.InitializeComponent call is required for Windows Forms designer support.
        Me.InitializeComponent()

        '
        ' TODO : Add constructor code after InitializeComponents
        '
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
                                                     End Try
                                                 End Sub))
        End If
    End Sub

    Private Sub Waiter_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        _thread.IsBackground = True
        _thread.Start()
    End Sub

    Private Sub BUTSTOP_Click(sender As Object, e As EventArgs) Handles BUTSTOP.Click
        BUTSTOP.Enabled = False
        canceled = True
    End Sub
End Class
