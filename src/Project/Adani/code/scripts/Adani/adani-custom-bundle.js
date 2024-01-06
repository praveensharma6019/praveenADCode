
/*carousle - slider.js*/


var primary = $("#carousel");
var secondary = $("#thumbnails");

$(document).ready(function () {
    if ($("#carousel").length > 0) {
        primary.owlCarousel({
            items: 1,
            nav: true,
            singleItem: true,
            slideSpeed: 1000,
            pagination: false,
            afterAction: syncPosition,
            responsiveRefreshRate: 200,
            navigation: true,
            navigationText: ["", ""]
        });
    }
   
    if ($("#thumbnails").length > 0) {
        secondary.owlCarousel({
            items: 5,

            itemsDesktop: [1200, 8],
            itemsDesktopSmall: [992, 7],
            itemsTablet: [768, 6],
            itemsMobile: [480, 4],
            pagination: false,
            responsiveRefreshRate: 100,
            navigation: true,
            navigationText: ["", ""],
            afterInit: function (el) {
                el.find(".owl-item").eq(0).addClass("synced");
            }
        });
    }

    function syncPosition(el) {
        var current = this.currentItem;
        secondary.find(".owl-item").removeClass("synced").eq(current).addClass("synced");
        if (secondary.data("owlCarousel") !== undefined) {
            center(current);
        }
        $('.current-item').html(this.owl.currentItem + 1);
        $('.max-items').html(this.owl.owlItems.length);
    }

    secondary.on("click", ".owl-item", function (e) {
        e.preventDefault();
        var number = $(this).data("owlItem");
        primary.trigger("owl.goTo", number);
    });

    function center(number) {
        var sync2visible = secondary.data("owlCarousel").owl.visibleItems;
        var num = number;
        var found = false;
        for (var i in sync2visible) {
            if (num === sync2visible[i]) {
                var found = true;
            }
        }

        if (found === false) {
            if (num > sync2visible[sync2visible.length - 1]) {
                secondary.trigger("owl.goTo", num - sync2visible.length + 2);
            } else {
                if (num - 1 === -1) {
                    num = 0;
                }
                secondary.trigger("owl.goTo", num);
            }
        } else if (num === sync2visible[sync2visible.length - 1]) {
            secondary.trigger("owl.goTo", sync2visible[1]);
        } else if (num === sync2visible[0]) {
            secondary.trigger("owl.goTo", num - 1);
        }
    }
});

$(".collapse-button").click(function () {
    var thumbnailsWrapper = $('.thumbnails-wrapper');
    if (thumbnailsWrapper.position().top < thumbnailsWrapper.parent().height() - 1) {
        thumbnailsWrapper.animate({ bottom: '-' + thumbnailsWrapper.outerHeight() + 'px' });
        thumbnailsWrapper.find('.icon').addClass('-flip');
    }
    else {
        thumbnailsWrapper.animate({ bottom: '0' });
        thumbnailsWrapper.find('.icon').removeClass('-flip');
    }
});

/*owl - thumbslider.js*/

