$(document).ready(function () {

    /*Hero banner*/
    $('.hero-banner').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
        nav: false,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })
    /*Hero banner*/

    var groups = {};
    $('.galleryItem').each(function () {
        var id = parseInt($(this).attr('data-group'), 10);

        if (!groups[id]) {
            groups[id] = [];
        }

        groups[id].push(this);
    });


    $.each(groups, function () {

        $(this).magnificPopup({
            type: 'image',
            closeOnContentClick: true,
            closeBtnInside: false,
            gallery: { enabled: true }
        })

    });



});

$(document).on('click', '.load-more', function (e) {
    //$('.load-more').click(function (e) {
    e.preventDefault();
    var loadmore = this;
    var page = $(this).attr('count');
    var pageCounter = parseInt(page);
    pageCounter++;
    var element = $('#PageId').val();
    $.ajax({
        type: "GET",
        url: "/api/AVRPL/PhotoGalleryLoadMore",
        data: { count: page, element: element },
        success: function (obj) {
            if (obj != "" && obj.MonthYearName != null && obj.AdaniRoadImages.length > 0) {
                var imgrows = '';
                for (var i = 0; i < obj.AdaniRoadImages.length; i++) {
                    imgrows = imgrows + '<div class="col-lg-3 col-md-4 col-sm-6 mt-4">'
                        + '<a href="' + obj.AdaniRoadImages[i].ImageUrl + '" data-group="1" class="galleryItem">'
                        + '<div>'
                        + '<img src="' + obj.AdaniRoadImages[i].ImageUrl + '" class="img-fluid w-100">'
                        + '</div>'
                        + '</a>'
                        + '</div>';
                }
                var rows = '<h2>' + obj.MonthYearName + '</h2>'
                    + '<div class="row">' + imgrows + '</div>';
                $('.load-more').remove();
                $('.gallery-container').append(rows);
                $('.gallery-container').append(loadmore);
                $('.load-more').attr('count', pageCounter)
            }
            else {
                $('.load-more').remove();
            }

        },
        error: function () {
            alert("error");
        }
    });

});