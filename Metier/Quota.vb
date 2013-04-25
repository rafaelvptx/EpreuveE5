Public Class Quota
    Inherits Element

    Private i_Code As Integer
    Private d_QuotaO As Decimal
    Private d_QuotaL As Decimal
    Private d_QuotaA As Decimal
    Private d_QuotaB As Decimal
    Private d_QuotaC As Decimal
    Private d_Total As Decimal


    Public Sub New(ByVal TheO As Decimal,
                   ByVal TheL As Decimal,
                   ByVal TheA As Decimal,
                   ByVal TheB As Decimal,
                   ByVal TheC As Decimal)

        d_QuotaO = TheO
        d_QuotaL = TheL
        d_QuotaA = TheA
        d_QuotaB = TheB
        d_QuotaC = TheC

    End Sub
    Public Sub New(ByVal TheO As Decimal,
                   ByVal TheL As Decimal,
                   ByVal TheA As Decimal,
                   ByVal TheB As Decimal,
                   ByVal TheC As Decimal,
                   ByVal TheCode As Integer)
        i_Code = TheCode
        d_QuotaO = TheO
        d_QuotaL = TheL
        d_QuotaA = TheA
        d_QuotaB = TheB
        d_QuotaC = TheC


    End Sub


    Public Property Code As Integer
        Get
            Return i_Code
        End Get
        Set(ByVal value As Integer)
            If i_Code = value Then Exit Property
            i_Code = value
            Signaler("Code")
        End Set
    End Property
   
    Public Property QuotaO As Decimal
        Get
            Return d_QuotaO
        End Get
        Set(ByVal value As Decimal)
            If d_QuotaO = value Then Exit Property
            d_QuotaO = value
            Signaler("QuotaO")
            Signaler("Total")
        End Set
    End Property

    Public Property QuotaL As Decimal
        Get
            Return d_QuotaL
        End Get
        Set(ByVal value As Decimal)
            If d_QuotaL = value Then Exit Property
            d_QuotaL = value
            Signaler("QuotaL")
            Signaler("Total")
        End Set
    End Property

    Public Property QuotaA As Decimal
        Get
            Return d_QuotaA
        End Get
        Set(ByVal value As Decimal)
            If d_QuotaA = value Then Exit Property
            d_QuotaA = value
            Signaler("QuotaA")
            Signaler("Total")
        End Set
    End Property

    Public Property QuotaB As Decimal
        Get
            Return d_QuotaB
        End Get
        Set(ByVal value As Decimal)
            If d_QuotaB = value Then Exit Property
            d_QuotaB = value
            Signaler("QuotaB")
            Signaler("Total")
        End Set
    End Property

    Public Property QuotaC As Decimal
        Get
            Return d_QuotaC
        End Get
        Set(ByVal value As Decimal)
            If d_QuotaC = value Then Exit Property
            d_QuotaC = value
            Signaler("QuotaC")
            Signaler("Total")
        End Set
    End Property

    Public ReadOnly Property Total As Decimal
        Get
            Return d_QuotaA + d_QuotaB + d_QuotaC + d_QuotaO + d_QuotaL
        End Get
    End Property


End Class
