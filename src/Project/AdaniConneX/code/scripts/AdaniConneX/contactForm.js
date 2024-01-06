window.addEventListener("beforeunload", function () {
    document.getElementById("ContactUsFormTag").reset();
    document.getElementById("GetInTouchFormTag").reset();
    document.getElementById("WhitePaperFormTag").reset();
    document.getElementById("JoinUSFormTag").reset();
    document.getElementById("mobileContactUsFormTag").reset();
    $('input').removeClass('error');
    grecaptcha.reset(GetInTouchrecaptcha);
    grecaptcha.reset(JoinUsrecaptcha);
    grecaptcha.reset(WhitePaperrecaptcha);
    grecaptcha.reset(mobileContactUsrecaptcha);
    grecaptcha.reset();
}, false);

window.addEventListener("pageshow", () => {
    document.getElementById("ContactUsFormTag").reset();
    document.getElementById("GetInTouchFormTag").reset();
    document.getElementById("WhitePaperFormTag").reset();
    document.getElementById("JoinUSFormTag").reset();
    document.getElementById("mobileContactUsFormTag").reset();
    $('input').removeClass('error');
    grecaptcha.reset(GetInTouchrecaptcha);
    grecaptcha.reset(WhitePaperrecaptcha);
    grecaptcha.reset(JoinUsrecaptcha);
    grecaptcha.reset(mobileContactUsrecaptcha);
    grecaptcha.reset();
});

// $(window).scroll(function() {
// var myString = window.location.href.substring(window.location.href.lastIndexOf('#') + 1);
// var element = document.getElementById(myString);
// element.classList.add("active-section");

// });

$(document).ready(function () {
    console.log("ready!");
	
	/*home page pop up start*/
	var path = window.location.pathname;
    var page = path.split("/").pop();
	if (page == "" || page == " ")
	{
      $('#pressPopup').addClass('show');   
    }
	/*home page pop up ends*/
			
    $(".tab-accordion-wrapper").find(".acc-title").click(function () {
        debugger;
        if ($(this).attr("title") == "meme_contest") {
            if (!$(this).hasClass('active')) {
                $(this).addClass("active");
                $(this).parents(".tab-title-wrp").find("a[title='general']").removeClass("active")
                $($(".acc-wrp")[0]).removeClass("active");
                $($(".acc-wrp")[0]).find(".acc-title").removeClass("active");
                $($(".acc-wrp")[1]).addClass("active");
                $($(".acc-wrp")[1]).find(".tab-acc-content").css('display', 'block');
                $($(".acc-wrp")[0]).find(".tab-acc-content").css('display', 'none');
            }
        }
        else if ($(this).attr("title") == "general") {
            if (!$(this).hasClass('active')) {
                $(this).addClass("active");
                $(this).parents(".tab-title-wrp").find("a[title='meme_contest']").removeClass("active")
                $($(".acc-wrp")[1]).find(".acc-title").removeClass("active");
                $($(".acc-wrp")[1]).removeClass("active");
                $($(".acc-wrp")[0]).addClass("active");
                $($(".acc-wrp")[0]).find(".tab-acc-content").css('display', 'block');
                $($(".acc-wrp")[1]).find(".tab-acc-content").css('display', 'none');



            }
        }



    });
    $(".tab-accordion-wrapper").find(".tab-title-wrp").find("a").click(function () {
        debugger;
        if ($(this).attr("title") == "meme_contest") {
            if (!$(this).hasClass('active')) {
                $(this).addClass("active");
                $(this).parents(".tab-title-wrp").find("a[title='general']").removeClass("active")
                $($(".acc-wrp")[0]).removeClass("active");
                $($(".acc-wrp")[1]).addClass("active");
                $($(".acc-wrp")[1]).find(".tab-acc-content").css('display', 'block');
                $($(".acc-wrp")[0]).find(".tab-acc-content").css('display', 'none');
            }
        } else if ($(this).attr("title") == "general") {
            if (!$(this).hasClass('active')) {
                $(this).addClass("active");
                $(this).parents(".tab-title-wrp").find("a[title='meme_contest']").removeClass("active")
                $($(".acc-wrp")[1]).removeClass("active");
                $($(".acc-wrp")[0]).addClass("active");
                $($(".acc-wrp")[0]).find(".tab-acc-content").css('display', 'block');
                $($(".acc-wrp")[1]).find(".tab-acc-content").css('display', 'none');
            }
        }
    });
});

var ContactUsrecaptcha;
var GetInTouchrecaptcha;
var WhitePaperrecaptcha;
var mobileContactUsrecaptcha;
var ebook_recaptcha;
var JoinUsrecaptcha;

