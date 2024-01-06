$(document).ready(function () {
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {
        debugger;
        $('#message_modal').modal('show');
        $("#message").val("");
        event.preventDefault();
    }

    $('.rbSelectUserName').change(function (event) {
        GetScrollPosition();
        var form = $(event.target).parents('form');
        form.submit();
    });

    $("#loader-wrapper").hide();


    if (localStorage['page'] == document.URL && localStorage['scrollTop'] != null && localStorage['scrollTop'] != "") {
        $(document).scrollTop(localStorage['scrollTop']);
    }
    else {
        $(document).scrollTop(0);
        localStorage['page'] = "";
        localStorage['scrollTop'] = "";
    }
});