$(document).ready(function () {
    var SelectedDateofBirth = $('#dob').val();
    var formattedDOB = SelectedDateofBirth.split("-").reverse().join("/");
    $('#dob').val(formattedDOB);
    $(".DetailsOfFullHalfMarathon_row ").hide();
    $(".RaceCertificate_row ").hide();

    if ($("#select_distance option:selected").val().indexOf("42") !== -1) {
        $(".DetailsOfFullHalfMarathon_row ").show();
        $(".RaceCertificate_row ").show();
    }
    // Rajesh 
    $(function () {
        $("#registration_form #dobr").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy',

            yearRange: '1918:' + new Date().getFullYear().toString()
        });
        $("#login_form #login_dob").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy',
            yearRange: '1918:' + new Date().getFullYear().toString()
        });
        $("#registration_form #mDate").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy',

            yearRange: '1918:' + new Date().getFullYear().toString()
        });
        $("#dob1").datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'dd/mm/yy',
            yearRange: '1918:' + new Date().getFullYear().toString()
        });

        // NEw
        // var enforceModalFocusFn = $.fn.modal.Constructor.prototype.enforceFocus;

        // $.fn.modal.Constructor.prototype.enforceFocus = function() {};

        // $confModal.on('hidden', function() {
        //    $.fn.modal.Constructor.prototype.enforceFocus = enforceModalFocusFn;
        // });

        // $confModal.modal({ backdrop : false });

        // End New

    });
});
$('input[type=radio][name=runMode]').change(function () {
    var idVal = $(this).val();
    if (idVal === "Physical") {
        $("label[id='dob_txt']").text("Date Of Birth");
        $("label[id='dob_txt']").text("");
        $('#select_distance').text('');
        $('#select_distance').append('<option value="">Select your distance 1</option>');
        $('#select_distance').append('<option class="all-dist " value="42.195KM">42.195 Km Run (₹                               2500.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="21.097KM">21.097 Km Run (₹                                2000.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="10KM">10 Km Run (₹                             1750.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="5KM">5 Km Run (₹                                1250.00)</option>');
        //$(".all-dist").css("display", "none");
        //$(".Physical").css("display", "block");
        $("#select_distance").val("");
    }
    else if (idVal === "Remote") {
        $("label[id='dob_txt']").text("");
        $("label[id='dob_txt']").text("Date of Birth ");
        $('#select_distance').text('');
        $('#select_distance').append('<option value="">Select your distance 1</option>');
        $('#select_distance').append('<option class="all-dist " value="42.195KM">42.195 Km Run (₹                                499.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="21.097KM">21.097 Km Run (₹                                499.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="10KM">10 Km Run (₹                              499.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="5KM">5 Km Run (₹                               499.00)</option>');
        //$(".all-dist").css("display", "none");
        //$(".Remote").css("display", "block");
        $("#select_distance").val("");
    }
    else {
        $("label[id='dob_txt']").text("");
        $("label[id='dob_txt']").text("Date of Birth");
        $("#select_distance").val("")
    }
});
$("#RunmodeListNew").change(function () {
    var idVal = $(this).val();
    var runtype = $('#RunmodeListNew').val();

    if (runtype === "Physical Run") {
        $("#RunDatediv").show();
        $("#TimeSlotdiv").show();
    }
    else {
        $("#RunDatediv").hide();
        $("#TimeSlotdiv").hide();
    }
    if (idVal === "Physical Run") {

        $('#select_distancenew').text('');
        $('#select_distancenew').append('<option value="">Select your distance 1</option>');
        $('#select_distancenew').append('<option class="all-dist " value="42.195KM">42.195 Km Run (₹                               2500.00)</option>');
        $('#select_distancenew').append('<option class="all-dist " value="21.097KM">21.097 Km Run (₹                                2000.00)</option>');
        $('#select_distancenew').append('<option class="all-dist " value="10KM">10 Km Run (₹                            1750.00)</option>');
        $('#select_distancenew').append('<option class="all-dist " value="5KM">5 Km Run (₹                                1250.00)</option>');
        $("#select_distancenew").val("");
    }
    else if (idVal === "Remote Run") {

        $('#select_distancenew').text('');
        $('#select_distancenew').append('<option value="">Select your distance</option>');
        $('#select_distancenew').append('<option class="all-dist " value="42.195KM">42.195 Km Run (₹                                499.00)</option>');
        $('#select_distancenew').append('<option class="all-dist " value="21.097KM">21.097 Km Run (₹                                499.00)</option>');
        $('#select_distancenew').append('<option class="all-dist " value="10KM">10 Km Run (₹                              499.00)</option>');
        $('#select_distancenew').append('<option class="all-dist " value="5KM">5 Km Run (₹                               499.00)</option>');
        $("#select_distancenew").val("");
    }
    else {

        $("#select_distancenew").val("");
    }
});

function numberchk(e) {
    if (e.id === "contactno") {
        var ContactValid = e.id;
        $('#registration_form .' + ContactValid).remove();
        var contactno = $(e).val();
        if (contactno === "") {
            $("#registration_form #contactno").after("<span class='errormsg " + ContactValid + "'>Please enter your Contact Number</span>");
        }
        if (contactno.length !== 10 && contactno !== "") {
            $("#registration_form #contactno").after("<span class='errormsg " + ContactValid + "'>Contact Number should be of 10 digit</span>");
        }
        if (/^[0-9]{10}$/.test(ContactValid)) {
            $("#registration_form #" + e.id).after("<span class='errormsg " + ContactValid + "'>Only numbers are allowed.</span>");
        }
    }
    if (e.id === "emergency_contact_number") {
        var EmContactValid = e.id;
        $('#registration_form .' + EmContactValid).remove();
        var EmContactno = $(e).val();
        if (EmContactno === "") {
            $("#registration_form #emergency_contact_number").after("<span class='errormsg " + EmContactValid + "'>Please enter Emergency Contact Number</span>");
        }
        if (EmContactno.length !== 10 && EmContactno !== "") {
            $("#registration_form #emergency_contact_number").after("<span class='errormsg " + EmContactValid + "'>Please enter 10 digit Emergency Contact Number</span>");
        }
        //if (/^[0-9]{10}$/.test(EmContactno)) {
        //  $("#registration_form #" + e.id).after("<span class='errormsg " + EmContactValid + "'>Only numbers are allowed.</span>");
        //}
    }
};

function TextCheck(e) {
    var TextValid = e.id;
    $('#registration_form .' + TextValid).remove();
    var txt = $(e).val();
    if (txt === "") {
        $("#registration_form #" + e.id).after("<span class='errormsg " + TextValid + "'>This is field is mandatory.</span>");
    }
    if (/[^a-zA-Z ]/.test(txt)) {
        $("#registration_form #" + e.id).after("<span class='errormsg " + TextValid + "'>Only alphabets are allowed.</span>");
    }
};
$(document).on('dp.change', '#dob', function () {
    if ($("#registration_form .dob").length) {
        $("#registration_form .dob").remove();
        var dob = $("#registration_form #dob").val();
        var distance = $("#registration_form #select_distance").val();
        if (dob === "") {
            $("#registration_form #dob").after("<span class='errormsg dob'>Please enter your Date of Birth</span>");
        }
        if (dob !== "") {
            if ($("input[name='RunMode']:checked").val() === "Remote") {
                if (distance === "5KM") {
                    if (new Date("2011-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                        $("#registration_form #dob").after("<span class='errormsg dob'>For 5 KM race, Age must be 10 years old.</span>");
                    }
                }
                else if (distance === "10KM") {
                    if (new Date("2009-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                        $("#registration_form #dob").after("<span class='errormsg dob'>For 10 KM race, Age must be 12 years old.</span>");
                    }
                }
                else if (distance === "21.097KM") {
                    if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                        $("#registration_form #dob").after("<span class='errormsg dob'>For 21 KM race, Age must be 18 years old.</span>");
                    }
                }
                else if (distance === "42.195KM") {
                    if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                        $("#registration_form #dob").after("<span class='errormsg dob'>For 42 KM race, Age must be 18 years old.</span>");
                    }
                }
            }
            else {
                if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    $("#registration_form #dob").after("<span class='errormsg dob'>Age must be 18 years old.</span>");
                }
            }
        }
    }

});

$("#famfirst_btn").click(function () {
    var checkStatus = fampersonalcheck();
    if (checkStatus === 0) {
        $(".famsecond").show();
        $(".famfirst").hide();
        $(".fam2li-personal").addClass("active_fieldset");
        $(".famli-personal").removeClass("active_fieldset");
        $('#fam2first_name').focus();
    }
});

$("#famsecond_btn").click(function () {
    var first_name = $("#famregistration_form #fam2first_name").val();
    var checkStatus = 0;
    //if (first_name !== "")
    //	{
    checkStatus = fampersonalcheck2();
    //  }
    if (checkStatus === 0) {
        $(".famthird").show();
        $(".famsecond").hide();
        $(".fam3li-personal").addClass("active_fieldset");
        $(".fam2li-personal").removeClass("active_fieldset");
        $('#fam3first_name').focus();
    }
});

$("#famthird_btn").click(function () {
    var first_name = $("#famregistration_form #fam3first_name").val();
    var checkStatus = 0;
    //if (first_name !== "") {
    checkStatus = fampersonalcheck3();
    //}
    if (checkStatus === 0) {
        $(".famaddress").show();
        $(".famfirst").hide();
        $(".famsecond").hide();
        $(".famthird").hide();
        $(".famli-address").addClass("active_fieldset");
        $(".fam3li-personal").removeClass("active_fieldset");
        $('#famcountry_info').focus();
    }
});

function fampersonalcheck2() {
    $('#famregistration_form .errormsg').remove();
    var flag = 0;
    var first_name = $("#famregistration_form #fam2first_name").val();

    /*if (first_name === "") {
        fampersonalcheck2();
        flag++;
    }*/
    if (first_name === "") {
        $("#famregistration_form #fam2first_name").after("<span class='errormsg'>Please enter your Name</span>");
        flag++;
    }

    var last_name = $("#famregistration_form #fam2last_name").val();
    if (last_name === "") {
        $("#famregistration_form #fam2last_name").after("<span class='errormsg'>Please enter your Last Name</span>");
        flag++;
    }
    var dob = $("#famregistration_form #fam2dob").val();
    if (dob === "") {
        $("#famregistration_form #fam2dob").after("<span class='errormsg'>Please enter your Date of Birth</span>");
        flag++;
    }
    var employee_id_error = $(".fam2employee_id_error").text();
    if (employee_id_error !== "") {
        flag++;
    }


    if ($("#new_participant #select_distance").val().indexOf("42") !== -1) {
        var RaceCertificate = $("#famregistration_form #fam2RaceCertificate").val();
        if (RaceCertificate === "") {
            $("#famregistration_form #fam2RaceCertificate").after("<span class='errormsg'>Please upload Race Certificate.</span>");
            flag++;
        }

        var DetailsOfFullHalfMarathon = $("#famregistration_form #fam2DetailsOfFullHalfMarathon").val();
        if (DetailsOfFullHalfMarathon === "") {
            $("#famregistration_form #fam2DetailsOfFullHalfMarathon").after("<span class='errormsg'>Please enter Details Of Full Half Marathon.</span>");
            flag++;
        }
    }


    if ($("#new_participant #select_distance").val().indexOf(' ') !== -1) {
        var dob1 = new Date(dob);

        var distance = $("#new_participant #select_distance").val().split(' ')[0];


        if (new Date("2009-11-20") < new Date($("#famregistration_form #fam2dob").val().split("/").reverse().join("-"))) {
            $("#famregistration_form #fam2dob").after("<span class='errormsg'>For second participant, Age must be 10 years old.</span>");
            flag++;

        }

        /* if (distance === 5) {
             if (new Date("2009-11-24") < new Date($("#famregistration_form #fam2dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam2dob").after("<span class='errormsg'>For 5 KM race, Age must be 10 years old.</span>");
                 flag++;
             }
         }
         else if (distance === 10) {
             if (new Date("2009-11-24") < new Date($("#famregistration_form #fam2dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam2dob").after("<span class='errormsg'>For 10 KM race, Age must be 10 years old.</span>");
                 flag++;
 
             }
         }
         else if (distance === 21.097) {
             if (new Date("2001-11-24") < new Date($("#famregistration_form #fam2dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam2dob").after("<span class='errormsg'>For 21 KM race, Age must be 18 years old.</span>");
                 flag++;
 
             }
         }
         else if (distance === 42.195) {
             if (new Date("2001-11-24") < new Date($("#famregistration_form #fam2dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam2dob").after("<span class='errormsg'>For 42 KM race, Age must be 18 years old.</span>");
                 flag++;
 
             }
         }*/

    }

    var gender = $("#famregistration_form #fam2gender_list").val();
    if (gender === "") {
        $("#famregistration_form #fam2gender_list").after("<span class='errormsg'>Please select gender</span>");
        flag++;
    }

    var email_id = $("#famregistration_form #fam2email_id").val();
    if (email_id === "") {
        $("#famregistration_form #fam2email_id").after("<span class='errormsg'>Please enter your Email Id</span>");
        flag++;
    }
    if (!validateEmail(email_id) && email_id !== "") {
        $("#famregistration_form #fam2email_id").after("<span class='errormsg'>Please enter valid email address</span>");
        flag++;
    }
    var contactno = $("#famregistration_form #fam2contactno").val();
    if (contactno === "") {
        $("#famregistration_form #fam2contactno").after("<span class='errormsg'>Please enter your Contact Number</span>");
        flag++;
    }
    if (document.famregistration_form.fam2contactno.value.length !== 10 && contactno !== "") {
        $("#famregistration_form #fam2contactno").after("<span class='errormsg'>Contact Number should be of 10 digit</span>");
        flag++;
    }
    var nationality = $("#famregistration_form #fam2nationality").val();
    if (nationality === "") {
        $("#famregistration_form #fam2nationality").after("<span class='errormsg'>Please select your Nationality</span>");
        flag++;
    }
    var tshirt_sizeMale = $("#famregistration_form #fam2tshirt_sizeMale").val();
    if (tshirt_sizeMale === "") {
        $("#famregistration_form #fam2tshirt_sizeMale").after("<span class='errormsg'>Please select your Tshirt Size</span>");
        flag++;
    }
    var idproof = $("#famregistration_form #fam2idproof").val();
    if (idproof === "Select One") {
        $("#famregistration_form #fam2idproof").after("<span class='errormsg'>Please select your valid ID Proof </span>");
        flag++;
    }
    var idnumber = $("#famregistration_form #fam2idnumber").val();
    if (idnumber === "") {
        $("#famregistration_form #fam2idnumber").after("<span class='errormsg'>Please enter ID Number</span>");
        flag++;
    }

    var file1 = $("#famregistration_form #fam2file1").val();
    if (file1 === "") {
        $("#famregistration_form #fam2file1").after("<span class='errormsg'>Please upload scanned copy for Identity Proof </span>");
        flag++;
    }


    var blood_group = $("#famregistration_form #fam2blood_group").val();
    if (blood_group === "") {
        $("#famregistration_form #fam2blood_group").after("<span class='errormsg'>Please select your Blood Group</span>");
        flag++;
    }

    var Chronic_illness = $("#famregistration_form #fam2chronic_illness").val();

    if (Chronic_illness === "") {
        $("#famregistration_form #fam2chronic_illness").after("<span class='errormsg'>Please enter your Chronic illness</span>");
        flag++;
    }

    var heart_ailment = $("#famregistration_form #fam2heart_ailment").val();

    if (heart_ailment === "") {
        $("#famregistration_form #fam2heart_ailment").after("<span class='errormsg'>Please enter your heart ailment</span>");
        flag++;
    }
    var fainting_episodes = $("#famregistration_form #fam2fainting_episodes").val();

    if (fainting_episodes === "") {
        $("#famregistration_form #fam2fainting_episodes").after("<span class='errormsg'>Please enter your fainting episodes</span>");
        flag++;
    }

    var Any_other_ailment = $("#famregistration_form #fam2other_ailment").val();

    if (Any_other_ailment === "") {
        $("#famregistration_form #fam2other_ailment").after("<span class='errormsg'>Please enter your any other ailment</span>");
        flag++;
    }
    var Any_other_allergies = $("#famregistration_form #fam2allergies").val();

    if (Any_other_allergies === "") {
        $("#famregistration_form #fam2allergies").after("<span class='errormsg'>Please enter your any other allergies</span>");
        flag++;
    }
    return flag;
}

