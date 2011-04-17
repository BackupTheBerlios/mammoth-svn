Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Friend Class Client
    Inherits TcpClient

    Private ConnType As ProtocolIDs = ProtocolIDs.Games
    Private HadProtocol As Boolean = False
    Private t As Thread

    Delegate Sub SendCallback(ByVal Data() As Byte)

    Sub New(ByVal Client As Socket)
        Me.Client = Client
        t = New Thread(AddressOf mon)
        t.Start()
    End Sub

    Sub Send(ByVal Data() As Byte)
        Client.Send(Data)
    End Sub

    ReadOnly Property SendMethod As SendCallback
        Get
            Return AddressOf Send
        End Get
    End Property

    Private Sub mon()
        Dim LastPollLoop As Integer = 0

        While Connected

            If Available > 0 Then
                Dim buff(Available - 1) As Byte
                Client.Receive(buff)
                If Not HadProtocol Then
                    Select Case buff(0)
                        Case ProtocolIDs.CHAT : ConnType = ProtocolIDs.CHAT
                        Case ProtocolIDs.FTP : ConnType = ProtocolIDs.FTP
                        Case ProtocolIDs.Games : ConnType = ProtocolIDs.Games
                        Case ProtocolIDs.Telnet : ConnType = ProtocolIDs.Telnet
                        Case Else : Client.Disconnect(False)
                    End Select
                    HadProtocol = True
                    If buff.Length > 1 Then
                        BNCSPacket.RemoveFromArray(buff, 1)
                    Else
                        buff = Nothing
                    End If
                End If
                If Not buff Is Nothing Then
                    Dim aa As New List(Of PacketBase.ReceivePacket)
                    Dim Stopped As Boolean = False
                    Select Case ConnType
                        Case ProtocolIDs.Games : aa = ReadPacket(buff, Stopped)
                    End Select
                    For Each Packet As PacketBase.ReceivePacket In aa
                        Dim INFO = CType(Packet, PacketBase.Receive.AUTH_INFO)
                        Dim LocaleID As Integer = BitConverter.ToInt32(INFO.LocaleID, 0)
                    Next
                    'Handle packet
                End If
            End If

            LastPollLoop = +1

            If LastPollLoop >= 500 And Client.Connected Then
                Try
                    If Client.Poll(-1, Net.Sockets.SelectMode.SelectRead) And Available <= 0 Then
                        Client.Disconnect(False)
                    End If
                Catch ex As Exception

                End Try

                LastPollLoop = 0
            End If
            GC.Collect()
            Thread.Sleep(25)
        End While
    End Sub

    ReadOnly Property IPAddress As IPAddress
        Get
            Return CType(Client.RemoteEndPoint, IPEndPoint).Address
        End Get
    End Property

End Class