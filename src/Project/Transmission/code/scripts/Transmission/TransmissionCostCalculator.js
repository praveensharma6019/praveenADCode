/*!
 * jQuery Cookie Plugin v1.4.1
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2013 Klaus Hartl
 * Released under the MIT license
 */
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // CommonJS
        factory(require('jquery'));
    } else {
        // Browser globals
        factory(jQuery);
    }
}(function ($) {

    var pluses = /\+/g;

    function encode(s) {
        return config.raw ? s : encodeURIComponent(s);
    }

    function decode(s) {
        return config.raw ? s : decodeURIComponent(s);
    }

    function stringifyCookieValue(value) {
        return encode(config.json ? JSON.stringify(value) : String(value));
    }

    function parseCookieValue(s) {
        if (s.indexOf('"') === 0) {
            // This is a quoted cookie as according to RFC2068, unescape...
            s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
        }

        try {
            // Replace server-side written pluses with spaces.
            // If we can't decode the cookie, ignore it, it's unusable.
            // If we can't parse the cookie, ignore it, it's unusable.
            s = decodeURIComponent(s.replace(pluses, ' '));
            return config.json ? JSON.parse(s) : s;
        } catch (e) { }
    }

    function read(s, converter) {
        var value = config.raw ? s : parseCookieValue(s);
        return $.isFunction(converter) ? converter(value) : value;
    }

    var config = $.cookie = function (key, value, options) {

        // Write

        if (value !== undefined && !$.isFunction(value)) {
            options = $.extend({}, config.defaults, options);

            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setTime(+t + days * 864e+5);
            }

            return (document.cookie = [
                encode(key), '=', stringifyCookieValue(value),
                options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
                options.path ? '; path=' + options.path : '',
                options.domain ? '; domain=' + options.domain : '',
                options.secure ? '; secure' : ''
            ].join(''));
        }

        // Read

        var result = key ? undefined : {};

        // To prevent the for loop in the first place assign an empty array
        // in case there are no cookies at all. Also prevents odd result when
        // calling $.cookie().
        var cookies = document.cookie ? document.cookie.split('; ') : [];

        for (var i = 0, l = cookies.length; i < l; i++) {
            var parts = cookies[i].split('=');
            var name = decode(parts.shift());
            var cookie = parts.join('=');

            if (key && key === name) {
                // If second argument (value) is a function it's a converter...
                result = read(cookie, value);
                break;
            }

            // Prevent storing a cookie that we couldn't decode.
            if (!key && (cookie = read(cookie)) !== undefined) {
                result[name] = cookie;
            }
        }

        return result;
    };

    config.defaults = {};

    $.removeCookie = function (key, options) {
        if ($.cookie(key) === undefined) {
            return false;
        }

        // Must not alter options, thus extending a fresh object...
        $.cookie(key, '', $.extend({}, options, { expires: -1 }));
        return !$.cookie(key);
    };

}));
var coookieValueCustom = "";
$("#btnEdit").click(function () {
    //sessionStorage.setItem("EditTransmissionCostCalculator", 1);
    //$.removeCookie("EditTransmissionCostCalculator");
    //$.cookie("EditTransmissionCostCalculator", 1);
    //coookieValueCustom = $.cookie("EditTransmissionCostCalculator");
    setCookie("EditTransmissionCostCalculator", 1, 365);
    coookieValueCustom = getCookie("EditTransmissionCostCalculator");
    window.location.reload();
});

// for auto off
$("input").each(function () {
    var inputIds = $(this).attr("id");
    $("#" + inputIds).attr("autocomplete", "off");
});
function setCookie(cname, cvalue, exdays) {
    const d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    let expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}

function getCookie(cname) {
    let name = cname + "=";
    let ca = document.cookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

var recaptcha1;
var onloadCallback = function () {


    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha1', {
        'sitekey': '6LdkC64UAAAAALc0FqqrHPlTizoP8x8WT7VoH_sI', //Replace this with your Site key
        'theme': 'light'
    });


};

var x = 2;
$(".add_ownership_document").click(function (e) {
    if (x == 20) {
        $(".add_ownership_document").hide();
    }
    else {
        $(".add_ownership_document").show();
    }
    $("#TR_OWNERDOC_" + x).show();
    x++;
});
var z = 2;
$(".add_document").click(function (e) {
    if (z == 3) {
        $(".add_document").hide();
    }
    else {
        $(".add_document").show();
    }
    $("#adddocument_" + z).show();
    z++;
});


