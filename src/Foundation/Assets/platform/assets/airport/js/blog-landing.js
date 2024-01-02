if (document.querySelector('.main-header')) {
    document.querySelector('.main-header').classList.add('floating-header');
}
/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem') {
    jQuery(objID).addClass('scrollbar-x mobSlider');
    jQuery(objID).children().addClass(cardClass);
}

const getDeviceType = () => {
    const ua = navigator.userAgent;
    if (/(tablet|ipad|playbook|silk)|(android(?!.*mobi))/i.test(ua)) {
        return "tablet";
    }
    if (
        /Mobile|iP(hone|od)|Android|BlackBerry|IEMobile|Kindle|Silk-Accelerated|(hpw|web)OS|Opera M(obi|ini)/.test(
            ua
        )
    ) {
        return "mobile";
    }
    return "desktop";
};

console.log(getDeviceType())

function sliderWonderTales() {
    if (window.innerWidth >= 991) {
        /* only desktop */
        jQuery(".mumbai-airports").slick({
            slidesToShow: 5,
            slidesToScroll: 5,
            dots: false,
            infinite: false,
            nav: true,
            prevArrow: '<i class="i-arrow-l slick-prev"></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 5,
                        slidesToScroll: 5,
                        nav: false,
                    },
                },
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 4,
                        nav: false,
                    },
                },
                {
                    breakpoint: 767,
                    nav: false,
                    settings: {
                        slidesToShow: 3.2,
                        slidesToScroll: 3.2,
                        nav: false,
                    },
                },
            ],
        });
        jQuery(".wonder-slider").slick({
            slidesToShow: 4,
            slidesToScroll: 1,
            dots: false,
            infinite: false,
            nav: true,
            prevArrow: '<i class="i-arrow-l slick-prev"></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        prevArrow: false,
                        nextArrow: false,
                    },
                },
                {
                    breakpoint: 768,
                    nav: false,
                    settings: {
                        slidesToShow: 1.5,
                        slidesToScroll: 1,
                        prevArrow: false,
                        nextArrow: false,
                    },
                },
                {
                    breakpoint: 480,
                    nav: false,
                    settings: {
                        slidesToShow: 1.5,
                        slidesToScroll: 1,
                        prevArrow: false,
                        nextArrow: false,
                    },
                },
            ],
        });
        jQuery(".slider-citytocity").slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            dots: false,
            infinite: false,
            nav: true,
            prevArrow: '<i class="i-arrow-l slick-prev"></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2,
                        prevArrow: false,
                        nextArrow: false,
                    },
                },
                {
                    breakpoint: 768,
                    nav: false,
                    settings: {
                        slidesToShow: 1.5,
                        slidesToScroll: 1,
                        prevArrow: false,
                        nextArrow: false,
                    },
                },
                {
                    breakpoint: 480,
                    nav: false,
                    settings: {
                        slidesToShow: 1.5,
                        slidesToScroll: 1,
                        prevArrow: false,
                        nextArrow: false,
                    },
                },
            ],
        });

    } else {
        mobileSlider(".mumbai-airports,.slider-citytocity,.wonder-slider", 'mobSlideItem');
    }
}
sliderWonderTales();

setTimeout(function () {
    jQuery('.mobile-slider').removeClass('blank-holder');
    jQuery('.blank').removeClass('blank');
}, 3000);


$(document).ready(function () {
    if ($(".slide").length < 11)
    {
        $("#loadMore").fadeOut(0)
    }
    $(".slide").slice(0, 10).show();
    $("#loadMore").on("click", function (e) {
        e.preventDefault();
        $(".slide:hidden").slice(0, getDeviceType() === 'desktop' ? 5 : 2).slideDown();
        if ($(".slide:hidden").length == 0) {
            $("#loadMore").text("No Content").addClass("noContent");
        }
    });
});



$('.video').parent().click(function () {
    if ($(this).children(".video").get(0).paused) {
        $(this).children(".video").get(0).play();
        $(this).children(".playpause").fadeOut();
    } else {
        $(this).children(".video").get(0).pause();
        $(this).children(".playpause").fadeIn();
    }
});
