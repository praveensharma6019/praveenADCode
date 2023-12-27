
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

function sliderAdd() {




    if (window.innerWidth >= 991) {



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
                        slidesToShow: 4,
                        slidesToScroll: 4,
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
        }),
            jQuery(".airlines-slider").slick({
                slidesToShow: 3,
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
                            slidesToShow: 4,
                            slidesToScroll: 4,
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
            })




        // document.addEventListener("scroll", function () {
        //   if ((document.body.scrollTop || document.documentElement.scrollTop) > document.getElementsByClassName("top_nav")[0].offsetHeight) {
        //     document.querySelector('.main-header').classList.remove('floating-header');
        //   } else {
        //     document.querySelector('.main-header').classList.add('floating-header');
        //   }
        // });

    } else {
        mobileSlider(".mumbai-airports", 'mobSlideItem');
        mobileSlider(".slider-airports", 'mobSlideItem');
        mobileSlider(".slider-mobile", 'mobSlideItem');
        mobileSlider(".airlines-slider", 'mobSlideItem');
    }
}

sliderAdd();
window.onload = function () {
   // logoChange();
}
function disableVideoControls() {
    var video = document.getElementsByClassName("videoPlayer");
    video.removeAttribute("controls");
}

setTimeout(function () {
    jQuery('.airport-features, .mobile-slider, .airlines-slider, .slider-airports').removeClass('blank-holder');
    jQuery('.blank').removeClass('blank');
    console.log('sh')
}, 2000);

var vids = $("video");
$.each(vids, function () {
    this.controls = false;
});


$(document).ready(function () {
    if ($(".slide").length < 11) {
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


if ($(".slide").length < 5) {
    $("#loadMore").fadeOut(0)
}

const answer = document.getElementById("copyResult");
const copy = document.getElementById("copyButton");
const selection = window.getSelection();
const range = document.createRange();
const textToCopy = document.getElementById("textToCopy")

copy.addEventListener('click', function (e) {
    range.selectNodeContents(textToCopy);
    selection.removeAllRanges();
    selection.addRange(range);
    const successful = document.execCommand('copy');
    if (successful) {
        toastMsg('Copied!', 'center', true);
        //answer.innerHTML = 'Copied!';
    } else {
        toastMsg('Unable to copy!', 'center', false);
        //answer.innerHTML = 'Unable to copy!';  
    }
    window.getSelection().removeAllRanges()
});