$('#op').change(function () {
    var optionID = $('option:selected').attr('id');
    if (this.id == 'all') {
        $('#add-parent > div').fadeIn(450);
    } else {
        var $el = $('.' + this.id).fadeIn(450);
        $('#add-parent > div').not($el).hide();
    }
    $("." + optionID).show();
});

$(function () {
    $('#btn-mrelease').change(function () {
        
        $('.media-r').hide();
        $('.m-loadMore').hide();
        $('.' + $(this).val()).show();
    });
}); 


$(document).ready(function(){
	
	
	
	/*Scroll*/
  $("#nav-tab-about a").click(function(event){
	var y = $(window).scrollTop();
	y = y+150;
        $('html, body').animate({scrollTop: y}, 800);
    });

	
	
	if ($(".homeslider .owl-item video").prop('muted', false)){
      $("#mute").css("background-image","url(images/volume.png)");
    }

  $("#mute").click( function (){
    if( $(".homeslider .owl-item video").prop('muted') ) {
      $(".homeslider .owl-item video").prop('muted', false);
      $("#mute").css("background-image","url(images/volume.png)");
    } else {
      $(".homeslider .owl-item video").prop('muted', true);
      $("#mute").css("background-image","url(images/mute.png)");
    }
  });
	
/*Home Video Slider*/
	  var owl = $('.homeslider');
  owl.owlCarousel({
      loop:false,
      margin:0,
      nav:false,
      dots:false,
	  autoHeight:true,
	  lazyload:true,
      items:1
  })
  owl.on('translate.owl.carousel',function(e){
    $('.homeslider .owl-item video').each(function(){
      $(this).get(0).pause();
    });
  });
  $('.homeslider .owl-item .item').each(function(){
      var attr = $(this).attr('data-videosrc');
      if (typeof attr !== typeof undefined && attr !== false) {
        var videosrc = $(this).attr('data-videosrc');
        $(this).prepend('<video autoplay="true" muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.homeslider .owl-item.active video').attr('autoplay',true).attr('loop',true);
		 // explicitly mute it...
			
      }
    });
  owl.on('translated.owl.carousel',function(e){
    $('.homeslider .owl-item.active video').get(0).play();
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
		$('.headerSec .navPanel').addClass('fixed-nav');
    if (st > lastScrollTop && st > navbarHeight){
        // Scroll Down
        
    } else {
        // Scroll Up
        if(st + $(window).height() < $(document).height()) {
        
        }
    }
    if(st < 150){
		
		$('#back-to-top').hide();
		$('.headerSec .navPanel').removeClass('fixed-nav');
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
$('.heroesatwork').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:false,
	autoplaytimeout:1500,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
	autoplayHoverPause: true,
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
  })
	 $('#other-ventures').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		autoplay: false,
		 autoplayTimeout: 2500,
		 navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		 autoplayHoverPause: false,
		responsive: {
		  0: {
			items: 2,
			nav: true,
			dots:false
		  },
		  600: {
			items: 3,
			nav: true,
			dots:false
		  },
		  1000: {
			items: 7,
			nav: true,
			dots:false,
			margin: 20
		  }
		}
	  })
	  
 /*Affiliates Selector Domestic*/
    var $btns = $('.btn-stories').click(function () {
        if (this.id == 'all') {
            $('#parent-stories > div').fadeIn(450).addClass('animated zoomIn');
        } else {
            var $el = $('.' + this.id).fadeIn(450).addClass('animated zoomIn');
            $('#parent-stories > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })

$('.modal').on('hide.bs.modal', function(e) {    
    var $if = $(e.delegateTarget).find('iframe');
    var src = $if.attr("src");
    $if.attr("src", '');
    $if.attr("src", src);
});


})

$('#nav-button').click(function() {
	$('#nav-button span').toggleClass('menu-close')
	$('#nav-button').toggleClass('active');
	$('.menu-overlay').toggleClass('d-none d-block');
	$('#mute').toggleClass('d-block d-none');
	$('#back-to-top').toggleClass('d-none');
});

$('.menu-overlay').click(function() {
	$('.menu-overlay').toggleClass('d-block d-none');
	$('#nav-button').toggleClass('active');
	$('#nav-button span').toggleClass('menu-close');
	$('.navbar-collapse').toggleClass('show');
});

//For Home alert
$(window).on("load", function () {
	$("#reradisclaimer").modal("show");
	$("#westernheights-popup").modal("show");
  $("#reradisclaimer").modal("show");
  $("#westernheights-popup").modal("show");
});