function fampersonalcheck3() {
    $('#famregistration_form .errormsg').remove();
    var flag = 0;
    var first_name = $("#famregistration_form #fam3first_name").val();

    /* if (first_name === "") {
         $("#famregistration_form #fam3first_name").after("<span class='errormsg'>Please enter your Name</span>");
         flag++;
     }*/

    if (first_name === "") {
        $("#famregistration_form #fam3first_name").after("<span class='errormsg'>Please enter your Name</span>");
        flag++;
    }

    var last_name = $("#famregistration_form #fam3last_name").val();
    if (last_name === "") {
        $("#famregistration_form #fam3last_name").after("<span class='errormsg'>Please enter your Last Name</span>");
        flag++;
    }
    var dob = $("#famregistration_form #fam3dob").val();
    if (dob === "") {
        $("#famregistration_form #fam3dob").after("<span class='errormsg'>Please enter your Date of Birth</span>");
        flag++;
    }
    var employee_id_error = $(".fam3employee_id_error").text();
    if (employee_id_error !== "") {
        flag++;
    }


    if ($("#new_participant #select_distance").val().indexOf("42") !== -1) {
        var RaceCertificate = $("#famregistration_form #fam3RaceCertificate").val();
        if (RaceCertificate === "") {
            $("#famregistration_form #fam3RaceCertificate").after("<span class='errormsg'>Please upload Race Certificate.</span>");
            flag++;
        }

        var DetailsOfFullHalfMarathon = $("#famregistration_form #fam3DetailsOfFullHalfMarathon").val();
        if (DetailsOfFullHalfMarathon === "") {
            $("#famregistration_form #fam3DetailsOfFullHalfMarathon").after("<span class='errormsg'>Please enter Details Of Full Half Marathon.</span>");
            flag++;
        }
    }


    if ($("#new_participant #select_distance").val().indexOf(' ') !== -1) {
        var dob1 = new Date(dob);

        var distance = $("#new_participant #select_distance").val().split(' ')[0];

        if (!(new Date("2009-11-20") > new Date($("#famregistration_form #fam3dob").val().split("/").reverse().join("-")) &&
            new Date("2003-11-20") < new Date($("#famregistration_form #fam3dob").val().split("/").reverse().join("-")))) {
            $("#famregistration_form #fam3dob").after("<span class='errormsg'>For third participant, Age must be in beteen 10 years to 17 years old.</span>");
            flag++;

        }


        /* if (distance === 5) {
             if (new Date("2009-11-24") < new Date($("#famregistration_form #fam3dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam3dob").after("<span class='errormsg'>For 5 KM race, Age must be 10 years old.</span>");
                 flag++;
             }
         }
         else if (distance === 10) {
             if (new Date("2009-11-24") < new Date($("#famregistration_form #fam3dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam3dob").after("<span class='errormsg'>For 10 KM race, Age must be 10 years old.</span>");
                 flag++;
 
             }
         }
         else if (distance === 21.097) {
             if (new Date("2001-11-24") < new Date($("#famregistration_form #fam3dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam3dob").after("<span class='errormsg'>For 21 KM race, Age must be 18 years old.</span>");
                 flag++;
 
             }
         }
         else if (distance === 42.195) {
             if (new Date("2001-11-24") < new Date($("#famregistration_form #fam3dob").val().split("/").reverse().join("-"))) {
                 $("#famregistration_form #fam3dob").after("<span class='errormsg'>For 42 KM race, Age must be 18 years old.</span>");
                 flag++;
 
             }
         }*/

    }

    var gender = $("#famregistration_form #fam2gender_list").val();
    if (gender === "") {
        $("#famregistration_form #fam2gender_list").after("<span class='errormsg'>Please select gender</span>");
        flag++;
    }


    var email_id = $("#famregistration_form #fam3email_id").val();
    if (email_id === "") {
        $("#famregistration_form #fam3email_id").after("<span class='errormsg'>Please enter your Email Id</span>");
        flag++;
    }
    if (!validateEmail(email_id) && email_id !== "") {
        $("#famregistration_form #fam3email_id").after("<span class='errormsg'>Please enter valid email address</span>");
        flag++;
    }
    var contactno = $("#famregistration_form #fam3contactno").val();
    if (contactno === "") {
        $("#famregistration_form #fam3contactno").after("<span class='errormsg'>Please enter your Contact Number</span>");
        flag++;
    }
    if (document.famregistration_form.fam3contactno.value.length !== 10 && contactno !== "") {
        $("#famregistration_form #fam3contactno").after("<span class='errormsg'>Contact Number should be of 10 digit</span>");
        flag++;
    }
    var nationality = $("#famregistration_form #fam3nationality").val();
    if (nationality === "") {
        $("#famregistration_form #fam3nationality").after("<span class='errormsg'>Please select your Nationality</span>");
        flag++;
    }
    var tshirt_sizeMale = $("#famregistration_form #fam3tshirt_sizeMale").val();
    if (tshirt_sizeMale === "") {
        $("#famregistration_form #fam3tshirt_sizeMale").after("<span class='errormsg'>Please select your Tshirt Size</span>");
        flag++;
    }
    var idproof = $("#famregistration_form #fam3idproof").val();
    if (idproof === "Select One") {
        $("#famregistration_form #fam3idproof").after("<span class='errormsg'>Please select your valid ID Proof </span>");
        flag++;
    }
    var idnumber = $("#famregistration_form #fam3idnumber").val();
    if (idnumber === "") {
        $("#famregistration_form #fam3idnumber").after("<span class='errormsg'>Please enter ID Number</span>");
        flag++;
    }

    var file1 = $("#famregistration_form #fam3file1").val();
    if (file1 === "") {
        $("#famregistration_form #fam3file1").after("<span class='errormsg'>Please upload scanned copy for Identity Proof </span>");
        flag++;
    }


    var blood_group = $("#famregistration_form #fam3blood_group").val();
    if (blood_group === "") {
        $("#famregistration_form #fam3blood_group").after("<span class='errormsg'>Please select your Blood Group</span>");
        flag++;
    }

    var Chronic_illness = $("#famregistration_form #fam3chronic_illness").val();

    if (Chronic_illness === "") {
        $("#famregistration_form #fam3chronic_illness").after("<span class='errormsg'>Please enter your Chronic illness</span>");
        flag++;
    }

    var heart_ailment = $("#famregistration_form #fam3heart_ailment").val();

    if (heart_ailment === "") {
        $("#famregistration_form #fam3heart_ailment").after("<span class='errormsg'>Please enter your heart ailment</span>");
        flag++;
    }
    var fainting_episodes = $("#famregistration_form #fam3fainting_episodes").val();

    if (fainting_episodes === "") {
        $("#famregistration_form #fam3fainting_episodes").after("<span class='errormsg'>Please enter your fainting episodes</span>");
        flag++;
    }

    var Any_other_ailment = $("#famregistration_form #fam3other_ailment").val();

    if (Any_other_ailment === "") {
        $("#famregistration_form #fam3other_ailment").after("<span class='errormsg'>Please enter your any other ailment</span>");
        flag++;
    }
    var Any_other_allergies = $("#famregistration_form #fam3allergies").val();

    if (Any_other_allergies === "") {
        $("#famregistration_form #fam3allergies").after("<span class='errormsg'>Please enter your any other allergies</span>");
        flag++;
    }
    return flag;
}

function fampersonalcheck() {
    $('#famregistration_form .errormsg').remove();
    var flag = 0;
    var first_name = $("#famregistration_form #famfirst_name").val();

    if (first_name === "") {
        $("#famregistration_form #famfirst_name").after("<span class='errormsg'>Please enter your Name</span>");
        flag++;
    }
    var last_name = $("#famregistration_form #famlast_name").val();
    if (last_name === "") {
        $("#famregistration_form #famlast_name").after("<span class='errormsg'>Please enter your Last Name</span>");
        flag++;
    }
    var dob = $("#famregistration_form #famdob").val();
    if (dob === "") {
        $("#famregistration_form #famdob").after("<span class='errormsg'>Please enter your Date of Birth</span>");
        flag++;
    }
    var employee_id_error = $(".famemployee_id_error").text();
    if (employee_id_error !== "") {
        flag++;
    }



    if ($("#new_participant #select_distance").val().indexOf("42") !== -1) {
        var RaceCertificate = $("#famregistration_form #famRaceCertificate").val();
        if (RaceCertificate === "") {
            $("#famregistration_form #famRaceCertificate").after("<span class='errormsg'>Please upload Race Certificate.</span>");
            flag++;
        }

        var DetailsOfFullHalfMarathon = $("#famregistration_form #famDetailsOfFullHalfMarathon").val();
        if (DetailsOfFullHalfMarathon === "") {
            $("#famregistration_form #famDetailsOfFullHalfMarathon").after("<span class='errormsg'>Please enter Details Of Full Half Marathon.</span>");
            flag++;
        }
    }


    if ($("#new_participant #select_distance").val().indexOf(' ') !== -1) {
        var dob1 = new Date(dob);

        var distance = $("#new_participant #select_distance").val().split(' ')[0];

        if (new Date("2001-11-24") < new Date($("#famregistration_form #famdob").val().split("/").reverse().join("-"))) {
            $("#famregistration_form #famdob").after("<span class='errormsg'>For first participant, Age must be 18 years old.</span>");
            flag++;

        }


        /*if (distance === 5) {
            if (new Date("2009-11-24") < new Date($("#famregistration_form #famdob").val().split("/").reverse().join("-"))) {
                $("#famregistration_form #famdob").after("<span class='errormsg'>For 5 KM race, Age must be 10 years old.</span>");
                flag++;
            }
        }
        else if (distance === 10) {
            if (new Date("2007-11-24") < new Date($("#famregistration_form #famdob").val().split("/").reverse().join("-"))) {
                $("#famregistration_form #famdob").after("<span class='errormsg'>For 10 KM race, Age must be 12 years old.</span>");
                flag++;

            }
        }
        else if (distance === 21.097) {
            if (new Date("2001-11-24") < new Date($("#famregistration_form #famdob").val().split("/").reverse().join("-"))) {
                $("#famregistration_form #famdob").after("<span class='errormsg'>For 21 KM race, Age must be 18 years old.</span>");
                flag++;

            }
        }
        else if (distance === 42.195) {
            if (new Date("2001-11-24") < new Date($("#famregistration_form #famdob").val().split("/").reverse().join("-"))) {
                $("#famregistration_form #famdob").after("<span class='errormsg'>For 42 KM race, Age must be 18 years old.</span>");
                flag++;

            }
        }*/

    }

    var gender = $("#famregistration_form #fam2gender_list").val();
    if (gender === "") {
        $("#famregistration_form #fam2gender_list").after("<span class='errormsg'>Please select gender</span>");
        flag++;
    }


    var email_id = $("#famregistration_form #famemail_id").val();
    if (email_id === "") {
        $("#famregistration_form #famemail_id").after("<span class='errormsg'>Please enter your Email Id</span>");
        flag++;
    }
    if (!validateEmail(email_id) && email_id !== "") {
        $("#famregistration_form #famemail_id").after("<span class='errormsg'>Please enter valid email address</span>");
        flag++;
    }
    var contactno = $("#famregistration_form #famcontactno").val();
    if (contactno === "") {
        $("#famregistration_form #famcontactno").after("<span class='errormsg'>Please enter your Contact Number</span>");
        flag++;
    }
    if (document.famregistration_form.famcontactno.value.length !== 10 && contactno !== "") {
        $("#famregistration_form #famcontactno").after("<span class='errormsg'>Contact Number should be of 10 digit</span>");
        flag++;
    }
    var nationality = $("#famregistration_form #famnationality").val();
    if (nationality === "") {
        $("#famregistration_form #famnationality").after("<span class='errormsg'>Please select your Nationality</span>");
        flag++;
    }
    var tshirt_sizeMale = $("#famregistration_form #famtshirt_sizeMale").val();
    if (tshirt_sizeMale === "") {
        $("#famregistration_form #famtshirt_sizeMale").after("<span class='errormsg'>Please select your Tshirt Size</span>");
        flag++;
    }
    var idproof = $("#famregistration_form #famidproof").val();
    if (idproof === "") {
        $("#famregistration_form #famidproof").after("<span class='errormsg'>Please select your valid ID Proof </span>");
        flag++;
    }
    var idnumber = $("#famregistration_form #famidnumber").val();
    if (idnumber === "") {
        $("#famregistration_form #famidnumber").after("<span class='errormsg'>Please enter ID Number</span>");
        flag++;
    }

    var file1 = $("#famregistration_form #famfile1").val();
    if (file1 === "") {
        $("#famregistration_form #famfile1").after("<span class='errormsg'>Please upload scanned copy for Identity Proof </span>");
        flag++;
    }


    var blood_group = $("#famregistration_form #famblood_group").val();
    if (blood_group === "") {
        $("#famregistration_form #famblood_group").after("<span class='errormsg'>Please select your Blood Group</span>");
        flag++;
    }

    var Chronic_illness = $("#famregistration_form #famchronic_illness").val();

    if (Chronic_illness === "") {
        $("#famregistration_form #famchronic_illness").after("<span class='errormsg'>Please enter your Chronic illness</span>");
        flag++;
    }

    var heart_ailment = $("#famregistration_form #famheart_ailment").val();

    if (heart_ailment === "") {
        $("#famregistration_form #famheart_ailment").after("<span class='errormsg'>Please enter your heart ailment</span>");
        flag++;
    }
    var fainting_episodes = $("#famregistration_form #famfainting_episodes").val();

    if (fainting_episodes === "") {
        $("#famregistration_form #famfainting_episodes").after("<span class='errormsg'>Please enter your fainting episodes</span>");
        flag++;
    }

    var Any_other_ailment = $("#famregistration_form #famother_ailment").val();

    if (Any_other_ailment === "") {
        $("#famregistration_form #famother_ailment").after("<span class='errormsg'>Please enter your any other ailment</span>");
        flag++;
    }
    var Any_other_allergies = $("#famregistration_form #famallergies").val();

    if (Any_other_allergies === "") {
        $("#famregistration_form #famallergies").after("<span class='errormsg'>Please enter your any other allergies</span>");
        flag++;
    }
    return flag;
}

$("#btnSendOTP").click(function (e) {
    e.preventDefault();
    $('#new_participant .errormsg').remove();
    var email_id = $("#new_participant #checkEmail").val();
    if (email_id === "") {
        $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter your Email Id</span>");
        $("#new_participant #checkEmail").focus();
    }
    else if (!validateEmail(email_id) && email_id !== "") {
        $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter valid email address</span>");
        $("#new_participant #checkEmail").focus();
    }
    else {
        $('#dloader').css('display', 'block');
        $.ajax({
            url: "/api/Marathon/SendEmailOTP",
            contentType: "application/json",
            type: 'POST',
            data: JSON.stringify({ 'Email': email_id }),
            dataType: 'json',
            success: function (data) {
                if (data === 1) {
                    $('#dloader').css('display', 'none');
                    alert("OTP has been sent successfully to you email id:" + email_id);
                }
                else if (data === 3) {
                    $('#dloader').css('display', 'none');
                    alert("Invalid Session!");
                    location.reload();
                }
                else if (data === 4) {
                    $('#dloader').css('display', 'none');
                    alert("You have exceeded OTP attempts. Please try after 30 min");
                    location.reload();
                }
                else {
                    $('#dloader').css('display', 'none');
                    alert("OTP has been sent successfully to you email id:" + email_id);
                }
            },
            error: function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }

});
$("#select_distancenew").change(function (e) {
    if ($("#DateSlotListNew").length) {
        var runMode = $("#RunmodeListNew").val();
        if (runMode === "Physical Run") {
            var distance = $("#select_distancenew").val();

            $('#DateSlotListNew').text('');
            $('#DateSlotListNew').append("<option value=''> Select your run date </option>");

            $('#TimeSlotListNew').text('');
            $('#TimeSlotListNew').append("<option value=''> Select your time slot </option>");
            if (distance !== "") {
                $('#dloader').css('display', 'block');
                $.ajax({
                    url: "/api/Marathon/GETRunDateList",
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify({ 'Distance': distance }),
                    dataType: 'json',
                    success: function (data) {
                        if (data.length > 0) {

                            //$("<option class='dateslot' value=''>Select your run date</option>").insertAfter("#DateSlotList");
                            $('#DateSlotListNew').text('');
                            $('#DateSlotListNew').append("<option value=''> Select your run date </option>");
                            for (var i = 0; i < data.length; i++) {
                                var name = data[i];
                                $('#DateSlotListNew').append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                            }
                        }
                        else {
                            alert("error! No run date found");
                        }
                    },
                    error: function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    },
                    complete: function () {
                        $('#dloader').css('display', 'none');
                    }
                });
            }
        }
    }
});
$("#DateSlotListNew").change(function (e) {
    if ($("#DateSlotListNew").length) {
        if ($("#RunmodeListNew").val() === "Physical Run") {
            var distance = $("#select_distancenew").val();
            var selectedDate = $("#DateSlotListNew").val();
            $('.timeslot').remove();
            $('#TimeSlotListNew').text('');
            $('#TimeSlotListNew').append("<option value=''> Select your time slot </option>");
            if (distance !== "" && selectedDate !== "") {
                var RunMode = $("input[name='RunMode']:checked").val();
                $('#dloader').css('display', 'block');
                $.ajax({
                    url: "/api/Marathon/GETRunTimeList",
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify({ 'RunMode': RunMode, 'Distance': distance, 'RunDate': selectedDate }),
                    dataType: 'json',
                    success: function (data) {
                        if (data.length > 0) {
                            $('.timeslot').remove();
                            //$("<option class='dateslot' value=''>Select your run date</option>").insertAfter("#DateSlotList");
                            $('#TimeSlotListNew').text('');
                            $('#TimeSlotListNew').append("<option value=''> Select your time slot </option>");
                            for (var i = 0; i < data.length; i++) {
                                if (data[i].Disabled) {
                                    $('#TimeSlotListNew').append("<option value='" + data[i].Value + "' disabled>" + data[i].Value + "</option>");
                                }
                                else {
                                    $('#TimeSlotListNew').append("<option value='" + data[i].Value + "'>" + data[i].Value + "</option>");
                                }
                            }
                        }
                        else {
                            alert("error! No run date found");
                        }
                    },
                    error: function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    },
                    complete: function () {
                        $('#dloader').css('display', 'none');
                    }
                });
            }
        }
    }
});
$("#select_distance").change(function (e) {
    if ($("#DateSlotList").length) {
        if ($("input[name='RunMode']:checked").val() === "Physical") {
            var distance = $("#select_distance").val();
            $('.dateslot').remove();
            $('#DateSlotList').text('');
            $('#DateSlotList').append("<option value=''> Select your run date </option>");
            $('.timeslot').remove();
            $('#TimeSlotList').text('');
            $('#TimeSlotList').append("<option value=''> Select your time slot </option>");
            if (distance !== "") {
                $('#dloader').css('display', 'block');
                $.ajax({
                    url: "/api/Marathon/GETRunDateList",
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify({ 'Distance': distance }),
                    dataType: 'json',
                    success: function (data) {
                        if (data.length > 0) {
                            $('.dateslot').remove();
                            //$("<option class='dateslot' value=''>Select your run date</option>").insertAfter("#DateSlotList");
                            $('#DateSlotList').text('');
                            $('#DateSlotList').append("<option value=''> Select your run date </option>");
                            for (var i = 0; i < data.length; i++) {
                                var name = data[i];
                                $('#DateSlotList').append("<option value='" + data[i].Value + "'>" + data[i].Text + "</option>");
                            }
                        }
                        else {
                            alert("error! No run date found");
                        }
                    },
                    error: function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    },
                    complete: function () {
                        $('#dloader').css('display', 'none');
                    }
                });
            }
        }
    }
});
$("#DateSlotList").change(function (e) {
    if ($("#DateSlotList").length) {
        if ($("input[name='RunMode']:checked").val() === "Physical") {
            var distance = $("#select_distance").val();
            var selectedDate = $("#DateSlotList").val();
            $('.timeslot').remove();
            $('#TimeSlotList').text('');
            $('#TimeSlotList').append("<option value=''> Select your time slot </option>");
            if (distance !== "" && selectedDate != "") {
                var RunMode = $("input[name='RunMode']:checked").val();
                $('#dloader').css('display', 'block');
                $.ajax({
                    url: "/api/Marathon/GETRunTimeList",
                    contentType: "application/json",
                    type: 'POST',
                    data: JSON.stringify({ 'RunMode': RunMode, 'Distance': distance, 'RunDate': selectedDate }),
                    dataType: 'json',
                    success: function (data) {
                        if (data.length > 0) {
                            $('.timeslot').remove();
                            //$("<option class='dateslot' value=''>Select your run date</option>").insertAfter("#DateSlotList");
                            $('#TimeSlotList').text('');
                            $('#TimeSlotList').append("<option value=''> Select your time slot </option>");
                            for (var i = 0; i < data.length; i++) {
                                if (data[i].Disabled) {
                                    $('#TimeSlotList').append("<option value='" + data[i].Value + "'>" + data[i].Value + "</option>");
                                }
                                else {
                                    $('#TimeSlotList').append("<option value='" + data[i].Value + "'>" + data[i].Value + "</option>");
                                }
                            }
                        }
                        else {
                            alert("error! No run date found");
                        }
                    },
                    error: function (request, error) {
                        alert("Request: " + JSON.stringify(request));
                    },
                    complete: function () {
                        $('#dloader').css('display', 'none');
                    }
                });
            }
        }
    }
});
$("#btnValidateOTP").click(function (e) {
    e.preventDefault();
    $('#new_participant .errormsg').remove();
    var email_id = $("#new_participant #checkEmail").val();
    var email_otp = $("#new_participant #emailotp").val();
    if (email_id === "") {
        $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter your Email Id</span>");
        $("#new_participant #checkEmail").focus();
        return false;
    }
    else if (!validateEmail(email_id) && email_id !== "") {
        $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter valid email address</span>");
        $("#new_participant #checkEmail").focus();
        return false;
    }
    else if (email_otp === "") {
        $("#new_participant #emailotp").after("<span class='errormsg'>Please enter 6 digit OTP sent to your Email id.</span>");
        $("#new_participant #emailotp").focus();
        return false;
    }
    else {
        $.ajax({
            url: "/api/Marathon/VerifyEmailOTP",
            contentType: "application/json",
            type: 'POST',
            data: JSON.stringify({ 'Email': email_id, 'OTP': email_otp }),
            dataType: 'json',
            success: function (data) {
                if (data.status === 1) {
                    alert(email_id + "validated successfully.");
                }
                else if (data.status === 2) {
                    $("#confirmation").show();
                    return false;
                }
                else if (data.status === 3) {
                    alert("Email already registerd, Please try with other email id.");
                    return false;
                }
                else {
                    alert("An error occured, OTP sending failed. Please try again later");
                    return false;
                }
            },
            error: function (request, error) {
                alert("Request: " + JSON.stringify(request));
            }
        });
    }

});


