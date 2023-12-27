
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

    jQuery(".muti-items").slick({
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

    jQuery(".items-2-slider").slick({
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
            slidesToScroll: 2,
            prevArrow: false,
            nextArrow: false,
          },
        },
        {
          breakpoint: 480,
          nav: false,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 2,
            prevArrow: false,
            nextArrow: false,
          },
        },
      ],
    });

  } else {
    mobileSlider(".muti-items,.items-2-slider,.travel-nav-slider", 'mobSlideItem');
  }
}

sliderAdd();

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

window.onscroll = function () {
  try {
    let horizontaltabHeader = document.getElementById("navbar-tabs");

    if (window.pageYOffset < horizontaltabHeader.offsetTop && document.querySelector('#navbar-tabs .tab-nav.active') == null) {
      document.getElementsByClassName("tab-nav")[0].classList.add('active');
    }
  } catch (error) {
  }
};


function closeOffcanvas() {
  const { innerWidth: width, innerHeight: height } = window;
  if (width < 992) {
    try {
      var $myOffcanvas = document.getElementById('offcanvasBottomTest');
      var $myOffBackdrop = document.getElementsByClassName('offcanvas-backdrop')[0];
      $myOffcanvas.classList.remove('show');
      $myOffcanvas.classList.add('hide');
      $myOffBackdrop.classList.remove('show');
      $myOffBackdrop.classList.add('offcanvas-hide');
      document.body.style.removeProperty('overflow');
      document.body.style.removeProperty('padding-right')
    } catch (error) {

    }
  }
}
window.addEventListener("load", handleResize);
window.addEventListener("resize", handleResize);
function handleResize() {
  let $toggle_offcanvas = document.getElementById("offcanvasBottomTest");
  console.log($toggle_offcanvas);
  if (!$toggle_offcanvas) { return; }
  const { innerWidth: width, innerHeight: height } = window;
  if (width < 992) {
    $toggle_offcanvas.classList.remove('offcanvas-desktop');
  }
  else {
    $toggle_offcanvas.classList.add('offcanvas-desktop');
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
    sliderNavigation("other-services");
  }, 400);
};

sliderNavigation("other-services");



// function myFunction(inputObj, listObj) {
//    var input, filter, ul, li, a, i, txtValue;
//    input = document.getElementById(inputObj);
//    filter = input.value.toUpperCase();
//    ul = document.getElementById(listObj);
//    li = ul.getElementsByTagName("li");
//    for (i = 0; i < li.length; i++) {
//        a = li[i];
//        txtValue = a.textContent || a.innerText;
//        if (txtValue.toUpperCase().indexOf(filter) > -1) {
//            li[i].style.display = "";
//        } else {
//            li[i].style.display = "none";
//        }
//    }
// }



// jQuery(".slider-citytocity").slick({
//    slidesToShow: 2,
//    slidesToScroll: 1,
//    dots: false,
//    infinite: false,
//    nav: true,
//    prevArrow: '<i class="i-arrow-l slick-prev"></i>',
//    nextArrow: '<i class="i-arrow-r slick-next"></i>',
//    responsive: [
//      {
//        breakpoint: 1024,
//        settings: {
//          slidesToShow: 2,
//          slidesToScroll: 2,
//        },
//      },
//      {
//        breakpoint: 480,
//        nav: false,
//        settings: {
//          slidesToShow: 1.5,
//          slidesToScroll: 1,
//        },
//      },
//    ],
//  });





// var scrollSpy = new bootstrap.ScrollSpy(document.body, {
//    target: '#navbar-tabs'
//  })



$('.about-btn').click(function () {
  $(this).closest('.bnr-content').toggleClass('active');
  $(this).closest('.club-content').toggleClass('active');
  $(this).closest('.hero-intro').toggleClass('active');
});

if ($(".slide").length < 5) {
  $("#loadMore").fadeOut(0)
}

$(document).ready(function () {
  if ($(".slide").length < 5) {
    $("#loadMore").fadeOut(0)
  }
  $(".slide").slice(0, 4).show();
  $("#loadMore").on("click", function (e) {
    e.preventDefault();
    $(".slide:hidden").slice(0, getDeviceType() === 'desktop' ? 5 : 2).slideDown();
    if ($(".slide:hidden").length == 0) {
      $("#loadMore").text("No Content").addClass("noContent");
    }
  });
});

$(document).ready(function () {
  $('.cm-tab-nav .nav-link').click(function () {
    $(this).parent().siblings().find('.nav-link').removeClass('active');
    $('.tab-pane').css('display', 'none');
    var tabLink = $(this).attr('id');
    $('#b' + tabLink).css('display', 'block');
    $(this).addClass('active');
  });
});