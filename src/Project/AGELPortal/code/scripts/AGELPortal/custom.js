function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}
function SendOtpMobile(event, t) {
    var self = $(t);
    var email = $(t).val();




    // var errorclass = $(t).attr("data-class");
    if (validateEmail(email) === true) {
        var html = $("#portal-register").html();
        $("#portal-register").html("Please wait for otp");
        $("#portal-register").attr("disabled", "disabled");

        $.ajax({
            type: 'POST',
            data: { mobile: $("#mobile").val(), email: email },
            url: "api/AGELPortal/SendOtp",
            success: function (data) {
                if (data.status === "1") {
                    $("#portal-register").html(html);
                    $("#portal-register").removeAttr("disabled", "disabled");
                    // $("#OtpDiv").show();
                    $("#Otperror").hide();
                } else {
                    $("#Otperror").show();
                    //$("#OtpDiv").hide();
                }

            },

            error: function (data) {
                $("emailerror").html("Some technical issue please after some time.");
            }
        });


    }
    else {
        $("emailerror").html("Please enter a valid Email.");
    }

}
function AGELSendOtpForValidation(event, t) {
	var Uname= $("#hdn_name").val();
    $.ajax({
        type: 'POST',
        data: { mobile: $("#mobile").val(), name: $("#hdn_name").val()},
        url: "/api/AGELPortal/AGELSendOtpForValidation",
        success: function (data) {
            if (data.status === "1") {
                $('#portal-register').hide();
                //$("#portal-register").html(html);
                //$("#portal-register").removeAttr("disabled", "disabled");
                $("#OtpSuccess").show();
                $("#Otperror").hide();
            } else {
                $("#Otperror").show();
                $("#OtpSuccess").hide();
            }

        },
    });
}

