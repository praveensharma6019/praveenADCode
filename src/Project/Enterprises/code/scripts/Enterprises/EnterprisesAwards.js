$(function () {

    var award_elements = document.getElementsByClassName('detailed-section');

    // Create an array to store the IDs
    var awards_data = [];

    // Iterate over the elements and extract their IDs
    for (var i = 0; i < award_elements.length; i++) {
        awards_data.push(award_elements[i].id);
    }
    let owl_carousels = [];

    awards_data.forEach((element, index) => {

        owl_carousels[index] = $("#Carousel-awards-" + element);
        owl_carousels[index].owlCarousel({
            items: 1,
            loop: true,
            nav: true,
            responsive: {
                0: {
                    items: 1
                },
                768: {
                    items: 3
                },
                992: {
                    items: 1
                }
            }
        });
        console.log("owl-prev" + element);

        $(".owl-prev-" + element).on("click", function () {
            owl_carousels[index].trigger("prev.owl.carousel");
        });

        // Trigger next slide when the custom next button is clicked
        $(".owl-next-" + element).on("click", function () {
            owl_carousels[index].trigger("next.owl.carousel");
        });


    });


});

$(document).ready(function () {
    // Hide all detailed sections on page load
    $(".detailed-section").hide();

    $(".award-link").on("click", function (event) {
        event.preventDefault();

        var targetId = $(this).data("target");

        // Hide all detailed sections
        $(".detailed-section").hide();

        // Show the clicked detailed section
        $("#" + targetId).show();
    });
});