$(document).ready(function () {
    var bigimage = $("#big");
    var thumbs = $("#thumbs");
    //var totalslides = 10;
    var syncedSecondary = true;
    if (bigimage.length > 0) {
        bigimage
            .owlCarousel({
                items: 1,
                slideSpeed: 2000,
                nav: false,
                autoplay: true,
                dots: true,
                animateOut: 'fadeOut',
                loop: true,
                responsiveRefreshRate: 200,
                navText: [
                    '<i class="fa fa-arrow-left" aria-hidden="true"></i>',
                    '<i class="fa fa-arrow-right" aria-hidden="true"></i>'
                ]
            })
            .on("changed.owl.carousel", syncPosition);
    }
 
    if (thumbs.length > 0) {
        thumbs
            .on("initialized.owl.carousel", function () {
                thumbs
                    .find(".owl-item")
                    .eq(0)
                    .addClass("current");
            })
            .owlCarousel({
                items: 2,
                margin: 20,
                dots: false,
                nav: true,
                navText: [
                    '<i class="fa fa-arrow-left" aria-hidden="true"></i>',
                    '<i class="fa fa-arrow-right" aria-hidden="true"></i>'
                ],
                smartSpeed: 200,
                slideSpeed: 500,
                margin: 10,
                nav: false,
                slideBy: 4,
                responsiveRefreshRate: 100
            })
            .on("changed.owl.carousel", syncPosition2);
    }

    function syncPosition(el) {
        //if loop is set to false, then you have to uncomment the next line
        //var current = el.item.index;

        //to disable loop, comment this block
        var count = el.item.count - 1;
        var current = Math.round(el.item.index - el.item.count / 2 - 0.5);

        if (current < 0) {
            current = count;
        }
        if (current > count) {
            current = 0;
        }
        //to this
        thumbs
            .find(".owl-item")
            .removeClass("current")
            .eq(current)
            .addClass("current");
        var onscreen = thumbs.find(".owl-item.active").length - 1;
        var start = thumbs
            .find(".owl-item.active")
            .first()
            .index();
        var end = thumbs
            .find(".owl-item.active")
            .last()
            .index();

        if (current > end) {
            thumbs.data("owl.carousel").to(current, 100, true);
        }
        if (current < start) {
            thumbs.data("owl.carousel").to(current - onscreen, 100, true);
        }
    }

    function syncPosition2(el) {
        if (syncedSecondary) {
            var number = el.item.index;
            bigimage.data("owl.carousel").to(number, 100, true);
        }
    }

    thumbs.on("click", ".owl-item", function (e) {
        e.preventDefault();
        var number = $(this).index();
        bigimage.data("owl.carousel").to(number, 300, true);
    });
});



/*adani-group-custom.js*/

$('.click-scroll a').on('click', function (e) {
    var href = $(this).attr('href');
    $('html, body').animate({
        scrollTop: $(href).offset().top - 70
    }, '600');
    e.preventDefault();
});

$('.scroll-tabs a').on('click', function (e) {
    var href = $(this).attr('href');
    $('html, body').animate({
        scrollTop: $(href).offset().top - 130
    }, '600');
    e.preventDefault();
});



$(function () {
    $('#investor-select select').change(function () {
        $('.investor-m-block').hide();
        $('.inv-tabs-query').hide();
        $('.tab-inv').hide();
        $('.' + $(this).val()).show();
    });
});

$(window).scroll(function () {
    if ($(this).scrollTop() > 200) {
    } else {
        $('.headerSec .fixed-top').removeClass('sticky-header');
    }
});

function CloseModal(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
}

$('body').on('hidden.bs.modal', '.modal', function () {
    $('video').trigger('pause');
});

$(".view-all video").each(function () {
    $(this).get(0).pause();
});
$('.ytpop').on('click', function (e) {
    var href = $(this).attr('data-value');
    $('#video-01 iframe').attr("src", href);
    e.preventDefault();
});

function CloseModal2() {
    $('#video-01 iframe').attr("src", '');
}

$(document).ready(function () {

    var owl = $('.homeslider');
    owl.owlCarousel({
        loop: true,
        margin: 0,
        lazyLoad: true,
        nav: false,
        dots: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        autoHeight: true,
        autoplay: true,
        autoplayTimeout: 6800,
        slideSpeed: 0,
        lazyload: true,
        items: 1
    })
    owl.on('translate.owl.carousel', function (e) {
        $('.homeslider .owl-item video').each(function () {
            $(this).get(0).pause();
            $(this).get(0).currentTime = 0;
        });
    });
    $('.homeslider .owl-item .item').each(function () {
        var attr = $(this).attr('data-videosrc');
        if (typeof attr !== typeof undefined && attr !== false) {
            var videosrc = $(this).attr('data-videosrc');
            $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="' + videosrc + '" type="video/mp4"></video>');
            $('.homeslider .owl-item.active video');
        }
    });
    owl.on('translated.owl.carousel', function (e) {
        $('.homeslider .owl-item.active video').get(0).play();
    });

    $(".homeslider").carousel({ interval: false }); // this prevents the auto-loop
    var videos = document.querySelectorAll("video");
    videos.forEach(function (e) {
        e.addEventListener('ended', myHandler, false);
    });
    function myHandler(e) {
        $(".homeslider").carousel('next');
    }
});

