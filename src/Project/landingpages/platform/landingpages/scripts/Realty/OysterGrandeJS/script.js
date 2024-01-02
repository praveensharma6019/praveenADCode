
$('.navbar a').on('click', function (e) {
  if (this.hash !== '') {
    e.preventDefault();

    const hash = this.hash;

    $('html, body')
      .animate({
        scrollTop: $(hash).offset().top
      },800);
  }
});


 const scroll = new SmoothScroll('.navbar a[href*="#"]', {
	speed: 500
});


 $(document).ready(function() {
    // Configure/customize these variables.
    var showChar = 290;  // How many characters are shown by default
    var ellipsestext = "";
    var moretext = "Read more";
    var lesstext = "Read Less";
    

    $('.more').each(function() {
        var content = $(this).html();
 
        if(content.length > showChar) {
 
            var c = content.substr(0, showChar);
            var h = content.substr(showChar, content.length - showChar);
 
            var html = c + '<span class="moreellipses">' + ellipsestext+ '&nbsp;</span><span class="morecontent"><span>' + h + '</span>&nbsp;&nbsp;<a href="" class="morelink">' + moretext + '</a></span>';
 
            $(this).html(html);
        }
 
    });
 
    $(".morelink").click(function(){
        if($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
	$('#phone').on('keypress', function(key) {
		if(key.charCode < 48 || key.charCode > 57) return false;
	});
	
	$('#phone_f').on('keypress', function(key) {
		if(key.charCode < 48 || key.charCode > 57) return false;
	});
});