
// $("#firstname").focusout(function (e)
// {
//     if (!$("#firstname").val())
//     {
//         $("#firstname").addClass("error-form");
//         alert("First name cannot be blank");
//         return false;
//     }
//     $("#firstname").removeClass("error-form");

//     if (!/^[a-zA-Z ]+$/.test($("#firstname").val())) {
//         alert("Invalid first name");
//         $("#firstname").addClass("error-form");
//         return false;
//     }
//     $("#firstname").removeClass("error-form");
// });

// $("#lastname").focusout(function (e) {
//     if (!$("#lastname").val()) {
//         $("#lastname").addClass("error-form");
//         alert("last name cannot be blank");
//         return false;
//     }
//     $("#lastname").removeClass("error-form");

//     if (!/^[a-zA-Z ]+$/.test($("#lastname").val())) {
//         alert("Invalid last name");
//         $("#lastname").addClass("error-form");
//         return false;
//     }
//     $("#firstname").removeClass("error-form");
// });

// $("#email").focusout(function (e) {
//     if (!$("#email").val()) {
//         $("#email").addClass("error-form");
//         alert("Email cannot be blank");
//         return false;
//     }
//     $("#email").removeClass("error-form");

//     if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#email").val())) {
//         $("#email").addClass("error-form");
//         alert("Invalid email");
//         return false;
//     }
//     $("#email").removeClass("error-form");
// });

// $("#contactNumber").focusout(function (e) {
//     $("#contactNumber").removeClass("error-form");
//     if ($("#contactNumber").val().length !== 10) {
//         $("#contactNumber").addClass("error-form");
//         alert("Contact Number should be of 10 digit only");
//         return false;
//     }
//     if (!/^\d*(?:\.\d{1,2})?$/.test($("#contactNumber").val())) {
//         $("#contactNumber").addClass("error-form");
//         alert("Invalid contact number");
//         return false;
//     }
//     $("#contactNumber").removeClass("error-form");
// });

// $("#customerQuery").focusout(function (e) {
//     if (!$("#customerQuery").val()) {
//         $("#customerQuery").addClass("error-form");
//         alert("Query cannot be blank");
//         return false;
//     }
//     $("#customerQuery").removeClass("error-form");
// });

$("#ParticipantName").focusout(function (e) {
    if (!$("#ParticipantName").val()) {
        $("#ParticipantName").addClass("error-form");
        //alert("Participant name cannot be blank");
        return false;
    }
    $("#ParticipantName").removeClass("error-form");

    if (!/^[a-zA-Z ]+$/.test($("#ParticipantName").val())) {
        alert("Invalid Business name");
        $("#ParticipantName").addClass("error-form");
        return false;
    }
    $("#ParticipantName").removeClass("error-form");
});

$("#BusinessName").focusout(function (e) {
    if (!$("#BusinessName").val()) {
        $("#BusinessName").addClass("error-form");
        //alert("Business name cannot be blank");
        return false;
    }
    $("#BusinessName").removeClass("error-form");

    if (!/^[a-zA-Z ]+$/.test($("#BusinessName").val())) {
        alert("Invalid Business name");
        $("#BusinessName").addClass("error-form");
        return false;
    }
    $("#BusinessName").removeClass("error-form");
});

$("#BusinessWebsite").focusout(function (e) {
    if (!$("#BusinessWebsite").val()) {
        $("#BusinessWebsite").addClass("error-form");
        //alert("Business Website cannot be blank");
        return false;
    }
    $("#BusinessWebsite").removeClass("error-form");

    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#BusinessWebsite").val())) {
        $("#BusinessWebsite").addClass("error-form");
        alert("Invalid Business Website");
        return false;
    }
    $("#BusinessWebsite").removeClass("error-form");
});

/*$("#BusinessType").focusout(function (e) {
    if (!$("#BusinessType").val()) {
        $("#BusinessType").addClass("error-form");
        alert("Business Type cannot be blank");
        return false;
    }
    $("#BusinessType").removeClass("error-form");

    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#BusinessType").val())) {
        $("#BusinessType").addClass("error-form");
        alert("Invalid Business Website");
        return false;
    }
    $("#BusinessType").removeClass("error-form");
});*/

