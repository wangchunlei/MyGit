@Code
    ViewData("Title") = "Home Page"
End Code

<h2>@ViewBag.Message</h2>
<p>
@Code
    If (Request.IsAuthenticated <> True) Then
End Code 
    <text>You must login to receive server time from PokeIn SampleClass</text>
@Code
Else
End Code        
    <script src="Home/h.PokeIn?ms=connect&rnd=@DateTime.Now.Millisecond.ToString()" type="text/javascript" ></script>
    <span id='conn'>Wait for connection..</span><br /><br />
    <input type="button" id="btn" value="Get Server Time" /><br /><br />
    <span id='tm'>............</span>
    <script>
        var _conn = document.getElementById("conn");
        var _tm = document.getElementById("tm");
        var _btn = document.getElementById("btn");

        document.OnPokeInReady = function () {
            PokeIn.Start(function (isConnected) {
                _conn.innerHTML = "You are connected!";
            });

            PokeIn.OnClose = function () {
                _conn.innerHTML = "Disconnected. Reconnecting..";
                PokeIn.ReConnect();
            };
        };

        _btn.onclick = function () {
            if (!PokeIn.IsConnected) {
                alert("PokeIn is not connected.");
                return;
            };
            pCall["Sample"].GetServerTime();
        };

        function ServerTime(dt){
            _tm.innerHTML = dt.toString();
        };
    </script>
@Code       
End If
End Code
</p>
