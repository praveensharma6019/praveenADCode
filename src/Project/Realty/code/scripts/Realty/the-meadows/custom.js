
/*Adding validator function*/


jQuery.validator.addMethod("mobile", function (value, element) {
    return this.optional(element) || value.match(/^[1-9][0-9]*$/);
}, "Please enter the number without beginning with '0'");

jQuery.validator.addMethod("alphabets", function (value, element) {
    return this.optional(element) || /^[a-zA-Z ]*$/.test(value);
}, "Please enter Alphabets only");

jQuery.validator.addMethod("email", function (value, element) {
    return this.optional(element) || /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(value);
}, "Please enter a valid email address.");

jQuery.validator.addMethod("captcha", function (value, element) {
    return this.optional(element) || value.match($('#txtCaptcha').val());
}, "Please enter a valid code");

jQuery.validator.addMethod("captcha", function (value, element) {
    return this.optional(element) || value.match($('#txtCaptcha1').val());
}, "Please enter a valid code");

jQuery.validator.addMethod("captcha", function (value, element) {
    return this.optional(element) || value.match($('#txtCaptcha2').val());
}, "Please enter a valid code");

jQuery.validator.addMethod("captcha", function (value, element) {
    return this.optional(element) || value.match($('#txtCaptcha3').val());
}, "Please enter a valid code");

jQuery.validator.addMethod("captcha", function (value, element) {
    return this.optional(element) || value.match($('#txtCaptcha4').val());
}, "Please enter a valid code");
/*Adding validator function*/


$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.scrollup').fadeIn();
        } else {
            $('.scrollup').fadeOut();
        }
    });

    $('.scrollup').click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });
});

/************************************************************************************ SWITCHER CSS STARTS */

$(document).ready(function () {
    "use strict";
    $("#hide, #show").click(function () {
        if ($("#show").is(":visible")) {

            $("#show").animate({
                "margin-left": "-500px"
            }, 500, function () {
                $(this).hide();
            });

            $("#switch").animate({
                "margin-left": "0px"
            }, 500).show();
        } else {
            $("#switch").animate({
                "margin-left": "-500px"
            }, 500, function () {
                $(this).hide();
            });
            $("#show").show().animate({
                "margin-left": "0"
            }, 500);
        }
    });

});


/************************************************************************************ PORTFOLIO DETAIL STARTS */

$('.items').click(function (event) {
    event.preventDefault();

    if ($('.portfolio-detail').hasClass('open-box')) {
        $('.portfolio-detail').addClass('closed-box');
        $('.portfolio-detail').removeClass('open-box');
    }

    var fileID = $(this).attr('data-project');

    if (fileID != null) {
        $('html,body').animate({
            scrollTop: $('.portfolio-detail').offset().top - 68
        }, 500);
        $('#portfolio_img').attr('src', fileID);
        $('.portfolio-detail').addClass('open-box');
        $('.portfolio-detail').removeClass('closed-box');
    }
    $('#portfolio .close-detail').click(function () {
        setTimeout(function () {
            $('.portfolio-detail').addClass('closed-box');
            $('.portfolio-detail').removeClass('open-box');
        }, 300);
        $('html,body').animate({
            scrollTop: $('#portfolio').offset().top - 68
        }, 250);

    });
    /*        $.ajax({
                url: fileID
            }).success(function (data) {
                $('.portfolio-detail').addClass('open-box');
                $('.portfolio-detail').html(data);
                $('.portfolio-detail').removeClass('closed-box');

                $('.close-detail').click(function () {
                    $('.portfolio-detail').addClass('closed-box');
                    $('.portfolio-detail').removeClass('open-box');
                    $('html,body').animate({
                        scrollTop: $('#portfolio').offset().top - 68
                    }, 500);
                    setTimeout(function () {
                        $('.portfolio-detail').html('');
                    }, 1000);
                });
            });*/
});
/*ajax image view for plans section*/

