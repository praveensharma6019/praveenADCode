$(document).ready(function () {
    //date picker
    $('#Date_of_Arrival_B').attr('readonly', 'readonly');
    $('#Date_of_Departure_B').attr('readonly', 'readonly');
    $("#Date_of_Arrival_B").val(picker1.getMoment().format("DD-MM-YYYY"));
    $("#Date_of_Departure_B").val(picker2.getMoment().format("DD-MM-YYYY"));


});
var picker1 = new Pikaday({
    field: document.getElementById('Date_of_Arrival_B'),
    format: 'DD-MM-YYYY',
    minDate: moment().toDate()
});
var picker2 = new Pikaday({
    field: document.getElementById('Date_of_Departure_B'),
    format: 'DD-MM-YYYY',
    minDate: picker1.getMoment().format("DD-MM-YYYY")
});
$("#Date_of_Departure_B").bind("click", function () { picker2.setMinDate(new Date(picker1.getMoment().format())); })


var recaptcha1;
     
      var onloadCallback = function() {
       

		
		
		 //Render the recaptcha1 on the element with ID "recaptcha1"
        recaptcha1 = grecaptcha.render('recaptcha1', {
          'sitekey' : '6LcV8KwUAAAAAPqt7sILJKD5Z0nOd7AtNmnJciow', 
          'theme' : 'light'
        });
	  };

    $("#btnContactUsSubmit").click(function() {
	var response = grecaptcha.getResponse(recaptcha1);
	if(response.length == 0) 
	{
		alert("Captcha required.");
		return false;
	}
    
        $('#btnContactUsSubmit').attr("disabled", "disabled");
        var name = $("#cname").val();
        if (name == "") { alert("Please enter your Name"); $("#cname").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var mailid = $("#cmailid").val();
        if (mailid == "") { alert("Email is Required"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var ccontactno = $("#ccontactno").val();
        if (ccontactno == "") { alert("Please specify your Mobile Number"); $("#ccontactno").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var message = $("#cmessage").val();
        if (message == "") { alert("Please enter any message"); $("#cmessage").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
 var cmessageType = $("#cmessageType").val();
        if (cmessageType == "") { alert("Please select valid subject"); $("#cmessageType").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
              
	   var formtype = $("#cFormType").val();
        var pageinfo = window.location.href;
        if (!validateEmail(mailid)) { alert("Please enter valid email address"); $("#cmailid").focus(); $('#btnContactUsSubmit').removeAttr("disabled"); return false; }
        var mobile = "1010101010";


        function validateEmail(mailid) {
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (filter.test(mailid)) { return true; }
            else { return false; }
        }

        var model = {

            Name: name,
            Mobile: mobile,
            Email: mailid

        };

        var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

        //create json object
        var savecustomdata = {


            Name: name,
            Email: mailid,
            MessageType: cmessageType,
            Mobile: ccontactno,
            Message: message,
            FormType: formtype,
            PageInfo: pageinfo,
            FormSubmitOn: currentdate


        };

        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Bunkering/InsertContactdetail",
            contentType: "application/json",
            success: function (data) {
                //////////////

                if (data.status == "1") {
                    window.location.href = "/thankyou";
                    //$('#contact_form1').submit();
                }
                else {
                    alert("Sorry Operation Failed!!! Please try again later");
                    $('#btnContactUsSubmit').removeAttr("disabled");
                    return false;
                }
            }
        });
        return false;


        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: "/api/Bunkering/CreateOTP",
            contentType: "application/json",
            success: function (data) {
                if (data.status == "1") {
                    var otp = prompt("Please enter OTP received on your mobile", "");

                    if (otp != null) {

                        var generatedOtp = {
                            mobile: mobile,
                            OTP: otp,
                        }
                        $.ajax({
                            type: "POST",
                            data: JSON.stringify(generatedOtp),
                            url: "/api/Bunkering/VerifyOTP",
                            contentType: "application/json",
                            success: function (data) {
                                if (data.status == "1") {


                                }

                                else {
                                    alert("Invalid OTP");
                                    $('#btnContactUsSubmit').removeAttr("disabled");
                                    return false;
                                }
                            }
                        });

                    }
                }
                else if (data == "-1") {
                    alert("Invalid Mobile Number");
                    $('#btnContactUsSubmit').removeAttr("disabled");
                }
            }
        });

        return false;
    });
    
