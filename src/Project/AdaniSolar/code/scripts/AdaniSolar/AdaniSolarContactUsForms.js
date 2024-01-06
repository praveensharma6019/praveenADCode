var recaptcha2;
var onloadCallback = function () {

    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha2', {
        'sitekey': '6Lfm5bMUAAAAABxHKwXkt5xkc7hHDR_m_fYTgKc-', //Replace this with your Site key //6Le7Ma0UAAAAAHuZ5Li5kM5StUbTIDEOAeabw1Gc
        'theme': 'light'
    });
};
var myCallBack = function () {
    //Render the recaptcha2 on the element with ID "recaptcha2"
    recaptcha1 = grecaptcha.render('recaptcha2', {
        'sitekey': '6Lfm5bMUAAAAABxHKwXkt5xkc7hHDR_m_fYTgKc-', //Replace this with your Site key
        'theme': 'light'
    });
};


$("#btnContactUsSubmit").click(function () {
    var response = grecaptcha.getResponse(recaptcha2);
    if (response.length == 0) {
        alert("Captcha required.");
        return false;
    }

    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
    if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var messagetype = $("#cmessageType").val();
    if (messagetype == "Default") { alert("Please specify your subject to the message"); $("#cmessageType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var projectcategory = $("#cprojectType").val();
    if (projectcategory == "Default") { alert("Please select project type"); $("#cprojectType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var region = $("#region").val();
    if (region == "Select") { alert("Please specify Region"); $("#region").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var State = $("#State").val();
    if (State == "Default") { alert("Please specify your subject to the message"); $("#State").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var ccontactno = $("#ccontactno").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    if (ccontactno.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false;
    }

    if (!validatemobile(ccontactno)) { alert("Please enter valid mobile number"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var countrycode = $("#countrycode").val();
    if (countrycode == "Select") { alert("Please specify your country"); $("#countrycode").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var message = $("#cmessage").val();
    if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var formtype = $("#cFormType").val();

 
    var Category = $("#cprojectType").val();
 

    var pageinfo = window.location.href;

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    function validatemobile(mob) {
        var filter = /^[0-9-+]+$/;
        if (filter.test(mob)) { return true; }
        else { return false; }
    }
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    var ProjectCategory = $("#cmessageType option:selected").text();
    var Category = $("#cprojectType option:selected").text();
    var state1 = $("#State option:selected").text(); 

    var savecustomdata = {

        Name: name,
        Email: mailid,
        Category: messagetype,
        Region: region,
        ProjectCategory: projectcategory,
        State: State,
        Mobile: ccontactno,
        Message: message,
        FormType: formtype,
        Countrycode: countrycode,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate,
        emailMessage: $("#emailMessage").val(),
        Response: response,
        topic: $("#cmessageType option:selected").text(),
        SubjectOfMessageText: $('#cmessageType option:selected').text(),
        CategoryText: $('#cprojectType option:selected').text(),
        StateText: $('#State option:selected').text()
    };


    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniSolar/SendMailContactUs",
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
        },
        error: function (data) {
            alert('error');
        }
    });
    return false;

});


$("#btnAskOurExpert").click(function () {

    $('#btnAskOurExpert').attr("disabled", "disabled");
    var name = $("#cname").val();
    if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }

    var ccontactno = $("#ccontactno").val();
    if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }

    if (ccontactno.length != 10) {
        alert("Contact Number should be of 10 digit"); $("#ccontactno").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false;
    }
    if (!validatemobile(ccontactno)) { alert("Please enter valid mobile number"); $("#ccontactno").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }

    var category = $("#CategoryAskExpert").val();
    if (category == "") { alert("Please specify Category"); $("#CategoryAskExpert").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }

    var projectcategory = $("#cprojectType").val();
    if (projectcategory == "Default") { alert("Please specify your subject to the message"); $("#cprojectType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var region = $("#region").val();
    if (region == "Select") { alert("Please specify Region"); $("#region").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }

    var State = $("#State").val();
    if (State == "Default") { alert("Please specify your subject to the message"); $("#State").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }

    var message = $("#cmessage").val();
    if (message == "") { alert("Please enter message"); $("#cmessage").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }

    var formtype = $("#cFormType").val();

    var pageinfo = window.location.href;
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    function validatemobile(mob) {
        var filter = /^[0-9-+]+$/;
        if (filter.test(mob)) { return true; }
        else { return false; }
    }
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    //create json object
    var savecustomdata = {
        Name: name,
        Email: mailid,
        Mobile: ccontactno,
        Category: category,
        Region: region,
        ProjectCategory: projectcategory,
        State: State,
        Message: message,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate,
        emailMessage: $("#emailMessage").val(),
        topic: $("#CategoryAskExpert option:selected").text()
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniSolar/SendMailAskExpert",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                window.location.href = "/thankyou";
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnAskOurExpert').removeAttr("disabled");
                return false;
            }
        },
        error: function (xhr, e, r) {
            //alert('Error while selecting list..!!');
        }
    });
    return false;
});

$("#btnSubscribe").click(function () {

    $('#btnSubscribe').attr("disabled", "disabled");
    var formtype = $("#cFormTypeSub").val();
    var pageinfo = $("#PageInfoSub").val();
    var mailid = $("#cmailidsub").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailidsub").focus(); $('#btnAskOurExpert').removeAttr("disabled"); return false; }
    if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailidsub").focus(); $('#btnSubscribe').removeAttr("disabled"); return false; }

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');
    //create json object
    var savecustomdata = {
        Email: mailid,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate,
        emailMessage: $("#emailMessageSub").val()
    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AdaniSolar/SendMailSubscribe",
        contentType: "application/json",
        success: function (data) {
            if (data.status == "1") {
                window.location.href = "/thankyou";
            }
            else {
                alert("Sorry Operation Failed!!! Please try again later");
                $('#btnAskOurExpert').removeAttr("disabled");
                return false;
            }
        }
    });
    return false;
});

$("#region").change(function () {
    loadStateDrp('#State');
});

function loadStateDrp(statedrp) {
    $(statedrp).text('');
    $(statedrp).append('<option value="Default">Select State</option>');
    statedrp = $(statedrp);
    if ($("#region").val() == 'india-east') {        
        statedrp.append($("<option />").val('INAR').text('Arunachal Pradesh'));
        statedrp.append($("<option />").val('INAS').text('Assam'));    
        statedrp.append($("<option />").val('INMN').text('Manipur'));
        statedrp.append($("<option />").val('INML').text('Meghalaya'));
        statedrp.append($("<option />").val('INMZ').text('Mizoram'));
        statedrp.append($("<option />").val('INNL').text('Nagaland'));
        statedrp.append($("<option />").val('INOR').text('Odisha'));      
        statedrp.append($("<option />").val('INSK').text('Sikkim'));
        statedrp.append($("<option />").val('INTR').text('Tripura'));
        statedrp.append($("<option />").val('INWB').text('West Bengal'));
    }
    else if ($("#region").val() == 'india-north') {
        statedrp.append($("<option />").val('INBR').text('Bihar'));
        statedrp.append($("<option />").val('INCH').text('Chandigarh'));
        statedrp.append($("<option />").val('INDL').text('Delhi'));
        statedrp.append($("<option />").val('INHR').text('Haryana'));
        statedrp.append($("<option />").val('INHP').text('Himachal Pradesh'));
        statedrp.append($("<option />").val('INJK').text('Jammu and Kashmir'));
        statedrp.append($("<option />").val('INJH').text('Jharkhand'));
        statedrp.append($("<option />").val('INLA').text('Ladakh'));
        statedrp.append($("<option />").val('INPB').text('Punjab'));
        statedrp.append($("<option />").val('INRJ').text('Rajasthan'));
        statedrp.append($("<option />").val('INUT').text('Uttarakhand'));
        statedrp.append($("<option />").val('INUP').text('Uttar Pradesh'));
    }
    else if ($("#region").val() == 'india-South') {
        statedrp.append($("<option />").val('INAN').text('Andaman and Nicobar Islands'));
        statedrp.append($("<option />").val('INAP').text('Andhra Pradesh'));
        statedrp.append($("<option />").val('INKA').text('Karnataka'));
        statedrp.append($("<option />").val('INKL').text('Kerala'));
        statedrp.append($("<option />").val('INLD').text('Lakshadweep'));
        statedrp.append($("<option />").val('INPY').text('Puducherry'));
        statedrp.append($("<option />").val('INTN').text('Tamil Nadu'));
        statedrp.append($("<option />").val('INTS').text('Telangana'));
    }
    else if ($("#region").val() == 'india-west') {
        statedrp.append($("<option />").val('INCT').text('Chhattisgarh'));
        statedrp.append($("<option />").val('INDN').text('Dadra and Nagar Haveli'));
        statedrp.append($("<option />").val('INDD').text('Daman and Diu'));
        statedrp.append($("<option />").val('INGA').text('Goa'));
        statedrp.append($("<option />").val('INGJ').text('Gujarat'));
        statedrp.append($("<option />").val('INMP').text('Madhya Pradesh'));
        statedrp.append($("<option />").val('INMH').text('Maharashtra'));
    }
    else {
        statedrp.append($("<option />").val('OTHERS').text('Others'));
    }
}
//$(document).ready(function () {

//    $.ajax({
//        type: "POST",
//        //data: JSON.stringify(savecustomdata),
//        url: "/api/AdaniSolar/GetRegion_Territories",
//        contentType: "application/json",
//        success: function (data) {
//            if (data.length > 0) {
//                $("#region").empty();
//                $('#region').append('<option value="Select">Select Region</option> ');
//                $.each(data, function (i, region) {
//                    $('#region').append('<option value=' + region.name + '>' + region.name + '</option > ');
//                });
//            }
//        }
//    });

//    $.ajax({
//        type: "POST",
//        //data: JSON.stringify(savecustomdata),
//        url: "/api/AdaniSolar/GetRegion_Countries",
//        contentType: "application/json",
//        success: function (data) {
//            if (data.length > 0) {
//                $("#countrycode").empty();
//                $('#countrycode').append('<option value="Select">Select Country</option> ');
//                $.each(data, function (i, country) {
//                    $('#countrycode').append('<option value=' + country.ispl_name + '>' + country.ispl_name + '</option > ');
//                });
//            }
//        }
//    });
//});