$(document).ready(function () {
    if (window.location.pathname == "/Carbon-Footprint-Calculator/History") {
        //$.cookie("EditTransmissionCostCalculator", 22);
        setCookie("EditTransmissionCostCalculator", 22, 365);
    }
    //$('#CalculateOffsetValue').hide();
    if (window.location.search.includes("RegistrationNo")) {
        $("#calculateOffset").click();
    } else {
        $('#CalculateOffsetValue').hide();
    }

    $('#Message').hide();

    $("#myTable").on('input', '.txtCal', function () {
        var current = 1;
        var ids = 1;
        var totalConsumption = 0;
        var totals = 0;
        var totalsum = 0;
        var sum = 0;
        $("#myTable .txtCal").each(function () {
            var get_textbox_value = $(this).val();
            var month = $('#MonthName').val();

            var year = $('#year').val();
            var wattage = $('#ConsumedElectricity_' + current).val();
            var getDaysInMonth = function (month, year) {
                return new Date(year, month + 1, 0).getDate();
            };
            var totalMonths = getDaysInMonth(month, year);
            if ($.isNumeric(get_textbox_value)) {
                totalConsumption = (parseFloat(wattage) * parseFloat(get_textbox_value) * totalMonths * 0.82) / 1000;
                totals = totalConsumption.toFixed(2);
                document.getElementById('d_consume_elec_' + current).value = totals;
            }
            current++;
            ids++;
        });
        $("#myTable .consumer").each(function () {
            var d_get_textbox_value = $(this).val();

            if ($.isNumeric(d_get_textbox_value)) {
                sum += parseFloat(d_get_textbox_value);
                totalsum = sum.toFixed(2);
            }


        });

        document.getElementById("ElectricityConsumedatResidence").value = totalsum;
    });


    $("#myTable").on('input', '.numbertrips', function () {
        var currents = 1;
        var totalConsumptionsss = 0;
        var totalConsumptions = 0;
        var totalsums = 0;
        var sums = 0;
        $("#myTable .numbertrips").each(function () {
            var get_textbox_values = $(this).val();
            var trip = $('#Trips_' + currents).val();
            if ($.isNumeric(get_textbox_values)) {
                totalConsumptions = (parseFloat(trip) * parseFloat(get_textbox_values) * 0.121);
                totalConsumptionsss = totalConsumptions.toFixed(2);
                document.getElementById('d_numbertrips_' + currents).value = totalConsumptionsss;
            }
            currents++;

        });
        $("#myTable .numbertrip").each(function () {
            var d_get_textbox_values = $(this).val();
            if ($.isNumeric(d_get_textbox_values)) {
                sums += parseFloat(d_get_textbox_values);
                totalsums = sums.toFixed(2);

            }


        });
        document.getElementById('TotalAir').value = totalsums;
    });

    $('#DiselPetrolCons').on("keyup change", function (e) {
        var currents = 1;
        var totalConsumptionsss = 0;
        var totalConsumptions = 0;
        var get_textbox_values = Number(document.getElementById("txt_1").value) + Number(document.getElementById("txt_2").value);
        document.getElementById('diselpetrol_' + currents).value = get_textbox_values;
        var cons = $(this).val();
        if ($.isNumeric(get_textbox_values)) {
            totalConsumptions = (parseFloat(cons) * parseFloat(get_textbox_values) * 15);
            totalConsumptionsss = totalConsumptions.toFixed(2);
            document.getElementById('d_diselpetrol_' + currents).value = totalConsumptionsss;
        }
        currents++;
    });


    $('#CNGBusCons').on("keyup change", function (e) {
        var currents = 1;
        var totalConsumptionsss = 0;
        var totalConsumptions = 0;
        var get_textbox_values = Number(document.getElementById("txt_6").value);
        document.getElementById('CNGBus_' + currents).value = get_textbox_values;
        var cons = $(this).val();
        if ($.isNumeric(get_textbox_values)) {
            totalConsumptions = (parseFloat(cons) * parseFloat(get_textbox_values));
            totalConsumptionsss = totalConsumptions.toFixed(2);
            document.getElementById('d_CNGBus_' + currents).value = totalConsumptionsss;
        }
        currents++;
    });
});


function calculate_CarbonOffset(i) {
    var cal_factor = new Array();
    cal_factor[0] = "";
    cal_factor[111] = "0.227011";//short haul
    cal_factor[222] = "0.137156";//medium haul
    cal_factor[333] = "0.167421";//long haul
    cal_factor[444] = "16.8783333333333";
    cal_factor[555] = "13.53";
    cal_factor[666] = "13.776";
    cal_factor[777] = "0.220416";
    cal_factor[888] = "1.81";
    cal_factor[8888] = "1.81";
    cal_factor[999] = "";
    var sum = 0;
    var totalsum = 0;
    if (i > 0) {

        if (document.getElementById('txt_' + i).value.search(/\S/) != -1) {
            if (isNaN(document.getElementById('txt_' + i).value) == false) {
                if (document.getElementById('txt_' + i).value >= 0) {
                    document.getElementById('d_txt_' + i).value = (document.getElementById('txt_' + i).value * cal_factor[i]).toFixed(2);
                    if (i == 999) {
                        document.getElementById('d_txt_' + i).value = Number(document.getElementById('txt_' + i).value) / 1000 * 1.81;
                    }
                }

                else {
                    document.getElementById('d_txt_' + i).value = '';
                }
            }
            else {
                document.getElementById('d_txt_' + i).value = '';
            }
        }
        else {
            document.getElementById('d_txt_' + i).value = '';
        }

    }
    $("#offset_table .offset").each(function () {
        var d_get_textbox_value = $(this).val();

        if ($.isNumeric(d_get_textbox_value)) {
            sum += parseFloat(d_get_textbox_value);
            totalsum = sum.toFixed(2);
        }
    });
    document.getElementById("TotalOffsetCarbonEmission").value = totalsum;
    var TotalFamilyMember = Number(document.getElementById("FamilyMembers").value);
    document.getElementById("AverageCarbonOffset").value = (totalsum / TotalFamilyMember).toFixed(2);
    var reducedCarbonoffsetFootprint = (totalsum / Number(document.getElementById("TotalOffsetCarbonEmission").value) * 12) / 1000;
    document.getElementById("ReducedAnnualffsetFootprint").value = reducedCarbonoffsetFootprint;
}

$(".table .btn_abcd").click(function () {
    //added instead of the function--> for the class "add_ownership_document"
    $(".table .abcd_add1:first").show();
    $(".table .abcd_add1:first").attr("class", "abcd_add");
});
$(".table .btn_secondary").click(function () {
    //added instead of the function--> for the class "add_document"
    $(".table .abcd_add_second1:first").show();
    $(".table .abcd_add_second1:first").attr("class", "abcd_add_second");
});

function ValidateMonth(month, year) {
    //var status = "0";
    var savecustomdata = { Month: month, Year: year };
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Transmission/ValidateMonth",
        contentType: "application/json",
        success: function (data) {
            //alert(data.status);
            if (data.status == '1') {
                //ConfirmDialog('Are you sure you want to edit the higher values for ' + month + '?', month);
                ConfirmDialog('Selected Month-Year record is already available, Kindly Edit same record with higher value.', month
                );
            }
        }
    });
}

