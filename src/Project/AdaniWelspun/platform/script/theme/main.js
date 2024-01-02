

$(document).ready(function () {
  $('.airport-gallry').slick({
    prevArrow: '<span class="i-arrow-r slick-next"><img src="./images/arrow.svg"></span>',
    nextArrow: '<span class="i-arrow-r slick-prev"><img src="./images/arrow.svg"></span>',
    dots: false,
    infinite: false,
    speed: 300,
    loop:true,
    slidesToShow: 7,
    slidesToScroll: 7,
  
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3,
          infinite: true,
          dots: false,
        }
      },
      {
        breakpoint: 600,
        
        settings: {
          slidesToShow: 3,
          slidesToScroll: 3
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
        }
      }
    ]
  });

  $('.item-slider').slick({
    dots: false,
    infinite: false,
    speed: 300,
    slidesToShow: 3,
    slidesToScroll: 3,
    
    prevArrow: '<span class="i-arrow-r slick-next"><img src="./images/right-arrow.png"></span>',
    nextArrow: '<span class="i-arrow-r slick-prev"><img src="./images/right-arrow.png"></span>',
   
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
          infinite: true,
          dots: false,
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1.1,
          slidesToScroll: 1.1,
        }
      }
    ]
  });
  


  $('.book-slider').slick({
    dots: false,
    infinite: true,
    speed: 300,
    slidesToShow: 1,
    slidesToScroll: 1,
    prevArrow: '<span class="i-arrow-r slick-prev"><img src="./images/right-gallry-logo.webp"></span>',
    nextArrow: '<span class="i-arrow-r slick-next"><img src="./images/right-arrow.png"></span>',
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          infinite: true,
          dots: false,
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1.1,
          slidesToScroll: 1.1,
        }
      }
    ]
  });

  $('.item-slider-iem2').slick({
    dots: false,
    infinite: false,
    speed: 300,
    slidesToShow: 2,
    slidesToScroll: 2,
    prevArrow: '<span class="i-arrow-r slick-prev"><img src="./images/right-arrow.png"></span>',
    nextArrow: '<span class="i-arrow-r slick-next"><img src="./images/right-arrow.png"></span>',
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
          infinite: true,
          dots: true
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });

  $('.mob-gallery-slider').slick({
    dots: false,
    infinite: false,
    speed: 300,
    slidesToShow: 1,
    slidesToScroll: 1,

      
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
          infinite: true,
          dots: false,
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1,
        }
      }
    ]
  });
  $('.mob-gallery-slider').on('afterChange', function(event, slick, currentSlide){
    // console.log("currentSlide",event);

    console.log(currentSlide)
    $('#current').html(currentSlide + 1)
  });

  
  $('.mobile-trigger').click(function (e) {
    e.stopPropagation();
    $(this).closest('body').toggleClass('menu-open');
  });
  $(".custom-menu-section").click(function (e) {
    e.stopPropagation();
  });
  $(document).click(function (e) {
    $('body').removeClass('menu-open');
  });
  $('.close-icon').click(function (e) {
    $('body').removeClass('menu-open');
  });




  var helpers = {
    addZeros: function (n) {
      // return (n < 10) ? '0' + n : '' + n;
      return (n < 10) ? 0 + n : n;
    }
  };
  function sliderInit() {
    var $slider = $('.one-time');
    $slider.each(function () {
      var $sliderParent = $(this).parent();
      $(this).slick({
        dots: false,
        infinite: true,
        speed: 500,
        slidesToShow: 1,
        adaptiveHeight: true,
        prevArrow: '<span class="i-arrow-l slick-prev"><img src="./images/left-arrow.png"></span>',
        nextArrow: '<span class="i-arrow-r slick-next"><img src="./images/right-arrow.png"></span>',
      });

      if ($(this).find('.item').length > 1) {
        $(this).siblings('.slides-numbers').show();
      }

      $(this).on('afterChange', function (event, slick, currentSlide) {
        $sliderParent.find('.slides-numbers .active').html(helpers.addZeros(currentSlide + 1));
      });

      var sliderItemsNum = $(this).find('.slick-slide').not('.slick-cloned').length;
      $sliderParent.find('.slides-numbers .total').html(helpers.addZeros(sliderItemsNum));

      if (sliderItemsNum < 2) {
        $(this).closest('.floor-plan').find('.slides-numbers').hide();
      }

    });

  };


  $(window).scroll(function () {
    if ($(this).scrollTop() > 20) {
      $('body').addClass("scroll-body");
    } else {
      $('body').removeClass("scroll-body");
    }
  });
  
  $('.enq-btn').click(function () {
    $('.popup-form h3').text('Enquire Now');
    $('.enquiryBtn').text('Send Enquiry');
  });
  $('.custom-menu-section ul li a').click(function () {
    $(this).parent('li').siblings('li').removeClass('active');
    $(this).parent('li').addClass('active');
    if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {

      var target = $(this.hash);
      target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
      if (target.length) {
        $('html,body').animate({
          scrollTop: target.offset().top - 79
        }, 1000);
        return false;
      }
    }
  });

  $('.gallery').each(function () {
    $(this).find('.popup-img').magnificPopup({
      type: 'image',
      mainClass: 'mfp-with-zoom',
      gallery: {
        enabled: true
      },

      zoom: {
        enabled: true,

        duration: 300, // duration of the effect, in milliseconds
        easing: 'ease-in-out', // CSS transition easing function

        opener: function (openerElement) {

          return openerElement.is('img') ? openerElement : openerElement.find('img');
        }
      }
    });

  });


});


$(document).ready(function () {
    sliderInit();
  $('.terms-condition-wrapper .icon-button').click(function () {
    $(this).closest('.terms-condition-wrapper').toggleClass('collapse-item');
  });
});




// Show the first tab and hide the rest
$('#tabs-nav li:first-child').addClass('active');
$('.tab-content').hide();
$('.tab-content:first').show();

// Click function
$('#tabs-nav li').click(function(){
  $('#tabs-nav li').removeClass('active');
  $(this).addClass('active');
  $('.tab-content').hide();
  
  var activeTab = $(this).find('a').attr('href');
  $(activeTab).fadeIn();
  return false;
});






  $(window).scroll(function () {
    if ($(this).scrollTop() > 20) {
      $('body').addClass("scroll-body");
    } else {
      $('body').removeClass("scroll-body");
    }
  });




//   var typing=new Typed(".text", {
//     strings: ["", "Youtuber", "Freelancer", "Graphics Designer", "Web Designer", "Web Developer"],
//     typeSpeed: 100,
//     backSpeed: 40,
//     loop: true,
// });

















      










