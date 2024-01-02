function mobileSlider(objID, cardClass = 'mobSlideItem'){
  jQuery(objID).addClass('scrollbar-x mobSlider');
  jQuery(objID).children().addClass(cardClass);
}
function sliderAdd(){
  
  if(window.innerWidth >= 991) {

 jQuery(".v-slider").slick({
   slidesToShow: 3,
   slidesToScroll: 3,
   dots: false,
   infinite: false,
   nav: true,
   prevArrow: '<i class="i-arrow-l slick-prev"></i>',
   nextArrow: '<i class="i-arrow-r slick-next"></i>',
   responsive: [
     {
       breakpoint: 1200,
       settings: {
         slidesToShow: 3,
         slidesToScroll: 3,
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

 jQuery(".gradient-slider").slick({
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
        slidesToShow: 3,
        slidesToScroll: 3,
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
  mobileSlider(".v-slider",'mobSlideItem');
  mobileSlider(".gradient-slider",'mobSlideItem');
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

