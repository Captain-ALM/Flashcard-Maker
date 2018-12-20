<Serializable>
Public MustInherit Class TermSource
    Public MustOverride Function getImage(width As Integer, height As Integer, Optional usegivensize As Boolean = True) As Image
    Public MustOverride ReadOnly Property termSourceType As String
End Class

<Serializable>
Public MustInherit Class ImageTermBase
    Inherits TermSource

    Public MustOverride Function getHeldImage() As Image

    Public Overrides ReadOnly Property termSourceType As String
        Get
            Return "ImageTerm"
        End Get
    End Property
End Class

<Serializable>
Public Class ImageStoreTerm
    Inherits ImageTermBase
    Implements IStoredData

    Protected storedImage As Byte() = Nothing
    Protected path As String = ""

    Public Overrides Function getHeldImage() As Image
        If storedImage Is Nothing Then loadData()
        Return New Bitmap(New IO.MemoryStream(storedImage))
    End Function

    Public Overrides Function getImage(width As Integer, height As Integer, Optional usegivensize As Boolean = True) As Image
        If width < 1 Or height < 1 Then Throw New NonFittingImageException("Width Or Height are less than 0.")
        Dim img As New Bitmap(width, height)
        Using oimg As Bitmap = getHeldImage()
            Using g As Graphics = Graphics.FromImage(img)
                g.DrawImage(oimg, 0, 0, width, height)
            End Using
        End Using
        Return img
    End Function

    Public Overrides ReadOnly Property termSourceType As String
        Get
            Return MyBase.termSourceType & "." & "ImageStoreTerm"
        End Get
    End Property

    Public Property dataPath As String Implements IReferencedData.dataPath
        Get
            Return path
        End Get
        Set(value As String)
            path = value
        End Set
    End Property

    Public Sub loadData() Implements IReferencedData.loadData
        storedImage = IO.File.ReadAllBytes(path)
    End Sub

    Public Sub saveData() Implements IReferencedData.saveData
        IO.File.WriteAllBytes(path, storedImage)
    End Sub

    Public Property storedData As Byte() Implements IStoredData.storedData
        Get
            Return storedImage
        End Get
        Set(value As Byte())
            storedImage = value
        End Set
    End Property
End Class

<Serializable>
Public Class ImageReferenceTerm
    Inherits ImageTermBase
    Implements IReferencedData

    Protected path As String = ""
    <NonSerialized>
    Protected storedImage As Byte() = Nothing

    Public Overrides Function getHeldImage() As Image
        loadData()
        Return New Bitmap(New IO.MemoryStream(storedImage))
    End Function

    Public Overrides Function getImage(width As Integer, height As Integer, Optional usegivensize As Boolean = True) As Image
        If width < 1 Or height < 1 Then Throw New NonFittingImageException("Width Or Height are less than 0.")
        Dim img As New Bitmap(width, height)
        Using oimg As Bitmap = getHeldImage()
            Using g As Graphics = Graphics.FromImage(img)
                g.DrawImage(oimg, 0, 0, width, height)
            End Using
        End Using
        Return img
    End Function

    Public Overrides ReadOnly Property termSourceType As String
        Get
            Return MyBase.termSourceType & "." & "ImageReferenceTerm"
        End Get
    End Property

    Public Property dataPath As String Implements IReferencedData.dataPath
        Get
            Return path
        End Get
        Set(value As String)
            path = value
        End Set
    End Property

    Public Sub loadData() Implements IReferencedData.loadData
        storedImage = IO.File.ReadAllBytes(path)
    End Sub

    Public Sub saveData() Implements IReferencedData.saveData
        IO.File.WriteAllBytes(path, storedImage)
    End Sub
End Class

<Serializable>
Public Class TextTerm
    Inherits TermSource

    Protected txt As String = ""
    Protected fnt As Font = Nothing
    Protected col As Color = Color.Empty
    Protected mfs As Integer = 1
    Protected ws As Boolean = False

    Public Sub New(t As String, f As Font, c As Color, minfsize As Integer, wsplit As Boolean)
        txt = t
        fnt = f
        col = c
        mfs = minfsize
        ws = wsplit
    End Sub

    Public Property Text As String
        Get
            Return txt
        End Get
        Set(value As String)
            txt = value
        End Set
    End Property

    Public Property Font As Font
        Get
            Return fnt
        End Get
        Set(value As Font)
            fnt = value
        End Set
    End Property

    Public Property Colour As Color
        Get
            Return col
        End Get
        Set(value As Color)
            col = value
        End Set
    End Property

    Public Property MinumumFontSize As Integer
        Get
            Return mfs
        End Get
        Set(value As Integer)
            mfs = value
        End Set
    End Property

    Public Property WordSplit As Boolean
        Get
            Return ws
        End Get
        Set(value As Boolean)
            ws = value
        End Set
    End Property

    Public Overloads Overrides Function getImage(width As Integer, height As Integer, Optional usegivensize As Boolean = True) As Image
        If width < 1 Or height < 1 Then Throw New NonFittingTextException("Width Or Height are less than 0.")
        Return TextRender.renderImage(New Size(width, height), txt, fnt, col, mfs, ws, Not usegivensize)
    End Function

    Public Overrides ReadOnly Property termSourceType As String
        Get
            Return "TextTerm"
        End Get
    End Property
