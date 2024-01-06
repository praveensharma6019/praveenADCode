$(document).ready(function () {
    $('.multiselectallow').multiselect({

        includeSelectAllOption: true

    });

    $('.business_type').on('change', function () {
        var currentElement = $(this).is(":checked");
        var business_type = $(this).attr('data-ref');
        if (currentElement) {
            $("#" + business_type).show();
        }
        else {
            $($('#' + business_type).find('.multiselectallow')).multiselect("deselectAll", false).multiselect("refresh");
            $("#" + business_type).hide();
        }
    });
});
$(document).ready(function (initialize) {

    $(".business_type").each(function () {
        var currentElement = $(this).is(":checked");
        var business_type = $(this).attr('data-ref');
        if (currentElement) {
            $("#" + business_type).show();
        }
        else {
            $("#" + business_type).hide();
        }
    });
});
$('selector').on('blur', function (e) {
    var v = this.value;
});

function drpValidate(ele) {
    var v = ele.value;
}
//$('#SubmitRentalForm').on('click', function (e) {
//    $('#formRealtyRental')
//});