$(document).ready(function (e) {

    var vids = $(".azuremediaplayer");
    $.each(vids, function () {
        this.controls = false;
    });
    $(".azuremediaplayer").click(function () {
        //console.log(this); 
        this.pause();
        window.location.href = "/agelportal/home/";
    });
    //var event = $.Event("click");
    //if (mType == "addCat") {
    //    $("a#add_category").trigger(event);
    //}
    //if (mType == "editCat") {
    //    $("a#add_category").trigger(event);
    //}

    $(".edit_category").click(function () {
        var id = $(this).attr("data-id");
        $.ajax({
            method: "POST",
            data: { Id: id },
            url: "/api/AGELPortal/GetCategory",
            success: function (data) {
                if (data.status === "1") {

                    $("#editname").val(data.Namecat);
                    $("#editname").focus();
                    $("#insertId").val(id);
                    if (data.CatStatus === "1" || data.CatStatus === true) {
                        $("#catStatus1").attr('checked', 'checked');

                    } else {
                        $("#catStatus0").attr('checked', 'checked');
                    }
                }

            },

            error: function (data) {
                $("emailerror").html("Some technical issue please after some time.");
            }
        });
    });


    $(".change-status").click(function () {
        var id = $(this).attr("data-id");
        var self = $(this);
        $.ajax({
            type: 'POST',
            data: { Id: id },
            url: "/api/AGELPortal/AGELPortalChangeUserStatus",
            success: function (data) {
                if (data.status === "1") 
	        {

                    self.html("<h3>" + data.txt + "</h3>");
		    if (data.txt == "Active") 
		    {
                        self.parents("#DivStatus").addClass("status");
                        self.parents("#DivStatus").removeClass("status2");
                    }
                    else 
		    {
                        self.parents("#DivStatus").removeClass("status");
                       self.parents("#DivStatus").addClass("status2");
                    }		

                }
		else
		{
		  $("#DivStatus").addClass("status"); 
		}	

            },

            error: function (data) {

            }
        });
    });


    $(".change-catagory-status").click(function () {
        var id = $(this).attr("data-id");
        var self = $(this);
        $.ajax({
            type: 'POST',
            data: { Id: id },
            url: "/api/AGELPortal/AGELPortalChangeCategoryStatus",
            success: function (data) {

                if (data.status === "1") {

                    self.html("<h3>" + data.txt + "</h3>");
                    if (data.txt == "Active") {
                        self.parents("#DivStatus").addClass("status");
                        self.parents("#DivStatus").removeClass("status2");
                    }
                    else {
                        self.parents("#DivStatus").removeClass("status");
                        self.parents("#DivStatus").addClass("status2");
                    }
                }
            },

            error: function (data) {

            }
        });
    });

    $(".change-content-status").click(function () {
        var id = $(this).attr("data-id");
        var self = $(this);
        $.ajax({
            type: 'POST',
            data: { Id: id },
            url: "/api/AGELPortal/AGELPortalChangeContentStatus",
            success: function (data) {
                if (data.status === "1") {

                    self.html("<h3>" + data.txt + "</h3>");
                    if (data.txt == "Published") {
                        self.parents("#DivStatus").addClass("status");
                        self.parents("#DivStatus").removeClass("status2");
                    }
                    else {
                        self.parents("#DivStatus").removeClass("status");
                        self.parents("#DivStatus").addClass("status2");
                    }
                }

            },

            error: function (data) {

            }
        });
    });


    /*$(".delete_category").click(function () {
        var id = $(this).attr("data-id");
       
        $.ajax({
            type: 'POST',
            data: { Id: id },
            url: "/api/AGELPortal/DeleteCategory",
            success: function (data) {
                if (data.status === "1") {
                    $("#tr-" + id).remove();
                    
                }

            },

            error: function (data) {
                $("emailerror").html("Some technical issue please after some time");
            }
        });
    });*/

    $(".delete_content").click(function () {


        if (confirm('Are you sure you want to delete this video?')) {
            var id = $(this).attr("data-id");
            $.ajax({
                type: 'POST',
                data: { Id: id },
                url: "/api/AGELPortal/DeleteContent",
                success: function (data) {
                    if (data.status === "1") {
                        $("#tr-" + id).remove();
                        window.location.reload();
                    }
                    else {
                        window.location.reload();

                    }

                },

                error: function (data) {
                    $("emailerror").html("Some technical issue please after some time.");
                }
            });
        } else {
            return false;
        }

    });
    $(".delete_category").click(function () {

        if (confirm('Are you sure you want to delete this category?')) {
            var id = $(this).attr("data-id");

            $.ajax({
                type: 'POST',
                data: { Id: id },
                url: "/api/AGELPortal/DeleteCategory",
                success: function (data) {
                    if (data.status === "1") {
                        $("#tr-" + id).remove();
                        window.location.reload();
                    }
                    else {
                        window.location.reload();

                    }

                },

                error: function (data) {
                    $("emailerror").html("Some technical issue please after some time.");
                }
            });
        } else {
            return false;
        }

    });

    $(".delete_user").click(function () {

        if (confirm('Are you sure you want to delete this User?')) {
            var id = $(this).attr("data-id");

            $.ajax({
                type: 'POST',
                data: { UserId: id },
                url: "/api/AGELPortal/DeleteUser",
                success: function (data) {
                    if (data.status === "1") {
                        $("#tr-" + id).remove();
                        window.location.reload();



                    }
                    else {
                        window.location.reload();

                    }

                },

                error: function (data) {
                    $("emailerror").html("Some technical issue please after some time");
                }
            });
        } else {
            return false;
        }

    });

    $('#uploadUserBulk').click(function () {

        // Checking whether FormData is available in browser  
        $('#uploadUserBulk').html('');
        $('#uploadUserBulk').addClass('button--loading');

        var allowedExtensions = "csv";
        if (window.FormData != undefined) {

            var fileUpload = $("#agel_file").get(0);
            var filePath = $("#agel_file").val();
            var file_extension = filePath.split('.').pop();
            var files = fileUpload.files;
            if (files.length > 0 && allowedExtensions == file_extension) {
                // Create FormData object  
                var fileData = new FormData();

                // Looping over all files and add it to FormData object  

                fileData.append(files[0].name, files[0]);




                $.ajax({
                    url: '/api/AGELPortal/AGELPortalUploadBulkUser',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {

                        if (result.status == 0)
                            $(".erro_csv_upload").html('<p style="color:red;">' + result.message + '</p>');
                        //$(".erro_csv_upload").show();
                        if (result.status == 1)
                            $(".erro_csv_upload").html('<p style="color:green;">' + result.message + '</p>');
                        //$(".erro_csv_upload").show();
                    },
                    error: function (err) {
                        $(".erro_csv_upload").html('<p style="color:red;">' + err.message + '</p>');
                    }
                });
            } else {
                $(".erro_csv_upload").html('<p style="color:red;">Please select CSV file to upload.</p>');
                //$(".erro_csv_upload").show();
            }
        } else {

            $(".erro_csv_upload").html('<p style="color:red;">FormData is not supported.</p>');
            //$(".erro_csv_upload").show();
        }
        setTimeout(
            function () {
                $('#uploadUserBulk').removeClass('button--loading');
                $('#uploadUserBulk').html('Upload');
            }, 1000);

    });
	
	$("#searchKeyword").keypress(function (e) {
        var key = e.which;
        if (key == 13)  // the enter key code
        {
			var searchKeyword = document.getElementById('searchKeyword').value;
var userTypeValue = document.getElementById('user_type').value;
var siteValue = document.getElementById('Site').value;
if (searchKeyword !== undefined && searchKeyword !== "") 
{
	if(userTypeValue !== undefined && userTypeValue !== "")
	{
		if(siteValue !== undefined && siteValue !== "")
		{
			window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&Site=" + siteValue + "&searchKeyword=" + searchKeyword;
		}
		else
		{
			window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&searchKeyword=" + searchKeyword;
		}
		
	}
	else if(siteValue !== undefined && siteValue !== "")
	{
		if(userTypeValue !== undefined && userTypeValue !== "")
		{
			window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&Site=" + siteValue + "&searchKeyword=" + searchKeyword;
		}
		else
		{
			 window.location.href = "/AGELPortal/Home/dashboard/user_managment?Site=" + siteValue + "&searchKeyword=" + searchKeyword;
		}
	}
	else
	{
		window.location.href = "/AGELPortal/Home/dashboard/user_managment?searchKeyword=" + searchKeyword;
	}	
}
        }
    });
	
    $('#user_type , #Site').change(function (e) {
        var userTypeValue = document.getElementById('user_type').value;
        var siteValue = document.getElementById('Site').value;
        var searchKeyword = document.getElementById('searchKeyword').value;
		
        if (userTypeValue !== undefined && siteValue !== undefined && searchKeyword !== undefined) {
            window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&Site=" + siteValue + "&searchKeyword=" + searchKeyword;
        }
        else if (siteValue !== undefined && siteValue !== "") {
            window.location.href = "/AGELPortal/Home/dashboard/user_managment?Site=" + siteValue;
        }
        else if (userTypeValue !== undefined && userTypeValue !== "") {
            window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue;
        }
        else if (siteValue !== undefined && siteValue !== "" && searchKeyword !== undefined && searchKeyword !== "") {
            window.location.href = "/AGELPortal/Home/dashboard/user_managment?Site=" + siteValue + "&searchKeyword=" + searchKeyword;
        }
        else if (userTypeValue !== undefined && userTypeValue !== "" && searchKeyword !== undefined && searchKeyword !== "") {
            window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&searchKeyword=" + searchKeyword;
        }
		
    });
});

