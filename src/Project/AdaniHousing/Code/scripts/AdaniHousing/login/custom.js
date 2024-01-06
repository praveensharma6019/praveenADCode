$('.sidebar-nav-dropdown').click(function () {
	// $('.topMenu .dropdown.active').removeClass('active');
	// $(this).toggleClass('active');
	if ( $(this).hasClass('show') ) {
		$(this).removeClass('show');
	} else {
		$('.sidebar-nav-dropdown').removeClass('show');
		$(this).addClass('show');
	}
});


$('.wrapper-header-avatar .has-submenu').hover(function () {
	// $('.topMenu .dropdown.active').removeClass('active');
	// $(this).toggleClass('active');
	if ( $(this).hasClass('show') ) {
		$(this).removeClass('show');
	} else {
		$('.wrapper-header-avatar .has-submenu').removeClass('show');
		$(this).addClass('show');
	}
});

$('.header-toggler').click(function(){
	$('#sidebar').toggleClass('sidebar-fixed-show sidebar-fixed-md');
	$('.overlay').toggleClass('show');
	$('.content-wrapper').toggleClass('show');
	$('footer').toggleClass('footer-lg footer-md');
});

$('.overlay').click(function(){
	$('#sidebar').toggleClass('sidebar-fixed-show sidebar-fixed-md');
	$('.overlay').toggleClass('show');
	$('.content-wrapper').toggleClass('show');
	$('footer').toggleClass('footer-lg footer-md');
});



// Activate Next Step

$(document).ready(function() {
    
    var navListItems = $('ul.setup-panel li a'),
        allWells = $('.setup-content');

    allWells.hide();

    navListItems.click(function(e)
    {
        e.preventDefault();
        var $target = $($(this).attr('href')),
            $item = $(this).closest('li');
        
        if (!$item.hasClass('disabled')) {
            navListItems.closest('li').removeClass('active');
            $item.addClass('active');
            allWells.hide();
            $target.show();
        }
    });
    
    $('ul.setup-panel li.active a').trigger('click');
    
    // DEMO ONLY //
    $('#activate-step-2').on('click', function(e) {
        $('ul.setup-panel li:eq(1)').removeClass('disabled');
        $('ul.setup-panel li a[href="#step-2"]').trigger('click');
        $(this).remove();
    })
    
    $('#activate-step-3').on('click', function(e) {
        $('ul.setup-panel li:eq(2)').removeClass('disabled');
        $('ul.setup-panel li a[href="#step-3"]').trigger('click');
        $(this).remove();
    })
    
    $('#activate-step-4').on('click', function(e) {
        $('ul.setup-panel li:eq(3)').removeClass('disabled');
        $('ul.setup-panel li a[href="#step-4"]').trigger('click');
        $(this).remove();
    })
    
    $('#activate-step-3').on('click', function(e) {
        $('ul.setup-panel li:eq(2)').removeClass('disabled');
        $('ul.setup-panel li a[href="#step-3"]').trigger('click');
        $(this).remove();
    })
	
	$('.offers_carousel').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        items: 1,
        nav: false,
        dots: true,
        slideSpeed: 2000,
        smartSpeed: 1500
    });

});


$('.inputdate').datepicker({
});