var onloadCallback = function () {

    //Render the ContactUsrecaptcha on the element with ID "ContactUsrecaptcha"
    if ($('#ContactUsrecaptcha').length) {
        ContactUsrecaptcha = grecaptcha.render('ContactUsrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
        //setTimeout(function(){$('.g-recaptcha-bubble-arrow').parent().addClass('captcha-container');}, 1200);

    }

    //Render the GetInTouchrecaptcha on the element with ID "GetInTouchrecaptcha"
    if ($('#GetInTouchrecaptcha').length) {
        GetInTouchrecaptcha = grecaptcha.render('GetInTouchrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }

    //Render the WhitePaperrecaptcha on the element with ID "WhitePaperrecaptcha"
    if ($('#WhitePaperrecaptcha').length) {
        WhitePaperrecaptcha = grecaptcha.render('WhitePaperrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
    if ($('#mobileContactUsrecaptcha').length) {
        mobileContactUsrecaptcha = grecaptcha.render('mobileContactUsrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }

    if ($('#ebook_recaptcha').length) {
        ebook_recaptcha = grecaptcha.render('ebook_recaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
    //Render the JoinUsrecaptcha on the element with ID "JoinUsrecaptcha"
    if ($('#JoinUsrecaptcha').length) {
        JoinUsrecaptcha = grecaptcha.render('JoinUsrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }

}

// E-book form
$("#ebook_FormSubmit").click(function (e) {
    var newWindow = window.open("", "_blank");
    var response = grecaptcha.getResponse(ebook_recaptcha);
    if (response.length == 0) {
        alert("captcha required.");
        return false;
    }

    if (!$("#ebook_name").val()) {
        $("#ebook_name").addClass("error");
        return false;
    }
    $("#ebook_name").removeClass("error");

    var ebook_namev = new RegExp('^[a-zA-Z ]+$');
    if (!/^[a-zA-Z ]+$/.test($("#ebook_name").val())) {
        $("#ebook_name").addClass("error");
        //$("#contact-error").html('Please enter valid name');
        return false;
    }
    $("#ebook_name").removeClass("error");
    $("#contact-error").html('');

    // ebook email validation
    if (!$("#ebook_email").val().trim()) {
        $("#ebook_email").addClass("error");
        return false;
    }

    var ebook_emailv = new RegExp('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$');
    $("#ebook_email").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#ebook_email").val().trim())) {
        $("#ebook_email").addClass("error");
        //$("#contact-error").html('Please enter valid e-mail address');
        return false;
    }
    $("#ebook_email").removeClass("error");

    // ebook mobile validation
    $("#ebook_mobile").removeClass("error");
    var mobilev = new RegExp('^\d*(?:\.\d{1,2})?$');
    if ($("#ebook_mobile").val().length !== 10) {
        $("#ebook_mobile").addClass("error");
        return false;
    }
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#ebook_mobile").val())) {
        $("#ebook_mobile").addClass("error");
        return false;
    }
    $("#ebook_mobile").removeClass("error");


    // ebook company name validation
    if (!$("#ebook_company").val()) {
        $("#ebook_company").addClass("error");
        return false;
    }
    $("#ebook_company").removeClass("error");
    var ebook_companyv = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#ebook_company").val())) {
        $("#ebook_company").addClass("error");

        return false;
    }
    $("#ebook_company").removeClass("error");


    // ebook country name validation
    if (!$("#ebook_country").val()) {
        $("#ebook_country").addClass("error");
        return false;
    }
    $("#ebook_country").removeClass("error");
    var ebook_companyv = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#ebook_country").val())) {
        $("#ebook_country").addClass("error");

        return false;
    }
    $("#ebook_country").removeClass("error");


    if (!($('#ebook_contact_remember').prop('checked'))) {
        $("#ebook-form-error").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $("#ebook-form-error").html('');
    $("input").removeClass("error");
    $("textarea").removeClass("error");


    var ebook_savecontactdata = {
        SendEmail: $("#sendEmailEbook").val(),
        Name: $("#ebook_name").val(),
        Email: $("#ebook_email").val().trim(),
        Contact: $("#ebook_mobile").val().trim(),
        Company: $("#ebook_company").val(),
        Location: $("#ebook_country").val(),
        FormType: $("#ebook_FormType").val(),
        FormUrl: $(location).attr("href"),
        reResponse: response
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(ebook_savecontactdata),
        url: "/api/AdaniConnex/InsertTakeATourdetail",
        contentType: "application/json",
        success: function (data) {
            if (data.status == 1) {
                $("#ebook_name").val('')
                $("#ebook_email").val('')
                $("#ebook_mobile").val('')
                $("#ebook_company").val('')
                $("#ebook_country").val('')
                $('#ebook_contact_remember').prop('checked', false);

                //$('#download-thankyouPopupClose').toggleClass('show');
                grecaptcha.reset(ebook_recaptcha);



                newWindow.location.href = "https://my.visme.co/view/x43pqjjn-introducing-adaniconnex-ebook?__hstc=248346456.012d2b8b3e7a1f7523435edc3233b772.1654599941166.1654599941166.1654599941166.1&__hssc=248346456.4.1654599941166&__hsfp=1318712316&submissionGuid=cd56f756-2452-4b44-b2c9-ecf3b01d2e80#s1";
                $('#ebook-thankyouPopup').toggleClass('show');
            }
            else if (data.status == 701) {
                $("#ebook_name").addClass("error");
            }

            else if (data.status == 703) {
                $("#ebook_email").addClass("error");

            }
            else if (data.status == 705) {
                $("#ebook_mobile").addClass("error");
            }

            else if (data.status == 707) {
                $("#ebook_company").addClass("error");

            }
            else if (data.status == 706) {
                $("#ebook_country").addClass("error");

            }
            else if (data.status == 708) {
                alert("Captcha Required");

            }
            else if (data.status == 409) {
                alert("Form submitted successfully. Email has not been sent to user ID.");

            }

            // else{
            // $('#ebook-thankyouPopup').toggleClass('show');
            // }

        },

        error: function (data) {
            alert(data.status);
        }
    });

})



$('.menu-closer').click(function () {
    $('.section-holder').removeClass("active-section");
    var idName = this.textContent;

    var element = document.getElementById(idName);
    element.classList.add("active-section");

});



$(".contact-closebtn").click(function () {
    $("#contactUsBtn").click();
});



$("#contactFormSubmit").click(function (e) {

    var response = grecaptcha.getResponse(ContactUsrecaptcha);
    if (response.length == 0) {
        alert("Captcha Required.");
        return false;
    }

    if (!$("#name").val()) {
        $("#name").addClass("error");
        return false;
    }
    $("#name").removeClass("error");

    var namev = new RegExp('^[a-zA-Z ]+$');
    if (!/^[a-zA-Z ]+$/.test($("#name").val())) {
        $("#name").addClass("error");
        //$("#contact-error").html('Please enter valid name');
        return false;
    }
    $("#name").removeClass("error");
    $("#contact-error").html('');

    if (!$("#email").val().trim()) {
        $("#email").addClass("error");
        return false;
    }

    var emailv = new RegExp('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$');
    $("#email").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#email").val().trim())) {
        $("#email").addClass("error");
        //$("#contact-error").html('Please enter valid e-mail address');
        return false;
    }
    $("#email").removeClass("error");
    $("#contact-error").html('');

    if (!$("#mobile").val()) {
        $("#mobile").addClass("error");
        return false;
    }

    $("#mobile").removeClass("error");
    var mobilev = new RegExp('^\d*(?:\.\d{1,2})?$');
    if ($("#mobile").val().length !== 10) {
        $("#mobile").addClass("error");
        //$("#contact-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#mobile").val())) {
        $("#mobile").addClass("error");
        //$("#contact-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    $("#mobile").removeClass("error");
    $("#contact-error").html('');

    if (!$("#message").val()) {
        $("#message").addClass("error");
        return false;
    }

    //company code
    if (!$("#contact_company").val()) {
        $("#contact_company").addClass("error");
        return false;
    }
    $("#contact_company").removeClass("error");
    var Contactcompanyv = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#contact_company").val())) {
        $("#contact_company").addClass("error");

        return false;
    }
    $("#contact_company").removeClass("error");



    $("input").removeClass("error");
    $("textarea").removeClass("error");
    $("#contact-error").html('');

    if (!($('#remember').prop('checked'))) {
        $("#contact-error").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $("#contact-error").html('');
    var savecontactdata = {
        SendEmail: $("#sendEmailContactWeb").val(),
        Name: $("#name").val(),
        Contact: $("#mobile").val(),
        Email: $("#email").val().trim(),
        Message: $("#message").val(),
        Company: $("#contact_company").val(),
        FormType: $("#FormType").val(),
        FormUrl: $(location).attr("href"),
        reResponse: response
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecontactdata),
        url: "/api/AdaniConnex/Insertcontactdetail",
        contentType: "application/json",
        success: function (data) {
            if (data.status == 1) {
                $("#name").val('')
                $("#email").val('')
                $("#mobile").val('')
                $("#message").val('')
                $("#contact_company").val('')
                $('#remember').prop('checked', false);
                $("#contactUsBtn").click();
                $('#getInTouchPopup1').toggleClass('show');
                grecaptcha.reset();
            }
            else if (data.status == 401) {
                $("#name").addClass("error");

            }
            else if (data.status == 406) {
                $("#message").addClass("error");

            }
            else if (data.status == 407) {
                $("#contact_company").addClass("error");

            }
            else if (data.status == 403) {
                $("#email").addClass("error");

            }
            else if (data.status == 405) {
                $("#mobile").addClass("error");
            }
            else if (data.status == 408) {
                alert("Captcha Required");

            }
            else if (data.status == 409) {
                alert("Form submitted successfully. Email has not been sent to user ID.");

            }

        },

        error: function (data) {
            alert(data.status);
        }
    });



})


$("#mobilecontactFormSubmit").click(function (e) {

    var response = grecaptcha.getResponse(mobileContactUsrecaptcha);
    if (response.length == 0) {
        alert("Captcha Required.");
        return false;
    }

    if (!$("#mobilename").val()) {
        $("#mobilename").addClass("error");
        return false;
    }
    $("#mobilename").removeClass("error");

    var namev = new RegExp('^[a-zA-Z ]+$');
    if (!/^[a-zA-Z ]+$/.test($("#mobilename").val())) {
        $("#mobilename").addClass("error");
        //$("#contact-error").html('Please enter valid name');
        return false;
    }
    $("#mobilename").removeClass("error");
    $("#mobilecontact-error").html('');

    if (!$("#mobileemail").val().trim()) {
        $("#mobileemail").addClass("error");
        return false;
    }

    var emailv = new RegExp('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$');
    $("#mobileemail").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#mobileemail").val().trim())) {
        $("#mobileemail").addClass("error");
        //$("#contact-error").html('Please enter valid e-mail address');
        return false;
    }
    $("#mobileemail").removeClass("error");
    $("#mobilecontact-error").html('');

    if (!$("#mobilecontact").val()) {
        $("#mobilecontact").addClass("error");
        return false;
    }

    $("#mobilecontact").removeClass("error");
    var mobilev = new RegExp('^\d*(?:\.\d{1,2})?$');
    if ($("#mobilecontact").val().length !== 10) {
        $("#mobilecontact").addClass("error");
        //$("#contact-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#mobilecontact").val())) {
        $("#mobilecontact").addClass("error");
        //$("#contact-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    $("#mobilecontact").removeClass("error");
    $("#mobilecontact-error").html('');

    if (!$("#mobilemessage").val()) {
        $("#mobilemessage").addClass("error");
        return false;
    }

    //company code
    if (!$("#mobilecontact_company").val()) {
        $("#mobilecontact_company").addClass("error");
        return false;
    }
    $("#mobilecontact_company").removeClass("error");
    var Contactcompanyv = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#mobilecontact_company").val())) {
        $("#mobilecontact_company").addClass("error");

        return false;
    }
    $("#mobilecontact_company").removeClass("error");



    $("input").removeClass("error");
    $("textarea").removeClass("error");
    $("#mobilecontact-error").html('');

    if (!($('#mobileremember').prop('checked'))) {
        $("#mobilecontact-error").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $("#mobilecontact-error").html('');
    var savecontactdata = {
        SendEmail: $("#sendEmailContactMobile").val(),
        Name: $("#mobilename").val(),
        Contact: $("#mobilecontact").val(),
        Email: $("#mobileemail").val().trim(),
        Message: $("#mobilemessage").val(),
        Company: $("#mobilecontact_company").val(),
        FormType: $("#mobileFormType").val(),
        FormUrl: $(location).attr("href"),
        reResponse: response
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecontactdata),
        url: "/api/AdaniConnex/Insertcontactdetail",
        contentType: "application/json",
        success: function (data) {
            if (data.status == 1) {
                $("#mobilename").val('')
                $("#mobileemail").val('')
                $("#mobilecontact").val('')
                $("#mobilemessage").val('')
                $("#mobilecontact_company").val('')
                $('#mobileremember').prop('checked', false);

                $("#mobileContactFormPopupClose").click();
                $('#getInTouchPopup1').toggleClass('show');

                grecaptcha.reset(mobileContactUsrecaptcha);
            }
            else if (data.status == 401) {
                $("#mobilename").addClass("error");

            }
            else if (data.status == 406) {
                $("#mobilemessage").addClass("error");

            }
            else if (data.status == 407) {
                $("#mobilecontact_company").addClass("error");

            }
            else if (data.status == 403) {
                $("#mobileemail").addClass("error");

            }
            else if (data.status == 405) {
                $("#mobilecontact").addClass("error");
            }
            else if (data.status == 408) {
                alert("Captcha Required");

            }
            else if (data.status == 409) {
                alert("Form submitted successfully. Email has not been sent to user ID.");

            }

        },

        error: function (data) {
            alert(data.status);
        }
    });



})

$('.mobileContactFormPopup').click(function () {
    $("#mobileContactFormPopup").toggleClass('show');
    $("body").toggleClass('no-scroll');
});

$('#mobileContactFormPopupClose').click(function () {
    $(this).parent().toggleClass('show');
    $("body").toggleClass('no-scroll');
});

$(document)
    .off('click', '.whitePaperActionnew')
    .on('click', '.whitePaperActionnew', function (e) {
        e.preventDefault();

        var nameType = $(this).attr("data-type");
        $("#formTitle").text(nameType);
        $("#whitePaperPopup").toggleClass('show');
        $("body").toggleClass('no-scroll');
        var url = $(this).attr("data-url");
        $("#data-url").val(url);
        var cardName = $(this).attr("doc-name");
        $("#card-name").val(cardName);
    });



$('#whitePaperPopupClose').click(function () {
    document.getElementById("WhitePaperFormTag").reset();
    $('input').removeClass('error');

    grecaptcha.reset(WhitePaperrecaptcha);

});

$('#contactUsBackdrop').click(function () {
    document.getElementById("ContactUsFormTag").reset();
    $('input').removeClass('error');

    grecaptcha.reset();
});

$('#mobileContactFormPopupClose').click(function () {
    document.getElementById("mobileContactUsFormTag").reset();
    $('input').removeClass('error');

    grecaptcha.reset(mobileContactUsrecaptcha);
});

$("#WhitePaperFormSubmit").click(function (e) {

    var CardName = $("#card-name").val();
    CardName = CardName.replace("DC", "Factsheet");
    CardName = CardName.replaceAll(" ", "-");
    var response = grecaptcha.getResponse(WhitePaperrecaptcha);
    if (response.length == 0) {
        alert("Captcha Required.");
        return false;
    }

    if (!$("#namewhite").val()) {
        $("#namewhite").addClass("error");
        return false;
    }

    $("#namewhite").removeClass("error");
    var namev = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#namewhite").val())) {
        $("#namewhite").addClass("error");
        //$("#dwld-message").html('Please enter valid name');
        return false;
    }
    $("#namewhite").removeClass("error");
    $("#dwld-message").html('');
    if (!$("#mobilewhite").val()) {
        $("#mobilewhite").addClass("error");
        return false;
    }
    $("#mobilewhite").removeClass("error");
    $("#dwld-message").html('');
    var mobilev = new RegExp('^\d*(?:\.\d{1,2})?$');

    if (!/^\d*(?:\.\d{1,2})?$/.test($("#mobilewhite").val())) {
        $("#mobilewhite").addClass("error");
        //$("#dwld-message").html('Please enter valid mobile no');
        return false;
    }
    $("#mobilewhite").removeClass("error");
    $("#dwld-message").html('');

    if (!$("#emailwhite").val().trim()) {
        $("#emailwhite").addClass("error");
        return false;
    }
    var emailv = new RegExp('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$');
    $("#emailwhite").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#emailwhite").val().trim())) {
        $("#emailwhite").addClass("error");
        //$("#dwld-message").html('Please enter valid e-mail address');
        return false;
    }
    $("#emailwhite").removeClass("error");
    $("#dwld-message").html('');

    if (!$("#company").val()) {
        $("#company").addClass("error");
        return false;
    }
    $("#company").removeClass("error");
    var companyv = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#company").val())) {
        $("#company").addClass("error");
        //$("#dwld-message").html('Please enter valid company name');
        return false;
    }
    $("#company").removeClass("error");
    $("#dwld-message").html('');

    if (!$("#country").val()) {
        $("#country").addClass("error");
        return false;
    }
    $("#country").removeClass("error");
    var countryv = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#country").val())) {
        $("#country").addClass("error");
        //$("#dwld-message").html('Please enter valid country name');
        return false;
    }
    $("#country").removeClass("error");
    $("#dwld-message").html('');

    if (!($('#whitePaperRemember').prop('checked'))) {
        $("#dwld-message").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $("input").removeClass("error");
    $("textarea").removeClass("error");
    $("#dwld-message").html('');

    var savecontactdata = {
        SendEmail: $("#sendEmailWhitePaper").val(),
        Name: $("#namewhite").val(),
        Contact: $("#mobilewhite").val(),
        Email: $("#emailwhite").val().trim(),
        Company: $("#company").val(),
        Country: $("#country").val(),
        reResponse: response

    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecontactdata),
        url: "/api/AdaniConnex/InsertWhitePaperFormdetail",
        contentType: "application/json",
        success: function (data) {
            if (data.status == 1) {
                $.ajax({
                    url: $("#data-url").val(),
                    method: 'GET',
                    xhrFields: {
                        responseType: 'blob'
                    },
                    success: function (data) {
                        closeModal();
                        $('#download-thankyouPopup').toggleClass('show');
                        var a = document.createElement('a');
                        var url = window.URL.createObjectURL(data);
                        a.href = url;
                        a.download = CardName + '.pdf';
                        document.body.append(a);
                        a.click();
                        a.remove();
                        window.URL.revokeObjectURL(url);
                        $('#whitePaperRemember').prop('checked', false);
                        $("#namewhite").val('');
                        $("#mobilewhite").val('');
                        $("#emailwhite").val('');
                        $("#company").val('');
                        $("#country").val('');
                        grecaptcha.reset(WhitePaperrecaptcha);
                    }
                });
                //window.location.href = $("#data-url").val();


            }
            else if (data.status == 401) {
                $("#company").addClass("error");

            }
            else if (data.status == 406) {
                $("#country").addClass("error");

            }
            else if (data.status == 407) {
                $("#country").addClass("error");

            }
            else if (data.status == 403) {
                $("#emailwhite").addClass("error");

            }
            else if (data.status == 405) {
                $("#mobilewhite").addClass("error");

            }
            else if (data.status == 408) {
                alert("Captcha Required");

            }
            else if (data.status == 409) {
                alert("Form submitted successfully. Email has not been sent to user ID.");

            }

        },

        error: function (data) {
            alert(data.status);
        }
    });
})




