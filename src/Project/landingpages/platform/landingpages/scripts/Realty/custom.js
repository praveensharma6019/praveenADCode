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
		$('header').addClass('fixed_group-header');
    if (st > lastScrollTop && st > navbarHeight){
        // Scroll Down
        
    } else {
        // Scroll Up
        if(st + $(window).height() < $(document).height()) {
        
        }
    }
    if(st < 150){
		$('header').removeClass('fixed_group-header');
	}
    lastScrollTop = st;
}

$(document).ready(function () {
	 var swiper = new Swiper(".mainslider", {
		        spaceBetween: 0,
		        centeredSlides: true,
		        loop:true,
		        autoplay: {
		          delay: 5000,
		          disableOnInteraction: false,
		        },
		        pagination: {
		          el: ".swiper-pagination",
		          clickable: true,
		        },
		        navigation: {
		          nextEl: ".swiper-button-next",
		          prevEl: ".swiper-button-prev",
		        },
		    });
});