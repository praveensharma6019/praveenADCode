$(document).ready(function () {
  $(".lifestyle-gallery-section .gallery").slick({
    speed: 500,
    fade: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    dots: false,
    infinite: true,
    prevArrow:
      '<span class="i-arrow-l slick-prev"><img src="images/Realty/ZeroEmiImages/left-arrow.png" alt=""></span>',
    nextArrow:
      '<span class="i-arrow-r slick-next"><img src="images/Realty/ZeroEmiImages/right-arrow.png" alt=""></span>',
  });
  $(".d-brochure-btn").click(function (e) {
      e.stopPropagation();
      const errorElements = document.querySelectorAll(".errorMsg");
      for (let i = 0; i < errorElements.length; i++) {
          errorElements[i].innerHTML = "";
      }
    $("body").toggleClass("brochure-form-open");
  });

  $(".enq-btn").click(function (e) {
      e.stopPropagation();
      const errorElements = document.querySelectorAll(".errorMsg");
      for (let i = 0; i < errorElements.length; i++) {
          errorElements[i].innerHTML = "";
      }
    $("body").toggleClass("enquire-form-open");
  });

  $(".popup-form").click(function (e) {
    e.stopPropagation();
  });

  $(document).click(function (e) {
    $("body").removeClass("form-open");
  });
  $(".ovarLay").click(function (e) {
    $("body").removeClass("brochure-form-open");
    $("body").removeClass("enquire-form-open");
  });

  $(".download-brochure .close_btn").click(function (e) {
    $("body").removeClass("brochure-form-open");
  });

  $(".enquire-now .close_btn").click(function (e) {
    $("body").removeClass("enquire-form-open");
  });

  $(".close_btn").click(function (e) {
    $("body").removeClass("form-open");
  });

  $(".mobile-trigger").click(function () {
    $("body").toggleClass("menu-open");
    $(this).next(".custom-menu-section").slideToggle(250);
  });
  var helpers = {
    addZeros: function (n) {
      // return (n < 10) ? '0' + n : '' + n;
      return n < 10 ? 0 + n : n;
    },
  };
  function sliderInit() {
    var $slider = $(".one-time");
    $slider.each(function () {
      var $sliderParent = $(this).parent();
      $(this).slick({
        dots: false,
        infinite: true,
        speed: 500,
        slidesToShow: 1,
        adaptiveHeight: true,
        prevArrow:
          '<span class="i-arrow-l slick-prev"><img src="images/Realty/ZeroEmiImages/left-arrow.png" alt=""></span>',
        nextArrow:
          '<span class="i-arrow-r slick-next"><img src="images/Realty/ZeroEmiImages/right-arrow.png" alt=""></span>',
      });

      if ($(this).find(".item").length > 1) {
        $(this).siblings(".slides-numbers").show();
      }

      $(this).on("afterChange", function (event, slick, currentSlide) {
        $sliderParent
          .find(".slides-numbers .active")
          .html(helpers.addZeros(currentSlide + 1));
      });

      var sliderItemsNum = $(this)
        .find(".slick-slide")
        .not(".slick-cloned").length;
      $sliderParent
        .find(".slides-numbers .total")
        .html(helpers.addZeros(sliderItemsNum));

      if (sliderItemsNum < 2) {
        $(this).closest(".floor-plan").find(".slides-numbers").hide();
      }
    });
  }

  sliderInit();
  $(window).scroll(function () {
    if ($(this).scrollTop() > 20) {
      $("body").addClass("scroll-body");
    } else {
      $("body").removeClass("scroll-body");
    }
  });
  // $('.enq-btn').click(function(){
  // $('.popup-form h3').text('Enquire Now');
  // $('.enquiryBtn').text('Send Enquiry');
  // });
  // $('.download-brochure-section .brochure-btn').click(function(){
  // $('.popup-form h3').text('Download Brochure');
  // $('.enquiryBtn').text('Download Now');
  // });

  $(".gallery").each(function () {
    $(this)
      .find(".popup-img")
      .magnificPopup({
        type: "image",
        mainClass: "mfp-with-zoom",
        gallery: {
          enabled: true,
        },

        zoom: {
          enabled: true,

          duration: 300, // duration of the effect, in milliseconds
          easing: "ease-in-out", // CSS transition easing function

          opener: function (openerElement) {
            return openerElement.is("img")
              ? openerElement
              : openerElement.find("img");
          },
        },
      });
  });
  $(".tab-list ul >li").click(function () {
    $(this).siblings().removeClass("active");
    $(".custom-tab-desc").css("display", "none");
    var tabLink = $(this).attr("id");
    $("#b" + tabLink).css("display", "block");
    $(this).addClass("active");
    $(".cm-refresh").slick("refresh");
    //$('.cm-refresh')[0].slick.refresh();
  });

  //scrollSpy function
  var header_height = $("header").outerHeight();
  var stickyTab_height = $(".archway-tabber-section .tab-list").outerHeight();

  $(".archway-tabber-section .tab-list").css("top", header_height);
  $(window).on("resize", function () {
    var header_height2 = $("header").outerHeight();
    $(".archway-tabber-section .tab-list").css("top", header_height2);
  });

  function scrollSpy() {
    var sections = ["overview", "offers", "project", "about"];
    var current;
    for (var i = 0; i < sections.length; i++) {
      var top =
        ($("#" + sections[i]).offset() || { top: NaN }).top -
        (header_height + 20);
      if (top <= $(window).scrollTop()) {
        current = sections[i];
      }
    }
    $(".custom-menu-section ul li a[href='#" + current + "']")
      .parent()
      .addClass("active");
    $(".custom-menu-section ul li a")
      .not("a[href='#" + current + "']")
      .parent()
      .removeClass("active");
  }
  // smooth scrolling navigation
  $(".custom-menu-section ul li a").click(function () {
    $(".custom-menu-section").slideUp(250);
    $("body").removeClass("menu-open");
    if (
      location.pathname.replace(/^\//, "") ==
        this.pathname.replace(/^\//, "") &&
      location.hostname == this.hostname
    ) {
      var target = $(this.hash);
      target = target.length ? target : $("[name=" + this.hash.slice(1) + "]");
      if (target.length) {
        $("html,body").animate(
          {
            scrollTop: target.offset().top - header_height,
          },
          1000
        );
        return false;
      }
    }
  });
  //scrollSpy call
  /* $(document).ready( function() {
        scrollSpy();
      }); */
  scrollSpy();
  $(window).scroll(function () {
    scrollSpy();
  });
});

$(document).ready(function () {
  setTimeout(() => {
    $("body").removeClass("overflowHidden");
  }, 3000);
  setTimeout(() => {
    $(".wrapperSplash").removeClass("active");
  }, 3000);

  setTimeout(() => {
    $(".wrapperSplash .wrapperSplashHeading").addClass("active");
  }, 300);
  setTimeout(() => {
    $(".wrapperSplash .splashItem").addClass("active");
  }, 700);
  setTimeout(() => {
    $(".wrapperSplash .splashLogoWrapper").addClass("active");
  }, 1200);

  $(".sliderWrapper").slick({
    slidesToShow: 1,
    dots: true,
    centerMode: false,
    arrows: false,
    speed: 500,
    autoplay: false,
    autoplaySpeed: 2000,
  });
});
const allowOnlyNumberInput = (ele) => {
    ele.value = ele.value.replace(/[^0-9]/g, "");
};

$(".brochure-btn").click(function () {

    var element = document.getElementById("broc_form");

    element.reset();

});

$(".enq-btn").click(function () {

    var element = document.getElementById("enq_form");

    element.reset();

});