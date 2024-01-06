      var recaptcha1;
      var myCallBack  = function() {
       
		
		 //Render the recaptcha2 on the element with ID "recaptcha2"
        recaptcha1 = grecaptcha.render('recaptcha1', {
          'sitekey' : '6LcAkasUAAAAAGmIZTUyEhNafee2adaz0BpZyrOw', //Replace this with your Site key
          'theme' : 'light'
        });
		
	
	  };

 



$('.accordion-tabs a').click(function(){
	$('.accordion-tabs a').toggleClass('collapsed'); 
});		
$('.accordion-tabs a').click(function(){		
  $('.accordion-tabs a').toggleClass('collapsed');		
}); 

$('.topMenu .dropdown').click(function(){
$(this).toggleClass('active');
});	
$('.topMenu .dropdown a').click(function(){
$('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');
});
$('.topMenu .dropdown').click(function(){
$(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)'); 
});	

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


$('#req-callback').on('shown.bs.modal', function (e) {
   //  $('#deliveryArrivalDate').datetimepicker({ format: 'DD/MM/YYYY' });
});


/*Home Map*/
$(function() {
  $('#map-accordion select').change(function(){
    $('.map-dots').hide();
    $('#' + $(this).val()).show();
  });
});

$(function() {
  $('#map-accordion select').change(function(){
    $('.m-expmore-btn').hide();
    $('.' + $(this).val()).show();
  });
}); 

  $('[data-toggle="tooltip"]').tooltip()

 $(window).scroll(function(){
    if ($(this).scrollTop() > 200) {
    } else {
       $('.headerSec .fixed-top').removeClass('sticky-header');
    }
});

  
$(function() {
  $('#btn-mrelease').change(function(){
	  
    $('.media-r').hide();
	$('.m-loadMore').hide();
    $('#' + $(this).val()).show();
	 
	});
}); 
 function CloseModal2(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
} 

var TotalProject = document.getElementsByClassName("column");
 $(document).ready(function() {
	$('.ProjectAssetsDataInner').hide();
$('#DefaultData').show();

$('.pointer').click(function(){
   $('.tab-panea').hide();		
   $('.ProjectAssetsDataInner').hide();
   var getTabId = $(this).attr('rel');
   $('#'+getTabId).show().addClass('animated fadeInDown');
   $(".indiaMap a").attr("class", "pointer");
   $(this).attr("class", "active");
}); 
	 
	 
	 $('.modal .close').click(function(){
		 
		 var videoId=$(this).attr('Id');
		 CloseModal2(videoId);
		 
	 });

 $( '.scroll-down a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 70
  }, '600' );
  e.preventDefault();
});
 
 $('.tabs-section .item ul li a').click(function(){
  $('.tabs-section .item ul li a').removeClass('active');
});
 
	 $("#trackingBtnSearch").click(function () {
		 var response  =grecaptcha.getResponse(recaptcha1);
	if(response.length == 0) 
	{
		alert("Captcha required.");
		return false;
	}
        $('.required').remove();
        var ptype = $("#TrackingportType").val();
        if (ptype == "--Select Port--") {
            $("#TrackingportType").after("<span class='required'>Please select the valid Port</span>");
            return false;
        }
        var containerNo = $("#TrackingcontainerNo").val();
        if (containerNo == "") {
            $("#TrackingcontainerNo").after("<span class='required'>Please Enter Container Number</span>");
            return false;
        }
    });
	
	
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
	
	$('.sitemap-link').click(function() {
	$('.ft-mobilemenu').removeClass('d-none');
	$('.sitemap-link').addClass('d-none');
	$('.sitemap-link').removeClass('d-block');
});
 
 
	
	$('#TrackingportType').change(function() {   
if($(this).children("option:selected").val() == "Kattupalli")
{
window.open(
  'https://ktactosxt1.adani.com/akppl/ctrHist',
  '_blank');
}
});

/*Port & Terminal Search Filter*/
filterSelection("all")
function filterSelection(c) {
  var x, i;
  x = document.getElementsByClassName("column");
  if (c == "all") c = "";
  for (i = 0; i < x.length; i++) {
    w3RemoveClass(x[i], "show");
    if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show");
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
$(".resetMap").hide();

localStorage.setItem("TotalItem",TotalProject);
}); 



