function calculate() {
    if (validateForm(), "" == $("#state").val() || "" == $("#customer_type").val()) return !1;
    $("#emi_data").hide();
    var t = document.getElementById("checkbox_fill_one").checked,
        e = document.getElementById("checkbox_fill_two").checked;
    document.getElementById("checkbox_fill_three").checked;
    budget_additionally = "";
    var a = $("#state").val(),
        o = $("#state option:selected").text(),
        i = $("#customer_type").val(),
        r = get_state_category(a);
    if (t) {
        if (checkbox_selected = "Roof Top Area", (_ = calculate_plant_size_area()) > 500) return message = "As per Roof Top Area ,capacity found is beyond 500kW. Maximum limit of capacity is 500kW", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1;
        var l = get_benchmark(_, r, i);
        output_without_subsidy = _ * l
    } else if (e) {
        checkbox_selected = "Capacity";
        var _ = $("#capacity_txt").val();
        if (_ > 500) return message = "Maximum limit of capacity is 500kW. Please try less than or equal to 500kW", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1;
        var n = get_benchmark(_, r, i);
        output_without_subsidy = _ * n
    } else {
        if (checkbox_selected = "Budget", budget = $("#budget_txt").val(), (_ = calculate_plant_size_budget(budget, r, i)) > 500) return message = "As per budget ,capacity found is beyond 500kW. Maximum limit of capacity is 500kW", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1;
        var c = get_benchmark(_, r, i);
        budget < c ? (output_without_subsidy = c, budget_additionally = " plus Rs " + (Number(c) - Number(budget)) + " more") : output_without_subsidy = $("#budget_txt").val()
    }
    var d = get_subsidy(_, r, i);
    c = get_benchmark(_, r, i);
    //subsidy2 = 1 - d / 100, output_subsidy = output_without_subsidy * subsidy2, output_subsidy = Number(output_subsidy).toFixed(0), irradiation = get_irradiation(a), generate = .0036 * irradiation * 1.1, generate = Number(generate).toFixed(1), output_electricity_annual = generate * _ * 300, output_electricity_annual = Number(output_electricity_annual).toFixed(0), output_electricity_lifetime = 25 * output_electricity_annual, value_a = .82 * output_electricity_lifetime, value_b = 625, co2 = value_a / 1e3, co2 = Number(co2).toFixed(0), tree = value_a / value_b, tree = Number(tree).toFixed(0), output_electricity_unit = $("#electricity_txt").val(), output_saving_annually = output_electricity_annual * output_electricity_unit, output_saving_monthly = output_electricity_annual / 12 * output_electricity_unit, output_saving_monthly = Number(output_saving_monthly).toFixed(0), output_saving_lifetime = 25 * output_saving_annually, $(".output").html(""), $("#output_state").html(o), $("#output_irradiation").html(irradiation), $("#output_generate").html(generate), $("#output_choosen").html(checkbox_selected), $("#output_budget_additionally").html(budget_additionally), $("#output_size").html(_), $("#bench_cost").html(c), $("#output_without_subsidy").html(output_without_subsidy), $("#output_subsidy_heading").html(d), $("#output_subsidy").html(output_subsidy), $("#output_electricity_annual").html(output_electricity_annual), $("#output_electricity_lifetime").html(output_electricity_lifetime), $("#output_electricity_unit").html(output_electricity_unit), $("#output_saving_monthly").html(output_saving_monthly), $("#output_saving_annually").html(output_saving_annually), $("#output_saving_lifetime").html(output_saving_lifetime), $("#output_co2").html(co2), $("#output_tree").html(tree), $("#capital_cost_txt").val(output_without_subsidy), $("#subsidy_txt").val(d), $("#equity_ratio_txt").val(d), debt_ratio = 100 - d, $("#debt_ratio_txt").val(debt_ratio), down_payment = d / 100 * output_without_subsidy, down_payment = Math.round(down_payment), loan_payment = output_without_subsidy - down_payment, $("#down_payment_txt").val(down_payment), $("#loan_payment_txt").val(loan_payment), $("#calculator_modal").modal("show")
    subsidy2 = 1 - d / 100, output_subsidy = output_without_subsidy * subsidy2, output_subsidy = Number(output_subsidy).toFixed(0), irradiation = get_irradiation(a), output_electricity_annual = Number(output_electricity_annual).toFixed(0), output_electricity_lifetime = 25 * output_electricity_annual, value_a = .82 * output_electricity_lifetime, value_b = 625, co2 = value_a / 1e3, co2 = Number(co2).toFixed(0), tree = value_a / value_b, tree = Number(tree).toFixed(0), output_electricity_unit = $("#electricity_txt").val(), output_saving_annually = output_electricity_annual * output_electricity_unit, output_saving_monthly = output_electricity_annual / 12 * output_electricity_unit, output_saving_monthly = Number(output_saving_monthly).toFixed(0), output_saving_lifetime = 25 * output_saving_annually, $(".output").html(""), $("#output_state").html(o), $("#output_irradiation").html(irradiation), $("#output_choosen").html(checkbox_selected), $("#output_budget_additionally").html(budget_additionally), $("#output_size").html(_), $("#bench_cost").html(c), $("#output_without_subsidy").html(output_without_subsidy), $("#output_subsidy_heading").html(d), $("#output_subsidy").html(output_subsidy), $("#output_electricity_annual").html(output_electricity_annual), $("#output_electricity_lifetime").html(output_electricity_lifetime), $("#output_electricity_unit").html(output_electricity_unit), $("#output_saving_monthly").html(output_saving_monthly), $("#output_saving_annually").html(output_saving_annually), $("#output_saving_lifetime").html(output_saving_lifetime), $("#output_co2").html(co2), $("#output_tree").html(tree), $("#capital_cost_txt").val(output_without_subsidy), $("#subsidy_txt").val(d), $("#equity_ratio_txt").val(d), debt_ratio = 100 - d, $("#debt_ratio_txt").val(debt_ratio), down_payment = d / 100 * output_without_subsidy, down_payment = Math.round(down_payment), loan_payment = output_without_subsidy - down_payment, $("#down_payment_txt").val(down_payment), $("#loan_payment_txt").val(loan_payment), $("#calculator_modal").modal("show");
    
    //generate = .0036 * irradiation * 1.1;
    //generate = Number(generate).toFixed(1);
    generate = get_irradiation_ele(a);
    output_electricity_annual = generate * _ * 300;

    $("#output_generate").html(generate);
}

