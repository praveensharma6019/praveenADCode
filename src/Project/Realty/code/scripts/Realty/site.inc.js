

$(document).ready(function(){
	// alert();
	$('#nav-icon').click(function (e) {
		e.preventDefault();
		$(this).toggleClass('open');
		$('ul.nav-list').toggleClass('active');
		$("ul.nav-list").toggle();
	});
	

	
	$('a.sticky-btn').click(function () {
        var thsRel = $(this).attr('rel');
        var topMinus = $('#'+thsRel).offset().top;
        scrollinAbout(topMinus);
    });
	
	
    $('.text').on('focus blur', function (e) {
		$(this).toggleClass('hascontent', (e.type === 'focus' || this.value.length > 0));
	});
	$('.textarea').on('focus blur', function (e) {
		$(this).toggleClass('hascontent', (e.type === 'focus' || this.value.length > 0));
	});

});
function scrollinAbout(topMinus) {
    $("html, body").animate({ scrollTop: topMinus - 130},1000);
}