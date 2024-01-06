function onEnv3Business(val) {
    ShowHideDoc();
}

function onEnv3Category(val) {
    ShowHideDoc();
}

function ShowHideDoc() {
    var businessValue = $("#Business").val();
    var categoryValue = $("#Category").val();

    $(".env3docs").hide();
    $(".env2docsMan").hide();

    if (businessValue == "1") {
        if (categoryValue == "1") {

            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManTypeTestReports").hide();
            $("#ManOtherDocuments").hide();
            $("#ManDrawingsandGeneralArrangement").hide();

        }
        else if (categoryValue == "2") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "3") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManTypeTestReports").hide();
            $("#ManOtherDocuments").hide();
            $("#ManDrawingsandGeneralArrangement").hide();

        }
    }
    else if (businessValue == "2") {
        if (categoryValue == "1") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "2") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "3") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
    }
    else if (businessValue == "3") {
        if (categoryValue == "1") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "2") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
        else if (categoryValue == "3") {
            $("#divQualificationRequirementDocuments").show();
            $("#divTechnicalSpecificationScopeofWork").show();
            $("#divTypeTestReports").show();
            $("#divDrawingsandGeneralArrangement").show();
            $("#divDeliverySchedule").show();
            $("#divDeviationsSheet").show();
            $("#divDulyStampedandSignTenderdocuments").show();
            $("#divOtherDocuments").show();

            $(".env2docsMan").show();
            $("#ManOtherDocuments").hide();
        }
    }
}

$(document).ready(function () {

    ShowHideDoc();

    $(function () {
        $('#datetimepickerTenderAdvDate').datetimepicker(
            { format: 'DD/MM/YYYY' });
    });
    $(function () {
        $('#datetimepickerTenderClosingDate').datetimepicker(
            { format: 'DD/MM/YYYY' });
    });

    $(function () {
        $('#EMDFeeDate').datetimepicker(
            { format: 'DD/MM/YYYY' });
    });
    $(function () {
        $('#TenderFeeDate').datetimepicker(
            { format: 'DD/MM/YYYY' });
    });

    $("#emiOption").change(function () {
        if (this.checked) {
            $("#divEmiInstallment").show();
            $("#divAdvanceAmount").hide();
            $("#amountPayable").attr('readonly', 'readonly');
        }
        else {
            $("#divEmiInstallment").hide();
            $("#divAdvanceAmount").show();
            if ($("#amountPayable").val() > 0) {
                $("#amountPayable").removeAttr('readonly');
            }
        }
    });

    $("#frmeBillAmendmentDetails").submit(function () {
        $(".validationmessage").text("");
        return true;
    });

    $("#loader-wrapper").hide();
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {
        $('#message_modal').modal('show');
        $("#message").val("");
    }

    if (localStorage['page'] == document.URL && localStorage['scrollTop'] != null && localStorage['scrollTop'] != "") {
        $(document).scrollTop(localStorage['scrollTop']);
    }
});

$("#reset").click(function () {
    window.location.reload();
});

function GetScrollPosition() {
    localStorage['page'] = document.URL;
    localStorage['scrollTop'] = $(document).scrollTop();
}

var clicked = false;
function CheckBrowser() {
    if (clicked == false) {
        //Browser closed
    }
    else {
        //redirected 
        clicked = false;
    }
}

function bodyUnload() {
    if (clicked == false)//browser is closed
    {
        var request = GetRequest();
        request.open("POST", "/api/Accounts/LogoutSessionOnTabclose", false);
        request.send();
    }
}

