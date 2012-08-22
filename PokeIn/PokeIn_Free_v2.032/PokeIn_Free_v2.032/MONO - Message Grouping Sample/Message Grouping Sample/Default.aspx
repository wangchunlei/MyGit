<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Message_Grouping_Sample._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn Message Grouping Sample</title>
    <script type="text/javascript" src="Handler.aspx?ms=connect"></script>
</head>
<body>
<p style='font-size:80%;color:#336699;'>Deploy this application onto IIS or Apache to get fastest result! </p>
<div id="status">Wait for the connection..</div>
<input type="button" id="channel" value="Join to the Stock1 Channel" /><br />
<div id="price" style="font:normal bold 14px sans,Arial">...</div>
<img id='chart1' src='http://chart.apis.google.com/chart?chs=360x245&cht=lc&chd=t:1&chtt=PokeIn+Realtime+Stock+Price+Demo&chxt=y'
/><br />
<p style='font-size:80%;color:#336699;'>This sample uses Google Charts and refresh rate of image is not related to PokeIn Library. </p>
<p style='font-size:80%;color:#336699;'>PokeIn Library. <a href='http://pokein.com' target=_blank>http://pokein.com</a> </p>
<script>
    var btnChannel = document.getElementById("channel");
    var txtStatus = document.getElementById("status");
    var txtPrice = document.getElementById("price");
    var chrt = document.getElementById("chart1");
    
    btnChannel.disabled = true;

    document.OnPokeInReady = function() {
        txtStatus.innerHTML = "Connecting to the server";
        PokeIn.Start(function(isConnected) {
            if (isConnected) {
                btnChannel.disabled = false;
                txtStatus.innerHTML = "Connected!";
            }
        });
    }

    btnChannel.joined = false;
    btnChannel.onclick = function() {
        if (!this.joined) 
        {
            pCall["StockDemo"].JoinChannel();
        }
        else 
        {
            pCall["StockDemo"].LeaveChannel();
        }
        btnChannel.disabled = true;
    }

    function Pinned() {
        txtStatus.innerHTML = "Subscribed!";
        btnChannel.value = "Leave the Stock1 Channel";
        btnChannel.joined = true;
        btnChannel.disabled = false;
    }

    function Unpinned() {
        txtStatus.innerHTML = "Unsubscribed!";
        btnChannel.value = "Join to the Stock1 Channel";
        btnChannel.joined = false;
        btnChannel.disabled = false;
    }

    var prices = [];
    function UpdatePrices(quotName, price) {
        txtPrice.innerHTML = quotName + " : " + price;
        prices.push(price);
        if (prices.length > 12) {
            prices.shift();
        }
        updateChart();
    }

    chrt.onload = function() {
        this.loading = false;
    }

    chrt.loading = false;
    function updateChart() {
        if (!chrt.loading) {
            var ptext = prices.join(",");
            chrt.src = "http://chart.apis.google.com/chart?chs=360x245&cht=lc&chd=t:" + ptext + "&chtt=PokeIn+Realtime+Stock+Price+Demo&chxt=y";
            chrt.loading = true;
        }
    }
</script>
</body>
</html>
