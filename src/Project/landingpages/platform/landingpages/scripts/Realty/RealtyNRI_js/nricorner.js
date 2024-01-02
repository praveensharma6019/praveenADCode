var regex_special_num = /^[0-9]+$/;
var email_regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/i;

function validateEmail(mailid) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    //var filter = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@(adani.com)$/;
    if (filter.test(mailid)) { return true; }
    else { return false; }
}
var globalurl;
var GlobalmasterId;
var GlobalAdvId;
var Globalutm_source;
var Globalutm_placement;
var GlobalrecordId;
var isincludedquerystring = false;
var globalurl = window.location.href;
if (globalurl.includes('?')) {
    function GetURLParameter(sParam) {
        var url_string = window.location.href;
        var url = new URL(url_string);
        var c = url.searchParams.get(sParam);
        return c
    }
    Globalutm_source = GetURLParameter('utm_source');
    Globalutm_placement = GetURLParameter('utm_placement');
    GlobalAdvId = GetURLParameter('AdvertisementId');
    isincludedquerystring = true;

}
else {
    Globalutm_source = "";
    GlobalAdvId = "";
    Globalutm_placement = "";
    var isincludedquerystring = false;
}

const checkbox = document.getElementById('agree')
const Mobilenumber = document.getElementById('mobileNo')
const Email = document.getElementById('enq_email')
const name = document.getElementById('enq_name')
const enq_project = document.getElementById('enq_project')
const enq_location = document.getElementById('enq_location')

checkbox.addEventListener('change', (event) => {
    event.preventDefault();
    if (event.currentTarget.checked) {
        $("#agree-error").html("");
    } else {
        $("#agree-error").html('Please Check the Terms!');
    }
})
enq_project.addEventListener('change', (event) => {
    event.preventDefault();
    var strUser = enq_project.options[enq_project.selectedIndex].value;
    if (strUser == 0) {
        $("#project1").html("Please select a Project!");
    }
    else {
        $("#project1").html("");
    }
})
enq_location.addEventListener('change', (event) => {
    event.preventDefault();
    var strUser = enq_location.options[enq_location.selectedIndex].value;
    if (strUser == 0) {
        $("#project2").html("Please select a location!");
    }
    else {
        $("#project2").html("");
    }
})
name.addEventListener('change', (event) => {
    event.preventDefault();
    if (!$("#enq_name").val()) {
        $("#enq_name").next().next().html('Please Enter valid Name!');
    } else {
        $("#enq_name").next().next().html("");
    }
})

Mobilenumber.addEventListener('change', (event) => {
    event.preventDefault();
    const maxlen = $("#mobileNo").attr("maxlength");
    if ($("#mobileNo").val().length != maxlen) {
        $("#mobileNo").next().next().html("Please enter valid Mobile Number!");
    } else {
        $("#mobileNo").next().next().html("");
    }
})

Email.addEventListener('change', (event) => {
    event.preventDefault();
    if (validateEmail($("#enq_email").val()) == false) {
        $("#enq_email").next().next().html("Please enter proper Email ID!");
    } else {
        $("#enq_email").next().next().html("");
    }
})

