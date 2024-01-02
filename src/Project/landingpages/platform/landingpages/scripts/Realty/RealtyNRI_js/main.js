const section = document.querySelectorAll("section");
const tabs = document.querySelectorAll(".tab");
const realtyHeader = document.querySelector(".custom-header");
const dropdown = document.getElementById("enq_project");
const dropdownLocation = document.getElementById("enq_location");
const cityDropdown = document.querySelectorAll("#city_dropdown");
const clickElementDropdown = document.querySelectorAll(".clickelement");
const arrowDownIcon = document.querySelectorAll(".arrow.down");

var projectList = [
  {
    location: "Ahmedabad",
    projects: [
      {
        option: "Archway",
        value: "Archway",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Atrius",
        value: "Atrius",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Paarijat",
        value: "Paarijat",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Ambrosia",
        value: "Ambrosia",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Ikaria",
        value: "Ikaria",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Green View",
        value: "GreenView",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "The Storeys",
        value: "TheStoreys",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "The North Park",
        value: "TheNorthPark",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Amogha",
        value: "Amogha",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Aster",
        value: "Aster",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Water Lily",
        value: "WaterLily",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Inspire Business Park",
        value: "InspireBusinessPark",
        type: "Ahmedabad",
        id: "a4S9D000000Cgci",
      },
    ],
  },
  {
    location: "Gurugram",
    projects: [
      {
        option: "Samsara Vilasa",
        value: "SamsaraVilasa",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Samsara Vilasa 2.0",
        value: "SamsaraVilasa2",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Oyster Grande",
        value: "OysterGrande",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Platinum Tower",
        value: "PlatinumTower",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Aangan",
        value: "Aangan",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Samsara (M Block)",
        value: "SamsaraMBlock",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Miracle Mile",
        value: "MiracleMile",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Oyster Arcade",
        value: "OysterArcade",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Aangan Arcade",
        value: "AanganArcade",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Aangan Galleria",
        value: "AanganGalleria",
        type: "Gurugram",
        id: "a4S9D000000Cgci",
      },
    ],
  },
  {
    location: "Mumbai",
    projects: [
      {
        option: "The Views",
        value: "TheViews",
        type: "Mumbai",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Linkbay Residences",
        value: "LinkbayResidences",
        type: "Mumbai",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Ten BKC",
        value: "TenBKC",
        type: "Mumbai",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Monte South",
        value: "MonteSouth",
        type: "Mumbai",
        id: "a4S9D000000Cgci",
      },
      {
        option: "Inspire BKC",
        value: "InspireBKC",
        type: "Mumbai",
        id: "a4S9D000000Cgci",
      },
    ],
  },
  {
    location: "Pune",
    projects: [
      {
        option: "Atelier Greens",
        value: "AtelierGreens",
        type: "Pune",
        id: "a4S9D000000Cgci",
      },
    ],
  },
];

document.querySelectorAll(".tab")[0].classList.add("active");

function activeLink(li) {
  tabs.forEach((item) => item.classList.remove("active"));
  li.classList.add("active");
}

tabs.forEach((item) =>
  item.addEventListener("click", function () {
    activeLink(this);
  })
);

$(window).scroll(function () {
  if ($(this).scrollTop() > 40) {
    $("body").addClass("scroll-body");
  } else {
    $("body").removeClass("scroll-body");
  }
});

