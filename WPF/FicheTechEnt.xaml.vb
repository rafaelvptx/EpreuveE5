Public Class FicheTechEnt

    Private o_SI As SI

    Private Sub BT_Modifier_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BT_Modifier.Click

        TxtBActivite.IsEnabled = True
        TxtBContact.IsEnabled = True
        TxtBCP.IsEnabled = True
        TxtBNaf.IsEnabled = True
        TxtBQuotaA.IsEnabled = True
        TxtBQuotaB.IsEnabled = True
        TxtBQuotaC.IsEnabled = True
        TxtBQuotaL.IsEnabled = True
        TxtBQuotaO.IsEnabled = True
        TxtBQuotaTotal.IsEnabled = True
        TxtBRaisonSocial.IsEnabled = True
        TxtBRue.IsEnabled = True
        TxtBSiret.IsEnabled = True
        TxtBVille.IsEnabled = True

        BT_Modifier.Visibility = Windows.Visibility.Collapsed
        BT_Annuler.Visibility = Windows.Visibility.Visible
        BT_Enregistrer.Visibility = Windows.Visibility.Visible
        BT_Supprimer.Visibility = Windows.Visibility.Collapsed

    End Sub

    Private Sub BT_Annuler_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BT_Annuler.Click

        TxtBActivite.IsEnabled = False
        TxtBContact.IsEnabled = False
        TxtBCP.IsEnabled = False
        TxtBNaf.IsEnabled = False
        TxtBQuotaA.IsEnabled = False
        TxtBQuotaB.IsEnabled = False
        TxtBQuotaC.IsEnabled = False
        TxtBQuotaL.IsEnabled = False
        TxtBQuotaO.IsEnabled = False
        TxtBQuotaTotal.IsEnabled = False
        TxtBRaisonSocial.IsEnabled = False
        TxtBRue.IsEnabled = False
        TxtBSiret.IsEnabled = False
        TxtBVille.IsEnabled = False

        BT_Modifier.Visibility = Windows.Visibility.Visible
        BT_Annuler.Visibility = Windows.Visibility.Collapsed
        BT_Enregistrer.Visibility = Windows.Visibility.Collapsed
        BT_Supprimer.Visibility = Windows.Visibility.Visible

    End Sub

    Private Sub BT_Supprimer_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BT_Supprimer.Click


        Dim o_Entreprise As Entreprise
        Dim o_Quota As Quota

        MsgBox("Etes vous sûr?", MsgBoxStyle.YesNo, "Suppression")

        o_Quota = New Quota(TxtBQuotaO.Text, TxtBQuotaL.Text, TxtBQuotaA.Text, TxtBQuotaB.Text, TxtBQuotaC.Text)

        If MsgBoxResult.Yes Then
            With o_SI.CtrlSaisieEnt
                o_Entreprise = New Entreprise(.SIRET, .RaisonSocial, .Rue, .CP, .Ville, .NAF, .Contact, .Activite, .Quota)
            End With
            o_SI.SupprimerUneEntreprise(o_Entreprise)
            Close()
        End If
    End Sub

    Private Sub BT_Afficher_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            Dim BTN As Button = sender
            Dim ETU As Etudiant = BTN.DataContext
            Dim WinFTE As New FichTechETU
            WinFTE.GRDGlob.DataContext = ETU
            WinFTE.Title = ETU.Nom
            WinFTE.GRDInfos.DataContext = ETU
            WinFTE.GRDInfosEnt.DataContext = ETU.Entreprise

            WinFTE.Show()

        Catch ex As Exception
            Dim toto As Integer = 5
        End Try
    End Sub

    Private Sub BT_Enregistrer_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BT_Enregistrer.Click
        Dim Button As Button = sender
        Dim ENT As Entreprise = Button.DataContext
        Dim CoDeQuota As String = ENT.Quota.Code
        Dim Quota As Quota

        Quota = New Quota(CoDeQuota, TxtBQuotaO.Text, TxtBQuotaL.Text, TxtBQuotaA.Text, TxtBQuotaB.Text, TxtBQuotaC.Text)
        Dim NewENT As New Entreprise(TxtBSiret.Text, TxtBRaisonSocial.Text, TxtBRue.Text, TxtBCP.Text, TxtBVille.Text, TxtBNaf.Text, TxtBContact.Text, TxtBActivite.Text, Quota)
        o_SI.ModifierUneEntreprise(NewENT, ENT)
        Close()
    End Sub

    Private Sub TxtBQuotaA_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TxtBQuotaA.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtBQuotaB_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TxtBQuotaB.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtBQuotaC_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TxtBQuotaC.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtBQuotaL_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TxtBQuotaL.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtBQuotaO_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TxtBQuotaO.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtBCP_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TxtBCP.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsNumber(c) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub


End Class
