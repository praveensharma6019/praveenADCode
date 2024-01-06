
function setHref(fileName) {

    document.getElementById("myModalSubmit").href = "javascript:download('" + fileName + "')";
    return false;

}


function download(fileName) {
    
    var icount = 0;
    var name = $("#name").val();
    var email = $("#email").val();
    var phone = $("#phone-num").val();
    var company = $("#company").val();

	if ($("#name").val().trim() == "") 
	{
		icount++;
		$('#name').addClass('has-error');
		$("#lblname").html("* Please enter your name");
		$("#lblname").show();
	}
	else
	{
		$('#name').removeClass('has-error');
		$("#lblname").hide();
	}
	
	if ($("#email").val().trim() == "")
	{
		icount++;
		$('#email').addClass('has-error');
		$("#lblemail").html("* Please enter your email");
		$("#lblemail").show();
	}
	else
	{
		var e = IsValidEmail($("#email").val());
		if (!e)
		{
			icount++;
			$('#email').addClass('has-error');
			$("#lblemail").html("* Please enter a valid email address");
			$("#lblemail").show();
		}
		else
		{
			$('#email').removeClass('has-error');
			$("#lblemail").hide();
		}
	}

    if (icount > 0)
    {
        
	}
    else 
    {
        window.location = "Services/downloadFile.ashx?fileName=" + fileName + "&name=" + name + "&email=" + email + "&phone=" + phone + "&company=" + company;        
        $("#myModal").modal('toggle');
        $("#name").val("");
        $("#email").val("");
	}
}

function IsValidEmail(email) {
    var spliter = [];
    if (email.toString().indexOf('@') >= 0) {
        spliter = email.toString().split("@");
        if (spliter.length > 2) {
            return false;
        } else {
            if (spliter[0].toString() == "")
                return false;
            if (email.toString().indexOf('.') >= 0) {
                spliter = spliter[1].toString().split('.');
                if (spliter.length > 2)
                    return false;
                else {
                    if (spliter[1].toString() == "")
                        return false;
                    else
                        return true;
                }
            } else
                return false;
        }
    } else
        return false;
}
