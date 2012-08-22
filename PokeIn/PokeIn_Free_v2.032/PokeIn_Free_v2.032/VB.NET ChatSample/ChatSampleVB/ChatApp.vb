Imports PokeIn
Imports PokeIn.Comet

Public Class ChatApp
	Implements IDisposable
	
    Private ClientID As String
    Public Shared Names As Dictionary(Of String, String)
    Private Username As String
    Public Shared Users As Dictionary(Of String, String)

    Public Overloads Sub Dispose() Implements IDisposable.Dispose
       'Do whatevet you want here, PokeIn will call this method when a user is disconnected
	   Try
            SyncLock ChatApp.Users
                ChatApp.Users.Remove(Me.ClientID)
            End SyncLock
            SyncLock ChatApp.Names
                ChatApp.Names.Remove(Me.Username)
            End SyncLock
        Finally
            MyBase.Finalize()
        End Try
    End Sub

    Public Sub New(ByVal clientId As String)
        Me.ClientID = clientId
        Me.Username = ""
    End Sub

    Shared Sub New()
        ChatApp.Users = New Dictionary(Of String, String)
        ChatApp.Names = New Dictionary(Of String, String)
    End Sub

    Protected Overrides Sub Finalize()

    End Sub

    Public Sub Send(ByVal message As String)
        Dim json As String = PokeIn.JSON.Method("ChatMessageFrom", Me.Username, message) ' ChatMessageFrom( "user", "message" );

        CometWorker.SendToAll(json)
    End Sub

    Public Sub SetName(ByVal user_name As String)
        If (Me.Username <> "") Then
            CometWorker.SendToClient(Me.ClientID, "alert('You already have an username!');")
        Else
            Dim duplicate As Boolean = False
            SyncLock ChatApp.Names
                duplicate = ChatApp.Names.ContainsKey(user_name)
            End SyncLock
            If duplicate Then
                CometWorker.SendToClient(Me.ClientID, "alert('Another user is using the name you choose!\nPlease try another one.');")
            Else
                SyncLock ChatApp.Names
                    ChatApp.Names.Add(user_name, Me.ClientID)
                End SyncLock
                SyncLock ChatApp.Users
                    ChatApp.Users.Add(Me.ClientID, user_name)
                End SyncLock
                Me.Username = user_name
                Dim js As String = JSON.Method("UsernameSet", user_name)

                CometWorker.SendToClient(Me.ClientID, js)
            End If
        End If
    End Sub

End Class
