/*
	Load more content with jQuery - May 21, 2013
	(c) 2013 @ElmahdiMahmoud
*/




$(document).ready(function () {

    var list = $(".business .col-lg-3");
    var numToShow = 4;
    var button = $("#loadMore");
    var numInList = list.length;
    list.hide();
    if (numInList > numToShow) {
        button.show();
    }
    list.slice(0, numToShow).show();

    button.click(function () {
        var showing = list.filter(':visible').length;
        list.slice(showing - 1, showing + numToShow).fadeIn();
        var nowShowing = list.filter(':visible').length;
        if (nowShowing >= numInList) {
            button.hide();
        }
    });

});






/*Scroll*/
$('.click-scroll a').click(function (e) {
    e.preventDefault();
    var target = $($(this).attr('href'));
    if (target.length) {
        var scrollTo = target.offset().top;
        $('body, html').animate({ scrollTop: scrollTo + 'px' }, 800);
    }
});
$(function () {
    $('#investor-select select').change(function () {
        $('.investor-m-block').hide();
        $('.inv-tabs-query').hide();
        $('.tab-inv').hide();
        $('.' + $(this).val()).show();
    });
});
$(document).ready(function () {

    /*
    $('.searchIcon').click(function() {
        $('.search-main').toggleClass("d-block").css('opacity', '1');
        $('.searchIcon').css('position','absolute').css('z-index', '9');
        // Alternative animation for example
        // slideToggle("fast");
      });*/

    $('.viewall').click(function () {
        $('.ft-business li').removeClass('d-none');
        $('.viewall').addClass('d-none');
    });
    $('.sitemap-link').click(function () {
        $('.ft-mobilemenu').removeClass('d-none');
        $('.sitemap-link').addClass('d-none');
        $('.sitemap-link').removeClass('d-block');
    });

    // Fakes the loading setting a timeout
    setTimeout(function () {
        $('body').addClass('loaded');
    }, 1500);


    /*Home Video Slider*/
    var owl = $('.homeslider');
    owl.owlCarousel({
        loop: false,
        margin: 0,
        nav: false,
        dots: true,
        // autoHeight:true,
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


    $('#footerArrow').click(function () {
        $('.footerPanel2 .mobile-none').toggleClass('d-none');
        $('.footerPanel2 .footerpanel-1').toggle(100);
        $('.footerPanel2 .txt-center').toggle(100);
        $('.footerPanel2 .ft-submenu').toggleClass('col-lg-12');
    });

    if ($(window).width() <= 992) {
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
    else {
        $("#loadMore").attr('href', 'businesses.html');
        // Hide Header on scroll down
        var didScroll;
        var lastScrollTop = 0;
        var delta = 5;
        var navbarHeight = 100;

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

    }

    $('#sustanibility').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        nav: true,
        dots: false,
        autoplay: false,
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
                items: 4
            }
        }
    })

    $('#consumer-essential').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        nav: true,
        dots: false,
        autoplay: false,
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
                items: 4
            }
        }
    })

    $('#industry-essential').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        nav: true,
        dots: false,
        autoplay: false,
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
                items: 4
            }
        }
    })





    $('#industry').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        nav: true,
        dots: false,
        autoplay: false,
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
                items: 4
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
    })

    $('.three-item').owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 1000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
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
                dots: false
            },
            768: {
                items: 4,
                nav: true,
                dots: false,
                dots: false
            },
            1000: {
                items: 6,
                nav: true,
                dots: false,
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
        $('.overlay-top').toggleClass('active');

    });

    $('input[type="checkbox"]').click(function () {
     //   $('.folding-menu li').addClass('hide');
        $(this).closest("section").find('.folding-menu li').addClass("hide");
        var filterCon = $(this).closest('.filter-content').find('input:checked');
        var x, i;
        x = $(this).closest('.filter-content').parent().find(".column");
        w3RemoveClass(x, "show");
        $(filterCon).each(function () {
            var ids = $(this).attr('value');
            for (i = 0; i < x.length; i++) {

                if (x[i].className.toLowerCase().indexOf(ids) > -1) {
                    w3AddClass(x[i], "show");
                }
            }

        });



        //if ($(this).prop("checked") == true) {
        //    var ids = $('input:checkbox:checked').val(); 


        //    var x, i;
        //    x = document.getElementsByClassName("column");
        //    for (i = 0; i < x.length; i++) {
        //        w3RemoveClass(x[i], "show");
        //        if (x[i].className.toLowerCase().indexOf(ids) > -1) {
        //            w3AddClass(x[i], "show");
        //        }
        //    }
        //}

    })



    //function FilterProduct1(obj) {
    //    var filterCon = $(obj).closest('.filter-content').find('input:checked');
    //    var x, i;
    //    x = $(obj).closest('.filter-content').find("column");
    //    w3RemoveClass(x, "show");
    //    $(filterCon).each(function () {
    //      var ids =  $(this).attr('name');



    //    for (i = 0; i < x.length; i++) {

    //        if (x[i].className.toLowerCase().indexOf(ids) > -1) {
    //            w3AddClass(x[i], "show");
    //        }
    //        }

    //    });
    //}
    function w3AddClass(element, name) {
        var i, arr1, arr2;
        arr1 = element.className.split(" ");
        arr2 = name.split(" ");
        for (i = 0; i < arr2.length; i++) {
            if (arr1.indexOf(arr2[i]) == -1) { element.className += " " + arr2[i]; }
        }
    }

    function w3RemoveClass(x, name) {

        for (var j = 0; j < x.length; j++) {
            var element = x[j]
            var i, arr1, arr2;
            arr1 = element.className.split(" ");
            arr2 = name.split(" ");
            for (i = 0; i < arr2.length; i++) {
                while (arr1.indexOf(arr2[i]) > -1) {
                    arr1.splice(arr1.indexOf(arr2[i]), 1);
                }
            }
            element.className = arr1.join(" ");

        }
    }

})