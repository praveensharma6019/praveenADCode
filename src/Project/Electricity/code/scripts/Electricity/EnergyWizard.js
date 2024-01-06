
$(document).ready(function () {
    var rowdiv = $(".question");
    for (var i = 0; i < rowdiv.length; i++) {
        if (i != 0) {
            $(".question")[i].style.display = "none";
        }
    }
    var innerdiv = $(rowdiv[0]).find('.ApplianceList');
    for (var i = 0; i < rowdiv.length; i++) {
        if (i == 0) {
            $(innerdiv)[i].style.display = "block";
        }
        else {
            $(innerdiv)[i].style.display = "none";
        }
    }
});
function checkbox(obj) {
    var array = $(obj).parent().parent().find('input[type="hidden"]')
    for (var i = 1; i < array.length; i++) {
        if (array[i].id.indexOf(obj.id) != -1) {
            array[i].value = true;
            $("#" + obj.id).prop("checked", true);
        } else {
            array[i].value = false;
            //if ($("#" + array[i].id).is(':checked'))
            //    $("#" + array[i].id).attr("checked", false);
        }
        i = i + 1;
    }
    var inputarray = obj.parentNode.parentNode.children;
    for (var i = 1; i < inputarray.length; i++) {
        if (inputarray[i].children[0].id.indexOf(obj.id) == -1) {
            $("#" + inputarray[i].children[0].id).prop("checked", false);
        }
    }
}
function Changepage(obj, current) {
    var flag = true;
    var array = $(obj[0]).find('.opt');
    $.each(array, function (index, data) {
        if ($(data).find('input[type=radio]:checked').length == 0) {
            flag = false;
            $(current.parentNode.parentNode)[0].children[0].style.display = "block";
            return false;
        }
    });
    if (flag) {
        if ($($(obj)[0].nextElementSibling).length > 0) {
            var next = $(obj)[0].nextElementSibling.style.display = "block";
            $(current.parentNode.parentNode)[0].children[0].style.display = "none";
            $(obj)[0].style.display = "none";
        }

        //var previous = $(obj)[0].previousElementSibling;
        //$.each(next, function (index, sbling) {
        //    $(sbling).style.display = "block";
        //    $(obj)[0].style.display = "none";
        //});
        return true;
    }
}

function changesubpage(obj, current) {
    var flag = true;
    var array = $(obj).find('.opt');
    $.each(array, function (index, data) {
        if ($(data).find('input[type=radio]:checked').length == 0) {
            flag = false;
            $(current.parentNode.parentNode)[0].children[0].style.display = "block";
            return false;
        }
    });
    if (flag) {
        if ($($(obj)[0].nextElementSibling).length > 0) {
            var next = $(obj)[0].nextElementSibling.style.display = "block";
            $(current.parentNode.parentNode)[0].children[0].style.display = "none";
        }
        $(obj)[0].style.display = "none";
        //var previous = $(obj)[0].previousElementSibling;
        //$.each(next, function (index, sbling) {
        //    $(sbling).style.display = "block";
        //    $(obj)[0].style.display = "none";
        //});
        return true;
    }
}
        //$('#survey').on("submit", function (obj, current) {
        //    if (Changepage(obj, current)) {
        //        alert("true");
        //    } else {
        //        alert("false");
        //    }
        //});
