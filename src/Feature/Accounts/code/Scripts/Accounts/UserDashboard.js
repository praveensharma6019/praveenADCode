$(document).ready(function () {
    var ComponentLoaderHtml = `<div class="section-loader">
                                    <div class="loader-box">
                                         <svg viewBox="0 0 70 100" class="loader-white large">
                                             <circle cx="6" cy="50" r="6">
                                                 <animateTransform attributeName="transform" dur="1s" type="translate" values="0 15 ; 0 -15; 0 15" repeatCount="indefinite" begin="0.1"></animateTransform>
                                             </circle>
                                             <circle stroke="none" cx="30" cy="50" r="6">
                                             <animateTransform attributeName="transform" dur="1s" type="translate" values="0 10 ; 0 -10; 0 10" repeatCount="indefinite" begin="0.2"></animateTransform>
                                             </circle>
                                             <circle stroke="none" cx="54" cy="50" r="6">
                                             <animateTransform attributeName="transform" dur="1s" type="translate" values="0 5 ; 0 -5; 0 5" repeatCount="indefinite" begin="0.3"></animateTransform>
                                             </circle>
                                         </svg>
                                    </div>
                                </div>`;

    var downwardArrow = `<svg width="24" height="32" viewBox="0 0 24 32" xmlns="http://www.w3.org/2000/svg">
        <path d="m23.718 20.812-.003.002-10.998 10.908a.997.997 0 0 1-.716.29.997.997 0 0 1-.718-.29L.285 20.814a1 1 0 0 1-.003-1.417l2.93-2.932v-.002l.094-.083a.999.999 0 0 1 1.32.085l4.301 4.303V1A1 1 0 0 1 9.81.007L9.927 0h4.146a1 1 0 0 1 1 1l.001 19.769 4.3-4.304a.999.999 0 0 1 1.32-.085l.094.083v.002l2.93 2.932a1 1 0 0 1 0 1.415z" fill="#18AA26" fill-rule="evenodd" />
    </svg>`;

    var upwardsArrow = `<svg width="24" height="32" viewBox="0 0 24 32" xmlns="http://www.w3.org/2000/svg">
    <path d="m23.718 11.188-.003-.002L12.717.278A.997.997 0 0 0 12-.012a.997.997 0 0 0-.718.29L.285 11.186a1 1 0 0 0-.003 1.417l2.93 2.932v.002l.094.083a.999.999 0 0 0 1.32-.085l4.301-4.303V31a1 1 0 0 0 .883.993l.117.007h4.146a1 1 0 0 0 1-1l.001-19.769 4.3 4.304c.36.361.928.39 1.32.085l.094-.083v-.002l2.93-2.932a1 1 0 0 0 0-1.415z" fill="#d30a0a" fill-rule="evenodd"/>
  </svg>`;


    var complaintDashboardUrl = $('#ComplaintDashboardUrl').val();
    var powerOutageUrl = $('#PowerOutageUrl').val();
    var greenPowerUrl = $('#GreenPowerUrl').val();
    var submitMeterReadingUrl = $('#SubmitMeterReadingUrl').val();
    //PaymentTrends
    var payObj = [];
    var consumptionDataObj = {};

    //LoadData_DownloadPayBillRevamp
    $.ajax({
        url: apiSettings + "/AccountsRevamp/LoadData_DownloadPayBillRevamp",
        dataType: "json",
        success: function (data) {
            var downloadMonths = ``;

            if (data.data != undefined && data.data != '' && data.data != null && data.data.InvoiceLines != '') {
                $('#downloadLatestBillDetails').html(`<a  href="javascript:void(0)" onclick="UDDownloadFile('${data.data.InvoiceLines[0].BillMonth}');">Download Bill</a>`);
            } else {
                $('#downloadLatestBillDetails').html('<a  href="javascript:void(0)">Download Bill</a>');
            }

            if (data.data != undefined && data.data != '' && data.data != null && data.data.InvoiceLines != '') {
                for (var i = 0; i < data.data.InvoiceLines.length; i++) {
                    downloadMonths += `<li>
                                            <a onclick="UDDownloadFile('${data.data.InvoiceLines[i].BillMonth}');" href="javascript:void(0)">${data.data.InvoiceLines[i].BillMonth}</a>
                                          </li>`;
                }

                $('#downloadMonthsUl').append(downloadMonths);
            }
        },
        error: function (jqXHR, exception) {
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }

        }
    });

    //UserDashboardElectricityCharges
    $.ajax({
        url: apiSettings + "/AEMLUserDashboard/UserDashboardElectricityCharges",
        dataType: "json",
        beforeSend: function () {
            $('#ElectricityChargesLoaderDiv').append(ComponentLoaderHtml);
        },
        success: function (data) {
            //debugger;
            //console.log(data);
            if ($('#ElectricityChargesLoaderDiv').find('.section-loader').length > 0) {
                $('#ElectricityChargesLoaderDiv').find('.section-loader').remove();
            }
            var zeroChargeString = 'Rs. 0.00';
            if (data.data != undefined && data.data != '') {
                if (data.data.EnergyCharge != '') {
                    $('.EnergyCharge').html(AddRupeeSymbol(data.data.EnergyCharge));
                } else {
                    $('.EnergyCharge').html(zeroChargeString);
                }

                if (data.data.FixedDemandCharge != '') {
                    $('.FixedDemandCharge').html(AddRupeeSymbol(data.data.FixedDemandCharge));
                } else {
                    $('.FixedDemandCharge').html(zeroChargeString);
                }

                if (data.data.FueladjustmentCharges != '') {
                    $('.FueladjustmentCharges').html(AddRupeeSymbol(data.data.FueladjustmentCharges));
                } else {
                    $('.FueladjustmentCharges').html(zeroChargeString);
                }

                if (data.data.GovernmentElectricityDuty != '') {
                    $('.GovernmentElectricityDuty').html(AddRupeeSymbol(data.data.GovernmentElectricityDuty));
                } else {
                    $('.GovernmentElectricityDuty').html(zeroChargeString);
                }

                if (data.data.Govtdutyrate != '') {
                    $('.Govtdutyrate').html(data.data.Govtdutyrate + "%");
                } else {
                    $('.Govtdutyrate').html("0.00%");
                }

                if (data.data.MahGovtTaxOnSaleRate != '') {
                    $('.MahGovtTaxOnSaleRate').html(data.data.MahGovtTaxOnSaleRate + ' p/unit');
                } else {
                    $('.MahGovtTaxOnSaleRate').html('0 p/unit');
                }

                if (data.data.RegulatoryAssetCharge != '') {
                    $('.RegulatoryAssetCharge').html(AddRupeeSymbol(data.data.RegulatoryAssetCharge));
                } else {
                    $('.RegulatoryAssetCharge').html(zeroChargeString);
                }

                if (data.data.CurrentMonthBillAmount != '') {
                    $('.CurrentMonthBillAmount').html(AddRupeeSymbol(data.data.CurrentMonthBillAmount));
                } else {
                    $('.CurrentMonthBillAmount').html(zeroChargeString);
                }

                if (data.data.TaxOnSale != '') {
                    $('.TaxOnSale').html(AddRupeeSymbol(data.data.TaxOnSale));
                } else {
                    $('.TaxOnSale').html(zeroChargeString);
                }

                if (data.data.Wheelingcharge != '') {
                    $('.Wheelingcharge').html(AddRupeeSymbol(data.data.Wheelingcharge));
                } else {
                    $('.Wheelingcharge').html(zeroChargeString);
                }

                if (data.data.CurrentMonthBillAmount != '') {
                    $('.CurrentMonthBillAmount').html(AddRupeeSymbol(data.data.CurrentMonthBillAmount));
                } else {
                    $('.CurrentMonthBillAmount').html(zeroChargeString);
                }

                if (data.data.BillPeriodFrom != '' && data.data.BillPeriodTo != '') {
                    $('.CurrentBillMonthDate').html(GetFullDateFormat(data.data.BillPeriodFrom) + ' - ' + GetFullDateFormat(data.data.BillPeriodTo));
                    $('.CurrentBillMonthDateChart').html(GetTwoDigitYearDateFormat(data.data.BillPeriodFrom) + ' - ' + GetTwoDigitYearDateFormat(data.data.BillPeriodTo));
                } else {
                    $('.CurrentBillMonthDate').html('');
                    $('.CurrentBillMonthDateChart').html('');
                }

                var govtCharges = parseFloat(data.data.GovernmentElectricityDuty) + parseFloat(data.data.MahGovtTaxOnSaleRate);
                govtCharges = govtCharges != undefined ? govtCharges : 0;

                var AEMLCharges = parseFloat(data.data.FixedDemandCharge) + parseFloat(data.data.EnergyCharge) + parseFloat(data.data.Wheelingcharge) + parseFloat(data.data.FueladjustmentCharges) + parseFloat(data.data.RegulatoryAssetCharge);
                AEMLCharges = AEMLCharges != undefined ? AEMLCharges : 0;
                //Donut Chart
                var xValues = ["Govt.Taxes", "AEMLCharges"];
                var yValues = [govtCharges, AEMLCharges];
                var barColors = [
                    "#ffb062",
                    "#0c78ef"
                ];

                var perUnitCharge = '0';
                var unitConsumed = parseInt(data.data.UnitConsumed);

                if (unitConsumed == 0 || unitConsumed <= 100) {
                    perUnitCharge = '3.05';
                } else if (unitConsumed >= 101 || unitConsumed <= 300) {
                    perUnitCharge = '5.00';
                } else if (unitConsumed >= 301 || unitConsumed <= 500) {
                    perUnitCharge = '6.65';
                } else if (unitConsumed > 500) {
                    perUnitCharge = '7.80';
                }

                var text = `<div class="donut-inner inside-circle">
                            <h5>₹ ${perUnitCharge}</h5>
                            <label>per unit</label>
                            </div>`;
                $(text).insertAfter($('#electricityChargesChart'));

                var eleChargesChart = new Chart("electricityChargesChart", {
                    type: "doughnut",
                    data: {
                        labels: xValues,
                        datasets: [{
                            backgroundColor: barColors,
                            data: yValues
                        }]
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        title: {
                            display: false,
                            text: "Electricity Charges"
                        },
                        cutoutPercentage: 80,
                        aspectRatio: 1,
                        tooltips: {
                            enabled: false
                        }
                    }
                });
            }

        },
        error: function (jqXHR, exception) {
            if ($('#ElectricityChargesLoaderDiv').find('.section-loader').length > 0) {
                $('#ElectricityChargesLoaderDiv').find('.section-loader').remove();
            }

            $('#ElectricityChargesLoaderDiv').html(`<div class="no-notification-box" >
                            <div class="no-notification-content">
                                <h3>Error Occured! Please try again after sometime...</h3>
                            </div>
                        </div>`);
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }

        }
    });

    //consumptionTrends
    $.ajax({
        url: apiSettings + "/AccountsRevamp/LoadData_ConsumptionHistoryRevamp",
        dataType: "json",
        beforeSend: function () {
            $('#consumptionTrendLoaderDiv').append(ComponentLoaderHtml);
        },
        success: function (data) {
            //console.log(data);
            if ($('#consumptionTrendLoaderDiv').find('.section-loader').length > 0) {
                $('#consumptionTrendLoaderDiv').find('.section-loader').remove();
            }
            if (data.data != undefined && data.data != '' && data.data != null && data.data.MeterConsumptions != '') {

                if (data.data.MeterConsumptions[0].ConsumptionRecords[0].Status != undefined) {
                    $('#UDMeterStatus').html(UDMeterStatusDetails(data.data.MeterConsumptions[0].ConsumptionRecords[0].Status));
                }
                //$('#consumptionTrendsYears div.tab-content').attr('id', )
                $('#consumptionTrendsYears').html('');


                var curdate = new Date(),
                    currentyear = curdate.getFullYear();

                var yearArray = [currentyear, (currentyear - 1), (currentyear - 2), (currentyear - 3)];

                //for (var i = 0; i < data.data.MeterConsumptions[0].ConsumptionRecords.length; i++) {
                //    var year = new Date(data.data.MeterConsumptions[0].ConsumptionRecords[i].ConsumptionDate).getFullYear();
                //    if ($.inArray(year, yearArray) == -1) {
                //        yearArray.push(year);
                //    }
                //}
                //console.log(yearArray);
                var cTrendYearsCardTabs = ``;
                var cTrendYearsCardContent = ``;
                var doubleYearArray = [];
                //debugger;
                if (yearArray.length > 1) {

                    for (var y = 0; y < yearArray.length; y++) {
                        if (yearArray[y + 1] != undefined) {
                            var yearString = yearArray[y] + '-' + yearArray[y + 1];
                            if ($.inArray(yearString, doubleYearArray) == -1) {
                                doubleYearArray.push(yearString);
                            }
                        }
                    }

                    for (var d = 0; d < doubleYearArray.length; d++) {

                        var dataLayer = `dataLayer.push({
                                        'event': 'user_dashboard_consumption_trend',
                                        'eventCategory': 'AEML User Dashboard',
                                        'eventAction': 'user_dashboard_consumption_trend',
                                        'eventLabel': '${doubleYearArray[d]}',
                                        'business_user_id': '${$('#BusinessUserId').val()}',
                                        'ca_number': '${$('#GACANumber').val()}',
                                        'login_status': '${$('#login_status').val()}',
                                        'year_month': '${doubleYearArray[d]}',
                                        });`

                        cTrendYearsCardContent += `<div id="${doubleYearArray[d]}" class="tab-content">
                                                        <div class="consumption-trend-head">
                                                           <ul>
                                                               <li class="active">${doubleYearArray[d].split('-')[0]}</li>
                                                               <li>${doubleYearArray[d].split('-')[1]}</li>
                                                           </ul>
                                                        </div>
                                                        <div class="trend-box"> 
                                                            <canvas id="${doubleYearArray[d].replace('-', '+')}"></canvas>
                                                        </div>
                                                    </div>`;

                        if (d == 0) {
                            cTrendYearsCardTabs += `<li class="tab">
                                                                <a href="#${doubleYearArray[d]}" onclick="${dataLayer}" class="active">
                                                                    <h4>${doubleYearArray[d]}</h4>
                                                                </a>
                                                            </li>`;
                        } else {
                            cTrendYearsCardTabs += `<li class="tab">
                                                                <a href="#${doubleYearArray[d]}" onclick="${dataLayer}">
                                                                    <h4>${doubleYearArray[d]}</h4>
                                                                </a>
                                                            </li>`;
                        }
                    }
                    $('#consumptionTrendsYears').append(`<ul class="tabs consumptiontabs">${cTrendYearsCardTabs}</ul>`);
                    $('#consumptionTrendsYears').append(cTrendYearsCardContent);

                    var xValues = ["0", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

                    for (var d = 0; d < doubleYearArray.length; d++) {

                        var datasetOne = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
                        var datasetTwo = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

                        var frontYear = doubleYearArray[d].substr(0, doubleYearArray[d].indexOf('-'));
                        var backYear = doubleYearArray[d].substr(doubleYearArray[d].indexOf("-") + 1);

                        //for (var i = 0; i < data.data.MeterConsumptions[0].ConsumptionRecords.length; i++) {
                        //    var year = new Date(data.data.MeterConsumptions[0].ConsumptionRecords[i].ConsumptionDate).getFullYear();
                        //    var month = new Date(data.data.MeterConsumptions[0].ConsumptionRecords[i].ConsumptionDate).getMonth();
                        //    var unitConsumed = data.data.MeterConsumptions[0].ConsumptionRecords[i].UnitsConsumed;


                        //    consumptionDataObj[xValues[month + 1] + '-' + year] = data.data.MeterConsumptions[0].ConsumptionRecords[i].ConsumptionDate;

                        //    if (year == frontYear) {
                        //        datasetOne[month + 1] = unitConsumed.substr(0, unitConsumed.indexOf('.')).replace(',', '');
                        //    }
                        //    if (year == backYear) {
                        //        datasetTwo[month + 1] = unitConsumed.substr(0, unitConsumed.indexOf('.')).replace(',', '');
                        //    }
                        //}

                        for (var i = 0; i < data.data.MeterConsumptions.length; i++) {
                            for (var j = 0; j < data.data.MeterConsumptions[i].ConsumptionRecords.length; j++) {

                                var year = new Date(data.data.MeterConsumptions[i].ConsumptionRecords[j].ConsumptionDate).getFullYear();
                                var month = new Date(data.data.MeterConsumptions[i].ConsumptionRecords[j].ConsumptionDate).getMonth();
                                var unitConsumed = data.data.MeterConsumptions[i].ConsumptionRecords[j].UnitsConsumed;

                                unitConsumed = unitConsumed.substr(0, unitConsumed.indexOf('.')).replace(',', '');

                                unitConsumed = unitConsumed != '' ? unitConsumed : 0;

                                if (consumptionDataObj[xValues[month + 1] + '-' + year] == undefined)
                                    consumptionDataObj[xValues[month + 1] + '-' + year] = data.data.MeterConsumptions[i].ConsumptionRecords[j].ConsumptionDate;

                                if (year == frontYear) {
                                    datasetOne[month + 1] = parseFloat(datasetOne[month + 1]) + parseFloat(unitConsumed);
                                }
                                if (year == backYear) {
                                    datasetTwo[month + 1] = parseFloat(datasetTwo[month + 1]) + parseFloat(unitConsumed);
                                }
                            }
                        }

                        //console.log(datasetOne);
                        //console.log(datasetTwo);
                        new Chart(doubleYearArray[d].replace('-', '+'), {
                            type: "line",
                            data: {
                                labels: xValues,
                                datasets: [{
                                    label: frontYear, //2021
                                    data: datasetOne,
                                    borderColor: "#0d67ca",
                                    fill: false,
                                    tension: 0
                                }, {
                                    label: backYear, //2020
                                    data: datasetTwo,
                                    borderColor: "#e1e0e0",
                                    fill: false,
                                    tension: 0
                                }]
                            },
                            options: {
                                responsive: true,
                                maintainAspectRatio: false,
                                legend: {
                                    display: false
                                },
                                tooltips: {
                                    displayColors: false,
                                    callbacks: {
                                        title: function (t, d) {
                                            const my = d.labels[t[0].index] + '-' + d.datasets[t[0].datasetIndex].label;
                                            return `Meter reading on : ${GetFullDateFormatConsumptionChart(consumptionDataObj[my])}`;
                                        },
                                        label: function (t, d) {
                                            const label = d.datasets[t.datasetIndex].label;
                                            const value = d.datasets[t.datasetIndex].data[t.index];
                                            return `Consumption: ${value} kwh`;
                                        }
                                    }
                                },
                                scales: {
                                    xAxes: [{
                                        gridLines: {
                                            display: false
                                        }
                                    }],
                                    yAxes: [{
                                        scaleLabel: {
                                            display: false,
                                            labelString: 'Electricity Consumption (kwh)'
                                        },
                                        ticks: {
                                            beginAtZero: true, // minimum value will be 0.
                                            //stepSize: 100 
                                            callback: function (value, index, values) {
                                                if (value.toString().indexOf('.') != - 1) {
                                                    return value.toFixed(2) + ' kwh';
                                                } else {
                                                    return value + ' kwh';
                                                }

                                            },
                                            backgroundColor: '#227799'
                                        },
                                    }]
                                }
                            }
                        });
                    }
                    M.Tabs.init(document.querySelectorAll(".consumptiontabs"), { swipeable: false });
                }

            } else {
                $('#consumptionTrendsYears').html(`<div class="no-notification-box" >
                            <div class="no-notification-content">
                                <h3>No Consumption History Found!</h3>
                            </div>
                        </div>`);
            }
        },
        error: function (jqXHR, exception) {
            if ($('#consumptionTrendLoaderDiv').find('.section-loader').length > 0) {
                $('#consumptionTrendLoaderDiv').find('.section-loader').remove();
            }
            $('#consumptionTrendsYears').html(`<div class="no-notification-box" >
                            <div class="no-notification-content">
                                <h3>Error Occured! Please try again after sometime...</h3>
                            </div>
                        </div>`);
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            console.log(msg);
        }
    });


    //UserDashboardPowerCutNotification
    UserDashboardPowerCutNotification();
    window.UserDashboardPowerCutNotification = UserDashboardPowerCutNotification
    function UserDashboardPowerCutNotification() {
        $.ajax({
            url: apiSettings + "/AEMLUserDashboard/UserDashboardPowerCutNotification",
            dataType: "json",
            beforeSend: function () {
                if ($('#NotificationComponentLoaderDiv').find('.section-loader').length == 0) {
                    $('#NotificationComponentLoaderDiv').append(ComponentLoaderHtml);
                }

            },
            success: function (OutageData) {
                //console.log(OutageData);
                var outageInfo = ``;
                var dataLayer = `dataLayer.push({
                'event': 'user_dashboard_notification',
                'eventCategory': 'AEML User Dashboard',
                'eventAction': 'user_dashboard_notification',
                'eventLabel': 'Power cut in your area',
                'business_user_id': '${$('#BusinessUserId').val()}',
                'ca_number': '${$('#GACANumber').val()}',
                'login_status': '${$('#login_status').val()}'
            });`;
                if (OutageData.data.CurrentOutageDetails != undefined && OutageData.data.CurrentOutageDetails != '' && OutageData.data.CurrentOutageDetails != []) {
                    outageInfo = `We regret that you may be facing a power outage in your area. Restoration time is mentioned below.`;
                    $('#aeml-notifications').append(`<li>
                            <figure>
                                <img src="/electricity_assets/icons/power-cut.png" alt="">
                            </figure>
                            <div class="list-content">
                                <div class="list-content-left">
                                    <h3>Power Cut In Your Area</h3>
                                    <h4>
                                        ${outageInfo}
                                    </h4>
                                    <p>Estimated Time: <span> ${tConvert(OutageData.data.CurrentOutageDetails[0].StartTime)} - ${tConvert(OutageData.data.CurrentOutageDetails[0].EndTime)}</span> </p>
                                </div>
                                <div class="list-content-right">
                                    <a onclick="${dataLayer}" href="${powerOutageUrl}">Need Help?</a>
                                </div>
                            </div>
                        </li>`);

                    $('#NoNotificationsEl').hide();
                    $('#NotificationsEl').show();

                }
                if (OutageData.data.FutureOutageDetails != undefined && OutageData.data.FutureOutageDetails != '' && OutageData.data.FutureOutageDetails != []) {

                    if (OutageData.data.FutureOutageDetails[0].ZFLAG = 'f') {
                        outageInfo = `For carrying out maintenance /upgradation activity there may be power outage in your area. Any change in the schedule shall be intimated in advance on your registered mobile number.`;
                    } else if (OutageData.data.FutureOutageDetails[0].ZFLAG = 'r') {
                        outageInfo = `Due to technical reasons, we have rescheduled maintenance activity in your area, Revised schedule is mentioned below for your reference.`;
                    }
                    $('#aeml-notifications').append(`<li>
                            <figure>
                                <img src="/electricity_assets/icons/power-cut.png" alt="">
                            </figure>
                            <div class="list-content">
                                <div class="list-content-left">
                                    <h3>Power Cut In Your Area</h3>
                                    <h4>
                                        ${outageInfo}
                                    </h4>
                                    <p>Date: <span> ${OutageData.data.FutureOutageDetails[0].Date} </span> </p>
                                    <p>Estimated Time: <span> ${tConvert(OutageData.data.FutureOutageDetails[0].StartTime)} - ${tConvert(OutageData.data.FutureOutageDetails[0].EndTime)} </span> </p>
                                </div>
                                <div class="list-content-right">
                                    <a onclick="${dataLayer}" href="${powerOutageUrl}">Need Help?</a>
                                </div>
                            </div>
                        </li>`);

                    $('#NoNotificationsEl').hide();
                    $('#NotificationsEl').show();
                }

                if ($('#NotificationComponentLoaderDiv').find('.section-loader').length > 0) {
                    $('#NotificationComponentLoaderDiv').find('.section-loader').remove();
                }
                UserDashboardSubmitMeterReadingNotification();

            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }


                UserDashboardSubmitMeterReadingNotification();
            }
        });
    }


    //UserDashboardSubmitMeterReadingNotification
    //UserDashboardSubmitMeterReadingNotification();
    window.UserDashboardSubmitMeterReadingNotification = UserDashboardSubmitMeterReadingNotification
    function UserDashboardSubmitMeterReadingNotification() {
        $.ajax({
            url: apiSettings + "/AEMLUserDashboard/UserDashboardSubmitMeterReadingNotification",
            dataType: "json",
            beforeSend: function () {
                if ($('#NotificationComponentLoaderDiv').find('.section-loader').length == 0) {
                    $('#NotificationComponentLoaderDiv').append(ComponentLoaderHtml);
                }
            },
            success: function (data) {
                //console.log(data);
                var dataLayer = `dataLayer.push({
                'event': 'user_dashboard_notification',
                'eventCategory': 'AEML User Dashboard',
                'eventAction': 'user_dashboard_notification',
                'eventLabel': 'Submit Meter Reading',
                'business_user_id': '${$('#BusinessUserId').val()}',
                'ca_number': '${$('#GACANumber').val()}',
                'login_status': '${$('#login_status').val()}'
            });`;
                if (data.data != undefined && data.data != null && data.data.MeterList != undefined && data.data.MeterList != null) {

                    $('#aeml-notifications').append(`<li>
                                        <figure>
                                            <img src="/electricity_assets/icons/meter-reading.png" alt="">
                                        </figure>
                                        <div class="list-content">
                                            <div class="list-content-left">
                                                <h3>Submit Meter Reading</h3>
                                                <h4>Submit your meter reading if you do not want to receive estimated bills
                                                </h4>
                                                <p>Submit Till: <span> ${GetFullDateFormatConsumptionChart(data.data.MeterList[0].SMRD)} </span> </p>
                                            </div>
                                            <div class="list-content-right">
                                                <a onclick="${dataLayer}" href="${submitMeterReadingUrl}">Submit Reading</a>
                                            </div>
                                        </div>
                                    </li>`);

                    $('#NoNotificationsEl').hide();
                    $('#NotificationsEl').show();
                }
                //if ($('#NotificationComponentLoaderDiv').find('.section-loader').length > 0) {
                //    $('#NotificationComponentLoaderDiv').find('.section-loader').remove();
                //}

                UserDashboardCGRFComplaintsNotifications();
            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }


                UserDashboardCGRFComplaintsNotifications();
            }
        });
    }

    //UserDashboardCGRFComplaintsNotifications
    //UserDashboardCGRFComplaintsNotifications();
    window.UserDashboardCGRFComplaintsNotifications = UserDashboardCGRFComplaintsNotifications
    function UserDashboardCGRFComplaintsNotifications() {
        $.ajax({
            url: apiSettings + "/AEMLUserDashboard/UserDashboardCGRFComplaintsNotifications",
            dataType: "json",
            beforeSend: function () {
                if ($('#NotificationComponentLoaderDiv').find('.section-loader').length == 0) {
                    $('#NotificationComponentLoaderDiv').append(ComponentLoaderHtml);
                }
            },
            success: function (data) {
                //console.log('CGRF Complaint:>');
                //console.log(data);
                var complaintNotification = ``;
                var complaintDesc = ``;
                var complaintstatus = ``;
                if (data.data.ComplaintList.length > 0) {
                    for (var i = 0; i < data.data.ComplaintList.length; i++) {

                        if (data.data.ComplaintList[i].ComplaintStatusDescription != null) {
                            complaintDesc = `<h4>${data.data.ComplaintList[i].ComplaintStatusDescription}</h4>`
                        }

                        //var dataLayer = `dataLayer.push({
                        //            'event': 'user_dashboard_notification',
                        //            'eventCategory': 'AEML User Dashboard',
                        //            'eventAction': 'user_dashboard_notification',
                        //            'eventLabel': 'CGRF Complaint Notification',
                        //            'index': ''
                        //            'complaint_status': '${data.data.ComplaintList[i].ComplaintStatusName}',
                        //            'business_user_id': '${$('#BusinessUserId').val()}',
                        //            'ca_number': '${$('#GACANumber').val()}',
                        //            'login_status': '${$('#login_status').val()}'
                        //        });`;

                        complaintNotification += `<li>
                            <figure>
                                <img src="/electricity_assets/icons/complaint.png" alt="">
                            </figure>
                            <div class="list-content">
                                <div class="list-content-left">
                                    <h3>${data.data.ComplaintList[i].ComplaintCategory}</h3>
                                    ${complaintDesc}
                                    <p>
                                        Complaint Number: <span>${data.data.ComplaintList[i].ComplaintRegistrationNumber + ' (CGRF)'} </span><br />
                                        Complaint Status: <span>${data.data.ComplaintList[i].ComplaintStatusName}</span>
                                    </p>
                                </div>
                                <div class="list-content-right">
                                    <a href="${complaintDashboardUrl}" onclick="handleNotificationViewDetails('TrackCGRFComplaintsRevamp', '${data.data.ComplaintList[i].ComplaintRegistrationNumber}', '${data.data.ComplaintList[i].ComplaintStatusName}');">View Details</a>
                                </div>
                            </div>
                        </li>`;
                    }
                    //UDcreateCookie("ActiveComplaintLi", "TrackCGRFComplaintsRevamp", "");
                    $('#aeml-notifications').append(complaintNotification);

                    $('#NoNotificationsEl').hide();
                    $('#NotificationsEl').show();
                }
                //if ($('#NotificationComponentLoaderDiv').find('.section-loader').length > 0) {
                //    $('#NotificationComponentLoaderDiv').find('.section-loader').remove();
                //}

                UserDashboardComplaintsNotification();
            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }



                UserDashboardComplaintsNotification();
            }
        });
    }

    //UserDashboardComplaintsNotification
    //UserDashboardComplaintsNotification();
    window.UserDashboardComplaintsNotification = UserDashboardComplaintsNotification
    function UserDashboardComplaintsNotification() {
        $.ajax({
            url: apiSettings + "/AEMLUserDashboard/UserDashboardComplaintsNotification",
            dataType: "json",
            beforeSend: function () {
                if ($('#NotificationComponentLoaderDiv').find('.section-loader').length == 0) {
                    $('#NotificationComponentLoaderDiv').append(ComponentLoaderHtml);
                }
            },
            success: function (data) {
                //console.log('Normal Complaint:>');
                //console.log(data);

                var complaintNotification = ``;
                var complaintDesc = ``;
                var complaintstatus = ``;
                if (data.data.ComplaintList.length > 0) {
                    for (var i = 0; i < data.data.ComplaintList.length; i++) {

                        if (data.data.ComplaintList[i].ComplaintDescription != null) {
                            complaintDesc = `<h4>${data.data.ComplaintList[i].ComplaintDescription}</h4>`
                        }

                        if (data.data.ComplaintList[i].ComplaintStatusName == "INPROCESS") {
                            complaintstatus = `In progress`;
                        } else {
                            complaintstatus = data.data.ComplaintList[i].ComplaintStatusName;
                        }

                        //var dataLayer = `dataLayer.push({
                        //            'event': 'user_dashboard_notification',
                        //            'eventCategory': 'AEML User Dashboard',
                        //            'eventAction': 'user_dashboard_notification',
                        //            'eventLabel': 'Complaint Notification',
                        //            'index': '${data.data.ComplaintList[i].ComplaintNumber}'
                        //            'complaint_status': '${complaintstatus}',
                        //            'business_user_id': '${$('#BusinessUserId').val()}',
                        //            'ca_number': '${$('#GACANumber').val()}',
                        //            'login_status': '${$('#login_status').val()}'
                        //        });`;

                        complaintNotification += `<li>
                            <figure>
                                <img src="/electricity_assets/icons/complaint.png" alt="">
                            </figure>
                            <div class="list-content">
                                <div class="list-content-left">
                                    <h3>${data.data.ComplaintList[i].ComplaintCategory}</h3>
                                    ${complaintDesc}
                                    <p>
                                        Complaint Number: <span>${data.data.ComplaintList[i].ComplaintNumber} </span><br />
                                        Complaint Status: <span>${complaintstatus}</span>
                                    </p>
                                </div>
                                <div class="list-content-right">
                                    <a href="${complaintDashboardUrl}" onclick="handleNotificationViewDetails('TrackComplaintRevamp', '${data.data.ComplaintList[i].ComplaintNumber}', '${complaintstatus}')" >View Details</a>
                                </div>
                            </div>
                        </li>`;

                    }
                    //UDcreateCookie("ActiveComplaintLi", "TrackComplaintRevamp", "");
                    $('#aeml-notifications').append(complaintNotification);

                    $('#NoNotificationsEl').hide();
                    $('#NotificationsEl').show();

                }
                //if ($('#NotificationComponentLoaderDiv').find('.section-loader').length > 0) {
                //    $('#NotificationComponentLoaderDiv').find('.section-loader').remove();
                //}

                UserDashboardGreenPowerNotification();
            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }



                UserDashboardGreenPowerNotification();
            }
        });
    }

    //UserDashboardGreenPowerNotification
    //UserDashboardGreenPowerNotification();
    window.UserDashboardGreenPowerNotification = UserDashboardGreenPowerNotification
    function UserDashboardGreenPowerNotification() {

        var dataLayer = `dataLayer.push({
                'event': 'user_dashboard_notification',
                'eventCategory': 'AEML User Dashboard',
                'eventAction': 'user_dashboard_notification',
                'eventLabel': 'Green Power',
                'business_user_id': '${$('#BusinessUserId').val()}',
                'ca_number': '${$('#GACANumber').val()}',
                'login_status': '${$('#login_status').val()}'
            });`;

        $('#aeml-notifications').append(`<li>
                            <figure>
                                <img src="/electricity_assets/icons/green-power-notification.png" alt="">
                            </figure>
                            <div class="list-content">
                                <div class="list-content-left">
                                    <h3>For a Greener Earth</h3>
                                    <h4>
                                        Switch to Green Energy with Adani Electricity’s Green Tariff
                                    </h4>
                                </div>
                                <div class="list-content-right">
                                    <a onclick="${dataLayer}" href="${greenPowerUrl}">View Details</a>
                                </div>
                            </div>
                        </li>`);

        $('#NoNotificationsEl').hide();
        $('#NotificationsEl').show();


        if ($('#NotificationComponentLoaderDiv').find('.section-loader').length > 0) {
            $('#NotificationComponentLoaderDiv').find('.section-loader').remove();
        }
    }

    //PaymentTrendsCharts
    $.ajax({
        url: apiSettings + "/AccountsRevamp/LoadData_PaymentHistoryRevamp",
        dataType: "json",
        beforeSend: function () {
            $('#paymentTrendLoaderDiv').append(ComponentLoaderHtml);
        },
        success: function (data) {
            if ($('#paymentTrendLoaderDiv').find('.section-loader').length > 0) {
                $('#paymentTrendLoaderDiv').find('.section-loader').remove();
            }
            var monthArray = ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];

            //console.log(data);

            if (data.data != undefined && data.data != '' && data.data != null) {


                var curdate = new Date(),
                    currentMonth = curdate.getMonth(),
                    currentyear = (curdate.getFullYear() % 100);

                var pyearArray = [currentyear];

                //for (var i = 0; i < data.data.length; i++) {
                //    var year = (new Date(data.data[i].PaymentDate).getFullYear() % 100);
                //    if ($.inArray(year, pyearArray) == -1) {
                //        pyearArray.push(year);
                //    }
                //}


                if (pyearArray != []) {

                    for (var y = 0; y < pyearArray.length; y++) {
                        if (pyearArray[y] == currentyear) {
                            for (var m = currentMonth; m >= 0; m--) {
                                if (pyearArray[y] == currentyear && monthArray[m] == currentMonth) {
                                    //debugger;
                                    payObj.push([monthArray[m] + '-' + pyearArray[y], '0.00']);
                                    break;

                                } else {
                                    payObj.push([monthArray[m] + '-' + pyearArray[y], '0.00']);
                                }

                            }
                        } else {
                            for (var m = 11; m >= 0; m--) {
                                if (pyearArray[y] == currentyear && monthArray[m] == currentMonth) {
                                    //debugger;
                                    payObj.push([monthArray[m] + '-' + pyearArray[y], '0.00']);
                                    break;

                                } else {
                                    payObj.push([monthArray[m] + '-' + pyearArray[y], '0.00']);
                                }

                            }
                        }

                    }

                    for (var i = 0; i < data.data.length; i++) {
                        var year = (new Date(data.data[i].PaymentDate).getFullYear() % 100);
                        var month = monthArray[new Date(data.data[i].PaymentDate).getMonth()];
                        if (payObj.some(item => item.some(p => item.includes(month + '-' + year)))) {
                            var index = payObj.findIndex((obj => obj[0] == month + '-' + year));
                            if (index != undefined) {
                                var Amount = parseFloat(payObj[index][1]) + parseFloat(data.data[i].Amount.replace(',', ''));
                                payObj[index][1] = Amount.toFixed(2);
                            } else {
                                payObj.push([month + '-' + year, parseFloat(data.data[i].Amount.replace(',', '')).toFixed(2)]);
                            }

                        } else {
                            payObj.push([month + '-' + year, parseFloat(data.data[i].Amount.replace(',', '')).toFixed(2)]);
                        }

                    }

                    CreatedBarChart(2);
                } else {
                    $('#PaymentTrendsCharts').html(`<div class="no-notification-box" >
                            <div class="no-notification-content">
                                <h3>No Payment History Found!</h3>
                            </div>
                        </div>`);
                }
                //console.log(payObj);
            } else {
                $('#PaymentTrendsCharts').html(`<div class="no-notification-box" >
                            <div class="no-notification-content">
                                <h3>No Payment History Found!</h3>
                            </div>
                        </div>`);
            }
        },
        error: function (jqXHR, exception) {
            if ($('#paymentTrendLoaderDiv').find('.section-loader').length > 0) {
                $('#paymentTrendLoaderDiv').find('.section-loader').remove();
            }

            $('#PaymentTrendsCharts').html(`<div class="no-notification-box" >
                            <div class="no-notification-content">
                                <h3>Error Occured! Please try again after sometime...</h3>
                            </div>
                        </div>`);
            var msg = '';
            if (jqXHR.status === 0) {
                msg = 'Not connect.\n Verify Network.';
            } else if (jqXHR.status == 404) {
                msg = 'Requested page not found. [404]';
            } else if (jqXHR.status == 500) {
                msg = 'Internal Server Error [500].';
            } else if (exception === 'parsererror') {
                msg = 'Requested JSON parse failed.';
            } else if (exception === 'timeout') {
                msg = 'Time out error.';
            } else if (exception === 'abort') {
                msg = 'Ajax request aborted.';
            } else {
                msg = 'Uncaught Error.\n' + jqXHR.responseText;
            }
            console.log(msg);
        }
    });

    window.checkPaperlessBillingStatus = checkPaperlessBillingStatus;
    function checkPaperlessBillingStatus(el) {
        dataLayer.push({
            'event': 'user_dashboard_setting',
            'eventCategory': 'AEML User Dashboard',
            'eventAction': 'user_dashboard_setting',
            'eventLabel': 'Paperless_Billing',
            'business_user_id': $('#BusinessUserId').val(),
            'ca_number': $('#GACANumber').val(),
            'login_status': $('#login_status').val()
        });
        var PaperlessBilling = $("#chkPaperlessBilling").prop('checked');
        $('.loader-wrap').show();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/PaperlessBilling",
            type: 'GET',
            /**/
            data: { PaperlessBilling: PaperlessBilling },
            /**/
            dataType: 'text',
            success: function (data) {
                $('.loader-wrap').hide();
                console.log("sucess");
                var result = JSON.parse(data);
                ShowAlert(result.IsSuccess, result.Header, result.Message);
                $(el).closest('li').hide();
                if (!($('#UDsettingsDiv li').is(":visible"))) {
                    $('#UDsettingDivEl').hide();
                    $('#UDNoSettingEl').show();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation--' + "http://" + $(location).attr('host') + '/Setting/ChangeBillLanguageSettingRevamp?ca_number=' + errorThrown);

            }
        });
    }

    window.checkBillOverSMS = checkBillOverSMS;
    function checkBillOverSMS() {
        var BillOverSMS = $("#chkBillOverSMS").prop('checked');
        $('.loader-wrap').show();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/PaperlessBilling",
            type: 'GET',
            /**/
            data: { BillOverSMS: BillOverSMS },
            /**/
            dataType: 'text',
            success: function (data) {
                $('.loader-wrap').hide();
                console.log("sucess");
                if (data == "success") {
                    location.reload();
                    console.log("sucess");
                } else {
                    location.reload();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation--' + "http://" + $(location).attr('host') + '/Setting/ChangeBillLanguageSettingRevamp?ca_number=' + errorThrown);

            }
        });
    }

    window.checkEmailAlert = checkEmailAlert;
    function checkEmailAlert(el) {
        dataLayer.push({
            'event': 'user_dashboard_setting',
            'eventCategory': 'AEML User Dashboard',
            'eventAction': 'user_dashboard_setting',
            'eventLabel': 'Email Alerts',
            'business_user_id': $('#BusinessUserId').val(),
            'ca_number': $('#GACANumber').val(),
            'login_status': $('#login_status').val()
        });
        var EmailAlert = $("#chkEmailAlert").prop('checked');
        $('.loader-wrap').show();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/EmailAlertsBody",
            type: 'GET',
            /**/
            data: { EmailAlert: EmailAlert },
            /**/
            dataType: 'text',
            success: function (data) {
                $('.loader-wrap').hide();
                console.log("sucess");
                var result = JSON.parse(data);
                ShowAlert(result.IsSuccess, result.Header, result.Message);
                $(el).closest('li').hide();
                if (!($('#UDsettingsDiv li').is(":visible"))) {
                    $('#UDsettingDivEl').hide();
                    $('#UDNoSettingEl').show();
                    //$('#UDsettingsDiv').hide();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation--' + "http://" + $(location).attr('host') + '/Setting/ChangeBillLanguageSettingRevamp?ca_number=' + errorThrown);

            }
        });
    }

    window.checkSMSAlert = checkSMSAlert;
    function checkSMSAlert(el) {
        dataLayer.push({
            'event': 'user_dashboard_setting',
            'eventCategory': 'AEML User Dashboard',
            'eventAction': 'user_dashboard_setting',
            'eventLabel': 'SMS Alerts',
            'business_user_id': $('#BusinessUserId').val(),
            'ca_number': $('#GACANumber').val(),
            'login_status': $('#login_status').val()
        });
        var SMSAlert = $("#chkSMSAlert").prop('checked');
        $('.loader-wrap').show();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/SMSAlertsBody",
            type: 'GET',
            /**/
            data: { SMSAlert: SMSAlert },
            /**/
            dataType: 'text',
            success: function (data) {
                $('.loader-wrap').hide();
                console.log("sucess");
                var result = JSON.parse(data);
                ShowAlert(result.IsSuccess, result.Header, result.Message);
                $(el).closest('li').hide();
                if (!($('#UDsettingsDiv li').is(":visible"))) {
                    $('#UDsettingDivEl').hide();
                    $('#UDNoSettingEl').show();
                    //$('#UDsettingsDiv').hide();
                }
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation--' + "http://" + $(location).attr('host') + '/Setting/ChangeBillLanguageSettingRevamp?ca_number=' + errorThrown);

            }
        });
    }

    window.CreatedBarChart = CreatedBarChart;
    function CreatedBarChart(id) {
        var xValues = [];
        var yValues = [];
        var barColors = [];

        if (id == 2) {
            barColors = ["#e1e0e0", "#18aa26"];
            var percentage = 0;
            //debugger;
            var currentMonthAmt = payObj[0] != undefined ? parseFloat(payObj[0][1]) : 0.00;
            var previousMonthAmt = payObj[1] != undefined ? parseFloat(payObj[1][1]) : 0.00;
            if (currentMonthAmt > previousMonthAmt) {
                percentage = (currentMonthAmt - previousMonthAmt) / currentMonthAmt * 100;
                $('#percentagePaymentsTrend').html(`${upwardsArrow}<h4>${percentage.toFixed(2)}%</h4>
                                <p>Increased from Previous Month</p>`).addClass('up');
            } else if (currentMonthAmt < previousMonthAmt) {
                percentage = (previousMonthAmt - currentMonthAmt) / previousMonthAmt * 100;
                $('#percentagePaymentsTrend').html(`${downwardArrow}<h4>${percentage.toFixed(2)}%</h4>
                                <p>Decreased from Previous Month</p>`);
            } else {
                $('#percentagePaymentsTrend').html(`<h4>${percentage.toFixed(2)}%</h4>`);
            }


            setTimeout(
                dataLayer.push({
                    'event': 'user_dashboard_payment_trend',
                    'eventCategory': 'AEML User Dashboard',
                    'eventAction': 'user_dashboard_payment_trend',
                    'eventLabel': '2-months',
                    'business_user_id': $('#BusinessUserId').val(),
                    'ca_number': $('#GACANumber').val(),
                    'login_status': $('#login_status').val(),
                    'year_month': '2-months',
                }), 5000);

        } else if (id == 3) {
            barColors = ["#e1e0e0", "#e1e0e0", "#18aa26"];


            setTimeout(
                dataLayer.push({
                    'event': 'user_dashboard_payment_trend',
                    'eventCategory': 'AEML User Dashboard',
                    'eventAction': 'user_dashboard_payment_trend',
                    'eventLabel': '3-months',
                    'business_user_id': $('#BusinessUserId').val(),
                    'ca_number': $('#GACANumber').val(),
                    'login_status': $('#login_status').val(),
                    'year_month': '3-months',
                }), 5000);

        } else if (id == 4) {
            barColors = ["#e1e0e0", "#e1e0e0", "#e1e0e0", "#18aa26"];


            setTimeout(
                dataLayer.push({
                    'event': 'user_dashboard_payment_trend',
                    'eventCategory': 'AEML User Dashboard',
                    'eventAction': 'user_dashboard_payment_trend',
                    'eventLabel': '4-months',
                    'business_user_id': $('#BusinessUserId').val(),
                    'ca_number': $('#GACANumber').val(),
                    'login_status': $('#login_status').val(),
                    'year_month': '4-months',
                }), 5000);

        }

        //var objId = id - 1;
        for (var i = (id - 1); i >= 0; i--) {
            if (payObj[i] != undefined) {
                xValues.push(payObj[i][0]);
                yValues.push(payObj[i][1]);
            }
        }

        //var DataLayerYearMonths = ``;
        //for (var j = 0; j < xValues.length; j++) {
        //    DataLayerYearMonths +=  xValues[j] + ' ';
        //}

        //setTimeout(
        //    dataLayer.push({
        //        'event': 'user_dashboard_payment_trend',
        //        'eventCategory': 'AEML User Dashboard',
        //        'eventAction': 'user_dashboard_payment_trend',
        //        'eventLabel': 'Payment Month Trend Selected',
        //        'business_user_id': $('#BusinessUserId').val(),
        //        'ca_number': $('#GACANumber').val(),
        //        'login_status': $('#login_status').val(),
        //        'year_month': DataLayerYearMonths,
        //    }), 5000);

        //console.log(xValues);
        //console.log(yValues);
        //console.log(barColors);

        var radiusPlus = 1;
        Chart.elements.Rectangle.prototype.draw = function () {
            //debugger
            var ctx = this._chart.ctx;
            var vm = this._view;
            var left, right, top, bottom, signX, signY, borderSkipped;
            var borderWidth = vm.borderWidth;

            if (!vm.horizontal) {
                left = vm.x - vm.width / 2;
                right = vm.x + vm.width / 2;
                top = vm.y;
                bottom = vm.base;
                signX = 1;
                signY = bottom > top ? 1 : -1;
                borderSkipped = vm.borderSkipped || 'bottom';
            } else {
                left = vm.base;
                right = vm.x;
                top = vm.y - vm.height / 2;
                bottom = vm.y + vm.height / 2;
                signX = right > left ? 1 : -1;
                signY = 1;
                borderSkipped = vm.borderSkipped || 'left';
            }

            if (borderWidth) {
                var barSize = Math.min(Math.abs(left - right), Math.abs(top - bottom));
                borderWidth = borderWidth > barSize ? barSize : borderWidth;
                var halfStroke = borderWidth / 2;
                var borderLeft = left + (borderSkipped !== 'left' ? halfStroke * signX : 0);
                var borderRight = right + (borderSkipped !== 'right' ? -halfStroke * signX : 0);
                var borderTop = top + (borderSkipped !== 'top' ? halfStroke * signY : 0);
                var borderBottom = bottom + (borderSkipped !== 'bottom' ? -halfStroke * signY : 0);

                if (borderLeft !== borderRight) {
                    top = borderTop;
                    bottom = borderBottom;
                }
                if (borderTop !== borderBottom) {
                    left = borderLeft;
                    right = borderRight;
                }
            }

            var barWidth = Math.abs(left - right);
            var roundness = this._chart.config.options.barRoundness || 0.5;
            var radius = barWidth * roundness * 0.5;

            var prevTop = top;

            top = prevTop + radius;
            var barRadius = top - prevTop;

            ctx.beginPath();
            ctx.fillStyle = vm.backgroundColor;
            ctx.strokeStyle = vm.borderColor;
            ctx.lineWidth = borderWidth;

            // draw the chart
            var x = left, y = (top - barRadius + 1), width = barWidth, height = bottom - prevTop, radius = barRadius + radiusPlus;

            ctx.moveTo(x + radius, y);
            ctx.lineTo(x + width - radius, y);
            ctx.quadraticCurveTo(x + width, y, x + width, y + radius);
            ctx.lineTo(x + width, y + height);
            ctx.lineTo(x, y + height);
            ctx.lineTo(x, y + radius);
            ctx.quadraticCurveTo(x, y, x + radius, y);
            ctx.closePath();

            ctx.fill();
            if (borderWidth) {
                ctx.stroke();
            }

            top = prevTop;
        }


        //debugger
        new Chart(id + "-Months", {
            type: "bar",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    data: yValues
                }]
            },
            options: {
                responsive: true,
                //barRoundness: 1,
                //events: [],
                legend: { display: false },
                title: {
                    display: false,
                    text: "Payment Trends"
                },
                scales: {
                    xAxes: [{
                        barThickness: 30,
                        gridLines: {
                            display: false
                        }
                    }],
                    yAxes: [{
                        //barPercentage: 0.4,
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        ticks: {
                            display: false,
                            beginAtZero: true, // minimum value will be 0.
                        }
                    }]
                }
            }
        });
    }

    window.UDMeterStatusDetails = UDMeterStatusDetails
    function UDMeterStatusDetails(id) {
        switch (id) {
            case 'R':
                return 'Running';
                break;
            case 'D':
                return 'Disconnected';
                break;
            case 'N':
                return 'New';
                break;
            default:
                return '';
        }
    }

    window.UDFetchQuickPay = UDFetchQuickPay
    function UDFetchQuickPay(caNumber) {
        $('.loader-wrap').show();
        var accountNumber = caNumber;
        var myQuickBillPaymentUrl = $('#myQuickBillPaymentUrl').val();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/PayBillRevamp",
            type: 'GET',
            data: { AccountNumber: UDgetEncriptedKey(accountNumber), AmountPayable: $('#advanceAmmount').val(), Pay_PaymentGateway: "Proceed to Pay" },
            success: function (data) {
                $('.loader-wrap').hide();
                dataLayer.push({
                    'event': 'user_dashboard_pay_bill',
                    'eventCategory': 'AEML User Dashboard',
                    'eventAction': 'user_dashboard_pay_bill',
                    'eventLabel': 'Pay Bill Click',
                    'business_user_id': $('#BusinessUserId').val(),
                    'ca_number': $('#GACANumber').val(),
                    'login_status': $('#login_status').val(),
                    'month': $('#UDBillMonth').html(),
                    'total_bill_amount': $('#UDAmountPayable').html(),
                    'due_date': $('#UDBillDueDate').val()
                });
                window.location.href = "http://" + $(location).attr('host') + '' + myQuickBillPaymentUrl + '?ca_number=' + UDgetEncriptedKey(accountNumber);



            },
            error: function (xhr, textStatus, errorThrown) {

            }
        });
    }

    window.UDgetEncriptedKey = UDgetEncriptedKey
    function UDgetEncriptedKey(stringToEncrypt) {
        var key = CryptoJS.enc.Utf8.parse('8080808080808080');
        var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
        var encrypted = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(stringToEncrypt), key,
            {
                keySize: 128 / 8,
                iv: iv,
                mode: CryptoJS.mode.CBC,
                padding: CryptoJS.pad.Pkcs7
            }).toString();
        return encrypted;
    }

    window.UDDownloadFile = UDDownloadFile
    function UDDownloadFile(month) {

        dataLayer.push({
            'event': 'user_dashbaord_bill_download',
            'eventCategory': 'AEML User Dashboard',
            'eventAction': 'user_dashbaord_bill_download',
            'eventLabel': 'Bill Details',
            'business_user_id': $('#BusinessUserId').val(),
            'ca_number': $('#GACANumber').val(),
            'year_month': month,
            'login_status': $('#login_status').val()
        });

        var accountNumber = $('#UDAcccountNumber').val();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/DownloadPayBillRevamp",
            type: 'POST',
            cache: false,
            data: { AccountNumber: UDgetEncriptedKey(accountNumber), selectedMonth: month, viewBill: 'viewbill' },
            success: function () {
                UDopenUrlInTab(apiSettings + "/AccountsRevamp/ActualPDFRenderingRevamp");
            },
            error: function (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }

            }
        });
    }

    window.UDopenUrlInTab = UDopenUrlInTab
    function UDopenUrlInTab(url) {
        // Create link in memory
        var a = window.document.createElement("a");
        a.target = '_blank';
        a.href = url;

        // Dispatch fake click
        var e = window.document.createEvent("MouseEvents");
        e.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
        a.dispatchEvent(e);
    }

    window.UDSwitchInternalAccount = UDSwitchInternalAccount
    function UDSwitchInternalAccount(el) {
        var itemId = $(el).attr('data-itemId');

        dataLayer.push({
            'event': 'user_dashboard_switch_account',
            'eventCategory': 'AEML User Dashboard',
            'eventAction': 'user_dashboard_switch_account',
            'eventLabel': $(el).text(),
            'business_user_id': $('#BusinessUserId').val(),
            'ca_number': $('#GACANumber').val(),
            'login_status': $('#login_status').val()
        });
        $.ajax({
            url: apiSettings + "/AccountsRevamp/SwitchAccountInternallyRevamp",
            type: 'GET',
            data: { ItemId: itemId },
            dataType: 'json',
            beforeSend: function () {
                // setting a timeout
                $('.loader-wrap').show();
            },
            success: function (data, textStatus, xhr) {
                //$("#meterInfo").html();
                //$("#meterNumber").html();
                location.reload();
                $('.loader-wrap').show();
            },
            error: function (xhr, textStatus, errorThrown) {
                $('.loader-wrap').hide();
                console.log('Error in Operation');
            }
        });
    }

    window.AddRupeeSymbol = AddRupeeSymbol
    function AddRupeeSymbol(amount) {
        if (amount == undefined)
            amount = '0.00';

        return "₹ " + amount;
    }

    window.GetFullDateFormat = GetFullDateFormat
    function GetFullDateFormat(date) {
        if (date != undefined) {
            const [day, month, year] = date.split('-');
            const d = new Date(+year, month - 1, +day)
            const ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d)
            const mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d)
            const da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d)
            return `${da} ${mo} ${ye}`;
        } else {
            return '00-00-0000';
        }

    }

    window.GetFullDateFormatConsumptionChart = GetFullDateFormatConsumptionChart
    function GetFullDateFormatConsumptionChart(date) {
        if (date != undefined) {
            const [year, month, day] = date.split('-');
            const d = new Date(+year, month - 1, +day)
            const ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d)
            const mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d)
            const da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d)
            return `${da} ${mo} ${ye}`;
        } else {
            return '00-00-0000';
        }

    }

    window.GetTwoDigitYearDateFormat = GetTwoDigitYearDateFormat
    function GetTwoDigitYearDateFormat(date) {
        if (date != undefined) {
            const [day, month, year] = date.split('-');
            const d = new Date(+year, month - 1, +day)
            const ye = new Intl.DateTimeFormat('en', { year: '2-digit' }).format(d)
            const mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d)
            const da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d)
            return `${da} ${mo} ${ye}`;
        } else {
            return '00-00-0000';
        }
    }

    window.tConvert = tConvert
    function tConvert(time) {
        // Check correct time format and split into components
        time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

        if (time.length > 1) { // If time format correct
            time = time.slice(1);  // Remove full string match value
            time[5] = +time[0] < 12 ? ' AM' : ' PM'; // Set AM/PM
            time[0] = +time[0] % 12 || 12; // Adjust hours
        }
        return time[0] + time[1] + time[2] + time[5]; // return adjusted time or original string
    }

    window.UDcreateCookie = UDcreateCookie
    function UDcreateCookie(name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else var expires = "";

        document.cookie = name + "=" + value + expires + "; path=/";
    }

    window.handleNotificationViewDetails = handleNotificationViewDetails
    function handleNotificationViewDetails(NotificationName, RegistrationNumber, status) {

        if (NotificationName == 'TrackComplaintRevamp') {
            dataLayer.push({
                'event': 'user_dashboard_notification',
                'eventCategory': 'AEML User Dashboard',
                'eventAction': 'user_dashboard_notification',
                'eventLabel': 'CGRF Complaint Notification',
                'index': RegistrationNumber,
                'complaint_status': status,
                'business_user_id': $('#BusinessUserId').val(),
                'ca_number': $('#GACANumber').val(),
                'login_status': $('#login_status').val()
            });

            UDcreateCookie('ActiveComplaintLi', 'TrackComplaintRevamp', '');

        } else if (NotificationName == 'TrackCGRFComplaintsRevamp') {

            dataLayer.push({
                'event': 'user_dashboard_notification',
                'eventCategory': 'AEML User Dashboard',
                'eventAction': 'user_dashboard_notification',
                'eventLabel': 'CGRF Complaint Notification',
                'index': RegistrationNumber,
                'complaint_status': status,
                'business_user_id': $('#BusinessUserId').val(),
                'ca_number': $('#GACANumber').val(),
                'login_status': $('#login_status').val()
            });

            UDcreateCookie('ActiveComplaintLi', 'TrackCGRFComplaintsRevamp', '');
        }
    }

});