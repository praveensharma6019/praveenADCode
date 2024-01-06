$(document).ready(function () {
    $("#a_reload").on('click', function () {
        window.location.href = window.location.href;
    });
	fnBindAreaOfCity($("#city").val(), '#Area');
});
function fnBindAreaOfCity(selectedval, targetDrp) {
    if (selectedval > 0) {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">select</option>');
        $.getJSON('/api/AdaniGas/GetAreaByCity', { cityCode: selectedval }, function (response) {
            $.each(response, function (index, item) {
                $('' + targetDrp + '').append('<option value="' + item.Value + '">' + item.Text + '</option>');
            });
        });
    }
    else {
        $('' + targetDrp + ' option').remove();
        $('' + targetDrp + '').append('<option value="">select</option>');
    }
};


function fnBindNetworkList(selectedval, patnertype) {

    $.getJSON('/api/AdaniGas/pngNetworklist', { group: selectedval, patnertype: patnertype }, function (response) {
        var tempdata = "";
		var cnt = 0;
		$.each(response, function (index, item) {
			
			if(item.length > 0)
			{
				tempdata = item
			}
			else
			{
				tempdata = "No Record Found."
			}
            cnt = cnt + 1;
        });
		$('#divnetworkdetails').html(tempdata);
		$("#divnetworkdetails div").removeClass("d-block");
    });

};

$("#myInput").on("keyup", function() {
    
    var value = $(this).val().toLowerCase();
    $("#divnetworkdetails *").filter(function() {
      $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
  });
  