/* Sample function that returns boolean in case the browser is Internet Explorer*/
function isIE() {
    ua = navigator.userAgent;
    /* MSIE used to detect old browsers and Trident used to newer ones*/
    var is_ie = ua.indexOf("MSIE ") > -1 || ua.indexOf("Trident/") > -1;

    return is_ie;
}
function b64toBlob(b64Data, contentType) {
    contentType = contentType || '';
    var sliceSize = 512;
    b64Data = b64Data.replace(/^[^,]+,/, '');
    b64Data = b64Data.replace(/\s/g, '');
    var byteCharacters = window.atob(b64Data);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);

        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);

        byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, {
        type: contentType
    });
    return blob;
}

function isMobile() {
    var match = window.matchMedia || window.msMatchMedia;
    if (match) {
        var mq = match("(pointer:coarse)");
        return mq.matches;
    }
    return false;
}
var recaptcha2;
var ReqCallbackrecaptcha;
var onloadCallback = function () {
    //Render the recaptcha1 on the element with ID "recaptcha1"
    if ($('#recaptcha2').length) {
        recaptcha2 = grecaptcha.render('recaptcha2', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
    if ($('#ReqCallbackrecaptcha').length) {
        ReqCallbackrecaptcha = grecaptcha.render('ReqCallbackrecaptcha', {
            'sitekey': '6Lcql9QZAAAAAKGvvD4LZKHKys2Dwh0kFUVE5MGc',
            'theme': 'light'
        });
    }
};
$("#AfterLoginSupportFormSubmit").on("click", function () {
    var response = grecaptcha.getResponse(recaptcha2);
    if (response.length === 0) {
        alert("Captcha required.");
        return false;
    }
    $("#reResponse").val(response);
});
$("#LogInApplyForLoanSubmitBtn").on("click", function () {
    var response = grecaptcha.getResponse(recaptcha2);
    if (response.length === 0) {
        alert("Captcha required.");
        return false;
    }
    $("#reResponse").val(response);
});
function toogle() {
    $('#AfterLoginSupportFormModal').modal('show');
}
$('input[type=radio][name=MobileNoOrLoanAccountNumber]').change(function () {
    var idVal = $(this).attr("id");
    var Labeltxt = $("label[for='" + idVal + "']").text();
    $("label[for='MobileNoOrLoanAccountNumberValue']").text("Enter " + Labeltxt);
});
$.fn.truncate = function () {
    $(this).each(function () {
        $(this).text($(this).text().substr(0, 2));
    });
    return $(this).each(function () {
        $(this).text($(this).text().substr(0, 2));
    });
};
$(".INR").text(function () {
    var CurrencyVal = this.innerText;
    CurrencyVal = CurrencyVal.toString();
    var afterPoint = '';
    if (CurrencyVal.indexOf('.') > 0)
        afterPoint = CurrencyVal.substring(CurrencyVal.indexOf('.'), CurrencyVal.length);
    CurrencyVal = Math.floor(CurrencyVal);
    CurrencyVal = CurrencyVal.toString();
    var lastThree = CurrencyVal.substring(CurrencyVal.length - 3);
    var ActualNumbers = CurrencyVal.substring(0, CurrencyVal.length - 3);
    if (ActualNumbers !== '')
        lastThree = ',' + lastThree;
    var CovertedVal = ActualNumbers.replace(/\B(?=(\d{2})+(?!\d))/g, ",") + lastThree + afterPoint;
    return CovertedVal;
});

if ($(".mode-block")[0]) {
    toogle();
}

$(document).ready(function () {
    $("#login").on('click', function (event) {
        var MoborLAN = $("#MobileNoOrLoanAccountNumberValue").val();
        if (MoborLAN !== "") {
            $('#dloader').css('display', 'block');
        }
    });
    $("#ResendOTP").on('click', function (event) {
        $("#OTP").removeAttr("required");
        $('#dloader').css('display', 'block');
    });
    var idVal = $($('input[type=radio][name=MobileNoOrLoanAccountNumber]:checked')).attr("id");
    var Labeltxt = $("label[for='" + idVal + "']").text();
    $("label[for='MobileNoOrLoanAccountNumberValue']").text("Enter " + Labeltxt);

    $(".GetSOA").on('click', function (event) {
        event.preventDefault();
        var invoiceval = $(this).attr('data-document');
        $.ajax({
            type: 'GET',
            url: '/api/AdaniCapital/Get_SOA_Report',
            dataType: 'json',
            data: { LAN: $(this).attr('data-document') },
            success: function (response) {
                if (isIE()) {
                    alert('It is InternetExplorer or mobile');
                    var filename = invoiceval + "_SOA_report.pdf";
                    var blob = b64toBlob(response.PDF_report, "application/pdf;base64;");
                    navigator.msSaveBlob(blob, filename);
                } else {
                    $("#main-pdf").show();
                    $("#main-pdf").html("<iframe id='iframe1' src='data:application/pdf;base64, " + response.PDF_report + "' width='100%' style='height:100%'></iframe>");
                    $("#printPDFModal").modal('show');
                    $("#pdftodownload").attr('href', "data:application/pdf;base64, " + response.PDF_report);
                }
                if (isMobile()) {
                    $("#printPDFModal").modal('hide');

                }
            },
            error: function (ex) {
                alert("Session time out.");
                window.location.href = "/login";
            }
        });
    });
    $(".GetIntCertificate").on('click', function (event) {
        event.preventDefault();
        var invoiceval = $(this).attr('data-document');
        $.ajax({
            type: 'GET',
            url: '/api/AdaniCapital/Get_InterestCertificate',
            dataType: 'json',
            data: { LAN: $(this).attr('data-document') },
            success: function (response) {
                if (isIE()) {
                    alert('It is InternetExplorer or mobile');
                    var filename = invoiceval + "_IntersetCertificate.pdf";
                    var blob = b64toBlob(response.PDF_report, "application/pdf;base64;");
                    navigator.msSaveBlob(blob, filename);
                } else {
                    $("#main-pdf").show();
                    $("#main-pdf").html("<iframe id='iframe1' src='data:application/pdf;base64, " + response.PDF_report + "' width='100%' style='height:100%'></iframe>");
                    $("#printPDFModal").modal('show');
                    $("#pdftodownload").attr('href', "data:application/pdf;base64, " + response.PDF_report);
                }
                if (isMobile()) {
                    $("#printPDFModal").modal('hide');

                }
            },
            error: function (ex) {
                alert("Session time out.");
                window.location.href = "/login";
            }
        });
    });
    $(".GetBalConfLetter").on('click', function (event) {
        event.preventDefault();
        var invoiceval = $(this).attr('data-document');
        $.ajax({
            type: 'GET',
            url: '/api/AdaniCapital/Get_BalConfLetter',
            dataType: 'json',
            data: { LAN: $(this).attr('data-document') },
            success: function (response) {
                if (isIE()) {
                    alert('It is InternetExplorer or mobile');
                    var filename = invoiceval + "_BalanceConfiramtionLetter.pdf";
                    var blob = b64toBlob(response.PDF_report, "application/pdf;base64;");
                    navigator.msSaveBlob(blob, filename);
                } else {
                    $("#main-pdf").show();
                    $("#main-pdf").html("<iframe id='iframe1' src='data:application/pdf;base64, " + response.PDF_report + "' width='100%' style='height:100%'></iframe>");
                    $("#printPDFModal").modal('show');
                    $("#pdftodownload").attr('href', "data:application/pdf;base64, " + response.PDF_report);
                }
                if (isMobile()) {
                    $("#printPDFModal").modal('hide');

                }
            },
            error: function (ex) {
                alert(Error);
            }
        });
    });
    /* Welcome Letter Start*/
    $(".GetWelcome").on('click', function (event) {
        event.preventDefault();
        var invoiceval = $(this).attr('data-document');
        $.ajax({
            type: 'GET',
            url: '/api/AdaniCapital/Get_WelcomeLetter',
            dataType: 'json',
            data: { LAN: $(this).attr('data-document') },
            success: function (response) {
                if (isIE()) {
                    alert('It is InternetExplorer or mobile');
                    var filename = invoiceval + "_WelcomeLetter.pdf";
                    var blob = b64toBlob(response.PDF_report, "application/pdf;base64;");
                    navigator.msSaveBlob(blob, filename);
                } else {
                    $("#main-pdf").show();
                    $("#main-pdf").html("<iframe id='iframe1' src='data:application/pdf;base64, " + response.PDF_report + "' width='100%' style='height:100%'></iframe>");
                    $("#printPDFModal").modal('show');
                    $("#pdftodownload").attr('href', "data:application/pdf;base64, " + response.PDF_report);
                }
                if (isMobile()) {
                    $("#printPDFModal").modal('hide');

                }
            },
            error: function (ex) {
                alert(Error);
            }
        });
    });
    /* Welcome Letter End*/
    /* Repayment Schedule Start*/
    $(".GetRepaySchedule").on('click', function (event) {
        event.preventDefault();
        var invoiceval = $(this).attr('data-document');
        $.ajax({
            type: 'GET',
            url: '/api/AdaniCapital/Get_RepaymentSchedule',
            dataType: 'json',
            data: { LAN: $(this).attr('data-document') },
            success: function (response) {
                if (isIE()) {
                    alert('It is InternetExplorer or mobile');
                    var filename = invoiceval + "_RepaymentSchedule.pdf";
                    var blob = b64toBlob(response.PDF_report, "application/pdf;base64;");
                    navigator.msSaveBlob(blob, filename);
                } else {
                    $("#main-pdf").show();
                    $("#main-pdf").html("<iframe id='iframe1' src='data:application/pdf;base64, " + response.PDF_report + "' width='100%' style='height:100%'></iframe>");
                    $("#printPDFModal").modal('show');
                    $("#pdftodownload").attr('href', "data:application/pdf;base64, " + response.PDF_report);
                }
                if (isMobile()) {
                    $("#printPDFModal").modal('hide');

                }
            },
            error: function (ex) {
                alert(Error);
            }
        });
    });
    /* Repayment Schedule End*/
    var LANno = $('#LanList').val();
    SelectLAN(LANno);
    $("#LanList").on('change', function (event) {
        LANno = $('#LanList').val();
        $(".AllLAN").css("display", "none");
        SelectLAN(LANno);
        var Sanc_str = ".piVal ." + LANno + " .Sanctioned";
        var Out_str = ".piVal ." + LANno + " .Outstanding";
        var Curr_Sanctioned = $(Sanc_str).html();
        Curr_Sanctioned = Curr_Sanctioned.replace(/,/g, "");
        var Curr_Outstanding = $(Out_str).html();
        Curr_Outstanding = Curr_Outstanding.replace(/,/g, "");
        var oilCanvas = document.getElementById("utilChart");

        Chart.defaults.global.defaultFontFamily = "Adani-Regular";
        Chart.defaults.global.defaultFontSize = 10;

        var oilData = {
            labels: [
                "Paid",
                "Outstanding"
            ],
            datasets: [
                {
                    data: [parseInt(Curr_Sanctioned) - parseInt(Curr_Outstanding), parseInt(Curr_Outstanding)],
                    backgroundColor: [
                        "#0065b3",
                        "#28a49a"
                    ]
                }]
        };

        var pieChart = new Chart(oilCanvas, {
            type: 'pie',
            data: oilData
        });
    });
    var Sanc_str = ".piVal ." + LANno + " .Sanctioned";
    var Out_str = ".piVal ." + LANno + " .Outstanding";
    var Curr_Sanctioned = $(Sanc_str).html();
    Curr_Sanctioned = Curr_Sanctioned.replace(/,/g, "");
    var Curr_Outstanding = $(Out_str).html();
    Curr_Outstanding = Curr_Outstanding.replace(/,/g, "");
    var oilCanvas = document.getElementById("utilChart");

    Chart.defaults.global.defaultFontFamily = "Adani-Regular";
    Chart.defaults.global.defaultFontSize = 10;

    var oilData = {
        labels: [
            "Paid",
            "Outstanding"
        ],
        datasets: [
            {
                data: [parseInt(Curr_Sanctioned) - parseInt(Curr_Outstanding), parseInt(Curr_Outstanding)],
                backgroundColor: [
                    "#0065b3",
                    "#28a49a"
                ]
            }]
    };

    var pieChart = new Chart(oilCanvas, {
        type: 'pie',
        data: oilData
    });
    var msgVal = $("#AfterLoginSupportFormModal #applyErrorMsg").html();
    if (msgVal !== "" && msgVal !== undefined) {
        toogle();
        $('#AfterLoginSupportForm').trigger("reset");
    };
});

function SelectLAN(LAN) {
    $("." + LAN).css("display", "block");
};
$(function () {
    $('#Locator-State').change(function () {
        var SelectedState = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniCapital/BranchLocation",
            data: { State: SelectedState, City: '' },
            success: function (obj) {
                if (obj !== null) {
                    $('#Locator-City').text('');
                    $('#Locator-City').append("<option value=''> Select City </option>");
                    $('#Locator-address').text('');
                    $('#Locator-address').text('One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051');
                    $('#locmap').remove();
                    for (var i = 0; i < obj.length; i++) {
                        var name = obj[i];
                        $('#Locator-City').append("<option value='" + name + "'>" + name.toUpperCase() + "</option>");
                    }
                }
                else {
                    alert("Unable to locate branch");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
    $('#Locator-City').change(function () {
        var SelectedState = $('#Locator-State').val();
        var SelectedCity = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniCapital/BranchLocation",
            data: { State: SelectedState, City: SelectedCity },
            success: function (obj) {
                if (obj !== null) {
                    if (obj["Address"] !== '') {
                        $('#Locator-address').text('');
                        $('#locmap').remove();
                        $('#Locator-address').text(obj["Address"]);
                        if (obj["Latitude"] !== '' && obj["Longitude"] !== '') {
                            $('#Locator-address').after("<p id='locmap'><a target='_blank' href='http://maps.google.com/maps?q=" + obj["Latitude"] + "," + obj["Longitude"] + "'>View on Map</a></p>");
                        }

                    }
                    else {
                        $('#Locator-address').text('');
                        $('#Locator-address').text('One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051');
                        $('#locmap').remove();
                        alert("Unable to locate branch at selected city");
                    }
                }
                else {
                    alert("Unable to locate branch");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
    $('#centre-State').change(function () {
        var SelectedState = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniCapital/CentreLocation",
            data: { State: SelectedState, City: '' },
            success: function (obj) {
                if (obj !== null) {
                    $('#centre-City').text('');
                    $('#centre-City').append("<option value=''> Select City </option>");
                    $('#centre-info').empty();
                    $('#centre-info').append('<div><h4> Adani Capital Pvt.Ltd.</h4><p>One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051</p><p><a href="tel:18002100444"><span class="fa fa-phone fa-rotate-90 mr-2"></span> 1800-210-0444</a></p><p id="Branch-mail"><a href="mailto:info@AdaniCapital.com"><span class="fa fa-envelope mr-2"></span>  info@AdaniCapital.com</a></p></div>');
                    for (var i = 0; i < obj.length; i++) {
                        var name = obj[i];
                        $('#centre-City').append("<option value='" + name + "'>" + name.toUpperCase() + "</option>");
                    }
                }
                else {
                    alert("Unable to locate collection centre");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
    $('#centre-City').change(function () {
        var SelectedState = $('#centre-State').val();
        var SelectedCity = $(this).val();
        //var propName = $("#PropertyName option:selected").text();
        $.ajax({
            type: "GET",
            url: "/api/AdaniCapital/CentreLocation",
            data: { State: SelectedState, City: SelectedCity },
            success: function (obj) {
                if (obj !== null) {
                    if (obj.length > 0) {
                        $('#centre-info').empty();
                        for (var i = 0; i < obj.length; i++) {
                            var Address = obj[i]["Address"];
                            var MerchantOrShop = obj[i]["MerchantOrShop"];
                            var OfficeType = obj[i]["OfficeType"];
                            var ContactNo = obj[i]["ContactNo"];
                            var Latitude = obj[i]["Latitude"];
                            var Longitude = obj[i]["Longitude"];
                            $('#centre-info').append("<div><h4>" + OfficeType + "</h4><p>" + MerchantOrShop + "</p><p>" + Address + '</p><p><a href="tel:' + ContactNo + '"><span class="fa fa-phone fa-rotate-90 mr-2"></span>' + ContactNo + "</p></div>");
                        }
                    }
                    else {
                        $('#centre-info').empty();
                        $('#centre-info').append('<div>< h4 > Adani Capital Pvt.Ltd.</h4 ><p>One BKC, 1004 / 5, C - Wing, 10th Floor, Bandra Kurla Complex, Bandra East, Mumbai, Maharashtra 400051</p><p><a href="tel:18002100444"><span class="fa fa-phone fa-rotate-90 mr-2"></span> 1800-210-0444</a></p><p id="Branch-mail"><a href="mailto:info@AdaniCapital.com"><span class="fa fa-envelope mr-2"></span>  info@AdaniCapital.com</a></p></div>');
                    }
                }
                else {
                    alert("Unable to locate collection centre");
                }
            },
            error: function () {
                alert("Error occurred");
            }
        });
    });
});
$("#btnReqCallbackSubmit").click(function () {
    var response = grecaptcha.getResponse(ReqCallbackrecaptcha);
    if (response.length === 0) {
        alert("Captcha required.");
        return false;
    }

    $('#btnReqCallbackSubmit').attr("disabled", "disabled");
    var name = $("#ReqCallbackName").val();
    if (name === "") { $("#ReqCallbackName").siblings('p:first').html("Please enter your Name"); $("#ReqCallbackName").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#ReqCallbackEmailId").val();
    if (mailid === "") { $("#ReqCallbackEmailId").siblings('p:first').html("Email is Required"); $("#ReqCallbackEmailId").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#ReqCallbackMobile").val();
    if (ccontactno === "") { $("#ReqCallbackMobile").siblings('p:first').html("Please specify your Mobile Number"); $("#ReqCallbackMobile").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }

    if (ccontactno.length !== 10) {
        $("#ReqCallbackMobile").siblings('p:first').html("Contact Number should be of 10 digit"); $("#ReqCallbackMobile").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false;

    }

    var message = $("#ReqCallbackMessage").val();
    if (message === "") { $("#ReqCallbackMessage").siblings('p:first').html("Please enter any message"); $("#ReqCallbackMessage").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }

    var formtype = $("#ReqCallbackformName").val();
    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnReqCallbackSubmit').removeAttr("disabled"); return false; }
    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {
        Name: name,
        EmailID: mailid,
        MobileNo: ccontactno,
        Message: message,
        reResponse: response,
        FormName: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniCapital/RequestCallbackForm",
        contentType: "application/json",
        success: function (data) {
            //////////////
            if (data.status === '0') {
                alert(data.msg);
                $('#btnReqCallbackSubmit').removeAttr("disabled");
                return false;
            }
            else {
                $("#req_callback .close").click();
                $('#ReqCallbackForm').trigger("reset");
                $('#btnReqCallbackSubmit').removeAttr("disabled");
                alert(data.msg);
            }

        },
        error: function (data) {
            alert("Something has been wrong, Please try again");
            $('#btnReqCallbackSubmit').removeAttr("disabled");
            return false;
        }
    });
    return false;

});