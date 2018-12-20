Imports captainalm.util.preference
Imports System.Drawing.Printing

<Serializable>
Public NotInheritable Class Project
    Private pset As ProjectPreferences = Nothing
    Private pimg As New List(Of Pair(Of Image, Image))
    Private pterm As New List(Of TermSet(Of TermSource, TermSource))
    Private pcard As New List(Of Card)
    Private pnom As String = ""
    Private Const pver As Integer = 1

    Public Sub New(name As String)
        pnom = name
        pset = New ProjectPreferences(name)
    End Sub

    Public ReadOnly Property projectVersion As Integer
        Get
            Return pver
        End Get
    End Property

    'TODO: FILL IN PROJECT CODE.
    Public Function render() As Boolean
        If generateCards() Then Return generateImages() Else Return False
    End Function

    Public Function generateCards() As Boolean
        pcard.Clear()
        clearImages()
        Try
            If pterm.Count < 1 Then Return False
            Dim cw As Integer = pset.getPreference(Of IPreference(Of Integer))("CardWidth").getPreference()
            Dim ch As Integer = pset.getPreference(Of IPreference(Of Integer))("CardHeight").getPreference()
            Dim tw As Integer = cw
            Dim th As Integer = ch
            Dim css As New CardSizeSetup(CardSizeSetting.none, 0)
            If pset.getPreference(Of IPreference(Of Boolean))("SetTermCountPerCard").getPreference() Then
                css.setting = CardSizeSetting.settermcountpercard
                css.termcount = pset.getPreference(Of IPreference(Of Integer))("TermCount").getPreference()
                th = ch \ css.termcount
            ElseIf pset.getPreference(Of IPreference(Of Boolean))("SetTermCountPerRecommendedFontSize").getPreference() Then
                css.setting = CardSizeSetting.settermcountperrecommendedfont
                css.recommendedfontsize = pset.getPreference(Of IPreference(Of Integer))("RecommendedFontSize").getPreference()
                th = ch
            End If
            Dim pos As Integer = 0
            Dim terms As New List(Of TermSet(Of TermSource, TermSource))
            Dim addcard As Boolean = False
            For Each ts As TermSet(Of TermSource, TermSource) In pterm
                Dim hta As Integer = 0
                While True
                    If addcard Then
                        addcard = False
                        pos = 0
                        th = ch
                        pcard.Add(New Card(terms.ToArray))
                        terms.Clear()
                    End If
                    Dim imgs As Image() = New Image() {Nothing, Nothing}
                    Try
                        If css.setting = CardSizeSetting.settermcountpercard Then
                            imgs = New Image() {ts.Term1Image(tw, th, True), ts.Term2Image(tw, th, True)}
                            hta = th
                        ElseIf css.setting = CardSizeSetting.settermcountperrecommendedfont Then
                            imgs = New Image() {ts.Term1Image(tw, th, False), ts.Term2Image(tw, th, False)}
                            If hta < imgs(0).Height Then hta = imgs(0).Height
                            If hta < imgs(1).Height Then hta = imgs(1).Height
                            th -= hta
                        End If
                        If pos + hta > ch - pos Then
                            addcard = True
                            Continue While
                        Else
                            terms.Add(ts)
                        End If
                        Exit While
                    Catch ex As NonFittingException
                        If pos <> 0 Then addcard = True Else Throw New NonFittingException("Term Does Not Fit On Card.")
                        Continue While
                    End Try
                End While
                pos += hta
            Next
            If terms.Count > 0 Then pcard.Add(New Card(terms.ToArray))
            terms.Clear()
            Return True
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
        End Try
        Return False
    End Function

    Public Function generateImages() As Boolean
        pimg.Clear()
        Try
            If pcard.Count < 1 Then Return False
            For Each c As Card In pcard
                Dim css As New CardSizeSetup(CardSizeSetting.none, 0)
                If pset.getPreference(Of IPreference(Of Boolean))("SetTermCountPerCard").getPreference() Then
                    css.setting = CardSizeSetting.settermcountpercard
                    css.termcount = pset.getPreference(Of IPreference(Of Integer))("TermCount").getPreference()
                ElseIf pset.getPreference(Of IPreference(Of Boolean))("SetTermCountPerRecommendedFontSize").getPreference() Then
                    css.setting = CardSizeSetting.settermcountperrecommendedfont
                    css.recommendedfontsize = pset.getPreference(Of IPreference(Of Integer))("RecommendedFontSize").getPreference()
                End If
                pimg.Add(c.getCardImages(pset.getPreference(Of IPreference(Of Integer))("CardWidth").getPreference(), pset.getPreference(Of IPreference(Of Integer))("CardHeight").getPreference(), css))
            Next
            Return True
        Catch ex As Threading.ThreadAbortException
            Throw ex
        Catch ex As Exception
        End Try
        Return False
    End Function

    Public Function addData(dat As TermSet(Of TermSource, TermSource)) As Integer
        clearCards()
        clearImages()
        pterm.Add(dat)
        Return pterm.Count - 1
    End Function

    Public Sub removeData(index As Integer)
        clearCards()
        clearImages()
        pterm.RemoveAt(index)
    End Sub

    Public Sub clearData()
        clearCards()
        clearImages()
        pterm.Clear()
    End Sub

    Public ReadOnly Property dataCount As Integer
        Get
            Return pterm.Count
        End Get
    End Property

    Public Property data(index As Integer) As TermSet(Of TermSource, TermSource)
        Get
            If index < pterm.Count And index > -1 Then Return pterm(index)
            Return New TermSet(Of TermSource, TermSource)(Nothing, Nothing)
        End Get
        Set(value As TermSet(Of TermSource, TermSource))
            clearCards()
            clearImages()
            If index < pterm.Count And index > -1 Then pterm(index) = value
        End Set
    End Property

    Public Sub clearCards()
        pcard.Clear()
    End Sub

    Public ReadOnly Property cardCount As Integer
        Get
            Return pcard.Count
        End Get
    End Property

    Public Sub clearImages()
        pimg.Clear()
    End Sub

    Public ReadOnly Property imageCount As Integer
        Get
            Return pimg.Count
        End Get
    End Property

    Default Public ReadOnly Property image(index As Integer) As Pair(Of Image, Image)
        Get
            If index < pimg.Count And index > -1 Then Return pimg(index)
            Return New Pair(Of Image, Image)(Nothing, Nothing)
        End Get
    End Property

    Public Function getImages() As Pair(Of Image, Image)()
        Return pimg.ToArray
    End Function
