/*var recaptcha1;
 var onloadCallback = function () { 


    //Render the recaptcha2 on the element with ID "recaptcha2" 
    recaptcha1 = grecaptcha.render('recaptcha1', { 
       'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc', //Replace this with your Site key 
        'theme': 'light' 
    });


 };*/
 
 


$(document).ready(function () {

    // var owl = $('.homeslider');
    // owl.owlCarousel({
    // loop:false,
    // margin:0,
    // lazyLoad: true,
    // nav:false,
    // dots:true,
    // autoHeight:true,
    // autoplay: true,
    // autoplayTimeout: 2000,
    // slideSpeed: 500,
    // items:1
    // })
    // owl.on('translate.owl.carousel',function(e){
    // $('.homeslider .owl-item video').each(function(){
    // $(this).get(0).pause();
    // $(this).get(0).currentTime = 0;
    // });
    // });
    // $('.homeslider .owl-item .item').each(function(){
    // var attr = $(this).attr('data-videosrc');
    // if (typeof attr !== typeof undefined && attr !== false) {
    // var videosrc = $(this).attr('data-videosrc');
    // $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="'+videosrc+'" type="video/mp4"></video>');
    // $('.homeslider .owl-item.active video');
    // }
    // });
    // owl.on('translated.owl.carousel',function(e){
    // $('.homeslider .owl-item.active video').get(0).play();
    // });

    var owl = $('.main-slider');
    owl.owlCarousel({
        autoplay: true,
        autoplayTimeout: 4000,
        loop: true,
        items: 1,
        center: true,
        dots: false,
        nav: true,
        thumbs: true,
        thumbImage: false,
        thumbsPrerendered: true,
        thumbContainerClass: 'owl-thumbs',
        thumbItemClass: 'owl-thumb-item',
        navText: ['<span class="prev">ï¼œ</span>', '<span class="next">ï¼ž</span>'],
    });

    $('.ProjectAssetsDataInner').hide();
    $('#DefaultData').show();
    $('#DefaultData2').show();
    $('#buss-owl').owlCarousel({
        items: 2,
        itemsDesktop: [1000, 2],
        itemsDesktopSmall: [900, 2],
        itemsTablet: [640, 1],
        pagination: true,

    });

    $('.pointer').click(function () {
        $('.tab-panea').hide();
        $('.ProjectAssetsDataInner').hide();
        var getTabId = $(this).attr('rel');
        $('#' + getTabId).show();
        $(".indiaMap a").attr("class", "pointer");
        $(this).attr("class", "active");
    });
    $('.homeslider-other').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 5000,
        autoplay: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
		autoplayHoverPause:true,
        nav: false,
        dots: true,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })
	$('.stop').on('click',function(){
    owl.trigger('stop.owl.autoplay');
})
var owl = $('.homeslider-other');
owl.owlCarousel({
    loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 5000,
        autoplay: true,
        animateOut: 'zoomOut',
        animateIn: 'zoomIn',
		autoplayHoverPause:true,
        nav: false,
        dots: true,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
});
$('.stop').on('click',function(){
    owl.trigger('stop.owl.autoplay');
	$(this).toggleClass('active');
})

	
        $('.testimonials').owlCarousel({
            loop: true,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 3000,
            autoplay: true,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        }),
        $('.service-carousel').owlCarousel({
            loop: true,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 3000,
            autoplay: false,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 2
                },
                500: {
                    items: 2
                },
                1000: {
                    items: 5
                }
            }
        }),
		$('.notification_carousel').owlCarousel({
            loop: true,
            margin: 100,
            responsiveClass: true,
            autoplayTimeout: 3000,
            autoplay: false,
            nav: false,
            dots: true,
            items: 1
        }),
	$('.airport-section__carousel-3item')
	.owlCarousel({
        loop: true,
        margin: 20,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3,
				margin:40,
				loop: false
            }
        }
    }),
	$('.airport-section__carousel-4item')
	.on('changed.owl.carousel initialized.owl.carousel', function(event) {
    $(event.target)
      .find('.owl-item').removeClass('last')
      .eq(event.item.index + event.page.size - 1).addClass('last');
  })
	.owlCarousel({
        loop: false,
        margin: 40,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: false,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
				margin: 20,
				loop: false
            },
            600: {
                items: 2
            },
            1000: {
                items: 4
            }
        }
    }),
	$('.airport-section__carousel-5item')
	.on('changed.owl.carousel initialized.owl.carousel', function(event) {
    $(event.target)
      .find('.owl-item').removeClass('last')
      .eq(event.item.index + event.page.size - 1).addClass('last');
  })
	.owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplayTimeout: 6000,
        autoplay: true,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 2.2,
				margin: 20,
				loop: true,
				nav:false
            },
            600: {
                items: 3,
				loop: true,
				nav: false
            },
            1000: {
                items: 5,
				loop: true,
				nav: true
            }
        }
    }),
 $('.business-carousel').owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: false,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    }),
        $('.timeline-carousel').owlCarousel({
            loop: false,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 3000,
            touchDrag: false,
            mouseDrag: false,
            autoplay: false,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 3
                }
            }
        }),
        $('.our-team').owlCarousel({
            loop: false,
            margin: 50,
            responsiveClass: true,
            autoplayTimeout: 3000,
            touchDrag: false,
            mouseDrag: false,
            autoplay: false,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 4
                }
            }
        }),
        $('.sponsor-carousel').owlCarousel({
            loop: false,
            margin: 25,
            responsiveClass: true,
            autoplayTimeout: 1500,
            autoplay: true,
            nav: false,
            dots: true,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 2
                }
            }
        }),
        $('.gallery-carousel').owlCarousel({
            loop: true,
            margin: 0,
            responsiveClass: true,
            autoplayTimeout: 1500,
            autoplay: true,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 2,
                    nav: false
                },
                600: {
                    items: 2
                },
                1000: {
                    items: 3
                }
            }
        }),
        //home page text update on sustainability Tile
        $($(".sustainabilityHomeTile h3")[1]).text("Social");
		/*Home VIdeo Section*/
	var sync1 = $(".sync1");
  var sync2 = $(".sync2");
 
  sync1.owlCarousel({
    loop: true,
	margin: 0,
	nav: false,
	dots: false,
	navText: ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"],
	responsiveClass: true,
	responsive: {
	  0: {
		items: 1,
		dots:false,
		nav:false
	  },
	  600: {
		items: 1,
		dots:false,
		nav:false
	  },
	  1000: {
		items: 1
	  }
	}
  });
 
  sync2.owlCarousel({
    items : 2,
    pagination:false,
	nav:false,
	dots:false,
    responsiveRefreshRate : 100,
    afterInit : function(el){
      el.find(".owl-item").eq(0).addClass("synced");
    }
  });
 
  function syncPosition(el){
    var current = this.currentItem;
    $(".sync2")
      .find(".owl-item")
      .removeClass("synced")
      .eq(current)
      .addClass("synced")
    if($(".sync2").data("owlCarousel") !== undefined){
      center(current)
    }
  }
 
  $(".sync2").on("click", ".owl-item", function(e){
    e.preventDefault();
    var number = $(this).data("owlItem");
    sync1.trigger("owl.goTo",number);
  });
 
  function center(number){
    var sync2visible = sync2.data("owlCarousel").owl.visibleItems;
    var num = number;
    var found = false;
    for(var i in sync2visible){
      if(num === sync2visible[i]){
        var found = true;
      }
    }
 
    if(found===false){
      if(num>sync2visible[sync2visible.length-1]){
        sync2.trigger("owl.goTo", num - sync2visible.length+2)
      }else{
        if(num - 1 === -1){
          num = 0;
        }
        sync2.trigger("owl.goTo", num);
      }
    } else if(num === sync2visible[sync2visible.length-1]){
      sync2.trigger("owl.goTo", sync2visible[1])
    } else if(num === sync2visible[0]){
      sync2.trigger("owl.goTo", num-1)
    }
    
    }

    var $fileUploadTndr = $(".tenderFileCTR input[type='file']");
      $fileUploadTndr.change(function () {
          if (parseInt($fileUploadTndr.get(0).files.length) > 5) {
              alert("You are only allowed to upload a maximum of 5 files");
              $fileUploadTndr.val(''); 
          }
      });
  
      var $fileUploadPQ = $(".pqFileCTR input[type='file']");
      $fileUploadPQ.change(function () {
          if (parseInt($fileUploadPQ.get(0).files.length) > 5) {
              alert("You are only allowed to upload a maximum of 5 files");
              $fileUploadPQ.val('');
          }
      });

    if (window.location.href.indexOf("/Business") > -1) {
        $('.navbar-brand__middle a').removeClass("active")
        $('.navbar-brand__middle .business').addClass("active")
    }
});


$('.click-scroll a').on('click', function (e) {
    var href = $(this).attr('href');
    $('html, body').animate({
        scrollTop: $(href).offset().top - 70
    }, '600');
    e.preventDefault();
});

$('.scroll-actions a').on('click', function (e) {
    var href = $(this).attr('href');
    $('html, body').animate({
        scrollTop: $(href).offset().top - 120
    }, '600');
    e.preventDefault();
});

$(function () {
    $('#investor-select select').change(function () {
        $('.investor-m-block').hide();
        $('.inv-tabs-query').hide();
        $('.tab-inv').hide();
        $('.' + $(this).val()).show();
    });
});


function CloseModal(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
}

$('body').on('hidden.bs.modal', '.modal', function () {
    $('video').trigger('pause');
});

$(".view-all video").each(function () {
    $(this).get(0).pause();
});