function ConfirmDialog(message, month) {


    if (confirm(message)) {
        // Save it!
        console.log('Thing was saved to the database.');
        window.location.href = "/Carbon-Footprint-Calculator/History";
    } else {
        // Do nothing!
        alert("The Record already exists against " + month);
        $("#btnsubmit").hide();
        $('#btnSaveTarget').hide();
        $('#hello').hide();
        $('#CalculatedValue').hide();
        $('#btnreset').hide();
        $('#Message').show();
    }
};

$("#MonthName").change(function () {
    var value = $("#MonthName option:selected");
    // alert(value.text());
    var CalculatorMonthName = value.text();
    var year = $('#year').val();
    var currentDate = new Date();
    var currentMonth = currentDate.getMonth() + 1;
    var currentYear = currentDate.getFullYear();
    var selectedmonth = parseInt($('#MonthName').val()) + 1;
    if (year == currentYear) {
        if (selectedmonth > currentMonth) {
            if (confirm("Future Date Not Allowed")) {
                window.location.reload();
            }
            else {
                window.location.href = "/Carbon-Footprint-Calculator/History";
            }
        }
    }
    ValidateMonth(CalculatorMonthName, year);
});
$("#year").change(function () {
    var value = $("#MonthName option:selected");
    // alert(value.text());
    var CalculatorMonthName = value.text();
    var year = $('#year').val();
    var currentDate = new Date();
    var currentMonth = currentDate.getMonth() + 1;
    var currentYear = currentDate.getFullYear();
    var selectedmonth = parseInt($('#MonthName').val()) + 1;
    if (year == currentYear) {
        if (selectedmonth > currentMonth) {
            if (confirm("Future Date Not Allowed")) {
                window.location.reload();
            }
            else {
                window.location.href = "/Carbon-Footprint-Calculator/History";
            }
        }
    }
    ValidateMonth(CalculatorMonthName, year);
});


$(document).ready(function () {
    $('button[data-confirm]').click(function (ev) {
        $('#modal').find('.modal-title').text($(this).attr('data-confirm'));
        $('#modal').modal({ show: true });
        return false;
    });

});
var response = true;

