Public Class Entreprises
    Inherits GiorgiList(Of Entreprise)

    Public Function Chercher(ByVal Entreprise As Entreprise) As Entreprise
        For Each En As Entreprise In Me
            If En.SIRET = Entreprise.SIRET Then Return En
        Next
        Return Nothing
    End Function

    Public Function ChercherUneEntreprise(ByVal SIRET As String) As Entreprise
        For Each Ent As Entreprise In Me
            If Ent.SIRET = SIRET Then Return Ent
        Next
        Return Nothing
    End Function
    Public Function ChercherUnSiret(ByVal RS As String) As Entreprise
        For Each Ent As Entreprise In Me
            If Ent.RaisonSocial = RS Then Return Ent
        Next
        Return Nothing
    End Function

    Public Function ChercherUnSiretBoo(ByVal RS As String) As Boolean
        For Each Ent As Entreprise In Me
            If Ent.RaisonSocial = RS Then Return True
        Next
        Return False
    End Function
End Class