// $("#searchKeyword").focusout(function (e) {
	// var searchKeyword = document.getElementById('searchKeyword').value;
	 // if (searchKeyword !== undefined && searchKeyword !== "") {
            // window.location.href = "/AGELPortal/Home/dashboard/user_managment?searchKeyword=" + searchKeyword;
        // }
// });

$(".searchuser").click(function (e) 
{
var searchKeyword = document.getElementById('searchKeyword').value;
var userTypeValue = document.getElementById('user_type').value;
var siteValue = document.getElementById('Site').value;
if (searchKeyword !== undefined && searchKeyword !== "") 
{
	if(userTypeValue !== undefined && userTypeValue !== "")
	{
		if(siteValue !== undefined && siteValue !== "")
		{
			window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&Site=" + siteValue + "&searchKeyword=" + searchKeyword;
		}
		else
		{
			window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&searchKeyword=" + searchKeyword;
		}
		
	}
	else if(siteValue !== undefined && siteValue !== "")
	{
		if(userTypeValue !== undefined && userTypeValue !== "")
		{
			window.location.href = "/AGELPortal/Home/dashboard/user_managment?user_type=" + userTypeValue + "&Site=" + siteValue + "&searchKeyword=" + searchKeyword;
		}
		else
		{
			 window.location.href = "/AGELPortal/Home/dashboard/user_managment?Site=" + siteValue + "&searchKeyword=" + searchKeyword;
		}
	}
	else
	{
		window.location.href = "/AGELPortal/Home/dashboard/user_managment?searchKeyword=" + searchKeyword;
	}	
}
});

