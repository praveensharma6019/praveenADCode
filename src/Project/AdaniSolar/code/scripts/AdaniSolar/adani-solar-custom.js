// $('.modal').on('shown.bs.modal', function () {
  // $('.modal video')[0].play();
// })
$('.modal').on('hidden.bs.modal', function () {
  $('.modal video')[0].pause();
 var src=$("#360v").attr('src');;
		
		url = src.substring(0, src.lastIndexOf("?"))
		$('#360v').attr('src',url); 
		$('#360v').attr('src',src);  
})

$(window).scroll(function() {
  
  // get the variable of how far we've scrolled from the top
  var offset = $(window).scrollTop();
	offset     = offset * 0.2;

  // add css transform with the offset variable
  $('.scroll-icon img').css({
    '-moz-transform': 'rotate(' + offset + 'deg)',
    '-webkit-transform': 'rotate(' + offset + 'deg)',
    '-o-transform': 'rotate(' + offset + 'deg)',
    '-ms-transform': 'rotate(' + offset + 'deg)',
    'transform': 'rotate(' + offset + 'deg)',
  });
  
  // add css transform with the offset variable
  $('.product-tab .floating-element:before').css({
    '-moz-transform': 'rotate(' + offset + 'deg)',
    '-webkit-transform': 'rotate(' + offset + 'deg)',
    '-o-transform': 'rotate(' + offset + 'deg)',
    '-ms-transform': 'rotate(' + offset + 'deg)',
    'transform': 'rotate(' + offset + 'deg)',
  });
  
});

$('.button-active').on('click', function(){
   $('.left-anim-wire').toggleClass('wire-left');
   setTimeout(function(){ 
   $('.right-anim-wire').toggleClass('wire-right'); 
   },00);
   setTimeout(function(){
	    $('.cls-405, .cls-401, .cls-373, .cls-371, .cls-368, .cls-358, .cls-360').toggleClass('window-highligh');
		$('.cls-334, .cls-331, .cls-332, .cls-333, .cls-334, .cls-335').toggleClass('d-none');
   },500);
  
});

$('.button-active').on('click', function(){
   $('.button-active').toggleClass('button-ac');
});
$('.button-active').on('click', function(){
   
});

$('.messi-wrapper').on('click', function(){
	$('.messi-wrapper').addClass('d-none');
});
   
$(function() {
  $('#btn-mrelease').change(function(){
	  
    $('.media-r').hide();
	$('.m-loadMore').hide();
    $('#' + $(this).val()).show();
	 
	});
}); 
$('#location-select').change(function() {   
if($(this).children("option:selected").val() == "usaoffice")
	{
	$('#india-marketing').hide();
	$('#india-office').hide();
	}
else{
	$('#usa-office').hide();
	}
});

$( '.scroll-arrow a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 50
  }, '600' );
  e.preventDefault();
});

 $(window).scroll(function(){
    if ($(this).scrollTop() > 200) {
    } else {
       $('.headerSec .fixed-top').removeClass('sticky-header');
    }
});

$('#location-select').change(function() {   
if($(this).children("option:selected").val() == "indiaoffice")
	{
	
	
	$('#india-marketing').show();
	$('#india-office').show();
	}
else{
	$('#usa-office').show();
	}
});

var $usa = $('#usa-office'),
	$usaTooltip = $('#usa-office-tooltip');

$usa.on('mousemove', showTooltip);
$usaTooltip.on('mouseleave', hideTooltip);

var $indiamarketing = $('#india-marketing'),
	$indiamarketingTooltip = $('#india-marketing-tooltip');

$indiamarketing.on('mousemove', showTooltip)
$indiamarketingTooltip.on('mouseleave', hideTooltip);

var $indiaoffice = $('#india-office'),
	$indiaofficeTooltip = $('#india-office-tooltip');

$indiaoffice.on('mousemove', showTooltip);
$indiaofficeTooltip.on('mouseleave', hideTooltip);

function showTooltip(event) {
	var el = event.currentTarget.id,
		selector = "#" + el + "-tooltip",
		tooltip = $(selector);
			
	var top = event.pageY,
		left = event.pageX,
		tooltipWidth = tooltip.outerWidth(),
		tooltipHeight = tooltip.outerHeight();
	
	var positionX = (left - (tooltipWidth / 2)) + "px",
		positionY = (top - (tooltipHeight + 15)) + "px";
		
	tooltip.css({
		"left": positionX, 
		"top": positionY, 
		"display": "block",
	});
}

