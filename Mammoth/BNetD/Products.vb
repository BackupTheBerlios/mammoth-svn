Friend Enum Products
    Telnet = 0
    Starcraft_Shareware = 1
    Starcraft_Japanese = 2
    Starcraft = 3
    Starcraft_Broodwar = 4
    Diablo_Shareware = 5
    Diablo_I = 6
    Diablo_II = 7
    Diablo_II_LOD = 8
    Warcraft_II = 9
    Warcraft_III_Demo = 10
    Warcraft_III = 11
    Warcraft_III_TFT = 12
End Enum

Friend Class ProductInfo

    Shared Function TagToID(ByVal TAG As String) As Products
        Select Case TAG.ToUpper
            Case "CHAT" : Return Products.Telnet
            Case "SSHR" : Return Products.Starcraft_Shareware
            Case "JSTR" : Return Products.Starcraft_Japanese
            Case "STAR" : Return Products.Starcraft
            Case "SEXP" : Return Products.Starcraft_Broodwar
            Case "DSHR" : Return Products.Diablo_Shareware
            Case "DRTL" : Return Products.Diablo_I
            Case "D2DV" : Return Products.Diablo_II
            Case "D2XP" : Return Products.Diablo_II_LOD
            Case "W2BN" : Return Products.Warcraft_II
            Case "W3DM" : Return Products.Warcraft_III_Demo
            Case "WAR3" : Return Products.Warcraft_III
            Case "W3XP" : Return Products.Warcraft_III_TFT
        End Select
        Return Products.Telnet
    End Function

    Shared Function IDToTag(ByVal ID As Products) As String
        Select Case ID
            Case Products.Telnet : Return "CHAT"
            Case Products.Starcraft_Shareware : Return "SSHR"
            Case Products.Starcraft_Japanese : Return "JSTR"
            Case Products.Starcraft : Return "STAR"
            Case Products.Starcraft_Broodwar : Return "SEXP"
            Case Products.Diablo_Shareware : Return "DSHR"
            Case Products.Diablo_I : Return "DRTL"
            Case Products.Diablo_II : Return "D2DV"
            Case Products.Diablo_II_LOD : Return "D2XP"
            Case Products.Warcraft_II : Return "W2BN"
            Case Products.Warcraft_III_Demo : Return "W3DM"
            Case Products.Warcraft_III : Return "WAR3"
            Case Products.Warcraft_III_TFT : Return "W3XP"
        End Select
        Return "CHAT"
    End Function

End Class