$('.plans-items').click(function (event) {
    event.preventDefault();
    //alert('clicked');
    if ($('.plans-portfolio-detail').hasClass('oplans-open-box')) {
        $('.plans-portfolio-detail').addClass('closed-box');
        $('.plans-portfolio-detail').removeClass('open-box');
    }

    var fileID = $(this).attr('data-project');

    if (fileID != null) {
        $('html,body').animate({

            scrollTop: $('.plans-portfolio-detail').offset().top - 68
        }, 500);
        $('#plans_img').attr('src', fileID);
        $('.plans-portfolio-detail').addClass('plans-open-box');
        $('.plans-portfolio-detail').removeClass('closed-box');
    }
    $('#lodha-rera .close-detail').click(function () {
        setTimeout(function () {
            $('.plans-portfolio-detail').addClass('closed-box');
            $('.plans-portfolio-detail').removeClass('plans-open-box');
        }, 300);
        $('html,body').animate({
            scrollTop: $('#lodha-rera').offset().top - 68
        }, 250);

    });
});

var priceValidate;
var instantValidate;

if ($('#PopupForm').length > 0) {
    $('#PopupForm').validate({
        rules: {
            name: {
                required: true,
                alphabets: true,
                maxlength: 100
            },
            CountryCode: {
                required: true
            },
            mobile: {
                required: true,
                number: true,
                mobile: true,
                minlength: 10,
                maxlength: 10
            },
            email: {
                required: true,
                email: true
            }
            // captchatxt: {
            //   required: true,
            //  captcha:true
            // }
            /*comment: {
                required: true,
            }*/
        },
        submitHandler: function (form) {
            $(form).find(':submit').prop('disabled', true);
            var fname = $('#PopupForm').find("[name='name']").val();
            var enuiryemail = $('#PopupForm').find("[name='email']").val();
            var enuirymobile = $('#PopupForm').find("[name='mobile']").val();
            var budget = 0.0;
            var remarks = "";
            var country = $('#PopupForm').find("[name='CountryCode']").val();
            var SalesSource = $('#PopupForm').find("[name='sales_source']").val();
            var model = {
                mobile: enuirymobile,
                first_name: fname,
                email: enuiryemail

            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(model),
                url: "/api/Realty/Codegreen_Enquiry",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == "1") {
                        var otp = prompt("Please enter OTP received on your mobile", "");

                        if (otp != null) {

                            var generatedOtp = {
                                mobile: enuirymobile,
                                OTP: otp,
                            }
                            $.ajax({
                                type: "POST",
                                data: JSON.stringify(generatedOtp),
                                url: "/api/Realty/VerifyOTP",
                                contentType: "application/json",
                                success: function (data) {
                                    if (data.status == "1") {
                                        //create json object
                                        var countryname = $('#PopupForm').find("[name='CountryCode']").val();
                                        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                        var formtype = $('#PopupForm').find("[id='FormType']").val();
                                        var pageinfo = window.location.href;
                                        var propertytype = "The Meadows";
                                        var propertilocation = "";
                                        var SalesSource = $('#PopupForm').find("[name='sales_source']").val();
                                        var utmSource = $('#PopupForm').find("[id='utm_source']").val();

                                        var savecustomdata = {
                                            first_name: fname,
                                            last_name: fname,
                                            mobile: enuirymobile,
                                            email: enuiryemail,
                                            Budget: budget,
                                            country_code: countryname,
                                            //state_code: statename,
                                            Projects_Interested__c: propertytype,
                                            PropertyLocation: propertilocation,
                                            sale_type: SalesSource,
                                            Remarks: remarks,
                                            FormType: formtype,
                                            PageInfo: pageinfo,
                                            FormSubmitOn: currentdate,
                                            UTMSource: utmSource,
                                            //PropertyCode:"a4F2v000000IEFN",
                                            RecordType: "0122v000001uNyyAAE",
                                            PropertyCode: "a4F2v000000IEFN"
                                        };

                                        //ajax calling to insert  custom data function
                                        $.ajax({
                                            type: "POST",
                                            data: JSON.stringify(savecustomdata),
                                            url: "/api/Realty/Insertcontactdetail",
                                            contentType: "application/json",
                                            success: function (opdata) {
                                                //////////////

                                                if (opdata.status == "1") {
                                                    //var selectedproperty = $('#describe').val();
                                                    //$('#PopupForm').find("[id='retURL']").val($('#PopupForm').find("[id='retURL']").val() + "?prop=" + selectedproperty);
                                                    //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                                    //window.location.href = $('#PopupForm').find("[id='retURL']").val();
                                                    //$('#PopupForm').submit();
                                                    var noData = "";
                                                    var sourcev = $('#PopupForm').find("[id='source']").val();
                                                    var customdata = {
                                                        name: fname,
                                                        email: enuiryemail,
                                                        mobile: enuirymobile,
                                                        countrycode: countryname,
                                                        city: noData,
                                                        source: sourcev,
                                                        keyword: noData,
                                                        channel: noData,
                                                        campaign: noData,
                                                        placement: noData,
                                                        device: noData,
                                                        sale_type: SalesSource,
                                                        description: noData
                                                    };

                                                    $.ajax({
                                                        type: "POST",
                                                        data: JSON.stringify(customdata),
                                                        url: "https://wzpocatg.in/api.php",
                                                        contentType: "application/json",
                                                        success: function (data) {

                                                            console.log("success");
                                                        },

                                                        error: function (jqXhr, textStatus, errorMessage) {
                                                            console.log("operation Failed!!! jqXhr=" + jqXhr + "&textStatus=" + textStatus + "&errorMessage=" + errorMessage);
                                                        }

                                                    });
                                                    //form.submit();


                                                    window.location.href = "https://www.adanirealty.com/the-meadows-thankyou";

                                                }
                                                else {
                                                    alert("Sorry Operation Failed!!! Please try again later");
                                                    $('#btnCodegreenEnquirySubmit').removeAttr("disabled");
                                                    return false;
                                                }
                                            }
                                        });
                                    }


                                    else {
                                        alert("Invalid OTP");
                                        $('#PopupForm').find(':submit').prop('disabled', false);
                                        //$('#btnCodegreenEnquirySubmit').removeAttr("disabled");
                                        return false;
                                    }
                                }
                            });

                        }
                    }
                    else if (data == "-1") {
                        alert("Invalid Mobile Number");
                        $('#PopupForm').find(':submit').prop('disabled', false);
                    }
                }
            });


        }
    });
}

