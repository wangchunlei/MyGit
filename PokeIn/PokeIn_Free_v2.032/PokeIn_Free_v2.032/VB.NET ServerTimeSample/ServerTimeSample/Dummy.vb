Imports PokeIn
Imports PokeIn.Comet
Imports System.Threading

Public Class Dummy
    Implements IDisposable
    ' Methods
    Shared Sub New()
        Dim ts As ThreadStart = New ThreadStart(AddressOf AppTime)
        Dim th As Thread = New Thread(ts)
        th.Start()
    End Sub

    Shared Function AppTime()
        Do While True
            Dim jsonMethod As String = JSON.Method("UpdateTime", New Object() {DateTime.Now})
            CometWorker.Groups.Send("TimeChannel", jsonMethod)
            Thread.Sleep(&H3E8)
        Loop
        Return vbNull
    End Function

    Public Sub New(ByVal clientId As String)
        Me._clientId = clientId
    End Sub 

    Public Sub GetServerTime()
        Dim jsonMethod As String = JSON.Method("UpdateTime", New Object() {DateTime.Now})
        CometWorker.SendToClient(Me._clientId, jsonMethod)
    End Sub

    Public Sub SubscribeToTimeChannel()
        CometWorker.Groups.PinClientID(Me._clientId, "TimeChannel")
    End Sub

    Public Sub TestString(ByVal str As String)
        str = JSON.Tidy(str)
        CometWorker.SendToClient(Me._clientId, ("UpdateString('" & str & "');"))
    End Sub


    ' Fields
    Private _clientId As String

    Public Sub IDisposable_Dispose() Implements IDisposable.Dispose

    End Sub
End Class
