Public Class MainProgram
    Private postcsh As TableControlSizeHooker = Nothing

    Private Sub MainProgram_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        postcsh = New TableControlSizeHooker(Me, TableScrollablepo1, TableLayoutPanelpo1, New Size(552, 274), New Size(0, 0), False)
        'postcsh.hookEvents()
        'postcsh.forceCheckAndUpdate()
    End Sub
End Class

Public Class TableControlSizeHooker
    Private _form As Form = Nothing
    Private _parent As TableLayoutPanel = Nothing
    Private _control As Control = Nothing
    Private minsize As Size = Nothing
    Private maxsize As Size = Nothing
    Private sizing As Boolean = False
    Private sizingp As Boolean = False
    Public Sub New(f As Form, p As TableLayoutPanel, Optional he As Boolean = False)
        _parent = p
        _control = _parent.Container.Components(0)
        _form = f
        If he Then hookEvents()
    End Sub
    Public Sub New(f As Form, p As TableLayoutPanel, c As Control, Optional he As Boolean = False)
        _parent = p
        If _parent.Contains(c) Then _control = c Else _control = _parent.Container.Components(0)
        _form = f
        If he Then hookEvents()
    End Sub
    Public Sub New(f As Form, p As TableLayoutPanel, c As Control, mins As Size, maxs As Size, Optional he As Boolean = False)
        _parent = p
        If _parent.Contains(c) Then _control = c Else _control = _parent.Container.Components(0)
        minsize = mins
        maxsize = maxs
        _form = f
        If he Then hookEvents()
    End Sub
    Public ReadOnly Property ParentControl As TableLayoutPanel
        Get
            Return _parent
        End Get
    End Property
    Public ReadOnly Property Control As Control
        Get
            Return _control
        End Get
    End Property
    Public ReadOnly Property Form As Form
        Get
            Return _form
        End Get
    End Property
    Public Property MinumumSize As Size
        Get
            Return minsize
        End Get
        Set(value As Size)
            minsize = value
        End Set
    End Property
    Public Property MaximumSize As Size
        Get
            Return maxsize
        End Get
        Set(value As Size)
            maxsize = value
        End Set
    End Property
    Public Sub hookEvents()
        AddHandler _parent.Layout, AddressOf onResizeParent
        AddHandler _control.Layout, AddressOf onResize
    End Sub
    Public Sub unhookEvents()
        RemoveHandler _parent.Layout, AddressOf onResizeParent
        RemoveHandler _control.Layout, AddressOf onResize
    End Sub
    Private Function hasMin() As Boolean
        If minsize.IsEmpty Or minsize.Height < 1 Or minsize.Width < 1 Then Return False Else Return True
    End Function
    Private Function hasMax() As Boolean
        If maxsize.IsEmpty Or maxsize.Height < 1 Or maxsize.Width < 1 Then Return False Else Return True
    End Function
    Private Function width(ctrl As Control, cctrl As Control) As Integer
        Return ctrl.Width - (cctrl.Margin.Right + cctrl.Margin.Left + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth)
    End Function
    Private Function height(ctrl As Control, cctrl As Control) As Integer
        Return ctrl.Height - (cctrl.Margin.Top + cctrl.Margin.Bottom + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight)
    End Function
    Public Sub forceCheckAndUpdate()
        onResize(Nothing, New LayoutEventArgs(Nothing, Nothing))
        onResizeParent(Nothing, New LayoutEventArgs(Nothing, Nothing))
    End Sub
    Private Sub onResizeParent(sender As Object, e As LayoutEventArgs)
        If sizingp Then Return
        sizingp = True
        _form.SuspendLayout()
        If hasMin() Then
            If width(_parent, _control) >= minsize.Width Or height(_parent, _control) >= minsize.Height Then
                _control.Dock = DockStyle.Fill
                _parent.Padding = New Padding(_parent.Padding.Left, _parent.Padding.Top, System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight)
                _parent.AutoScroll = False
                _parent.AutoScroll = True
            End If
        End If
        If hasMax() Then
            If width(_parent, _control) <= minsize.Width Or height(_parent, _control) <= minsize.Height Then
                _control.Dock = DockStyle.Fill
            End If
        End If
        _form.ResumeLayout(False)
        _form.PerformLayout()
        sizingp = False
    End Sub
    Private Sub onResize(sender As Object, e As LayoutEventArgs)
        If sizing Then Return
        sizing = True
        _form.SuspendLayout()
        If hasMin() Then
            If _control.Width + System.Windows.Forms.SystemInformation.VerticalScrollBarWidth < minsize.Width Or _control.Height + System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight < minsize.Height Then
                _control.Dock = DockStyle.Top
                _control.Width = minsize.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth
                _control.Height = minsize.Height - System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight
                _parent.Padding = New Padding(_parent.Padding.Left, _parent.Padding.Top, System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, System.Windows.Forms.SystemInformation.HorizontalScrollBarHeight)
                _parent.AutoScroll = False
                _parent.AutoScroll = True
            End If
        End If
        If hasMax() Then
            If _control.Width > maxsize.Width Or _control.Height > maxsize.Height Then
                _control.Dock = DockStyle.Top
                _control.Width = maxsize.Width
                _control.Height = maxsize.Height
            End If
        End If
        _form.ResumeLayout(False)
        _form.PerformLayout()
        sizing = False
    End Sub
End Class