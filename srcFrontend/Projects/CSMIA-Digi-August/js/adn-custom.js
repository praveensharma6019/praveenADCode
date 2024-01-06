$(document).ready(function () {
    // scrolling function js
    $(window).scroll(function () {
        var scroll = $(window).scrollTop();
        if (scroll >= 50) {
            $("body").addClass("scrolled");
        } else {
            $("body").removeClass("scrolled");
        }
    });
    // scrolling function js end
    // mobile navigation button
    $(".nav-toggle").on("click", function () {
        $("html").toggleClass("nav-mobile");
        $("body").removeClass("searching");
    });
    $(".nav-outer").on("click", function () {
        $("html").removeClass("nav-mobile");
    });
    $(".nav-inner").on("click", function () {
        event.stopPropagation();
    });

    // mobile navigation button end
    $('.nv-toggle').click(function (e) {
        e.preventDefault();
        var $this = $(this);
        $('.nav li.nav-item').removeClass('show');
        $this.parent().parent('li.nav-item').addClass('show');
    });

    // Hover search open		
    $(".btn-search").hover(function () {
        $("body").addClass("searching");
    });
    $(".dropdown-menu .search-close").on("click", function () {
        $("body").removeClass("searching");
    });
    // Hover search open end
    $('.mob-toggle').click(function () {
        $('body').addClass('scrolled');
    });
    var theOffset2 = $(".btn-smap").offset();
    $(".btn-smap").on("click", function () {
        $(this).toggleClass('clicked');
        $(".ft-navOuter").slideToggle();
        theOffset2 = $(this).offset();
        $("body,html").animate({ scrollTop: theOffset2.top - 100 });
    });
    // scroll body to 0px on click
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('#scrolltoTop').addClass('scrollIn');
        } else {
            $('#scrolltoTop').removeClass('scrollIn');
        }
    });
    $('#scrolltoTop').click(function () {
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });
});