var recaptcha2;
var onloadCallback = function () {

    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha2', {
        'sitekey': '6LclBqwUAAAAAJ2KtS78FPoLPod26RXeKH5iddFy', //Replace this with your Site key
        'theme': 'light'
    });


};
var myCallBack = function () {
    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha2', {
        'sitekey': '6LclBqwUAAAAAJ2KtS78FPoLPod26RXeKH5iddFy', //Replace this with your Site key
        'theme': 'light'
    });


};

$("#cccity").on("change", function () {
    var cccity = $(this).val();
    if (cccity == "others") {
        $("#OtherCity").show();

    } else {
        $("#OtherCity").hide();
    }

});


$("#drpstate").on("change", function () {
    var cstate = $(this).val();

    loadStateDrp('#contact_form #cccity');
});
function loadStateDrp(statedrp) {
    statedrp = $(statedrp);
    statedrp.html('');
    statedrp.append($('<option value>Select City</option>'));

    if ($("#contact_form #drpstate").val() == 'Guj') {

        statedrp.append($("<option />").val('Ahmedabad').text('Ahmedabad'));
        statedrp.append($("<option />").val('Vadodara').text('Vadodara'));
        statedrp.append($("<option />").val('Navsari').text('Navsari'));
        statedrp.append($("<option />").val('Surat').text('Surat'));
        statedrp.append($("<option />").val('Tapi').text('Tapi'));
        statedrp.append($("<option />").val('TheDangs').text('The Dangs'));
        statedrp.append($("<option />").val('Kheda').text('Kheda'));
        statedrp.append($("<option />").val('Morbi').text('Morbi'));
        statedrp.append($("<option />").val('Mahisagar').text('Mahisagar'));
        statedrp.append($("<option />").val('Porbandar').text('Porbandar'));
        statedrp.append($("<option />").val('Surendranagar').text('Surendranagar'));
        statedrp.append($("<option />").val('Barwala').text('Barwala'));
        statedrp.append($("<option />").val('Ranpur').text('Ranpur'));


    }
    if ($("#contact_form #drpstate").val() == 'Raj') {

        statedrp.append($("<option />").val('Udaipur').text('Udaipur'));
        statedrp.append($("<option />").val('Bhilwara').text('Bhilwara'));
        statedrp.append($("<option />").val('Chittorgarh').text('Chittorgarh'));
        statedrp.append($("<option />").val('Bundi').text('Bundi'));

    }
    if ($("#contact_form #drpstate").val() == 'Har') {

        statedrp.append($("<option />").val('Faridabad').text('Faridabad'));
        statedrp.append($("<option />").val('Palwal').text('Palwal'));
        statedrp.append($("<option />").val('Bhiwani').text('Bhiwani'));
        statedrp.append($("<option />").val('CharkhiDadri').text('Charkhi Dadri'));
        statedrp.append($("<option />").val('Mahendragarh').text('Mahendragarh'));
        statedrp.append($("<option />").val('Nuh').text('Nuh'));

    }
    if ($("#contact_form #drpstate").val() == 'Uttar') {

        statedrp.append($("<option />").val('Khurja').text('Khurja'));
        statedrp.append($("<option />").val('Jhansi').text('Jhansi'));
        statedrp.append($("<option />").val('Bhind').text('Bhind'));
        statedrp.append($("<option />").val('Jalaun').text('Jalaun'));
        statedrp.append($("<option />").val('Lalitpur').text('Lalitpur'));
        statedrp.append($("<option />").val('Datia').text('Datia'));

    }
    if ($("#contact_form #drpstate").val() == 'Chhattis') {

        statedrp.append($("<option />").val('Bilaspur').text('Bilaspur'));
        statedrp.append($("<option />").val('Korba').text('Korba'));


    }
    if ($("#contact_form #drpstate").val() == 'Karna') {

        statedrp.append($("<option />").val('Udupi').text('Udupi'));

    }
    if ($("#contact_form #drpstate").val() == 'Odi') {

        statedrp.append($("<option />").val('Balasore').text('Balasore'));
        statedrp.append($("<option />").val('Bhadrak').text('Bhadrak'));
        statedrp.append($("<option />").val('Mayurbhanj').text('Mayurbhanj'));

    }
    if ($("#contact_form #drpstate").val() == 'Tamil') {

        statedrp.append($("<option />").val('Cuddalore').text('Cuddalore'));
        statedrp.append($("<option />").val('Nagapatinam').text('Nagapatinam'));
        statedrp.append($("<option />").val('Tiruvarur').text('Tiruvarur'));
        statedrp.append($("<option />").val('Tiruppur ').text('Tiruppur '));

    }
    if ($("#contact_form #drpstate").val() == 'Madhya') {

        statedrp.append($("<option />").val('Anuppur').text('Anuppur'));
    }
    statedrp.append($('<option value="others">Others</option>'));

}

