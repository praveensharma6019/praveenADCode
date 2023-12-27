/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem') {
    jQuery(objID).addClass('scrollbar-x mobSlider');
    jQuery(objID).children().addClass(cardClass);
}

function sliderAdd() {

    if (window.innerWidth >= 991) {
        /* only desktop */
        jQuery(".mumbai-airport").slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            dots: true,
            infinite: true,
            nav: true,
            autoplay: true,
            autoplaySpeed: 6000,
            prevArrow: '<i class="i-arrow-l slick-prev"></i>',
            nextArrow: '<i class="i-arrow-r slick-next"></i>',
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        nav: false,

                    },
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1,
                        nav: false,
                    },
                },
            ],
        });

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
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 2,
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

        jQuery(".mumbai-airports").slick({
            slidesToShow: 5,
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
                        slidesToShow: 3.4,
                        slidesToScroll: 3.4,
                        nav: false,
                    },
                },
            ],
        });

    } else {
        mobileSlider(".slider-airports", 'mobSlideItem');
        mobileSlider(".mumbai-airports", 'mobSlideItem');
    }
}

sliderAdd();

function readMore(obj, ID, focusId='') {
    targetObj = jQuery('#' + ID);
    if (!targetObj.hasClass('show')) {
        targetObj.addClass('show');
        targetObj.fadeIn(400);
        jQuery(obj).text('Read Less');
    } else {
        targetObj.removeClass('show');
        targetObj.fadeOut(400);
        jQuery(obj).text('Read More');
        if (focusId != '') {
            setTimeout(function () {
                if (document.getElementById(focusId)) {
                    window.scrollTo({
                        top: document.getElementById(focusId).offsetTop - document.querySelector('#header .main-header').clientHeight - 10,
                        behavior: "smooth",
                    });
                }
            }, 400);
        }
    }

}

function disableVideoControls() {
    var video = document.getElementsByClassName("videoPlayer");
    video.removeAttribute("controls");
}
var vids = $("video");
$.each(vids, function () {
    this.controls = false;
});


setTimeout(function () {
    jQuery('.airport-features, .mobile-slider, .slider-airports, .mobSlider').removeClass('blank-holder');
    jQuery('.blank').removeClass('blank');
}, 3000);

function sfunc() {
    if (sessionStorage.shimmerOnce === undefined && document.querySelector('.ShimmerWrap')) {
        sessionStorage.shimmerOnce = 1;
        jQuery('.ShimmerWrap').addClass('blank-holder');
        jQuery('.Shimmeritem').addClass('blank');
    }
}
window.addEventListener('load', sfunc);


function gtmEventCategory(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page";
        var subcategory = jQuery(element).attr("data-subcategory");
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory
        });
    }
}

function gtmLinkSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var type = jQuery(element).attr("data-type");
        var label = jQuery(element).text();
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label,
            type: type
        });
    }
}
function gtmBottomTilesClickEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = "help_and_support";
        var label = jQuery(element).attr("data-label");
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label
        });
    }
}
if(document.querySelector(".mumbai-airport .slick-prev")){
    document.querySelector(".mumbai-airport .slick-prev").addEventListener("click", function (e) {
        gtmViewBannerClickEvent("left");
    })
    document.querySelector(".mumbai-airport .slick-next").addEventListener("click", function (e) {
        gtmViewBannerClickEvent("right");
    })
    document.querySelectorAll(".mumbai-airport .slick-dots li").forEach(x => {
        x.addEventListener("click", function (e) {
            gtmViewBannerClickEvent("dot");
        })
    })
}
function gtmViewBannerClickEvent(type) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = "view_banner";
        var category = "home_page"; //jQuery(this).attr("data-category");
        var label = document.querySelector(".slick-active .banner-content h3").textContent;
        var subcategory = label;
        var bannerCategory = "Hero/Slider Banner";
        var source = label;
        window.dataLayer.push({
            event: "view_banner",
            category: category,
            sub_category: subcategory,
            banner_category: bannerCategory,
            label: label,
            type: type,
            source:source
        });
    }
}
function gtmBannerClickEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = "view_banner";
        var category = "home_page"; //jQuery(this).attr("data-category");
        var label = jQuery(element).attr("data-label");
        var subcategory = jQuery(element).attr("data-subcategory");
        var bannerCategory = jQuery(element).attr("data-bannercategory");
        var source = jQuery(element).attr("data-label");
        var type1 = "Select";
        window.dataLayer.push({
            event: "view_banner",
            category: category,
            sub_category: subcategory,
            banner_category: bannerCategory,
            label: label,
            type: type1,
            source: source
        });
    }
}
function gtmKnowMoreClickEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var bannerCategory = jQuery(element).attr("data-bannercategory");
        var label = jQuery(element).attr("data-label");
        if (subcategory == "") {
            subcategory = label;
        }
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            banner_category: bannerCategory,
            label: label
        });
    }
}

function gtmTopNavigationClickEvent() {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = "view_top_navigation";
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = "top_nav";
        var label = "Business";
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label
        });
    }
}

if (document.querySelectorAll(".gtmLinkClick")) {
    document.querySelectorAll(".gtmLinkClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmLinkSelectEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmKnowMoreClick")) {
    document.querySelectorAll(".gtmKnowMoreClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmKnowMoreClickEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmViewBannerClick")) {
    document.querySelectorAll(".gtmViewBannerClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmViewBannerClickEvent('select');
            })
        }
    })
}
if (document.querySelectorAll(".gtmBannerClick")) {
    document.querySelectorAll(".gtmBannerClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmBannerClickEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmEventClick")) {
    document.querySelectorAll(".gtmEventClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmEventCategory(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmBottomTileClick")) {
    document.querySelectorAll(".gtmBottomTileClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmBottomTilesClickEvent(e.currentTarget);
            })
        }
    })
}
if (document.querySelectorAll(".gtmdutyFreeCategoriesClick")) {
    document.querySelectorAll(".gtmdutyFreeCategoriesClick").forEach(x => {
        if (jQuery(x).attr('href') != "#") {
            x.addEventListener("click", function (e) {
                gtmdutyFreeCategoriesClickEvent(e.currentTarget);
            })
        }
    })
}
function gtmAirportSelectEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = "airport_selection";
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var businessunit = jQuery(element).attr("data-businessunit");
        var label = jQuery(element).attr("data-label");
        if (label == "" || label == "undefined" || label == undefined) {
            label = businessunit;
        }
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            businessunit: businessunit,
            label: label
        });
    }
}
function gtmdutyFreeCategoriesClickEvent(element) {
    if (!(window)['google_tag_manager']) { console.error('Hey! GTM not found'); return } else {
        var event = jQuery(element).attr("data-event");
        var category = "home_page"; //jQuery(this).attr("data-category");
        var subcategory = jQuery(element).attr("data-subcategory");
        var label = jQuery(element).attr("data-label");
        window.dataLayer.push({
            event: event,
            category: category,
            sub_category: subcategory,
            label: label
        });
    }
}