function GetRequest() {
    var request = null;
    if (window.XMLHttpRequest) {
        //incase of IE7,FF, Opera and Safari browser
        request = new XMLHttpRequest();
    }
    else {
        //for old browser like IE 6.x and IE 5.x
        request = new ActiveXObject('MSXML2.XMLHTTP.3.0');
    }
    return request;
}
function Abandon(e) {
    jQuery.ajax(
        {
            url: "/api/Accounts/LogoutSessionOnTabclose",
            method: "POST",
            async: true,
            success: function (data) {
                //e.cancelBubble is supported by IE - this will kill the bubbling process.
                e.cancelBubble = true;
                e.returnValue = leave_message;
                //e.stopPropagation works in Firefox.
                if (e.stopPropagation) {
                    e.stopPropagation();
                    e.preventDefault();
                }
                //return works for Chrome and Safari
                return leave_message;
            }
        });
}

function IsSuccessCity() {
    jQuery.ajax(
        {
            url: "/api/Accounts/City_Callback_Check",
            method: "POST",
            success: function (data) {
                if (data !== "") {
                    myStopFunction();
                    location.href = data;
                }
            }
        });
}


function IsSuccess() {
    jQuery.ajax(
        {
            url: "/api/Accounts/Benow_Callback",
            method: "POST",
            success: function (data) {
                if (data !== "") {
                    myStopFunction();
                    location.href = data;
                }
            }
        });
}

function IsSuccessCity() {
    jQuery.ajax(
        {
            url: "/api/Accounts/City_Callback_Check",
            method: "POST",
            success: function (data) {
                if (data !== "") {
                    myStopFunction();
                    location.href = data;
                }
            }
        });
}

function IsSuccessDBS() {
    jQuery.ajax(
        {
            url: "/api/Accounts/IsSuccessDBS_Callback",
            method: "POST",
            success: function (data) {
                if (data !== "") {
                    myStopFunction();
                    location.href = data;
                }
            }
        });
}

setTimeout(myStopFunction, 300000);


function myStopFunction() {
    clearInterval(myVar);
}

$(document).ready(function () {
    $('#Communities').click(function () {
        $('html, body').animate({
            scrollTop: $("#communitiesSec").offset().top
                - 130
        }, 1000);
    });



    var owl = $('#serviceCarousels');
    owl.owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        dots: true,
        autoplay: true,
        navRewind: false,
        responsive: {
            0: {
                items: 1,
                //nav: false,
                //dots: true
            },
            600: {
                items: 1,
                //nav: false,
                //dots: true
            },
            768: {
                items: 2,
                //nav: false,
                //dots: true
            },
            1000: {
                items: 3
            }
        }
    });

    var owl2 = $('#homeslider');
    owl2.owlCarousel({
        loop: true,
        autoplay: true,
        margin: 10,
        nav: true,
        autoHeight: false,
        dots: true,
        navRewind: false,
        autoplayTimeout: 10000,
        lazyLoad: false,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    });

    $('.asg-milestones').owlCarousel({
        loop: true,
        margin: 30,
        responsiveClass: true,
        autoplayTimeout: 2000,
        autoplay: true,
        nav: false,
        dots: true,
        autoplayHoverPause: true,
        autoPlay: 2500,
        slideSpeed: 2000,
        smartSpeed: 1500,
        paginationSpeed: 2000,
        navText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    });

    ///* Initialize Banner Carousel on home page */
    //$("#bannerCarousel").owlCarousel({
    //    autoplay:true,
    //    //autoplayTimeout:2000,
    //    //autoplayHoverPause:true,
    //    center: true,
    //    loop:true,
    //    items: 1,
    //    dots: false,
    //    pagination : true,
    //}); 

    /* Initialize Carousel  on energy calculator*/
    $("#energyCarousel").owlCarousel({
        nav: true,
        navText: [],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 2
            },
            // breakpoint from 480 up
            480: {
                items: 3
            },
            // breakpoint from 768 up
            768: {
                items: 3
            },
            // breakpoint from 992 up
            992: {
                items: 4
            },
            // breakpoint from 1025 up
            1025: {
                items: 5
            }
        }
    });

    /* Initialize Carousel on home page */
    //$(".owl-carousel").owlCarousel({
    //    nav: true,
    //    navText: [],
    //    responsive: {
    //        // breakpoint from 0 up
    //        0: {
    //            items: 1
    //        },
    //        // breakpoint from 480 up
    //        480: {
    //            items: 2
    //        },
    //        // breakpoint from 768 up
    //        768: {
    //            items: 3,
    //            nav: true,
    //            loop: false,
    //            touchDrag: false,
    //            mouseDrag: false
    //        },
    //        1000: {
    //            nav: true,
    //            loop: false,
    //            touchDrag: false,
    //            mouseDrag: false
    //        }
    //    }
    //});

    /* Menu on mobile devices */

    $('#dismiss, .overlay, .overlay-top').on('click', function () {
        $('#sideNav').removeClass('active');
        $('.overlay, .overlay-top').removeClass('active');
        $('#topMenu').removeClass('active');
    });

    /*Side menu on mobile devices*/

    $('#sidebarCollapse').on('click', function () {
        $('#sideNav, #mainContent').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('.overlay').addClass('active');
        //$('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    /* Top menu on mobile devices*/

    $('#topNavCollapse').on('click', function () {
        $('#topMenu').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('.overlay-top').addClass('active');

    });

    ///* For smooth page scroll on Top menu link */
    //document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    //    anchor.addEventListener('click', function (e) {
    //        e.preventDefault();

    //        document.querySelector(this.getAttribute('href')).scrollIntoView({
    //            behavior: 'smooth'
    //        });
    //    });
    //});

    ///* to reduce the bottom space in between two control, if control have  minBottomSpace class*/
    //$('.minBottomSpace').closest(".pageContent").css("min-height", "100px")
});

