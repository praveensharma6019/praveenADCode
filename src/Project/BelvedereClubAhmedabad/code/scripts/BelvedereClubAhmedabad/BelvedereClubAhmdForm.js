

$(document).ready(function () {

    var SelectedRadioVal = "";


    $('#btnEnquirySubmit').click(function () {



        $('#btnEnquirySubmit').attr("disabled", "disabled");
        var name = $("#name").val();
        if (name == "") { alert("Please enter your Name"); $("#name").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
        var bookfrom = $("#bookfrom").val();
        if (bookfrom == "") { alert("Please select the booking date"); $("#bookfrom").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
        var bookto = $("#bookto").val();
        if (bookto == "") { alert("Please select the booking date"); $("#bookto").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
        var noOfRoom = $("#noOfRoom").val();
        if (noOfRoom == 0) { alert("Please select No. of Rooms required"); $("#noOfRoom").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
        var noOfAdults = $("#noOfAdults").val();
        if (noOfAdults == 0) { alert("Please specify no. of Adults Count"); $("#noOfAdults").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
        var noOfKids = $("#noOfKids").val();
        var currentdate = moment(new Date()).format("YYYY-MM-DD");
        var mailid = $("#mailid").val();
        if (mailid == "") { alert("Email is Required"); $("#mailid").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false; }
        var mobile = $("#mobile").val();
        if (mobile == "") {
            alert("Please provide your Mobile Number");
            $("#mobile").focus();
            $('#btnEnquirySubmit').removeAttr("disabled");
            return false;
        }
        else {
            if (document.enquiry_form1.mobile.value.length != 10) {
                alert("Mobile Number sould be 10 digit!");
                document.enquiry_form1.mobile.focus();
                $('#btnEnquirySubmit').removeAttr("disabled");
                return false;
            }
        }
        var isAfter = moment(currentdate).isAfter(bookfrom);
        var isAfter1 = moment(currentdate).isAfter(bookto);
        var isAfter2 = moment(bookfrom).isAfter(bookto);

        if (isAfter) {
            alert("Booking not allowed on past days"); $("#bookfrom").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;
        }



        if (isAfter1) {
            alert("Booking not allowed on past days"); $("#bookto").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;

        }

        if (isAfter2) {
            alert("Sorry!! Booking date (From) cannot be greater than Booking date(Till)"); $("#bookto").focus(); $('#btnEnquirySubmit').removeAttr("disabled"); return false;
        }
        var model = {

            Name: name,
            Mobile: mobile,
            Email: mailid

        };

        currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

        //create json object
        var savecustomdata = {

            Name: name,
            BookDateFrom: bookfrom,
            BookDateTo: bookto,
            NoOfRoom: noOfRoom,
            NoOfAdults: noOfAdults,
            NoOfKids: noOfKids,
            Email: mailid,
            Mobile: mobile,
            FormSubmitOn: currentdate
        };

        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/BelvedereClubAhmedabad/Insertcontactdetail",
            contentType: "application/json",
            success: function (data) {
                //////////////

                if (data.status == "1") {
                    window.location.href = "/ThankYou";
                    //$('#enquiry_form1').submit();
                }
                else {
                    alert("Sorry Operation Failed!!! Please try again later");
                    $('#btnEnquirySubmit').removeAttr("disabled");
                    return false;
                }
            }
        });
        return;


        $.ajax({
            type: "POST",
            data: JSON.stringify(model),
            url: "/api/BelvedereClubAhmedabad/RoomAvailability",
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
                            url: "/api/BelvedereClubAhmedabad/VerifyOTP",
                            contentType: "application/json",
                            success: function (data) {
                                if (data.status == "1") {

                                  



                                }

                                else {
                                    alert("Invalid OTP");
                                    $('#btnEnquirySubmit').removeAttr("disabled");
                                    return false;
                                }
                            }
                        });

                    }
                }
                else if (data == "-1") {
                    alert("Invalid Mobile Number");
                    $('#btnEnquirySubmit').removeAttr("disabled");
                }
            }
        });

        return false;
    });
});


