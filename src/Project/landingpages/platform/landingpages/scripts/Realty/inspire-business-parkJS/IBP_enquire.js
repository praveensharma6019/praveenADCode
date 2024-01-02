var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}
function GetURLParameter(sParam) {
    var url_string = window.location.href;
    var url = new URL(url_string);
    var c = url.searchParams.get(sParam);
    return c
}
var globalurl;
var GlobalmasterId;
var GlobalAdvId;
var Globalutm_source;
var Globalutm_placement;
var GlobalrecordId;
var isincludedquerystring = false;

var globalurl = window.location.href;
if (globalurl.includes('?')) {
    function GetURLParameter(sParam) {
        var url_string = window.location.href;
        var url = new URL(url_string);
        var c = url.searchParams.get(sParam);
        return c
    }
    Globalutm_source = GetURLParameter('utm_source');
    Globalutm_placement = GetURLParameter('utm_placement');
    GlobalAdvId = GetURLParameter('AdvertisementId');
    GlobalrecordId = "0122v000001uO6j";
    GlobalmasterId = "a4F2v000000IEFN";
    isincludedquerystring = true;
}
else {
    Globalutm_source = "";
    Globalutm_placement = "";
    GlobalAdvId = "";
    GlobalrecordId = "";
    GlobalmasterId = "";
    isincludedquerystring = false;
}


