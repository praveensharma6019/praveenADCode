$(document).ready(function () {
    //------------Scam Alert Toast Message----------------

    if ($('#modal-scam-alert').length > 0) {
        setTimeout(
            function () {
                var elemFraudAlert = document.getElementById('modal-scam-alert');
                var instanceFraudAlert = M.Modal.init(elemFraudAlert, { dismissible: false });
                instanceFraudAlert.open();
            }, 1500);
    }

    //------------Scam Alert Toast Message----------------


    //----------------------Convert Url to Lower case---------------------

    //commenting this cause Added rule for the same
    //convertURLToLowerCase();

    //function convertURLToLowerCase() {
    //    var oldUrl = window.location.href;
    //    var newUrl = oldUrl.toLowerCase();
    //    window.history.replaceState({}, document.title, newUrl);
    //}

    //--------------------------------------------------------------------


    //-------------------Change Payment Canonical URL---------------------------
    changePaymentCanonicalURL();

    function changePaymentCanonicalURL() {
        var currentUrl = location.pathname;
        currentUrl = currentUrl.split('/');
        currentUrl = currentUrl[currentUrl.length - 1].toLowerCase();
        if (currentUrl == 'pay-your-bill') {
            $('link').each(function () {
                if ($(this).attr('rel') == 'canonical') {
                    $(this).attr('href', 'https://' + $(location).attr('host') + '/pay-your-bill');
                    //<link rel="canonical" href="https://www.adanielectricity.com/pay-your-bill">
                }
            })
        }
    }
    //---------------------------------------------------------------------------


    if (window.innerWidth < 992) {
        $('.mobile-none').hide();
        //$('.desktop-none').show();
    }
    else {
        $('.desktop-none').hide();
        // $('.mobile-none').hide();
    }


    //Checking if any validation msg is coming on input field-------starts----------
    if ($('.input-field').length > 0) {
        if ($('p.field-validation-error').length > 0) {
            $('p.field-validation-error').each(function () {
                if ($(this).css('display') != 'none' && $(this).html() != '' && !($(this).hasClass('otp-black-text'))) {
                    $(this).closest('.input-field').find('input').addClass('invalid');
                }

                //For Login Page 
                if ($('#LoginFormField').length > 0) {
                    //debugger;
                    var errorMsg = $('#LoginFormField').find('p.field-validation-error').html();
                    if (errorMsg.indexOf('or') > 0) {
                        $('#LoginUserName').addClass('invalid');
                    }
                }
            })
        } else {
            $('.input-field input').each(function () {
                $(this).removeClass('invalid')
            })
        }
    }

    $('.input-field input').click(function () {
        $(this).removeClass('invalid')
        if ($(this).siblings('p.field-validation-error').length > 0) {
            // $(this).siblings('p.field-validation-error').hide();
        }
    })

    window.getEncriptedKey = getEncriptedKey

    function getEncriptedKey(stringToEncrypt) {
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
        var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(stringToEncrypt), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            }).toString();
        return encrypted;
    }

    //Checking if any validation msg is coming on input field-------Ends-----------

    //Google captch Callback function --------starts---------------

    function recaptchaCallback() {
        $('.captcha_box').find('p.field-validation-error').hide();
    };

    //Google captch Callback function --------starts---------------

    //------Field Type text & number Starts-----------

    $(".input-field").find('input[type=text], input[type=number]').each(function () {
        if ($(this).val() != '' && !($(this).is('[readonly]')) && !($(this).is('[disabled=""]'))) {

            if (!($(this).closest('.input-field').find('.input-icon').length > 0) && !($(this).closest('.input-field').hasClass('input-has-icon'))) {
                $(this).closest('.input-field').addClass('input-has-icon');
                $(`<div class="input-icon">
                <span class="icon-holder waves-effect" style="display: none;">
                    <i onclick="commonCrossIcon(this);" class="i-cross"></i>
                </span>
               </div>`).appendTo($(this).closest('.input-field'));
            }

        }
    })

    $(".input-field").find('input[type=text], input[type=number]').keydown(function () {

        //logic for only number in Input type number field
        if ($(this).attr('type') == "number") {
            if (!($.isNumeric($(this).val()))) {
                $(this).val('');
            }
        }

        //Checking if input field has any existing other icon
        if (!($(this).closest('.input-field').find('.input-icon').length > 0)) {
            if ($(this).val() != '') {
                if (!($(this).closest('.input-field').hasClass('input-has-icon')) && !($(this).hasClass('quantity')) && !($(this).hasClass('ameldisabled'))) {
                    $(this).closest('.input-field').addClass('input-has-icon');
                    $(`<div class="input-icon">
                <span class="icon-holder waves-effect">
                    <i onclick="commonCrossIcon(this);" class="i-cross"></i>
                </span>
               </div>`).appendTo($(this).closest('.input-field'));
                }
            } else {
                // alert(1);
                // $(this).closest('.input-field').find('.input-icon .i-cross').remove();
                if ($(this).closest('.input-field').hasClass('input-has-icon')) {
                    $(this).closest('.input-field').removeClass('input-has-icon');
                    $(this).closest('.input-field').find('.input-icon').remove();
                }

                if ($(this).closest('.input-field').find('input').hasClass('invalid')) {
                    $(this).closest('.input-field').find('input').removeClass('invalid');
                }

                if ($(this).closest('.input-field').find('p.field-validation-error').length > 0) {
                    //$(this).closest('.input-field').find('p.field-validation-error').hide();
                }
                if ($(this).closest('.input-field').siblings('p.field-validation-error').length > 0) {
                    // $(this).closest('.input-field').siblings('p.field-validation-error').hide();
                }
            }
        }
    });


    $(".input-field").find('input[type=text], input[type=number]').on('blur input', function () {

        if (!($(this).closest('.input-field').find('.input-icon').length > 0)) {

            if ($(this).val() != '') {
                if (!($(this).closest('.input-field').hasClass('input-has-icon')) && !($(this).hasClass('quantity')) && !($(this).hasClass('ameldisabled'))) {
                    $(this).closest('.input-field').addClass('input-has-icon');
                    $(`<div class="input-icon">
                <span class="icon-holder waves-effect">
                    <i onclick="commonCrossIcon(this);" class="i-cross"></i>
                </span>
               </div>`).appendTo($(this).closest('.input-field'));
                }
            } else {
                if ($(this).closest('.input-field').hasClass('input-has-icon')) {
                    $(this).closest('.input-field').removeClass('input-has-icon');
                    $(this).closest('.input-field').find('.input-icon').remove();
                }

                if ($(this).closest('.input-field').find('input').hasClass('invalid')) {
                    $(this).closest('.input-field').find('input').removeClass('invalid');
                }

                if ($(this).closest('.input-field').find('p.field-validation-error').length > 0) {
                    // $(this).closest('.input-field').find('p.field-validation-error').hide();
                }
                if ($(this).closest('.input-field').siblings('p.field-validation-error').length > 0) {
                    // $(this).closest('.input-field').siblings('p.field-validation-error').hide();
                }
            }

        }

    });

    function commonCrossIcon(el) {
        $(el).closest('.input-field').find('input').val('');
        $(el).closest('.input-field').find('input').focus();
        $(el).closest('.input-field').find('label').addClass('active');

        if ($(el).closest('.input-field').hasClass('input-has-icon')) {
            $(el).closest('.input-field').removeClass('input-has-icon');
            //$(el).closest('.input-field').find('label').removeClass('active');
            $(el).closest('.input-field').find('.input-icon').remove();
        }

        if ($(el).closest('.input-field').find('input').hasClass('invalid')) {
            $(el).closest('.input-field').find('input').removeClass('invalid');
        }

        if ($(el).closest('.input-field').find('p.field-validation-error').length > 0) {
            $(el).closest('.input-field').find('p.field-validation-error').hide();
        }
        if ($(el).closest('.input-field').siblings('p.field-validation-error').length > 0) {
            $(el).closest('.input-field').siblings('p.field-validation-error').hide();
        }

    }
    window.commonCrossIcon = commonCrossIcon

    $(".input-field").find('input[type=text], input[type=number]').focusin(function () {
        if ($(this).val() != '' && !($(this).is('[disabled=""]')) && !($(this).is('[readonly]')) && !($(this).hasClass('ameldisabled'))) {
            $(this).closest('.input-field').find('.i-cross').parent().show();
        }
    });

    $(".input-field").find('input[type=text], input[type=number]').focusout(function () {
        var This = $(this);
        window.setTimeout(function () {
            $(This).closest('.input-field').find('.i-cross').parent().hide();
        }, 200);
    });

    //------Field Type text & number Ends-----------

    //------Field Type Password Starts-----------

    $(".input-field").find('input[type=password]').focusin(function () {
        if ($(this).val() != '') {
            $(this).closest('.input-field').find('.i-cross').parent().show();
        }
    });

    $(".input-field").find('input[type=password]').focusout(function () {
        var This = $(this);
        window.setTimeout(function () {
            $(This).closest('.input-field').find('.i-cross').parent().hide();
        }, 200);
    });

    $(".input-field").find('input[type=password]').on('blur input', function () {
        if ($(this).val() != '') {
            if (!($(this).closest('.input-field').find('.input-icon .i-cross').length > 0)) {
                $(`<span class="icon-holder cross-icon-holder waves-effect">
                    <i onclick="commonCrossIconPasswordField(this);" class="i-cross"></i>
                </span>`).insertBefore($(this).closest('.input-field').find('.input-icon .show-pass'));
            }
        } else {
            $(this).closest('.input-field').find('.input-icon .i-cross').remove();

            if ($(this).closest('.input-field').find('input').hasClass('invalid')) {
                $(this).closest('.input-field').find('input').removeClass('invalid');
            }

            if ($(this).closest('.input-field').find('p.field-validation-error').length > 0) {
                // $(this).closest('.input-field').find('p.field-validation-error').hide();
            }
            if ($(this).closest('.input-field').siblings('p.field-validation-error').length > 0) {
                //$(this).closest('.input-field').siblings('p.field-validation-error').hide();
            }
        }
    });

    $(".input-field").find('input[type=password]').keydown(function () {
        if ($(this).val() != '') {
            if (!($(this).closest('.input-field').find('.cross-icon-holder').length >= 0)) {
                $(`<span class="icon-holder cross-icon-holder waves-effect">
                    <i onclick="commonCrossIconPasswordField(this);" class="i-cross"></i>
                </span>`).insertBefore($(this).closest('.input-field').find('.input-icon .show-pass'));
            }
        } else {
            $(this).closest('.input-field').find('.cross-icon-holder').remove();

            //if ($(this).closest('.input-field').hasClass('input-has-icon')) {
            //    $(this).closest('.input-field').removeClass('input-has-icon');
            //    $(this).closest('.input-field').find('.input-icon').remove();
            //}

            if ($(this).closest('.input-field').find('input').hasClass('invalid')) {
                $(this).closest('.input-field').find('input').removeClass('invalid');
            }

            if ($(this).closest('.input-field').find('p.field-validation-error').length > 0) {
                //$(this).closest('.input-field').find('p.field-validation-error').hide();
            }
            if ($(this).closest('.input-field').siblings('p.field-validation-error').length > 0) {
                //$(this).closest('.input-field').siblings('p.field-validation-error').hide();
            }
        }
    });


    $(".input-field").find('input[type=password]').each(function () {
        if ($(this).val() != '') {
            $(`<span class="icon-holder cross-icon-holder waves-effect" style="display: none;">
                    <i onclick="commonCrossIconPasswordField(this);" class="i-cross"></i>
                </span>`).insertBefore($(this).closest('.input-field').find('.input-icon .show-pass'));
        }
    });

    function commonCrossIconPasswordField(el) {

        $(el).closest('.input-field').find('input').val('');
        $(el).closest('.input-field').find('input').focus();
        $(el).closest('.input-field').find('label').addClass('active');

        //$(el).closest('.input-field').find('label').removeClass('active');
        $(el).closest('.input-field').find('.cross-icon-holder').remove();

        if ($(el).closest('.input-field').find('input').hasClass('invalid')) {
            $(el).closest('.input-field').find('input').removeClass('invalid');
        }

        if ($(el).closest('.input-field').find('p.field-validation-error').length > 0) {
            $(el).closest('.input-field').find('p.field-validation-error').hide();
        }
        if ($(el).closest('.input-field').siblings('p.field-validation-error').length > 0) {
            $(el).closest('.input-field').siblings('p.field-validation-error').hide();
        }
    }
    window.commonCrossIconPasswordField = commonCrossIconPasswordField


    //------Field Type Password Ends-----------


    //------------Main Navigation starts-------------

    ActiveMainNavigationParent();

    function ActiveMainNavigationParent() {
        var currentUrl = location.pathname;
        if (currentUrl.indexOf('/') >= 0) {
            currentUrl = currentUrl.split('/');
            currentUrl = currentUrl[currentUrl.length - 1].toLowerCase();
        } else {
            currentUrl = currentUrl.toLowerCase();
        }
        $('#headerPrimaryMenuL ul li a').each(function () {
            var elementUrl = $(this).attr('href');
            if (elementUrl != undefined && elementUrl.indexOf('/') >= 0) {
                elementUrl = elementUrl.split('/');
                elementUrl = elementUrl[elementUrl.length - 1].toLowerCase();
            } else if (elementUrl != undefined) {
                elementUrl = elementUrl.toLowerCase();
            }
            if (elementUrl != undefined) {
                if (elementUrl === currentUrl) {
                    $(this).closest('li.0active').addClass('active-page');
                    return false; //breaks
                }
            }
        })
    }

    //------------Main Navigation Ends---------------
})


