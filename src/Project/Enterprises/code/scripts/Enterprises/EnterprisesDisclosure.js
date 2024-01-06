
// ====================Disclosure===================
  $('#disclouser').owlCarousel({
        items: 4,
        loop: true,
        margin: 15,
        nav: false,
        responsive: {
            0: {
                items: 1
            },
            768: {
                items: 4
            }
        }
    });

    $(".last-next-button").click(function () {
        $('.sliding-images').trigger('next.owl.carousel');
    });

    $(".last-prev-button").click(function () {
        $('.sliding-images').trigger('prev.owl.carousel');
    });