function redirect() {
    var searchKeyword = document.getElementById('searchKeyword').value;
    if (window.location.href.includes("?")) {
        window.location.href + "&searchKeyword=" + searchKeyword;
    } else {
        window.location.href + "?searchKeyword=" + searchKeyword;
    }

}

var password = document.getElementById("password")
    , confirm_password = document.getElementById("confirm_password");

function validatePassword() {
    if (password.value !== confirm_password.value) {
        confirm_password.setCustomValidity("Passwords Don't Match");
    } else {
        confirm_password.setCustomValidity('');
    }
}
$("#content_type").change(function () {
    if ($("#content_type").val() == "document") {
        $('#sizelable').text("Enter number of pages");
       
    } 
	 else if ($("#content_type").val() == "video") {
        $('#sizelable').text("Enter video length in minutes & seconds (mm:ss)");
       
    } 
	else if($("#content_type").val() == ""){
        $('#sizelable').text("Please select content type");
    }
});
//password.onchange = validatePassword;
//confirm_password.onkeyup = validatePassword;

$(function () {                       
    $('.modal-toast .modal-close').click(function () { 
        $('.modal-toast').hide();     
    });
    setTimeout(function () {
        $('.modal-toast').hide();     
    }, 6000);
    $('.modal-close').click(function () {
        $(".erro_csv_upload").html('');
        $("#empList").val('');
    });
});



    $('#tab a').click(function (e) {
        e.preventDefault();
    $("#tab a").removeClass('active brand-gradient-parent brand-gradient-left brand-gradient-thin outlined');
    $(this).addClass('active brand-gradient-parent brand-gradient-left brand-gradient-thin outlined');
});





window.onload = function () {
    //document.getElementById('closeModal').click();
    var x = document.getElementById("mobile");
	if (x != undefined || x != null)
    {
	if (x.value == "" || x.value == null)
    {
        document.getElementById("portal-register").disabled = true; 
    } 
}	
};

$("#mobile").focusout(function (e) {
    if ($("#mobile").val().length == 0 || $("#mobile").val() == "") {
        document.getElementById("portal-register").disabled = true;
    }
    else if (!/^\d*(?:\.\d{1,2})?$/.test($("#mobile").val())) {
        document.getElementById("portal-register").disabled = true;
    }
    else if ($("#mobile").val().length != 10) {
        document.getElementById("portal-register").disabled = true;
    }
    else
    {
        document.getElementById("portal-register").disabled = false;
    }
}); 

