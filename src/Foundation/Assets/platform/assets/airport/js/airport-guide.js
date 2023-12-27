/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem'){
  jQuery(objID).addClass('scrollbar-x mobSlider');
  jQuery(objID).children().addClass(cardClass);
}

function sliderAdd(){
  
  if(window.innerWidth >= 991) {
jQuery(".airportGuide").slick({
    slidesToShow: 3,
    slidesToScroll: 3,
    dots: false,
    infinite: false,
    nav: false,
    prevArrow: '<i class="i-arrow-l slick-prev"></i>',
    nextArrow: '<i class="i-arrow-r slick-next"></i>',
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          nav: false,
          prevArrow: false,
          nextArrow: false,
         
        },
      },
      {
        breakpoint:768,
        settings: {
          slidesToShow: 1.2,
          slidesToScroll: 1.2,
          nav: false,
          prevArrow: false,
          nextArrow: false,
         
        },
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1.2,
          slidesToScroll: 1.5,
          nav: false,
          prevArrow: false,
          nextArrow: false,
        },
      },
    ],
  });


  jQuery(".airportExperiences").slick({
    slidesToShow: 2,
    slidesToScroll: 2,
    dots: false,
    infinite: false,
    nav: false,
    prevArrow: '<i class="i-arrow-l slick-prev"></i>',
    nextArrow: '<i class="i-arrow-r slick-next"></i>',
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
          nav: false,
         
        },
      },
      {
        breakpoint:767,
        settings: {
          slidesToShow: 1.2,
          slidesToScroll: 1.2,
          nav: false,
          prevArrow:false,
          nextArrow:false,
         
        },
      },
      {
        breakpoint:540,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          nav: false,
          prevArrow:false,
          nextArrow:false,
        },
      },
    ],
  });
} 
else {
  mobileSlider(".airportGuide",'mobSlideItem');
  mobileSlider(".airportExperiences",'mobSlideItem');
  }
}

sliderAdd();
window.onload = function (){
  logoChange();
} 

setTimeout(function(){
  jQuery('.airport-features, .mobile-slider, .slider-airports').removeClass('blank-holder');
  jQuery('.blank').removeClass('blank');
}, 2000);

function getAirportList() {
  const { innerWidth: width, innerHeight: height } = window;
  if (width > 992) {
    try {
      var $staticBackdrop = document.getElementById("staticBackdrop");
      var bsOffcanvas = new bootstrap.Modal($staticBackdrop);
      bsOffcanvas.show();

      var airportList = document.getElementById("airportList");
      airportList.style.display = "grid";
      document.getElementById("modal-body").appendChild(airportList);
      document
        .querySelector(".closeAirportListDialog")
        .setAttribute("data-bs-dismiss", "modal");
    } catch (error) {}
  } else if (width <= 992) {
    try {
      var $myOffcanvas = document.getElementById("offcanvas_airportList");
      var bsOffcanvas = new bootstrap.Offcanvas($myOffcanvas);
      bsOffcanvas.show();

      var airportList = document.getElementById("airportList");
      airportList.style.display = "flex";
      document.getElementById("offcanvas-body").appendChild(airportList);
      document
        .querySelector(".closeAirportListDialog")
        .setAttribute("data-bs-dismiss", "offcanvas");
    } catch (error) {}
  }
}
