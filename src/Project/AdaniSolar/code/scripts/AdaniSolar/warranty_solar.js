var recaptcha5;
var onloadCallback = function () {

    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha5 = grecaptcha.render('recaptcha5', {
        'sitekey': '6Lfm5bMUAAAAABxHKwXkt5xkc7hHDR_m_fYTgKc-', //Replace this with your Site key //6Le7Ma0UAAAAAHuZ5Li5kM5StUbTIDEOAeabw1Gc
        'theme': 'light'
    });
};
var myCallBack = function () {
    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha5 = grecaptcha.render('recaptcha5', {
        'sitekey': '6Lfm5bMUAAAAABxHKwXkt5xkc7hHDR_m_fYTgKc-', //Replace this with your Site key
        'theme': 'light'
    });
};

$("#myform1").on("click", function (e) {
    ClearData();
    $("#errorvalidation").hide();
    var AdaniInvoiceNumber = $("#number").val();
    var AdaniInvoiceDate = $("#invoicedate").val();

    if (!AdaniInvoiceNumber) {
        $("#number").after('<span class="error" id="errorvalidation" style="color: red;">This field is required</span>');
        $("#errorvalidation").show();
        return false;
    }
    if (!/^[0-9]+$/.test(AdaniInvoiceNumber)) {
        $("#number").after('<span class="error" id="errorvalidation" style="color: red;">Please enter valid Invoice number </span>');
        $("#errorvalidation").show();
        return false;
    }

    if (!AdaniInvoiceDate) {
        $("#invoicedate").after('<span class="error" id="errorvalidation" style="color: red;">Please enter valid Invoice number </span>');
        return false;
    }

    var isDateValid = isFutureDate(AdaniInvoiceDate);
    if (!isDateValid) {
        $("#invoicedate").after('<span class="error" id="errorvalidation" style="color: red;">Please enter valid Invoice number </span>');
        return false;
    }
    $("#errorvalidation").hide();
    var SectionType = "CP";

    var formdata = new FormData();
    formdata.append('AdaniInvoiceNumber', AdaniInvoiceNumber);
    formdata.append('AdaniInvoiceDate',AdaniInvoiceDate);
    formdata.append('SectionType', SectionType);
    sessionStorage.setItem("invoiceno", AdaniInvoiceNumber);
    sessionStorage.setItem("invoicedate", AdaniInvoiceDate);

    $.ajax({
        url: "/api/AdaniSolar/WarrantyCertificate",
        method: "POST",
        processData: false,
        contentType: false,
        data: formdata,
        cache: false,
        success: function (data) {
            if (data.status == "1") {
                var count = 1;
                $("#myTable").find('tbody').html('');
                $.each(data.list, function (index, item) {
                    $("#myTable").find('tbody').append('<tr><td>' + count + '</td><td>' + item.Serial_No + '</td><td><input type="checkbox" id="cb' + item.Serial_No + '" name="serialData" class="dtCheckbox" onclick="CountSelectedCheckbox()" value=' + item.Serial_No + ' data-PalletID=' + item.Pallet_ID + ' data-WarrantyEndDate=' + item.Warranty_End_Date + '></td></<tr>');
                    count++;
                });
                $("#number").val('')
                $("#invoicedate").val('')
                sessionStorage.setItem("hdnBtn", '0');
                $('#InvoiceNumberPopup').modal('show')


            }
            else if (data.status == "0") {
                $("#number").addClass("error");
                $("#invoicedate").addClass("error");
            }
            else if (data.status == "2") {
                alert("Oops! There seems to be an input error.\n\nIf you continue to experience difficulties, please reach out to us at cs@adani.com for further assistance.\nThank you for your cooperation.");
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            }

        },

    });
});
function isFutureDate(invoicedate) {
    var today = new Date();
    var userDate = new Date(invoicedate);

    if (userDate > today) {
        alert("Invoice date is not taken future date");
        return false;
    }
    return true;

}

function CountSelectedCheckbox() {
    var $checkboxes = $('#myTable td input[type="checkbox"]');
    $checkboxes.on('change', function () {
        var countCheckedCheckboxes = $checkboxes.filter(':checked').length;
        $('#selectedcheckboxcount').text(countCheckedCheckboxes);
    });
}