$("#famsecond-prev").click(function () {


    $(".famaddress").hide();
    $(".famthird").show();
    $(".famli-address").addClass("active_fieldset");

    $(".fam3li-personal").removeClass("active_fieldset");
    $('#famstate_info').focus();

});


$("#fam2-prev").click(function () {


    $(".famsecond").hide();
    $(".famfirst").show();
    $(".fam2li-personal").addClass("active_fieldset");

    $(".famli-personal").removeClass("active_fieldset");
    $('#famstate_info').focus();

});

$("#fam3-prev").click(function () {


    $(".famthird").hide();
    $(".famsecond").show();
    $(".fam3li-personal").addClass("active_fieldset");

    $(".famli2-personal").removeClass("active_fieldset");
    $('#famstate_info').focus();

});


$('#select_distance').each(function (index, el) {
    $(this).parent().append('<label></label>');
});
/* $('#login').on('click', function(){
	//var field_set = $(this).closest('fieldset');
	$('label.error').removeClass('error').text('');
	$('.validated').each(function(){
		var this_place = $(this).attr('data-error-msg');
		var input_val = $(this).val();
		if (input_val === "") {
			$(this).siblings('label').addClass('error').text(this_place);
			return false;
		}
	});
}); */
var coupon_data;

$(document).ready(function () {


    var CountdownRace = $('#CountdownRace');

    if (CountdownRace.length === 1) {

        var time = $('#CountdownRaceHidden').val();
        //  var countDownDate = new Date(time).getTime();

        var countDownDate = new Date('26 November 2023 05:00:00').getTime();
        var x = setInterval(function () {

            // Get today's date and time
            var now = new Date().getTime();

            // Find the distance between now and the count down date
            var distance = countDownDate - now;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);


            // Display the result in the element with id="demo"
            document.getElementById("CountdownRace").innerHTML = days + " Days - " + hours + " Hours - "
                + minutes + " Minutes - " + seconds + " Seconds";


            var raceover = days + " Days / " + hours + " Hours / "
                + minutes + " Minutes / " + seconds + " Seconds";

            // If the count down is finished, write some text 
            if (distance < 0) {
                clearInterval(x);
                document.getElementById("CountdownRace").innerHTML = raceover;
            }
        }, 1000);

    }
    UserInfoField();
    $('#example').DataTable();
    diablekey();
});

	$(document).ready(function () {


    var CountdownRace2 = $('#CountdownRace2');

    if (CountdownRace2.length === 1) {

        var time = $('#CountdownRaceHidden2').val();
        //  var countDownDate2 = new Date(time).getTime();

        var countDownDate2 = new Date('09 October 2022 05:00:00').getTime();
        var x = setInterval(function () {

            // Get today's date and time
            var now = new Date().getTime();

            // Find the distance between now and the count down date
            var distance = countDownDate2 - now;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);


            // Display the result in the element with id="demo"
            document.getElementById("CountdownRace2").innerHTML = days + " Days | " + hours + " Hours | "
                + minutes + " Minutes | " + seconds + " Seconds";


            var raceover = days + " DAYS / " + hours + " Hours / "
                + minutes + " Minutes / " + seconds + " Seconds";

            // If the count down is finished, write some text 
            if (distance < 0) {
                clearInterval(x);
                document.getElementById("CountdownRace2").innerHTML = raceover;
            }
        }, 1000);

    }
    UserInfoField();
    $('#example').DataTable();
    diablekey();
});

function UserInfoField() {
	/*$('.userInfoPage select').attr('disabled','true')
$('.userInfoPage input').attr('disabled','true')

$('.userInfoPage input[type="radio"]').removeAttr('disabled')
$('.userInfoPage input[type="button"]').removeAttr('disabled')
$('.userInfoPage input[type="submit"]').removeAttr('disabled')
$('.userInfoPage input[type="checkbox"]').removeAttr('disabled')
$('#tshirt_sizeMale').removeAttr('disabled')*/
}

function showhideRaceCertificateDiv() {
    $(".DetailsOfFullHalfMarathon_row ").hide();
    $(".RaceCertificate_row ").hide();

    if ($("#select_distance option:selected").val().indexOf("42") !== -1) {
        $(".DetailsOfFullHalfMarathon_row ").show();
        $(".RaceCertificate_row ").show();
    }

}


function userInfoValues() {
    var SelectedNationality = $('#Nationality').val();
    var SelectedDateofBirth = $('#DateofBirth').val();
    var formattedDOB = SelectedDateofBirth.split("-").reverse().join("/");
    var SelectedTshirtSize = $('#TShirtSize').val();
    var SelectedCountry = $('#Country').val();
    var SelectedIDProof = $('#IdentityProofType').val();
    var SelectedBloodGroup = $('#BloodGroup').val();
    var SelectedRaceDistance = $('#RaceDistance').val();
    var SelectedRaceAmount = $('#RaceAmount').val();
    var Selectedpayroll_company = $('#PayrollCompany').val();
    var SelectedGender = $('#Gender').val();
    var SelectedDefencePersonnel = $("#DefencePersonnel").val();
    var catvalueee = "";
    var catmargevalue = SelectedRaceDistance.trim();// + "#" + SelectedRaceAmount.trim();
    var arr = $("#select_distance option").map(function () { return $(this).val(); }).get();
    var arrl = arr.length;
    for (var i = 0; i < arrl; i++) {
        if (arr[i].trim().split('#')[0] === catmargevalue) {
            catvalueee = arr[i];
        }
    }
    var SelectedState = $('#State').val();
    var SelectedCity = $('#City').val();

    $('#dob').val(formattedDOB);
    $('#select_distance').val(catvalueee);
    $('#defencePersonnel').val(SelectedDefencePersonnel);
    $('#payroll_company').val(Selectedpayroll_company);
    $('#nationality').val(SelectedNationality);
    $('#idproof').val(SelectedIDProof);
    $('#country_info').val(SelectedCountry);
    $('#blood_group').val(SelectedBloodGroup);
    $('#tshirt_sizeMale').val(SelectedTshirtSize);
    $('#gender_list').val(SelectedGender);
    $('#city_info').val(SelectedCity);
    $('#state_info').val(SelectedState);
    //if (SelectedGender === "Male")
    //    $("#male").prop("checked", true);
    //else if (SelectedGender === "Female")
    //    $("#female").prop("checked", true);
    //else if (SelectedGender === "Transgender")
    //    $("#transgender").prop("checked", true);
    showhideRaceCertificateDiv();
}

$("#select_distance").change(function () {
    showhideRaceCertificateDiv();
});



$('form').on('focus', 'input[type=number]', function (e) {
    $(this).on('wheel.disableScroll', function (e) {
        e.preventDefault()
    })
});
$('form').on('blur', 'input[type=number]', function (e) {
    $(this).off('wheel.disableScroll')
});
$("#first_btn").click(function (e) {
    var checkStatus;
	if($("#registration_form #select_distance").val()=='' || $("#registration_form #select_distance").val()==undefined)
	{
		$("#registration_form #select_distance").val($("#registration_form #select_distance_charity").val())
	}
    $("#page1msg").css("display", "none");
    if ($('.userInfoPage').length === 1)
        checkStatus = 0;
    else
        checkStatus = rundetailscheck();

	

    if (checkStatus === 0) {
		
        $("#st2").trigger('click');
        //$(".second").show();
        //$(".first").hide();
        //// $(".first").hide();
        // $(".li-address").addClass("active_fieldset");
        //$(".li-medical").removeClass("active_fieldset");
        //$(".li-personal").addClass("active_fieldset");
        //$('#first_name').focus();
    }
    else {
        $("#first_btn").after("<span class='errormsg'>This page has some errors.</span>");
    }
});

/*file mime typ code start*/
function fileValidation(event, fileId) {
    var file = event.target.files[0];
    if (file.size >= 10 * 1024 * 1024) {
        alert("maximum file size can be 10 MB");
        $("#" + fileId).val(''); //the tricky part is to "empty" the input file here I reset the form.
        return;
    }

    if (file.type.match('image/jp.*') == null && file.type.match('application/pdf') == null && file.type.match('image/png') == null) {
        alert("only PDF,JPEG,JPG,PNG files allowed");
        $("#" + fileId).val(''); //the tricky part is to "empty" the input file here I reset the form.
        return;
    }

    if (file.type !== 'image/png') {
        var fileReader = new FileReader();
        fileReader.onload = function (e) {
            var int32View = new Uint8Array(e.target.result);
            //verify the magic number
            // for JPG is 0xFF 0xD8 0xFF 0xE0 (see https://en.wikipedia.org/wiki/List_of_file_signatures)
            if (int32View.length > 4 && int32View[0] == 0x25 && int32View[1] == 0x50 && int32View[2] == 0x44 && int32View[3] == 0x46) {
                console.log("ok");
            }
            else if (int32View.length > 4 && int32View[0] == 0xFF && int32View[1] == 0xD8 && int32View[2] == 0xFF && int32View[3] == 0xE1) {
                console.log("jpg-ok");
            }
            else if (int32View.length > 4 && int32View[0] == 0xFF && int32View[1] == 0xD8 && int32View[2] == 0xFF && int32View[3] == 0xDB) {
                console.log("jpg-ok");
            }
            else if (int32View.length > 4 && int32View[0] == 0xFF && int32View[1] == 0xD8 && int32View[2] == 0xFF && int32View[3] == 0xE0) {
                console.log("jpg-ok");
            }
            else if (int32View.length > 4 && int32View[0] == 0xFF && int32View[1] == 0xD8 && int32View[2] == 0xFF && int32View[3] == 0xEE) {
                console.log("jpg-ok");
            }
            else if (int32View.length > 4 && int32View[0] == 0x49 && int32View[1] == 0x46 && int32View[2] == 0x00 && int32View[3] == 0x11) {
                console.log("jpg-ok");
            }
            else if (int32View.length > 4 && int32View[0] == 0x69 && int32View[1] == 0x66 && int32View[2] == 0x00 && int32View[3] == 0x00) {
                console.log("jpg-ok");
            }
            else {
                alert("only valid JPG images");
                $("#" + fileId).val(''); //the tricky part is to "empty" the input file here I reset the form.
                return;
            }
        };
    }
    fileReader.readAsArrayBuffer(file);
}
//$("#file1").on('change', function (event) {
//    fileValidation(event, this.id);
//});
$("#Covidfile").on('change', function (event) {
    fileValidation(event, this.id);
});
$("#file2").on('change', function (event) {
    fileValidation(event, this.id);
});
$("#RaceCertificate").on('change', function (event) {
    fileValidation(event, this.id);
});
/*file mime type code end*/

$("#second-next").click(function () {
	var j=0;
    var checkStatus = personalcheck();
    if (checkStatus === 0) {
        $("#st3").trigger('click');
		if(location.href.toString().toLowerCase().includes('userinfo'))
		{
			for(i=0;$('#city_info option').length;i++)
			{
				if($('#city_info option')[i].text.trim()==$('#City').val().trim())
				{
					if(j==0)
					{
							$('#city_info option')[i].remove()
					}
					else{
						$('#city_info option')[i].selected='true';
						break;
					}
					j++;
				}
			}
		}
        $(".third").show();
        $(".second").hide();
        $(".li-address").addClass("active_fieldset");
        $(".li-medical").addClass("active_fieldset");
        $(".li-personal").removeClass("active_fieldset");
        $(".li-race").removeClass("active_fieldset");
        $('#state_info').focus();
    }
});
$("#third-next").click(function () {
    var checkStatus = addresscheck();
    if (checkStatus === 0) {
        $("#st4").trigger('click');
        $(".four").show();
        $(".third").hide();
        $(".li-address").removeClass("active_fieldset");
        $(".li-medical").addClass("active_fieldset");
        $(".li-personal").removeClass("active_fieldset");
        $(".li-race").removeClass("active_fieldset");
        $('#emergency_contact_name').focus();
    }
});
$("#four-next").click(function () {
	$('.conformationDOB').val($('#dob').val())
    var checkStatus = MedicalInformation();
    if (checkStatus === 0) {
        $("#st5").trigger('click');
        $(".five").show();
        var city = $("#city_info :selected").val();
        $("#state_id").val($('#state_info :selected').val());
        loadCity(city);
        $("#city_id").text(city);
        $("#country_id").val($('#country_info :selected').text());
        $("#address_id").val($("#address").val());
        $("#pincode_id").val($("#pincode").val());
        $("#emergency_contact_name_id").val($("#emergency_contact_name").val());
        $("#emergency_contact_number_id").val($("#emergency_contact_number").val());
        $("#idproof_id").val($('#idproof :selected').val());
        $("#blood_group_id").val($('#blood_group :selected').val());
        $("#contactno_id").val($("#contactno").val());
        $("#f_name").val($("#first_name").val());
        $("#l_name").val($("#last_name").val());
        $("#gender_li").val($('#gender_list :selected').val());
        $("#email_idd").val($("#email_id").val());
        $("#nationality_id").val($('#nationality :selected').val());
        $("#disclaimer_id").prop('checked', true);

        //$("#registration_form :input").each(function () {
        //    if ($(this).attr("id") === "city_info") {
        //        $("#city_id").val($('#city_info :selected').val());
        //        //$("#city_id").val($("select#city_info option:checked").val()); //val($("#city_info").val());
        //    }
        //}
    }
    else {
        $("#four-next").after("<span class='errormsg'>This page has some errors.</span>");
    }
});
$("#second-prev").click(function () {
    $(".first").show();
    $(".second").hide();
    $(".li-address").removeClass("active_fieldset");
    $(".li-medical").removeClass("active_fieldset");
    $(".li-personal").removeClass("active_fieldset");
    $(".li-race").addClass("active_fieldset");
    $('#DateSlotList').focus();
});
$("#third-prev").click(function () {
    $(".third").hide();
    $(".second").show();
    $(".li-address").removeClass("active_fieldset");
    $(".li-medical").removeClass("active_fieldset");
    $(".li-personal").addClass("active_fieldset");
    $(".li-race").removeClass("active_fieldset");
    $('#first_name').focus();
});
$("#four-prev").click(function () {
    $(".third").show();
    $(".four").hide();
    $(".li-address").addClass("active_fieldset");
    $(".li-medical").removeClass("active_fieldset");
    $(".li-personal").removeClass("active_fieldset");
    $(".li-race").removeClass("active_fieldset");
    $('#state_info').focus();
});
$("#five-prev").click(function () {
    $(".four").show();
    $(".five").hide();
    $(".li-address").removeClass("active_fieldset");
    $(".li-medical").addClass("active_fieldset");
    $(".li-personal").removeClass("active_fieldset");
    $(".li-race").removeClass("active_fieldset");
    $('#emergency_contact_name').focus();
});




$(".famfirst_btn1").click(function () {
    var checkStatus;

    if ($('.userInfoPage').length === 1)
        checkStatus = 0;
    else
        checkStatus = fampersonalcheck();

    if (checkStatus === 0) {
        $(".famsecond").show();
        $(".famfirst").hide();
        $(".famli-address").addClass("active_fieldset");

        $(".famli-personal").removeClass("active_fieldset");
        $('#famstate_info').focus();
    }
});








function uploadfile(obj) {
    alert($('#file1').text(this.val()));

}



function famMedicalInformation() {
    $('#famregistration_form .errormsg').remove();
    var flag = 0;
    var emergency_contact_name = $("#famregistration_form #famemergency_contact_name").val();
    if (emergency_contact_name === "") {
        $("#famregistration_form #famemergency_contact_name").after("<span class='errormsg'>Please enter Emergency Contact Name</span>");
        flag++;
    }
    var emergency_contact_relationship = $("#famregistration_form #famemergency_contact_relationship").val();
    if (emergency_contact_relationship === "") {
        $("#famregistration_form #famemergency_contact_relationship").after("<span class='errormsg'>Please enter Emergency Contact Relationship</span>");
        flag++;
    }
    var emergency_contact_number = $("#famregistration_form #famemergency_contact_number").val();
    if (emergency_contact_number === "") {
        $("#famregistration_form #famemergency_contact_number").after("<span class='errormsg'>Please enter Emergency Contact Number</span>");
        flag++;
    }
    if (document.famregistration_form.famemergency_contact_number.value.length !== 10 && famemergency_contact_number !== "") {
        $("#famregistration_form #famemergency_contact_number").after("<span class='errormsg'>Emergency Contact Number should be of 10 digit</span>");
        flag++;
    }
    var blood_group = $("#famregistration_form #famblood_group").val();
    if (blood_group === "") {
        $("#famregistration_form #famblood_group").after("<span class='errormsg'>Please select your Blood Group</span>");
        flag++;
    }
    return flag;
}

