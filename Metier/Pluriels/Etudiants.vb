Public Class Etudiants
    Inherits GiorgiList(Of Etudiant)

    Public Function Chercher(ByVal TheCode As String) As Etudiant
        For Each E As Etudiant In Me
            If E.Nom = TheCode Then Return E
        Next
        Return Nothing
    End Function
End Class