$("#btnContactUsSubmit").click(function () {
    var response = grecaptcha.getResponse(recaptcha2);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }

    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
    if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    if (!validateName(name)) { alert("Please enter valid name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var messagetype = $("#cmessageType").val();
    if (messagetype == "") { alert("Please specify your subject to the message"); $("#cmessageType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var statetype = $("#drpstate").val();
    if (statetype == "") { alert("Please specify your state"); $("#drpstate").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }


    var ccontactno = $("#ccontactno").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    if (!ccontactno.match('[0-9]{10}')) {
        alert("Please enter 10 digit mobile number.");
        $('#btnContactUsSubmit').removeAttr("disabled");
        return false;
    }
    if (ccontactno.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false;

    }
    var addresses = $("#address").val();
    if (addresses == "") { alert("Please enter any address"); $("##address").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    if (!validateMessage(addresses)) { alert("Please enter valid message"); $("#address").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var message = $("#cmessage").val();
    if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    if (!validateMessage(message)) { alert("Please enter valid message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var formtype = $("#cFormType").val();
    var pageinfo = window.location.href;
    var c = $("#cccity option:selected").text();

    if (c == "Others") {
        var OthersCity = $("#others").val();
        if (OthersCity == "") { alert("Please enter your City"); $("#others").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    }



    function validateName(sname) {
        var regex = /^[a-zA-Z ]+$/;

        if (regex.test(sname)) { return true; }
        else { return false; }
    }

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    function validateMessage(smessage) {
        var letterNumber = /[a-zA-Z0-9,. ]/;
        if (letterNumber.test(smessage)) { return true; }
        else { return false; }
    }


    var model = {

        Name: name,
        Mobile: ccontactno,
        Email: mailid

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        Name: name,
        Email: mailid,
        MessageType: $("#cmessageType option:selected").text(),
        Mobile: ccontactno,
        City: $("#cccity option:selected").val(),
        State: $("#drpstate option:selected").text(),
        Address: addresses,
        CustomerId: $("#customerid").val(),
        Message: message,
        OtherCity: OthersCity,

        FormType: formtype,
        PageInfo: pageinfo,
        reResponse: response,
        FormSubmitOn: currentdate,
        emailMessage: $("#emailMessage").val(),
        FromEmail: $("#cmessageType option:selected").val()


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Adanigas/InsertContactdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }
            else if (data.status == "2") {
                alert("OOPS! You have missed Captcha Validation. Kindly validate to proceed further.");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
            else if (data.FieldName != null && data.ErrorMessgae != null) {
                alert("Enter valid details");
                $('#btnContactUsSubmit').removeAttr("disabled");
                var Id = "#" + data.FieldName;
                $(Id).focus();
                return false;
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;

});


$("#btnContactUsSubmitmedia").click(function () {



    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
    if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    if (!validateName(name)) { alert("Please enter valid name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var ccontactno = $("#ccontactno").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    if (!ccontactno.match('[0-9]{10}')) {
        alert("Please enter 10 digit mobile number.");
        $('#btnContactUsSubmit').removeAttr("disabled");
        return false;
    }
    if (ccontactno.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false;

    }

    var message = $("#cmessage").val();
    if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    if (!validateMessage(message)) { alert("Please enter valid message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var response = grecaptcha.getResponse(recaptcha2);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }

    var formtype = $("#cFormType").val();
    var pageinfo = window.location.href;




    function validateName(sname) {
        var regex = /^[a-zA-Z ]+$/;

        if (regex.test(sname)) { return true; }
        else { return false; }
    }

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    function validateMessage(smessage) {
        var letterNumber = /[a-zA-Z0-9,. ]/;
        if (letterNumber.test(smessage)) { return true; }
        else { return false; }
    }

    var model = {

        Name: name,
        Mobile: ccontactno,
        Email: mailid

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        Name: name,
        Email: mailid,
        MessageType: 'Media',
        Mobile: ccontactno,
        Message: message,
        FormType: formtype,
        reResponse: response,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate,
        emailMessage: $("#emailMessage").val(),
        FromEmail: ''


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Adanigas/InsertMediadetail",
        contentType: "application/json",
        success: function (data) {

            if (data.status == "1") {
                window.location.href = "/thankyou";
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnContactUsSubmit').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;

});