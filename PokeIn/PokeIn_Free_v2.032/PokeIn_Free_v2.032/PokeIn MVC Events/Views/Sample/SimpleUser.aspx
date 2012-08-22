<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>"  %>
<%@ Import Namespace="PokeInMVC_Sample.Core" %>
<%@ Import Namespace="PokeIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn Event Oriented MVC Sample</title> 
    <script type="text/javascript" src="Handler?ms=connect&j=<%=System.Web.HttpContext.Current.Session.SessionID %>" ></script>
</head>
<body style='font:normal normal 12px Arial;'>
    <div style='color:Maroon;float:left;' id='welcome'>Connecting...</div> :: <a href='javascript:logout()'>logout</a><br />
    <p style='clear:both'></p>
    <input value='Request An Apples from Admin' id='btn_req' disabled=disabled type=button /><br />
    <div style='color:#336699;' id='basket'>You have 0 Apple</div>
    <div style='color:#336699;' id='requests'>You have 0 Requests</div>
    <script>
        function logout() {
            PokeIn.Close();
        }
        PokeIn.OnClose = function () {
            self.location = "/Sample/Home";
        }

        PokeIn.Start(function (status) {
            if (status) {
                //Call Server side function to define this view
                pCall['User'].StartListenForEvents();
            };
        });
        var _title = document.getElementById("welcome");
        var _basket = document.getElementById("basket");
        var _reqs = document.getElementById("requests");
        var _btn_req = document.getElementById("btn_req");

        function SetTitle(mess) {
            _btn_req.disabled = "";
            _title.innerHTML = mess;
        }

        function AdminSentApple(count, countLeft) {
            _basket.innerHTML = 'You have ' + count + ' Apple(s)';
            _reqs.innerHTML = 'You have ' + countLeft + ' Request(s)  ';
        }

        _btn_req.onclick = function () {
            //call server side function
            pCall['User'].RequestSomeApple(); // or you may call directly User.RequestSomeApple() here
        } 
        
    </script>
    <br />
    <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
</body>
</html>
