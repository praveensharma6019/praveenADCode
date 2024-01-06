$(document).ready(function () {

    $('.hero-banner').owlCarousel({
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

    $('.video-modal').owlCarousel({
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
    $('.latest-news').owlCarousel({
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

});

$('#nav-button').click(function () {
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
});
$('.collapse_toggle').click(function () {
    $('.navbar-collapse').removeClass('show');
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
});

$('.fade-overlay').click(function () {
    $('.navbar-collapse').removeClass('show');
    $('.fade-overlay').toggleClass('active');
    $('body').toggleClass('overflow-hidden');
    $('#back-to-top').toggleClass('d-none');
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

// Read more read less on click
$(".btn-readmore").click(function () {
    $(this).parent().parent().addClass("all-visible");
});
$(".btn-readless").click(function () {
    $(this).parent().parent().removeClass("all-visible");
});

var urlhome = $("#v-3").attr('src');
$("#about-video").on('hide.bs.modal', function () {
    $("#v-3").attr('src', '');
});

/* Assign the initially stored url back to the iframe src
attribute when modal is displayed again */
$("#about-video").on('show.bs.modal', function () {
    $("#v-3").attr('src', urlhome);
});

var urlabout = $("#v-2").attr('src');
$("#home-video").on('hide.bs.modal', function () {
    $("#v-2").attr('src', '');
});

/* Assign the initially stored url back to the iframe src
attribute when modal is displayed again */
$("#home-video").on('show.bs.modal', function () {
    $("#v-2").attr('src', urlabout);
});	