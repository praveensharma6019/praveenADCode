var recaptcha1;
var onloadCallback1 = function () {


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






$(document).ready(function(){
	if(window.location.href == "https://www.adanidefence.com/careers")
	{

$('select[name^="MessageType"] option[value="Careers"]').attr("selected","selected");
 $("#ResumeAttached").show();
	}
else if (window.location.href == "https://www.adanidefence.com/newsroom")
{
	$('select[name^="MessageType"] option[value="Media"]').attr("selected","selected");
	$("#ResumeAttached").hide();
}	
else
{
	$('select[name^="MessageType"] option[value=""]').attr("selected","selected");
	$("#ResumeAttached").hide();
}
	
	if(window.location.href.indexOf('contact-us') > 0  || window.location.href.indexOf('privacy-policy') > 0 || window.location.href.indexOf('newsroom/media-release')  > 0)
	{		
		$('.main_header').addClass('headerSec-inner');
	}
	
	
	$('.hero-banner').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
		items: 1,
		nav: false,
		dots: true
    });
	
var owl = $('.video-banner');
  owl.owlCarousel({
      loop:false,
      margin:0,
	  lazyLoad: true,
      nav:false,
      dots:true,
	  animateOut: 'fadeOut',
	  animateIn: 'fadeIn',
	  autoHeight:true,
	  autoplay: true,
	  autoplayTimeout: 5000,
		slideSpeed: 0,
	  lazyload:true,
      items:1
  })
  owl.on('translate.owl.carousel',function(e){
    $('.video-banner .owl-item video').each(function(){
      $(this).get(0).pause();
	  $(this).get(0).currentTime = 0;
    });
  });
  $('.video-banner .owl-item .item').each(function(){
      var attr = $(this).attr('data-videosrc');
      if (typeof attr !== typeof undefined && attr !== false) {
        var videosrc = $(this).attr('data-videosrc');
        $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.video-banner .owl-item.active video');
      }
    });
  owl.on('translated.owl.carousel',function(e){
	$('.video-banner .owl-item.active video').get(0).play();
  });
	
	$(".video-banner").carousel({ interval: false}); // this prevents the auto-loop
    var videos = document.querySelectorAll("video");
    videos.forEach(function(e) {
        e.addEventListener('ended', myHandler, false);
    }); 

    function myHandler(e) {
        $(".video-banner").carousel('next');
    }   	

$('#other-ventures').owlCarousel({
		loop: true,
		margin: 10,
		responsiveClass: true,
		autoplay: true,
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
	  
	$('.inner-herobanner-carousel').owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        items: 1,
		nav: false,
		dots: false
    })
	
	$('.vision-carousel').owlCarousel({
            loop: false,
            margin: 0,
			items: 1,
            responsiveClass: true,
            autoplayTimeout: 3000,
            autoplay: false,
            nav: true,
            dots: false,
            navText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"]
        })
	
});


$('#nav-button').click(function(){
	$('.navbar-collapse').toggleClass('show');
	$('.fade-overlay').toggleClass('active');
	$('.menu-overlay').toggleClass('d-none d-block');
	$('body').toggleClass('overflow-hidden');
	$('#back-to-top').toggleClass('d-none');
});
$('.nav-close').click(function(){
	$('.navbar-collapse').removeClass('show');
	$('.fade-overlay').toggleClass('active');
	$('.menu-overlay').toggleClass('d-none d-block');
	$('body').toggleClass('overflow-hidden');
	$('#back-to-top').toggleClass('d-none');
});

$('.menu-overlay').click(function(){
	$('.navbar-collapse').toggleClass('show');
	$('.navbar-collapse').removeClass('show');
	$('.menu-overlay').toggleClass('d-none d-block');
	$('body').toggleClass('overflow-hidden');
	$('#back-to-top').toggleClass('d-none');
});

$('.fade-overlay').click(function(){
	$('.navbar-collapse').removeClass('show');
	$('.fade-overlay').toggleClass('active');
	$('.menu-overlay').toggleClass('d-none d-block');
	$('body').toggleClass('overflow-hidden');
	$('#back-to-top').toggleClass('d-none');
});

$('.sitemap-link').click(function() {
	$('footer .sitemap_visible').toggleClass('active');
    $('.sitemap-link').toggleClass('sitemap-link-active');
	$('html, body').animate({
		scrollTop: $(".sitemap-link").offset().top
			- 100
	}, 1000);
});