if ($('#pricepopup').length > 0) {
    priceValidate = $('#pricepopup').validate({
        rules: {
            name: {
                required: true,
                alphabets: true,
                maxlength: 100
            },
            CountryCode: {
                required: true
            },
            mobile: {
                required: true,
                number: true,
                mobile: true,
                minlength: 10,
                maxlength: 10
            },
            email: {
                required: true,
                email: true
            }
            // captchatxt3: {
            //   required: true,
            //   captcha:true
            // }
            /*comment: {
                required: true,
            }*/
        },
        submitHandler: function (form) {
            $(form).find(':submit').prop('disabled', true);
            var fname = $('#pricepopup').find("[name='name']").val();
            var enuiryemail = $('#pricepopup').find("[name='email']").val();
            var enuirymobile = $('#pricepopup').find("[name='mobile']").val();
            var SalesSource = $('#pricepopup').find("[name='sales_source']").val();
            var budget = 0.0;
            var remarks = "";
            var country = $('#pricepopup').find("[name='CountryCode']").val();
            var model = {
                mobile: enuirymobile,
                first_name: fname,
                // last_name: lastname,
                email: enuiryemail

            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(model),
                url: "/api/Realty/Codegreen_Enquiry",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == "1") {
                        var otp = prompt("Please enter OTP received on your mobile", "");

                        if (otp != null) {

                            var generatedOtp = {
                                mobile: enuirymobile,
                                OTP: otp,
                            }
                            $.ajax({
                                type: "POST",
                                data: JSON.stringify(generatedOtp),
                                url: "/api/Realty/VerifyOTP",
                                contentType: "application/json",
                                success: function (data) {
                                    if (data.status == "1") {
                                        //create json object

                                        var countryname = $('#pricepopup').find("[name='CountryCode']").val();
                                        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                        var formtype = $('#pricepopup').find("[id='FormType']").val();
                                        var pageinfo = window.location.href;
                                        var propertytype = "The Meadows";
                                        var propertilocation = "";
                                        var SalesSource = $('#pricepopup').find("[name='sales_source']").val();
                                        var utmSource = $('#pricepopup').find("[id='utm_source']").val();


                                        var savecustomdata = {
                                            first_name: fname,
                                            last_name: fname,
                                            mobile: enuirymobile,
                                            email: enuiryemail,
                                            Budget: budget,
                                            country_code: countryname,
                                            //state_code: statename,
                                            Projects_Interested__c: propertytype,
                                            PropertyLocation: propertilocation,
                                            sale_type: SalesSource,
                                            Remarks: remarks,
                                            FormType: formtype,
                                            PageInfo: pageinfo,
                                            FormSubmitOn: currentdate,
                                            UTMSource: utmSource,
                                            RecordType: "0122v000001uNyyAAE",
                                            PropertyCode: "a4F2v000000IEFN"
                                        };

                                        //ajax calling to insert  custom data function
                                        $.ajax({
                                            type: "POST",
                                            data: JSON.stringify(savecustomdata),
                                            url: "/api/Realty/Insertcontactdetail",
                                            contentType: "application/json",
                                            success: function (opdata) {
                                                //////////////

                                                if (opdata.status == "1") {
                                                    //var selectedproperty = $('#describe').val();
                                                    //$('#pricepopup').find("[id='retURL']").val($('#pricepopup').find("[id='retURL']").val() + "?prop=" + selectedproperty);
                                                    //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                                    //window.location.href = $('#pricepopup').find("[id='retURL']").val();
                                                    //$('#pricepopup').submit();
                                                    var noData = "";
                                                    var sourcev = $('#pricepopup').find("[id='source']").val();
                                                    var customdata = {
                                                        name: fname,
                                                        email: enuiryemail,
                                                        mobile: enuirymobile,
                                                        countrycode: countryname,
                                                        city: noData,
                                                        source: sourcev,
                                                        keyword: noData,
                                                        channel: noData,
                                                        campaign: noData,
                                                        placement: noData,
                                                        device: noData,
                                                        sale_type: SalesSource,
                                                        description: noData
                                                    };

                                                    $.ajax({
                                                        type: "POST",
                                                        data: JSON.stringify(customdata),
                                                        url: "https://wzpocatg.in/api.php",
                                                        contentType: "application/json",
                                                        success: function (data) {

                                                            console.log("success");
                                                        },

                                                        error: function (jqXhr, textStatus, errorMessage) {
                                                            console.log("operation Failed!!! jqXhr=" + jqXhr + "&textStatus=" + textStatus + "&errorMessage=" + errorMessage);
                                                        }

                                                    });
                                                    window.location.href = "https://www.adanirealty.com/the-meadows-thankyou";
                                                }
                                                else {
                                                    alert("Sorry Operation Failed!!! Please try again later");
                                                    $('#pricepopup').find(':submit').prop('disabled', false);
                                                    return false;
                                                }
                                            }
                                        });
                                    }


                                    else {
                                        alert("Invalid OTP");
                                        $('#pricepopup').find(':submit').prop('disabled', false);
                                        //$('#btnCodegreenEnquirySubmit').removeAttr("disabled");
                                        return false;
                                    }
                                }
                            });

                        }
                    }
                    else if (data == "-1") {
                        alert("Invalid Mobile Number");
                        $('#pricepopup').find(':submit').prop('disabled', false);
                    }
                }
            });


        }

    });
}