$(document).ready(function () {

    $('#downloadFileButton').click(function () {
        var getSelectedCheckboxValue = $.map($("input:checkbox[name=serialData]:checked"), function (n, i) {
            return n.value;
        }).join(',');
        if (getSelectedCheckboxValue !== '') {
            var cerinput = [];
            var warrantydate = null;
            $("#warrantyDataTable").empty();
            var getCheckboxIDs = getSelectedCheckboxValue.split(',');
            for (var i = 0; i < getCheckboxIDs.length; i++) {
                var cbID = "#cb" + getCheckboxIDs[i];
                warrantydate = $(cbID).attr("data-WarrantyEndDate").substr(6, 2) + '-' + $(cbID).attr("data-WarrantyEndDate").substr(4, 2) + '-' + $(cbID).attr("data-WarrantyEndDate").slice(0, 4);
                cerinput.push({ 'palletId': $(cbID).attr("data-PalletID"), 'moduleSerialNumber': $(cbID).attr("value"), 'performanceWarrantyTill': warrantydate });
                var formdata = new FormData();
                formdata.append('Invoice_Number', $(cbID).attr("value"));
                var UserName = $('#consumerName').val();
                UpdateDownloadHistory($(cbID).attr("data-PalletID"), $(cbID).attr("value"), warrantydate,UserName);
            }
            var cerinputStr = '';
            var coookieValueCustom = "";
            if (cerinput != '') {
                cerinputStr = JSON.stringify(cerinput);
                sessionStorage.setItem("selcerdata", cerinputStr);
                $("#cerinputHidden").val(cerinputStr)
                coookieValueCustom = $("#cerinputHidden").val();
                console.log(cerinputStr);

            }
            window.open("/show-certificate", "_blank");
        }


    });
});

function selectAllMyTable() {
    var dataTable = document.getElementById('myTable');
    var checkItAll = document.getElementById("selectAllFromTable");
    var isSelecteAllChecked = $("#selectAllFromTable").is(":checked");
    var inputs = dataTable.querySelectorAll('tbody>tr>td>input');
    if (isSelecteAllChecked) {
        checkItAll.addEventListener('change', function () {
            if (checkItAll.checked) {
                inputs.forEach(function (input) {
                    input.checked = true;
                });
                var $checkboxes = $('#myTable td input[type="checkbox"]');
                var countCheckedCheckboxes = $checkboxes.filter(':checked').length;
                $('#selectedcheckboxcount').text(countCheckedCheckboxes);
            }
            else {
                inputs.forEach(function (input) {
                    input.checked = false;
                });
                $('#selectedcheckboxcount').text(0);
            }
        });
    }

}

$("#closebutton").click(function () {

    $('#InvoiceNumberPopup').hide();
    setTimeout(function () {
        window.location.reload();
    }, 500);
});
$("#Mymodalvi").click(function (e) {


    if (!$("#consumernumber").val()) {
        $("#consumernumber").addClass("error");
        return false;
    }

    if (!/^[a-zA-Z0-9]+$/.test($("#consumernumber").val())) {
        $("#consumernumber").addClass("error");
        return false;
    }

    var saveConsumer = {
        ModuleSerialNumber: $("#consumernumber").val()
    };

    $.ajax({
        type: "POST",
        data: JSON.stringify(saveConsumer),
        url: "/api/AdaniSolar/SaveWarrantyCertificate",
        contentType: "application/json",
        success: function (data) {
            $('#Mymodalvi').modal('show')
            if (data.status == 1) {

            } else if (data.status == 0) {
                $("#consumernumber").addClass("error");
            }

        },
        error: function (data) {
            alert(data.status);
        }
    });
})

//add more serial row
$('#addSerialRow').click(function (e) {

    // Create a new element
    var newNode = document.createElement('input');
    var newNode1 = document.createElement('button');
    // add properties to new element
    newNode.setAttribute('type', 'text');
    newNode.setAttribute('id', 'my-title');
    newNode.setAttribute('placeholder', 'Enter module serial number');
    newNode.setAttribute('class', 'serial-number');
    newNode1.setAttribute('title', "remove");
    newNode1.setAttribute('class', "remove");
    newNode1.setAttribute('type', "button");
    newNode1.innerHTML = "<span><i class='fa fa-times'></i></span>";

    //get total child to create an id
    var childCount = $('.remove').last().attr('id') == undefined ? 1 : parseInt($('.remove').last().attr('id')) + 1;
    var elementID = "consumernumber" + childCount.toString();
    newNode1.setAttribute('id', childCount.toString());
    newNode.setAttribute('id', elementID);

    // Get the parent node
    var parentNode = document.querySelector('#serialNumbers');
    // Insert the new node after the last element in the parent node
    parentNode.appendChild(newNode);
    parentNode.appendChild(newNode1);

    if (childCount >= 5) {
        $('#serialNumbers').css({ 'overflow-y': "scroll", 'height': '200px' });
    }
});

