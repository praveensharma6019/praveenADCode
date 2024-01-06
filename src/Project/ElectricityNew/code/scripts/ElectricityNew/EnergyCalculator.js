var CurrentCat = 2;
var totalunitSum = 0;
$(document).ready(function () {
    $("input.validate").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    clearFieldValues();
    DaysSelectChange();
    totalUnits();

    $('.menuSelect').click(function () {
        $(".menuSelect").each(function () {
            $(this).attr('src', $(this).attr('data-src'));

        });
        $(this).attr('src', $(this).attr('data-srcs'));
    });
    var node = $(".calc");
    for (var i = 0; i < node.length; i++) {
        if (i == 0) {
            $(".calc")[i].style.display = "block"
        } else { $(".calc")[i].style.display = "none" }
    }
});

window.CurrentCategory = CurrentCategory
function CurrentCategory(obj) {
    // $("#days").val($("#days option:first").val()).change();
    //ResetDropdownContainer();
    CurrentCat = obj;
    DaysSelectChange();
    totalUnits();
}

function ResetDropdownContainer() {
    $($("#days").parent().find('input.select-dropdown')).val($("#days option:first").text());
    /*	$('ul.select-dropdown li').each(function(i)
        {
           $(this).removeClass('selected'); 
        });
        $($('ul.select-dropdown li')[0]).addClass("selected");
        */
}

function clearFieldValues() {
    $("div .container .bill-calculator").find("input.calfld").val("");
    $("div .container .bill-calculator").find("input.units").val("");
    //$("div .container .bill-calculator").find('select option[value="1"]').attr("selected", true);
}
$('.bill-calculator .owl-item:first-child .card.bill-item').addClass('active');


window.calculate = calculate

function calculate(obj) {

    var total = 0.0;
    var days = $("#days").val();

    var units = 1.0;
    var counter = 0;
    var actualLoad = $(obj).closest("li.calparent").find(".actualLoad").val();
    var quantity = $(obj).closest("li.calparent").find(".quantity").val();
    var duration = $(obj).closest("li.calparent").find(".duration").val();
    duration = duration / 60;
    units = (days * actualLoad * quantity * duration) / 1000;
    $(obj).closest("li.calparent").find(".units").val(units.toFixed(3));
    totalUnits();
}
window.totalUnits = totalUnits
function totalUnits() {
    var totalUnit = 0;

    for (let i = 2; i <= 9; i++) {
        var liListCategory = $("#MainContentContainer").find(".bill-calculator:nth-child(" + i + ")").find("li.calparent");
        $(liListCategory).each(function (index, element) {
            if (($(element).find(".units").val()) != '') {
                totalUnit = totalUnit + parseFloat($(element).find(".units").val()) * $("#days").val();;
            }
        });
    }

    $(".total_units").val(totalUnit.toFixed(3));

}

function totalUnitsByDays(CurrentCat, liCurrent, liList) {
    var totalUnit = 0.0000;
    var count = $(liList).length;
    $(liList).each(function (index, element) {
        totalUnit = totalUnit + parseFloat($(element).find(".units").val());
    });
    totalUnit += parseFloat($(liCurrent).find(".units").val());
    $(".total_units").val(totalUnit);
}

window.DaysSelectChange = DaysSelectChange
function DaysSelectChange() {
    var totalUnit = 0;
    for (let i = 2; i <= 9; i++) {
        var liList = $("#MainContentContainer").find(".bill-calculator:nth-child(" + i + ")").find("li.calparent");
        $(liList).each(function (index, element) {
            var unit = $(element).find(".units").val() * $("#days").val();
            // $(element).find(".units").val(unit);
            totalUnit = totalUnit + unit;
        });
    }
    $(".total_units").val(totalUnit.toFixed(3));
}
//function DaysSelectChange() {
//    var totalUnit = 0;
//    var liList = $("#MainContentContainer").find(".bill-calculator:nth-child(" + CurrentCat + ")").find("li.calparent");
//    $(liList).each(function (index, element) {
//        var unit = $(element).find(".units").val() * $("#days").val();
//        // $(element).find(".units").val(unit);
//        totalUnit = totalUnit + unit;
//    });
//    $(".total_units").val(totalUnit.toFixed(3));

//}
$(".eqiptiles").on("click", function (obj) {
    $("." + obj.currentTarget.id)[0].style.display = "block";
    var node = $(".calc");
    $(this).closest('.owl-item').siblings().find('.card.bill-item.active').removeClass('active');
    $(this).find('.card.bill-item').addClass('active');
    for (var i = 0; i < node.length; i++) {
        var id = $("." + obj.currentTarget.id)[0].id;
        if (node[i].id != id) {
            $("div[name=" + node[i].id + "]")[0].style.display = "none"
        }
    }

})