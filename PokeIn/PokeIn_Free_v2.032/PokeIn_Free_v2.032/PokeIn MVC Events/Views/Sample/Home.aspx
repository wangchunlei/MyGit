<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%@ Import Namespace="PokeInMVC_Sample.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> 
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn Event Oriented MVC Sample Login</title>
</head>
<body style='font:normal normal 12px sans,Arial' >  
    <p>Login as a User and request some apple from admin. And then (or in same time from the another browser or pc) login as Admin user and approve the requests.</p>
    <form id="form1" method="post">
    <input type=hidden id='Username' name='Username' value='' />
    <input type=hidden id='Password' name='Password' value='1234' />
    <input type=button id='btnOguz' value='Login As User Oguz' /><br />
    <input type=button id='btnMadeline' value='Login As User Madeline' /><br />
    <input type=button id='btnFrank' value='Login As User Frank' /><br />
    <input type=button id='btnEran' value='Login As User Eran' /><br /> 
    <input type=button id='btnAdmin' value='Login As Admin' /><br />
    </form>  
    <script>
        var btnFrank = document.getElementById('btnFrank');
        var btnMad = document.getElementById('btnMadeline');
        var btnOguz = document.getElementById('btnOguz');
        var btnEran = document.getElementById('btnEran');
        var btnAdmin = document.getElementById('btnAdmin');
        var hUser = document.getElementById('Username');
        var hPass = document.getElementById('Password');
        var hForm = document.getElementById('form1');

        btnFrank.onclick = function () {
            hUser.value = 'Frank'; 
            hForm.submit();
        }
        btnEran.onclick = function () {
            hUser.value = 'Eran';
            hForm.submit();
        }
        btnMad.onclick = function () {
            hUser.value = 'Madeline';
            hForm.submit();
        }
        btnOguz.onclick = function () {
            hUser.value = 'Oguz';
            hForm.submit();
        }
        btnAdmin.onclick = function () {
            hUser.value = 'Admin';
            hForm.submit();
        }
    </script>
    <br />
    <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
</body>
</html>
