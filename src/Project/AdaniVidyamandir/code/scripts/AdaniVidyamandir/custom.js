var recaptcha1;
var onloadCallback = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc', //Replace this with your Site key
        'theme': 'light'
    });


};

$("#btnsubmit").on('click', function () {

    var message = '';
    var error = false;
 var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        message = message + "Captcha required";
        error = true;
    }
    $("#reResponse").val(response);
    var EmailId = $("#cmailid").val();
    if (EmailId != "") {
        var IsValidMail = validateEmail(EmailId);
        if (!IsValidMail) {
            message = message + "Invalid Email Address </br>";
            error = true;
        }
    }
    var name = $("#cname").val();
    if (name != "") {
        var IsValidName = validateName(name);
        if (!IsValidName) {
            message = message + "Invalid Name </br>";
            error = true;
        }
    }
    var MessageType = $("#cmessageType").val();
    var Messages = $("#cmessage").val();
    if (Messages != "") {
        var IsValidMessage = validateMessage(Messages);
        if (!IsValidMessage) {
            messages = message + "Invalid Message </br>";
            error = true;
        }
    }
    function validateMessage(smessage) {
        var letterNumber = /[a-zA-Z0-9,. ]/;
        if (letterNumber.test(smessage)) { return true; }
        else { return false; }
    }



    if (error) {
        $("#docErrorMessage").html(message);
        $("#docErrorMessage").focus();
        return false;
    }
    else $("#docErrorMessage").html("");
    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    };
    function validateName(sname) {
        var regex = /^[a-zA-Z ]+$/;

        if (regex.test(sname)) { return true; }
        else { return false; }
    };


});






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
      dots:true,
	  autoHeight:true,
	  lazyload:true,
	autoplay:true,
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
		$('.headerSec').addClass('fixedheader');
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
		$('.headerSec').removeClass('fixedheader');
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
  }),
  $('.hero-banner_inner').owlCarousel({
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
  }),
  $('.carousel_ql').owlCarousel({
	loop: false,
	margin: 0,
	responsiveClass: true,
	nav:false,
	dots:true,
	autoplay:true,
	autoplaytimeout:1500,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-arrow-left'></i>","<i class='fa fa-arrow-right'></i>"],
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
  $('.carousel-trustee').owlCarousel({
	loop: true,
	margin: 40,
	responsiveClass: true,
	nav:false,
	dots:true,
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
		items: 2
	  },
	  1000: {
		items: 3,
		margin: 60
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
	   $('.stories-carousel').owlCarousel({
		loop: true,
		margin: 20,
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
			items: 3,
			nav: true,
			dots:false,
			margin: 20
		  }
		}
	  }),
$('.success-stories_carousel').owlCarousel({
	loop: false,
	margin: 30,
	responsiveClass: true,
	nav:false,
	dots:true,
	autoplay:false,
	autoplaytimeout:1500,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
	autoplayHoverPause: true,
	items: 1
  });

	$('.events-carousel').owlCarousel({
	loop: false,
	margin: 30,
	responsiveClass: true,
	nav:false,
	dots:false,
	autoplay:false,
	autoplaytimeout:1500,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
	autoplayHoverPause: true,
	items: 1
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

 /*Affiliates Selector Domestic*/
    var $btns = $('.fac-btn').click(function () {
        if (this.id == 'all') {
            $('#facilities-parent > div').fadeIn(450).addClass('animated zoomIn');
        } else {
            var $el = $('.' + this.id).fadeIn(450).addClass('animated zoomIn');
            $('#facilities-parent > div').not($el).hide();
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })
	
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

$('.modal').on('shown.bs.modal', function () {
  var url = $('.modal.show iframe').attr('data-src');
  $('.modal.show iframe').attr("src", url);
});

$('.modal').on('hide.bs.modal', function () {
	var nullvalue = 0;
  $('.modal.show iframe').attr("src", nullvalue);
});



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


$('.sitemap-link a').click(function(){
	$(this).toggleClass('active');
	$('#sitemap_block').toggleClass('active');
	var href = $(this).attr( 'href' );
	$( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 150
  }, '600' );
});

$('.primaryMenu .dropdown').click(function () {
		// $('.topMenu .dropdown.active').removeClass('active');
        // $(this).toggleClass('active');
		if ( $(this).hasClass('active') ) {
			$(this).removeClass('active');
		} else {
			$('.primaryMenu .dropdown').removeClass('active');
			$(this).addClass('active');
		}
    });
	


  $('#btn-mrelease').change(function(){
	//alert("Showing");
	$(".all").hide();
    $('.' + $(this).val()).show();
	
  });
 

$('.topMenu .dropdown').click(function () {
		// $('.topMenu .dropdown.active').removeClass('active');
        // $(this).toggleClass('active');
		if ( $(this).hasClass('active') ) {
			$(this).removeClass('active');
		} else {
			$('.topMenu .dropdown').removeClass('active');
			$(this).addClass('active');
		}
    });
    $('.topMenu .dropdown a').click(function () {
        $('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');
    });
    $('.topMenu .dropdown').click(function () {
        $(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');
    });
