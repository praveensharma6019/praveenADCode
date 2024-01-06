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
  $('.timeline-carousel').owlCarousel({
		loop: true,
		margin: 0,
		responsiveClass: true,
		autoplayTimeout: 3000,
		autoplay: true,
		autoHeight:true,
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
	  }),
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
	$('.ft-mobilemenu').removeClass('d-none');
	$('.sitemap-link').addClass('d-none');
	$('.sitemap-link').removeClass('d-block');
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
	  autoplay:true,
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
		

$('#footerArrow').click(function() {
	$('.footerPanel2 .mobile-none').toggleClass('d-none');
	$('.footerPanel2 .footerpanel-1').toggle(100);
	$('.footerPanel2 .txt-center').toggle(100);
	$('.footerPanel2 .ft-submenu').toggleClass('col-lg-12');
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
        }),
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
  }),
  $('.case-study').owlCarousel({
		loop: true,
		margin: 0,
		responsiveClass: true,
		autoplay: true,
		 autoplayTimeout: 5000,
		 animateOut: 'fadeOut',
		 animateIn: 'fadeIn',
		 navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
		 autoplayHoverPause: true,
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
	  }),
  
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

    $('.ft-business li a').click(function () {
        $('html, body').animate({
            scrollTop: $("#accordion").offset().top
                - 130
        }, 1000);
    });

})
 function CloseModal2(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
}