$(document).ready(function () {
    $('.modal .close').click(function () {

        var videoId = $(this).attr('Id');
        CloseModal(videoId);

    });

$('.sitemap-link').click(function() {
	$('footer .ft-submenu').toggleClass('d-none d-block');
    $('.sitemap-link').toggleClass('sitemap-link-active');
	$('html, body').animate({
		scrollTop: $(".sitemap-link").offset().top
			- 100
	}, 1000);
});

    $('body').on('hidden.bs.modal', '.modal', function () {
        $('video').trigger('pause');
    });
    $('.topMenu .dropdown').click(function () {
		// $('.topMenu .dropdown.active').removeClass('active');
        // $(this).toggleClass('active');
		if ( $(this).hasClass('active') ) {
			$(this).removeClass('active');
		} else {
			$('.topMenu .dropdown').removeClass('active');
			$(this).addClass('active');
		}
    });
    /*$('.topMenu .dropdown a').click(function () {
        $('.topMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');
    });*/
    $('.topMenu .dropdown').click(function () {
        $(this).parent('.topMenu .dropdown:after').css('transform', 'rotate(0deg)');
    });

$('#mainMenu .dropdown-item').click(function(){
$('#mainMenu').removeClass('show');
$('.menu-overlay').removeClass('d-block')
$('.menu-overlay').addClass('d-none')
$('.primaryMenu li').removeClass('active');
$('.primaryMenu li:first-child').addClass('active');
});


    $('.scroll-down a').on('click', function (e) {
        var href = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(href).offset().top - 70
        }, '600');
        e.preventDefault();
    });

    $('#other-ventures').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 2,
                nav: true,
                dots: false
            },
            600: {
                items: 3,
                nav: true,
                dots: false
            },
            1000: {
                items: 7,
                nav: true,
                dots: false,
                margin: 20
            }
        }
    })

    $('.double-carousel').owlCarousel({
        loop: false,
        margin: 10,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-arrow-left'></i>", "<i class='fa fa-arrow-right'></i>"],
        responsiveClass: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 2
            },
            1000: {
                items: 2
            }
        }
    })
    $('.infrastructure-block').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        nav: true,
        dots: false,
        autoplay: false,
        autoplaytimeout: 1500,
        slideSpeed: 1500,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 2
            },
            1000: {
                items: 2
            }
        }
    })

    $('.single-carousel').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoHeight: false,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false
            },
            600: {
                items: 1,
                nav: true,
                dots: false
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                margin: 10
            }
        }
    })



    $('.case-study').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false
            },
            600: {
                items: 1,
                nav: true,
                dots: false
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                margin: 20
            }
        }
    })
    $('.single-item').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: true,
        autoplayTimeout: 1500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false
            },
            600: {
                items: 1,
                nav: true,
                dots: false
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                margin: 20
            }
        }
    })

    $('#blog-more').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        autoplay: false,
        autoplayTimeout: 2500,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        autoplayHoverPause: false,
        responsive: {
            0: {
                items: 1,
                nav: true,
                dots: false
            },
            600: {
                items: 2,
                nav: true,
                dots: false
            },
            1000: {
                items: 2,
                nav: true,
                dots: false,
                margin: 20
            }
        }
    })

    // Fakes the loading setting a timeout
    setTimeout(function () {
        $('body').addClass('loaded');
    }, 1500);


    /*Inner Video Slider*/
    var owl = $('.innerv-carousel');
    owl.owlCarousel({
        loop: false,
        margin: 0,
        nav: false,
        dots: false,
        autoHeight: true,
        lazyload: true,
        items: 1
    })
    owl.on('translate.owl.carousel', function (e) {
        $('.innerv-carousel .owl-item video').each(function () {
            $(this).get(0).pause();
        });
    });
    $('.innerv-carousel .owl-item .item').each(function () {
        var attr = $(this).attr('data-videosrc');
        if (typeof attr !== typeof undefined && attr !== false) {
            var videosrc = $(this).attr('data-videosrc');
            $(this).prepend('<video muted><source src="' + videosrc + '" type="video/mp4"></video>');
            $('.innerv-carousel .owl-item.active video').attr('autoplay', true).attr('loop', true);
        }
    });
    owl.on('translated.owl.carousel', function (e) {
        $('.innerv-carousel .owl-item.active video').get(0).play();
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
        $('.headerSec .navPanel').addClass('sticky-header');
        $('.headerSec .header--top_bar').addClass('header--top_bar--fixed');
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
            $('.headerSec .navPanel').removeClass('sticky-header');
            $('.headerSec .header--top_bar').removeClass('header--top_bar--fixed');
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

    //for newsroom
    $(function () {
        $('#btn-mrelease').change(function () {
            $('.media-r').hide();
            $('.m-loadMore').hide();
            $('#' + $(this).val()).show();
        });
    });


    $('#footerArrow').click(function () {
        $('.footerPanel2 .mobile-none').toggleClass('d-none');
        $('.footerPanel2 .footerpanel-1').toggle(100);
        $('.footerPanel2 .txt-center').toggle(100);
        $('.footerPanel2 .ft-submenu').toggleClass('col-lg-12');
    });


    $('#sustanibility').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        nav: false,
        dots: false,
        autoplay: false,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1,
                nav: true
            },
            600: {
                items: 2,
                nav: true
            },
            1000: {
                items: 3,
                touchDrag: false,
                mouseDrag: false,
            }
        }
    })

    $('#resources').owlCarousel({
        loop: false,
        margin: 0,
        responsiveClass: true,
        nav: false,
        dots: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 2
            },
            1000: {
                items: 2
            }
        }
    })

    // $('#other-ventures').owlCarousel({
    // loop: true,
    // margin: 30,
    // autoplaytimeout: 2000,
    // animateOut: 'fadeOutLeft',
    // animateIn: 'fadeInRight',
    // slideSpeed: 1500,
    // autoplay: true,
    // responsiveClass: true,
    // responsive: {
    // 0: {
    // items: 2,
    // nav: true,
    // dots: false
    // },

    // 300: {
    // items: 2,
    // nav: false,
    // dots: false
    // },
    // 420: {
    // items: 3,
    // nav: false,
    // dots: false
    // },
    // 576: {
    // items: 4,
    // nav: false,
    // dots: true
    // },
    // 768: {
    // items: 4,
    // nav: false,
    // dots: true
    // },
    // 1000: {
    // items: 6,
    // nav: false,
    // loop: false
    // }
    // }
    // })


    $('.single-slide').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 3000,
        autoplay: true,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    }),
	$('.partner-carousel').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 2000,
        autoplay: true,
        nav: true,
        dots: false,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 2
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    }),
    $('#about-banner').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 2000,
        autoplay: true,
        autoHeight: true,
        mouseDrag: true,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
            },
            960: {
                items: 1,
                nav: false,
                dots: false,
            },
            1100: {
                items: 1,
                nav: true,
                dots: false,
            }
        }
    })
    $('#pop-gallery').owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
        dots: true,
        autoplay: true,
        autoplayTimeout: 2500,
        responsiveClass: true,
        navText: [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        slideSpeed: 1000,
        responsive: {
            0: {
                items: 1,
                nav: true
            },
            600: {
                items: 3,
                nav: true
            },
            1000: {
                items: 3,
                nav: true
            }
        }
    });

    // $(".popup").magnificPopup({
    // type: "image",
    // removalDelay: 160,
    // preloader: false,
    // fixedContentPos: true,
    // gallery: {
    // enabled: true
    // }
    // });


    $(function () {
        $('#btn-mrelease').change(function () {

            $("#business-parent .col-sm-6").hide();
            $('#business-parent .' + $(this).val()).show();

        });
    });




    /* Menu on mobile devices */

    $('#dismiss, .overlay, .overlay-top').on('click', function () {
        $('#sideNav').removeClass('active');
        $('.overlay, .overlay-top').removeClass('active');
        $('#topMenu').removeClass('active');
		$('body').toggleClass('overflow-hidden');
    });

    /*Side menu on mobile devices*/

    $('#sidebarCollapse').on('click', function () {
        $('#sideNav, #mainContent').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('.overlay').addClass('active');
		$('body').toggleClass('overflow-hidden');
        //$('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

    /* Top menu on mobile devices*/

    $('#topNavCollapse').on('click', function () {
        $('#topMenu').toggleClass('active');
        $('.collapse.in').toggleClass('in');
        $('.overlay-top').toggleClass('active');
        $('body').toggleClass('overflow-hidden');

    });

    // $('[data-toggle="tooltip"]').tooltip();

    $(".loadmore-content p").slice(0, 1).show();
    $(".loadMore").on("click", function (e) {
        e.preventDefault();
        $(".loadMore").toggleClass("d-block d-none");
        $(".loadmore-content p").toggleClass("d-none");
        $(".loadmore-content ul").toggleClass("d-none");
        $(".loadmore-content ul li").toggleClass("d-none d-list");
        $("p.loadMore").text("").show(1000);
        $(".arrow.loadMore").text("Read more ").show(1000);
        $('<i class="fa fa-chevron-down"/>').appendTo('.arrow.loadMore');
        if ($(".loadmore-content p:hidden").length == 0) {
            $(".arrow.loadMore").text("Read less ").show(1000);
            $(' <i class="fa fa-chevron-up"/>').appendTo('.arrow.loadMore');
            $(".arrow.loadMore").show(1000);
        }

    });

    $('.viewall').click(function () {
        $('.ft-business li').toggleClass('d-none d-block');
        $(".viewall").text("+ View more ").show(1000);
        if ($(".ft-business li:hidden").length == 0) {
            $(".viewall").text("- View less ").show(1000);
            $(".viewall").show(1000);
        }
    });


    $('.nav-border-bottom li').click(function () {
        $('.d-lg-block #cont-tab-2 #DefaultData2').addClass('d-block');
    });

    $('#cont-tab-2 a').click(function () {
        $('.d-lg-block #cont-tab-2 #DefaultData2').removeClass('d-block');
    });



    // $("a.dropdown-item")
    // .on("mouseenter", function (e) {
    // $(".menu-thumb").children()[0].style.display = "none";
    // for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
    // if ($(".menu-thumb").children()[i].alt == $(e.currentTarget).text()) {
    // $(".menu-thumb").children()[i].style.display = "block";
    // }
    // else {
    // $(".menu-thumb").children()[i].style.display = "none";
    // }
    // }
    // })
    // .on("mouseleave", function (e) {
    // for (var i = 1; i < $(".menu-thumb").children().length - 1; i++) {
    // $(".menu-thumb").children()[i].style.display = "none";
    // }
    // $(".menu-thumb").children()[0].style.display = "block";
    // });

    $('.single-item').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        nav: false,
        dots: true,
        autoplay: true,
        autoplaytimeout: 2000,
        animateOut: 'fadeOutLeft',
        animateIn: 'fadeInRight',
        slideSpeed: 1500,
        autoplayHoverPause: true,
        responsive: {
            0: {
                items: 1,
                margin: 10
            },
            600: {
                items: 1,
                margin: 10
            },
            1000: {
                items: 1,
                margin: 10
            }
        }
    })
    $('.three-item').owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false
            },
            600: {
                items: 2,
                nav: false,
                dots: false
            },
            1000: {
                items: 3,
                nav: false,
                dots: false
            }
        }
    })
	
	$('.otherservices_carousel').owlCarousel({
	loop: true,
	margin: 30,
	responsiveClass: true,
	nav:true,
	dots:false,
	autoplay:false,
	autoplaytimeout:1500,
	slideSpeed: 1500,
	navText: ["<i class='fa fa-angle-left'></i>","<i class='fa fa-angle-right'></i>"],
	autoplayHoverPause: true,
	responsive: {
	  0: {
		items: 1
	  },
	  600: {
		items: 2
	  },
	  1000: {
		items: 4,
		margin:40
	  }
	}
  });

	
	$('.inner-herobanner-carousel').owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: true,
        autoplaytimeout: 800,
        autoplayHoverPause: true,
        navText: ["<i class='fa fa-angle-left'></i>", "<i class='fa fa-angle-right'></i>"],
        items: 1,
		nav: false,
		dots: false
    })
    $('a.btn-gallery').on('click', function (event) {
        event.preventDefault();

        var gallery = $(this).attr('href');

        $(gallery).magnificPopup({
            delegate: 'a',
            type: 'image',
            gallery: {
                enabled: true
            }
        }).magnificPopup('open');
    });


})

