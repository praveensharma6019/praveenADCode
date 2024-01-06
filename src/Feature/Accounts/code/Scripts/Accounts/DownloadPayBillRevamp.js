

$(document).ready(function () {
    BindDownPayBillRevampDataTable("#tblDownPayBillDataTable", apiSettings + "/AccountsRevamp/LoadData_DownloadPayBillRevamp");

    function DownloadFile(month) {
        var accountNumber = $('#AccountNumber').val();
        $.ajax({
            url: apiSettings + "/AccountsRevamp/DownloadPayBillRevamp", //  /api/Accounts/CreateDownloadPayBillRevamp    /api/AccountsRevamp/DownloadPayBillRevamp
            type: 'POST',
            cache: false,
            data: { AccountNumber: getEncriptedKey(accountNumber), selectedMonth: month, viewBill: 'viewbill' },
            beforeSend: function () {
                // setting a timeout
                $('.loader-wrap').show();
            },
            success: function () {
                $('.loader-wrap').hide();
                openUrlInTab(apiSettings + "/AccountsRevamp/ActualPDFRenderingRevamp");
                //window.open(apiSettings + "/AccountsRevamp/ActualPDFRenderingRevamp", "_blank");  //   /api/Accounts/ActualPDFRendering    /api/AccountsRevamp/ActualPDFRenderingRevamp
            },
            error: function (jqXHR, exception) {
                $('.loader-wrap').hide();
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
            },
            complete: function () {
                $('.loader-wrap').hide();
            }
        });
    }
    window.DownloadFile = DownloadFile
});


function BindDownPayBillRevampDataTable(tableId, actionURL) {
    var dictDownload = $('#hdnDictDownload').val();
    var dictBillNo = $('#hdnDictBillNo').val();
    $('.loader-wrap').show();
    $.ajax({
        url: actionURL,
        dataType: "json",
        success: function (data) {
            if (data.data != undefined && data.data != '' && data.data != null && data.data.InvoiceLines != '') {
                $('.loader-wrap').hide();
                $('#DownloadPayBillCount').html(data.data.InvoiceLines.length);
                $('#downloadLatestBillDetails').html(`<a href="javascript:void(0)" onclick="DownloadFile('${data.data.InvoiceLines[0].BillMonth}');">${dictDownload}</a>`);
                $(tableId).DataTable({
                    lengthChange: false,
                    pageLength: 12,
                    searching: false,
                    autoWidth: false,
                    ordering: false,
                    info: false,
                    data: data.data.InvoiceLines,
                    columns: [
                        {
                            'data': 'BillMonth',
                            'render': function (data, type, row) {
                                return `<div class="table-cell border-right"><label>${row.BillMonth}</label></div>`
                            }
                        },
                        //{
                        //    'data': 'InvoiceNumber',
                        //    'render': function (data, type, row) {
                        //        return `<div class="table-cell"><label>${row.InvoiceNumber}</label></div>`
                        //    }
                        //},
                        {
                            'data': 'BillMonth',
                            'render': function (data, type, row) {
                                return `<div class="table-cell"><label><a href="javascript:void(0)" onclick="DownloadFile('${row.BillMonth}');">${dictDownload}</a></label></div>`
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

                $('#tblDownPayBillDataTable_paginate').appendTo('.pagination');
                if (data.data.InvoiceLines.length <= 12) {
                    $('#tblDownPayBillDataTable_paginate').hide();
                } else {
                    $('#tblDownPayBillDataTable_paginate').show();
                }


            } else {
                $('.loader-wrap').hide();
                $('#DownloadPayBillCount').html('0');
                $('#tblDownPayBillData').html(`<tr>
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
                    </tr>`);
            }
        },
        error: function (jqXHR, exception) {
            $('.loader-wrap').hide();
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

            $('#DownloadPayBillCount').html('0');
            $('#tblDownPayBillData').html(`<tr>
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
                    </tr>`);
        },
        complete: function () {
            $('.loader-wrap').hide();
        }
    });
}

function openUrlInTab(url) {
    // Create link in memory
    var a = window.document.createElement("a");
    a.target = '_blank';
    a.href = url;

    // Dispatch fake click
    var e = window.document.createEvent("MouseEvents");
    e.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
    a.dispatchEvent(e);
};