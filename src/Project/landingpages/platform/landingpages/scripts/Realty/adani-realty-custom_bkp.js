$(document).ready(function () {
	 $(function () {
        $('.datetimepicker').datetimepicker(
            {
                format: 'DD/MM/YYYY HH:mm',
            });
    });
	
	if(window.location.href=="https://www.adanirealty.com/club")
	{
		 window.location = "https://www.belvedereclub.in/";
		
	}
	
    $('#club').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            600: {
                items: 2,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 3,
                nav: true,
                dots: false,
                loop: false,
                margin: 20,
                autoplay: true

            }
        }
    });
$("#main-slider").owlCarousel({
        loop: true,
        // margin: 10,
        nav: false,
        dots: false,
        autoplay: true,
		autoplayTimeout: 7000,
		animateOut: 'fadeOut',
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false
            },
            400: {
                items: 1,
                nav: false,
                dots: false
            },
            600: {
                items: 1,
                nav: false,
                dots: false
            },
            1000: {
                items: 1,
                nav: false,
                dots: false

            }
        }

    });

    $("#gallery1").owlCarousel({
        loop: false,
        margin: 10,
        nav: false,
        dots: false,
        autoplay: true,
        responsive: {
            0: {
                items: 2,
                nav: false,
                dots: false
            },
            400: {
                items: 2,
                nav: false,
                dots: false
            },
            600: {
                items: 3,
                nav: false,
                dots: false
            },
            1000: {
                items: 4,
                nav: false,
                dots: false

            }
        }

    });
    $("#gallery-bottom").owlCarousel({
        loop: true,
        margin: 10,
        nav: false,
        dots: false,
        autoplay: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false
            },
            400: {
                items: 1,
                nav: false,
                dots: false
            },
            600: {
                items: 1,
                nav: false,
                dots: false
            },
            1000: {
                items: 1,
                nav: false,
                dots: false

            }
        }

    });

    /*navigation*/
    $(".nav-tabs a").click(function (e) {
        e.stopPropagation()
        $(this).tab('show');
    });

    /*Rera*/
    $(window).on('load', function () {
        $('#reradisclaimer').modal('show');
		 $('#westernheights-popup').modal('show');
    });
	
	
	$("#btnwheights").click(function(){
	$("#westernheights-popup").modal('hide');
	}); 

    /* partners Carousel For about us */
    $("#partners-carousel").owlCarousel({
        loop: false,
        margin: 10,
        autoplay: true,
        autoplayTimeout: 2000,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 3,
                nav: false,
                dots: false
            },
            // breakpoint from 480 up
            480: {
                items: 3,
                nav: false,
                dots: false
            },
            600: {
                items: 4,
                nav: false,
                dots: false
            },
            // breakpoint from 768 up
            991: {
                items: 6,
                nav: false,
                dots: false
            },
            1100: {
                items: 8,
                nav: false,
                dots: false
            }
        }
    });
    /* partners Carousel For about us */
    $("#blog-videos").owlCarousel({
        loop: false,
        margin: 10,
        autoplay: true,
        autoplayTimeout: 1500,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 2,
                nav: true,
                dots: false
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: false,
                dots: false
            }
        }
    });
    /* Accolades For about us */
    $("#accoladesslider").owlCarousel({
        loop: true,
        margin: 10,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 2,
                nav: true
            },
            // breakpoint from 480 up
            480: {
                items: 3,
                nav: true,
                dots: false
            },
            // breakpoint from 768 up
            768: {
                items: 4,
                nav: true,
                dots: false
            }
        }
    });
	
	$(".project_gallery-carousel").owlCarousel({
        		nav:      false,
		navText: ["<span class='fa fa-angle-left'></span>", "<span class='fa fa-angle-right'></span>"],
		margin:   20,
		loop:     true,
		autoplay: true,
		responsive:{
			0:{
					items:1
			},
			480:{
					items:2
			},
			768:{
					items:2
			},
			992:{
					items:2
			}
		}
    });
	
    /* Initialize Latest Projects Carousel on home page */
    $("#testimonials").owlCarousel({
        nav: false,
        loop: true,
        margin: 30,
        dots: true,
        autoplay: true,
        autoplayTimeout: 3000,
		autoplayHoverPause:true,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 1
            },
            // breakpoint from 768 up
            768: {
                items: 1,
                nav: false,
                dots: true,
                autoplay: true
            },
            1200: {
                items: 2,
                nav: false,
                dots: true,
                autoplay: true
            }
        }
    });

    $('#searchproperty').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: false
            },
            600: {
                items: 2,
                nav: false,
                dots: false,
                nav: false,
                autoplay: false
            },
            1200: {
                items: 3,
                nav: true,
                dots: false,
                loop: false,
                margin: 20,
                autoplay: false

            }
        }
    });

    $('#propertytestimonials').owlCarousel({
        loop: false,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: false
            },
            600: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: false
            },
            1200: {
                items: 1,
                nav: false,
                dots: true,
                loop: false,
                margin: 20,
                autoplay: false

            }
        }
    });

    $('#residential').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            600: {
                items: 2,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            1200: {
                items: 3,
                nav: true,
                dots: false,
                loop: true,
                margin: 20,
                autoplay: true

            }
        }
    });
    $('#commercial').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            600: {
                items: 2,
                nav: false,
                dots: true,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 3,
                nav: true,
                dots: false,
                loop: false,
                margin: 20,
                autoplay: true

            }
        }
    });

    /* Accolades For about us */
    $("#blog-carousel").owlCarousel({
        loop: true,
        margin: 150,
		autoplay:true,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1,
                nav: false
            },
            // breakpoint from 480 up
            480: {
                items: 2,
                nav: false,
                dots: false
            },
            // breakpoint from 768 up
            768: {
                items: 1,
                nav: false,
                dots: false
            },
            1000: {
                items: 2,
                nav: false,
                dots: false
            }
        }
    });


    $('#carousel').owlCarousel({
        loop: true,
        animateOut: 'fadeOut',
        margin: 10,
        responsiveClass: true,
        autoplayTimeout: 7000,
        responsive: {
            0: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            600: {
                items: 1,
                nav: false,
                dots: false,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 1,
                nav: true,
                dots: false,
                loop: true,
                margin: 20,
                autoplay: true

            }
        }
    });

    $('#residential-slider').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                autoplay: true
            },
            600: {
                items: 1,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 1,
                nav: false,
                loop: true,
                margin: 20,
                dots: true,
                autoplay: true
            }
        }
    });

    $('#commercial-slider').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                nav: false,
                autoplay: true
            },
            600: {
                items: 1,
                nav: false,
                autoplay: true
            },
            1000: {
                items: 1,
                nav: false,
                loop: true,
                margin: 20,
                dots: true,
                autoplay: true
            }
        }
    });


    $('#pcarousel').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 3,
                nav: false,
                dots: false,
                autoplay: true
            },
            600: {
                items: 4,
                nav: false,
                dots: false,
                autoplay: true
            },
            1000: {
                items: 5,
                nav: false,
                loop: true,
                margin: 20,
                dots: false,
                autoplay: true
            }
        }
    });

    /*Media Carousel JS*/
    $("#media-carousel").owlCarousel({
        nav: false,
        loop: false,
        margin: 30,
        dots: true,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 2
            },
            // breakpoint from 480 up
            480: {
                items: 3
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: false,
                dots: true,
                autoplay: true,

            }
        }
    });
    /*News Carousel JS*/
    $("#news-carousel").owlCarousel({
        nav: true,
        loop: false,
        margin: 30,
        dots: true,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 2
            },
            // breakpoint from 768 up
            768: {
                items: 2,
                nav: false,
                dots: true,
                autoplay: true,

            }
        }
    });


    /*News Carousel JS*/
    $("#media-coverage").owlCarousel({
        nav: true,
        loop: false,
        margin: 30,
        dots: true,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 2
            },
            // breakpoint from 768 up
            768: {
                items: 3,
                nav: true,
                dots: false,
                autoplay: true,

            }
        }
    });

    /*NewCareer Carousel JS*/
    $("#career-carousel").owlCarousel({
        nav: false,
        loop: false,
        margin: 0,
        dots: true,
        responsive: {
            // breakpoint from 0 up
            0: {
                items: 1
            },
            // breakpoint from 480 up
            480: {
                items: 1
            },
            // breakpoint from 768 up
            768: {
                items: 1,
                nav: false,
                dots: true,
                autoplay: true,

            }
        }
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

    $('.dropdown').on("hover", function (e) {
        $(this).next('ul').toggle();
        e.stopPropagation();
        e.preventDefault();
    });

    $('.adv-search').click(function () {
        $('.bannerForm').toggleClass('navtop navb');
    });

    $('.ad-inner').click(function () {
        $('.bannerForm').toggleClass('navtop');
    });

    /*Team tiles*/
    $('#leadershipteam').on("click", function () {
        $('#team-tile').slideDown(700, function () {
            $('#team-tile').show(700, "");
            $('.accoladess').hide(500);
        });
    });
    $('#teams-tiles-close').on("click", function () {
        $('#team-tile').slideDown(700, function () {
            $('#team-tile').hide(700, "");
        });
    });



    /*Team 1*/
    $('#team-1').on("click", function () {
        $('#team-01').slideDown(700, function () {
            $('#team-01').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-01').on("click", function () {
        $('#team-01').slideDown(700, function () {
            $('#team-01').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /*Team 1*/

    /*Team 2*/
    $('#team-2').on("click", function () {
        $('#team-02').slideDown(700, function () {
            $('#team-02').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-02').on("click", function () {
        $('#team-02').slideDown(700, function () {
            $('#team-02').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /*Team 2*/


    /*Team 3*/
    $('#team-3').on("click", function () {
        $('#team-03').slideDown(700, function () {
            $('#team-03').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-03').on("click", function () {
        $('#team-03').slideDown(700, function () {
            $('#team-03').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /* /Team 3*/

    /*Team 4*/
    $('#team-4').on("click", function () {
        $('#team-04').slideDown(700, function () {
            $('#team-04').show(700, "");
        });
        $('#team-tile .teams-tiles').hide(700, "");
    });
    $('#close-04').on("click", function () {
        $('#team-04').slideDown(700, function () {
            $('#team-04').hide(700, "");
        });
        $('#team-tile .teams-tiles').show(700, "");
    });
    /* /Team 4*/

    $('#accolades').on("click", function () {
        $('.accoladess').slideDown(700, function () {
            $('.accoladess').show(700, "");
            $('#team-tile').hide(400, "");
        });
    });

    /*Partners Carousel*/
    $('#partnercarousel0').owlCarousel({
        loop: true,
        margin: 10,
        responsiveClass: true,
        responsive: {
            0: {
                items: 3,
                nav: false,
                autoplay: true,
                dots: false,
                loop: false
            },
            600: {
                items: 4,
                nav: false,
                autoplay: true,
                dots: false,
                loop: false
            },
            1000: {
                items: 5,
                nav: false,
                loop: true,
                margin: 20,
                dots: false,
                autoplay: true,
                loop: false
            }
        }
    });

    var hashValue = location.hash;
    hashValue = hashValue.replace(/^#/, '');
    //do something with the value here 
    if (hashValue === 'accoladess') {
        $('.accoladess').show(700, "");
        if (window.location.href.indexOf("#accolades") > 0) {

            $('html, body').animate({
                scrollTop: $("#accolades").offset().top
                    - 100
            }, 1000);
        }
    }
});

/*Gallery Popup*/

$("#gallery").owlCarousel({
    loop: false,
    margin: 10,
    nav: false,
    dots: false,
    responsive: {
        0: {
            items: 1,
            nav: false,
            dots: false
        },
        400: {
            items: 2,
            nav: false,
            dots: false
        },
        600: {
            items: 3,
            nav: false,
            dots: false
        },
        1000: {
            items: 3,
            nav: true,
            dots: false

        }
    }

});
$("#gallery1").owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    dots: false,
    autoplay: true,
    responsive: {
        0: {
            items: 1,
            nav: false,
            dots: false
        },
        400: {
            items: 2,
            nav: false,
            dots: false
        },
        600: {
            items: 3,
            nav: false,
            dots: false
        },
        1000: {
            items: 5,
            nav: false,
            dots: false

        }
    }

});
$("#projectstatus1").owlCarousel({
    loop: true,
    margin: 10,
    nav: false,
    dots: false,
    autoplay: true,
    responsive: {
        0: {
            items: 1,
            nav: false,
            dots: false
        },
        400: {
            items: 2,
            nav: false,
            dots: false
        },
        600: {
            items: 3,
            nav: false,
            dots: false
        },
        1000: {
            items: 3,
            nav: false,
            dots: false

        },
        1500: {
            items: 4,
            nav: false,
            dots: false

        }
    }

});


$('#btnwheights').click(function () {
        $('html, body').animate({
            scrollTop: $("#glry-wheights").offset().top
                - 130
        }, 1000);
    }); 

/*Mega Menu*/
$(function () {
    var selectedClass = "";
    $(".fil-cat").click(function () {
        selectedClass = $(this).attr("data-rel");
        $("#tabbedmenu").fadeTo(100, 0.1);
        $("#tabbedmenu li").not("." + selectedClass).fadeOut(50).removeClass('scale-anm');
        setTimeout(function () {
            $("." + selectedClass).fadeIn().addClass('scale-anm');
            $("#tabbedmenu").fadeTo(300, 1);
        }, 300);

    });
});

jQuery('.tablinks').click(function (event) {
    //remove all pre-existing active classes
    jQuery('.active-tab').removeClass('active-tab');

    //add the active class to the link we clicked
    jQuery(this).addClass('active-tab');

    //Load the content
    //e.g.
    //load the page that the link was pointing to
    //$('#content').load($(this).find(a).attr('href'));      

    event.preventDefault();
});



function CloseModal(count) {
    count++;
    jQuery('#Video' + count + " " + 'iframe').attr("src", jQuery("#Video" + count + " " + "iframe").attr("src"));
}
  //model video close
	function CloseModal1(count) {
    jQuery('#video_' + count + " " + 'iframe').attr("src", jQuery("#video_" + count + " " + "iframe").attr("src"));
}  
  //model video close
	function CloseModal2(count) {
    jQuery('#' + count + " " + 'iframe').attr("src", jQuery("#" + count + " " + "iframe").attr("src"));
}

//close video modal
		 $("#video-01").on('hidden.bs.modal', function (e) {
        $("#video-01 iframe").attr("src", $("#video-01 iframe").attr("src"));
    });  


/*Mega Menu*/
$(function () {
    var selectedClass = "";
    $(".fil-cat").click(function () {
        selectedClass = $(this).attr("data-rel");
        $("#tabbedmenu").fadeTo(100, 0.1);
        $("#tabbedmenu li").not("." + selectedClass).fadeOut(50).removeClass('scale-anm');
        setTimeout(function () {
            $("." + selectedClass).fadeIn().addClass('scale-anm');
            $("#tabbedmenu").fadeTo(300, 1);
        }, 300);

    });
});
/*NewCareer Carousel JS*/
$("#career-carousel").owlCarousel({
    nav: false,
    loop: false,
    margin: 0,
    dots: true,
    responsive: {
        // breakpoint from 0 up
        0: {
            items: 1
        },
        // breakpoint from 480 up
        480: {
            items: 1
        },
        // breakpoint from 768 up
        768: {
            items: 1,
            nav: false,
            dots: true,
            autoplay: true,

        }
    }
});



$("#GoBackBtn").click(function () {
    window.history.back();
});

/*Scroll To top JS*/
// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        document.getElementById("scrolltop").style.display = "block";
    } else {
        document.getElementById("scrolltop").style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}
$("#disclaimerbtn").click(function () {
    $('#roadblock').modal();
}); 



// When the user scrolls the page, execute myFunction 
window.onscroll = function () { myFunction() };

// Get the navbar
var navbar = document.getElementsByClassName("navPanel");

// Get the offset position of the navbar
var sticky = navbar[0].offsetTop;

// Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
function myFunction() {
    if (window.pageYOffset >= sticky) {
        navbar[0].classList.add("sticky")
    } else {
        navbar[0].classList.remove("sticky");
    }
}



$(function () {
    $('#PropertyName').change(function () {

        $('#business-parent .py-lg-4').hide();
        //$('#business-parents .property-details').hide();
        var propValue = $(this).val();
        propValue = propValue.replace(/\s/g, '-');
        $('#' + propValue).show();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/Realty/BookNowPropertyTypes",
            data: { PropName: propValue },
            success: function (obj) {
                $('#PropertyType').text('');                
                for (var i = 0; i < obj.PropertiesTypeList.length; i++) {                    
                    var name = obj.PropertiesTypeList[i]['PropertySelection'];
                    $('#PropertyType').append("<option value='" + name + "'>" + name + "</option>");
                }
                $('#CarpetArea').text('');
                $('#CarpetArea').append(obj.CarpetArea);
                $('#StartingPrice').text('');
                $('#StartingPrice').append('<i class="fa fa-rupee-sign mr-2"></i>' + obj.BookingPrice);
                $('#BookingAmount').text('');
                $('#BookingAmount').append('<i class="fa fa-rupee-sign mr-2"></i>' + obj.BookingAmount);
            },
            error: function () {
                alert("error");
            }
        });
        $('#' + propValue + '-details').show();
    });
});


// $(document).ajaxComplete(function () {
    // $('.propertyselections').change(function () {
        // var z = $(this).val();
        // z = z.replace('.', '');
        // z = z.replace(/\s/g, '');
        // var propName = $('#PropertyName').val();
        // $(".propertytype").hide();
        // $("." + propName + "-" + z).show();

    // });
	
	// $('.btnNext').click(function () {
    // $('.nav-tabs > .active').next('li').find('a').trigger('click');
    // $('html, body').animate({
        // scrollTop: $("#navtabs").offset().top
            // - 100
    // }, 1000);
// });
// });
  


$('.propertyselections').change(function () {
    var z = $(this).val();
    var propName = $('#PropertyName').val();
    $.ajax({
        type: "GET",
        url: "/api/Realty/BookNowPropertyTypesList",
        data: { PropName: propName, PropertyType:z },
        success: function (obj) {
            $('#CarpetArea').text('');
            $('#CarpetArea').append(obj.CarpetArea);
            $('#StartingPrice').text('');
            $('#StartingPrice').append('<i class="fa fa-rupee-sign mr-2"></i>' + obj.BookingPrice);
            $('#BookingAmount').text('');
            $('#BookingAmount').append('<i class="fa fa-rupee-sign mr-2"></i>' + obj.BookingAmount);
        },
        error: function () {
            alert("error");
        }
    });

});

$('.btnNext1').click(function(){
          $('.nav-tabs > .active').next('li').find('a').trigger('click');
          $('html, body').animate({
            scrollTop: $("#navtabs").offset().top
                - 100
        }, 1000);
        });
		
$('#home .btnNext').click(function(){
	$('#home .collapse').removeClass('show');
	$('#home a.d-md-none').addClass('collapsed');
		$('#menu1 .collapse').addClass('show');
	$('#menu1 a.d-md-none').removeClass('collapsed');
});

$("#home .btnNext").click(function() {
    $('html, body').animate({
        scrollTop: $("#formRealtyBookNow").offset().top
    }, 2000);
});
	
		
		
		
		
		

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
        //if ($($(e.currentTarget)[0]).find('img')[0] != undefined) {
        //    $("#limenu").remove($($(e.currentTarget)[0]).find('img')[0]);
        //}
        //$($($($("#ulimg").children())[$("#ulimg").children().length - 1])).find('img')[0].src = "";
    });
	
function codeAddress() {
           if(window.location.href=="https://www.adanirealty.com/blogs/embrace-quality-lifestyle-and-livemore-in-shantigram")
	{
		var y=$(".breadcrumb li");
         $(y[0]).hide();
	}
        }
        window.onload = codeAddress;



$('.submenu a[href^="#"]').on('click', function (e) {
        var href = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(href).offset().top - 170
        }, '600');
        e.preventDefault();
    });
	

	

$('.popup-gallery').magnificPopup({
		delegate: '.owl-item:not(.cloned) a',
		type: 'image',
		removalDelay: 500, //delay removal by X to allow out-animation
		callbacks: {
			beforeOpen: function() {
				// just a hack that adds mfp-anim class to markup 
				 this.st.image.markup = this.st.image.markup.replace('mfp-figure', 'mfp-figure mfp-with-anim');
				 this.st.mainClass = this.st.el.attr('data-effect');
			}
		},
		tLoading: 'Loading image #%curr%...',
		mainClass: 'mfp-img-mobile',
		gallery: {
			enabled: true,
			navigateByImgClick: true,
			preload: [0,1] // Will preload 0 - before current, and 1 after the current image
		},
		image: {
			tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
			titleSrc: function(item) {
				return item.el.attr('title') + '<small></small>';
			}
		}
	});
	
	
$('.popup-gallery-sub1').magnificPopup({
		delegate: '.owl-item:not(.cloned) a',
		type: 'image',
		removalDelay: 500, //delay removal by X to allow out-animation
		callbacks: {
			beforeOpen: function() {
				// just a hack that adds mfp-anim class to markup 
				 this.st.image.markup = this.st.image.markup.replace('mfp-figure', 'mfp-figure mfp-with-anim');
				 this.st.mainClass = this.st.el.attr('data-effect');
			}
		},
		tLoading: 'Loading image #%curr%...',
		mainClass: 'mfp-img-mobile',
		gallery: {
			enabled: true,
			navigateByImgClick: true,
			preload: [0,1] // Will preload 0 - before current, and 1 after the current image
		},
		image: {
			tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
			titleSrc: function(item) {
				return item.el.attr('title') + '<small></small>';
			}
		}
	});
	
	
	
$(document).ready(function () {
	var owl = $('.video-banner');
  owl.owlCarousel({
      loop:true,
      margin:0,
	  lazyLoad: true,
      nav:false,
      dots:true,
	  animateOut: 'fadeOut',
	  animateIn: 'fadeIn',
	  autoHeight:true,
	  autoplay: false,
	  autoplayTimeout: 20000,
		slideSpeed: 0,
	  lazyload:true,
      items:1
  })
  owl.on('translate.owl.carousel',function(e){
    $('.video-banner .owl-item video').each(function(){
      $(this).get(0).pause();
	  $(this).get(0).currentTime = 0;
    });
  });
  $('.video-banner .owl-item .item').each(function(){
      var attr = $(this).attr('data-videosrc');
      if (typeof attr !== typeof undefined && attr !== false) {
        var videosrc = $(this).attr('data-videosrc');
        $(this).prepend('<video preload="auto" loop="true" autoplay="true" muted><source src="'+videosrc+'" type="video/mp4"></video>');
		 $('.video-banner .owl-item.active video');
      }
    });
  owl.on('translated.owl.carousel',function(e){
	$('.video-banner .owl-item.active video').get(0).play();
  });
	
	$(".video-banner").carousel({ interval: false}); // this prevents the auto-loop
    var videos = document.querySelectorAll("video");
    videos.forEach(function(e) {
        e.addEventListener('ended', myHandler, false);
    });
});
//For Rental Form start
var recaptcha1;
var onloadCallback = function () {
    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc', //Replace this with your Site key
        'theme': 'light'
    });
};
$('form').on('blur', 'input[type=number]', function (e) {
    $(this).off('wheel.disableScroll')
});
$(document).ready(function () {
    setTimeout(function () {

        var head = document.getElementsByTagName('head')[0]
        var script = document.createElement('script')
        script.type = 'text/javascript';
        script.src = "https://www.google.com/recaptcha/api.js?onload=onloadCallback&render=explicit"
        head.appendChild(script);
    }, 1500);
});
if($('#SubmitRentalForm').length) {
    $('input[type="text"]').on('input', function (e) {
        var regKey = $(this).attr('data-val-regex-pattern');
        var regMsg = $(this).attr('data-val-regex');
        var ValKey = $(this).attr('required');
        var ValMsg = $(this).attr('data-val-required');
        if ($(this).prop('required')) {
            if ($(this).val() == '') {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(ValMsg);
            }
            else if (!$(this).val().match(regKey)) {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(regMsg);
            }
            else {
                $(this).siblings('p:first').empty();
            }
        }
    });
    $('input[type="text"]').on('blur', function (e) {
        var ValKey = $(this).attr('required');
        var ValMsg = $(this).attr('data-val-required');
        if ($(this).prop('required')) {
            if ($(this).val() == '') {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(ValMsg);
            }
            else {
                $(this).siblings('p:first').empty();
            }
        }
    });
    $('input[type="email"]').on('input', function (e) {
        var regKey = $(this).attr('data-val-regex-pattern');
        var regMsg = $(this).attr('data-val-regex');
        var ValKey = $(this).attr('required');
        var ValMsg = $(this).attr('data-val-required');
        if ($(this).prop('required')) {
            if ($(this).val() == '') {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(ValMsg);
            }
            else if (!$(this).val().match(regKey)) {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(regMsg);
            }
            else {
                $(this).siblings('p:first').empty();
            }
        }
    });
    $('input[type="email"]').on('blur', function (e) {
        var ValKey = $(this).attr('required');
        var ValMsg = $(this).attr('data-val-required');
        if ($(this).prop('required')) {
            if ($(this).val() == '') {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(ValMsg);
            }
            else {
                $(this).siblings('p:first').empty();
            }
        }
    });

    $('input[type="number"]').on('input', function (e) {
        var regKey = $(this).attr('data-val-regex-pattern');
        var regMsg = $(this).attr('data-val-regex');
        var ValKey = $(this).attr('required');
        var ValMsg = $(this).attr('data-val-required');
        if ($(this).prop('required')) {
            if ($(this).val() == '') {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(ValMsg);
            }
            else if (!$(this).val().match(regKey)) {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(regMsg);
            }
            else {
                $(this).siblings('p:first').empty();
            }
        }
    });
    $('input[type="number"]').on('blur', function (e) {
        var ValKey = $(this).attr('required');
        var ValMsg = $(this).attr('data-val-required');
        if ($(this).prop('required')) {
            if ($(this).val() == '') {
                $(this).siblings('p:first').empty();
                $(this).siblings('p:first').html(ValMsg);
            }
            else {
                $(this).siblings('p:first').empty();
            }
        }
    });
};
//For Rental Form end