$(".enquiryBtn").click(function (e) {
    if (!$("#enq_name").val()) {
        $("#enq_name").next().next().html('Please Enter valid Name!');
        return false;
    } else {
        $("#enq_name").next().next().html("");
    }
    const maxlen = $("#mobileNo").attr("maxlength");

    if (!$("#mobileNo").val()) {
        $("#mobileNo").next().next().html('Please Enter Your Mobile Number!');
        return false;
    } else {
        $("#mobileNo").next().next().html("");
    }
    if ($("#mobileNo").val().length != maxlen) {
        $("#mobileNo").next().next().html("Please enter valid Mobile Number!");
        return false;
    } else {
        $("#mobileNo").next().next().html("");
    }
    if (!$("#enq_email").val()) {
        $("#enq_email").next().next().html('Please Enter Your Email ID!');
        return false;
    } else {
        $("#enq_email").next().next().html("");
    }
    if (validateEmail($("#enq_email").val()) == false) {
        $("#enq_email").next().next().html("Please enter proper Email ID!");
        return false;
    } else {
        $("#enq_email").next().next().html("");
    }
    var e = document.getElementById("enq_project");
    var strUser = e.options[e.selectedIndex].value;
    if (strUser == 0) {
        $("#project1").html("Please select a Project!");
        return false;
    }
    var f = document.getElementById("enq_location");
    var strUser = f.options[f.selectedIndex].value;
    if (strUser == 0) {
        $("#project2").html("Please select a location!");
        return false;
    }
    var i, j;
    if (!$("#agree").is(":checked")) {
        $("#agree-error").html('Please Check the Terms!');
        return false;
    } else {
        j = "true";
        $("#agree-error").next().html("");
    }
    if (!$("#interested").is(":checked")) {
        i = "false";
    } else {
        i = "true";
    }
    document.getElementById("enq_submit").disabled = true;

    var savecustomdata = {
        fullname: $("#enq_name").val(),
        mobile: $(".selectBtn").attr("data-id") + $("#mobileNo").val(),
        // code: $(".selectBtn").attr("data-id"),
        email: $("#enq_email").val(),
        country: document.getElementById('flagid').getAttribute('value'),
        propertytype: "RESIDENTIAL",
        ProjectName: e.options[e.selectedIndex].text,
        PropertyLocation: e.options[e.selectedIndex].getAttribute("data-type"),
        FormType: $(".popup-form h3").attr("id"),
        PageInfo: "Invest",
        PageURL: window.location.href,
        UTMSource: Globalutm_source,
        UTMPlacement: Globalutm_placement,
        AdvertisementId: GlobalAdvId,
        HomeLoan: i,
        TermsAndcondition: j,
        isincludedquerystring: isincludedquerystring
    };

    $.ajax({

        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Realty/NriCornerEnquiryNow",
        contentType: "application/json",
        success: function (data) {
            if (data.status === "101") {

                if ($(".popup-form h3").attr("id") === "Download Brochure") {
                    var userAgent = navigator.userAgent || navigator.vendor || window.opera;

                    if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i) || userAgent.match(/iPod/i)) {
                        // alert('IOS Device');
                        var link = document.createElement('a');
                        var i = "" + "images/Realty/RealtyNRI_images/brochure/";
                        var loc = e.options[e.selectedIndex].text + ".pdf";
                        link.href = i + loc;
                        link.download = loc;
                        setTimeout(function () {
                            link.click();
                            link.remove();
                        }, 200);
                        $(this).prop('disabled', true);
                        document.getElementById("formsids").reset();
                        setTimeout(function () { document.location.href = "/invest/thankyou"; }, 450);
                    }
                    else if (userAgent.toString().includes("Mac OS")) {
                        var link = document.createElement('a');
                        var i = "" + "images/Realty/RealtyNRI_images/brochure/";
                        var loc = e.options[e.selectedIndex].text + ".pdf";
                        link.href = i + loc;
                        link.download = loc;
                        setTimeout(function () {
                            link.click();
                            link.remove();
                        }, 50);
                        $(this).prop('disabled', true);
                        document.getElementById("formsids").reset();
                        setTimeout(function () { document.location.href = "/invest/thankyou"; }, 250);
                    }
                    // Desktop Version Start
                    else {
                        var link = document.createElement('a');
                        var i = "" + "images/Realty/RealtyNRI_images/brochure/";
                        var loc = e.options[e.selectedIndex].text + ".pdf";
                        link.href = i + loc;
                        link.download = loc;
                        link.click();
                        link.remove();
                        $(this).prop('disabled', true);
                        document.getElementById("formsids").reset();
                        window.location.href = "/invest/thankyou";
                    }
                }
                else {
                    $(this).prop('disabled', true);
                    document.getElementById("formsids").reset();
                    window.location.href = "/invest/thankyou";
                }
            }
            else if (data.status === "401") {
                document.getElementById("enq_submit").disabled = false;
                $("#enq_name").next().next().html('Please Enter valid Name!');
            } else if (data.status === "403") {
                document.getElementById("enq_submit").disabled = false;
                $("#enq_email").next().next().html('Please enter proper Email ID!');
            } else if (data.status === "405") {
                document.getElementById("enq_submit").disabled = false;
                $("#enq_phone").next().next().html('Please Enter Your Mobile Number!');
            }
            else if (data.status === "RecordType" || data.status === "MasterProjectID" || data.status === "ProjectName" ||
                data.status === "country" || data.status === "propertyType") {
                document.getElementById("enq_submit").disabled = false;
                $("#enq_form").next().html('Please Enter Correct Data!');
            }
        },
        error: function (data) {
            document.getElementById("enq_submit").disabled = false;
            $("#enq_form").next().html('Some technical issue please try after some time');
        }
    });

})





