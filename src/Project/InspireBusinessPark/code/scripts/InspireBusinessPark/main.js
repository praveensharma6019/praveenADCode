var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

var url = location.href;
var utm_url = url.split("?");
var utm_codes = [];

var utm_source = '';
var utm_medium = '';
var utm_campaign = '';
var utm_adgroup = '';
var utm_term = '';

if (utm_url[1]) {
    utm_codes = utm_url[1].split("&");
    if (utm_codes[0]) {
        utm_source = utm_codes[0].replace('utm_source=', '');
    } else {
        utm_source = 'Generic-Visit';
    }

    if (utm_codes[1]) {
        utm_medium: utm_codes[1].replace('utm_medium=', '');
    } else {
        utm_medium = 'Generic-Visit';
    }

    if (utm_codes[2]) {
        utm_campaign = utm_codes[2].replace('utm_campaign=', '');
    } else {
        utm_campaign = 'Generic-Visit';
    }

    if (utm_codes[3]) {
        utm_adgroup = utm_codes[3].replace('utm_adgroup=', '');
    } else {
        utm_adgroup = 'Generic-Visit';
    }

    if (utm_codes[4]) {
        utm_term = utm_codes[4].replace('utm_term=', '');
    } else {
        utm_term = 'Generic-Visit';
    }
} else {
    utm_source = 'Generic-Visit';
    utm_medium = 'Generic-Visit';
    utm_campaign = 'Generic-Visit';
    utm_adgroup = 'Generic-Visit';
    utm_term = 'Generic-Visit';
}

(function($) {
    "use strict";

    // Preloader (if the #preloader div exists)
    $(window).on('load', function() {
        if ($('#preloader').length) {
            $('#preloader').delay(100).fadeOut('slow', function() {
                $(this).remove();
            });
        }

        $.ajax({
            url: base_url+'api/loadUTMcodes',
            method : 'POST',
            crossDomain: true,
            data:{ 
                utm_source: utm_source,
                utm_medium: utm_medium,
                utm_campaign: utm_campaign,
                utm_adgroup: utm_adgroup,
                utm_term: utm_term,
            },
        }).done(function(res){
            console.log('Success');
        }).fail(function(res){
            console.log('Some technical error occured!');
        });
    });

    // Back to top button
    $(window).scroll(function() {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function() {
        $('html, body').animate({
            scrollTop: 0
        }, 1500, 'easeInOutExpo');
        return false;
    });

    // Initiate the wowjs animation library
    new WOW().init();

    // Header scroll class
    $(window).scroll(function() {
        if ($(this).scrollTop() > 100) {
            $('#header').addClass('header-scrolled');
        } else {
            $('#header').removeClass('header-scrolled');
        }
    });

    if ($(window).scrollTop() > 100) {
        $('#header').addClass('header-scrolled');
    }

    // Smooth scroll for the navigation and links with .scrollto classes
    $('.main-nav a, .mobile-nav a, .scrollto').on('click', function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            if (target.length) {
                var top_space = 0;

                if ($('#header').length) {
                    top_space = $('#header').outerHeight();

                    if (!$('#header').hasClass('header-scrolled')) {
                        top_space = top_space - 40;
                    }
                }

                $('html, body').animate({
                    scrollTop: target.offset().top - top_space
                }, 1500, 'easeInOutExpo');

                if ($(this).parents('.main-nav, .mobile-nav').length) {
                    $('.main-nav .active, .mobile-nav .active').removeClass('active');
                    $(this).closest('li').addClass('active');
                }

                if ($('body').hasClass('mobile-nav-active')) {
                    $('body').removeClass('mobile-nav-active');
                    $('.mobile-nav-toggle i').toggleClass('fa-times fa-bars');
                    $('.mobile-nav-overly').fadeOut();
                }
                return false;
            }
        }
    });

    // Navigation active state on scroll
    var nav_sections = $('section');
    var main_nav = $('.main-nav, .mobile-nav');
    var main_nav_height = $('#header').outerHeight();

    $(window).on('scroll', function() {
        var cur_pos = $(this).scrollTop();

        nav_sections.each(function() {
            var top = $(this).offset().top - main_nav_height,
                bottom = top + $(this).outerHeight();

            if (cur_pos >= top && cur_pos <= bottom) {
                main_nav.find('li').removeClass('active');
                main_nav.find('a[href="#' + $(this).attr('id') + '"]').parent('li').addClass('active');
            }
        });
    });

    // jQuery counterUp (used in Whu Us section)
    $('[data-toggle="counter-up"]').counterUp({
        delay: 10,
        time: 1000
    });

    // Porfolio isotope and filter
    $(window).on('load', function() {
        var portfolioIsotope = $('.portfolio-container').isotope({
            itemSelector: '.portfolio-item'
        });
        $('#portfolio-flters li').on('click', function() {
            $("#portfolio-flters li").removeClass('filter-active');
            $(this).addClass('filter-active');

            portfolioIsotope.isotope({
                filter: $(this).data('filter')
            });
        });
    });

    // Testimonials carousel (uses the Owl Carousel library)
    $(".testimonials-carousel").owlCarousel({
        autoplay: true,
        dots: true,
        loop: true,
        items: 1
    });

    // Clients carousel (uses the Owl Carousel library)
    $(".clients-carousel").owlCarousel({
        autoplay: true,
        dots: true,
        loop: true,
        responsive: {
            0: {
                items: 2
            },
            768: {
                items: 4
            },
            900: {
                items: 6
            }
        }
    });

})(jQuery);