function IsSuccess() {
    jQuery.ajax(
        {
            url: apiSettings + "/AccountsRevamp/Benow_Callback",
            method: "POST",
            success: function (data) {
                if (data !== "") {
                    myStopFunction();
                    location.href = data;
                }
            }
        });
}

function IsSuccessCity() {
    jQuery.ajax(
        {
            url: apiSettings + "/AccountsRevamp/City_Callback_Check",
            method: "POST",
            success: function (data) {
                if (data !== "") {
                    myStopFunction();
                    location.href = data;
                }
            }
        });
}

function IsSuccessDBS() {
    jQuery.ajax(
        {
            url: apiSettings + "/AccountsRevamp/IsSuccessDBS_Callback",
            method: "POST",
            success: function (data) {
                if (data !== "") {
                    myStopFunction();
                    location.href = data;
                }
            }
        });
}

setTimeout(myStopFunction, 300000);

function myStopFunction() {
    clearInterval(myVar);
}

//----------------------- Sitecore form field---------------------------

var formEleArray = [
    '.sitecore-form-field input[type="text"]:not([type="hidden"])',
    '.sitecore-form-field input[type="email"]',
    '.sitecore-form-field input[type="number"]',
    '.sitecore-form-field input[type="tel"]',
    '.sitecore-form-field textarea',
    '.sitecore-form-field input[type="date"]'
];
var formEle = formEleArray.join();

