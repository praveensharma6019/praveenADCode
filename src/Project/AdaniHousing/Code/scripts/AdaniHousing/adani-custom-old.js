$(document).ready(function () {

    $('.hero-banner').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        items: 1,
        nav: false,
        dots: true,
        slideSpeed: 2000,
        smartSpeed: 1500
    });

    $('.inner-banner').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        items: 1,
        nav: false,
        dots: true
    });
    $('.wholesale-finance').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        items: 1,
        nav: true,
        dots: false,
        navText: ["<span class='fa fa-angle-left'></span>", "<span class='fa fa-angle-right'></span>"]
    });
    $('.career_carousel').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        items: 1,
        nav: false,
        dots: true,
        navText: ["<span class='fa fa-angle-left'></span>", "<span class='fa fa-angle-right'></span>"]
    });
    $('#other-ventures').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 2,
                nav: true,
                dots: false
            },
            600: {
                items: 3,
                nav: true,
                dots: false
            },
            1000: {
                items: 7,
                nav: true,
                dots: false,
                margin: 20
            }
        }
    });

    $('.campus-carousel').on('initialized.owl.carousel changed.owl.carousel', function (e) {
        if (!e.namespace) {
            return;
        }
        var carousel = e.relatedTarget;
        $('.campus-carousel-counter').text(carousel.relative(carousel.current()) + 1 + '/' + carousel.items().length);
    }).owlCarousel({
        items: 1,
        loop: false,
        margin: 0,
        nav: true,
        dots: false,
        navText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
    });

    $('.news_carousel').owlCarousel({
        loop: false,
        margin: 30,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: true,
        dots: false,
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

    $('.carousel_cust--stories').owlCarousel({
        loop: true,
        margin: 20,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: true,
        dots: false,
        slideSpeed: 2000,
        smartSpeed: 1500,
        navText: ["<span class='fa fa-angle-left'></span>", "<span class='fa fa-angle-right'></span>"],
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

    $('.single-slide').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    });


    var $star_rating = $('.star-rating span');

    var SetRatingStar = function () {
        return $star_rating.each(function () {
            if (parseInt($star_rating.siblings('input.rating-value').val()) >= parseInt($(this).data('rating'))) {
                return $(this).removeClass('far fa-star').addClass('fas fa-star');
            } else {
                return $(this).removeClass('fas fa-star').addClass('far fa-star');
            }
        });
    };

    $star_rating.on('click', function () {
        $star_rating.siblings('input.rating-value').val($(this).data('rating'));
        return SetRatingStar();
    });

    SetRatingStar();
    $(document).ready(function () {

    });

});

$('.primary-menu .nav ul li a').click(function () {
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    $('.siderbar_fixed').toggleClass('d-none');
    $('.primary-menu .nav').removeClass('show');
});

$('#navbar-toggle').click(function () {
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    $('.siderbar_fixed').toggleClass('d-none');
});

$('#nav-button').click(function () {
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    $('.sidebar_s-icons').toggleClass('d-none');
    $('.siderbar_fixed').toggleClass('d-none');
});
$('.collapse_toggle').click(function () {
    $('.navbar-collapse').removeClass('show');
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    $('.sidebar_s-icons').toggleClass('d-none');
});

$('.fade-overlay').click(function () {
    $('.navbar-collapse').removeClass('show');
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    $('.sidebar_s-icons').toggleClass('d-none');
});

$('.sitemap-link').click(function () {
    $('footer .sitemap_visible').toggleClass('active');
    $('.sitemap-link').toggleClass('sitemap-link-active');
    $('html, body').animate({
        scrollTop: $(".sitemap-link").offset().top
            - 100
    }, 1000);
});


$('.scroll_down a').on('click', function (e) {
    var href = $(this).attr('href');
    $('html, body').animate({
        scrollTop: $(href).offset().top - 70
    }, '600');
    e.preventDefault();
});

