Public Class WinAjoutEntr

    Private o_SI As SI

    Private Sub BT_AjouterEnt_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs)

        Dim o_Quota As Quota
        Dim o_Entreprise As Entreprise

        If (TxtSIRET.Text <> "" And o_SI.Entreprises.ChercherUnSiretBoo(TxtRaisonSo.Text) = False) Then

            o_SI.OuvrirConnexion()

            o_Quota = New Quota(TB_QuotaO.Text, TB_QuotaL.Text, TB_QuotaA.Text, TB_QuotaB.Text, TB_QuotaC.Text)
            o_SI.EnregisterQuota(o_Quota)

            With o_SI.CtrlSaisieEnt
                o_Entreprise = New Entreprise(.SIRET, .RaisonSocial, .Rue, .CP, .Ville, .NAF, .Contact, .Activite, o_Quota)
            End With
            o_SI.EnregisterEntreprise(o_Entreprise)

            o_SI.FermerConnexion()

            Close()
        Else
            MsgBox("Vous devez remplir le numéro SIRET")
        End If
    End Sub

    Private Sub TxtCP_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TxtCP.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsNumber(c) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtQuotaA_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TB_QuotaA.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtQuotaB_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TB_QuotaB.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtQuotaC_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TB_QuotaC.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtQuotaL_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TB_QuotaL.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub TxtQuotaO_PreviewTextInput(ByVal sender As Object, ByVal e As System.Windows.Input.TextCompositionEventArgs) Handles TB_QuotaO.PreviewTextInput
        Dim c As Char = Convert.ToChar(e.Text)
        If Char.IsDigit(c) Then
            e.Handled = False
        ElseIf e.Text = "," Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub BT_AnnulerEnt_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Close()
    End Sub

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        o_SI = Me.FindResource("OSI")
    End Sub

End Class
