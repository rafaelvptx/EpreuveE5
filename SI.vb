Imports xls = Microsoft.Office.Interop.Excel
Imports System.Data.OleDb


Public Class SI
    Inherits Element

    Private o_Entreprises As Entreprises
    Private o_Etudiants As Etudiants
    Private o_Etudiant As Etudiant
    Private o_Sections As Sections
    Private o_Section As Section
    Private o_Entreprise As Entreprise
    Private o_Contrat As Contrat
    Private o_Quota As Quota

    Private obj_SaisieEtu As SaisieEtudiant
    Private obj_SaisieEnt As SaisieEntreprise

    Private s_FiltreEntreprise As String
    Private s_FiltreEtudiant As String

    Private o_Cnx As New OleDbConnection

    Private appXls As xls.Application ' excel application
    Private sheetXls As xls.Worksheets ' variable pour une feuille

    Public Sub New()

        o_Sections = New Sections
        o_Etudiants = New Etudiants
        o_Entreprises = New Entreprises

        obj_SaisieEtu = New SaisieEtudiant(Me)
        obj_SaisieEnt = New SaisieEntreprise(Me)

        'appXls = New xls.Application
        s_FiltreEntreprise = ""
        ChargerLesDonnees()

    End Sub

    Public ReadOnly Property DesignTime As Boolean
        Get
            Return System.ComponentModel.LicenseManager.UsageMode = ComponentModel.LicenseUsageMode.Designtime
        End Get
    End Property

    Public ReadOnly Property CtrlSaisieEnt As SaisieEntreprise
        Get
            Return obj_SaisieEnt
        End Get
    End Property

    Public ReadOnly Property CtrlSaisieEtu As SaisieEtudiant
        Get
            Return obj_SaisieEtu
        End Get
    End Property

    Public Property FiltreEntreprise As String
        Get
            Return s_FiltreEntreprise
        End Get
        Set(ByVal value As String)
            If s_FiltreEntreprise = value Then Exit Property
            s_FiltreEntreprise = value
            For Each e In o_Entreprises
                e.Filtrer(s_FiltreEntreprise)
            Next
            Signaler("FiltreEntreprise")
        End Set
    End Property

    Public Property FiltreEtudiant As String
        Get
            Return s_FiltreEtudiant
        End Get
        Set(ByVal value As String)
            If s_FiltreEtudiant = value Then Exit Property
            s_FiltreEtudiant = value
            For Each e In o_Etudiants
                e.Filter(s_FiltreEtudiant)
            Next
            Signaler("FiltreEtudiant")
        End Set
    End Property
 
    Public Sub OuvrirConnexion()
        o_Cnx.Open()
    End Sub

    Public Sub FermerConnexion()
        o_Cnx.Close()
    End Sub

    Public ReadOnly Property Entreprises As Entreprises
        Get
            Return o_Entreprises
        End Get
    End Property

    Public ReadOnly Property Etudiants As Etudiants
        Get
            Return o_Etudiants
        End Get
    End Property

    Public ReadOnly Property Sections As Sections
        Get
            Return o_Sections
        End Get
    End Property

    ' Charger le données du fichier excel 
    Public Function ChargerLesDonnees() As Boolean
        Dim Ligne As Integer = 2
        'Dim XLWb As xls.Workbook = appXls.Workbooks.Open("C:\Users\disterburn\Desktop\BaseEntEiffel.xls")
        'Dim XLSh As xls.Worksheet = XLWb.Sheets("Feuil1")


        o_Cnx.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=""C:\Users\rafael\Documents\Visual Studio 2010\Projects\EntrepriseCFA\BaseEntEiffel.accdb"""
        o_Cnx.Open()
        ' connection a la base 
        ChargerLesSections()
        ChargerEntrepriseDsBase()
        ChargerEtudiantDsBase()
        'For Ligne = 2 To 1116
        '    ChargerEntreprise(XLSh, Ligne)
        '    ChargerEtudiant(XLSh, Ligne)
        'Next

        'XLWb.Close()
        'appXls.Quit()
        o_Cnx.Close()

        Return True


    End Function

    Private Sub ChargerEntrepriseDsBase()
        '          0                         1             2                  3            4                     5             6                       7                   8
        'SELECT SIRET, RaisonSocial, Rue, CP, Ville, NAF, Contact, Activite, IDQuota, 
        '       9              10          11         12            13
        'QuotaO, QuotaL, QuotaA, QuotaB, QuotaC
        'FROM Quota INNER JOIN Entreprise ON Quota.Id = Entreprise.IdQuota;

        o_Entreprises.Muet = True
        o_Entreprises.Clear()
        Dim SelectEnt As New OleDbCommand

        'SelectEnt.CommandText = "SELECT SIRET, RaisonSocial, Rue, CP, Ville, NAF, Contact, Activite, IDQuota" &
        '    Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
        '    "FROM     Entreprise"

        SelectEnt.CommandText = "SELECT SIRET, RaisonSocial, Rue, CP, Ville, NAF, Contact, Activite, IDQuota, QuotaO, QuotaL, QuotaA, QuotaB, QuotaC" &
            " FROM Quota INNER JOIN Entreprise ON Quota.Id = Entreprise.IdQuota"
        SelectEnt.Connection = Me.o_Cnx

        Try
            Dim RDR As OleDbDataReader = SelectEnt.ExecuteReader
            Do While RDR.Read()
                o_Quota = New Quota(RDR.GetInt32(8), RDR.GetDecimal(9), RDR.GetDecimal(10), RDR.GetDecimal(11), RDR.GetDecimal(12), RDR.GetDecimal(13))
                ' o_Quota = ChargerQuotaDsBase(RDR.GetInt32(8))
                Dim ENT As New Entreprise(RDR.GetString(0), NullToNothing(RDR.GetString(1)), NullToNothing(RDR.GetString(2)),
                                         NullToNothing(RDR.GetString(3)), NullToNothing(RDR.GetString(4)), NullToNothing(RDR.GetValue(5)), NullToNothing(RDR.GetValue(6)),
                                         NullToNothing(RDR.GetValue(7)), o_Quota)

                o_Entreprises.Add(ENT)
                If Me.DesignTime AndAlso o_Entreprises.Count = 20 Then Exit Do

            Loop
            RDR.Close()

        Catch ex As Exception
            Dim toto As Integer = 5
        End Try
        o_Entreprises.Muet = False

    End Sub

    'Private Function ChargerQuotaDsBase(ByVal IdQuota As Integer) As Quota
    '    Static SectQuota As OleDbCommand
    '    If SectQuota Is Nothing Then
    '        SectQuota = New OleDbCommand
    '        SectQuota.CommandText = "SELECT QuotaO, QuotaL, QuotaA, QuotaB, QuotaC" &
    '                    Global.Microsoft.VisualBasic.ChrW(13) &
    '                    Global.Microsoft.VisualBasic.ChrW(10) &
    '                    "FROM     Quota" &
    '                    Global.Microsoft.VisualBasic.ChrW(13) &
    '                    Global.Microsoft.VisualBasic.ChrW(10) &
    '                    "WHERE  (Id = ?)"
    '        SectQuota.Connection = Me.o_Cnx

    '        SectQuota.Parameters.AddRange(New OleDbParameter() {
    '                                      New OleDbParameter("Id", System.Data.OleDb.OleDbType.[Integer], 0, "Id")})
    '    End If


    '    Try
    '        SectQuota.Parameters.Item(0).Value = IdQuota
    '        Dim RDR As OleDbDataReader = SectQuota.ExecuteReader
    '        Do While RDR.Read()

    '            o_Quota = New Quota(RDR.GetDecimal(0), RDR.GetDecimal(1), RDR.GetDecimal(2), RDR.GetDecimal(3), RDR.GetDecimal(4))

    '        Loop
    '        RDR.Close()
    '        Return o_Quota

    '    Catch ex As Exception
    '    End Try
    '    Return Nothing
    'End Function

    Private Sub ChargerEtudiantDsBase()
        Dim SelectEtu As New OleDbCommand
        Dim Ent As Entreprise
        Dim Sec As Section
        'SelectEtu.CommandText = "SELECT Nom, Prenom, IdSection, IdContrat, IdEntreprise FROM     Etudiant"
        o_Etudiants.Muet = True
        o_Etudiants.Clear()
        '                               0   1       2           3       4               5           6       7       8       9       10
        SelectEtu.CommandText = "SELECT Etudiant.ID ,Nom, Prenom, IdSection, IdContrat, CentreDeForm, DateDebut, DateFin, Resilie, Presence,IdEntreprise" &
            " FROM Contrat INNER JOIN Etudiant ON Contrat.Id = Etudiant.IdContrat"


        SelectEtu.Connection = Me.o_Cnx

        Try
            Dim RDR As OleDbDataReader = SelectEtu.ExecuteReader
            Do While RDR.Read()
                If NullToNothing(RDR.GetValue(3)) = Nothing Then
                    Sec = Nothing
                Else
                    Sec = o_Sections.ChercherNum(RDR.GetInt32(3))

                End If

                If NullToNothing(RDR.GetValue(10)) = Nothing Then
                    Ent = Nothing
                Else
                    Ent = o_Entreprises.ChercherUneEntreprise(RDR.GetString(10))
                End If

                o_Contrat = New Contrat(RDR.GetInt32(4), NullToNothing(RDR.GetValue(5)), NullToNothing(RDR.GetValue(6)),
                                        NullToNothing(RDR.GetValue(7)), NullToNothing(RDR.GetValue(8)), NullToNothing(RDR.GetValue(9)))

                Dim ETU As New Etudiant(RDR.GetInt32(0), NullToNothing(RDR.GetValue(1)), NullToNothing(RDR.GetValue(2)), Sec, o_Contrat, Ent)
                o_Etudiants.Add(ETU)
                If Me.DesignTime AndAlso o_Etudiants.Count = 20 Then Exit Do
            Loop
            RDR.Close()

        Catch ex As Exception
            Dim toto As Integer = 5
        End Try
        o_Etudiants.Muet = False

    End Sub

    Private Function ChargerLesSections() As Boolean
        Dim SelectSect As New OleDbCommand

        SelectSect.CommandText = "SELECT Code, Libelle, Niveau FROM     [Section] order by Code"
        SelectSect.Connection = Me.o_Cnx
        o_Sections.Muet = True
        o_Sections.Clear()


        Try
            Dim RDR As OleDbDataReader = SelectSect.ExecuteReader
            Do While RDR.Read()

                o_Sections.Add(New Section(RDR.GetInt32(0), NullToNothing(RDR.GetValue(1)), NullToNothing(RDR.GetValue(2))))

            Loop
            RDR.Close()

        Catch ex As Exception
            Dim toto As Integer = 5
        End Try
        Return True
    End Function

    Public Function ChargerSectionDsBase(ByVal IdSection As Integer) As Section

        Dim SelectSect As New OleDbCommand

        SelectSect.CommandText = "SELECT Code, Libelle, Niveau FROM     [Section] WHERE  (Code = ?)"

        SelectSect.Connection = Me.o_Cnx

        SelectSect.Parameters.AddRange(New OleDbParameter() {
                                      New OleDbParameter("Code", System.Data.OleDb.OleDbType.[Integer], 0, "Code")})

        SelectSect.Parameters.Item(0).Value = IdSection

        Try
            Dim RDR As OleDbDataReader = SelectSect.ExecuteReader
            Do While RDR.Read()

                o_Section = New Section(RDR.GetInt32(0), NullToNothing(RDR.GetValue(1)), NullToNothing(RDR.GetValue(2)))

            Loop
            RDR.Close()
            Return o_Section

        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    Public Function ChargerContratDsBase(ByVal IdContrat As Integer) As Contrat
        Dim SelectContrat As New OleDbCommand

        SelectContrat.CommandText = "SELECT Id, CentreDeForm, DateDebut, DateFin, Resilie, Presence" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FROM     Contrat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WHE" & _
            "RE  (Id = ?)"
        SelectContrat.Connection = Me.o_Cnx
        SelectContrat.Parameters.AddRange(New OleDbParameter() {
                              New OleDbParameter("Id", System.Data.OleDb.OleDbType.[Integer], 0, "Id")})

        SelectContrat.Parameters.Item(0).Value = IdContrat

        Try
            Dim RDR As OleDbDataReader = SelectContrat.ExecuteReader
            Do While RDR.Read()

                o_Contrat = New Contrat(RDR.GetInt32(0), NullToNothing(RDR.GetValue(1)), NullToNothing(RDR.GetValue(2)),
                                       NullToNothing(RDR.GetValue(3)), RDR.GetBoolean(4), RDR.GetBoolean(5))

            Loop
            RDR.Close()
            Return o_Contrat

        Catch ex As Exception
        End Try
        Return Nothing
    End Function

    ' On charge ligne par ligne  les entreprises, etudiants et les informations correspondantes 
    Private Sub ChargerEntreprise(ByVal TheSheet As xls.Worksheet, ByVal Ligne As Integer)
        Dim SIRET, RS, Rue, CP, Ville, NAF, Contact, Activite As String
        Dim OBJ As Object
        Contact = Nothing
        OBJ = TheSheet.Cells(Ligne, 10)
        SIRET = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 6)
        RS = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 7)
        Rue = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 8)
        CP = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 9)
        Ville = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 47)
        NAF = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 48)
        Activite = OBJ.Value


        o_Entreprise = New Entreprise(SIRET, RS, Rue, CP, Ville, NAF, Contact, Activite, ChargerQuota(TheSheet, Ligne))
        If Not TrouverUnDoublon(o_Entreprise) Then
            o_Entreprises.Add(New Entreprise(o_Entreprise))
            EnregisterQuota(o_Quota)
            EnregisterEntreprise(o_Entreprise)

        End If



    End Sub

    Private Sub ChargerEtudiant(ByVal TheSheet As xls.Worksheet, ByVal ligne As Integer)
        Dim NOM, PRENOM As String
        Dim OBJ As Object

        OBJ = TheSheet.Cells(ligne, 17)
        NOM = OBJ.Value
        OBJ = TheSheet.Cells(ligne, 18)
        PRENOM = OBJ.Value

        If NOM = Nothing Then Exit Sub

        o_Etudiants.Add(New Etudiant("", NOM, PRENOM, ChargerSection(TheSheet, ligne), ChargerContrat(TheSheet, ligne), o_Entreprises.Chercher(o_Entreprise)))
        o_Etudiant = New Etudiant("", NOM, PRENOM, o_Section, o_Contrat, o_Entreprise)
        EnregisterEtudiant(o_Etudiant)

    End Sub

    Public Function ChargerSection(ByVal theSheet As xls.Worksheet, ByVal ligne As Integer) As Section
        Dim OBJ As Object
        Dim SECTION, CODE As String
        OBJ = theSheet.Cells(ligne, 55)
        CODE = OBJ.value

        OBJ = theSheet.Cells(ligne, 19)
        SECTION = OBJ.Value

        o_Section = New Section(CODE, SECTION, "")

        If o_Section Is Nothing Then
            Return Nothing
        Else
            If Not TrouverUneSection(o_Section) Then
                o_Sections.Add(New Section(o_Section))
                EnregisterSection(o_Section)
            End If
        End If
        Return o_Section
    End Function

    Public Function ChargerContrat(ByVal TheSheet As xls.Worksheet, ByVal Ligne As Integer) As Contrat

        Dim OBJ As Object
        Dim CentreDeForm As String
        Dim DateDebut, DateFin As String
        Dim Resilie, Presence As Boolean
        Dim o_Contrat As Contrat

        OBJ = TheSheet.Cells(Ligne, 51)
        CentreDeForm = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 49)
        DateDebut = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 50)
        DateFin = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 52)
        If OBJ.value = "O" Then
            Resilie = True
        End If
        OBJ = TheSheet.Cells(Ligne, 53)
        If OBJ.value = "O" Then
            Presence = True
        End If

        o_Contrat = New Contrat(Nothing, CentreDeForm, DateDebut, DateFin, Resilie, Presence)
        EnregisterContrat(o_Contrat)

        Return o_Contrat

    End Function

    Public Function ChargerQuota(ByVal TheSheet As xls.Worksheet, ByVal Ligne As Integer) As Quota

        Dim OBJ As Object
        Dim O, L, A, B, C As Decimal


        OBJ = TheSheet.Cells(Ligne, 11)
        O = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 12)
        L = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 13)
        A = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 14)
        B = OBJ.Value
        OBJ = TheSheet.Cells(Ligne, 15)
        C = OBJ.Value

        o_Quota = New Quota("", O, L, A, B, C)

        ' Debug.WriteLine(O & " " & L & " " & A & " " & B & " " & C)

        Return o_Quota

    End Function

    Public Sub EnregisterEntreprise(ByVal UneEntreprise As Entreprise)
        Dim InsertEnt As New OleDbCommand
        Dim DernierQuota As New OleDbCommand

        InsertEnt.CommandText = "INSERT INTO Entreprise" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) &
            "                  (SIRET, RaisonSocial, Rue, CP, Ville, NAF, Contact, Activite, IDQuota)" &
             Global.Microsoft.VisualBasic.ChrW(13) &
             Global.Microsoft.VisualBasic.ChrW(10) &
             "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)"

        InsertEnt.Connection = Me.o_Cnx

        InsertEnt.Parameters.AddRange(New OleDbParameter() {
                                      New OleDbParameter("SIRET", System.Data.OleDb.OleDbType.WChar, 255, "SIRET"),
                                      New OleDbParameter("RaisonSocial", System.Data.OleDb.OleDbType.WChar, 255, "RaisonSocial"),
                                      New OleDbParameter("Rue", System.Data.OleDb.OleDbType.WChar, 255, "Rue"),
                                      New OleDbParameter("CP", System.Data.OleDb.OleDbType.WChar, 255, "CP"),
                                      New OleDbParameter("Ville", System.Data.OleDb.OleDbType.WChar, 255, "Ville"),
                                      New OleDbParameter("NAF", System.Data.OleDb.OleDbType.WChar, 255, "NAF"),
                                      New OleDbParameter("Contact", System.Data.OleDb.OleDbType.WChar, 255, "Contact"),
                                      New OleDbParameter("Activite", System.Data.OleDb.OleDbType.WChar, 255, "Activite"),
                                      New OleDbParameter("IDQuota", System.Data.OleDb.OleDbType.[Integer], 0, "IDQuota")})



        '---------------------------N° Du quota de lentreprise ----------------------------
        Dim ch As String = "SELECT MAX(Id) AS LastQuota FROM     Quota"

        Dim TheSelect As New OleDbCommand(ch, o_Cnx)
        Dim LastInsert As Integer = 0

        Try
            Dim OBJ As Object = TheSelect.ExecuteScalar
            LastInsert = OBJ

        Catch ex As Exception
            LastInsert = 1
        End Try
        '---------------------------------------------------------------------------------
        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0

        Try
            With InsertEnt
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UneEntreprise
                    MettreParametres(InsertEnt, .SIRET, .RaisonSocial, .Rue, .CP, .Ville,
                                     NothingToNull(.NAF), NothingToNull(.Contact), NothingToNull(.Activite), LastInsert)
                End With
                'EQUIVALENT A METTREPARAMETTRE
                'With .Parameters
                '    .Item(0).Value = UneEntreprise.SIRET
                '    .Item(1).Value = UneEntreprise.RaisonSocial
                '    .Item(2).Value = UneEntreprise.Rue
                '    .Item(3).Value = UneEntreprise.CP
                '    .Item(4).Value = UneEntreprise.Ville
                '    .Item(5).Value = NothingToNull(UneEntreprise.NAF)
                '    .Item(6).Value = NothingToNull(UneEntreprise.Activite)
                '    .Item(7).Value = LastInsert
                'End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then
                o_Entreprises.Add(UneEntreprise)
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
        End Try
        InsertEnt.Transaction = Nothing
    End Sub

    Public Sub EnregisterEtudiant(ByVal UnEtudiant As Etudiant)

        Dim InsertEtu As New OleDbCommand

        InsertEtu.CommandText = "INSERT INTO Etudiant" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) &
            " ( Nom, Prenom, IdSection, IdContrat, IdEntreprise)" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) &
            "VALUES ( ?, ?, ?, ?, ?)"

        InsertEtu.Connection = Me.o_Cnx
        InsertEtu.Parameters.AddRange(New OleDbParameter() {
                                      New OleDbParameter("Nom", System.Data.OleDb.OleDbType.WChar, 255, "Nom"),
                                      New OleDbParameter("Prenom", System.Data.OleDb.OleDbType.WChar, 255, "Prenom"),
                                      New OleDbParameter("IdSection", System.Data.OleDb.OleDbType.[Integer], 0, "IdSection"),
                                      New OleDbParameter("IdContat", System.Data.OleDb.OleDbType.[Integer], 0, "IdContat"),
                                      New OleDbParameter("IdEntreprise", System.Data.OleDb.OleDbType.WChar, 255, "IdEntreprise")})
        '------------------------------------------------------------------------------------------------------

        '----------------------------Retrouver le code de sa section-------------------------------------------



        Dim ch As String = "SELECT Code FROM  [Section] WHERE  (Libelle = ?)"


        Dim SelectSection As New OleDbCommand(ch, o_Cnx)
        Dim LaSection As Integer = 0

        SelectSection.Parameters.AddRange(New OleDbParameter() {
                                          New OleDbParameter("Libelle", System.Data.OleDb.OleDbType.WChar, 255, "Libelle")})
        Try
            SelectSection.Parameters.Item(0).Value = NothingToNull(UnEtudiant.Section.Libelle)

            Dim OBJ As Object = SelectSection.ExecuteScalar
            LaSection = OBJ

        Catch ex As Exception
            LaSection = 1
        End Try

        '------------------------------------------------------------------------------------------------------

        '---------------------------N° du contrat de L'étudiant --------------------------------------------------
        Dim ch2 As String = "SELECT MAX(Id) AS LastQuota FROM     Contrat"

        Dim TheSelectContrat As New OleDbCommand(ch2, o_Cnx)
        Dim LastInsert As Integer = 0
        Try
            Dim OBJ As Object = TheSelectContrat.ExecuteScalar
            LastInsert = OBJ
        Catch ex As Exception
            LastInsert = 1
        End Try
        '------------------------------------------------------------------------------------------------------

        '------------------------------------------------------------------------------------------------------
        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0

        Try
            With InsertEtu
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnEtudiant
                    MettreParametres(InsertEtu, NothingToNull(.Nom), NothingToNull(.Prenom),
                                     NothingToNullInt(LaSection), LastInsert, NothingToNull(.Entreprise.SIRET))
                End With
                NB = .ExecuteNonQuery
            End With

            If NB = 1 Then
                TRANS.Commit()
                o_Etudiants.Add(UnEtudiant)
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            InsertEtu.Transaction = Nothing
        End Try

    End Sub

    Public Sub EnregisterSection(ByVal UneSection As Section)
        Dim InsertSection As New OleDbCommand

        InsertSection.CommandText = "INSERT INTO [Section]" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) & " (Libelle)" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) & "VALUES ( ?)"

        InsertSection.Connection = Me.o_Cnx

        InsertSection.Parameters.AddRange(New OleDbParameter() {
                                          New OleDbParameter("Libelle", System.Data.OleDb.OleDbType.WChar, 255, "Libelle")})

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0


        Try
            With InsertSection
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UneSection
                    MettreParametres(InsertSection, NothingToNull(.Libelle))
                End With
                NB = .ExecuteNonQuery
            End With

            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If
        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
        End Try
        InsertSection.Transaction = Nothing
    End Sub

    Public Sub EnregisterContrat(ByVal UnContrat As Contrat)
        Dim InsertContrat As New OleDbCommand

        InsertContrat.CommandText = "INSERT INTO Contrat" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) & "(CentreDeForm, DateDebut, DateFin, Resilie, Presence)" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) & "VALUES ( ?, ?, ?, ?, ?)"

        InsertContrat.Connection = Me.o_Cnx

        InsertContrat.Parameters.AddRange(New OleDbParameter() {
                                          New OleDbParameter("CentreDeForm", System.Data.OleDb.OleDbType.WChar, 255, "CentreDeForm"),
                                          New OleDbParameter("DateDebut", System.Data.OleDb.OleDbType.WChar, 255, "DateDebut"),
                                          New OleDbParameter("DateFin", System.Data.OleDb.OleDbType.WChar, 255, "DateFin"),
                                          New OleDbParameter("Resilie", System.Data.OleDb.OleDbType.[Boolean], 2, "Resilie"),
                                          New OleDbParameter("Presence", System.Data.OleDb.OleDbType.[Boolean], 2, "Presence")})

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0


        Try
            With InsertContrat
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnContrat
                    MettreParametres(InsertContrat, NothingToNull(.CentreDeForm),
                                     NothingToNull(.DateDebut), NothingToNull(.DateFin),
                                     NothingToNull(.Resilie), NothingToNull(.Presence))

                End With
                NB = .ExecuteNonQuery
            End With

            If NB = 1 Then
                TRANS.Commit()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            InsertContrat.Transaction = Nothing
        End Try
    End Sub

    Public Sub EnregisterQuota(ByVal UnQuota As Quota)

        Dim InsertQuota As New OleDbCommand

        InsertQuota.CommandText = "INSERT INTO Quota" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) & " ( QuotaO, QuotaL, QuotaA, QuotaB, QuotaC, Total)" &
            Global.Microsoft.VisualBasic.ChrW(13) &
            Global.Microsoft.VisualBasic.ChrW(10) & "VALUES ( ?, ?, ?, ?, ?, ?)"

        InsertQuota.Connection = Me.o_Cnx
        InsertQuota.Parameters.AddRange(New OleDbParameter() {
                                        New OleDbParameter("QuotaO", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input, False, CType(19, Byte), CType(0, Byte), "QuotaO", System.Data.DataRowVersion.Current, Nothing),
                                        New OleDbParameter("QuotaL", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input, False, CType(19, Byte), CType(0, Byte), "QuotaL", System.Data.DataRowVersion.Current, Nothing),
                                        New OleDbParameter("QuotaA", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input, False, CType(19, Byte), CType(0, Byte), "QuotaA", System.Data.DataRowVersion.Current, Nothing),
                                        New OleDbParameter("QuotaB", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input, False, CType(19, Byte), CType(0, Byte), "QuotaB", System.Data.DataRowVersion.Current, Nothing),
                                        New OleDbParameter("QuotaC", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input, False, CType(19, Byte), CType(0, Byte), "QuotaC", System.Data.DataRowVersion.Current, Nothing),
                                        New OleDbParameter("Total", System.Data.OleDb.OleDbType.WChar, 255, "Total")})

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0
        Try
            With InsertQuota
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnQuota
                    MettreParametres(InsertQuota, .QuotaO, .QuotaL, .QuotaA, .QuotaB, .QuotaC, .Total)
                End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            InsertQuota.Transaction = Nothing
        End Try
    End Sub

    Public Sub SupprimerUnEtudiant(ByVal UnEtudiant As Etudiant)
        Dim DelETU As New OleDbCommand
        OuvrirConnexion()

        DelETU.CommandText = "DELETE FROM Etudiant" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WHERE  (ID = ?)"
        DelETU.Connection = Me.o_Cnx
        DelETU.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {New System.Data.OleDb.OleDbParameter("ID", System.Data.OleDb.OleDbType.[Integer], 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing)})

        DelETU.Parameters.Item(0).Value = UnEtudiant.Code

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0
        Try
            With DelETU
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnEtudiant
                    MettreParametres(DelETU, .Code)
                End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then

                TRANS.Commit()
                o_Etudiants.Remove(UnEtudiant)
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            DelETU.Transaction = Nothing
        End Try
        SupprimerUnContrat(UnEtudiant.Contrat)
        FermerConnexion()
    End Sub

    Public Sub SupprimerUnContrat(ByVal UnContrat As Contrat)
        Dim DelContrat As New OleDbCommand

        DelContrat.CommandText = "DELETE FROM Contrat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WHERE  (Id = ?)"
        DelContrat.Connection = Me.o_Cnx
        DelContrat.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {New System.Data.OleDb.OleDbParameter("Id", System.Data.OleDb.OleDbType.[Integer], 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing)})

        DelContrat.Parameters.Item(0).Value = UnContrat.Code

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0
        Try
            With DelContrat
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnContrat
                    MettreParametres(DelContrat, .Code)
                End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If
        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            DelContrat.Transaction = Nothing
        End Try
    End Sub

    Public Sub SupprimerUneEntreprise(ByVal UneEntreprise As Entreprise)
        OuvrirConnexion()
        Dim DelENT As New OleDbCommand
        DelENT.CommandText = "DELETE FROM Entreprise" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WHERE  (SIRET = ?)"
        DelENT.Connection = Me.o_Cnx
        DelENT.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {New System.Data.OleDb.OleDbParameter("SIRET", System.Data.OleDb.OleDbType.WChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SIRET", System.Data.DataRowVersion.Original, Nothing)})
        DelENT.Parameters.Item(0).Value = UneEntreprise.SIRET

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0
        Try
            With DelENT
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UneEntreprise
                    MettreParametres(DelENT, .SIRET)
                End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then
                TRANS.Commit()
                o_Entreprises.Remove(UneEntreprise)
            Else
                TRANS.Rollback()
            End If
        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            DelENT.Transaction = Nothing
        End Try
        SupprimerUnQuota(UneEntreprise.Quota)
        FermerConnexion()
    End Sub

    Public Sub SupprimerUnQuota(ByVal UnQuota As Quota)
        Dim DelQuota As New OleDbCommand

        DelQuota.CommandText = "DELETE FROM Quota" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WHERE  (Id = ?)"
        DelQuota.Connection = Me.o_Cnx
        DelQuota.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {New System.Data.OleDb.OleDbParameter("Id", System.Data.OleDb.OleDbType.[Integer], 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing)})
        DelQuota.Parameters.Item(0).Value = UnQuota.Code

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0
        Try
            With DelQuota
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnQuota
                    MettreParametres(DelQuota, .Code)
                End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If
        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            DelQuota.Transaction = Nothing
        End Try
    End Sub

    Public Sub ModifierUneEntreprise(ByVal UneEntreprise As Entreprise, ByVal AncienENT As Entreprise)
        Dim ModifEnt As New OleDbCommand
        ModifEnt.CommandText = "UPDATE Entreprise" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
            "SET          SIRET = ?, RaisonSocial = ?, Rue = ?, CP = ?, Ville = ?, NAF = ?, Contact = ?, Activite = ?" &
            Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
            "WHERE  (SIRET = ?)"
        OuvrirConnexion()
        ModifEnt.Connection = Me.o_Cnx
        ModifEnt.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {
                                     New System.Data.OleDb.OleDbParameter("SIRET", System.Data.OleDb.OleDbType.WChar, 255, "SIRET"),
                                     New System.Data.OleDb.OleDbParameter("RaisonSocial", System.Data.OleDb.OleDbType.WChar, 255, "RaisonSocial"),
                                     New System.Data.OleDb.OleDbParameter("Rue", System.Data.OleDb.OleDbType.WChar, 255, "Rue"),
                                     New System.Data.OleDb.OleDbParameter("CP", System.Data.OleDb.OleDbType.WChar, 255, "CP"),
                                     New System.Data.OleDb.OleDbParameter("Ville", System.Data.OleDb.OleDbType.WChar, 255, "Ville"),
                                     New System.Data.OleDb.OleDbParameter("NAF", System.Data.OleDb.OleDbType.WChar, 255, "NAF"),
                                     New System.Data.OleDb.OleDbParameter("Contact", System.Data.OleDb.OleDbType.WChar, 255, "Contact"),
                                     New System.Data.OleDb.OleDbParameter("Activite", System.Data.OleDb.OleDbType.WChar, 255, "Activite"),
                                     New System.Data.OleDb.OleDbParameter("Original_SIRET", System.Data.OleDb.OleDbType.WChar, 255, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "SIRET", System.Data.DataRowVersion.Original, Nothing)})


        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0

        ModifierUnQuota(AncienENT.Quota)

        Try
            With ModifEnt
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UneEntreprise
                    MettreParametres(ModifEnt, .SIRET, .RaisonSocial, NothingToNull(.Rue), NothingToNull(.CP), NothingToNull(.Ville),
                                     NothingToNull(.NAF), NothingToNull(.Contact), NothingToNull(.Activite), AncienENT.SIRET)
                End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
        End Try
        ModifEnt.Transaction = Nothing

        FermerConnexion()
    End Sub

    Public Sub ModifierUnQuota(ByVal UnQota As Quota)
        Dim ModifQuota As New OleDbCommand

        ModifQuota.CommandText = "UPDATE Quota SET          QuotaO = ?, QuotaL = ?, QuotaA = ?, QuotaB = ?, QuotaC = ? WHERE  (Id = ?)"
        ModifQuota.Connection = Me.o_Cnx
        ModifQuota.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {
                                       New System.Data.OleDb.OleDbParameter("QuotaO", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input,
                                                                            False, CType(19, Byte), CType(0, Byte), "QuotaO", System.Data.DataRowVersion.Current, Nothing),
                                       New System.Data.OleDb.OleDbParameter("QuotaL", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input,
                                                                            False, CType(19, Byte), CType(0, Byte), "QuotaL", System.Data.DataRowVersion.Current, Nothing),
                                       New System.Data.OleDb.OleDbParameter("QuotaA", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input,
                                                                            False, CType(19, Byte), CType(0, Byte), "QuotaA", System.Data.DataRowVersion.Current, Nothing),
                                       New System.Data.OleDb.OleDbParameter("QuotaB", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input,
                                                                            False, CType(19, Byte), CType(0, Byte), "QuotaB", System.Data.DataRowVersion.Current, Nothing),
                                       New System.Data.OleDb.OleDbParameter("QuotaC", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input,
                                                                            False, CType(19, Byte), CType(0, Byte), "QuotaC", System.Data.DataRowVersion.Current, Nothing),
                                       New System.Data.OleDb.OleDbParameter("Original_Id", System.Data.OleDb.OleDbType.[Integer], 0, System.Data.ParameterDirection.Input,
                                                                            False, CType(0, Byte), CType(0, Byte), "Id", System.Data.DataRowVersion.Original, Nothing)})

        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0



        Try
            With ModifQuota
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnQota
                    MettreParametres(ModifQuota, .QuotaO, .QuotaL, .QuotaA, .QuotaB, .QuotaC, UnQota.Code)
                End With
                NB = .ExecuteNonQuery
            End With
            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
        End Try
        ModifQuota.Transaction = Nothing
    End Sub

    Public Sub ModifierUnEtudiant(ByVal UnEtudiant As Etudiant, ByVal SIRET As String)

        Dim ModifEtud As New OleDbCommand
        ModifEtud.CommandText = "UPDATE Etudiant SET Nom = ?, Prenom = ?, IdSection = ?, IdEntreprise = ? WHERE ID = ?"
        ModifEtud.Connection = Me.o_Cnx
        ModifEtud.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {New System.Data.OleDb.OleDbParameter("Nom", System.Data.OleDb.OleDbType.WChar, 255, "Nom"),
                                                                              New System.Data.OleDb.OleDbParameter("Prenom", System.Data.OleDb.OleDbType.WChar, 255, "Prenom"),
                                                                              New System.Data.OleDb.OleDbParameter("IdSection", System.Data.OleDb.OleDbType.[Integer], 0, "IdSection"),
                                                                              New System.Data.OleDb.OleDbParameter("IdEntreprise", System.Data.OleDb.OleDbType.WChar, 255, "IdEntreprise"),
                                                                              New System.Data.OleDb.OleDbParameter("Original_ID", System.Data.OleDb.OleDbType.[Integer], 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "ID", System.Data.DataRowVersion.Original, Nothing)})


        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0
        UnEtudiant.Section = o_Sections.Chercher(UnEtudiant.Section)
        UnEtudiant.Entreprise = o_Entreprises.ChercherUneEntreprise(SIRET)
        ModifierUnContrat(UnEtudiant.Contrat)
        Try
            With ModifEtud
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnEtudiant
                    MettreParametres(ModifEtud, NothingToNull(.Nom), NothingToNull(.Prenom),
                                     NothingToNullInt(.Section.Code), NothingToNull(.Entreprise.SIRET), UnEtudiant.Code)
                End With
                NB = .ExecuteNonQuery
            End With

            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            ModifEtud.Transaction = Nothing
        End Try
        FermerConnexion()
    End Sub

    Private Sub ModifierUnContrat(ByVal UnContrat As Contrat)
        Dim ModifContrat As New OleDbCommand
        ModifContrat.CommandText = "UPDATE       Contrat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "SET                CentreDeForm = ?, DateDebut = ?, DateFin" & _
 " = ?, Resilie = ?, Presence = ?" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "WHERE        (Id = ?)"
        ModifContrat.Connection = Me.o_Cnx
        ModifContrat.Parameters.AddRange(New System.Data.OleDb.OleDbParameter() {
                                         New System.Data.OleDb.OleDbParameter("CentreDeForm", System.Data.OleDb.OleDbType.WChar, 255, "CentreDeForm"),
                                         New System.Data.OleDb.OleDbParameter("DateDebut", System.Data.OleDb.OleDbType.WChar, 255, "DateDebut"),
                                         New System.Data.OleDb.OleDbParameter("DateFin", System.Data.OleDb.OleDbType.WChar, 255, "DateFin"),
                                         New System.Data.OleDb.OleDbParameter("Resilie", System.Data.OleDb.OleDbType.[Boolean], 2, "Resilie"),
                                         New System.Data.OleDb.OleDbParameter("Presence", System.Data.OleDb.OleDbType.[Boolean], 2, "Presence"),
                                         New System.Data.OleDb.OleDbParameter("Original_Id", System.Data.OleDb.OleDbType.[Integer], 0, System.Data.ParameterDirection.Input, False, CType(0, Byte), CType(0, Byte), "Id", System.Data.DataRowVersion.Original, Nothing)})



        Dim TRANS As OleDbTransaction = Nothing
        Dim NB As Integer = 0
        Try
            With ModifContrat
                TRANS = .Connection.BeginTransaction
                .Transaction = TRANS
                With UnContrat
                    MettreParametres(ModifContrat, NothingToNull(.CentreDeForm), NothingToNull(.DateDebut),
                                     NothingToNull(.DateDebut), NothingToNull(.DateFin), NothingToNull(.Resilie), NothingToNull(.Presence), UnContrat.Code)
                End With
                NB = .ExecuteNonQuery
            End With

            If NB = 1 Then
                TRANS.Commit()
            Else
                TRANS.Rollback()
            End If

        Catch ex As Exception
            If TRANS IsNot Nothing Then
                TRANS.Rollback()
            End If
            ModifContrat.Transaction = Nothing
        End Try

    End Sub

    'On cherche les entreprises qui sont en double afin d 'obtenir un listOf correct 
    Public Function TrouverUnDoublon(ByVal UneEntreprise As Entreprise) As Boolean
        Dim En As Entreprise

        En = o_Entreprises.Chercher(UneEntreprise)
        If En Is Nothing Then
            Return False
        Else
            If En.SIRET = UneEntreprise.SIRET Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function TrouverUneSection(ByVal UneSection As Section) As Boolean
        Dim Sec As Section
        Sec = o_Sections.Chercher(UneSection)


        If Sec Is Nothing Then
            Return False
        Else
            If Sec.Libelle = UneSection.Libelle Then
                Return True
            End If
        End If
        Return False
    End Function

    ' mettre les paramètres dans une requête SQL 
    Private Sub MettreParametres(ByVal CMD As OleDbCommand, ByVal ParamArray Params() As Object)
        Dim i As Integer = 0
        For Each OBJ As Object In Params
            CMD.Parameters.Item(i).Value = OBJ
            i += 1
        Next
    End Sub

    'Verifie si la valeur est null afin de l enregistrer a null dans la BDD (Une valeur null typer ne peut pas être enregisté)
    Private Function NothingToNull(ByVal V As String) As Object
        If V Is Nothing Then Return DBNull.Value
        Return V
    End Function

    Private Function NothingToNullInt(ByVal VI As Integer) As Object
        If VI = Nothing Then Return DBNull.Value
        Return VI
    End Function

    Private Function NullToNothing(ByVal V As Object) As Object
        If V Is DBNull.Value Then Return Nothing
        Return V
    End Function

End Class
