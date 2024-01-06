

$(document).ready(function () {
    BindConsumptionHistoryRevampDataTable("#tblConsumptionHistoryDataTable", apiSettings + "/AccountsRevamp/LoadData_ConsumptionHistoryRevamp");
});

function connectionStatusDetails(id) {
    switch (id) {
        case 'R':
            return '<span class="success">Running</span>';
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

function GetFullDateFormat(date) {
    const d = new Date(date)
    const ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d)
    const mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d)
    const da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d)
    return `${da} ${mo} ${ye}`;
}

function GetMonthYear(date) {
    const d = new Date(date)
    const ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d)
    const mo = new Intl.DateTimeFormat('en', { month: 'short' }).format(d)
    const da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d)
    return `${mo} ${ye}`;
}

function BindConsumptionHistoryRevampDataTable(tableId, actionURL) {
    $('.loader-wrap').show();
    var dictNoted = $('#hdnDictNoted').val();
    var dictConnection = $('#hdnDictConnection').val();

    $.ajax({
        url: actionURL,
        dataType: "json",
        success: function (data) {
            $('.loader-wrap').hide();
            //debugger;
            //console.log(data);
            var xValues = ["0", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];
            var ConsumptionHistoryobj = [];
            if (data.data != undefined && data.data != '' && data.data != null && data.data.MeterConsumptions != '') {
                for (var i = 0; i < data.data.MeterConsumptions.length; i++) {
                    for (var j = 0; j < data.data.MeterConsumptions[i].ConsumptionRecords.length; j++) {
                        //debugger;
                        var notedDate = data.data.MeterConsumptions[i].ConsumptionRecords[j].ConsumptionDate;
                        var status = data.data.MeterConsumptions[i].ConsumptionRecords[j].Status;
                        var year = new Date(data.data.MeterConsumptions[i].ConsumptionRecords[j].ConsumptionDate).getFullYear();
                        var Reading = data.data.MeterConsumptions[i].ConsumptionRecords[j].Reading;
                        var month = new Date(data.data.MeterConsumptions[i].ConsumptionRecords[j].ConsumptionDate).getMonth();
                        var unitConsumed = data.data.MeterConsumptions[i].ConsumptionRecords[j].UnitsConsumed;
                        unitConsumed = parseFloat(unitConsumed.substring(0, unitConsumed.indexOf('.') + 3).replace(',', ''));

                        unitConsumed = unitConsumed != '' ? unitConsumed : 0;

                        if (ConsumptionHistoryobj != [] && ConsumptionHistoryobj.some(item => item.some(p => item.includes(xValues[month + 1] + '-' + year)))) {
                            var index = ConsumptionHistoryobj.findIndex((obj => obj[0] == xValues[month + 1] + '-' + year));
                            unitConsumed = ConsumptionHistoryobj[index][2] + unitConsumed;
                            ConsumptionHistoryobj[index][2] = unitConsumed;
                        } else {
                            ConsumptionHistoryobj.push([xValues[month + 1] + '-' + year, notedDate, unitConsumed, Reading, status])
                        }
                    }
                }

                //console.log(ConsumptionHistoryobj);
            }

            if (data.data != undefined && data.data != '' && data.data != null && data.data.MeterConsumptions != '') {
                //debugger;
                $('#consumptionHistoryCount').html(ConsumptionHistoryobj.length);
                $(tableId).DataTable({
                    lengthChange: false,
                    pageLength: 12,
                    searching: false,
                    autoWidth: false,
                    ordering: false,
                    info: false,
                    data: ConsumptionHistoryobj,
                    columns: [
                        {
                            'data': 'notedDate',
                            'render': function (data, type, row) {
                                //debugger;
                                return `<div class="table-cell border-right"><label>${GetMonthYear(row[1])}</label>
                                    <div class="status">${dictNoted}: ${GetFullDateFormat(row[1])}</div>
                                    <div class="status">${dictConnection}: ${connectionStatusDetails(row[4])}</div></div>`
                            }
                        },
                        {
                            'data': 'Reading',
                            'render': function (data, type, row) {
                                return `<div class="table-cell"><label>${row[3]}</label></div>`
                            }
                        },
                        {
                            'data': 'unitConsumed',
                            'render': function (data, type, row) {
                                return `<div class="table-cell"><label>${row[2].toLocaleString()}</label></div>`
                            }
                        },
                    ],
                    //pagingType: "custom",
                    language: {
                        paginate: {
                            next: '<i class="i-arrow-r"></i>',
                            previous: '<i class="i-arrow-l"></i>'
                        }
                    },
                    bPaginate: true,
                    "fnDrawCallback": function (oSettings) {
                        $(".section-container")[0].scrollIntoView(true);
                    }
                });

                $('#tblConsumptionHistoryDataTable_paginate').appendTo('#desktopConsumptionTablePagination');
                if (ConsumptionHistoryobj.length <= 12) {
                    $('#tblConsumptionHistoryDataTable_paginate').hide();
                    $('#mobileConsumptionTablePagination').hide();
                } else {
                    $('#tblConsumptionHistoryDataTable_paginate').show();
                    $('#mobileConsumptionTablePagination').show();
                }

                var mobileTable = ``;
                var count = 1;
                var displayPropMobileTable = ``;
                for (var i = 0; i < ConsumptionHistoryobj.length; i++) {

                    if (i / (count * 12) == 1) {
                        count = count + 1;
                    }
                    if (count == 1) {
                        displayPropMobileTable = 'block';
                    } else {
                        displayPropMobileTable = 'none';
                    }


                    mobileTable += `<div class="mob-listing " id="ConsumptionHistoryMobileListingDiv" data-id="${count}" style="display:@divstyle">
                        <ul>
                            <li>
                                <label>Monthly Units</label>
                                <h4>${ConsumptionHistoryobj[i][2].toLocaleString()}</h4>
                            </li>
                            <li>
                                <label>Month</label>
                                <span>${GetFullDateFormat(ConsumptionHistoryobj[i][1])}</span>
                            </li>
                            <li>
                                <label>Last Reading</label>
                                <span>${ConsumptionHistoryobj[i][3]}</span>
                            </li>
                            <li>
                                <label>status</label>
                                <span>${connectionStatusDetails(ConsumptionHistoryobj[i][4])}</span>
                            </li>
                        </ul>
                    </div>`;
                }
                $('#mobileConsumptionTable').html(mobileTable);
                DrawPaginationNumber(1, '#MobileConsumptionTablePaginationDiv');

            } else {
                $('#consumptionHistoryCount').html('0');
                $('#tblConsumptionHistoryData').html(`<tr>
                                <td>
                                    <div class="table-cell border-right">
                                        <label>NA</label>
                                    </div
                                </td>
                                <td>
                                    <div class="table-cell">
                                        <label>NA</label>
                                    </div>
                                </td>
                                <td>
                                    <div class="table-cell">
                                        <label>NA</label>
                                    </div>
                                </td>
                            </tr>`);

                $('#mobileConsumptionTable').html('');
            }
        },
        error: function (jqXHR, exception) {
            $('#consumptionHistoryCount').html('0');
            $('#tblConsumptionHistoryData').html(`<tr>
                                <td>
                                    <div class="table-cell border-right">
                                        <label>NA</label>
                                    </div
                                </td>
                                <td>
                                    <div class="table-cell">
                                        <label>NA</label>
                                    </div>
                                </td>
                                <td>
                                    <div class="table-cell">
                                        <label>NA</label>
                                    </div>
                                </td>
                            </tr>`);

            $('#mobileConsumptionTable').html('');
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
}