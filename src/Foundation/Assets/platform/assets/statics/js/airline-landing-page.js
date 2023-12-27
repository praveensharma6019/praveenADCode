

/* mobile Slider */
function mobileSlider(objID, cardClass = 'mobSlideItem'){
  jQuery(objID).addClass('scrollbar-x mobSlider');
  jQuery(objID).children().addClass(cardClass);
}

function sliderAdd(){
  
  if(window.innerWidth >= 991) {
    
  jQuery(".airlines-slider").slick({
  slidesToShow: 3,
  slidesToScroll: 3,
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
        prevArrow: false,
        nextArrow:false,
      },
    },
    {
      breakpoint: 768,
      nav: false,
      settings: {
        slidesToShow: 1.5,
        slidesToScroll: 1,
        prevArrow: false,
        nextArrow:false,
      },
    },
    {
      breakpoint: 480,
      nav: false,
      settings: {
        slidesToShow: 1.5,
        slidesToScroll: 1,
        prevArrow: false,
        nextArrow:false,
      },
    },
  ],
});

} else {
      mobileSlider(".airlines-slider",'mobSlideItem');
}
}

sliderAdd();

var btn = $('#scrollToTop');

$(window).scroll(function() {
  if ($(window).scrollTop() > 2000) {
    btn.addClass('show');
  } else {
    btn.removeClass('show');
  }
  // if($(window).scrollTop()>jQuery(".ahemdabadAirport").offset().top){
  //   jQuery('.nav-Section').show();
  //   console.log('menu ON')
  // } else {
  //   jQuery('.nav-Section').hide();
  //   console.log('menu off')

  // }
});

// btn.on('click', function(e) {
//   e.preventDefault();
//   jQuery('html, body').animate({scrollTop:0}, '300');
// });


function readMore(obj,ID){
  targetObj = jQuery('#'+ID);
  if(!targetObj.hasClass('show')){
    targetObj.addClass('show');
    targetObj.fadeIn(400);
    jQuery(obj).text('Read Less');
  } else {
    targetObj.removeClass('show');
    targetObj.fadeOut(400);
    jQuery(obj).text('Read More');
  }
}

window.onload = function (){
  document.body.scrollTop = 0;
  document.documentElement.scrollTop = 0;
  setTimeout(() => {
     var navlinks=document.getElementsByClassName("tab-nav");
     for (var i=0; i<navlinks.length; i++) {
        navlinks[i].classList.remove('active');
        if (i===0)
        {
           navlinks[i].classList.add('active');
        }
     }
     document.body.scrollTop = 0 ; // For Safari
     document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera  
  }, 200);
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

$(document).ready(function () {
    $(".slide").slice(0, 10).show();
    $("#loadMore").on("click", function (e) {
        e.preventDefault();
        $(".slide:hidden").slice(0, 10).slideDown();
        if ($(".slide:hidden").length == 0) {
            $("#loadMore").text("No Content").addClass("noContent");
        }
    });

})