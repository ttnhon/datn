$(function () {
    jQuery('.date-picker').datetimepicker({
        format: 'm/d/Y H:i',
        lang: 'vi',

    });

});

$('#js-question').click(function () {
    $(this).addClass('active');
    $('#js-details').removeClass('active');
    $('#js-participant').removeClass('active');
    $('#js-challenge').removeClass('active');
    $('#js-score').removeClass('active');

    $.ajax({
        method: 'GET',
        url: '/Moderator/RenderQuestionView',
        data: {
            id: competeID
        },
        success: function (response) {
            $('.tab-section').html(response);
        }
    });
});

$('#js-participant').click(function () {
    $(this).addClass('active');
    $('#js-details').removeClass('active');
    $('#js-question').removeClass('active');
    $('#js-challenge').removeClass('active');
    $('#js-score').removeClass('active');

    $.ajax({
        method: 'GET',
        url: '/Moderator/RenderParticipantView',
        data: {
            id: competeID
        },
        success: function (response) {
            $('.tab-section').html(response);
            $('#js-participant-add-btn').attr("disabled", true);
        }
    })
});

$('#js-challenge').click(function () {
    $(this).addClass('active');
    $('#js-details').removeClass('active');
    $('#js-question').removeClass('active');
    $('#js-participant').removeClass('active');
    $('#js-score').removeClass('active');

    $.ajax({
        method: 'GET',
        url: '/Moderator/RenderChallengeView',
        data: {
            id: competeID
        },
        success: function (response) {
            $('.tab-section').html(response);
        }
    })
});

$('#js-score').click(function () {
    $(this).addClass('active');
    $('#js-details').removeClass('active');
    $('#js-question').removeClass('active');
    $('#js-participant').removeClass('active');
    $('#js-challenge').removeClass('active');

    $.ajax({
        method: 'GET',
        url: '/Moderator/RenderScoreView',
        data: {
            id: competeID
        },
        success: function (response) {
            $('.tab-section').html(response);
        }
    })
});

function OnSuccess(data) {
    console.log(data)
    if (data) {
        alert('Save changes succeed');
    } else {
        alert('operation fail');
    }
}
function OnFailure(data) {
    console.log(data)
    alert('POST: Save changes fail');
}

function DeleteParticipant(btn) {
    $.ajax({
        method:'POST',
        url: '/Moderator/DeleteParticipant',
        data: {
            contestID: competeID,
            userID: btn.dataset.id
        },
        success: function (response) {
            alert(response);
            $(`div[id=p-${btn.dataset.id}]`).remove();
            if ($('#participant-list').html() === '') {
                var html = `<div id="no-content" class="row table-row no-margin table-cs">
                    <div class="col-sm-12 text-center">
                        You have not invite any participant.
                    </div>
                    </div>`;
                $('#participant-list').append(html);
            }
        }
    });
}

function DeleteChallenge(btn) {
    $.ajax({
        method: 'POST',
        url: '/Moderator/DeleteChallenge',
        data: {
            contestID: competeID,
            challengeID: btn.dataset.id
        },
        success: function (response) {
            alert(response);
            $(`div[id=c-${btn.dataset.id}]`).remove();
            if ($('#challenge-list').html() === '') {
                var html = `<div id="no-content" class="row table-row no-margin table-cs">
                    <div class="col-sm-12 text-center">
                        You have not created any challenges.
                    </div>
                    </div>`;
                $('#challenge-list').append(html);
            }
        }
    });
}

function DeleteQuestion(btn) {
    $.ajax({
        method: 'POST',
        url: '/Moderator/DeleteQuestion',
        data: {
            contestID: competeID,
            questionID: btn.dataset.id
        },
        success: function (response) {
            alert(response);
            $(`div[id=q-${btn.dataset.id}]`).remove();
            if ($('#question-list').html() === '') {
                var html = `<div id="no-content" class="row table-row no-margin table-cs">
                    <div class="col-sm-12 text-center">
                        You have not created any questions.
                    </div>
                    </div>`;
                $('#question-list').append(html);
            }
        }
    });
}


