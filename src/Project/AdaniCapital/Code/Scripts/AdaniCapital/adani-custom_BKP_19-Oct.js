
$(document).ready(function () {

    $('.hero-banner').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        items: 1,
        nav: true,
        dots: true,
        slideSpeed: 2000,
        smartSpeed: 1500,
	navText: ["<i></i>","<i></i>"]
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
	
	$('.home-product_carousel').owlCarousel({
        loop: true,
        margin: 30,
        responsiveClass: true,
        autoplay: true,
        autoplayTimeout: 2500,
        navText: ["<span class='fa fa-angle-left'></span>", "<span class='fa fa-angle-right'></span>"],
        autoplayHoverPause: true,
		nav: true,
		dots: false,
        slideSpeed: 2000,
        smartSpeed: 1500,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3,
                margin: 40
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
    //$('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    //$('.siderbar_fixed').toggleClass('d-none');
    $('.primary-menu .nav').removeClass('show');
});

$('#navbar-toggle').click(function () {
    $('.fade-overlay').toggleClass('active');
    //$('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    //$('.siderbar_fixed').toggleClass('d-none');
});

$('#nav-button').click(function () {
    $('.fade-overlay').toggleClass('active');
    //$('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    $('.sidebar_s-icons').toggleClass('d-none');
    //$('.siderbar_fixed').toggleClass('d-none');
});
$('.collapse_toggle').click(function () {
    $('.navbar-collapse').removeClass('show');
    $('.fade-overlay').toggleClass('active');
    //$('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    //$('.sidebar_s-icons').toggleClass('d-none');
});

$('.fade-overlay').click(function () {
    $('.navbar-collapse').removeClass('show');
    $('.fade-overlay').toggleClass('active');
    //$('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
    //$('.sidebar_s-icons').toggleClass('d-none');
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
    //$('#back-to-top').fadeIn();
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

        //$('#back-to-top').hide();
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
$(window).on('load', function () {
    if (window.location.pathname === "/" || window.location.pathname === "/home") {
        $('#default_modal').modal('show');
    }
    var currURL = window.location.pathname;
    if (currURL.toLowerCase() === "/farm-equipments-loan/tractor-loan" || currURL.toLowerCase() === "/farm%20equipments%20loan/tractor%20loan") {
        $('#PopLoan-Product').val('Farm New');
    }
    if (currURL.toLowerCase() === "/business-loan/msme-secured-business-loan" || currURL.toLowerCase() === "/business%20loan/msme%20secured%20business%20loan") {
        $('#PopLoan-Product').val('MSME Secured');
    }
    if (currURL.toLowerCase() === "/business-loan/msme-unsecured-business-loan" || currURL.toLowerCase() === "/business%20loan/msme%20unsecured%20business%20loan") {
        $('#PopLoan-Product').val('MSME Unsecured');
    }
    if (currURL.toLowerCase() === "/commercial-vehicle/3wheelers" || currURL.toLowerCase() === "/commercial%20vehicle/3wheelers") {
        $('#PopLoan-Product').val('CV New');
    }
    if (currURL.toLowerCase() === "/commercial-vehicle/small-commercial-vehicles" || currURL.toLowerCase() === "/commercial%20vehicle/small%20commercial%20vehicles") {
        $('#PopLoan-Product').val('CV New');
    }
    if (currURL.toLowerCase() === "/supply-chain-finance/dealer-and-distributer-finance" || currURL.toLowerCase() === "/supply%20chain%20finance/dealer%20and%20distributer%20finance") {
        $('#PopLoan-Product').val('Purchase Invoice Financing');
    }
    if (currURL.toLowerCase() === "/supply-chain-finance/vendor-finance" || currURL.toLowerCase() === "/supply%20chain%20finance/vendor%20finance") {
        $('#PopLoan-Product').val('Sales Invoice Financing');
    }
});

$(function() {
  $('#loantype').change(function(){
    $('.box').hide();
    $('.' + $(this).val()).show();
  });
});
//Change in new for emi calculator

// $('#businterestrate').click(function(){
	// alert('asdas');
	
// })
function show_value(x) {
    document.getElementById("slider_value").innerHTML = x;
}
function comcalculateEMI() {
    // Formula: 
    // EMI = (P * R/12) * [ (1+R/12)^N] / [ (1+R/12)^N-1]  
    // isNaN(isNotaNumber): Check whether the value is Number or Not                

    var emi = 0;
    var EmiPerLakh = 0;
    var Eligible = 0;
    var totintyear = 0;
    var totamount = 0;
    var totintamt = 0;
    //For Commercial Loan Start
    var P = document.getElementById("loanamount").value;
    var r = document.getElementById("interestrate").value;
    var n = document.getElementById("months").value;
    //For Commercial Loan End


    var per = parseFloat(r / 100);

    var EmiPerLakh = (P * (per / 12)) * (Math.pow(1 + (per / 12)), 1) / (1 - Math.pow((1 + (per / 12)), -1 * n));
    var totamount = (EmiPerLakh * n);
    var totintamt = (totamount - P);
    var totintyear = (totintamt / n);

    document.getElementById("lbMonthEmi").innerHTML = '' + Math.round(EmiPerLakh).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotamt").innerHTML = '' + Math.round(totamount).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintamt").innerHTML = '' + Math.round(totintamt).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintyear").innerHTML = '' + Math.round(totintyear).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtenure").innerHTML = '' + n;
    document.getElementById("lbinterest").innerHTML = '' + r;
    document.getElementById("lbprincipal").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");


    document.getElementById("lbtenure1").innerHTML = '' + n;
    document.getElementById("lbinterest1").innerHTML = '' + r;
    document.getElementById("lbprincipal1").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

}

function supcalculateEMI() {
    // Formula: 
    // EMI = (P * R/12) * [ (1+R/12)^N] / [ (1+R/12)^N-1]  
    // isNaN(isNotaNumber): Check whether the value is Number or Not                

    var emi = 0;
    var EmiPerLakh = 0;
    var Eligible = 0;
    var totintyear = 0;
    var totamount = 0;
    var totintamt = 0;

    //For Supply Chain Finance Start
    var P = document.getElementById("suploanamount").value;
    var r = document.getElementById("supinterestrate").value;
    var n = document.getElementById("supmonths").value;
    //For Supply Chain Finance End

    var per = parseFloat(r / 100);

    var EmiPerLakh = (P * (per / 12)) * (Math.pow(1 + (per / 12)), 1) / (1 - Math.pow((1 + (per / 12)), -1 * n));
    var totamount = (EmiPerLakh * n);
    var totintamt = (totamount - P);
    var totintyear = (totintamt / n);

    document.getElementById("lbMonthEmi").innerHTML = '' + Math.round(EmiPerLakh).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotamt").innerHTML = '' + Math.round(totamount).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintamt").innerHTML = '' + Math.round(totintamt).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintyear").innerHTML = '' + Math.round(totintyear).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");



    //For Supply Chain Finance Start
    document.getElementById("suptenure").innerHTML = '' + n;
    document.getElementById("supinterest").innerHTML = '' + r;
    document.getElementById("supprincipal").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //For Supply Chain Finance End

    document.getElementById("lbtenure1").innerHTML = '' + n;
    document.getElementById("lbinterest1").innerHTML = '' + r;
    document.getElementById("lbprincipal1").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

}

