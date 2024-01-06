function getRows(selector) {
    var height = jQuery(selector).height();
    var line_height = jQuery(selector).css('line-height');
    line_height = parseFloat(line_height)
    var rows = height / line_height;
    return Math.round(rows);
}
jQuery(document).ready(function () {
    jQuery('.text-wrapper').each(function (e) {
        if (!jQuery(this).hasClass('cacographic')) {
            if (getRows(jQuery(this)) > 2) {
                jQuery(this).addClass('cacographic');
            } else {
                jQuery(this).siblings('.sectionReadMore').remove();
            }
        }
    });
})
function sectionReadMoreToggle(obj) {
    if (jQuery(obj).siblings('.text-wrapper').hasClass('cacographic')) {
        jQuery(obj).siblings('.text-wrapper').removeClass("cacographic");
        jQuery(obj).siblings('.sectionReadMore').html('Read Less');
        jQuery(obj).html('Read Less');
    } else {
        jQuery(obj).siblings('.text-wrapper').addClass("cacographic");
        jQuery(obj).html('Read More');
    }
}


$(window).on("load", function () {
    $("#navTab a,a[rel='m_PageScroll2id']").mPageScroll2id({
        highlightSelector: "#navTab a",
        offset: 170,
        highlightClass: "selected",
        forceSingleHighlight: true
    });
});

$(document).ready(function () {

    $('#show-more').click(function () {
        $('table tbody tr:hidden').slice(0, 15).show();
        if ($('table tbody tr:hidden').length === 0) {
            $('#show-more').hide();
        }
    });

    //FAQs accordian
    $(".accordion").accordionjs({
        autoHeight: false,
        activeIndex: 1,
        //collapsible: true,
        heightStyle: "content",
        active: 0,
        closeOther: true,
        slideSpeed: 300,
        closeAble: true,
    });

    $(".acc_head").bind("click", function () {
        var self = this;
        setTimeout(function () {
            theOffset = $(self).offset();
            $("body,html").animate({ scrollTop: theOffset.top - 200 });
        }, 650); // ensure the collapse animation is done
    });

    var headerHeight = $(".site-header").outerHeight();
    console.log(headerHeight);
    var stickyNavTop = $('#pageTab').offset().top - headerHeight;
    var stickyNav = function () {
        var scrollTop = $(window).scrollTop();
        if (scrollTop > stickyNavTop) {
            $('#pageTab').addClass('sticky');
        } else {
            $('#pageTab').removeClass('sticky');
        }
    };
    stickyNav();
    $(window).scroll(function () {
        stickyNav();
    });

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