End Class

Public Class TextRender
    Public Shared Function renderImage(ssize As Size, txt As String, fnt As Font, col As Color, efsize As Integer, wordsplit As Boolean, Optional usecalculatedheight As Boolean = False) As Image
        Dim itr As Integer = 1
        Dim lines As New List(Of Pair(Of String, Integer))
        Dim aheight As Integer = 0
        Dim rheight As Integer = ssize.Height
        Dim paras As List(Of TextParagraph) = getParagraphs(txt)
        While itr <= (fnt.Size - efsize)
            Try
                lines.Clear()
                aheight = 0
                rheight = ssize.Height
                For i As Integer = 0 To paras.Count - 1 Step 1
                    Dim cpara As TextParagraph = paras(i)
                    If wordsplit Then
                        Dim cc As Integer = cpara.characterCount
                        Dim os As Integer = 0
                        While os < cpara.characterCount
                            Dim cw As String = cpara.getCharacters(cc, os)
                            Dim us As New Size(ssize.Width, rheight)
                            If us.Height < 1 Then Throw New NonFittingTextException("Text does not fit.")
                            Dim fs As FontSize = getFontSizeToUse(cw, fnt, us, efsize, itr)
                            If fs.remainingIterations > 0 Then Throw New NonFittingException("Text does not fit, Iterations:" & fs.remainingIterations)
                            If fs.areasizes.Length > 0 And fs.fontsizes.Length > 0 Then
                                Dim ls As Integer = fs.fontsizes(fs.fontsizes.Length - 1)
                                Dim la As Size = fs.areasizes(fs.areasizes.Length - 1)
                                aheight += la.Height
                                rheight -= la.Height
                                lines.Add(New Pair(Of String, Integer)(cw, ls))
                                os += cc
                                cc = cpara.wordCount - os '-1
                            Else
                                cc -= 1
                            End If
                        End While
                    Else
                        Dim wc As Integer = cpara.wordCount
                        Dim os As Integer = 0
                        While os < cpara.wordCount
                            Dim cw As String = cpara.getWords(wc, os)
                            Dim us As New Size(ssize.Width, rheight)
                            If us.Height < 1 Then Throw New NonFittingTextException("Text does not fit.")
                            Dim fs As FontSize = getFontSizeToUse(cw, fnt, us, efsize, itr)
                            If fs.remainingIterations > 0 Then Throw New NonFittingException("Text does not fit, Iterations:" & fs.remainingIterations)
                            If fs.areasizes.Length > 0 And fs.fontsizes.Length > 0 Then
                                Dim ls As Integer = fs.fontsizes(fs.fontsizes.Length - 1)
                                Dim la As Size = fs.areasizes(fs.areasizes.Length - 1)
                                aheight += la.Height
                                rheight -= la.Height
                                lines.Add(New Pair(Of String, Integer)(cw, ls))
                                os += wc
                                wc = cpara.wordCount - os '-1
                            Else
                                wc -= 1
                            End If
                        End While
                    End If
                Next
            Catch ex As NonFittingTextException
                If ex.Message.Contains("Text does not fit.") Then
                    Throw ex
                Else
                    itr += 1
                End If
            End Try
        End While
        If itr > (fnt.Size - efsize) Then Throw New NonFittingException("Text does not fit, Iterations:" & itr)
        Dim img As Image = Nothing
        If usecalculatedheight Then img = New Bitmap(ssize.Width, aheight) Else img = New Bitmap(ssize.Width, ssize.Height)
        Dim pos As Integer = 0
        Using g As Graphics = Graphics.FromImage(img), b As New SolidBrush(col)
            For Each ln As Pair(Of String, Integer) In lines
                Using f As New Font(fnt.Name, ln.Item2, fnt.Style, fnt.Unit)
                    g.DrawString(ln.Item1, f, b, 0, pos)
                    pos += f.Height
                End Using
            Next
        End Using
        Return img
    End Function

    Public Shared Function getParagraphs(txt As String) As List(Of TextParagraph)
        Dim lst As New List(Of TextParagraph)
        If txt.Contains(ControlChars.Cr) And txt.Contains(ControlChars.Lf) Then
            txt = txt.Replace(ControlChars.Cr, "")
        End If
        If txt.Contains(ControlChars.Cr) Then
            Dim sets As String() = txt.Split(ControlChars.Cr)
            For Each s As String In sets
                lst.Add(New TextParagraph(s))
            Next
        ElseIf txt.Contains(ControlChars.Lf) Then
            Dim sets As String() = txt.Split(ControlChars.Lf)
            For Each s As String In sets
                lst.Add(New TextParagraph(s))
            Next
        End If
        Return lst
    End Function

    Public Shared Function getFontSizeToUse(txt As String, fntbase As Font, tsize As Size, Optional minfsize As Integer = 1, Optional sitercnt As Integer = 1) As FontSize
        If sitercnt < 1 Then sitercnt = 1
        Dim cnt As Integer = sitercnt
        Dim cs As Integer = fntbase.Size
        Dim fsizes As New List(Of Integer)
        Dim asizes As New List(Of Size)
        While cnt > 0 And cs > minfsize - 1
            Using fnt As New Font(fntbase.Name, cs, fntbase.Style, fntbase.Unit)
                Dim s As Size = TextRenderer.MeasureText(txt, fnt)
                If s.Width <= tsize.Width And s.Height <= tsize.Height Then
                    fsizes.Add(cs)
                    asizes.Add(s)
                    cnt -= 1
                End If
            End Using
            cs -= 1
            If cnt < 1 Or cs < minfsize Then
                Exit While
            End If
        End While
        Dim fs As New FontSize(fntbase, fsizes.ToArray, asizes.ToArray, cnt)
        Return fs
    End Function

    Public Structure FontSize
        Public basefont As Font
        Public fontsizes As Integer()
        Public areasizes As Size()
        Public remainingIterations As Integer
        Public Sub New(bf As Font, fs As Integer(), ars As Size(), ri As Integer)
            basefont = bf
            fontsizes = fs
            areasizes = ars
            remainingIterations = ri
        End Sub
    End Structure

    Public Class TextParagraph
        Public words As String()
        Public Sub New(para As String)
            words = para.Split(" ".ToCharArray)
        End Sub
        Public ReadOnly Property wordCount As Integer
            Get
                Return words.Length
            End Get
        End Property
        Public ReadOnly Property characterCount As Integer
            Get
                Return getParagraph().Length
            End Get
        End Property
        Public Function getParagraph() As String
            Dim toret As String = ""
            For i As Integer = 0 To words.Length - 1 Step 1
                toret &= words(i) & " "
            Next
            Return toret.Substring(0, toret.Length - 1)
        End Function
        Public Function getWords(count As Integer, Optional offset As Integer = 0) As String
            If count < 1 Then count = 1
            If offset < 0 Then offset = 0
            If offset > words.Length - 1 Then offset = words.Length - 1
            If offset + count > words.Length Then count = words.Length - offset
            Dim toret As String = ""
            For i As Integer = offset To count Step 1
                toret &= words(i) & " "
            Next
            Return toret.Substring(0, toret.Length - 1)
        End Function
        Public Function getCharacters(count As Integer, Optional offset As Integer = 0) As String
            Dim para As String = getParagraph()
            If count < 1 Then count = 1
            If offset < 0 Then offset = 0
            If offset > para.Length - 2 Then offset = para.Length - 2
            If offset + count > para.Length Then count = para.Length - offset
            Dim toret As String = para.Substring(offset, count)
            Dim echar As String = toret.Substring(toret.Length - 1, 1)
            Dim achar As String = ""
            If offset + count < para.Length Then achar = toret.Substring(offset + count, 1)
            If echar = " " Then
                Return toret.Substring(0, toret.Length - 1)
            Else
                If achar = "" Or achar = " " Then
                    Return toret
                Else
                    Return toret & "-"
                End If
            End If
        End Function
    End Class
