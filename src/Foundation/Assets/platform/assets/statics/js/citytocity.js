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

/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem') {
    jQuery(objID).addClass('scrollbar-x mobSlider');
    jQuery(objID).children().addClass(cardClass);
}

function sliderAdd() {

    if (window.innerWidth >= 991) {

        jQuery(".slider-citytocity").slick({
            slidesToShow: 3,
            slidesToScroll: 3,
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
        mobileSlider(".slider-citytocity", 'mobSlideItem');
    }
}

sliderAdd();

var btn = $('#scrollToTop');

$(window).scroll(function () {
    if ($(window).scrollTop() > 2000) {
        btn.addClass('show');
    } else {
        btn.removeClass('show');
    }

});
 
  
  $("#loadMore").on("click", function (e) {
    e.preventDefault();
    $(".slide:hidden").slice(0, getDeviceType() === 'desktop' ? 2 : 2).slideDown();
    if ($(".slide:hidden").length == 0) {
      $("#loadMore").text("No Content").addClass("noContent");
    }
  });