$("#form_EQ").click(function (e) {
    e.preventDefault();
    if (!$("#full_name_eq").val()) {
        $("#full_name_eq").next().html("Please enter valid name");
        $("#full_name_eq").next().css("color", "#DE0000");
        $("#full_name_eq").css("border-color", "#DE0000");
        return;
    } else {
        $("#full_name_eq").next().html("Name");
        $("#full_name_eq").next().css("color", "#404040");
        $("#full_name_eq").css("border-color", "#676767");
    }
    if (!$("#email_eq").val() || validateEmail($("#email_eq").val()) == false) {
        $("#email_eq").next().html("Please enter valid email id");
        $("#email_eq").next().css("color", "#DE0000");
        $("#email_eq").css("border-color", "#DE0000");
        return;
    } else {
        $("#email_eq").next().html("Email");
        $("#email_eq").next().css("color", "#404040");
        $("#email_eq").css("border-color", "#676767");
    }
    if ($(".checklabel input").prop("checked") == false) {
        $(".checklabel .error").html("Please agree to the terms");
        return;
    } else {
        $(".checklabel .error").html("");
    }
    if (
        !$("#phone_number_eq").val() ||
        $("#phone_number_eq").val().length !== 10 ||
        regex_special_num.test($("#phone_number_eq").val()) == false
    ) {
        $("#phone_number_eq").next().html("Please enter valid mobile number");
        $("#phone_number_eq").next().css("color", "#DE0000");
        $("#phone_number_eq").css("border-color", "#DE0000");
        return;
    } else {
        $("#phone_number_eq").next().html("Mobile Number");
        $("#phone_number_eq").next().css("color", "#404040");
        $("#phone_number_eq").css("border-color", "#676767");
    }

    document.getElementById("form_EQ").disabled = true;

    if ($("#full_name_eq").val() !== '') {
        var savecustomdata = {

            full_name: $("#full_name_eq").val(),
            mobile: $("#phone_number_eq").val(),
            email: $("#email_eq").val(),
            Projects_Interested__c: "COMMERCIAL",
            PropertyLocation: "Ahmedabad",
            Remarks: "",
            FormType: $("#form-heading").html(),
            Country: "India",
            PageInfo: "inspire-business-park",
            ProjectName: "inspire-business-park",
            PageURL: window.location.href,
            FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
            UTMSource: Globalutm_source,
            UTMPlacement: Globalutm_placement,
            RecordType: GlobalrecordId,
            PropertyCode: GlobalmasterId,
            AdvertisementId: GlobalAdvId,
            reResponse: $(".g-recaptcha").val(),
            isincludedquerystring: isincludedquerystring,
        };
        if ($("#form-heading").html() == "Enquire Now") {
            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/InspireBusinessParkEnquireNow",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "101") {

                        $("body").removeClass("form-open");
                        $("#thankname").html($("#full_name_eq").val());
                        $("#thankemail").html($("#email_eq").val());
                        $("#thanknum").html("+91 " + $("#phone_number_eq").val());

                        $(".verification-code--inputs input").removeClass("verification-input");
                        $(".invalid_otp_err").html("");
                        $("body").toggleClass("thnku-form-open");
                        $("body").removeClass("otp-form-open");

                        $(".thankyou-form").click(function (e) {
                            e.stopPropagation();
                        });
                        $(".thankyou_close_btn").click(function () {
                            $("body").removeClass("thnku-form-open");
                        });

                        $(".thankyou_done").click(function (e) {
                            e.stopPropagation();
                        });

                        $(".thankyou_done").click(function () {
                            $("body").removeClass("thnku-form-open");
                            document.getElementById("footer_form").reset();
                            document.getElementById("OTP_form").reset();
                            document.getElementById("EQ_Form").reset();
                            $("#full_name_fot").next().html("");
                            $("#email_fot").next().html("");
                            $("#phone_number_fot").next().html("");
                            $("#agreeCheckboxError").hide();
                            document.getElementById("form_EQ").disabled = false;
                        });


                    }
                    else if (data.status === "401") {
                        document.getElementById("form_EQ").disabled = false;
                        $("#full_name_eq").next().html('Please Enter Valid Name!');
                    } else if (data.status === "403") {
                        document.getElementById("form_EQ").disabled = false;
                        $("#email_eq").next().html('Please enter proper Email ID!');
                    } else if (data.status === "405") {
                        document.getElementById("form_EQ").disabled = false;
                        $("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
                    } else if (data.status === "0") {
                        document.getElementById("form_EQ").disabled = false;
                        document.getElementById("OTP_form").reset();
                        $(".verification-code--inputs input").addClass("verification-input");
                        $(".invalid_otp_err").html("Invalid OTP. Please try again.");
                    } else if (data.status === "503") {
                        document.getElementById("form_EQ").disabled = false;
                        $("#otp_limit").html('OTP limit exceeded');

                    }
                },
                error: function (data) {
                    $("#SVform_EQ").next().html('Some technical issue please try after some time'); // 
                }

            });
        } else {
            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Realty/InspireBusinessParkEnquireNow",
                contentType: "application/json",
                success: function (data) {
                    if (data.status === "101") {

                        $("body").removeClass("form-open");
                        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

                        if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i) || userAgent.match(/iPod/i)) {
                            // alert('IOS Device');


                            var link = document.createElement('a');
                            link.href = "images/Realty/inspire-business-park/images/Inspire-Business-Park.pdf";
                            link.download = "Inspire Business Park Brochure.pdf";
                            link.click();
                            link.remove();
                            setTimeout(function () {
                                link.click();
                                link.remove();
                            }, 500);

                        }
                        else if (userAgent.toString().includes("Mac OS")) {
                            var link = document.createElement('a');
                            link.href = "images/Realty/inspire-business-park/images/Inspire-Business-Park.pdf";
                            link.download = "Inspire Business Park Brochure.pdf";
                            setTimeout(function () {
                                link.click();
                                link.remove();
                            }, 50);
                            $(this).prop('disabled', true);
                        }
                        // Desktop Version Start
                        else {
                            var link = document.createElement('a');
                            link.href = "images/Realty/inspire-business-park/images/Inspire-Business-Park.pdf";
                            link.download = "Inspire Business Park Brochure.pdf";
                            link.click();
                            link.remove();
                            $(this).prop('disabled', true);
                        }
                        $("#thankname").html($("#full_name_eq").val());
                        $("#thankemail").html($("#email_eq").val());
                        $("#thanknum").html("+91 " + $("#phone_number_eq").val());

                        $(".verification-code--inputs input").removeClass("verification-input");
                        $(".invalid_otp_err").html("");
                        $("body").toggleClass("thnku-form-open");
                        $("body").removeClass("otp-form-open");

                        $(".thankyou-form").click(function (e) {
                            e.stopPropagation();
                        });
                        $(".thankyou_close_btn").click(function () {
                            $("body").removeClass("thnku-form-open");
                        });

                        $(".thankyou_done").click(function (e) {
                            e.stopPropagation();
                        });

                        $(".thankyou_done").click(function () {
                            $("body").removeClass("thnku-form-open");
                            document.getElementById("footer_form").reset();
                            document.getElementById("EQ_Form").reset();
                            $("#full_name_fot").next().html("");
                            $("#email_fot").next().html("");
                            $("#phone_number_fot").next().html("");
                            $("#agreeCheckboxError").hide();
                            document.getElementById("form_EQ").disabled = false;
                        });
                    }
                    else if (data.status === "401") {
                        document.getElementById("form_EQ").disabled = false;
                        $("#broc_name").next().html('Please Enter Valid Name!');
                    } else if (data.status === "403") {
                        document.getElementById("form_EQ").disabled = false;
                        $("#broc_email").next().html('Please enter proper Email ID!');
                    } else if (data.status === "405") {
                        document.getElementById("form_EQ").disabled = false;
                        $("#broc_mobile").next().html('Please Enter Your Mobile Number!');
                    } else if (data.status === "0") {
                        document.getElementById("form_EQ").disabled = false;
                        document.getElementById("OTP_form").reset();
                        $(".verification-code--inputs input").addClass("verification-input");
                        $(".invalid_otp_err").html("Invalid OTP. Please try again.");
                    }

                },

                error: function (data) {
                    document.getElementById("form_EQ").disabled = false;
                    $("#broc_eqform").next().html('Some technical issue please try after some time'); // 
                }
            });
        }
    }

});