function addParticipant() {
    let email_input = $('#participant_Input').val();
    $('#participant_Input').val('');
    let loading = document.querySelector(".spinner");
    loading.style.display = 'flex';
    $.ajax({
        method: 'POST',
        url: '/Moderator/SendInvitation',
        data: {
            contestID: competeID,
            email: email_input
        },
        success: function (response) {
            loading.style.display = 'none';
            alert(response.msg);
            if (response.result) {
                $('#no-content').remove();
                var html = `<div class="row table-row no-margin table-cs"  id="${response.data.ID}">
                        <div class="col-xs-1 vd-col-xs-40" style="margin-left: 5px;">${response.data.Name}</div>
                        <div class="col-xs-1 vd-col-xs-20 text-center">${response.data.Email}</div>
                        <div class="col-xs-1 vd-col-xs-20 text-center">Waiting for response...</div>
                        <div class="col-xs-1 vd-col-xs-20 text-center">
                            <button class="btn btn-primary"  id="btn-${response.data.ID}">
                                 Delete
                            </button>
                        </div>
                            </div>`
                $('#participant-list').prepend(html);
            }
            
        }
    });
};

/*Email Autocomplete*/

function EmailAutocomplete() {
    var searchVal = $('#participant_Input').val();
    if (searchVal.length >= 3) {
        $('#js-participant-add-btn').attr("disabled", false);
    }
    else {
        $('#js-participant-add-btn').attr("disabled", true);
    }

    $.ajax({
        method: 'GET',
        url: '/Moderator/GetUserName',
        success: function (data) {
            $('#participant_Input').autocomplete({
                source: data
            },
                {
                    autoFocus: true,
                    minChars: 3,
                });
        }
    });
}

/* Add Existing Challenge*/

function OpenChallengePopup() {
    $('#challenge-popup').css("height", "100%");
}

function CloseChallengePopup() {
    $('#challenge-popup').css("height", "0%");
}

function AddToCompete() {
    var CheckedChallenges = document.querySelectorAll('input[type=checkbox]');
    var ChallengeIDList = new Array();
    CheckedChallenges.forEach((checkbox, index) => {
        if (checkbox.checked == true) {
            ChallengeIDList.push(checkbox.dataset.id);
        }
    });
    $.ajax({
        method: 'POST',
        url: '/Moderator/AddChallengeToCompete',
        data: {
            competeID: competeID,
            challengeIDList: ChallengeIDList
        },
        success: function (response) {
            alert(response.msg);
            $('#challenge-popup').css("height", "0%");
            if (response.result = true) {
                var html = '';
                response.data.forEach(challenge => {
                    var difficulty;
                    switch (challenge.Difficulty) {
                        case 1:
                            difficulty = 'Easy';
                            break;
                        case 2:
                            difficulty = 'Medium';
                            break;
                        case 3:
                            difficulty = 'Hard';
                            break;
                        case 4:
                            difficulty = 'Advanded';
                            break;
                        case 5:
                            difficulty = 'Expert';
                            break;
                    }
                    html += `<div class="row table-row no-margin table-cs" id="c-${challenge.competeID}">

                <div class="col-xs-1 vd-col-xs-30" style="margin-left: 1em;">
                    <a href="/Moderator/Edit/Challenge/${challenge.competeID}">
                        ${challenge.Name}
                    </a>
                </div>
                <div class="col-xs-1 vd-col-xs-30 text-center">${difficulty}</div>
                <div class="col-xs-1 vd-col-xs-20 text-center">${challenge.Score}</div>
                <div class="col-xs-1 vd-col-xs-20 text-center">
                    <button class="btn btn-primary" data-id="${challenge.competeID}" onclick="DeleteChallenge(this)">
                        Delete
                    </button>
                </div>
            </div>`
                });
                $('#challenge-list').prepend(html);
            }
            
        }
    });
}