End Class

<Serializable>
Public NotInheritable Class ProjectPreferences
    Inherits Preferences

    Public Sub New()
        Me.New("Global")
    End Sub

    Public Sub New(name As String)
        MyBase.New(name & "ProjectPreferences")
        MyBase.addPreference(Of IPreference(Of PaperKind))(New Preference(Of PaperKind)("PageSize"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("CardWidth"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("CardHeight"))
        MyBase.addPreference(Of IPreference(Of Font))(New Preference(Of Font)("Font"))
        MyBase.addPreference(Of IPreference(Of Color))(New Preference(Of Color)("Color"))
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("SetTermCountPerCard"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("TermCount"))
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("SetTermCountPerRecommendedFontSize"))
        MyBase.addPreference(Of IPreference(Of Integer))(New Preference(Of Integer)("RecommendedFontSize"))
        MyBase.addPreference(Of IPreference(Of Boolean))(New Preference(Of Boolean)("CanSplitWords"))
    End Sub

    Public Overrides Sub addPreference(Of t As IPreference)(pref As t)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub removePreference(name As String, Optional index As Integer = 0)
        Throw New InvalidOperationException()
    End Sub

    Public Overrides Sub setPreference(pref As Object)
        Throw New InvalidOperationException()
    End Sub
End Class

<Serializable>
Public NotInheritable Class Card
    Private terms As List(Of TermSet(Of TermSource, TermSource)) = Nothing
    Public Sub New(ts As TermSet(Of TermSource, TermSource)())
        terms = New List(Of TermSet(Of TermSource, TermSource))(ts)
    End Sub
    Public Property heldTerms As TermSet(Of TermSource, TermSource)()
        Get
            Return terms.ToArray
        End Get
        Set(value As TermSet(Of TermSource, TermSource)())
            terms = New List(Of TermSet(Of TermSource, TermSource))(value)
        End Set
    End Property
    Public Sub clearTerms()
        terms.Clear()
    End Sub
    Public Function getCardImages(pagewidth As Integer, pageheight As Integer, css As CardSizeSetup) As Pair(Of Image, Image)
        If css.setting = CardSizeSetting.settermcountpercard Then
            Dim th As Integer = pageheight \ css.termcount
            Dim imgs As Bitmap() = New Bitmap() {New Bitmap(pagewidth, pageheight), New Bitmap(pagewidth, pageheight)}
            Dim pos As Integer = 0
            Using g1 As Graphics = Graphics.FromImage(imgs(0)), g2 As Graphics = Graphics.FromImage(imgs(1))
                For Each ts As TermSet(Of TermSource, TermSource) In terms
                    g1.DrawImage(ts.Term1Image(pagewidth, th, True), 0, pos, pagewidth, th)
                    g2.DrawImage(ts.Term2Image(pagewidth, th, True), 0, pos, pagewidth, th)
                    pos += th
                Next
            End Using
            Return New Pair(Of Image, Image)(imgs(0), imgs(1))
        ElseIf css.setting = CardSizeSetting.settermcountperrecommendedfont Then
            Dim imgs As Bitmap() = New Bitmap() {New Bitmap(pagewidth, pageheight), New Bitmap(pagewidth, pageheight)}
            Dim pos As Integer = 0
            Using g1 As Graphics = Graphics.FromImage(imgs(0)), g2 As Graphics = Graphics.FromImage(imgs(1))
                For Each ts As TermSet(Of TermSource, TermSource) In terms
                    Dim th As Integer = 0
                    Dim timgs As Image() = New Image() {ts.Term1Image(pagewidth, pageheight, False), ts.Term2Image(pagewidth, pageheight, False)}
                    If timgs(0).Height > th Then th = timgs(0).Height
                    If timgs(1).Height > th Then th = timgs(1).Height
                    g1.DrawImage(timgs(0), 0, pos, pagewidth, th)
                    g2.DrawImage(timgs(1), 0, pos, pagewidth, th)
                    pos += th
                Next
            End Using
            Return New Pair(Of Image, Image)(imgs(0), imgs(1))
        End If
        Return New Pair(Of Image, Image)(New Bitmap(pagewidth, pageheight), New Bitmap(pagewidth, pageheight))
    End Function
End Class

<Serializable>
Public Structure CardSizeSetup
    Public setting As CardSizeSetting
    Public recommendedfontsize As Integer
    Public termcount As Integer
    Public Sub New(s As CardSizeSetting, p As Integer)
        setting = s
        If s = CardSizeSetting.settermcountpercard Then
            termcount = p
        ElseIf s = CardSizeSetting.settermcountperrecommendedfont Then
            recommendedfontsize = p
        End If
    End Sub
End Structure

<Serializable>
Public Enum CardSizeSetting As Integer
    none = 0
    settermcountpercard = 1
    settermcountperrecommendedfont = 2
End Enum