$(document).on("click", ".txt-orange", function () {
    Id = $(this).data('id');
    $("#TenderID").val(Id);
});


$(document).on("click", "#download", function () {
    var id = $(this).data('id');
    jQuery.ajax(
        {
            url: "/api/Electricity/GetTenderDocument",
            method: "GET",
            data: {
                Id: id
            },
            success: function (data) {
                $("#downloadModal").html(data);
                $('#downloadModal').modal('show');
            }
        });


})

//for career page popup
$(window).on('load', function () {
    $('#ac-wrapper').show();
});
$("#closeopop").on("click", function () {
    $('#ac-wrapper').hide();
});


$(document).ready(function () {

    if (window.location.href.indexOf("#communitiesSec") > 0) {

        $('html, body').animate({
            scrollTop: $("#communitiesSec").offset().top
                - 130
        }, 1000);
    }

    $(window).on('load', function () {
        if ($('#homemodalpopup').length) {
            $('#homemodalpopup').modal('show');
        }
    });

});

function Validate(obj) {
    var format = /[!@&#$%^*()+\=\[\]{};':"\\|,<>\/?]+/;

    if (format.test($(obj).val().replace(/^.*[\\\/]/, ''))) {
        alert('File name should not contain any special characters!');
        $(obj).val("");
        return false;
    }
    var ext = $(obj).val().split('.').pop().toLowerCase();
    // if($.inArray(ext, ['jpg','jpeg','png','pdf','doc','docx','xls','xlsx','.zip']) == -1) {
    // alert('invalid extension!');
    // }
    if ($.inArray(ext, ['jpg', 'jpeg', 'dwg', 'pdf', 'doc', 'docx', 'xls', 'xlsx']) == -1) {
        alert('File extension not supported!');
        $(obj).val("");
        return false;
    }
}

function ValidateMeterImage(obj) {
    var format = /[!@&#$%^*()+\=\[\]{};':"\\|,<>\/?]+/;

    if (format.test($(obj).val().replace(/^.*[\\\/]/, ''))) {
        alert('File name should not contain any special characters!');
        $(obj).val("");
        return false;
    }
    var ext = $(obj).val().split('.').pop().toLowerCase();
    // if($.inArray(ext, ['jpg','jpeg','png','pdf','doc','docx','xls','xlsx','.zip']) == -1) {
    // alert('invalid extension!');
    // }
    if ($.inArray(ext, ['jpg', 'jpeg', 'png', 'gif']) == -1) {
        alert('File extension not supported!');
        $(obj).val("");
        return false;
    }
}

function validateEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function validateMobileNo(mobile) {
    if (mobile.match(/^[6789]\d{9}$/)) {
        return true;
    }
    else {
        return false;
    }
}

function validateMobile(event, t) {
    var mobile = $("#mobileNumber").val();
    if (validateMobileNo(mobile)) {
        $("#mobileerror").html("");
    }
    else {
        $("#mobileerror").html("Please enter a 10 digit valid mobile number");
    }
}

function validateEmailId(event, t) {
    var emailAddress = $("#emailAddress").val();
    if (validateEmail(emailAddress)) {
        $("#emailerror").html("");
    }
    else {
        $("#emailerror").html("Please enter a valid Email Address");
    }
}



function validateName(name) {
    var regex = /^[a-zA-Z ]+$/;
    return regex.test(name);

}

function onchangeValidateFax(event, t) {
    var fax = $("#fax").val();
    if (fax != null && fax.trim() != "") {
        if (!validateFax(fax)) {
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            //$("#fax").focus();
        }
        else {
            $("#faxerror").html("");
        }
    }
    else {
        $("#faxerror").html("");
    }
}

function validateFax(fax) {
    var regex = /^[0-9]{12,12}$/;
    return regex.test(fax);
}

$(".reset").click(function () {
    $("#nameerror").html("");
    $("#companyerror").html("");
    $("#mobileerror").html("");
    $("#faxerror").html("");
    $("#emailerror").html("");
    $("#name").val("");
    $("#company").val("");
    $("#mobileNumber").val("");
    $("#fax").val("");
    $("#emailAddress").val("");
});

function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode == 32)) {
            $("#nameerror").html("");
            return true;
        }
        else {
            $("#nameerror").html("Please enter a valid name containing alphabets only");
            return false;
        }
    }
    catch (err) {
        alert(err.Description);
    }
}



