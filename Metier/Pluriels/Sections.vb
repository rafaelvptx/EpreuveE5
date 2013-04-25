Public Class Sections
    Inherits GiorgiList(Of Section)

    Public Function Chercher(ByVal Section As Section) As Section
        For Each Sec As Section In Me
            If Sec.Libelle = Section.Libelle Then Return Sec
        Next
        Return Nothing
    End Function


    Public Function ChercherNum(ByVal theCode As Integer) As Section
        For Each Sec As Section In Me
            If Sec.Code = theCode Then Return Sec
        Next
        Return Nothing
    End Function
End Class