$('.getInTouchActionnew').click(function () {
    $("#getInTouchPopup").toggleClass('show');
    $("body").toggleClass('no-scroll');
});

$('#getInTouchPopupClose').click(function () {
    $(this).parent().toggleClass('show');
    $("body").toggleClass('no-scroll');
});




$("#getintouchform").click(function (e) {
    var response = grecaptcha.getResponse(GetInTouchrecaptcha);
    if (response.length == 0) {
        alert("Captcha Required.");
        return false;
    }

    if (!$("#getname").val()) {
        $("#getname").addClass("error");
        return false;
    }
    $("#getname").removeClass("error");

    var namev = new RegExp('^[a-zA-Z ]+$');
    if (!/^[a-zA-Z ]+$/.test($("#getname").val())) {
        $("#getname").addClass("error");
        //$("#git-error").html('Please enter valid name');
        return false;
    }
    $("#getname").removeClass("error");
    $("#git-error").html('');

    if (!$("#getemail").val().trim()) {
        $("#getemail").addClass("error");
        return false;
    }

    var emailv = new RegExp('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$');
    $("#getemail").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#getemail").val().trim())) {
        $("#getemail").addClass("error");
        //$("#git-error").html('Please enter valid e-mail address');
        return false;
    }
    $("#getemail").removeClass("error");
    $("#git-error").html('');

    if (!$("#getmobile").val()) {
        $("#getmobile").addClass("error");
        return false;
    }

    $("#getmobile").removeClass("error");
    var mobilev = new RegExp('^\d*(?:\.\d{1,2})?$');
    if ($("#getmobile").val().length !== 10) {
        $("#getmobile").addClass("error");
        //$("#git-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#getmobile").val())) {
        $("#getmobile").addClass("error");
        // $("#git-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    $("#getmobile").removeClass("error");
    $("#git-error").html('');


    //company code
    if (!$("#getcompany").val()) {
        $("#getcompany").addClass("error");
        return false;
    }
    $("#getcompany").removeClass("error");
    var getcompanyv = new RegExp('^[a-zA-Z ]+$');

    if (!/^[a-zA-Z ]+$/.test($("#getcompany").val())) {
        $("#getcompany").addClass("error");

        return false;
    }
    $("#getcompany").removeClass("error");



    if (!$("#getmessage").val()) {
        $("#getmessage").addClass("error");
        return false;
    }

    $("input").removeClass("error");
    $("textarea").removeClass("error");
    $("#git-error").html('');

    if (!($('#GetInTouchRemember').prop('checked'))) {
        $(".error-message").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $(".error-message").html('');
    var savecontactdata = {
        SendEmail: $("#sendEmailGetInTouch").val(),
        Name: $("#getname").val(),
        Contact: $("#getmobile").val(),
        Email: $("#getemail").val().trim(),
        Message: $("#getmessage").val(),
        Company: $("#getcompany").val(),
        FormType: $("#getFormType").val(),
        FormUrl: $(location).attr("href"),
        reResponse: response
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecontactdata),
        url: "/api/AdaniConnex/Insertcontactdetail",
        contentType: "application/json",
        success: function (data) {
            if (data.status == 1) {
                $("#getname").val('')
                $("#getemail").val('')
                $("#getmobile").val('')
                $("#getcompany").val('')
                $("#getmessage").val('')
                $('#GetInTouchRemember').prop('checked', false);
                $("#getintouchBtn").click();
                $('#getInTouchPopup').toggleClass('show');
                $('#getInTouchPopup1').toggleClass('show');
                grecaptcha.reset(GetInTouchrecaptcha);
            }
            else if (data.status == 401) {
                $("#getname").addClass("error");

            }
            else if (data.status == 407) {
                $("#getcompany").addClass("error");

            }
            else if (data.status == 406) {
                $("#getmessage").addClass("error");

            }
            else if (data.status == 403) {
                $("#getemail").addClass("error");

            }
            else if (data.status == 405) {
                $("#getmobile").addClass("error");

            }
            else if (data.status == 408) {
                alert("Captcha Required");

            }
            else if (data.status == 409) {
                alert("Form submitted successfully. Email has not been sent to user ID.");

            }

        },

        error: function (data) {
            alert(data.status);
        }
    });


})

