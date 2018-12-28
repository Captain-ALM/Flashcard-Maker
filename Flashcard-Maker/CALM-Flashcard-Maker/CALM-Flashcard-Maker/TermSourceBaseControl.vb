Imports System.Drawing.Imaging

#Const Paint = False

Public Class TermSourceBaseControl
    Protected _col As Integer = 0
    Protected _row As Integer = 0
    Protected _scol As Color = Color.Blue
    Protected _s As Boolean = False
    Protected _obcol As Color = Color.Empty
    Protected _ocbcol As Color = Color.Empty
    Public Event TermModified(sender As Object, e As TermSourceControlEventArgs)
    Public Event ControlSelected(sender As Object, e As TermSourceControlEventArgs)
    Public Event ControlDeselected(sender As Object, e As TermSourceControlEventArgs)
    Public Sub New()
        InitializeComponent()
        Me.Enabled = False
    End Sub
    Public Sub New(c As Integer, r As Integer)
        InitializeComponent()
        _col = c
        _row = r
    End Sub
    Public Overridable Function getTerm() As TermSource
        Throw New NotImplementedException()
    End Function
    Public Overridable Sub setTerm(term As TermSource)
        Throw New NotImplementedException()
    End Sub
    Public Overridable Property Term As TermSource
        Get
            Return getTerm()
        End Get
        Set(value As TermSource)
            setTerm(value)
        End Set
    End Property
    Protected Overridable Sub OnTermModified(sender As Object, e As TermSourceControlEventArgs)
        RaiseEvent TermModified(sender, e)
    End Sub
    Protected Overridable Sub OnControlSelected(sender As Object, e As TermSourceControlEventArgs)
        RaiseEvent ControlSelected(sender, e)
    End Sub
    Protected Overridable Sub OnControlDeselected(sender As Object, e As TermSourceControlEventArgs)
        RaiseEvent ControlDeselected(sender, e)
    End Sub
    Public Overridable Property Row As Integer
        Get
            Return _row
        End Get
        Set(value As Integer)
            _row = value
        End Set
    End Property
    Public Overridable Property Column As Integer
        Get
            Return _col
        End Get
        Set(value As Integer)
            _col = value
        End Set
    End Property
    Public Overridable ReadOnly Property InternalControl As Control
        Get
            Throw New NotImplementedException()
        End Get
    End Property
    Public Overridable Property SelectionColor As Color
        Get
            Return _scol
        End Get
        Set(value As Color)
            _scol = value
        End Set
    End Property
    Public Overridable Shadows Sub [Select]()
        Selected = True
    End Sub
    Public Overridable Sub Deselect()
        Selected = False
    End Sub
    Public Overridable Property Selected As Boolean
        Get
            Return _s
        End Get
        Set(value As Boolean)
            If _s = value Then Return
            _s = value
#If Paint Then
            Me.Invalidate(True)
#Else
            If _s Then
                _obcol = Me.BackColor
                Me.BackColor = _scol
                If InternalControl IsNot Nothing Then
                    _ocbcol = InternalControl.BackColor
                    InternalControl.BackColor = _scol
                End If
                RaiseEvent ControlSelected(Me, New TermSourceControlEventArgs(Column, Row))
            Else
                Me.BackColor = _obcol
                _obcol = Color.Empty
                If InternalControl IsNot Nothing Then
                    InternalControl.BackColor = _ocbcol
                    _ocbcol = Color.Empty
                End If
                RaiseEvent ControlDeselected(Me, New TermSourceControlEventArgs(Column, Row))
            End If
#End If
        End Set
    End Property
#If Paint Then
    Private called As Boolean = False
    Private Sub TermSourceBaseControl_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        If called Then Return
        called = True
        MyBase.OnPaint(e)
        Dim bmp As New Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height)
        Me.DrawToBitmap(bmp, e.ClipRectangle)
        Dim tf As New ColorMatrix(New Single()() {New Single() {1, 0, 0, 0, 0}, New Single() {0, 1, 0, 0, 0}, New Single() {0, 0, 1, 0, 0}, New Single() {0, 0, 0, 1, 0}, New Single() {swapColors(Me.BackColor.R, _scol.R), swapColors(Me.BackColor.G, _scol.G), swapColors(Me.BackColor.B, _scol.B), 0, 1}})
        Using ia As New ImageAttributes()
            ia.SetColorMatrix(tf)
            e.Graphics.DrawImage(bmp, New Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, ia)
        End Using
        called = False
    End Sub
    Private Function swapColors(_base As Byte, _new As Byte) As Single
        Return CInt(_new) - CInt(_base) / 255.0!
    End Function
#End If
    Private Sub TermSourceBaseControl_MouseClick(sender As Object, e As MouseEventArgs) Handles MyBase.MouseClick
        Selected = True
#If Paint Then
        Me.Invalidate(True)
#End If
    End Sub
End Class

Public Class TermSourceControlEventArgs
    Inherits EventArgs
    Protected _row As Integer = 0
    Protected _col As Integer = 0
    Protected _ht As Boolean = False
    Protected _term As TermSource = Nothing
    Public Sub New(col As Integer, row As Integer)
        _row = row
        _col = col
        _ht = False
    End Sub
    Public Sub New(col As Integer, row As Integer, t As TermSource)
        _row = row
        _col = col
        _term = t
        _ht = True
    End Sub
    Public ReadOnly Property Row As Integer
        Get
            Return _row
        End Get
    End Property
    Public ReadOnly Property Column As Integer
        Get
            Return _col
        End Get
    End Property
    Public ReadOnly Property HasTerm As Boolean
        Get
            Return _ht
        End Get
    End Property
    Public ReadOnly Property Term As TermSource
        Get
            Return _term
        End Get
    End Property
End Class