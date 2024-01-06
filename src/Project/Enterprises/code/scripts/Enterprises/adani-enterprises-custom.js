(function() {

  'use strict';

  // define variables
  var items = document.querySelectorAll(".timeline li");

  // check if an element is in viewport
  // http://stackoverflow.com/questions/123999/how-to-tell-if-a-dom-element-is-visible-in-the-current-viewport
  function isElementInViewport(el) {
    var rect = el.getBoundingClientRect();
    return (
      rect.top >= 0 &&
      rect.left >= 0 &&
      rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
      rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
  }

  function callbackFunc() {
    for (var i = 0; i < items.length; i++) {
      if (isElementInViewport(items[i])) {
        items[i].classList.add("in-view");
      }
    }
  }

  // listen for events
  window.addEventListener("load", callbackFunc);
  window.addEventListener("resize", callbackFunc);
  window.addEventListener("scroll", callbackFunc);

})();


$(document).ready(function() {	
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

$('.timeline-carousel').owlCarousel({
                loop: false,
                margin: 0,
                responsiveClass: true,
				autoplayTimeout: 3000,
				touchDrag:false,
				mouseDrag:false,
				autoplay: false,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
                responsive: {
                  0: {
                    items: 1,
					nav:false
                  },
                  600: {
                    items: 2
                  },
                  1000: {
                    items: 3
                  }
                }
              })
			  
			  //home page text update on sustainability Tile
			  $($(".sustainabilityHomeTile h3")[1]).text("Social");
			  

});


 $( '.click-scroll a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 70
  }, '600' );
  e.preventDefault();
});
$(function() {
  $('#investor-select select').change(function(){
    $('.investor-m-block').hide();
	$('.inv-tabs-query').hide();
	$('.tab-inv').hide();
    $('.' + $(this).val()).show();
  });
});

$(window).scroll(function(){
    if ($(this).scrollTop() > 200) {
    } else {
       $('.headerSec .fixed-top').removeClass('sticky-header');
    }
});

 function CloseModal(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
} 

$('body').on('hidden.bs.modal', '.modal', function () {
$('video').trigger('pause');
});

