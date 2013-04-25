Public Class Section
    Inherits Element

    Private s_Code As Integer
    Private s_Libelle As String
    Private s_Niveau As String



    Public Sub New(ByVal TheCode As Integer,
                   ByVal TheLibelle As String,
                   ByVal TheNiveau As String)

        s_Code = TheCode
        s_Libelle = TheLibelle
        s_Niveau = TheNiveau

    End Sub

    Public Sub New()

    End Sub

    Public Sub New(ByVal o_section As Section)

        s_Code = o_section.Code
        s_Libelle = o_section.Libelle

    End Sub


    Public Property Code As Integer
        Get
            Return s_Code
        End Get
        Set(ByVal value As Integer)
            If s_Code = value Then Exit Property
            s_Code = value
            Signaler("Code")
        End Set
    End Property


    Public Property Libelle As String
        Get
            Return s_Libelle
        End Get
        Set(ByVal value As String)
            If s_Libelle = value Then Exit Property
            s_Libelle = value
            Signaler("Libelle")
        End Set
    End Property

    Public Property Niveau As String
        Get
            Return s_Niveau
        End Get
        Set(ByVal value As String)
            If s_Niveau = value Then Exit Property
            s_Niveau = value
            Signaler("Niveau")
        End Set
    End Property
End Class
