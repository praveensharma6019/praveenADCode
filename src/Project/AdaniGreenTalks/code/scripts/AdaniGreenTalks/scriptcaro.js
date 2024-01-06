$(function () {
    // Owl Carousel
    var owl = $(".owl-carousel");
    owl.owlCarousel({
        items: 3,
        margin: 10,
        loop: true,
        nav: true,
        dots: false,
        responsive: {
            400: {
                margin: 30,
                nav: true,
                items: 1
            },
            800: {
                margin: 30,
                stagePadding: 0,
                nav: true,
                items: 2
            },
            1200: {
                margin: 20,
                stagePadding: 0,
                nav: true,
                items: 3
            },
            100: {
                margin: 20,
                stagePadding: 0,
                nav: true,
                items: 1
            }
        }
    });
});