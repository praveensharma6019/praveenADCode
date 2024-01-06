





function WilmarExportsForm() {
  
    var Name = $("#eFirstname").val();
    var lastname = $("#eLastname").val();
    var AddressLine1 = $("#eaddressline1").val();
    var AddressLine2 = $("#eaddressline2").val();
    var state = $("#estate").val();
    var city = $("#ecity").val();
    var pincode = $("#epincode").val();
    var mailid = $("#eEmail").val();
    var country = $("#ecountry").val();
     var MobileNo = $("#eMobile").val();
    var LandlineNo = $("#eLandlineNumber").val();
    var Salutation = $("#esalutation").val();
    var formtype = $("#cFormType").val();
    var businesscategory = $("#ebusinessCategory").val();
    var remarks = $("#eRemarks").val();
    var pageinfo = window.location.href;

   
    

    function validateEmail(sEmail) {
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) { return true; }
        else { return false; }
    }

    var model = {

        FirstName: Name,
        Mobile: MobileNo,
        Email: mailid

    };

    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');

    //create json object
    var savecustomdata = {


        FirstName: Name,
        Lastname: lastname,
        Address1: AddressLine1,
        Address2: AddressLine2,
        State: state,
        Country:country,
        City: city,
        Pincode: pincode,
        Email: mailid,
        Mobile: MobileNo,
        salutation:Salutation,
        LandlineNumber: LandlineNo,
         FormType: formtype,
        PageInfo: pageinfo,
        BusinessCategory: businesscategory,
        Remarks: remarks,
        FormSubmit: currentdate


    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Wilmar/InsertContactFormdetail",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status == "1") {
                window.location.href = "/thankyou";
                //$('#contact_form1').submit();
            }
          
            else {
                alert("Sorry Operation Failed!!! Please try again later");
               
                return false;
            }
        }
    });
    return false;
}

 


