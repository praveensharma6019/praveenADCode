/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem') {
    jQuery(objID).addClass('scrollbar-x mobSlider');
    jQuery(objID).children().addClass(cardClass);
}

function sliderAdd() {

    if (window.innerWidth >= 991) {

        jQuery(".cargo-slider").slick({
            slidesToShow: 2,
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
                        slidesToShow: 2,
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
        mobileSlider(".cargo-slider", 'mobSlideItem');
    }
}

sliderAdd();