$(function () {
    $('#btn-mrelease').change(function () {
        $('.media-r').hide();
        $('.m-loadMore').hide();
        $('#' + $(this).val()).show();
    });
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
	
$(".exp-capabilities").on("click", function(){
	$('.explore-navtabs').toggleClass('d-none')
})

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


  
  
  $('.topMenu .dropdown').click(function(){	 	 
 	$(this).toggleClass('active');	 	 
 	});	
$('.topMenu .dropdown a').click(function(){	 	 
 	$('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');	 	 
 	});
$('.topMenu .dropdown').click(function(){	 	 
 	$(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');	 	 
 	});	

 $( '.scroll-down a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 50
  }, '600' );
  e.preventDefault();
});


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
  
  $(".about-apsez .loadMore_R").on("click", function(e){
	  $(".about-apsez p").toggleClass("d-none");
	$(".about-apsez h2").toggleClass("d-none d-block");
	$(".about-apsez ul").toggleClass("d-none");
	$(".about-apsez .readmore").toggleClass("d-none d-block");
	$(".about-apsez .readless").toggleClass("d-block");	
  });
  
  
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
$.extend({
    distinct : function(anArray) {
       var result = [];
       $.each(anArray, function(i,v){
           if ($.inArray(v, result) == -1) result.push(v);
       });
       return result;
    }
});
function filterSelectionMW(c) {
  var x, i;
  var count=0;
  var cList=[];
   $(".pagination_cont").hide();
   var stateLoc=['Tamil-Nadu','Karnataka','Punjab','Chhattisgarh','Telangana','Andhra-Pradesh','Uttar-Pradesh','Rajasthan','Maharashtra','Madhya-Pradesh','Gujarat'];
  x = document.getElementsByClassName("column");
  if (c == "all") c = "all";
  $("#filterMW").val(c);
  var loc=$("#filterLocation").val();
  for (i = 0; i < x.length; i++) {
   // w3RemoveClass(x[i], "show");
	// w3RemoveClass(x[i], "d-block");
	// w3AddClass(x[i], "d-none");
	$(x[i]).css("display","none");
	$(x[i]).removeClass("dispLoc");
	if(loc=="" || c=="")
	{
    if (x[i].className.indexOf(c) > -1){

	//w3AddClass(x[i], "show");
	//w3AddClass(x[i], "d-block");
	$(x[i]).css("display","block");
	$(x[i]).addClass("dispLoc");
	
	};
	}
	else if(c=="all" && loc=="all")
	{
	if(count<=5)	{
		//w3AddClass(x[i], "show");
		$(x[i]).css("display","block");}
		$(x[i]).addClass("dispLoc");
		count++;
	}
	else
	{
	if (x[i].className.indexOf(c) > -1 && x[i].className.indexOf(loc) > -1)
	{
		
		//w3AddClass(x[i], "show");
		$(x[i]).css("display","block");
		$(x[i]).addClass("dispLoc");
		
	}
	}
  }
  var p=0;
  for (i = 0; i < x.length; i++)
  {
	  
	  if($(x[i]).hasClass("dispLoc"))
	  {
		  var cArrList = $(x[i]).attr("class").split(/\s+/);
		  cList[p]=  cArrList[5];
		  p++;
		  
	  }
  }
 var uniq = $.distinct(cList);
	for(var k=0; k<stateLoc.length;k++)
	{
		
		var na="loc-"+stateLoc[k];
		$('#'+na).css("display","none");
		for(var l=0; l<uniq.length;l++)
		{
			if("loc-"+uniq[l] == "loc-"+stateLoc[k])
			{
				
				$('#'+na).css("display","block");
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
  var cList=[];
  var statePow =['W0-20','W21-50','W51-100','W101-300','W301-600','W600'];
  $(".pagination_cont").hide();
  x = document.getElementsByClassName("column");
  if (c == "all") c = "all";
  $("#filterLocation").val(c);
  var mw=$("#filterMW").val();
  for (i = 0; i < x.length; i++) {
   // w3RemoveClass(x[i], "show");
   $(x[i]).css("display","none");
   $(x[i]).removeClass("dispPow");
	if(c=="" || mw=="")
	{
    if (x[i].className.indexOf(c) > -1) {
		
	//w3AddClass(x[i], "show");
	//w3AddClass(x[i], "d-block");
	$(x[i]).css("display","block");
	$(x[i]).addClass("dispPow");
	
	}
	}
	else if(c=="all" && mw=="all")
	{
	if(count<=5)	{
		//w3AddClass(x[i], "show");
		$(x[i]).css("display","block");}
		$(x[i]).addClass("dispPow");
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
		$(x[i]).addClass("dispPow");
		
		
		
	}
	}
  }
  
    var q=0;
  for (i = 0; i < x.length; i++)
  {
	  
	  if($(x[i]).hasClass("dispPow"))
	  {
		  var cArrList = $(x[i]).attr("class").split(/\s+/);
		  cList[q]=  cArrList[6];
		  q++;
		  
	  }
  }
 var uniq = $.distinct(cList);
	for(var k=0; k<statePow.length;k++)
	{
		
		var na="pow-"+statePow[k];
		$('#'+na).css("display","none");
		for(var l=0; l<uniq.length;l++)
		{
			if("loc-"+uniq[l] == "lsolarprojectoc-"+statePow[k])
			{
				
				$('#'+na).css("display","block");
			}
		}
	}
  
  if(c=="all" && mw=="all")
		{
		 $(".pagination_cont").show();
		 $('.filter2').text(c);
		}
	if(c=="all")
	{
		$('.filter2').text("All");
	}
	else
	{
		$('.filter2').text(c);
	}
  
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
	  
	  if(replacelocation != "Gujarat")
	  {  $(".SolarfilterReadMore").removeClass("d-none");
	  var href = "/solar-power?state=" + replacelocation+"#filter-section";
	  $(".SolarfilterReadMore").attr("href",href);
	  }
	  
	  $(".WindfilterReadMore").removeClass("d-none");
	  var href = "/windpower?state=" + replacelocation+"#filter-section";
	  $(".WindfilterReadMore").attr("href",href);
	  
	  
	  $(".HybirdfilterReadMore").removeClass("d-none");
	  var href = "/hybridpower?state=" + replacelocation+"#filter-section";
	  $(".HybirdfilterReadMore").attr("href",href);
	  
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