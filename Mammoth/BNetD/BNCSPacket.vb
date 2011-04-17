Imports Intcon.Mammoth.PacketBase
Imports Intcon.Mammoth.PacketBase.Receive
Friend Module BNCSPacket

    Function ReadPacket(ByVal Data() As Byte, ByRef Stopped As Boolean) As List(Of ReceivePacket)
        Try
            If Not Data(0) = 255 Then Return Nothing
            Dim pck As New List(Of ReceivePacket)
            Dim multiplePck As Boolean = True
            While multiplePck
                If Not Data(0) = 255 Then multiplePck = False : Stopped = True : Exit While 'BNCS packets do ALWAYS start with BYTE 255, if it does not; Disconnect with an IP Ban.
                Dim len As Integer = BitConverter.ToInt32(New Byte() {Data(2), Data(3), 0, 0}, 0) - 1
                Dim tPack(len) As Byte
                Array.Copy(Data, tPack, tPack.Length)
                pck.Add(FormatPacket(tPack))
                Erase tPack 'Erase removes all data that is in the array, to free some memory.
                If len < Data.Length Then
                    Array.Reverse(Data)
                    ReDim Preserve Data(Data.Length - len - 2)
                    Array.Reverse(Data)
                    If Data.Length = 0 Then multiplePck = False
                Else
                    multiplePck = False
                End If
            End While
            Stopped = False
            Return pck
        Catch ex As Exception
            Stopped = True
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Returns a structure of the packet. Note that you need to convert it in the packets own method. (ex.: CType(FormatPacket(Data), AUTH_INFO))
    ''' </summary>
    ''' <param name="Data">The byte array to parse.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FormatPacket(ByVal Data() As Byte) As ReceivePacket
        Dim BNCSID As BNCSIDs = ReadDWORD(Data)(1)
        Select Case BNCSID
            Case BNCSIDs.SID_AUTH_INFO
                If Not Data.Length >= 38 Then Return Nothing
                Dim AUTH_INFO As New AUTH_INFO()
                AUTH_INFO.ProtocolID = ReadDWORD(Data)
                AUTH_INFO.PlatformID = ReadDWORD(Data)
                AUTH_INFO.ProductID = ReadDWORD(Data)
                AUTH_INFO.VersionID = ReadDWORD(Data)
                AUTH_INFO.ProductLanguage = ReadDWORD(Data)
                AUTH_INFO.LocalIP = ReadDWORD(Data)
                AUTH_INFO.TimeZone = ReadDWORD(Data)
                AUTH_INFO.LocaleID = ReadDWORD(Data)
                AUTH_INFO.LanguageID = ReadDWORD(Data)
                AUTH_INFO.CountryLong = ReadNTString(Data)
                AUTH_INFO.CountryShort = ReadNTString(Data)
                Return AUTH_INFO
            Case BNCSIDs.SID_PING
                If Not Data.Length = 4 Then Return Nothing
                Dim PING As New PING()
                PING.PingValue = ReadDWORD(Data)
        End Select
    End Function

    ''' <summary>
    ''' Removes an X amount of bounds from the given array.
    ''' </summary>
    ''' <typeparam name="T">Type of the array.</typeparam>
    ''' <param name="Arr">The array to modify.</param>
    ''' <param name="Length">The length of bounds to remove.</param>
    ''' <remarks>It reverses the array first so, it's an upside-down removal.</remarks>
    Function RemoveFromArray(Of T)(ByRef Arr() As T, ByVal Length As Integer) As T()
        Array.Reverse(Arr)
        ReDim Preserve Arr(Arr.Length - Length - 1)
        Array.Reverse(Arr)
        Return Arr
    End Function

#Region "Byte() Reading"
    ''' <summary>
    ''' Returns a single byte.
    ''' </summary>
    ''' <param name="Data">Byte array.</param>
    Function ReadByte(ByRef Data() As Byte) As Byte
        If Data Is Nothing Then Return 0
        If Data.Length = 0 Then Return 0
        Dim tB As Byte = Data(0)
        RemoveFromArray(Of Byte)(Data, 1)
        Return tB
    End Function
    ''' <summary>
    ''' Returns an array of bytes.
    ''' </summary>
    ''' <param name="Data">Byte array.</param>
    ''' <param name="Length">The length of bytes to return.</param>
    Function ReadBytes(ByRef Data() As Byte, ByVal Length As Integer) As Byte()
        If Data Is Nothing Then Return New Byte() {0}
        If Data.Length = 0 Then Return New Byte() {0}
        Dim tB(Length) As Byte
        For i As Integer = 0 To Length - 1
            tB(i) = Data(i)
        Next
        RemoveFromArray(Of Byte)(Data, Length)
        Return tB
    End Function
    ''' <summary>
    ''' Returns a DWORD (BYTE[4])
    ''' </summary>
    ''' <param name="Data">Byte array.</param>
    Function ReadDWORD(ByRef Data() As Byte) As Byte()
        If Data Is Nothing Then Return New Byte() {0, 0, 0, 0}
        If Data.Length = 0 Then Return New Byte() {0, 0, 0, 0}
        If Data.Length < 4 Then Return New Byte() {0, 0, 0, 0}
        Dim tB() As Byte = New Byte() {Data(0), Data(1), Data(2), Data(3)}
        RemoveFromArray(Of Byte)(Data, 4)
        Return tB
    End Function
    ''' <summary>
    ''' Returns a QWORD (BYTE[8])
    ''' </summary>
    ''' <param name="Data">Byte array.</param>
    Function ReadQWORD(ByRef Data() As Byte) As Byte()
        If Data Is Nothing Then Return New Byte() {0, 0, 0, 0, 0, 0, 0, 0}
        If Data.Length = 0 Then Return New Byte() {0, 0, 0, 0, 0, 0, 0, 0}
        If Data.Length < 8 Then Return New Byte() {0, 0, 0, 0, 0, 0, 0, 0}
        Dim tB() As Byte = New Byte() {Data(0), Data(1), Data(2), Data(3), Data(4), Data(5), Data(6), Data(7)}
        RemoveFromArray(Of Byte)(Data, 8)
        Return tB
    End Function
    ''' <summary>
    ''' Returns an ASCII null terminated string.
    ''' </summary>
    ''' <param name="Data">Byte array.</param>
    Function ReadNTString(ByRef Data() As Byte) As String
        If Data Is Nothing Then Return ""
        If Data.Length = 0 Then Return ""
        Dim tS As String = ""
        Dim tCount As Integer = 1 'Keeps how many bytes should be removed.
        '                           ALWAYS 1 - The BYTE 0 must be remove as well.
        For Each B As Byte In Data
            If Not B = 0 Then tS &= Chr(B) : tCount += 1 Else Exit For
        Next
        RemoveFromArray(Of Byte)(Data, tCount)
        Return tS
    End Function

    ''' <summary>
    ''' Returns an UTF-8 null terminated string.
    ''' </summary>
    ''' <param name="Data">Byte array.</param>
    Function ReadNTWideString(ByRef Data() As Byte) As String
        If Data Is Nothing Then Return ""
        If Data.Length = 0 Then Return ""
        Dim tB() As Byte = Nothing
        Dim tCount As Integer = 1 'Keeps how many bytes should be removed.
        '                           ALWAYS 1 - The BYTE 0 must be remove as well.
        For Each B As Byte In Data
            If Not B = 0 Then ReDim Preserve tB(tB.Length) Else Exit For
        Next
        RemoveFromArray(Of Byte)(Data, tCount)
        Return Text.Encoding.UTF8.GetString(tB)
    End Function

#End Region
End Module