$(".view-all video").each(function(){
    $(this).get(0).pause();
});
 
 
$(document).ready(function () {
	$('.modal .close').click(function(){
		 
		 var videoId=$(this).attr('Id');
		 CloseModal(videoId);
		 
	 }); 
	
	var y = $(window).scrollTop();
        y = y + 150;
	$('.sitemap-link').click(function() {
	$('.ft-mobilemenu').toggleClass('d-none').animate({ scrollTop: y },800);
	$('html, body').animate({scrollTop:$(document).height()}, 'slow');
	$('.sitemap-link').toggleClass('sitemap-link-active');
	});

	$('body').on('hidden.bs.modal', '.modal', function () {
$('video').trigger('pause');
});
$('.topMenu .dropdown').click(function(){
$(this).toggleClass('active');
});	
$('.topMenu .dropdown a').click(function(){
$('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');});
$('.topMenu .dropdown').click(function(){
	$(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');});


 $( '.scroll-down a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 70
  }, '600' );
  e.preventDefault();
});

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
	  $('.infrastructure-block').owlCarousel({
	loop: true,
	margin: 10,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:false,
	autoplaytimeout:1500,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
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
	  
	  $('.single-carousel').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		autoplay: false,
		 autoplayTimeout: 2500,
		 navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		 autoplayHoverPause: false,
		responsive: {
		  0: {
			items: 1,
			nav: true,
			dots:false
		  },
		  600: {
			items: 1,
			nav: true,
			dots:false
		  },
		  1000: {
			items: 1,
			nav: true,
			dots:false,
			margin: 10
		  }
		}
	  }),
	  
	  $('.covid-carousel').owlCarousel({
                loop: true,
                margin: 10,
				nav:false,
				autoplay:true,
				autoplayTimeout:5000,
				animateOut: 'fadeOut',
				animateIn: 'fadeIn',
				dots:true,
                responsiveClass: true,
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
	  
	  
	  
	  $('.case-study').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		autoplay: false,
		 autoplayTimeout: 2500,
		 navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		 autoplayHoverPause: false,
		responsive: {
		  0: {
			items: 1,
			nav: true,
			dots:false
		  },
		  600: {
			items: 1,
			nav: true,
			dots:false
		  },
		  1000: {
			items: 1,
			nav: true,
			dots:false,
			margin: 20
		  }
		}
	  })
	   $('.single-item').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		autoplay: true,
		 autoplayTimeout: 1500,
		 navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		 autoplayHoverPause: false,
		responsive: {
		  0: {
			items: 1,
			nav: true,
			dots:false
		  },
		  600: {
			items: 1,
			nav: true,
			dots:false
		  },
		  1000: {
			items: 1,
			nav: true,
			dots:false,
			margin: 20
		  }
		}
	  })
	  
	  $('#blog-more').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		autoplay: false,
		 autoplayTimeout: 2500,
		 navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		 autoplayHoverPause: false,
		responsive: {
		  0: {
			items: 1,
			nav: true,
			dots:false
		  },
		  600: {
			items: 2,
			nav: true,
			dots:false
		  },
		  1000: {
			items: 2,
			nav: true,
			dots:false,
			margin: 20
		  }
		}
	  })

    // Fakes the loading setting a timeout
    setTimeout(function () {
        $('body').addClass('loaded');
    }, 1500);


 var owl = $('.homeslider');
  owl.owlCarousel({
      loop:true,
      margin:0,
	  lazyLoad: true,
      nav:false,
      dots:true,
	  animateOut: 'fadeOut',
	  animateIn: 'fadeIn',
	  autoHeight:true,
	  autoplay: true,
	  autoplayTimeout: 6800,
		slideSpeed: 0,
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
	
	$(".homeslider").carousel({ interval: false}); // this prevents the auto-loop
    var videos = document.querySelectorAll("video");
    videos.forEach(function(e) {
        e.addEventListener('ended', myHandler, false);
    }); 

    function myHandler(e) {
        $(".homeslider").carousel('next');
    }
	
	 /*Inner Video Slider*/
    var owl = $('.innerv-carousel');
    owl.owlCarousel({
        loop: false,
        margin: 0,
        nav: false,
        dots: false,
        autoHeight: true,
        lazyload: true,
        items: 1
    })
    owl.on('translate.owl.carousel', function (e) {
        $('.innerv-carousel .owl-item video').each(function () {
            $(this).get(0).pause();
        });
    });
    $('.innerv-carousel .owl-item .item').each(function () {
        var attr = $(this).attr('data-videosrc');
        if (typeof attr !== typeof undefined && attr !== false) {
            var videosrc = $(this).attr('data-videosrc');
            $(this).prepend('<video muted><source src="' + videosrc + '" type="video/mp4"></video>');
            $('.innerv-carousel .owl-item.active video').attr('autoplay', true).attr('loop', true);
        }
    });
    owl.on('translated.owl.carousel', function (e) {
        $('.innerv-carousel .owl-item.active video').get(0).play();
    });

    // Hide Header on scroll down
    var didScroll;
    var lastScrollTop = 0;
    var delta = 5;
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

        // Make sure they scroll more than delta
        if (Math.abs(lastScrollTop - st) <= delta)
            return;

        // If they scrolled down and are past the navbar, add class .nav-up.
        // This is necessary so you never see what is "behind" the navbar.
        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down
            $('#back-to-top').fadeIn();
            $('.headerTopBar').removeClass('nav-down');
            $('.navPanel').addClass('sticky-header').css('top', '0px');;
        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {
                $('.headerTopBar').addClass('nav-down');
                $('.navPanel').css('top', '0px');
            }
        }
        if (st == 0) {
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

//for newsroom
$(function() {
  $('#btn-mrelease').change(function(){
    $('.media-r').hide();
	$('.m-loadMore').hide();
    $('#' + $(this).val()).show();
  });
}); 









    $('#footerArrow').click(function () {
        $('.footerPanel2 .mobile-none').toggleClass('d-none');
        $('.footerPanel2 .footerpanel-1').toggle(100);
        $('.footerPanel2 .txt-center').toggle(100);
        $('.footerPanel2 .ft-submenu').toggleClass('col-lg-12');
    });

	
	
	
	
	
    if ($(window).width() <= 768) {
        $("#loadMore").text('Load more');
        $(function () {
            $(".business .col-lg-3").slice(0, 4).show();
            var cnt = 0;
            $("#loadMore").on('click', function (e) {
                cnt = cnt + 1;
                $(".business .col-lg-3:hidden").slice(0, 4).slideDown('slow', function () {
                    if (cnt == 4) {
                        $("#loadMore");
                        $("#loadMore").text('Explore more');
                        $("#loadMore").attr('href', 'businesses');
                    }
                });
            });
        });
    }
    else {
        $("#loadMore").attr('href', 'businesses');
    }

    $('#sustanibility').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        nav: false,
        dots: false,
        autoplay: false,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1,
				nav:true
            },
            600: {
                items: 2,
				nav:true
            },
            1000: {
                items: 3,
		touchDrag: false,
    mouseDrag: false,
            }
        }
    })

    $('#resources').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        nav: false,
        dots: true,
        autoplay: true,
        autoplaytimeout: 2000,
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

    // $('#other-ventures').owlCarousel({
        // loop: true,
        // margin: 30,
        // autoplaytimeout: 2000,
        // animateOut: 'fadeOutLeft',
        // animateIn: 'fadeInRight',
        // slideSpeed: 1500,
        // autoplay: true,
        // responsiveClass: true,
        // responsive: {
            // 0: {
                // items: 2,
                // nav: true,
                // dots: false
            // },

            // 300: {
                // items: 2,
                // nav: false,
                // dots: false
            // },
            // 420: {
                // items: 3,
                // nav: false,
                // dots: false
            // },
            // 576: {
                // items: 4,
                // nav: false,
                // dots: true
            // },
            // 768: {
                // items: 4,
                // nav: false,
                // dots: true
            // },
            // 1000: {
                // items: 6,
                // nav: false,
                // loop: false
            // }
        // }
    // })
	

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
	   $('#about-banner').owlCarousel({
		loop: true,
		margin: 0,
		responsiveClass: true,
		autoplayTimeout: 2000,
		autoplay: true,
		autoHeight:true,
		mouseDrag: true,
		navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		responsive: {
		  0: {
			items: 1,
			nav:false,
			dots:false,
		  },
		  960: {
			items: 1,
			nav:false,
			dots:false,
		  },
		  1100: {
			items: 1,
			nav:true,
			dots:false,
		  }
		}
	  })
	  $('#pop-gallery').owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
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

	$(".popup").magnificPopup({
     type: "image",
     removalDelay: 160,
     preloader: false,
     fixedContentPos: true,
     gallery: {
     enabled: true
     }
   });
   