$("#mobile").keyup(function (e) {
    if ($("#mobile").val().length == 0 || $("#mobile").val() == "") {
        document.getElementById("portal-register").disabled = true;
    }
    else if (!/^\d*(?:\.\d{1,2})?$/.test($("#mobile").val())) {
        document.getElementById("portal-register").disabled = true;
    }
    else if ($("#mobile").val().length != 10) {
        document.getElementById("portal-register").disabled = true;
    }
    else {
        document.getElementById("portal-register").disabled = false;
    }
});


$(document).ready(function (e) {

var path = window.location.pathname;
var page = path.split("/").pop();
if(page == "admin_Rese_Password")
{
	document.getElementById("password").classList.add('active');
}

if(page == "categories")
{
	document.getElementById("category").classList.add('active');
}

if(page == "user_managment" || page == "add-user" || page == "edit-user")
{
	document.getElementById("usermanagment").classList.add('active');
	
}

    if (page == "add-user" || page == "edit-user")
{
	var value = $( 'input[name=user_type]:checked' ).val();
	var input = document.getElementById('vendor');
	
	if(value == "2")
	{
		input.setAttribute('required', '');
		$('#vendor').css("display", "block");
		$('#vendorName').css("display", "block");
	}
	else
	{
		input.removeAttribute('required', '');
		$('#vendor').css("display", "none");
		$('#vendorName').css("display", "none");
		
	}
}

if(page == "content-managements" || page == "add-content")
{
	document.getElementById("content").classList.add('active');
}


if(page == "change_mobile")
{
	document.getElementById("my-profile").classList.add('active');
}

if(page == "dashboard")
{
	document.getElementById("dashboard").classList.add('active');
}

$(".cross-icon").mousedown(function (e) {  
   document.getElementById("searchKeyword").value = ""; 
   document.getElementById("searchLabel").classList.remove('active');
   window.location.href = "/AGELPortal/Home/dashboard/user_managment";
}); 

// $("#searchKeyword").focusin(function (e) {  
    // //$("#search-cross-icon").prop("display","block");
	
   // var crossIcon = document.getElementById("search-cross-icon"); 
   // crossIcon.style.display="block";
// }); 

// $("#searchKeyword").focusout(function (e) {  
 // //$("#search-cross-icon").prop("display","none");

   // var crossIcon = document.getElementById("search-cross-icon"); 
   // crossIcon.style.display="none"; 
// });



$('#new_password').focusout(function (e) {
        var old_password = document.getElementById('password').value;
        var new_password = document.getElementById('new_password').value;    
        var myBtn = document.getElementById('ResetPassword');
    if (old_password != "" && new_password != "")
    {
        if (old_password == new_password) {
            //alert("New Password cannot be same as old password");
            myBtn.disabled = true;
        }
        else
        {
            myBtn.disabled = false;
        }
    }   

});

$('#confirm_password').focusout(function (e) {
    var cnfm_password = document.getElementById('confirm_password').value;
        var new_password = document.getElementById('new_password').value;    
        var myBtn = document.getElementById('ResetPassword');
    if (cnfm_password != "" && new_password != "") {
        if (cnfm_password != new_password) {
            //alert("confirm password should be same as new password");
            $(".erruservalidation").html("Please enter new password and confirm new password. ");
            myBtn.disabled = true;
        }
        else {
            $(".erruservalidation").html("");
            myBtn.disabled = false;
        }
    }
    else {
        $(".erruservalidation").html("");
        myBtn.disabled = true;
    }
   
});

$('#new_password').focusout(function (e) {
if(page == "admin_Rese_Password")
{
	var pass = document.getElementById('password1').value;
    var new_password = document.getElementById('new_password').value;    
    var myBtn = document.getElementById('ResetPassword');
    
	if (pass != "" && new_password != "")
    {
        if (pass != new_password)
        {
            $("#spnErrorValidation").html("Both password should be same");
           // alert("Both password should be same");
            myBtn.disabled = true;
        }
        else {
            $("#spnErrorValidation").html("");
            myBtn.disabled = false;
        }
	}
	
}	  
});

$('#password1').focusout(function (e) {
if(page == "admin_Rese_Password")
{
	var pass = document.getElementById('password1').value;
    var new_password = document.getElementById('new_password').value;    
    var myBtn = document.getElementById('ResetPassword');
    
	if (new_password != "")
    {
        if (pass != new_password)
        {
            $("#spnErrorValidation").html("Both password should be same");
           // alert("Both password should be same");
            myBtn.disabled = true;
        }
        else {
            $("#spnErrorValidation").html("");
            myBtn.disabled = false;
        }
	}
	
}	  
});

    $('input[name=user_type]').change(function () {
        
var value = $( 'input[name=user_type]:checked' ).val();
var input = document.getElementById('vendor');
var myBtn = document.getElementById('btn');
		var Name = document.getElementById("name").value;
var Vendor= document.getElementById("vendor").value;
        var site = document.getElementById("site").value;
        var MobileNumber = document.getElementById("mobile").value;
        var email = document.getElementById("email").value;
if(value == "2")
	{
		input.setAttribute('required', '');
		$('#vendor').css("display", "block");
		$('#vendorName').css("display", "block");
		if(Name != "" && site != "" && MobileNumber != "" && email != "" && Vendor != "")
		{
			myBtn.disabled = false;
		}
		else
		{
			myBtn.disabled = true;
		}
	}
else
	{
		input.removeAttribute('required', '');
		input.value = '';
		$('#vendor').css("display", "none");
		$('#vendorName').css("display", "none");
		if(Name != "" && site != "" && MobileNumber != "" && email != "")
		{
			myBtn.disabled = false;
		}
		else
		{
			myBtn.disabled = true;
		}
	}
});

});


