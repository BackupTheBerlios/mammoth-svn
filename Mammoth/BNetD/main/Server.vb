Imports System.Net
Imports System.Net.Sockets
Imports System.Threading

Friend Class Server
    Inherits TcpListener
    Private tListener As Thread
    Private tCCheck As Thread
    Private r As Boolean = True

    Private Cs As List(Of Client)
    Private Channels As List(Of Channel)

    Shared Sub Execute()
        Dim tmp As New Server
    End Sub

    Private Sub New()
        MyBase.New(IPAddress.Parse("127.0.0.1"), 6112)
        Me.Start()
        Cs = New List(Of Client)
        tListener = New Thread(AddressOf mon)
        tListener.Start()
        tCCheck = New Thread(AddressOf CCHeck)
        tCCheck.Start()
    End Sub

    Private Sub mon()
        While Active
            If Pending() Then
                If Not Cs.Count = Settings.MaxCon Then Cs.Add(New Client(AcceptSocket))
                Thread.Sleep(50)
            End If
        End While
    End Sub

    Private Sub CCHeck()
        While (Cs.Count >= 0 Or Active)
            For Each C As Client In Cs
                If Not C.Connected Then Cs.Remove(C)
            Next
            Thread.Sleep(5000)
        End While
    End Sub
End Class

