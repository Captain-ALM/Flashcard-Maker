﻿Imports captainalm.workerpumper
Imports captainalm.util.preference
Imports System.Drawing.Printing
Imports captainalm.FlashCardMaker.FileAssociations
Imports System.IO

Public Class PGlobalOptions
    Implements IEventParser

    Public Function Parse(ev As WorkerEvent) As Boolean Implements IEventParser.Parse
        Dim toret As Boolean = False
        Dim gp As GlobalPreferences = globalops.getPreference(Of GlobalPreferences)("GlobalPreferences")
        Dim gpp As ProjectPreferences = globalops.getPreference(Of ProjectPreferences)("GlobalProjectPreferences")
        Dim fap As FileAssociations = globalops.getPreference(Of FileAssociations)("FileAssociations")
        If canCastObject(Of GlobalOptions)(ev.EventSource.sourceObj) Then
            Dim frm As GlobalOptions = castObject(Of GlobalOptions)(ev.EventSource.sourceObj)
            If ev.EventType = EventTypes.Shown Then
                Dim s As Boolean = fap.getPreferencesFromRegistry()
                If Not s Then
                    Try
                        Throw New Exception("File Associations Could Not Be Loaded")
                    Catch ex As Exception
                        Dim frm2 As New UnhandledExceptionViewer(True, False, True, ex)
                        Dim r As System.Windows.Forms.DialogResult = frm2.ShowDialog()
                        If Not frm2.Disposing And Not frm2.IsDisposed Then
                            frm2.Dispose()
                        End If
                        frm2 = Nothing
                        If r = DialogResult.Abort Then
                            Environment.Exit(1)
                        End If
                    End Try
                End If
                frm.Invoke(Sub()
                               frm.chkbxesl.Checked = gp.getPreference(Of IPreference(Of Boolean))("EnableFontSizeLimit").getPreference()
                               frm.nudmin.Value = gp.getPreference(Of IPreference(Of Integer))("MinumumFontSize").getPreference()
                               frm.nudmax.Value = gp.getPreference(Of IPreference(Of Integer))("MaximumFontSize").getPreference()
                               frm.chkbxetem.Checked = gp.getPreference(Of IPreference(Of Boolean))("EnableThreadErrorMessages").getPreference()
                               Dim indx As Integer = 0
                               For Each v As String In frm.cbxps.Items
                                   If v.Contains(gpp.getPreference(Of IPreference(Of PaperKind))("PageSize").getPreference().ToString()) Then
                                       frm.cbxps.SelectedIndex = indx
                                   End If
                                   indx += 1
                               Next
                               frm.nudcw.Value = gpp.getPreference(Of IPreference(Of Integer))("CardWidth").getPreference()
                               frm.nudch.Value = gpp.getPreference(Of IPreference(Of Integer))("CardHeight").getPreference()
                               frm.FontDialogNoSize.FontValue = gpp.getPreference(Of IPreference(Of Font))("Font").getPreference()
                               frm.txtbxfn.Font = New Font(frm.FontDialogNoSize.FontValue.Name, 8.25)
                               frm.txtbxfn.Text = frm.FontDialogNoSize.FontValue.Name
                               frm.txtbxfe.Text = ""
                               If frm.FontDialogNoSize.FontValue.Bold Then
                                   frm.txtbxfe.Text &= "Bold "
                               End If
                               If frm.FontDialogNoSize.FontValue.Italic Then
                                   frm.txtbxfe.Text &= "Italic "
                               End If
                               If frm.FontDialogNoSize.FontValue.Strikeout Then
                                   frm.txtbxfe.Text &= "Strikeout "
                               End If
                               If frm.FontDialogNoSize.FontValue.Underline Then
                                   frm.txtbxfe.Text &= "Underline "
                               End If
                               If frm.txtbxfe.Text.EndsWith(" ") Then
                                   frm.txtbxfe.Text = frm.txtbxfe.Text.TrimEnd(" ")
                               End If
                               frm.ColorDialog1.Color = gpp.getPreference(Of IPreference(Of Color))("Color").getPreference()
                               frm.txtbxfc.Text = frm.ColorDialog1.Color.ToString
                               frm.txtbxfc.BackColor = frm.ColorDialog1.Color
                               frm.rbutcpc.Checked = gpp.getPreference(Of IPreference(Of Boolean))("SetTermCountPerCard").getPreference()
                               frm.nudtc.Value = gpp.getPreference(Of IPreference(Of Integer))("TermCount").getPreference()
                               frm.rbututcprfs.Checked = gpp.getPreference(Of IPreference(Of Boolean))("SetTermCountPerRecommenedFontSize").getPreference()
                               frm.nudrfs.Value = gpp.getPreference(Of IPreference(Of Integer))("RecommenedFontSize").getPreference()
                               If frm.rbututcprfs.Checked Then
                                   frm.Label9.Enabled = False
                                   frm.nudtc.Enabled = False
                                   frm.Label10.Enabled = True
                                   frm.nudrfs.Enabled = True
                               ElseIf frm.rbutcpc.Checked Then
                                   frm.Label9.Enabled = True
                                   frm.nudtc.Enabled = True
                                   frm.Label10.Enabled = False
                                   frm.nudrfs.Enabled = False
                               End If
                               frm.chkbxasw.Checked = gpp.getPreference(Of IPreference(Of Boolean))("CanSplitWords").getPreference()
                               If fap.getPreference(Of IPreference(Of Boolean))("ApplicationRegistered").getPreference() Then
                                   frm.rbutcalmfcmppnr.Enabled = True
                                   frm.rbutcalmfcmpradp.Enabled = True
                                   frm.rbutcalmfcmpraop.Enabled = True
                                   frm.rbutfcppnr.Enabled = True
                                   frm.rbutfcpradp.Enabled = True
                                   frm.rbutfcpraop.Enabled = True
                                   Dim fcprm As RegisterMode = fap.getPreference(Of IPreference(Of RegisterMode))(".fcp").getPreference()
                                   Dim calmfcmprm As RegisterMode = fap.getPreference(Of IPreference(Of RegisterMode))(".calmfcmp").getPreference()
                                   If fcprm = RegisterMode.RegisteredAndDefault Then
                                       frm.rbutfcpradp.Checked = True
                                       frm.rbutfcpraop.Checked = False
                                       frm.rbutfcppnr.Checked = False
                                   ElseIf fcprm = RegisterMode.Registered Then
                                       frm.rbutfcpradp.Checked = False
                                       frm.rbutfcpraop.Checked = True
                                       frm.rbutfcppnr.Checked = False
                                   Else
                                       frm.rbutfcpradp.Checked = False
                                       frm.rbutfcpraop.Checked = False
                                       frm.rbutfcppnr.Checked = True
                                   End If
                                   If calmfcmprm = RegisterMode.RegisteredAndDefault Then
                                       frm.rbutcalmfcmpradp.Checked = True
                                       frm.rbutcalmfcmpraop.Checked = False
                                       frm.rbutcalmfcmppnr.Checked = False
                                   ElseIf fcprm = RegisterMode.Registered Then
                                       frm.rbutcalmfcmpradp.Checked = False
                                       frm.rbutcalmfcmpraop.Checked = True
                                       frm.rbutcalmfcmppnr.Checked = False
                                   Else
                                       frm.rbutcalmfcmpradp.Checked = False
                                       frm.rbutcalmfcmpraop.Checked = False
                                       frm.rbutcalmfcmppnr.Checked = True
                                   End If
                               Else
                                   frm.rbutcalmfcmppnr.Checked = False
                                   frm.rbutcalmfcmpradp.Checked = False
                                   frm.rbutcalmfcmpraop.Checked = False
                                   frm.rbutfcppnr.Checked = False
                                   frm.rbutfcpradp.Checked = False
                                   frm.rbutfcpraop.Checked = False
                                   frm.rbutcalmfcmppnr.Enabled = False
                                   frm.rbutcalmfcmpradp.Enabled = False
                                   frm.rbutcalmfcmpraop.Enabled = False
                                   frm.rbutfcppnr.Enabled = False
                                   frm.rbutfcpradp.Enabled = False
                                   frm.rbutfcpraop.Enabled = False
                               End If
                               frm.Enabled = True
                           End Sub)
            ElseIf ev.EventType = EventTypes.Closing Then
                frm.Invoke(Sub() frm.Enabled = False)
                Dim s As Boolean = fap.setPreferencesToRegistry()
                If Not s Then
                    Try
                        Throw New Exception("File Associations Could Not Be Saved")
                    Catch ex As Exception
                        Dim frm2 As New UnhandledExceptionViewer(True, False, True, ex)
                        Dim r As System.Windows.Forms.DialogResult = frm2.ShowDialog()
                        If Not frm2.Disposing And Not frm2.IsDisposed Then
                            frm2.Dispose()
                        End If
                        frm2 = Nothing
                        If r = DialogResult.Abort Then
                            Environment.Exit(1)
                        End If
                    End Try
                End If
                Try
                    File.WriteAllText(execdir & "\settings.ser", globalops.savePreference())
                Catch ex As IOException
                    Dim frm2 As New UnhandledExceptionViewer(True, False, True, ex)
                    Dim r As System.Windows.Forms.DialogResult = frm2.ShowDialog()
                    If Not frm2.Disposing And Not frm2.IsDisposed Then
                        frm2.Dispose()
                    End If
                    frm2 = Nothing
                    If r = DialogResult.Abort Then
                        Environment.Exit(1)
                    End If
                End Try
            End If
        ElseIf ev.EventSource.parentObjs IsNot Nothing Then
            If ev.EventSource.parentObjs.Count = 1 Then
                If canCastObject(Of GlobalOptions)(ev.EventSource.parentObjs(0)) Then
                    Dim frm As GlobalOptions = castObject(Of GlobalOptions)(ev.EventSource.parentObjs(0))
                    If canCastObject(Of Control)(ev.EventSource.sourceObj) Then
                        Dim ctrl As Control = castObject(Of Control)(ev.EventSource.sourceObj)
                        'If ctrl Is frm.Then Then

                        'End If
                        frm.Invoke(Sub() ctrl.Enabled = True)
                        toret = True
                    End If
                End If
            ElseIf ev.EventSource.parentObjs.Count = 2 Then
                If canCastObject(Of GlobalOptions)(ev.EventSource.parentObjs(1)) And canCastObject(Of Control)(ev.EventSource.parentObjs(0)) Then
                    Dim frm As GlobalOptions = castObject(Of GlobalOptions)(ev.EventSource.parentObjs(1))
                    Dim ctrl As Control = castObject(Of Control)(ev.EventSource.parentObjs(0))
                    If ctrl Is frm.butcfs And canCastObject(Of FontDialogSuccessEventArgs)(ev.EventData) Then
                        Dim f As FontDialogSuccessEventArgs = castObject(Of FontDialogSuccessEventArgs)(ev.EventData)
                        frm.Invoke(Sub() frm.txtbxfn.Text = f.Font.ToString)
                        frm.Invoke(Sub()
                                       frm.txtbxfe.Text = ""
                                       If frm.FontDialogNoSize.FontValue.Bold Then
                                           frm.txtbxfe.Text &= "Bold "
                                       End If
                                       If frm.FontDialogNoSize.FontValue.Italic Then
                                           frm.txtbxfe.Text &= "Italic "
                                       End If
                                       If frm.FontDialogNoSize.FontValue.Strikeout Then
                                           frm.txtbxfe.Text &= "Strikeout "
                                       End If
                                       If frm.FontDialogNoSize.FontValue.Underline Then
                                           frm.txtbxfe.Text &= "Underline "
                                       End If
                                       If frm.txtbxfe.Text.EndsWith(" ") Then
                                           frm.txtbxfe.Text = frm.txtbxfe.Text.TrimEnd(" ")
                                       End If
                                   End Sub)
                        gpp.getPreference(Of IPreference(Of Font))("Font").setPreference(f.Font)
                    ElseIf ctrl Is frm.butcfc And canCastObject(Of ColorDialogSuccessEventArgs)(ev.EventData) Then
                        Dim c As ColorDialogSuccessEventArgs = castObject(Of ColorDialogSuccessEventArgs)(ev.EventData)
                        frm.Invoke(Sub() frm.txtbxfc.Text = c.Color.ToString)
                        frm.Invoke(Sub() frm.txtbxfc.BackColor = c.Color)
                        gpp.getPreference(Of IPreference(Of Color))("Color").setPreference(c.Color)
                    End If
                End If
            End If
        End If
        Return toret
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
End Class