$(document).on('click', '.remove', function () {
    var found = document.getElementById("consumernumber" + this.id);
    found.remove();
    this.remove();
});


//generate OTP
$("#generateOTPbtn").click(function (e) {
    $("#timer").css("display", "block");
    $('#ist').val('');
    $('#sec').val('');
    $('#third').val('');
    $('#fourth').val('');
    $('#fifth').val('');
    $('#sixth').val('');
    var UserName = $('#consumerName').val();
    var Address = $('#consumerAddress').val();
    var Country = $('#region').val();
    var State = $('#State').val();
    var City = $('#consumerCity').val();
    var Email = $('#consumerEmail').val();
    var Mobile = $('#consumerMobile').val();

    var mobileNumber = $('#consumerMobile').val();
    var consumerEmail = $('#consumerEmail').val();
    if (!mobileNumber) {
        $("#consumerMobile").after('<p class="text-danger">mobile required ?</p>');
        $("#consumerMobile").addClass("error");
        return false;
    } else {
        $("#consumerMobile").next('p').remove();
        $("#consumerMobile").removeClass("error");
    }
    if (!/^[0-9]+$/.test(mobileNumber)) {
        $("#consumerMobile").after('<p class="text-danger">Invalid mobile Number</p>');
        $("#consumerMobile").addClass("error");
        return false;
    } else {
        $("#consumerMobile").next('p').remove();
        $("#consumerMobile").removeClass("error");
    }
    if (mobileNumber.length < 5) {
        $("#consumerMobile").addClass("error");
        return false;
    }
    if (!consumerEmail) {
        $("#consumerEmail").after('<p class="text-danger">EmailId required ?</p>');
        $("#consumerEmail").addClass("error");
        return false;
    } else {
        $("#consumerEmail").next('p').remove();
        $("#consumerEmail").removeClass("error");
    }

    if (!validateEmail(consumerEmail)) {
        $("#consumerEmail").after('<p class="text-danger">Can you please enter correct EmailID ?</p>');
        $("#consumerEmail").focus();
        $("#consumerEmail").addClass("error");
        return false;
    }
    else {
        $("#consumerEmail").next('p').remove();
        $("#consumerEmail").removeClass("error");
    }

    var OtpType = "Email";
    var UserName = $('#consumerName').val();
    var Email = $('#consumerEmail').val();
    var formdata = new FormData();
    formdata.append('Mobile', Mobile);
    formdata.append('OtpType', OtpType);
    formdata.append('UserName', UserName);
    formdata.append('Email', Email);

    $.ajax({
        url: "/api/AdaniSolar/GenerateOTP",
        method: "POST",
        processData: false,
        contentType: false,
        data: formdata,
        cache: false,
        success: function (data) {
            if (data.status == "1") {
                document.getElementById("otpBlock").style.display = "block";
                var counter = 120;
                var interval = setInterval(function () {
                    counter--;
                    // Display 'counter' wherever you want to display it.
                    $('#generateOTPbtn').css('display', 'none');
                    if (counter <= 0) {
                        clearInterval(interval);
                        $('#generateOTPbtn').css('display', 'block');
                        document.getElementById("otpBlock").style.display = "none";
                        return;
                    } else {
                        $('#time').text(counter);
                    }
                }, 1000);
            }
            else if (data.status == "0") {
            }
            else if (data.status == "3") {
                alert("OTP Generate more than 4 times, Try after 1 Hour");
            }
        },
        error: function (data) {
            alert(data.status);
        }
    });
});

$("#filter").on("change", function () {
    var value = $(this).val();

    $("table tr").each(function (index) {
        if (index != 0) {
            $row = $(this);
            var id = $row.find("td:first").text();
            if (id.indexOf(value) != 0) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        }
    });


});


