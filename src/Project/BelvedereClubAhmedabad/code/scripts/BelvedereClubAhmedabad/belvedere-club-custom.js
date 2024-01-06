$(document).ready(function () {




    /*Setting URL of downloadbrochure*/
    $('a[href*=' + "downloadbrochure" + ']').attr("data-target", "#downloadbrochure")
    $('a[href*=' + "downloadbrochure" + ']').attr("data-toggle", "modal")
    /*Setting URL of downloadbrochure*/

    /*COpy to Clipboard*/
    $(".sharebtn").click(function (event) {
        event.preventDefault();
        CopyToClipboard("https://www.google.com/maps?ll=23.165903,72.537182&z=16&t=m&hl=en-US&gl=IN&mapclient=embed&cid=7317226060033219478", true, "Location URL Copied to Clipboard");
    });
    function CopyToClipboard(value, showNotification, notificationText) {
        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val(value).select();
        document.execCommand("copy");
        $temp.remove();

        if (typeof showNotification === 'undefined') {
            showNotification = true;
        }
        if (typeof notificationText === 'undefined') {
            notificationText = "Copied to clipboard";
        }

        var notificationTag = $("div.copy-notification");
        if (showNotification && notificationTag.length == 0) {
            notificationTag = $("<div/>", { "class": "copy-notification", text: notificationText });
            $("body").append(notificationTag);

            notificationTag.fadeIn("slow", function () {
                setTimeout(function () {
                    notificationTag.fadeOut("slow", function () {
                        notificationTag.remove();
                    });
                }, 10000);
            });
        }
    }



    /*Search*/
    $("#search").click(function (e) {
        e.preventDefault();
        $(".search_box").toggleClass('active');
    });


    /*Member Tooltip Selector*/
    jQuery(function () {
        jQuery('.showSingle').click(function () {
            jQuery('.targetDiv').hide();
            jQuery('#div' + $(this).attr('target')).show();
        });
    });

    /*Events Block Filter*/
    var $btns = $('.events-btn').click(function () {
        if (this.id == 'all') {
            $('#parent > div').fadeIn(450);
        } else {
            var $el = $('.' + this.id).fadeIn(450);
            $('#parent > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })

    /*Affiliates Selector Domestic*/
    var $btns = $('.affliates-btn').click(function () {
        if (this.id == 'all') {
            $('#parent > div').fadeIn(450);
        } else {
            var $el = $('.' + this.id).fadeIn(450);
            $('#parent > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })

    /*Affiliates Selector International*/
    var $btns = $('.affliates-btn').click(function () {
        if (this.id == 'all') {
            $('#inter-affiliation > div').fadeIn(450);
        } else {
            var $el = $('.' + this.id).fadeIn(450);
            $('#inter-affiliation > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })



    /*Home Video Slider*/
    var owl = $('.homeslider');
    owl.owlCarousel({
        autoplayHoverPause: true,
        loop: false,
        margin: 10,
        nav: false,
        dots: false,
        autoplay: true,
        autoplayTimeout: 5000,
        autoplayHoverPause: true, // Stops autoplay
        items: 1
    })
    owl.on('translate.owl.carousel', function (e) {
        $('.owl-item video').each(function () {
            $(this).get(0).pause();
        });
    });
    owl.on('translated.owl.carousel', function (e) {
        $('.owl-item.active video').get(0).play();
    })
    if (!isMobile()) {
        $('.owl-item .item').each(function () {
            var attr = $(this).attr('data-videosrc');
            if (typeof attr !== typeof undefined && attr !== false) {
                console.log('hit');
                var videosrc = $(this).attr('data-videosrc');
                $(this).prepend('<video muted><source src="' + videosrc + '" type="video/mp4"></video>');
            }
        });
        $('.owl-item.active video').attr('autoplay', true).attr('loop', true);
    }
    function isMobile(width) {
        if (width == undefined) {
            width = 0;
        }
        // if (window.innerWidth <= width) {
        // return true;
        // } else {
        // return false;
        // }
    }


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

    /*Home Main Slider*/
    $('#home-carousel').owlCarousel({
        loop: false,
        autoplayHoverPause: true,
        margin: 10,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 2000,
        responsiveClass: true,
        autoplayTimeout: 2500,
        autoplay: 1500,
        video: true,
        lazyLoad: true,
        center: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
            },
            600: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
            },
            1000: {
                items: 1,
                nav: false,
                dots: false,
                loop: false,
                margin: 20,
            }
        }
    });

    /*NewCareer Carousel JS*/
    $(".golfslider #thumbs").owlCarousel({
        nav: false,
        loop: false,
        margin: 0,
        autoplayHoverPause: true,
        dots: false,
        mouseDrag: false,
        touchDrag: false,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 2
            },
            // breakpoint from 480 up
            480: {
                items: 2
            },
            // breakpoint from 768 up
            768: {
                items: 2

            }
        }
    });

    /*NewCareer Carousel JS*/
    $("#big").owlCarousel({
        nav: false,
        loop: true,
        autoHeight: true,
        margin: 0,
        autoplayHoverPause: true,
        dots: false,
        mouseDrag: false,
        touchDrag: false,
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


    /*NewCareer Carousel JS*/
    $("#career-carousel").owlCarousel({
        nav: false,
        loop: false,
        margin: 0,
        autoplayHoverPause: true,
        dots: true,
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
                items: 1,
                nav: false,
                dots: true,
                autoplay: true,

            }
        }
    });
    /*Events Slider*/
    $(".fourtiles-gallery").owlCarousel({
        loop: false,
        autoplayHoverPause: true,
        margin: 10,
        nav: false,
        dots: false,
        autoplay: false,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false
            },
            400: {
                items: 2,
                nav: false,
                dots: false
            },
            600: {
                items: 3,
                nav: false,
                dots: false
            },
            1000: {
                items: 4,
                nav: false,
                dots: false

            }
        }

    });

    /*Events Slider*/
    $('.three-tiles').owlCarousel({
        loop: true,
        autoplayHoverPause: true,
        margin: 10,
        nav: false,
        dots: false,
        responsiveClass: true,
        autoplay: false,
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


    /*Slider*/
    $('#four-thumb').owlCarousel({
        loop: true,
        margin: 10,
        autoplayHoverPause: true,
        nav: false,
        dots: false,
        responsiveClass: true,
        autoplay: true,
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
    });

    /*Slider*/
    $('.single-tiles').owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
        dots: false,
        responsiveClass: true,
        autoplayHoverPause: true,
        autoplay: true,
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
    });

    /*Amenities*/
    $('#amenities').owlCarousel({
        loop: true,
        margin: 10,
        autoplay: true,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 2000,
        autoplayHoverPause: true,
        responsiveClass: true,
        autoplayTimeout: 2500,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            0: {
                items: 2,
                nav: true,
                dots: false,
                nav: false,
                autoplay: true
            },
            600: {
                items: 3,
                nav: true,
                dots: false,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 3,
                nav: true,
                dots: false,
                margin: 20,
                autoplay: true

            }
        }
    });


    /*Events*/
    $('#events').owlCarousel({
        loop: true,
        margin: 20,
        animateOut: 'fadeOut',
        autoplayHoverPause: true,
        slideSpeed: 2000,
        responsiveClass: true,
        autoplayTimeout: 2500,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false,
                nav: false,
                autoplay: true
            },
            600: {
                items: 2,
                nav: true,
                dots: false,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 2,
                nav: true,
                dots: false,
                loop: true,
                margin: 20,
                autoplay: true

            }
        }
    });
    /* Initialize Testimonial Carousel on home page */
    $("#testimonials").owlCarousel({
        nav: true,
        loop: true,
        margin: 50,
        dots: false,
        autoplay: true,
        autoplayTimeout: 1500,
        autoplayHoverPause: true,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false,
                dots: false
            },
            // breakpoint from 480 up
            480: {
                items: 1,
                nav: false,
                dots: false
            },
            // breakpoint from 768 up
            768: {
                items: 1,
                autoplay: true
            },
            1200: {
                items: 2,
                autoplay: true
            }
        }
    });

    /* Initialize Testimonial Carousel on home page */
    $("#accolades").owlCarousel({
        nav: true,
        loop: true,
        margin: 50,
        autoplayHoverPause: true,
        dots: false,
        autoplay: true,
        autoplayTimeout: 1500,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 2,
                nav: false
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: false,
                dots: false,
                autoplay: true
            },
            1200: {
                items: 4,
                nav: true,
                dots: false,
                autoplay: true
            }
        }
    });

    /* Initialize Carousel */
    $("#carousel-one-column").owlCarousel({
        nav: true,
        loop: true,
        autoplayHoverPause: true,
        margin: 50,
        dots: false,
        autoplay: true,
        autoplayTimeout: 1500,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 1,
                nav: false
            },
            // breakpoint from 768 up
            768: {
                items: 1,
                nav: false,
                dots: false
            },
            1200: {
                items: 1,
                nav: true,
                dots: false
            }
        }
    });

    /* Initialize Carousel */
    $("#carousel-two-column").owlCarousel({
        nav: true,
        loop: true,
        autoplayHoverPause: true,
        margin: 50,
        dots: false,
        autoplay: true,
        autoplayTimeout: 1500,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 2,
                nav: false
            },
            // breakpoint from 768 up
            768: {
                items: 2,
                nav: false,
                dots: false,
                autoplay: true
            },
            1200: {
                items: 2,
                nav: true,
                dots: false,
                autoplay: false
            }
        }
    });

    /* Initialize Carousel */
    $("#carousel-three-column").owlCarousel({
        nav: true,
        autoplayHoverPause: true,
        loop: true,
        margin: 50,
        dots: false,
        autoplay: true,
        autoplayTimeout: 1500,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 2,
                nav: false
            },
            // breakpoint from 768 up
            768: {
                items: 2,
                nav: false,
                dots: false,
                autoplay: true
            },
            1200: {
                items: 3,
                nav: true,
                dots: false,
                autoplay: false
            }
        }
    });

    /*Menu*/
    $('#toggle-menu').on('click', function () {
        document.getElementById("myNav").style.height = "100%";
    });

    $('#toggle-menu-close').on('click', function () {
        document.getElementById("myNav").style.height = "0%";
    });






    /*Gallery Filter*/
    filterSelection("all")
    function filterSelection(c) {
        var x, i;
        x = document.getElementsByClassName("column");
        if (c == "all") c = "";
        for (i = 0; i < x.length; i++) {
            w3RemoveClass(x[i], "show");
            if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show");
        }
    }

    function w3AddClass(element, name) {
        var i, arr1, arr2;
        arr1 = element.className.split(" ");
        arr2 = name.split(" ");
        for (i = 0; i < arr2.length; i++) {
            if (arr1.indexOf(arr2[i]) == -1) { element.className += " " + arr2[i]; }
        }
    }

    function w3RemoveClass(element, name) {
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



    /*
    // Add active class to the current button (highlight it)
    var btnContainer = document.getElementById("myBtnContainer");
    var btns = btnContainer.getElementsByClassName("btn");
    for (var i = 0; i < btns.length; i++) {
      btns[i].addEventListener("click", function(){
        var current = document.getElementsByClassName("active");
        current[0].className = current[0].className.replace(" active", "");
        this.className += " active";
      });
    }
    */
    $(window).scroll(function () {
        if ($(this).scrollTop() > 200) {
            $('#back-to-top').fadeIn();
            // Scroll Down
            $('#toggle-menu').addClass('nav-down');
            $('.menu-inner').addClass('fixed-menu');
        } else {
            $('#back-to-top').fadeOut();
            // Scroll Down
            $('#toggle-menu').removeClass('nav-down');
            $('.menu-inner').removeClass('fixed-menu');
        }
    });
    // scroll body to 0px on click
    $('#back-to-top').click(function () {
        $('#back-to-top').tooltip('hide');
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });


    $(function () {
        $('#bookfrom').datetimepicker({ format: 'YYYY-MM-DD' });
    });

    $(function () {
        $('#bookto').datetimepicker({ format: 'YYYY-MM-DD' });
    });

});