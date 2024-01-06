$(document).ready(function () {
    try {
        /* Expand/Collapse footer */
        $(".footerArrow").click(function () {
            $(".footerExpanded, .footerCollapsed").toggle();
            //$(".footerExpanded").addClass('collapsed');
        })

        /* Hide Leadership Bio  on close click */
        $(".leadershipBio .close").click(function () {
            $(this).parent().slideUp();
        })

        /* Expand the relevant leadership bio and hide others*/

        $(".member").click(function () {
            var id = $(this).attr('id');
            $(".leadershipBio .teamModal").slideUp();
            $(".leadershipBio #" + id + "_block").slideToggle();
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


        /*Partner Logo Carousel*/
        $(document).ready(function () {
            var owl = $('#partner-logos');
            owl.owlCarousel({
                margin: 10,
                nav: true,
                loop: false,
                responsive: {
                    0: {
                        items: 1,
                        nav: true,
                        dots: false
                    },
                    400: {
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
                        items: 5,
                        nav: true,
                        dots: false

                    }
                }
            })


        })



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

        /* Initialize Banner Carousel on home page */
        $("#bannerCarousel").owlCarousel({
            //autoplay:true,
            //autoplayTimeout:2000,
            //autoplayHoverPause:true,
            center: true,
            //loop:true,
            items: 1,
            //pagination : true,

        });

        $("#subCarousel").owlCarousel({
            //autoplay:true,
            //autoplayTimeout:2000,
            //autoplayHoverPause:true,
            center: true,
            //loop:true,
            items: 1,
            //pagination : true,

        });

        /* Initialize Projects Carousel on About us page */
        $("#prjtsCarousel").owlCarousel({
            autoplay: true,
            autoplayTimeout: 3000,
            autoplayHoverPause: true,
            center: true,
            loop: true,
            items: 1,
            pagination: true,

        });

        $("#gallery").owlCarousel({
            margin: 10,
            nav: true,
            loop: false,
            rewind: true,
            responsive: {
                0: {
                    items: 1,
                    nav: true,
                    dots: false
                },
                400: {
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
                    items: 5,
                    nav: true,
                    dots: false

                }
            }
        })


        var href = window.location.href;
        $('.primaryMenu li a').each(function (e, i) {
            if (href.indexOf($(this).attr('href')) >= 0) {
                $(this).parent().addClass('active');
            }
        });
    }


    catch (err) { console.log(err); }
})



function CloseModal(count) {
    jQuery('#video_' + count + " " + 'iframe').attr("src", jQuery("#video_" + count + " " + "iframe").attr("src"));
}

$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('#back-to-top').css('display', 'block');
        $('.navPanel').addClass('fixed-header')

    } else {
        $('#back-to-top').css('display', 'none');
        $('.navPanel').removeClass('fixed-header');
        $('.headerSec').addClass('top-fixed-bar');
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

$('#back-to-top').tooltip('show');
