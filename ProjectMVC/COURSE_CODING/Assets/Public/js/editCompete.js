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

    $.ajax({
        method: 'GET',
        url: '/Moderator/RenderParticipantView',
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
            $(`div[id=${btn.dataset.id}]`).remove();
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


function addParticipant() {
    let email_input = $('#participant_Input').val();
    $('#participant_Input').html('');
    $.ajax({
        method: 'POST',
        url: '/Moderator/SendInvitation',
        data: {
            contestID: competeID,
            email: email_input
        },
        success: function (response) {
            alert(response.result);
            $('#no-content').remove();
            var html = `<div class="row table-row no-margin table-cs"  id="${response.data.ID}">
                        <div class="col-xs-1 vd-col-xs-40" style="margin-left: 5px;">${response.data.Name}</div>
                        <div class="col-xs-1 vd-col-xs-30 text-center">${response.data.Email}</div>
                        <div class="col-xs-1 vd-col-xs-30 text-center">
                            <button class="btn btn-primary"  id="btn-${response.data.ID}">
                                 Delete
                            </button>
                        </div>
                            </div>`
            $('#participant-list').append(html);
        }
    });
};