$("#submit_form").click(function(e){
    e.preventDefault();
    if (!$("#input-name").val()) {
        $(".err_msg").html('Please Enter Your Name!');
        return false;
    }

    if (!$("#input-lastname").val()) {
        $(".err_msg").html('Please Enter Your Last Name!');
        return false;
    }

    if (!$("#input-email").val()) {
        $(".err_msg").html('Please Enter Your Email ID!');
        return false;
    }

    if (!$("#input-email").val()) {
        $("#err_msg").html("Please provide your Email ID!");
        return false;
    }

    if (email_regex.test($("#input-email").val())  == false) {
        $("#err_msg").html("Please enter proper Email ID!");
        return false;
    }

    if (!$("#input-mobile").val()) {
        $(".err_msg").html('Please Enter Your Mobile Number!');
        return false;
    }

    if ($("#input-mobile").val().length != 10 || regex_special_num.test($("#input-mobile").val()) == false) {
        $(".err_msg").html("Please enter 10 digit Mobile Number!");
        return false;
    }

    // if ($("#input-mobile-verified").val() == 'NO') {
    //     $(".err_msg").html("Please verify your number!");
    //     return false;
    // }

    if (!$("#input-company").val()) {
        $(".err_msg").html('Please Enter Your City Name!');
        return false;
    }

    $.ajax({
        url: base_url+'api/generate_otp',
        type: 'POST',
        crossDomain: true,
        data: {
            mobile: $("#input-mobile").val(),
        }
    }).done(function(res){
        console.log(res);
        if (res.Status == 'Success') {
            $(".err_msg").html('OTP has been shared on your mobile number, Please verify your mobile number using the OTP!');
            // $(".err_msg").html('');
            $("#OTPModal").modal('show');
        } else {
            $(".err_msg").html("Some technical error occured, Please try again later!");
            return false;
        }
    }).fail(function(res){
        $(".err_msg").html("Some technical error occured, Please try again later!");
        return false;
    })

    // $.ajax({
    //     url: base_url+'api/submit_adani_commercial_form',
    //     method : 'POST',
    //     crossDomain: true,
    //     data:{ 
    //         name: $("#input-name").val(),
    //         lastname: $("#input-lastname").val(),
    //         email: $("#input-email").val(),
    //         mobile: $("#input-mobile").val(),
    //         company: $("#input-company").val(),
    //         website: "Inspire Business Park - Adani Realty",
    //         form_type: "Website Leads",
    //         utm_source: utm_source,
    //         utm_medium: utm_medium,
    //         utm_campaign: utm_campaign,
    //         utm_adgroup: utm_adgroup,
    //         utm_term: utm_term,
    //     },
    // }).done(function(res){
    //     if (res.status == 'Success') {
    //         $.ajax({
    //             url: base_url+'api/send_email_adani_ibp',
    //             method : 'POST',
    //             crossDomain: true,
    //             data:{ 
    //                 name: $("#input-name").val(),
    //                 lastname: $("#input-lastname").val(),
    //                 email: $("#input-email").val(),
    //                 mobile: $("#input-mobile").val(),
    //                 company: $("#input-company").val(),
    //                 website: "Inspire Business Park - Adani Realty",
    //                 form_type: "Website Leads",
    //             }, 
    //         }).done(function(res){
    //             $(".err_msg").html('Thank you for contacting Adani Realty. Our Sales Manager will get back to you shortly. Have a good day!');
    //             // location.href = 'https://www.adanirealtycommercialspaces.com/thank-you';
    //             $("#input-name").val('');
    //             $("#input-lastname").val('');
    //             $("#input-email").val('');
    //             $("#input-mobile").val('');
    //             $("#input-company").val('');
    //         });
    //     } else {
    //         $(".err_msg").html('Some technical error occured, Please try again later!');
    //     }
    // }).fail(function(res){
    //     $(".err_msg").html('Some technical error occured, Please try again later!');
    // })
})

$(".submit_otp").click(function(e){
    e.preventDefault();
    $("#OTPModal").modal('show');
});