$(document)
    .off('click', '.takVirtaulTourActionnew')
    .on('click', '.takVirtaulTourActionnew', function (e) {
        e.preventDefault();

        $("#takVirtaulTourPopup").toggleClass('show');

        $("body").toggleClass('no-scroll');
    });


$('#takeATourPopupClose').click(function () {
    $(this).parent().toggleClass('show');
    $("body").toggleClass('no-scroll');
});

$("#TakeATourSubmitBtn").click(function (e) {


    if (!$("#tourname").val()) {
        $("#tourname").addClass("error");
        return false;
    }
    $("#tourname").removeClass("error");

    var namev = new RegExp('^[a-zA-Z ]+$');
    if (!/^[a-zA-Z ]+$/.test($("#tourname").val())) {
        $("#tourname").addClass("error");
        //$("#tat-error").html('Please enter valid name');
        return false;
    }
    $("#tourname").removeClass("error");
    $("#tat-error").html('');

    if (!$("#touremail").val()) {
        $("#touremail").addClass("error");
        return false;
    }

    var emailv = new RegExp('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$');
    $("#touremail").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#touremail").val())) {
        $("#touremail").addClass("error");
        //$("#tat-error").html('Please enter valid e-mail address');
        return false;
    }
    $("#touremail").removeClass("error");
    $("#tat-error").html('');

    if (!$("#tourmobile").val()) {
        $("#tourmobile").addClass("error");
        return false;
    }

    $("#tourmobile").removeClass("error");
    var mobilev = new RegExp('^\d*(?:\.\d{1,2})?$');
    if ($("#tourmobile").val().length !== 10) {
        $("#tourmobile").addClass("error");
        //$("#tat-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#tourmobile").val())) {
        $("#tourmobile").addClass("error");
        //$("#tat-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    $("#tourmobile").removeClass("error");
    $("#tat-error").html('');


    //company code
    if (!$("#tourcompany").val()) {
        $("#tourcompany").addClass("error");
        return false;
    }
    $("#tourcompany").removeClass("error");
    var getcompanyv = new RegExp('^[a-zA-Z.]+$');

    if (!/^[a-zA-Z.]+$/.test($("#tourcompany").val())) {
        $("#tourcompany").addClass("error");

        return false;
    }
    $("#tourcompany").removeClass("error");


    $("input").removeClass("error");
    $("textarea").removeClass("error");
    $("#tat-error").html('');

    if (!($('#rememberCheckBox').prop('checked'))) {
        $(".error-message").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $(".error-message").html('');
    var savecontactdata = {

        Name: $("#tourname").val(),
        Contact: $("#tourmobile").val(),
        Email: $("#touremail").val(),
        Company: $("#tourcompany").val(),
        Location: "",
        FormType: $("#TourFormType").val(),
        FormUrl: $(location).attr("href")
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecontactdata),
        url: "/api/AdaniConnex/InsertTakeATourdetail",
        contentType: "application/json",
        success: function (data) {
            if (data.status == 1) {
                $("#tourname").val('')
                $("#touremail").val('')
                $("#tourmobile").val('')
                $("#tourcompany").val('')
                //$("#tourlocation").val('')
                $('#rememberCheckBox').prop('checked', false);
                //$("#getintouchBtn").click();
                //alert("Thankyou! form submitted successfully");

                $('#takVirtaulTourPopup').toggleClass('show');
                //$('#takeATourPopup').toggleClass('show');


            }
            else if (data.status == 701) {
                $("#tourname").addClass("error");

            }
            else if (data.status == 707) {
                $("#tourcompany").addClass("error");

            }
            //else if (data.status == 406) {
            //    $("#tourlocation").addClass("error");

            //}
            else if (data.status == 703) {
                $("#touremail").addClass("error");

            }
            else if (data.status == 705) {
                $("#tourmobile").addClass("error");

            }

        },
        error: function (data) {
            alert(data.status);
        }
    });


})



