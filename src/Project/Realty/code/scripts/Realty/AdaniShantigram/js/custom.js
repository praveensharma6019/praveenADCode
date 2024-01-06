
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
  .not('[href="#1"]')
  .not('[href="#2"]')
  .not('[href="#3"]')
  .not('[href="#4"]')

  // .not('([data-toggle="tab"])')
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
          scrollTop: target.offset().top - 90
        }, 1000, function() {
          // Callback after animation
          // Must change focus!
          var $target = $(target);
          $target.focus();
          if ($target.is(":focus")) { // Checking if the target was focused
            return false;
          } else {
            $target.attr('tabindex','-1'); // Adding tabindex for elements not focusable
            $target.focus(); // Set focus again
          };
        });
      }
    }
  });

$('.enquire').click(function() {
    var pricePopup = $('#enquire');
    // pricePopup.find('input[name=source]').val('Price Popup');
       pricePopup.modal();
    $('#price').on('hidden.bs.modal',function(){        
        $(this).find('.has-error').removeClass('has-error');
        priceValidate.resetForm();
    });
});

$('.enquire1').click(function() {
    var pricePopup = $('#enquire1');
    // pricePopup.find('input[name=source]').val('Price Popup');
       pricePopup.modal();
    $('#price').on('hidden.bs.modal',function(){        
        $(this).find('.has-error').removeClass('has-error');
        priceValidate.resetForm();
    });
});

$('.enquire2').click(function() {
    var pricePopup = $('#enquire2');
    // pricePopup.find('input[name=source]').val('Price Popup');
       pricePopup.modal();
    $('#price').on('hidden.bs.modal',function(){        
        $(this).find('.has-error').removeClass('has-error');
        priceValidate.resetForm();
    });
});

function projectname(project) {
    var projname = project;
    //Set_Cookie('projnameCookie', projname, '', '/', '', '');
    $("input[value = 'Enquire Now Popup']").val(projname + " " + "Enquire Now Popup");
    $('#enquiresource').remove();
    $('.enquirebtn').before('<input type="hidden" name="source" value="' + projname + " " + 'Enquire Now Popup" />');
    $('#popProj').html(projname);
    
}

function aqaprojname(p) {
  var projnameaqua = p;
    $("input[value = 'proj']").val(projnameaqua);

}

/*
function selectprj(d) {
  var selectedprj = d;
    $("select[value = 'prj']").val(selectedprj);

}
*/

function projectname2(project) {
    var projname = project;
    //Set_Cookie('projnameCookie', projname, '', '/', '', '');
    $("input[value = 'Price Popup']").val(projname + " " + "Price popup");
    $('#pricesource').remove();
    $('.pricebtn').before('<input type="hidden" name="source" value="' + projname + " " + 'price popup" />');
    // $('#popProj').html(projname);
    
}

// Form Validation

 jQuery.validator.addMethod("mobile", function(value, element) {
        return this.optional(element) || value.match(/^[1-9][0-9]*$/);
    }, "Please enter the number without beginning with '0'");

    jQuery.validator.addMethod("alphabets", function(value, element) {
        return this.optional(element) || /^[a-zA-Z ]*$/.test(value);
    }, "Please enter Alphabets only");

    jQuery.validator.addMethod("email", function(value, element) {
        return this.optional(element) || /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/.test(value);
    }, "Please enter a valid email address.");

var priceValidate;
var instantValidate;

if ($('#ContactForm').length > 0) {
  $('#ContactForm').validate({        
    rules: {
      name: {
        required: true,
        alphabets: true,
        maxlength: 100
      },
      CountryCode: {
        required: true
      },
      mobile: {
        required: true,
        number: true,
        mobile: true
      /*  minlength: 10,
        maxlength: 10*/
      },
      email: {
        required: true,
        email: true
      },
      location: {
        required: true,
      },
      comment: {
        required: true,
      }
    },
    submitHandler: function(form) {
      $(form).find(':submit').prop('disabled', true);
      form.submit();
    }
  });
}

