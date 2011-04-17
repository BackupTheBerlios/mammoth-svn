Imports MySql.Data.MySqlClient

Partial Class MySQLClient

#Region ".ctor"
    Private ReadOnly Pass As String = "toigla1"
    Private ReadOnly User As String = "root"
#End Region

    Private con As MySqlConnection
#If CONFIG = "Debug" Then
    Private lstQueries As New List(Of String)
#End If

    Sub New()
        con = New MySqlConnection("Server=localhost;Database=mamoth;Uid=" & User & ";Pwd=" & Pass & ";")
        Try
            con.Open()
        Catch ex As Exception
            Console.WriteLine("SQLERR: Cannot open SQL connection!")
            Console.ReadLine()
            End
        End Try
    End Sub

    ReadOnly Property Open() As Boolean
        Get
            If con Is Nothing Then Return False
            If con.State = ConnectionState.Closed Then Return False
            If con.State = ConnectionState.Open Then Return True
        End Get
    End Property

    Function ValidMySQL(ByRef input As String) As String
        input = Replace(Replace(input, "\", "\\"), "'", "\'")
        Return input
    End Function

    Function ExecuteQuery(ByVal Query As String) As MySqlDataReader
#If CONFIG = "Debug" Then
        SyncLock lstQueries
            lstQueries.Add(Query)
        End SyncLock
#End If
        If con.State = ConnectionState.Closed Then Return Nothing
        Dim Cmd As New MySqlCommand(Query, con)
        If Query.ToLower.StartsWith("select") Then
            Dim Rdr As MySqlDataReader = Cmd.ExecuteReader
            Return Rdr
        Else
            Cmd.ExecuteReader().Close()
            Return Nothing
        End If
    End Function

    Function GetMaxOf(ByVal Table As String, ByVal Column As String) As Integer
        Dim Rdr As MySqlDataReader = Nothing
        Dim Res As Integer = 0
        Try
            Rdr = ExecuteQuery("SELECT MAX(`" & Column & "`) FROM `" & Table & "`")
            Rdr.Read()
            Res = Rdr.GetInt32(0) + 1
            Rdr.Close()
            Return Res
        Catch ex As Data.SqlTypes.SqlNullValueException
            Try
                Rdr.Close()
            Catch : End Try
            Return Res
        End Try
    End Function

End Class