//popoup video play - stop


$('.takVirtaulTourActionnew').click(function () {
    var url = $('#takVirtaulTourPopup iframe').attr('data-src');
    $('#takVirtaulTourPopup iframe').attr("src", url);
});
$('#takVirtaulTourPopupClose').click(function () {
    var url = null;
    $('#takVirtaulTourPopup iframe').attr("src", url);
});
$('.i-close').click(function () {

    var url = null;
    $('#takVirtaulTourPopup iframe').attr("src", url);
    document.getElementById("ContactUsFormTag").reset();
    document.getElementById("GetInTouchFormTag").reset();
    document.getElementById("WhitePaperFormTag").reset();
    document.getElementById("mobileContactUsFormTag").reset();
    $('input').removeClass('error');
    grecaptcha.reset(GetInTouchrecaptcha);
    grecaptcha.reset(WhitePaperrecaptcha);
    grecaptcha.reset(mobileContactUsrecaptcha);
    grecaptcha.reset();
});


$(function () {
    $('.takVirtaulTourActionnew').on("click", function () {
        $('#takVirtaulTourPopup .embed-container').addClass("loader");
        setTimeout(RemoveClass, 900);
    });
    function RemoveClass() {

        $('#takVirtaulTourPopup .embed-container').removeClass("loader");
    }
});



