
window.addEventListener('load', () => {
    let likeCheckboxs = document.querySelectorAll(".like__checkbox");
    likeCheckboxs.forEach((checkbox, index) => {
        if (checkbox.dataset.type === "comment") {
            for (var i = 0; i < commentStatus.length; i++) {
                if (checkbox.dataset.id === commentStatus[i])
                    checkbox.checked = true;
            }
        }
        if (checkbox.dataset.type === "reply") {
            for (var i = 0; i < replyStatus.length; i++) {
                if (checkbox.dataset.id === replyStatus[i])
                    checkbox.checked = true;
            }
        }
    });

    let sortBox = document.querySelector("#discussion_sort");
    sortBox.addEventListener('change', () => {
        let selectedOption = sortBox.options[sortBox.selectedIndex].value;
        let loading = document.querySelector(".lds-roller");
        loading.setAttribute("display", "flex");
        $.ajax({
            method: 'GET',
            url: '/Challenge/GetSortComment',
            data: {
                challengeID: challengeId,
                sort: selectedOption
            },
            success: function (response) {
                loading.setAttribute("display", "none");
                document.querySelector('.discussion-group').innerHTML = response;
            },
            error: function (response) {
                alert(response);
            }
        })
    });
});


function ShowReplyInput(btn) {
    let replies = document.querySelector(`#replies-${btn.dataset.id}`);
    let liNode = document.createElement("li");
    liNode.innerHTML = `<div class="discussion__input reply__input">
                <img class="discussion__avatar" id="current_avatar"
                     src="~/Assets/Public/images/profile_photos/${userPhoto}" alt="Profile Picture">
                <textarea class="discussion__text" name="reply_input" data-id="${btn.dataset.id}" id="reply_input"
                          rows="1"></textarea>
                <button class="btn btn-primary" onclick="addReply()">Post Comment</button>
            </div>`;
    console.log(liNode);
    let inputCheck = document.querySelector(".reply__input");
    if (document.body.contains(inputCheck)) {
        inputCheck.remove();
    }
    replies.appendChild(liNode);

    $('html,body').animate({
        scrollTop: liNode.offsetTop
    }, 1000, 'easeOutCubic');
}

function Like(btn) {
    let commentID = btn.dataset.id;
    let commentType = btn.dataset.type;
    let likeElement = btn.previousElementSibling.firstElementChild;
    let likes = parseInt(likeElement.innerHTML, 10);
    if (btn.checked == true) {
        likes++;
    }
    else {
        likes--;
    }
    $.ajax({
        method: 'POST',
        url: '/Challenge/UpdateLikes',
        data: {
            likes: likes,
            commentId: commentID,
            userId: userId,
            type: commentType,
        },
        success: function (response) {
            likeElement.innerHTML = likes;
        }
    })
}

function addComment() {
    var comment = $('#comment_input').val();
    $.ajax({
        method: 'POST',
        url: "/Challenge/AddComment",
        data: {
            userID: userId,
            challengeID: challengeId,
            commentInput: comment,
        },
    })
        .done(function (response) {
            var template = $("#comment_template").html();
            var photo = $("#current_avatar").attr('src');
            var html = Mustache.render(template, {
                Photo: photo,
                UserName: userName,
                CreateDate: 'Just now',
                Text: comment,
                Likes: 0,
                commentID: response,
            });
            $(".comments").prepend(html);
            $('#comment_input').html("");
            
        });
}

function addReply() {
    var comment = $('#reply_input').val();
    var commentID = $('#reply_input').data("id");
    $.ajax({
        method: 'POST',
        url: "/Challenge/AddReply",
        data: {
            userID: userId,
            commentID: commentID,
            commentInput: comment,
        },
    })
        .done(function (response) {
            var template = $("#reply_template").html();
            var photo = $("#current_avatar").attr('src');
            var html = Mustache.render(template, {
                Photo: photo,
                UserName: userName,
                CreateDate: 'Just now',
                Text: comment,
                Likes: 0,
                replyID: response,
                commentID: commentID
            });
            $(`#replies-${commentID}`).append(html);
            $('.reply__input').remove();
        });
}