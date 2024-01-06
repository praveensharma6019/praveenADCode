
let elemadv = document.querySelector('.team-wrap .right-area .img-section'); 
if(elemadv){
let cloneadv = elemadv.outerHTML;
document.querySelector('.team-head').insertAdjacentHTML('afterend',cloneadv);
}
function domExe() {
    document.querySelector(".mobile-trigger").addEventListener("click", function() {
        document.querySelector('body').classList.toggle('menu-open');
    });
    if (document.querySelectorAll('.advHome-banner').length === 0) {
        document.querySelector('body').classList.add('banner-is-not-here');
    }
}

document.addEventListener("DOMContentLoaded", function() {
    domExe();
});  

const setCookie = (name) => {
        document.cookie = name + "=accepted; path=/";
    };
    const getCookie = (name) => {
        return document.cookie.match("(^|;)\\s*" + name + "\\s*=\\s*([^;]+)");
    };
    const cookieHandler = () => {
        setCookie("adlCookies");
    };
    const isCookieExist = getCookie("adlCookies");
    const setIsCookie = (flag) => {
        if (window.innerWidth >= 992) {
            if (document.getElementById("cookiesModal")) {
                if (flag) {
                    jQuery("#cookiesModal").removeClass('d-none');
                } else {
                    jQuery("#cookiesModal").addClass('d-none');
                }
            }
        } else {
            if (document.getElementById("cookies_offcanvas")) {
                var $cookiesOffcanvas = document.getElementById("cookies_offcanvas");
                var cookiesOffcanvas = new bootstrap.Offcanvas($cookiesOffcanvas);
                if (flag) {
                    cookiesOffcanvas.show();
                } else {
                    cookiesOffcanvas.hide();
                }
            }
        }
    }
     
        if (isCookieExist) {
            setIsCookie(false);
        } else {
            setIsCookie(true);
        }

function closeCookie() {
        cookieHandler();
        setIsCookie(false);
    }
  
  
  $(function() { 
	$('.team-slide-wrapper').slick({
        dots: false,
        centerMode: true,
        centerPadding: '250px',
        slidesToShow: 1,
        responsive: [
          {
            breakpoint: 992,
            settings: {
              centerPadding: '100px'
            }
          },
          {
            breakpoint: 767,
            settings: {
              centerPadding: '70px'
            }
          },
          {
            breakpoint: 500,
            settings: {
              centerPadding: '30px'
            }
          }
        ]
      });     
     });


if($('.advHome-banner .container .advHome-banner-img').length > 0) {
  $(window).on('scroll', function() {
    if ($(this).scrollTop() > $('.advHome-banner .container .advHome-banner-img').offset().top) {
        $('.advHome-banner .container .advHome-banner-img').addClass('arrow-hide');
    } else {
        $('.advHome-banner .container .advHome-banner-img').removeClass('arrow-hide');
    }
  });
}


