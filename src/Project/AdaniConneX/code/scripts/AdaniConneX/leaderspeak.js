$(document).ready(function () {


    $('#leaderspeak_slider_Home').owlCarousel({
      items: 3,
      nav: false,
      dots: true,
      mouseDrag: true,
      loop: true,
      autoplay: true,
	  autoplayHoverPause: true,
      responsiveClass: true,
      responsive: {
        0: {
          items: 1.1,
          dots: false,
        },
        767: {
          items: 1.48,
          dots: false,
        },
        964: {
          items: 3,
        }
      }
    });

  });