$('#empList_img').change(function ()
{
 var file = document.getElementById('empList_img').files[0];
    var fname = file.name;
    var count = fname.split('.').length - 1;
    if (count > 1)
    {
        document.getElementById('empList_img').value = "";
        setTimeout(function () { $(".agel-remove-upload").click(); isEmpty(); }, 500);
        alert("Please select file with single extension");
        return false;
    }

    var fileUpload = $("#empList_img")[0];
    var reader = new FileReader();
    //Read the contents of Image File.
    reader.readAsDataURL(fileUpload.files[0]);
    reader.onload = function (e) {
        //Initiate the JavaScript Image object.
        var image = new Image();
        //Set the Base64 string return from FileReader as source.
        image.src = e.target.result;
        image.onload = function () {
            //Determine the Height and Width.
            var height = this.height;
            var width = this.width;
            if (width != 338) {
                alert("Please upload thumbnail in 338 x 290 required dimnesion. Uplaoded image width is : " + width);
                document.getElementById('empList_img').value = "";
                setTimeout(function () { $(".agel-remove-thumb").click(); isEmpty(); }, 500);
                return false;
            }
            if (height != 290) {
                alert("Please upload thumbnail in 338 x 290 required dimnesion. Uplaoded image height is : " + height);
                document.getElementById('empList_img').value = "";
                setTimeout(function () { $(".agel-remove-thumb").click(); isEmpty(); }, 500);
                return false;
            }
            return true;
        };


    }
});