function buscalculateEMI() {
    // Formula: 
    // EMI = (P * R/12) * [ (1+R/12)^N] / [ (1+R/12)^N-1]  
    // isNaN(isNotaNumber): Check whether the value is Number or Not                

    var emi = 0;
    var EmiPerLakh = 0;
    var Eligible = 0;
    var totintyear = 0;
    var totamount = 0;
    var totintamt = 0;

    //For Business Loan Start
    var P = document.getElementById("busloanamount").value;
    var r = document.getElementById("businterestrate").value;
    var n = document.getElementById("busmonths").value;
    //For Business Loan End


    var per = parseFloat(r / 100);

    var EmiPerLakh = (P * (per / 12)) * (Math.pow(1 + (per / 12)), 1) / (1 - Math.pow((1 + (per / 12)), -1 * n));
    var totamount = (EmiPerLakh * n);
    var totintamt = (totamount - P);
    var totintyear = (totintamt / n);

    document.getElementById("lbMonthEmi").innerHTML = '' + Math.round(EmiPerLakh).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotamt").innerHTML = '' + Math.round(totamount).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintamt").innerHTML = '' + Math.round(totintamt).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintyear").innerHTML = '' + Math.round(totintyear).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");


    //For Business Loan Start
    document.getElementById("bustenure").innerHTML = '' + n;
    document.getElementById("businterest").innerHTML = '' + r;
    document.getElementById("busprincipal").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //For Business Loan End


    document.getElementById("lbtenure1").innerHTML = '' + n;
    document.getElementById("lbinterest1").innerHTML = '' + r;
    document.getElementById("lbprincipal1").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

}

