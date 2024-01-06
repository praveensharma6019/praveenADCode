/*---------------------------------------------------------------------*/
;(function($){

/*================= Global Variable Start =================*/		   
var isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
var IEbellow9 = !$.support.leadingWhitespace;
var iPhoneAndiPad = /iPhone|iPod/i.test(navigator.userAgent);
var isIE = navigator.userAgent.indexOf('MSIE') !== -1 || navigator.appVersion.indexOf('Trident/') > 0;
function isIEver () {
  var myNav = navigator.userAgent.toLowerCase();
  return (myNav.indexOf('msie') != -1) ? parseInt(myNav.split('msie')[1]) : false;
}
//if (isIEver () == 8) {}
		   
var jsFolder = "js/";
var cssFolder = "css/";	
var ww = document.body.clientWidth, wh = document.body.clientHeight;
var mobilePort = 1200, ipadView = 1024, wideScreen = 1600;

/*================= Global Variable End =================*/	

//css3 style calling 
//document.write('<link rel="stylesheet" type="text/css" href="' + cssFolder +'animate.css">');	

/*================= On Document Load Start =================*/	
$(document).ready( function(){
	$('body').removeClass('noJS').addClass("hasJS");
	$(this).scrollTop(0);
	getWidth();
	
	//Set Element to vertical center using padding
	 $.fn.verticalAlign = function () {return this.css("padding-top", ($(this).parent().height() - $(this).height()) / 2 + 'px');};
	 
	setTimeout(function(){
		$('.vCenter').each(function () {$(this).verticalAlign();});
	}, 800);
	
	
// Index Banner Slider	
		if( $(".sliderBanner").length) {
		var owl = $(".sliderBanner");
		var autoplay;
		if (owl.children().length == 1) {autoplay = false}
		else {autoplay = true}
		owl.owlCarousel({
			loop:autoplay
			,autoplay:autoplay
			,mouseDrag:false
			,mouseHover:false
			,autoplayTimeout:3000
			,autoplaySpeed:800
			,smartSpeed:1200
			,animateOut: 'owlFadeOut'
    		,animateIn: 'owlFadeIn'
			,nav:false
			,lazyLoad : true
			,dots:true
			,items : 1
			,autoplayHoverPause:false
			//dots : false		
			,onInitialized: function(event) {
				if (owl.children().length > 1) { 
					 //owl.trigger('stop.owl.autoplay');
					 //this.settings.autoplay = true;
					 //this.settings.loop = true;
				}
			}
		});
			};
	
	// Inner Banner Slider	
	if( $(".videoSlider").length) {
		var owl2 = $(".videoSlider");
		var autoplay;
		if (owl2.children().length == 1) {autoplay = false	}
		else {autoplay = true}
		
		owl2.owlCarousel({
			loop:autoplay
			,autoplay:false
			,mouseDrag:autoplay
			,autoplayTimeout:3000
			,autoplaySpeed:800
			,smartSpeed:1200
			,nav:autoplay
			,dots:false
			,items : 1
			,autoplayHoverPause: true
			//dots : false		
			,onInitialized: function(event) {
				if (owl2.children().length > 1) { 
				}
			}
		});
	};
	
	
	if( $(".groupWebsites").length) {
		$('.groupWebsites').owlCarousel({
			 loop:true
			,autoplay:false
			,dots:false
			,autoplayTimeout:3000
			,smartSpeed:1200
			,margin:25
			,nav:true
			,responsive:{
				0:{
					items:1
				},
				360:{
					items:2
				},
				600:{
					items:3
				},
				800:{
					items:5
				},
				1000:{
					items:6
				}
			}
		})
	};
		 
	 
	/*swiper slider js*/
	 
	/* $(".swiperSildeOuter").lightGallery({
		loop: true,
		auto: false,
		caption: true,
		pause: 1000,
		cursor: false,
		 subHtmlSelectorRelative: true,
		
	});*/
	var swiper1 = new Swiper('.swipperGallery', {
		pagination:'.swiper-pagination-custom',
		scrollbarHide:false,
		slidesPerView:'auto',
		centeredSlides:false,
		spaceBetween:20,
		grabCursor:false,
		
	paginationClickable: true,
	nextButton:'.swiper-button-next',
	 prevButton:'.swiper-button-prev'
	 
	});
	  
	   setTimeout(function(){
	 $('.lightGallery').lightGallery({
		cssEasing:'cubic-bezier(0.680, -0.550, 0.265, 1.550)',
		closable:false,
		enableTouch: false,
		enableDrag: false,
		 subHtmlSelectorRelative: true,
		loop:true,
		speed:1500,
		addClass:'cssEasing',
		})
	}, 800);
	
	 /*end swiper*/
	
	
	if( $(".projectCarousel ").length) {
		var owl2 = $(".projectCarousel ");
		var autoplay;
		if (owl2.children().length == 1) {autoplay = false	}
		else {autoplay = true}
		
		owl2.owlCarousel({
			loop:false
			,autoplay:false
			,mouseDrag:autoplay
			,autoplayTimeout:3000
			,autoplaySpeed:800
			,smartSpeed:1200
			,nav:false
			,dots:true
			,items : 1
			,autoplayHoverPause: true
		
		});
	};
	
	if( $(".marqueeScrolling li").length > 1){
		var $mq = $('.marquee').marquee({
			 speed: 25000
			,gap: 0
			,duplicated: true
			,pauseOnHover: true
			});
		$(".btnMPause").toggle(function(){
			$(this).addClass('play');
			$(this).text('Play');
			$mq.marquee('pause');
		},function(){
			$(this).removeClass('play');
			$(this).text('Pause');
			$mq.marquee('resume');
			return false;
		});
	};
	
	// Multiple Ticker	
	 if( $(".ticker").length){
		$('.ticker').each(function(i){
			$(this).addClass('tickerDiv'+i).attr('id', 'ticker'+i);
			$('#ticker'+i).find('.tickerDivBlock').first().addClass('newsTikker'+i).attr('id', 'newsTikker'+i);
			$('#ticker'+i).find('a.playPause').attr('id', 'stopNews'+i)
			$('#newsTikker'+i).vTicker({ speed: 1E3, pause: 4E3, animation: "fade", mousePause: true, showItems: 3, height: 150, direction: 'up' })
			$("#stopNews"+i).click(function () {
				if($(this).hasClass('stop')){
					$(this).removeClass("stop").addClass("play").text("Play").attr('title', 'Play');
				}else{
					$(this).removeClass("play").addClass("stop").text("Pause").attr('title', 'pause');
				}
				return false;
			});
		});
	}; 
	
	
	
	// Responsive Tabing Script
	if( $(".resTab").length) {
		$('.resTab').responsiveTabs({
			 rotate: false
			,startCollapsed: 'tab' //accordion
			,collapsible: 'tab' //accordion
			,scrollToAccordion: true
		});
	};
				
	if( $(".accordion").length){
	   $('.accordion .accordDetail').hide();
	   $(".accordion .accordDetail:first").hide(); 
	   //$(".accordion .accTrigger:first").addClass("active");	
	   $('.accordion .accTrigger').click(function(){
		  if ($(this).hasClass('active')) {
			   $(this).removeClass('active');
			   $(this).next().slideUp();
		  } else {
			  if ($('body').hasClass('desktop')) {
			   $('.accordion .accTrigger').removeClass('active');
			   $('.accordion .accordDetail').slideUp();
			  }
			   $(this).addClass('active');			   
			   $(this).next().slideDown();
			   
				 setTimeout(function(){
					accTop = $('.desktop .accTrigger.active').offset().top -53;
					$('html, body').animate({scrollTop:accTop}, '200');
				 },600);
				 
		  }
		  return false;
	   });
	};
	var windowUrl = window.location.href;
	var jamp = windowUrl.split("/").pop();	
	if(jamp != ""){
		$(jamp+'.accTrigger').trigger( "click" );
	}
	
	$('a[rel="jampLink"]').click(function(){
		jampLink = $(this).attr('href');
		jamp = jampLink.split("/").pop();
		if(!$(jamp+'.accTrigger').hasClass('active')){
			$(jamp+'.accTrigger').trigger( "click" );
		}else{
			return false;
		}
	});	
	
	if( $(".tableData").length > 0){
		$('.tableData').each(function(){
			$(this).wrap('<div class="tableOut"></div>');
			$(this).find('tr').each(function(){
			$(this).find('td:first').addClass('firstTd');
			$(this).find('th:first').addClass('firstTh');
			$(this).find('th:last').addClass('lastTh');
			});
			$(this).find('tr:last').addClass('lastTr');
			$(this).find('tr:even').addClass('evenRow');
			$(this).find('tr:nth-child(2)').find('th:first').removeClass('firstTh');
		});	
	};
	
	// Responsive Table
	if( $(".responsiveTable").length){
		$(".responsiveTable").each(function(){		
		$(this).find('td').removeAttr('width');
		//$(this).find('td').removeAttr('align');
		var head_col_count =  $(this).find('tr th').size();
		// loop which replaces td
		for ( i=0; i <= head_col_count; i++ )  {
			// head column label extraction
			var head_col_label = $(this).find('tr th:nth-child('+ i +')').text();
			// replaces td with <div class="column" data-label="label">
			$(this).find('tr td:nth-child('+ i +')').attr("data-label", head_col_label);
		}
		});
	};
	
	// Responsive Table
	if( $(".tableScroll").length){
		$(".tableScroll").each(function(){
			$(this).wrap('<div class="tableOut"></div>');
		});
	};
	
	// Back to Top function
	if( $("#backtotop").length){
		$(window).scroll(function(){
			if ($(window).scrollTop()>120){
			$('#backtotop').fadeIn('250').css('display','block');}
			else {
			$('#backtotop').fadeOut('250');}
		});
		$('#backtotop').click(function(){
			$('html, body').animate({scrollTop:0}, '200');
			return false;
		});
	};
	
	//Tooltip
	
//tooltip
	if ($(".aboutUsMap").length > 0) {
            $(".aboutUsMap area").tooltip({
                track: true,
                hide: false,
                show: false,
				content: function() {
    			return $( this ).attr( "title" );
				}
            });
			 //$("img[usemap]").maphilight();
        }
	 					
		
		
        
	// Get Focus Inputbox
	if( $(".getFocus").length){
			$(".getFocus").each(function(){
			$(this).on("focus", function(){
			if ($(this).val() == $(this)[0].defaultValue) { $(this).val("");};
		  }).on("blur", function(){
			  if ($(this).val() == "") {$(this).val($(this)[0].defaultValue);};
		  });								  
		});
	};
	
	// For device checking
	if (isMobile == false) {
	
	};
	

	
	// JavaScript
		window.sr = ScrollReveal();
		sr.reveal('.fadeIn', { duration:1000,origin : 'bottom'});
		sr.reveal('.fadeInLeft', { duration:1500,origin : 'left'});
		sr.reveal('.fadeInRight', { duration:1500,origin : 'right'});
			
			$('.newsTenderOuter').parallax("50%", 0.4);
	
	// Video JS
	if( $(".videoplayer").length){	
		$(".videoplayer").each(function(){
			var $this = $(this);
			var poster = $this.children("a").find("img").attr("src");
			var title = $this.children("a").find("img").attr("alt");	
			var videotype = $this.children("a").attr("rel");
			var video = $this.children("a").attr("href");
			$this.children("a").remove();
			
			projekktor($this, {
			 poster: poster
			,title: title
			,playerFlashMP4: '../videoplayer/jarisplayer.swf'
			,playerFlashMP3: '../videoplayer/jarisplayer.swf'
			,width: 640
			,height: 385
			,fullscreen:true
			,playlist: [
				{0: {src: video, type: videotype}}
			],
			plugin_display: {
				logoImage: '',
				logoDelay: 1
			}
			}, function(player) {} // on ready 
			);
		})
	};
	
	// Google Map gmap3 
	if( $("#gmap").length){	
	
		var lang = 26.311202; 
		var lati =  73.050206;
		var lang = 26.915749; 
		var lati =  70.908344;
		var contentString = '<div id="content">'+
	    '<strong>' + 'Adani Renewable Energy Park Rajasthan Ltd' + '</strong>'+
	    '<div id="bodyContent">'+ 'Bhadla Phase IV, Bhadla,' + '<br/>' +
		'Jodhpur Rajastha'  
	    '</div></div>';
		var contentString = '<div id="content">'+
	    '<strong>' + 'Adani Renewable Energy Park Rajasthan Ltd' + '</strong>'+
	    '<div id="bodyContent">'+ 'Bhadla Phase IV, Bhadla,' + '<br/>' +
		'Jodhpur Rajastha'  
	    '</div></div>';
		
		 
		
		var map = new google.maps.Map(document.getElementById('gmap'), {
		zoom:15
		,center: new google.maps.LatLng(lang , lati)
		,mapTypeId: google.maps.MapTypeId.ROADMAP
		});	
		
	    var infowindow = new google.maps.InfoWindow({
	        content: contentString
	    });
		google.maps.event.addListener(map, 'click', function() {
		  infowindow.close();
		});
		var marker = new google.maps.Marker({
		  map: map,
		  position: new google.maps.LatLng(lang , lati)
		});
		google.maps.event.addListener(marker, 'click', function() {
	        infowindow.open(map,marker);
	    });
		infowindow.open(map,marker);
	};
	
// Google Map mapMultiple
	if( $("#mapMultiple").length){	
		var locations = [];
		$('.mapMultiple').each(function(){
			mapAddress = '<div class="mapHtml"> <div class="addressInMap">'+$(this).children('.heding').html()+'<br>'+$(this).children('.addressLine1').html()+',<br>'+$(this).children('.addressLine2').html()+'</div></div>';
			lat = $(this).children('.lat').html();
			lng = $(this).children('.lng').html();
			mapImg = $(this).children('.mapImg').html();
			locations.push([mapAddress,lat,lng]);
		});
      

    var map = new google.maps.Map(document.getElementById('mapMultiple'), {
      zoom:6,
      center: new google.maps.LatLng(26.871671, 73.531394),
      mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var infowindow = new google.maps.InfoWindow();

    var marker, i;

   for (var i = 0; i < locations.length; i++) {
    var marker = new google.maps.Marker({
        position: new google.maps.LatLng(locations[i][1], locations[i][2]),
        map: map,
         
    });
	
	  var infowindow = new google.maps.InfoWindow({
      content: locations[i][0],
    });
    	infowindow.open(map, marker);

		google.maps.event.addListener(marker, 'click', (function(marker, i) {
        return function() {
          infowindow.setContent(locations[i][0]);
          infowindow.open(map, marker);
        }
	
		 
      }) (marker, i));
	 
		}
	};
	
		
	
	
	
	
	
	
	
	
	
	
	
	if( $(".litebox").length){	
		$('.litebox').liteBox();
	};	
	
	if( $(".customSelect").length){	
		$('.customSelect').customSelect();
	};
	
	 $('.modalPopup').click(function(event) {
      event.preventDefault();
      $(this).modal({
        fadeDuration: 250
      });
    });
		// Search Slide Up Down
	 
	if( $("#searchToggle").length) {
		$('#searchToggle').click(function(e){
			if($(this).hasClass('active')){
				$(this).removeClass('active');
				$('.serchBlock').removeClass('active');
			}else{
				$(this).addClass('active');
				$('.serchBlock').addClass('active');
			}
		   return false;
		});
	}
		
			$('#wrapper').mouseup(function(e){
		var serchBlock = $(".serchBlock");
		if (serchBlock.has(e.target).length === 0) {
			$('#searchToggle').removeClass("active");
			$('.serchBlock').removeClass('active');
		}
	});
	
	
	
	$('.equalHeights > div').equalHeight();
	
	setTimeout (function(){
		if( $(".fixedErrorMsg").length){					 
			$(".fixedErrorMsg").slideDown("slow");				 
			setTimeout( function(){$('.fixedErrorMsg').slideUp();},5000 );
		}
		if( $(".fixedSuccessMsg").length){					 
			$(".fixedSuccessMsg").slideDown("slow");				 
			setTimeout( function(){$('.fixedSuccessMsg').slideUp();},5000 );
		}
	},500);
	
	/*================= On Document Load and Resize Start =================*/
	$(window).on('resize', function () {
									 
		ww = document.body.clientWidth; 
		wh = document.body.clientHeight;		
		
		$('.vCenter').each(function () {$(this).verticalAlign();});	
		
		if($("body").hasClass("mobilePort")){
			$("body").removeClass("wob");
		}
		
		//$('.container').resize(function(){});
		
    }).trigger('resize');
	/*================= On Document Load and Resize End =================*/
	
	/*Navigation */
	if( $("#nav").length) {
		if($(".toggleMenu").length == 0){
			$("#mainNav").prepend('<a href="#" class="toggleMenu"><span class="mobileMenu">Menu</span><span class="iconBar"></span></a>');	
		}
		$(".toggleMenu").click(function() {
			$(this).toggleClass("active");
			$("#nav").slideToggle();
			return false;
		});
		$("#nav li a").each(function() {
			if ($(this).next().length) {
				$(this).parent().addClass("parent");
			};
		})
		$("#nav li.parent").each(function () {
			if ($(this).has(".menuIcon").length <= 0) $(this).append('<i class="menuIcon">&nbsp;</i>')
		});
		dropdown('nav', 'hover', 1);
		adjustMenu();	
	};
	
	
	
if($('.datepicker').length){
	$.datepicker.setDefaults({
	  showOn: "both"
	  ,dateFormat:"dd/mm/yy"
	  ,changeMonth: true
	  ,changeYear: true
	  //,buttonImage: "images/calendar.png"
	 //,buttonImageOnly: true
	  ,shortYearCutoff: 50
	  ,buttonText: "<span class='sprite calIcon'></span>"
	  ,beforeShow: function (textbox, instance) {
		instance.dpDiv.css({
			marginTop: /*(textbox.offsetHeight)*/ 0 + 'px'
			,marginLeft: 0 + 'px'
		});
		}
	});
	
	$( ".datepicker" ).datepicker({
		   dateFormat:"dd/mm/yy"
		   ,showOn: "both"
		   ,buttonText: "<span class='sprite calIcon'></span>"
		   ,shortYearCutoff: 50
		 //,buttonImage: "images/calendar.png"
		 //,buttonImageOnly: true
		   ,beforeShow: function (textbox, instance) {
			instance.dpDiv.css({
					marginTop: /*(textbox.offsetHeight)*/ 0 + 'px'
					,marginLeft: 0 + 'px'
					});
			}
	  });	
}

if( $(".datetimepicker").length){
	$( ".datetimepicker" ).datetimepicker({
           dateFormat:"dd-mm-yy", 
           showOn: "both",
		   buttonText: "<span class='sprite calIcon'></span>",
           //buttonImage: "images/calendar.png"
           //buttonImageOnly: true,
		   beforeShow: function (textbox, instance) {
            instance.dpDiv.css({
                    marginTop: /*(textbox.offsetHeight)*/ 15 + 'px',
                    marginLeft: -13 + 'px'
            		});
    		}
      });
}



	//Header
	if($('#header').length){
		$(window).scroll(function(){
			$(window).on('resize', function(){
				if($(window).width() > 310){
					if($(window).scrollTop() > 100){
						$('#header').addClass('slidePanel animated fadeInDown');
					} else {
						$('#header').removeClass('slidePanel animated fadeInDown');
					}
				} else {
					$('#header').removeClass('slidePanel animated fadeInDown');
				}
			}).trigger('resize');
		});
	}

 
	  // Parallax start scroll
		 $('li[data-menuanchor="sectionOne"]').addClass('active')
		 $("a[href='#sectionOne']").click(function(event) {
			 event.preventDefault();
		
			 //$('li[data-menuanchor]').removeClass('active');
			 //$('li[data-menuanchor="sectionOne"]').addClass('active')
			 $("html, body").animate({
				 scrollTop: $("#sectionOne").offset().top - 100
			 }, "slow");
		
		 });
		
		 $("a[href='#sectionTwo']").click(function(event) {
			 event.preventDefault();
		
			 //$('li[data-menuanchor]').removeClass('active');
			 //$('li[data-menuanchor="sectionTwo"]').addClass('active')
			 if (!$('#mainContainer header').hasClass("headerDown")) {
		
		
				 $("html, body").animate({
					 scrollTop: $("#sectionTwo").offset().top - 120
				 }, "slow");
			 } else {
		
				 $("html, body").animate({
					 scrollTop: $("#sectionTwo").offset().top - 45
				 }, "slow");
		
		
			 }
		
		 });
		 $("a[href='#sectionThree']").click(function(event) {
			 event.preventDefault();
		
			 //$('li[data-menuanchor]').removeClass('active');
			 //$('li[data-menuanchor="sectionThree"]').addClass('active')
		
			 if (!$('#mainContainer header').hasClass("headerDown")) {
		
				 $("html, body").animate({
					 scrollTop: $("#sectionThree").offset().top - 30
				 }, "slow");
			 } else {
		
				 $("html, body").animate({
					 scrollTop: $("#sectionThree").offset().top - 10
				 }, "slow");
		
		
			 }
		
		 });
		 $("a[href='#sectionFour']").click(function(event) {
			 event.preventDefault();
		
			 //$('li[data-menuanchor]').removeClass('active');
			 //$('li[data-menuanchor="sectionFour"]').addClass('active')
		
			 if (!$('#mainContainer header').hasClass("headerDown")) {
		
		
				 $("html, body").animate({
					 scrollTop: $("#sectionFour").offset().top - 55
				 }, "slow");
			 } else {
		
				 $("html, body").animate({
					 scrollTop: $("#sectionFour").offset().top - 45
				 }, "slow");
		
		
			 }
		
		
		
		 });
		 var currentSelection = "sectionOne"
		
		 $(window).scroll(function() {
		
			 var scrollHeight = $(document).scrollTop();
			 //console.log(scrollHeight);	
			 //console.log ( " Section -> " + currentSelection ) ;
			 if (scrollHeight < 470) {
		
				 if (!$('li[data-menuanchor="sectionOne"]').hasClass("active")) {
					 $('li[data-menuanchor]').removeClass('active');
					 $('li[data-menuanchor="sectionOne"]').addClass('active')
					 currentSelection = "sectionOne";
				 }
		
		
			 } else if (scrollHeight >= 470 && scrollHeight < 900) {
				 if (!$('li[data-menuanchor="sectionTwo"]').hasClass("active")) {
		
					 $('li[data-menuanchor]').removeClass('active');
					 $('li[data-menuanchor="sectionTwo"]').addClass('active')
					 currentSelection = "sectionTwo";
				 }
		
		
		
			 } else if (scrollHeight > 899 && scrollHeight < 1400) {
				 if (!$('li[data-menuanchor="sectionThree"]').hasClass("active")) {
		
					 $('li[data-menuanchor]').removeClass('active');
					 $('li[data-menuanchor="sectionThree"]').addClass('active')
					 currentSelection = "sectionThree";
				 }
		
		
			 } else if (scrollHeight > 1399) {
				 if (!$('li[data-menuanchor="sectionFour"]').hasClass("active")) {
		
					 $('li[data-menuanchor]').removeClass('active');
					 $('li[data-menuanchor="sectionFour"]').addClass('active')
					 currentSelection = "sectionFour";
				 }
			 }
		
		
		
		 });
		 var sections = ['sectionOne', 'sectionTwo', 'sectionThree', 'sectionFour'];
		
		 function nextElement(num) {
			 var curIndex = $.inArray(num, sections);
			 if (curIndex >= 0) {
				 curIndex = curIndex + 1;
			 }
		
			 return sections[curIndex];
		 }
		
		 function prevElement(num) {
			 return sections[($.inArray(num, sections) - 1 + sections.length) % sections.length];
		 }
		
		
		 $(document).keydown(function(e) {
			 <!--e.preventDefault();-->
			 //console.log ( "current Section " + currentSelection ) ;
			 //console.log ( "Key Stroke is "  + e.keyCode );
			 if (e.keyCode == 38 || e.keyCode == 40) {
				 var element = $("li[data-menuanchor]").find("active");
				 var nextEl = "";
				 if (e.keyCode == 38) {
					 nextEl = prevElement(currentSelection)
		
				 }
				 if (e.keyCode == 40) {
					 nextEl = nextElement(currentSelection);
		
				 }
		
				 var element = "a[href='#" + nextEl + "']";
				 //console.log("Next Selection After Key Stroke: "+element );
				 $(element).trigger("click");
			 }
		
		 });
		
	
		 //End

	// project show hide
		if($("#projectMap").length){	
		$(".mapPointsDetial:first").show();
		$("#projectMap area").each(function(){
			areaid = $(this).attr('href');
			areaTitle= $(this).attr('title');
			coords = $(this).attr('coords').split(',');
			areaLeft = coords[0];
			areaTop = coords[1]-10;
			labelLeft = $(this).data('left');
			labelTop = $(this).data('top');
			$('.mapHomeImg map').before("<a class='mapMarkName' href='"+areaid+"' style='margin:"+labelTop+"px 0 0 "+labelLeft+"px; left:"+areaLeft+"px; top:"+areaTop+"px;'>"+areaTitle+"</a>")
			//marks.push("<a class='mapMarkName' href='"+areaid+"' style='left:"+areaLeft+"; top:"+areaTop+"'>"+areaTitle+"</a>");
		})
		/*$('.mapMarkName').each(function(){
			marginLeft = ($(this).width()/2)-8;
			marginTop = $(this).height()+10;
			$(this).css({'margin-left':-marginLeft,'margin-top':-marginTop})
		})*/
		
		
			$("#projectMap area, #projectMap a[href*='#']").click(function(event){	
			$(".mapPointsData").fadeIn();
			event.preventDefault();
			 var projectId = $(this).attr("href");
			 if($(projectId).hasClass('active')){
				 
			 }
			 else{
				  $(".mapPointsDetial").hide().removeClass("active");
				  $(projectId).addClass("active").fadeIn('slow');
				 }
			//$(".mapPointsDetial").removeClass("active").hide();
			//$(projectId).addClass("active").show();
			});
			$(".closeIconBtn").click(function(){
			$(".mapPointsData").fadeOut();							  
			});
			}
			$('#wrapper').mouseup(function(e){
		var serchBlock = $(".serchBlock");
		if (serchBlock.has(e.target).length === 0) {
			$('#searchToggle').removeClass("active");
			$('.serchBlock').removeClass('active');
		}
	});
	if ($(".mapHomeImg").length > 0) {
			$('img[usemap]').rwdImageMaps();		   
        }


	// contenMap
		if($("#contenMap").length){
			/*var pingDis = [];
			$('.mapPinDis li').each(function(){
				pingDis.push({$(this).text()});
			});
			console.log(pingDis)*/
			pingDis = [
								 {
									 office:"Fatehgarh",
									 CompanyName:"Adani Renewable Energy Park Rajasthan Ltd",
									 Address:"Fatehgarh, Jaisalmer, Rajasthan",
									 TelePhone:"",
									 Email:"info@areprl.com",
									// href:"#"
									},
									{
										office:"Bhadla",
										CompanyName:"Adani Renewable Energy Park Rajasthan Ltd",
										Address:"Bhadla, Jodhpur, Rajasthan",
										TelePhone:"",
										Email:"info@areprl.com",
										//href:"#"
									},
									{
										office:"Jaipur",
										CompanyName:"Adani Green Energy Ltd",
										Address:"31A, 6th Floor, Mahima Trinity, Plot 5, Swej Farm, New Sanganer Road, Sodala, Jaipur - 302019",
										TelePhone:"",
										Email:"info@areprl.com",
										//href:"#"
									}
								];
			$("#contenMap area").each(function(i){
				areaid = $(this).attr('href');
				areaTitle= $(this).attr('title');
				coords = $(this).attr('coords').split(',');
				areaLeft = coords[0];
				areaTop = coords[1]-10;
				dataPin= $(this).data('map');
				labelLeft = $(this).data('left');
				labelTop = $(this).data('top');
				$('#contenMap map').before("<div class='pinBox "+dataPin+"Pin' style='position: absolute; left:"+areaLeft+"px; top:"+areaTop+"px;'><div style='position: absolute; white-space:pre; margin:"+labelTop+"px 0 0 "+labelLeft+"px;'>"+pingDis[i]['office']+"</div><div class='pin'></div><div class='pulse'></div><blockquote class='example-obtuse'><p class='pinCompanyName'>"+pingDis[i]['CompanyName']+"</p><p class='pinAddress'>"+pingDis[i]['Address']+"</p><p class='pinTelephone'>"+pingDis[i]['TelePhone']+"</p><p class='pinEmail'>"+pingDis[i]['Email']+"</p></blockquote></div> </div>")
				//marks.push("<a class='mapMarkName' href='"+areaid+"' style='left:"+areaLeft+"; top:"+areaTop+"'>"+areaTitle+"</a>");
			});
			$('.pin').mouseenter(function(){
				$(this).parent().find('blockquote').addClass('activeBubble');
			});
			$('.pin').mouseleave(function(){
				$(this).parent().find('blockquote').removeClass('activeBubble');
			});
		}
		
		$('#contactCountry').change(function(){
			$('.pinBox').stop().fadeOut();
			showPing = $(this).val();
			if(showPing == "All"){
				$('.pinBox').stop().fadeIn();
			}else{
				$("."+showPing+"Pin").stop().fadeIn();
			}
			
		})


	
	// Message on Cookie Disabled
	$.cookie('cookieWorked', 'yes', { path: '/' });
	if ($.cookie('cookieWorked') == 'yes') {
		}
	else{
		if( $("div.jsRequired").length == 0){
			$("body").prepend(
				'<div class="jsRequired">Cookies are not enabled on your browser. Need to adjust this in your browser security preferences. Please enable cookies for better user experience.</div>'
			);	
		}
	}
	
});
/*================= On Document Load End =================*/

/*================= On Window Resize Start =================*/	
$(window).bind('resize orientationchange', function() {
	getWidth();												
	adjustMenu();
	$('.vCenter').each(function () {$(this).verticalAlign();});
});

/*================= On Window Resize End =================*/	

/*================= On Window Load Start =================*/
$(window).load(function() {
						
});
/*================= On Document Load End =================*/


function getWidth() {
	ww = document.body.clientWidth;
	if(ww>wideScreen){$('body').removeClass('device').addClass('desktop widerDesktop');}
	if(ww>mobilePort && ww<=wideScreen){	$('body').removeClass('device widerDesktop').addClass('desktop');}
	if(ww<=mobilePort) {$('body').removeClass('desktop widerDesktop').addClass('device');}
	if(ww > 767 && ww < 1025){$('body').addClass('ipad');}
	else {$('body').removeClass('ipad');}	
}



})(jQuery);

//-------------------------------------------------------------------------------------------------------------------------------------------





