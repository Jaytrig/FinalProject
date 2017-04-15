$(function () {
    waitForSocketConnection(ws, function () {
        online();
        setInterval(online, 10000);
    });

    $('#messageContents').bind('keyup', function (e) {
        if (e.keyCode === 13) { // 13 is enter key
            SendPressed();
        }
    });

    $('#sendBTN').click(function () {
        SendPressed();
    });
});

function online() {
    $(".friendIDs").each(function() {
        var friendId = $(this)[0].childNodes[3].value;
        sendMessage("|online|" + friendId);


        userOnline(friendId, onlineUsers, function(e) {
            for (var key in onlineUsers) {
                // skip loop if the property is from prototype
                if (!onlineUsers.hasOwnProperty(key)) continue;

                var obj = onlineUsers[key];

                if (obj) {
                    $('#' + key).css({ 'background-color': "lightgreen" });
                } else {
                    $('#' + key).css({ 'background-color': "orangered" });
                }

            }
        });


    });
}

function showMessage(userId) {
    
    $('#' + userId).empty();

    $.ajax({
        url: "/chat/getChat",
        data: { "name": userId },
        success: function(data) {
            $('#ChatMessagesContainer')
                .empty()
                .html(data);

            var element = document.getElementById("Chat-" + userId);
            element.scrollTop = element.scrollHeight;
        }
    });
}

function recMessage(userId) {
    setTimeout(showMessage(userId), 1000);
}

function SendPressed() {
    if ($('#openChatUserId').length !== 0) {
        var message = $('#messageContents').val();
        $('#messageContents').val('');
        var id = $('#openChatUserId').val();

        sendMessage("|message|" + id + "|" + moment().format("YYYY-MM-DD HH:mm:ss") + "|" + message);
        setTimeout(showMessage(id), 1000);
    }
}