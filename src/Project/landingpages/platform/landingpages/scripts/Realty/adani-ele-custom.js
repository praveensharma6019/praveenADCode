$(document).ready(function () {



    /* Initialize Carousel  on energy calculator*/
    $("#energyCarousel").owlCarousel({
        nav: true,
        navText: [],
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 2
            },
            // breakpoint from 480 up
            480: {
                items: 3
            },
            // breakpoint from 768 up
            768: {
                items: 3
            },
            // breakpoint from 992 up
            992: {
                items: 4
            },
            // breakpoint from 1025 up
            1025: {
                items: 5
            }
        }
    });

    /* Initialize Carousel on home page */
    $(".owl-carousel").owlCarousel({
        nav: true,
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
                items: 3,
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            },
            1000: {
                nav: true,
                loop: false,
                touchDrag: false,
                mouseDrag: false
            }
        }
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
        $('.overlay-top').addClass('active');

    });

    /* For smooth page scroll on Top menu link */
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();

            document.querySelector(this.getAttribute('href')).scrollIntoView({
                behavior: 'smooth'
            });
        });
    });
})
/*Bootstrap DatePicker JS*/
$(function () {
    $('#datetimepicker1').datetimepicker({ format: 'DD/MM/YYYY' });
});
