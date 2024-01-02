$(window).scroll(function () {
  //var banner_heigt = $('.arch-banner-section').outerHeight();
  if ($(this).scrollTop() > 40) {
    $("body").addClass("scroll-body");
  } else {
    $("body").removeClass("scroll-body");
  }
});


const allowOnlyNumberInput = function (ele) {
  ele.value = ele.value.replace(/[^0-9]/g, "");
};

const tabChange = function (val,parentElement, inputsIdentifier, noOfFields) {
  var parentElement = document.querySelector(parentElement);
  const inputEle = parentElement.querySelectorAll(inputsIdentifier);
  inputEle[val].onkeydown = () => {
    const key = event.keyCode || event.charCode;
    if (key === 8 || key === 46) {
      if (val > 0 && inputEle[val].value === "") {
        inputEle[val - 1].focus();
      }
    } else if (val < noOfFields - 1 && inputEle[val].value !== "") {
      inputEle[val + 1].focus();
    }
  };
};

$(document).ready(function () {
    setTimeout(() => {
        $('.arch-banner-carousel').slick({
            dots: true, // Enable dots navigation
            arrows: false, // Hide arrows
            autoplay: true, // Enable autoplay
            autoplaySpeed: 5000, // Set autoplay speed (in milliseconds)
            slidesToShow: 1,
            pauseOnHover: false,
            // Add additional options as needed
        });
    }, 3000)

  setTimeout(function () {
    $(".highlight-slider-wrapper").slick({
      centerMode: true,
      centerPadding: "245px",
      dots: true,
      arrows: true,
      slidesToShow: 1,
      autoplay: true,
      autoplaySpeed: 5000,
      responsive: [
        {
          breakpoint: 991,
          settings: {
            centerPadding: "100px",
          },
        },
        {
          breakpoint: 700,
          settings: {
            centerPadding: "50px",
          },
        },
      ],
    });

    var helpers = {
      addZeros: function (n) {
        // return (n < 10) ? '0' + n : '' + n;
        return n < 10 ? 0 + n : n;
      },
    };

    // debounce from underscore.js
    function debounce(func, wait, immediate) {
      var timeout;
      return function () {
        var context = this,
          args = arguments;
        var later = function () {
          timeout = null;
          if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
      };
    }

    // use x and y mousewheel event data to navigate flickity
    function slick_handle_wheel_event(e, slick_instance, slick_is_animating) {
      // do not trigger a slide change if another is being animated
      if (!slick_is_animating) {
        // pick the larger of the two delta magnitudes (x or y) to determine nav direction
        var direction =
          Math.abs(e.deltaX) > Math.abs(e.deltaY) ? e.deltaX : e.deltaY;

        console.log("wheel scroll ", e.deltaX, e.deltaY, direction);

        if (direction > 0) {
          // next slide
          slick_instance.slick("slickNext");
        } else {
          // prev slide
          slick_instance.slick("slickPrev");
        }
      }
    }

    // debounce the wheel event handling since trackpads can have a lot of inertia
    var slick_handle_wheel_event_debounced = debounce(
      slick_handle_wheel_event,
      100,
      true
    );

    // init slider
    const slick_2 = $(".slides");
    slick_2.slick({
      dots: true,
      vertical: false,
      infinite: false,
      // verticalSwiping: true,
      arrows: false,
    });
    var slick_2_is_animating = false;

    slick_2.on("afterChange", function (index) {
      console.log("Slide after change " + index);
      slick_2_is_animating = false;
    });

    slick_2.on("beforeChange", function (index) {
      console.log("Slide before change " + index);
      slick_2_is_animating = true;
    });

    slick_2.on("wheel", function (e) {
      slick_handle_wheel_event_debounced(
        e.originalEvent,
        slick_2,
        slick_2_is_animating
      );
    });
    function sliderInit() {
      var $slider = $(".lifestyle-gallery-section .gallery");
      $slider.each(function () {
        var $sliderParent = $(this).parent();
        $(this).slick({
          speed: 500,
          fade: true,
          slidesToShow: 1,
          slidesToScroll: 1,
          dots: false,
          infinite: true,
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
      });
    }

    sliderInit();
  }, 3100);

  $(".floor-plan ul.tab-list>li").click(function () {
    $(this).siblings().removeClass("act");
    $(".floor-detail").css("display", "none");
    var tabLink = $(this).attr("id");
    $("#b" + tabLink).css("display", "flex");
    $(this).addClass("act");
  });

  // $('.checkBtn,.sticky-btn a,.enq-btn,.brochure-btn ').click(function(e){
  // e.stopPropagation();
  // $('body').toggleClass('form-open');
  // });

  $(".enq-btn").click(function (e) {
    e.stopPropagation();
    $("body").toggleClass("enquire-form-open");
  });

  $(".brochure-btn").click(function (e) {
    e.stopPropagation();
    $("body").toggleClass("brochure-form-open");
    $("body").removeClass("menu-open");
  });

  $(".checkBtn").click(function (e) {
    e.stopPropagation();
    $("body").toggleClass("configuration-form-open");
  });

  $(".popup-form").click(function (e) {
    e.stopPropagation();
  });

  $(document).click(function (e) {
    $("body").removeClass("form-open");
  });

  // $(".ovarLay").click(function (e) {
  //   $("body").removeClass("enquire-form-open");
  //   $("body").removeClass("brochure-form-open");
  //   $("body").removeClass("configuration-form-open");
  // });

  $(".enquire-now .close_btn").click(function (e) {
    $("body").removeClass("enquire-form-open");
  });

  $(".download-brochure .close_btn").click(function (e) {
    $("body").removeClass("brochure-form-open");
  });

  $(".price-configuration .close_btn").click(function (e) {
    $("body").removeClass("configuration-form-open");
  });

  $(".close_btn").click(function (e) {
    document.getElementById("OTP_form").reset();
      $("body").removeClass("form-open");
      $("body").removeClass("otpModal");
    $(".popup-form .modal").removeClass("showOTP");
    $('.enquire-now h3').text('Enquire Now');
  });

  $(".otp-btn,.otp_close_btn").click(function () {
    $("body").toggleClass("otp-form-open");
  });
  $(".thanku-btn,.thankyou_close_btn").click(function () {
    $("body").toggleClass("thnku-form-open");
  });

  $(".mobile-trigger").click(function (e2) {
    e2.stopPropagation();
    $("body").toggleClass("menu-open");
  });
  $(".custom-menu-section").click(function (e2) {
    e.stopPropagation();
  });

  $(document).click(function (e2) {
    $("body").removeClass("menu-open");
  });

  $(".verification-code input").on("paste", function (event, pastedValue) {
    console.log(event);
    $("#txt").val($content);
    console.log($content);
    //console.log(values)
  });

  $(".video")
    .parent()
    .click(function () {
      if ($(this).children(".video").get(0).paused) {
        $(this).children(".video").get(0).play();
        $(this).children(".playpause").fadeOut();
      } else {
        $(this).children(".video").get(0).pause();
        $(this).children(".playpause").fadeIn();
      }
    });

  function openCity(evt, cityName) {
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
      tabcontent[i].style.display = "none";
    }
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
      tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(cityName).style.display = "block";
    evt.currentTarget.className += " active";
  }
});
$(function () {
  $(".custom-menu-section ul li a").click(function () {
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
            scrollTop: target.offset().top - 78,
          },
          1000
        );
        return false;
      }
    }
  });

  setTimeout(function () {
    $(".animation-section img.img2").addClass("remove");
    $(".animation-section img.img1").addClass("add");
  }, 1200);

  setTimeout(function () {
    $(".arch-site-wrapper.main").fadeIn(500);
    $(".animation-section").fadeOut(200);
  }, 3000);

  $(".popup-img").magnificPopup({
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
  $(".popup-img2").magnificPopup({
    type: "image",
    closeOnBgClick: false,
  });
});

// if ($('.footer-top-desc h5 a').length > 0) {
// var link = $('.footer-top-desc h5 a').attr('href');
// var origin = window.location.origin+'/';
// var finalLink = origin+link;
// $('.footer-top-desc h5 a').attr('href', finalLink);
// }
