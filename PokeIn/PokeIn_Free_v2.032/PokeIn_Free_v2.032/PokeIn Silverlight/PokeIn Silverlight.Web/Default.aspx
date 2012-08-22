<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PokeIn_Silverlight.Web.Default" %>
<%@ Import Namespace="PokeIn_Silverlight.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn Silverlight</title>
    <script src="Handler.aspx?ms=connect" type="text/javascript"></script>
    <script src="Silverlight.js" type="text/javascript"></script>
</head>
<body onload='start()'>

<div id="silverlightControlHost" style='width:400px;height:400px;'>
    <script type="text/javascript">
        function start() {
            var getSilverlightMethodCall =
            "javascript:Silverlight.getSilverlight(\"3.0.40818.0\");"
            var installImageUrl =
            "http://go.microsoft.com/fwlink/?LinkId=161376";
            var imageAltText = "Get Microsoft Silverlight";
            var altHtml =
            "<a href='{1}' style='text-decoration: none;'>" +
            "<img src='{2}' alt='{3}' " +
            "style='border-style: none'/></a>";
            altHtml = altHtml.replace('{1}', getSilverlightMethodCall);
            altHtml = altHtml.replace('{2}', installImageUrl);
            altHtml = altHtml.replace('{3}', imageAltText);

            Silverlight.createObject(
            "ClientBin/PokeIn Silverlight.xap",
            document.getElementById("silverlightControlHost"), "slPlugin",
            {
                width: "100%", height: "100%",
                background: "white", alt: altHtml,
                version: "3.0.40818.0"
            },
            { onError: function (err) { alert(err); }, onLoad: ConnectPokeIn },
            "param1=value1,param2=value2", "row3");
        };
    </script>
</div> 

    <script>
        var slControl = null;
        var TimeReceived = null;

        //Silverlight application is loaded?
        function ConnectPokeIn(sender, args) {
            slControl = sender;

            //PokeIn will be calling the below function when it is ready. 
            document.OnPokeInReady = function () {

                //Let PokeIn connect to server
                PokeIn.Start(function (status) {

                    //connection done?
                    if (status) {
                        slControl.Content.Page.Connected();
                        TimeReceived = function (t) { slControl.Content.Page.TimeReceived(t) };
                    }
                });

            }
        } 
            
    </script>
</body>
</html>

