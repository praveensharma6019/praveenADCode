$(document).ready(function () {
	//Active Tab 1
	var pTabItem = $(".prodNav .ptItem");
	$(pTabItem).click(function () {
		$(pTabItem).removeClass("active");
		$(this).addClass("active");
		var tabid = $(this).attr("id");
		$(".prodMain").removeClass("active");
		$("#" + tabid + "C").addClass("active");
		return false;
	});
	
	//Active Tab 2
	var psItem = $(".str_m .psItem");
	$(psItem).click(function () {
		$(psItem).removeClass("active");
		$(this).addClass("active");
		var tabid = $(this).attr("id");
		$(".str_ms").removeClass("active");
		$("#" + tabid + "s").addClass("active");
		return false;
	});
	
	var psItem_m = $(".str_d .psItem");
	$(psItem_m).click(function () {
		$(psItem_m).removeClass("active");
		$(this).addClass("active");
		var tabid = $(this).attr("id");
		$(".str_ds").removeClass("active");
		$("#" + tabid + "s").addClass("active");
		return false;
	});


	//Sticky Header
	initStickyHeader();
	function initStickyHeader() {

		"use strict";

		var win = $(window),
			stickyClass = 'sticky';

		$('#header.sticky-header').each(function() {
			var header = $(this);
			var headerOffset = header.offset().top || 0;
			var flag = true;


			$(this).css('height' , jQuery(this).innerHeight());

			function scrollHandler() {
				if (win.scrollTop() > headerOffset) {
					if (flag){
						flag = false;
						header.addClass(stickyClass);
					}
				} else {
					if (!flag) {
						flag = true;
						header.removeClass(stickyClass);
					}
				}
			}

			scrollHandler();
			win.on('scroll resize orientationchange', scrollHandler);
		});
	}
	
	initbackTop();
	// Back Top
	function initbackTop() {

		"use strict";

		var jQuerybackToTop = jQuery("#back-top");
		jQuery(window).on('scroll', function() {
			if (jQuery(this).scrollTop() > 100) {
				jQuerybackToTop.addClass('active');
			} else {
				jQuerybackToTop.removeClass('active');
			}
		});
		jQuerybackToTop.on('click', function(e) {
			jQuery("html, body").animate({scrollTop: 0}, 500);
		});
	}

    window.addEventListener("scroll", function () {
		if ($(window).width() < 768) {
			var header = document.querySelector(".footer_btn_e");
            header.classList.toggle("active", scrollY > 1400);
		}
		else {
			var footer_btn_e = document.querySelector(".section_enquire");
			footer_btn_e.classList.toggle("active", scrollY > 1200);
		}
    });
	
	$(".footer_e").click(function () {
        $(".section_enquire").show();
    });
	
	$(".close-enquiry").click(function () {
        $(".section_enquire").hide();
    });
	
	$(".btn-enquire").click(function () {
        $(".section_enquire").show();
    });
	
	$(window).on('load', function() {
		setTimeout(function() {
			$('#exampleModal').modal('show')
		}, 3000);
    });
	$('.smooth').click(function(){
		$('html, body').animate({
			scrollTop: $( $(this).attr('href') ).offset().top - 100
		}, 300);
		return false;
	});

   var syncedSecondary = true;
  
	var item = $('.owl-carousel').owlCarousel({
		
		
		autoplay:true,
		autoplayTimeout:5000,
		
		autoplayHoverPause:true,
		responsive:{
			0:{
				items:1,
				margin:10,
				loop:true,
				stagePadding: 50,
				nav:true
			},
			/*600:{
				items:1,
				margin:10,
				nav:true,
				stagePadding: 200,
				loop: true,
			},*/
			768:{
				items:1,
				margin:10,
				nav:true,
				stagePadding: 200,
				loop: true,
			},
			1000:{
				items:1,
				margin:10,
				nav:true,
				stagePadding: 250,
				loop:true
			},
			1200:{
				items:1,
				margin:30,
				nav:true,
				stagePadding: 300,
				loop:true
			}
		},

	})
	;
	$('.yBox').magnificPopup({
		type: 'image',
		gallery:{
			enabled: true
		},
	});
	
	var formInputs = $('div.form-item input');
	formInputs.focus(function() {
       $(this).parent().children('label.formLabel').addClass('formTop');
	});
	formInputs.focusout(function() {
		if ($.trim($(this).val()).length == 0){
		$(this).parent().children('label.formLabel').removeClass('formTop');
		}
	});
	$('p.formLabel').click(function(){
		 $(this).parent().children('.form-style').focus();
	});
	
	$("#crs_icon").click(function(){
		$(this).toggleClass("active_menu");
	});
	
	$('#mobile').on('keypress', function(key) {
		if(key.charCode < 48 || key.charCode > 57) return false;
	});
	
	$('#phone_number_fot').on('keypress', function(key) {
		if(key.charCode < 48 || key.charCode > 57) return false;
	});
	
	$('#b_mobile').on('keypress', function(key) {
		if(key.charCode < 48 || key.charCode > 57) return false;
	});
	
});