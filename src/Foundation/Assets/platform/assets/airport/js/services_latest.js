// services.js

window.onload = function () {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
    setTimeout(() => {
        var navlinks = document.getElementsByClassName("tab-nav");
        for (var i = 0; i < navlinks.length; i++) {
            navlinks[i].classList.remove("active");
            if (i === 0) {
                navlinks[i].classList.add("active");
            }
        }
        document.body.scrollTop = 0; // For Safari
        document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
    }, 200);
};

window.onscroll = function () {
    try {
        let horizontaltabHeader = document.getElementById("navbar-tabs");
        if (window.pageYOffset < horizontaltabHeader.offsetTop && document.querySelector("#navbar-tabs .tab-nav.active") == null) {
            document.getElementsByClassName("tab-nav")[0].classList.add("active");
        }
    } catch (error) { }
};

function closeOffcanvas() {
    const { innerWidth: width, innerHeight: height } = window;
    if (width < 992) {
        try {
            var $myOffcanvas = document.getElementById("offcanvasBottomTest");
            var $myOffBackdrop =
                document.getElementsByClassName("offcanvas-backdrop")[0];
            $myOffcanvas.classList.remove("show");
            $myOffcanvas.classList.add("hide");
            $myOffBackdrop.classList.remove("show");
            $myOffBackdrop.classList.add("offcanvas-hide");
            document.body.style.removeProperty("overflow");
            document.body.style.removeProperty("padding-right");
        } catch (error) { }
    }
}

window.addEventListener("load", handleResize);
window.addEventListener("resize", handleResize);

function handleResize() {
    let $toggle_offcanvas = document.getElementById("offcanvasBottomTest");
    if (!$toggle_offcanvas) {
        return;
    }
    const { innerWidth: width, innerHeight: height } = window;
    if (width < 992) {
        $toggle_offcanvas.classList.remove("offcanvas-desktop");
    } else {
        $toggle_offcanvas.classList.add("offcanvas-desktop");
    }
}

let isClicked = false;
const onChange = () => {
    let tabContent = document.getElementById("v-pills-tabContent");
    let tab = document.getElementById("services-main-menu");
    if (isClicked) {
        tabContent.classList.replace(
            "services-container__expand",
            "services-container"
        );
        tab.classList.replace("services-menu__collapse", "services-menu");
        isClicked = !isClicked;
    } else {
        tabContent.classList.replace(
            "services-container",
            "services-container__expand"
        );
        tab.classList.replace("services-menu", "services-menu__collapse");
        isClicked = !isClicked;
    }
    setTimeout(() => {
        if (document.querySelector("#other-services")) {
            sliderNavigation("other-services");
        }
        if (document.querySelector("#Airports")) {
            sliderNavigation("Airports");
        }
    }, 400);
};

if (document.querySelector("#Airports")) {
    sliderNavigation("Airports");
}

if (document.querySelector("#other-services")) {
    sliderNavigation("other-services");
}



// pranaam-static-page.js

// airport-guide-service-page.js

jQuery(".slider-citytocity").slick({
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

jQuery(".slider-airportguide").slick({
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
            breakpoint: 480,
            nav: false,
            settings: {
                slidesToShow: 1.2,
                slidesToScroll: 1,
            },
        },
    ],
});


$('.detailTable').closest('.address-box').addClass('full_table')


if ($('#serviceList li').length < 11) {
    $('.searchSection').hide();
}

