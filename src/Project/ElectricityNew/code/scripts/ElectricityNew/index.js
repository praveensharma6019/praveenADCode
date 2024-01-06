// OWL carousel

var slides = $(".js-owl-carousel-synced-slides");
var thumbnails = $(".js-owl-carousel-synced-thumbnails");

slides.owlCarousel({
    singleItem: true,
    items: 1,
    autoHeight:true,
    AutoHeight:Defaults = {
        autoHeight: true,
        autoHeightClass: 'owl-height'
    },
    slideSpeed: 500,
    navigation: false,
    pagination: false,
    responsive: true,
    addClassActive: true,
    transitionStyle: false,
    afterAction: syncPosition
});

thumbnails.owlCarousel({
    items: 3,
    autoHeight: true,
    AutoHeight: Defaults = {
        autoHeight: true,
        autoHeightClass: 'owl-height'
    },
    pagination: true,
    margin: 10,
    nav: true,
    dots: true,
    responsive: false,
    transitionStyle: false,
    afterInit: function (element) {
        element.find(".owl-item").eq(0).addClass("synced");
    }
});

function syncPosition(element) {
    var current = this.currentItem;
    thumbnails
        .find(".owl-item")
        .removeClass("synced")
        .eq(current)
        .addClass("synced");
}

$(thumbnails).on("click", ".owl-item", function (element) {
    element.preventDefault();
    var number = $(this).data("owlItem");
    slides.trigger("owl.goTo", number);
});

$(".owl-item").on("swipe", function () {
    alert("Swipe");
});


