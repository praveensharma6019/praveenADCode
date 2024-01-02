
function resize() {
	$('#slider').height($('#slider').children('.aslide').height());
}
$(window).resize(resize);
$(window).load(resize);


// Nav Scroll

// $(document).ready(function() {
//   $('.nav').onePageNav({
//     filter: ':not(.external)',
//     begin: function() {
//       console.log('start')
//     },
//     end: function() {
//       console.log('stop')
//     }
//   });

// });

$('a[href*="#"]')
  // Remove links that don't actually link to anything
  .not('[href="#"]')
  .not('[href="#0"]')
  .not('[href="#map-img"]')
  .not('[href="#googlemap"]')
  .not('.carousel-control')
  .click(function(event) {
    // On-page links
    if (
      location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') 
      && 
      location.hostname == this.hostname
    ) {
      // Figure out element to scroll to
      var target = $(this.hash);
      target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
      // Does a scroll target exist?
      if (target.length) {
        // Only prevent default if animation is actually gonna happen
        event.preventDefault();
        $('html, body').animate({
          scrollTop: target.offset().top - 60
        }, 1000, function() {
          // Callback after animation
          // Must change focus!
          var $target = $(target);
          $target.focus();
          if ($target.is(":focus")) { // Checking if the target was focused
            return false;
          } else {
            $target.attr('tabindex',''); // Adding tabindex for elements not focusable
            $target.focus(); // Set focus again
          };
        });
      }
    }
  });

// jQuery.validator.addMethod("mobile", function(value, element) {
        // return this.optional(element) || value.match(/^[0-9]{10}$/);
    // }, "Enter 10 digit number");

  // jQuery.validator.addMethod("alphabets", function(value, element) {
    // return this.optional(element) || /^[a-zA-Z ]*$/.test(value);
// }, "Please enter Alphabets only");

  // jQuery.validator.addMethod("email", function(value, element) {
    // return this.optional(element) || /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(value);
// }, "Please enter a valid email address.");
    // jQuery.validator.addMethod("captcha", function(value, element) {
        // return this.optional(element) || value.match($('#txtCaptcha').val());
    // }, "Please enter a valid code");

     // jQuery.validator.addMethod("captcha", function(value, element) {
        // return this.optional(element) || value.match($('#txtCaptcha1').val());
    // }, "Please enter a valid code");

      // jQuery.validator.addMethod("captcha", function(value, element) {
        // return this.optional(element) || value.match($('#txtCaptcha2').val());
    // }, "Please enter a valid code");

       // jQuery.validator.addMethod("captcha", function(value, element) {
        // return this.optional(element) || value.match($('#txtCaptcha3').val());
    // }, "Please enter a valid code");
     
     // jQuery.validator.addMethod("captcha", function(value, element) {
        // return this.optional(element) || value.match($('#txtCaptcha4').val());
    // }, "Please enter a valid code");


  var priceValidate;
  var instantValidate;

  // if ($('#ContactForm').length > 0) {
    // $('#ContactForm').validate({        
      // rules: {
        // name: {
          // required: true,
          // alphabets: true,
          // maxlength: 100
        // },
        // CountryCode: {
          // required: true
        // },
        // mobile: {
          // required: true,
          // number: true,
          // mobile: true
        // /*  minlength: 10,
          // maxlength: 10*/
        // },
        // email: {
          // required: true,
          // email: true
        // },
        // comment: {
          // required: true,
        // }
      // },
      // submitHandler: function(form) {
        // $(form).find(':submit').prop('disabled', true);
        // form.submit();
      // }
    // });
  // }



  // if ($('#PopupForm').length > 0) {
    // $('#PopupForm').validate({
      // rules: {
        // name: {
          // required: true,
          // alphabets: true,
          // maxlength: 100
        // },
        // CountryCode: {
          // required: true
        // },
        // mobile: {
          // required: true,
          // number: true,
          // mobile: true
         // /* minlength: 10,
          // maxlength: 10*/
        // },
        // captchatxt1: {
                // required: true,
                // captcha:true
            // },
        // email: {
          // required: true,
          // email: true
        // },
            // /*comment: {
                // required: true,
              // }*/
            // },
            // submitHandler: function(form) {
              // $(form).find(':submit').prop('disabled', true);
              // form.submit();
            // }
          // });
  // }


  // if ($('#pricepopup').length > 0) {
    // priceValidate = $('#pricepopup').validate({
      // rules: {
        // name: {
          // required: true,
          // alphabets: true,
          // maxlength: 100
        // },
        // CountryCode: {
          // required: true
        // },
        // mobile: {
          // required: true,
          // number: true,
          // mobile: true
        // /*  minlength: 10,
          // maxlength: 10*/
        // },
         // captchatxt3: {
                // required: true,
                // captcha:true
            // },

        // email: {
          // required: true,
          // email: true
        // },
            // /*comment: {
                // required: true,
              // }*/
            // },
            // submitHandler: function(form) {
              // $(form).find(':submit').prop('disabled', true);
              // form.submit();
            // }
          // });
  // }

  // if ($('#InstantCallback').length > 0) {
   // instantValidate= $('#InstantCallback').validate({
    // rules: {
      // name: {
        // required: true,
        // alphabets: true,
        // maxlength: 100
      // },
      // CountryCode: {
        // required: true
      // },
      // mobile: {
        // required: true,
        // number: true,
        // mobile: true
     // /*   minlength: 10,
        // maxlength: 10*/
      // },
      // captchatxt: {
                // required: true,
                // captcha:true
            // },
      // email: {
        // required: true,
        // email: true
      // },
            // /*comment: {
                // required: true,
              // }*/
            // },
            // submitHandler: function(form) {
              // $(form).find(':submit').prop('disabled', true);
              // form.submit();
            // }
          // });
 // }

 // if ($('#inquiryForm').length > 0) {
  // $('#inquiryForm').validate({
    // rules: {
      // name: {
        // required: true,
        // alphabets: true,
        // maxlength: 100
      // },
      // CountryCode: {
        // required: true
      // },
      // mobile: {
        // required: true,
        // number: true,
        // mobile: true
     // /*   minlength: 10,
        // maxlength: 10*/
      // } ,
     // captchatxt2: {
                // required: true,
                // captcha:true
            // },
      // email: {
        // required: true,
        // email: true
      // }

    // },
    // submitHandler: function(form) {
      // $(form).find(':submit').prop('disabled', true);
      // form.submit();
    // }
  // });