$(function() {
  $('#btn-mbusiness').change(function(){

	$("#business-parent .col-sm-6").hide();
    $('#business-parent .' + $(this).val()).show();
	
  });
}); 




   /* Menu on mobile devices */

    $('#dismiss, .overlay, .overlay-top').on('click', function () {
        $('#sideNav').removeClass('active');
        $('.overlay, .overlay-top').removeClass('active');
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

// $('[data-toggle="tooltip"]').tooltip();

$(".loadmore-content p").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
	$(".loadMore").toggleClass("d-block d-none");
    $(".loadmore-content p").toggleClass("d-none");
	$(".loadmore-content ul").toggleClass("d-none");
	$(".loadmore-content ul li").toggleClass("d-none d-list");
	$("p.loadMore").text("").show(1000);
	$(".arrow.loadMore").text("Read more ").show(1000);
	  $('<i class="fa fa-chevron-down"/>').appendTo('.arrow.loadMore');
	if($(".loadmore-content p:hidden").length == 0) {
	  $(".arrow.loadMore").text("Read less ").show(1000);
	  $(' <i class="fa fa-chevron-up"/>').appendTo('.arrow.loadMore');
	  $(".arrow.loadMore").show(1000);
    }
	
  });    

    $('.viewall').click(function () {
        $('.ft-business li').removeClass('d-none');
        $('.viewall').addClass('d-none');
    });

  
  $('.nav-border-bottom li').click(function () {
        $('.d-lg-block #cont-tab-2 #DefaultData2').addClass('d-block');
    }); 
	
	$('#cont-tab-2 a').click(function(){
		$('.d-lg-block #cont-tab-2 #DefaultData2').removeClass('d-block');
	});
	
	
	
    // $("a.dropdown-item")
        // .on("mouseenter", function (e) {
            // $(".menu-thumb").children()[0].style.display = "none";
            // for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
                // if ($(".menu-thumb").children()[i].alt == $(e.currentTarget).text()) {
                    // $(".menu-thumb").children()[i].style.display = "block";
                // }
                // else {
                    // $(".menu-thumb").children()[i].style.display = "none";
                // }
            // }
        // })
        // .on("mouseleave", function (e) {
            // for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
                // $(".menu-thumb").children()[i].style.display = "none";
            // }
            // $(".menu-thumb").children()[0].style.display = "block";
        // });
		
		$('.single-item').owlCarousel({
	loop: false,
	margin: 10,
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
		items: 1,
		margin: 10
	  },
	  600: {
		items: 1,
		margin: 10
	  },
	  1000: {
		items: 1,
		margin: 10
	  }
	}
  })
  $('.three-item').owlCarousel({
	loop: false,
	margin: 20,
	responsiveClass: true,
	autoplay:true,
	autoplaytimeout:800,
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
  $('a.btn-gallery').on('click', function(event) {
		event.preventDefault();
		
		var gallery = $(this).attr('href');
    
		$(gallery).magnificPopup({
      delegate: 'a',
			type:'image',
			gallery: {
				enabled: true
			}
		}).magnificPopup('open');
	});
	
	
})