$('.scroll_down a').on('click', function (e) {
        var href = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(href).offset().top - 70
        }, '600');
        e.preventDefault();
    });
	
// Hide Header on scroll down
    var didScroll;
    var lastScrollTop = 0;
    var delta = 0;
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
        $('#back-to-top').fadeIn();
        $('header.main_header').addClass('fixed-header');
        $('header.inner_header').addClass('box-shadow');
        $('header.main_header').removeClass('headerSec');
        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down
			//$('.btm-floating').addClass('active');

        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {

            }
        }
        if (st < 150) {

            $('#back-to-top').hide();
            $('header.main_header').removeClass('fixed-header');
			$('header.main_header').addClass('headerSec');
			$('header.inner_header').removeClass('box-shadow');
			//$('.btm-floating').removeClass('active');
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
	 
	 $('#mainMenu .dropdown').click(function () {
		if ( $(this).hasClass('active') ) {
			$(this).removeClass('active');
		} else {
			$('#mainMenu .dropdown').removeClass('active');
			$(this).addClass('active');
		}
    });
	
$('.loadmore').click(function (){
	$('.loadmore').toggleClass('d-none d-inline-block');
	$('.loadmore_section p').toggleClass('d-block');
});

$(function() {
  $('#map_selection').change(function(){
    $('.map-marker').hide();
    $('#' + $(this).val()).show();
  });
});

$(function() {
  $('#map_selection').change(function(){
    $('.' + $(this).val()).show();
  });
}); 


$(document).ready(function(){
          var pageURL = $(location).attr("href");
          if(pageURL == "https://adanidefence.com/terms-and-conditions")
          {
            $('.headerSec').addClass("headerSec-inner");
         }
 if(pageURL == "https://www.adanidefence.com/disclaimer")
          {
            $('.headerSec').addClass("headerSec-inner");
         }
		   var subStr = pageURL.substring(0, 39);
 if(subStr == "https://adanidefence.com/page-not-found")
          {
            $('.headerSec').addClass("headerSec-inner");
         }
    });
	
	
/* Assign empty url value to the iframe src attribute when
    modal hide, which stop the video playing */
	var url2 = $("#v-1").attr('src');
    $("#video-1").on('hide.bs.modal', function(){
        $("#v-1").attr('src', '');
    });
    
    /* Assign the initially stored url back to the iframe src
    attribute when modal is displayed again */
    $("#video-1").on('show.bs.modal', function(){
        $("#v-1").attr('src', url2);
    });		
	
	
	$('body').on('hidden.bs.modal', '.modal', function () {
$('video').trigger('pause');
});


$(function() {
  $('#btn-mrelease').change(function(){
	  
    $('.media-r').hide();
	$('.m-loadMore').hide();
    $('#' + $(this).val()).show();
	 
	});
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


$('.show-more').on('click', function () {
    //   $(this).siblings('.additional-content').toggle();

    $(this).closest('.plane-desc').find('.additional-content').toggle();
    //var elem = $("#toggle").text();
    var elem = $(this).closest('.plane-desc').find('#toggle').text();

    if (elem == "View More") {
        $(this).closest('.plane-desc').find('#toggle').text("View Less");
        $(this).closest('.plane-desc').find('#content_scroll').css({
            height: "150px",
            overflow: "scroll",
            overflowX: "hidden",
            display: "block"
        });
    }
    else {
        $(this).closest('.plane-desc').find('#toggle').text("View More");
        $(this).closest('.plane-desc').find('#content_scroll').css({
            height: "80px",
            overflowX: "hidden",
            overflowY: "hidden",

        });

    }
});

/*Sitecore Form Country code changes*/
$(document).ready(function(){
	$('.CountryCodeList').val($('.iti__selected-dial-code').text())
})
$('.iti__country-list li').click(function(){$('.CountryCodeList').val($('.iti__selected-flag').children('.iti__selected-dial-code').text())});



/*Hamburger our capabilities arrow expand*/

$(document).ready(function () {
            $(".sub-menu-btn").click(function () {
                $(this).next(".collapse").slideToggle(250);
                $(this).toggleClass('active');
                setTimeout(function () {
                    document.querySelector(".sidebar-navigation .hamburger_section.single_item")?.scrollIntoView();
                }, 300);
            });
        });
		


