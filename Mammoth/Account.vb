Imports MySql.Data.MySqlClient
Friend Class Account
    Private SQL As MySQLClient
    Private _UID As Integer = 0

    Protected Sub New(ByVal UID As Integer, ByRef SQL As MySQLClient)
        Me.SQL = SQL
        Me._UID = UID
    End Sub
    'ALTER TABLE `friends` ADD COLUMN `0` INT NULL  AFTER `uid` ;

    Shared Function UserExists(ByVal Username As String, ByRef SQL As MySQLClient) As Boolean
        Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `uid` FROM `users` WHERE `username`='" & SQL.ValidMySQL(Username.ToLower) & "'")
        Dim res As Boolean = False
        If Rdr.Read Then res = True
        Rdr.Close()
        Return res
    End Function

    Shared Function Load(ByVal Username As String, ByRef SQL As MySQLClient) As Account
        Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `uid` FROM `users` WHERE `username`='" & SQL.ValidMySQL(Username.ToLower) & "'")
        Dim UID As Integer = 0
        If Rdr.Read Then UID = Rdr(0)
        Rdr.Close()
        Return New Account(UID, SQL)
    End Function

    Shared Function Create(ByVal Username As String, ByRef SQL As MySQLClient) As Account
        Try
            Dim FreeID As Integer = 0
            FreeID = SQL.GetMaxOf("users", "uid")
            SQL.ExecuteQuery("INSERT INTO `users` (`uid`, `usersname`) VALUES (" & FreeID & ", '" & SQL.ValidMySQL(Username) & "')")
            Return New Account(FreeID, SQL)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

#Region "properties"
    ReadOnly Property UID As Integer
        Get
            Return _UID
        End Get
    End Property

    ReadOnly Property Username As String
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `username` FROM `users` WHERE `uid`=" & _UID)
            Dim res As String = ""
            If Rdr.Read Then res = Rdr(0)
            Rdr.Close()
            Return res
        End Get
    End Property

    Property Password As String
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `password` FROM `users` WHERE `uid`=" & _UID)
            Dim res As String = ""
            If Rdr.Read Then res = Rdr(0)
            Rdr.Close()
            Return res
        End Get
        Set(ByVal value As String)
            SQL.ExecuteQuery("UPDATE `users` SET `password`='" & SQL.ValidMySQL(value) & "' WHERE `uid`=" & _UID)
        End Set
    End Property

    Property EMail As String
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `username` FROM `users` WHERE `uid`=" & _UID)
            Dim res As String = ""
            If Rdr.Read Then res = Rdr(0)
            Rdr.Close()
            Return res
        End Get
        Set(ByVal value As String)
            SQL.ExecuteQuery("UPDATE `users` SET `email`='" & SQL.ValidMySQL(value) & "' WHERE `uid`=" & _UID)
        End Set
    End Property

    Property Rank As UserFlags
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `rank` FROM `users` WHERE `uid`=" & _UID)
            Dim res As String = ""
            If Rdr.Read Then res = Rdr(0)
            Rdr.Close()
            Return res
        End Get
        Set(ByVal value As UserFlags)
            SQL.ExecuteQuery("UPDATE `users` SET `rank`='" & value & "' WHERE `uid`=" & _UID)
        End Set
    End Property

    Property LastOnline As DateTime
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `lastonline` FROM `users` WHERE `uid`=" & _UID)
            Dim res As New DateTime(2000, 1, 1)
            If Rdr.Read Then res.AddSeconds(Rdr(0))
            Rdr.Close()
            Return res
        End Get
        Set(ByVal value As DateTime)
            SQL.ExecuteQuery("UPDATE `users` SET `lastonline`='" & DateDiff(DateInterval.Second, Now, New DateTime(2000, 1, 1)) & "' WHERE `uid`=" & _UID)
        End Set
    End Property

    ReadOnly Property FirstOnline As DateTime
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `rank` FROM `users` WHERE `uid`=" & _UID)
            Dim res As New DateTime(2000, 1, 1)
            If Rdr.Read Then res.AddSeconds(Rdr(0))
            Rdr.Close()
            Return res
        End Get
    End Property

    Property LastIP As Net.IPAddress
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `lastip` FROM `users` WHERE `uid`=" & _UID)
            Dim res As Net.IPAddress = Net.IPAddress.Parse("127.0.0.1")
            If Rdr.Read Then res = New Net.IPAddress(CLng(Rdr(0)))
            Rdr.Close()
            Return res
        End Get
        Set(ByVal value As Net.IPAddress)
            SQL.ExecuteQuery("UPDATE `users` SET `lastip`='" & value.Address & "' WHERE `uid`=" & _UID)
        End Set
    End Property

    Property Banned As Boolean
        Get
            Dim Rdr As MySqlDataReader = SQL.ExecuteQuery("SELECT `banned` FROM `users` WHERE `uid`=" & _UID)
            Dim res As Boolean = False
            If Rdr.Read Then res = If(Rdr(0) = 0, False, True)
            Rdr.Close()
            Return res
        End Get
        Set(ByVal value As Boolean)
            SQL.ExecuteQuery("UPDATE `users` SET `banned`='" & If(value, 1, 0) & "' WHERE `uid`=" & _UID)
        End Set
    End Property

#End Region

End Class