function ClearData() {
    $('#consumerName').val('');
    $('#consumerAddress').val('');
    $('#consumerCity').val('');
    $('#consumerMobile').val('');
    $('#consumerEmail').val('');
    $("#region").prop('selectedIndex', 'Select');
    $("#State").prop('selectedIndex', 'Select');
    $("#selectedcheckboxcount").text(0);
    $("#selectAllFromTable").prop("checked", false);
    $('#ist').val('');
    $('#sec').val('');
    $('#third').val('');
    $('#fourth').val('');
    $('#fifth').val('');
    $('#sixth').val('');
    $("#EnterCountry").remove();
    $("#EnterState").remove();
}

$("#consumerSubmit").click(function (e) {
    ClearData();
    var arrayinput = [];
    $("#serialNumbers input").each(function () {
        var inputIds = $(this).attr("id");
        arrayinput.push($("#" + inputIds).val());
    });
    var Serial_No = arrayinput.join(",");
    var formdata = new FormData();
    formdata.append('Serial_No', Serial_No);
    $.ajax({
        url: "/api/AdaniSolar/VerifySerialNumber",
        method: "POST",
        processData: false,
        contentType: false,
        data: formdata,
        cache: false,
        success: function (data) {
            if (data.status == "1") {
                sessionStorage.setItem("invoiceno", "");
                sessionStorage.setItem("invoicedate", "");
                $('#consumerDataFormPopup').show();
            }
            else if (data.status == "2") {
                $('#consumerDataFormPopup').hide();
                alert("Oops! There seems to be an input error.\n\nIf you continue to experience difficulties, please reach out to us at cs@adani.com for further assistance.\nThank you for your cooperation.");
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            }
        },
    });

});

function validateEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(sEmail)) { return true; }
    else { return false; }
}

