<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NotifyClients._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>PokeIn Notifier Sample</title> 
    
    <!-- PokeIn Connection Request -->
    <script type="text/javascript" src="PokeIn.ashx?ms=connect"></script>
</head>
<body>
    <div style='font:normal normal 12px sans-serif,Arial;color:Black;' id='zone'></div>
    <script>
        //below event fires when PokeIn client side files loaded
        document.OnPokeInReady = function() {
            //Begin!
            PokeIn.Start(function(is_started) {
                if (is_started) {
                    document.title = "PokeIn is Connected";
                }
            });

            //OnClose event which fires when this client disconnects from server
            PokeIn.OnClose = function() {
                if (!PokeIn.PageUnloading) {
                    document.title = "Reconnecting.."; 
                    PokeIn.ReConnect();
                }
            };
        };

        var elm = document.getElementById('zone');
        function UpdateTime(tm) {
            elm.innerHTML = tm.toLocaleString();
        };
    </script>
</body>
</html>
