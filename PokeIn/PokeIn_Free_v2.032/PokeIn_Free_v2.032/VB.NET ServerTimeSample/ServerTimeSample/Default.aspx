<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ChatSampleVB._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PokeIn ServerTime Sample</title>
    <script src="PokeIn.ashx?ms=connect" type="text/javascript"></script>
</head>
<body>
<input type="button" id="Button1" value="Send Test String" />&nbsp;<input type=text id="txt" value="Sample Text" /><br />
<input type="button" id="time_btn" value="Get Server Time (one time)" /><br />
<input type="button" id="auto_btn" value="Join to Server Time Channel" /><br />
<input type="button" id="Button2" value="Disconnect" /><br />
    <div>Received :: <span id='tm'></span></div> 
<br />
<p style='font-size:80%;color:#336699;'>Deploy this sample application onto IIS or APACHE to achive extraordinary performance! </p>
<hr />
    <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
    <script>
        var msg_div = document.getElementById("tm");

        //PokeIn will call the below function when it is ready
        document.OnPokeInReady = function () {
            //Start PokeIn
            PokeIn.Start(
                function (is_done) {// Connected?
                    if (is_done) {
                        msg_div.innerHTML = "Connected!";
                    }
                }
            );

            PokeIn.OnError = function (err) {
                msg_div.innerHTML = "PokeIn Error: " + err;
            };
        }


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
        document.getElementById("auto_btn").onclick = function (e) {
            if (PokeIn.IsConnected) {
                pCall['Dummy'].SubscribeToTimeChannel();
                document.getElementById("auto_btn").value = "Subscribed to the channel";
                document.getElementById("auto_btn").disabled = true;
            }
            else
                alert("This client is not connected!");
        }

        var btn2 = document.getElementById("Button2");
        btn2.onclick = function (e) {
            if (msg_div.innerHTML == "Connecting..")
                return;

            if (PokeIn.IsConnected) {
                PokeIn.Close();
                btn2.value = "Connect";
                msg_div.innerHTML = "Disconnected!";
            }
            else {
                document.getElementById("auto_btn").value = "Join to Server Time Channel";
                document.getElementById("auto_btn").disabled = false;
                PokeIn.ReConnect();
                btn2.value = "Disconnect";
                msg_div.innerHTML = "Connecting..";
            }
        }

    </script>   
    

</body>
</html>