$("#Email_Id").focusout(function (e) {
    if (!$("#Email_Id").val()) {
        $("#Email_Id").addClass("error-form");
        //alert("Email Id cannot be blank");
        return false;
    }
    $("#Email_Id").removeClass("error-form");

    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#Email_Id").val())) {
        $("#Email_Id").addClass("error-form");
        alert("Invalid Email Id");
        return false;
    }
    $("#Email_Id").removeClass("error-form");
});

$("#Address").focusout(function (e) {
    if (!$("#Address").val()) {
        $("#Address").addClass("error-form");
        //alert("Address cannot be blank");
        return false;
    }
    $("#Address").removeClass("error-form");

    if (!/^[a-z A-Z0-9 ]*$/.test($("#Address").val())) {
        alert("Invalid Address");
        $("#Address").addClass("error-form");
        return false;
    }
    $("#Address").removeClass("error-form");
});

/* Join Us page file upload check starts*/
function fileUploadCheck(id) {
    // show label of uploaded file
    var i = $(this).prev('label').clone();
    var filename = $('#' + id)[0].files[0].name;
    $(this).prev('label').text(filename);

    //check whether uploded file having double extension
    var count = filename.split('.').length - 1;
    if (count > 1) {
        document.getElementById(id).value = "";
        alert("Please upload file with single extension");
        return false;
    }

    //check whether uploded file is pdf or not
    regex = new RegExp("(.*?)\.(pdf)$");

    if (!(regex.test(filename.toLowerCase()))) {
        $(this).val('');
        alert('Please upload correct file format, validate file format is pdf only');
        return false;
    }

    //check uploded pdf size
    var fsize = document.getElementById(id).files[0].size;
    fsize = Math.max((fsize / 1024));
    if (fsize > 5000) {
        document.getElementById(id).value = "";
        alert("document size should be less 5 MB");
        return false;
    }
}




$('#Doc_UploadProject1').change(function () {
    id = "Doc_UploadProject1";
    fileUploadCheck(id);

});
$('#Doc_ProfitLoss1').change(function () {
    id = "Doc_ProfitLoss1";
    fileUploadCheck(id);

});
$('#Doc_Ownership1').change(function () {
    id = "Doc_Ownership1";
    fileUploadCheck(id);

});
$('#Doc_ProjectionSales1').change(function () {
    id = "Doc_ProjectionSales1";
    fileUploadCheck(id);

});
$('#Doc_ValuationReport1').change(function () {
    id = "Doc_ValuationReport1";
    fileUploadCheck(id);

});
/* Join Us page file upload check till here */

$(document).ready(function () {
    $("#AdaniPrizesBlock").hide();


});




$("#adani2").click(function () {
    var isCheck = document.getElementById("adani2").value;
    if (isCheck == "uncheck") {
        document.getElementById("adani2").value = "check";
        $("#AdaniPrizesBlock").show();
    }
    else {
        document.getElementById("adani2").value = "uncheck";
        $("#AdaniPrizesBlock").hide();
    }

});

$("#adanidoc").click(function () {
    var isCheck = document.getElementById("adanidoc").value;
    if (isCheck == "uncheck") {
        document.getElementById("adanidoc").value = "check";
    }
    else {
        document.getElementById("adanidoc").value = "uncheck";
    }

});
/*Contact Us form Google response
$("#contactFormSubmit_Btn").click(function (e) {
    getcontactFormSCaptchaResponseForm();
    e.preventDefault();

});*/
function getcontactFormSCaptchaResponseForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'SubscribeUsForm' }).then(function (token) {
            $('#googleCaptchaToken').val(token);
        });
    });
}
/*Subscribe Us Form*/
$("#btnSubscribe").click(function (e) {
    getCaptchaResponseForm();
    e.preventDefault();

});



