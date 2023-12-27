
function mobileSlider(objID, cardClass = 'mobSlideItem') {
  jQuery(objID).addClass('scrollbar-x mobSlider');
  jQuery(objID).children().addClass(cardClass);
}
$(document).ready(function () {
  $('.cm-tab-nav .nav-link').click(function () {
    $(this).parent().siblings().find('.nav-link').removeClass('active');
    $('.tab-pane').css('display', 'none');
    var tabLink = $(this).attr('id');
    $('#b' + tabLink).css('display', 'block');
    $(this).addClass('active');
  });
});

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
// document.querySelector('.menu-city').addEventListener()

document.addEventListener('click', (event) => {
  const menu = document.querySelector('.menu-city')
  const heading = document.querySelector('.heading-dropdown')
  if (!menu.contains(event.target) && !heading.contains(event.target))
    menu.style.display = 'none'
  else {
    menu.style.display = 'block'
  }
})

function sliderAdd() {

  if (window.innerWidth >= 991) {

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
    jQuery(".kids-slider").slick({
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
    $('.travel-nav-slider').slick({
      dots: false,
      infinite: false,
      speed: 300,
      slidesToShow: 8,
      slidesToScroll: 1,
      arrows: true,
      // variableWidth: true,
      prevArrow:
        '<span class="i-arrow-r slick-prev"><svg width="7" height="13" viewBox="0 0 7 13" xmlns="http://www.w3.org/2000/svg"><path d="M5.084 6.656.18 1.414a.667.667 0 0 1 .973-.911l5.334 5.7c.24.257.24.658-.003.914L1.151 12.75a.667.667 0 0 1-.968-.917l4.901-5.177z" fill="#333" fill-rule="nonzero"/></svg></span>',
      nextArrow:
        '<span class="i-arrow-r slick-next"><svg width="7" height="13" viewBox="0 0 7 13" xmlns="http://www.w3.org/2000/svg"><path d="M5.084 6.656.18 1.414a.667.667 0 0 1 .973-.911l5.334 5.7c.24.257.24.658-.003.914L1.151 12.75a.667.667 0 0 1-.968-.917l4.901-5.177z" fill="#333" fill-rule="nonzero"/></svg></span>',
    });
  } else {
    mobileSlider(".slider-citytocity,.stay-slider,.travel-nav-slider", 'mobSlideItem');
  }
}

sliderAdd();



$('.travel-nav-slider').on('click', '.slick-active:last + .slick-slide', function () {
  $('.slick-next').click();
})

$('.travel-nav-slider').on('click', '.slick-active:first', function () {
  $('.slick-prev').click();
})


$(document).ready(function () {
  $(".heading-dropdown").click(function () {
    $(".menu-city ").toggle();
  });
});

if ($(".slide").length < 5) {
  $("#loadMore").fadeOut(0)
}


$("#loadMore").on("click", function (e) {
  e.preventDefault();
  $(".slide:hidden").slice(0, getDeviceType() === 'desktop' ? 2 : 2).slideDown();
  if ($(".slide:hidden").length == 0) {
    $("#loadMore").text("No Content").addClass("noContent");
  }

});