$('.primaryMenu .dropdown span').click(function () {
		if ( $(this).parent('.dropdown').hasClass('active') ) {
			$(this).parent('.dropdown').removeClass('active');
		} else {
			$('.primaryMenu .dropdown span').parent('.dropdown').removeClass('active');
			$(this).parent('.dropdown').addClass('active');
		}
    });
// $('.primaryMenu .dropdown').click(function () {
		// // $('.topMenu .dropdown.active').removeClass('active');
        // // $(this).toggleClass('active');
		// if ( $(this).hasClass('active') ) {
			// $(this).removeClass('active');
		// } else {
			// $('.primaryMenu .dropdown').removeClass('active');
			// $(this).addClass('active');
		// }
    // });
    /*$('.primaryMenu .dropdown a').click(function () {
        $('.primaryMenu .dropdown .dropdown-menu').css('height', '0px').css('overflow', 'hidden').css('padding', '0px');
    });*/

var a = 0;
$(window).scroll(function () {
    if ($('#counter').length != 0) {
        var oTop = $('#counter').offset().top - window.innerHeight;
        if (a == 0 && $(window).scrollTop() > oTop) {
            (function ($) {
                $.fn.countTo = function (options) {
                    options = options || {};

                    return $(this).each(function () {
                        // set options for current element
                        var settings = $.extend({}, $.fn.countTo.defaults, {
                            from: $(this).data('from'),
                            to: $(this).data('to'),
                            speed: $(this).data('speed'),
                            refreshInterval: $(this).data('refresh-interval'),
                            decimals: $(this).data('decimals')
                        }, options);

                        // how many times to update the value, and how much to increment the value on each update
                        var loops = Math.ceil(settings.speed / settings.refreshInterval),
                            increment = (settings.to - settings.from) / loops;

                        // references & variables that will change with each update
                        var self = this,
                            $self = $(this),
                            loopCount = 0,
                            value = settings.from,
                            data = $self.data('countTo') || {};

                        $self.data('countTo', data);

                        // if an existing interval can be found, clear it first
                        if (data.interval) {
                            clearInterval(data.interval);
                        }
                        data.interval = setInterval(updateTimer, settings.refreshInterval);

                        // initialize the element with the starting value
                        render(value);

                        function updateTimer() {
                            value += increment;
                            loopCount++;

                            render(value);

                            if (typeof (settings.onUpdate) == 'function') {
                                settings.onUpdate.call(self, value);
                            }

                            if (loopCount >= loops) {
                                // remove the interval
                                $self.removeData('countTo');
                                clearInterval(data.interval);
                                value = settings.to;

                                if (typeof (settings.onComplete) == 'function') {
                                    settings.onComplete.call(self, value);
                                }
                            }
                        }

                        function render(value) {
                            var formattedValue = settings.formatter.call(self, value, settings);
                            $self.html(formattedValue);
                        }
                    });
                };

                $.fn.countTo.defaults = {
                    from: 0,               // the number the element should start at
                    to: 0,                 // the number the element should end at
                    speed: 1000,           // how long it should take to count between the target numbers
                    refreshInterval: 100,  // how often the element should be updated
                    decimals: 0,           // the number of decimal places to show
                    formatter: formatter,  // handler for formatting the value before rendering
                    onUpdate: null,        // callback method for every time the element is updated
                    onComplete: null       // callback method for when the element finishes updating
                };

                function formatter(value, settings) {
                    return value.toFixed(settings.decimals);
                }
            }(jQuery));

            jQuery(function ($) {
                // custom formatting example
                $('.count-number').data('countToOptions', {
                    formatter: function (value, options) {
                        return value.toFixed(options.decimals).replace(/\B(?=(?:\d{3})+(?!\d))/g, ',');
                    }
                });
                $('.count-number2').countTo({
                    decimals: 2,

                });
                // start all the timers
                $('.timer').each(count);

                //$('.timer2').each(count);  

                function count(options) {
                    var $this = $(this);
                    options = $.extend({}, options || {}, $this.data('countToOptions') || {});
                    $this.countTo(options);
                }
            });
            a = 1;
        }
    }
});

$('.centralindiatab').click(function () {
    $($('.nav-tabs li a[href="#centralindia"]')).trigger('click');
});

$('.rajasthantab').click(function () {
    $($('.nav-tabs li a[href="#rajasthan"]')).trigger('click');
});


$('.maharashtratab').click(function () {
    $($('.nav-tabs li a[href="#maharashtra"]')).trigger('click');
});


$('.uttarpradeshtab').click(function () {
    $($('.nav-tabs li a[href="#uttarpradesh"]')).trigger('click');
});

//$(document).ready(function () {
//    var message = $("#message").val();
//    if (message !== undefined && message !== null && message !== "")
//        $('#message_modal').modal('show');
//});

//var clicked = false;
//function CheckBrowser() {
//    if (clicked == false) {
//        //Browser closed
//    }
//    else {
//        //redirected 
//        clicked = false;
//    }
//}

//function bodyUnload() {
//    if (clicked == false)//browser is closed
//    {
//        var request = GetRequest();
//        request.open("POST", "/api/Accounts/LogoutSessionOnTabclose", false);
//        request.send();
//    }
//}

//function GetRequest() {
//    var request = null;
//    if (window.XMLHttpRequest) {
//        //incase of IE7,FF, Opera and Safari browser
//        request = new XMLHttpRequest();
//    }
//    else {
//        //for old browser like IE 6.x and IE 5.x
//        request = new ActiveXObject('MSXML2.XMLHTTP.3.0');
//    }
//    return request;
//}
//function Abandon(e) {
//    jQuery.ajax(
//        {
//            url: "/api/Accounts/LogoutSessionOnTabclose",
//            method: "POST",
//            async: true,
//            success: function (data) {
//                //e.cancelBubble is supported by IE - this will kill the bubbling process.
//                e.cancelBubble = true;
//                e.returnValue = leave_message;
//                //e.stopPropagation works in Firefox.
//                if (e.stopPropagation) {
//                    e.stopPropagation();
//                    e.preventDefault();
//                }
//                //return works for Chrome and Safari
//                return leave_message;
//            }
//        });
//}

//function IsSuccess() {
//    jQuery.ajax(
//        {
//            url: "/api/Accounts/Benow_Callback",
//            method: "POST",
//            success: function (data) {
//                if (data !== "") {
//                    myStopFunction();
//                    location.href = data;
//                }
//            }
//        });
//}

//setTimeout(myStopFunction, 300000);


//function myStopFunction() {
//    clearInterval(myVar);
//}

//$(window).load(function () {
//    $('#Communities').click(function () {
//        $('html, body').animate({
//            scrollTop: $("#communitiesSec").offset().top
//                - 130
//        }, 1000);
//    });



//    var owl = $('#serviceCarousels');
//    owl.owlCarousel({
//        loop: false,
//        margin: 10,
//        nav: false,
//        dots: false,
//        navRewind: false,
//        responsive: {
//            0: {
//                items: 1,
//                nav: false,
//                dots: true
//            },
//            600: {
//                items: 1,
//                nav: false,
//                dots: true
//            },
//            768: {
//                items: 2,
//                nav: false,
//                dots: true
//            },
//            1000: {
//                items: 3
//            }
//        }
//    });

//    var owl2 = $('#homeslider');
//    owl2.owlCarousel({
//        loop: true,
//        autoplay: true,
//        margin: 10,
//        nav: true,
//        autoHeight: true,
//        dots: true,
//        navRewind: false,
//        autoplayTimeout: 5000,
//        lazyLoad: true,
//        responsive: {
//            0: {
//                items: 1
//            },
//            600: {
//                items: 1
//            },
//            1000: {
//                items: 1
//            }
//        }
//    });

//    ///* Initialize Banner Carousel on home page */
//    //$("#bannerCarousel").owlCarousel({
//    //    autoplay:true,
//    //    //autoplayTimeout:2000,
//    //    //autoplayHoverPause:true,
//    //    center: true,
//    //    loop:true,
//    //    items: 1,
//    //    dots: false,
//    //    pagination : true,
//    //}); 

//    /* Initialize Carousel  on energy calculator*/
//    $("#energyCarousel").owlCarousel({
//        nav: true,
//        navText: [],
//        responsive: {
//            // breakpoint from 0 up
//            0: {
//                items: 2
//            },
//            // breakpoint from 480 up
//            480: {
//                items: 3
//            },
//            // breakpoint from 768 up
//            768: {
//                items: 3
//            },
//            // breakpoint from 992 up
//            992: {
//                items: 4
//            },
//            // breakpoint from 1025 up
//            1025: {
//                items: 5
//            }
//        }
//    });

