/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem') {
    jQuery(objID).addClass('scrollbar-x mobSlider');
    jQuery(objID).children().addClass(cardClass);
}
function sliderAdd() {

    if (window.innerWidth >= 991) {
        /* only desktop */
        jQuery(".slider-airports").slick({
            slidesToShow: 4,
            slidesToScroll: 4,
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
                    },
                },
                {
                    breakpoint: 480,
                    nav: false,
                    settings: {
                        slidesToShow: 1.5,
                        slidesToScroll: 1,
                    },
                },
            ],
        });

        window.onload = function () {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
            setTimeout(() => {
                var navlinks = document.getElementsByClassName("tab-nav");
                for (var i = 0; i < navlinks.length; i++) {
                    navlinks[i].classList.remove('active');
                    if (i === 0) {
                        navlinks[i].classList.add('active');
                    }
                }
                document.body.scrollTop = 0; // For Safari
                document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera  
            }, 200);
        }
    } else {
        mobileSlider(".slider-airports", 'mobSlideItem');
        mobileSlider(".mumbai-airports", 'mobSlideItem');
    }
}

sliderAdd();

window.onscroll = function () {
    try {
        let horizontaltabHeader = document.getElementById("navbar-tabs");
        var navlinks = document.getElementsByClassName("tab-nav");
        if (document.querySelectorAll("#navbar-tabs .nav-link.active").length < 1) {
            navlinks[0].classList.add('active');
        }
        // console.log(horizontaltabHeader.offsetTop);
        // console.log(window.pageYOffset < horizontaltabHeader.offsetTop);
        // if (window.pageYOffset < horizontaltabHeader.offsetTop) {
        //       document.getElementsByClassName("tab-nav")[0].classList.add('active');
        // }
    } catch (error) {
    }
};


