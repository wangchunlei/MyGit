<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="PokeInMVC_Sample.Core" %>
<%@ Import Namespace="PokeIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>PokeIn Event Oriented MVC Sample</title> 
    <script type="text/javascript" src="Handler?ms=connect&j=<%=System.Web.HttpContext.Current.Session.SessionID %>" ></script>
</head>
<body style='font:normal normal 12px Arial;'>
    
    <div style='color:Maroon;float:left;' id='welcome'>Connecting...</div> :: <a href='javascript:logout()'>logout</a><br />
    <p style='clear:both'></p>
    <div style='color:#669900;' id='Div1'>Below list contains the request from users.</div>

    <div style='color:#336699;width:302px;min-height:400px;border:solid 1px #999;' id='requests'>..</div>

    
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
                pCall['Admin'].StartListenForEvents();
            };
        });
        var _title = document.getElementById("welcome"); 

        function SetTitle(mess) {
            _title.innerHTML = mess;
        }

        var req_list = {};
        function CreateElement(userName, count) {
            req_list[userName] = {};
            req_list[userName].appleCount = count;
            req_list[userName].element = document.createElement("DIV");
            req_list[userName].element.style.cssText = "border-bottom:solid 1px #666;cursor:pointer;width:300px;height:25px;";
            req_list[userName].element.innerHTML = "USER " + userName + " Wants " + count + " Apple. Click to Send";

            req_list[userName].element.userName = userName;
            req_list[userName].element.appleCount = count;
            req_list[userName].element.onmousedown = function () {
                if (req_list[this.userName].appleCount != 0) {
                    //call server side admin function
                    pCall['Admin'].SendApple(this.userName, req_list[this.userName].appleCount);
                    UpdateUserRequest(this.userName, 0);
                } else {
                    alert('User didn\'t request for more apple yet');
                }
            };
            var _requests = document.getElementById("requests");
            _requests.appendChild(req_list[userName].element); 
        }

        function UpdateUserRequest(userName, count) {
            if (req_list[userName] == null) {
                CreateElement(userName, count);  
            };
            req_list[userName].element.innerHTML = "USER " + userName + " Wants " + count + " Apple. Click to Send";
            req_list[userName].appleCount = count;
        }

        function RequestList(list) { 
            for (var o in list) { 
                CreateElement(o, list[o]);
            };
        };  
    </script>
    <br />
    <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
</body>
</html>

