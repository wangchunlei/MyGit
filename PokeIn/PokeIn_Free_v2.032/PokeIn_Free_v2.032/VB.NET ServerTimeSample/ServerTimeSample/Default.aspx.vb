Imports PokeIn.Comet

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Shared Sub New()
        AddHandler CometWorker.OnClientConnected, New PokeIn.Comet.DefineClassObjects(AddressOf OnClientConnected)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub 

    Public Shared Sub OnClientConnected(ByVal details As ConnectionDetails, ByRef list As Dictionary(Of String, Object))
        list.Add("Dummy", New Dummy(details.ClientId))
    End Sub

End Class