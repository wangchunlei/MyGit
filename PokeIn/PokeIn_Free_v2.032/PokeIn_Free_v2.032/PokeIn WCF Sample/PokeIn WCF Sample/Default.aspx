<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PokeIn_WCF_Sample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PokeIn WCF Demo</title>
    <script type="text/javascript" src="Handler.aspx?ms=connect"></script>
    <style>body{font:14px normal normal sans-serif;}</style>
</head>
<body>
<strong>Before Click to the button below, make sure your WCF Server Console Project is running.</strong><br /><br />
<input type=button id='Btn' value='Get WCF Server Time' /><br /><br /><br />
<div id='test'>
</div>
<br />
    <p style='color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
        <script>
            var connected = false;

            //Start PokeIn
            PokeIn.Start(
            function(is_done) {// Connected?
                    if (is_done) {
                        connected = is_done;
                    }
                }
            );

            document.getElementById('Btn').onclick = function() {
                if (PokeIn.IsConnected) {
                    WCFSample.GetWCFServerTime();
                }
                else {
                    alert('PokeIn is not Connected!');
                }
            }

            function WCFInfo(message) {
                document.getElementById('test').innerHTML = message;
            }
 
        </script>
</body>
</html>