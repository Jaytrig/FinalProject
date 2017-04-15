$(function () {
    $('#postFormBtn').click(function () {
        console.log('clicked');
        $.ajax({
            type: 'Post',
            url: '/profile/SubmitPost',
            data: {
                title: $('#titleinput').val(),
                message: $('#messageinput').val(),
                PostedTime: moment().format("YYYY-MM-DD HH:mm:ss")
            },
            success: function (data, textstatus) {
                location.reload();
            }
        });
    }); 


    updateCountdown();
    $('#messageinput').keyup(updateCountdown);
    $('#messageinput').change(updateCountdown);



    $('#DeletePostModelBtn').click(function () {
        $.ajax({
            type: 'Post',
            url: '/profile/DeletePost',
            data: {
                postId: $('#postToDelete').val()
            },
            success: function (data, textstatus) {
                $('#DeletePostModelBtn').modal('toggle');
                location.reload();
                setTimeout(function() {
                    location.reload();
                }, 100);
            }
        });
    });
});

function deletePost(postid) {
    $('#postToDelete').val(postid);
    $('#delete-Post-Model').modal('toggle');
}




function GoToRoom(id) {
    window.location = "/Profile/user/" + id;
}

function gotouser(id) {
    window.location = "/Profile/user/" + id;
}

function likePost(postID) {
    $.ajax({
        type: 'Get',
        url: '/Home/likePost',
        data: {
            postId: postID
        },
        success: function (data, textstatus) {


            $('.PostID-' + postID).each(function (index) {
                $(this).empty();
                $(this).html(data);
            });


        }
    });
}

function dislikePost(postID) {
    $.ajax({
        type: 'Get',
        url: '/Home/DislikePost',
        data: {
            postId: postID
        },
        success: function (data, textstatus) {
            $('.PostID-' + postID).each(function (index) {
                $(this).empty();
                $(this).html(data);
            });
        }
    });
}

function updateCountdown() {
    // 500 is the max message length
    console.log('hit');
    var remaining = 500 - $('#messageinput').val().length;
    $('.countdown').text(remaining + ' Characters Left');

    if (remaining === 500 || remaining < 0) {
        $('#postFormBtn').prop('disabled', true);

    } else {
        $('#postFormBtn').prop('disabled', false);

    }
}