function getCaptchaResponseForm(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Le0pu0gAAAAAD9pAHmh_CiRwBo6VZCNEvhMMnes', { action: 'SubscribeUsForm' }).then(function (token) {
            $('#googleCaptchaToken').val(token);


            var emailId = document.getElementById("emailSubscribe").value.trim();
            if (emailId == "") {
                $("#emailSubscribe").addClass("error-form");
                $("#lblemail").text('*Email cannot be blank');
                $("#lblemail").show();
                return false;
            }
            $("#emailSubscribe").removeClass("error-form");
            if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($("#emailSubscribe").val())) {
                $("#emailSubscribe").addClass("error-form");
                $("#lblemail").text('*Invalid Email');
                $("#lblemail").show();
                return false;
            }
            $("#emailSubscribe").removeClass("error-form");

            var saveSubscribeUsData = {
                Email: $("#emailSubscribe").val(),
                FormType: $("#FormType").val(),
                FormUrl: $('#FormUrl').val().trim() == "" ? "Subscribe Us" : $('#FormUrl').val().trim(),
                googleCaptchaToken: token
            };

            $.ajax({
                type: "POST",
                data: JSON.stringify(saveSubscribeUsData),
                url: "/api/AdaniGreenTalks/SubscribeUsForm",
                contentType: "application/json",
                success: function (data) {
                    if (data.status == 1) {
                        $("#emailSubscribe").val('')
                        $("#subscribe").hide();
                        $("#subscribe-success").show();
                    }
                    else if (data.status == 401) {
                        $("#emailSubscribe").addClass("error");
                        $("#lblemail").text('*Invalid Email Address');
                        $("#lblemail").show();
                    }
                    else if (data.status == 11) {
                        $("#emailSubscribe").addClass("error");
                        $("#lblemail").text('*Invalid Captcha');
                        $("#lblemail").show();
                    }
                },
                error: function (data) {
                    alert(data.status);
                }
            });
        });
    });
}




$("#searchBox").keyup(function (event) {
    if (event.keyCode === 13) {
        $("#faqSearch").click();
    }
});

$("#faqSearch").click(function () {
    var path = window.location.pathname;
    var page = path.split("/").pop();

    var searchContent = document.getElementById("searchBox").value;
    if (searchContent == "") {
        alert("Please type something to search");
        return false;
    }

    $.ajax({
        type: "GET",
        data: { searchData: $('#searchBox').val() },
        dataType: 'json',
        url: "/api/AdaniGreenTalks/FAQSearch",
        contentType: "application/json",
        success: function (data) {
            var searchResult = JSON.parse(data);
            if (searchResult.length == 0) {
                alert("No Data Found");
                // Hide and show Error message Or Show error 
            }
            else {
                $('#accordionExample .accordion-item').remove();
                let collapseID = 0;
                let collapse = "";
                let headingId = "";
                let bstarget = "";
                for (var x = 0; x < searchResult.length; x++) {
                    collapseID++;
                    collapse = "collapse" + collapseID;
                    headingId = "heading" + collapseID;
                    bstarget = "#" + collapse;
                    //$('#accordionExample').append('<div class="accordion-item"><h2 class="accordion-header" id="' + headingId + '"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="' + bstarget +'" aria-expanded="true" aria-controls="' + collapse + '">' + searchResult[x].Key + '</button></h2><div id="' + collapse + '" class="accordion-collapse collapse" aria-labelledby="' + headingId +'" data-bs-parent="#accordionExample"><div class="accordion-body"><p>' + searchResult[x].Value + '</p></div></div></div>');
                    // <h2 id="heading0" class="accordion-header"><button type="button" data-bs-toggle="collapse" aria-expanded="true" aria-controls="collapse0" data-bs-target="#collapse0" class="accordion-button collapsed">searchResult.Key</button></h2><div id="collapse0" aria-labelledby="heading0" data-bs-parent="#accordionExample" class="accordion-collapse collapse"><div id="accordionbody" class="accordion-body"><p>searchResult</p></div></div>

                    var divaccordion = $("<div>").attr("id", "accordion").addClass("accordion-item");
                    var h2 = $("<h2>").attr("id", headingId).addClass("accordion-header");
                    var button = $("<button>").attr("type", "button").attr("data-bs-toggle", "collapse").attr("aria-expanded", "true").attr("aria-controls", collapse).attr("data-bs-target", bstarget).addClass("accordion-button").addClass("collapsed").text(searchResult[x].Key);
                    var collapsediv = $("<div>").attr("id", collapse).attr("aria-labelledby", headingId).attr("data-bs-parent", "#accordionExample").addClass("accordion-collapse").addClass("collapse");
                    var divaccordionbody = $("<div>").attr("id", "accordionbody").addClass("accordion-body");
                    var p = $("<p>").text(searchResult[x].Value);
                    divaccordionbody.append(p);
                    collapsediv.append(divaccordionbody);
                    h2.append(button);
                    divaccordion.append(h2);
                    divaccordion.append(collapsediv);
                    $('#accordionExample').append(divaccordion);



                    //<div id="' + collapse + '" class="accordion-collapse collapse" aria-labelledby="' + headingId +'" data-bs-parent="#accordionExample">
                    //<div class="accordion-body"><p>' + searchResult[x].Value + '</p></div></div ></div > ');


                    //<button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="' + bstarget +'" aria-expanded="true" aria-controls="' + collapse + '">' + searchResult[x].Key + '</button></h2>
                    //$('#accordionExample').append('<div class="accordion-item"><h2 class="accordion-header" id="' + headingId + '"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="' + bstarget +'" aria-expanded="true" aria-controls="' + collapse + '">' + searchResult[x].Key + '</button></h2><div id="' + collapse + '" class="accordion-collapse collapse" aria-labelledby="' + headingId +'" data-bs-parent="#accordionExample"><div class="accordion-body"><p>' + searchResult[x].Value + '</p></div></div></div>');
                }
            }

        },
        error: function (data) {
            alert(data.status);
        }
    });
});

