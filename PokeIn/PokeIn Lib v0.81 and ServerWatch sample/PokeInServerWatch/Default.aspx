<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PokeInServerWatch._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%
        string ClientId = "";
        if (!PokeIn.Comet.CometWorker.Bind("Listen.aspx", "Send.aspx", this, new PokeIn.Comet.DefineClassObjects(PokeInServerWatch._Default.classDefiner), out ClientId))
        {
            Response.Write("</head><body>Application Stopped!</body></html>");
            return;
        }
 
    %>
</head>
<body>
    <div>Server Time Is :: <span id='tm'></span></div>
    <script>
        PokeIn.Start();

        var _start = setInterval(function () {
            if (PokeIn.Started) {
                Dummy.RunTimer();
                clearInterval(_start);
            }
        }, 10);

        function UpdateTime(_time) {
            document.getElementById("tm").innerHTML = _time;
        }
    </script>
</body>
</html>
