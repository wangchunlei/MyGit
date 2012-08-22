<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ChatWindow.aspx.vb" Inherits="ChatSampleVB._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Simple Chat</title>
    <script type="text/javascript" src="Handler.aspx?ms=connect"></script>
    <style>
        body{ margin:0px; padding:0px; font:normal normal 10pt 'sans','Arial';}
        .white{ color:White; }
    </style>
</head>
<body>
<div style="position:absolute;left:5px;top:5px;width:411px;height:440px;background-color:orange;z-index:-1;border:solid 1px #ff6699;"></div>
<div id="ChatBase" style="position:absolute;left:10px;top:10px;width:400px;height:400px;overflow-y:scroll;overflow-x:hidden;border:solid 1px #006699;background-color:#336699;">
    <div id="Chats" class="white" style="margin:0px;padding:5px;width:380px;">
    PokeIn Chat Application<br />
    </div>
</div>
<div id="lbl" style="position:absolute;left:10px;top:417px;">Username ::</div>
<input id="info" style="position:absolute;left:90px;top:415px;width:250px;height:20px;border:solid 1px #006699;" type="text" />
<input type="button" id="btnChat" value="Start" style="position:absolute;left:350px;top:415px;cursor:pointer;" />

<p style='position:absolute;top:450px;font-size:80%;color:#336699;'>PokeIn Ajax Comet Library. <a href='http://pokein.codeplex.com' target=_blank>http://pokein.codeplex.com</a> </p>
<script>

    //Wait for the download of PokeIn Library
    document.OnPokeInReady = function () {
        //Start PokeIn
        PokeIn.Start(function (status) {
            //if connected
            if (status) {
                btnChat.disabled = "";
            }
        });

        //PokeIn connection closed
        PokeIn.OnClose = function () {
            ChatMessageFrom({ Username: "SERVER", Message: "Your Connection Closed!" });
        };
    }

    var btnChat = document.getElementById("btnChat");
    var lbl = document.getElementById("lbl");
    var info = document.getElementById("info");
    var chatWind = document.getElementById("Chats");
    var chatBase = document.getElementById("ChatBase");

    var user_name = "";
    var user_names = [];

    btnChat.onclick = function() {
        if (user_name == "") {
            user_name = info.value;
            if (user_name.replace(/ /g, "").length == 0) {
                alert('You should enter the user name');
                return;
            }
            Chat.SetName(user_name);
            user_name = "";
        }
        else {
            var mess = info.value;
            if (mess.replace(/ /g, "").length == 0) {
                alert('You should enter some message to send');
                return;
            }
            Chat.Send(mess);
            info.value = "";
        }
    }

    info.onkeydown = function(ev) {
        var ev = ev || window.event;
        if (ev.keyCode == 13) {
            btnChat.onclick();
        }
    }

    function ChatMessageFrom(user, message) {
        chatWind.innerHTML += "<strong>" + user + "</strong>:: " + message + "<br/>";
        chatBase.scrollTop = chatWind.clientHeight;
    }

    function UserNameList_Updated(names) {
        user_names = names;
    }

    function UsernameSet(user) {
        user_name = user;
        btnChat.value = "Send";
        info.value = "";
        lbl.innerHTML = "Message ::";
        ChatMessageFrom("SERVER", "Hi "+user+"!<br/>Welcome to Our Server!");
    } 
</script>     
</body>
</html>