function emi_calculate() {
    $(".emi_output").html(""), "" != $("#loan_inst_rate_txt").val() && "" != $("#loan_period_txt").val() && (down_payment = $("#down_payment_txt").val(), loan_payment = $("#loan_payment_txt").val(), inst_rate = $("#loan_inst_rate_txt").val(), loan_period = $("#loan_period_txt").val(), p = loan_payment, r = inst_rate / 100, r /= 12, tenure = 12 * loan_period, r_pow = Math.pow(1 + r, tenure), num_emi = r * r_pow, den_emi = r_pow - 1, emi = p * (num_emi / den_emi), emi = Number(emi).toFixed(0), $("#output_emi_amount").html(loan_payment), $("#output_loan_interest").html(inst_rate), $("#output_loan_period").html(loan_period), $("#calculated_emi").html(emi), $("#emi_data").show())
}

function calculate_plant_size_area() {
    return roof_area_txt = $("#roof_area_txt").val(), radio_gp = $('input[name="radio_gp"]:checked').val(), "1" == radio_gp ? roof_area = .092903 * roof_area_txt : roof_area = roof_area_txt, roof_area_percentage = $("#roof_area_percentage_txt").val(), capacity = roof_area * roof_area_percentage / 1e3, capacity = Number(capacity).toFixed(1), capacity
}