//$("#btnsubmit").click(function () {
$('#modal-btn-yes').click(function () {

    var confirm = $(this).attr('value');
    if (confirm == 'false') {
        return false;
    }
    else {
        //window.close();

        $('#modal, .modal-backdrop').hide();
        $('#btnsubmit').attr("disabled", "disabled");
        var MonthNaming = document.getElementById("MonthName");
        var month = MonthNaming.options[MonthNaming.selectedIndex].text;
        var year = $('#year').val();
        var ElectricityConsumption = Number(document.getElementById("d_txt_11").value);
        var ElectricityConsumptionVal = Number(document.getElementById("txt_11").value);
        var ElectricityAppliance = 0.00;
        var ApplianceValue = 0.00;
        var DropdownAppliancesConsumption = [];
        var DropdownAppliancesConsumptionVal = [];
        var DropdownAppliancesList = [];
        for (var j = 1; j <= 20; j++) {
            var CaltextField = "d_consume_elec_" + j;
            var ConsumptionValtextField = "consume_elec_" + j;
            var ApplianceValuetextField = "ConsumedElectricity_" + j;
            ApplianceValue = document.getElementById(CaltextField) !== null ? document.getElementById(CaltextField).value : "";
            ConsumptionValue = document.getElementById(ConsumptionValtextField) !== null ? document.getElementById(ConsumptionValtextField).value : "";

            ApplianceListValue = document.getElementById(ApplianceValuetextField) !== null ? document.getElementById(ApplianceValuetextField).value : "";
            if (ApplianceValue !== '' && ApplianceValue !== null) {
                DropdownAppliancesConsumption.push(ApplianceValue);
                DropdownAppliancesConsumptionVal.push(ConsumptionValue);
                DropdownAppliancesList.push(ApplianceListValue);
            }
            ElectricityAppliance = Number(ElectricityAppliance) + Number(ApplianceValue);

        }
        var DropdownAppliancesConsumptionStr = DropdownAppliancesConsumption.join(",");
        var DropdownAppliancesConsumptionValStr = DropdownAppliancesConsumptionVal.join(",");
        var DropdownAppliancesListStr = DropdownAppliancesList.join(",");

        var ElectricityConsumedatResidence = ElectricityConsumption + ElectricityAppliance;
        ElectricityConsumedatResidence = parseFloat(ElectricityConsumedatResidence).toFixed(2);
        var CNGUsed = Number(document.getElementById("d_txt_9").value);
        var CNGUsedValue = Number(document.getElementById("txt_9").value);
        var LPGCylinder = Number(document.getElementById("d_txt_8").value);
        var LPGCylinderVal = Number(document.getElementById("txt_8").value);



        //var TotalDomesticUsed = Number(ElectricityConsumedatResidence) + CNGUsed + LPGCylinder;
        var TotalDomesticUsed = (Number(ElectricityConsumedatResidence) + CNGUsed + LPGCylinder) / 1000;


        //document.getElementById('EmissionfromDomesticUse').innerHTML = parseFloat(TotalDomesticUsed).toFixed(2) + ' ' + 'KG' 
        document.getElementById('EmissionfromDomesticUse').innerHTML = parseFloat(TotalDomesticUsed).toFixed(2) + ' ' + 'Tonnes'

        var DieselConsumption = Number(document.getElementById("d_txt_2").value);
        var DieselConsumptionValue = Number(document.getElementById("txt_2").value);
        var PetrolConsumption = Number(document.getElementById("d_txt_1").value);
        var PetrolConsumptionValue = Number(document.getElementById("txt_1").value);
        var AutoRikshaw = Number(document.getElementById("d_txt_6").value);
        var AutoRikshawValue = Number(document.getElementById("txt_6").value);
        var Bus = Number(document.getElementById("d_txt_5").value);
        var BusValue = Number(document.getElementById("txt_5").value);
        var Train = Number(document.getElementById("d_txt_7").value);
        var TrainValue = Number(document.getElementById("txt_7").value);
        var regno = document.getElementById("reg-number-hidden") ? Number(document.getElementById("reg-number-hidden").value) : "";
        if (regno !== "") {
            //document.getElementById("CalculateOffsetValue").disabled = true;
            $('#CalculateOffsetValue').hide();
        }
        //var TotalTransportationUsed = DieselConsumption + PetrolConsumption + AutoRikshaw + Bus + Train;
        var TotalTransportationUsed = (DieselConsumption + PetrolConsumption + AutoRikshaw + Bus + Train) / 1000;
        var NumberOfTrips = 0.00;
        var Tripss = 0.00;
        var TripsConsumption = [];
        var TripsConsumptionVal = [];
        var TripsDropdownList = [];
        for (var z = 1; z <= 3; z++) {
            var TripsDropdownTextField = "Trips_" + z;
            var TripsValtextField = "numbertrips_" + z;
            var TripsField = "d_numbertrips_" + z;
            TripsValue = document.getElementById(TripsValtextField) !== null ? document.getElementById(TripsValtextField).value : "";
            Tripss = document.getElementById(TripsField) !== null ? document.getElementById(TripsField).value : "";
            TripsListValue = document.getElementById(TripsDropdownTextField) !== null ? document.getElementById(TripsDropdownTextField).value : "";
            if (Tripss !== '' && Tripss !== null) {
                TripsConsumption.push(Tripss);
                TripsConsumptionVal.push(TripsValue);
                TripsDropdownList.push(TripsListValue);
            }
            NumberOfTrips = Number(NumberOfTrips) + Number(Tripss);

        }
        var TripsValueStr = TripsConsumptionVal.join(",");
        var TripsStr = TripsConsumption.join(",");
        var TripsListValueStr = TripsDropdownList.join(",");
        var totalTrips1 = (Number(document.getElementById("TotalAir").value)) / 1000;
        var TotalTransportationUse = TotalTransportationUsed.toFixed(2);
        //document.getElementById('EmissionfromTransportation').innerHTML = TotalTransportationUse + ' ' + 'KG'
        document.getElementById('EmissionfromTransportation').innerHTML = TotalTransportationUse + ' ' + 'Tonnes'
        var TotalCarbonEmission = (TotalDomesticUsed + TotalTransportationUsed + totalTrips1).toFixed(2);
        document.getElementById('TotalCarbonEmission').innerHTML = TotalCarbonEmission + ' ' + 'Tonnes'
        var TotalFamilyMember = Number(document.getElementById("FamilyMembers").value);
        if (TotalFamilyMember != "") {

            var EmployeeTotalemissionsperMonthss = (TotalDomesticUsed + TotalTransportationUsed + totalTrips1) / TotalFamilyMember;

        }

        else {
            EmployeeTotalemissionsperMonthss = TotalTransportationUsed;
        }

        var EmployeeTotalemissionsperMonth = EmployeeTotalemissionsperMonthss.toFixed(2);
        //display the result
        //document.getElementById('TotalemissionsperMonth').innerHTML = EmployeeTotalemissionsperMonth + ' ' + 'KG'
        document.getElementById('TotalemissionsperMonth').innerHTML = EmployeeTotalemissionsperMonth + ' ' + 'Tonnes'
        //var AverageAnnualCarbonFootprinted = EmployeeTotalemissionsperMonthss * 12 / 1000;
        var AverageAnnualCarbonFootprinted = EmployeeTotalemissionsperMonthss * 12;
        var CalculatorMonthNaming = document.getElementById("MonthName");
        var CalculatorMonthName = CalculatorMonthNaming.options[CalculatorMonthNaming.selectedIndex].text;

        var CalculatorYear = $("#year").val();
        var AnnualCarbonFootprint = AverageAnnualCarbonFootprinted.toFixed(2);
        //display the result    
        document.getElementById('EmployeeAverageAnnualCarbonFootprint').innerHTML = AnnualCarbonFootprint + '  ' + 'Tonnes'

        document.getElementById('AverageAnnualCarbonFootprintHidden').innerHTML = Number(AnnualCarbonFootprint);

        var HistoryAnnualCarbonFootprint = Number(document.getElementById("HistoryAnnualCarbonFootprint").value);



        var DiffCarbonFootprint = HistoryAnnualCarbonFootprint - AnnualCarbonFootprint;
        var DifferenceCarbonFootprint = DiffCarbonFootprint.toFixed(2);
        if (HistoryAnnualCarbonFootprint !== null || HistoryAnnualCarbonFootprint != 0) {
            //document.getElementById('CarbonFootprintDifference').innerHTML = 'Yours reduced carbon footprint is: ' + DifferenceCarbonFootprint + ' Tonnes'
        }
        if (HistoryAnnualCarbonFootprint == 0) {
            //document.getElementById('CarbonFootprintDifference').innerHTML = ' Yours present carbon footprint:'
        }


        var LandNeed = AnnualCarbonFootprint * 0.04;
        //display the result
        var LandNeededHectares = LandNeed.toFixed(2);
        //document.getElementById('LandNeeded').innerHTML = LandNeededHectares + '  ' + 'hectares'
        var NoTreesNeeded = AnnualCarbonFootprint * 64;
        //display the result
        var TreesNeeded = Math.round(NoTreesNeeded);
        //document.getElementById('NoOfTreesNeeded').innerHTML = TreesNeeded;
        //var totalTrips = Number(document.getElementById("TotalAir").value);
        var totalTrips = (Number(document.getElementById("TotalAir").value)) / 1000;
        //document.getElementById('TotalTrips').innerHTML = totalTrips  + ' ' + 'KG';
        document.getElementById('TotalTrips').innerHTML = totalTrips.toFixed(2) + ' ' + 'Tonnes';
        $('#CalculatedValue').show();
        $('#offset').show();
        $('#footprintTarget').parent().parent().hide();
        $('#emissionReduction').parent().hide();
        var text9 = Number(document.getElementById("txt_9").value);
        if (text9 == "") {
            $("#txt_9").val("0");


        }
        var txt9 = document.getElementById("txt_9");
        txt9.setAttribute("disabled", "disabled");
        var text8 = Number(document.getElementById("txt_8").value);
        if (text8 == "") {
            $("#txt_8").val("0");


        }
        var txt8 = document.getElementById("txt_8");
        txt8.setAttribute("disabled", "disabled");
        var text2 = Number(document.getElementById("txt_2").value);
        if (text2 == "") {
            $("#txt_2").val("0");


        }
        var txt2 = document.getElementById("txt_2");
        txt2.setAttribute("disabled", "disabled");
        var text1 = Number(document.getElementById("txt_1").value);
        if (text1 == "") {
            $("#txt_1").val("0");


        }
        var txt1 = document.getElementById("txt_1");
        txt1.setAttribute("disabled", "disabled");
        var text6 = Number(document.getElementById("txt_6").value);
        if (text6 == "") {
            $("#txt_6").val("0");


        }
        var txt6 = document.getElementById("txt_6");
        txt6.setAttribute("disabled", "disabled");
        var text5 = Number(document.getElementById("txt_5").value);
        if (text5 == "") {
            $("#txt_5").val("0");


        }
        var txt5 = document.getElementById("txt_5");
        txt5.setAttribute("disabled", "disabled");
        var text7 = Number(document.getElementById("txt_7").value);
        if (text7 == "") {
            $("#txt_7").val("0");


        }
        var txt7 = document.getElementById("txt_7");
        txt7.setAttribute("disabled", "disabled");
        var FamilyMemberss = document.getElementById("FamilyMembers");
        FamilyMemberss.setAttribute("disabled", "disabled");
        $('#btnsubmit').hide();
        $('#backButton-primary').hide();
        //var Emissionpercentage = $("#CarbonEmissionReducePercentage").val();
        var Emissionpercentage = Number(document.getElementById('CarbonEmissionReducePercentage').innerText);
        var Emissionyear = $("#CarbonEmissionReviewYear").val();
        var Emissiondate = $("#CarbonEmissionReviewDate").val();
        if ((Emissionpercentage != "") && ((Emissionyear != "") || (Emissiondate != ""))) {

            $('#btnreset').show();
            $('#backButton-secondary').toggleClass("d-none");
        }
        else {
            $('#btnreset').show();
        }
        var HistoryElectricty1 = Number(document.getElementById("HistoryElectricityConsumedatResidences").value);
        var Dielsel1 = Number(document.getElementById("HistoryCNGUsed").value);
        var CNG1 = Number(document.getElementById("HistoryDiesel").value);
        if (HistoryElectricty1 == "") {
            $('#hello').hide();
        }
        else {
            $('#hello').show();

        }
        //var HistoryElectricty = Number(document.getElementById("ElectricityConsumedatResidence").value);
        document.getElementById('Currentelectrcity').innerHTML = ElectricityConsumedatResidence;
        var CurrentLPG_1 = document.getElementById("d_txt_8").value;
        if (CurrentLPG_1 == "") {
            document.getElementById('currentLPG1').innerHTML = "0";
        }
        else {
            document.getElementById('currentLPG1').innerHTML = CurrentLPG_1;
        }

        var CNGHistory = Number(document.getElementById("d_txt_9").value);
        document.getElementById('currentCNG').innerHTML = CNGHistory;
        var CurrentDieselHistory = Number(document.getElementById("d_txt_2").value);
        document.getElementById('CurrentDiesel').innerHTML = CurrentDieselHistory;
        var CurrentPetrolHistory = Number(document.getElementById("d_txt_1").value);
        document.getElementById('CurrentPetrol').innerHTML = CurrentPetrolHistory;
        var CurrentAutoRickshaw = Number(document.getElementById("d_txt_6").value);
        document.getElementById('CurrentAutoRickshaw').innerHTML = CurrentAutoRickshaw;
        var CurrentBusHistory = Number(document.getElementById("d_txt_5").value);
        document.getElementById('CurrentBus').innerHTML = CurrentBusHistory;
        var CurrentTrainHistory = Number(document.getElementById("d_txt_7").value);
        document.getElementById('CurrentTrains').innerHTML = CurrentTrainHistory;
        var CurrentTripHistory = Number(document.getElementById("TotalAir").value);
        document.getElementById('CurrentTrip').innerHTML = CurrentTripHistory;
        if (window.location.search.includes("RegistrationNo")) {
            //document.getElementById('CurrentTrip').innerHTML = $("#TotalAirInViewEdit").attr("name");
            document.getElementById('CurrentTrip').innerHTML = NumberOfTrips.toFixed(2);
            totalTrips = NumberOfTrips;
            var totalTrips2 = NumberOfTrips / 1000;
            document.getElementById('TotalTrips').innerHTML = totalTrips2.toFixed(2) + ' ' + 'Tonnes';
        }
        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
        var formtype = $("#cFormType").val();
        var pageinfo = window.location.href;
        //create json object
        var savecustomdata = {


            ElectricityConsumedatResidences: ElectricityConsumedatResidence,
            ElectricityBillConsumptionVal: ElectricityConsumptionVal,
            TotalFamilyMembers: TotalFamilyMember,
            MonthNames: CalculatorMonthName,
            Years: CalculatorYear,
            CNGUseds: CNGUsed,
            CNGUsedsVal: CNGUsedValue,
            LPGCylinders: LPGCylinder,
            LPGCylindersVal: LPGCylinderVal,
            DieselConsumptions: DieselConsumption,
            DieselConsumptionsVal: DieselConsumptionValue,
            PetrolConsumptions: PetrolConsumption,
            PetrolConsumptionsVal: PetrolConsumptionValue,
            AutoRikshaws: AutoRikshaw,
            AutoRikshawsVal: AutoRikshawValue,
            Buses: Bus,
            BusesVal: BusValue,
            //reResponse: response,
            Trains: Train,
            TrainsVal: TrainValue,
            FormType: formtype,
            PageInfo: pageinfo,
            TotalTrips: totalTrips,
            TotalTransportationUses: TotalTransportationUse,
            //TotalDomesticUses: TotalDomesticUse,
            TotalDomesticUses: TotalDomesticUsed,
            EmployeeTotalemissionsperMonths: EmployeeTotalemissionsperMonth,
            AnnualCarbonFootprints: AnnualCarbonFootprint,
            AverageAnnualCarbonFootprints: '1.94Tonnes',
            NumberOfTreesNeeded: TreesNeeded,
            LandNeeded: LandNeededHectares,
            FormSubmitOn: currentdate,
            RegistrationNumber: regno,
            DropdownAppliancesConsumption: DropdownAppliancesConsumptionStr,
            DropdownAppliancesList: DropdownAppliancesListStr,
            DropdownAppliancesConsumptionVal: DropdownAppliancesConsumptionValStr,
            AirTripsDropdownList: TripsListValueStr,
            AirTripsValue: TripsValueStr,
            AirTrips: TripsStr
        };

    }
    //ajax calling to save  custom data function
    if (regno !== null && regno !== "") {
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Transmission/EditCostCalculator",
            contentType: "application/json",
            success: function (data) {
                if (data.status == "1") {
                }

                else if (data.status == "2") {
                    alert("Please check and validate your form before submitting");
                    $('#btnsubmit').removeAttr("disabled");
                    return false;
                }
                else if (data.status == "3") {
                    $('#btnSaveTarget').hide();
                    $('#hello').hide();
                    $('#CalculatedValue').hide();
                    $('#btnreset').hide();
                    $('#Message').show();
                    $('#Message').html(data.Result + " should be higher than previous " + data.Result);
                    setTimeout(function () {
                        window.location.reload();
                    }, 3000);
                    return false;
                }
                else {
                    alert("Sorry, Operation has Failed!!! Please try again later. Thankyou");
                    $('#btnsubmit').removeAttr("disabled");
                    return false;
                }


            }

        });
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Transmission/InsertCostCalculator",
            contentType: "application/json",
            success: function (data) {
                if (data.status == "1") {
                }

                else if (data.status == "2") {
                    alert("Please check and validate your form before submitting");
                    $('#btnsubmit').removeAttr("disabled");
                    return false;
                    //$('#contact_form1').submit();
                }
                else if (data.status == "3") {
                    // alert("Entry cannot be done against this month. The Record Already Exist.");
                    $('#btnSaveTarget').hide();
                    $('#hello').hide();
                    $('#CalculatedValue').hide();
                    $('#btnreset').hide();
                    $('#Message').show();
                    $('#Message').html(data.Result + " should be higher than previous " + data.Result);
                    setTimeout(function () {
                        window.location.reload();
                    }, 3000);
                    return false;
                    //$('#contact_form1').submit();
                }
                else {
                    alert("Sorry, Operation has Failed!!! Please try again later. Thankyou");
                    $('#btnsubmit').removeAttr("disabled");
                    return false;
                }


            }

        });
        return false;
    }
    //ajax calling to insert  custom data function

});