$(".verify_otp").click(function(e){
    e.preventDefault();

    if (!$("#input-mobile").val()) {
        $(".err_msg").html('Please Enter Your Mobile Number!');
        return false;
    }

    if ($("#input-mobile").val().length != 10 || regex_special_num.test($("#input-mobile").val()) == false) {
        $("#err_msg").html("Please enter 10 digit Mobile Number!");
        return false;
    }

    if (!$("#input-otp").val()) {
        $(".err_msg").html('Please Enter OTP!');
        return false;
    }

    var mobile = $("#input-mobile").val();

    $.ajax({
        url: base_url+'api/verify_otp',
        type: 'POST',
        crossDomain: true,
        data: {
            mobile: mobile,
            otp: $("#input-otp").val(),
        }
    }).done(function(res){
        if (res.status == 'Success') {
            $(".err_msg").html('Your mobile number is verified successfully!');
            // $("#OTPModal").modal('hide');
            $("#input-mobile-verified").val('YES');
            $("#input-mobile-status").val(mobile);

            $.ajax({
                url: base_url+'api/submit_adani_commercial_form',
                method : 'POST',
                crossDomain: true,
                data:{ 
                    name: $("#input-name").val(),
                    lastname: $("#input-lastname").val(),
                    email: $("#input-email").val(),
                    mobile: $("#input-mobile").val(),
                    company: $("#input-company").val(),
                    website: "Inspire Business Park - Adani Realty",
                    form_type: "Website Leads",
                    utm_source: utm_source,
                    utm_medium: utm_medium,
                    utm_campaign: utm_campaign,
                    utm_adgroup: utm_adgroup,
                    utm_term: utm_term,
                },
            }).done(function(res){
                if (res.status == 'Success') {
                    $.ajax({
                        url: base_url+'api/send_email_adani_ibp',
                        method : 'POST',
                        crossDomain: true,
                        data:{ 
                            name: $("#input-name").val(),
                            lastname: $("#input-lastname").val(),
                            email: $("#input-email").val(),
                            mobile: $("#input-mobile").val(),
                            company: $("#input-company").val(),
                            website: "Inspire Business Park - Adani Realty",
                            form_type: "Website Leads",
                        }, 
                    }).done(function(res){
                        // $(".err_msg").html('Thank you for contacting Adani Realty. Our Sales Manager will get back to you shortly. Have a good day!');
                        location.href = base_url+'thank-you';
                        $("#input-name").val('');
                        $("#input-lastname").val('');
                        $("#input-email").val('');
                        $("#input-mobile").val('');
                        $("#input-company").val('');
                    });
                } else {
                    $(".err_msg").html('Some technical error occured, Please try again later!');
                }
            }).fail(function(res){
                $(".err_msg").html('Some technical error occured, Please try again later!');
            })

        } else {
            $("#err_msg").html("Some technical error occured, Please try again later!");
            return false;
        }
    }).fail(function(res){
        $("#err_msg").html("Some technical error occured, Please try again later!");
        return false;
    })
})

$("#input-mobile").on('change', function(e){
    if ($("#input-mobile").val() == $("#input-mobile-status").val()) {
    } else {
        $("#input-mobile-verified").val('NO');
        $("#input-mobile-status").val('');
        $(".generate_otp").show();
        $(".resend_otp").hide();
    }
});

$("#submit").click(function (e) {
    e.preventDefault();
    if (!$("#input-name").val()) {
        $(".err_msg").html('Please Enter Your Name!');
        return false;
    }

    if (!$("#input-lastname").val()) {
        $(".err_msg").html('Please Enter Your Last Name!');
        return false;
    }

    if (!$("#input-email").val()) {
        $(".err_msg").html('Please Enter Your Email ID!');
        return false;
    }

    if (!$("#input-email").val()) {
        $("#err_msg").html("Please provide your Email ID!");
        return false;
    }

    if (email_regex.test($("#input-email").val()) == false) {
        $("#err_msg").html("Please enter proper Email ID!");
        return false;
    }

    if (!$("#input-mobile").val()) {
        $(".err_msg").html('Please Enter Your Mobile Number!');
        return false;
    }

    if ($("#input-mobile").val().length != 10 || regex_special_num.test($("#input-mobile").val()) == false) {
        $(".err_msg").html("Please enter 10 digit Mobile Number!");
        return false;
    }

    // if ($("#input-mobile-verified").val() == 'NO') {
    //     $(".err_msg").html("Please verify your number!");
    //     return false;
    // }
    
    if (!$("#input-company").val()) {
        $(".err_msg").html('Please Enter Your City Name!');
        return false;
    }
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    var pageinfo = window.location.href;
    var savecustomdata = {
        first_name: $("#input-name").val(),
        last_name: $("#input-lastname").val(),
        mobile: $("#input-mobile").val(),
        email: $("#input-email").val(),
        City: $("#input-company").val(),
        FormType: "Test",
        PageInfo: pageinfo,
        FormSubmitOn: currentdate,
        LeadSource: "Web to Lead"

            
    };
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/InspireBusinessPark/Insertcontactdetail",
        contentType: "application/json",
        success: function (data) {
            alert(data.status)
            //location.reload();

        },

        error: function (data) {
            alert(data.status);  // 
        }
    });

    var captcha_length = $('.g-recaptcha').length;
    if (captcha_length >= 1) {
        var response = grecaptcha.getResponse();
        //recaptcha failed validation
        if (response.length == 0) {
            $('#loading').remove();
            $('#google-recaptcha-error').remove();
            $('#' + section_id).find('.g-recaptcha').after('<span class="google-recaptcha-error" id="google-recaptcha-error">Invalid recaptcha</span>');
            tz_process = false;
        } else {
            $('#google-recaptcha-error').remove();
            $('#recaptcha-error').hide();
            tz_process = true;
        }
    }


})
