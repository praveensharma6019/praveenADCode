var regExp = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/; // reg exp to check email address

/* *********************************************************************** */

$(document).ready(function() {
	$("input#username").focus(); 
	$("#err_username").hide();
	$("#err_password").hide();
	$('#loginresponse').hide();

	$("#submit_btn").click(function() {
		var username = $("#username").val();
		var password = $("#password").val();
		
		if(username=="" || username==null){
			//$("#err_username").show(); 
			$("#err_username").fadeTo(200,0.1,function() {
				$(this).html('Proper username is required!').fadeTo(200,1);
				$("input#username").focus(); 
				return false;
			});
		}
		else if(password=="" || password==null){ 
			//$("#err_password").show(); 
			$("#err_password").fadeTo(200,0.1,function() {
				$(this).html('Valid password is required!').fadeTo(200,1);
				$("input#log_password").focus(); 
				return false;
			});
		}
		else{
            //console.log(username + " | " + password);
			/* ******************/
			$('#loginresponse').show();
			$("#submit_btn").attr("disabled", "disabled");
			$.post("admlogin_exec.php", { 
				username: username,
				password: password
			}, 
			function(data) {
                /*$("#loginresponse").fadeTo(200,0.1,function() {
                    $(this).html(data).fadeTo(900,1);
                    $("#submit_btn").removeAttr("disabled");
                });*/
				if(data=='RMNOM') {
					$("#loginresponse").fadeTo(200,0.1,function() {
						$(this).html('Username and password does not match!').fadeTo(900,1);
						$("#submit_btn").removeAttr("disabled");
					});
					//setTimeout(function(){ $("#rmloginresponse").html("",""); }, 10000);
                    setTimeout(function(){ $("#username").val("",""); $("#password").val("",""); $("#loginresponse").html("",""); }, 10000);
				}
				else if(data=='RMBLK') {
					$("#loginresponse").fadeTo(200,0.1,function() {
						$(this).html('Your account has been disabled. Contact Admin.').fadeTo(900,1);
						$("#submit_btn").removeAttr("disabled");
					});                    
                    setTimeout(function(){ $("#username").val("",""); $("#password").val("",""); $("#loginresponse").html("",""); }, 10000);
				}
				else{
					$("#loginresponse").fadeTo(0,0.2,function(){  
					   $(this).html('').fadeTo(900,1,
						function(){
							 setTimeout(function(){document.location='check.php';}, 500);
						});
					});
				}
			});
			/********************/
			//alert('Form Submitted');
			//return false;
		}
/* **************************************************** */
		return false;
	}); // #alertsubmit on click function ends.
}); // document ready function ends.

/* *********************************************************************** */

function CloseErrorDisp(ctrl){
	var chk = document.getElementById(ctrl);
	if(chk.innerHTML!=""){
		chk.innerHTML="";
		setTimeout("document.getElementById('"+ctrl+"').style.display ='none';",500);
	}
}