$(document).ready(function () {
    $("#pills-home").hide();
    $("#myChart").hide();

});

function OpenTips() {
    $("#pills-home").show();
    var pos = $("#pills-home").offset().top - 200;
    $('body, html').animate({ scrollTop: pos });
}
var global = 0;
$('#calculateOffset').click('change', function () {
    var count = 0;
    global++;
    for (var i = 0; i < $('input[type=checkbox]').length; i++) {
        var status = $('input[type=checkbox]')[i].checked;
        if (status) {
            count++;
        }
    }
    if (count == 0 && global == 2) {
        alert("Please checkmark atleast one offset");
        return false;
    }
    $('#CalculateOffsetValue').show();
    $('#footprintTarget').parent().parent().show();
    $('#emissionReduction').parent().show();
    var sum = 0;
    var totalsum = 0;
    $("#offset_table .offset").each(function () {
        var d_get_textbox_value = $(this).val();
        debugger;
        if ($.isNumeric(d_get_textbox_value)) {
            sum += parseFloat(d_get_textbox_value);
            totalsum = sum.toFixed(2);
        }
    });
    debugger;
    var regno = document.getElementById("reg-number-hidden") ? Number(document.getElementById("reg-number-hidden").value) : "";
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    var personalTransport = document.getElementById("d_diselpetrol_1").value;
    var personalTransportDropdownVal = document.getElementById("DiselPetrolCons").value;
    var personalTransportVal = document.getElementById("diselpetrol_1").value;
    var PublicTransport = document.getElementById("d_CNGBus").value;
    var publicTransportVal = document.getElementById("CNGBus").value;
    var OnlineMeeting = document.getElementById("d_OnlineMeeting_Airtrips").value;
    var onlineMeetingVal = document.getElementById("OnlineMeeting_Airtrips").value;
    var FiveStarAppliances = document.getElementById("d_FiveStar_Appliances").value;
    var fiveStarAppliancesVal = document.getElementById("FiveStar_Appliances").value;
    var NoofTrees = document.getElementById("d_txt_888").value;
    var NoofTreesValue = document.getElementById("txt_888").value;
    var NoofPlantationProjectTrees = document.getElementById("d_txt_8888").value;
    var NoofPlantationProjectTreesValue = document.getElementById("txt_8888").value;
    var FundForTrees = document.getElementById("d_txt_999").value;
    var FundForTreesValue = document.getElementById("txt_999").value;
    var offsetDomesticUse = (Number(document.getElementById("d_FiveStar_Appliances").value) / 1000).toFixed(4);
    var OffsetDomesticEmission = offsetDomesticUse.toString();
    document.getElementById("OffsetEmissionfromDomesticUse").innerHTML = offsetDomesticUse + ' ' + 'Tonnes';
    var offsetTransporationUse = ((Number(document.getElementById("d_diselpetrol_1").value) + Number(document.getElementById("d_CNGBus").value)) / 1000).toFixed(4);
    document.getElementById("OffsetEmissionfromTransportation").innerHTML = offsetTransporationUse + ' ' + 'Tonnes';
    var transportoffset = Number(document.getElementById("d_diselpetrol_1").value) + Number(document.getElementById("d_CNGBus").value);
    var OffsetTransportaionEmission = offsetTransporationUse.toString();
    var offsetAirTipUse = (Number(document.getElementById("d_OnlineMeeting_Airtrips").value) / 1000).toFixed(2);
    var offsetAirTripEmission = offsetAirTipUse.toString();
    document.getElementById("OffsetEmissionfromAirTrips").innerHTML = offsetAirTipUse + ' ' + 'Tonnes';
    var TotalOffsetCarbonEmission = (totalsum / 1000).toFixed(2);
    document.getElementById("TotalOffsetCarbonEmission").innerHTML = TotalOffsetCarbonEmission + ' ' + 'Tonnes';
    var TotalFamilyMember = Number(document.getElementById("FamilyMembers").value);
    var AverageCarbonOffset = totalsum / TotalFamilyMember;
    var FinalAverageCarbonOffset = (AverageCarbonOffset / 1000).toFixed(5);
    document.getElementById("AverageCarbonOffset").innerHTML = FinalAverageCarbonOffset + ' ' + 'Tonnes';
    var reducedCarbonoffsetFootprint = (Number(AverageCarbonOffset) * 12 / 1000).toFixed(2);
    document.getElementById("ReducedAnnualffsetFootprint").innerHTML = parseFloat(reducedCarbonoffsetFootprint) + ' ' + 'Tonnes';
    var ReducedAnnualCarbonFootprint = Number(document.getElementById('AverageAnnualCarbonFootprintHidden').innerText);
    var PercentageReducedCarbonOffsetFootprint = Number((ReducedAnnualCarbonFootprint - reducedCarbonoffsetFootprint) / ReducedAnnualCarbonFootprint * 100).toFixed(2);
    PercentageReducedCarbonOffsetFootprint = Number(100 - PercentageReducedCarbonOffsetFootprint).toFixed(2);
    $("#per").hide();
    document.getElementById("PercntCarbonOffsetReduction").innerHTML = PercentageReducedCarbonOffsetFootprint + ' ' + '%';
    document.getElementById("CarbonEmissionReducePercentage").innerHTML = PercentageReducedCarbonOffsetFootprint;
    document.getElementById("CarbonEmissionReducePercentage").innerText = PercentageReducedCarbonOffsetFootprint;


    // Normal Carbon Footprint Values
    var ElectricityConsumption = Number(document.getElementById("d_txt_11").value);
    var ElectricityAppliance = 0.00;
    var ApplianceValue = 0.00;
    for (var i = 1; i <= 20; i++) {
        var CaltextField = "d_consume_elec_" + i;
        ApplianceValue = document.getElementById(CaltextField) !== null ? document.getElementById(CaltextField).value : "";
        ElectricityAppliance = Number(ElectricityAppliance) + Number(ApplianceValue);

    }

    var ElectricityConsumedatResidence = ElectricityConsumption + ElectricityAppliance;
    ElectricityConsumedatResidence = parseFloat(ElectricityConsumedatResidence).toFixed(2);
    var CNGUsed = Number(document.getElementById("d_txt_9").value);
    var LPGCylinder = Number(document.getElementById("d_txt_8").value);
    var TotalDomestic = ((Number(ElectricityConsumedatResidence) + CNGUsed + LPGCylinder) / 1000).toFixed(2);
    var TotalDomesticUsed = Number(TotalDomestic);
    var DieselConsumption = Number(document.getElementById("d_txt_2").value);
    var PetrolConsumption = Number(document.getElementById("d_txt_1").value);
    var AutoRikshaw = Number(document.getElementById("d_txt_6").value);
    var Bus = Number(document.getElementById("d_txt_5").value);
    var Train = Number(document.getElementById("d_txt_7").value);
    var TotalTransportation = ((DieselConsumption + PetrolConsumption + AutoRikshaw + Bus + Train) / 1000).toFixed(2);
    var TotalTransportationUsed = Number(TotalTransportation);
    var airtrip = ((Number(document.getElementById("TotalAir").value)) / 1000).toFixed(2);
    var totalTrips = Number(airtrip);
    var TotalCarbonEmission = TotalDomesticUsed + TotalTransportationUsed + totalTrips;
    if (TotalFamilyMember != "") {

        var EmployeeTotalemissionsperMonthss = (TotalDomesticUsed + TotalTransportationUsed + totalTrips) / TotalFamilyMember;
    }
    else {
        EmployeeTotalemissionsperMonthss = TotalTransportationUsed;
    }
    var EmployeeTotalemissionsperMonth = EmployeeTotalemissionsperMonthss.toFixed(2);
    var AverageAnnualCarbonFootprinted = EmployeeTotalemissionsperMonthss * 12;
    var AnnualCarbonFootprint = AverageAnnualCarbonFootprinted.toFixed(2);

    var CalculatorMonthNaming = document.getElementById("MonthName");
    var CalculatorMonthName = CalculatorMonthNaming.options[CalculatorMonthNaming.selectedIndex].text;
    var CalculatorYear = $("#year").val();
    createChart(TotalDomesticUsed, TotalTransportationUsed, totalTrips, TotalCarbonEmission, EmployeeTotalemissionsperMonth, AnnualCarbonFootprint, offsetDomesticUse, offsetTransporationUse, offsetAirTipUse, TotalOffsetCarbonEmission, FinalAverageCarbonOffset, reducedCarbonoffsetFootprint);
    $("#myChart").show();

    var savecustomdata123 = {
        PersonalTransport: personalTransport,
        PublicTransport: PublicTransport,
        OnlineMeeting: OnlineMeeting,
        FiveStarAppliances: FiveStarAppliances,
        NumberofTreesNeeded: NoofTrees,
        NumberofPlantationProjectTreesNeeded: NoofPlantationProjectTrees,
        FundNeededtoPlantTrees: FundForTrees,
        OffsetEmissionfromDomesticUse: OffsetDomesticEmission,
        OffsetEmissionfromTransportation: OffsetTransportaionEmission,
        OffsetEmissionfromAirTrips: offsetAirTripEmission,
        TotalOffsetCarbonEmission: TotalOffsetCarbonEmission,
        AverageOffsetEmissionperMonth: FinalAverageCarbonOffset,
        OffsetAnnualCarbonFootprint: reducedCarbonoffsetFootprint,
        FormSubmitOn: currentdate,
        MonthNames: CalculatorMonthName,
        Years: CalculatorYear,
        NumberofTreesNeededValue: NoofTreesValue,
        NumberofPlantationProjectTreesNeededValue: NoofPlantationProjectTreesValue,
        FundNeededtoPlantTreesValue: FundForTreesValue,
        PersonalTransportDropdownValue: personalTransportDropdownVal,
        PersonalTransportValue: personalTransportVal,
        PublicTransportValue: publicTransportVal,
        OnlineMeetingValue: onlineMeetingVal,
        RegistrationNumber: regno,
        FiveStarAppliancesValue: fiveStarAppliancesVal
    };
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata123),
        url: "api/Transmission/ViewCostCalculatorDetails",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                console.log("data inserted");
            }
        }

    });
});

