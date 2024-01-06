$(document).ready(function () {
    $("#drpstate").change(function () {
        var targetDrp = '#drpgeo';
        if ($(this).val()) {
            $('' + targetDrp + ' option').remove();
            $.ajax({
                type: 'GET',
                url: '/api/AdaniGas/CngDodoCity',
                data: { "state": $(this).val() },
                async:false,
                success: function (response) {
                    $.each(response, function (index, item) {
                        $('' + targetDrp + '').append('<option value="' + item.Value + '">' + item.Text + '</option>');
                    });
                },
                error: function (response, success, error) {
                    alert("Error: " + error);
                }
            });
        }
        else {
            $('' + targetDrp + ' option').remove();
            $('' + targetDrp + '').append('<option value="">select</option>');
        }
    });
});


