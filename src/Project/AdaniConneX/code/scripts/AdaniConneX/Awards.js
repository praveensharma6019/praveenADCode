$(document).ready(function () {

$(".extra_slide").slideUp();

//checking for mobile and Web items both 
var listLength = document.querySelectorAll(".dr_locations_image").length;
if(listLength > 10)
{
$('.view_more_btn').show();
}
else
{
$('.view_more_btn').hide();
}

//checking for mobile and Web items both 
var listLength = document.querySelectorAll(".dr_locations_image").length;
if(listLength > 10)
{
$('.view_more_mobile').show();
}
else
{
$('.view_more_mobile').hide();
}


    $(".getInTouchActionnew").on('click',function(){
        $('#getInTouchPopup').addClass('show');
    });

    var Videolightbox = GLightbox({
        selector: '.Videolightbox',
        touchNavigation: true,
        //loop: true,
        autoplayVideos: true,
        preload:true,
    });

    $('#user_quote_slider').slick({
        autoplay:false,
        arrows: false,
        fade: false,
        //dots:true,
        adaptiveHeight: true,
        //asNavFor: '#dr_career_text'
        slidesToShow: 1,
        slidesToScroll: 1,
        swipeToSlide: false,
        vertical: true,
        verticalSwiping: false,
        centerMode: false,
        // asNavFor: '#user_videos_slider',
        infinite:false,
    });

    var user_videos_slider = $('#user_videos_slider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: false,
        dots: false,
        arrows: false,
        fade: true,
        swipeToSlide: false,
    });

  });

$('#user_quote_slider').on('afterChange',function(){
    $('#user_quote_slider .slick-slide.slick-current').find(".dr_locations_image").first().trigger("click");
});

$(".view_more_btn").on("click", function(e){
  if($(this).hasClass("next")){
    $('#user_quote_slider').slick('slickPrev');
    $(this).removeClass("next");
    $(this).addClass("prev")
  }

  else{
    $('#user_quote_slider').slick('slickNext');
    $(this).removeClass("prev");
    $(this).addClass("next")
  }

});

  $("#user_quote_slider .dr_locations_image").on("click",function(){
    $("#user_quote_slider .dr_locations_image").removeClass("active");
    dataindex = $(this).attr("data-index");
    $('#user_videos_slider').slick('slickGoTo', dataindex);
    $('#user_videos_slider[data-slick-index='+(dataindex)+']').css("opacity","1").fadeIn();
    $(this).addClass("active");
  });

  $('#user_videos_slider').on("init", function (event, slick) {
            var noOfSlides = slick.slideCount;
            if (noOfSlides > 5) {
              $(".view_more_trigger").show();
                // show more button on desktop and mobile
            }
        });

$(".view_more_mobile").on("click",function(){
  currtext = $(this).text();
  if($(this).html().toLowerCase().indexOf("more") != -1){
    $(".extra_slide").slideDown();
    $(this).text(currtext.replace("More","Less").replace("more","less"))
  }

  else{
    $(".extra_slide").slideUp();
    $(this).text(currtext.replace("Less","More").replace("less","more"))
  }
});

// updated file
    $('#data-center-list').owlCarousel({
        items: 3,
        nav: false,
        dots: true,
        // mouseDrag: true,
        loop: true,
        autoplay: true,
        autoplayHoverPause: true,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1.15,
            },
            767: {
                items: 1.6,
            },
            992: {
                items: 3,
            }
        }
    });