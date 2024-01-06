
$(document).ready(function () {
    $('#frm1').css('display', 'block');
    //$('#frm1').css('display', 'block');
    $('#fram1').css("border-bottom", "3px solid #007ea8");
    $('#fram1').css("border-bottom-width: medium", "30%");
    $("#fram2").click(function () {

        $('#frm2').css('display', 'block');
        $('#frm1').css('display', 'none');
        $('#fram1').css("border-bottom", "");
    });
    $("#fram1").click(function () {

        $('#frm1').css('display', 'block');
        $('#frm2').css('display', 'none');
        $('#fram2').css("border-bottom", "");

        //  $('.consumer-number').css("border-bottom", "3px solid #007ea8");

        // $('#fram2').css("border-bottom", "none");
    });

    $('.consumer-number').click(function () {
        $('#fram1').css("border-bottom", "3px solid #007ea8");
    });

    $('.serial-number').click(function () {
        $('#fram2').css("border-bottom", "3px solid #007ea8");
    });


});

$(function () {
    $('.blocking-span input').on('focus', function () {
        $(this).parents('.parents-elm').addClass('foucs-content'); // When focus the input area
    });
    $(document).mouseup(function (e) {
        //var inputValue = $('.consumer-number').val();
        if ($(e.target).parents('.blocking-span input').length == 0 && !$(e.target).is('.blocking-span input')) {
            if ($('.consumer-number').val() == '') {
            //var inputValue = $('.consumer-number').val();
            // console.log(inputValue);

            $('.parents-elm').removeClass('foucs-content');
        }
        }
    });
});

//$(document).ready(function () {
//    $('#mybtnmodal').click(function () {

//        $('#mymodal').modal('show')
//    });
//});

//$(document).ready(function () {
//    $('#MybtnModal-tab2').click(function () {

  //      $('#Mymodal-tab2').modal('show')
//    });
//});

$(document).ready(function () {
    $("#filter").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

// International telephone format
// $("#phone").intlTelInput();
// get the country data from the plugin
var countryData = window.intlTelInputGlobals.getCountryData(),
    input = document.querySelector("#fxb_3d39bc0e-acd7-4914-a076-05e30425192e_Fields_9e2d6f45-40ef-459f-8590-18b13e76ee6c__Value"),
    addressDropdown = document.querySelector("#address-country");
console.log(input)

// init plugin
var iti = window.intlTelInput(input, {
    hiddenInput: "full_phone",
    preferredCountries: ["in"],
    utilsScript: "https://intl-tel-input.com/node_modules/intl-tel-input/build/js/utils.js?1549804213570" // just for formatting/placeholders etc
});


// populate the country dropdown
for (var i = 0; i < countryData.length; i++) {
    var country = countryData[i];
    var optionNode = document.createElement("option");
    optionNode.value = country.iso2;
    var textNode = document.createTextNode(country.name);
    optionNode.appendChild(textNode);
    addressDropdown.appendChild(optionNode);
}
// set it's initial value
addressDropdown.value = iti.getSelectedCountryData().iso2;