$(document).ready(function () {
    $('.sitemap-link').click(function () {
        $('.ft-mobilemenu').toggleClass('d-none').animate({ scrollTop: $('.ft-mobilemenu').offset().top - 70 }, '600');
        $('.sitemap-link').toggleClass('sitemap-link-active');
    });

    $('body').on('hidden.bs.modal', '.modal', function () {
        $('video').trigger('pause');
    });
    $('.topMenu .dropdown').click(function () {
        $(this).toggleClass('active');
    });
    $('.topMenu .dropdown a').click(function () {
        $('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');
    });
    $('.topMenu .dropdown').click(function () {
        $(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');
    });


    $('.scroll-down a').on('click', function (e) {
        var href = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(href).offset().top - 70
        }, '600');
        e.preventDefault();
    });

    $('#other-ventures').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
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
    })

    $('#blog-more').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false
            },
            600: {
                items: 2,
                nav: true,
                dots: false
            },
            1000: {
                items: 2,
                nav: true,
                dots: false,
                margin: 20
            }
        }
    })


    $('.market_ticker1').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        items: 1,
        nav: true,
        dots: false
    })

    // Fakes the loading setting a timeout
    setTimeout(function () {
        $('body').addClass('loaded');
    }, 1500);


    // /*Home Video Slider*/
    // var owl = $('.homeslider');
    // owl.owlCarousel({
    // loop:true,
    // margin:0,
    // lazyLoad: true,
    // nav:false,
    // dots:true,
    // animateOut: 'fadeOut',
    // animateIn: 'fadeIn',
    // autoHeight:true,
    // autoplay: true,
    // autoplayTimeout: 12620,
    // slideSpeed: 3000,
    // lazyload:true,
    // items:1
    // })
    // owl.on('translate.owl.carousel', function (e) {
    // $('.homeslider .owl-item video').each(function () {
    // $(this).get(0).pause();
    // });
    // });
    // $('.homeslider .owl-item .item').each(function () {
    // var attr = $(this).attr('data-videosrc');
    // if (typeof attr !== typeof undefined && attr !== false) {
    // var videosrc = $(this).attr('data-videosrc');
    // $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="' + videosrc + '" type="video/mp4"></video>');
    // $('.homeslider .owl-item.active video').attr('autoplay', true).attr('loop', true);
    // }
    // });
    // owl.on('translated.owl.carousel', function (e) {
    // $('.homeslider .owl-item.active video').get(0).play();
    // });


    /*Inner Video Slider*/
    var owl = $('.innerv-carousel');
    owl.owlCarousel({
        loop: false,
        margin: 0,
        nav: false,
        dots: false,
        touchDrag: false,
        mouseDrag: false,
        autoHeight: true,
        autoplay: true,
        autoplayTimeout: 1500,
        lazyload: true,
        items: 1
    })
    owl.on('translate.owl.carousel', function (e) {
        $('.innerv-carousel .owl-item video').each(function () {
            $(this).get(0).pause();
        });
    });
    $('.innerv-carousel .owl-item .item').each(function () {
        var attr = $(this).attr('data-videosrc');
        if (typeof attr !== typeof undefined && attr !== false) {
            var videosrc = $(this).attr('data-videosrc');
            $(this).prepend('<video muted><source src="' + videosrc + '" type="video/mp4"></video>');
            $('.innerv-carousel .owl-item.active video').attr('autoplay', true).attr('loop', true);
        }
    });
    owl.on('translated.owl.carousel', function (e) {
        $('.innerv-carousel .owl-item.active video').get(0).play();
    });

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
                $('.navPanel').css('top', '0px');
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

    //for newsroom
    $(function () {
        $('#btn-mrelease').change(function () {
            $('.media-r').hide();
            $('.m-loadMore').hide();
            $('#' + $(this).val()).show();
        });
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
                    if (cnt == 4) {
                        $("#loadMore");
                        $("#loadMore").text('Explore more');
                        $("#loadMore").attr('href', 'businesses');
                    }
                });
            });
        });
    }
    else {
        $("#loadMore").attr('href', 'businesses');
    }

    $('#sustanibility').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        nav: false,
        dots: false,
        autoplay: false,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1,
                nav: true
            },
            600: {
                items: 2,
                nav: true
            },
            1000: {
                items: 3,
                touchDrag: false,
                mouseDrag: false,
            }
        }
    }),
        $('.covid-carousel').owlCarousel({
            loop: true,
            margin: 10,
            nav: false,
            autoplay: true,
            autoplayTimeout: 5000,
            animateOut: 'fadeOut',
            animateIn: 'fadeIn',
            dots: true,
            responsiveClass: true,
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
        }),

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

    // $('#other-ventures').owlCarousel({
    // loop: true,
    // margin: 30,
    // autoplaytimeout: 2000,
    // animateOut: 'fadeOutLeft',
    // animateIn: 'fadeInRight',
    // slideSpeed: 1500,
    // autoplay: true,
    // responsiveClass: true,
    // responsive: {
    // 0: {
    // items: 2,
    // nav: true,
    // dots: false
    // },

    // 300: {
    // items: 2,
    // nav: false,
    // dots: false
    // },
    // 420: {
    // items: 3,
    // nav: false,
    // dots: false
    // },
    // 576: {
    // items: 4,
    // nav: false,
    // dots: true
    // },
    // 768: {
    // items: 4,
    // nav: false,
    // dots: true
    // },
    // 1000: {
    // items: 6,
    // nav: false,
    // loop: false
    // }
    // }
    // })


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
    })
    $('#about-banner').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 2000,
        autoplay: true,
        autoHeight: true,
        touchDrag: false,
        mouseDrag: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
            },
            960: {
                items: 1,
                nav: false,
                dots: false,
            },
            1100: {
                items: 1,
                nav: true,
                dots: false,
            }
        }
    })
    $('#pop-gallery').owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
        dots: true,
        autoplay: true,
        autoplayTimeout: 2500,
        responsiveClass: true,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        slideSpeed: 1000,
        responsive: {
            0: {
                items: 1,
                nav: true
            },
            600: {
                items: 3,
                nav: true
            },
            1000: {
                items: 3,
                nav: true
            }
        }
    });

    $(".popup").magnificPopup({
        type: "image",
        removalDelay: 160,
        preloader: false,
        fixedContentPos: true,
        gallery: {
            enabled: true
        }
    });


    $(function () {
        $('#btn-mrelease').change(function () {

            $("#business-parent .col-sm-6").hide();
            $('#business-parent .' + $(this).val()).show();

        });
    });




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

    });

    $(".loadmore-content p").slice(0, 1).show();

    $(".loadMore").on("click", function (e) {

        e.preventDefault();

        $(".loadMore").toggleClass("d-block d-none");

        $(".loadmore-content p").toggleClass("d-none");
        $(".loadmore-content ul").toggleClass("d-none");

        $("p.loadMore").text("").show(1000);

        $(".arrow.loadMore").text("Read more ").show(1000);

        $('<i class="fa fa-chevron-down"/>').appendTo('.arrow.loadMore');

        if ($(".loadmore-content p:hidden").length == 0) {

            $(".arrow.loadMore").text("Read less ").show(1000);

            $(' <i class="fa fa-chevron-up"/>').appendTo('.arrow.loadMore');

            $(".arrow.loadMore").show(1000);

        }



    });

    $('.viewall').click(function () {
        $('.ft-business li').removeClass('d-none');
        $('.viewall').addClass('d-none');
    });








    $("a.dropdown-item")
        .on("mouseenter", function (e) {
            if ($(".menu-thumb").length > 0) {
                $(".menu-thumb").children()[0].style.display = "none";
                for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
                    if ($(".menu-thumb").children()[i].alt == $(e.currentTarget).text()) {
                        $(".menu-thumb").children()[i].style.display = "block";
                    }
                    else {
                        $(".menu-thumb").children()[i].style.display = "none";
                    }
                }
            }

        })
        .on("mouseleave", function (e) {
            if ($(".menu-thumb").length > 0) {
                for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
                    $(".menu-thumb").children()[i].style.display = "none";
                }
                $(".menu-thumb").children()[0].style.display = "block";
            }
        });

    $('.single-item').owlCarousel({
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
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    }),
        $('.vision-c').owlCarousel({
            loop: false,
            margin: 0,
            responsiveClass: true,
            autoplay: true,
            nav: true,
            dots: false,
            autoplaytimeout: 800,
            autoplayHoverPause: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
            responsive: {
                0: {
                    items: 1
                },
                500: {
                    items: 1
                },
                767: {
                    items: 1
                },
                768: {
                    items: 2
                },
                1000: {
                    items: 3
                },
                1200: {
                    items: 4
                }
            }
        }),
        $('.three-item').owlCarousel({
            loop: false,
            margin: 20,
            responsiveClass: true,
            autoplay: true,
            nav: true,
            dots: false,
            autoplaytimeout: 800,
            autoplayHoverPause: true,
            navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
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
        }),
        $('a.btn-gallery').on('click', function (event) {
            event.preventDefault();

            var gallery = $(this).attr('href');

            $(gallery).magnificPopup({
                delegate: 'a',
                type: 'image',
                gallery: {
                    enabled: true
                }
            }).magnificPopup('open');
        });


})

