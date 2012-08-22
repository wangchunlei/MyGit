<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>"  %>
<%@ Import Namespace="PokeInMVC_Chat.Core" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PokeIn MVC Chat Room</title>
    <script src="/Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="/Scripts/Custom.js" type="text/javascript"></script>
    <link href="/Content/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Handler?ms=connect"></script>
</head>
<body>
    <div id="chatWrapper">
        <div id="chatBase">
            <div id="chats">PokeIn Chat Room Application<br />
                <ol id="messagesList">                    
                </ol>
            </div>
        </div>
        <div id="userListWrapper">
            <select id="userList" multiple="multiple">               
            </select>
        </div>
        <div id="messageArea">
            <div id="lbl">Username ::</div>
            <input id="info" type="text" />
            <input id="btnChat" type="button" value="Start" />
            <input id="btnPrivate" type="button" disabled="disabled" value="Send Private Message" />
        </div>
    </div>

    <div id="resumeLater" class="overlay-content jqmWindow">
        <div class="fancy_close jqmClose" style="display: block;">
        </div>
        <div class="header padded">
            <h2>Send Private Message</h2>
		</div>
		<div class="content">
        <textarea rows="8" cols="50" id="privateMessage"></textarea>
        <input id="btnPrivateSend" type="button" value="Send" /></div>
    </div>

<script type="text/javascript">
    document.OnPokeInReady = function () {
        PokeIn.Start();
        //PokeIn Connection Closed By Server 
        PokeIn.OnClose = function () {
            ChatMessageFrom("SERVER", "Your Connection Closed!");
        };
    };
        
    var info = $("#info");
    
    var user_name = "";
    var user_names = [];

    $(document).ready(function () {

        pCall['Chat'].RenderMemberList();

        $("#btnChat").click(function () {
            if (user_name == "") {
                user_name = info.val();
                if (user_name.replace(/ /g, "").length == 0) {
                    alert('You should enter the user name');
                    return;
                }
                pCall['Chat'].SetName(user_name);
                user_name = "";
            }
            else {
                var mess = info.val();
                if (mess.replace(/ /g, "").length == 0) {
                    alert('You should enter some message to send');
                    return;
                }
                pCall['Chat'].Send(mess);
                info.val("");
            }
        });

        $("#messagesList li:nth-child(odd)").addClass("even");

        $("#btnPrivate").attr('disabled', 'disabled');
        $('#userList').mousedown(function () {
            if ($(this).val != '') {
                $("#btnPrivate").removeAttr('disabled');
            }
        });


        $("#btnPrivateSend").click(function () {
            userID = $('#userList').val();
            var str = $("#privateMessage").val();
            str = str.replace(/[\n]/g, '<br>');
            SendPrivateMessage(userID, str);
            $("#privateMessage").val("");
            jQuery('#resumeLater').jqmHide();
        });

        $("#btnPrivate").click(function () {
            //   debugger;            
            userName = $('#userList :selected').text();
            if (userName != user_name) {
                jQuery('#resumeLater').jqmShow();
            }
            else {
                alert("You must select someone else to send a private message.");
            }

        });

        $("#info").keydown(function (ev) {
            var ev = ev || window.event;
            if (ev.keyCode == 13) {
                $("#btnChat").click();
            }
        });

        jQuery('#resumeLater').jqm({
            modal: true,
            onHide: closeModal
        });

        var closeModal = function (hash) {
            var $modalWindow = jQuery(hash.w);
            $modalWindow.hide("fast", function () {
                hash.o.remove();
            });
        };

    });

    function SendPrivateMessage(userID, message) {
        if (userID == null) {
            alert("Select A User To Send Message");
            return;
        }
        pCall['Chat'].SendPrivateMessage(userID.toString(), message);
    }

    function HandlePrivateMessage(message) {
        message = message.replace(/[\n]/g, '<br>');
        AppendToChat("<strong>Private Message</strong>:: " + message + "<br/>");
    }

    function GenerateMemberList(memberList) {
        
        $("#userList").html(memberList);
    }

    function ChatMessageFrom(user, message) {
        AppendToChat("<strong>" + user + "</strong>:: " + message + "<br/>");        
    }

    function AppendToChat(message) {

        var messageList = $("#messagesList");
        if ($("li:last", messageList).hasClass("even") == true) {
            messageList.append("<li>" + message + "</li>");
            
        }
        else if ($("li:last", messageList).hasClass("even") == false) {
            messageList.append("<li class='even'>" + message + "</li>");
        }
        $("li:last", messageList).glow();
        $("#chatBase").scrollTop($("#chats").height());        


    }

    function UserNameList_Updated(names) {
        user_names = names;
    }

    function UsernameSet(user) {
        user_name = user;
        $("#btnChat").val("Send");
        info.val("");
        $("#lbl").html("Message ::");
        ChatMessageFrom("SERVER", "Hi " + user + "!<br/>Welcome to Our Server!");
        $("#btnPrivate").show();
    }

    
</script> 
</body>
</html>
