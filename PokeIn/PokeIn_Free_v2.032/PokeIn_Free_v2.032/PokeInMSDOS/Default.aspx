<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PokeInMSDOS._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn MSDOS</title>
    <script type="text/javascript" src="Handler.aspx?ms=connect"></script>
</head>
<body style='font:normal normal 12px sans-serif,Arial'>
     <div style='overflow:auto;width:630px;height:410px;padding:4px;background-color:Black;'>
        <div id='console' style='width:600px;min-height:400px;background-color:Black;color:White;'></div>
     </div>
     <div style='width:630px;height:45px;padding:4px;background-color:Black;'>
     <input type='text' id='cmd' style='width:500px' />
     <input type=button id='btnSend' value='Send' disabled=disabled />
     <div style='display:block;color:yellow' id='status'>Processing...</div>
     </div>
     <script>
         var btnSend = document.getElementById('btnSend');
         var conWin = document.getElementById('console');
         var cmd = document.getElementById('cmd');
         var status = document.getElementById('status');
         var _active = false; 

         cmd.onkeydown = function(ev) {
             if (!_active) {
                 return;
             }
             var ev = ev || window.event;
             if (ev.keyCode == 13) {
                 btnSend.onclick();
             }
         }

         btnSend.onclick = function() {
             if (!_active) {
                 return;
             }
             status.style.display = "block";
             pCall['Console'].Run(cmd.value);
             if (cmd.value == "cls") {
                 conWin.innerHTML = "";
                 conWin.parentNode.scrollTop = 0;
             }
             cmd.value = "";
         }
         
         function ConsoleUpdated(info) {
             status.style.display = "none";
             conWin.innerHTML += info + "<br/>";
             var h = parseInt(conWin.clientHeight);
             if (h > 400) {
                 conWin.parentNode.scrollTop = h;
             }
         }

         function ConsoleClosed() {
             if (PokeIn.IsConnected) {
                 PokeIn.Close();
                 _active = false;
                 ConsoleUpdated("Console Closed!");
                 btnSend.disabled = "disabled";
             }
         }

         document.OnPokeInReady = function() {
             PokeIn.Start(function(status) {
                 _active = status;
                 if (status) {
                     btnSend.disabled = "";
                 }
             });
             PokeIn.OnClose = ConsoleClosed;
         }
     </script>
     <br />
     <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
</body>
</html>