function mobileSlider(objID, cardClass = 'mobSlideItem') {
    jQuery(objID).addClass('scrollbar-x mobSlider');
    jQuery(objID).children().addClass(cardClass);
}
function sliderAdd() {
    if (window.innerWidth >= 991) {
        $('.inner-service-slider').slick({
            dots: true,
            infinite: true,
            speed: 300,
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: true,
            // variableWidth: true,
            prevArrow: '<i class="i-arrow-l slick-prev "></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 480,
                    nav: true,
                    settings: {
                        slidesToShow: 1.3,
                        slidesToScroll: 1,
                    },
                },
            ],
        });
        $('.parking-fac').slick({
            dots: true,
            infinite: true,
            speed: 300,
            slidesToShow: 2,
            slidesToScroll: 1,
            arrows: true,
            // variableWidth: true,
            prevArrow: '<i class="i-arrow-l slick-prev "></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1,
                    },
                },
                {
                    breakpoint: 480,
                    nav: true,
                    settings: {
                        slidesToShow: 1.3,
                        slidesToScroll: 1,
                    },
                },
            ],
        });
        $('.grid-2').slick({

            dots: false,

            infinite: false,

            speed: 300,

            slidesToShow: 2,

            slidesToScroll: 2,

            arrows: true,

            // variableWidth: true,

            prevArrow: '<i class="i-arrow-l slick-prev "></i>',

            nextArrow: '<i class="i-arrow-r slick-next"></i>',

            responsive: [

                {

                    breakpoint: 1200,

                    settings: {

                        slidesToShow: 4,

                        slidesToScroll: 3,

                    },

                },

                {

                    breakpoint: 480,

                    nav: false,

                    settings: {

                        slidesToShow: 1.3,

                        slidesToScroll: 1,

                    },

                },

            ],

        });
        $('.slider-grid-3').slick({
            dots: false,
            infinite: false,
            speed: 300,
            slidesToShow: 3,
            slidesToScroll: 3,
            arrows: true,
            // variableWidth: true,
            prevArrow: '<i class="i-arrow-l slick-prev "></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 3,
                    },
                },
                {
                    breakpoint: 480,
                    nav: false,
                    settings: {
                        slidesToShow: 1.3,
                        slidesToScroll: 1,
                    },
                },
            ],
        });
        $('.slider-currency').slick({
            dots: false,
            infinite: false,
            speed: 300,
            slidesToShow: 2,
            slidesToScroll: 1,
            arrows: true,
            // variableWidth: true,
            prevArrow: '<i class="i-arrow-l slick-prev "></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
        });
        $('.travel-nav-slider').slick({
            dots: false,
            infinite: false,
            speed: 300,
            slidesToShow: 6,
            slidesToScroll: 1,
            arrows: true,
            // variableWidth: true,
            prevArrow: '<i class="i-arrow-l slick-prev "></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 3,
                    },
                },
                {
                    breakpoint: 480,
                    nav: false,
                    settings: {
                        slidesToShow: 1.3,
                        slidesToScroll: 1,
                    },
                },
            ],
        });
    } else {
        mobileSlider(".slider-grid-2", 'mobSlideItem');
        mobileSlider(".slider-grid-3", 'mobSlideItem');
        mobileSlider(".slider-currency", 'mobSlideItem');
        mobileSlider(".travel-nav-slider", 'mobSlideItem');
        mobileSlider(".inner-service-slider", 'mobSlideItem');
        mobileSlider(".parking-fac", 'mobSlideItem');
    }
}
sliderAdd();


$(document).ready(function () {

    $('.tabs .tab-link').click(function () {
        var tab_id = $(this).attr('data-tab');

        $('.tabs .tab-link').removeClass('current');
        $('.tab-content').removeClass('current');

        $(this).addClass('current');
        $("#" + tab_id).addClass('current');

    })

    let tabNum = 0;

    window.setInterval(() => {
        if (tabNum < 3) {
            tabNum++
        }
        else {
            tabNum = 0;
        }
        $(`.tabs .tab-link:nth-of-type(${tabNum})`).click();
    }, 4000)

})

if(document.querySelectorAll(".tabcontent").length) {
    const buttonElement = document.querySelectorAll('.tablinks'); const tabContent = document.querySelectorAll(".tabcontent"); tabContent[0].style.display = "block"; buttonElement.forEach(function (i) { i.addEventListener('click', function (event) { for (let x = 0; x < buttonElement.length; x++) { if (event.target.id == buttonElement[x].id) { buttonElement[x].className = buttonElement[x].className.replace(" active", ""); tabContent[x].style.display = "block"; event.currentTarget.className += " active"; } else { tabContent[x].style.display = "none"; buttonElement[x].className = buttonElement[x].className.replace(" active", ""); } } }); });
}