$("#fambtnRegistrationSubmit").click(function () {



    var checkStatus = famMedicalInformation();
    if (checkStatus !== 0) {
        return false;
    }
    var agreeterms = $("input[name='famdisclaimer']:checked").val();
    if (agreeterms === undefined || agreeterms === "") {
        $("input[name='famdisclaimer']").after("<label id='agree-error' class='error' for='agree'>Please accept the terms and conditions.</label>");
        $('#fambtnRegistrationSubmit').removeAttr("disabled"); return false;
    }


    //  $('#fambtnRegistrationSubmit').attr("disabled", "disabled");

    var category = $("#new_participant #select_distance").val();
    if (category === "") {
        alert("Please select category"); $("#new_participant #select_distance").focus();
        //   $('#fambtnRegistrationSubmit').removeAttr("disabled");
        return false;
    }
    else {
        var arr = category.split("#");
        document.getElementById("famraceDistance").value = arr[0];
        document.getElementById("famraceAmount").value = arr[1];
        // document.getElementById("rstat").value = "no";
    }
});









$("#btnTrainingRegister").click(function () {

    if ($("input[name='select_option']:checked").val() === undefined || $("input[name='select_option']:checked").val() === "") {
        alert("Please select the tracking option.")
        return false;
    }
    var rsDistance = $("input[name='select_option']:checked").val();
    var rsUfname = $.trim($("#first_name").text());
    var rsUlname = $.trim($("#last_name").text());
    var rsUemail = $.trim($("#email_id").text());
    var rsUcontactno = $.trim($("#contactno").text());
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');



    var savecustomdata = {
        RaceDistance: 'NA',
        Rstat: rsDistance,
        FirstName: rsUfname,
        LastName: rsUlname,
        Email: rsUemail,
        ContactNumber: rsUcontactno,
        FormSubmitOn: currentdate
    };




    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Marathon/InsertTrainingRegistration",
        contentType: "application/json",
        success: function (data) {
            //////////////
            window.location.href = "/registration-thankyou";
            /* if (data.status === "1") {
 
                 window.location.href = "/registration-thankyou";
             }
             else {
                 alert("Sorry Operation Failed!!! Please try again later");
                 $('#btnRegistrationSubmit').removeAttr("disabled");
                 return false;
             }*/
        },
        error: function (request, error) {
            console.log("Request: " + JSON.stringify(request));
            console.log(error);
            window.location.href = "/registration-thankyou";
        }
    });

    return 0;
});






$("#btnTrainingOpnRegister").click(function () {

    if ($("input[name='select_option']:checked").val() === undefined || $("input[name='select_option']:checked").val() === "") {
        alert("Please select the tracking option.")
        return false;
    }
    var rsDistance = $("input[name='select_option']:checked").val();
    var rsUfname = $.trim($("#first_name").text());
    var rsUlname = $.trim($("#last_name").text());
    var rsRegistartionNo = $.trim($("#RegistrationId").text());
    var rsUemail = $.trim($("#email_id").text());
    var rsUcontactno = $.trim($("#contactno").text());
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');



    var savecustomdata = {
        RaceDistance: 'NA',
        Rstat: rsDistance,
        FirstName: rsUfname,
        AffiliateCode: rsRegistartionNo,
        LastName: rsUlname,
        Email: rsUemail,
        ContactNumber: rsUcontactno,
        FormSubmitOn: currentdate
    };




    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Marathon/InsertTrainingOption",
        contentType: "application/json",
        success: function (data) {
            //////////////
            window.location.href = "/registration-thankyou";
            /* if (data.status === "1") {
 
                 window.location.href = "/registration-thankyou";
             }
             else {
                 alert("Sorry Operation Failed!!! Please try again later");
                 $('#btnRegistrationSubmit').removeAttr("disabled");
                 return false;
             }*/
        },
        error: function (request, error) {
            console.log("Request: " + JSON.stringify(request));
            console.log(error);
            window.location.href = "/registration-thankyou";
        }
    });

    return 0;
});


$('input[type=radio][name=Vaccinationted]').change(function () {
    var idVal = $(this).val();
    if (idVal === "Yes") {
        $(".Covidfile").css("display", "block");
        $(".covidMsg").css("display", "none");
    }
    else if (idVal === "No") {
        $(".Covidfile").css("display", "none");
        $(".covidMsg").css("display", "block");
        $("#Covidfile").next(".errormsg").remove();
    }
});

$(function () {
    $('input').blur(function () {
        $(this).val(
            $.trim($(this).val())
        );
    });
});