$('.section-Quick_links p').on('click', function (e) {
    $('.close').toggleClass('d-none');
    $('.open').toggleClass('d-block');
    $('.section-Quick_links').toggleClass('hide');
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

// scroll body to 0px on click
$('#back-to-top').click(function () {
    $('#back-to-top').tooltip('hide');
    $('body,html').animate({
        scrollTop: 0
    }, 800);
    return false;
});

$('header .nav-item-dropdown').click(function () {
    // $('.topMenu .dropdown.active').removeClass('active');
    // $(this).toggleClass('active');
    if ($(this).hasClass('active')) {
        $(this).removeClass('active');
    } else {
        $('header .nav-item-dropdown').removeClass('active');
        $(this).addClass('active');
    }
});


var $r = $('input.emi[type="range"]');
var $ruler = $('<div class="rangeslider__ruler" />');

// Initialize
$r.rangeslider({
    polyfill: false,
    onInit: function () {
        $ruler[0].innerHTML = getRulerRange(this.min, this.max, this.step);
        this.$range.prepend($ruler);
    }
});

function getRulerRange(min, max, step) {
    var range = '';
    var i = 1;

    while (i <= max) {
        range += i + ' ';
        i = i + step;
    }
    return range;
}

var $r = $('input.eligibility-range[type="range"]');
var $ruler = $('<div class="rangeslider__ruler" />');

// Initialize
$r.rangeslider({
    polyfill: false,
    onInit: function () {
        $ruler[0].innerHTML = getRulerRange(this.min, this.max, this.step);
        this.$range.prepend($ruler);
    }
});

//function getRulerRange(min, max, step) {
//    var range = '';
//    var i = 1;

//    while (i <= max) {
//        range += i + ' ';
//        i = i + step;
//    }
//    return range;
//}
var recaptcha1;
var Feedbackrecaptcha;
var ReqCallbackrecaptcha;
var ApplyPoprecaptcha;

var onloadCallback = function () {
    //Render the recaptcha1 on the element with ID "recaptcha1"
    if ($('#recaptcha1').length) {
        recaptcha1= grecaptcha.render('recaptcha1', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
    if ($('#Feedbackrecaptcha').length) {
        Feedbackrecaptcha = grecaptcha.render('Feedbackrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
    if ($('#ReqCallbackrecaptcha').length) {
        ReqCallbackrecaptcha = grecaptcha.render('ReqCallbackrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
    if ($('#ApplyPoprecaptcha').length) {
        ApplyPoprecaptcha = grecaptcha.render('ApplyPoprecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
    //recaptcha1 = grecaptcha.render('recaptcha1', {
    //    'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
    //    'theme': 'light'
    //});
};
$("#btnContactUsSubmit").on("click", function () {
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length === 0) {
        alert("Captcha required.");
        return false;
    }
    $("#reResponse").val(response);
});

$("#btnBecomePartnerSubmit").on("click", function () {
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length === 0) {
        alert("Captcha required.");
        return false;
    }
    $("#BAPreResponse").val(response);

});

$("#ApplyForLoanSubmitBtn").on("click", function () {
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length === 0) {
        alert("Captcha required.");
        return false;
    }
    $("#Apply-reResponse").val(response);

});

$(window).on('load', function () {
    if (window.location.pathname === "/" || window.location.pathname === "/home") {
        $('#default_modal').modal('show');
    }

});


// Custom JS Start
function calculateEMI() {
	// Formula: 
	// EMI = (P * R/12) * [ (1+R/12)^N] / [ (1+R/12)^N-1]  
	// isNaN(isNotaNumber): Check whether the value is Number or Not                

	var emi = 0;
	var EmiPerLakh = 0;
	var Eligible = 0;
	var totintyear = 0;
	var totamount = 0;
	var totintamt = 0;
	var P = document.getElementById("loanamount").value;
	var r = document.getElementById("interestrate").value;
	var n = document.getElementById("months").value;

	var per = parseFloat(r / 100);

	EmiPerLakh = (P * (per / 12)) * (Math.pow(1 + (per / 12)), 1) / (1 - Math.pow((1 + (per / 12)), -1 * n));
	totamount = (EmiPerLakh*n);
	totintamt = (totamount-P);
	totintyear = (totintamt/n);
	
	document.getElementById("lbMonthEmi").innerHTML = '' + Math.round(EmiPerLakh).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
	document.getElementById("lbtotamt").innerHTML = '' + Math.round(totamount).toString().replace(/\B(?=(\d{2})+(?!\d))/g, ",")
	document.getElementById("lbtotintamt").innerHTML = '' + Math.round(totintamt).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
	document.getElementById("lbtotintyear").innerHTML = '' + Math.round(totintyear).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
	document.getElementById("lbtenure").innerHTML = '' +n
	document.getElementById("lbinterest").innerHTML = '' +r
	document.getElementById("lbprincipal").innerHTML = '' +Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
	document.getElementById("lbtenure1").innerHTML = '' +n
	document.getElementById("lbinterest1").innerHTML = '' +r
	document.getElementById("lbprincipal1").innerHTML = '' +Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
}
$(function () {
    $('#zipcode').change(function () {

        var ZipValue = $(this).val();
        //var propName = $("#PropertyName option:selected").text();

        $.ajax({
            type: "GET",
            url: "/api/AdaniHousing/PincodeDetails",
            data: { Pin: ZipValue },
            success: function (obj) {
                if (obj !== null) {
                    $('#state').val(obj.State);
                    $('#city').val(obj.City);
                }
                else {
                    alert("Invalid Pincode");
                }
            },
            error: function () {
                alert("Error in retrieving Pincode");
            }
        });
    });
});
function ValidateEmail(mail) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(mail);
}
$(function () {
$('#Locator-State').change(function () {
        var SelectedState = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniHousing/BranchLocation",
            data: { State: SelectedState, City: '' },
            success: function (obj) {
                if (obj !== null) {
                    $('#Locator-City').text('');
                    $('#Locator-City').append("<option value=''> Select City </option>");
                    $('#Locator-address').text('');
                    $('#Locator-address').text('One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051');
                    $('#locmap').remove();
                    for (var i = 0; i < obj.length; i++) {
                        var name = obj[i];
                        $('#Locator-City').append("<option value='" + name + "'>" + name.toUpperCase() + "</option>");
                    }
                }
                else {
                    alert("Unable to locate branch");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
    $('#Locator-City').change(function () {
        var SelectedState = $('#Locator-State').val();
        var SelectedCity = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniHousing/BranchLocation",
            data: { State: SelectedState, City: SelectedCity },
            success: function (obj) {
                if (obj != null) {
                    if (obj["Address"] != '') {
                        $('#Locator-address').text('');
						$('#locmap').remove();
                        $('#Locator-address').text(obj["Address"]);
                        if (obj["Latitude"] !== '' && obj["Longitude"] !== '') {
                            $('#Locator-address').after("<p id='locmap'><a target='_blank' href='http://maps.google.com/maps?q=" + obj["Latitude"] + "," + obj["Longitude"] + "'>View on Map</a></p>");
                        }
                        
                    }
                    else {
                        $('#Locator-address').text('');
                        $('#Locator-address').text('One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051');
                        $('#locmap').remove();
                        alert("Unable to locate branch at selected city");
                    }
                }
                else {
                    alert("Unable to locate branch");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
    $('#centre-State').change(function () {
        var SelectedState = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniHousing/CentreLocation",
            data: { State: SelectedState, City: '' },
            success: function (obj) {
                if (obj !== null) {
                    $('#centre-City').text('');
                    $('#centre-City').append("<option value=''> Select City </option>");
                    $('#centre-info').empty();
                    $('#centre-info').append('<div><h4> Adani Capital Pvt.Ltd.</h4><p>One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051</p><p><a href="tel:18002100444"><span class="fa fa-phone fa-rotate-90 mr-2"></span> 1800-210-0444</a></p><p id="Branch-mail"><a href="mailto:info@adaniHousing.com"><span class="fa fa-envelope mr-2"></span>  info@adaniHousing.com</a></p></div>');
                    for (var i = 0; i < obj.length; i++) {
                        var name = obj[i];
                        $('#centre-City').append("<option value='" + name + "'>" + name.toUpperCase() + "</option>");
                    }
                }
                else {
                    alert("Unable to locate collection centre");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
    $('#centre-City').change(function () {
        var SelectedState = $('#centre-State').val();
        var SelectedCity = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniHousing/CentreLocation",
            data: { State: SelectedState, City: SelectedCity },
            success: function (obj) {
                if (obj !== null) {
                    if (obj.length > 0) {
                        $('#centre-info').empty();
                        for (var i = 0; i < obj.length; i++) {
                            var Address = obj[i]["Address"];
                            var MerchantOrShop = obj[i]["MerchantOrShop"];
                            var OfficeType = obj[i]["OfficeType"];
                            var ContactNo = obj[i]["ContactNo"];
                            var Latitude = obj[i]["Latitude"];
                            var Longitude = obj[i]["Longitude"];
                            $('#centre-info').append("<div><h4>" + OfficeType + "</h4><p>" + MerchantOrShop + "</p><p>" + Address + '</p><p><a href="tel:' + ContactNo + '"><span class="fa fa-phone fa-rotate-90 mr-2"></span>' + ContactNo + "</p></div>");
                        }
                    }
                    else {
                        $('#centre-info').empty();
                        $('#centre-info').append('<div>< h4 > Adani Capital Pvt.Ltd.</h4 ><p>One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051</p><p><a href="tel:18002100444"><span class="fa fa-phone fa-rotate-90 mr-2"></span> 1800-210-0444</a></p><p id="Branch-mail"><a href="mailto:info@adanicapital.com"><span class="fa fa-envelope mr-2"></span>  info@adanicapital.com</a></p></div>');
                    }
                }
                else {
                    alert("Unable to locate collection centre");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
    $("#PopLoan-NextBtn").bind("click", function () {
        $('#PopLoan-NextBtn').attr("disabled", "disabled");
        var firstname = $("#PopLoan-Name").val();
        if (firstname === "") {
            alert("Please enter your Name"); $("#PopLoan-Name").focus(); $('#PopLoan-NextBtn').removeAttr("disabled"); return false;
        }
        var enuirymobile = $("#PopLoan-Mobile").val();
        if (enuirymobile === "") {
            alert("Please enter your 10 digit Mobile Number");
            $("#PopLoan-Mobile").focus();
            $('#PopLoan-NextBtn').removeAttr("disabled");
            return false;
        }
        if (enuirymobile.length !== 10) {
            alert("Please enter your 10 digit Mobile Number");
            $("#PopLoan-Mobile").focus();
            $('#PopLoan-NextBtn').removeAttr("disabled");
            return false;
        }
        var loanAmt = $("#PopLoan-Loan-Amount").val();
        if (loanAmt === "") { alert("Please enter loan amount"); $("#PopLoan-Loan-Amount").focus(); $('#PopLoan-NextBtn').removeAttr("disabled"); return false; }
        var enemail = $("#PopLoan-Email").val();
        if (enemail === "") { alert("Email is Required"); $("#PopLoan-Email").focus(); $('#PopLoan-NextBtn').removeAttr("disabled"); return false; }
        if (!ValidateEmail(enemail)) {
            alert("Email is not Valid"); $("#PopLoan-Email").focus(); $('#PopLoan-NextBtn').removeAttr("disabled"); return false;
        }
        var url = "/apply-for-loan?name=" + encodeURIComponent($("#PopLoan-Name").val()) + "&mobile=" + encodeURIComponent($("#PopLoan-Mobile").val()) + "&loanamt=" + encodeURIComponent($("#PopLoan-Loan-Amount").val()) + "&email=" + encodeURIComponent($("#PopLoan-Email").val());
        window.location.href = url;
    });
});
$("#btnFeedbackSubmit").click(function () {
    var response = grecaptcha.getResponse(Feedbackrecaptcha);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    $('#btnFeedbackSubmit').attr("disabled", "disabled");
    var name = $("#FeedbackName").val();
    if (name == "") { alert("Please enter your Name"); $("#FeedbackName").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#FeedbackEmailId").val();
    if (mailid == "") { alert("Email is Required"); $("#FeedbackEmailId").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#FeedbackMobile").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#FeedbackMobile").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }

    if (ccontactno.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#FeedbackMobile").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false;
    }
    var message = $("#FeedbackMessage").val();
    if (message == "") { alert("Please enter any message"); $("#FeedbackMessage").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var rating = $("#FeedbackRating").val();
    if (rating == "") { alert("Please rate us."); $("#FeedbackRating").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var formtype = $("#FeedbackformName").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    //create json object
    var savecustomdata = {
        Name: name,
        EmailID: mailid,
        MobileNo: ccontactno,
        Message: message,
        Rating: rating,
        reResponse: response,
        FormName: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniHousing/FeedbackForm",
        contentType: "application/json",
        success: function (data) {
            //////////////
            if (data.status == '0') {
                alert(data.msg);
                $('#btnFeedbackSubmit').removeAttr("disabled");
                return false;
            }
            else {
                $("#Feedback_form .close").click();
                $('#feedbackForm').trigger("reset");
                $('#btnFeedbackSubmit').removeAttr("disabled");
                alert(data.msg);
            }

        },
        error: function (data) {
            alert("Something has been wrong, Please try again");
            $('#btnFeedbackSubmit').removeAttr("disabled");
            return false;
        }
    });
    return false;

});
$("#btnReqCallbackSubmit").click(function () {
    //var response = grecaptcha.getResponse(recaptcha1);
    //if (response.length == 0) {
    //    alert("Captcha required.");
    //    return false;
    //}

    $('#btnReqCallbackSubmit').attr("disabled", "disabled");
    var name = $("#ReqCallbackName").val();
    if (name == "") { alert("Please enter your Name"); $("#ReqCallbackName").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#ReqCallbackEmailId").val();
    if (mailid == "") { alert("Email is Required"); $("#ReqCallbackEmailId").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#ReqCallbackMobile").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ReqCallbackMobile").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }

    if (ccontactno.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#ReqCallbackMobile").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false;

    }

    var message = $("#ReqCallbackMessage").val();
    if (message == "") { alert("Please enter any message"); $("#ReqCallbackMessage").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }




    var formtype = $("#ReqCallbackformName").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {
        Name: name,
        EmailID: mailid,
        MobileNo: ccontactno,
        Message: message,
        //reResponse: response,
        FormName: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniHousing/RequestCallbackForm",
        contentType: "application/json",
        success: function (data) {
            //////////////
            if (data.status == '0') {
                alert(data.msg);
                $('#btnReqCallbackSubmit').removeAttr("disabled");
                return false;
            }
            else {
                $("#req_callback .close").click();
                $('#ReqCallbackForm').trigger("reset");
                $('#btnReqCallbackSubmit').removeAttr("disabled");
                alert(data.msg);
            }

        },
        error: function (data) {
            alert("Something has been wrong, Please try again");
            $('#btnReqCallbackSubmit').removeAttr("disabled");
            return false;
        }
    });
    return false;

});
$("#PopLoan-ApplyBtn").click(function () {
    var response = grecaptcha.getResponse(ApplyPoprecaptcha);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }

    $('#PopLoan-ApplyBtn').attr("disabled", "disabled");
    var fname = $("#PopLoan-fname").val();
    if (fname === "") { alert("Please enter your First Name"); $("#PopLoan-fname").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
    var lname = $("#PopLoan-lname").val();
    if (lname === "") { alert("Please enter your Last Name"); $("#PopLoan-lname").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
    var ccontactno = $("#PopLoan-Mobile").val();
    if (ccontactno === "") { alert("Please specify your Mobile Number"); $("#PopLoan-Mobile").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }

    if (ccontactno.length !== 10) {
        alert("Contact Number should be of 10 digit"); $("#PopLoan-Mobile").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false;
    }
    var mailid = $("#ReqCallbackEmailId").val();
    if (mailid !== "") { if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; } }

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }
    var Pincode = $("#PopLoan-zipcode").val();
    if (Pincode === "") { alert("Please enter pincode"); $("#PopLoan-zipcode").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
    var ProductName = $("#PopLoan-Product").val();
    if (ProductName == "") { alert("Please select Product"); $("#PopLoan-fname").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
    //var message = $("#ReqCallbackMessage").val();

    var formtype = "Apply For Loan PopUp Form";
    var pageinfo = window.location.href;


    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {
        FirstName: fname,
        LastName: lname,
        EmailID: mailid,
        MobileNo: ccontactno,
        PinCode: Pincode,
        ProductType: ProductName,
        reResponse: response,
        FormName: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniHousing/AdaniHousingApplyForLoanPopForm",
        contentType: "application/json",
        success: function (data) {
            //////////////
            if (data.status == '0') {
                alert(data.msg);
                $('#PopLoan-ApplyBtn').removeAttr("disabled");
                return false;
            }
            else {
                $("#default_modal .close").click();
                $('#ApplyPopUPForm').trigger("reset");
                $('#PopLoan-ApplyBtn').removeAttr("disabled");
                window.location.href = "https://stage.adaniHousing.in/thankyou";
            }

        },
        error: function (data) {
            alert("Something has been wrong, Please try again");
            $('#PopLoan-ApplyBtn').removeAttr("disabled");
            return false;
        }
    });
    return false;

});
$('#LMSmessage_modal button').click(function () {
    $('#LMSmessage_modal').css('display', 'none').removeClass('show');
    $('.modal-backdrop').css('display', 'none');
}
);
