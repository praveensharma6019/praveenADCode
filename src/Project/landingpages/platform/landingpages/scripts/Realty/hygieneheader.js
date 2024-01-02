$('.top_bar_nav').click(function(){
	$(this).toggleClass('active');
  });
  
  $('.hamburger_icon').click(function(){
	$('.hamburger').toggleClass('active');
	$('.header_overlay').toggleClass('active');
$('body').toggleClass('overflow-hidden');
  });
  
$('.back_header').click(function(){
	$('.hamburger_main').toggleClass('in_active');
	$('.hamburger_submenu').toggleClass('active');
  });

  $('.header_overlay').click(function(){
	$('.hamburger').toggleClass('active');
	$('.header_overlay').toggleClass('active');
$('body').toggleClass('overflow-hidden');
  });

 $('.hamburger_links li').click(function(){
$('.hamburger_links li').removeClass('active');
$(this).addClass('active')
});


$('.hamburger_links ul li a.hamburger_dropdown').click(function(){
	$('.hamburger_main').toggleClass('in_active');
	$('.hamburger_submenu').toggleClass('active');
     $('.hamburger_submenu--item').hide();
     $('.hamburger_links ul li a.hamburger_dropdown.active').removeClass('active');
     $(this).addClass('active');
     
     var panel = $(this).attr('href');
     $(panel).fadeIn(1000);
     return false; 
  }); 
  
  
  $('.hamburger .single_item a').click(function(){
	$('.hamburger_main').toggleClass('in_active');
	$('.hamburger_submenu').toggleClass('active');
     $('.hamburger_submenu--item').hide();
     $('.hamburger .single_item ul li a.active').removeClass('active');
     $(this).addClass('active');
     
     var panel = $(this).attr('href');
     $(panel).fadeIn(1000);
     return false; 
  }); 

$('.hamburger_links ul li a.hamburger_link--dropdown').click(function(){
	$('.hamburger_main').toggleClass('in_active');
	$('.hamburger_submenu').toggleClass('active');
     $('.hamburger_submenu--item').hide();
     $('.hamburger_links ul li a.hamburger_link--dropdown.active').removeClass('active');
     $(this).addClass('active');
     
     var panel = $(this).attr('href');
     $(panel).fadeIn(1000);
     return false; 
  }); 

     $('.tabs li:first a').click();

$('.hamburger_login').click(function(){
$('.hamburger').removeClass('active');
$('.overlay').removeClass('active');
});


/*Menu Hover JS*/
$(".scale-anm")
    .on("mouseenter", function (e) {
        if ($($(e.currentTarget)[0]).find('img').length > 0) {
            for (var i = 0; i < $("#ulimg").children().length - 1; i++) {
                $($($("#ulimg").children())[i]).addClass("hide-ul");
            }
            if (($("#ulimg :nth-child(4)")[0]).style.display == "none") {
                ($("#ulimg :nth-child(4)")[0]).style.display = "block";
            }
            if ($($(e.currentTarget)[0]).find('img')[0] != undefined) {
                $($(e.currentTarget)[0]).find('img').clone().appendTo($("#limenu"))
                $($("#limenu")[0]).children()[0].style.display = "block";
            }
            //$($($($("#ulimg").children())[$("#ulimg").children().length - 1])).find('img')[0].src = $($(e.currentTarget)[0]).find('img')[0].src;
        }
    })
    .on("mouseleave", function (e) {
        for (var i = 0; i < $("#ulimg").children().length - 1; i++) {
            $($($("#ulimg").children())[i]).removeClass("hide-ul");
        }
        for (var i = 0; i <= $("#limenu").children().length; i++) {
            $($("#limenu").children()).remove(i);
        }
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
    $('header').addClass('fixed-header');

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
        $('header').removeClass('fixed-header');
        //$('.btm-floating').removeClass('active');
    }
    lastScrollTop = st;
}




$(window).scroll(function(){
    if ($(this).scrollTop() > 750) {
	$('#ymPluginDivContainerInitial').addClass('active');
	$('.enquiryBtn').addClass('active');
    } else {
	$('#ymPluginDivContainerInitial').removeClass('active');
	$('.enquiryBtn').removeClass('active');
    }
});





