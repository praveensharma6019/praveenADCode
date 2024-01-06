$(document).ready(function () {
    $('#club').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            600: {
                items: 2,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 3,
                nav: true,
                dots: false,
                loop: false,
                margin: 20,
                autoplay: true

            }
        }
    });


    $("#gallery1").owlCarousel({
        loop: false,
        margin: 10,
        nav: false,
        dots: false,
        autoplay: true,
        responsive: {
            0: {
                items: 2,
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
    $("#gallery-bottom").owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
        dots: false,
        autoplay: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false
            },
            400: {
                items: 1,
                nav: false,
                dots: false
            },
            600: {
                items: 1,
                nav: false,
                dots: false
            },
            1000: {
                items: 1,
                nav: false,
                dots: false

            }
        }

    });

    /*navigation*/
    $(".nav-tabs a").click(function (e) {
        e.stopPropagation()
        $(this).tab('show');
    });

    /*Rera*/
    $(window).on('load', function () {
        $('#reradisclaimer').modal('show');
    });

    /* partners Carousel For about us */
    $("#partners-carousel").owlCarousel({
        loop: false,
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
    });
    /* partners Carousel For about us */
    $("#blog-videos").owlCarousel({
        loop: false,
        margin: 10,
        autoplay: true,
        autoplayTimeout: 1500,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 2,
                nav: true,
                dots: false
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: false,
                dots: false
            }
        }
    });
    /* Accolades For about us */
    $("#accoladesslider").owlCarousel({
        loop: true,
        margin: 10,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 2,
                nav: true
            },
            // breakpoint from 480 up
            480: {
                items: 3,
                nav: true,
                dots: false
            },
            // breakpoint from 768 up
            768: {
                items: 4,
                nav: true,
                dots: false
            }
        }
    });
    /* Initialize Latest Projects Carousel on home page */
    $("#testimonials").owlCarousel({
        nav: false,
        loop: true,
        margin: 30,
        dots: true,
        autoplay: true,
        autoplayTimeout: 1500,
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
                autoplay: true
            },
            1200: {
                items: 2,
                nav: false,
                dots: true,
                autoplay: true
            }
        }
    });

    $('#searchproperty').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: false
            },
            600: {
                items: 2,
                nav: false,
                dots: false,
                nav: false,
                autoplay: false
            },
            1200: {
                items: 3,
                nav: true,
                dots: false,
                loop: false,
                margin: 20,
                autoplay: false

            }
        }
    });

    $('#propertytestimonials').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: false
            },
            600: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: false
            },
            1200: {
                items: 1,
                nav: false,
                dots: true,
                loop: false,
                margin: 20,
                autoplay: false

            }
        }
    });

    $('#residential').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            600: {
                items: 2,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            1200: {
                items: 3,
                nav: true,
                dots: false,
                loop: true,
                margin: 20,
                autoplay: true

            }
        }
    });
    $('#commercial').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            600: {
                items: 2,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 3,
                nav: true,
                dots: false,
                loop: false,
                margin: 20,
                autoplay: true

            }
        }
    });

    /* Accolades For about us */
    $("#blog-carousel").owlCarousel({
        loop: true,
        margin: 150,
        autoplay: true,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 2,
                nav: false,
                dots: false
            },
            // breakpoint from 768 up
            768: {
                items: 1,
                nav: false,
                dots: false
            },
            1000: {
                items: 2,
                nav: false,
                dots: false
            }
        }
    });


    $('#carousel').owlCarousel({
        loop: true,
        animateOut: 'fadeOut',
        margin: 10,
        responsiveClass: true,
        autoplayTimeout: 7000,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            600: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                loop: true,
                margin: 20,
                autoplay: true

            }
        }
    });

    $('#residential-slider').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                autoplay: true
            },
            600: {
                items: 1,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 1,
                nav: false,
                loop: true,
                margin: 20,
                dots: true,
                autoplay: true
            }
        }
    });

    $('#commercial-slider').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                autoplay: true
            },
            600: {
                items: 1,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 1,
                nav: false,
                loop: true,
                margin: 20,
                dots: true,
                autoplay: true
            }
        }
    });


    $('#pcarousel').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 3,
                nav: false,
                dots: false,
                autoplay: true
            },
            600: {
                items: 4,
                nav: false,
                dots: false,
                autoplay: true
            },
            1000: {
                items: 5,
                nav: false,
                loop: true,
                margin: 20,
                dots: false,
                autoplay: true
            }
        }
    });

    /*Media Carousel JS*/
    $("#media-carousel").owlCarousel({
        nav: false,
        loop: false,
        margin: 30,
        dots: true,
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
                items: 3,
                nav: false,
                dots: true,
                autoplay: true,

            }
        }
    });
    /*News Carousel JS*/
    $("#news-carousel").owlCarousel({
        nav: true,
        loop: false,
        margin: 30,
        dots: true,
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
                items: 2,
                nav: false,
                dots: true,
                autoplay: true,

            }
        }
    });


    /*News Carousel JS*/
    $("#media-coverage").owlCarousel({
        nav: true,
        loop: false,
        margin: 30,
        dots: true,
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
                dots: false,
                autoplay: true,

            }
        }
    });

    /*NewCareer Carousel JS*/
    $("#career-carousel").owlCarousel({
        nav: false,
        loop: false,
        margin: 0,
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

    $('.dropdown').on("hover", function (e) {
        $(this).next('ul').toggle();
        e.stopPropagation();
        e.preventDefault();
    });

    $('.adv-search').click(function () {
        $('.bannerForm').toggleClass('navtop navb');
    });

    $('.ad-inner').click(function () {
        $('.bannerForm').toggleClass('navtop');
    });

    /*Team tiles*/
    $('#leadershipteam').on("click", function () {
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



    /*Team 1*/
    $('#team-1').on("click", function () {
        $('#team-01').slideDown(700, function () {
            $('#team-01').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-01').on("click", function () {
        $('#team-01').slideDown(700, function () {
            $('#team-01').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /*Team 1*/

    /*Team 2*/
    $('#team-2').on("click", function () {
        $('#team-02').slideDown(700, function () {
            $('#team-02').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-02').on("click", function () {
        $('#team-02').slideDown(700, function () {
            $('#team-02').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /*Team 2*/


    /*Team 3*/
    $('#team-3').on("click", function () {
        $('#team-03').slideDown(700, function () {
            $('#team-03').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-03').on("click", function () {
        $('#team-03').slideDown(700, function () {
            $('#team-03').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /* /Team 3*/

    /*Team 4*/
    $('#team-4').on("click", function () {
        $('#team-04').slideDown(700, function () {
            $('#team-04').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-04').on("click", function () {
        $('#team-04').slideDown(700, function () {
            $('#team-04').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /* /Team 4*/

    $('#accolades').on("click", function () {
        $('.accoladess').slideDown(700, function () {
            $('.accoladess').show(700, "");
            $('#team-tile').hide(400, "");
        });
    });

    /*Partners Carousel*/
    $('#partnercarousel0').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 3,
                nav: false,
                autoplay: true,
                dots: false,
                loop: false
            },
            600: {
                items: 4,
                nav: false,
                autoplay: true,
                dots: false,
                loop: false
            },
            1000: {
                items: 5,
                nav: false,
                loop: true,
                margin: 20,
                dots: false,
                autoplay: true,
                loop: false
            }
        }
    });

    var hashValue = location.hash;
    hashValue = hashValue.replace(/^#/, '');
    //do something with the value here 
    if (hashValue === 'accoladess') {
        $('.accoladess').show(700, "");
        if (window.location.href.indexOf("#accolades") > 0) {

            $('html, body').animate({
                scrollTop: $("#accolades").offset().top
                    - 100
            }, 1000);
        }
    }
});

/*Gallery Popup*/

$("#gallery").owlCarousel({
    loop: false,
    margin: 10,
    nav: false,
    dots: false,
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
            items: 3,
            nav: true,
            dots: false

        }
    }

});
$("#gallery1").owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    dots: false,
    autoplay: true,
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
            items: 5,
            nav: false,
            dots: false

        }
    }

});
$("#projectstatus").owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    dots: false,
    autoplay: true,
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
            items: 3,
            nav: false,
            dots: false

        },
        1500: {
            items: 4,
            nav: false,
            dots: false

        }
    }

});
/*Scroll*/
$(function () {
    $('.submenu a[href*=#]').on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: $($(this).attr('href')).offset().top }, 500, 'linear');
    });
});

/*Mega Menu*/
$(function () {
    var selectedClass = "";
    $(".fil-cat").click(function () {
        selectedClass = $(this).attr("data-rel");
        $("#tabbedmenu").fadeTo(100, 0.1);
        $("#tabbedmenu li").not("." + selectedClass).fadeOut(50).removeClass('scale-anm');
        setTimeout(function () {
            $("." + selectedClass).fadeIn().addClass('scale-anm');
            $("#tabbedmenu").fadeTo(300, 1);
        }, 300);

    });
});

jQuery('.tablinks').click(function (event) {
    //remove all pre-existing active classes
    jQuery('.active-tab').removeClass('active-tab');

    //add the active class to the link we clicked
    jQuery(this).addClass('active-tab');

    //Load the content
    //e.g.
    //load the page that the link was pointing to
    //$('#content').load($(this).find(a).attr('href'));      

    event.preventDefault();
});


/*Scroll*/
$(function () {
    $('.submenu a[href*=#]').on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({ scrollTop: $($(this).attr('href')).offset().top }, 500, 'linear');
    });
});

function CloseModal(count) {
    count++;
    jQuery('#Video' + count + " " + 'iframe').attr("src", jQuery("#Video" + count + " " + "iframe").attr("src"));
}

/*Mega Menu*/
$(function () {
    var selectedClass = "";
    $(".fil-cat").click(function () {
        selectedClass = $(this).attr("data-rel");
        $("#tabbedmenu").fadeTo(100, 0.1);
        $("#tabbedmenu li").not("." + selectedClass).fadeOut(50).removeClass('scale-anm');
        setTimeout(function () {
            $("." + selectedClass).fadeIn().addClass('scale-anm');
            $("#tabbedmenu").fadeTo(300, 1);
        }, 300);

    });
});
/*NewCareer Carousel JS*/
$("#career-carousel").owlCarousel({
    nav: false,
    loop: false,
    margin: 0,
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



$("#GoBackBtn").click(function () {
    window.history.back();
});

/*Scroll To top JS*/
// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("scrolltop").style.display = "block";
    } else {
        document.getElementById("scrolltop").style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
$("#disclaimerbtn").click(function () {
    $('#roadblock').modal();
});



// When the user scrolls the page, execute myFunction 
window.onscroll = function () { myFunction() };

// Get the navbar
var navbar = document.getElementsByClassName("navPanel");

// Get the offset position of the navbar
var sticky = navbar[0].offsetTop;

// Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
function myFunction() {
    if (window.pageYOffset >= sticky) {
        navbar[0].classList.add("sticky")
    } else {
        navbar[0].classList.remove("sticky");
    }
}


$(".scale-anm")
    .on("mouseenter", function (e) {
        if ($($(e.currentTarget)[0]).find('img').length > 0) {
            for (var i = 0; i < $("#ulimg").children().length - 1; i++) {
                $($($("#ulimg").children())[i]).addClass("hide-ul");
            }
            if (($("#ulimg :nth-child(4)")[0]).style.display == "none") {
                ($("#ulimg :nth-child(4)")[0]).style.display = "block";
            }
            if ($($(e.currentTarget)[0]).find('img')[0] != undefined) {
                $($(e.currentTarget)[0]).find('img').clone().appendTo($("#limenu"))
                $($("#limenu")[0]).children()[0].style.display = "block";
            }
            //$($($($("#ulimg").children())[$("#ulimg").children().length - 1])).find('img')[0].src = $($(e.currentTarget)[0]).find('img')[0].src;
        }
    })
    .on("mouseleave", function (e) {
        for (var i = 0; i < $("#ulimg").children().length - 1; i++) {
            $($($("#ulimg").children())[i]).removeClass("hide-ul");
        }
        for (var i = 0; i <= $("#limenu").children().length; i++) {
            $($("#limenu").children()).remove(i);
        }
        //if ($($(e.currentTarget)[0]).find('img')[0] != undefined) {
        //    $("#limenu").remove($($(e.currentTarget)[0]).find('img')[0]);
        //}
        //$($($($("#ulimg").children())[$("#ulimg").children().length - 1])).find('img')[0].src = "";
    });
