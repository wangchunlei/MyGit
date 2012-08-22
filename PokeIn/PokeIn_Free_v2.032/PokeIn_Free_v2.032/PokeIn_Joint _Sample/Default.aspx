<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PokeIn_Joint_Sample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn Joint Sample</title>
    <!--                                                       !!!!!!!!!!!!!               -->
    <!--                                                       j=TestJoint << we gave a name to this connection               -->
    <!--                                                       !!!!!!!!!!!!! << it can be a dynamic parameter, for example the username                -->
    <script type="text/javascript" src="Handler.aspx?ms=connect&j=TestJoint"></script>
</head>
<body style="font:normal normal 12px sans,Arial;">
    <div id="ConnectionInfo">Waiting for connection..</div>
    <input type="button" id="connection" value="Connecting.." disabled="disabled" />
    <hr />
    <input type="button" id="Button1" value="Increase Shared Number" disabled=disabled />
    <span id="sharedSPAN">Shared Number is 0</span>
    <hr />
    <input type="button" id="Button2" value="Increase Individual Number" disabled=disabled />
    <span id="individualSPAN">Individual Number is 0</span>
    <hr />
    <p style='font-size:90%;color:#336699;'>Test this application under IIS or APACHE to get instant messages from server.<br />PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
    <script>
        var connectionInfo = document.getElementById("ConnectionInfo");
        var btnConnection = document.getElementById("connection");
        var btn1 = document.getElementById("Button1"); 
        var btn2 = document.getElementById("Button2");

        //the below event will be fired when PokeIn client side codes completely downloaded
        document.OnPokeInReady = function() {
            PokeIn.Start(
                function(is_connected) {
                    if (is_connected) {
                        connectionInfo.innerHTML = "Connected! -- JointId : " + PokeIn.GetJointId()
                                                  + " -- ClientId : " + PokeIn.GetClientId() + "<br/>";

                        btnConnection.value = "Disconnect";
                        btnConnection.disabled = "";
                        btn1.disabled = "";
                        btn2.disabled = "";
                    };
                }
            );

            PokeIn.OnClose = function() {
                connectionInfo.innerHTML = "Disconnected";
                btnConnection.value = "Connect";
                btnConnection.disabled = "";
            };
        };

        btnConnection.onclick = function() {
            if (PokeIn.IsConnected) {
                btnConnection.value = "Disconnecting";
                btnConnection.disabled = "disabled";
                btn1.disabled = "disabled";
                btn2.disabled = "disabled";
                //close connection
                PokeIn.Close();
            }
            else {
                btnConnection.value = "Connecting";
                btnConnection.disabled = "";
                btn1.disabled = "";
                btn2.disabled = "";
                
                //reconnect
                PokeIn.ReConnect();
            };
        };

        document.getElementById("Button1").onclick = function() {
            if (!PokeIn.IsConnected) {
                alert("Please connect to the server");
                return;
            };
            //call server side method of shared class
            pCall["MySharedClass"].Test();
        };

        document.getElementById("Button2").onclick = function() {
            if (!PokeIn.IsConnected) {
                alert("Please connect to the server");
                return;
            };
            //call server side method of individual class
            pCall["MyIndividualClass"].Test();
        };

        function ActiveUsers(count) {
            document.getElementById("ActiveSPAN").innerHTML = "Total " + count + " user connected to this shared instance";
        };

        function SharedNumber(number) {
            document.getElementById("sharedSPAN").innerHTML = "Shared Number is " + number;
        };

        function NonSharedNumber(clientId, number) {
            document.getElementById("individualSPAN").innerHTML = clientId + " updated his individual number to " + number;
        };
    </script>
</body>
</html>
