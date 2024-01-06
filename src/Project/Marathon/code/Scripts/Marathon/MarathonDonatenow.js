$("#btnDonateSubmit").on('click', function () {



    var AffiliateCodes = $("#AffiliateCode").val();


    var EmailId = $("#cEmail").val();
    if (EmailId == "") { alert("Email is Required"); $("#cEmail").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }
    if (!validateEmail(EmailId)) { alert("Please enter valid mail"); $("#cEmail").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }



    var name = $("#cName").val();
    if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }
    if (!validateName(name)) { alert("Please enter valid name"); $("#cName").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#cMobileNo").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#cMobileNo").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }
    if (!validateMobileNo(ccontactno)) { alert("Please enter valid Mobile No"); $("#cMobileNo").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }
    var cAmounts = $("#cAmount").val();
    if (cAmounts == "") { alert("Please specify Amount"); $("#cAmount").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }
    if (!validateAmount(cAmounts)) { alert("Please enter valid Amount"); $("#cAmount").focus(); $('#btnDonateSubmit').removeAttr("disabled"); return false; }




    function validateAmount(inputtxts) {
        var numbers = /^[0-9]+$/;
        if (numbers.test(inputtxts)) { return true; }
        else { return false; }
    };

    function validateMobileNo(inputtxt) {
        var phoneno = /^\d{10}$/;
        if (phoneno.test(inputtxt)) { return true; }
        else { return false; }
    };

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    };
    function validateName(sname) {
        var regex = /^[a-zA-Z ]+$/;

        if (regex.test(sname)) { return true; }
        else { return false; }
    };



});
$('#cAmount').blur(function () {
    $('.amount_error').text('');

    var amount = $("#login_form #cAmount").val();
    if (amount != "") {
        if ($("#login_form #cAmount").val() < 100) {
            $('.amount_error').text('Amount should be greater then 100. ');
            $("#login_form #cAmount").val("");
        }
        else { $('.amount_error').text(''); }

    }
});
//$('#AffiliateCode').blur(function () {

//    $('.affiliate_code_error').text('');

//    var acode = $("#login_form #AffiliateCode").val();
//    if (acode != "") {
//        var donationData = {
//            AffiliateCode: acode
//        };

//        $.ajax({
//            type: "POST",
//            data: JSON.stringify(donationData),
//            url: "/api/Marathon/CheckAffiliateCode",
//            contentType: "application/json",
//            success: function (data) {
//                if (data.status == "1") {
//                    $('.affiliate_code_error').text('');
//                }
//                else {
//                    $('.affiliate_code_error').text('Invalid Affiliate Code!');
//                    $("#login_form #AffiliateCode").val("");
//                    return false;
//                }
//            }

//        });
//    }
//    return 0;
//});

/*PAN Number Validation*/
$("#cAffiliateCode").blur(function () {
    var PANNumber = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/g;
    if (!PANNumber.test($("#cAffiliateCode").val())) {
        var tag ="<span class='errormsg'>Please enter a valid PAN Number</span>";
		$('#cAffiliateCode').after(tag);
        return false;
    } else {
        $('#cAffiliateCode').next('.errormsg').text('');
    }
});