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
        Swal.fire({
            position: 'top-end',
            type: 'success',
            title: 'Save changes succeed',
            showConfirmButton: false,
            timer: 1000
        })
    } else {
        Swal.fire({
            position: 'top-end',
            type: 'error',
            title: 'Operation fail',
            showConfirmButton: false,
            timer: 1000
        })
    }
}
function OnFailure(data) {
    //console.log(data)
    Swal.fire({
        position: 'top-end',
        type: 'error',
        title: 'POST: fail',
        showConfirmButton: false,
        timer: 1000
    })
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
            Swal.fire({
                position: 'top-end',
                type: 'info',
                title: response.msg,
                showConfirmButton: false,
                timer: 1000
            })
            $(`div[id=p-${btn.dataset.id}]`).remove();
            if ($('#participant-list').val() == '') {
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
            Swal.fire({
                position: 'top-end',
                type: 'success',
                title: response,
                showConfirmButton: false,
                timer: 1000
            })
            $(`div[id=c-${btn.dataset.id}]`).remove();
            if ($('#challenge-list').val() == '') {
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
            Swal.fire({
                position: 'top-end',
                type: 'info',
                title: response.msg,
                showConfirmButton: false,
                timer: 1000
            })
            $(`div[id=q-${btn.dataset.id}]`).remove();
            if ($('#question-list').val() == '') {
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
            Swal.fire({
                position: 'top-end',
                type: 'info',
                title: response.msg,
                showConfirmButton: false,
                timer: 1000
            })
            if (response.result) {
                $('#no-content').remove();
                var html = `<div class="row table-row no-margin table-cs"  id="p-${response.data.ID}">
                        <div class="col-xs-1 vd-col-xs-40" style="margin-left: 5px;">${response.data.Name}</div>
                        <div class="col-xs-1 vd-col-xs-20 text-center">${response.data.Email}</div>
                        <div class="col-xs-1 vd-col-xs-20 text-center">Waiting for response...</div>
                        <div class="col-xs-1 vd-col-xs-20 text-center">
                            <button class="btn btn-primary"  data-id="${response.data.ID}" onclick="DeleteParticipant(this)">
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
        url: '/Moderator/GetUserEmail',
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
            Swal.fire({
                position: 'top-end',
                type: 'info',
                title: response.msg,
                showConfirmButton: false,
                timer: 1000
            })
            $('#challenge-popup').css("height", "0%");
            if (response.result = true) {
                var html = '';
                response.data.forEach(challenge => {
                    var difficulty;
                    switch (challenge.Difficulty) {
                        case 0:
                            difficulty = 'Easy';
                            break;
                        case 1:
                            difficulty = 'Medium';
                            break;
                        case 2:
                            difficulty = 'Hard';
                            break;
                        case 3:
                            difficulty = 'Advanded';
                            break;
                        case 4:
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