$("#btnRegistrationSubmit").click(function () {
    var checkStatus = MedicalInformation();
    if (checkStatus !== 0) {
        return false;
    }
    $('#btnRegistrationSubmit').attr("disabled", "disabled");

    var category = $("#registration_form #select_distance").val();
    // if (category === "") {
        // alert("Please select category"); $("#registration_form #select_distance").focus();
        // $('#btnRegistrationSubmit').removeAttr("disabled"); return false;
    //}
    //else {
    //    var arr = category.split("#");
    //    document.getElementById("raceDistance").value = arr[0];
    //    document.getElementById("raceAmount").value = arr[1];
    //    // document.getElementById("rstat").value = "no";
    //}


    if (category.indexOf("42") !== -1) {
        if ($("#prevRaceCertificate").attr('href') === "") {
            var RaceCertificate = $("#registration_form #RaceCertificate").val();
            if (RaceCertificate === "") { alert("Please upload scanned copy of race certificate."); $("#registration_form #RaceCertificate").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }
        }
        var DetailsOfFullHalfMarathon = $("#registration_form #DetailsOfFullHalfMarathon").val();
        if (DetailsOfFullHalfMarathon === "") { alert("Please enter details Of Full/Half Marathon"); $("#registration_form #DetailsOfFullHalfMarathon").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    }


    var reference_code = $("#registration_form #reference_code").val();
    // if (reference_code === "") { alert("Please Enter reference code"); $("#registration_form #reference_code").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var first_name = $("#registration_form #first_name").val().toUpperCase();
    if (first_name === "") { alert("Please Enter first name"); $("#registration_form #first_name").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }


    var last_name = $("#registration_form #last_name").val().toUpperCase();
    if (last_name === "") { alert("Please Enter last name"); $("#registration_form #last_name").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }


    var dob = $("#registration_form #dob").val();
    if (dob === "") { alert("Please Enter date of birth"); $("#registration_form #dob").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }



    if ($("#registration_form #select_distance").val().indexOf(' ') !== -1) {
        var dob1 = new Date(dob);

        var distance = $("#registration_form #select_distance").val().split(' ')[0];
        if ($("input[name='RunMode']:checked").val() === "Remote") {
            if (distance === 5) {
                if (new Date("2011-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    alert("For 5 KM race, Age must be 10 years old.");
                    $("#registration_form #dob").focus();
                    $('#btnRegistrationSubmit').removeAttr("disabled");
                    return false;
                }
            }
            else if (distance === 10) {
                if (new Date("2009-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    alert("For 10 KM race, Age must be 12 years old.");
                    $("#registration_form #dob").focus();
                    $('#btnRegistrationSubmit').removeAttr("disabled");
                    return false;
                }
            }
            else if (distance === 21.097) {
                if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    alert("For 21 KM race, Age must be 18 years old.");
                    $("#registration_form #dob").focus();
                    $('#btnRegistrationSubmit').removeAttr("disabled");
                    return false;
                }
            }
            else if (distance === 42.195) {
                if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    alert("For 42 KM race, Age must be 18 years old.");
                    $("#registration_form #dob").focus();
                    $('#btnRegistrationSubmit').removeAttr("disabled");
                    return false;
                }
            }
        }
        else {
            if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                alert("Age must be 18 years old.");
                $("#registration_form #dob").focus();
                $('#btnRegistrationSubmit').removeAttr("disabled");
                return false;
            }
        }


    }


    //dob = new Date(dob).getDate() + "-" + new Date(dob).getMonth()+ "-" + new Date(dob).getYear();



    var email_id = $("#registration_form #email_id").val();
    if (email_id === "") { alert("Please Enter email id"); $("#registration_form #email_id").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    if (!validateEmail(email_id)) { alert("Please enter valid email address."); $("#email_id").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }


    var email_domain = $('#registration_form #EmailDomain').val();
    if (email_domain !== "") {
        if (email_id.indexOf('@') !== -1) {
            var email = email_id.split('@')[1];
            if (email_domain !== email) {
                alert("Please enter valid email address domain.");
                $("#email_id").focus();
                $('#btnRegistrationSubmit').removeAttr("disabled");
                return false;
            }
        }
    }



    var contactno = $("#registration_form #contactno").val();
    if (contactno === "") { alert("Please Enter contact number"); $("#registration_form #contactno").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }
    else {
        if (contactno.length !== 10) {
            alert("Mobile Number sould be 10 digit!");
            $("#registration_form #contactno").focus();
            $('#btnRegistrationSubmit').removeAttr("disabled"); return false;

        }
    }
    var nationality = $("#registration_form #nationality").val();
    if (nationality === "") { alert("Please Enter nationality"); $("#registration_form #nationality").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var gender = $("#famregistration_form #fam2gender_list").val();
    if (gender === "") {
        alert("Please select gender"); $("#registration_form #gender_list").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false;
    }
    //var gender = $("input[name='Gender']:checked").val();
    //if (gender === undefined || gender === "") { alert("Please select gender"); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    if ($("input[name='RunMode']:checked").val() === "Physical") {
        var Vaccinationted = $("input[name='Vaccinationted']:checked").val();
        if (Vaccinationted === "yes") { $("#VaccinationCertificate").attr("required"); } else { $("#VaccinationCertificate").removeAttr("required"); }
    }

    var tshirt_sizeMale = $("#registration_form #tshirt_sizeMale").val();
    if (tshirt_sizeMale === "-- select one --") { alert("Please Enter t-shirt size"); $("#registration_form #tshirt_sizeMale").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var bibname = $("#registration_form #bibname").val();
    //  if (bibname === "") { alert("Please Enter name on BIB"); $("#registration_form #bibname").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }



    var idproof = $("#registration_form #idproof").val();
    if (idproof === "") { alert("Please select valid ID Proof"); $("#registration_form #idproof").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }


    var idnumber = $("#registration_form #idnumber").val();
    if (idnumber === "") { alert("Please enter Id Number"); $("#registration_form #idnumber").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var country_info = $("#registration_form #country_info").val();
    if (country_info === "") { alert("Please select your country"); $("#registration_form #country_info").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var state_info = $("#registration_form #state_info").val();
    if (state_info === "") { alert("Please select your state"); $("#registration_form #state_info").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var city_info = $("#registration_form #city_info").val();
    if (city_info === "") { alert("Please select your city"); $("#registration_form #city_info").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var address = $("#registration_form #address").val();
    if (address === "") { alert("Please enter your Address"); $("#registration_form #address").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var pincode = $("#registration_form #pincode").val();
    if (pincode === "") { alert("Please enter your pincode"); $("#registration_form #pincode").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var emergency_contact_name = $("#registration_form #emergency_contact_name").val();
    if (emergency_contact_name === "") { alert("Please enter emergency contact person name"); $("#registration_form #emergency_contact_name").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var emergency_contact_relationship = $("#registration_form #emergency_contact_relationship").val();
    if (emergency_contact_relationship === "") { alert("Please enter your relation with emergency contact person"); $("#registration_form #emergency_contact_relationship").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var emergency_contact_number = $("#registration_form #emergency_contact_number").val();
    if (emergency_contact_number === "") {
        alert("Please enter your emergency contact person number");
        $("#registration_form #emergency_contact_number").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false;
    }
    else {
        if (emergency_contact_number.length !== 10) {
            alert("Emergency Number sould be of 10 digit!");
            $("#registration_form #emergency_contact_number").focus();
            $('#btnRegistrationSubmit').removeAttr("disabled"); return false;

        }
    }
    var blood_group = $("#registration_form #blood_group").val();
    if (blood_group === "") { alert("Please enter your Blood Group"); $("#registration_form #blood_group").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var chronic_illness = $("#registration_form #chronic_illness").val();
    // if (chronic_illness === "") { alert("Please enter Chronic Illness"); $("#registration_form #chronic_illness").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var heart_ailment = $("#registration_form #heart_ailment").val();
    // if (heart_ailment === "") { alert("Please enter Heart Ailment"); $("#registration_form #heart_ailment").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var fainting_episodes = $("#registration_form #fainting_episodes").val();
    var other_ailment = $("#registration_form #other_ailment").val();
    var allergies = $("#registration_form #allergies").val();
    var currentdate = new Date().toISOString().slice(0, 19).replace('T', ' ');


    //var race_dist = '';
    //var race_amount = '';
    //var discount_rate = '';
    //var remaining_amount_status = '';



    //var fileUpload = $("#registration_form #file1").get(0);
    //var files = fileUpload.files;

    //// Create FormData object  
    //var fileData = new FormData();

    //// Looping over all files and add it to FormData object  
    //for (var i = 0; i < files.length; i++) {
    //    fileData.append(files[i].name, files[i]);
    //} 


    var agreeterms = $("input[name='agree']:checked").val();
    if (agreeterms === undefined || agreeterms === "") { alert("Please accept the terms and conditions"); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }

    var IDCardAttachment = $("#registration_form #file1").val();

    if ($('.userInfoPage').length === 0) {
        if (IDCardAttachment === "") {
            if ($("#registration_form #uploadedId").val() === 'undefined') { alert("Please upload scanned copy for Identity Proof "); $("#registration_form #file1").focus(); $('#btnRegistrationSubmit').removeAttr("disabled"); return false; }
        }
    }
    var employee_id = $("#registration_form #employee_id").val();

    var EmployeeID_Source = $('#registration_form #EmployeeIDSource').val();

    if (EmployeeID_Source !== "") {
        if (employee_id !== "") {
            if (EmployeeID_Source.indexOf(employee_id + ',') === -1) {
                alert("Please enter the valid employee ID");
                $('#btnRegistrationSubmit').removeAttr("disabled");
                return false;
            }
        }
        else {
            alert("Please enter the employee ID");
            $('#btnRegistrationSubmit').removeAttr("disabled");
            return false;
        }
    }

    var payroll_company = $("#registration_form #payroll_company").val();

    var unit_station = $("#registration_form #unit_station").val();

    var shantigram_id_proof = $("#registration_form #id_proof").val();


    var requiredFld = $(".requiredFld input");

    if (requiredFld !== null) {
        if (requiredFld.length !== 0) {
            for (var i = 0; i < requiredFld.length; i++) {
                if ($(requiredFld[i]).val() === "") {
                    alert("Reference field's information is required");
                    $('#btnRegistrationSubmit').removeAttr("disabled");
                    return false;
                }
            }

        }

    }
    $('#dloader').css('display', 'block');
    $('#registration_form').submit();
    return;




    //var savecustomdata = {
    //    Category: category,
    //    ReferenceCode: reference_code,
    //    FirstName: first_name,
    //    LastName: last_name,
    //    DateofBirth: dob,
    //    Email: email_id,
    //    ContactNumber: contactno,
    //    Nationality: nationality,
    //    Gender: gender,
    //    TShirtSize: tshirt_sizeMale,
    //    NamePreferredonBIB: bibname,
    //    IdentityProofType: idproof,
    //    IdentityProofNumber: idnumber,
    //    IDCardAttachment: IDCardAttachment,
    //    Country: country_info,
    //    State: state_info,
    //    City: city_info,
    //    Address: address,
    //    Pincode: pincode,
    //    EmergencyContactName: emergency_contact_name,
    //    EmergencyContactRelationship: emergency_contact_relationship,
    //    EmergencyContactNumber: emergency_contact_number,
    //    BloodGroup: blood_group,
    //    ChronicIllness: chronic_illness,
    //    HeartAilment: heart_ailment,
    //    AnyFaintingEpisodesinPast: fainting_episodes,
    //    AnyOtherAilment: other_ailment,
    //    AnyKnownAllergies: allergies,
    //    PayrollCompany: payroll_company,
    //    EmployeeID: employee_id,
    //    UnitStation: unit_station,
    //    ShantigramIdProof: shantigram_id_proof,
    //    RaceDist: race_dist,
    //    RaceAmount: race_amount,
    //    DiscountRate: discount_rate,
    //    RemainingAmountStatus: remaining_amount_status,
    //    FormSubmitOn: currentdate
    //};




    ////ajax calling to insert  custom data function
    //$.ajax({
    //    type: "POST",
    //    data: JSON.stringify(savecustomdata),
    //    url: "/api/Marathon/InsertRegistrationdetail",
    //    contentType: "application/json",
    //    success: function (data) {
    //        //////////////

    //        if (data.status === "1") {

    //            window.location.href = "/registration-thankyou?a=" + reference_code;
    //        }
    //        else {
    //            alert("Sorry Operation Failed!!! Please try again later");
    //            $('#btnRegistrationSubmit').removeAttr("disabled");
    //            return false;
    //        }
    //    }
    //});


    //return;



    //$.ajax({
    //    type: "POST",
    //    data: JSON.stringify(model),
    //    url: "/api/Marathon/Registration",
    //    contentType: "application/json",
    //    success: function (data) {
    //        if (data.status === "1") {
    //            var otp = prompt("Please enter OTP received on your mobile", "");

    //            if (otp !== null) {

    //                var generatedOtp = {
    //                    mobile: mobile,
    //                    OTP: otp,
    //                }
    //                $.ajax({
    //                    type: "POST",
    //                    data: JSON.stringify(generatedOtp),
    //                    url: "/api/Marathon/VerifyOTP",
    //                    contentType: "application/json",
    //                    success: function (data) {
    //                        if (data.status === "1") {





    //                        }

    //                        else {
    //                            alert("Invalid OTP");
    //                            $('#btnRegistrationSubmit').removeAttr("disabled");
    //                            return false;
    //                        }
    //                    }
    //                });

    //            }
    //        }
    //        else if (data === "-1") {
    //            alert("Invalid Mobile Number");
    //            $('#btnRegistrationSubmit').removeAttr("disabled");
    //        }
    //    }
    //});

    //return false;

});

function rundetailscheck() {
    $('#registration_form .errormsg').remove();
    var flag = 0;
	var PANNumber = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/g;
    
    if (!$("input[name='RunMode']:checked").val()) {
        $("#registration_form #runmode_radio").after("<span class='errormsg'>Select any run mode</span>");
        flag++;
    }
	if((!$('#select_distance').hasClass('d-none')&& $('#select_distance').val()=='')||(!$('#select_distance_charity').hasClass('d-none')&& $('#select_distance_charity').val()==''))
	{
		$('#select_distance_charity').after("<span class='errormsg'>Please select Distance.</span>");
		flag++;
	}
	if(!$('.donationAmount').hasClass('d-none')&& $('#DonationAmount').val()=='0.00')
	{
		$('#DonationAmount').after("<span class='errormsg'>Please enter Amount.</span>");
		flag++;
	}
	if(!$('.PAN').hasClass('d-none')&& $('#TaxExemptionCause').val()=='')
	{
		$('#TaxExemptionCause').after("<span class='errormsg'>Please Select Tax Exemption Cause.</span>");
		flag++;
	}
	if ((!$('.PAN').hasClass('d-none')&& $('#PANNumber').val()=='') ||(!$('.PAN').hasClass('d-none') && !PANNumber.test($("#PANNumber").val()))) {
        $('#PANNumber').after("<span class='errormsg'>Please enter a valid PAN number.</span>");
		flag++;
    }
	if(!$('.EmployeeID').hasClass('d-none')&& $('#EmployeeID').val()=='')
	{
		$('#EmployeeID').after("<span class='errormsg'>Please enter employee ID.</span>");
		flag++;
	}	
    var EmployeeEmailId = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,17}$/g;
    if (!$('.EmployeeEmailId').hasClass('d-none') && !EmployeeEmailId.test($('#EmployeeEmailId').val())) {
        $('#EmployeeEmailId').after("<span class='errormsg'>Please enter a valid employee email.</span>");
        flag++;
    }
    if ($('#RunType2').val() == 'Charity') {
        if ($('#DonationAmount').val() < parseFloat($('.registrationValue').text()) + parseFloat($('.DonationValue').text())) {
            flag++;
        }
    }    
    // if ($("#registration_form #select_distance").val() !== "") {
        // var dob1 = new Date(dob);

        // var distance = $("#registration_form #select_distance").val();
        // if ($("input[name='RunMode']:checked").val() === "Remote") {
            // if (distance === "5KM") {
                // if (new Date("2011-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    // $("#registration_form #dob").after("<span class='errormsg'>For 5 KM race, Age must be 10 years old.</span>");
                    // flag++;
                // }
            // }
            // else if (distance === "10KM") {
                // if (new Date("2009-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    // $("#registration_form #dob").after("<span class='errormsg'>For 10 KM race, Age must be 12 years old.</span>");
                    // flag++;

                // }
            // }
            // else if (distance === "21.097KM") {
                // if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    // $("#registration_form #dob").after("<span class='errormsg'>For 21 KM race, Age must be 18 years old.</span>");
                    // flag++;

                // }
            // }
            // else if (distance === "42.195KM") {
                // if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    // $("#registration_form #dob").after("<span class='errormsg'>For 42 KM race, Age must be 18 years old.</span>");
                    // flag++;

                // }
            // }
        // }
        // else {
            // if (new Date("2011-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                // $("#registration_form #dob").after("<span class='errormsg'>Age must be 18 years old.</span>");
                // flag++;

            // }
        // }
    // }
    return flag;
}

function personalcheck() {
    var flag = 0;
    var first_name = $("#registration_form #first_name").val();

    if (first_name === "") {
		$("#registration_form #first_name").next('span').remove();
        $("#registration_form #first_name").after("<span class='errormsg'>Please enter your Name</span>");
        flag++;
    }
    if (first_name !== "") {
        if (/[^a-zA-Z ]/.test(first_name)) {
			$("#registration_form #first_name").next('span').remove();
            $("#registration_form #first_name").after("<span class='errormsg'>Only alphabets are allowed.</span>");
            flag++;
        }
    }
    var last_name = $("#registration_form #last_name").val();
    if (last_name === "") {
		$("#registration_form #last_name").next('span').remove();
        $("#registration_form #last_name").after("<span class='errormsg'>Please enter your Last Name</span>");
        flag++;
    }
    if (last_name !== "") {
        if (/[^a-zA-Z ]/.test(last_name)) {
			$("#registration_form #last_name").next('span').remove();
            $("#registration_form #last_name").after("<span class='errormsg'>Only alphabets are allowed.</span>");
            flag++;
        }
    }
    var dob = $("#registration_form #dob").val();
    if (dob === "") {
		$("#registration_form #dob").next('span').remove();
        $("#registration_form #dob").after("<span class='errormsg'>Please enter your Date of Birth</span>");
        flag++;
    }
  

    // if ($("#registration_form #select_distance").val().indexOf("42") !== -1) {
        // var RaceCertificate = $("#registration_form #RaceCertificate").val();

        // if (RaceCertificate === "") {
            // if ($("#registration_form #uploadedCerti").val() === 'undefined' || $("#registration_form #uploadedCerti").val() === undefined) {
                // $("#registration_form #RaceCertificate").after("<span class='errormsg'>Please upload Race Certificate.</span>");
                // flag++;
            // }
        // }      
    // }

    var email_id = $("#registration_form #email_id").val();
    if (email_id === "") {
		$("#registration_form #email_id").next('span').remove();
        $("#registration_form #email_id").after("<span class='errormsg'>Please enter your Email Id</span>");
        flag++;
    }
    if (!validateEmail(email_id) && email_id !== "") {
		$("#registration_form #email_id").next('span').remove();
        $("#registration_form #email_id").after("<span class='errormsg'>Please enter valid email address</span>");
        flag++;
    }
    var contactno = $("#registration_form #contactno").val();
    if (contactno === "") {
		$("#registration_form #contactno").next('span').remove();
        $("#registration_form #contactno").after("<span class='errormsg'>Please enter your Contact Number</span>");
        flag++;
    }
    
    if (contactno.length !== 10 && contactno !== "") {
		$("#registration_form #contactno").next('span').remove();
        $("#registration_form #contactno").after("<span class='errormsg'>Contact Number should be of 10 digit</span>");
        flag++;
    }
   
    var tshirt_sizeMale = $("#registration_form #tshirt_sizeMale").val();
    if (tshirt_sizeMale === "") {
		$("#registration_form #tshirt_sizeMale").next('span').remove();
        $("#registration_form #tshirt_sizeMale").after("<span class='errormsg'>Please select your T-shirt Size</span>");
        flag++;
    }
    var idproof = $("#registration_form #idproof").val();
    if (idproof === "") {
		$("#registration_form #idproof").next('span').remove();
        $("#registration_form #idproof").after("<span class='errormsg'>Please select your valid ID Proof </span>");
        flag++;
    }
    

    var file1 = $("#registration_form #file1").val();
    if ($("#registration_form #uploadedId").val() === 'undefined' || $("#registration_form #uploadedId").val() === undefined)
        if (file1 === "") {
			$("#registration_form #file1").next('span').remove();
            $("#registration_form #file1").after("<span class='errormsg'>Please upload scanned copy for Identity Proof </span>");
            flag++;
        }


    return flag;
}

function famaddresscheck() {
    $('#famregistration_form .errormsg').remove();
    var flag = 0;
    var country_info = $("#famregistration_form #famcountry_info").val();

    if (country_info === "") {
        $("#famregistration_form #famcountry_info").after("<span class='errormsg'>Please select your Country</span>");
        flag++;
    }
    var state_info = $("#famregistration_form #famstate_info").val();
    if (state_info === "") {
        $("#famregistration_form #famstate_info").after("<span class='errormsg'>Please enter your State </span>");
        flag++;
    }
    var city_info = $("#famregistration_form #famcity_info").val();
    if (city_info === "") {
        $("#famregistration_form #famcity_info").after("<span class='errormsg'>Please enter your City </span>");
        flag++;
    }
    var address = $("#famregistration_form #famaddress").val();
    if (address === "") {
        $("#famregistration_form #famaddress").after("<span class='errormsg'>Please enter your Address </span>");
        flag++;
    }
    var pincode = $("#famregistration_form #fampincode").val();
    if (pincode === "") {
        $("#famregistration_form #fampincode").after("<span class='errormsg'>Please enter your Pin Code Number</span>");
        flag++;
    }
    var emergency_contact_name = $("#famregistration_form #famemergency_contact_name").val();

    if (emergency_contact_name === "") {
        $("#famregistration_form #famemergency_contact_name").after("<span class='errormsg'>Please enter Emergency Contact Name</span>");
        flag++;
    }
    var emergency_contact_relationship = $("#famregistration_form #famemergency_contact_relationship").val();
    if (emergency_contact_relationship === "") {
        $("#famregistration_form #famemergency_contact_relationship").after("<span class='errormsg'>Please enter Emergency Contact Relationship</span>");
        flag++;
    }
    var emergency_contact_number = $("#famregistration_form #famemergency_contact_number").val();
    if (emergency_contact_number === "") {
        $("#famregistration_form #famemergency_contact_number").after("<span class='errormsg'>Please enter Emergency Contact Number</span>");
        flag++;
    }
    if (document.famregistration_form.famemergency_contact_number.value.length !== 10 && emergency_contact_number !== "") {
        $("#famregistration_form #famemergency_contact_number").after("<span class='errormsg'>Emergency Contact Number should be of 10 digit</span>");
        flag++;
    }
    return flag;
}

function addresscheck() {
    $('#registration_form .errormsg').remove();
    var flag = 0;
    var country_info = $("#registration_form #country_info").val();

    if (country_info === "") {
        $("#registration_form #country_info").after("<span class='errormsg'>Please select your Country</span>");
        flag++;
    }
    var state_info = $("#registration_form #state_info").val();
    if (state_info === "") {
        $("#registration_form #state_info").after("<span class='errormsg'>Please enter your State </span>");
        flag++;
    }
    //if (state_info !== "") {
    //    if (/[^a-zA-Z ]/.test(state_info)) {
    //        $("#registration_form #state_info").after("<span class='errormsg'>Only alphabets are allowed.</span>");
    //        flag++;
    //    }
    //}
    var city_info = $("#registration_form #city_info").val();
    if (city_info === "") {
        $("#registration_form #city_info").after("<span class='errormsg'>Please enter your City </span>");
        flag++;
    }
    //if (city_info !== "") {
    //    if (/[^a-zA-Z ]/.test(city_info)) {
    //        $("#registration_form #city_info").after("<span class='errormsg'>Only alphabets are allowed.</span>");
    //        flag++;
    //    }
    //}
    var address = $("#registration_form #address").val();
    if (address === "") {
        $("#registration_form #address").after("<span class='errormsg'>Please enter your Address </span>");
        flag++;
    }
    var pincode = $("#registration_form #pincode").val();
    if (pincode === "") {
        $("#registration_form #pincode").after("<span class='errormsg'>Please enter your Pin Code Number</span>");
        flag++;
    }
    if (pincode.length !== 6 && pincode !== "") {
        $("#registration_form #pincode").after("<span class='errormsg'>Please enter valid pincode</span>");
        flag++;
    }
    return flag;
}

function MedicalInformation() {
    $('#registration_form .errormsg').remove();
    var flag = 0;
    var emergency_contact_name = $("#registration_form #emergency_contact_name").val();

    if (emergency_contact_name === "") {
        $("#registration_form #emergency_contact_name").after("<span class='errormsg'>Please enter Emergency Contact Name</span>");
        flag++;
    }
    if (emergency_contact_name !== "") {
        if (/[^a-zA-Z ]/.test(emergency_contact_name)) {
            $("#registration_form #emergency_contact_name").after("<span class='errormsg'>Only alphabets are allowed.</span>");
            flag++;
        }
    }

    var emergency_contact_number = $("#registration_form #emergency_contact_number").val();
    if (emergency_contact_number === "") {
        $("#registration_form #emergency_contact_number").after("<span class='errormsg'>Please enter Emergency Contact Number</span>");
        flag++;
    }
    if (emergency_contact_number.length !== 10 && emergency_contact_number !== "") {
        $("#registration_form #emergency_contact_number").after("<span class='errormsg'>Emergency Contact Number should be of 10 digit</span>");
        flag++;
    }
    
    var blood_group = $("#registration_form #blood_group").val();
    if (blood_group === "") {
        $("#registration_form #blood_group").after("<span class='errormsg'>Please select your Blood Group</span>");
        flag++;
    }


    return flag;
}

function FamValidate() {


    var category = $("#new_participant #select_distance").val();
    if (category === "") {
        alert("Please select category"); $("#new_participant #select_distance").focus();
        $('#fambtnRegistrationSubmit').removeAttr("disabled"); return false;
    }
    else {
        var arr = category.split("#");
        document.getElementById("famraceDistance").value = arr[0];
        document.getElementById("famraceAmount").value = arr[1];
        document.getElementById("famrstat").value = "no";
    }

    var ccode = $("#famregistration_form #famreference_code").val();

    // var ccode = document.getElementById("CouponCode").value;

    //create json object



    if (ccode !== "") {
        var savecustomdata = {

            ReferenceCode: ccode

        };
        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Marathon/CheckCouponCode",
            contentType: "application/json",
            success: function (data) {
                //////////////
                $(".famemployee_id_row").hide();
                $(".fampayroll_company_row").hide();
                $(".famunit_station_row").hide();
                $(".famid_proof_row ").hide();
                if (data.status === "1") {


                    $('.famreference_code_success').text('Coupon Code Applied!');
                    document.getElementById("famrstat").value = "yes";
                    $('.famreference_code_error').text("");
                    var extrafield = data.extra;
                    var extralist = extrafield.split("$");

                    document.getElementById("famEmailDomain").value = data.EmailDomain;
                    document.getElementById("famEmployeeIDSource").value = data.EmployeeIDSource;



                    for (var i = 0; i < extralist.length; i++) {

                        if (extralist[i] === "Employee ID") {
                            $(".famemployee_id_row").show();
                            $(".famemployee_id_row").addClass("requiredFld");
                        }
                        else if (extralist[i] === "Payroll Company") {
                            $(".fampayroll_company_row").hide();

                        }
                        else if (extralist[i] === "Unit or Station") {
                            $(".famunit_station_row").show();
                            $(".famunit_station_row").addClass("requiredFld");
                        }
                        else if (extralist[i] === "Shantigram ID") {
                            $(".famid_proof_row ").show();
                            $(".famid_proof_row").addClass("requiredFld");
                        }

                    }
                }
                else {
                    $('.famreference_code_error').text('Invalid Coupon Code!');
                    $('.famreference_code_success').text("");
                    return false;
                }
            }
        });
    }
    return 0;


}

function validate() {
    var category = $("#registration_form .select_distance").val();
    if (category === "") {
        alert("Please select category"); $("#registration_form #select_distance").focus();
        $('#btnRegistrationSubmit').removeAttr("disabled"); return false;
    }
    else {
        //var arr = category.split("#");
        //document.getElementById("raceDistance").value = arr[0];
        //document.getElementById("raceAmount").value = arr[1];
        document.getElementById("rstat").value = "no";
    }

    var ccode = $("#registration_form #reference_code").val();

    // var ccode = document.getElementById("CouponCode").value;

    //create json object



    if (ccode !== "") {
        var savecustomdata = {

            ReferenceCode: ccode

        };
        //ajax calling to insert  custom data function
        $.ajax({
            type: "POST",
            data: JSON.stringify(savecustomdata),
            url: "/api/Marathon/CheckCouponCode",
            contentType: "application/json",
            success: function (data) {
                //////////////
                $(".employee_id_row").hide();
                $(".payroll_company_row").hide();
                $(".unit_station_row").hide();
                $(".id_proof_row ").hide();
                if (data.status === "1") {


                    $('.reference_code_success').text('Coupon Code Applied!');
                    document.getElementById("rstat").value = "yes";
                    $('.reference_code_error').text("");
                    var extrafield = data.extra;
                    var extralist = extrafield.split("$");
                    var RefernceCodes = $("#reference_code").val();
                    document.getElementById("EmailDomain").value = data.EmailDomain;
                    document.getElementById("EmployeeIDSource").value = data.EmployeeIDSource;



                    for (var i = 0; i < extralist.length; i++) {

                        if (extralist[i] === "Employee ID") {
                            $(".employee_id_row").show();
                            $(".employee_id_row").addClass("requiredFld");
                        }
                        else if (extralist[i] === "Payroll Company") {
                            $(".payroll_company_row").show();
                            if (RefernceCodes === "Adv") {
                                $('.payroll_company_row').find('option').remove().end();
                                $('#payroll_company').append('<option value="Adani Mining And ICM">Adani Mining And ICM</option>');
                            }
                            $(".payroll_company_row").addClass("requiredFld");
                        }
                        else if (extralist[i] === "Unit or Station") {
                            $(".unit_station_row").show();
                            $(".unit_station_row").addClass("requiredFld");
                        }
                        else if (extralist[i] === "Shantigram ID") {
                            $(".id_proof_row ").show();
                            $(".id_proof_row").addClass("requiredFld");
                        }

                    }
                }
                else {
                    $('.reference_code_error').text('Invalid Coupon Code!');
                    $('.reference_code_success').text("");
                    return false;
                }
            }
        });
    }
    return 0;


}

$(function () {
    $('#dob').datetimepicker({ format: 'DD/MM/YYYY' });
    $("#registration_form #dob").datetimepicker({ format: 'DD/MM/YYYY' });
    $("#famregistration_form #famdob").datetimepicker({ format: 'DD/MM/YYYY' });
    $("#famregistration_form #famdob").datetimepicker({ format: 'DD/MM/YYYY' });
    $("#famregistration_form #fam2dob").datetimepicker({ format: 'DD/MM/YYYY' });
    $("#famregistration_form #fam3dob").datetimepicker({ format: 'DD/MM/YYYY' });
});

$(function () {
    $('#login_dob').datetimepicker({ format: 'DD/MM/YYYY' });
    $("#login_form #login_dob").datetimepicker({ format: 'DD/MM/YYYY' });
});

function validateEmployeeID() {


    var employee_id = $("#employee_id").val();
    var ReferenceCode = $("#reference_code").val();

    if (employee_id === "")
        return;

    var savecustomdata = {

        EmployeeID: employee_id,
        ReferenceCode: ReferenceCode

    };

    //ajax calling to insert  custom data function
    $.ajax({
        type: "POST",
        data: JSON.stringify(savecustomdata),
        url: "/api/Marathon/ValidateEmployeeID",
        contentType: "application/json",
        success: function (data) {
            //////////////

            if (data.status === "0") {
                $('.employee_id_error').text('');
                return true;
            }
            else {
                $('.employee_id_error').text('This Employee ID is already in Use.');
                return false;
            }
        }
    });
    return 0;


}

$('#renewal').on('click', function () {
    $('#new_participant .errormsg').remove();
    var email_id = $("#new_participant #checkEmail").val();
    var email_otp = $("#new_participant #emailotp").val();
    var distance_select = $("#select_distance option:selected").val();
    if (email_id === "") {
        $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter your Email Id</span>");
        $("#new_participant #checkEmail").focus();
        return false;
    }
    else if (!validateEmail(email_id) && email_id !== "") {
        $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter valid email address</span>");
        $("#new_participant #checkEmail").focus();
        return false;
    }
    $.ajax({
        url: "/api/Marathon/GetUserInfo",
        contentType: "application/json",
        type: 'POST',
        data: JSON.stringify({ 'Email': email_id, 'OTP': email_otp, 'distance': distance_select }),
        dataType: 'json',
        success: function (data) {
            if (data.result === 'Redirect') {
                window.location.href = "/MarathonRegisteredInfo";
            }
        },
        error: function (request, error) {
            alert("Request: " + JSON.stringify(request));
        }
    });
});

$('#freshReg').on('click', function () {
    $('.row.has-seprator').hide();
    $("#confirmation").modal('toggle');
    $('#registration_form').fadeIn(250);
    var distance_select = $("#select_distance option:selected").val();
    $('#race_dist').val(distance_select);
    $(".DetailsOfFullHalfMarathon_row ").hide();
    $(".RaceCertificate_row ").hide();

    if ($("#select_distance option:selected").val().indexOf("42") !== -1) {
        $(".DetailsOfFullHalfMarathon_row ").show();
        $(".RaceCertificate_row ").show();
    }

    return false;
});
//$('#register_btn').on('click', function () {

//    var e = document.getElementById("select_distance");
//    var distance_val = e.options[e.selectedIndex].value;
//    if (distance_val === "") {
//        $(e).next('label').addClass('error').text('Please select your marathon distance');
//    }
//    else {
//        $('#new_participant .errormsg').remove();
//        var email_id = $("#new_participant #checkEmail").val();
//        var email_otp = $("#new_participant #emailotp").val();
//        if (email_id === "") {
//            $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter your Email Id</span>");
//            $("#new_participant #checkEmail").focus();
//            return false;
//        }
//        else if (!validateEmail(email_id) && email_id !== "") {
//            $("#new_participant #checkEmail").after("<span class='errormsg'>Please enter valid email address</span>");
//            $("#new_participant #checkEmail").focus();
//            return false;
//        }
//        else if (email_otp === "") {
//            $("#new_participant #emailotp").after("<span class='errormsg'>Please enter 6 digit OTP sent to your Email id.</span>");
//            $("#new_participant #emailotp").focus();
//            return false;
//        }
//        else {
//            $.ajax({
//                url: "/api/Marathon/VerifyEmailOTP",
//                contentType: "application/json",
//                type: 'POST',
//                data: JSON.stringify({ 'Email': email_id, 'OTP': email_otp }),
//                dataType: 'json',
//                success: function (data) {
//                    if (data.status === "1") {
//                        alert(email_id + "validated successfully.");
//                        $('.row.has-seprator').hide();
//                        $('#registration_form').fadeIn(250);
//                        var distance_select = $("#select_distance option:selected").val();
//                        $('#race_dist').val(distance_select);
//                        $("#registration_form #email_id").val(email_id);
//                    }
//                    else if (data.status === "2") {
//                        $("#confirmation").modal('toggle');
//                        return false;
//                    }
//                    else if (data.status === "3") {
//                        alert("Email already registerd, Please try with other email id.");
//                        return false;
//                    }
//                    else if (data.status === "4") {
//                        $("#new_participant #emailotp").after("<span class='errormsg'>Invalid OTP.</span>");
//                        $("#new_participant #emailotp").focus();
//                        return false;
//                    }
//                    else {
//                        alert("An error occured. Please try again later");
//                        return false;
//                    }
//                },
//                error: function (request, error) {
//                    alert("Request: " + JSON.stringify(request));
//                }
//            });
//        }


//            //$('.row.has-seprator').hide();
//            //if (distance_val.toLowerCase().indexOf("family") !== -1) {
//            //    $('#registration_formFamily').fadeIn(250);
//            //    $('#famregistration_form').show();

//            //}
//            //else
//            //    $('#registration_form').fadeIn(250);

//            //var distance_select = $("#select_distance option:selected").val();
//            //$('#race_dist').val(distance_select);



//    }

//    $(".DetailsOfFullHalfMarathon_row ").hide();
//    $(".RaceCertificate_row ").hide();

//    if ($("#select_distance option:selected").val().indexOf("42") !== -1) {
//        $(".DetailsOfFullHalfMarathon_row ").show();
//        $(".RaceCertificate_row ").show();
//    }

//    return false;
//});



$('.optional').on("change focus", function () {
    if ($(this).val() !== "") {
        $(this).removeClass('skip');
        $('#email_id').addClass('skip').removeClass("error");
        $('#contact').addClass('skip').removeClass("error");
        $('#dob').addClass('skip').removeClass("error").val(null);
    } else {
        $(this).addClass('skip').removeClass("error");
        $('#email_id').removeClass('skip');
        $('#contact').removeClass('skip');
        $('#dob').removeClass('skip');
    }
    validator.resetForm();
});
// End Login

$('#registration_form').on('keyup keypress', function (e) {
    var keyCode = e.keyCode || e.which;
    if (keyCode === 13) {
        e.preventDefault();
        return false;
    }
});
var reffer_field = $('.reffer-inputs').detach();

$(document).on('click', '.next_btn', function () {
    var form = $("#registration_form");
    form.validate({


        errorElement: 'label',
        errorClass: 'error',
        highlight: function (element, errorClass, validClass) {
            $(element).addClass("error").closest('.form-group').addClass("has-error");
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass("error").closest('.form-group').removeClass("has-error");
        },
        invalidHandler: function (form, validator) {
            var errors = validator.numberOfInvalids();
            if (errors) {
                validator.errorList[0].element.focus(); //Set Focus
            }
        },
        rules: {
            payroll_company: {
                required: true,
            },
            unit_station: {
                required: true,
            },
            employee_id: {
                required: true,
                digits: true,
                minlength: 8,
                //maxlength: 8,
            },
            first_name: {
                required: true,
            },
            last_name: {
                required: true,
            },
            dobnew: {
                required: true,
            },
            email_id: {
                required: true,
                myEmailformat: true,
                validateEmailCompany: true,
            },
            contact: {
                required: true,
                Phoneformat: true,
                digits: true,
                minlength: 10,
                maxlength: 11
            },
            nationality: {
                required: true,
            },
            tshirt_size: {
                required: true,
            },
            bibname: {
                required: true,
            },
            idproof: {
                required: function function_name(argument) {

                    if (coupon_data && coupon_data.setting.id_required_or_not === 'yes') {
                        return false;
                    }
                    else {
                        return true;
                    }


                },
            },
            idnumber: {
                required: function function_name(argument) {

                    if (coupon_data && coupon_data.setting.id_required_or_not === 'yes') {
                        return false;
                    }
                    else {
                        return true;
                    }


                },
            },
            file1: {
                required: function function_name(argument) {

                    if (coupon_data && coupon_data.setting.id_required_or_not === 'yes') {
                        return false;
                    }
                    else {

                        if ($('#reg_id').val() !== '') {
                            return false;
                        }
                        else {
                            return true;
                        }
                    }


                },
                extension: "pdf|jpg|png|jpeg"
            },
            last_marathon_details: {
                required: true,
            },
            last_marathon_race_date: {
                required: true,
            },
            last_marathon_race_name: {
                required: true,
            },
            file2: {
                required: function function_name(argument) {

                    if ($('#reg_id').val() !== '') {
                        return false;
                    }
                    else {
                        return true;
                    }


                },
                extension: "pdf|jpg|png|jpeg",
            },
            address: {
                required: true,
            },
            pincode: {
                required: true,
                digits: true,
                minlength: 6,
                maxlength: 6
            },
            emergency_contact_name: {
                required: true,
            },
            emergency_contact_relationship: {
                required: true,
            },
            emergency_contact_number: {
                required: true,
                Phoneformat: true,
                digits: true,
                minlength: 10,
                maxlength: 11
            },
            blood_group: {
                required: true,
            },
            chronic_illness: {
                required: true,
            },
            agree: {
                required: true,
            },
            id_proof_unit: {
                extension: "pdf|jpg|png|jpeg",
            },
        },
        messages: {
            file1: {
                filesize: "Maximum file size limit is 2 MB.",
                extension: "Only pdf, jpg and png file formats are allowed.",
            },
            file2: {
                filesize: "Maximum file size limit is 2 MB.",
                extension: "Only pdf, jpg and png file formats are allowed.",
            },
            id_proof_unit: {
                filesize: "Maximum file size limit is 2 MB.",
                extension: "Only pdf, jpg and png file formats are allowed.",
            },
        },

    });
    if (form.valid() === true) {
        if ($('#first').is(":visible")) {
            current_fs = $('#first');
            next_fs = $('#second');
            $('.form_step').find('.active_fieldset').removeClass('active_fieldset').next().addClass('active_fieldset');
        } else if ($('#second').is(":visible")) {
            current_fs = $('#second');
            next_fs = $('#third');
            $('.form_step').find('.active_fieldset').removeClass('active_fieldset').next().addClass('active_fieldset');
        }

        next_fs.show();
        current_fs.hide();

        setTimeout(function () {
            var fieldsetScroll = $("body").find('.multi-step-form').offset().top - 10;
            $('body,html').animate({ scrollTop: fieldsetScroll }, 200);
        }, 100);
    }
});




$('.prev_btn').click(function () {
    if ($('#second').is(":visible")) {
        current_fs = $('#second');
        next_fs = $('#first');
    } else if ($('#third').is(":visible")) {
        current_fs = $('#third');
        next_fs = $('#second');
    }
    next_fs.show();
    current_fs.hide();
    $('.form_step').find('.active_fieldset').removeClass('active_fieldset').prev().addClass('active_fieldset');
    setTimeout(function () {
        var fieldsetScroll = $("body").find('.multi-step-form').offset().top - 10;
        $('body,html').animate({ scrollTop: fieldsetScroll }, 200);
    }, 100);
});



//$("#promorunregister_form").validate({
//    rules: {
//        raceCategory: {
//            required: true,
//        },
//        promo_user_name: {
//            required: true,
//        },
//        empid: {
//            required: true,
//        },
//        dob1: {
//            required: true,
//        },
//        'venue[]': {
//            required: true,
//            minlength: 1
//        },
//        promo_user_email: {
//            required: true,
//            myEmailformat: true,
//        },
//        promo_user_contact: {
//            required: true,
//            Phoneformat: true,
//            digits: true,
//            minlength: 10,
//            maxlength: 11
//        },
//    },
//    messages: {
//        'venue[]': {
//            required: "Please Select Venue.",
//        }
//    },
//});

$(document).on("focusin", "#dobr , #mDate , #dob1", function () {
    $(this).prop('readonly', true);
});

$(document).on("focusout", "#dobr , #mDate , #dob1", function () {
    $(this).prop('readonly', false);
});
// Custom Method
//// Custom method to validate Email 
//jQuery.validator.addMethod("myEmailformat", function (value, element) {
//    return this.optional(element) || (/^[a-z0-9]+([-._][a-z0-9]+)*@([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,4}$/.test(value) && /^(?=.{1,64}@.{4,64}$)(?=.{6,100}$).*/.test(value));
//}, 'Please enter a valid email address.');


//// Custom method to validate Phone
//jQuery.validator.addMethod("Phoneformat", function (value, element) {
//    return this.optional(element) || (/^[6-9][0-9]{9}$/.test(value));
//}, 'Please enter a valid Phone Number.');

// INDUS10
//$.validator.addMethod('validateEmailCompany', function (value) {

//    if (coupon_data && coupon_data.code === 'indus10') {
//        return /^([A-Za-z0-9_\-\.])+\@(indusind)+\.(com)$/.test(value);
//    }
//    else {
//        return true;
//    }

//}, 'Use company email for the registration.');
// End Validator======================================

var ymale = $('#male').detach();
var yfemale = $('#female').detach();
var ykids = $('#transgender').detach();
$('.t-shirt-size').prepend(ymale);
$('input:radio[name="Gender"]').change(function () {
    if ($(this).val() === 'female') {
        $('#male').remove();
        $('#transgender').remove();
        $('.t-shirt-size').prepend(yfemale);
    }
    else if ($(this).val() === 'transgender') {
        $('#male').remove();
        $('#female').remove();
        $('.t-shirt-size').prepend(ykids);
    } else {
        $('#female').remove();
        $('#transgender').remove();
        $('.t-shirt-size').prepend(ymale);
    }
});

$('#country_info').on('change blur', function () {
    $('.addinotal_field').css('margin-top', '').attr('value', '');
    if ($(this).val() !== "India") {
        $('.addinotal_field').show().prev('select').hide();
        $("#state_info").prop("disabled", true);
        $("#city_info").prop("disabled", true);
    }
    else {
        $("#state_info").prop("disabled", false);
        $("#city_info").prop("disabled", false);
        $(this).closest('.form-group').next().find('.addinotal_field').hide().prev('select').show();
        if ($('#city_info').val() !== "others") {
            $('#city_info').show().next('.addinotal_field').hide();
        }
        else {
            $('#city_info').show().next('.addinotal_field').show().css('margin-top', '25px');
        }
    }
});
$('#state_info').on('change blur', function () {
    $('.addinotal_field').css('margin-top', '').attr('value', '');
    if ($(this).val() !== "Gujarat") {
        $(this).closest('.form-group').next().find('.addinotal_field').show().prev('select').hide();
    }
    else {
        if ($('#city_info').val() !== "others") {
            $('#city_info').show().next('.addinotal_field').hide();
        }
        else {
            $('#city_info').show().next('.addinotal_field').show().css('margin-top', '25px');
        }
    }
});
$('#city_info').on('change blur', function () {
    if ($(this).val() !== "others") {
        $(this).next('.addinotal_field').hide();
    }
    else {
        $(this).next('.addinotal_field').show().css('margin-top', '25px');
    }
});




$(document).on('change', '#select_distance', function () {
    $("#registration_form .dob").remove();
    var dob = $("#registration_form #dob").val();
    var distance = $("#registration_form #select_distance").val();
    if (dob !== "") {
        if ($("input[name='RunMode']:checked").val() === "Remote") {
            if (distance === "5KM") {
                if (new Date("2011-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    $("#registration_form #dob").after("<span class='errormsg dob'>For 5 KM race, Age must be 10 years old.</span>");
                }
            }
            else if (distance === "10KM") {
                if (new Date("2009-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    $("#registration_form #dob").after("<span class='errormsg dob'>For 10 KM race, Age must be 12 years old.</span>");
                }
            }
            else if (distance === "21.097KM") {
                if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    $("#registration_form #dob").after("<span class='errormsg dob'>For 21 KM race, Age must be 18 years old.</span>");
                }
            }
            else if (distance === "42.195KM") {
                if (new Date("2003-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                    $("#registration_form #dob").after("<span class='errormsg dob'>For 42 KM race, Age must be 18 years old.</span>");
                }
            }
        }
        else {
            if (new Date("2011-11-20") < new Date($("#registration_form #dob").val().split("/").reverse().join("-"))) {
                $("#registration_form #dob").after("<span class='errormsg dob'>Age must be 18 years old.</span>");
            }
        }
    }

    showExtraFields();

});

function validateEmail(sEmail) {
    var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    if (filter.test(sEmail)) { return true; }
    else { return false; }
}

function showExtraFields() {

    if ($('#select_distance').val() === '42.195 Km') {
        $("#prevmdetails").show();

        $("#prevmdetails input").addClass('validated');
    } else {
        $("#prevmdetails input").removeClass('validated');
        $("#prevmdetails").hide();
    }

}

$("#registration_form").on('load', function () {
    populateSize();
});
function populateSize() {

    var Male = ["-- select one --", "X-Small (36 cm)", "Small (38 cm)", "Medium (40 cm)", "Large (42 cm)", "X-Large (44 cm)", "XX-Large (46 cm)"];
    var Female = ["-- select one --", "Small (34 cm)", "Medium (36 cm)", "Large (38 cm)", "X-Large (40 cm)"];
    var sizelist = "";
    var gen = $("input[name='Gender']:checked").val();
    if (gen === "Male") {
        for (size in Male) {
            sizelist += "<option>" + Male[size] + "</option>";
        }
    }
    else if (gen === "Female") {
        for (size in Female) {
            sizelist += "<option>" + Female[size] + "</option>";
        }
    }

    document.getElementById("tshirt_sizeMale").innerHTML = sizelist;

}

function check(a) {
    var c = "#" + a;
    var v = $(c).val();
    if (v.length > 10) {
        $(c).val(v.substring(0, 10));
    }
}
function myFunction(b) {
    var c = "#" + b;
    var v = $(c).val();
    $(c).val(v.substring(0, 10));

}
function diablekey() {
    document.getElementById('contactno').addEventListener('keydown', function (e) {
        if (e.which === 38 || e.which === 40) {
            e.preventDefault();
        }

        document.getElementById('emergency_contact_number').addEventListener('keydown', function (e) {
            if (e.which === 38 || e.which === 40) {
                e.preventDefault();
            }
        });
    });
}
$(document).ready(function () {
    showExtraFields();
    if ($("#select_distance").val() === "42.195KM") {
        $(".DetailsOfFullHalfMarathon_row ").show();
        $(".RaceCertificate_row ").show();
    }
    var idVal = $('input[type=radio]:checked').val();
    if (idVal === "Physical") {
        $("label[id='dob_txt']").text("");
        $("label[id='dob_txt']").text("Date of Birth");
        // var selcted_dist1 = $('#select_distance').val();
        // $('#select_distance').val(selcted_dist1);
        //$(".all-dist").css("display", "none");
        //$(".Physical").css("display", "block");
    }
    else if (idVal === "Remote") {
        $("label[id='dob_txt']").text("");
        $("label[id='dob_txt']").text("Date of Birth");
        var selcted_dist = $('#select_distance').val();
        $('#select_distance').text('');
        $('#select_distance').append('<option value="">Select your distance</option>');
        $('#select_distance').append('<option class="all-dist " value="42.195KM">42.195 Km Run (₹                                499.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="21.097KM">21.097 Km Run (₹                                499.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="10KM">10 Km Run (₹                              499.00)</option>');
        $('#select_distance').append('<option class="all-dist " value="5KM">5 Km Run (₹                               499.00)</option>');
        $('#select_distance').val(selcted_dist);
        //$(".all-dist").css("display", "none");
        //$(".Remote").css("display", "block");
    }

    if ($('input[name=Vaccinationted]:checked').length) {
        var Vaccinationted = $('input[name=Vaccinationted]:checked').val();
        if (Vaccinationted === "Yes") {
            $(".Covidfile").css("display", "block");
            $(".covidMsg").css("display", "none");
        }
        else if (Vaccinationted === "No") {
            $(".Covidfile").css("display", "none");
            $(".covidMsg").css("display", "block");
            $("#Covidfile").next(".errormsg").remove();
        }
    }
});


$(window).on('beforeunload', function () { $("input[name='submit_form']").prop("disabled", "disabled"); });

//Resend OTP timer
// Get refreence to span and button
var spn = document.getElementById("count");
var btn = document.getElementById("resendOTP");

var count = 30;     // Set count
var timer = null;  // For referencing the timer

//function countDown(){
//    // Display counter and start counting down
//    spn.textContent = count;

//    // Run the function again every second if the count is not zero
//    if (count !== 0) {
//        timer = setTimeout(countDown, 1000);
//        count--; // decrease the timer
//    }
//    else {
//        // Enable the button
//        document.getElementById('timer').style.display = 'none';
//        btn.style.display = 'block';
//    }
//};

// $(document).ready(function () {
function loadCity(cityName) {
    var state = $("#state_id").val();
    $("#city_id").empty();
    $.ajax({
        url: '/api/Marathon/getCity',
        contentType: "application/json",
        type: 'POST',
        //data: { id: '1' },
        dataType: 'json',
        data: JSON.stringify({ 'id': state }),
        success: function (city) {
            $.each(city, function (i, city) {
                $("#city_id").append('<option value="'
                    + city.Value + '">'
                    + city.Text + '</option>');
            });
            $("#city_id").val(cityName);
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
}
$("#state_info").change(function () {
    var state = $("#state_info").val();
	$('#State').val($('#state_info').val());
    $("#city_info").empty();
    $.ajax({
        url: '/api/Marathon/getCity',
        contentType: "application/json",
        type: 'POST',
        //data: { id: '1' },
        dataType: 'json',
        data: JSON.stringify({ 'id': state }),
        success: function (city) {
            $.each(city, function (i, city) {
                $("#city_info").append('<option value="'
                    + city.Value + '">'
                    + city.Text + '</option>');
            });
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
    return false;
});

$(".newRaceapply").click(function () {
    $("#applynew").show();
    $(".newRaceapply").hide();
    $(".newRaceapplyDiv").hide();
});

$("#ExReferenceCode").change(function () {
    var ExReferenceCode = $("#ExReferenceCode").val();
    if (ExReferenceCode != null) {
        var ccode = $("#donation_form #ExReferenceCode").val();

         //var ccode = document.getElementById("CouponCode").value;

        //create json object
        if (ccode !== "") {
            var savecustomdata = {

                ReferenceCode: ccode

            };
            //ajax calling to insert  custom data function
            $.ajax({
                type: "POST",
                data: JSON.stringify(savecustomdata),
                url: "/api/Marathon/CheckCouponCode",
                contentType: "application/json",
                success: function (data) {
                    //////////////
                    //$(".famemployee_id_row").hide();
                    //$(".fampayroll_company_row").hide();
                    //$(".famunit_station_row").hide();
                    //$(".famid_proof_row ").hide();
                    if (data.status === "1") {

                        $('.Exreference_code_error').text("");
                        $('.Exreference_code_success').text('Coupon Code Applied!');
                        //document.getElementById("famrstat").value = "yes";
                        $('.Exreference_code_error').text("");
                        var extrafield = data.extra;
                        var extralist = extrafield.split("$");

                        //document.getElementById("famEmailDomain").value = data.EmailDomain;
                        //document.getElementById("famEmployeeIDSource").value = data.EmployeeIDSource;



                        //for (var i = 0; i < extralist.length; i++) {

                        //    if (extralist[i] === "Employee ID") {
                        //        $(".famemployee_id_row").show();
                        //        $(".famemployee_id_row").addClass("requiredFld");
                        //    }
                        //    else if (extralist[i] === "Payroll Company") {
                        //        $(".fampayroll_company_row").hide();

                        //    }
                        //    else if (extralist[i] === "Unit or Station") {
                        //        $(".famunit_station_row").show();
                        //        $(".famunit_station_row").addClass("requiredFld");
                        //    }
                        //    else if (extralist[i] === "Shantigram ID") {
                        //        $(".famid_proof_row ").show();
                        //        $(".famid_proof_row").addClass("requiredFld");
                        //    }

                        //}
                    }
                    else {
                        $('.Exreference_code_error').text('Invalid Coupon Code!');
                        $('.Exreference_code_success').text("");
                        ExReferenceCode == "EmptyCouponCode";
                        return false;

                    }
                }
            });
        }

    }
});

$("#submitbutton").click(function (e) {
    //debugger;
    e.preventDefault();
    var model = {
        FirstName: $("#first_name").text().trim(),
        LastName: $("#last_name").text().trim(),
        DateofBirth: $("#date_birth").text().trim(),
        IdCardFilename: $("#uploadedId").text().trim(),
        Email: $("#email_id").text().trim(),
        ContactNumber: $("#contactno").text().trim(),
        TShirtSize: $("#tshirt_sizeMale").text().trim(),
        VaccinationCertificateName: $("#uploadedId").text().trim(),
        //RaceDistance: $("#RaceDistance").text(),
        //RunMode: $("#RunMode").text(),
        //TimeSlot: $("#TimeSlot").text(),
        //RunDate: $("#RunDate").text(),
        PaymentStatus: $("#PaymentStatus").text().trim(),
        RegistrationStatus: $("#RegistrationStatus").text().trim(),
        RunMode: $("#RunmodeListNew").val().trim(),
        RaceDistance: $("#select_distancenew").val().trim(),
        RunDate: $("#DateSlotListNew").val().trim(),
        TimeSlot: $("#TimeSlotListNew").val().trim(),
        ReferenceCode: $('#ExReferenceCode').val(),
        //RaceAmount: $('#RaceDistance').val()
    };
    $.ajax({
        url: '/api/Marathon/AddRunnerUserInfo',
        type: 'POST',
        data: model,
        success: function (response) {
            if (response.result == 'Redirect')
                window.location = response.url;
            else { 
                alert("else"); }
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
});


// $("#PayNowbutton").click(function () {
    // //debugger;
    // var model = {
        // FirstName: $("#first_name").text(),
        // LastName: $("#last_name").text(),
        // DateofBirth: $("#date_birth").text(),
        // IdCardFilename: $("#uploadedId").text(),
        // Email: $("#email_id").text(),
        // ContactNumber: $("#contactno").text(),
        // TShirtSize: $("#tshirt_sizeMale").text(),
        // VaccinationCertificateName: $("#uploadedId").text(),
        // //RaceDistance: $("#RaceDistance").text(),
        // //RunMode: $("#RunMode").text(),
        // //TimeSlot: $("#TimeSlot").text(),
        // //RunDate: $("#RunDate").text(),
        // PaymentStatus: $("#PaymentStatus").text(),
        // RegistrationStatus: $("#RegistrationStatus").text(),
        // RunMode: $("#RunmodeListNew").val(),
        // RaceDistance: $("#select_distancenew").val(),
        // RunDate: $("#DateSlotListNew").val(),
        // TimeSlot: $("#TimeSlotListNew").val()
    // };
    // $.ajax({
        // url: '/api/Marathon/PayNowUserInfo',
        // type: 'POST',
        // data: model,
        // success: function (response) {
            // if (response.result == 'Redirect')
                // window.location = response.url;
        // },
        // error: function (ex) {
            // alert('Failed.' + ex);
        // }
    // });
// });

//RemoveNow: Welcome Runner Page :
// $("#RemoveNowbutton").click(function () {
    // var wid = $(this).attr("data-id");
    // $.ajax({
        // url: '/api/Marathon/RemoveUserInfo',
        // type: 'POST',
        // data: { location: wid },
        // cache: false,
        // success: function (response) {
            // if (response.result == 'Redirect')
                // window.location = response.url;
        // },
        // error: function (ex) {
            // alert('Failed.' + ex);
        // }
    // });
// });
/* New Changes July 2023*/


$('#SaveGroup').click(function()
	{
	$.ajax({
        url: "/api/Marathon/SaveGroupRegistration",
		type: 'POST',
        success: function (response) {
            if (response.result == 'Redirect')
                window.location = response.url;
        },
        error: function (ex) {
            alert('Failed.' + ex);
        }
    });
});
	


$('.DeleteGroupParticipant').click(function()
{
		$.ajax({
			url: "/api/Marathon/DeleteGroupParticipant",
			type: 'GET',
			data: { id: $(this).attr("id") },
			success: function (response) {
				location.reload(true);
			},
			error: function (ex) {
				alert('Failed.' + ex);
			}
		});
});

$('#TaxExemptionCertificate').click(function(){
    if($('#TaxExemptionCertificate')[0].checked)
    {
		$('.PAN').removeClass('d-none');
    }
    else{
         {
		$('.PAN').addClass('d-none');
    }
    }
})
$('.runtype').change(function()
{
	if($('input[name="RunType"]:checked').val()=='Normal')
	{
		$('#first_btn').removeAttr('disabled');
		$('.NormalList').removeClass('d-none');
		$('.CharityList').addClass('d-none');
		$('.donationAmount').addClass('d-none');
		$('.PAN').addClass('d-none');
	}
	else{
		$("#registration_form #select_distance").val($("#registration_form #select_distance_charity").val())
		$('.NormalList').addClass('d-none');
		$('.CharityList').removeClass('d-none');
		$('.donationAmount').removeClass('d-none');
	}
});
$('.CharityList').change(function(){
    		$("#registration_form #select_distance").val($("#registration_form #select_distance_charity").val());
})
$('.panel').click(function(){
    $('.panel').removeClass('pannelActive');
    $(this).addClass('pannelActive');
})
 $( document ).ready(function() {
$("#panelDefault1").addClass('pannelActive');
});
 
/*Register CLick*/

$('#register_btn').click(function(e)
{

	if($('#receivedOTP').val()!="1")
	{
		if($('.ListData').children('.active').attr('id')=="IndividualId")
		{
			$('#Count').val('1');
			$('#FinalAmount').val($('.IndividualAmount').text());
			$('#FinalAmount').val($('.IndividualAmount').text());
			if($('input[name=BibsRadio]:checked').val()==undefined ||$('input[name=BibsRadio]:checked').val()=="")
			{
				$('.registrationerror').removeClass('d-none');
				e.preventDefault();
				return false;
			}
			$('#select_RunType').val($('input[name=BibsRadio]:checked').val());
			localStorage.setItem('singleRuntype', $('input[name=BibsRadio]:checked').val());

			$('#select_distance').val($('input[name=BibsRadio]:checked').parent().parent().parent().parent().siblings().children().children().text());
			localStorage.setItem('singleDistance', $('input[name=BibsRadio]:checked').parent().parent().parent().parent().siblings().parent().attr('id'));
			if($('input[name=BibsRadio]:checked').val()!="Normal")
			{
			$('#DonationAmount').val(parseInt($('.IndividualAmount').text()));
			}
		}
		else{
			for(var i=0;i<$('.ListData').children().length;i++)
			{
			   if($('.ListData').children()[i].classList.contains('active'))
			   {
				   localStorage.setItem('ActiveTab',$('.ListData').children()[i].getAttribute('id'));
			   }
			}
			$('#Count').val($('.groupCount').text());
		}
	
	}
	
})
/*Captcha Code*/



   function onSubmitRegister(token) {
	   if($('#otp').val()=="" || $('#otp').val()==undefined)
		{
			$('#validate').val("validate")	
		}
	   else{
			$('#register').val("register")	
		}
		 $("#reResponse").val(token);
		 document.getElementById("new_participant").submit();
   }
   function onSubmitLogin(token) {
     $("#reResponseLogin").val(token);
     document.getElementById("login_form").submit();
   }
   
function onSubmitDonationSubmit(token) {
     $("#reResponseLogin").val(token);
     document.getElementById("login_form").submit();
   }
$(".resendSMSOTP").click(function (e) {
    RegistrationResendOTP();
    e.preventDefault();
});
function RegistrationResendOTP(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lffy6MmAAAAAJWCrNIqaaSIz3mHsEy_xpKrmiaY', { action: 'RegistrationCheck' }).then(function (token) {
		 $("#reResponse").val(token);

			var ContactRegex = /^\d{10}$/g;
			var MobileNumber = $('#checkPhoneNumber').val();
			if (MobileNumber === "") {
				$("#checkPhoneNumber").after("<span class='errormsg'>Please enter your Email Id</span>");
				$("#checkPhoneNumber").focus();
			}
			else if (!ContactRegex.test(MobileNumber)) {
				$("#checkPhoneNumber").after("<span class='errormsg'>Please enter valid email address</span>");
				$("#checkPhoneNumber").focus();
			}
			else {
				$('#dloader').css('display', 'block');
				$.ajax({
					url: "/api/Marathon/ResendOTPSMS",
					contentType: "application/json",
					type: 'POST',
					data: JSON.stringify({ 'PhoneNumber': MobileNumber ,'formName':'new_participant','reResponse':$("#reResponse").val()}),
					dataType: 'json',
					success: function (data) {
						if (data === 1) {
							$('#dloader').css('display', 'none');
							alert("OTP has been sent successfully to your Mobile Number:" + PhoneNumber);
						}
						else if (data === 3) {
							$('#dloader').css('display', 'none');
							alert("Invalid Session!");
							location.reload();
						}
						else if (data === 4) {
							$('#dloader').css('display', 'none');
							alert("You have exceeded OTP attempts. Please try after 30 min");
							location.reload();
						}
						else {
							$('#dloader').css('display', 'none');
							alert("OTP has been sent successfully to your Mobile Number:" + PhoneNumber);
						}
					},
					error: function (request, error) {
						alert("Request: " + JSON.stringify(request));
					}
				});
			}

		});
    });
}

$(".LoginresendSMSOTP").click(function (e) {
    LoginResendOTP();
    e.preventDefault();
});
function LoginResendOTP(e) {
    grecaptcha.ready(function () {
        grecaptcha.execute('6Lffy6MmAAAAAJWCrNIqaaSIz3mHsEy_xpKrmiaY', { action: 'RegistrationCheck' }).then(function (token) {
		 $("#reResponseLogin").val(token);

			var ContactRegex = /^\d{10}$/g;
			var MobileNumber = $('#contact').val();
			if (MobileNumber === "") {
				$("#contact").after("<span class='errormsg'>Please enter your Email Id</span>");
				$("#contact").focus();
			}
			else if (!ContactRegex.test(MobileNumber)) {
				$("#contact").after("<span class='errormsg'>Please enter valid email address</span>");
				$("#contact").focus();
			}
			else {
				$('#dloader').css('display', 'block');
				$.ajax({
					url: "/api/Marathon/ResendOTPSMS",
					contentType: "application/json",
					type: 'POST',
					data: JSON.stringify({ 'PhoneNumber': MobileNumber ,'formName':'login_form','reResponse':$("#reResponseLogin").val()}),
					dataType: 'json',
					success: function (data) {
						if (data === 1) {
							$('#dloader').css('display', 'none');
							alert("OTP has been sent successfully to your Mobile Number:" + PhoneNumber);
						}
						else if (data === 3) {
							$('#dloader').css('display', 'none');
							alert("Invalid Session!");
							location.reload();
						}
						else if (data === 4) {
							$('#dloader').css('display', 'none');
							alert("You have exceeded OTP attempts. Please try after 30 min");
							location.reload();
						}
						else {
							$('#dloader').css('display', 'none');
							alert("OTP has been sent successfully to your Mobile Number:" + PhoneNumber);
						}
					},
					error: function (request, error) {
						alert("Request: " + JSON.stringify(request));
					}
				});
			}

		});
    });
}






/*Register Form UI Releated changes*/


$(document).ready(function () {
    $("#GroupCard").hide();
    $("#IndividualId").on("click", function () {
        $("#IndividualCard").show();
        $("#GroupCard").hide();
        
    });

    $("#GroupId").on("click", function () {
        $("#GroupCard").show();

        $("#IndividualCard").hide(); 
    });
});




$(document).ready(function () {
    $("#add-member1").click(function () {
        var currentValue = $("#member-count1").val();
        var givenValue = 1; // Replace with your desired value
        var newValue = parseInt(currentValue) + givenValue;
        $("#member-count1").val(newValue);
    });

    $("#subtract-member1").click(function () {
        var currentValue = $("#member-count1").val();
        var givenValue = 1; // Replace with your desired value
        var newValue = parseInt(currentValue) - givenValue;
        $("#member-count1").val(newValue);
    });
});
$(document).ready(function () {
    $(".collapsed").click(function () {
        $(this).find("span").toggleClass("glyphicon-chevron-up glyphicon-chevron-down ");
    });
});


const listItems = document.querySelectorAll('.InnerTiles');

  listItems.forEach((item) => {
    item.addEventListener('click', function () {
      // Remove "active" class from all list items
      listItems.forEach((item) => item.classList.remove('active'));
      // Add "active" class to the clicked list item
      this.classList.add('active');
    });
  });
  
  var groupCount=0;
  $(".add-member").click(function () {
        var currentValue = $(this).siblings('.member-count').val();
		var amount = parseInt($(this).parent().siblings(".BibsInfo").children('.bibs-inr').children('.runAmount').text());
        var givenValue = 1; 
        var newValue = parseInt(currentValue) + givenValue;
        $(this).siblings('.member-count').val(newValue);
		var currentAmount = parseInt($(this).parent().parent().siblings(".accordion-regular-right").children('p').children('.final').text())+amount;
		$(this).parent().parent().siblings(".accordion-regular-right").children('p').children('.final').text(currentAmount);
		$('#Count').val(parseInt($('#Count').val())+1);
		groupCount++;
		$('.groupCount').text(groupCount);
});
	
$(".subtract-member").click(function () {
    var currentValue = $(this).siblings('.member-count').val();
	if(parseInt(currentValue)>0)
	{
		 var amount = parseInt($(this).parent().siblings(".BibsInfo").children('.bibs-inr').children('.runAmount').text());
         var givenValue = 1; 
         var newValue = parseInt(currentValue) - givenValue;
         $(this).siblings('.member-count').val(newValue);
		 var currentAmount = parseInt($(this).parent().parent().siblings(".accordion-regular-right").children('p').children('.final').text())-amount;
		 $(this).parent().parent().siblings(".accordion-regular-right").children('p').children('.final').text(currentAmount);
		 $('#Count').val(parseInt($('#Count').val())-1);
		 groupCount--;
		$('.groupCount').text(groupCount);
	 }
 });

$('input[name=BibsRadio]').click(function(){
	if($('input[name=BibsRadio]:checked').parent().parent().attr('class').includes("bibs-charity"))
	{
		$('.IndividualAmount').text($('input[name=BibsRadio]:checked').parent().siblings().val());
	}
	else{
		$('.IndividualAmount').text($('input[name=BibsRadio]:checked').parent().siblings().children().text());
	}
})
$('.individualCharity').blur(function(){
	if(parseInt($(this).next().text())>$(this).val())
	{
		$(this).val(parseInt($(this).next().text()));
	}
	$('.IndividualAmount').text($(this).val());
})
/*DOB Validation*/
$('#dob').blur(function(){
	$.ajax({
            type: "POST",
			data: JSON.stringify({ 'DateofBirth': $('#dob').val() ,'RaceCategory':$('#select_distance').val()}),
            url: "/api/Marathon/AgeValidator",
            contentType: "application/json",
            success: function (data) {
                if (data.status != "") {
					$('#dob').next().remove();
					$('#second-next').attr('disabled','disabled');
					var tag ="<span class='errormsg'>"+data.status+".</span>";
					$('#dob').after(tag);
                }
                else {
					$('#second-next').removeAttr('disabled');
                    $('#dob').next('.errormsg').text('');
                }
            }
    });
})
/*Coupon code Validation*/
$('#reference_code').blur(function(){
	if($('#reference_code').val().length>=4)
	{
		$.ajax({
            type: "POST",
			data: JSON.stringify({ 'ReferenceCode': $('#reference_code').val()}),
            url: "/api/Marathon/CheckCouponCodeValidation",
            contentType: "application/json",
            success: function (data) {
				data = JSON.parse(data);
				var selectCOunt=0;
                if (data.status == "1") {
					var RaceCategory = data.RaceCategory.split("$");
					for(var i=0; i<$('#select_distance').children().length;i++)
					{
						if(RaceCategory.includes($('#select_distance option')[i].value))
						{
							if(RaceCategory.includes($('#select_distance option:selected').val()))
							{
								$('#select_distance option')[i].selected="selected";	
								selectCOunt++;								
							}
							if(selectCOunt==0)
							{
								$('#select_distance option')[0].selected="selected";	
							}
						}
						else{
							$('#select_distance option')[i].classList.add("d-none")	
						}
						
					}
					var extraFields = data.extra.split("$");
					for (var i = 0; i < extraFields.length; i++) 
					{
						if (extraFields[i] === "Employee ID") {
                            $(".EmployeeID").removeClass('d-none');
                        }
						if (extraFields[i] === "Employee Email") {
                            $(".EmployeeEmailId").removeClass('d-none');
                        }
					}
					$('.reference_code_error').addClass('d-none');
					$('.reference_code_success').text('Coupon Code Applied!');
					$('.reference_code_success').removeClass('d-none');
					$('#rstat').val("yes");
					$('#RunType1').click();
					$('#RunType2').parent().addClass('d-none');					
                }
                else {
					for(var i=0; i<$('#select_distance').children().length;i++)
					{
						$('#select_distance option')[i].classList.remove("d-none")
					}
					$(".EmployeeID").addClass('d-none');
                    $(".EmployeeEmailId").addClass('d-none');
					$('#rstat').val("no");
					$('.reference_code_success').addClass('d-none');
					$('.reference_code_error').text('Invalid Coupon Code!');
					$('.reference_code_error').removeClass('d-none');
					$('#RunType2').parent().removeClass('d-none');
                }
            }
		});
	}
	else{
		for(var i=0; i<$('#select_distance').children().length;i++)
		{
			$('#select_distance option')[i].classList.remove("d-none")
		}
		$(".EmployeeID").addClass('d-none');
        $(".EmployeeEmailId").addClass('d-none');
		$('#Rstat').val("no");
		$('.reference_code_success').addClass('d-none');
		$('.reference_code_error').addClass('d-none');
		$('#RunType2').parent().removeClass('d-none');
	}	
})
/*Employee Id Validation*/
$('#EmployeeID').blur(function(){
	if($('#EmployeeID').val().length>=4)
	{
		$.ajax({
            type: "POST",
			data: JSON.stringify({ 'EmployeeID': $('#EmployeeID').val()}),
            url: "/api/Marathon/CheckEmployeeCode",
            contentType: "application/json",
            success: function (data) {
                if (data.status != "") {
					$('#EmployeeID').next().remove();
					var tag ="<span class='errormsg'>"+data.status+".</span>";
					$('#EmployeeID').after(tag);
                }
                else {
                    $('#EmployeeID').next('.errormsg').text('');
                }
            }
    });
	}
	else{
		$('#EmployeeID').next('.errormsg').text('');
	}

	
})
/*PAN Number Validation*/
$("#PANNumber").blur(function () {
    var PANNumber = /^[A-Z]{5}[0-9]{4}[A-Z]{1}$/g;
    if (!PANNumber.test($("#PANNumber").val())) {
        $("#PANNumber").next('.errormsg').removeClass('d-none');
        return false;
    } else {
        $("#PANNumber").next('.errormsg').addClass('d-none')
    }
});
/*Donation Amount*/
$('#DonationAmount').blur(function(){
	if($('#DonationAmount').val().length>=3)
	{
		$.ajax({
            type: "POST",
			data: JSON.stringify({ 'RaceDistance': $('#select_distance_charity').val(),'DonationAmount': $('#DonationAmount').val()}),
            url: "/api/Marathon/MinimumDonationAmount",
            contentType: "application/json",
            success: function (data) {
                if (parseInt($('#DonationAmount').val()) < parseInt(data.minimumAMount)) {
					$('.AmountDetails').addClass('d-none');
					$('#DonationAmount').next('span').remove();
					var tag ="<span class='errormsg' style='display: block;'> Minimum amount "+data.minimumAMount+".</span>";
					$('#DonationAmount').after(tag);
					$('#first_btn').attr('disabled','disabled');
                }
                else {
                    $('#DonationAmount').next('.errormsg').text('');
					$('.registrationValue').text(" "+data.minRaceAmount);
					$('.DonationValue').text(" "+parseFloat($('#DonationAmount').val())-parseFloat(data.minRaceAmount));
					$('.AmountDetails').removeClass('d-none');
					$('#first_btn').removeAttr('disabled');
                }
            }
    });
	}
	else{
		$('#DonationAmount').next('.errormsg').text('');
	}	
})
$('#select_distance_charity').change(function(){
	if($('#select_distance_charity').val()!='')
	{
		$.ajax({
            type: "POST",
			data: JSON.stringify({ 'RaceDistance': $('#select_distance_charity').val(),'DonationAmount': $('#DonationAmount').val()}),
            url: "/api/Marathon/MinimumDonationAmount",
            contentType: "application/json",
            success: function (data) {
                if (parseInt($('#DonationAmount').val()) < parseInt(data.minimumAMount)) {
					$('.AmountDetails').addClass('d-none');
					$('#DonationAmount').next('span').remove();
					$('#DonationAmount').val(data.minimumAMount);
					$('.registrationValue').text(" "+data.minRaceAmount);
					$('.DonationValue').text(" "+parseFloat($('#DonationAmount').val())-parseFloat(data.minRaceAmount));
					$('.AmountDetails').removeClass('d-none');
                }
                else {
					$('#DonationAmount').val(data.minimumAMount);
                    $('#DonationAmount').next('.errormsg').text('');
					$('.registrationValue').text(" "+data.minRaceAmount);
					$('.DonationValue').text(" "+parseFloat($('#DonationAmount').val())-parseFloat(data.minRaceAmount));
					$('.AmountDetails').removeClass('d-none');
                }
            }
    });
	}
	else{
		$('#DonationAmount').next('.errormsg').text('');
	}	
})

/*On refresh value of registration form*/
$(document).ready(function () {
	$('.LoginresendSMSOTP').css('padding','0px');
	$('.LoginresendSMSOTP').css('margin-top','10px');
	if(localStorage.ActiveTab!='')
	{
		$('#'+localStorage.ActiveTab).click();
	}
	if(localStorage.singleDistance!='')
	{
		$('.panel').removeClass('pannelActive');
		$('#'+localStorage.singleDistance).addClass('pannelActive');
		if($('#'+localStorage.singleDistance).children('.panel-collapse').children().children().children().children().attr('value')==localStorage.singleRuntype)
		{
			$('#'+localStorage.singleDistance).children('.panel-collapse').children().children('.accordion-regular-bibs:first-child').children().children().prop('checked', true)
		}
		if($('#'+localStorage.singleDistance).children('.panel-collapse').children().children('.bibs-charity').children().children().attr('value')==localStorage.singleRuntype)
		{
			$('#'+localStorage.singleDistance).children('.panel-collapse').children().children('.bibs-charity').children().children().prop('checked', true)
		}		
	}
});

$('.fa-angle-down').click(function(){
    $(this).addClass('d-none');
    $(this).siblings('.fa-angle-up').removeClass('d-none');
})
$('.fa-angle-up').click(function(){
    $(this).addClass('d-none');
    $(this).siblings('.fa-angle-down').removeClass('d-none');
})

$(document).ready(function () {
	if($('#select_distance_charity').val()!='' && $('#DonationAmount').val()!='')
	{
		$('#DonationAmount').click();$('#DonationAmount').blur();
	}
})
/*Already registerd User Registration and Payment*/
$('#AlreadyRegistredUserSubmit').click(function(e){
	$('#AlreadyRegistredUserSubmit').attr('disabled','disabled')
	if($('input[name="RunType"]:checked').val()=='Normal')
	{
		if($('#select_distance').val()=='')
		{
			var tag ="<span class='errormsg'> Please select distance</span>";
			$('#select_distance_charity').after(tag);
			$('#AlreadyRegistredUserSubmit').removeAttr('disabled');
			return false;
		}
		else{
			if($('#select_distance_charity').next().hasClass('.errormsg'))
			{$('#select_distance_charity').next('.errormsg').text('');}
		}
	}
	if($('input[name="RunType"]:checked').val()=='Charity')
	{
		if($('#select_distance_charity').val()=='')
		{
			var tag ="<span class='errormsg'> Please select distance</span>";
			$('#select_distance_charity').after(tag);
			$('#AlreadyRegistredUserSubmit').removeAttr('disabled');
			return false;
		}
		else{
			if($('#select_distance_charity').next().hasClass('.errormsg'))
			{$('#select_distance_charity').next('.errormsg').text('');}
		}
		if(!$('.donationAmount').hasClass('d-none') && $('#DonationAmount').val()=='0.00')
		{
			$('#DonationAmount').click();$('#DonationAmount').blur();
			$('#AlreadyRegistredUserSubmit').removeAttr('disabled');
			return false;
		}
		if($('#TaxExemptionCertificate:checked').val())
		{
			if($('#PANNumber').val()=='')
			{
				var tag ="<span class='errormsg'> Please enter PAN Number</span>";
				$('#PANNumber').after(tag);
				$('#AlreadyRegistredUserSubmit').removeAttr('disabled');
				return false;
			}
			else{
				if($('#PANNumber').next().hasClass('.errormsg'))
				{$('#PANNumber').next('.errormsg').text('');}
			}
			if($('#TaxExemptionCause').val()=='')
			{
				var tag ="<span class='errormsg'> Please select Tax Exemption Cause</span>";
				$('#TaxExemptionCause').after(tag);
				$('#AlreadyRegistredUserSubmit').removeAttr('disabled');
				return false;
			}
			else{
				if($('#TaxExemptionCause').next().hasClass('.errormsg'))
				{$('#TaxExemptionCause').next('.errormsg').text('');}
			}
		}
	}
	AlreadyRegisteruserNewRegistration();
	e.preventDefault();
})

function AlreadyRegisteruserNewRegistration(e)
{
	  grecaptcha.ready(function () {
        grecaptcha.execute('6Lffy6MmAAAAAJWCrNIqaaSIz3mHsEy_xpKrmiaY', { action: 'AlreadyRegisteredUser' }).then(function (token) {
            $('.g-recaptcha').val(token);
			var distance;
			if($('input[name="RunType"]:checked').val()=="Charity")
			{
				distance=$("#select_distance_charity").val();
			}
			else{
				distance=$("#select_distance").val();
			}
			 var registeredUser = {
                RunMode: $("#runMode1").val(),
                ReferenceCode: $("#reference_code").val(),
                RunType: $('input[name="RunType"]:checked').val(),
                RaceDistance: distance,
                EmployeeID: $("#EmployeeID").val(),
                EmployeeEmailId: $("#EmployeeEmailId").val(),
                DonationAmount: $("#DonationAmount").val(),
                PANNumber: $("#PANNumber").val(),
                TaxExemptionCertificate: $("#TaxExemptionCertificate").val(),
                TaxExemptionCause: $("#TaxExemptionCause").val(),
                reResponse: token
            };
			
			$.ajax({
				type:"POST",
				data: JSON.stringify(registeredUser),
                url: "/api/Marathon/AlreadyRegisteredUser",
                contentType: "application/json",
                success: function (data) {
					if (data.redirect != '' && data.redirect != '')
					{
						window.location = data.redirectUrl;
					}
				}
			})
		})
	  })
}
$("#PayNowbutton").click(function (e) {
	PayNowAndCompleteRegistration();
	e.preventDefault();
});

function PayNowAndCompleteRegistration(e)
{
	  grecaptcha.ready(function () {
        grecaptcha.execute('6Lffy6MmAAAAAJWCrNIqaaSIz3mHsEy_xpKrmiaY', { action: 'PayNowAndCompleteRegistration' }).then(function (token) {
            $('.g-recaptcha').val(token);
			
			$.ajax({
				type:"POST",
                data: JSON.stringify({ 'reResponse': token}),
                url: "/api/Marathon/PayNowAndCompleteRegistration",
                contentType: "application/json",
                success: function (data) {
					if (data.redirect == '1' && data.redirect != '')
					{
						window.location = data.redirectUrl;
					}
				}
			})
		})
	  })
}

$('#RemoveNowbutton').click(function(){
	window.location = "/userinfo";
})



$('input[name="FirstName"]').blur(function(){
	if($(this).next().hasClass('errormsg'))
	{$(this).next('.errormsg').text('');}
    if($(this).val().length<2)
    {
		var tag ="<span class='errormsg'> Please enter a valid First Name</span>";
		$(this).after(tag);
		$('#second-next').removeAttr('disabled');
    }
	else{
		$('#scond-next').removeAttr('disabled');
	}
})
$('input[name="LastName"]').blur(function(){
	if($(this).next().hasClass('errormsg'))
	{$(this).next('.errormsg').text('');}
    if($(this).val().length<2)
    {
		var tag ="<span class='errormsg'> Please enter a valid Last Name</span>";
		$(this).after(tag);
		$('#second-next').removeAttr('disabled');
    }
	else{
		$('#second-next').removeAttr('disabled');
	}
})

if(location.href.toLowerCase().includes('welcomerunner'))
{
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_12fb61f9-5453-4a8f-907a-78b45da230e2__Value').val($('#first_name').text().trim());
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_617b523c-f3cb-4ed1-913e-de4724fc794a__Value').val($('#last_name').text().trim());
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_e1dbf6da-8a77-45c4-9585-b02b87fa8a08__Value').val($('#email_id').text().trim());
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_3aa50b43-a462-4938-a299-03ed1f0ff568__Value').val($('#contactno').text().trim());
	for(i=0;i<$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_61ed9c2e-0346-479c-8f81-09a999900e50__Value').children().length;i++)
	{
		if($('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_61ed9c2e-0346-479c-8f81-09a999900e50__Value option')[i].value.toLowerCase()==$('.usergender').text().toLowerCase().trim())
		{
		$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_61ed9c2e-0346-479c-8f81-09a999900e50__Value option')[i].selected = 'selected';
		}
	}
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_12fb61f9-5453-4a8f-907a-78b45da230e2__Value').addClass('d-none');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_617b523c-f3cb-4ed1-913e-de4724fc794a__Value').addClass('d-none');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_e1dbf6da-8a77-45c4-9585-b02b87fa8a08__Value').addClass('d-none');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_3aa50b43-a462-4938-a299-03ed1f0ff568__Value').addClass('d-none');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_61ed9c2e-0346-479c-8f81-09a999900e50__Value').addClass('d-none');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_e1dbf6da-8a77-45c4-9585-b02b87fa8a08__ItemId').parent().addClass('d-none');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_Index_3aa50b43-a462-4938-a299-03ed1f0ff568').parent().addClass('d-none');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_01e1fb67-1538-49ed-8191-df4d11ac75f5__Value').css('padding','0px 10px');
	$('#fxb_73b665ad-266c-4572-b8fc-5e19b3f6dafa_Fields_01e1fb67-1538-49ed-8191-df4d11ac75f5__Value').css('border','1px solid black');

}

/*City Dropdown Issue*/
$(document).ready(function () {
    if (location.href.toString().toLowerCase().includes('userinfo')) {
        var state;
        for (i = 0; $('#state_info option').length; i++) {
            if ($('#state_info option')[i].text.trim() == $('#State').val().trim()) {
                $('#state_info option')[i].selected = 'true';
                state = $('#state_info option')[i].value;
                break;
            }
        }
        $.ajax({
            url: '/api/Marathon/getCity',
            contentType: "application/json",
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify({ 'id': state }),
            success: function (city) {
                $.each(city, function (i, city) {
                    $("#city_info").append('<option value="'
                        + city.Value + '">'
                        + city.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed.' + ex);
            }
        });
    }
    if (location.href.toString().toLowerCase().includes('marathonregisterationinfo')) {
        var state;
        for (i = 0; $('#state_info option').length; i++) {
            if ($('#state_info option')[i].text.trim() == $('#State').val().trim()) {
                $('#state_info option')[i].selected = 'true';
                state = $('#state_info option')[i].value;
                break;
            }
        }
        $.ajax({
            url: '/api/Marathon/getCity',
            contentType: "application/json",
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify({ 'id': state }),
            success: function (city) {
                $.each(city, function (i, city) {
                    $("#city_info").append('<option value="'
                        + city.Value + '">'
                        + city.Text + '</option>');
                });
            },
            error: function (ex) {
                alert('Failed.' + ex);
            }
        });
    }
})
$('input[name="IDCardAttachment"]').change(function () {
    $('input[name="IDCardAttachment"]').next('.errormsg').text('');
    var selectedFile = document.querySelector('input[name="IDCardAttachment"]').files[0];
    var allowedTypes = ['image/jpeg', 'image/png', 'application/pdf'];
    var fsize = selectedFile.size;
    fsize = Math.round((fsize / 1024));
    if (!allowedTypes.includes(selectedFile.type)) {
        $('#second-next').attr('disabled', 'disabled');
        $('input[name="IDCardAttachment"]').after("<span class='errormsg'>Unsupported file format. Please upload a file in JPEG, PNG, or PDF format.</span>");
        document.querySelector('input[name="IDCardAttachment"]').value = '';
        return false;
    }
    else if (fsize >= 10240) {
        $('#second-next').attr('disabled', 'disabled');
        $('input[name="IDCardAttachment"]').after("<span class='errormsg'>The file is too large. Please choose a file smaller than 10MB.</span>");
        document.querySelector('input[name="IDCardAttachment"]').value = '';
        return false;
    }
    else {
        $('#second-next').removeAttr('disabled');
        $('input[name="IDCardAttachment"]').next('.errormsg').text('');
    }
})

/*Address Validation*/
$("#address").keyup(function () {
    if ($("#address").val().length > 3) {
        $("#address").next('.errormsg').text('');
        var Address = /^[a-zA-Z0-9() .,-/'\"]{3,500}$/g;
        if (!Address.test($("#address").val())) {
            $('#third-next').attr('disabled', 'disabled');
            $("#address").after("<span class='errormsg'>Only characters A-Z, a-z, 0-9, (), ., -, ', /, \" and , are permitted.</span>");
            return false;
        } else {
            $('#third-next').removeAttr('disabled');
            $("#address").next('.errormsg').text('');
        }
    }
});

$("#address").blur(function () {
    if ($("#address").val().length < 3) {
        $("#address").next('.errormsg').text('');
        $('#third-next').removeAttr('disabled');
        $("#address").after("<span class='errormsg'>Please enter a valid address.</span>");
    }
});

