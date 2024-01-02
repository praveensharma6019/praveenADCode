$(window).on('load', function() {
	setTimeout(function() { $("#Enquire-now").modal('show'); }, 500);
        $("#Enquire-now").modal('show');
    });
$(document).ready(function(){
	
	$('.hero-banner').owlCarousel({
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
	
	$('a.btn-gallery').on('click', function(event) {
		event.preventDefault();
		
		var gallery = $(this).attr('href');
    
		$(gallery).magnificPopup({
      delegate: 'a',
			type:'image',
			gallery: {
				enabled: true
			}
		}).magnificPopup('open');
	});
	
	$('a.btn-floor-gallery').on('click', function(event) {
		event.preventDefault();
		
		var gallery = $(this).attr('href');
    
		$(gallery).magnificPopup({
      delegate: 'a',
			type:'image',
			gallery: {
				enabled: true
			}
		}).magnificPopup('open');
	});
	
	setTimeout(function(){
       $(".parallax__layer__1").addClass("active");
   }, 500);
	
});
 
/*Smooth Scrool*/
$('.anchor-link').click(function(e){
  e.preventDefault();
  var target = $($(this).attr('href'));
  if(target.length){
    var scrollTo = target.offset().top;
    $('body, html').animate({scrollTop: scrollTo-100+'px'}, 800);
  }
});



$('.hamburger_icon').click(function(){
	$('.modal_header').toggleClass('show');
	$('.enquire_now').removeClass('active');
});


$('.download_brochure').on('click',function(){
	$('.modal_header').removeClass('show');
});



$('.modal_header--menu li').click(function(){
	$('.modal_header').toggleClass('show');
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
        $('#back-to-top').addClass('show');
        $('header').addClass('sticky-header');
        if (st > lastScrollTop && st > navbarHeight) {
            // Scroll Down
			//$('.btm-floating').addClass('active');

        } else {
            // Scroll Up
            if (st + $(window).height() < $(document).height()) {

            }
        }
        if (st < 250) {

            $('#back-to-top').removeClass('show');
            $('header').removeClass('sticky-header');
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
	
	
	setTimeout(function () {
        $('body').removeClass('overflow-hidden');
    }, 4500);
	
	setTimeout(function () {
        $('.reveal_banner').addClass('v_hidden');
    }, 5500);
	
	setTimeout(function () {
        $('body').removeClass('overflow-hidden');
        $('.reveal_banner').addClass('show');
    }, 1000);
	
	
/*Menu selectors*/
class NavBehavior {
  
  constructor(){
    this.currentId = null;
    this.navContainerHeight = 70;
    $(window).scroll(() => {
        this.scrollDistance();
        this.findCurrentSection();
    });
  }

  
  scrollDistance() {
        $("#showscroll").text(function(){
          return "Scroll Distance is " + window.scrollY + "px from Top.";
        });
        $("#showscroll").css("display", "unset");
    }


  findCurrentSection(element) {
      let newCurrentId;
      let self = this;
      $('.anchor-link').each(function() {
        let id = $(this).attr('href');
        let offsetTop = $(id).offset().top - self.navContainerHeight;
        let offsetBottom = $(id).offset().top + $(id).height() - self.navContainerHeight;
        if($(window).scrollTop() > offsetTop && $(window).scrollTop() < offsetBottom) {
          newCurrentId = id;
          $(this).addClass('active');
        }
      });
      if(this.currentId != newCurrentId || this.currentId === null) {
        this.currentId = newCurrentId;
        $('.active').removeClass('active');
        if (this.currentId != null){
          //console.log ("You are viewing " + this.currentId);
          //in this we can add a history.pushState or window.location thing to change the url when a new section has been reached
        }
      }
   }
}
  
new NavBehavior();

$('#btn_disclaimer').click(function(){
	$('.disclaimer_text').toggleClass('d-block');
	$('#btn_disclaimer span').toggleClass('active');
	$('html, body').animate({
	scrollTop: $('.disclaimer_text').offset().top - 100
	}, '600');
})

// form submit code

var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}

function SendOtpMobile(event, t) {
    var self = $(t);
	var valattr = $(t).val();
	if (valattr!="") {
    var mobile = $(t).val();
	}else{
		var mobile = $(t).attr("data-value");
	}
	
    var count = parseInt($(t).attr("data-click"));
    var attr = $(t).attr("data-id");
	
   // var errorclass = $(t).attr("data-class");
    if (mobile.length === 10 && regex_special_num.test(mobile) === true) {
		
		self.prev().show();
		
        self.prev().html('Please Wait....');
		

	if(count<3)
	{		
        $.ajax({
            type: 'POST',
            data: { mobile: mobile },
            url: "/api/Realty/PlatinumRealtySendOtp",
            success: function (data) {
                if (data.status === "1") {
                    self.attr('data-click', count + 1);
                    //$("." + attr).show();
                    //$("." + errorclass).html("");
                    //$("#input-otp-button").hide();
                    //self.attr('readonly',true);
                    self.hide();
					self.prev().html('You will receive an otp on your number');
					$("#"+attr +' #otp-button a').attr("data-value",mobile);
					OTPTimer("#"+attr);
                }else if(data.status === "503"){
					alert("You have sent 3 times OTP Please try again after 30 minutes")
				}

            },

            error: function (data) {
               self.prev().html("Some technical issue please after some time");  // 
            }
        });
	}else
	{
		

		$("#"+attr +' #otp-button').text("Otp limit reached");
	}

    }
    else {
        self.prev().html("Please enter a valid Mobile Number");
    }

}

// function ShowOtpMobile(event, t) {
    // var self = $(t);
	// var valattr = $(t).val();
	// if (valattr!="") {
    // var mobile = $(t).val();
	// }else{
		// var mobile = $(t).attr("data-value");
	// }
	
    // var count = parseInt($(t).attr("data-click"));
    // var attr = $(t).attr("data-id");
   // // var errorclass = $(t).attr("data-class");
    // if (mobile.length === 10 && regex_special_num.test(mobile) === true) {

        // self.next().next('.send-otp').show();
		// self.next().next('.send-otp').children().attr("data-value",mobile);
		// self.next().hide();
		// self.next().removeClass('d-block');
		

    // }
    // else {
        // self.next().html("Please enter a valid Mobile Number");
		// self.next().next('.send-otp').hide();
    // }

// }

function GetURLParameter(sParam) {
			var url_string = window.location.href;
			var url = new URL(url_string);
			var c = url.searchParams.get(sParam);
			return c
		}
	var utm_source = GetURLParameter('utm_source');
	var utm_medium = GetURLParameter('utm_medium');
	var utm_campaign = GetURLParameter('utm_campaign');
	
function OTPTimer(formid) {
			var counter = 60;
			$(formid +' #otp-button').hide();
		$(formid +' #timer').show();
	var interval = setInterval(function() {
    counter--;

    // Display 'counter' wherever you want to display it.
    if (counter <= 0) {
     		
      	$(formid +' #timer').hide();
		$(formid +' #otp-button').show();
		clearInterval(interval);
        return;
    }else{
		
    	$(formid +' #timer').html('<i class="fa fa-redo-alt"></i> Resend OTP in <span>00:'+counter+'</span>');
      
    }
}, 1000);
		}
$("#form_fot").click(function (e) {
	$('#form_fot').attr('disabled','true');
    e.preventDefault();
    if (!$("#full_name_fot").val()) {
        $("#full_name_fot").next().html('Please Enter Your Name!');
		$('#form_fot').removeAttr('disabled');
        return false;
    } else {
        $("#full_name_fot").next().html("");
    }

   

    if (!$("#email_fot").val()) {
        $("#email_fot").next().html('Please Enter Your Email ID!');
		$('#form_fot').removeAttr('disabled');
        return false;
    } else {
        $("#email_fot").next().html("");
    }

    

    if (validateEmail($("#email_fot").val()) == false) {
        $("#email_fot").next().html("Please enter proper Email ID!");
		$('#form_fot').removeAttr('disabled');
        return false;
    } else {
        $("#email_fot").next().html("");
    }

    if (!$("#phone_number_fot").val()) {
        $("#phone_number_fot").next().html('Please Enter Your Mobile Number!');
		$('#form_fot').removeAttr('disabled');
        return false;
    } else {
        $("#phone_number_fot").next().html("");
    }


    if ($("#phone_number_fot").val().length !== 10 || regex_special_num.test($("#phone_number_fot").val()) == false) {
        $("#phone_number_fot").next().html("Please enter 10 digit Mobile Number!");
		$('#form_fot').removeAttr('disabled');
        return false;
    } else {
        $("#phone_number_fot").next().html("");
    } 
//if($("#form_fot").val()=="submit")
//{
	// if (!$("#otp_fot").val()) {
		// $("#otp_fot").next().show();
        // $("#otp_fot").next().html('Please Enter OTP!');
        // return false;
    // } else {
        // $("#otp_fot").next().html("");
    // }
	   
    
    var savecustomdata = {
                                        full_name: $("#full_name_fot").val(),
                                        last_name: "",
										mobile: $("#phone_number_fot").val(),
                                        email: $("#email_fot").val(),
                                        Budget: "",
                                        country_code: "",
                                        state_code: "",
                                        Projects_Interested__c: "RESIDENTIAL",
                                        PropertyLocation: "gurgaon",
                                        sale_type: "",
                                        Remarks: "",
                                        FormType: "Platinum Realty Footer enquire from",
                                        PageInfo: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/InsertPlatinumRealtycontactdetail",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
                            
								$('#success').modal('hide');
								$('#footer_form #otp-button').hide();
								$('#footer_form #timer').hide();
								document.getElementById("footer_form").reset();
                            window.location.href = "/platinum-tower/thank-you?utm_source=" + utm_source;
                        } 
						// else if (data.status === "102"){
                            // $("#form_fot").next().html('please enter valid otp');
                        // }
						else if(data.status === "401"){
							$("#full_name_fot").next().html('Please Enter Your Name!');
							$('#form_fot').removeAttr('disabled');
						}else if(data.status === "403"){
							$("#email_fot").next().html('Please enter proper Email ID!');
							$('#form_fot').removeAttr('disabled');
						}else if(data.status === "405"){
							$("#phone_number_fot").next().html('Please Enter Your Mobile Number!');
							$('#form_fot').removeAttr('disabled');
						}
						// else if(data.status === "406"){
							// $("#otp_fot").next().html('Please Enter OTP!');
						// }else if(data.status === "103"){
							 // $("#form_fot").next().html('You Entered 3 times wrong otp please send otp again');
							 // $('#footer_form #otp-button').hide();
							 // $("#phone_number_fot").next().next().show();
							 // $("#phone_number_fot").next().next().children().show();
						// }
						// else if(data.status === "108"){
							// $("#form_fot").next().html('Please send otp on your mobile number');
							 // $('#footer_form #otp-button').hide();
							 // $("#phone_number_fot").next().next().show();
							 // $("#phone_number_fot").next().next().children().show();
						// }

                    },

                    error: function (data) {
                        $("#form_fot").next().html('Some technical issue please try after some time'); // 
						$('#form_fot').removeAttr('disabled');
                    }
                });
//}
 
/*else if($("#form_fot").val()=="send-otp")
{
	SendOtpMobile(event,$("#phone_number_fot")[0]);
	$("#form_fot").val("submit");
	$("#form_fot").html("Submit");
	OTPTimer("#form_fot");
}*/
    

});

$("#form_db").click(function (e) {
	$('#form_db').attr('disabled','true');
    e.preventDefault();
    if (!$("#full_name_db").val()) {
        $("#full_name_db").next().html('Please Enter Your Name!');
		$('#form_db').removeAttr('disabled');
        return false;
    } else {
        $("#full_name_db").next().html("");
    }

   

    if (!$("#email_db").val()) {
        $("#email_db").next().html('Please Enter Your Email ID!');
		$('#form_db').removeAttr('disabled');
        return false;
    } else {
        $("#email_db").next().html("");
    }

    

    if (validateEmail($("#email_db").val()) == false) {
        $("#email_db").next().html("Please enter proper Email ID!");
		$('#form_db').removeAttr('disabled');
        return false;
    } else {
        $("#email_db").next().html("");
    }

    if (!$("#phone_number_db").val()) {
        $("#phone_number_db").next().html('Please Enter Your Mobile Number!');
		$('#form_db').removeAttr('disabled');
        return false;
    } else {
        $("#phone_number_db").next().html("");
    }


    if ($("#phone_number_db").val().length !== 10 || regex_special_num.test($("#phone_number_db").val()) == false) {
        $("#phone_number_db").next().html("Please enter 10 digit Mobile Number!");
		$('#form_db').removeAttr('disabled');
        return false;
    } else {
        $("#phone_number_db").next().html("");
    }
    
    
    

    // if (!$("#otp_db").val()) {
		// $("#otp_db").next().show();
        // $("#otp_db").next().html('Please Enter OTP!');
        // return false;
    // } else {
        // $("#otp_db").next().html("");
    // }
    
    var savecustomdata = {
                                        full_name: $("#full_name_db").val(),
                                        last_name: "",
										mobile: $("#phone_number_db").val(),
                                        email: $("#email_db").val(),
                                       Budget: "",
                                        country_code: "",
                                        state_code: "",
                                        Projects_Interested__c: "RESIDENTIAL",
                                        PropertyLocation: "gurgaon",
                                        sale_type: "",
                                        Remarks: "",
                                        FormType: "Platinum Realty download brochure from",
                                        PageInfo: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/InsertPlatinumRealtycontactdetail",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
							
								$('#Download-brochure').modal('hide');
								$('#success').modal('hide');
                            //window.location.href = "-/media-archive/brochure.pdf";
							$("#download-file").trigger('click');
							$('#bro_form #otp-button').hide();
								$('#bro_form #timer').hide();
								document.getElementById("bro_form").reset();
								setTimeout(
								  function() 
								  {
									window.location.href = "/platinum-tower/thank-you?utm_source=" + utm_source;
								  }, 2000);
								
                        } 
						// else if (data.status === "102"){
                            // $("#form_db").next().html('please enter valid otp');
                        // }
						else if(data.status === "401"){
							$("#full_name_db").next().html('Please Enter Your Name!');
							$('#form_db').removeAttr('disabled');
						}else if(data.status === "403"){
							$("#email_db").next().html('Please enter proper Email ID!');
							$('#form_db').removeAttr('disabled');
						}else if(data.status === "405"){
							$("#phone_number_db").next().html('Please Enter Your Mobile Number!');
							$('#form_db').removeAttr('disabled');
						}
						// else if(data.status === "406"){
							// $("#otp_db").next().html('Please Enter OTP!');
						// }else if(data.status === "103"){
							 // $("#form_db").next().html('You Entered 3 times wrong otp please send otp again');
							 // $('#bro_form #otp-button').hide();
							 // $("#phone_number_db").next().next().show();
							 // $("#phone_number_db").next().next().children().show();
						// }
						// else if(data.status === "108"){
							// $("#form_db").next().html('Please send otp on your mobile number');
							 // $('#bro_form #otp-button').hide();
							 // $("#phone_number_db").next().next().show();
							 // $("#phone_number_db").next().next().children().show();
						// }

                    },

                    error: function (data) {
                        $("#form_db").next().html('Some technical issue please try after some time'); // 
						$('#form_db').removeAttr('disabled');
                    }
                });

    

});

