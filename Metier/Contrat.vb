Public Class Contrat
    Inherits Element
    Private s_Code As Integer
    Private s_CentreDeForm As String
    Private d_DateDebut As String
    Private d_DateFin As String
    Private b_Resilie As Boolean
    Private b_Presence As Boolean

    Public Sub New(ByVal TheCode As Integer,
                   ByVal TheCentre As String,
                   ByVal TheDateDebut As String,
                   ByVal TheDateFin As String,
                   ByVal TheResilie As Boolean,
                   ByVal ThePresence As Boolean)

        s_Code = TheCode
        s_CentreDeForm = TheCentre
        d_DateDebut = TheDateDebut
        d_DateFin = TheDateFin
        b_Resilie = TheResilie
        b_Presence = ThePresence

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

    Public Property CentreDeForm As String
        Get
            Return s_CentreDeForm
        End Get
        Set(ByVal value As String)
            If s_CentreDeForm = value Then Exit Property
            s_CentreDeForm = value
            Signaler("CentreDeForm")
        End Set
    End Property

    Public Property DateDebut As String
        Get
            Return d_DateDebut
        End Get
        Set(ByVal value As String)
            If d_DateDebut = value Then Exit Property
            d_DateDebut = value
            Signaler("DateDebut")
        End Set
    End Property

    Public Property DateFin As String
        Get
            Return d_DateFin
        End Get
        Set(ByVal value As String)
            If d_DateFin = value Then Exit Property
            d_DateFin = value
            Signaler("DateFin")
        End Set
    End Property

    Public Property Resilie As Boolean
        Get
            Return b_Resilie
        End Get
        Set(ByVal value As Boolean)
            If b_Resilie = value Then Exit Property
            b_Resilie = value
            Signaler("Resilie")
        End Set
    End Property

    Public Property Presence As Boolean
        Get
            Return b_Presence
        End Get
        Set(ByVal value As Boolean)
            If b_Presence = value Then Exit Property
            b_Presence = value
            Signaler("Presence")
        End Set
    End Property

End Class