$("#showMoreVideo").click(function () {
    var curre = parseInt($('#currentpage').val());
    var next = curre + 1;
    $('#currentpage').val(next)
    $.ajax({
        type: "GET",
        data: { currentpage: $('#currentpage').val() },
        dataType: 'json',
        url: "/api/AdaniGreenTalks/PaginationLatestVideoSection",
        contentType: "application/json",
        success: function (data) {
            var MoreLatestVideo = JSON.parse(data);
            if (MoreLatestVideo.length == 0) {
                alert("No Data Found");
                // Hide and show Error message Or Show error 
            }
            else {

                $('.action-bottom-section .item').remove();
                document.getElementById('currentPageNo').textContent = $('#currentpage').val();
                for (var x = 0; x < MoreLatestVideo.length; x++) {
                    var action = $("<div>").attr("id", "action-bottom").addClass("col-12 col-md-6 col-lg-4 item");
                    var videothumb = $("<div>").attr("id", "video-thumb").addClass("video-thumb");
                    var divimg = $("<img>").attr("src", "/-/media/project/adanigreentalks/themes/images/player.png").attr("alt", "play").addClass("play-img");
                    videothumb.append(divimg);
                    var divdesc = $("<div>").addClass("desc");
                    var span = $("<span>").addClass("date").text(MoreLatestVideo[x].MediaDate);
                    var h3 = $("<h3>").text(MoreLatestVideo[x].Title);
                    var p = $("<p>").text(MoreLatestVideo[x].SubTitle);
                    var a = $("<a>").attr("href", MoreLatestVideo[x].CTALink).addClass("read_more").text("Read More");
                    divdesc.append(span)
                    divdesc.append(h3)
                    divdesc.append(p)
                    divdesc.append(a)
                    action.append(videothumb);
                    action.append(divdesc);
                    $('.action-bottom-section .row').append(action);

                    //$('.action-bottom-section .row').append('<div class="col-12 col-md-6 col-lg-4 item"><div class="video-thumb"><img src="' + MoreLatestVideo[x].ThumbnailImage + '" alt="ikure" class="video-thumb-img"><img src="/-/media/project/adanigreentalks/themes/images/player.png" alt="play" class="play-img"></div><div class="desc"><span class="date">' + MoreLatestVideo[x].MediaDate + '</span><h3>' + MoreLatestVideo[x].Title + '</h3><p>' + MoreLatestVideo[x].SubTitle + '</p><a href="' + MoreLatestVideo[x].CTALink + '" class="read_more">Read More</a></div></div>');
                }
            }

        },
        error: function (data) {
            alert(data.status);
        }
    });
});
