@Code
    If (Request.IsAuthenticated) Then
        PokeIn.Comet.CometWorker.Handle()
    Else
    End Code 
        <text>Please login to your account</text>    
@Code
    End If
    Layout = Nothing
End Code 
