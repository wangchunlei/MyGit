<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PokeIn_WCF_Sample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PokeIn WCF Demo</title>
    <script type="text/javascript" src="Handler.aspx?ms=connect"></script>
    <style>body{font:normal normal 12px sans-serif,Arial;}</style>
</head>
<body>
<strong>Run ClientWCF Control Windows Forms Application First!</strong>
<hr />
<br />
<span id='status'><strong>WCF Service Status : </strong></span><br />
<p>Server Message:</p>
<hr />
<div id='test' style='min-height:120px;'>
Waiting for a message from the WCF Server
</div>
<hr />
    <p style='color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
        <script>
            var connected = false;

            document.OnPokeInReady = function() {
                //Start PokeIn
                PokeIn.Start(
                function(is_done) {// Connected?
                        if (is_done) {
                            connected = is_done;
                        }
                    }
                );
            }

            function ServerMessage(message) {
                document.getElementById('test').innerHTML = message;
            }

            function UpdateServiceStatus(isWorking) {
                var statusElement = document.getElementById("status");
                if (isWorking) {
                    statusElement.innerHTML = "<strong>WCF Service Status is :<font color='green'>Active</font>";
                }
                else {
                    statusElement.innerHTML = "<strong>WCF Service Status is :<font color='red'>Closed</font>";
                }
            }
 
        </script>
</body>
</html>