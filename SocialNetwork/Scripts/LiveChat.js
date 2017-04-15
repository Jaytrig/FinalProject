// ALL STUFF BELOW IS CHAT STUFF...

//TODO:
//1: Change the ID's of the chat to other members USERID. (hard code to test.)
//2: 
//3:
//4:

var ws;
var wsOpen;
var onlineUsers = {};
$(document).ready(function () {

    //when document has loaded.
    ws = new WebSocket("ws://" + window.location.host + "/LiveChat.ashx");

    ws.onopen = function (open) {
        wsOpen = true;
    };

    ws.onclose = function () {

    }

    ws.onmessage = function (data) {
        var id;
        var message = data.data;
        //UserOnline
        if (message.startsWith("|online|true|")) {
            id = message.substring(13);
            onlineUsers[id] = true;
        } else if (message.startsWith("|online|false|")) {
            id = message.substring(14);
            onlineUsers[id] = false;
        } else if (message.startsWith("|message|")) {
            
            var indexOfIdStart = 9;
            // + indexOfIdStart to move along the string.
            var indexOfIdEnd = message.substring(indexOfIdStart).indexOf('|') + indexOfIdStart;
            var frindsID = message.substring(indexOfIdStart, indexOfIdEnd);
            var fullMessage = message.substring(indexOfIdEnd + 1);
            messageFromFriend(frindsID, fullMessage);

        }
    };

    ws.onerror = function (data) {
        console.log(data);
    };

    //window.onbeforeunload = function () {
    //    ws.onclose = function () { }; // disable onclose handler first
    //    ws.close();
    //};
});




function sendMessage(message) {
    ws.send(message);
}

function userOnline(id, array, callback) {
    setTimeout(
        function () {
            if (id in array) {
                if (callback != null) {
                    callback();
                }
                return;
            }
        }, 100); // wait 5 milisecond for the connection...
}

function waitForSocketConnection(socket, callback) {
    setTimeout(
        function () {
            if (socket.readyState === 1) {
                console.log("Connection is made");
                if (callback != null) {
                    callback();
                }
                return;

            } else {
                console.log("wait for connection...");
                waitForSocketConnection(socket, callback);
            }

        }, 100); // wait 5 milisecond for the connection...
}

function messageFromFriend(frindsId, fullMessage) {
    if ($('#Chat-' + frindsId).length !== 0) {
        recMessage(frindsId);
    } else {
        //add notification of unread.
        if ($('#' + frindsId).length !== 0) {
            var curr = parseInt($('#' + frindsId).text()) || 0;
            $('#' + frindsId).text(curr + 1);
        }
    }
}