$("#formSV_fot").click(function (e) {
    e.preventDefault();

    if (!$("#full_name_fot").val()) {
        $("#full_name_fot").next().html("Please enter valid name");
        return false;
    } else {
        $("#full_name_fot").next().html("");
    }

    if (!$("#email_fot").val()) {
        $("#email_fot").next().html("Please enter valid email id");
        return false;
    } else {
        $("#email_fot").next().html("");
    }

    if (validateEmail($("#email_fot").val()) == false) {
        $("#email_fot").next().html("Please enter valid email id");
        return false;
    } else {
        $("#email_fot").next().html("");
    }

    if (!$("#phone_number_fot").val()) {
        $("#phone_number_fot").next().html("Please enter valid mobile number");
        return false;
    } else {
        $("#phone_number_fot").next().html("");
    }

    if (!$("#agreeCheckbox").prop("checked")) {
        $("#agreeCheckboxError").show();
        return false;
    } else {
        $("#agreeCheckboxError").hide();
    }

    if (
        $("#phone_number_fot").val().length !== 10 ||
        regex_special_num.test($("#phone_number_fot").val()) == false
    ) {
        $("#phone_number_fot").next().html("Please enter valid mobile number");
        return false;
    } else {
        $("#phone_number_fot").next().html("");
    }

    document.getElementById("formSV_fot").disabled = true;

    if ($("#full_name_fot").val() !== '') {
        var savecustomdata = {
            full_name: $("#full_name_fot").val(),
            mobile: $("#phone_number_fot").val(),
            email: $("#email_fot").val(),
            Projects_Interested__c: "COMMERCIAL",
            PropertyLocation: "Ahmedabad",
            Remarks: "",
            FormType: "Enquire Now",
            Country: "India",
            PageInfo: "inspire-business-park",
            ProjectName: "inspire-business-park",
            PageURL: window.location.href,
            FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
            UTMSource: Globalutm_source,
            UTMPlacement: Globalutm_placement,
            RecordType: GlobalrecordId,
            PropertyCode: GlobalmasterId,
            AdvertisementId: GlobalAdvId,
            reResponse: $(".g-recaptcha").val(),
            isincludedquerystring: isincludedquerystring,

        };

        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Realty/InspireBusinessParkEnquireNow",
            contentType: "application/json",
            success: function (data) {
                if (data.status === "101") {

                    $("#thankname").html($("#full_name_fot").val());
                    $("#thankemail").html($("#email_fot").val());
                    $("#thanknum").html("+91 " + $("#phone_number_fot").val());

                    $(".verification-code--inputs input").removeClass("verification-input");
                    $(".invalid_otp_err").html("");
                    $("body").toggleClass("thnku-form-open");
                    $("body").removeClass("otp-form-open");

                    $(".thankyou-form").click(function (e) {
                        e.stopPropagation();
                    });
                    $(".thankyou_close_btn").click(function () {
                        $("body").removeClass("thnku-form-open");
                    });

                    $(".thankyou_done").click(function (e) {
                        e.stopPropagation();
                    });

                    $(".thankyou_done").click(function () {
                        $("body").removeClass("thnku-form-open");
                        document.getElementById("footer_form").reset();
                        document.getElementById("OTP_form").reset();
                        $("#full_name_fot").next().html("");
                        $("#email_fot").next().html("");
                        $("#phone_number_fot").next().html("");
                        $("#agreeCheckboxError").hide();
                        document.getElementById("formSV_fot").disabled = false;
                    });
                }
                else if (data.status === "401") {
                    document.getElementById("formSV_fot").disabled = false
                    $("#full_name_fot").next().html('Please Enter Valid Name!');
                } else if (data.status === "403") {
                    document.getElementById("formSV_fot").disabled = false
                    $("#email_fot").next().html('Please enter proper Email ID!');
                } else if (data.status === "405") {
                    document.getElementById("formSV_fot").disabled = false
                    $("#phone_number_fot").next().html('Please Enter Your Mobile Number!');
                } else if (data.status === "0") {
                    document.getElementById("formSV_fot").disabled = false
                    document.getElementById("OTP_form").reset();
                    $(".verification-code--inputs input").addClass("verification-input");
                    $(".invalid_otp_err").html("Invalid OTP. Please try again.");
                }

            },

            error: function (data) {
                document.getElementById("formSV_fot").disabled = false
                $("#formSV_fot").next().html('Some technical issue please try after some time'); // 
            }
        });
    }
});