function Isvalidate() {

    var consumerName = $('#consumerName').val();
    if (!consumerName) {
        $("#consumerName").after('<p class="text-danger">Name required ?</p>');
        $("#consumerName").addClass("error");
        return false;
    }
    else {
        $("#consumerName").next('p').remove();
        $("#consumerName").removeClass("error");
    }

    var consumerAddress = $('#consumerAddress').val();
    if (!consumerAddress) {
        $("#consumerAddress").after('<p class="text-danger">Address required ?</p>');
        $("#consumerAddress").addClass("error");
        return false;
    }
    else {
        $("#consumerAddress").next('p').remove();
        $("#consumerAddress").removeClass("error");
    }

    var consumerCity = $('#consumerCity').val();
    if (!consumerCity) {
        $("#consumerCity").after('<p class="text-danger">City required ?</p>');
        $("#consumerCity").addClass("error");
        return false;
    }
    else {
        $("#consumerCity").next('p').remove();
        $("#consumerCity").removeClass("error");
    }

    var mobileNumber = $('#consumerMobile').val();
    var consumerEmail = $('#consumerEmail').val();
    if (!mobileNumber) {
        $("#consumerMobile").after('<p class="text-danger">mobile required ?</p>');
        $("#consumerMobile").addClass("error");
        return false;
    } else {
        $("#consumerMobile").next('p').remove();
        $("#consumerMobile").removeClass("error");
    }
    if (!/^[0-9]+$/.test(mobileNumber)) {
        $("#consumerMobile").after('<p class="text-danger">Invalid mobile Number</p>');
        $("#consumerMobile").addClass("error");
        return false;
    } else {
        $("#consumerMobile").next('p').remove();
        $("#consumerMobile").removeClass("error");
    }
    if (mobileNumber.length < 5) {
        $("#consumerMobile").addClass("error");
        return false;
    }
    if (!consumerEmail) {
        $("#consumerEmail").after('<p class="text-danger">EmailId required ?</p>');
        $("#consumerEmail").addClass("error");
        return false;
    } else {
        $("#consumerEmail").next('p').remove();
        $("#consumerEmail").removeClass("error");
    }

    if (!validateEmail(consumerEmail)) {
        $("#consumerEmail").after('<p class="text-danger">Can you please enter correct EmailID ?</p>');
        $("#consumerEmail").focus();
        $("#consumerEmail").addClass("error");
        return false;
    }
    else {
        $("#consumerEmail").next('p').remove();
        $("#consumerEmail").removeClass("error");
    }
    return true;
}
$("#submitConsumerDataBtn").click(function (e) {
   var response = grecaptcha.getResponse(recaptcha5);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }

    if (!Isvalidate()) {
        return false;
    }
    var arrayinput = [];
    $("#serialNumbers input").each(function () {
        var inputIds = $(this).attr("id");
        arrayinput.push($("#" + inputIds).val());
    });
    var Serial_No = arrayinput.join(",");
    var otpinput = [];
    $("#otpnumber input").each(function () {
        var inputIds = $(this).attr("id");
        otpinput.push($("#" + inputIds).val());
    });
    var otpNumbers = otpinput.join("");
    var UserName = $('#consumerName').val();
    var Address = $('#consumerAddress').val();
    var Country = $("#region option:selected").text(); // $('#region').val();
    var State = $("#State option:selected").text();  // $('#State').val();
    var City = $('#consumerCity').val();
    var Email = $('#consumerEmail').val();
    var Mobile = $('#consumerMobile').val();
    var FormSubmitOn = new Date().toISOString().slice(0, 19).replace('T', ' ');
    var SectionType = "Consumer";

    var formdata = new FormData();
    formdata.append('userOTP', otpNumbers);
    formdata.append('UserName', UserName);
    formdata.append('Address', Address);
    formdata.append('Country', Country);
    formdata.append('State', State);
    formdata.append('City', City);
    formdata.append('Email', Email);
    formdata.append('Mobile', Mobile);
    formdata.append('FormSubmitOn', FormSubmitOn);
    formdata.append('Serial_No', Serial_No);
    formdata.append('SectionType', SectionType);
    formdata.append('Response', response);

    $.ajax({
        url: "/api/AdaniSolar/VerifyOTP",
        method: "POST",
        processData: false,
        contentType: false,
        data: formdata,
        cache: false,
        success: function (data) {
            if (data.status == "1") {
                var count = 1;
                $('#consumerDataFormPopup').hide();
                $("#myTable").find('tbody').html('');
              
                $.each(data.list, function (index, item) {
                    $("#myTable").find('tbody').append('<tr><td>' + count + '</td><td>' + item.Serial_No + '</td><td><input type="checkbox" id="cb' + item.Serial_No + '" name="serialData" class="dtCheckbox" onclick="CountSelectedCheckbox()" value=' + item.Serial_No + ' data-PalletID=' + item.Pallet_ID + ' data-WarrantyEndDate=' + item.Warranty_End_Date + '></td></<tr>');
                    count++;
                });
                $("#number").val('')
                $("#invoicedate").val('')
                sessionStorage.setItem("hdnBtn", '1');
                $('#InvoiceNumberPopup').show();
            }
            else if (data.status == "0") {
                $("#number").addClass("error");
                $("#invoicedate").addClass("error");
            }
            else if (data.status == "2") {
                alert("Oops! There seems to be an input error.\n\nIf you continue to experience difficulties, please reach out to us at cs@adani.com for further assistance.\nThank you for your cooperation.");
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            }
            else if (data.status == "3") {
                alert("Invalid OTP");
            }
            else if (data.status == "4") {
                alert("Wrong Attempt Exceeds 4 times.Try after 1 hour");
            }
        },

        //error: function (data) {
        //    alert(data.status);
        //}

    });
})

//close consumer form popup
$('#closeConsumerPopup').click(function (e) {
    $('#consumerDataFormPopup').hide();
    setTimeout(function () {
        window.location.reload();
    }, 500);
});
$("#region").change(function () {
    if ($("#region").val() == 'Select') {
        $("#State").text('');
        $("#State").append('<option value="Default">Select State</option>');
        $("#State").append('<option value="OTHERS">Others</option>');
        statedrp = $(statedrp);
    }
    else {
        loadStateDrp('#State');
    }

});

