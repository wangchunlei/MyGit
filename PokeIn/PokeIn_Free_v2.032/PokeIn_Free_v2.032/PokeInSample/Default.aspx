<%@ Page Language="C#" AutoEventWireup="true" EnableSessionState="ReadOnly" CodeBehind="Default.aspx.cs" Inherits="PokeInServerWatch._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PokeIn ServerTime Sample</title>

    <!-- Below "dt" parameter is for bypassing browser caching -->
    <script src="PokeIn.ashx?ms=connect&dt=<%=DateTime.Now.Millisecond.ToString()%>" type="text/javascript"></script>
    
</head>
<body>
<input type="button" id="Button1" value="Send Test String" />&nbsp;<input type=text id="txt" value="Sample Text" /><br />
<input type="button" id="time_btn" value="Get Server Time (one time)" /><br />
<input type="button" id="auto_btn" value="Join to Server Time Channel" /><br /> 
    <div>Received :: <span id='tm'></span></div> 
<br />
<p style='color:Maroon;font-weight:bold;' id='PclientID'></p>
<p style='font-size:80%;color:#336699;'>Deploy this sample application onto IIS or APACHE to achive extraordinary performance! </p>
<hr />
    <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
    <script> 
        var msg_div = document.getElementById("tm");

        //PokeIn will call the below function when it is ready
        document.OnPokeInReady = function() {
            //Start PokeIn
            PokeIn.Start(
                function(is_done) {// Connected?
                    if (is_done) {
                        msg_div.innerHTML = "Connected!";
                        document.getElementById('PclientID').innerHTML = "Client Id:" + PokeIn.GetClientId();
                    }
                }
            );

            PokeIn.OnError = function(err) {
                msg_div.innerHTML = "PokeIn Error: " + err;
            };

            PokeIn.OnClose = function() {
                if (!PokeIn.PageUnloading) {
                    msg_div.innerHTML = "ReConnecting..";
                    PokeIn.ReConnect();
                };
            };
        };

        
        //our server side function will call this client function
        function UpdateTime(_date) {
            msg_div.innerHTML = _date.toString();
        }

        function UpdateString(_str) {
            msg_div.innerHTML = _str;
        }

        //Get Time Click
        document.getElementById("time_btn").onclick = function (e) {
            if (PokeIn.IsConnected)
                pCall['Dummy'].GetServerTime();
            else
                alert("This client is not connected!");
        }

        //Test String Click
        document.getElementById("Button1").onclick = function (e) {
            if (PokeIn.IsConnected)
                pCall['Dummy'].TestString(document.getElementById('txt').value);
            else
                alert("This client is not connected!");
        }

        //continous server time
        document.getElementById("auto_btn").onclick = function(e) {
            if (PokeIn.IsConnected) {
                pCall['Dummy'].SubscribeToTimeChannel();
                document.getElementById("auto_btn").value = "Subscribed to the channel";
                document.getElementById("auto_btn").disabled = true;
            }
            else
                alert("This client is not connected!");
        } 

    </script>   
    

</body>
</html>
