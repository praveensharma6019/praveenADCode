





$("#btnContactUsSubmit").click(function () {
  var response = grecaptcha.getResponse(recaptcha1);
 
    $('#btnContactUsSubmit').attr("disabled", "disabled");
    var name = $("#cname").val();
  
	var mailid = $("#cmailid").val();
  
   

    var message = $("#cmessage").val();
    
    var messagetype = $("#cmessageType").val();
    


    var formtype = $("#cFormType").val();
    var pageinfo = window.location.href;
    var model = {

        Name: name,
      
        Email: mailid

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        Name: name,
        Email: mailid,
       
        Message: message,
       
        MessageType: messagetype,
        FormType: formtype,
        PageInfo: pageinfo,
        FormSubmitOn: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/AREPRL/InsertContact",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }

            else if (data.status == "4") {
                alert("Please provide proper input values.");
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

