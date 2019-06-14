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

function addParticipant() {
    console.log("clicked");
    let email_input = $('#participant_Input').val();
    $.ajax({
        method: 'POST',
        url: '/Moderator/SendInvitation',
        data: {
            contestID: competeID,
            email: email_input
        },
        success: function (response) {
            var html = `<div class="row table-row no-margin table-cs">
                        <div class="col-xs-1 vd-col-xs-40" style="margin-left: 5px;">${response.Name}</div>
                        <div class="col-xs-1 vd-col-xs-30 text-center">${response.Email}</div>
                        <div class="col-xs-1 vd-col-xs-30 text-center">
                            <button class="btn btn-primary">

                            </button>
                        </div>
                            </div>`
            $('#participant-list').append(html);
        }
    });
};