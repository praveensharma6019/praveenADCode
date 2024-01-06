export default {
    init(){
        var btn = $('.btn-scroll-top');
        var header = $('.header-holder');

        $(window).scroll(function() {
          if ($(window).scrollTop() > 1000) {
            btn.addClass('show');
          } else {
            btn.removeClass('show');
          }
          if ($(window).scrollTop() > 48) {
            header.addClass('sticky');
          } else {
            header.removeClass('sticky');
          }
        });

        btn.on('click', function(e) {
          e.preventDefault();
          // $('html, body').animate({scrollTop:0}, '300');
          scrollTo(0, 0)
        });

          // remove caraousel in mobile

          // function removeCarousel() {
          //   var owlSlider = $('.owl-carousel.removeSliderMob');
          //   owlSlider.trigger('destroy.owl.carousel');
          //   owlSlider.addClass('off');
          // }
            
          // if (window.innerWidth <= 991) {
          //   removeCarousel();
          // }

    }
} 