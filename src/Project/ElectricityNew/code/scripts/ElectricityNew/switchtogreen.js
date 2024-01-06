$(document).ready(function () {

    $("#readMore").on('click', function (e) {
        e.preventDefault();
        $("#readMore").hide();
        $("#readLess").show();
        $("#readLessSection").show();
    });

    $("#readLess").on('click', function (e) {
        e.preventDefault();
        $("#readMore").show();
        $("#readLess").hide();
        $("#readLessSection").hide();
    });

    $("#loadMore").on('click', function (e) {
        e.preventDefault();
        $(".EV:hidden").slice(0, 16).slideDown();
        $("#loadMore").hide();
        $("#showLess").show();
        if ($(".EV:hidden").length == 0) {
            $("#load").fadeOut('slow');
        }

    });

    $("#showLess").on('click', function (e) {
        e.preventDefault();
        $(".EVs").show();
        $(".EV").hide();
        $("#loadMore").show();
        $("#showLess").hide();
        if ($(".EV:hidden").length == 0) {
            $("#load").fadeOut('slow');
        }

    });

    if (localStorage['page'] == document.URL && localStorage['scrollTop'] != null && localStorage['scrollTop'] != "") {
        $(document).scrollTop(localStorage['scrollTop']);
    }
    else {
        $(document).scrollTop(0);
        localStorage['page'] = "";
        localStorage['scrollTop'] = "";
    }

    var selectedFormType = $("#formTypeVal").val();
    if (selectedFormType != null) {
        if (selectedFormType == "O") {
            //show organization div
            tab(event, 'organizations', 'signIn');
        }
        else if (selectedFormType == "S") {
            tab(event, 'evChargersSoc', 'evChargers');
        }
        else if (selectedFormType == "P") {
            tab(event, 'evChargersStr', 'evChargers');
        }
    }
});

$("#frmCreate1").submit(function () {
    GetScrollPosition();
    return true;
});
$("#frmCreate2").submit(function () {
    GetScrollPosition();
    return true;
});
$("#frmCreate3").submit(function () {
    GetScrollPosition();
    return true;
});
$("#frmCreate4").submit(function () {
    GetScrollPosition();
    return true;
});
$("#frmCreate5").submit(function () {
    GetScrollPosition();
    return true;
});


function tab(evt, tab, holder) {
    let i;
    let tabs;
    let content;
    let current = document.getElementById(tab);
    let wrapper = document.getElementById((holder ? holder : 'xyz'));

    if (wrapper) {
        content = wrapper.getElementsByClassName("tab-content-wrapper");
        for (i = 0; i < content.length; i++) {
            content[i].className = content[i].className.replace(" active", "");
        }

        tabs = wrapper.getElementsByClassName("tab-item");
        for (i = 0; i < tabs.length; i++) {
            tabs[i].className = tabs[i].className.replace(" active", "");
        }

        if (current) {
            current.className += " active";
        }
        if (evt != null && evt.currentTarget != null && evt.currentTarget.className != null)
            evt.currentTarget.className += " active";
        else {
            var formval = $("#formTypeVal").val();
            if (formval == "O") {
                $("#OrgTab").addClass(" active");
            }
            else if (formval == "S") {
                $("#EV2Tab").addClass(" active");
            }
            else if (formval == "P") {
                $("#EV3Tab").addClass(" active");
            }
        }
    }
}


function GetScrollPosition() {
    localStorage['page'] = document.URL;
    localStorage['scrollTop'] = $(document).scrollTop();
}

//GreenPledge
$('#organizations_sec').click(function () {
    $(this).toggleClass('checked');
});

$('#individual_sec').click(function () {
    $(this).toggleClass('checked');
});