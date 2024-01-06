// OWL carousel

var slides = $(".js-owl-carousel-synced-slides");
var thumbnails = $(".js-owl-carousel-synced-thumbnails");

slides.owlCarousel({
   singleItem: true,
   items: 1,
   autoplay:true,
   autoplayTimeout:500,
   autoHeight:true,
   slideSpeed: 300,
   navigation: true,
   pagination: false,
   responsive: true,
   addClassActive: true,
   transitionStyle : false,
   afterAction: syncPosition,
   dots: true,
   navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"]
});

thumbnails.owlCarousel({
   items: 3,
   pagination: true,
   responsive: true,
   transitionStyle : false,
   afterInit: function(element){
       element.find(".owl-item").eq(0).addClass("synced");
   }
});

function syncPosition(element){
   var current = this.currentItem;
   thumbnails
       .find(".owl-item")
       .removeClass("synced")
       .eq(current)
       .addClass("synced");
}

$(thumbnails).on("click", ".owl-item", function(element){
   element.preventDefault();
   var number = $(this).data("owlItem");
   slides.trigger("owl.goTo",number);
});

$(".owl-item").on("swipe",function(){
  alert("Swipe");
});


// Magnific Popup
var $owlCarouselBlock = $('.owl-carousel-block'),
   owl = $(".js-owl-carousel-synced-slides").data('owlCarousel');

$('.view').magnificPopup({
   items: {
       src: $owlCarouselBlock,
       type: 'inline'
   },
   closeBtnInside: false,
   preloader: true,
   removalDelay: 500,
   tLoading: 'Loading',
   callbacks: {
       open: function () {
         currentSlide = owl.currentItem;

           owl.reinit({
               afterInit : function () {
                   console.log('%c✅ start typing...', 'color: green;');
                   this.currentItem = currentSlide;
               }
           });
       },
       elementParse: function () {
           setTimeout(function() {
               $(".mfp-content")
                   .show()
                   .animate({
                       opacity: 1
                   }, 300);
           }, 500); // delay for synchronous loading both carousels
       },
       close: function() {
           $owlCarouselBlock
               .removeClass('mfp-hide');
           $(".mfp-content")
               .removeClass('mfp-content-load');

           owl.reinit();
       }
   }
});
/*Gallery Popup*/
	$(document).ready(function() {
  $(".popup").magnificPopup({
    type: "image",
    removalDelay: 160,
    preloader: false,
    fixedContentPos: true,
    gallery: {
      enabled: true
    }
  });
});
