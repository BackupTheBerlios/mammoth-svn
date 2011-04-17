Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Module mdlMain

    Sub main()

        'Dim ff() As Byte = BitConverter.GetBytes(255)

        'Dim pck As Byte() = New Byte() {255, &H50, 13, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
        '                                255, &H50, 13, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
        'Dim pck As New PacketBase.Send.AUTH_INFO() With {.IX86verFilename = "test.mpq", .LogonType = New Byte() {PacketBase.Send.AUTH_INFO.LogonTypes.NLSv2, 0, 0, 0},
        '                                                 .ServerToken = New Byte() {1, 2, 3, 4}, .UDPValue = New Byte() {8, 7, 6, 5}, .ValueString = "A=B B=C 132546",
        '                                                 .MPQFiletime = New Byte() {9, 10, 11, 12, 13, 14, 15, 16}}
        'Dim dat() As Byte = pck.ToByteArray
        'Dim a As List(Of BNCSPacket) = BNCSPacket.ReadPacket(pck, New Boolean)


        Server.Execute()
    End Sub

End Module

