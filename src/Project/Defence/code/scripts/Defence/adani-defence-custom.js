var recaptcha1;
var onloadCallback = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6LdCg7kUAAAAAKx4D0EOc8qE4e8Hak0wT5jG_ZIa', //Replace this with your Site key
        'theme': 'light'
    });


};
var recaptcha2;
var registerCallBack = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha2 = grecaptcha.render('recaptcha2', {
        'sitekey': '6LdCg7kUAAAAAKx4D0EOc8qE4e8Hak0wT5jG_ZIa', //Replace this with your Site key
        'theme': 'light'
    });


};

$('.modal').on('shown.bs.modal', function () {
  $('.modal video')[0].play();
})
$('.modal.video').on('hidden.bs.modal', function () {
  $('.modal video')[0].pause();
})

$(document).ready(function(){
	
 /**/
    var $btns = $('.vfilter-btn').click(function () {
        if (this.id == 'all') {
            $('#parent > div').fadeIn(450);
        } else {
            var $el = $('.' + this.id).fadeIn(450);
            $('#parent > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })
	
	
    $('#other-ventures').owlCarousel({
	loop: true,
	margin: 30,
	autoplaytimeout:100,
	animateOut: 'fadeOutLeft',
	animateIn: 'fadeInRight',
	slideSpeed: 100,
	autoplay:false,
	responsiveClass: true,
	responsive: {
	  0: {
		items: 2,
		nav: true,
		dots:false
	  },
	  
	  300: {
		items: 2,
		nav: true,
		dots:false
	  },
	  420: {
		items: 3,
		nav: true,
		dots:false
	  },
	  576: {
		items: 4,
		nav: true,
		dots:true
	  },
	  768: {
		items: 4,
		nav: true,
		dots:false
	  },
	  1000: {
		items: 7,
		nav: true,
		loop: false,
		dots:false
	  }
	}
  }),
/*
$('.searchIcon').click(function() {
    $('.search-main').toggleClass("d-block").css('opacity', '1');
	$('.searchIcon').css('position','absolute').css('z-index', '9');
    // Alternative animation for example
    // slideToggle("fast");
  });*/
  
   $('.modal .close').click(function(){
		 
		 var videoId=$(this).attr('Id');
		 CloseModal2(videoId);
		 
	 });
	
$('.viewall').click(function() {
	$('.ft-business li').removeClass('d-none');
	$('.viewall').addClass('d-none');
});
$('.sitemap-link').click(function() {
	$('.ft-mobilemenu').toggleClass('d-none d-block');
	$('html, body').animate({
                scrollTop: $(".sitemap-link").offset().top
                    - 100
            }, 1000);
});
	
// Fakes the loading setting a timeout
    setTimeout(function() {
        $('body').addClass('loaded');
    }, 1500);
	
	
/*Home Video Slider*/
	  var owl = $('.homeslider');
  owl.owlCarousel({
      loop:true,
      margin:0,
      nav:false,
      dots:true,
	  autoHeight:true,
	  autoplay:false,
	  lazyload:true,
	  autoplaytimeout:2000,
	slideSpeed: 1500,
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
        $(this).prepend('<video muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.homeslider .owl-item.active video').attr('autoplay',true).attr('loop',true);
      }
    });
  owl.on('translated.owl.carousel',function(e){
    $('.homeslider .owl-item.active video').get(0).play();
  });
		
	// $(".loadmore-content p").slice(0, 1).show();
  // $(".loadMore").on("click", function(e){
    // e.preventDefault();
    // $(".loadmore-content p").attr("style", "display: block !important;transition: ease display 0.5s;");
	// if($(".loadmore-content p:hidden").length == 0) {
      // $(".loadMore").text("").show(1000);
    // }
  // });  
  
  $(".exp-more").on("click", function(e){
    e.preventDefault();
    $(".loadmore-content p").attr("style", "display: block !important;transition: ease display 0.5s;");
	if($(".loadmore-content p:hidden").length == 0) {
      $(".loadMore").text("").show(1000);
    }
  });  
  
$('#footerArrow').click(function() {
	$('.footerPanel2 .mobile-none').toggleClass('d-none');
	$('.footerPanel2 .footerpanel-1').toggle(100);
	$('.footerPanel2 .txt-center').toggle(100);
	$('.footerPanel2 .ft-submenu').toggleClass('col-lg-12');
});
	
if ( $(window).width() <= 992 ) {
	$("#loadMore").text('Load more');
    $(function () {
    $(".business .col-lg-3").slice(0, 4).show();
	var cnt=0;
    $("#loadMore").on('click', function (e) {
        cnt=cnt+1;
        $(".business .col-lg-3:hidden").slice(0, 4).slideDown('slow',function(){
			if (cnt==2) {
            $("#loadMore");
            $("#loadMore").text('Explore more');
			$("#loadMore").attr('href','businesses.html');
        }
		});
    });
});
  }
  else{
	  $("#loadMore").attr('href','businesses.html');
	  // Hide Header on scroll down
var didScroll;
var lastScrollTop = 0;
var delta = 5;
var navbarHeight = 100;

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
    if (st > lastScrollTop && st > navbarHeight){
        // Scroll Down
		$('#back-to-top').fadeIn();
        $('.headerTopBar').removeClass('nav-down');
		$('.navPanel').addClass('sticky-header').css('top','0px');
    } else {
        // Scroll Up
        if(st + $(window).height() < $(document).height()) {
            $('.headerTopBar').addClass('nav-down');
			$('.navPanel').css('top','0px');
        }
    }
    if(st==0){
		$('.headerTopBar').removeClass('nav-down');
		$('.navPanel').removeClass('sticky-header');
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
	  
  }
  $('.three-slides').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:true,
	autoplaytimeout:800,
	animateOut: 'fadeOutLeft',
	animateIn: 'fadeInRight',
	slideSpeed: 100,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	autoplayHoverPause: true,
	responsive: {
	  0: {
		items: 3
	  },
	  600: {
		items: 4
	  },
	  1000: {
		items: 3
	  }
	}
  }),
  $('.capabilities').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:true,
	autoplaytimeout:800,
	animateOut: 'fadeOutLeft',
	animateIn: 'fadeInRight',
	slideSpeed: 100,
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
  
  $('#sustanibility').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:false,
	autoplaytimeout:2000,
	animateOut: 'fadeOutLeft',
	animateIn: 'fadeInRight',
	slideSpeed: 1500,
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
$('#about-banner').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	autoplayTimeout: 3000,
	animateOut: 'fadeInLeft',
	animateIn: 'fadeOut',
	autoplay: true,
	nav:false,
	dots:false,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	responsive: {
	  0: {
		items: 1
	  },
	  960: {
		items: 1
	  },
	  1100: {
		items: 1
	  }
	}
  })
  
    $('#resources').owlCarousel({
	loop: false,
	margin: 0,
	responsiveClass: true,
	nav:false,
	dots:true,
	autoplay:true,
	autoplaytimeout:2000,
	animateOut: 'fadeOutLeft',
	animateIn: 'fadeInRight',
	slideSpeed: 1500,
	autoplayHoverPause: true,
	responsive: {
	  0: {
		items: 2
	  },
	  600: {
		items: 2
	  },
	  1000: {
		items: 2
	  }
	}
  })
  
  $('.single-item').owlCarousel({
	loop: false,
	margin: 0,
	responsiveClass: true,
	nav:false,
	dots:true,
	autoplay:true,
	autoplaytimeout:2000,
	animateOut: 'fadeOutLeft',
	animateIn: 'fadeInRight',
	slideSpeed: 1500,
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
  
  $('.three-item').owlCarousel({
	loop: false,
	margin: 20,
	responsiveClass: true,
	autoplay:true,
	autoplaytimeout:1000,
	animateOut: 'fadeOutLeft',
	animateIn: 'fadeInRight',
	slideSpeed: 1500,
	autoplayHoverPause: true,
	navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
	responsive: {
	  0: {
		items: 1,
		nav:false,
		dots:false
	  },
	  600: {
		items: 2,
		nav:false,
		dots:false
	  },
	  1000: {
		items: 3,
		nav:false,
		dots:false
	  }
	}
  })
$('.single-slide').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	autoplayTimeout: 3000,
	autoplay: true,
	nav:true,
	dots:false,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	responsive: {
	  0: {
		items: 1,
		nav:false
	  },
	  600: {
		items: 1
	  },
	  1000: {
		items: 1
	  }
	}
  })
$('.b-model-carousel').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	autoplayTimeout: 3000,
	autoplay: true,
	nav:false,
	dots:false,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	responsive: {
	  0: {
		items: 1,
		nav:false
	  },
	  600: {
		items: 1
	  },
	  1000: {
		items: 1
	  }
	}
  })
  $('.business-carousel').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	autoplayTimeout: 3000,
	autoplay: true,
	nav:true,
	dots:false,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	responsive: {
	  0: {
		items: 1,
		nav:false
	  },
	  600: {
		items: 1
	  },
	  1000: {
		items: 1
	  }
	}
  })

    //for newsroom
    $(function () {
        $('#btn-mrelease').change(function () {
            $('.media-r').hide();
            $('.m-loadMore').hide();
            $('#' + $(this).val()).show();
        });
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
		 $('.overlay-top').toggleClass('active');
	   
	});
	
