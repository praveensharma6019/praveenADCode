const Carousel = {
  settings: {
    dots: false,
    nav: true,
    margin: 16,
    navText: ["<i class='i-arrow-l'></i>", "<i class='i-arrow-r'></i>", ,],
    responsive: {
      0: {
        items: 1.5,
        margin: 16,
      },
      600: {
        items: 3,
        margin: 30,
      },
    },
  },
  init: function () {
    let carouselScope = this;

    $(".owl-carousel").each(function (index, item) {
      let settings = { ...carouselScope.settings };
      let rootIdClassName = "carousel-" + index;
      let parentDiv = $(item).parent();

      if ($(item).data("items-desktop")) {
        settings.responsive = {
          ...settings.responsive,
          ...{ 600: { items: $(item).data("items-desktop") } },
        };
      }

      if ($(item).data("items-mobile")) {
        settings.responsive = {
          ...settings.responsive,
          ...{ 0: { items: $(item).data("items-mobile") } },
        };
      }

      if ($(item).data("nav")) {
        settings = {
          ...settings,
          ...{ nav: $(item).data("nav") === "disable" ? false : settings.nav },
        };
      }

      if ($(item).data("dots")) {
        settings = {
          ...settings,
          ...{ dots: $(item).data("dots") === "enable" ? true : settings.dots },
        };
      }

      if ($(item).data("margin") || $(item).data("margin") > -1) {
        settings = {
          ...settings,
          ...{ margin: $(item).data("margin") },
        };
      }

      if ($(parentDiv).hasClass("carousel_wrapper")) {
        $(parentDiv).addClass(rootIdClassName);

        settings = {
          ...settings,
          ...{ navContainer: `.${rootIdClassName} .carousel-nav` },
        };
      }

      $(item).owlCarousel(settings);
    });
	// Destroy owl in mobile | START

function removeCarousel() {

var owlSlider = $('.owl-carousel.removeSliderMob');

owlSlider.trigger('destroy.owl.carousel');

owlSlider.addClass('off');

}


if (window.innerWidth <= 991) {

removeCarousel();

}

// Destroy owl in mobile | END
	
  },
};

export { Carousel };