// }

 // if ($('#sitevisit').length > 0) {
  // $('#sitevisit').validate({
    // rules: {
      // name: {
        // required: true,
        // alphabets: true,
        // maxlength: 100
      // },
      // CountryCode: {
        // required: true
      // },
      // mobile: {
        // required: true,
        // number: true,
        // mobile: true
       // /* minlength: 10,
        // maxlength: 10*/
      // } ,
      // email: {
        // required: true,
        // email: true
      // }

    // },
    // submitHandler: function(form) {
      // $(form).find(':submit').prop('disabled', true);
      // form.submit();
    // }
  // });
// }



// if ($('#speake_to_expert').length > 0) {
  // $('#speake_to_expert').validate({
    // rules: {
      // mobile: {
        // required: true,
        // number: true,
        // mobile: true
      // /*  minlength: 10,
        // maxlength: 10*/
      // },
      // CountryCode: {
        // required: true
      // }
    // },
    // submitHandler: function(form) {
      // $(form).find(':submit').prop('disabled', true);
      // form.submit();
    // }
  // });
// }

/*popup js starts here*/
$(window).load(function() {
  if (!Get_Cookie('popout')) {

    if ($(window).width() > 550) {
      window.setTimeout(function() {
        $('#popupModal').modal({
                    /*backdrop: 'static',
                    keyboard: false*/
                  });
      }, 3000);
    }
  }
});

$('#popupModal .close').click(function() {
  Set_Cookie('popout', 'it works');
});
$('#popupModal').on('hide.bs.modal',function(){
  Set_Cookie('popout', 'it works');
});

$('.pricepop').click(function() {
  var pricePopup = $('#price');
  pricePopup.find('input[name=source]').val('Price Popup');
  pricePopup.find('strong').html('Please enter the details below to get the detailed pricing information.');
  pricePopup.modal();
  $('#price').on('hidden.bs.modal',function(){        
    $(this).find('.has-error').removeClass('has-error');
    priceValidate.resetForm();
  });
});

$('#bookvisit').click(function() {
  var pricePopup = $('#sitevisit');
   pricePopup.modal();
  $('#sitevisit').on('hidden.bs.modal',function(){        
    $(this).find('.has-error').removeClass('has-error');
    priceValidate.resetForm();
  });
});

$('.inquireButton').click(function(){
  var inquirePopup = $('#price');
  inquirePopup.find('input[name=source]').val('Inquiry Form - Mobile');
  inquirePopup.find('strong').html('Enter your details for project information.');
  inquirePopup.modal();
});

/*popup js ends here*/

jQuery(function($) {
  $(document).ready(function() {
    "use strict";
    var instantFlag = false;
    var hotlineFlag = false;
    $("#instant-callback-div .instant-switch").click(function() {
      $('#instant-callback-div').addClass('opened');
      $("#instant-callback-div").animate({
        "right": $("#instant-callback-div").css('right') == "-1px" ? "-247px" : "-1px"
      }, 500);
      instantFlag = true;
      if (hotlineFlag) {
        $("#hotline-div").animate({
          "right": "-277px"
        }, 500);
        hotlineFlag = false;
      }
    });
    $("#hide").click(function() {
       $('#instant-callback-div').removeClass('opened');
      $("#instant-callback-div").animate({
        "right": "-247px"
      }, 500);
      instantFlag = false;
      $('#InstantCallback').find('.has-error').removeClass('has-error');
      instantValidate.resetForm();
    });

    $("#hotline-div .hotline-switch").click(function() {
      $("#hotline-div").animate({
        "right": "-1px"
      }, 500);
      hotlineFlag = true;
      if (instantFlag) {
        $("#instant-callback-div").animate({
          "right": "-246px"
        }, 500);
        instantFlag = false;
      }
    });
    $("#hide-hotline").click(function() {
      $("#hotline-div").animate({
        "right": "-245px"
      }, 500);
      hotlineFlag = false;
    });
  });
});

