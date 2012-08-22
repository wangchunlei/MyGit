<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web_Controls_under_Pokein._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn & Update Panel Collaboration</title> 
    
    <!-- PokeIn Connection Request -->
    <script type="text/javascript" src="Handler.aspx?ms=connect"></script>
</head>
<body>
    <!-- ASP.NET Form Controls -->
    <form id="form1" runat="server"  >
    <div>
        <asp:ScriptManager ID="ScriptManager2" runat="server">
        </asp:ScriptManager>
    </div>
    <div>
    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
    Text="Get ServerTime by UpdatePanel" />
                <asp:TextBox ID="TextBox1" runat="server" Width="264px"></asp:TextBox>
                <br />
                <asp:HiddenField ID="HiddenField1" runat="server" />
                <br />
                <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
                    Text="Call PokeIn Instance Method by UpdatePanel" />
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form> 

    <!-- Basic HTML Controls -->
    <hr />
    <input type = "button" value ="Get ServerTime by PokeIn" id="btnTime" />
    <input type = "text" value = "" style = "width:264px" id="txtTime" />
    
    <br />
    <hr /> 
        <p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
    
    <!-- Javascript Code -->
       <script>
           //below event fires when PokeIn client side files loaded
           document.OnPokeInReady = function() {
               //Begin!
               PokeIn.Start(function(is_started) {
                   if (is_started) {
                       document.title = "pokein connected";

                       //post PokeIn clientid to the HiddenField1 component
                       document.getElementById("HiddenField1").value = PokeIn.GetClientId();
                       __doPostBack('UpdatePanel1', '');
                   }
               });

               //OnClose event which fires when this client disconnects from server
               PokeIn.OnClose = function() {
                   if (!PokeIn.PageUnloading) {
                       document.title = "pokein disconnected";
                       //reset PokeIn client id
                       document.getElementById("HiddenField1").value = "";
                       __doPostBack('UpdatePanel1', '');

                       //Re - Connect! PokeIn. (document.OnPokeInReady will be fired after succesful re-initializition) 
                       PokeIn.ReConnect();
                   }
               };
           };

            //Call Server Side Test.GetTime() method from the instance which is bind to this specific client
           document.getElementById("btnTime").onclick = function() {
               pCall["Test"].GetTime();
           };

            //Server side method calls the below function
           function UpdateTime(_time) {
               document.getElementById("txtTime").value = _time;
           }; 
   </script>
</body>
</html>
