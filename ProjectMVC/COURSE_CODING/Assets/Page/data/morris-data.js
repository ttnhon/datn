jQuery(document).ready(function () {

    $('.updateStatus').off('click').on('click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var dataList = "{ 'id':'" + id + "'}";
        var contentStatus = $('#' + 'user_' + id);
        $.ajax({
            url: "/Admin/User/UpdateStatus",
            contentType: "application/json",
            dataType: "json",
            method: "POST",
            data: dataList,
            success: function (respone) {
                if (respone.status === 1) contentStatus.text('unlock');
                else contentStatus.text('lock');
                alert("Update status  successfully !");

            },
            error: function (err) {
                console.log(err);
            }
        });
    })

    var dataUser;
    $.ajax({
        dataType: "json",
        url: "/Admin/Home/GetAllDataUsers",
        contentType: "application/json",
        type: "GET",
        success: function (respone) {
            dataUser = respone;
            //user chart
            Morris.Area({
                element: 'morris-area-chart',
                data: dataUser,
                xkey: 'month',
                ykeys: ['count'],
                labels: ['User', 'Month'],
                xLabels: 'month',
                pointSize: 6,
                xLabelMargin: 3,
                hideHover: 'auto',
                resize: true
            });
        },
        error: function (err) {
            console.log(err);
        }
    });

    $.ajax({
        dataType: "json",
        url: "/Admin/Home/GetAllDataLanguages",
        contentType: "application/json",
        type: "GET",
        success: function (respone) {
            dataLanguages = respone;
          // chanllenge chart and compete chart  
            Morris.Donut({
                element: 'morris-donut-chart',
                data: dataLanguages,
                resize: true
            });
        },
        error: function (err) {
            console.log(err);
        }
    });


});
