$(document).ready(function () {
	try {
	var selectedValue = $('.multiselectallow').val();
	if( selectedValue != undefined)
	{
		  $('.multiselectallow').multiselect({

			includeSelectAllOption: true

			});

    $('.business_type').on('change', function () 
	{
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
	}
}
  catch(e) {
        // handle an exception here if lettering doesn't exist or throws an exception
    }
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
$('selector').on('blur', function (e){
	var v=this.value;
});

function drpValidate(ele)
{
	var v=ele.value;
}
