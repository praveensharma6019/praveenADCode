

$(document).ready(function () {

  





    $('#btnHeaderContactSubmit').click(function () {



        $('#btnHeaderContactSubmit').attr("disabled", "disabled");
      
        var hmobile = $("#HeaderContact_form1 #hmobile").val();
        if (hmobile == "") {
            alert("Please provide your Mobile Number");
            $("#HeaderContact_form1 #hmobile").focus();
            $('#btnHeaderContactSubmit').removeAttr("disabled");
            return false;
        }
        else {
            if (document.HeaderContact_form1.hmobile.value.length != 10) {
                alert("Mobile Number sould be 10 digit!");
                document.HeaderContact_form1.hmobile.focus();
                $('#btnHeaderContactSubmit').removeAttr("disabled");
                return false;
            }
        }
       
        // if (!captchavarified) {
        // alert("please click on captcha field");
        // $('#btnHeaderContactSubmit').removeattr("disabled");
        // return false;
        // }


        //create json object
   
        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
        var hFormType = $('#HeaderContact_form1 #hFormType').val();
        var hPageInfo = ($('#HeaderContact_form1 #hPageInfo').val()).replace("/", "");

        if (hPageInfo == "" || hPageInfo == "/") { hPageInfo = "Home"; }


        var savecustomdata = {
           
            Mobile: hmobile,
            FormType: hFormType,
            PageInfo: hPageInfo,
            FormSubmitOn: currentdate
        };

        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Affordable/InsertHeaderContactdetail",
            contentType: "application/json",
            success: function (data) {
                //////////////

                if (data.status == "1") {

                  //  $('#HeaderContact_form1').submit();
                    window.location.href = "about-us";
                }
                else {
                    alert("Sorry Operation Failed!!! Please try again later");
                    $('#btnHeaderContactSubmit').removeAttr("disabled");
                    return false;
                }
            }
        });
        return 0;










        var model = {
            Mobile: hmobile,
            Name: "",
            Email: ""
        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: "/api/Affordable/sendOTP",
            contentType: "application/json",
            success: function (data) {
                if (data.status == "1") {
                    var otp = prompt("Please enter OTP received on your mobile", "");

                    if (otp != null) {

                        var generatedOtp = {
                            mobile: hmobile,
                            OTP: otp
                        };
                        $.ajax({
                            type: "POST",
                            data: JSON.stringify(generatedOtp),
                            url: "/api/Realty/VerifyOTP",
                            contentType: "application/json",
                            success: function (data) {
                                if (data.status == "1") {
                                    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                    var hFormType = $('#HeaderContact_form1 #hFormType').val();
                                    var hPageInfo = ($('#HeaderContact_form1 #hPageInfo').val()).replace("/", "");

                                    if (hPageInfo == "" || hPageInfo == "/") { hPageInfo = "Home"; }


                                    var savecustomdata = {

                                        Mobile: hmobile,
                                        FormType: hFormType,
                                        PageInfo: hPageInfo,
                                        FormSubmitOn: currentdate
                                    };

                                    //ajax calling to insert  custom data function
                                    $.ajax({
                                        type: "POST",
                                        data: JSON.stringify(savecustomdata),
                                        url: "/api/Affordable/InsertHeaderContactdetail",
                                        contentType: "application/json",
                                        success: function (data) {
                                            //////////////

                                            if (data.status == "1") {

                                                //  $('#HeaderContact_form1').submit();
                                                window.location.href = "about-us";
                                            }
                                            else {
                                                alert("Sorry Operation Failed!!! Please try again later");
                                                $('#btnHeaderContactSubmit').removeAttr("disabled");
                                                return false;
                                            }
                                        }
                                    });
                                }
                                else {
                                    alert("Invalid OTP");
                                    $('#btnHeaderContactSubmit').removeAttr("disabled");
                                    return false;
                                }
                            }
                        });

                    }
                }
                else if (data == "-1") {
                    alert("Invalid Mobile Number");
                    $('#btnHeaderContactSubmit').removeAttr("disabled");
                }
            }
        });

        return false;
    });


});
