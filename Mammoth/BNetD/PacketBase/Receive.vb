Namespace PacketBase.Receive

    Friend Class AUTH_INFO
        Inherits ReceivePacket
        Sub New()
            MyBase.New(BNCSIDs.SID_AUTH_INFO)
        End Sub
        Public ProtocolID() As Byte
        Public PlatformID() As Byte
        Public ProductID() As Byte
        Public VersionID() As Byte
        Public ProductLanguage() As Byte
        Public LocalIP() As Byte
        Public TimeZone() As Byte
        Public LocaleID() As Byte
        Public LanguageID() As Byte
        Public CountryLong As String
        Public CountryShort As String
    End Class

    Friend Class PING
        Inherits ReceivePacket
        Sub New()
            MyBase.New(BNCSIDs.SID_PING)
        End Sub
        Public PingValue() As Byte
    End Class

    Friend Class AUTH_CHECK
        Inherits ReceivePacket
        Sub New()
            MyBase.New(BNCSIDs.SID_AUTH_CHECK)
        End Sub
        Public ClientToken() As Byte
        Public ExeVersion() As Byte
        Public ExeHash() As Byte
        Public CDKeys() As Byte
        Public SpawnKey() As Byte
    End Class

    Friend Class CDKeys
        Public KeyLength() As Byte
        Public ProductValue() As Byte
        Public PublicValue() As Byte
        Public Unknown() As Byte
        ''' <summary>
        ''' A DWORD with 
        ''' </summary>
        Public HashedKeyData() As Byte
    End Class

End Namespace



