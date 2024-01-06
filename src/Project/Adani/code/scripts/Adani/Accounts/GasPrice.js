$(document).ready(function () {

    $("#ResidentialCity").change(function () {
        var selectedCity = $(this).val().toLowerCase();
        $(".residential").hide();
        if (selectedCity) {
            $(".btm-position.residential." + selectedCity + "").show();
        }
    });

    $("#CommercialCity").change(function () {
        var selectedCity = $(this).val().toLowerCase();
        $(".commercial").hide();
        if (selectedCity) {
            $(".btm-position.commercial." + selectedCity + "").show();
        }
    });

    $("#IndustrialCity").change(function () {
        var selectedCity = $(this).val().toLowerCase();
        $(".industrial").hide();
        if (selectedCity) {
            $(".btm-position.industrial." + selectedCity + "").show();
        }
    });

    $("#CngCity").change(function () {
        var selectedCity = $(this).val().toLowerCase();
        $(".detail").hide();
        if (selectedCity) {
            $(".detail." + selectedCity + "").show();
        }
    });

    // $('#a_redirectToCngStation').on('click', function (e) {
        // e.preventDefault();
        // $("#frmCNGGasPrice").submit();

    // });

    var selectedCity = $("#city").val().toLowerCase();
    $(".citydiv").hide();
    $("#" + selectedCity).show();

    $('#city').change(function () {
        var selectedCity = $(this).val().toLowerCase();
        $(".citydiv").hide();
        $("#" + selectedCity).show();
    });

});