$('.topMenu .dropdown').click(function(){});	
$('.topMenu .dropdown a').click(function(){$('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');});
$('.topMenu .dropdown').click(function(){$(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');});


var a = 0;
$(window).scroll(function() {
	if($('#counter').length!=0){
	  var oTop = $('#counter').offset().top - window.innerHeight;
  if (a == 0 && $(window).scrollTop() > oTop) {
(function ($) {
	$.fn.countTo = function (options) {
		options = options || {};
		
		return $(this).each(function () {
			// set options for current element
			var settings = $.extend({}, $.fn.countTo.defaults, {
				from:            $(this).data('from'),
				to:              $(this).data('to'),
				speed:           $(this).data('speed'),
				refreshInterval: $(this).data('refresh-interval'),
				decimals:        $(this).data('decimals')
			}, options);
			
			// how many times to update the value, and how much to increment the value on each update
			var loops = Math.ceil(settings.speed / settings.refreshInterval),
				increment = (settings.to - settings.from) / loops;
			
			// references & variables that will change with each update
			var self = this,
				$self = $(this),
				loopCount = 0,
				value = settings.from,
				data = $self.data('countTo') || {};
			
			$self.data('countTo', data);
			
			// if an existing interval can be found, clear it first
			if (data.interval) {
				clearInterval(data.interval);
			}
			data.interval = setInterval(updateTimer, settings.refreshInterval);
			
			// initialize the element with the starting value
			render(value);
			
			function updateTimer() {
				value += increment;
				loopCount++;
				
				render(value);
				
				if (typeof(settings.onUpdate) == 'function') {
					settings.onUpdate.call(self, value);
				}
				
				if (loopCount >= loops) {
					// remove the interval
					$self.removeData('countTo');
					clearInterval(data.interval);
					value = settings.to;
					
					if (typeof(settings.onComplete) == 'function') {
						settings.onComplete.call(self, value);
					}
				}
			}
			
			function render(value) {
				var formattedValue = settings.formatter.call(self, value, settings);
				$self.html(formattedValue);
			}
		});
	};
	
	$.fn.countTo.defaults = {
		from: 0,               // the number the element should start at
		to: 0,                 // the number the element should end at
		speed: 1000,           // how long it should take to count between the target numbers
		refreshInterval: 100,  // how often the element should be updated
		decimals: 0,           // the number of decimal places to show
		formatter: formatter,  // handler for formatting the value before rendering
		onUpdate: null,        // callback method for every time the element is updated
		onComplete: null       // callback method for when the element finishes updating
	};
	
	function formatter(value, settings) {
		return value.toFixed(settings.decimals);
	}
}(jQuery));

jQuery(function ($) {
  // custom formatting example
  $('.count-number').data('countToOptions', {
	formatter: function (value, options) {
	  return value.toFixed(options.decimals).replace(/\B(?=(?:\d{3})+(?!\d))/g, ',');
	}
  });
  $('.count-number2').countTo( {
	decimals:2,
	
  });
  // start all the timers
  $('.timer').each(count); 
  
  //$('.timer2').each(count);  
  
  function count(options) {
	var $this = $(this);
	options = $.extend({}, options || {}, $this.data('countToOptions') || {});
	$this.countTo(options);
  }
});
 a = 1;
  }
}
});

$('.centralindiatab').click(function() {
$($('.nav-tabs li a[href="#centralindia"]')).trigger('click');
});

$('.rajasthantab').click(function() {
$($('.nav-tabs li a[href="#rajasthan"]')).trigger('click');
});


$('.maharashtratab').click(function() {
$($('.nav-tabs li a[href="#maharashtra"]')).trigger('click');
});


$('.uttarpradeshtab').click(function() {
$($('.nav-tabs li a[href="#uttarpradesh"]')).trigger('click');
});


$(document).ready(function()
{
	var btxt= $("section.breadcrumb-sub-layout").children("div.container").children("div.row").children("div.col-md-12").children("h1").text();
	if(btxt == "debt Information")
	{
		$("section.breadcrumb-sub-layout").children("div.container").children("div.row").children("div.col-md-12").children("h1").css("font-weight","bolder");
	}
});

$(document).ready(function ($) {
  var owl = $("#sustainability-businesses");
  owl.owlCarousel({
    autoplay: true,
    autoplayTimeout: 2000,
    autoplayHoverPause: true,
    items: 5,
    loop: true,
    center: false,
    rewind: false,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: true,
    freeDrag: false,
    margin: 0,
    stagePadding: 0,
    merge: false,
    mergeFit: true,
    autoWidth: false,
    startPosition: 0,
    rtl: false,
    smartSpeed: 500,
    fluidSpeed: false,
    dragEndSpeed: false,
    responsive: {
      0: {
        items: 1,
        loop: true,
      },
      480: {
        items: 1,
        loop: true,
      },
      768: {
        items: 2,
        loop: true,
      },
      992: {
        items: 5,
        loop: true,
      },
    },
    responsiveRefreshRate: 200,
    responsiveBaseElement: window,
    fallbackEasing: "swing",
    info: false,
    nestedItemSelector: false,
    itemElement: "div",
    stageElement: "div",
    refreshClass: "owl-refresh",
    loadedClass: "owl-loaded",
    loadingClass: "owl-loading",
    rtlClass: "owl-rtl",
    responsiveClass: "owl-responsive",
    dragClass: "owl-drag",
    itemClass: "owl-item",
    stageClass: "owl-stage",
    stageOuterClass: "owl-stage-outer",
    grabClass: "owl-grab",
    autoHeight: false,
    lazyLoad: false,
    dots: false,
    arrow: true,
  });

  $(".sustainability-businesses-slider-next").click(function () {
    owl.trigger("next.owl.carousel");
  });
  $(".sustainability-businesses-slider-prev").click(function () {
    owl.trigger("prev.owl.carousel");
  });
});


$(document).ready(function ($) {
  var owl = $("#ancillaries");
  owl.owlCarousel({
    autoplay: true,
    autoplayTimeout: 2000,
    autoplayHoverPause: true,
    items: 4,
    loop: true,
    center: false,
    rewind: false,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: true,
    freeDrag: false,
    margin: 0,
    stagePadding: 0,
    merge: false,
    mergeFit: true,
    autoWidth: false,
    startPosition: 0,
    rtl: false,
    smartSpeed: 500,
    fluidSpeed: false,
    dragEndSpeed: false,
    responsive: {
      0: {
        items: 1,
        loop: true,
      },
      480: {
        items: 1,
        loop: true,
      },
      768: {
        items: 2,
        loop: true,
      },
      992: {
        items: 4,
        loop: true,
      },
    },
    responsiveRefreshRate: 200,
    responsiveBaseElement: window,
    fallbackEasing: "swing",
    info: false,
    nestedItemSelector: false,
    itemElement: "div",
    stageElement: "div",
    refreshClass: "owl-refresh",
    loadedClass: "owl-loaded",
    loadingClass: "owl-loading",
    rtlClass: "owl-rtl",
    responsiveClass: "owl-responsive",
    dragClass: "owl-drag",
    itemClass: "owl-item",
    stageClass: "owl-stage",
    stageOuterClass: "owl-stage-outer",
    grabClass: "owl-grab",
    autoHeight: false,
    lazyLoad: false,
    dots: false,
    arrow: true,
  });

  $(".ancillaries-slider-next").click(function () {
    owl.trigger("next.owl.carousel");
  });
  $(".ancillaries-slider-prev").click(function () {
    owl.trigger("prev.owl.carousel");
  });
});