$(document).ready(function(){
	
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
	$('.inner-banner').owlCarousel({
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
	
$('.campus-carousel').on('initialized.owl.carousel changed.owl.carousel', function(e) {
    if (!e.namespace)  {
      return;
    }
    var carousel = e.relatedTarget;
    $('.campus-carousel-counter').text(carousel.relative(carousel.current()) + 1 + '/' + carousel.items().length);
  }).owlCarousel({
    items: 1,
    loop:false,
    margin:0,
    nav:true,
	dots:false,
	navText: ["<span class='fa fa-arrow-left'></span>","<span class='fa fa-arrow-right'></span>"]
  });
	
$('.news_carousel').owlCarousel({
            loop: false,
            margin: 30,
            responsiveClass: true,
            autoplayTimeout: 3000,
            autoplay: true,
            nav: true,
            dots: false,
            navText: ["<span class='fa fa-arrow-left'></span>", "<span class='fa fa-arrow-right'></span>"],
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
        });
	
});

$('#nav-button').click(function(){
	$('.fade-overlay').toggleClass('active');
	$('body').toggleClass('overflow-hidden');
	$('#back-to-top').toggleClass('d-none');
});
$('.collapse_toggle').click(function(){
	$('.navbar-collapse').removeClass('show');
	$('.fade-overlay').toggleClass('active');
	$('body').toggleClass('overflow-hidden');
	$('#back-to-top').toggleClass('d-none');
});

$('.fade-overlay').click(function(){
	$('.navbar-collapse').removeClass('show');
	$('.fade-overlay').toggleClass('active');
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


function Search() {
    //  $('#SearchBtn').submit();

    var itemId = $('#SearchBox').val();
    $.ajax({
        method: "GET",
        data: { itemId: itemId },
        url: "/api/AdaniInstitute/GetSearchResult",
        context: this,
        success: function (objects) {
            // alert("s1");
            console.log(objects);
            $('#SearchResult').html('');
            var html = "";

            for (var i = 0; i < objects.length; i++) {
                var value = objects[i]["Value"];
                if (value.indexOf("New Local Datasource Folder") > 0) {

                }
                else {
                    if (value.indexOf("AdaniInstitute/Home") > 0)
                        value = value.replace("/AdaniInstitute/Home/", "");

                    if (!value.startsWith("/"))
                        value = "/" + value;


                    html = html + " <li><a href='" + value + "'>" + objects[i]["Key"] + "</a></li>";
                }
            }
            $('#SearchResult').html(html);
            return false;

        }
    });

}


$('#SearchBox').keypress(function (event) {
    // event.preventDefault();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        window.location.href = "/searchresult?key=" + $('#SearchBox').val();
        event.preventDefault();
    }

});

function SearchRedirect() {
    window.location.href = "/searchresult?key=" + $('#SearchBox').val();
    event.preventDefault();

}

$('#SearchBox').keypress(function (event) {
    // event.preventDefault();
    var keycode = (event.keyCode ? event.keyCode : event.which);
    if (keycode == '13') {
        $('#SearchBtn').click();// Search();
        event.preventDefault();
    }

});