$("#adanienterprisesMenu").click(function () {
    $($('.tabs-section li a[href="#incubation"]')).trigger('click');
    $($('.power ul li a[href="#1"]')).trigger('click');


});

$("#adanigasMenu").click(function () {
    $($('.tabs-section li a[href="#poweramputilities"]')).trigger('click');
    $($('.power ul li a[href="#6"]')).trigger('click');


});


$("#adanitransmissionMenu").click(function () {
    $($('.tabs-section li a[href="#poweramputilities"]')).trigger('click');
    $($('.power ul li a[href="#4"]')).trigger('click');


});

$("#adaniportMenu").click(function () {
    $($('.tabs-section li a[href="#transportamplogistics"]')).trigger('click');
    $($('.power ul li a[href="#2"]')).trigger('click');


});

$("#adanigreenMenu").click(function () {
    $($('.tabs-section li a[href="#poweramputilities"]')).trigger('click');
    $($('.power ul li a[href="#5"]')).trigger('click');


});

$("#adanipowerMenu").click(function () {
    $($('.tabs-section li a[href="#poweramputilities"]')).trigger('click');
    $($('.power ul li a[href="#3"]')).trigger('click');


});

/* Assign empty url value to the iframe src attribute when
    modal hide, which stop the video playing */
var url1 = $("#v-1").attr('src');
$("#video-1").on('hide.bs.modal', function () {
    $("#v-1").attr('src', '');
});

