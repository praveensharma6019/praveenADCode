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



$(function() {
  $('#btn-mrelease').change(function(){
    $('.media-r').hide();
	$('.m-loadMore').hide();
    $('#' + $(this).val()).show();
  });
});


$(document).ready(function(){
	
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




  $(".about-apsez p").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
    $(".about-apsez p").attr("style", "display: block !important;transition: ease display 0.5s;");
	if($(".about-apsez p:hidden").length == 0) {
      $(".loadMore").text("").show(1000);
    }
  });
  $(".m-loadMore").on("click", function(e){
	$('#media-r-parent .media-r').addClass('d-block');
	$('.m-loadMore').addClass('d-none');
  });
  
  
  $(".view-all .row").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
    $(".view-all .row").attr("style", "display: flex !important;transition: ease display 0.5s;");
	if($(".view-all .row:hidden").length == 0) {
      $(".loadMore").text("").show(1000);
    }
  });
  
  $(".media-r .col-md-12").slice(0, 1).show();
  $(".loadMore").on("click", function(e){
    e.preventDefault();
    $(".media-r .col-md-12").attr("style", "display: block !important;transition: ease display 0.5s;");
	if($(".media-r .col-md-12:hidden").length == 0) {
      $(".loadMore").text("").show(1000);
    }
  });
	
 var owl = $('.homeslider');
  owl.owlCarousel({
      loop:false,
      margin:0,
	  lazyLoad: true,
      nav:false,
      dots:true,
	  autoHeight:true,
	  autoplay: true,
		autoplayTimeout: 50000,
		slideSpeed: 2000,
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
        $(this).prepend('<video muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.homeslider .owl-item.active video').attr('autoplay',true).attr('loop',true);
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
			  $('.three-slide').owlCarousel({
                loop: true,
                margin: 10,
				nav:true,
				dots:false,
				navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
                responsiveClass: true,
                responsive: {
                  0: {
                    items: 2
                  },
                  600: {
                    items: 2
                  },
                  1000: {
                    items: 3
                  }
                }
              })
			  
			  $('#about-banner').owlCarousel({
                loop: false,
                margin: 0,
                responsiveClass: true,
				autoplayTimeout: 3000,
				animateOut: 'fadeInLeft',
				animateIn: 'fadeOut',
				autoplay: true,
				
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
					nav: true
                  },
                  600: {
                    items: 3,
					nav: true
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
        nav: false,
        dots:true,
        responsiveClass: true,
        navText: [
            '<i class="fa fa-arrow-left" aria-hidden="true"></i>',
            '<i class="fa fa-arrow-right" aria-hidden="true"></i>'
        ],
        autoplay: false,
        autoplaytimeout: 2000,
        slideSpeed: 1500,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 3
            },
            1000: {
                items: 3
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
	
	
	/*News Carousel*/
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
   });
   
   /*Play/Pause News on Click*/
   $('.stop').on('click',function(){
if($('.banner-news').attr('data-click-state') == 1) {
owl.trigger('play.owl.autoplay', [3000])
$('.banner-news').attr('data-click-state', 0)
$(".stop i").addClass('fa-pause')
$(".stop i").removeClass('fa-play')
} else {
owl.trigger('stop.owl.autoplay')
$('.banner-news').attr('data-click-state', 1)
$(".stop i").addClass('fa-play')
$(".stop i").removeClass('fa-pause')
}
	});

	
/*Video Banner*/
var videoPlayButton,
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
var btnContainer = document.getElementById("port-filters");
var btns = btnContainer.getElementsByClassName("btn-f");
for (var i = 0; i < btns.length; i++) {
  btns[i].addEventListener("click", function(){
    var current = document.getElementsByClassName("btn-f");
    current[0].className = current[0].className.replace(" active", "");
    this.className += " active";
  });
}
