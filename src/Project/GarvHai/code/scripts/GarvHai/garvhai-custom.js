

$(document).ready(function(){

 $('.ContactUsSocial').html('<ul class="ul-social-share"> <li><a href="https://www.facebook.com/AdaniOnline" target="_blank"><i class="fab fa-facebook-f"></i></a></li> <li><a href="https://twitter.com/adanionline" target="_blank"><i class="fab fa-twitter"></i></a></li> <li><a href="https://www.instagram.com/adanionline/" target="_blank"><i class="fab fa-instagram"></i></a></li> <li><a href="https://www.youtube.com/user/AdaniOnline" target="_blank"><i class="fab fa-youtube"></i></a></li> <li><a href="https://www.linkedin.com/company/adani-group/" target="_blank"><i class="fab fa-linkedin-in"></i></a></li> </ul>');

 
 /*Scroll*/
  $("#nav-tab-about a").click(function(event){
	var y = $(window).scrollTop();
	y = y+150;
        $('html, body').animate({scrollTop: y}, 800);
    }); 



$(function() {
  $( "input[data-sc-field-name*='FirstName']" ).keydown(function (e) {  
    if (e.shiftKey || e.ctrlKey || e.altKey) {    
      e.preventDefault();      
    } else {    
      var key = e.keyCode;      
      if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {      
        e.preventDefault();        
      }    }    
  });  
});


$(function() {
  $( "input[data-sc-field-name*='Surname']" ).keydown(function (e) {  
    if (e.shiftKey || e.ctrlKey || e.altKey) {    
      e.preventDefault();      
    } else {    
      var key = e.keyCode;      
      if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90))) {      
        e.preventDefault();        
      }    }    
  });  
});


 
if ($(".homeslider .owl-item video").prop('muted', false)){
      $("#mute").css("background-image","url(../images/GarvHai/mute.png)");
    }

  $("#mute").click( function (){
    if( $(".homeslider .owl-item video").prop('muted') ) {
      $(".homeslider .owl-item video").prop('muted', false);
      $("#mute").css("background-image","url(../images/GarvHai/volume.png)");
    } else {
      $(".homeslider .owl-item video").prop('muted', true);
      $("#mute").css("background-image","url(../images/GarvHai/mute.png)");
    }
  });
    $('.basicdetail').attr('id', 'step-1');
    $('.persondetail').attr('id', 'step-2');
    $('.parentdetail').attr('id', 'step-3');
    $('.sportsdetail').attr('id', 'step-4');



    $(".BasicDetailsSectionHeader").on('click', function (event) {
        $('.PersonalDetailsSection').slideUp();
        $('.BasicDetailsSection').slideToggle("fast");
        $('.ParentsDetailsSection').slideUp();
        $('.sportsBackgroundSection').slideUp();
    });


    $(".btnNextBasicDetail, .PersonalDetailsSectionHeader").on('click', function (event) {
        $('.step2cls').click();
		$('html, body').animate({        scrollTop: $(".step2cls").offset().top    }, 2000);
        $('.PersonalDetailsSection').slideToggle("fast");
        $('.BasicDetailsSection').slideUp();
        $('.ParentsDetailsSection').slideUp();
        $('.sportsBackgroundSection').slideUp();
    });

    $(".btnNextPersonalDetails, .ParentsDetailsSectionHeader").on('click', function (event) {
        $('.step3cls').click();
		$('html, body').animate({        scrollTop: $(".step3cls").offset().top    }, 2000);
        $('.PersonalDetailsSection').slideUp();
        $('.BasicDetailsSection').slideUp();
        $('.ParentsDetailsSection').slideToggle("fast");
        $('.sportsBackgroundSection').slideUp();
    });


    $(".btnNextParentsDetails, .sportsBackgroundSectionHeader").on('click', function (event) {
        $('.step4cls').click();
		$('html, body').animate({        scrollTop: $(".step4cls").offset().top    }, 2000);
        $('.PersonalDetailsSection').slideUp();
        $('.BasicDetailsSection').slideUp();
        $('.ParentsDetailsSection').slideUp();
        $('.sportsBackgroundSection').slideToggle("fast");
    });

$('#nav-button').click(function() {
	$('#nav-button span').toggleClass('menu-close');
$('.menu-overlay').toggleClass('d-block d-none');
                $('#mainMenu').toggleClass('d-block d-none');
				$('#mute').toggleClass('d-none'); 
 

});
$('.menu-overlay').click(function() {
                $('.menu-overlay').toggleClass('d-block d-none');
                $('#mainMenu').toggleClass('d-block d-none');
                $('#nav-button span').toggleClass('menu-close');
				$('#mute').remoClass('d-none'); 
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
        $(this).prepend('<video  muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.homeslider .owl-item.active video').attr('autoplay',true).attr('loop',true);
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

var navListItems = $('div.setup-panel div a'),
        allWells = $('.setup-content'),
        allNextBtn = $('.nextBtn');

    allWells.hide();

    navListItems.click(function (e) {
        e.preventDefault();
        var $target = $($(this).attr('href')),
            $item = $(this);
			
			
        if (!$item.hasClass('disabled')) {
            navListItems.removeClass('btn-success').addClass('btn-default');
            navListItems.parent().find('p').removeClass('active')
			$item.addClass('btn-success');
			$(this).parent().find('p').addClass('active');
			allWells.hide();
            $target.show();
            $target.find('input:eq(0)').focus();
        }
    });

    allNextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url']"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid) nextStepWizard.removeAttr('disabled').trigger('click');
    });

    $('div.setup-panel div a.btn-success').trigger('click');
})


/*Media*/
filterSelection("all") // Execute the function and show all columns
function filterSelection(c) {
  var x, i;
  x = document.getElementsByClassName("column");
  if (c == "all") c = "";
  // Add the "show" class (display:block) to the filtered elements, and remove the "show" class from the elements that are not selected
  for (i = 0; i < x.length; i++) {
      w3RemoveClass(x[i], "show fadeInUp animated");
      if (x[i].className.indexOf(c) > -1) w3AddClass(x[i], "show fadeInUp animated");
  }
}

// Show filtered elements
function w3AddClass(element, name) {
  var i, arr1, arr2;
  arr1 = element.className.split(" ");
  arr2 = name.split(" ");
  for (i = 0; i < arr2.length; i++) {
    if (arr1.indexOf(arr2[i]) == -1) {
      element.className += " " + arr2[i];
    }
  }
}

// Hide elements that are not selected
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
var btnContainer = document.getElementById("gallery-filter");
var btns = btnContainer.getElementsByClassName("btn");
for (var i = 0; i < btns.length; i++) {
  btns[i].addEventListener("click", function(){
    var current = document.getElementsByClassName("active");
    current[0].className = current[0].className.replace(" active", "");
    this.className += " active";
  });
  
 }