/* Assign the initially stored url back to the iframe src
attribute when modal is displayed again */
$("#video-1").on('show.bs.modal', function () {
    $("#v-1").attr('src', url1);
});

/* Assign empty url value to the iframe src attribute when
    modal hide, which stop the video playing */
var url2 = $("#v-2").attr('src');
$("#video-2").on('hide.bs.modal', function () {
    $("#v-2").attr('src', '');
});

/* Assign the initially stored url back to the iframe src
attribute when modal is displayed again */
$("#video-2").on('show.bs.modal', function () {
    $("#v-2").attr('src', url2);
});

/* Assign empty url value to the iframe src attribute when
    modal hide, which stop the video playing */
var url3 = $("#v-3").attr('src');
$("#video-3").on('hide.bs.modal', function () {
    $("#v-3").attr('src', '');
});

/* Assign the initially stored url back to the iframe src
attribute when modal is displayed again */
$("#video-3").on('show.bs.modal', function () {
    $("#v-3").attr('src', url3);
});

/* Assign empty url value to the iframe src attribute when
    modal hide, which stop the video playing */
var url4 = $("#v-4").attr('src');
$("#video-4").on('hide.bs.modal', function () {
    $("#v-4").attr('src', '');
});

/* Assign the initially stored url back to the iframe src
attribute when modal is displayed again */
$("#video-4").on('show.bs.modal', function () {
    $("#v-4").attr('src', url4);
});