$("#ConnectWithHRform").click(function (e) {
    var response = grecaptcha.getResponse(GetInTouchrecaptcha);
    if (response.length == 0) {
        alert("Captcha Required.");
        return false;
    }

    if (!$("#getname").val()) {
        $("#getname").addClass("error");
        return false;
    }
    $("#getname").removeClass("error");

    var namev = new RegExp('^[a-zA-Z ]+$');
    if (!/^[a-zA-Z ]+$/.test($("#getname").val())) {
        $("#getname").addClass("error");
        //$("#git-error").html('Please enter valid name');
        return false;
    }
    $("#getname").removeClass("error");
    $("#git-error").html('');

    if (!$("#getemail").val().trim()) {
        $("#getemail").addClass("error");
        return false;
    }

    var emailv = new RegExp('^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$');
    $("#getemail").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#getemail").val().trim())) {
        $("#getemail").addClass("error");
        //$("#git-error").html('Please enter valid e-mail address');
        return false;
    }
    $("#getemail").removeClass("error");
    $("#git-error").html('');

    if (!$("#getmobile").val()) {
        $("#getmobile").addClass("error");
        return false;
    }

    $("#getmobile").removeClass("error");
    var mobilev = new RegExp('^\d*(?:\.\d{1,2})?$');
    if ($("#getmobile").val().length !== 10) {
        $("#getmobile").addClass("error");
        //$("#git-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#getmobile").val())) {
        $("#getmobile").addClass("error");
        // $("#git-error").html('Please enter valid 10 digit mobile no');
        return false;
    }
    $("#getmobile").removeClass("error");
    $("#git-error").html('');


    if (!$("#getmessage").val()) {
        $("#getmessage").addClass("error");
        return false;
    }

    $("input").removeClass("error");
    $("textarea").removeClass("error");
    $("#git-error").html('');

    if (!($('#GetInTouchRemember').prop('checked'))) {
        $(".error-message").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $(".error-message").html('');
    var savecontactdata = {
        SendEmail: $("#sendEmailConnectWithHR").val(),
        Name: $("#getname").val(),
        Contact: $("#getmobile").val(),
        Email: $("#getemail").val().trim(),
        Message: $("#getmessage").val(),
        Company: " ",
        FormType: $("#getFormType").val(),
        FormUrl: $(location).attr("href"),
        reResponse: response
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(savecontactdata),
        url: "/api/AdaniConnex/Insertcontactdetail",
        contentType: "application/json",
        success: function (data) {
            if (data.status == 1) {
                $("#getname").val('')
                $("#getemail").val('')
                $("#getmobile").val('')
                $("#getmessage").val('')
                $('#GetInTouchRemember').prop('checked', false);
                $("#getintouchBtn").click();
                $('#ConnectWiththeHR').toggleClass('show');
                $('#getInTouchPopup1').toggleClass('show');
                grecaptcha.reset(GetInTouchrecaptcha);
            }
            else if (data.status == 401) {
                $("#getname").addClass("error");

            }

            else if (data.status == 406) {
                $("#getmessage").addClass("error");

            }
            else if (data.status == 403) {
                $("#getemail").addClass("error");

            }
            else if (data.status == 405) {
                $("#getmobile").addClass("error");

            }
            else if (data.status == 408) {
                alert("Captcha Required");

            }
            else if (data.status == 409) {
                alert("Form submitted successfully. Email has not been sent to user ID.");

            }
        },

        error: function (data) {
            alert(data.status);
        }
    });


});