$(formEle).on('blur input', function () {
    if (!($(this).closest('.sitecore-form-field').find('.input-icon').length > 0)) {
        if ($(this).val() != '') {
            if (!($(this).closest('.sitecore-form-field').hasClass('input-has-icon')) && !($(this).hasClass('quantity'))) {
                $(this).closest('.sitecore-form-field').addClass('input-has-icon');
                $(`<div class="input-icon">
                <span class="icon-holder waves-effect">
                    <i onclick="commonSFCrossIcon(this);" class="i-cross"></i>
                </span>
               </div>`).appendTo($(this).closest('.sitecore-form-field'));
            }
        } else {
            if ($(this).closest('.sitecore-form-field').hasClass('input-has-icon')) {
                $(this).closest('.sitecore-form-field').removeClass('input-has-icon');
                $(this).closest('.sitecore-form-field').find('.input-icon').remove();
            }
        }
    }
});

$(formEle).keydown(function () {
    //logic for only number in Input type number field
    if ($(this).attr('type') == "number") {
        if (!($.isNumeric($(this).val()))) {
            $(this).val('');
        }
    }
    //Checking if input field has any existing other icon
    if (!($(this).closest('.sitecore-form-field').find('.input-icon').length > 0)) {
        if ($(this).val() != '') {
            if (!($(this).closest('.sitecore-form-field').hasClass('input-has-icon')) && !($(this).hasClass('quantity'))) {
                $(this).closest('.sitecore-form-field').addClass('input-has-icon');
                $(`<div class="input-icon">
                <span class="icon-holder waves-effect">
                    <i onclick="commonSFCrossIcon(this);" class="i-cross"></i>
                </span>
               </div>`).appendTo($(this).closest('.input-field'));
            }
        } else {
            if ($(this).closest('.sitecore-form-field').hasClass('input-has-icon')) {
                $(this).closest('.sitecore-form-field').removeClass('input-has-icon');
                $(this).closest('.sitecore-form-field').find('.input-icon').remove();
            }
        }
    }
});

$(formEle).focusout(function () {
    var This = $(this);
    window.setTimeout(function () {
        $(This).closest('.sitecore-form-field').find('.i-cross').parent().hide();
    }, 200);
});

$(formEle).focusin(function () {
    if ($(this).val() != '') {
        $(this).closest('.sitecore-form-field').find('.i-cross').parent().show();
    }
});

function commonSFCrossIcon(el) {

    $(el).closest('.sitecore-form-field').find('input').val('');
    $(el).closest('.sitecore-form-field').find('input').focus();
    $(el).closest('.sitecore-form-field').find('label').addClass('active');


    $(el).closest('.sitecore-form-field').find('textarea').val('');
    $(el).closest('.sitecore-form-field').find('textarea').focus();
    $(el).closest('.sitecore-form-field').find('label').addClass('active');

    if ($(el).closest('.sitecore-form-field').hasClass('input-has-icon')) {
        $(el).closest('.sitecore-form-field').removeClass('input-has-icon');
        $(el).closest('.sitecore-form-field').find('.input-icon').remove();
    }
}
window.commonSFCrossIcon = commonSFCrossIcon




//--------------------------------------------------------------------------------------------