//    /* Initialize Carousel on home page */
//    //$(".owl-carousel").owlCarousel({
//    //    nav: true,
//    //    navText: [],
//    //    responsive: {
//    //        // breakpoint from 0 up
//    //        0: {
//    //            items: 1
//    //        },
//    //        // breakpoint from 480 up
//    //        480: {
//    //            items: 2
//    //        },
//    //        // breakpoint from 768 up
//    //        768: {
//    //            items: 3,
//    //            nav: true,
//    //            loop: false,
//    //            touchDrag: false,
//    //            mouseDrag: false
//    //        },
//    //        1000: {
//    //            nav: true,
//    //            loop: false,
//    //            touchDrag: false,
//    //            mouseDrag: false
//    //        }
//    //    }
//    //});

//    /* Menu on mobile devices */

//    $('#dismiss, .overlay, .overlay-top').on('click', function () {
//        $('#sideNav').removeClass('active');
//        $('.overlay, .overlay-top').removeClass('active');
//        $('#topMenu').removeClass('active');
//    });

//    /*Side menu on mobile devices*/

//    $('#sidebarCollapse').on('click', function () {
//        $('#sideNav, #mainContent').toggleClass('active');
//        $('.collapse.in').toggleClass('in');
//        $('.overlay').addClass('active');
//        //$('a[aria-expanded=true]').attr('aria-expanded', 'false');
//    });

//    /* Top menu on mobile devices*/

//    $('#topNavCollapse').on('click', function () {
//        $('#topMenu').toggleClass('active');
//        $('.collapse.in').toggleClass('in');
//        $('.overlay-top').addClass('active');

//    });

//    ///* For smooth page scroll on Top menu link */
//    //document.querySelectorAll('a[href^="#"]').forEach(anchor => {
//    //    anchor.addEventListener('click', function (e) {
//    //        e.preventDefault();

//    //        document.querySelector(this.getAttribute('href')).scrollIntoView({
//    //            behavior: 'smooth'
//    //        });
//    //    });
//    //});

//    ///* to reduce the bottom space in between two control, if control have  minBottomSpace class*/
//    //$('.minBottomSpace').closest(".pageContent").css("min-height", "100px")
//});
///*Bootstrap DatePicker JS*/
//$(function () {
//    $('#datetimepicker1').datetimepicker({ format: 'DD/MM/YYYY' });
//});


$(document).on("click", ".txt-orange", function () {
    Id = $(this).data('id');
    var SupportEmailAddress = $(this).data('content');
    $("#TenderID").val(Id);
    if (SupportEmailAddress != "") {
        $("#SupportEmailAddress").text('you can conect for any suuport to :' + SupportEmailAddress);
    }
});


//$(document).on("click", "#download", function () {
//    var id = $(this).data('id');
//    jQuery.ajax(
//        {
//            url: "/api/TrivandrumAirport/GetTenderDocument",
//            method: "GET",
//            data: {
//                Id: id
//            },
//            success: function (data) {
//                $("#downloadModal").html(data);
//                $('#downloadModal').modal('show');
//            }
//        });


//})

////for career page popup
//$(window).on('load', function () {
//    $('#ac-wrapper').show();
//});
//$("#closeopop").on("click", function () {
//    $('#ac-wrapper').hide();
//});


//$(document).ready(function () {

//    if (window.location.href.indexOf("#communitiesSec") > 0) {

//        $('html, body').animate({
//            scrollTop: $("#communitiesSec").offset().top
//                - 130
//        }, 1000);
//    }

//    $(window).on('load', function () {
//        $('#homemodalpopup').modal('show');
//    });

//});

function Validate(obj) {
    var format = /[!@&#$%^*()+\=\[\]{};':"\\|,<>\/?]+/;

    if (format.test($(obj).val().replace(/^.*[\\\/]/, ''))) {
        alert('File name should not contain any special characters!');
        $(obj).val("");
        return false;
    }
    var ext = $(obj).val().split('.').pop().toLowerCase();
    // if($.inArray(ext, ['jpg','jpeg','png','pdf','doc','docx','xls','xlsx','.zip']) == -1) {
    // alert('invalid extension!');
    // }
    if ($.inArray(ext, ['jpg', 'jpeg', 'dwg', 'pdf', 'doc', 'docx', 'xls', 'xlsx']) == -1) {
        alert('File extension not supported!');
        $(obj).val("");
        return false;
    }
}

function validateEmail(email) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(email);
}

function validateMobileNo(mobile) {
    if (mobile.match(/^[6789]\d{9}$/)) {
        return true;
    }
    else {
        return false;
    }
}

function validateMobile(event, t) {
    var mobile = $("#mobileNumber").val();
    if (validateMobileNo(mobile)) {
        $("#mobileerror").html("");
    }
    else {
        $("#mobileerror").html("This is not a valid mobile number");
    }
}

function validateEmailId(event, t) {
    var emailAddress = $("#emailAddress").val();
    if (validateEmail(emailAddress)) {
        $("#emailerror").html("");
    }
    else {
        $("#emailerror").html("Please enter a valid Email Address");
    }
}



function validateName(name) {
    var regex = /^[a-zA-Z ]+$/;
    return regex.test(name);

}

function onchangeValidateFax(event, t) {
    var fax = $("#fax").val();
    if (fax != null && fax.trim() != "") {
        if (!validateFax(fax)) {
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            //$("#fax").focus();
        }
        else {
            $("#faxerror").html("");
        }
    }
    else {
        $("#faxerror").html("");
    }
}
	
function onchangeCompanyPAN(event, t) {	
      var fax = $("#UniqueId").val();
    if (fax == null && fax.trim() == "") {        
            $("#UniqueIderror").html("Please enter a company PAN");
	//$("#UniqueIderror").focus();
			
    }
    else {
        $("#UniqueIderror").html("");
    }
}	

function validateFax(fax) {
    var regex = /^[0-9]{12,12}$/;
    return regex.test(fax);
}

$(".reset").click(function () {
    $("#nameerror").html("");
    $("#companyerror").html("");
    $("#mobileerror").html("");
    $("#faxerror").html("");
    $("#emailerror").html("");
    $("#name").val("");
    $("#company").val("");
    $("#mobileNumber").val("");
    $("#fax").val("");
    $("#emailAddress").val("");
});

function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123)) {
            $("#nameerror").html("");
            return true;
        }
        else {
            $("#nameerror").html("Please enter a valid name containing alphabets only");
            return false;
        }
    }
    catch (err) {
        alert(err.Description);
    }
}


$("#UseRegistrationBtn").click(function (event) {
    var mobileNo = $("#mobileNumber").val();
    var emailAddress = $("#emailAddress").val();
    var company = $("#company").val();
    var fax = $("#fax").val();
    var name = $("#name").val();
    var legalDisclaimer = $("#chkLegalDisclaimer")[0]["checked"];

    if (!validateName(name)) {
        event.preventDefault();
        $("#nameerror").html("Please enter a valid name containing alphabets only");
        $("#name").focus();
        return false;
    }
    else {
        $("#nameerror").html("");
    }
    if (company == null || company.trim() == "") {
        event.preventDefault();
        $("#companyerror").html("Please enter a valid company name");
        $("#company").focus();
        return false;
    }
    else {
        $("#companyerror").html("");
    }
    if (!validateMobileNo(mobileNo)) {
        event.preventDefault();
        $("#mobileerror").html("Please enter a 9 digit valid mobile number");
        $("#mobileNumber").focus();
        return false;
    }
    else {
        $("#mobileerror").html("");
    }
    if (fax != null && fax.trim() != "") {
        if (!validateFax(fax)) {
            event.preventDefault();
            $("#faxerror").html("Please enter valid Fax number containing 12 digits");
            $("#fax").focus();
            return false;
        }
        else {
            $("#faxerror").html("");
        }
    }
    else {
        $("#faxerror").html("");
    }
    if (!validateEmail(emailAddress)) {
        event.preventDefault();
        $("#emailerror").html("Please enter a valid Email Address");
        $("#emailAddress").focus();
        return false;
    }
    else {
        $("#emailerror").html("");
    }
    if (legalDisclaimer != true) {
        event.preventDefault();
        $("#legalDisclaimerError").html("Please checked the Terms and condition");
        $("#legalDisclaimerError").focus();
        return false;
    }
    else {
        $("#legalDisclaimerError").html("");
    }
    // $('#UseRegistrationForm').submit();
    // return true;
});


//$(function () {
//    var optionsDatetime = $.extend({}, defaults, { format: 'DD-MM-YYYY HH:mm' });


//    $('#Adv_Date').datetimepicker(optionsDatetime);
//});
$(function () {
   
    $('#Adv_Date').datetimepicker({ format: 'DD-MM-YYYY HH:mm:ss' });
    $('#Closing_Date').datetimepicker({ format: 'DD-MM-YYYY HH:mm:ss' });


    //$('#Closing_Date').datepicker({
    //    uiLibrary: 'bootstrap4',
    //    iconsLibrary: 'fontawesome',
    //     format: 'dd-mm-yyyy 23:59:59',
    //});

});


$("#btnuploadDoc").click(function (e) {
    if (confirm("Do you want to submit finally? you will be not able to edit after submit.")) {
        console.log('Form is submitting');
        $("#frmuserupload").submit();
    } else {
        console.log('User clicked no.');
        return false;
    }
});

 //var today = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
  var startDate = new Date(2019, 0, 1);
$('#dep_flight').datepicker({
            uiLibrary: 'bootstrap4',
            iconsLibrary: 'fontawesome',
            //minDate: startDate,
          value: '01/01/2019',
            maxDate: function () {
                return $('#ar_flight').val();
            }
        });
  $('#sd').text(' ' + startDate + 'Start Date Entered: ' + $('#dep_flight').val());
