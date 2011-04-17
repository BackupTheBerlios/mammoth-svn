Namespace PacketBase.Send

    Friend Class PING
        Inherits SendPacket

        Sub New()
            MyBase.New(BNCSIDs.SID_PING)
        End Sub
        Public PingValue() As Byte

        Public Overrides Function ToByteArray() As Byte()
            Return _ToByteArray(New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = PingValue})
        End Function
    End Class

    Friend Class AUTH_INFO
        Inherits SendPacket
        Private IsWar As Boolean
        Sub New(Optional ByVal IsWar As Boolean = False)
            MyBase.New(BNCSIDs.SID_AUTH_INFO)
            Me.IsWar = IsWar
        End Sub
        Public LogonType() As Byte = New Byte() {0, 0, 0, 0}
        Public Enum LogonTypes As Byte
            BrokenSHA1 = 0
            NLSv1 = 1
            NLSv2 = 2
        End Enum
        Public ServerToken() As Byte
        Public UDPValue() As Byte
        Public MPQFiletime() As Byte
        Public IX86verFilename As String
        Public ValueString As String
        ''' <summary>
        ''' WAR3/W3XP ONLY! 128-byte array.
        ''' </summary>
        Public ServerSignature() As Byte
        Public Overloads Overrides Function ToByteArray() As Byte()
            Return _ToByteArray(New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = LogonType},
                                New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = ServerToken},
                                New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = UDPValue},
                                New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = MPQFiletime},
                                New ToByteArrayHelper With {.DataType = DataTypes.STRING, .Data = IX86verFilename},
                                New ToByteArrayHelper With {.DataType = DataTypes.STRING, .Data = ValueString})
        End Function
    End Class

    Friend Class CHATEVENT
        Inherits SendPacket
        Sub New()
            MyBase.New(BNCSIDs.SID_CHATEVENT)
        End Sub
        Public EventID As EventIDs
        Public UserFlags As UserFlags
        Public IPAddresss() As Byte
        Public AccountNumber() As Byte
        Public RegistrationAuthority() As Byte
        Public Username As String
        Public Text As String
        Public Overrides Function ToByteArray() As Byte()
            Return _ToByteArray(New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = New Byte() {EventID, 0, 0, 0}},
                                New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = New Byte() {UserFlags, 0, 0, 0}},
                                New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = IPAddresss},
                                New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = AccountNumber},
                                New ToByteArrayHelper With {.DataType = DataTypes.DWORD, .Data = RegistrationAuthority},
                                New ToByteArrayHelper With {.DataType = DataTypes.STRING, .Data = Username},
                                New ToByteArrayHelper With {.DataType = DataTypes.STRING, .Data = Text})
        End Function
    End Class

End Namespace