$("#form_eq").click(function (e) {
	$('#form_eq').attr('disabled','true');
    e.preventDefault();
    if (!$("#full_name_eq").val()) {
        $("#full_name_eq").next().html('Please Enter Your Name!');
		$('#form_eq').removeAttr('disabled');
        return false;
    } else {
        $("#full_name_eq").next().html("");
    }

   

    if (!$("#email_eq").val()) {
        $("#email_eq").next().html('Please Enter Your Email ID!');
		$('#form_eq').removeAttr('disabled');
        return false;
    } else {
        $("#email_eq").next().html("");
    }

    

    if (validateEmail($("#email_eq").val()) == false) {
        $("#email_eq").next().html("Please enter proper Email ID!");
		$('#form_eq').removeAttr('disabled');
        return false;
    } else {
        $("#email_eq").next().html("");
    }

    if (!$("#phone_number_eq").val()) {
        $("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
		$('#form_eq').removeAttr('disabled');
        return false;
    } else {
        $("#phone_number_eq").next().html("");
    }


    if ($("#phone_number_eq").val().length !== 10 || regex_special_num.test($("#phone_number_eq").val()) == false) {
        $("#phone_number_eq").next().html("Please enter 10 digit Mobile Number!");
		$('#form_eq').removeAttr('disabled');
        return false;
    } else {
        $("#phone_number_eq").next().html("");
    }
    
    
    

    // if (!$("#otp_eq").val()) {
		// $("#otp_eq").next().show();
        // $("#otp_eq").next().html('Please Enter OTP!');
        // return false;
    // } else {
        // $("#otp_eq").next().html("");
    // }
    
    var savecustomdata = {
                                        full_name: $("#full_name_eq").val(),
                                        last_name: "",
										mobile: $("#phone_number_eq").val(),
                                        email: $("#email_eq").val(),
                                        Budget: "",
                                        country_code: "",
                                        state_code: "",
                                        Projects_Interested__c: "RESIDENTIAL",
                                        PropertyLocation: "gurgaon",
                                        sale_type: "",
                                        Remarks: "",
                                        FormType: "Platinum Realty enquire from",
                                        PageInfo: window.location.href,
                                        FormSubmitOn: new Date().toISOString().slice(0, 19).replace('T', ' '),
                                        UTMSource: utm_source
                                    };

	$.ajax({
                    type: "POST",
                    data: JSON.stringify(savecustomdata),
                    url: "/api/Realty/InsertPlatinumRealtycontactdetail",
                    contentType: "application/json",
                    success: function (data) {
                        if (data.status === "101") {
							
								$('#Enquire-now').modal('hide');
								$('#success').modal('hide');
								$('#eq_form #otp-button').hide();
								$('#eq_form #timer').hide();
                            //window.location.href = "/thankyou";
							document.getElementById("eq_form").reset();
							window.location.href = "/platinum-tower/thank-you?utm_source=" + utm_source;
                        }
						// else if (data.status === "102"){
                            // $("#form_eq").next().html('please enter valid otp');
                        // }
						else if(data.status === "401"){
							$("#full_name_eq").next().html('Please Enter Your Name!');
							$('#form_eq').removeAttr('disabled');
						}else if(data.status === "403"){
							$("#email_eq").next().html('Please enter proper Email ID!');
							$('#form_eq').removeAttr('disabled');
						}else if(data.status === "405"){
							$("#phone_number_eq").next().html('Please Enter Your Mobile Number!');
							$('#form_eq').removeAttr('disabled');
						}
						// else if(data.status === "406"){
							// $("#otp_eq").next().html('Please Enter OTP!');
						// }else if(data.status === "103"){
							 // $("#form_eq").next().html('You Entered 3 times wrong otp please send otp again');
							 // $('#eq_form #otp-button').hide();
							 // $("#phone_number_eq").next().next().show();
							 // $("#phone_number_eq").next().next().children().show();
						// }
						// else if(data.status === "108"){
							// $("#form_eq").next().html('Please send otp on your mobile number');
							 // $('#eq_form #otp-button').hide();
							 // $("#phone_number_eq").next().next().show();
							 // $("#phone_number_eq").next().next().children().show();
						// }

                    },

                    error: function (data) {
                        $("#form_eq").next().html('Some technical issue please try after some time'); // 
						$('#form_eq').removeAttr('disabled');
                    }
                });

    

});

function DownloadFile(fileName) {
            //Set the File URL.
            var url = "-/media-archive/brochure.pdf";
 
            //Create XMLHTTP Request.
            var req = new XMLHttpRequest();
            req.open("GET", url, true);
            req.responseType = "blob";
            req.onload = function () {
                //Convert the Byte Data to BLOB object.
                var blob = new Blob([req.response], { type: "application/octetstream" });
 
                //Check the Browser type and download the File.
                var isIE = false || !!document.documentMode;
                if (isIE) {
                    window.navigator.msSaveBlob(blob, fileName);
                } else {
                    var url = window.URL || window.webkitURL;
                    link = url.createObjectURL(blob);
                    var a = document.createElement("a");
                    a.setAttribute("download", fileName);
                    a.setAttribute("href", link);
                    document.body.appendChild(a);
                    a.click();
                    document.body.removeChild(a);
                }
            };
            req.send();
        };


<!-- $('.modal_header--menu ul li').hover(function(){ -->
  <!-- $('.modal_header--menu ul li').removeClass('active'); -->
  <!-- $(this).addClass('active'); -->
  
  <!-- var data = $(this).attr('data-filter'); -->
  <!-- $grid.isotope({ -->
    <!-- filter: data -->
  <!-- }) -->
<!-- }); -->

<!-- var $grid = $(".grid").isotope({ -->
  <!-- itemSelector: ".all", -->
  <!-- percentPosition: true, -->
  <!-- masonry: { -->
    <!-- columnWidth: ".all" -->
  <!-- } -->
<!-- }) -->


jQuery(document).ready(function() {

  /* new changes*/jQuery(".close-enquiry").click(function(){jQuery(".section_enquire").removeClass("active");jQuery(".section_enquire").addClass("eqclose")})

    var sync1 = jQuery("#project__sync--main");
    var sync2 = jQuery("#project__sync--thumb");
    var slidesPerPage = 1; //globaly define number of elements per page
    var syncedSecondary = true;

    sync1
      .owlCarousel({
      items: 1,
      slideSpeed: 3000,
      nav: false,

      //   animateOut: 'fadeOut',
      animateIn: "fadeIn",
      autoplayHoverPause: true,
      autoplaySpeed: 1400,
      dots: false,
      loop: true,
      responsiveClass: true,
      responsive: {
        0: {
          item: 1,
          autoplay: false
        },
        600: {
          items: 1,
          autoplay: true
        }
      },
      responsiveRefreshRate: 200,
      navText: [
        
      ]
    })
      .on("changed.owl.carousel", syncPosition);

    sync2
      .on("initialized.owl.carousel", function() {
      sync2
        .find(".owl-item")
        .eq(0)
        .addClass("current");
    })
      .owlCarousel({
      items: slidesPerPage,
      dots: false,
      nav: true,
      smartSpeed: 1000,
      slideSpeed: 1000,
      slideBy: slidesPerPage, 
	responsiveClass: true,
      responsive: {
        0: {
          item: 1
        },
        600: {
          items: 1
        },
	1000: {
          items: 4
        }
      },
      responsiveRefreshRate: 100
    })
      .on("changed.owl.carousel", syncPosition2);

    function syncPosition(el) {
      //if you set loop to false, you have to restore this next line
      //var current = el.item.index;

      //if you disable loop you have to comment this block
      var count = el.item.count - 1;
      var current = Math.round(el.item.index - el.item.count / 2 - 0.5);

      if (current < 0) {
        current = count;
      }
      if (current > count) {
        current = 0;
      }

      //end block

      sync2
        .find(".owl-item")
        .removeClass("current")
        .eq(current)
        .addClass("current");
      var onscreen = sync2.find(".owl-item.active").length - 1;
      var start = sync2
      .find(".owl-item.active")
      .first()
      .index();
      var end = sync2
      .find(".owl-item.active")
      .last()
      .index();

      if (current > end) {
        sync2.data("owl.carousel").to(current, 100, true);
      }
      if (current < start) {
        sync2.data("owl.carousel").to(current - onscreen, 100, true);
      }
    }

    function syncPosition2(el) {
      if (syncedSecondary) {
        var number = el.item.index;
        sync1.data("owl.carousel").to(number, 100, true);
      }
    }

    sync2.on("click", ".owl-item", function(e) {
      e.preventDefault();
      var number = jQuery(this).index();
      sync1.data("owl.carousel").to(number, 300, true);
    });
  });
  
  
  
  
  jQuery(document).ready(function() {
    var gsync1 = jQuery("#gallery__sync--main");
    var gsync2 = jQuery("#gallery__sync--thumb");
    var slidesPerPage = 5; //globaly define number of elements per page
    var syncedSecondary = true;

    gsync1
      .owlCarousel({
      items: 1,
      slideSpeed: 3000,
      nav: true,

      //   animateOut: 'fadeOut',
      animateIn: "fadeIn",
      autoplayHoverPause: true,
      autoplaySpeed: 1400,
      dots: false,
      loop: true,
	center:true,
      responsiveClass: true,
      responsive: {
        0: {
          item: 1,
          autoplay: false,
	stagePadding: 50
        },
        600: {
          items: 1,
          autoplay: true,
	stagePadding: 200,
	nav:false
        },
	1000: {
          items: 1,
          autoplay: true,
	stagePadding: 320
        }

      },
      responsiveRefreshRate: 200,
      navText: [
        
      ]
    })
      .on("changed.owl.carousel", syncPosition);

    gsync2
      .on("initialized.owl.carousel", function() {
      gsync2
        .find(".owl-item")
        .eq(0)
        .addClass("current");
    })
      .owlCarousel({
      items: slidesPerPage,
      dots: false,
      //   nav: true,
      smartSpeed: 1000,
      slideSpeed: 1000,
      slideBy: slidesPerPage, //alternatively you can slide by 1, this way the active slide will stick to the first item in the second carousel
      responsiveRefreshRate: 100
    })
      .on("changed.owl.carousel", syncPosition2);

    function syncPosition(el) {
      //if you set loop to false, you have to restore this next line
      //var current = el.item.index;

      //if you disable loop you have to comment this block
      var count = el.item.count - 1;
      var current = Math.round(el.item.index - el.item.count / 2 - 0.5);

      if (current < 0) {
        current = count;
      }
      if (current > count) {
        current = 0;
      }

      //end block

      gsync2
        .find(".owl-item")
        .removeClass("current")
        .eq(current)
        .addClass("current");
      var onscreen = gsync2.find(".owl-item.active").length - 1;
      var start = gsync2
      .find(".owl-item.active")
      .first()
      .index();
      var end = gsync2
      .find(".owl-item.active")
      .last()
      .index();

      if (current > end) {
        gsync2.data("owl.carousel").to(current, 100, true);
      }
      if (current < start) {
        gsync2.data("owl.carousel").to(current - onscreen, 100, true);
      }
    }

    function syncPosition2(el) {
      if (syncedSecondary) {
        var number = el.item.index;
        gsync1.data("owl.carousel").to(number, 100, true);
      }
    }

    gsync2.on("click", ".owl-item", function(e) {
      e.preventDefault();
      var number = jQuery(this).index();
      gsync1.data("owl.carousel").to(number, 300, true);
    });
  });


$("#gallery__sync--main")
  .on("initialized.owl.carousel changed.owl.carousel", function(e) {
    if (!e.namespace) {
      return;
    }
    $(".gallery__carousel--counter").text(
      e.relatedTarget.relative(e.item.index) + 1 + "/" + e.item.count
    );
  });


$(window).scroll(function(){
    if ($(this).scrollTop() > 550) {


	$('.enquire_now').addClass('active');
  if(!$('.section_enquire').hasClass('eqclose')) {
	$('.section_enquire').addClass('active');
  }
    } else {
$('.enquire_now').removeClass('active');
$('.section_enquire').removeClass('active');
    }
});