function farmcalculateEMI() {
    // Formula: 
    // EMI = (P * R/12) * [ (1+R/12)^N] / [ (1+R/12)^N-1]  
    // isNaN(isNotaNumber): Check whether the value is Number or Not                

    var emi = 0;
    var EmiPerLakh = 0;
    var Eligible = 0;
    var totintyear = 0;
    var totamount = 0;
    var totintamt = 0;

    //For Farm Equipments Start
    var P = document.getElementById("feloanamount").value;
    var r = document.getElementById("feinterestrate").value;
    var n = document.getElementById("femonths").value;
    //For Farm Equipments End

    var per = parseFloat(r / 100);

    var EmiPerLakh = (P * (per / 12)) * (Math.pow(1 + (per / 12)), 1) / (1 - Math.pow((1 + (per / 12)), -1 * n));
    var totamount = (EmiPerLakh * n);
    var totintamt = (totamount - P);
    var totintyear = (totintamt / n);

    document.getElementById("lbMonthEmi").innerHTML = '' + Math.round(EmiPerLakh).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotamt").innerHTML = '' + Math.round(totamount).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintamt").innerHTML = '' + Math.round(totintamt).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    document.getElementById("lbtotintyear").innerHTML = '' + Math.round(totintyear).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");


    //For Farm Equipments Start
    document.getElementById("fetenure").innerHTML = '' + n;
    document.getElementById("feinterest").innerHTML = '' + r;
    document.getElementById("feprincipal").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //For Farm Equipments End

    document.getElementById("lbtenure1").innerHTML = '' + n;
    document.getElementById("lbinterest1").innerHTML = '' + r;
    document.getElementById("lbprincipal1").innerHTML = '' + Math.round(P).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

}

