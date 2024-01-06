var recaptcha2;
var onloadCallback = function () {

    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha2', {
        'sitekey': '6LclBqwUAAAAAJ2KtS78FPoLPod26RXeKH5iddFy', //Replace this with your Site key
        'theme': 'light'
    });


};
var myCallBack = function () {
    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha2', {
        'sitekey': '6LclBqwUAAAAAJ2KtS78FPoLPod26RXeKH5iddFy', //Replace this with your Site key
        'theme': 'light'
    });


};
$("#btnSubscribeUsSubmit").click(function() {
			 var response = grecaptcha.getResponse(recaptcha2);
    // if (response.length == 0) {
        // alert("Captcha required.");
        // return false;
    // }
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

            
            Name: '',
            Email: mailid,
            MessageType: '',
            Mobile: '',
            Message: '',
            FormType: formtype,
            PageInfo: pageinfo,
            reResponse: response,
            FormSubmitOn: currentdate,
            emailMessage: $("#emailMessage").val()

        };

        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Adanigas/InsertSubscribedetail",
            contentType: "application/json",
            success: function (data) {
                //////////////

                if (data.status === "1") {
         window.location.href = "/thankyou";
                    //$('#contact_form1').submit();
                }
				else if (data.status == "2") {
                    alert("OOPS! You have missed Captcha Validation. Kindly validate to proceed further.");
                    $('#btnSubscribeUsSubmit').removeAttr("disabled");
                    return false;
                }
                else {
                    alert("Sorry Operation Failed!!! Please try again later");
                    $('#btnContactUsSubmit').removeAttr("disabled");
                    return false;
                }
            }
        });
        return false;

    });
    