End Class

<Serializable>
Public Class TermSet(Of a As TermSource, b As TermSource)
    Private vara As a
    Private varb As b

    Public Sub New(t1 As a, t2 As b)
        vara = t1
        varb = t2
    End Sub

    Public Property Term1() As a
        Get
            Return vara
        End Get
        Set(value As a)
            vara = value
        End Set
    End Property

    Public Property Term2() As b
        Get
            Return varb
        End Get
        Set(value As b)
            varb = value
        End Set
    End Property

    Public ReadOnly Property Term1Image(w As Integer, h As Integer, Optional ugs As Boolean = True) As Image
        Get
            Return vara.getImage(w, h, ugs)
        End Get
    End Property

    Public ReadOnly Property Term2Image(w As Integer, h As Integer, Optional ugs As Boolean = True) As Image
        Get
            Return varb.getImage(w, h, ugs)
        End Get
    End Property
End Class

<Serializable>
Public Class Pair(Of a, b)
    Private vara As a
    Private varb As b

    Public Sub New(t1 As a, t2 As b)
        vara = t1
        varb = t2
    End Sub

    Public Property Item1() As a
        Get
            Return vara
        End Get
        Set(value As a)
            vara = value
        End Set
    End Property

    Public Property Item2() As b
        Get
            Return varb
        End Get
        Set(value As b)
            varb = value
        End Set
    End Property
End Class

<Serializable>
Public Class NonFittingException
    Inherits Exception
    Public Sub New()
    End Sub
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class
<Serializable>
Public Class NonFittingTextException
    Inherits NonFittingException
    Public Sub New()
    End Sub
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class
<Serializable>
Public Class NonFittingImageException
    Inherits NonFittingException
    Public Sub New()
    End Sub
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class

Public Interface IReferencedData
    Property dataPath As String
    Sub loadData()
    Sub saveData()
End Interface

Public Interface IStoredData
    Inherits IReferencedData
    Property storedData As Byte()
End Interface