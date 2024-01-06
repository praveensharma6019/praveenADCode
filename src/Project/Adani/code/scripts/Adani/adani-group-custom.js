$(document).ready(function () {

    $(".close").on("click", function () {
        jQuery('#video iframe').attr("src", jQuery('#video iframe').attr("src"));
    })
    /*Home Video Slider*/
    var owl = $('.homeslider');
    owl.owlCarousel({
        loop: false,
        margin: 0,
        nav: false,
        dots: false,
        autoHeight: true,
        lazyload: true,
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

    // Fakes the loading setting a timeout
    setTimeout(function () {
        $('body').addClass('loaded');
    }, 1500);


    // Hide Header on scroll down
    var didScroll;
    var lastScrollTop = 0;
    var delta = 5;
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
            $('.navPanel').addClass('sticky-header').css('top', '0px');;
        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {
                $('.headerTopBar').addClass('nav-down');
                $('.navPanel').css('top', '35px');
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
    $('#footerArrow').click(function () {
        $('.footerPanel2 .mobile-none').toggleClass('d-none');
        $('.footerPanel2 .footerpanel-1').toggle(100);
        $('.footerPanel2 .txt-center').toggle(100);
        $('.footerPanel2 .ft-submenu').toggleClass('col-lg-12');
    });

    if ($(window).width() <= 768) {
        $("#loadMore").text('Load more');
        $(function () {
            $(".business .col-lg-3").slice(0, 4).show();
            var cnt = 0;
            $("#loadMore").on('click', function (e) {
                cnt = cnt + 1;
                $(".business .col-lg-3:hidden").slice(0, 4).slideDown('slow', function () {
                    if (cnt == 2) {
                        $("#loadMore");
                        $("#loadMore").text('Explore more');
                        $("#loadMore").attr('href', 'businesses.html');
                    }

                });


            });
        });
    }

    $('#sustanibility').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        nav: true,
        dots: false,
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

    $('#mainslider').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,

        nav: true,
        dots: false,
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

    $('#resources').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        nav: false,
        dots: true,
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
                items: 2
            },
            1000: {
                items: 2
            }
        }
    })

    $('#other-ventures').owlCarousel({
        loop: true,
        margin: 30,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplay: true,
        responsiveClass: true,
        responsive: {
            0: {
                items: 2,
                nav: true,
                dots: false
            },

            300: {
                items: 2,
                nav: false,
                dots: false
            },
            420: {
                items: 3,
                nav: false,
                dots: false
            },
            576: {
                items: 4,
                nav: false,
                dots: true
            },
            768: {
                items: 4,
                nav: false,
                dots: true
            },
            1000: {
                items: 6,
                nav: false,
                loop: false
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


    $(".nav-sublink")
        .on("mouseenter", function (e) {
            $(".menu-thumb").children()[0].style.display = "none";
            for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
                if ($(".menu-thumb").children()[i].alt == $(e.currentTarget).children()[0].text) {
                    $(".menu-thumb").children()[i].style.display = "block";
                }
                else {
                    $(".menu-thumb").children()[i].style.display = "none";
                }
            }
        })
        .on("mouseleave", function (e) {
            for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
                $(".menu-thumb").children()[i].style.display = "none";
            }
            $(".menu-thumb").children()[0].style.display = "block";
        });
})