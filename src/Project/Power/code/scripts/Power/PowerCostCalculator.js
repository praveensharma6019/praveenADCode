

$("#btnsubmit").click(function () {
    //var response = grecaptcha.getResponse(recaptcha1);
    //if (response.length == 0) {
    //    alert("Captcha required.");
    //    return false;
    //}


   // $('#btnsubmit').attr("disabled", "disabled");

    var mailid = $("#Emails").val();

    var ElectricityConsumedatResidence = Number(document.getElementById("d_txt_10").value);
    var CNGUsed = Number(document.getElementById("d_txt_9").value);
    var LPGCylinder = Number(document.getElementById("d_txt_8").value);
    var TotalDomesticUsed = ElectricityConsumedatResidence + CNGUsed + LPGCylinder;

    //display the result
    var TotalDomesticUse = TotalDomesticUsed.toFixed(2);
    document.getElementById('EmissionfromDomesticUse').innerHTML = TotalDomesticUse + ' ' + 'KG'


    var DieselConsumption = Number(document.getElementById("d_txt_2").value);
    var PetrolConsumption = Number(document.getElementById("d_txt_1").value);
    var AutoRikshaw = Number(document.getElementById("d_txt_6").value);
    var Bus = Number(document.getElementById("d_txt_5").value);
    var Train = Number(document.getElementById("d_txt_7").value);
    var TotalTransportationUsed = DieselConsumption + PetrolConsumption + AutoRikshaw + Bus + Train;

    //display the result
    var TotalTransportationUse = TotalTransportationUsed.toFixed(2);
    document.getElementById('EmissionfromTransportation').innerHTML = TotalTransportationUse + ' ' + 'KG'

    var TotalFamilyMember = Number(document.getElementById("FamilyMembers").value);
    if (TotalFamilyMember != "") {
        var EmployeeTotalemissionsperMonthss = (TotalDomesticUsed / TotalFamilyMember) + TotalTransportationUsed;
    }

    else {
        var EmployeeTotalemissionsperMonthss = TotalTransportationUsed;
    }

    var EmployeeTotalemissionsperMonth = EmployeeTotalemissionsperMonthss.toFixed(2);
    //display the result
    document.getElementById('TotalemissionsperMonth').innerHTML = EmployeeTotalemissionsperMonth + ' ' + 'KG'
    var AverageAnnualCarbonFootprinted = EmployeeTotalemissionsperMonthss * 12 / 1000;
    var CalculatorMonthNaming = document.getElementById("MonthName");
    var CalculatorMonthName = CalculatorMonthNaming.options[CalculatorMonthNaming.selectedIndex].text;

    var CalculatorYear = $("#year").val();
    var AnnualCarbonFootprint = AverageAnnualCarbonFootprinted.toFixed(2);
    //display the result
    document.getElementById('EmployeeAverageAnnualCarbonFootprint').innerHTML = AnnualCarbonFootprint + '  ' + 'Tonnes'

    var LandNeed = AnnualCarbonFootprint * 0.04;
    //display the result
    var LandNeededHectares = LandNeed.toFixed(2);;
    document.getElementById('LandNeeded').innerHTML = LandNeededHectares + '  ' + 'hectares'
    var NoTreesNeeded = AnnualCarbonFootprint * 64;
    //display the result
    var TreesNeeded = Math.round(NoTreesNeeded);
    document.getElementById('NoOfTreesNeeded').innerHTML = TreesNeeded;


    $('#CalculatedValue').show();
    $('#offset').show();
    var txt10 = Number(document.getElementById("txt_10").value);
    if (txt10 == "") {
        $("#txt_10").val("0");


    }
    var text10 = document.getElementById("txt_10");
    text10.setAttribute("disabled", "disabled");
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
    $('#btnreset').show();
    var model = {
        Email: mailid
    };


    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    var formtype = $("#cFormType").val();
    var pageinfo = window.location.href;
    //create json object
    var savecustomdata = {


        ElectricityConsumedatResidences: ElectricityConsumedatResidence,
        Email: mailid,
        TotalFamilyMembers: TotalFamilyMember,
        MonthNames: CalculatorMonthName,
        Years: CalculatorYear,
        CNGUseds: CNGUsed,
        LPGCylinders: LPGCylinder,
        DieselConsumptions: DieselConsumption,
        PetrolConsumptions: PetrolConsumption,
        AutoRikshaws: AutoRikshaw,
        Buses: Bus,
      //  reResponse: response,
        Trains: Train,
        FormType: formtype,
        PageInfo: pageinfo,
        TotalTransportationUses: TotalTransportationUse,
        TotalDomesticUses: TotalDomesticUse,
        EmployeeTotalemissionsperMonths: EmployeeTotalemissionsperMonth,
        AnnualCarbonFootprints: AnnualCarbonFootprint,
        AverageAnnualCarbonFootprints: '1.94Tonnes',
        NumberOfTreesNeeded: TreesNeeded,
        LandNeeded: LandNeededHectares,
        FormSubmitOn: currentdate

    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Power/InsertCostCalculator",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {

            }

            else if (data.status == "2") {
                alert("Please validate your form before submitting");
                $('#btnsubmit').removeAttr("disabled");
                return false;
                //$('#contact_form1').submit();
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnsubmit').removeAttr("disabled");
                return false;
            }


        }

    });
    return false;
});





$(document).ready(function () {
    const monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
    var qntYears = 10;
    var selectYear = $("#year");
    var selectMonth = $("#MonthName");
    var currentYear = new Date().getFullYear();

    for (var y = 0; y < qntYears; y++) {
        let date = new Date(currentYear);
        var yearElem = document.createElement("option");
        yearElem.value = currentYear
        yearElem.textContent = currentYear;
        selectYear.append(yearElem);
        currentYear--;
    }

    for (var m = 0; m < 12; m++) {
        let monthNum = new Date(2018, m).getMonth()
        let month = monthNames[monthNum];
        var monthElem = document.createElement("option");
        monthElem.value = monthNum;
        monthElem.textContent = month;
        selectMonth.append(monthElem);
    }

    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();
    var day = d.getDate();

    selectYear.val(year);
    selectYear.on("change", AdjustDays);
    selectMonth.val(month);
    selectMonth.on("change", AdjustDays);

    AdjustDays();
    selectDay.val(day)

    function AdjustDays() {
        var year = selectYear.val();
        var month = parseInt(selectMonth.val()) + 1;
        selectDay.empty();


    }
});