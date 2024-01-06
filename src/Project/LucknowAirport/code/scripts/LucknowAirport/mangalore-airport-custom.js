$(document).ready(function () {
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "")
        $('#message_modal').modal('show');
});
$('.modal .close').click(function () {

    var videoId = $(this).attr('Id');
    CloseModal2(videoId);

});

$('.modal').on('shown.bs.modal', function () {
    $('.modal video')[0].play();
})
$('.modal.video').on('hidden.bs.modal', function () {
    $('.modal video')[0].pause();
})

function CloseModal(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
}

$('body').on('hidden.bs.modal', '.modal', function () {
    $('video').trigger('pause');
});

$(".view-all video").each(function () {
    $(this).get(0).pause();
});

function DownloadFile(fileId) {
    $("#hfFileId").val(fileId);
    $("#btnDownload").click();
};

(function () {

    'use strict';

    // define variables
    var items = document.querySelectorAll(".timeline li");

    // check if an element is in viewport
    // http://stackoverflow.com/questions/123999/how-to-tell-if-a-dom-element-is-visible-in-the-current-viewport
    function isElementInViewport(el) {
        var rect = el.getBoundingClientRect();
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    }

    function callbackFunc() {
        for (var i = 0; i < items.length; i++) {
            if (isElementInViewport(items[i])) {
                items[i].classList.add("in-view");
            }
        }
    }

    // listen for events
    window.addEventListener("load", callbackFunc);
    window.addEventListener("resize", callbackFunc);
    window.addEventListener("scroll", callbackFunc);

})();


$(document).ready(function () {

    // var owl = $('.homeslider');
    // owl.owlCarousel({
    // loop:false,
    // margin:0,
    // lazyLoad: true,
    // nav:false,
    // dots:true,
    // autoHeight:true,
    // autoplay: true,
    // autoplayTimeout: 2000,
    // slideSpeed: 500,
    // items:1
    // })
    // owl.on('translate.owl.carousel',function(e){
    // $('.homeslider .owl-item video').each(function(){
    // $(this).get(0).pause();
    // $(this).get(0).currentTime = 0;
    // });
    // });
    // $('.homeslider .owl-item .item').each(function(){
    // var attr = $(this).attr('data-videosrc');
    // if (typeof attr !== typeof undefined && attr !== false) {
    // var videosrc = $(this).attr('data-videosrc');
    // $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="'+videosrc+'" type="video/mp4"></video>');
    // $('.homeslider .owl-item.active video');
    // }
    // });
    // owl.on('translated.owl.carousel',function(e){
    // $('.homeslider .owl-item.active video').get(0).play();
    // });

    var owl = $('.main-slider');
    owl.owlCarousel({
        autoplay: true,
        autoplayTimeout: 4000,
        loop: true,
        items: 1,
        center: true,
        dots: false,
        nav: true,
        thumbs: true,
        thumbImage: false,
        thumbsPrerendered: true,
        thumbContainerClass: 'owl-thumbs',
        thumbItemClass: 'owl-thumb-item',
        navText: ['<span class="prev">＜</span>', '<span class="next">＞</span>'],
    });

    $('.ProjectAssetsDataInner').hide();
    $('#DefaultData').show();
    $('#DefaultData2').show();
    $('#buss-owl').owlCarousel({
        items: 2,
        itemsDesktop: [1000, 2],
        itemsDesktopSmall: [900, 2],
        itemsTablet: [640, 1],
        pagination: true,

    });

    $('.pointer').click(function () {
        $('.tab-panea').hide();
        $('.ProjectAssetsDataInner').hide();
        var getTabId = $(this).attr('rel');
        $('#' + getTabId).show();
        $(".indiaMap a").attr("class", "pointer");
        $(this).attr("class", "active");
    });
    $('.homeslider-other').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        nav: false,
        dots: true,
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
    }),
        $('.timeline-carousel').owlCarousel({
            loop: false,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 3000,
            touchDrag: false,
            mouseDrag: false,
            autoplay: false,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 3
                }
            }
        }),
        $('.our-team').owlCarousel({
            loop: false,
            margin: 50,
            responsiveClass: true,
            autoplayTimeout: 3000,
            touchDrag: false,
            mouseDrag: false,
            autoplay: false,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 4
                }
            }
        }),
        $('.sponsor-carousel').owlCarousel({
            loop: false,
            margin: 25,
            responsiveClass: true,
            autoplayTimeout: 1500,
            autoplay: true,
            nav: false,
            dots: true,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 2
                }
            }
        }),
        $('.gallery-carousel').owlCarousel({
            loop: true,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 1500,
            autoplay: true,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 2,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 3
                }
            }
        }),
        //home page text update on sustainability Tile
        $($(".sustainabilityHomeTile h3")[1]).text("Social");
});