$(function () {
  $(".custom-menu-section ul li a").click(function () {
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
            scrollTop: target.offset().top - 90,
          },
          1000
        );
        return false;
      }
    }
  });
  // setTimeout(function () {
  //   $(".animation-section img.img2").addClass("remove");
  //   $(".animation-section img.img1").addClass("add");
  // }, 1200);

  $(document).ready(function () {
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
      infinite: false,
      dots: true,
      vertical: true,
      verticalSwiping: true,
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

    // $(".lifestyle-gallery-section .gallery").slick({
    //   slidesToShow: 2,
    //   slidesToScroll: 3,
    //   speed: 500,
    //   fade: true,
    //   dots: false,
    //   infinite: true,
    //   arrows: true,
    // });
    $("#life-gallery .gallery").slick({
      dots: false,
      infinite: false,
      speed: 300,
      slidesToShow: 3.25,
      slidesToScroll: 4,
      dots: false,
      responsive: [
        {
          breakpoint: 1024,
          settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
            infinite: true,
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
            slidesToShow: 1.2,
            slidesToScroll: 1,
            arrows: false,
          },
        },
      ],
    });
    $(".projectCaresoul").slick({
      dots: false,
      infinite: false,
      speed: 300,
      slidesToShow: 3,
      slidesToScroll: 1,
      dots: false,
      responsive: [
        {
          breakpoint: 1024,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 1,
            infinite: true,
          },
        },
        {
          breakpoint: 600,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            arrows: false,
          },
        },
        {
          breakpoint: 480,
          settings: {
            slidesToShow: 1.2,
            slidesToScroll: 1,
            arrows: false,
          },
        },
      ],
    });
    $("#testimonialSec .gallery").slick({
      dots: false,
      infinite: false,
      speed: 300,
      slidesToShow: 1,
      slidesToScroll: 1,
      dots: true,
      responsive: [
        {
          breakpoint: 1024,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
            infinite: true,
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
            // arrows: false,
          },
        },
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
      ],
    });

    $("#testimonialSec .slick-slider").on("beforeChange", function () {
      const activeVideo = $("#testimonialSec .slick-slider .slick-active")
        .find("video")
        ?.get(0);

      setTimeout(() => activeVideo?.pause(), 200);

      const currentItem = $("#testimonialSec .slick-slider .slick-active")
        .find("iframe")
        ?.get(0);
      setTimeout(
        () => currentItem?.setAttribute("src", currentItem.getAttribute("src")),
        200
      );
    });

    $(".floor-plan ul.tab-list>li").click(function () {
      $(this).siblings().removeClass("act");
      $(".floor-detail").addClass("hiddenCaresoul");
      var tabLink = $(this).attr("id");
      $("#b" + tabLink).css("display", "flex");
      $("#b" + tabLink).removeClass("hiddenCaresoul");
      $(this).addClass("act");
    });
    $(".checkBtn,.sticky-btn a,.enq-btn,.brochure-btn, .projectLink ").click(
      function (e) {
        e.stopPropagation();
        $("body").toggleClass("form-open");
        const dropdownBtn = document.querySelector(".select .selectBtn");
        if (dropdownBtn) {
          dropdownBtn.setAttribute("data-type", "1");
          dropdownBtn.setAttribute("value", "United Arab Emirates");
          dropdownBtn.setAttribute("data-id", "+971");
          dropdownBtn.children[0].setAttribute(
            "src",
            "/-/media/Project/Realty/CountryFlags/Flags-with-Name/united.png"
          );
          dropdownBtn.children[1].innerText = "+971";
        }
      }
    );
    $(".popup-form").click(function (e) {
      e.stopPropagation();
    });

    function clearForm() {
      const errorElements = document.querySelectorAll(".errorMsg");
      const formInputs = document.querySelectorAll("input");

      for (let i = 0; i < formInputs.length; i++) {
        formInputs[i].value = "";
      }
      for (let i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
      }
      for (let i = 1; i < dropdown.options.length; i++) {
        dropdown.options[i].removeAttribute("selected");
      }
      dropdown.options[0].setAttribute("selected", true);
      dropdown.value = dropdown.options[0].value;
    }

    $(document).click(function (e) {
      e.stopPropagation();
      clearForm();
      $("body").removeClass("form-open");
      $(".selectBtn").removeClass("active");
      $(".selectDropdown.toggle").removeClass("toggle");
    });
    $(".close_btn").click(function (e) {
      clearForm();
      $("body").removeClass("form-open");
      $(".selectBtn").removeClass("active");
      $(".selectDropdown.toggle").removeClass("toggle");
    });

    $(".otp-btn,.otp_close_btn").click(function () {
      $("body").toggleClass("otp-form-open");
    });
    $(".thanku-btn,.thankyou_close_btn").click(function () {
      $("body").toggleClass("thnku-form-open");
    });
    $(".mobile-trigger, .custom-menu-section ul li a").click(function (e2) {
      e2.stopPropagation();
      $("body").toggleClass("menu-open");
    });
    // $(".custom-menu-section").click(function (e2) {
    // e.stopPropagation();
    // });

    $(document).click(function (e2) {
      $("body").removeClass("menu-open");
    });

    //Code Verification
    var verificationCode = [];
    $(".verification-code input[type=text]").keyup(function (e) {
      // Get Input for Hidden Field
      $(".verification-code input[type=text]").each(function (i) {
        verificationCode[i] = $(".verification-code input[type=text]")[i].value;
        $("#verificationCode").val(Number(verificationCode.join("")));
        //console.log( $('#verificationCode').val() );
      });

      //console.log(event.key, event.which);

      if ($(this).val() > 0) {
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
    $(".readmorelinkTxt").click(function () {
      if ($(this).html() === "Read Less") {
        $("span.more").css("display", "none");
        $(this).html("Read More");
      } else {
        $("span.more").css("display", "inline-block");
        $(this).html("Read Less");
      }
    });
  });

  $(".testimonial-img-popup").magnificPopup({
    type: "image",
    mainClass: "mfp-with-zoom",
    gallery: {
      enabled: true,
    },
    zoom: {
      enabled: true,
      duration: 300,
      easing: "ease-in-out",
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

  $(
    ".custom-menu-section ul li.cta-btn a.enq-btn,.sticky-btn a, .btnEnquire.enq-btn, .projectLink"
  ).click(function () {
    $(".popup-form h3").text("Enquire Now");
    let i = "Enquire Now";
    $(".popup-form h3").attr("id", i);
  });
  $(".brochure-btn").click(function () {
    $(".popup-form h3").text("Download Brochure");
    let i = "Download Brochure";
    $(".popup-form h3").attr("id", i);
  });
  $(".checkPrice>a").click(function () {
    $(".popup-form h3").text("Check Price");
  });
});
function myFunction() {
  var dots = document.getElementById("dots");
  var moreText = document.getElementById("more");
  var btnText = document.getElementById("myBtn");

  if (dots.style.display === "none") {
    dots.style.display = "inline";
    btnText.innerHTML = "Read more";
    moreText.style.display = "none";
  } else {
    dots.style.display = "none";
    btnText.innerHTML = "Read less";
    moreText.style.display = "inline";
  }
}

function investmentTxtMore() {
  var dots = document.getElementById("dots");
  var moreText = document.getElementById("more1");
  var btnText = document.getElementById("myBtn1");

  if (dots.style.display === "none") {
    dots.style.display = "inline";
    btnText.innerHTML = "Read more";
    moreText.style.display = "none";
  } else {
    dots.style.display = "none";
    btnText.innerHTML = "Read less";
    moreText.style.display = "inline";
  }
}
function cityTxtMore() {
  var dots = document.getElementById("dots");
  var moreText = document.getElementById("more2");
  var btnText = document.getElementById("myBtn2");

  if (dots.style.display === "none") {
    dots.style.display = "inline";
    btnText.innerHTML = "Read more";
    moreText.style.display = "none";
  } else {
    dots.style.display = "none";
    btnText.innerHTML = "Read less";
    moreText.style.display = "inline";
  }
}

const countryCodeDropdown = document.querySelector("#enq_flag_code");
$.getJSON(
  "images/Realty/RealtyNRI_images/CountryFlag/countryFields.json",
  function (data) {
    data?.country?.forEach((val) => {
      const newEle = document.createElement("div");
      newEle.setAttribute("class", "option");
      newEle.setAttribute("data-type", val?.id);
      newEle.setAttribute("data-src", val?.countryFlagImage);
      newEle.setAttribute("value", val?.countryName);
      newEle.setAttribute("data-id", val?.dialCode);
      newEle.setAttribute("data-length", val?.contactNoLength);

      const flagImage = document.createElement("img");
      flagImage.setAttribute("src", val?.countryFlagImage);
      newEle.innerHTML = val?.dialCode;
      newEle.append(flagImage);

      countryCodeDropdown.append(newEle);
    });
    const select = document.querySelectorAll(".selectBtn");
    const option = document.querySelectorAll(".option");
    let index = 1;
    select.forEach((a) => {
      a.addEventListener("click", (b) => {
        $(a).toggleClass("active");
        const next = b.target.nextElementSibling;
        next.classList.toggle("toggle");
        next.style.zIndex = index++;
      });
    });
    option.forEach((a) => {
      a.addEventListener("click", (b) => {
        b.target.parentElement.classList.remove("toggle");
        const parent = b.target.closest(".select").children[0];
        parent.setAttribute("data-type", b.target.getAttribute("data-type"));
        parent.setAttribute("value", b.target.getAttribute("value"));
        parent.setAttribute("data-id", b.target.getAttribute("data-id"));
        parent.children[0].setAttribute(
          "src",
          b.target.getAttribute("data-src")
        );
        parent.children[1].innerText = b.target.innerText;

        if (b?.target?.getAttribute("data-length")) {
          parent.setAttribute(
            "data-length",
            b.target.getAttribute("data-length")
          );
          $("#mobileNo").attr(
            "maxlength",
            b.target.getAttribute("data-length")
          );
          if (
            $("#mobileNo").val().length > b.target.getAttribute("data-length")
          ) {
            $("#mobileNo").get(0).value = $("#mobileNo")
              ?.val()
              ?.substr(0, b.target.getAttribute("data-length"));
          }
        }
        $(".select .selectBtn.active").removeClass("active");
      });
    });
  }
);

$(".downloadBrochure , .projectLink").click(function () {
  var element = document.getElementById("formsids");
  element.reset();
  enq_project.removeAttribute("disabled", "");
  let currVal = $(this).attr("value");
  let currLocation = $(this).attr("data-location");
  var enq_projectoptions = document.querySelectorAll("#enq_project option");
  enq_projectoptions.forEach((o) => o.remove());
  for (let i = dropdown.options.length - 1; i >= 0; i--) {
    if (currVal.toLowerCase() === dropdown.options[i].value.toLowerCase()) {
      dropdown.options[i].selected = true;
    } else {
      dropdown.options[i].selected = false;
    }
  }

  for (let i = dropdownLocation.options.length - 1; i >= 0; i--) {
    if (
      currLocation.toLowerCase() ===
      dropdownLocation.options[i].value.toLowerCase()
    ) {
      dropdownLocation.options[i].selected = true;
      const enq_project = document.getElementById("enq_project");
      const projectArray = projectList.map((index) => {
        {
          if (currLocation === index?.location) {
            {
              const projectArray = index?.projects.map((i) => {
                const projectOptions = document.createElement("option");
                projectOptions.setAttribute("value", i?.value);
                projectOptions.setAttribute("data-type", i?.type);
                projectOptions.setAttribute("data-id", i?.id);
                projectOptions.innerHTML = i?.option;

                if (currVal === i?.value) {
                  projectOptions.setAttribute("selected", "selected");
                  projectOptions.selected = true;
                } else {
                  projectOptions.selected = false;
                }

                enq_project.append(projectOptions);
              });
            }
          }
        }
      });
    } else {
      dropdownLocation.options[i].selected = false;
    }
  }
});

$("#enq_location").on("change", function () {
  const currLocation = $(this).val();
  const enq_project = document.getElementById("enq_project");
  enq_project.removeAttribute("disabled", "");
  var enq_projectoptions = document.querySelectorAll("#enq_project option");
  enq_projectoptions.forEach((o) => o.remove());
  const projectArray = projectList.map((index) => {
    {
      if (currLocation === index?.location) {
        {
          const projectArray = index?.projects.map((i) => {
            const projectOptions = document.createElement("option");
            projectOptions.setAttribute("value", i?.value);
            projectOptions.setAttribute("data-type", i?.type);
            projectOptions.setAttribute("data-id", i?.id);
            projectOptions.innerHTML = i?.option;
            enq_project.append(projectOptions);
          });
        }
      }
    }
  });
  if (enq_project.length === 0) {
    const projectOptions = document.createElement("option");
    projectOptions.innerHTML = "Select Project";
    enq_project.append(projectOptions);
    enq_project.setAttribute("disabled", "disabled");
  }
});

var sections = $(".section"),
  nav = $(".menu-wrapper"),
  nav_height = nav.outerHeight();
$(window).on("scroll", function () {
  var cur_pos = $(this).scrollTop();
  sections.each(function () {
    var top = $(this).offset().top - 100,
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
$(document).ready(function () {
  $(".readmorelink").click(function () {
    $(this).prev().toggle();

    if ($(this).text() == "Read More") {
      $(this).text("Read Less");
    } else {
      $(this).text("Read More");
    }
  });
});

//$(".downloadBrochure").click(function () {
//  var element = document.getElementById("formsids");
//  element.reset();
//});
$(".enq-btn").click(function () {
  var element = document.getElementById("formsids");
  element.reset();

  const enq_project = document.getElementById("enq_project");

  enq_project.setAttribute("disabled", "disabled");

  var enq_projectoptions = document.querySelectorAll("#enq_project option");
  enq_projectoptions.forEach((o) => o.remove());

  const projectOptions = document.createElement("option");
  projectOptions.innerHTML = "Select Project";
  enq_project.append(projectOptions);
});

const allowOnlyNumberInput = (ele) => {
  ele.value = ele.value.replace(/[^0-9]/g, "");
};

const accordionButtons = document.querySelectorAll(".accordion-button");
accordionButtons.forEach((button) => {
  button.addEventListener("click", () => {
    const accordionContent = button.parentElement.nextElementSibling;
    accordionButtons.forEach((otherButton) => {
      if (otherButton !== button) {
        otherButton.classList.add("collapsed");
        otherButton.setAttribute("aria-expanded", "false");
        const otherAccordionContent =
          otherButton.parentElement.nextElementSibling;
        otherAccordionContent.classList.add("collapse");
      }
    });
    button.classList.toggle("collapsed");
    accordionContent.classList.toggle("collapse");
    const isExpanded = button.getAttribute("aria-expanded") === "true";
    button.setAttribute("aria-expanded", String(!isExpanded));
  });
});

for (let i = 0; i < clickElementDropdown?.length; i++) {
  clickElementDropdown?.[i].addEventListener("click", function () {
    if (!cityDropdown?.[i]?.classList?.contains("active")) {
      arrowDownIcon?.[i]?.classList?.add("rotate");
      const elements = document.querySelectorAll("*");
      elements.forEach((element) => {
        element.classList.remove("active");
      });
      cityDropdown?.[i]?.classList?.add("active");
    } else {
      arrowDownIcon?.[i]?.classList?.remove("rotate");
      cityDropdown?.[i]?.classList?.remove("active");
    }
  });
}
const cards = document.querySelectorAll(".card");

function handleDropdownChange(index) {
  

  let projectTypes = cards[index].querySelectorAll(".project_type");
  
  const residentialCardBody = cards[index].querySelector("#residential");
  const commercialCardBody = cards[index].querySelector("#commercial");
  const clubsCardBody = cards[index].querySelector("#clubs");

  projectTypes.forEach(function (projectType) {
    projectType.addEventListener("click", function () {
      let selectedCardTabHeading = cards[index].querySelector(".project_type:hover").textContent;
      let headingElement = cards[index].querySelector(".tabHeading");
      headingElement.textContent = selectedCardTabHeading;

      if (projectType.id == "residentialDropdown") {
        residentialCardBody.style.display = "block";
        commercialCardBody.style.display = "none";
        if (clubsCardBody) {
          clubsCardBody.style.display = "none";
        }
        const elements = document.querySelectorAll("*");
        elements.forEach((element) => {
          element.classList.remove("active");
        });
        arrowDownIcon?.[index]?.classList?.remove("rotate");
      } else if (projectType.id == "commercialDropdown") {
        residentialCardBody.style.display = "none";
        commercialCardBody.style.display = "block";
        if (clubsCardBody) {
          clubsCardBody.style.display = "none";
        }
        const elements = document.querySelectorAll("*");
        elements.forEach((element) => {
          console.log("element");
    
          element.classList.remove("active");
        });
        arrowDownIcon?.[index]?.classList?.remove("rotate");
      } else {
        residentialCardBody.style.display = "none";
        commercialCardBody.style.display = "none";
        clubsCardBody.style.display = "block";
        const elements = document.querySelectorAll("*");
        elements.forEach((element) => {
          element.classList.remove("active");
        });
        arrowDownIcon?.[index]?.classList?.remove("rotate");
      }

    });
  });

  
}

for (let i = 0; i < cards.length; i++) {
  const element = cards[i].querySelector(
    ".cardthumb .bottomfixed #city_dropdown"
  );
  element.addEventListener("click", (event) => {
    handleDropdownChange(i);
  });
}