if ($('#InstantCallback').length > 0) {
    instantValidate = $('#InstantCallback').validate({
        rules: {
            name: {
                required: true,
                alphabets: true,
                maxlength: 100
            },
            CountryCode: {
                required: true
            },
            mobile: {
                required: true,
                number: true,
                mobile: true,
                minlength: 10,
                maxlength: 10
            },
            email: {
                required: true,
                email: true
            }
            // captchatxt: {
            //     required: true,
            //     captcha:true
            // }
            /*comment: {
                required: true,
            }*/
        },
        submitHandler: function (form) {
            $(form).find(':submit').prop('disabled', true);
            var fname = $('#InstantCallback').find("[name='name']").val();
            var enuiryemail = $('#InstantCallback').find("[name='email']").val();
            var enuirymobile = $('#InstantCallback').find("[name='mobile']").val();
            var SalesSource = $('#InstantCallback').find("[name='sales_source']").val();
            var budget = 0.0;
            var remarks = "";
            var country = $('#InstantCallback').find("[name='CountryCode']").val();
            var model = {
                mobile: enuirymobile,
                first_name: fname,
                // last_name: lastname,
                email: enuiryemail

            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(model),
                url: "/api/Realty/Codegreen_Enquiry",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == "1") {
                        var otp = prompt("Please enter OTP received on your mobile", "");

                        if (otp != null) {

                            var generatedOtp = {
                                mobile: enuirymobile,
                                OTP: otp,
                            }
                            $.ajax({
                                type: "POST",
                                data: JSON.stringify(generatedOtp),
                                url: "/api/Realty/VerifyOTP",
                                contentType: "application/json",
                                success: function (data) {
                                    if (data.status == "1") {
                                        //create json object
                                        var countryname = $('#InstantCallback').find("[name='CountryCode']").val();
                                        //$('#country :selected').text();
                                        // var statename = $('#state :selected').text();
                                        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                        var formtype = $('#InstantCallback').find("[id='FormType']").val();
                                        var pageinfo = window.location.href;
                                        // ($('#PageInfo').val()).replace("/", "");
                                        var propertytype = "The Meadows";
                                        var propertilocation = "";
                                        var SalesSource = $('#InstantCallback').find("[name='sales_source']").val();
                                        var utmSource = $('#InstantCallback').find("[id='utm_source']").val();
                                        //if (pageinfo == "" || pageinfo == "/") { pageinfo = "Home"; }


                                        var savecustomdata = {
                                            first_name: fname,
                                            last_name: fname,
                                            mobile: enuirymobile,
                                            email: enuiryemail,
                                            Budget: budget,
                                            country_code: countryname,
                                            //state_code: statename,
                                            Projects_Interested__c: propertytype,
                                            PropertyLocation: propertilocation,
                                            sale_type: SalesSource,
                                            Remarks: remarks,
                                            FormType: formtype,
                                            PageInfo: pageinfo,
                                            FormSubmitOn: currentdate,
                                            UTMSource: utmSource,
                                            RecordType: "0122v000001uNyyAAE",
                                            PropertyCode: "a4F2v000000IEFN"
                                        };

                                        //ajax calling to insert  custom data function
                                        $.ajax({
                                            type: "POST",
                                            data: JSON.stringify(savecustomdata),
                                            url: "/api/Realty/Insertcontactdetail",
                                            contentType: "application/json",
                                            success: function (opdata) {
                                                //////////////

                                                if (opdata.status == "1") {
                                                    //var selectedproperty = $('#describe').val();
                                                    //$('#InstantCallback').find("[id='retURL']").val($('#InstantCallback').find("[id='retURL']").val() + "?prop=" + selectedproperty);
                                                    //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                                    //window.location.href = $('#InstantCallback').find("[id='retURL']").val();
                                                    //$('#InstantCallback').submit();
                                                    var noData = "";
                                                    var sourcev = $('#InstantCallback').find("[id='source']").val();
                                                    var customdata = {
                                                        name: fname,
                                                        email: enuiryemail,
                                                        mobile: enuirymobile,
                                                        countrycode: countryname,
                                                        city: noData,
                                                        source: sourcev,
                                                        keyword: noData,
                                                        channel: noData,
                                                        campaign: noData,
                                                        placement: noData,
                                                        device: noData,
                                                        sale_type: SalesSource,
                                                        description: noData
                                                    };

                                                    $.ajax({
                                                        type: "POST",
                                                        data: JSON.stringify(customdata),
                                                        url: "https://wzpocatg.in/api.php",
                                                        contentType: "application/json",
                                                        success: function (data) {

                                                            console.log("success");
                                                        },

                                                        error: function (jqXhr, textStatus, errorMessage) {
                                                            console.log("operation Failed!!! jqXhr=" + jqXhr + "&textStatus=" + textStatus + "&errorMessage=" + errorMessage);
                                                        }

                                                    });
                                                    window.location.href = "https://www.adanirealty.com/the-meadows-thankyou";

                                                }
                                                else {
                                                    alert("Sorry Operation Failed!!! Please try again later");
                                                    $('#InstantCallback').find(':submit').prop('disabled', false);
                                                    return false;
                                                }
                                            }
                                        });
                                    }


                                    else {
                                        alert("Invalid OTP");
                                        $('#InstantCallback').find(':submit').prop('disabled', false);
                                        //$('#btnCodegreenEnquirySubmit').removeAttr("disabled");
                                        return false;
                                    }
                                }
                            });

                        }
                    }
                    else if (data == "-1") {
                        alert("Invalid Mobile Number");
                        $('#InstantCallback').find(':submit').prop('disabled', false);
                    }
                }
            });


        }

    });
}

