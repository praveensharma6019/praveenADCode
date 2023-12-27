/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem'){
  jQuery(objID).addClass('scrollbar-x mobSlider');
  jQuery(objID).children().addClass(cardClass);
}

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
       breakpoint: 1024,
       settings: {
         slidesToShow: 2,
         slidesToScroll: 2,
       },
     },
     {
       breakpoint: 480,
       nav: false,
       settings: {
         slidesToShow: 1.8,
         slidesToScroll: 1,
       },
     },
   ],
 });

} else {
  mobileSlider(".slider-airports",'mobSlideItem');
}
}

sliderAdd();

 