/* Assign empty url value to the iframe src attribute when
    modal hide, which stop the video playing */
var url5 = $("#v-5").attr('src');
$("#video-5").on('hide.bs.modal', function () {
    $("#v-5").attr('src', '');
});

/* Assign the initially stored url back to the iframe src
attribute when modal is displayed again */
$("#video-5").on('show.bs.modal', function () {
    $("#v-5").attr('src', url5);
});

/*Sticky Sustainability tabs*/
var stab = $('.scroll-tabs');
if (stab.length) {
    var stickyOffset = $('.scroll-tabs').offset().top;

    $(window).scroll(function () {
        if ($(window).scrollTop() >= stickyOffset) {
            $('.scroll-tabs').addClass('fixed');
        }
        else {
            $('.scroll-tabs').removeClass('fixed');
        }
    });
}


/*Sticky Sustainability tabs*/

let pages = document.querySelectorAll(".section_sus");
let nav = document.querySelectorAll(".scroll-tabs a");

function getTopOfElement(element) {
    return (
        element.getBoundingClientRect().top -
        document.body.getBoundingClientRect().top - 200
    );
}

function setPageActive(scrollPosition) {
    for (let page of pages) {
        let bottom = getTopOfElement(page) + page.clientHeight;

        for (let anchor of nav) {
            if (scrollPosition >= getTopOfElement(page) && scrollPosition <= bottom) {
                anchor.hash.split("#")[1] === page.id
                    ? anchor.classList.add("active")
                    : anchor.classList.remove("active");
            }
        }
    }
}

setPageActive(window.scrollY);

window.addEventListener("scroll", event => {
    setPageActive(window.scrollY);
});







// $(document).ready(function () {

// var url = "https://stage.adani.com/newsroom?title=Media_Statement";
// $(location).attr('href',url);

// JavaScript
// //similar to window.location
// window.location.href="https://www.adani.com/Newsroom/Media-Release/Media-Statementâ€;

// //does't put originating page in history
// window.location.replace("https://www.adani.com/Newsroom/Media-Release/Media-Statement"); 

// });


function myFunction() {
    var iframe = document.getElementById("market-cap");
    var elmnt = iframe.contentWindow.document.getElementsByTagName("div")[0];
    elmnt.style.border = "1px solid #ccc";
}

$(document).ready(function () {
    if ($(".marketCapValue").length) {
        let monthNames = ["January", "February", "March", "April",
            "May", "June", "July", "August",
            "September", "October", "November", "December"];
        var today = new Date();
        var dd = String(today.getDate()).padStart(2, '0');
        var mm = today.getMonth(); //January is 0!
        var yyyy = today.getFullYear();

        today = monthNames[mm] + ' ' + dd + ', ' + yyyy;
        $.ajax({
            type: "Get",
            data: '',
            url: "https://www.advaiya.com/mkt/mktCapTotal.php",
            dataType: 'json',
            cors: true,
            contentType: 'application/json',
            secure: true,
            headers: {
                'Access-Control-Allow-Origin': '*',
            },
            success: function (opdata) {
                $(".marketCapValue").html("$" + opdata.mktcapTotal + " billion (as on " + today + ")");
                var Hovertext = "USD exchange rate: " + opdata.usdRate + " INR <br><hr> Data Source: Google Finance <br><hr> Last updated: " + today + "<br><hr> Note: These are approximate figures";
                $(".btn-mcap").attr("data-original-title", Hovertext);
                // $("#USDrate").html("USD exchange rate: "+opdata.usdRate+" INR");
                // $("#lastUpdate").html("Last updated on: "+today);
            },
            error: function (result) {
                alert('Error occured!!');
            }
        });
    }


    // $.post('https://www.advaiya.com/mkt/mktCapTotal.php', {mkt:'total'}, cors: true,
    // headers: {
    // 'Access-Control-Allow-Origin': '*',
    // }, function(response){
    // $('.marketCapValue').text(response.mktcapTotal);
    // },"json");
});

