/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem') {
    jQuery(objID).addClass('scrollbar-x mobSlider');
    jQuery(objID).children().addClass(cardClass);
}

function sliderAdd() {
    if (window.innerWidth >= 991) {
        /* only desktop */
        jQuery(".stay-slider").slick({
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

    } else {
        mobileSlider(".stay-slider", 'mobSlideItem');
    }
}
sliderAdd();


$(function () {
    $('#myTab a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
    })
});

/* $(document).ready(function(){
  $('.exp-links ul li a').click(function(){
    $(this).parent().siblings().find('.nav-link').removeClass('active');
    $('.tab-pane').css('display','none');
    var tabLink = $(this).attr('id');
    $('#b'+tabLink).css('display','block');
    $(this).addClass('active');
  });
}); */

// Filter
$(document).ready(function () {
    $('.exp-links ul li a').click(function () {
        $(this).parent().siblings().find('.nav-link').removeClass('active');
        $(this).addClass('active');

        $('.blog-tiles > .slide').css('display', 'none');
        var tabLink = $(this).attr('data-id');

        getSlides(tabLink);

    });

    const firstActive = document.querySelector('.exp-links ul li a');
    firstActive.click();
});

const getSlides = (tabLink = 'All') => {
    const slides = document.querySelectorAll('.blog-tiles > .slide');

    const newSlides = [];
    slides.forEach((slide) => {
        if ((tabLink === 'All' || slide.getAttribute('data-item') === tabLink) && slide.style.display === 'none') {
            newSlides.push(slide);
        }
    })

    let index = 0;
    for (let i = 0; i < newSlides.length; i++) {
        if (index >= 6) {
            break;
        }
        newSlides[i].style.display = 'block';
        index++;
    }
    if (index >= newSlides.length) {
        $("#loadMore").text("No Content").addClass("noContent");
    } else {
        $("#loadMore").text("View more").removeClass("noContent");
    }
}

$(document).ready(function () {




    // $(".blog-tiles > .slide").slice(0, 2).show();
    $("#loadMore").on("click", function (e) {
        e.preventDefault();
        const tabLink = document.querySelector('.exp-links ul li a.active').getAttribute('data-id');
        const slides = document.querySelectorAll(`.blog-tiles > .slide`);
        const newSlides = [];
        slides.forEach((slide) => {
            if ((tabLink === 'All' || slide.getAttribute('data-item') === tabLink) && slide.style.display === 'none') {
                newSlides.push(slide);
            }
        })
        let index = 0;
        for (let i = 0; i < newSlides.length; i++) {
            if (index >= 6) { break; }

            index++;
            newSlides[i].style.display = 'block';
        }

        if (index >= newSlides.length) {
            $("#loadMore").text("No Content").addClass("noContent");
        } else {
            $("#loadMore").text("View more").removeClass("noContent");
        }

    });
});