$(document).ready(function(){
	$('#other-ventures').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		autoplay: false,
		 autoplayTimeout: 2500,
		 navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
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
	  $('.plant-carousel').owlCarousel({
		loop: true,
		margin: 0,
		responsiveClass: true,
		autoplay: true,
		
		autoHeight:true,
		 autoplayTimeout: 2000,
		 navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
		 autoplayHoverPause: false,
		responsive: {
		  0: {
			items: 1,
			nav: false,
			dots:true
		  },
		  600: {
			items: 1,
			nav: false,
			dots:true
		  },
		  1000: {
			items: 1,
			nav: true,
			dots:false
		  }
		}
	  })
	  
	$(function () {
  $('[data-toggle="tooltip"]').tooltip()
})
	
/*Port landing Page Search Filter*/
$("#search-btn").on("keyup", function() {
    var value = $(this).val().toLowerCase();
    $(".dropdown-menu li").filter(function() {
      $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
  });
  
	/*Contact Address Filter*/
	var $btns = $('#op').change(function() {
  if (this.id == 'all') {
    $('#add-parent > div').fadeIn(450);
  } else {
    var $el = $('.' + this.id).toggleClass('d-block');
    $('#add-parent > div').not($el).toggleClass('d-none');
  }
  $btns.removeClass('active');
  $(this).addClass('active');
})
/*Contact Address*/
	var $btns = $('#op').change(function() {
  if (this.id == 'all') {
    $('.main-add > div').fadeIn(450);
  } else {
    var $el = $('.' + this.id).toggleClass('d-block');
    $('.main-add > div').not($el).toggleClass('d-block');
  }
  $btns.removeClass('active');
  $(this).addClass('active');
})

	
	
/*Map Selector*/
	var $btns = $('.map-btn').hover(function() {
  var $el = $('.' + this.id).fadeIn(450).toggleClass('map-active');
    $('.map-markers > li').not($el).removeClass('map-active');
  $btns.removeClass('map-active');
  $(this).addClass('map-active');
})

$(".loadmore-content p").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
	$(".loadMore").toggleClass("d-block d-none");
    $(".loadmore-content p").toggleClass("d-none");
	$(".loadmore-content h2").toggleClass("d-none d-block");
	$(".loadmore-content ul").toggleClass("d-none");
	$("p.loadMore").text("").show(1000);
	$(".arrow.loadMore").text("Read more ").show(1000);
	  $('<i class="fa fa-chevron-down"/>').appendTo('.arrow.loadMore');
	if($(".loadmore-content p:hidden").length == 0) {
	  $(".arrow.loadMore").text("Read less ").show(1000);
	  $(' <i class="fa fa-chevron-up"/>').appendTo('.arrow.loadMore');
	  $(".arrow.loadMore").show(1000);
    }
	
  });   

  /*$(".about-apsez p").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
    $(".about-apsez p").attr("style", "display: block !important;transition: ease display 0.5s;");
	if($(".about-apsez p:hidden").length == 0) {
      $(".loadMore").text("Read less").show(1000);
    }
  });*/
  $(".m-loadMore").on("click", function(e){
	$('#media-r-parent .media-r').addClass('d-block');
	$('.m-loadMore').addClass('d-none');
  });
  
  
  /*
  $(".view-all .row").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
    $(".view-all .row").attr("style", "display: flex !important;transition: ease display 0.5s;");
	if($(".view-all .row:hidden").length == 0) {
      $(".loadMore").text("Read less").show(1000);
    }
  });*/
  
  /*
  $(".media-r .col-md-12").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
    $(".media-r .col-md-12").attr("style", "display: block !important;transition: ease display 0.5s;");
	if($(".media-r .col-md-12:hidden").length == 0) {
      $(".loadMore").text("").show(1000);
    }
  });*/
	
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
		autoplayTimeout: 11000,
		slideSpeed: 3000,
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
        $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.homeslider .owl-item.active video');
      }
    });
  owl.on('translated.owl.carousel',function(e){
    $('.homeslider .owl-item.active video').get(0).play();
  });
	
	
              $('#sustanibility').owlCarousel({
                loop: true,
                margin: 10,
                responsiveClass: true,
                responsive: {
                  0: {
                    items: 1,
                    nav: true
                  },
                  600: {
                    items: 3,
                    nav: false
                  },
                  1000: {
                    items: 3,
                    nav: false,
                    loop: false,
                    margin: 20,
					dots:true
                  }
                }
              })
			  
			  
			  $('.single-carousel').owlCarousel({
                loop: false,
                margin: 10,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
                responsiveClass: true,
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
			  $('.single-item').owlCarousel({
                loop: false,
                margin: 10,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
                responsiveClass: true,
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
			  
			  $('.arrow-carousel').owlCarousel({
                loop: false,
                margin: 0,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
                responsiveClass: true,
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
			  
			  $('.arrow-c').owlCarousel({
                loop: false,
                margin: 0,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
                responsiveClass: true,
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
			  
			  $('#about-banner').owlCarousel({
                loop: true,
                margin: 0,
                responsiveClass: true,
				autoplayTimeout: 3000,
				autoplay: true,
				autoHeight:true,
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
			  
			   $('.four-item').owlCarousel({
                loop: false,
                margin: 0,
                responsiveClass: true,
				autoplayTimeout: 3000,
				autoplay: true,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
                responsive: {
                  0: {
                    items: 2,
					nav:false
                  },
                  600: {
                    items: 3
                  },
                  1000: {
                    items: 4
                  }
                }
              })
			  
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
			  
			  $('#home-video-block').owlCarousel({
                loop: true,
                margin: 0,
				nav: false,
				dots: false,
				navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
                responsiveClass: true,
                responsive: {
                  0: {
                    items: 1,
					dots:true,
					nav:false
                  },
                  600: {
                    items: 1,
					dots:true,
					nav:false
                  },
                  1000: {
                    items: 1
                  }
                }
              })

	$('#business-stats').owlCarousel({
                loop: false,
                margin: 10,
				nav: false,
				autoplay: true,
				slideSpeed: 2000,
				dots:false,
                responsiveClass: true,
                responsive: {
                  0: {
                    items: 1,
					nav: true,
					loop:true
                  },
                  600: {
                    items: 3,
					nav: true,
					loop:true
                  },
                  1000: {
                    items: 5
                  }
                }
              })
	/* Initialize Latest Projects Carousel on home page */
	$("#commPrjtsCarousel").owlCarousel({
		nav:true,
		center:true,
		loop:true,
		margin:30,
		navText:[],
		responsive : {
		// breakpoint from 0 up
		0 : {
			items : 1
		},
		// breakpoint from 480 up
		480 : {
			items : 2
		},
		// breakpoint from 768 up
		768 : {
			items : 3
		}
	}
	});
	
	$('#testimonials').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		responsive: {
		  0: {
			items: 1,
			nav: true,
			dots: true,
			nav: false
		  },
		  600: {
			items: 2,
			nav: false,
			dots: true,
			nav: false
		  },
		  1000: {
			items: 2,
			nav: false,
			dots: true,
			loop: false,
			margin: 20,
			
		  }
		}
	  })
	
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
	});
	
			  
	$('.vision-carousel').owlCarousel({
                loop: true,
                margin: 0,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
                responsiveClass: true,
				autoplay: true,
				autoplayTimeout: 2500,
				autoplayHoverPause: false,
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
	
	
	/*Home VIdeo Section*/
	var sync1 = $("#sync1");
  var sync2 = $("#sync2");
 
  sync1.owlCarousel({
    loop: true,
	margin: 0,
	nav: false,
	dots: false,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	responsiveClass: true,
	responsive: {
	  0: {
		items: 1,
		dots:true,
		nav:false
	  },
	  600: {
		items: 1,
		dots:true,
		nav:false
	  },
	  1000: {
		items: 1
	  }
	}
  });
 
  sync2.owlCarousel({
    items : 2,
    itemsDesktop      : [1199,10],
    itemsDesktopSmall     : [979,10],
    itemsTablet       : [768,8],
    itemsMobile       : [479,4],
    pagination:false,
	nav:true,
	dots:false,
    responsiveRefreshRate : 100,
    afterInit : function(el){
      el.find(".owl-item").eq(0).addClass("synced");
    }
  });
 
  function syncPosition(el){
    var current = this.currentItem;
    $("#sync2")
      .find(".owl-item")
      .removeClass("synced")
      .eq(current)
      .addClass("synced")
    if($("#sync2").data("owlCarousel") !== undefined){
      center(current)
    }
  }
 
  $("#sync2").on("click", ".owl-item", function(e){
    e.preventDefault();
    var number = $(this).data("owlItem");
    sync1.trigger("owl.goTo",number);
  });
 
  function center(number){
    var sync2visible = sync2.data("owlCarousel").owl.visibleItems;
    var num = number;
    var found = false;
    for(var i in sync2visible){
      if(num === sync2visible[i]){
        var found = true;
      }
    }
 
    if(found===false){
      if(num>sync2visible[sync2visible.length-1]){
        sync2.trigger("owl.goTo", num - sync2visible.length+2)
      }else{
        if(num - 1 === -1){
          num = 0;
        }
        sync2.trigger("owl.goTo", num);
      }
    } else if(num === sync2visible[sync2visible.length-1]){
      sync2.trigger("owl.goTo", sync2visible[1])
    } else if(num === sync2visible[0]){
      sync2.trigger("owl.goTo", num-1)
    }
    
  }
	
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
		$('#back-to-top-enq').fadeIn();
    if (st > lastScrollTop && st > navbarHeight){
        // Scroll Down
        
    } else {
        // Scroll Up
        if(st + $(window).height() < $(document).height()) {
        
        }
    }
    if(st < 150){
		
		$('#back-to-top').hide();
		$('#back-to-top-enq').hide();
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
	
	
	/*$(document).ready(function() {
	var state = "paused";
	$('#pause').on('click', function() {
		if(state == 'paused') {
			state = "playing";
			$("#circle").attr("class", "play");
			$("#from_pause_to_play")[0].beginElement();
			$(".stop i").toggleClass('fa fa-play');
		} else {
			state = "paused";
			$("#circle").attr("class", "");
			$("#from_play_to_pause")[0].beginElement();
			$(".stop i").toggleClass('fa fa-play')
		}
	});
});*/
	
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

	$(".popup").magnificPopup({
    type: "image",
    removalDelay: 160,
    preloader: false,
    fixedContentPos: true,
    gallery: {
      enabled: true
    }
  });
	
	
	/*News Carousel
	 var owl = $('.banner-news');
   owl.owlCarousel({
     items: 1,
     loop: true,
     margin: 0,
	 dots:false,
	 nav:true,
     autoplay: true,
     autoplayTimeout: 3000,
		animateOut: 'fadeOut',
    animateIn: 'fadeInLeft',
     autoplayHoverPause: false
   });*/
   
   /*Play/Pause News on Click*/
   $('.stop').on('click',function(){
if($('.homeslider').attr('data-click-state') == 1) {
owl.trigger('play.owl.autoplay', [3000])
$('.homeslider').attr('data-click-state', 0)
$(".stop i").addClass('fa-pause')
$(".stop i").removeClass('fa-play')
} else {
owl.trigger('stop.owl.autoplay')
$('.homeslider').attr('data-click-state', 1)
$(".stop i").addClass('fa-play')
$(".stop i").removeClass('fa-pause')
}
	});

	
/*Video Banner*/
/*var videoPlayButton,
	videoWrapper = document.getElementsByClassName('video-wrapper')[0],
    video = document.getElementsByTagName('video')[0],
    videoMethods = {
        renderVideoPlayButton: function() {
            if (videoWrapper.contains(video)) {
				this.formatVideoPlayButton()
                video.classList.add('has-media-controls-hidden')
                videoPlayButton = document.getElementsByClassName('video-overlay-play-button')[0]
                videoPlayButton.addEventListener('click', this.hideVideoPlayButton)
            }
        },

        formatVideoPlayButton: function() {
            videoWrapper.insertAdjacentHTML('beforeend', '\
                <svg class="video-overlay-play-button" viewBox="0 0 200 200" alt="Play video">\
                    <circle cx="100" cy="100" r="90" fill="none" stroke-width="15" stroke="#fff"/>\
                    <polygon points="70, 55 70, 145 145, 100" fill="#fff"/>\
                </svg>\
            ')
        },

        hideVideoPlayButton: function() {
            video.play()
            videoPlayButton.classList.add('is-hidden')
            video.classList.remove('has-media-controls-hidden')
            video.setAttribute('controls', 'controls')
        }
	}

videoMethods.renderVideoPlayButton()



})
*/

/*Port & Terminal Search Filter*/
filterSelection("all")
function filterSelection(c) {
  var x, i;
  x = document.getElementsByClassName("column");
  if (c == "all") c = "";
  for (i = 0; i < x.length; i++) {
    w3RemoveClass(x[i], "show");
    if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show");
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
/*
var btnContainer = document.getElementById("port-filters");
for (var i = 0; i < btns.length; i++) {
  btns[i].addEventListener("click", function(){
    var current = document.getElementsByClassName("active");
    current[0].className = current[0].className.replace(" active", "");
	$('#port-filters .btn-f').removeClass('active') 
    this.className += " active";
  });
)}*/

$(function() {
  $("#bars li .bar").each( function( key, bar ) {
    var percentage = $(this).data('percentage');
    
    $(this).css('height', percentage + '%');
    
    //$(this).animate({
    //  'height' : percentage + '%'
    //}, 1000);
  });
}); 

});


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

function filterSelectionMW(c) {
  var x, i;
  var count=0;
   $(".pagination_cont").hide();
  x = document.getElementsByClassName("column");
  if (c == "all") c = "all";
  $("#filterMW").val(c);
  var loc=$("#filterLocation").val();
  for (i = 0; i < x.length; i++) {
   // w3RemoveClass(x[i], "show");
	// w3RemoveClass(x[i], "d-block");
	// w3AddClass(x[i], "d-none");
	$(x[i]).css("display","none");
	if(loc=="" || c=="")
	{
    if (x[i].className.indexOf(c) > -1){

	//w3AddClass(x[i], "show");
	//w3AddClass(x[i], "d-block");
	$(x[i]).css("display","block");
	
	};
	}
	else if(c=="all" && loc=="all")
	{
	if(count<=5)	{
		//w3AddClass(x[i], "show");
		$(x[i]).css("display","block");}
		count++;
	}
	else
	{
	if (x[i].className.indexOf(c) > -1 && x[i].className.indexOf(loc) > -1)
	{
		
		//w3AddClass(x[i], "show");
		$(x[i]).css("display","block");
		
	}
	}
  }
  if(c=="all" && loc=="all")
		{
		 $(".pagination_cont").show();
		}
  
  if(c=="W600")
  {
	 $('.filter1').text("600+ MW"); 
  }
	else if(c!="all")
	{
		var z=c.replace("W","");
	 $('.filter1').text(z+" MW"); 	
	}
	else if(c=="all")
	{
		$('.filter1').text("All"); 
	}
	else
	{
		$('.filter1').text(c); 
	}
  
}
function filterSelectionLOC(c) {
  var x, i;
  var count=0;
  $(".pagination_cont").hide();
  x = document.getElementsByClassName("column");
  if (c == "all") c = "all";
  $("#filterLocation").val(c);
  var mw=$("#filterMW").val();
  for (i = 0; i < x.length; i++) {
   // w3RemoveClass(x[i], "show");
   $(x[i]).css("display","none");
	if(c=="" || mw=="")
	{
    if (x[i].className.indexOf(c) > -1) {
		
	//w3AddClass(x[i], "show");
	//w3AddClass(x[i], "d-block");
	$(x[i]).css("display","block");
	
	}
	}
	else if(c=="all" && mw=="all")
	{
	if(count<=5)	{
		//w3AddClass(x[i], "show");
		$(x[i]).css("display","block");}
		count++;
	}
	else
	{
	if (x[i].className.indexOf(c) > -1 && x[i].className.indexOf(mw) > -1)
	{
		//w3AddClass(x[i], "show");
		//$(x[i]).css("display","block");
		
		//w3AddClass(x[i], "show");
		$(x[i]).css("display","block");
		
		
		
	}
	}
  }
  if(c=="all" && mw=="all")
		{
		 $(".pagination_cont").show();
		}
  $('.filter2').text(c);
}

//Renewable Plant Filter Map
function filterMap(c) {
  var x, i;
  x = document.getElementsByClassName("map-box");
  if (c == "all") c = "";
  for (i = 0; i < x.length; i++) {
    w3RemoveClass(x[i], "d-block");
    if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "d-block");
  }
  if(c=="allplants")
  {
	  $(".selectState").hide();
	  $(".resetMap").hide();
	  
	  
  }
  else
  {
	 var replacelocation=c.replace("-"," ");
	$(".selectState").text(replacelocation);  
	$(".selectState").show();
	  $(".resetMap").show();
	  
	 // $("#filterReadMore").removeClass("d-none");
	  var href = "/solar-power?state=" + replacelocation;
	  $("#filterReadMore").attr("href",href);
	  
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

(function($, window, document) { 'use strict';
    var Paginator = function(element, options) {
        this.el = $(element);
        this.options = $.extend({}, $.fn.paginathing.defaults, options);
        this.startPage = 1;
        this.currentPage = 1;
        this.totalItems = this.el.children().length;
        this.totalPages = Math.ceil(this.totalItems / this.options.perPage);
        this.container = $('<div></div>').addClass(this.options.containerClass);
        this.ul = $('<ul></ul>').addClass(this.options.ulClass);
        this.show(this.startPage);
        return this;
    }
    Paginator.prototype = {
        pagination: function(type, page) {
            var _self = this;
            var li = $('<li></li>');
            var a = $('<a class="page-link"></a>').attr('href', '#');
            var cssClass = type === 'number' ? _self.options.liClass : type;
            var text = type === 'number' ? page : _self.paginationText(type);
            li.addClass(cssClass);
            li.data('pagination-type', type);
            li.data('page', page);
            li.append(a.html(text));
            return li;
        },
        paginationText: function(type) { return this.options[type + 'Text']; },
        buildPagination: function() {
            var _self = this;
            var pagination = [];
            var prev = _self.currentPage - 1 < _self.startPage ? _self.startPage : _self.currentPage - 1;
            var next = _self.currentPage + 1 > _self.totalPages ? _self.totalPages : _self.currentPage + 1;
            var start, end;
            var limit = 6;
            var interval = 2;
            console.log(_self.totalPages);
            if(_self.totalPages > limit) {
                if(_self.currentPage <= Math.ceil(limit / 2) + 0.5) { start = 1; end = limit; }
                else if (_self.currentPage + Math.floor(limit / 2) >= _self.totalPages) { start = _self.totalPages + 1 - limit; end = _self.totalPages; }
                else { start = _self.currentPage - Math.ceil(limit / 2 - 0.5); end = _self.currentPage + Math.floor(limit / 2); }
            } else { start = _self.startPage; end = _self.totalPages; }
            if(_self.options.firstLast) { pagination.push(_self.pagination('first', _self.startPage)); }
            if(_self.options.prevNext) { pagination.push(_self.pagination('prev', prev)); }
            for(var i = start; i <= end; i++) { pagination.push(_self.pagination('number', i)); }
            if(_self.options.prevNext) { pagination.push(_self.pagination('next', next)); }
            if(_self.options.firstLast) { pagination.push(_self.pagination('last', _self.totalPages)); }
            return pagination;
        },
        render: function(page) {
            var _self = this;
            var options = _self.options;
            var pagination = _self.buildPagination();
            _self.ul.children().remove();
            _self.ul.append(pagination);
            var startAt = page === 1 ? 0 : (page - 1) * options.perPage;
            var endAt = page * options.perPage;
            _self.el.children().hide();
            _self.el.children().slice(startAt, endAt).show();
            _self.ul.children().each(function() {
                var _li = $(this);
                var type = _li.data('pagination-type');
                switch (type) {
                    case 'number':
                    if(_li.data('page') === page) { _li.addClass(options.activeClass); } break; case 'first':
                    page === _self.startPage && _li.toggleClass(options.disabledClass); break; case 'last':
                    page === _self.totalPages && _li.toggleClass(options.disabledClass); break; case 'prev':
                    (page - 1) < _self.startPage && _li.toggleClass(options.disabledClass); break; case 'next':
                    (page + 1) > _self.totalPages && _li.toggleClass(options.disabledClass); break; default: break;
                }
            });
            if(options.insertAfter) { _self.container.append(_self.ul).insertAfter($(options.insertAfter)); }
            else { _self.el.after(_self.container.append(_self.ul)); }
        },
        handle: function() { var _self = this; _self.container.find('li').each(function(){ var _li = $(this); _li.click(function(e) { e.preventDefault(); var page = _li.data('page'); _self.currentPage = page; _self.show(page); }); }); },
        show: function(page) { var _self = this; _self.render(page); _self.handle(); }
    }
    $.fn.paginathing = function(options) {
        var _self = this;
        var settings = (typeof options === 'object') ? options : {};
        return _self.each(function(){ var paginate = new Paginator(this, options); return paginate; });
    };
    $.fn.paginathing.defaults = {
        perPage: 6,
        prevNext: true,
        firstLast: false,
        prevText: 'Previous',
        nextText: 'Next',
        firstText: 'First',
        lastText: 'Last',
        containerClass: 'pagination_cont text-lg-right text-center',
        ulClass: 'pagination_numbers pagination pagination-sm',
        liClass: 'pagination_button page-item',
        activeClass: 'active',
        disabledClass: 'passive',
        insertAfter: null
    }
}(jQuery, window, document));



$('.schedule-row-loop-div').paginathing({
  perPage: 6,
})
$('.pagination_cont ul li a').click(function(){
   $('.schedule-row-loop').addClass('animated fadeIn');		
   
}); 
$(document).ready(function(){
	var today = new Date();

var day = today.getDate();
var month = today.getMonth() + 1;
var year = today.getFullYear();

if (day < 10) {
  day = '0' + day
}
if (month < 10) {
  month = '0' + month
}

var out = document.getElementById("datetime");

out.innerHTML = day + "-" + month + "-" + year;
});