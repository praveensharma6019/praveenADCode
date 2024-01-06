$(document).ready(function(){
		
	$('.hero-banner').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplayHoverPause: true,
		items: 1,
		nav: false,
		dots: true,
		autoPlay: 2500,
		slideSpeed: 2000,
		smartSpeed: 1500,
		paginationSpeed: 2000
    });
	
	$('.single_item').owlCarousel({
            loop: true,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 2000,
            autoplay: true,
            nav: true,
            dots: false,
			autoplayHoverPause: true,
			autoPlay: 2500,
			slideSpeed: 2000,
			smartSpeed: 1500,
			paginationSpeed: 2000,
            navText: ["<span class='fa fa-angle-left'></span>", "<span class='fa fa-angle-right'></span>"],
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        });
		
		$('.inner-carousel').owlCarousel({
            loop: false,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 2000,
            autoplay: true,
            nav: false,
            dots: true,
			autoplayHoverPause: true,
			autoPlay: 2500,
			slideSpeed: 2000,
			smartSpeed: 1500,
			paginationSpeed: 2000,
            navText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"],
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        });
		

	
});

$('.scroll_down a').on('click', function (e) {
        var href = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(href).offset().top - 70
        }, '600');
        e.preventDefault();
});





// Hide Header on scroll down
var didScroll;
var lastScrollTop = 0;
var delta = 0;
var navbarHeight = 0;

$(window).scroll(function(event){
    didScroll = true;
});

setInterval(function() {
    if (didScroll) {
        hasScrolled();
        didScroll = false;
    }
}, 250);
function hasScrolled() {
    var st = $(this).scrollTop();
    
    // Make sure they scroll more than delta
    if(Math.abs(lastScrollTop - st) <= delta)
        return;
    
    // If they scrolled down and are past the navbar, add class .nav-up.
    // This is necessary so you never see what is "behind" the navbar.
		$('#back-to-top').fadeIn();
		$('header').addClass('fixed-nav');
    if (st > lastScrollTop && st > navbarHeight){
        // Scroll Down
        
    } else {
        // Scroll Up
        if(st + $(window).height() < $(document).height()) {
        
        }
    }
    if(st < 150){
		
		$('#back-to-top').hide();
		$('header').removeClass('fixed-nav');
	}
    lastScrollTop = st;
}
	
	// scroll body to 0px on click
         $('#back-to-top').click(function () {
             $('#back-to-top').tooltip('hide');
             $('body,html').animate({
                 scrollTop: 0
             }, 800);
             return false;
         });
		



$('body').on('hidden.bs.modal', '.modal', function () {
$('video').trigger('pause');
});