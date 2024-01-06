$(document).ready(function () {
    $("#loader-wrapper").hide();
    var message = $("#message").val();
    if (message !== undefined && message !== null && message !== "") {
        $('#message_modal').modal('show');
        $("#message").val("");
    }

    if (localStorage['page'] == document.URL && localStorage['scrollTop'] != null && localStorage['scrollTop'] != "") {
        $(document).scrollTop(localStorage['scrollTop']);
    }
    else {
        $(document).scrollTop(0);
        localStorage['page'] = "";
        localStorage['scrollTop'] = "";
    }

    OnLoadChecks();
});

//function GetScrollPosition() {
//    localStorage['page'] = document.URL;
//    localStorage['scrollTop'] = $(document).scrollTop();
//}

$("#frmUserFeedback").submit(function (event) {

    var buttonName = $(document.activeElement).attr('name');
    if (buttonName === "SubmitAndClose") {
        //answer more questions popup
        $('#answermorequestion_modal').modal("show");
        event.preventDefault();
        return false;
    }

    //GetScrollPosition();
    $("#Recomandation_scale_Adani_Electricity").val($("#range-slider").attr("data-value"));
    return true;
});

function OnLoadChecks() {
    var checkedValue = $("input[name='Attitude_Empathy']:checked").val();
    if (checkedValue == "3" || checkedValue == "2" || checkedValue == "1") {
        $("#Attitude_Empathy_Unsatisfied_reason").show();
    }
    else {
        $("#Attitude_Empathy_Unsatisfied_reason").hide();
    }

    checkedValue = $("input[name='AEML_could_be_doing_differently']:checked").val();
    if (checkedValue == "5") {
        $("#AEML_could_be_doing_differently_Other").show();
    }
    else {
        $("#AEML_could_be_doing_differently_Other").hide();
    }

    checkedValue = $("input[name='Quality']:checked").val();
    if (checkedValue == "3" || checkedValue == "2" || checkedValue == "1") {
        $("#Quality_Unsatisfied_reason").show();
    }
    else {
        $("#Quality_Unsatisfied_reason").hide();
    }

    checkedValue = $("input[name='Process']:checked").val();
    if (checkedValue == "3" || checkedValue == "2" || checkedValue == "1") {
        $("#Process_Unsatisfied_reason").show();
    }
    else {
        $("#Process_Unsatisfied_reason").hide();
    }

    if ($("#AnswerMoreQuestions").val() == "true") {
        $("#moreQuestionAskDiv").hide();
        $("#moreQuestionShowDiv").show();

        $("#moreQuestionDiv").show();
        $("#AnswerMoreQuestions").val(true);
    }

}

$('.answermorequestion_modalbtnYesNO').click(function (e) {
    if (this.value === '1') {
        YesNO = '1';
        $('#answermorequestion_modal').modal("hide");
        $("#moreQuestionAskDiv").hide();
        $("#moreQuestionShowDiv").show();
        $("#moreQuestionDiv").show();
        $("#AnswerMoreQuestions").val(true);
        //$("#SubmitAndClose").click();
    }
    else {
        $('#answermorequestion_modal').modal("hide");
        $("#SubmitAndClose").click();
    }
    e.preventDefault();
});

$("#WantToAnswerMoreQuestions").click(function () {
    $("#moreQuestionAskDiv").hide();
    $("#moreQuestionShowDiv").show();
    $("#moreQuestionDiv").show();
    $("#AnswerMoreQuestions").val(true);
});

$('.OverallExperience').change(function (event) {
    //GetScrollPosition();
    $(".OverallExperienceError").html("");
});

$('.Attitude_Empathy').change(function (event) {
    //GetScrollPosition();
    $(".Attitude_EmpathyError").html("");
    var checkedValue = $("input[name='Attitude_Empathy']:checked").val();
    if (checkedValue == "3" || checkedValue == "2" || checkedValue == "1") {
        $("#Attitude_Empathy_Unsatisfied_reason").show();
    }
    else {
        $("#Attitude_Empathy_Unsatisfied_reason").hide();
    }
});

$('.Quality').change(function (event) {
    //GetScrollPosition();
    $(".QualityError").html("");
    var checkedValue = $("input[name='Quality']:checked").val();
    if (checkedValue == "3" || checkedValue == "2" || checkedValue == "1") {
        $("#Quality_Unsatisfied_reason").show();
    }
    else {
        $("#Quality_Unsatisfied_reason").hide();
    }
});

$('.Process').change(function (event) {
    //GetScrollPosition();
    $(".ProcessError").html("");
    var checkedValue = $("input[name='Process']:checked").val();
    if (checkedValue == "3" || checkedValue == "2" || checkedValue == "1") {
        $("#Process_Unsatisfied_reason").show();
    }
    else {
        $("#Process_Unsatisfied_reason").hide();
    }
});

$('.AEML_could_be_doing_differently').change(function (event) {
    //GetScrollPosition();
    var checkedValue = $("input[name='AEML_could_be_doing_differently']:checked").val();
    if (checkedValue == "5") {
        $("#AEML_could_be_doing_differently_Other").show();
    }
    else {
        $("#AEML_could_be_doing_differently_Other").hide();
    }
});

var max = 5, // Set max value
    initvalue = 5, // Set the initial value
    icon = "fa-fire", // Set the icon (https://fontawesome.com/icons)
    target = document.querySelectorAll('[data-value]'),
    listIcon = document.getElementById("labels-list");

// Function to update du value

function updateValue(target, value) {
    target.forEach(function (currentIndex) {
        currentIndex.dataset.value = value;
    });
}

// Init the number of item with the initial value settings

for (i = 0; i < max; i++) {
    var picto = "<i class='fas " + icon + "'></i>";
    $(".labels").append(picto);
}

updateValue(target, initvalue);

// Update the slider on click

$('.fas').on("click", function () {
    var index = $(this).index() + 1;
    $("#range-slider").slider("value", index);
    updateValue(target, index);
});


// Init the slider

$("#range-slider").slider({
    range: "min",
    value: initvalue,
    min: 1,
    max: max,

    slide: function (event, ui) {
        updateValue(target, ui.value);
    }
});
