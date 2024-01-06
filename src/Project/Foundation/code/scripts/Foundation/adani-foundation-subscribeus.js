

$("#btnSubscribeUsSubmit").click(function () {

    $('#btnSubscribeUsSubmit').attr("disabled", "disabled");

    var mailid = $("#subscribeEmail").val();
    if (mailid == "") { alert("Email is Required"); $("#subscribeEmail").focus(); $('#btnSubscribeUsSubmit').removeAttr("disabled"); return false; }

    var formtype = $("#sFormType").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#subscribeEmail").focus(); $('#btnSubscribeUsSubmit').removeAttr("disabled"); return false; }



    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }



    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {

        Email: mailid,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Foundation/InsertSubscribeUsFormdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnSubscribeUsSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;
});
