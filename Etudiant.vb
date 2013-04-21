Public Class Etudiant
    Inherits Element

    Private s_Code As String
    Private s_Nom As String
    Private s_Prenom As String
    Private o_Section As Section
    Private o_Contrat As Contrat
    Private o_Entreprise As Entreprise
    Private b_Selected As Boolean



    Public Sub New(ByVal TheCode As String,
                   ByVal TheNom As String,
                   ByVal ThePrenom As String,
                   ByVal TheSection As Section,
                   ByVal TheContrat As Contrat,
                   ByVal TheEntreprise As Entreprise)

        s_Code = TheCode
        s_Nom = TheNom
        s_Prenom = ThePrenom
        o_Section = TheSection
        o_Contrat = TheContrat
        o_Entreprise = TheEntreprise
        b_Selected = True

        If o_Entreprise IsNot Nothing Then
            o_Entreprise.Etudiants.Add(Me)
        End If

    End Sub
    Public Property Code As String
        Get
            Return s_Code
        End Get
        Set(ByVal value As String)
            If s_Code = value Then Exit Property
            s_Code = value
            Signaler("Code")
        End Set
    End Property
    Public Property Nom As String
        Get
            Return s_Nom
        End Get
        Set(ByVal value As String)
            If s_Nom = value Then Exit Property
            s_Nom = value
            Signaler("Nom")
        End Set
    End Property

    Public Property Prenom As String
        Get
            Return s_Prenom
        End Get
        Set(ByVal value As String)
            If s_Prenom = value Then Exit Property
            s_Prenom = value
            Signaler("Prenom")
        End Set
    End Property


    Public Property Section As Section
        Get
            Return o_Section
        End Get
        Set(ByVal value As Section)
            If o_Section Is value Then Exit Property
            o_Section = value
            Signaler("Section")
        End Set
    End Property

    Public Property Contrat As Contrat
        Get
            Return o_Contrat
        End Get
        Set(ByVal value As Contrat)
            If o_Contrat Is value Then Exit Property
            o_Contrat = value
            Signaler("Contrat")
        End Set
    End Property

    Public Property Entreprise As Entreprise
        Get
            Return o_Entreprise
        End Get
        Set(ByVal value As Entreprise)
            If o_Entreprise Is value Then Exit Property
            o_Entreprise = value
            Signaler("Entreprise")
        End Set
    End Property
    Public Property Selected As Boolean
        Get
            Return b_Selected
        End Get
        Set(ByVal value As Boolean)
            If b_Selected = value Then Exit Property
            b_Selected = value
            Signaler("Selected")
        End Set
    End Property

    Public Sub Filter(ByVal TheFiltre As String)
        If TheFiltre = "" Then
            Me.Selected = True
        Else
            Me.Selected = InStr(s_Nom, TheFiltre) = 1
        End If
    End Sub
End Class
