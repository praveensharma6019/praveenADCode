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



$("#btnContactUsSubmit").click(function () {
    var response = grecaptcha.getResponse(recaptcha2);
    //if (response.length == 0) {
    //    alert("Captcha required.");
    //    return false;
    //}

    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
    if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
	 if (!validateName(name)) { alert("Please enter valid name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
    var mailid = $("#cmailid").val();
    if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
	
	if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
	
    var messagetype = $("#cmessageType").val();
    if (messagetype == "") { alert("Please specify your subject to the message"); $("#cmessageType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
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

function validateMessage(smessage)
{
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
        Message: message,
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
    //if (response.length == 0) {
    //    alert("Captcha required.");
    //    return false;
    //}

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

function validateMessage(smessage)
{
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
        FromEmail:''


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
                //$('#contact_form1').submit();
            }
            else if (data.status == "2") {
                alert("OOPS! You have missed Captcha Validation. Kindly validate to proceed further.");
                $('#btnContactUsSubmit').removeAttr("disabled");
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