function loadStateDrp(statedrp) {
    $(statedrp).text('');
    $(statedrp).append('<option value="Default">Select State</option>');
    statedrp = $(statedrp);
    if ($("#region").val() == 'India') {
        $("#EnterCountry").hide();
        $("#EnterState").hide();
        //$("#State").replaceWith($('<select/>', {'class': 'form-control', 'id': 'State' }));
        statedrp.append($("<option />").val('IN-AP').text('Andhra Pradesh'));
        statedrp.append($("<option />").val('IN-AR').text('Arunachal Pradesh'));
        statedrp.append($("<option />").val('IN-AS').text('Assam'));
        statedrp.append($("<option />").val('IN-BR').text('Bihar'));
        statedrp.append($("<option />").val('IN-CT').text('Chhattisgarh'));
        statedrp.append($("<option />").val('IN-GA').text('Goa'));
        statedrp.append($("<option />").val('IN-GJ').text('Gujarat'));
        statedrp.append($("<option />").val('IN-HR').text('Haryana'));
        statedrp.append($("<option />").val('IN-HP').text('Himachal Pradesh'));
        statedrp.append($("<option />").val('IN-JH').text('Jharkhand'));
        statedrp.append($("<option />").val('IN-KA').text('Karnataka'));
        statedrp.append($("<option />").val('IN-KE').text('Kerala'));
        statedrp.append($("<option />").val('IN-MP').text('Madhya Pradesh'));
        statedrp.append($("<option />").val('IN-MH').text('Maharashtra'));
        statedrp.append($("<option />").val('IN-MN').text('Manipur'));
        statedrp.append($("<option />").val('IN-ML').text('Meghalaya'));
        statedrp.append($("<option />").val('IN-MZ').text('Mizoram'));
        statedrp.append($("<option />").val('IN-NL').text('Nagaland'));
        statedrp.append($("<option />").val('IN-OR').text('Odisha'));
        statedrp.append($("<option />").val('IN-PB').text('Punjab'));
        statedrp.append($("<option />").val('IN-RJ').text('Rajasthan'));
        statedrp.append($("<option />").val('IN-SK').text('Sikkim'));
        statedrp.append($("<option />").val('IN-TN').text('Tamil Nadu'));
        statedrp.append($("<option />").val('IN-TE').text('Telangana'));
        statedrp.append($("<option />").val('IN-TR').text('Tripura'));
        statedrp.append($("<option />").val('IN-UP').text('Uttar Pradesh'));
        statedrp.append($("<option />").val('IN-UT').text('Uttarakhand'));
        statedrp.append($("<option />").val('IN-WB').text('West Bengal'));
        $("#State").show();
    }
    else if ($("#region").val() == 'Other') {
        $("#EnterCountry").remove();
        $("#EnterState").remove();
        $("#State").hide();
        $("#State").after($('<input/>', { 'type': 'text', 'placeholder': 'Enter Country', 'class': 'form-control', 'id': 'EnterCountry' }));
        $("#EnterCountry").after($('<input/>', { 'type': 'text', 'placeholder': 'Enter State', 'class': 'form-control', 'id': 'EnterState' }));
    }
    else {

        $("#EnterCountry").hide();
        $("#EnterState").hide();
        $("#State").show();



    }
}
function formatDate(dateString) {
    var date = new Date(dateString);
    var month = String(date.getMonth() + 1).padStart(2, '0'); // Adding 1 because month is zero-based
    var day = String(date.getDate()).padStart(2, '0');
    var year = String(date.getFullYear());

    return year + '-' + month + '-' + day ;
}
function UpdateDownloadHistory(Pallet_Id, Module_Serial_Number, Warranty_valid_till, UserName) {
    let invoicenosess = sessionStorage.getItem("invoiceno");
    let invoicedate = formatDate(sessionStorage.getItem("invoicedate"));

    if (invoicedate == 'NaN-NaN-NaN') {
        invoicedate = null;
    }

    var formdata = new FormData();
    formdata.append('UserName', UserName);

    formdata.append('Invoice_Number', invoicenosess)

    var newdate = Warranty_valid_till.toString().split("-").reverse().join("-");
 
    formdata.append('Invoice_Date', invoicedate);
    formdata.append('Pallet_Id', Pallet_Id);
    formdata.append('Module_Serial_Number', Module_Serial_Number);
    formdata.append('Warranty_valid_till', newdate);
    const date = new Date();
    formdata.append('Currentdate', '12-06-2023');
    formdata.append('CurrentTime', '12-06-2023');

    $.ajax({
        url: "/api/AdaniSolar/SaveDownloadHistoryData",
        method: "POST",
        processData: false,
        contentType: false,
        data: formdata,
        cache: false,
        success: function (data) {
            console.log("True");
        },
    });

}