$('.confirmPasscode').keyup(function ()
{
    
    var confirmPasscode = document.getElementById("confirm_password");
    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (confirmPasscode.value.match(lowerCaseLetters)) {
        letter.classList.remove("invalid");
        letter.classList.add("valid");
        document.getElementById("capital").src = "/images/AGELPortal/check.webp";
    }
    else {
        letter.classList.remove("valid");
        letter.classList.add("invalid");
        document.getElementById("capital").src = "/images/AGELPortal/cross.png";
    }

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (confirmPasscode.value.match(upperCaseLetters)) {
        capital.classList.remove("invalid");
        capital.classList.add("valid");
        document.getElementById("capital").src = "/images/AGELPortal/check.webp";
    }
    else {
        capital.classList.remove("valid");
        capital.classList.add("invalid");
        document.getElementById("capital").src = "/images/AGELPortal/cross.png";
    }
    // Validate numbers
    var numbers = /[0-9]/g;
    if (confirmPasscode.value.match(numbers)) {
        number.classList.remove("invalid");
        number.classList.add("valid");
        document.getElementById("number").src = "/images/AGELPortal/check.webp";
    }
    else {
        number.classList.remove("valid");
        number.classList.add("invalid");
        document.getElementById("number").src = "/images/AGELPortal/cross.png";
    }
    //validate Length
    if (myInput.value.length >= 8) {
        length.classList.remove("invalid");
        length.classList.add("valid");
        document.getElementById("letter").src = "/images/AGELPortal/check.webp";
    }
    else {
        length.classList.remove("valid");
        length.classList.add("invalid");
        document.getElementById("letter").src = "/images/AGELPortal/cross.png";
    }
   
});

$('.passcode').keyup(function () {

    var passcode = document.getElementById("new_password");
    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (passcode.value.match(lowerCaseLetters))
    {
        letter.classList.remove("invalid");
        letter.classList.add("valid");
        document.getElementById("capital-image").src = "/images/AGELPortal/check.webp";
       
    }
    else {
        letter.classList.remove("valid");
        letter.classList.add("invalid");
        document.getElementById("capital-image").src = "/images/AGELPortal/cross.png";
    }

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (passcode.value.match(upperCaseLetters))
    {
        capital.classList.remove("invalid");
        capital.classList.add("valid");
        document.getElementById("capital-image").src = "/images/AGELPortal/check.webp";
       
    }
    else {
        capital.classList.remove("valid");
        capital.classList.add("invalid");
        document.getElementById("capital-image").src = "/images/AGELPortal/cross.png";
    }

    // Validate numbers
    var numbers = /[0-9]/g;
    if (passcode.value.match(numbers)) {
        number.classList.remove("invalid");
        number.classList.add("valid");
        document.getElementById("number-image").src = "/images/AGELPortal/check.webp";
    }
    else {
        number.classList.remove("valid");
        number.classList.add("invalid");
        document.getElementById("number-image").src = "/images/AGELPortal/cross.png";
    }
    
    //validate Length
    if (passcode.value.length >= 8) {
        length.classList.remove("invalid");
        length.classList.add("valid");
        document.getElementById("letter-image").src = "/images/AGELPortal/check.webp";
    }
    else {
        length.classList.remove("valid");
        length.classList.add("invalid");
        document.getElementById("letter-image").src = "/images/AGELPortal/cross.png";
    }

});

$('.profileconfirmPasscode').keyup(function () {

    var passcode = document.getElementById("profile_confirm_password");
    var myBtn = document.getElementById("ResetPassword");
    var valid = true;
    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (passcode.value.match(lowerCaseLetters)) {
        letter.classList.remove("invalid");
        letter.classList.add("valid");
        document.getElementById("capital-image").src = "/images/AGELPortal/check.webp";

    }
    else {
        valid = false;
        letter.classList.remove("valid");
        letter.classList.add("invalid");
        document.getElementById("capital-image").src = "/images/AGELPortal/cross.png";
    }

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (passcode.value.match(upperCaseLetters)) {
        capital.classList.remove("invalid");
        capital.classList.add("valid");
        document.getElementById("capital-image").src = "/images/AGELPortal/check.webp";

    }
    else {
        valid = false;
        capital.classList.remove("valid");
        capital.classList.add("invalid");
        document.getElementById("capital-image").src = "/images/AGELPortal/cross.png";
    }

    // Validate numbers
    var numbers = /[0-9]/g;
    if (passcode.value.match(numbers)) {
        number.classList.remove("invalid");
        number.classList.add("valid");
        document.getElementById("number-image").src = "/images/AGELPortal/check.webp";
    }
    else {
        valid = false;
        number.classList.remove("valid");
        number.classList.add("invalid");
        document.getElementById("number-image").src = "/images/AGELPortal/cross.png";
    }

    //validate Length
    if (passcode.value.length >= 8) {
        length.classList.remove("invalid");
        length.classList.add("valid");
        document.getElementById("letter-image").src = "/images/AGELPortal/check.webp";
    }
    else {
        valid = false;
        length.classList.remove("valid");
        length.classList.add("invalid");
        document.getElementById("letter-image").src = "/images/AGELPortal/cross.png";
    }

});

