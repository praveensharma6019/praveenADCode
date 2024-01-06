function onScroll() {
  var top = $(window).scrollTop();
  var size = $(window).height();
    $('.adv-counter-carousel .wrap').each(function() {
    var offset = $(this).offset().top;
    var difference = top + size - offset;
    if(difference < 0) {
      difference = 0;
    }
    if(top > offset + $(this).height()) {
      difference = size + $(this).height();
    }
    $(this).attr('style', '--scroll:' + difference + 'px;');
  });
}

var lastScrollTop = 0;
$(window).on('scroll',function(event){
   var st = $(this).scrollTop();
   var screenWidth = screen.width/2;
   if (st > lastScrollTop){
      $('#js-slideContainer').removeClass('downscroll');
      
      var rightPos = parseInt($('.adv-counter-carousel .wrap .counter-item:last-child').position().left - $('.adv-counter-carousel .wrap .counter-item:last-child').width());
      if (rightPos === 669) {
        $('#js-slideContainer').addClass('upscroll');
      }
   } else {
      $('#js-slideContainer').removeClass('upscroll');
      var leftPos = screenWidth - 520;
      if (leftPos === 236) {
        $('#js-slideContainer').addClass('downscroll');
      }
       
   }
   lastScrollTop = st;
});



$(function() { 
    $('.team-slide-wrapper').slick({
      dots: false,
      centerMode: true,
      centerPadding: '250px',
      slidesToShow: 1,
      responsive: [
        {
          breakpoint: 992,
          settings: {
            centerPadding: '100px'
          }
        }
      ]
    });
    onScroll();
    $(window).on('scroll', function() {
      onScroll();
    });
});
$(window).on('scroll', function() {
  if ($(this).scrollTop() > $('.advHome-banner .container .advHome-banner-img').offset().top) {
      $('.advHome-banner .container .advHome-banner-img').addClass('arrow-hide');
  } else {
      $('.advHome-banner .container .advHome-banner-img').removeClass('arrow-hide');
  }
});