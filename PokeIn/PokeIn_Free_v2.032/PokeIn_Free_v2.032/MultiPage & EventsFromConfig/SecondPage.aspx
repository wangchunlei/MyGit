<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SecondPage.aspx.cs" Inherits="EventsFromConfig.SamplePage" %>

<!DOCTYPE html>
<html>
<head>
    <title>PokeIn Sample - Second - (EventsFromConfig)</title>
    
    <!-- 
    In order to benefit from CustomEvent definition feature;
    evTarget paramater's value must be consistent to corresponding UniqueName value under web.config settings
    -->
    <script type="text/javascript" charset="UTF8" src="a.PokeIn?ms=connect&evTarget=Second&rnd=<%=DateTime.Now.Second.ToString()%>"></script>
</head>
<body style='background-color:White;color:#333;font:normal normal 12px sans-serif,Arial'>
    <input type="text" id='txtI' /> 
    <input type='button' id='btnSendI' value='Send To All' /> 
    <input type='button' id='btnSendCI' value='Send Only To Second Pagers' />
    
    <br /><br />

    <input type='button' id='btnTime' value='GetServerTimeString' />
    <input type='button' id='btnCleanI' value='Clean Output' />

    <p id='msgP' style='margin:10px;padding:10px;border:dashed 1px #aaa;max-height:400px;overflow:auto;max-width:500px;'>
        
    </p>
    <p style='font-size:90%;font-weight:bold;color:#b00000'><a target="blank" href='FirstPage.aspx'>Open FirstPage</a></p>
    <br />
    <hr />
    <p style='font-size:90%;font-weight:bold'><a target="blank" href='http://pokein.com'>PokeIn</a> WebSocket & Comet Ajax Library</p>
    <script>
        var btn = document.getElementById('btnTime');
        var btnClean = document.getElementById('btnCleanI');
        var btnSendAll = document.getElementById('btnSendI');
        var btnSendOnly = document.getElementById('btnSendCI');
        var txt = document.getElementById('txtI');
        var msg = document.getElementById('msgP');

        function s(m) {
            msg.innerHTML += m + "<br/>";
        };

        function ShowTime(m) {
            msg.innerHTML += m.toString() + "<br/>";
        };

        btnClean.onclick = function () {
            msg.innerHTML = "";
        };

        btn.onclick = function () {
            pCall['SecondSample'].GetServerTime();
        };

        btnSendAll.onclick = function () {
            pCall['SecondSample'].SendToAll(txt.value);
        };

        btnSendOnly.onclick = function () {
            pCall['SecondSample'].SendOnlyToSecond(txt.value);
        };
        
        document.OnPokeInReady = function () {
            PokeIn.Start(function (status) {
                s("Connected - ClientId is " + PokeIn.GetClientId());
                PokeIn.OnError = function (err) {
                    s("Error! " + err);
                };

                PokeIn.OnClose = function (err) {
                    s("Connection Closed");
                    if (!PokeIn.PageUnloading) {
                        PokeIn.ReConnect();
                    };
                };
            });
        };
    </script>
</body>
</html>
