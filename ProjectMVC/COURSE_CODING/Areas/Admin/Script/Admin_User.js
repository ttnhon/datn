jQuery(document).ready(function () {

    //update status user
    $('.updateStatus').off('click').on('click', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var dataList = "{ 'id':'" + id + "'}";
        var contentStatus = $('#' + 'user_' + id) ;
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




});