if ($('#inquiryForm').length > 0) {
    $('#inquiryForm').validate({
        rules: {
            name: {
                required: true,
                alphabets: true,
                maxlength: 100
            },
            CountryCode: {
                required: true
            },
            mobile: {
                required: true,
                number: true,
                mobile: true,
                minlength: 10,
                maxlength: 10
            },
            email: {
                required: true,
                email: true
            }
            // captchatxt: {
            //   required: true,
            //  captcha:true
            // }

        },
        submitHandler: function (form) {
            $(form).find(':submit').prop('disabled', true);
            var fname = $('#inquiryForm').find("[name='name']").val();
            var enuiryemail = $('#inquiryForm').find("[name='email']").val();
            var SalesSource = $('#inquiryForm').find("[name='sales_source']").val();
            var enuirymobile = $('#inquiryForm').find("[name='mobile']").val();
            var budget = 0.0;
            var remarks = "";
            var country = $('#inquiryForm').find("[name='CountryCode']").val();
            var model = {
                mobile: enuirymobile,
                first_name: fname,
                // last_name: lastname,
                email: enuiryemail

            };
            $.ajax({
                type: "POST",
                data: JSON.stringify(model),
                url: "/api/Realty/Codegreen_Enquiry",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == "1") {
                        var otp = prompt("Please enter OTP received on your mobile", "");

                        if (otp != null) {

                            var generatedOtp = {
                                mobile: enuirymobile,
                                OTP: otp,
                            }
                            $.ajax({
                                type: "POST",
                                data: JSON.stringify(generatedOtp),
                                url: "/api/Realty/VerifyOTP",
                                contentType: "application/json",
                                success: function (data) {
                                    if (data.status == "1") {
                                        //create json object
                                        var countryname = $('#inquiryForm').find("[name='CountryCode']").val();
                                        //$('#country :selected').text();
                                        // var statename = $('#state :selected').text();
                                        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
                                        var formtype = $('#inquiryForm').find("[id='FormType']").val();
                                        var pageinfo = window.location.href;
                                        // ($('#PageInfo').val()).replace("/", "");
                                        var propertytype = "The Meadows";
                                        var propertilocation = "";
                                        var SalesSource = $('#inquiryForm').find("[name='sales_source']").val();
                                        var utmSource = $('#inquiryForm').find("[id='utm_source']").val();
                                        //if (pageinfo == "" || pageinfo == "/") { pageinfo = "Home"; }


                                        var savecustomdata = {
                                            first_name: fname,
                                            last_name: fname,
                                            mobile: enuirymobile,
                                            email: enuiryemail,
                                            Budget: budget,
                                            country_code: countryname,
                                            //state_code: statename,
                                            Projects_Interested__c: propertytype,
                                            PropertyLocation: propertilocation,
                                            sale_type: SalesSource,
                                            Remarks: remarks,
                                            FormType: formtype,
                                            PageInfo: pageinfo,
                                            FormSubmitOn: currentdate,
                                            UTMSource: utmSource,
                                            RecordType: "0122v000001uNyyAAE",
                                            PropertyCode: "a4F2v000000IEFN"
                                        };

                                        //ajax calling to insert  custom data function
                                        $.ajax({
                                            type: "POST",
                                            data: JSON.stringify(savecustomdata),
                                            url: "/api/Realty/Insertcontactdetail",
                                            contentType: "application/json",
                                            success: function (opdata) {
                                                //////////////

                                                if (opdata.status == "1") {
                                                    //var selectedproperty = $('#describe').val();
                                                    //$('#inquiryForm').find("[id='retURL']").val($('#inquiryForm').find("[id='retURL']").val() + "?prop=" + selectedproperty);
                                                    //$('#retURL').val($('#retURL').val() + "?prop=" + selectedproperty);
                                                    //window.location.href = $('#inquiryForm').find("[id='retURL']").val();
                                                    //$('#inquiryForm').submit();
                                                    var noData = "";
                                                    var sourcev = $('#inquiryForm').find("[id='source']").val();
                                                    var customdata = {
                                                        name: fname,
                                                        email: enuiryemail,
                                                        mobile: enuirymobile,
                                                        countrycode: countryname,
                                                        city: noData,
                                                        source: sourcev,
                                                        keyword: noData,
                                                        channel: noData,
                                                        campaign: noData,
                                                        placement: noData,
                                                        device: noData,
                                                        sale_type: SalesSource,
                                                        description: noData
                                                    };

                                                    $.ajax({
                                                        type: "POST",
                                                        data: JSON.stringify(customdata),
                                                        url: "https://wzpocatg.in/api.php",
                                                        contentType: "application/json",
                                                        success: function (data) {

                                                            console.log("success");
                                                        },

                                                        error: function (jqXhr, textStatus, errorMessage) {
                                                            console.log("operation Failed!!! jqXhr=" + jqXhr + "&textStatus=" + textStatus + "&errorMessage=" + errorMessage);
                                                        }

                                                    });
                                                    window.location.href = "https://www.adanirealty.com/the-meadows-thankyou";
                                                }
                                                else {
                                                    alert("Sorry Operation Failed!!! Please try again later");
                                                    $('#inquiryForm').find(':submit').prop('disabled', false);
                                                    return false;
                                                }
                                            }
                                        });
                                    }


                                    else {
                                        alert("Invalid OTP");
                                        $('#inquiryForm').find(':submit').prop('disabled', false);
                                        //$('#btnCodegreenEnquirySubmit').removeAttr("disabled");
                                        return false;
                                    }
                                }
                            });

                        }
                    }
                    else if (data == "-1") {
                        alert("Invalid Mobile Number");
                        $('#inquiryForm').find(':submit').prop('disabled', false);
                    }
                }
            });


        }

    });
}