$("#UseRegistrationBtn").click(function (event) {
    var mobileNo = $("#mobileNumber").val();
    var emailAddress = $("#emailAddress").val();
    var company = $("#company").val();
    var fax = $("#fax").val();
    var name = $("#name").val();
    if (!validateName(name)) {
        event.preventDefault();
        $("#nameerror").html("Please enter a valid name containing alphabets only");
        $("#name").focus();
        return false;
    }
    else {
        $("#nameerror").html("");
    }
    if (company == null || company.trim() == "") {
        event.preventDefault();
        $("#companyerror").html("Please enter a valid company name");
        $("#company").focus();
        return false;
    }
    else {
        $("#companyerror").html("");
    }
    if (!validateMobileNo(mobileNo)) {
        event.preventDefault();
        $("#mobileerror").html("Please enter a 9 digit valid mobile number");
        $("#mobileNumber").focus();
        return false;
    }
    else {
        $("#mobileerror").html("");
    }
    if (fax != null && fax.trim() != "") {
        if (!validateFax(fax)) {
            event.preventDefault();
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            $("#fax").focus();
            return false;
        }
        else {
            $("#faxerror").html("Please enter valid Fax number");
        }
    }
    else {
        $("#faxerror").html("");
    }
    if (!validateEmail(emailAddress)) {
        event.preventDefault();
        $("#emailerror").html("Please enter a valid Email Address");
        $("#emailAddress").focus();
        return false;
    }
    else {
        $("#emailerror").html("");
    }
    // $('#UseRegistrationForm').submit();
    // return true;
});


$('.primaryMenu .dropdown').on('click', function () {
    $(this).toggleClass('active');

});

