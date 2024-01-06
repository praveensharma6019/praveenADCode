$(function () {
    window.onload = function () {

        var $oe_menu = $('.mainNav');
        var $oe_menu_items = $oe_menu.children('li');
        $(".selecte").on("mouseover", function () {
            var suLi = $(this).offset().left;
            var su = $('.mainNav >li:last').offset().left;
            var suTotal = su - suLi;
            $(this).children('.navBg').css({
                "left": suTotal - 612 + "px"
            });
            $(this).addClass('selecte');
            $(this).children('.navBg').stop().slideDown(0);

            if (!$(this).children("div").children("ul").children("li").hasClass("subLi_select")) {
                $('.navText').css('display', 'block');
            }
        }).on('mouseleave', function () {
            $('.navText').css('display', 'none');
            $(this).removeClass('selecte');
            $(this).children('.navBg').stop().slideUp(0);
        });

        $('ul.subNav > li').on('mouseenter', function () {
            $('.navText').css('display', 'none');
            $('.subLi_select').removeClass('subLi_select');
            $(this).addClass('subLi_select');
            $('.subNav_thirdBg').hide();

            var topPos = $(this).index();
            var liHgt = $(this).height();
            var totalHgt = topPos * liHgt + 1;

            $(this).find('.subNav_thirdBg').css({
                //'top': "-" + (totalHgt - 135) + "px"
            });
            $(this).find('.subNav_thirdBg').show();
            $('.navText').css('display', 'none');
        }).on('mouseleave', function () {
            $('.subLi_select').removeClass('subLi_select');
            $(this).find('.subNav_thirdBg').hide();
            $('.navText').css('display', 'block');
        });

        $('ul.thirdNav > li').on('mouseenter', function () {
            $('.thirdLi_select').removeClass('thirdLi_select');
            $(this).addClass('thirdLi_select');
            $('.FourthNav').hide();

            var topPos = $(this).index();
            var liHgt = $(this).height();
            var totalHgt = topPos * liHgt + 1;
            $(this).find('.FourthNav').css({
                'top': "-" + totalHgt + "px"
            })

            $(this).find('.FourthNav').show();
        }).on('mouseleave', function () {
            $(this).addClass('thirdLi_select');
            $(this).find('.FourthNav').show();
        });

    };
});

$(window).on("beforeunload", function () {
    $(window).scrollTop(0)
})

$(document).ready(function () {
    $(window).onscroll(function () {
        $(this).scrollTop() < 100 ? $(".scrollToBottom").fadeIn() : $(".scrollToBottom").fadeOut()
    })
})
$(document).ready(function () {
    $(document).click(function (e) {
        //$(e.target).is("a") || $(".collapse").collapse("hide")
    }), $(document).on("click", ".navbar-collapse.in", function (e) {
        $(e.target).is("a") && "dropdown-toggle" != $(e.target).attr("class") && $(this).collapse("hide")
    })
})

$(document).ready(function () {
    $(window).scroll(function () {
        $(this).scrollTop() > 100 ? $(".scrollToTop").fadeIn() : $(".scrollToTop").fadeOut()
    }), $(".scrollToTop").click(function () {
        return $("html, body").animate({
            scrollTop: 0
        }, 800), !1
    })
})
$(document).ready(function () {
    $('#groupWeb_carsol').owlCarousel({
        center: false,
        loop: false,
        nav: true,
        lazyLoad: true,
        margin: 25,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 4
            },
            960: {
                items: 5
            },
            1200: {
                items: 6
            }
        }
    })
});

$(document).ready(function () {
    if ($(window).width() <= 910) {
        $('#btnSearch').click(function () {
            if ($('.mbl_searchBox').css('display') == 'none') {
                $('.mbl_searchBox').show();
            } else {
                $('.mbl_searchBox').hide();
            }
        })
        $(window).scroll(function () {
            if ($(window).scrollTop() <= 20) {

                if ($('.headerPort').hasClass('headerDown')) {
                    $('.headerPort').removeClass('headerDown');
                }
            } else {

                $('.headerPort').addClass('headerDown');
            }
        });
    } else {
        $(window).scroll(function () {
            //if ($(window).scrollTop() <= 30) {
            if ($(window).scrollTop() <= 1) {
                if ($('.headerPort').hasClass('headerDown')) {
                    //alert("AA");
                    $('.headerPort').removeClass('headerDown');

                }
            } else {
                //alert("BB");
                $('.headerPort').addClass('headerDown');

            }
        });
    }


    $('#menu-toggle').click(function () {
        $(this).toggleClass('maneActive');
        $('.mainNav').slideToggle(300);
        setTimeout(function () {
            $('.mainNav li').toggleClass('navSlid');
        }, 200);
        return false;
    });

    var x = document.getElementsByTagName("header")[0].getAttribute("class");
    if (x == "headerPort headerDown") {

        $('div.bQF').removeClass('act');
    }
    if ($('.headerPort').hasClass('headerDown')) {
        //alert("AA");
        $('.headerForm').css('display', 'none');

    }
    function popup() {
        $('.mid_layer_popup').fadeIn(300);
    }
    $("#popupQuery").click(popup);

    function close_popup() {
        $('.mid_layer_popup').fadeOut(300);
    }
    $("#closePopup").click(close_popup);

    function btnNormalback() {
        var top = 1;
        sessionStorage.setItem("tops", top);
    }
    $("#btnNormalback.btnNormal").click(btnNormalback);
})

function hdrSearch() {
    $('.hdrSearchPanel').toggleClass('hdrSearchPanel_act')
}


function scrollSpy() {

    var sections = ['i1', 'offerings', 'group-link', 'i4', 'i5'];

    var current;

    for (var i = 0; i < sections.length; i++) {

        var top = ($('#' + sections[i]).offset() || { "top": NaN }).top - 95;

        if (top <= $(window).scrollTop()) {

            current = sections[i];

        }

    }


    $(".scollspy a[href='#" + current + "']").addClass('active');

    $(".scollspy a").not("a[href='#" + current + "']").removeClass('active');

}

// smooth scrolling navigation

$(".scollspy a").click(function () {

    var target = $(this).attr("href");

    $("body, html").animate({

        scrollTop: target.offset().top

    }, 500);

    return false;

});

//scrollSpy call

$(document).ready(function () {

    scrollSpy();

});

$(window).scroll(function () {

    scrollSpy();

});



// $(document).ready(function () {

//     var z = document.createElement('p'); // is a node
//     z.innerHTML = "<div id='sentFeedbackSuccess' class='modal fade' role='dialog'><div class='modal-dialog'><div class='modal-content'><div class='modal-header'> <button type='button' class='close' data-dismiss='modal'>&times;</button></div><div class='modal-body text-center'><h4 class='ad-secondary-color' id='messageSuccess'>Thank you for contacting us.</h4><p id='messageShort'>We have received your enquiry and shall get back to you shortly.</p></div><div class='modal-footer'> <button type='button' class='btn btn-default' data-dismiss='modal'>Close</button></div></div></div></div>";
//     document.body.appendChild(z);

//     (function (i, s, o, g, r, a, m) {
//         i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
//             (i[r].q = i[r].q || []).push(arguments)
//         }, i[r].l = 1 * new Date(); a = s.createElement(o),
//             m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
//     })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

//     ga('create', 'UA-73097506-12', 'auto');
//     ga('send', 'pageview');
// })