function calculate_plant_size_budget(t, e, a) {
    capacity = 1;
    t = parseInt(t);
    for (i = 0; i < data.length; i++)
        if (data[i].category == e && data[i].customer == a) {
            var o = 1e3 * (o = data[i].cost),
                r = data[i].rangeFrom * o,
                l = data[i].rangeTo * o;
            t >= r && t <= l && (capacity = t / o)
        } return capacity > 1 && (capacity = Number(capacity).toFixed(1)), capacity
}

function get_subsidy(t, e, a) {
    var o = 0;
    t = parseFloat(t), a = parseInt(a);
    for (i = 0; i < data.length; i++) data[i].category == e && data[i].customer == a && data[i].rangeFrom <= t && data[i].rangeTo >= t && (o = data[i].subsidy);
    return o
}

function get_benchmark(t, e, a) {
    t = parseInt(t);
    for (benchmark = 0, i = 0; i < data.length; i++) data[i].category == e && data[i].customer == a && t >= data[i].rangeFrom && t <= data[i].rangeTo && (cost = data[i].cost, benchmark = 1e3 * cost);
    if (0 == benchmark)
        for (i = 0; i < data.length; i++) data[i].category == e && t >= data[i].rangeFrom && t <= data[i].rangeTo && (cost = data[i].cost, benchmark = 1e3 * cost);
    return benchmark
}

function get_state_category(t) {
    for (i = 0; i < data_state_catg.length; i++) data_state_catg[i].state == t && (category = data_state_catg[i].category);
    return category
}

function get_irradiation(t) {
    for (i = 0; i < data_irradiation.length; i++) data_irradiation[i].state == t && (irradiation = data_irradiation[i].irradiation);
    return irradiation
}

function get_irradiation_ele(t) {
    for (i = 0; i < data_irradiation_ele.length; i++) data_irradiation_ele[i].state == t && (irradiation_ele = data_irradiation_ele[i].irradiation_ele);
    return irradiation_ele
}

function check_fill(t) {
    $(".chk-grp").val(""), $(".chk-grp").attr("disabled", !0), $(".chk-grp").hide(), $(".txt_option").hide(), "checkbox_fill_one" == t.id && t.checked ? ($(".txt_option1").show(), $(".chk-grp1").show(), $(".chk-grp1").attr("disabled", !1), $("#checkbox_fill_two").attr("checked", !1), $("#checkbox_fill_three").attr("checked", !1)) : "checkbox_fill_two" == t.id && t.checked ? ($(".txt_option2").show(), $(".chk-grp2").show(), $(".chk-grp2").attr("disabled", !1), $("#checkbox_fill_one").attr("checked", !1), $("#checkbox_fill_three").attr("checked", !1), $("#radio_gp").prop("checked", !1)) : "checkbox_fill_three" == t.id && t.checked && ($(".txt_option3").show(), $(".chk-grp3").show(), $(".chk-grp3").attr("disabled", !1), $("#checkbox_fill_one").attr("checked", !1), $("#checkbox_fill_two").attr("checked", !1), $("#radio_gp").prop("checked", !1))
}

function check_fill_one() {
    $("#checkbox_fill_one").is(":checked") ? ($("#roof_area_txt").val(""), $("#roof_area_percentage_txt").val(""), $("#roof_area_txt").attr("disabled", !1), $("#roof_area_percentage_txt").attr("disabled", !1), $("#capacity_txt").attr("disabled", !0), $("#budget_txt").attr("disabled", !0), $("#checkbox_fill_two").attr("checked", !1), $("#checkbox_fill_three").attr("checked", !1)) : ($("#roof_area_txt").val(""), $("#roof_area_percentage_txt").val(""), $("#capacity_txt").val(""), $("#budget_txt").val(""), $("#roof_area_txt").attr("disabled", !0), $("#roof_area_percentage_txt").attr("disabled", !0), $("#capacity_txt").attr("disabled", !0), $("#budget_txt").attr("disabled", !0))
}

