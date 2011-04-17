Namespace PacketBase

    ''' <summary>
    ''' A base class for packets.
    ''' </summary>
    Friend MustInherit Class ReceivePacket
        Private _BNCSID As BNCSIDs

        Sub New(ByVal BNCSID As BNCSIDs)
            _BNCSID = BNCSID
        End Sub

        ReadOnly Property BNCSID As BNCSIDs
            Get
                Return _BNCSID
            End Get
        End Property

    End Class

    Friend MustInherit Class SendPacket
        Inherits ReceivePacket
        Private _Data() As Byte

        Sub New(ByVal BNCSID As BNCSIDs)
            MyBase.New(BNCSID)
            _Data = Data
        End Sub

        ReadOnly Property Data As Byte()
            Get
                Return _Data
            End Get
        End Property

        Structure ToByteArrayHelper
            Public Data As Object
            Public DataType As DataTypes
        End Structure

        Enum DataTypes
            [STRING]
            WIDESTRING
            QWORD
            DWORD
            WORD
            BOOL
            VOID
        End Enum

        MustOverride Function ToByteArray() As Byte()

        Protected Function _ToByteArray(ByVal ParamArray Data() As ToByteArrayHelper) As Byte()
            Dim pck() As Byte = New Byte() {255, BNCSID, 4, 0}
            For Each D As ToByteArrayHelper In Data
                Select Case D.DataType
                    Case DataTypes.BOOL : AddToArray(Of Byte)(pck, BitConverter.GetBytes(CType(D.Data, Boolean)))
                    Case DataTypes.DWORD, DataTypes.QWORD, DataTypes.VOID : AddToArray(Of Byte)(pck, D.Data)
                    Case DataTypes.STRING : AddToArray(Of Byte)(pck, Text.Encoding.ASCII.GetBytes(D.Data)) : AddToArray(Of Byte)(pck, 0)
                    Case DataTypes.WIDESTRING : AddToArray(Of Byte)(pck, Text.Encoding.UTF8.GetBytes(D.Data)) : AddToArray(Of Byte)(pck, 0)
                End Select
            Next
            Dim len() As Byte = BitConverter.GetBytes(pck.Length) : pck(2) = len(0) : pck(3) = len(1)
            Return pck
        End Function

        Sub AddToArray(Of T)(ByRef Arr() As T, ByVal Items() As T)
            For Each Item As T In Items
                AddToArray(Arr, Item)
            Next
        End Sub

        Sub AddToArray(Of T)(ByRef Arr() As T, ByRef Item As T)
            If Arr Is Nothing Then
                ReDim Arr(0)
            Else
                ReDim Preserve Arr(Arr.Length)
            End If

            Arr(Arr.Length - 1) = Item
        End Sub

    End Class


    ''' <summary>
    ''' A module providing miscellaneous functions and definitions.
    ''' </summary>
    Friend Module Utils
        ''' <summary>
        ''' A WORD is a byte array with a maximum length of 2 bytes
        ''' </summary>
        Public ReadOnly WORD_Length As Integer = 2
        ''' <summary>
        ''' A DWORD is a byte array with a maximum length of 4 bytes
        ''' </summary>
        Public ReadOnly DWORD_Length As Integer = 4
        ''' <summary>
        ''' A QWORD is a byte array with a maximum length of 8 bytes
        ''' </summary>
        Public ReadOnly QWORD_Length As Integer = 8
        ''' <summary>
        ''' Filetime is a byte array with a maximum length of 8 bytes
        ''' </summary>
        Public ReadOnly FileTime_Length As Integer = 8

        ''' <summary>
        ''' Gets a random DWORD value.
        ''' </summary>
        ''' <returns>A random DWORD value.</returns>
        Public Function CreateRandomDWORD() As Byte()
            Dim rnd As New Random
            ReDim CreateRandomDWORD(4)
            For i As Integer = 0 To 3
                CreateRandomDWORD(i) = rnd.Next(254, 0)
            Next
        End Function

        ''' <summary>
        ''' Gets a random QWORD value.
        ''' </summary>
        ''' <returns>A random DWORD value.</returns>
        Public Function CreateRandomQWORD() As Byte()
            Dim rnd As New Random
            ReDim CreateRandomQWORD(4)
            For i As Integer = 0 To 3
                CreateRandomQWORD(i) = rnd.Next(254, 0)
            Next
        End Function

    End Module

End Namespace