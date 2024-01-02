const allowOnlyNumberInput = (ele) => {
  ele.value = ele.value.replace(/[^0-9]/g, "");
};

var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;
var name_regex = "^[a-zA-Z]+$";

function validateEmail(mailid) {
  var filter =
    /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
  //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
  if (filter.test(mailid)) {
    return true;
  } else {
    return false;
  }
}

$(window).scroll(function () {
  //var banner_heigt = $('.arch-banner-section').outerHeight();
  if ($(this).scrollTop() > 40) {
    $("header").addClass("sticky-header");
  } else {
    $("header").removeClass("sticky-header");
  }
});

var sections = $(".section"),
  nav = $("nav"),
  nav_height = nav.outerHeight();

$(window).on("scroll", function () {
  var cur_pos = $(this).scrollTop();

  sections.each(function () {
    var top = $(this).offset().top - nav_height,
      bottom = top + $(this).outerHeight();

    if (cur_pos >= top && cur_pos <= bottom) {
      nav.find("li").removeClass("active");
      sections.removeClass("active");

      $(this).addClass("active");
      nav
        .find('a[href="#' + $(this).attr("id") + '"]')
        .parent()
        .addClass("active");
    }
  });
});

$("nav ul li a").click(function () {
  if (
    location.pathname.replace(/^\//, "") == this.pathname.replace(/^\//, "") &&
    location.hostname == this.hostname
  ) {
    var target = $(this.hash);
    target = target.length ? target : $("[name=" + this.hash.slice(1) + "]");
    if (target.length) {
      $("html,body").animate(
        {
          scrollTop: target.offset().top - 40,
        },
        1000
      );
      return false;
    }
  }
});

$(".close-form").click(function () {
  $(".sticky-form").addClass("hide");
});

$(".mobile-trigger").click(function () {
  $(this).toggleClass("active");
  $("nav").toggleClass("active");
  $("body").toggleClass("menu-active");
  $(".overlay").toggleClass("active");
});

$(".header-wrapper nav ul li a, .overlay").click(function () {
  $(".mobile-trigger").removeClass("active");
  $("nav").removeClass("active");
  $("body").removeClass("menu-active");
  $(".overlay").removeClass("active");
});

$(document).ready(function () {
  $("#testimonialSec .gallery").slick({
    infinite: false,
    speed: 300,
    slidesToShow: 1,
    slidesToScroll: 1,
    dots: true,
    arrows: true,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          infinite: false,
        },
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
        },
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          arrows: false,
        },
      },
    ],
  });

  $("#testimonialSec .slick-slider").on("beforeChange", function () {
    const activeVideo = $("#testimonialSec .slick-slider .slick-active")
      .find("video")
      ?.get(0);

    setTimeout(() => activeVideo?.pause(), 200);
  });

  $("#masterLayout .gallery").slick({
    infinite: false,
    speed: 300,
    slidesToShow: 1.1,
    slidesToScroll: 1,
    dots: true,
    arrows: true,
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 1.2,
          slidesToScroll: 1,
          infinite: false,
        },
      },
      {
        breakpoint: 800,
        settings: {
          slidesToShow: 1.2,
          slidesToScroll: 1,
        },
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1.2,
          slidesToScroll: 1,
          arrows: false,
          infinite: false,
          dots: false,
        },
      },
    ],
  });

  $(".galleryCarousel .slick-arrow").css(
    "background-image",
    "url('../images/Realty/inspire-business-park/images/arrow.png')"
  );

  $(".galleryCarousel .playpause").css(
    "background-image",
    "url('../images/Realty/inspire-business-park/images/play-btn.png')"
  );

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

  $(".enq-btn").click(function (e) {
    e.stopPropagation();
    const heading = $(e.currentTarget).attr("data-name");

    $("body").toggleClass("form-open");
    $("#form-heading").html(heading);
  });

  $(".popup-form").click(function (e) {
    e.stopPropagation();
  });

  $(".close_btn").click(function (e) {
    $("body").removeClass("form-open");
  });

  const formInputs = $("#EQ_Form .form-item input");

  const MobilenumberEq = document.getElementById("phone_number_eq");
  const EmailEq = document.getElementById("email_eq");
  const nameEq = document.getElementById("full_name_eq");

  nameEq.addEventListener("change", (event) => {
    event.preventDefault();
    if (
      !$("#full_name_eq").val() ||
      name_regex.test($("#full_name_eq").val()) == false
    ) {
      $("#full_name_eq").next().html("Please enter valid name");
      $("#full_name_eq").next().css("color", "#DE0000");
      $("#full_name_eq").css("border-color", "#DE0000");
    } else {
      $("#full_name_eq").next().html("Name");
      $("#full_name_eq").next().css("color", "#404040");
      $("#full_name_eq").css("border-color", "#676767");
    }
  });

  MobilenumberEq.addEventListener("change", (event) => {
    event.preventDefault();
    if (
      $("#phone_number_eq").val().length !== 10 ||
      regex_special_num.test($("#phone_number_eq").val()) == false
    ) {
      $("#phone_number_eq").next().html("Please enter valid mobile number");
      $("#phone_number_eq").next().css("color", "#DE0000");
      $("#phone_number_eq").css("border-color", "#DE0000");
    } else {
      $("#phone_number_eq").next().html("Mobile Number");
      $("#phone_number_eq").next().css("color", "#404040");
      $("#phone_number_eq").css("border-color", "#676767");
    }
  });

  EmailEq.addEventListener("change", (event) => {
    event.preventDefault();
    if (validateEmail($("#email_eq").val()) == false) {
      $("#email_eq").prev().html("Please enter valid email id");
      $("#email_eq").prev().css("color", "#DE0000");
      $("#email_eq").css("border-color", "#DE0000");
    } else {
      $("#email_eq").prev().html("Email");
      $("#email_eq").prev().css("color", "#404040");
      $("#email_eq").css("border-color", "#676767");
    }
  });
});