$(document).ready(function () {

    var YesNO = "";
    $("#frmRegister").submit(function (event) {

        var buttonName = $(document.activeElement).attr('name');
        if (buttonName === "Pay_PaymentGateway") {
            var actualAmount = $("#amount_payable_actual").val();
            var newAmount = $("#amountPayable").val();
            if ($("#emiOption").is(':checked')) {
                if (YesNO !== "1") {
                    var EMIOutstandingAmount = $("#EMIOutstandingAmount").val();
                    var EMIInstallmentAmount = $("#EMIamount").val();
                    $("#modelEMIOutstandingAmount").html($("#EMIOutstandingAmount").val());
                    $("#modelEMIamount").html($("#EMIamount").val());
                    $('#emi_model').modal("show");
                    return false;
                }
                else {
                    return true;
                }
            }
            if (actualAmount > 0 && actualAmount >= 100 && newAmount < 100) {
                $('#partpaymentmessage').html("Minimum Amount payable Value is 100. Please enter valid amount.");
                $('#default_modal').modal("show");
                event.preventDefault();
                return false;
            }

            if (actualAmount > 0 && newAmount !== actualAmount) {
                if (YesNO !== "1") {
                    $('#partpaymentmessage1').html("You are paying an amount which is different from the amount payable, please confirm before proceeding.");
                    $('#default_modal2').modal("show");
                    return false;
                }
                else {
                    return true;
                }

            }
            else {
                return true;
            }
        }
        else {
            return true;
        }

        //event.preventDefault();
        //return false;
    });

    $('.btnYesNO').click(function (e) {
        //alert("btnYesNO");

        if (this.value === '1') {
            YesNO = '1';
            //$("#frmRegister").submit();
            $("#Pay_PaymentGateway").click();
        }
        else {
            $('#default_modal2').modal("hide");
            //$('#amountPayable').focus();
        }
        e.preventDefault();
    });



});
$('.submits').on("click", function () {
    if ($('*[data-sc-field-name="Program"]').val() == "Please select an option") {
        $('*[data-sc-field-name="Program"]').val("");

    }

});
$('*[data-sc-field-name="Program"]').on("change", function () {
    var programs = $(this).val();
    if (programs == "Adani Electricity Executive") {
        $('.Details').css('display', 'flex');
    }
    else {
        $('.Details').css('display', 'none');
    }

});


jQuery(function () {
    jQuery('.showsection').click(function () {
        jQuery('.targetDiv').hide();
        jQuery('.showsection.current').removeClass('current');
        jQuery($(this).toggleClass('current'));
        jQuery('#div' + $(this).attr('target')).show();
    });
});

$('#tile-1').click(function () {
    $('html, body').animate({
        scrollTop: $("#tile-1").offset().top
    }, 1000);
});

$('#tile-2').click(function () {
    $('html, body').animate({
        scrollTop: $("#tile-2").offset().top
    }, 1000);
});

$('#tile-3').click(function () {
    $('html, body').animate({
        scrollTop: $("#tile-3").offset().top
    }, 1000);
});


// Hide Header on scroll down
var didScroll;
var lastScrollTop = 0;
var delta = 0;
var navbarHeight = 0;

$(window).scroll(function (event) {
    didScroll = true;
});

setInterval(function () {
    if (didScroll) {
        hasScrolled();
        didScroll = false;
    }
}, 250);
function hasScrolled() {
    var st = $(this).scrollTop();

    // Make sure they scroll more than delta
    if (Math.abs(lastScrollTop - st) <= delta)
        return;

    // If they scrolled down and are past the navbar, add class .nav-up.
    // This is necessary so you never see what is "behind" the navbar.
    $('#back-to-top').fadeIn();
    $('header').addClass('fixed-header');

    if (st > lastScrollTop && st > navbarHeight) {
        // Scroll Down
        //$('.btm-floating').addClass('active');

    } else {
        // Scroll Up
        if (st + $(window).height() < $(document).height()) {

        }
    }
    if (st < 150) {

        $('#back-to-top').hide();
        $('header').removeClass('fixed-header');
        //$('.btm-floating').removeClass('active');
    }
    lastScrollTop = st;
}


$(window).scroll(function () {
    if ($(this).scrollTop() > 750) {
        $('#ymPluginDivContainerInitial').addClass('active');
    } else {
        $('#ymPluginDivContainerInitial').removeClass('active');
    }
});