$("#JoinUsform").click(function (e) {
    var response = grecaptcha.getResponse(JoinUsrecaptcha);
    if (response.length == 0) {
        alert("Captcha Required.");
        return false;
    }

    if (!$("#uploadCvPopup #getname").val()) {
        $("#uploadCvPopup #getname").addClass("error");
        return false;
    }
    $("#uploadCvPopup #getname").removeClass("error");


    if (!/^[a-zA-Z ]+$/.test($("#uploadCvPopup #getname").val())) {
        $("#uploadCvPopup #getname").addClass("error");
        //$("#git-error").html('Please enter valid name');
        return false;
    }
    $("#uploadCvPopup #getname").removeClass("error");
    $("#uploadCvPopup #git-error").html('');

    if (!$("#uploadCvPopup #getemail").val().trim()) {
        $("#uploadCvPopup #getemail").addClass("error");
        return false;
    }


    $("#uploadCvPopup #getemail").removeClass("error");
    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#uploadCvPopup #getemail").val().trim())) {
        $("#uploadCvPopup #getemail").addClass("error");
        //$("#git-error").html('Please enter valid e-mail address');
        return false;
    }
    $("#uploadCvPopup #getemail").removeClass("error");
    $("#uploadCvPopup #git-error").html('');

    if (!$("#uploadCvPopup #getmobile").val()) {
        $("#uploadCvPopup #getmobile").addClass("error");
        return false;
    }

    $("#uploadCvPopup #getmobile").removeClass("error");

    if ($("#uploadCvPopup #getmobile").val().length !== 10) {
        $("#uploadCvPopup #getmobile").addClass("error");

        return false;
    }
    if (!/^\d*(?:\.\d{1,2})?$/.test($("#uploadCvPopup #getmobile").val())) {
        $("#uploadCvPopup #getmobile").addClass("error");

        return false;
    }
    $("#uploadCvPopup #getmobile").removeClass("error");
    $("#uploadCvPopup #git-error").html('');

    // upload cv 
    if (!$("#uploadCvPopup #cvfile").val()) {
        $("#uploadCvPopup #cvfileInput, #uploadCvPopup #cvfile").addClass("error");
        $("#uploadCvPopup #cvfile").parents(".input-wrapper").first().css("border-bottom", "1px solid red");
        return false;
    }
    $("#uploadCvPopup #cvfileInput, #uploadCvPopup #cvfile").removeClass("error");
    $("#uploadCvPopup #cvfile").parents(".input-wrapper").first().css("border-bottom", "1px solid rgba(0,0,0,.5)");



    $("input").removeClass("error");
    $("textarea").removeClass("error");
    $("#git-error").html('');


    if (!($('#uploadCvPopup #rememberCheckBox').prop('checked'))) {
        $(".error-message").html('Please accept terms and conditions to proceed.');
        return false;
    }
    $(".error-message").html('');

    var fileUpload = $("#cvfile").get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    var Name = $("#uploadCvPopup #getname").val();
    var Contact = $("#uploadCvPopup #getmobile").val();
    var Email = $("#uploadCvPopup #getemail").val().trim();
    var FormType = "JoinUSForm";
    var FormUrl = $(location).attr("href");
    var SendEmail = $("#sendEmailJoinUs").val();
    var reResponse = response;
    fileData.append("CVFile", files[0]);
    fileData.append("Name", Name);
    fileData.append("Contact", Contact);
    fileData.append("Email", Email);
    fileData.append("FormType", FormType);
    fileData.append("FormUrl", FormUrl);
    fileData.append("reResponse", reResponse);
    fileData.append("SendEmail", SendEmail);

    $.ajax({
        type: "POST",
        data: fileData,
        contentType: false,
        processData: false,
        url: "/api/AdaniConnex/InsertJoinUsFormdetail",
        success: function (data) {
            if (data.status == 1) {
                $('#JoinUSFormTag')[0].reset();
                $(".removeCvFile").attr("src", "/-/media/Project/AdaniConneX/Career/Form/upload.png");
                $(".right_sign").attr("src", "");
                $('#cvfile').val('');
                $('#uploadCvPopup').toggleClass('show');
                $('#getInTouchPopup1').toggleClass('show');
                grecaptcha.reset(JoinUsrecaptcha);
            }
            else if (data.status == 401) {
                $("#uploadCvPopup #getname").addClass("error");

            }
            else if (data.status == 403) {
                $("#uploadCvPopup #getemail").addClass("error");

            }
            else if (data.status == 405) {
                $("#uploadCvPopup #getmobile").addClass("error");

            }
            else if (data.status == 406) {
                $("#uploadCvPopup #cvfile").addClass("error");
                $("#uploadCvPopup #cvfileInput").addClass("error");
                alert("Invalid File Input");

            }
            else if (data.status == 408) {
                alert("Captcha Required");

            }
            else if (data.status == 409) {
                alert("Form submitted successfully. Email has not been sent to user ID.");

            }

        },

        error: function (data) {
            alert(data.status);
        }
    });


});

$('.removeCvFile').on('click', function () {
    $('#cvfile').val('');
});

 $("#pressPopupClose").on("click",function(){
        $('#pressPopup').removeClass('show');
		});