$('.marketcap-dropdown1 button').hover(function () {
    if ($('.marketcap-dropdown .dropdown-menu').hasClass('active')) {
        $('.marketcap-dropdown .dropdown-menu').removeClass('active');
    }
    else {
        $('.marketcap-dropdown .dropdown-menu').addClass('active');
    }

})

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})


$('.custom-switch label').click(function () {
    $('.custom-switch').toggleClass('active');
    $('.section_business').toggleClass('active');
    $('.section_companies').toggleClass('active');
});

$(document).ready(function () {

    var myString = window.location.href.substring(window.location.href.lastIndexOf('#') + 1);

    if (myString == 'EnergyUtilities') {
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }

    if (myString == 'TransporationLogistics') {
        $('.section_business .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_business .nav-tabs li:nth-child(2) a').addClass('active');
        $('#TransporationLogistics').removeClass('active');
        $('#Incubation').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    } if (myString == 'Incubation') {
        $('.section_business .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_business .nav-tabs li:nth-child(3) a').addClass('active');
        $('#EnergyUtilities').removeClass('active');
        $('#Incubation').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }

    if (myString == 'Others') {
        $('.section_business .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_business .nav-tabs li:nth-child(4) a').addClass('active');
        $('#EnergyUtilities').removeClass('active');
        $('#Others').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }

    if (myString == 'AdaniEnterprises') {
        $('.custom-switch').toggleClass('active');
        $('.section_business').toggleClass('active');
        $('.section_companies').toggleClass('active');

        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }

    if (myString == 'AdaniGreen') {
        $('.custom-switch').toggleClass('active');
        $('.section_business').toggleClass('active');
        $('.section_companies').toggleClass('active');
        $('.section_companies .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_companies .nav-tabs li:nth-child(2) a').addClass('active');
        $('#AdaniEnterprises').removeClass('active');
        $('#AdaniGreen').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }


    if (myString == 'AdaniPorts') {
        $('.custom-switch').toggleClass('active');
        $('.section_business').toggleClass('active');
        $('.section_companies').toggleClass('active');
        $('.section_companies .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_companies .nav-tabs li:nth-child(3) a').addClass('active');
        $('#AdaniEnterprises').removeClass('active');
        $('#AdaniPorts').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }

    if (myString == 'AdaniTransmission') {
        $('.custom-switch').toggleClass('active');
        $('.section_business').toggleClass('active');
        $('.section_companies').toggleClass('active');
        $('.section_companies .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_companies .nav-tabs li:nth-child(4) a').addClass('active');
        $('#AdaniEnterprises').removeClass('active');
        $('#AdaniTransmission').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }



    if (myString == 'AdaniGas') {
        $('.custom-switch').toggleClass('active');
        $('.section_business').toggleClass('active');
        $('.section_companies').toggleClass('active');
        $('.section_companies .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_companies .nav-tabs li:nth-child(5) a').addClass('active');
        $('#AdaniEnterprises').removeClass('active');
        $('#AdaniGas').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }



    if (myString == 'AdaniPower') {
        $('.custom-switch').toggleClass('active');
        $('.section_business').remoeClass('active');
        $('.section_companies').addClass('active');
        $('.section_companies .nav-tabs li:nth-child(1) a').removeClass('active');
        $('.section_companies .nav-tabs li:nth-child(6) a').addClass('active');
        $('#AdaniEnterprises').removeClass('active');
        $('#AdaniPower').addClass('active');
        $('html, body').animate({
            scrollTop: $('.business_tabs').offset().top
                - 0
        }, 1000);
    }



});

