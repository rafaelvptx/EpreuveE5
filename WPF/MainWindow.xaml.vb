
Class MainWindow

    Private o_Si As SI



    Private Sub ficheTech_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Dim IMG As Image = sender
            Dim ENT As Entreprise = IMG.DataContext
            Dim WinFTE As New FicheTechEnt
            WinFTE.GRDGlob.DataContext = ENT
            WinFTE.Title = ENT.RaisonSocial
            WinFTE.GRDInfos.DataContext = ENT
            WinFTE.listBoxETU.ItemsSource = ENT.Etudiants

            WinFTE.Activate()
            WinFTE.Show()




        Catch ex As Exception
            Dim toto As Integer = 5
        End Try
    End Sub

    Private Sub ficheETU_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.MouseButtonEventArgs)
        Try
            Dim IMG As Image = sender
            Dim ETU As SaisieEtudiant = IMG.DataContext
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

    Private Sub BT_AjouterEnt_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BT_AjouterEnt.Click
        Dim LaWin As New WinAjoutEntr
        LaWin.Show()
    End Sub

    Private Sub BT_AjouterEtu_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles BT_AjouterEtu.Click
        Dim LaWin As New WinAjoutEtu
        LaWin.Show()
    End Sub
End Class
