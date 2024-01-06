$('#leaderspeak_slickslider').slick({
    dots: false,
    infinite: false,
    arrows:true,
    autoplay: false,
    autoplaySpeed: 2000,
    speed: 300,
    slidesToShow: 3,
    slidesToScroll: 1,
    vertical:true,
    swipeToSlide: true,
    verticalSwiping: true,
    responsive: [
       {
          breakpoint: 1024,
          settings: {
          slidesToShow: 3,
          slidesToScroll: 1,
          }
       },
       {
          breakpoint: 767,
          settings: {
          slidesToShow: 3,
          slidesToScroll: 1
          }
       }
    ]
});
 
 
var scrolltimes = 0
 
 
 
$(window).scroll(function () {
    if($(window).width() < 767) {
      var offsetDistance = 75; }
    else {
      var offsetDistance = 50;
}
 
try {
  var offset = $("#slick_slider_scroll").offset().top-offsetDistance;
    if ($(window).scrollTop() >= offset) {
        if(scrolltimes == 0){
            scrolltimes == 1;
            if($(window).width() < 767){
                $('#leaderspeak_slickslider').on('touchstart', e => {
                $('#leaderspeak_slickslider').slick('slickPlay');
                });
            $('#leaderspeak_slickslider').trigger('touchstart');
         }
        else{
                $('#leaderspeak_slickslider').slick('slickSetOption', {'autoplay': true}, true);
         }
       }  
     }
}
catch(err) {

}

 });