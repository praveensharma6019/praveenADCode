$('#leaderspeak_slider_blogs').owlCarousel({
  items: 2,
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
      dots: true,
    },
    600: {
      items: 1.3,
      dots: true,
    },
    768: {
      items: 2.1,
      dots: true,
    },
    963: {
      items: 2.4,
      dots: true,
    },
    1120: {
      items: 2.5,
      dots: true,
    }
  }
});