$('#ar_flight').datepicker({
            uiLibrary: 'bootstrap4',
            iconsLibrary: 'fontawesome',
            minDate: function () {
                return $('#dep_flight').val();
            }
        });

 /*Selector*/
    var $btns = $('.quick-links-tabs__btn').click(function () {
        if (this.id == 'all') {
            
        } else {
            var $el = $('.' + this.id).addClass('active ');
            $('.quick-links-tabs__parent > div').not($el).removeClass('active');
        }
        $btns.removeClass('active');
        $(this).addClass('active');
    })
	
	
/*Flight Search Terminal Button Active*/
var selector = '.select_terminal ul li button';

$(selector).on('click', function(){
    $(selector).removeClass('active');
    $(this).addClass('active');
});

/*travel Scrollable nav*/
var activescroll = '.scroll-nav li';

$(activescroll).on('click', function(){
    $(activescroll).removeClass('active-before');
    $(this).addClass('active-before');
    $(this).prevAll().addClass('active');
    $(this).nextAll().removeClass('active');
});
/*travel Scrollable nav*/

/*smooth scroll travel guide*/
$( '.scroll-nav a' ).on( 'click', function(e){
	var href = $(this).attr( 'href' );
  $( 'html, body' ).animate({
		scrollTop: $( href ).offset().top - 220
  }, '600' );
  e.preventDefault();
});
/*/smooth scroll travel guide*/



/*Gallery Filter*/

$('.filters a').click(function (e) {
    e.preventDefault();
    $('.filters a').removeClass('active');
    $(this).addClass('active');
    var a = $(this).attr('href');
    a = a.substr(1);
    $('.filters-content .grid .col-lg-4').each(function () {
        if (!$(this).hasClass(a) && a != 'all')
            $(this).addClass('hide');
        else
            $(this).removeClass('hide');
    });

});


$('.filters a').click(function (e) {
    e.preventDefault();
    $('.filters a').removeClass('active');
    $(this).addClass('active');
    var a = $(this).attr('href');
    a = a.substr(1);
    $('.filters-content .airport-section__carousel airport-section__carousel-4item .item').each(function () {
        if (!$(this).hasClass(a) && a != 'all')
            $(this).addClass('hide');
        else
            $(this).removeClass('hide');
    });

});


$('.header-topbar button.close').click(function(){
	$('.header-topbar').addClass('hidden');
	$('body').addClass('alert_hidden');
});
$('.sidebar_cta').click(function(){
	$('.sidebar_cta').toggleClass('hidden');
});

$(document).ready(function () {
if (window.location.href.indexOf("#UnaccompaniedBaggageProcessing") > 0) {
        $('html, body').animate({
            scrollTop: $("#UnaccompaniedBaggageProcessing").offset().top
                - 100
        }, 1000);
    }
	if (window.location.href.indexOf("#HumanRemainProcessing") > 0) {
        $('html, body').animate({
            scrollTop: $("#HumanRemainProcessing").offset().top
                - 100
        }, 1000);
    }
	if (window.location.href.indexOf("#ValuableShipmentProcessing") > 0) {
        $('html, body').animate({
            scrollTop: $("#ValuableShipmentProcessing").offset().top
                - 100
        }, 1000);
    }
});


/*Sticky Sustainability tabs*/

// $(window).scroll(function(){
    // if ($(this).scrollTop() > 600) {
       // $('.scroll-nav .container-fluid').addClass('scroll-nav__fixed');
    // } else {
       // $('.scroll-nav .container-fluid').removeClass('scroll-nav__fixed');
    // }
// });


/*Sticky Sustainability tabs*/

let pages = document.querySelectorAll("section");
let nav = document.querySelectorAll(".scroll-nav a");

function getTopOfElement(element) {
  return (
    element.getBoundingClientRect().top -
    document.body.getBoundingClientRect().top - 200
  );
}

function setPageActive(scrollPosition) {
  for (let page of pages) {
    let bottom = getTopOfElement(page) + page.clientHeight;

    for (let anchor of nav) {
      if (scrollPosition >= getTopOfElement(page) && scrollPosition <= bottom) {
        anchor.hash.split("#")[1] === page.id
          ? anchor.classList.add("active")
          : anchor.classList.remove("active");
      }
    }
  }
}

setPageActive(window.scrollY);

window.addEventListener("scroll", event => {
  setPageActive(window.scrollY);
});

/*function loadWeather()
{
	var url = 'https://weather-ydn-yql.media.yahoo.com/forecastrss';
var method = 'GET';
var app_id = 'NnFnyXH8';
var consumer_key = 'dj0yJmk9Wmc5S0RJd3NvYmluJmQ9WVdrOVRtNUdibmxZU0RnbWNHbzlNQT09JnM9Y29uc3VtZXJzZWNyZXQmc3Y9MCZ4PWU0';
var consumer_secret = '579ebf77d536ccf1d0ffe79b3f09c8bbb6dd60a4';
var concat = '&';
var query = {'location': 'Jaipur,in', 'format': 'json','u':'c'};
var oauth = {
    'oauth_consumer_key': consumer_key,
    'oauth_nonce': Math.random().toString(36).substring(2),
    'oauth_signature_method': 'HMAC-SHA1',
    'oauth_timestamp': parseInt(new Date().getTime() / 1000).toString(),
    'oauth_version': '1.0'
};

var merged = {}; 
$.extend(merged, query, oauth);
// Note the sorting here is required
var merged_arr = Object.keys(merged).sort().map(function(k) {
  return [k + '=' + encodeURIComponent(merged[k])];
});
var signature_base_str = method
  + concat + encodeURIComponent(url)
  + concat + encodeURIComponent(merged_arr.join(concat));

var composite_key = encodeURIComponent(consumer_secret) + concat;
var hash = CryptoJS.HmacSHA1(signature_base_str, composite_key);
var signature = hash.toString(CryptoJS.enc.Base64);

oauth['oauth_signature'] = signature;
var auth_header = 'OAuth ' + Object.keys(oauth).map(function(k) {
  return [k + '="' + oauth[k] + '"'];
}).join(',');

$.ajax({
  url: url + '?' + $.param(query),
  headers: {
    'Authorization': auth_header,
    'X-Yahoo-App-Id': app_id 
  },
  method: 'GET',
  success: function(data){

if(data != null)
{
	if(data.current_observation != null)
	{
		if(data.current_observation.condition != null)
		{
			if(data.current_observation.condition.temperature != null)
			{
			console.log(data.current_observation.condition.temperature);
				$('#tempInfo').html('<span>' + data.current_observation.condition.temperature +' &#8451;</span>');
				
			}
		}
	}
}
  }
});
	
}*/
function loadWeather()
{
    var url = "https://api.weatherapi.com/v1/current.json?key=e6f018f1071b4a2c9c392224212408&q=trivandrum&aqi=no";
    $.ajax({
        url: url,
        
        method: 'GET',
        success: function(data){
      
      if(data != null)
      {
          if(data.current != null)
          {
              if(data.current.temp_c != null)
              {
                  
                  console.log(data.current.temp_c);
                      //$('#tempInfo').html('<span>' + data.current.temp_c +' &#8451;</span>');
                      $(".weather-plugin").html('<a href="#" class="nav-link"><span id="tempInfo"><span>' + data.current.temp_c +'&#8451;</span></span><img width="35px" src="'+data.current.condition.icon +'"></a>');
                      
                  
              }
          }
      }
        }
      });
}



/*Fixed Scroll Nav*/
var header = $(".scroll-nav");
  $(window).scroll(function() {    
    var scroll = $(window).scrollTop();
       if (scroll >= window.innerHeight) {
          header.addClass("fixed");
        } else {
          header.removeClass("fixed");
        }
});

var tenderheader = $(".tender-scroll-nav");
  $(window).scroll(function() {    
    var scroll = $(window).scrollTop();
       if (scroll >= window.innerHeight) {
          tenderheader .addClass("fixed");
        } else {
          tenderheader .removeClass("fixed");
        }
});



$('#nav-button').click(function() {
	$('#nav-button span').toggleClass('menu-close')
	$('#nav-button').toggleClass('active');
	$('.menu-overlay').toggleClass('d-none d-block');
	$('#back-to-top').toggleClass('d-none');
	$('.scroll-down').toggleClass('d-none');
	$('.sidebar_cta').toggleClass('d-none');
	$('body').toggleClass('overflow-hidden');
});

$('#nav-button-mobile').click(function() {
	$('#nav-button-mobile span').toggleClass('menu-close')
	$('#nav-button-mobile').toggleClass('active');
	$('#back-to-top').toggleClass('d-none');
	$('.scroll-down').toggleClass('d-none');
	$('.headerSec').toggleClass('active-bg_white');
	$('.primaryMenu li').removeClass('active');
	$('.primaryMenu li:first-child').addClass('active');
});

$('.menu-overlay').click(function() {
	$('.menu-overlay').toggleClass('d-block d-none');
	$('#nav-button').toggleClass('active');
	$('#nav-button span').toggleClass('menu-close');
	$('.navbar-collapse').toggleClass('show');
	$('.scroll-down').toggleClass('d-none');
	$('body').toggleClass('overflow-hidden');
	$('.sidebar_cta').toggleClass('d-none');
	$('.primaryMenu li').removeClass('active');	
	$('.primaryMenu li:first-child').addClass('active');
});
$('.nav-close').click(function() {
	$('.menu-overlay').toggleClass('d-block d-none');
	$('#nav-button').toggleClass('active');
	$('#nav-button span').toggleClass('menu-close');
	$('.navbar-collapse').toggleClass('show');
	$('.scroll-down').toggleClass('d-none');
	$('body').toggleClass('overflow-hidden');
	$('.sidebar_cta').toggleClass('d-none');
	$('.primaryMenu li').removeClass('active');
	$('.primaryMenu li:first-child').addClass('active');
	
});
$("#search-toggle").hover(function()
      {
		$('.headerSec').toggleClass('active-bg_white');
		$('.navbar-collapse').removeClass('show');  
	  });
	  
$(function() {
  $('#filter-dropdown select').change(function(){
    $('.filters-content .my-3').hide();
    $('.' + $(this).val()).show();
  });
});


var recaptcha1;
/*
var onloadCallback = function () {

    //Render the recaptcha1 on the element with ID "recaptcha1"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
        'theme': 'light'
    });
};*/

