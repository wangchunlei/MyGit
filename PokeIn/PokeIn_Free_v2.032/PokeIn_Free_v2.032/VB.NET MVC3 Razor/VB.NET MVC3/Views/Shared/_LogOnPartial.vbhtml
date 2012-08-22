@If Request.IsAuthenticated Then
    @<text>Welcome <b>@Context.User.Identity.Name</b>!
    [ @Html.ActionLink("Log Off", "LogOff", "Account") ]</text>
Else
    @:[ @Html.ActionLink("Log On", "LogOn", "Account") ]
End If