$('[data-toggle="tooltip"]').tooltip(); 

$(".loadmore-content p").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
	$(".loadMore").toggleClass("d-block d-none");
    $(".loadmore-content p").toggleClass("d-none");
    $(".loadmore-content ul").toggleClass("d-none");
	$("p.loadMore").text("").show(1000);
	$(".arrow.loadMore").text("Read more").show(1000);
	  $('<i class="fa fa-chevron-down"/>').appendTo('.arrow.loadMore');
	if($(".loadmore-content p:hidden").length == 0) {
	  $(".arrow.loadMore").text("Read less ").show(1000);
	  $(' <i class="fa fa-chevron-up"/>').appendTo('.arrow.loadMore');
	  $(".arrow.loadMore").show(1000);
    }
	
  });  

})
 function CloseModal2(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
}






$('#adaniaerospacepark').click(function () {

    var c = window.location.href;

    c += ('#AdaniAerospacePark');


   


});


 /*Product Filter*/
    var $btns = $('.ul-products-btn').click(function () {
        if (this.id == 'all') {
            $('#product-parent > div').fadeIn(450).addClass('animated zoomIn');
        } else {
            var $el = $('.' + this.id).fadeIn(450).addClass('animated zoomIn');
            $('#product-parent > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })
	var y = $(window).scrollTop();
        y = y +0;
$(".exp-capabilities").on("click", function(e){
	$('.explore-navtabs').toggleClass('d-none').animate({ scrollTop: y },100);
	// $('html, body').animate({scrollTop:$(document).height()}, 'slow');
		
})


$('.topMenu .dropdown').click(function(){	 	 
 	$(this).toggleClass('active');	 	 
 	});	
$('.topMenu .dropdown a').click(function(){	 	 
 	$('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');	 	 
 	});
$('.topMenu .dropdown').click(function(){	 	 
 	$(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');	 	 
 	});	
