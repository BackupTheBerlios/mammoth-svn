Friend Class Channel
    Private _Name As String = ""
    Private _Users As List(Of User)
    Private _Permanent As Boolean = False

    Sub New(ByVal Name As String, Optional ByVal Permanent As Boolean = False)
        _Name = Name
        _Permanent = Permanent
    End Sub

    Structure User
        Public Account As Account
        Public Send As Client.SendCallback
    End Structure

    Sub AddAccount(ByVal user As User)
        For Each U As User In _Users

        Next
        _Users.Add(user)
    End Sub

    ReadOnly Property Name As String
        Get
            Return _Name
        End Get
    End Property

    ReadOnly Property Users As List(Of User)
        Get
            Return _Users
        End Get
    End Property

    ReadOnly Property Permanent As Boolean
        Get
            Return _Permanent
        End Get
    End Property

End Class
