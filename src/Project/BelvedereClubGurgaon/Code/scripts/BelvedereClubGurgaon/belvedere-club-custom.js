$(document).ready(function(){

	/* Initialize Banner Carousel on home page */
	
	
	/* Menu on mobile devices */
	
	$('#dismiss, .overlay, .overlay-top').on('click', function () {
		$('#sideNav').removeClass('active');
		$('.overlay, .overlay-top').removeClass('active');
		 $('#topMenu').removeClass('active');
	});
	
	/*Side menu on mobile devices*/
	
	$('#sidebarCollapse').on('click', function () {
		$('#sideNav, #mainContent').toggleClass('active');
		$('.collapse.in').toggleClass('in');
		 $('.overlay').addClass('active');
		//$('a[aria-expanded=true]').attr('aria-expanded', 'false');
	});
			
	/* Top menu on mobile devices*/
	
	 $('#topNavCollapse').on('click', function () {
		$('#topMenu').toggleClass('active');
		$('.collapse.in').toggleClass('in');
		 $('.overlay-top').addClass('active');
	   
	});
	
	$('#home-carousel').owlCarousel({
        loop: true,
        autoplayHoverPause: true,
		margin: 10,
		animateOut: 'fadeOut',
		slideSpeed: 2500,
		responsiveClass: true,
		autoplayTimeout:2500 ,
		responsive: {
		  0: {
			items: 1,
			nav: false,
			dots: true,
			nav: false,
			autoplay: true
		  },
		  600: {
			items: 1,
			nav: false,
			dots: true,
			nav: false,
			autoplay: true
		  },
		  1000: {
			items: 1,
			nav: true,
			dots: true,
			loop: true,
			margin: 20,
			autoplay: true
			
		  }
		}
	  });
    $('.bg-slider').owlCarousel({
        autoplayHoverPause: true,
		loop: false,
		margin: 10,
		slideSpeed: 2500,
		animateOut: 'fadeOut',
		responsiveClass: true,
		autoplayTimeout:2500 ,
		autoplay: true,
		nav: true,
		dots: false,
		autoHeight:true,
		navText: [
      '<i class="fa fa-angle-left" aria-hidden="true"></i>',
      '<i class="fa fa-angle-right" aria-hidden="true"></i>'
    ],
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
    $('#what-members-say').owlCarousel({
        autoplayHoverPause: true,
		loop: true,
		disabled: false,
		margin: 10,
		animateOut: 'fadeOut',
		slideSpeed: 2500,
		responsiveClass: true,
		autoplayTimeout:2500 ,
		responsive: {
		  0: {
			items: 1,
			nav: true,
			dots: true,
			nav: true,
			autoplay: false
		  },
		  600: {
			items: 1,
			nav: true,
			dots: true,
			nav: true,
			autoplay: false
		  },
		  1000: {
			items: 2,
			nav: true,
			dots: true,
			loop: true,
			margin: 20,
			autoplay: false
			
		  }
		}
	  });
	  /* Accolades For about us */
    $("#accolades").owlCarousel({
        autoplayHoverPause: true,
		loop:true,
		margin:10,
		autoplay: true,
		responsive : {
		// breakpoint from 0 up
		0 : {
			items : 2,
			nav:false
		},
		// breakpoint from 480 up
		480 : {
			items : 3,
			nav:false,
			dots:false
		},
		// breakpoint from 768 up
		768 : {
			items : 4,
			nav:false,
			dots:false
		}
	}
	});
	
	
	
	/*Scroll To top JS*/
// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function() {scrollFunction()};

function scrollFunction() {
    if (document.body.scrollTop > 40 || document.documentElement.scrollTop > 80) {
        document.getElementById("scrolltop").style.display = "block";
		$('#mainMenu').addClass('stickyheader');
		$('.mainMenu').css("display","none");
    } else {
        document.getElementById("scrolltop").style.display = "none";
		$('#mainMenu').removeClass('stickyheader');
		$('.mainMenu').css("display","block");
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

	
	
	
	
	 
	  /* Initialize Banner Carousel on home page */
        $("#bannerCarousel").owlCarousel({
            autoplay:true,
            autoplayTimeout:2000,
            autoplayHoverPause:true,
            center: true,
            loop:true,
            items: 1,
            pagination : true,

        });
		AOS.init({
				easing: 'ease-out-back',
				duration: 1000
			});
});