$("#BtnAPASubmit").click(function () {
    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    $('#BtnAPASubmit').attr("disabled", "disabled");

	var NameType = $("#NameType").val();

    var FirstName = $("#FirstName").val();
    if (FirstName == "") { alert("Please enter your First Name"); $("#FirstName").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    var MiddleName = $("#MiddleName").val();
    /*if (MiddleName == "") { alert("Please enter your Middle Name"); $("#MiddleName").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }*/

    var LastName = $("#LastName").val();
    if (LastName == "") { alert("Please enter your Last Name"); $("#LastName").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    var AreaCode = $("#AreaCode").val();
    if (AreaCode == "") { alert("Please enter your Area Code"); $("#AreaCode").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }


    var MobileNumber = $("#MobileNumber").val();
    if (MobileNumber == "") { alert("Please enter your Mobile Number"); $("#MobileNumber").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    if (!validateNumber(MobileNumber)) { alert("Please enter valid mobile number"); $("#MobileNumber").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }
	
	 var AlternateISD = $("#AlternateISD").val();
	  // var AlternateSTD = $("#AlternateSTD").val();
	    var AlternateMobileNumber = $("#AlternateMobileNumber").val();
		
		if(AlternateMobileNumber != "")
		{
			if(AlternateISD == "")
			{
				alert("Please enter Code for Alternate Mobile Number"); $("#AlternateISD").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; 
				
			}
    if (!validateNumber(AlternateMobileNumber)) { alert("Please enter valid Alternate Mobile Number"); $("#AlternateMobileNumber").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }
		}
/*
   
    if (AlternateISD == "") { alert("Please enter ISD for Alternate Mobile Number"); $("#AlternateISD").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

  
    if (AlternateSTD == "") { alert("Please enter STD for Alternate Mobile Number"); $("#AlternateSTD").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

   
    if (AlternateMobileNumber == "") { alert("Please enter your Alternate Mobile Number"); $("#AlternateMobileNumber").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    if (!validateNumber(AlternateMobileNumber)) { alert("Please enter valid Alternate Mobile Number"); $("#AlternateMobileNumber").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }
*/
    var EmailAddress = $("#EmailAddress").val();
    if (EmailAddress == "") { alert("Please enter your Email Address"); $("#EmailAddress").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    var ServiceType = $('input[name="ServiceType"]:checked').val();
    if (ServiceType == "" || ServiceType == undefined) { alert("Please select the Service Type"); $("#ServiceType").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

	 var TravelSector = $('input[name="TravelSector"]:checked').val();
    if (TravelSector == "" || TravelSector == undefined) { alert("Please select the Travel Sector"); $("#TravelSector").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }


    var TravelYear = $("#TravelYear").val();
    if (TravelYear == "") { alert("Please enter your Travel Year"); $("#TravelYear").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    var TravelMonth = $("#TravelMonth").val();
    if (TravelMonth == "") { alert("Please enter your Travel Month"); $("#TravelMonth").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    var TravelDay = $("#TravelDay").val();
    if (TravelDay == "") { alert("Please enter your Travel Day"); $("#TravelDay").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }

    var TravelDate =   new Date(
        $("#TravelYear").val(),
      $("#TravelMonth").val() -1,
        $("#TravelDay").val()
    );

    var tDate = Number($("#TravelMonth").val())+1 + "/" + $("#TravelDay").val() + "/" + $("#TravelYear").val();
	
    if (!isValidDate(tDate)) {
        alert("Please select valid Travel Date"); $("#TravelMonth").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; 
    }

    var CurrentDate = new Date();

   /* if (TravelDate.setHours(0,0,0,0) < CurrentDate.setHours(0,0,0,0)) { alert("Travel Date should not be past date."); $("#TravelYear").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }
*/
    if (!validateEmail(EmailAddress)) { alert("Please enter valid email address"); $("#EmailAddress").focus(); $('#BtnAPASubmit').removeAttr("disabled"); return false; }


	TravelMonth = Number(TravelMonth)+1;



    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    var model = {

        Name: NameType + " " +FirstName + " " + MiddleName + " " + LastName,
        ContactNumber: AreaCode + " " + MobileNumber,
        AlternateContactNumber: AlternateISD + " " + AlternateMobileNumber,
        EmailAddress: EmailAddress,
        TravelDate: TravelDay + "/" + TravelMonth + "/" + TravelYear,
        TypeOfService: ServiceType,
		TravelSector: TravelSector,
        reResponse: response,
    };


    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/TrivandrumAirport/BookAirportAssistance",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "https://stage.adaniuat.com/thiruvananthapuram-airport/Assistance-thankyou";
                //$('#contact_form1').submit();
            }
            else if (data.status == "3") {
                alert("Preferred contact number is already used for registration.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "4") {
                alert("Please fill mandatory fields.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "5") {
                alert("Please enter a valid contact number.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "6") {
                alert("Please enter a valid Email Address.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "7") {
                alert("Please enter a valid travel date.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "8") {
                alert("Please enter a valid name.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "9") {
                alert("Please enter a valid Type of Service.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "10") {
                alert("Please enter a valid Travel Sector.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
        }
    });
   
   
});

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}


function validateNumber(inputtxts) {
    var numbers = /^[0-9]+$/;
    if (numbers.test(inputtxts)) { return true; }
    else { return false; }
};

function validateMobileNo(inputtxt) {
      var phoneno = /^[0-9]{8,16}$/;
    if (phoneno.test(inputtxt)) { return true; }
    else { return false; }
};


function validateName(sname) {
    var regex = /^[a-zA-Z ]+$/;

    if (regex.test(sname)) { return true; }
    else { return false; }
};

function isValidDate(s) {
    var bits = s.split('/');
    var d = new Date(bits[2] + '/' + bits[1] + '/' + bits[0]);
    return !!(d && (d.getMonth() + 1) == bits[1] && d.getDate() == Number(bits[0]));
}
function isValidDate(dateString) {
    // First check for the pattern
    if (!/^\d{1,2}\/\d{1,2}\/\d{4}$/.test(dateString))
        return false;

    // Parse the date parts to integers
    var parts = dateString.split("/");
    var day = parseInt(parts[1], 10);
    var month = parseInt(parts[0], 10);
    var year = parseInt(parts[2], 10);

    // Check the ranges of month and year
    if (year < 1000 || year > 3000 || month == 0 || month > 12)
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Adjust for leap years
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
        monthLength[1] = 29;

    // Check the range of the day
    return day > 0 && day <= monthLength[month - 1];
};
	
$('#search-toggle .btn-close').on("click", function () {
$('#search-toggle').addClass("dropdown_inactive");
});

$('#search-toggle').mouseover(function() {
   $(this).removeClass('dropdown_inactive');
});

$('#search-toggle a').on("click", function () {
$('#search-toggle').removeClass("dropdown_inactive");
});

$(window).on('load', function () {
$('.homeslider-other').owlCarousel({
        loop: true,
        margin: 0,
        responsiveClass: true,
        autoplayTimeout: 5000,
        autoplay: true,
        animateOut: 'fadeOut',
        animateIn: 'fadeIn',
		autoplayHoverPause:true,
        nav: false,
        dots: true,
        navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
        responsive: {
            0: {
                items: 1,
                nav: false
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })

});


var recaptcha1;
/*var onloadCallback = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc', //Replace this with your Site key
        'theme': 'light'
    });


};*/


$("#btnsubmit").on('click', function () {


    var response = grecaptcha.getResponse(recaptcha1);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }
    $("#reResponse").val(response);
});
$("#DateArrival").on("change", function () {


    var ArrivalFlights = $(this).val();
	var FlightType = "Domestic";

    $.ajax({
        type: "Get",
        data: { ArrivalTime: ArrivalFlights,FlightType: FlightType },
        url: "/api/TrivandrumAirport/TrivandrumFlightDetailsArrivalUpdate",
        contentType: "application/json",
        success: function (data) {
            $('#arrival2').html(data);
        }
    });
});

$('span#submit').click(function () {
    var Id = $(this).closest('tr').find('.name').text().trim();
    if (confirm("Do you really want to delete this User")) {
        $.ajax({
            type: 'POST',
            data: { id: Id },
            url: "/api/TrivandrumAirport/DisableEnvelopeUser",
            success: function (data) {
                location.reload();

            },

            error: function (data) {
                alert("error!");  // 
            }
        });
    }
    else {
        location.reload();
    }
});

$('span#submits').click(function () {
    var Id = $(this).closest('tr').find('.name').text().trim();
    if (confirm("Do you really want to delete this User")) {
        $.ajax({
            type: 'POST',
            data: { id: Id },
            url: "/api/TrivandrumAirport/DisableAdminUser",
            success: function (data) {
                location.reload();

            },

            error: function (data) {
                alert("error!");  // 
            }
        });
    }
    else {
        location.reload();
    }

});



$("#DateDeparture").on("change", function () {


    var DepartureFlights = $(this).val();
	var FlightType = "Domestic"

    $.ajax({
        type: "Get",
        data: { DepartureFlight: DepartureFlights,FlightType: FlightType },
        url: "/api/TrivandrumAirport/TrivandrumFlightDetailsUpdateDeparture",
        contentType: "application/json",
        success: function (data) {
            $('#departure2').html(data);
        }
    });
});


$("#DateArrivalInter").on("change", function () {


    var DepartureFlights = $(this).val();
	var FlightType = "International";
	
    $.ajax({
        type: "Get",
        data: { ArrivalTime: DepartureFlights,FlightType: FlightType },
        url: "/api/TrivandrumAirport/TrivandrumFlightDetailsArrivalUpdate",
        contentType: "application/json",
        success: function (data) {
            $('#arrival2Inter').html(data);
        }
    });
});



$("#DateDepart").on("change", function () {


    var DepartureFlights = $(this).val();
	var FlightType = "International"

    $.ajax({
        type: "Get",
        data: { DepartureFlight: DepartureFlights,FlightType: FlightType },
        url: "/api/TrivandrumAirport/TrivandrumFlightDetailsUpdateDeparture",
        contentType: "application/json",
        success: function (data) {
            $('#departure2Inter').html(data);
        }
    });
});


$('#txtSearch').keyup(function (e) {
    if (e.keyCode == 13) {
        $('#btnSearch').click()
    }

});
// this will use for Get Data based on parameter
$("#btnSearch").click(function () {
    var ArrivalFlights = $("#DateArrival").val();
    var FlightType = "Domestic";
    if ($("#txtSearch").val().length > 0 || $("#txtSearch").val() != "") {
        $.ajax({
            type: "Get",
            data: {
                Search: $('#txtSearch').val(),
                ArrivalFlights: ArrivalFlights,
                FlightType: FlightType
            },
            url: "/api/TrivandrumAirport/TrivandrumFlighSearchDetailsArival",
            contentType: "application/json",
            success: function (data) {
                 if(data =="")
				{
					$('#arrival2').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				else if(data =="\r\n\r\n")
				{
					$('#arrival2').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				
				else
					$('#arrival2').html(data);
            },
            error: function () {
                alert("Failed! Please try again.");
            }
        });
    }
    else if ($("#txtSearches").val().length == 0) {
        var ArrivalFlights = $("#DateArrival").val();


        $.ajax({
            type: "Get",
            data: { ArrivalTime: ArrivalFlights, FlightType: FlightType },
            url: "/api/TrivandrumAirport/TrivandrumFlightDetailsArrivalUpdate",
            contentType: "application/json",
            success: function (data) {
                $('#arrival2').html(data);
            }
        });
    }
});

$("#btnSearchInter").click(function () {
    var ArrivalFlights = $("#DateArrivalInter").val();
    var FlightType = "International";
    if ($("#txtSearchInter").val().length > 0 || $("#txtSearchInter").val() != "") {
        $.ajax({
            type: "Get",
            data: {
                Search: $('#txtSearchInter').val(),
                ArrivalFlights: ArrivalFlights,
                FlightType: FlightType
            },
            url: "/api/TrivandrumAirport/TrivandrumFlighSearchDetailsArival",
            contentType: "application/json",
            success: function (data) {
				  if(data =="")
				{
					$('#arrival2Inter').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				else if(data =="\r\n\r\n")
				{
					$('#arrival2Inter').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				
				else
                $('#arrival2Inter').html(data);
            },
            error: function () {
                alert("Failed! Please try again.");
            }
        });
    }
    else if ($("#txtSearchesInter").val().length == 0) {
        var ArrivalFlights = $("#DateArrivalInter").val();


        $.ajax({
            type: "Get",
            data: { ArrivalTime: ArrivalFlights,FlightType: FlightType },
            url: "/api/TrivandrumAirport/TrivandrumFlightDetailsArrivalUpdate",
            contentType: "application/json",
            success: function (data) {
                $('#arrival2Inter').html(data);
            }
        });
    }
});

$('#txtSearches').keyup(function (e) {
    if (e.keyCode == 13) {
        $('#btnSearchDeparture').click()
    }
});

// this will use for Get Data based on parameter
$("#btnSearchDeparture").click(function () {
    var DepartureFlights = $("#DateDeparture").val();
    var FlightType = "Domestic";
    if ($("#txtSearches").val().length > 0 || $("#txtSearches").val() != "") {
        $.ajax({
            type: "Get",
            data: {
                Search: $('#txtSearches').val(),
                DepartureFlights: DepartureFlights,
                FlightType: FlightType
            },
            url: "/api/TrivandrumAirport/TrivandrumFlighSearchDetailsDeparture",
            contentType: "application/json",
            success: function (data) {
				  if(data =="")
				{
					$('#departure2').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				else if(data =="\r\n\r\n")
				{
					$('#departure2').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				
				else
                $('#departure2').html(data);
            },
            error: function () {
                alert("Failed! Please try again.");
            }
        });
    }
    else if ($("#txtSearches").val().length == 0) {
        var DepartureFlights = $("#DateDeparture").val();


  
        $.ajax({
            type: "Get",
            data: { DepartureFlight: DepartureFlights,FlightType: FlightType },
            url: "/api/TrivandrumAirport/TrivandrumFlightDetailsUpdateDeparture",
            contentType: "application/json",
            success: function (data) {
                //$('#departure2').html(data);
            
			   if(data =="")	
				{	
					$('#departure2').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');	
				}	
				else if(data =="\r\n\r\n")	
				{	
					$('#departure2').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');	
				}	
					
				else	
					$('#departure2').html(data);
			
			
			}
        });
    }


});

$("#btnSearchDepartureInter").click(function () {
    var DepartureFlights = $("#DateDepart").val();
    var FlightType = "International";
    if ($("#txtSearchesInter").val().length > 0 || $("#txtSearchesInter").val() != "") {
        $.ajax({
            type: "Get",
            data: {
                Search: $('#txtSearchesInter').val(),
                DepartureFlights: DepartureFlights,
                FlightType: FlightType
            },
            url: "/api/TrivandrumAirport/TrivandrumFlighSearchDetailsDeparture",
            contentType: "application/json",
            success: function (data) {
				  if(data =="")
				{
					$('#departure2Inter').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				else if(data =="\r\n\r\n")
				{
					$('#departure2Inter').html('<tr><td colspan="6"><p class="text-center" style="color: #737373;    text-decoration: underline;"><span class="fa fa-exclamation-triangle mr-2"></span>There is no data to  show</p></td></tr>');
				}
				
				else
                $('#departure2Inter').html(data);
            },
            error: function () {
                alert("Failed! Please try again.");
            }
        });
    }
    else if ($("#txtSearchesInter").val().length == 0) {
        var DepartureFlights = $("#DateDepartureInter").val();


        $.ajax({
            type: "Get",
            data: { DepartureFlight: DepartureFlights, FlightType: FlightType },
            url: "/api/TrivandrumAirport/TrivandrumFlightDetailsUpdateDeparture",
            contentType: "application/json",
            success: function (data) {
                $('#departure2Inter').html(data);
            }
        });
    }


});






$(document).ready(function () {
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "")
        $('#message_modal').modal('show');
  
    var date = new Date(); var it = ((date.getMonth() > 8) ? (date.getMonth() + 1) : ('0' + (date.getMonth() + 1))) + '/' + ((date.getDate() > 9) ? date.getDate() : ('0' + date.getDate())) + '/' + date.getFullYear(); $('#dep_flight').val(it);
 $('#ar_flight').val(it);
	 loadWeather();

 if(location.pathname=="/")
 {
	 $('.weather-app div').css("margin-top","-140px");
 }
 if(window.location.href.indexOf('/Business')>0)
 {
	 $($('.primaryMenu .nav-item ')[0]).removeClass('active');
	 $($('.primaryMenu .nav-item ')[1]).addClass('active');
 }

 if(window.location.href.indexOf('/BookAirportAssistance')>0)
 {
	 var d = new Date();
	$('#TravelYear').val(d.getFullYear());
	$('#TravelMonth').val(d.getMonth() + 1);
	$('#TravelDay').val(d.getDate());
 }

});
$('.modal .close').click(function () {

    var videoId = $(this).attr('Id');
    CloseModal2(videoId);

});

$('.modal').on('shown.bs.modal', function () {
    $('.modal video')[0].play();
})
$('.modal.video').on('hidden.bs.modal', function () {
    $('.modal video')[0].pause();
})

function CloseModal(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
}

$('body').on('hidden.bs.modal', '.modal', function () {
    $('video').trigger('pause');
});

$(".view-all video").each(function () {
    $(this).get(0).pause();
});

function DownloadFile(fileId) {
    $("#hfFileId").val(fileId);
    $("#btnDownload").click();
};

(function () {

    'use strict';

    // define variables
    var items = document.querySelectorAll(".timeline li");

    // check if an element is in viewport
    // http://stackoverflow.com/questions/123999/how-to-tell-if-a-dom-element-is-visible-in-the-current-viewport
    function isElementInViewport(el) {
        var rect = el.getBoundingClientRect();
        return (
            rect.top >= 0 &&
            rect.left >= 0 &&
            rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.right <= (window.innerWidth || document.documentElement.clientWidth)
        );
    }

    function callbackFunc() {
        for (var i = 0; i < items.length; i++) {
            if (isElementInViewport(items[i])) {
                items[i].classList.add("in-view");
            }
        }
    }

    // listen for events
    window.addEventListener("load", callbackFunc);
    window.addEventListener("resize", callbackFunc);
    window.addEventListener("scroll", callbackFunc);

})();

$('.top_bar_nav').click(function(){
	$(this).toggleClass('active');
  });
  
  $('#mainMenu .dropdown-item').click(function(){
$('.loaded').removeClass('overflow-hidden');
$('.sidebar_cta').removeClass('d-none');
});

  
    	
  $('a[href="https://www.adani.com/Sustainability"]').attr('target','_blank')	
  $('a[href="https://careers.adani.com/"]').attr('target','_blank')
  $("#BtnASFSubmit").click(function () {
    /* var response = grecaptcha.getResponse(recaptcha1);
     if (response.length == 0) {
         alert("Captcha required.");
         return false; 
     }*/
	 
	var FlightDetails =$('#FlightDetails').val() 
	if (FlightDetails=='null') { alert("Please select the Travel Sector");  $('#BtnASFSubmit').removeAttr("disabled"); return false; }

 	var FirstName = $("#FirstName").val();
	//if (FirstName == "") { alert("Please enter your First Name"); $("#FirstName").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var LastName = $("#LastName").val();
	//if (LastName == "") { alert("Please enter your Last Name"); $("#LastName").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var DateOfVisit = $(".visitDate").val(); 
	//if (DateOfVisit == "" || AirportFacilities == undefined) { alert("Please enter your Date of Visit"); $("#DateOfVisit").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var Contact = $("#Contact").val().trim();
	//if (Contact == "") { alert("Please enter your Contact detail"); $("#Contact").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var FacilitiesServices = $('input[name="FacilitiesServices"]:checked').val();
	if (FacilitiesServices == "" || FacilitiesServices == undefined) { alert("Please select the Check in Facilities & Services"); $("#FacilitiesServices").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var AirportCleanliness = $('input[name="AirportCleanliness"]:checked').val();
	if (AirportCleanliness == "" || AirportCleanliness == undefined) { alert("Please select the Cleanliness of Washrooms /Terminal"); $("#AirportCleanliness").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var AirportFacilities = $('input[name="AirportFacilities"]:checked').val();
	if (AirportFacilities == "" || AirportFacilities == undefined) { alert("Please select the Airport Facilities"); $("#AirportFacilities").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var ShoppingFacilities = $('input[name="ShoppingFacilities"]:checked').val();
	if (ShoppingFacilities == "" || ShoppingFacilities == undefined) { alert("Please select the F&B /Shopping Facilities"); $("#ShoppingFacilities").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }

    var CourtesyOfStaff = $('input[name="CourtesyOfStaff"]:checked').val();
	if (CourtesyOfStaff == "" || CourtesyOfStaff == undefined) { alert("Please select the Courtesy of Staff"); $("#CourtesyOfStaff").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var WaitingTime = $('input[name="WaitingTime"]:checked').val();
	if (WaitingTime == "" || WaitingTime == undefined) { alert("Please select the Waiting time in Queues"); $("#WaitingTime").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var ArrivalFacilities = $('input[name="ArrivalFacilities"]:checked').val();
	if (ArrivalFacilities == "" || ArrivalFacilities == undefined) { alert("Please select the Arrival Facilities"); $("#ArrivalFacilities").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var TransportationFacilities = $('input[name="TransportationFacilities"]:checked').val();
	if (TransportationFacilities == "" || TransportationFacilities == undefined) { alert("Please select the Transportation Facilities To & Fro Airport"); $("#TransportationFacilities").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var FlightConnectivity = $('input[name="FlightConnectivity"]:checked').val();
	if (FlightConnectivity == "" || FlightConnectivity == undefined) { alert("Please select the Flight connectivity"); $("#FlightConnectivity").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var MaintenanceUpkeep = $('input[name="MaintenanceUpkeep"]:checked').val();
	if (MaintenanceUpkeep == "" || MaintenanceUpkeep == undefined) { alert("Please select the Maintenance & Upkeep"); $("#MaintenanceUpkeep").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
	var Travel = $('input[name="travel"]:checked').val();
	if(Travel=="Business Traveller")
	{ var TravelPurpose = document.getElementById("TravelPurpose").value;}
	else if(Travel=="Leisure Traveller")
	{ var TravelPurpose = document.getElementById("TravelPurpose1").value;}
	if (TravelPurpose == "" || TravelPurpose == undefined) { alert("Please select the Travel Purpose"); $("#FlightConnectivity").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    var RecommendationScale = rangevalue;
	if (RecommendationScale == "") { alert("Please enter your Recommendation Scale"); $("#RecommendationScale").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
	var Suggestion = $("#Suggestion").val();
	if (Suggestion == "") { alert("Please enter your Suggestion"); $("#Suggestion").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	if ($('#Suggestion').val().length>230) { alert("230 Characters only "); $("#Suggestion").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
	var AgeGroup = document.getElementById("AgeGroup").value;
	if (AgeGroup == "" || AgeGroup == undefined) { alert("Please select the Age Group"); $("#AgeGroup").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
	var Gender = document.getElementById("Gender").value;
	if (Gender == "" || Gender == undefined) { alert("Please select the Gender"); $("#Gender").focus(); $('#BtnASFSubmit').removeAttr("disabled"); return false; }
	
    

    var model = {
        FirstName: FirstName,
        LastName: LastName,
        DateOfVisit: DateOfVisit,
        FlightDetails: FlightDetails,
        Contact: Contact,
        FacilitiesServices: FacilitiesServices,
        AirportCleanliness: AirportCleanliness,
        AirportFacilities: AirportFacilities,
        ShoppingFacilities: ShoppingFacilities,
        CourtesyOfStaff: CourtesyOfStaff,
        WaitingTime: WaitingTime,
        ArrivalFacilities: ArrivalFacilities,
        TransportationFacilities: TransportationFacilities,
        FlightConnectivity: FlightConnectivity,
        MaintenanceUpkeep: MaintenanceUpkeep,
        RecommendationScale: RecommendationScale,
        Suggestion: Suggestion,
        TravelPurpose: TravelPurpose,
        AgeGroup: AgeGroup,
        Gender: Gender
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/api/TrivandrumAirport/AirportSurveyFeedback",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "https://stage.adaniuat.com/thiruvananthapuram-airport/assistance-thankyou";
                //$('#contact_form1').submit();
            }
            else if (data.status == "3") {
                alert("Please Enter First Name.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "4") {
                alert("Please Enter Last Name.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "5") {
                alert("Please enter valid Mobile No Or Email.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "6") {
                alert("Please enter a valid Date of Visit.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "7") {
                alert("Please enter Flight Detail");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "8") {
                alert("Please enter a valid Feedback for Check in Facilities & Services.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "9") {
                alert("Please enter a valid Feedback for Cleanliness of Washrooms /Terminal.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "10") {
                alert("Please enter a valid Feedback for Airport Facilities.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "11") {
                alert("Please enter a valid Feedback for F&B /Shopping Facilities.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "12") {
                alert("Please enter a valid Feedback for Courtesy of Staff.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "13") {
                alert("Please enter a valid Feedback for Waiting time in Queues.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "14") {
                alert("Please enter a valid Feedback for Arrival Facilities.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "15") {
                alert("Please enter a valid Feedback for Transportation Facilities To & Fro Airport.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "16") {
                alert("Please enter a valid Feedback for Flight connectivity.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "17") {
                alert("Please enter a valid Feedback for Maintenance & Upkeep.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "18") {
                alert("Please Enter a valid range of recommendation .");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "19") {
                alert("Please enter a valid Suggestion.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "20") {
                alert("Please enter a valid Travel Purpose.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "21") {
                alert("Please enter a valid AgeGroupe.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else if (data.status == "22") {
                alert("Please enter a valid Gender.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
			else if (data.status == "23") {
                alert("Please enter a valid Mobile No/ Email Address.");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#BtnAPASubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;

});
function yourfunction(radioid)
{
	if(radioid == 1)
	{    
		//document.getElementById('one').style.display = 'flex';
		jQuery('#one').show();
		//document.getElementById('two').style.display = 'none';
		jQuery('#two').hide();
	 }
	 else if(radioid == 2)
	{  
		//document.getElementById('one').style.display = 'none';
		jQuery('#two').show();
		//document.getElementById('two').style.display = 'flex';
		jQuery('#one').hide();
	}
}


//Contact Us Form
$("#ContactUsButton").click(function (e) {
	$('#ContactUsButton').attr('disabled','true');
	if($("#Fullname").val()=='' || $("#Email").val().trim()=='' || $("#ContactNo").val()=='' || $("#ContactType").val()=='' || $('#Message').val()=='' ){
		$("#Fullname").blur()
		$("#Email").blur()
		$("#ContactType").blur()
		$("#ContactNo").blur()
		$("#Message").blur()
		$('#ContactUsButton').removeAttr('disabled');

			return false;
		}
    getCaptchaResponseForm();
    e.preventDefault();

});
$("#Fullname").blur(function(){
	var FullNameRegex= /^[A-Za-z ]{3,100}$/g;
	if (!FullNameRegex.test($("#Fullname").val())) {
        $("#Fullname").next().html('Please Enter a valid Name!');
        return false;
    } else {
        $("#Fullname").next().html("");
    }
});
$("#Email").blur(function(){
	var EmailRegex= /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/g;
	if (!EmailRegex.test($("#Email").val())) {
        $("#Email").next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#Email").next().html("");
    }
});
$("#ContactType").blur(function(){
	if ($("#ContactType").val()=='') {
        $("#ContactType").next().html('Please Select Subjecct!');
        return false;
    } else {
    $("#ContactType").next().html("");
    }
});
$("#ContactNo").blur(function(){
	var ContactRegex= /^\d{10}$/g;
	if (!ContactRegex.test($("#ContactNo").val())) {
       $("#ContactNo").next().html("Please enter 10 digit Mobile Number!");
                return false;
            } else {
                $("#ContactNo").next().html("");
            }
});
$("#Message").blur(function(){
    var MessageRegex = /^[A-Za-z0-9;,.?_\-!@&:""'\/\\ ]{0,1000}$/g;
    if (!MessageRegex.test($("#Message").val())) {
        $("#Message").next().html("Please enter a valid Message, special characters not allowed");
        return false;
    } else {
        $("#Message").next().html("");
    }
});


function getCaptchaResponseForm(e) {
        grecaptcha.ready(function() {
          grecaptcha.execute('6LdoDj4hAAAAAD7gqiPJEsAy96GJBkiNRu9qohiz', {action: 'AirportContactUs'}).then(function(token) {
			  $('.g-recaptcha').val(token);
      
 
 


            var savecontactdata = {
                Fullname: $("#Fullname").val(),
                Email: $("#Email").val().trim(),
                ContactNo: $("#ContactNo").val(),
                ContactType: $("#ContactType").val(),
                Message: $("#Message").val(),
                ipAddress: $("#IpAddress").val(),
                reResponse: token
            };

			$.ajax({
					type: "POST",
					data: JSON.stringify(savecontactdata),
					url: "/api/TrivandrumAirport/AirportContactUs",
					contentType: "application/json",
					success: function (data) {
						if (data.status == "1") {
							window.location.href = "/thiruvananthapuram-airport/thank-you";
						}
						else if (data.status == "2") {
							alert("Invalid Captcha!!");
							$('#ContactUsButton').removeAttr('disabled');
						}
						else if (data.status == "3") {
							alert("Please fill all required fields with valid information");
							$('#ContactUsButton').removeAttr('disabled');
						}
						else if (data.status == "4") {
							alert("Please enter a valid Subjecct");
							$('#ContactUsButton').removeAttr('disabled');
                        }	
                        else if (data.status == "5") {
                            alert("Something went wrong");
                            $('#ContactUsButton').removeAttr('disabled');
                        }
						else {
							alert("System operation failed. Please try again later!!");
							$('#ContactUsButton').removeAttr('disabled');
						}
					}
			});
	});
});
}