$('.click-scroll a').on('click', function (e) {
    var href = $(this).attr('href');
    $('html, body').animate({
        scrollTop: $(href).offset().top - 70
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


$(document).ready(function () {
    $('.modal .close').click(function () {

        var videoId = $(this).attr('Id');
        CloseModal(videoId);

    });

    var y = $(window).scrollTop();
    y = y + 150;
    $('.sitemap-link').click(function () {
        $('.ft-mobilemenu').toggleClass('d-none').animate({ scrollTop: y }, 800);
        $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
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

    $('.double-carousel').owlCarousel({
        loop: false,
        margin: 10,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-arrow-left'></i>", "<i class='fa fa-arrow-right'></i>"],
        responsiveClass: true,
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
    $('.infrastructure-block').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        nav: true,
        dots: false,
        autoplay: false,
        autoplaytimeout: 1500,
        slideSpeed: 1500,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
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

    $('.single-carousel').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoHeight: false,
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
                items: 1,
                nav: true,
                dots: false
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                margin: 10
            }
        }
    })



    $('.case-study').owlCarousel({
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
                items: 1,
                nav: true,
                dots: false
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                margin: 20
            }
        }
    })
    $('.single-item').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: true,
        autoplayTimeout: 1500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false
            },
            600: {
                items: 1,
                nav: true,
                dots: false
            },
            1000: {
                items: 1,
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

    // Fakes the loading setting a timeout
    setTimeout(function () {
        $('body').addClass('loaded');
    }, 1500);


    /*Inner Video Slider*/
    var owl = $('.innerv-carousel');
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
        mouseDrag: true,
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

    // $(".popup").magnificPopup({
    // type: "image",
    // removalDelay: 160,
    // preloader: false,
    // fixedContentPos: true,
    // gallery: {
    // enabled: true
    // }
    // });


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
        $('.overlay-top').toggleClass('active');
        $('body').toggleClass('overflow-hidden');

    });

    // $('[data-toggle="tooltip"]').tooltip();

    $(".loadmore-content p").slice(0, 1).show();
    $(".loadMore").on("click", function (e) {
        e.preventDefault();
        $(".loadMore").toggleClass("d-block d-none");
        $(".loadmore-content p").toggleClass("d-none");
        $(".loadmore-content ul").toggleClass("d-none");
        $(".loadmore-content ul li").toggleClass("d-none d-list");
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
        $('.ft-business li').toggleClass('d-none d-block');
        $(".viewall").text("+ View more ").show(1000);
        if ($(".ft-business li:hidden").length == 0) {
            $(".viewall").text("- View less ").show(1000);
            $(".viewall").show(1000);
        }
    });


    $('.nav-border-bottom li').click(function () {
        $('.d-lg-block #cont-tab-2 #DefaultData2').addClass('d-block');
    });

    $('#cont-tab-2 a').click(function () {
        $('.d-lg-block #cont-tab-2 #DefaultData2').removeClass('d-block');
    });



    // $("a.dropdown-item")
    // .on("mouseenter", function (e) {
    // $(".menu-thumb").children()[0].style.display = "none";
    // for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
    // if ($(".menu-thumb").children()[i].alt == $(e.currentTarget).text()) {
    // $(".menu-thumb").children()[i].style.display = "block";
    // }
    // else {
    // $(".menu-thumb").children()[i].style.display = "none";
    // }
    // }
    // })
    // .on("mouseleave", function (e) {
    // for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
    // $(".menu-thumb").children()[i].style.display = "none";
    // }
    // $(".menu-thumb").children()[0].style.display = "block";
    // });

    $('.single-item').owlCarousel({
        loop: false,
        margin: 10,
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
                items: 1,
                margin: 10
            },
            600: {
                items: 1,
                margin: 10
            },
            1000: {
                items: 1,
                margin: 10
            }
        }
    })
    $('.three-item').owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false
            },
            600: {
                items: 2,
                nav: false,
                dots: false
            },
            1000: {
                items: 3,
                nav: false,
                dots: false
            }
        }
    })
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

