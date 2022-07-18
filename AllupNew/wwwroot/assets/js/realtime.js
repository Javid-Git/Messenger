$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl('/chathub').build();

    connection.start();

    connection.on('online', function (id) {
        let span = $(`#${id} span`);
        span.addClass('bg-success');
        span.removeClass('bg-secondary');
    })

    connection.on('ofline', function (id) {
        let span = $(`#${id} span`);
        span.removeClass('bg-success');
        span.addClass('bg-secondary');
    })

    $(document).on('click', '.userItem', function () {
        

        let fullName = $(this).attr("data-userName")
        let userId = $(this).attr('id');
        console.log(`${fullName} ${userId}`);
        $('.sender').text(fullName);
        $('#userId').val(userId);
    })
})