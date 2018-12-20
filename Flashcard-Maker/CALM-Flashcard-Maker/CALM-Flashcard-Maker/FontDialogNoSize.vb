Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Public Class FontDialogNoSize
    Inherits CommonDialog

    Private components As IContainer = Nothing
    Protected dlgFont As FontDialog
    Protected Const WM_ACTIVATE As Integer = 6
    Protected Const UFLAGSHIDE As SetWindowPosFlags = CType(659, SetWindowPosFlags)
    Protected Delegate Function EnumWindowsCallBack(hWnd As IntPtr, lParam As Integer) As Boolean
    Protected Declare Function GetDlgItem Lib "user32.dll" (hDlg As IntPtr, nIDDlgItem As Integer) As IntPtr
    Protected Declare Function ShowWindow Lib "user32.dll" (hWnd As IntPtr, nCmdShow As Integer) As Boolean
    Protected Declare Sub GetClassName Lib "User32.Dll" (hWnd As IntPtr, param As StringBuilder, length As Integer)
    Protected Declare Function EnumChildWindows Lib "user32.Dll" (hWndParent As IntPtr, lpEnumFunc As EnumWindowsCallBack, lParam As Integer) As Boolean
    Protected Declare Auto Function SetWindowPos Lib "user32.dll" (hWnd As IntPtr, hWndInsertAfter As IntPtr, x As Integer, y As Integer, Width As Integer, Height As Integer, flags As SetWindowPosFlags) As Boolean

    Protected Class FontDialogNative
        Inherits NativeWindow
        Implements IDisposable

        Private mFontDialogHandle As IntPtr

        Private mBaseDialogNative As FontDialogNoSize.BaseDialogNative

        Private mSourceControl As FontDialogNoSize

        Public Sub New(handle As IntPtr, sourceControl As FontDialogNoSize)
            Me.mFontDialogHandle = handle
            Me.mSourceControl = sourceControl
            MyBase.AssignHandle(Me.mFontDialogHandle)
            ShowWindow(GetDlgItem(Me.mFontDialogHandle, 1090), 0)
            ShowWindow(GetDlgItem(Me.mFontDialogHandle, 1138), 0)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.ReleaseHandle()
            If Me.mBaseDialogNative IsNot Nothing Then
                Me.mBaseDialogNative.Dispose()
            End If
        End Sub

        Private Sub PopulateWindowsHandlers()
            EnumChildWindows(Me.mFontDialogHandle, AddressOf Me.OpenColorDialogEnumWindowCallBack, 0)
        End Sub

        Private Function OpenColorDialogEnumWindowCallBack(hwnd As IntPtr, lParam As Integer) As Boolean
            Dim stringBuilder As StringBuilder = New StringBuilder(256)
            GetClassName(hwnd, stringBuilder, stringBuilder.Capacity)
            Dim result As Boolean = False
            If stringBuilder.ToString().StartsWith("#32770") Then
                Me.mBaseDialogNative = New FontDialogNoSize.BaseDialogNative(hwnd)
                result = True
            Else
                result = True
            End If
            Return result
        End Function
    End Class

    Protected Class BaseDialogNative
        Inherits NativeWindow
        Implements IDisposable

        Private mHandle As IntPtr

        Public Sub New(handle As IntPtr)
            Me.mHandle = handle
            MyBase.AssignHandle(handle)
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Me.ReleaseHandle()
        End Sub
    End Class

    Protected Class DialogBooterForm
        Inherits Form

        Private mNativeDialog As FontDialogNoSize.FontDialogNative = Nothing

        Private mFileDialogEx As FontDialogNoSize = Nothing

        Private mWatchForActivate As Boolean = False

        Private mFontDialogHandle As IntPtr = IntPtr.Zero

        Public Property WatchForActivate() As Boolean
            Get
                Return Me.mWatchForActivate
            End Get
            Set(value As Boolean)
                Me.mWatchForActivate = value
            End Set
        End Property

        Public Sub New(fileDialogEx As FontDialogNoSize)
            Me.mFileDialogEx = fileDialogEx
            Me.Text = ""
            MyBase.StartPosition = FormStartPosition.Manual
            MyBase.Location = New Point(-32000, -32000)
            MyBase.ShowInTaskbar = False
        End Sub

        Protected Overrides Sub OnClosing(e As CancelEventArgs)
            If Me.mNativeDialog IsNot Nothing Then
                Me.mNativeDialog.Dispose()
            End If
            MyBase.OnClosing(e)
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            If Me.mWatchForActivate AndAlso m.Msg = WM_ACTIVATE Then
                Me.mWatchForActivate = False
                Me.mFontDialogHandle = m.LParam
                Me.mNativeDialog = New FontDialogNoSize.FontDialogNative(m.LParam, Me.mFileDialogEx)
            End If
            MyBase.WndProc(m)
        End Sub
    End Class

    Public ReadOnly Property FontDialog() As FontDialog
        Get
            Return Me.dlgFont
        End Get
    End Property

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso Me.components IsNot Nothing Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub InitializeComponent()
        Me.dlgFont = New System.Windows.Forms.FontDialog()
        'Me.SuspendLayout()
        ''
        ''FontDialogNoSize
        ''
        'Me.Name = "FontDialogNoSize"
        'Me.Size = New System.Drawing.Size(0, 0)
        'Me.ResumeLayout(False)
    End Sub

    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Public Sub New(CanSelectEffects As Boolean, CanSelectColours As Boolean)
        Me.InitializeComponent()
        dlgFont.ShowEffects = CanSelectEffects
        dlgFont.ShowColor = CanSelectColours
    End Sub

    Public Overloads Function ShowDialog() As DialogResult
        Return Me.ShowDialog(Nothing)
    End Function

    Public Overloads Function ShowDialog(owner As IWin32Window) As DialogResult
        Dim result As DialogResult = DialogResult.Cancel
        Dim dummyForm As FontDialogNoSize.DialogBooterForm = New FontDialogNoSize.DialogBooterForm(Me)
        dummyForm.Show(owner)
        SetWindowPos(dummyForm.Handle, IntPtr.Zero, 0, 0, 0, 0, UFLAGSHIDE)
        dummyForm.WatchForActivate = True
        Try
            result = Me.dlgFont.ShowDialog(dummyForm)
        Catch ex_45 As Exception
        End Try
        dummyForm.Close()
        Return result
    End Function

    Public Property FontValue As Font
        Get
            Return dlgFont.Font
        End Get
        Set(value As Font)
            dlgFont.Font = value
        End Set
    End Property

    Public Property ColorValue As Color
        Get
            Return dlgFont.Color
        End Get
        Set(value As Color)
            dlgFont.Color = value
        End Set
    End Property

    <Flags()>
    Protected Enum SetWindowPosFlags
        SWP_NOSIZE = 1
        SWP_NOMOVE
        SWP_NOZORDER = 4
        SWP_NOREDRAW = 8
        SWP_NOACTIVATE = 16
        SWP_FRAMECHANGED = 32
        SWP_SHOWWINDOW = 64
        SWP_HIDEWINDOW = 128
        SWP_NOCOPYBITS = 256
        SWP_NOOWNERZORDER = 512
        SWP_NOSENDCHANGING = 1024
        SWP_DRAWFRAME = 32
        SWP_NOREPOSITION = 512
        SWP_DEFERERASE = 8192
        SWP_ASYNCWINDOWPOS = 16384
    End Enum

    Public Overrides Sub Reset()
        Me.dlgFont.Reset()
    End Sub

    Protected Overrides Function RunDialog(hwndOwner As IntPtr) As Boolean
        If hwndOwner <> IntPtr.Zero Then ShowDialog(New Win32WindowHolder(hwndOwner)) Else ShowDialog()
        Return True
    End Function

    Protected Class Win32WindowHolder
        Implements IWin32Window
        Private _handle As IntPtr = IntPtr.Zero
        Public Sub New(hnd As IntPtr)
            _handle = hnd
        End Sub
        Public ReadOnly Property Handle As IntPtr Implements IWin32Window.Handle
            Get
                Return _handle
            End Get
        End Property
    End Class
End Class
