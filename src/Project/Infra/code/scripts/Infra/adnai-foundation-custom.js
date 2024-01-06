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
    if (st > lastScrollTop && st > navbarHeight){
        // Scroll Down
        
    } else {
        // Scroll Up
        if(st + $(window).height() < $(document).height()) {
        
        }
    }
    if(st < 150){
		
		$('#back-to-top').hide();
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
	loop: false,
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
  }),
  $('.projects-carousel').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:true,
	autoplaytimeout:1500,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
	autoplayHoverPause: true,
	responsive: {
	  0: {
		items: 1
	  },
	  600: {
		items: 2
	  },
	  1000: {
		items: 3
	  }
	}
  }),
  $('.map-gallery').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:false,
	autoplaytimeout:1500,
	autoHeight:true,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
	autoplayHoverPause: true,
	responsive: {
	  0: {
		items: 1
	  },
	  600: {
		items: 2
	  },
	  1000: {
		items: 3
	  }
	}
  }),
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
	  }),
	  $(".popup").magnificPopup({
    type: "image",
    removalDelay: 160,
    preloader: false,
    fixedContentPos: true,
    gallery: {
      enabled: true
    }
  });
	  
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

$('.ProjectAssetsDataInner').hide();
$('#DefaultData').show();
$('#DefaultData2').show();	 
	$('#buss-owl').owlCarousel({
		items : 2,
		itemsDesktop : [1000,2], 
        itemsDesktopSmall : [900,2],
		itemsTablet: [640,1],
		pagination:true,
		
	});

$('.pointer').click(function(){
   $('.tab-panea').hide();		
   $('.ProjectAssetsDataInner').hide();
   var getTabId = $(this).attr('rel');
   $('#'+getTabId).show();
   $(".indiaMap a").attr("class", "pointer");
   $(this).attr("class", "active");
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

$( '.scroll-down a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 70
  }, '600' );
  e.preventDefault();
});



function filterSelection(c) {
  var x, i;
  x = document.getElementsByClassName("column");
  for (i = 0; i < x.length; i++) {
    w3RemoveClass(x[i], "show animated fadeInUp");
    if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show animated fadeInUp");
  }
}

function w3AddClass(element, name) {
  var i, arr1, arr2;
  arr1 = element.className.split(" ");
  arr2 = name.split(" ");
  for (i = 0; i < arr2.length; i++) {
    if (arr1.indexOf(arr2[i]) == -1) {element.className += " " + arr2[i];}
  }
}

function w3RemoveClass(element, name) {
  var i, arr1, arr2;
  arr1 = element.className.split(" ");
  arr2 = name.split(" ");
  for (i = 0; i < arr2.length; i++) {
    while (arr1.indexOf(arr2[i]) > -1) {
      arr1.splice(arr1.indexOf(arr2[i]), 1);     
    }
  }
  element.className = arr1.join(" ");
}


// Add active class to the current button (highlight it)
var btnContainer = document.getElementById("myBtnContainer");
var btns = btnContainer.getElementsByClassName("btn");
for (var i = 0; i < btns.length; i++) {
  btns[i].addEventListener("click", function(){
    var current = document.getElementsByClassName("active");
    current[0].className = current[0].className.replace(" active", "");
    this.className += " active";
  });
}