function hideTooltip() {
	$('.contact-tooltip').hide();
}

$('.home-miletone .row').each(function(){
  var LiN = $(this).find('.col-lg-3').length;
  if( LiN > 4){    
    $('.col-lg-3', this).eq(3).nextAll().hide().addClass('toggleable');
    $(this).append('<div class="more w-100 text-center">View more<em class="fa fa-chevron-down ml-2"></em></div>');    
  }  
});
$('.home-miletone .row').on('click','.more', function(){
  if( $(this).hasClass('less') ){    
    $(this).text('View More').append('<em class="fa fa-chevron-down ml-2"></em>').removeClass('less');    
  }else{
    $(this).text('View less').append('<em class="fa fa-chevron-up ml-2"></em>').addClass('less'); 
  }
  $(this).siblings('.col-lg-3.toggleable').slideToggle(); 
}); 

$('.about-miletone .row').each(function(){
  var LiN = $(this).find('.col-lg-3').length;
  if( LiN > 4){    
    $('.col-lg-3', this).eq(3).nextAll().hide().addClass('toggleable');
    $(this).append('<div class="more w-100 text-center">View more<em class="fa fa-chevron-down ml-2"></em></div>');    
  }  
});
$('.about-miletone .row').on('click','.more', function(){
  if( $(this).hasClass('less') ){    
    $(this).text('View More').append('<em class="fa fa-chevron-down ml-2"></em>').removeClass('less');    
  }else{
    $(this).text('View less').append('<em class="fa fa-chevron-up ml-2"></em>').addClass('less'); 
  }
  $(this).siblings('.col-lg-3.toggleable').slideToggle(); 
}); 
$(function() {
  $('#location-select').change(function(){
    $('.map-dot').hide();
    $('.' + $(this).val()).show().addClass('active'); 
  });
});
$('.map-dot a').hover(
  function(){ 
    $(".map-dot").addClass('inactive').removeClass('active');
  }
);

$(".loadmore-content p").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
	$(".loadMore").toggleClass("d-block d-none");
    $(".loadmore-content p").toggleClass("d-none");
	$("p.loadMore").text("").show(1000);
	$(".arrow.loadMore").text("Read more ").show(1000);
	  $('<i class="fa fa-chevron-down"/>').appendTo('.arrow.loadMore');
	if($(".loadmore-content p:hidden").length == 0) {
	  $(".arrow.loadMore").text("Read less ").show(1000);
	  $(' <i class="fa fa-chevron-up"/>').appendTo('.arrow.loadMore');
	  $(".arrow.loadMore").show(1000);
    }
	
  });  

$('.product-tabs .nav-tabs li').click(function(){
  $('.product-tabs .tab-content .tab-pane').addClass('');
});

$('.product-tabs .nav-tabs li').click(function(){
  $('.product-tabs .tab-content .text-right').addClass('animated zoomIn');
});

$('.product-nav li').click(function(){
  $('.tab-content .text-lg-right').addClass('animated zoomIn');
});

$(function() {
  $('#power-select').change(function(){
    $('.power-stats').hide();
    $('.' + $(this).val()).show().addClass('animated fadeIn');
  });
}); 
/*
	Load more content with jQuery - May 21, 2013
	(c) 2013 @ElmahdiMahmoud
*/   

	var $btns = $('.features-btn').click(function() {
  if (this.id == 'all') {
    $('#parent > img').fadeIn(450);
  } else {
    var $el = $('.' + this.id).fadeIn(450);
    $('#parent > img').not($el).hide();
  }
  $btns.removeClass('active');
  $(this).addClass('active');
})

