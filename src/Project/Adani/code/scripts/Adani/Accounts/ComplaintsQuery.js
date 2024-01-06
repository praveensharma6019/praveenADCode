
$(document).ready(function () {
    getComplaintsType();
	$('#category').append('<option value="">Select</option>');
    $('#submitform').click(function () {
        if (($('#type').val() > 0) && ($("#comment").val() !== "")) {
            return true;
        }
        else {
            if ($('#type').val() <= 0) {
                alert('Please Select Type Complaint/Inquiry');
                return false;
            }
            else if (!$('#category').val()) {
                alert('Please Select category');
                return false;
            }
            else if ($("#comment").val() == "") {
                alert('Please fill additional comment field');
                return false;
            }

        }
    });




    $('#type').change(function () {
        if (!$('#type').val()) {
            $('#category option').remove();
            $('#category').append('<option value="">Select</option>');
        }
        else {
            $('#category option').remove();
            $.getJSON('/api/AdaniGas/GetComplaintsQuery_Category', { Complaintscategoryvalue: $('#type').val() }, function (data) {
                $.each(data, function (index, item) {
                    $('#category').append('<option value="' + item.Value + '">' + item.Text + '</option>');
                });


            });
        }
    });

});





function getComplaintsType() {
    $.ajax(
        {
            type: "post",
            dataType: 'JSON',
            url: '/api/AdaniGas/GetComplaintsQuery_Type',
            success:
                function (response) {
                    if (response.length > 0) {
                        $('#type').html('');
                        var options = '';
                        for (var i = 0; i < response.length; i++) {
                            options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                        }
                        $('#type').append(options);
                    }
                }
        });
}