/*popup js starts here*/
$(window).load(function () {
    if (!Get_Cookie('popout')) {

        if ($(window).width() > 550) {
            window.setTimeout(function () {
                $('#popupModal').modal({

                });
            }, 3000);
        }
    }
});

$('#popupModal .close').click(function () {
    Set_Cookie('popout', 'it works');
});
$('#popupModal').on('hide.bs.modal', function () {
    Set_Cookie('popout', 'it works');
});

$('.pricepop').click(function () {
    var pricePopup = $('#price');
    pricePopup.find('input[name=source]').val('Price Popup');
    pricePopup.find('strong').html('Please enter the details below to get the detailed pricing information.');
    pricePopup.modal();
    $('#price').on('hidden.bs.modal', function () {
        $(this).find('.has-error').removeClass('has-error');
        priceValidate.resetForm();
    });
});

$('.inquireButton').click(function () {
    var inquirePopup = $('#price');
    inquirePopup.find('input[name=source]').val('Inquiry Form - Mobile');
    inquirePopup.find('strong').html('Enter your details for project information.');
    inquirePopup.modal();
});

/*popup js ends here*/

jQuery(function ($) {
    $(document).ready(function () {
        "use strict";
        var instantFlag = false;
        var hotlineFlag = false;
        $("#instant-callback-div .instant-switch").click(function () {
            $("#instant-callback-div").animate({
                "right": $("#instant-callback-div").css('right') == "-1px" ? "-247px" : "-1px"
            }, 500);
            instantFlag = true;
            if (hotlineFlag) {
                $("#hotline-div").animate({
                    "right": "-277px"
                }, 500);
                hotlineFlag = false;
            }
        });
        $("#hide").click(function () {
            $("#instant-callback-div").animate({
                "right": "-247px"
            }, 500);
            instantFlag = false;
            $('#InstantCallback').find('.has-error').removeClass('has-error');
            instantValidate.resetForm();
        });

        $("#hotline-div .hotline-switch").click(function () {
            $("#hotline-div").animate({
                "right": "-1px"
            }, 500);
            hotlineFlag = true;
            if (instantFlag) {
                $("#instant-callback-div").animate({
                    "right": "-246px"
                }, 500);
                instantFlag = false;
            }
        });
        $("#hide-hotline").click(function () {
            $("#hotline-div").animate({
                "right": "-245px"
            }, 500);
            hotlineFlag = false;
        });
    });
});
//(function(){
//    $('.icon').on('mouseenter',function(){
//        console.log('in');
//    });
//})();
