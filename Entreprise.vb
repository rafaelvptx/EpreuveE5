Public Class Entreprise
    Inherits Element

    Private s_SIRET As String
    Private s_RaisonSocial As String
    Private s_Rue As String
    Private s_CP As String
    Private s_Ville As String
    Private s_NAF As String
    Private s_Contact As String
    Private s_Activite As String
    Private b_Selected As Boolean

    Private o_Etudiants As Etudiants
    Private o_Quota As Quota



    Public Sub New(ByVal TheSIRET As String,
                   ByVal TheRaison As String,
                   ByVal TheRue As String,
                   ByVal TheCP As String,
                   ByVal TheVille As String,
                   ByVal TheNAF As String,
                   ByVal TheContact As String,
                   ByVal TheActivite As String,
                   ByVal TheQuota As Quota)


        s_SIRET = TheSIRET
        s_RaisonSocial = TheRaison
        s_Rue = TheRue
        s_CP = TheCP
        s_Ville = TheVille
        s_NAF = TheNAF
        s_Contact = TheContact
        s_Activite = TheActivite
        o_Quota = TheQuota
        b_Selected = True
        o_Etudiants = New Etudiants

        'o_Versement = TheVersement
        'ByVal TheVersement As Versement

    End Sub

    Public Sub New(ByVal Ent As Entreprise)
        s_SIRET = Ent.s_SIRET
        s_RaisonSocial = Ent.s_RaisonSocial
        s_Rue = Ent.s_Rue
        s_CP = Ent.s_CP
        s_Ville = Ent.s_Ville
        s_NAF = Ent.s_NAF
        s_Contact = Ent.s_Contact
        s_Activite = Ent.s_Activite
        o_Quota = Ent.o_Quota
        o_Etudiants = New Etudiants

    End Sub

    Public Sub New()

    End Sub

    Public Property SIRET As String
        Get
            Return s_SIRET
        End Get
        Set(ByVal value As String)
            If s_SIRET = value Then Exit Property
            s_SIRET = value
            Signaler("SIRET")
        End Set
    End Property

    Public Property RaisonSocial As String
        Get
            Return s_RaisonSocial
        End Get
        Set(ByVal value As String)
            If s_RaisonSocial = value Then Exit Property
            s_RaisonSocial = value
            Signaler("RaisonSocial")
        End Set
    End Property

    Public Property Rue As String
        Get
            Return s_Rue
        End Get
        Set(ByVal value As String)
            If s_Rue = value Then Exit Property
            s_Rue = value
            Signaler("Rue")
        End Set
    End Property

    Public Property CP As String
        Get
            Return s_CP
        End Get
        Set(ByVal value As String)
            If s_CP = value Then Exit Property
            s_CP = value
            Signaler("CP")
        End Set
    End Property

    Public Property Ville As String
        Get
            Return s_Ville
        End Get
        Set(ByVal value As String)
            If s_Ville = value Then Exit Property
            s_Ville = value
            Signaler("Ville")
        End Set
    End Property

    Public Property NAF As String
        Get
            Return s_NAF
        End Get
        Set(ByVal value As String)
            If s_NAF = value Then Exit Property
            s_NAF = value
            Signaler("NAF")
        End Set
    End Property

    Public Property Contact As String
        Get
            Return s_Contact
        End Get
        Set(ByVal value As String)
            If s_Contact = value Then Exit Property
            s_Contact = value
            Signaler("Contact")
        End Set
    End Property

    Public Property Activite As String
        Get
            Return s_Activite
        End Get
        Set(ByVal value As String)
            If s_Activite = value Then Exit Property
            s_Activite = value
            Signaler("Activite")
        End Set
    End Property

    Public Property Quota As Quota
        Get
            Return o_Quota
        End Get
        Set(ByVal value As Quota)
            If o_Quota Is value Then Exit Property
            o_Quota = value
            Signaler("Quota")
        End Set
    End Property

    Public ReadOnly Property Etudiants As Etudiants
        Get
            Return o_Etudiants
        End Get
    End Property

    Public Sub Filtrer(ByVal TheFiltre As String)
        If TheFiltre = "" Then
            Me.Selected = True
        Else
            Me.Selected = InStr(s_RaisonSocial, TheFiltre) = 1
        End If

    End Sub

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
End Class