function check_fill_two() {
    $("#checkbox_fill_two").is(":checked") ? ($("#roof_area_txt").attr("disabled", !0), $("#roof_area_percentage_txt").attr("disabled", !0), $("#capacity_txt").val(""), $("#capacity_txt").attr("disabled", !1), $("#budget_txt").attr("disabled", !0), $("#checkbox_fill_one").attr("checked", !1), $("#checkbox_fill_three").attr("checked", !1), $("#radio_gp").prop("checked", !1)) : ($("#roof_area_txt").val(""), $("#roof_area_percentage_txt").val(""), $("#capacity_txt").val(""), $("#budget_txt").val(""), $("#roof_area_txt").attr("disabled", !0), $("#roof_area_percentage_txt").attr("disabled", !0), $("#capacity_txt").attr("disabled", !0), $("#budget_txt").attr("disabled", !0))
}

function check_fill_three() {
    $("#checkbox_fill_three").is(":checked") ? ($("#roof_area_txt").attr("disabled", !0), $("#roof_area_percentage_txt").attr("disabled", !0), $("#capacity_txt").attr("disabled", !0), $("#budget_txt").val(""), $("#budget_txt").attr("disabled", !1), $("#checkbox_fill_one").attr("checked", !1), $("#checkbox_fill_two").attr("checked", !1), $("#radio_gp").prop("checked", !1)) : ($("#roof_area_txt").val(""), $("#roof_area_percentage_txt").val(""), $("#capacity_txt").val(""), $("#budget_txt").val(""), $("#roof_area_txt").attr("disabled", !0), $("#roof_area_percentage_txt").attr("disabled", !0), $("#capacity_txt").attr("disabled", !0), $("#budget_txt").attr("disabled", !0))
}

function calculate_ratio() {
    capital_cost = $("#capital_cost_txt").val(), debt_ratio = $("#debt_ratio_txt").val(), equity_ratio = $("#equity_ratio_txt").val(), loan_payment = debt_ratio / 100 * capital_cost, loan_payment = Math.round(loan_payment), down_payment = capital_cost - loan_payment, $("#down_payment_txt").val(down_payment), $("#loan_payment_txt").val(loan_payment)
}

function calculate_debt_ratio() {
    capital_cost = $("#capital_cost_txt").val(), debt_ratio = $("#debt_ratio_txt").val(), 0 != debt_ratio ? (equity_ratio = 100 - debt_ratio, $("#equity_ratio_txt").val(equity_ratio), loan_payment = debt_ratio / 100 * capital_cost, loan_payment = Math.round(loan_payment), $("#loan_payment_txt").val(loan_payment), down_payment = capital_cost - loan_payment, down_payment = Math.round(down_payment), $("#down_payment_txt").val(down_payment)) : (equity_ratio = 100 - debt_ratio, $("#equity_ratio_txt").val(equity_ratio), $("#loan_payment_txt").val("0"), $("#down_payment_txt").val(capital_cost))
}

function calculate_equity_ratio() {
    capital_cost = $("#capital_cost_txt").val(), equity_ratio = $("#equity_ratio_txt").val(), 0 != equity_ratio ? (debt_ratio = 100 - equity_ratio, $("#debt_ratio_txt").val(debt_ratio), down_payment = equity_ratio / 100 * capital_cost, down_payment = Math.round(down_payment), $("#down_payment_txt").val(down_payment), loan_payment = capital_cost - down_payment, loan_payment = Math.round(loan_payment), $("#loan_payment_txt").val(loan_payment)) : (debt_ratio = 100 - equity_ratio, $("#debt_ratio_txt").val(debt_ratio), $("#down_payment_txt").val("0"), $("#loan_payment_txt").val(capital_cost))
}

