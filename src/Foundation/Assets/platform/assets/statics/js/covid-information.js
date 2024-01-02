
/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem'){
  jQuery(objID).addClass('scrollbar-x mobSlider');
  jQuery(objID).children().addClass(cardClass);
}
window.onscroll = function () {
  try {
  let horizontaltabHeader = document.getElementById("navbar-tabs");
  var navlinks=document.getElementsByClassName("tab-nav");
  if(document.querySelectorAll("#navbar-tabs .nav-link.active").length < 1){
    navlinks[0].classList.add('active');
  }
  // console.log(horizontaltabHeader.offsetTop);
  // console.log(window.pageYOffset < horizontaltabHeader.offsetTop);
  // if (window.pageYOffset < horizontaltabHeader.offsetTop) {
  //       document.getElementsByClassName("tab-nav")[0].classList.add('active');
  // }
} catch (error) {       
}
};

   window.onscroll = function () {
  try {
  let horizontaltabHeader = document.getElementById("navbar-tabs");
  var navlinks=document.getElementsByClassName("tab-nav");
  if(document.querySelectorAll("#navbar-tabs .nav-link.active").length < 1){
    navlinks[0].classList.add('active');
  }
  // console.log(horizontaltabHeader.offsetTop);
  // console.log(window.pageYOffset < horizontaltabHeader.offsetTop);
  // if (window.pageYOffset < horizontaltabHeader.offsetTop) {
  //       document.getElementsByClassName("tab-nav")[0].classList.add('active');
  // }
} catch (error) {       
}
};

function sliderAdd(){
  
  if(window.innerWidth >= 991) {



 jQuery(".slider-airports").slick({
   slidesToShow: 4,
   slidesToScroll: 4,
   dots: false,
   infinite: false,
   nav: true,
   prevArrow: '<i class="i-arrow-l slick-prev"></i>',
   nextArrow: '<i class="i-arrow-r slick-next"></i>',
   responsive: [
     {
       breakpoint: 1200,
       settings: {
         slidesToShow: 4,
         slidesToScroll: 4,
       },
     },
     {
       breakpoint: 480,
       nav: false,
       settings: {
         slidesToShow: 1.5,
         slidesToScroll: 1,
       },
     },
   ],
 });



 } else {
  mobileSlider(".mumbai-airports",'mobSlideItem');
  mobileSlider(".slider-airports",'mobSlideItem');
  mobileSlider(".slider-mobile",'mobSlideItem');
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

