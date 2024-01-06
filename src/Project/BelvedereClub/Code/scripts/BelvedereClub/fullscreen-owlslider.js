 $("#fullscarousel").on("initialized.owl.carousel", function () {
  setTimeout(function () {
    $(".owl-item.active .owl-slide-animated").addClass("is-transitioned");
    $("section").show();
  }, 200);
});

var $owlCarousel = $("#fullscarousel").owlCarousel({
  items: 1,
  loop: true,
  nav: false,
  autoplay: true,
  	slideSpeed: 2000,
		responsiveClass: true,
		autoplayTimeout:3000 ,
  dots:false });



$owlCarousel.on("changed.owl.carousel", function (e) {
  $(".owl-slide-animated").removeClass("is-transitioned");

  var $currentOwlItem = $(".owl-item").eq(e.item.index);
  $currentOwlItem.find(".owl-slide-animated").addClass("is-transitioned");


});