$(function() { 
    AOS.init({
      easing: 'ease-out-back',
	once: true,
      duration: 500
    });

    var value1 = window.location.href.substring(window.location.href.lastIndexOf('/') + 1); 
    $('.custom-header .row .menu-section ul li a').each(function(){ 
        var url = $(this).attr('href'); 
        var lastSegment = url.split('/').pop(); 
        if (lastSegment == value1) { 
            $(this).parent().addClass('active'); 
        } 
    }); 
    $('.cstm-team-det').closest('body').addClass('cstm-team-page');
    $('.cstm-portfolio-det').closest('body').addClass('cstm-portfolio-page');

$(window).scroll(function() {
            if($(this).scrollTop() > 1) {
                $('body').addClass("scroll-body");
            } else {
                $('body').removeClass("scroll-body");
            }
        });
        $('.footer').after('<a id="back-to-top" href="#top"><svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 24 24" width="24"><path d="M0 0h24v24H0z" fill="none"/><path d="M12 8l-6 6 1.41 1.41L12 10.83l4.59 4.58L18 14z"/></svg></a>');
        $(window).scroll(function(){
        if ($(window).scrollTop()>100){
        $("#back-to-top").addClass('active');
        }
        else
        {
            $("#back-to-top").removeClass('active');
        }
        });
        //back to top
        $("#back-to-top").click(function(){
        $('body,html').animate({scrollTop:0},200);
        return false;
        });
    $("#btnjoinsocialcommunity").click(function () {
        //$(this).attr
        sendjoinsocialcommunity();
    });

    function sendjoinsocialcommunity() {
        var icount = 0;

        if ($("#txtemail").val().trim() == "") {
            icount++;
            $('#txtemail').addClass('has-error');
            $("#lblemail").html("* Please enter your email id");
            $("#lblemail").show();
        } else {
            var e = IsValidEmail($("#txtemail").val());
            if (!e) {
                icount++;
                $('#txtemail').addClass('has-error');
                $("#lblemail").html("* Please enter a valid email address");
                $("#lblemail").show();
            } else {
                $('#txtemail').removeClass('has-error');
                $("#lblemail").hide();
            }
        }

        if (icount > 0) {
            return false;
        } else {

            var obj = {
                'email': $("#txtemail").val().trim()
            };

            var contactvalidationlist = [];



            var objemail = {
                'txtFieldName': 'txtemail',
                'txtFieldValue': $("#txtemail").val().trim(),
                'lblErrorField': 'lblemail',
                'lblErrorFieldMessage': '* Please enter your email id'
            }

            contactvalidationlist.push(objemail);


            var objJsonResult = {
                'IsSuccess': false,
                'IsValid': true,
                'IsError': false,
                'ErrorMessage': '',
                'ErrorSource': '',
                'contactvalidationlist': contactvalidationlist,
                'objContactUs': obj
            }
            console.log("object" + objJsonResult);
            //   $("#contactLoader").show();
            $.ajax({
                url: "/api/SocialSubscribe/JoinSocialSubscribe",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(objJsonResult),
                type: "POST",
                cache: false,
                success: function (response) {

                    console.log(response.data)
                    clearvalidationmessage();
                    //  $("#contactLoader").hide();
                    var data = response;

                    if (data.IsSuccess) {
                        if (data.IsValid) {

                            $("#txtemail").val('');
                            $("#messageShort").show();
                          
                            setTimeout(function () {
                                $("#messageShort").hide();
				$("#messageShort").removeClass('active');	
                            }, 4000);
                            //$("#messageSuccess").html("Thank you for contacting us.");
                           // $("#messageShort").html("We have received your enquiry and shall get back to you shortly.");
 $("#messageShort").addClass('active');
                            // $('#sentFeedbackSuccess').modal('show');
                        }
                        else {
                            if (!data.IsValid) {
                                $.each(data.contactvalidationlist, function (index, object) {
                                    if (object.lblErrorField != "") {
                                        $('#' + object.txtFieldName).addClass('has-error');
                                        $("#" + object.lblErrorField).show();
                                        $("#" + object.lblErrorField).html(object.lblErrorFieldMessage);

                                    }
                                });
                            }
                        }
                    }
                    else if (data.IsError) {
                        alert("Please try aftersome time, we are facing issue. " + response.ErrorMessage);

                    }
                },
                error: function (response) {
                    // $("#contactLoader").hide();
                    $("#txtemail").val('');
                    $("#messageSuccess").html("Error submitting the form");
                    $("#messageShort").html("");
                }
            });
        }
    }

    function clearvalidationmessage() {
        $('#txtemail').removeClass('has-error');
        $("#lblemail").hide();
    }

        function IsValidEmail(email) {
        var spliter = [];
        if (email.toString().indexOf('@') >= 0) {
            spliter = email.toString().split("@");
            if (spliter.length > 2) {
                return false;
            } else {
                if (spliter[0].toString() == "")
                    return false;
                if (spliter[1].toString().indexOf('.') >= 0) {
                    spliter = spliter[1].toString().split('.');
                    if (spliter.length == 1)
                        return false;
                    else if (spliter.length > 2)
                        return false;
                    else {
                        if (spliter[1].toString() == "")
                            return false;
                        else
                            return true;
                    }
                } else
                    return false;
            }
        } else
            return false;
    }

});

$(function() {
    $('.bookmark').click(function() {
    if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'') && location.hostname == this.hostname) {
    var target = $(this.hash);
    target = target.length ? target : $('[name=' + this.hash.slice(1) +']');
    if (target.length) {
    $('html,body').animate({
    scrollTop: target.offset().top - 96
    }, 1000);
    return false;
    }
    }
    });
});
$(document).ready(function(){
  $('.adv-team-details .team-popup .modal-dialogs .close-btn').click(function() {
      window.history.back();
  });

$('.advHome-banner-img').click(function() {
    var nextSection_top = $(this).closest('.advHome-banner').next().offset().top;
    
    var headerHeight;
    if (window.innerWidth >= 992) {
        // Desk
        headerHeight = 96;
    } else {
        // Mob
        headerHeight = 72;
    }
    
    var finalScroll = nextSection_top - headerHeight;
    $('html, body').animate({scrollTop: finalScroll}, 500);
});

});


$(document).ready(function() {
    if($(this).scrollTop() > 1) {
        $('body').addClass("scroll-body");
    } else {
        $('body').removeClass("scroll-body");
    }

    
     
    if (document.getElementById('cookies_offcanvas')) {
        var myOffcanvas = document.getElementById('cookies_offcanvas')
        myOffcanvas.addEventListener('hidden.bs.offcanvas', function () {
            cookieHandler();
        })
    }
    

    
});

