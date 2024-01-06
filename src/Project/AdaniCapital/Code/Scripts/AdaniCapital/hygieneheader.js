$('.top_bar_nav').click(function(){
	$(this).toggleClass('active');
  });
  
  $('.hamburger_icon').click(function(){
	$('.hamburger').toggleClass('active');
	$('.overlay').toggleClass('active');
$('body').toggleClass('overflow-hidden');
  });
  
$('.back_header').click(function(){
	$('.hamburger_main').toggleClass('in_active');
	$('.hamburger_submenu').toggleClass('active');
  });

  $('.overlay').click(function(){
	$('.hamburger').toggleClass('active');
	$('.overlay').toggleClass('active');
$('body').toggleClass('overflow-hidden');
$('.hamburger_main').removeClass('in_active');
$('.hamburger_submenu').removeClass('active');
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
     $('.tabs li:first a').click();

$('.hamburger_login').click(function(){
$('.hamburger').removeClass('active');
$('.overlay').removeClass('active');
$('.hamburger_main').removeClass('in_active');
$('.hamburger_submenu').removeClass('active');
$('.overflow-hidden').removeClass('overflow-hidden');
});