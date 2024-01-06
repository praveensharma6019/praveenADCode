function sendToC() {
        var icount = 0;
        if ($("#names").val().trim() == "") {
            icount++;
            $('#meetName').addClass('has-error');
            $("#labelname").html("* Please enter your name");
            $('#labelname').css('color', 'red');
            $("#labelname").show();
        } else {
       
            var p = validateName($("#names").val());
            if (!p) {
                icount++;
                $('#meetName').addClass('has-error');
                $("#labelname").html("* Please enter alphabet characters only");
                $('#labelname').css('color', 'red');
                $("#labelname").show();
            } else {
                $('#meetName').removeClass('has-error');
                $("#labelname").hide();
            }
        }
        if ($("#mobile").val().trim() == "") {
            icount++;
            $('#meetMobile').addClass('has-error');
            $("#lblmobile").show();
            $("#lblmobile").html("* Please enter your contact no");
            $('#lblmobile').css('color', 'red');
        }
        else if ($("#mobile").val().length != 10) {
            icount++;
            $('#meetMobile').addClass('has-error');
            $("#lblmobile").show();
            $("#lblmobile").html("* Please enter 10 digit contact no");
            $('#lblmobile').css('color', 'red');
        }
        else {

            var c = IsMobileNumber(mobile);
            if (!c) {
                icount++;
                $('#meetMobile').addClass('has-error');
                $("#lblmobile").show();
                $("#lblmobile").html("* Please enter valid  contact no");
                $('#lblmobile').css('color', 'red');
            } else {
                $('#meetMobile').removeClass('has-error');
                $("#lblmobile").hide();
            }

        }
    if ($("#emails").val().trim() == "") {
        icount++;
		$('#meetEmail').addClass('has-error');
		$("#lblremail").html("* Please enter your email");
		$('#lblremail').css('color', 'red');
        $("#lblremail").show();
    } else {
        var e = IsValidEmail($("#emails").val());
        if (!e) {
            icount++;
			$('#meetEmail').addClass('has-error');
			$("#lblremail").html("* Please enter a valid email address");
			$('#lblremail').css('color', 'red');
            $("#lblremail").show();
        } else{
			$('#meetEmail').removeClass('has-error');
            $("#lblremail").hide();
			}
    }
  
    if ($("#company").val().trim() == "") {
        icount++;
        $('#meetCompany').addClass('has-error');
        $("#lblcompany").html("* Please enter your company name");
        $('#lblcompany').css('color', 'red');
        $("#lblcompany").show();
    } else{
        $('#meetCompany').removeClass('has-error');
        $("#lblcompany").hide();}
    if (icount > 0)
        return false;
    $.ajax({
        url: "Services/send.ashx",
        type: "GET",
        data: {
            sAction: "send",
            name: $("#names").val(),
            email: $("#emails").val(),
            mobile: $("#mobile").val(),
            company: $("#company").val(),

            sUrl: window.location.href,
            loader: function () {
                $("#adLoader2").show();
            }
        },
        success: function (result) {
            /*$("#sendingInfo").html("Your information sent successfully.");
            setTimeout(function() {
            $("#iLLoader").hide();
            }, 4000);*/
            $("#adLoader2").hide();
			$('#meetUs').modal('hide');
            $('#sentSuccess').modal('show');
            $("#names").val('');
            $("#emails").val('');
            $("#mobile").val('');
            $("#company").val('');
        },
        error: function (err) { }
    }); 
}

$('#meetUs').on('hidden.bs.modal', function () {
    clearValue();
})
function clearValue(){
	$("#names").val('');
	$("#emails").val('');
	$("#mobile").val('');
	$("#company").val('');
	
	$("#labelname").hide();
	$("#lblmobile").hide();
	$("#lblremail").hide();
	$("#lblcompany").hide();
	
	$('#meetName').removeClass('has-error');
	$('#meetMobile').removeClass('has-error');
	$('#meetEmail').removeClass('has-error');
	$('#meetCompany').removeClass('has-error');
	
	$("#adLoader2").hide();
}

//function IsValidEmail(email) {
//    var spliter = [];
//    if (email.toString().indexOf('@') >= 0) {
//        spliter = email.toString().split("@");
//        if (spliter.length > 2) {
//            return false;
//        } else {
//            if (spliter[0].toString() == "")
//                return false;
//            if (email.toString().indexOf('.') >= 0) {
//                spliter = spliter[1].toString().split('.');
//                if (spliter.length > 2)
//                    return false;
//                else {
//                    if (spliter[1].toString() == "")
//                        return false;
//                    else
//                        return true;
//                }
//            } else
//                return false;
//        }
//    } else
//        return false;
//}

function IsValidEmail(email) {
    var x = document.forms["myForm"]["email"].value;
    var atpos = x.indexOf("@");
    var dotpos = x.lastIndexOf(".");
    if (atpos<1 || dotpos<atpos+2 || dotpos+2>=x.length) {
       
        return false;
}

function IsMobileNumber(txtMobId) {
    
    var mob = /^[1-9]{1}[0-9]{9}$/;
    var txtMobile = document.getElementById(txtMobId);
 
    if (mob.test(mobile.value) == false) {
        return false;
    }
    return true;
}

function validateName(name) {
    if (/^[A-Za-z\s]+$/.exec(name)) {
        return true;
    }
    return false;
}

$(document).on('keypress', 'names', function (event) { var regex = new RegExp("^[a-zA-Z ]+$"); var key = String.fromCharCode(!event.charCode ? event.which : event.charCode); if (!regex.test(key)) { event.preventDefault(); return false; } });