$('.profilepasscode').keyup(function () {

    var passcode = document.getElementById("profile_new_password");
    var myBtn = document.getElementById("ResetPassword");
    var valid = true;
    // Validate lowercase letters
    var lowerCaseLetters = /[a-z]/g;
    if (passcode.value.match(lowerCaseLetters)) {
        letter.classList.remove("invalid");
        letter.classList.add("valid");
        document.getElementById("capital-image").src = "/images/AGELPortal/check.webp";

    }
    else {
        valid = false;
        letter.classList.remove("valid");
        letter.classList.add("invalid");
        document.getElementById("capital-image").src = "/images/AGELPortal/cross.png";
    }

    // Validate capital letters
    var upperCaseLetters = /[A-Z]/g;
    if (passcode.value.match(upperCaseLetters)) {
        capital.classList.remove("invalid");
        capital.classList.add("valid");
        document.getElementById("capital-image").src = "/images/AGELPortal/check.webp";

    }
    else {
        valid = false;
        capital.classList.remove("valid");
        capital.classList.add("invalid");
        document.getElementById("capital-image").src = "/images/AGELPortal/cross.png";
    }

    // Validate numbers
    var numbers = /[0-9]/g;
    if (passcode.value.match(numbers)) {
        number.classList.remove("invalid");
        number.classList.add("valid");
        document.getElementById("number-image").src = "/images/AGELPortal/check.webp";
    }
    else {
        valid = false;
        number.classList.remove("valid");
        number.classList.add("invalid");
        document.getElementById("number-image").src = "/images/AGELPortal/cross.png";
    }

    //validate Length
    if (passcode.value.length >= 8) {
        length.classList.remove("invalid");
        length.classList.add("valid");
        document.getElementById("letter-image").src = "/images/AGELPortal/check.webp";
    }
    else {
        valid = false;
        length.classList.remove("valid");
        length.classList.add("invalid");
        document.getElementById("letter-image").src = "/images/AGELPortal/cross.png";
    }
    if (valid) {
        myBtn.disabled = false;
    }
    else {
        myBtn.disabled = true;
    }
});


$('#agel_file').change(function () {
    var file = document.getElementById('agel_file').files[0];
    var fname = file.name;    
    var count = fname.split('.').length - 1;
    if (count > 1)
    {
    document.getElementById('agel_file').value = "";
    setTimeout(function () { $(".agel-remove-upload").click(); isEmpty(); }, 500);
    alert("Please select file with single extension");
    return false;
    }
   
});



$('#btn').click(function(){
// Parsekey :  '8080808080808080'
// keySize: 128 / 8,
// mode: CryptoJS.mode.CBC,
// padding: CryptoJS.pad.Pkcs7 

var stringToEncrypt = document.getElementById('password').value;
var Parsekey =  '8080808080808080';
var keySize = 128/8;
var mode = CryptoJS.mode.CBC;
var padding= CryptoJS.pad.Pkcs7;

key = CryptoJS.enc.Utf8.parse(Parsekey);
iv = CryptoJS.enc.Utf8.parse(Parsekey);
encryptedString= CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(stringToEncrypt), key,
        {
            keySize: keySize,
            iv: iv,
            mode: mode,
            padding: padding 
        }).toString();
document.getElementById('password').value = encryptedString;
return true;
});






