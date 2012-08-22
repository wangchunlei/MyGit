Imports PokeIn
Imports PokeIn.Comet

Public Class SampleClass 
    Implements IDisposable

    Private ClientID As String

    Public Sub New(ByVal clientId As String)
        Me.ClientID = clientId
    End Sub

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        'Do whatevet you want here, PokeIn will call this method when a user is disconnected
    End Sub

    Public Sub GetServerTime()
        Dim msg As String
        msg = JSON.Method("ServerTime", DateTime.Now)
        CometWorker.SendToClient(ClientID, msg)
    End Sub
End Class
