function openDropdown() {
    var dropdown = document.getElementById("myDropdown");
    dropdown.classList.toggle("show");
}

// header side section
const menuIcon = document.getElementById("menuIcon");
const sidebar = document.getElementById("sidebar");


/*Home Page Banner Slider*/
headerHeight = document.querySelector("header").clientHeight;
window.addEventListener("scroll", function () {
    const header = document.querySelector("header"),
        scroll = window.pageYOffset | document.body.scrollTop;

    if (scroll > headerHeight) {
		$('.header--top_bar').addClass('d-none');
        header.className = "header sticky";
    } else if (scroll <= headerHeight) {
		$('.header--top_bar').removeClass('d-none');
        header.className = "header transparent";
    }
});
document.querySelector(".mobile-trigger").addEventListener("click", function (e) {
        e.stopPropagation();
        document.querySelector("body").classList.toggle("menu-open");
        document.querySelector("html").classList.toggle("cm-menu-open");
});
document.querySelector("body").addEventListener("click", function (ele) {
    this.classList.remove("menu-open");
    document.querySelector("html").classList.remove("cm-menu-open");
});
$(document).ready(function () {
$(".carousel").slick({
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
		speed: 1000,
        dots: true,
        autoplay: true,
        responsive: [
            {
                breakpoint: 600,
                settings: {
                    autoplay: false,
                }
            }
        ]
    });
	 $(".videocarousel").slick({
	  centerMode: true,
	  centerPadding: '300px',
	  slidesToShow: 1,
	  slidesToScroll: 1,
	  speed: 1000,
      dots: true,
	  autoplay: true,
	  pauseOnHover:true,
	  responsive: [
		{
		  breakpoint: 768,
		  settings: {
			arrows: false,
			centerMode: true,
			centerPadding: '40px',
			slidesToShow: 3
		  }
		},
		{
		  breakpoint: 480,
		  settings: {
			arrows: false,
			centerMode: true,
			centerPadding: '40px',
			slidesToShow: 1
		  }
		}
	  ]
	});
 
	$(".multiple-items").slick({
	  centerMode: true,
	  centerPadding: '450px',
	  slidesToShow: 1,
	  slidesToScroll: 1,
	  speed: 1000,
      dots: true,
	  autoplay: true,
	  arrows: true,
	  pauseOnHover:true,
	  responsive: [
		{
		  breakpoint: 768,
		  settings: {
			arrows: false,
			centerMode: true,
			centerPadding: '40px',
			slidesToShow: 3
		  }
		},
		{
		  breakpoint: 480,
		  settings: {
			arrows: false,
			centerMode: true,
			centerPadding: '40px',
			slidesToShow: 1
		  }
		}
	  ]
	});
 
 });
 
 $('.businessHeader').click(function()
 {
	 $(this).toggleClass('active');
	 $('.business_dropdown').toggleClass('showBusinessDropdown');
 })
 
 
 //home video carousel
 $(document).ready(function() {  
  $('.carousel-video').owlCarousel({

            stagePadding: 200,

            loop: true,

            margin: 10,

            items: 1,

            nav: true,

            responsive: {

                0: {
                    items: 1,
                    stagePadding: 60
                },

                600: {
                    items: 1,
                    stagePadding: 100
                },
                1000: {
                    items: 1,
                    stagePadding: 200
                },
                1200: {
                    items: 1,
                    stagePadding: 250
                },

                1400: {
                    items: 1,
                    stagePadding: 300
                },

                1600: {
                    items: 1,
                    stagePadding: 350
                },

                1800: {
                    items: 1,
                    stagePadding: 400
                }
            }
        })

        var playerSettings = {
            controls: ['play-large'],
            fullscreen: { enabled: false },
            resetOnEnd: true,
            hideControls: true,
            clickToPlay: true,
            keyboard: false,
        }

        const players = Plyr.setup('.js-player', playerSettings);

        players.forEach(function (instance, index) {
            instance.on('play', function () {
                players.forEach(function (instance1, index1) {
                    if (instance != instance1) {
                        instance1.pause();
                    }
                });
            });
        });

        $('.video-section').on('translated.owl.carousel', function (event) {
            players.forEach(function (instance, index1) {
                instance.pause();
            });
        });
		 });
		 
		
  