if ($('#enquirenowform').length > 0) {
  $('#enquirenowform').validate({
    rules: {
      name: {
        required: true,
        alphabets: true,
        maxlength: 100
      },
      CountryCode: {
        required: true
      },
      mobile: {
        required: true,
        number: true,
        mobile: true
       /* minlength: 10,
        maxlength: 10*/
      },
      email: {
        required: true,
        email: true
      },
      location: {
        required: true,
      },
      comment: {
        required: true,
      }
    },
    submitHandler: function(form) {
      $(form).find(':submit').prop('disabled', true);
      form.submit();
    }
  });
}

if ($('#PopupForm').length > 0) {
  $('#PopupForm').validate({
    rules: {
      name: {
        required: true,
        alphabets: true,
        maxlength: 100
      },
      CountryCode: {
        required: true
      },
      mobile: {
        required: true,
        number: true,
        mobile: true
      /*  minlength: 10,
        maxlength: 10*/
      },
      location: {
        required: true,
      },
      email: {
        required: true,
        email: true
      },
            /*comment: {
                required: true,
              }*/
            },
            submitHandler: function(form) {
              $(form).find(':submit').prop('disabled', true);
              form.submit();
            }
          });
}


if ($('#pricepopup').length > 0) {
  priceValidate = $('#pricepopup').validate({
    rules: {
      name: {
        required: true,
        alphabets: true,
        maxlength: 100
      },
      CountryCode: {
        required: true
      },
      mobile: {
        required: true,
        number: true,
        mobile: true
      /*  minlength: 10,
        maxlength: 10*/
      },
      email: {
        required: true,
        email: true
      },
      location: {
        required: true,
      },
            /*comment: {
                required: true,
              }*/
            },
            submitHandler: function(form) {
              $(form).find(':submit').prop('disabled', true);
              form.submit();
            }
          });
}

if ($('#InstantCallback').length > 0) {
 instantValidate= $('#InstantCallback').validate({
  rules: {
    name: {
      required: true,
      alphabets: true,
      maxlength: 100
    },
    CountryCode: {
      required: true
    },
    mobile: {
      required: true,
      number: true,
      mobile: true
    /*  minlength: 10,
      maxlength: 10*/
    },
    location: {
        required: true,
    },
    email: {
      required: true,
      email: true
    },
            /*comment: {
                required: true,
              }*/
            },
            submitHandler: function(form) {
              $(form).find(':submit').prop('disabled', true);
              form.submit();
            }
          });
}



if ($('#inquiryForm').length > 0) {
    $('#inquiryForm').validate({
        rules: {
            name: {
                required: true,
                alphabets: true,
                maxlength: 100
            },
            CountryCode: {
                required: true
            },
            mobile: {
                required: true,
                number: true,
                mobile: true,
              /* minlength: 10,
               maxlength: 10*/
            } ,
            email: {
                required: true,
                email: true
            },
            location: {
          required: true,
            },

        },
        submitHandler: function(form) {
            $(form).find(':submit').prop('disabled', true);
            form.submit();
        }
    });
}


if ($('#sitevisitform').length > 0) {
  $('#sitevisitform').validate({
    rules: {
      name: {
        required: true,
        alphabets: true,
        maxlength: 100
      },
      mobile: {
        required: true,
        number: true,
        mobile: true
      /*  minlength: 10,
        maxlength: 10*/
      } ,
      email: {
        required: true,
        email: true
      },
      projectname: {
        required: true,
      },

    },
    submitHandler: function(form) {
      $(form).find(':submit').prop('disabled', true);
      form.submit();
    }
  });
}



if ($('#speake_to_expert').length > 0) {
  $('#speake_to_expert').validate({
    rules: {
      mobile: {
        required: true,
        number: true,
        mobile: true,
        minlength: 10,
        maxlength: 10
      },
      CountryCode: {
        required: true
      }
    },
    submitHandler: function(form) {
      $(form).find(':submit').prop('disabled', true);
      form.submit();
    }
  });
}

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
  pricePopup.find('strong').html('Please enter the details');
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

