function getvalue() {
    var Building = $("#Building").val();
    var Floor = $("#Floor").val();
    //var Unit = $("#Unit").val();
    jQuery.ajax(
        {
            url: "/api/Affordable/GetBuildings?id=" + window.location.href.substr(window.location.href.lastIndexOf('/') + 15),
            method: "GET",
            data: {
                building: Building,
                //floor: Floor,
                //units: Unit
            },
            success: function (data) {
                $("#myPartialViewContainer").html("");
                $("#myPartialViewContainer").html(data);
            }
        });
}
function getfloor() {
    var Building = $("#Building").val();
    var Floor = $("#Floor").val();
    //var Unit = $("#Unit").val();
    jQuery.ajax(
        {
            url: "/api/Affordable/GetUFloor?id=" + window.location.href.substr(window.location.href.lastIndexOf('/') + 15),
            method: "GET",
            data: {
                building: Building,
                floor: Floor,
                //units: Unit
            },
            success: function (data) {
                $("#myPartialViewContainer").html("");
                $("#myPartialViewContainer").html(data);
            }
        });
}
function GetAmt() {
    var Building = $("#Building").val();
    var Floor = $("#Floor").val();
    var Unit = $("#Unit").val();
    jQuery.ajax(
        {
            url: "/api/Affordable/SearchProperties?id=" + window.location.href.substr(window.location.href.lastIndexOf('/') + 15),
            method: "GET",
            data: {
                building: Building,
                floor: Floor,
                units: Unit
            },
            success: function (data) {
                $("#myPartialViewContainer").html("");
                $("#myPartialViewContainer").html(data);
            }
        });
}

$(document).ready(function () {

    // Hide Header on scroll down
    var didScroll;
    var lastScrollTop = 0;
    var delta = 1;
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
        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down
            $('#back-to-top').fadeIn();
            $('.headerTopBar').removeClass('nav-down');
            $('.navPanel').addClass('sticky-header');
        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {
                $('.headerTopBar').addClass('nav-down');
                $('.navPanel');
            }
        }
        if (st == 0) {
            $('.headerTopBar').removeClass('nav-down');
            $('.navPanel').removeClass('sticky-header');
            $('#back-to-top').hide();
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



    /*Home Video Slider*/
    var owl = $('.homeslider');
    owl.owlCarousel({
        loop: false,
        margin: 0,
        nav: false,
        dots: false,
        autoHeight: true,
        items: 1
    })
    owl.on('translate.owl.carousel', function (e) {
        $('.homeslider .owl-item video').each(function () {
            $(this).get(0).pause();
        });
    });
    $('.homeslider .owl-item .item').each(function () {
        var attr = $(this).attr('data-videosrc');
        if (typeof attr !== typeof undefined && attr !== false) {
            var videosrc = $(this).attr('data-videosrc');
            $(this).prepend('<video muted><source src="' + videosrc + '" type="video/mp4"></video>');
            $('.homeslider .owl-item.active video').attr('autoplay', true).attr('loop', true);
        }
    });
    owl.on('translated.owl.carousel', function (e) {
        $('.homeslider .owl-item.active video').get(0).play();
    });

    $('#our-projects').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 2000,
        dots: false,
        nav: false,
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3,
                nav: false,
                loop: false,
                margin: 20,
                dots: true,
                touchDrag: false,
                mouseDrag: false,
            }
        }
    })

    $('.gallery-slider').owlCarousel({
        loop: true,
        margin: 10,
        animateOut: 'fadeOut',
        slideSpeed: 2500,
        responsiveClass: true,
        autoplayTimeout: 1000,
        autoplay: true,
        dots: true,
        nav: false,
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
    })


    $('#career-carousel').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
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
    })

    $('#property-inner').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
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
    })

    $('#media-coverage').owlCarousel({
        loop: true,
        margin: 70,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 3
            },
            1000: {
                items: 4
            }
        }
    })


    $('#state-infra').owlCarousel({
        loop: false,
        margin: 10,
        dots: false,
        nav: false,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
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
    })

    $('#projectstatus').owlCarousel({
        loop: false,
        margin: 10,
        dots: false,
        nav: false,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
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
    })

    $('.our-brands').owlCarousel({
        loop: true,
        margin: 50,
        dots: false,
        nav: false,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 3
            },
            1000: {
                items: 5
            }
        }
    })

    /* Initialize Latest Projects Carousel on home page */
    $("#commPrjtsCarousel").owlCarousel({
        nav: true,
        center: true,
        loop: true,
        margin: 30,
        navText: [],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 2
            },
            // breakpoint from 768 up
            768: {
                items: 3
            }
        }
    });

    /* Initialize Latest Projects Carousel on home page */
    $(".testimonial").owlCarousel({
        nav: false,
        center: true,
        loop: false,
        margin: 0,
        autoplay: true,
        autoplaytimeout: 2000,
        slideSpeed: 1500,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 1
            },
            // breakpoint from 768 up
            768: {
                items: 1
            }
        }
    });

    $('#testimonials').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: true,
                nav: false
            },
            600: {
                items: 2,
                nav: false,
                dots: true,
                nav: false
            },
            1000: {
                items: 2,
                nav: false,
                dots: true,
                loop: false,
                margin: 20,

            }
        }
    })

    $('#accolades').owlCarousel({
        loop: true,
        margin: 10,
        dots: false,
        responsiveClass: true,
        responsive: {
            0: {
                items: 2,
                nav: false
            },
            600: {
                items: 3,
                nav: false
            },
            1000: {
                items: 4,
                nav: false,
                loop: false,
                margin: 20
            }
        }
    })

    /* partners Carousel For about us */
    $("#partners-carousel").owlCarousel({
        loop: true,
        margin: 10,
        autoplay: true,
        autoplayTimeout: 2000,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 3,
                nav: false,
                dots: false
            },
            // breakpoint from 480 up
            480: {
                items: 3,
                nav: false,
                dots: false
            },
            600: {
                items: 4,
                nav: false,
                dots: false
            },
            // breakpoint from 768 up
            991: {
                items: 6,
                nav: false,
                dots: false
            },
            1100: {
                items: 8,
                nav: false,
                dots: false
            }
        }
    })

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

    /*Team tiles*/
    $('#team').on("click", function () {
        $('#team-tile').slideDown(700, function () {
            $('#team-tile').show(700, "");
            $('.accoladess').hide(500);
        });
    });
    $('#teams-tiles-close').on("click", function () {
        $('#team-tile').slideDown(700, function () {
            $('#team-tile').hide(700, "");
        });
    });
    $('#accoladess').on("click", function () {
        $('.accoladess').slideDown(700, function () {
            $('.accoladess').show(700, "");
            $('#team-tile').hide(400, "");
        });
    });

    $('.submenu > div > ul > li > a').on('click', function (e) {
        var href = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(href).offset().top - 50
        }, '600');
        e.preventDefault();

    });
})

//$(".noreferrer").on("click", function () {
//    var link = window.location.href.substr(window.location.href.lastIndexOf('/') + 1);
//    var currentlink = $(".noreferrer")[0].href + "?id=" + link;
//    window.open(currentlink, '_blank');
//});
/*Gallery Popup*/
$(document).ready(function () {
    $(".popup").magnificPopup({
        type: "image",
        removalDelay: 160,
        preloader: false,
        fixedContentPos: true,
        gallery: {
            enabled: true
        }
    });
});


