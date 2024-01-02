"use strict";
! function(p) {

	var $window = $(window);

    /* WOW JS
    * ------------------------------------------------------ */
    if ($window.width() > 1100) {
        new WOW().init();
    }
	

    /*-----------------------------------
    // SCROLL TO FIX CLASS
    *-----------------------------------*/

    // $(window).scroll(function() {
    //     var currentScroll = $(window).scrollTop();
    //     if (currentScroll >= 20) {
    //         $('header').addClass('is-scrolling');
    //     } else {
    //         $('header').removeClass("is-scrolling");
    //     }
    // });

    
    /*-----------------------------------
     * NAVBAR CLOSE ON CLICK
     *-----------------------------------*/

    $('.navbar-nav > li:not(.dropdown) > a').on('click', function() {
        $('.navbar-collapse').collapse('hide');
    });

    $('.mobile-toggle').click(function() {
        $(this).toggleClass('show');
        $('.main-menu').toggleClass('show');
        $('body').toggleClass('menushow');
    });

    
    /* Back to Top
    * ------------------------------------------------------ */
    var clBackToTop = function() {
        var scrollPos = 0;
        var element = $('#back2top');
        $(window).scroll(function() {
            var scrollCur = $(window).scrollTop();
            if (scrollCur > scrollPos) {
                // scroll down
                if (scrollCur > 200) {
                    element.addClass('active');
                } else {
                    element.removeClass('active');
                }
            } else {
                // scroll up
                element.removeClass('active');
            }

            scrollPos = scrollCur;
        });

        element.on('click', function() {
            $('html, body').animate({
                scrollTop: '0px'
            }, 800);
        })
    };


    /* Initialize
    * ------------------------------------------------------ */
    (function clInit() {
        clBackToTop();

    })();
	

	 /* FORM VALIDATION
    * ------------------------------------------------------ */
	
	$(document).ready(function() {
        $(':input[type="submit"]').prop('disabled', false);
    });


	/* TOOLTIP */

	$(function () {
	  $('[data-toggle="tooltip"]').tooltip()
	});


	/* Owl Carousel
    * ------------------------------------------------------ */
	$('.seamlessly-slider').owlCarousel({
        loop: true,
        margin: 0,
        autoplay: false,
        navText: ["<i class='left-arrow'></i>", "<i class='right-arrow'></i>"],
        nav: true,
        dots: false,
        responsive: {
            0: {
                items: 1,
            },
            500: {
                items: 3,
            },
            768: {
                items: 4,
            },
            1024: {
                items: 5,
            },
        },
    });


    if ($.fn.onePageNav) {
        $('#nav').onePageNav({
            currentClass: 'active',
            scrollSpeed: 1500,
            easing: 'easeOutQuad'
        });
    }

    $('[data-fancybox="gallery"]').fancybox({
        afterLoad : function(instance, current) {
            var pixelRatio = window.devicePixelRatio || 1;

            if ( pixelRatio > 1.5 ) {
                current.width  = current.width  / pixelRatio;
                current.height = current.height / pixelRatio;
            }
        }

    });
    
}(jQuery);