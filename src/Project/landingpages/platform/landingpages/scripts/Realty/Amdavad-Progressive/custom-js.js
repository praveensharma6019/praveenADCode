
$('.carousel-stories').on('initialized.owl.carousel changed.owl.carousel', function (e) {
    if (!e.namespace) {
        return;
    }
    var carousel = e.relatedTarget;
    $('.carousel-count').text(carousel.relative(carousel.current()) + 1 + '/' + carousel.items().length);
}).owlCarousel({
    items: 1,
    loop: false,
    margin: 0,
    nav: true,
    dots: false,
    navText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
});

$('.carousel-banner').owlCarousel({
    items: 1,
    loop: false,
    margin: 0,
    dots: true,
    responsive: {
        0: {
            dots: true
        },
        600: {
            dots: true
        }
    }
});


var didScroll;
var lastScrollTop = 0;
var delta = 0;
var navbarHeight = 0;

$(window).scroll(function (event) {
    didScroll = true;
});

setInterval(function () {
    if (didScroll) {
        hasScrolled();
        didScroll = false;
    }
}, 250);
function hasScrolled() {
    var st = $(this).scrollTop();

    if (Math.abs(lastScrollTop - st) <= delta)
        return;
    $('header').addClass('sticky-header');

    if (st > lastScrollTop && st > navbarHeight) {

    } else {
        if (st + $(window).height() < $(document).height()) {

        }
    }
    if (st < 150) {

        $('header').removeClass('sticky-header');
    }
    lastScrollTop = st;
}

$('.navbar-collapse-inner a').click(function (e) {
    e.preventDefault();
    var target = $($(this).attr('href'));
    if (target.length) {
        var scrollTo = target.offset().top;
        $('body, html').animate({ scrollTop: scrollTo - 100 + 'px' }, 800);
    }
});


$(window).scroll(function () {
    var windscroll = $(window).scrollTop();
    if (windscroll >= 100) {
        $('section').each(function (i) {
            if ($(this).position().top <= windscroll - 0) {
                $('nav li.active').removeClass('active');
                $('nav li').eq(i).addClass('active');
            }
        });

    } else {

        $('nav li.active').removeClass('active');
        $('nav li:first').addClass('active');
    }

}).scroll();

$('.readbtn').click(function () {
    $(this).children().toggleClass('active in-active');
    $(this).parent().parent().toggleClass('block-readmore');
})

$('#togglePara').on('click', function () {
    $(this).siblings('#more').slideToggle()
    $(this).toggleClass('clicked');
    if ($(this).text() === "Read More") {
        $(this).text("Read Less")
    }
    else {
        $(this).text("Read More")
    }
})

if ($(window).width() < 992) {
    $('.nav-item').on('click', function () {
        $('.navbar-toggler').trigger('click')
    })
}