function calculate_down_payment() {
    if (capital_cost = $("#capital_cost_txt").val(), down_payment = $("#down_payment_txt").val(), down_payment > capital_cost) return message = "Down payment can not be greater than Input Capital Cost!", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1;
    loan_payment = capital_cost - down_payment, $("#loan_payment_txt").val(loan_payment), equity_ratio = down_payment / capital_cost * 100, equity_ratio = Math.round(equity_ratio), $("#equity_ratio_txt").val(equity_ratio), debt_ratio = 100 - equity_ratio, $("#debt_ratio_txt").val(debt_ratio)
}

function calculate_loan_payment() {
    if (capital_cost = $("#capital_cost_txt").val(), loan_payment = $("#loan_payment_txt").val(), loan_payment > capital_cost) return message = "Loan payment can not be greater than Input Capital Cost!", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1;
    down_payment = capital_cost - loan_payment, $("#down_payment_txt").val(down_payment), debt_ratio = loan_payment / capital_cost * 100, debt_ratio = Math.round(debt_ratio), $("#debt_ratio_txt").val(debt_ratio), equity_ratio = 100 - debt_ratio, $("#equity_ratio_txt").val(equity_ratio)
}

function validateForm() {
    var t = document.getElementById("checkbox_fill_one").checked,
        e = document.getElementById("checkbox_fill_two").checked,
        a = document.getElementById("checkbox_fill_three").checked;
    if (t) {
        if ("" == document.frm.roof_area_txt.value) return message = "Please Enter Total Roof Top Area!", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1;
        var o = document.frm.roof_area_percentage_txt.value;
        if ("" == o) return message = "Please Enter Percentage of Roof Top Area !", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1;
        if ("" != o && !percent(o)) return message = "Invalid Percentage of Roof Top Area", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1
    } else if (e) {
        if ("" == document.frm.capacity_txt.value) return message = "Please Enter Capacity!", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1
    } else {
        if (!a) return message = "Please choose any one of the 3 Plant Size criteria", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1;
        var i = document.frm.budget_txt.value;
        if ("" == i) return message = "Please Enter Budget!", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1;
        if ("" != i && Number(i) < 4e4) return message = "Very Low Budget!", new Messi(message, {
            title: "Error",
            titleClass: "anim error",
            modal: !0,
            buttons: [{
                id: 0,
                label: "Ok",
                val: "X"
            }]
        }), !1
    }
    if ("" == document.frm.state.value) return message = "Please Select State", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1;
    if ("" == document.frm.customer_type.value) return message = "Please Select Customer type", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1;
    var r = document.frm.electricity_txt.value;
    return "" == r ? (message = "Please Enter average Electricity Cost", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1) : "" == r || Number(r) ? "" != r && Number(r) > 100 ? (message = "Invalid average Electricity Cost", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1) : !("" != r && Number(r) > 20) || (message = "Very high average Electricity Cost", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1) : (message = "Invalid average Electricity Cost", new Messi(message, {
        title: "Error",
        titleClass: "anim error",
        modal: !0,
        buttons: [{
            id: 0,
            label: "Ok",
            val: "X"
        }]
    }), !1)
}

function cal_output() {
    var t = document.getElementById("js-display-callback-size").innerHTML;
    document.getElementById("output_size").innerHTML = t;
    var e = t * document.getElementById("js-display-callback-cost").innerHTML;
    document.getElementById("output_without_subsidy").innerHTML = e
}

function percent(t) {
    return !(t > 100)
}

function number(t) {
    var e;
    if (window.event) e = window.event.keyCode;
    else {
        if (!t) return !0;
        e = t.which
    }
    return 8 == e || 0 == e || (String.fromCharCode(e).toLowerCase(), e > 47 && e < 58)
}
$(document).ready(function() {
    baseurl = $("#url").val(), $(".txt_option").hide(), $(".chk-grp").attr("disabled", !0), $(".chk-grp").hide(), $("#capital_cost_txt").prop("disabled", !0), $("#subsidy_txt").prop("disabled", !0), $("#emi_data").hide()
});