$(document).ready(function(){
	
	var owl = $('.homeslider');
  owl.owlCarousel({
      loop:true,
      margin:0,
	  lazyLoad: true,
      nav:false,
      dots:true,
	  autoHeight:true,
	  autoplay: true,
	  autoplayTimeout: 21700,
	  slideSpeed: 500,
	  lazyload:true,
      items:1
  })
  owl.on('translate.owl.carousel',function(e){
    $('.homeslider .owl-item video').each(function(){
      $(this).get(0).pause();
	  $(this).get(0).currentTime = 0;
    });
  });
  $('.homeslider .owl-item .item').each(function(){
      var attr = $(this).attr('data-videosrc');
      if (typeof attr !== typeof undefined && attr !== false) {
        var videosrc = $(this).attr('data-videosrc');
        $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.homeslider .owl-item.active video');
      }
    });
  owl.on('translated.owl.carousel',function(e){
	$('.homeslider .owl-item.active video').get(0).play();
  });
  
  
	
	// $(".homeslider").carousel({ interval: false}); // this prevents the auto-loop
    // var videos = document.querySelectorAll("video");
    // videos.forEach(function(e) {
        // e.addEventListener('ended', myHandler, false);
    // }); 

    // function myHandler(e) {
        // $(".homeslider").carousel('next');
    // }
$(".popup").magnificPopup({
    type: "image",
    removalDelay: 160,
    preloader: false,
    fixedContentPos: true,
    gallery: {
      enabled: true
    }
  });
	
	$('.events-tile').owlCarousel({
	loop: true,
	margin: 10,
	responsiveClass: true,
	autoplayTimeout: 3000,
	autoplay: true,
	nav:false,
	dots:true,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	responsive: {
	  0: {
		items: 2,
		nav:false
	  },
	  600: {
		items: 2
	  },
	  1000: {
		items: 2
	  }
	}
  })
   $('.events-item').owlCarousel({
	loop: true,
	margin: 10,
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
  
	  var list = $(".business .col-lg-3");
      var numToShow = 3;
      var button = $("#loadMore");
      var numInList = list.length;
      list.hide();
      if (numInList > numToShow) {
        button.show();
      }
      list.slice(0, numToShow).show();

      button.click(function(){
          var showing = list.filter(':visible').length;
          list.slice(showing - 1, showing + numToShow).fadeIn();
          var nowShowing = list.filter(':visible').length;
          if (nowShowing >= numInList) {
            button.hide();
          }
      });
var $owl = $('');

$owl.children().each( function( index ) {
  $(this).attr( 'data-position', index );
});



$owl.owlCarousel({
  center: true,
  loop: true,
  items: 3,
  nav:true,
  navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
		responsive: {
            0: {
                items: 1,
				nav:true
            },
            600: {
                items: 3,
				nav:true
            },
            1000: {
                items: 3,
				nav:true
            }
        }
});

$(document).on('click', '.owl-item>div', function() {
  $owl.trigger('to.owl.carousel', $(this).data( 'position' ) );
});
$('#pop-gallery').owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        dots:true,
		autoplay: true,
        autoplayTimeout: 2500,
        responsiveClass: true,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        slideSpeed: 1000,
        responsive: {
            0: {
                items: 1,
				nav:true
            },
            600: {
                items: 3,
				nav:true
            },
            1000: {
                items: 3,
				nav:true
            }
        }
    });
	
  
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
  $('.sunplugged-carousel').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	autoplayTimeout: 3000,
	autoplay: false,
	nav:true,
	dots:false,
	autoHeight:true,
	navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
	responsive: {
	  0: {
		items: 1,
		nav:false
	  },
	  600: {
		items: 3
	  },
	  1000: {
		items: 3
	  },
	   1200: {
		items: 4
	  }
	}
  })  
  
  $('.double-slide').owlCarousel({
	loop: true,
	margin: 0,
	responsiveClass: true,
	autoplayTimeout: 3000,
	autoplay: false,
	autoHeight:true,
	nav:true,
	dots:false,
	navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
	responsive: {
	  0: {
		items: 1,
		nav:false
	  },
	  600: {
		items: 2
	  },
	  1000: {
		items: 2
	  }
	}
  })  
});

  $('.topMenu .dropdown').click(function(){
$(this).toggleClass('active'); 
});	
$('.topMenu .dropdown a').click(function(){
// $('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');
});
$('.topMenu .dropdown').click(function(){$(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');});
  
 $('[data-toggle="tooltip"]').tooltip();
/*Scroll*/
$('.click-scroll a').click(function(e){
  e.preventDefault();
  var target = $($(this).attr('href'));
  if(target.length){
    var scrollTo = target.offset().top;
    $('body, html').animate({scrollTop: scrollTo+'px'}, 800);
  }
});
$(function() {
  $('#investor-select select').change(function(){
    $('.investor-m-block').hide();
	$('.inv-tabs-query').hide();
	$('.tab-inv').hide();
    $('.' + $(this).val()).show();
  });
}); 
$(document).ready(function(){
	$('body').on('hidden.bs.modal', '.modal', function () {
$('video').trigger('pause');
});
 $(".modal .modal-body video").autoplay = false;
 
$('body').on('hidden.bs.modal', '.modal', function () {
$('video').trigger('pause');
});
 $('body').on('hidden.bs.modal', '.modal', function () {
$('video').trigger('pause');
});
 $(".view-all video").each(function(){
    $(this).get(0).pause();
});
/*
$('.searchIcon').click(function() {
    $('.search-main').toggleClass("d-block").css('opacity', '1');
	$('.searchIcon').css('position','absolute').css('z-index', '9');
    // Alternative animation for example
    // slideToggle("fast");
  });*/
	
$('.viewall').click(function() {
	$('.ft-business li').removeClass('d-none');
	$('.viewall').addClass('d-none');
});
var y = $(window).scrollTop();
        y = y + 150;
	$('.sitemap-link').click(function() {
	$('.ft-mobilemenu').toggleClass('d-none').animate({ scrollTop: y },800);
	$('html, body').animate({scrollTop:$(document).height()}, 'slow');
	$('.sitemap-link').toggleClass('sitemap-link-active');
	});
	
// Fakes the loading setting a timeout
    setTimeout(function() {
        $('body').addClass('loaded');
    }, 1500);		
	
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
		$('.navPanel').addClass('sticky-header').css('top','0px');;
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
	loop: true,
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
			  
	$('.certificate-block').owlCarousel({
                loop: true,
                margin: 80,
				autoplaytimeout:2000,
				animateOut: 'fadeOutLeft',
				animateIn: 'fadeInRight',
				slideSpeed: 1500,
				autoplay:false,
                responsiveClass: true,
				navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
                responsive: {
                  0: {
                    items: 3,
                    nav: true,
					dots:false,
					margin:10
                  },
				  
				  300: {
                    items: 3,
                    nav: true,
					dots:false,
					margin:10
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
              })
			  
			  // $('.home-miletone').owlCarousel({
                // loop: true,
                // margin: 30,
				// autoplaytimeout:1000,
				// nav:true,
				// dots:false,
				// autoplay:false,
                // responsiveClass: true,
				// navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
                // responsive: {
                  // 0: {
                    // items: 2
                  // },
				  
				  // 300: {
                    // items: 1
                  // },
				  // 420: {
                    // items: 1
                  // },
                  // 576: {
                    // items: 2
                  // },
				  // 768: {
                    // items: 3
					// },
                  // 1000: {
                    // items: 4
                  // }
                // }
              // })
			  
			   $('.channel-p').owlCarousel({
                loop: true,
                margin: 30,
				autoplaytimeout:1000,
				autoplay:true,
                responsiveClass: true,
				navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
                responsive: {
                  0: {
                    items: 2,
                    nav: false,
					dots:false
                  },
				  
				  300: {
                    items: 1,
                    nav: false,
					dots:false
                  },
				  420: {
                    items: 1,
                    nav: false,
					dots:false
                  },
                  576: {
                    items: 2,
                    nav: false,
					dots:true
                  },
				  768: {
                    items: 3,
                    nav: false,
					dots:false
                  },
                  1000: {
                    items: 4,
                    nav: false,
                    loop: false,
					dots:false
                  }
                }
              })
			  
			   $('.epc-solution').owlCarousel({
                loop: true,
                margin: 30,
				autoplaytimeout:1000,
				autoplay:true,
                responsiveClass: true,
				navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
                responsive: {
                  0: {
                    items: 2,
                    nav: true,
					dots:false
                  },
				  
				  300: {
                    items: 1,
                    nav: true,
					dots:false
                  },
				  420: {
                    items: 1,
                    nav: true,
					dots:false
                  },
                  576: {
                    items: 2,
                    nav: true,
					dots:true
                  },
				  768: {
                    items: 3,
                    nav: true,
					dots:false
                  },
                  1000: {
                    items: 4,
                    nav: true,
					dots:false
                  }
                }
              })
  
	/* Menu on mobile devices */
	
	$('#dismiss, .overlay, .overlay-top').on('click', function () {
		$('#sideNav').removeClass('active');
		 $('#topMenu').removeClass('active');
		 $('.overlay-top').removeClass('menu-overlay');
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
$('.overlay-top').toggleClass('menu-overlay');		
	});

})

$('.nav-tabs li').on('click', function () {
		$('.tab-content .tab-pane').addClass('animated fadeIn');   
	});
	

 $( '.scroll-down a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 70
  }, '600' );
  e.preventDefault();
});