// round half away from zero
function round(num, decimalPlaces = 0) {
    var p = Math.pow(10, decimalPlaces);
    var n = (num * p) * (1 + Number.EPSILON);
    return Math.round(n) / p;
}

function createChart(TotalDomesticUsed, TotalTransportationUsed, totalTrips, TotalCarbonEmission, EmployeeTotalemissionsperMonth, AnnualCarbonFootprint, offsetDomesticUse, offsetTransporationUse, offsetAirTipUse, TotalOffsetCarbonEmission, FinalAverageCarbonOffset, reducedCarbonoffsetFootprint) {
    var xValues = ['Emission from Domestic Use', 'Emission from Transportation', 'Emission from Air Trips', 'Total Emission', 'Average Emission per Month per person', 'Reduced Annual Carbon Footprint'];
    new Chart("myChart", {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                data: [TotalDomesticUsed, TotalTransportationUsed, totalTrips, TotalCarbonEmission, EmployeeTotalemissionsperMonth, 0],
                backgroundColor: "#fd625e",
                label: "Carbon Calculation",
                fill: false
            }, {
                //data: [TotalTransportationUsed, totalTrips, TotalDomesticUsed, AnnualCarbonFootprint, TotalCarbonEmission, EmployeeTotalemissionsperMonth],
                data: [offsetDomesticUse, offsetTransporationUse, offsetAirTipUse, TotalOffsetCarbonEmission, FinalAverageCarbonOffset, reducedCarbonoffsetFootprint],
                backgroundColor: "#01b8aa",
                label: "Carbon Offset Calculation",
                fill: false
            }]
        },
        //options: {
        //    legend: { display: false }
        //}
        options: {
            responsive: true,
            title: { display: true, text: 'Carbon Calculation Comparison' },
            legend: { display: true, position: "top" }
        }
    });


}


/*User-Registration*/
$(document).ready(function () {
    if ($('.customerror').text() == '') {
        $('.customerror').next().addClass('mt-4');
    }
});
$('#FullName').blur(function () {
    var NameRegex = /^[a-zA-Z' -]+$/;
    if (!NameRegex.test($('#FullName').val())) {
        $('#FullName').next().text('Invalid Full Name');
    }
    else {
        $('#FullName').next().text(' ');
    }
});
$('#EmailId').blur(function () {
    var EmailIdRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    if (!EmailIdRegex.test($('#EmailId').val())) {
        $('#InvalidEMailMSg').text('Invalid email format.');
        $('#EmailId').parent().next().next().removeClass('mt-4');
    } else {
        $('#InvalidEMailMSg').text('');
    }
});
$('#MobileNumber').blur(function () {
    var MobileRegex = /^([+]\d{2})?\d{10}$/;
    if (!MobileRegex.test($('#MobileNumber').val())) {
        $('#InvalidMobileMSg').text('Invalid Mobile format.');
        $('#MobileNumber').parent().next().next().removeClass('mt-4');
    } else {
        $('#InvalidMobileMSg').text('');
    }
});

/*End here*/