$(document).change(function () {
    $('#loantype').change(function () {
        $('.cv.box').removeClass('d-block');
    });
});
$(document).change(function () {
    $('#loantype').change(function () {
        $('.supply.box').removeClass('d-none');
    });
});
$(document).change(function () {
    $('#loantype').change(function () {
        $('.business.box').removeClass('d-none');
    });
});
$(document).change(function () {
    $('#loantype').change(function () {
        $('.farm.box').removeClass('d-none');
    });
});
$(function () {
    $('#zipcode').change(function () {

        var ZipValue = $(this).val();
        //var propName = $("#PropertyName option:selected").text();

        $.ajax({
            type: "GET",
            url: "/api/AdaniCapital/PincodeDetails",
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
            url: "/api/AdaniCapital/BranchLocation",
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
            url: "/api/AdaniCapital/BranchLocation",
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
            url: "/api/AdaniCapital/CentreLocation",
            data: { State: SelectedState, City: '' },
            success: function (obj) {
                if (obj !== null) {
                    $('#centre-City').text('');
                    $('#centre-City').append("<option value=''> Select City </option>");
                    $('#centre-info').empty();
                    $('#centre-info').append('<div><h4> Adani Capital Pvt.Ltd.</h4><p>One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051</p><p><a href="tel:18002100444"><span class="fa fa-phone fa-rotate-90 mr-2"></span> 1800-210-0444</a></p><p id="Branch-mail"><a href="mailto:info@adanicapital.com"><span class="fa fa-envelope mr-2"></span>  info@adanicapital.com</a></p></div>');
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
            url: "/api/AdaniCapital/CentreLocation",
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
    if (name == "") { $("#FeedbackName").siblings('p:first').html("Please enter your Name"); $("#FeedbackName").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#FeedbackEmailId").val();
    if (mailid == "") { $("#FeedbackEmailId").siblings('p:first').html("Email is Required"); $("#FeedbackEmailId").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#FeedbackMobile").val();
    if (ccontactno == "") { $("#FeedbackMobile").siblings('p:first').html("Please specify your Mobile Number"); $("#FeedbackMobile").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }

    if (ccontactno.length != 10) {
        $("#FeedbackMobile").siblings('p:first').html("Contact Number should be of 10 digit"); $("#FeedbackMobile").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false;
    }
    var message = $("#FeedbackMessage").val();
    if (message == "") { $("#FeedbackMessage").siblings('p:first').html("Please enter any message"); $("#FeedbackMessage").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var rating = $("#FeedbackRating").val();
    if (rating == "") { $("#FeedbackRating").siblings('p:first').html("Please rate us."); $("#FeedbackRating").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
    var formtype = $("#FeedbackformName").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { $("#FeedbackEmailId").siblings('p:first').html("Please enter valid email address"); $("#FeedbackEmailId").focus(); $('#btnFeedbackSubmit').removeAttr("disabled"); return false; }
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
        url: "/api/AdaniCapital/FeedbackForm",
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
    var response = grecaptcha.getResponse(ReqCallbackrecaptcha);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }

    $('#btnReqCallbackSubmit').attr("disabled", "disabled");
    var name = $("#ReqCallbackName").val();
    if (name == "") { $("#ReqCallbackName").siblings('p:first').html("Please enter your Name"); $("#ReqCallbackName").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#ReqCallbackEmailId").val();
    if (mailid == "") { $("#ReqCallbackEmailId").siblings('p:first').html("Email is Required"); $("#ReqCallbackEmailId").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#ReqCallbackMobile").val();
    if (ccontactno == "") { $("#ReqCallbackMobile").siblings('p:first').html("Please specify your Mobile Number"); $("#ReqCallbackMobile").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }

    if (ccontactno.length != 10) {
        $("#ReqCallbackMobile").siblings('p:first').html("Contact Number should be of 10 digit"); $("#ReqCallbackMobile").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false;

    }

    var message = $("#ReqCallbackMessage").val();
    if (message == "") { $("#ReqCallbackMessage").siblings('p:first').html("Please enter any message"); $("#ReqCallbackMessage").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }




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
        reResponse: response,
        FormName: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };
    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniCapital/RequestCallbackForm",
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
	$('#PopLoan-ApplyBtn').attr("disabled", "disabled");
    if (response.length == 0) {
        $("#ApplyPoprecaptcha").siblings('p:first').html("Captcha required.");
		$("#ApplyPoprecaptcha").focus();
		$('#PopLoan-ApplyBtn').removeAttr("disabled");
        return false;
    }
	else {
		$("#ApplyPoprecaptcha").siblings('p:first').empty();
	}    
    var fname = $("#PopLoan-fname").val();
    if (fname === "") { $("#PopLoan-fname").siblings('p:first').html("Please enter your First Name"); $("#PopLoan-fname").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
    var lname = $("#PopLoan-lname").val();
    if (lname === "") { $("#PopLoan-lname").siblings('p:first').html("Please enter your Last Name"); $("#PopLoan-lname").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
    var ccontactno = $("#PopLoan-Mobile").val();
    if (ccontactno === "") { $("#PopLoan-Mobile").siblings('p:first').html("Please specify your Mobile Number"); $("#PopLoan-Mobile").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }

    if (ccontactno.length !== 10) {
        $("#PopLoan-Mobile").siblings('p:first').html("Contact Number should be of 10 digit"); $("#PopLoan-Mobile").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false;
    }
    var mailid = $("#PopLoan-email").val();
    if (mailid !== "") { if (!validateEmail(mailid)) { $("#PopLoan-email").siblings('p:first').html("Please enter valid email address"); $("#cmailid").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; } }

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }
    var Pincode = $("#PopLoan-zipcode").val();
    if (Pincode === "") { $("#PopLoan-zipcode").siblings('p:first').html("Please enter pincode"); $("#PopLoan-zipcode").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
    var ProductName = $("#PopLoan-Product").val();
    if (ProductName == "") { $("#PopLoan-Product").siblings('p:first').html("Please select Product"); $("#PopLoan-fname").focus(); $('#PopLoan-ApplyBtn').removeAttr("disabled"); return false; }
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
        url: "/api/AdaniCapital/AdaniCapitalApplyForLoanPopForm",
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
                window.location.href = "https://stage.adanicapital.in/thankyou";
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
function validate(evt) {
  var theEvent = evt || window.event;

  // Handle paste
  if (theEvent.type === 'paste') {
      key = event.clipboardData.getData('text/plain');																	
  } else {
  // Handle key press
      var key = theEvent.keyCode || theEvent.which;
      key = String.fromCharCode(key);
  }
  var regex = /[0-9]|\./;
  if( !regex.test(key) ) {
    theEvent.returnValue = false;
    if(theEvent.preventDefault) theEvent.preventDefault();
  }
  $("input[name=MobileNo]").attr("maxlength", "10");
}
// disable mousewheel on a input number field when in focus
// (to prevent Cromium browsers change the value when scrolling)
$('form').on('focus', 'input[type=number]', function (e) {
  $(this).on('wheel.disableScroll', function (e) {
    e.preventDefault()
  })
});
$('form').on('blur', 'input[type=number]', function (e) {
  $(this).off('wheel.disableScroll')
});
$('input[type="text"]').on('input', function(e) {
    var regKey = $(this).attr('data-val-regex-pattern');
	var regMsg = $(this).attr('data-val-regex');
	var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
			$(this).siblings('p:first').html(ValMsg);
		}
		else if (!$(this).val().match(regKey)){
				$(this).siblings('p:first').empty();
				$(this).siblings('p:first').html(regMsg);
			}
			else{
				$(this).siblings('p:first').empty();
			}
	}		
});
$('input[type="text"]').on('blur', function(e) {
    var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
		$(this).siblings('p:first').html(ValMsg);
		}
		else{
			$(this).siblings('p:first').empty();
		}		
	}	
});

$('input[type="email"]').on('input', function(e) {
    var regKey = $(this).attr('data-val-regex-pattern');
	var regMsg = $(this).attr('data-val-regex');
	var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
			$(this).siblings('p:first').html(ValMsg);
		}
		else if (!$(this).val().match(regKey)){
				$(this).siblings('p:first').empty();
				$(this).siblings('p:first').html(regMsg);
			}
			else{
				$(this).siblings('p:first').empty();
			}
	}		
});
$('input[type="email"]').on('blur', function(e) {
    var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
		$(this).siblings('p:first').html(ValMsg);
		}
		else{
			$(this).siblings('p:first').empty();
		}		
	}	
});

$('input[type="number"]').on('input', function(e) {
    var regKey = $(this).attr('data-val-regex-pattern');
	var regMsg = $(this).attr('data-val-regex');
	var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
			$(this).siblings('p:first').html(ValMsg);
		}
		else if (!$(this).val().match(regKey)){
				$(this).siblings('p:first').empty();
				$(this).siblings('p:first').html(regMsg);
			}
			else{
				$(this).siblings('p:first').empty();
			}
	}		
});
$('input[type="number"]').on('blur', function(e) {
    var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
		$(this).siblings('p:first').html(ValMsg);
		}
		else{
			$(this).siblings('p:first').empty();
		}		
	}	
});

$('input[type="date"]').on('input', function(e) {
    var regKey = $(this).attr('data-val-regex-pattern');
	var regMsg = $(this).attr('data-val-regex');
	var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
			$(this).siblings('p:first').html(ValMsg);
		}
		else if (!$(this).val().match(regKey)){
				$(this).siblings('p:first').empty();
				$(this).siblings('p:first').html(regMsg);
			}
			else{
				$(this).siblings('p:first').empty();
			}
	}		
});
$('input[type="date"]').on('blur', function(e) {
    var ValKey = $(this).attr('required');
	var ValMsg = $(this).attr('data-val-required');
	if ($(this).prop('required')){
		if($(this).val()==''){
			$(this).siblings('p:first').empty();
		$(this).siblings('p:first').html(ValMsg);
		}
		else{
			$(this).siblings('p:first').empty();
		}		
	}	
});


$(window).scroll(function(){
    if ($(this).scrollTop() > 750) {
       $('.siderbar_fixed').addClass('active');
	$('#ymPluginDivContainerInitial').addClass('active');
	$('#back-to-top').addClass('show');
    } else {
       $('.siderbar_fixed').removeClass('active');
$('#ymPluginDivContainerInitial').removeClass('active');
$('#back-to-top').removeClass('show');
    }
});
