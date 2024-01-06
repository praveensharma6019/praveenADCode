
$(document).ready(function () {

    $("input.calculator").on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

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

$(".eqiptiles").on("click", function (obj) {
    $("." + obj.currentTarget.id)[0].style.display = "block";
    var node = $(".calc");
    for (var i = 0; i < node.length; i++) {
        var id = $("." + obj.currentTarget.id)[0].id;
        if (node[i].id != id) {
            $("div[name=" + node[i].id + "]")[0].style.display = "none"
        }
    }
})

function dayschanges() {
    var grandtotal = 0;
    var tot = 0;
    var days = $("#days").val();
    var parentnode = $(".energy");
    for (var j = 0; j < parentnode.length; j++) {
        for (var i = 1; i < parentnode[i].children.length - 2; i++) {
            if (parseInt(parentnode[j].children[i].children[1].children[0].value) && parseInt(parentnode[j].children[i].children[1].children[0].value) != 0) {
                if (tot == 0) {
                    tot = parseInt(parentnode[j].children[i].children[1].children[0].value);
                } else if (parseInt(parentnode[j].children[i].children[1].children[0].value)) {
                    tot = tot * parseInt(parentnode[j].children[i].children[1].children[0].value);
                }
            }
            else if (parseInt(parentnode[j].children[4].children[1].children[0].value) != 0) { break; }
            else { tot = 0; break; }
        }
        if (parseInt(parentnode[j].children[3].children[1].children[0].value) == 0 && parseInt(parentnode[j].children[4].children[1].children[0].value) != 0) {
            var hr = (parseInt(parentnode[j].children[4].children[1].children[0].value) / 60);
            tot = (tot * hr);
        }
        if (parseInt(parentnode[j].children[3].children[1].children[0].value) != 0 && parseInt(parentnode[j].children[4].children[1].children[0].value) != 0) {
            var hr = (parseInt(parentnode[j].children[4].children[1].children[0].value) / 60);
            tot += (tot * hr);
        }
        tot = (tot * days) / 1000;
        //parentnode[j].children[5].children[1].children[0].value = tot;
       // grandtotal += tot;
        grandtotal = parseFloat(grandtotal) + parseFloat(tot);
        grandtotal = parseFloat(grandtotal).toFixed(3);
        tot = 0;
        $("#grandtotal").val(grandtotal);
    }
}
function calc(obj) {
    var tot = 0;
    var days = $("#days").val();
    var parentnode = obj.parentElement.parentNode.parentElement;
    for (var i = 1; i < parentnode.children.length - 2; i++) {
        if (parseInt(parentnode.children[i].children[1].children[0].value) && parseInt(parentnode.children[i].children[1].children[0].value) != 0) {
            if (tot == 0) {
                tot = parseInt(parentnode.children[i].children[1].children[0].value);
            } else if (parseInt(parentnode.children[i].children[1].children[0].value)) {
                tot = tot * parseInt(parentnode.children[i].children[1].children[0].value);
            }
        }
        else if (parseInt(parentnode.children[4].children[1].children[0].value) != 0) { break; }
        else { tot = 0; break; }
    }
    if (parseInt(parentnode.children[3].children[1].children[0].value) == 0 && parseInt(parentnode.children[4].children[1].children[0].value) != 0) {
        var hr = (parseInt(parentnode.children[4].children[1].children[0].value) / 60);
        tot = (tot * hr);
    }
    if (parseInt(parentnode.children[3].children[1].children[0].value) != 0 && parseInt(parentnode.children[4].children[1].children[0].value) != 0) {
        var hr = (parseInt(parentnode.children[4].children[1].children[0].value) / 60);
        tot += (tot * hr);
    }
    tot = (tot * days) / 1000;
    parentnode.children[5].children[1].children[0].value = tot;
    dayschanges();
}