$(".otp_close_btn").click(function () {
    $(".verification-code--inputs input").removeClass("verification-input");
    $(".invalid_otp_err").html("");
    $("body").removeClass("otp-form-open");
});

//$(".otpButton").click(function () {
//  if (
//    $("#otp_input1").val() == "" ||
//    $("#otp_input2").val() == "" ||
//    $("#otp_input3").val() == "" ||
//    $("#otp_input4").val() == "" ||
//    $("#otp_input5").val() == ""
//  ) {
//    $(".verification-code--inputs input").addClass("verification-input");
//    $(".invalid_otp_err").html("Invalid OTP. Please try again.");
//  } else {
//    $(".verification-code--inputs input").removeClass("verification-input");
//    $(".invalid_otp_err").html("");
//    $("body").toggleClass("thnku-form-open");
//    $("body").removeClass("otp-form-open");

//    $(".thankyou-form").click(function (e) {
//      e.stopPropagation();
//    });
//    $(".thankyou_close_btn").click(function () {
//      $("body").removeClass("thnku-form-open");
//    });

//    $(".thankyou_done").click(function (e) {
//      e.stopPropagation();
//    });

//    $(".thankyou_done").click(function () {
//      $("body").removeClass("thnku-form-open");
//      document.getElementById("footer_form").reset();
//      $("#full_name_fot").next().html("");
//      $("#email_fot").next().html("");
//      $("#phone_number_fot").next().html("");
//      $("#agreeCheckboxError").hide();
//    });
//  }
//});

var verificationCode = [];
$(".verification-code input[type=text]").keyup(function (e) {
  $(".verification-code input[type=text]").each(function (i) {
    verificationCode[i] = $(".verification-code input[type=text]")[i].value;
    $("#verificationCode").val(Number(verificationCode.join("")));
  });

  if ($(this).val() >= 0) {
    if (
      event.key == 1 ||
      event.key == 2 ||
      event.key == 3 ||
      event.key == 4 ||
      event.key == 5 ||
      event.key == 6 ||
      event.key == 7 ||
      event.key == 8 ||
      event.key == 9 ||
      event.key == 0
    ) {
      $(this).next().focus();
    }
  } else {
    if (event.key == "Backspace") {
      $(this).prev().focus();
    }
  }
});
$(".enq-btn, .download-brochure").click(function () {
  var element = document.getElementById("EQ_Form");
  element.reset();
  $("#full_name_eq").next().html("Name");
  $("#full_name_eq").next().css("color", "#404040");
  $("#full_name_eq").css("border-color", "#404040");
  $("#email_eq").next().html("Email");
  $("#email_eq").next().css("color", "#404040");
  $("#email_eq").css("border-color", "#404040");
  $("#phone_number_eq").next().html("Mobile number");
  $("#phone_number_eq").next().css("color", "#404040");
  $("#phone_number_eq").css("border-color", "#404040");
  $(".checklabel .error").html("");
});

$(".address-wrapper h3").click(function () {
  $(this).parent().toggleClass("active");
});

$(".otp_close_btn").click(function () {
  document.getElementById("OTP_form").reset();
});

const otpValidate = function (ele) {
    ele.value = ele.value.replace(/[^0-9]/g, "");
};