$('.business_tabs .tab-content .panel').click(function () {
    $('.panel-collapse').removeClass('show');
    $('html, body').animate({
        scrollTop: $(this).offset().top - 70
    }, '600');
})

$(document).ready(function () {
    $('#belimitvideo1').prepend('<video class="w-100" preload="auto" loop="true" autoplay id="bannerVideo" muted controls controlsList="nodownload"><source src="https://adanidotcom.azureedge.net/belimitless_Desktop.mp4" type="video/mp4"></video>');
});
$(document).ready(function () {
    $('#belimitvideomobile').prepend('<video class="w-100" preload="auto" loop="true" autoplay id="bannerVideo" muted controls controlsList="nodownload"><source src="https://adanidotcom.azureedge.net/belimit_mobile.mp4" type="video/mp4"></video>');
});

/*adaniContactForm.js*/

    $(document).ready(function () {
        if (window.location.href.indexOf("?title") > -1) {
            var title = GetParameterValues('title');
            window.location = "/Newsroom/Media-Release/Media-Statement";
            //return true;
        }

        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }
    });


var recaptcha1;

var onloadCallback = function () {
    if ($("recaptcha1").length > 0) {
        //Render the recaptcha1 on the element with ID "recaptcha1"
        recaptcha1 = grecaptcha.render('recaptcha1', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
};

$("#btnContactUsSubmit").click(function () {
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
    if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#ccontactno").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var message = $("#cmessage").val();
    if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var cmessageType = $("#cmessageType").val();
    if (cmessageType == "") { alert("Please select valid subject"); $("#cmessageType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var formtype = $("#cFormType").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var mobile = "1010101010";


    function validateEmail(mailid) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(mailid)) { return true; }
        else { return false; }
    }

    var model = {

        Name: name,
        Mobile: mobile,
        Email: mailid

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        Name: name,
        Email: mailid,
        SubjectType: cmessageType,
        Mobile: ccontactno,
        Message: message,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Adani/InsertContactdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "https://stage.adani.com/thankyou";
                //$('#contact_form1').submit();
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;


    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Adani/CreateOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                var otp = prompt("Please enter OTP received on your mobile", "");

                if (otp != null) {

                    var generatedOtp = {
                        mobile: mobile,
                        OTP: otp,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(generatedOtp),
                        url: "/api/Adani/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {


                            }

                            else {
                                alert("Invalid OTP");
                                $('#btnContactUsSubmit').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnContactUsSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});

/*AdaniSubscribeForm.js*/

$("#btnSubscribeUsSubmit").click(function () {

    $('#btnSubscribeUsSubmit').attr("disabled", "disabled");

    var mailid = $("#subscribeEmail").val();
    if (mailid == "") { alert("Email is Required"); $("#subscribeEmail").focus(); $('#btnSubscribeUsSubmit').removeAttr("disabled"); return false; }

    var formtype = $("#sFormType").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#subscribeEmail").focus(); $('#btnSubscribeUsSubmit').removeAttr("disabled"); return false; }



    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }



    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {

        Email: mailid,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Adani/InsertContactdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;


    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/Adani/CreateOTP",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                var otp = prompt("Please enter OTP received on your mobile", "");

                if (otp != null) {

                    var generatedOtp = {
                        mobile: mobile,
                        OTP: otp,
                    }
                    $.ajax({
                        type: "POST",
                        data: JSON.stringify(generatedOtp),
                        url: "/api/Adani/VerifyOTP",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.status == "1") {


                            }

                            else {
                                alert("Invalid OTP");
                                $('#btnContactUsSubmit').removeAttr("disabled");
                                return false;
                            }
                        }
                    });

                }
            }
            else if (data == "-1") {
                alert("Invalid Mobile Number");
                $('#btnContactUsSubmit').removeAttr("disabled");
            }
        }
    });

    return false;
});