$('.topMenu .dropdown').click(function () { });
$('.topMenu .dropdown a').click(function () { $('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px'); });
$('.topMenu .dropdown').click(function () { $(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)'); });


var a = 0;
$(window).scroll(function () {
    if ($('#counter').length != 0) {
        var oTop = $('#counter').offset().top - window.innerHeight;
        if (a == 0 && $(window).scrollTop() > oTop) {
            (function ($) {
                $.fn.countTo = function (options) {
                    options = options || {};

                    return $(this).each(function () {
                        // set options for current element
                        var settings = $.extend({}, $.fn.countTo.defaults, {
                            from: $(this).data('from'),
                            to: $(this).data('to'),
                            speed: $(this).data('speed'),
                            refreshInterval: $(this).data('refresh-interval'),
                            decimals: $(this).data('decimals')
                        }, options);

                        // how many times to update the value, and how much to increment the value on each update
                        var loops = Math.ceil(settings.speed / settings.refreshInterval),
                            increment = (settings.to - settings.from) / loops;

                        // references & variables that will change with each update
                        var self = this,
                            $self = $(this),
                            loopCount = 0,
                            value = settings.from,
                            data = $self.data('countTo') || {};

                        $self.data('countTo', data);

                        // if an existing interval can be found, clear it first
                        if (data.interval) {
                            clearInterval(data.interval);
                        }
                        data.interval = setInterval(updateTimer, settings.refreshInterval);

                        // initialize the element with the starting value
                        render(value);

                        function updateTimer() {
                            value += increment;
                            loopCount++;

                            render(value);

                            if (typeof (settings.onUpdate) == 'function') {
                                settings.onUpdate.call(self, value);
                            }

                            if (loopCount >= loops) {
                                // remove the interval
                                $self.removeData('countTo');
                                clearInterval(data.interval);
                                value = settings.to;

                                if (typeof (settings.onComplete) == 'function') {
                                    settings.onComplete.call(self, value);
                                }
                            }
                        }

                        function render(value) {
                            var formattedValue = settings.formatter.call(self, value, settings);
                            $self.html(formattedValue);
                        }
                    });
                };

                $.fn.countTo.defaults = {
                    from: 0,               // the number the element should start at
                    to: 0,                 // the number the element should end at
                    speed: 1000,           // how long it should take to count between the target numbers
                    refreshInterval: 100,  // how often the element should be updated
                    decimals: 0,           // the number of decimal places to show
                    formatter: formatter,  // handler for formatting the value before rendering
                    onUpdate: null,        // callback method for every time the element is updated
                    onComplete: null       // callback method for when the element finishes updating
                };

                function formatter(value, settings) {
                    return value.toFixed(settings.decimals);
                }
            }(jQuery));

            jQuery(function ($) {
                // custom formatting example
                $('.count-number').data('countToOptions', {
                    formatter: function (value, options) {
                        return value.toFixed(options.decimals).replace(/\B(?=(?:\d{3})+(?!\d))/g, ',');
                    }
                });
                $('.count-number2').countTo({
                    decimals: 2,

                });
                // start all the timers
                $('.timer').each(count);

                //$('.timer2').each(count);  

                function count(options) {
                    var $this = $(this);
                    options = $.extend({}, options || {}, $this.data('countToOptions') || {});
                    $this.countTo(options);
                }
            });
            a = 1;
        }
    }
});

$('.centralindiatab').click(function () {
    $($('.nav-tabs li a[href="#centralindia"]')).trigger('click');
});

$('.rajasthantab').click(function () {
    $($('.nav-tabs li a[href="#rajasthan"]')).trigger('click');
});


$('.maharashtratab').click(function () {
    $($('.nav-tabs li a[href="#maharashtra"]')).trigger('click');
});


$('.uttarpradeshtab').click(function () {
    $($('.nav-tabs li a[href="#uttarpradesh"]')).trigger('click');
});

//$(document).ready(function () {
//    var message = $("#message").val();
//    if (message !== undefined && message !== null && message !== "")
//        $('#message_modal').modal('show');
//});

//var clicked = false;
//function CheckBrowser() {
//    if (clicked == false) {
//        //Browser closed
//    }
//    else {
//        //redirected 
//        clicked = false;
//    }
//}

//function bodyUnload() {
//    if (clicked == false)//browser is closed
//    {
//        var request = GetRequest();
//        request.open("POST", "/api/Accounts/LogoutSessionOnTabclose", false);
//        request.send();
//    }
//}

//function GetRequest() {
//    var request = null;
//    if (window.XMLHttpRequest) {
//        //incase of IE7,FF, Opera and Safari browser
//        request = new XMLHttpRequest();
//    }
//    else {
//        //for old browser like IE 6.x and IE 5.x
//        request = new ActiveXObject('MSXML2.XMLHTTP.3.0');
//    }
//    return request;
//}
//function Abandon(e) {
//    jQuery.ajax(
//        {
//            url: "/api/Accounts/LogoutSessionOnTabclose",
//            method: "POST",
//            async: true,
//            success: function (data) {
//                //e.cancelBubble is supported by IE - this will kill the bubbling process.
//                e.cancelBubble = true;
//                e.returnValue = leave_message;
//                //e.stopPropagation works in Firefox.
//                if (e.stopPropagation) {
//                    e.stopPropagation();
//                    e.preventDefault();
//                }
//                //return works for Chrome and Safari
//                return leave_message;
//            }
//        });
//}

//function IsSuccess() {
//    jQuery.ajax(
//        {
//            url: "/api/Accounts/Benow_Callback",
//            method: "POST",
//            success: function (data) {
//                if (data !== "") {
//                    myStopFunction();
//                    location.href = data;
//                }
//            }
//        });
//}

//setTimeout(myStopFunction, 300000);


//function myStopFunction() {
//    clearInterval(myVar);
//}

//$(document).ready(function () {
//    $('#Communities').click(function () {
//        $('html, body').animate({
//            scrollTop: $("#communitiesSec").offset().top
//                - 130
//        }, 1000);
//    });



//    var owl = $('#serviceCarousels');
//    owl.owlCarousel({
//        loop: false,
//        margin: 10,
//        nav: false,
//        dots: false,
//        navRewind: false,
//        responsive: {
//            0: {
//                items: 1,
//                nav: false,
//                dots: true
//            },
//            600: {
//                items: 1,
//                nav: false,
//                dots: true
//            },
//            768: {
//                items: 2,
//                nav: false,
//                dots: true
//            },
//            1000: {
//                items: 3
//            }
//        }
//    });

//    var owl2 = $('#homeslider');
//    owl2.owlCarousel({
//        loop: true,
//        autoplay: true,
//        margin: 10,
//        nav: true,
//        autoHeight: true,
//        dots: true,
//        navRewind: false,
//        autoplayTimeout: 5000,
//        lazyLoad: true,
//        responsive: {
//            0: {
//                items: 1
//            },
//            600: {
//                items: 1
//            },
//            1000: {
//                items: 1
//            }
//        }
//    });

//    ///* Initialize Banner Carousel on home page */
//    //$("#bannerCarousel").owlCarousel({
//    //    autoplay:true,
//    //    //autoplayTimeout:2000,
//    //    //autoplayHoverPause:true,
//    //    center: true,
//    //    loop:true,
//    //    items: 1,
//    //    dots: false,
//    //    pagination : true,
//    //}); 

//    /* Initialize Carousel  on energy calculator*/
//    $("#energyCarousel").owlCarousel({
//        nav: true,
//        navText: [],
//        responsive: {
//            // breakpoint from 0 up
//            0: {
//                items: 2
//            },
//            // breakpoint from 480 up
//            480: {
//                items: 3
//            },
//            // breakpoint from 768 up
//            768: {
//                items: 3
//            },
//            // breakpoint from 992 up
//            992: {
//                items: 4
//            },
//            // breakpoint from 1025 up
//            1025: {
//                items: 5
//            }
//        }
//    });

//    /* Initialize Carousel on home page */
//    //$(".owl-carousel").owlCarousel({
//    //    nav: true,
//    //    navText: [],
//    //    responsive: {
//    //        // breakpoint from 0 up
//    //        0: {
//    //            items: 1
//    //        },
//    //        // breakpoint from 480 up
//    //        480: {
//    //            items: 2
//    //        },
//    //        // breakpoint from 768 up
//    //        768: {
//    //            items: 3,
//    //            nav: true,
//    //            loop: false,
//    //            touchDrag: false,
//    //            mouseDrag: false
//    //        },
//    //        1000: {
//    //            nav: true,
//    //            loop: false,
//    //            touchDrag: false,
//    //            mouseDrag: false
//    //        }
//    //    }
//    //});

//    /* Menu on mobile devices */

//    $('#dismiss, .overlay, .overlay-top').on('click', function () {
//        $('#sideNav').removeClass('active');
//        $('.overlay, .overlay-top').removeClass('active');
//        $('#topMenu').removeClass('active');
//    });

//    /*Side menu on mobile devices*/

//    $('#sidebarCollapse').on('click', function () {
//        $('#sideNav, #mainContent').toggleClass('active');
//        $('.collapse.in').toggleClass('in');
//        $('.overlay').addClass('active');
//        //$('a[aria-expanded=true]').attr('aria-expanded', 'false');
//    });

//    /* Top menu on mobile devices*/

//    $('#topNavCollapse').on('click', function () {
//        $('#topMenu').toggleClass('active');
//        $('.collapse.in').toggleClass('in');
//        $('.overlay-top').addClass('active');

//    });

//    ///* For smooth page scroll on Top menu link */
//    //document.querySelectorAll('a[href^="#"]').forEach(anchor => {
//    //    anchor.addEventListener('click', function (e) {
//    //        e.preventDefault();

//    //        document.querySelector(this.getAttribute('href')).scrollIntoView({
//    //            behavior: 'smooth'
//    //        });
//    //    });
//    //});

//    ///* to reduce the bottom space in between two control, if control have  minBottomSpace class*/
//    //$('.minBottomSpace').closest(".pageContent").css("min-height", "100px")
//});
///*Bootstrap DatePicker JS*/
//$(function () {
//    $('#datetimepicker1').datetimepicker({ format: 'DD/MM/YYYY' });
//});


$(document).on("click", ".txt-orange", function () {
    Id = $(this).data('id');
    $("#TenderID").val(Id);
});


//$(document).on("click", "#download", function () {
//    var id = $(this).data('id');
//    jQuery.ajax(
//        {
//            url: "/api/MangaloreAirport/GetTenderDocument",
//            method: "GET",
//            data: {
//                Id: id
//            },
//            success: function (data) {
//                $("#downloadModal").html(data);
//                $('#downloadModal').modal('show');
//            }
//        });


//})

////for career page popup
//$(window).on('load', function () {
//    $('#ac-wrapper').show();
//});
//$("#closeopop").on("click", function () {
//    $('#ac-wrapper').hide();
//});


//$(document).ready(function () {

//    if (window.location.href.indexOf("#communitiesSec") > 0) {

//        $('html, body').animate({
//            scrollTop: $("#communitiesSec").offset().top
//                - 130
//        }, 1000);
//    }

//    $(window).on('load', function () {
//        $('#homemodalpopup').modal('show');
//    });

//});

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
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) {
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





