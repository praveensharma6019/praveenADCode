
$(document).ready(function () {
    //debugger;
    BindPaymentHistoryRevampDataTable("#tblPaymentHistoryRevamp", apiSettings + "/AccountsRevamp/LoadData_PaymentHistoryRevamp");
});

function formartPaymentHistoryDate(date) {
    if (date != undefined) {
        const [year, month, day] = date.split('-');
        const d = new Date(+year, month - 1, +day)
        const ye = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(d)
        const mo = new Intl.DateTimeFormat('en', { month: '2-digit' }).format(d)
        const da = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(d)
        return `${da}-${mo}-${ye}`;
    } else {
        return '00-00-0000';
    }
}

function paymentDetails(id) {
    switch (id) {
        case 'C':
            return 'Cheque Payment';
            break;
        case 'B':
            return 'Bank Payment';
            break;
        case 'A':
            return 'Adjustments';
            break;
        case 'F':
            return 'Adjustments';
            break;
        case 'G':
            return 'Adjustments';
            break;
        case 'E':
            return 'Supplementary Bill';
            break;
        case 'Blank':
            return 'Cash Payment';
            break;
        case 'I':
            return 'Internet Payment';
            break;
        case 'V':
            return 'VDS( Voluntary Deposit Scheme )';
            break;
        default:
            return '';
    }
}

function BindPaymentHistoryRevampDataTable(tableId, actionURL) {
    $('.loader-wrap').show();
    //debugger;
    $.ajax({
        url: actionURL,
        dataType: "json",
        success: function (data) {
            $('.loader-wrap').hide();
            if (data.data != undefined && data.data != '' && data.data != null) {
                var dictDownload = $('#hdnDictDownload').val();
                var dictReceiptNumber = $('#hdnDictReceiptNo').val();
                $('#paymentHistoryCount').html(data.data.length);
                //console.log(data);
                $(tableId).DataTable({
                    lengthChange: false,
                    pageLength: 12,
                    searching: false,
                    ordering: false,
                    autoWidth: false,
                    info: false,
                    data: data.data,
                    columns: [
                        {
                            'data': 'PaymentDate',
                            'render': function (data, type, row) {
                                return `<div class="table-cell border-right"><label>${dictReceiptNumber}: ${row.Receipt}</label>
                                        <div class="status">Payment Method: ${paymentDetails(row.PaymentMode)}</div>
                                        <div class="status">Payment Center: ${row.Center}</div></div>`
                            }
                        },
                        {
                            'data': 'PaymentDate',
                            'render': function (data, type, row) {
                                return `<div class="table-cell"><label>${formartPaymentHistoryDate(row.PaymentDate)}</label></div>`
                            }
                        },
                        {
                            'data': 'Amount',
                            'render': function (data, type, row) {
                                return `<div class="table-cell"><label>${row.Amount}</label></div>`
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

                $('#tblPaymentHistoryRevamp_paginate').appendTo('.pagination');
                if (data.data.length <= 10) {
                    $('#tblPaymentHistoryRevamp_paginate').hide();
                } else {
                    $('#tblPaymentHistoryRevamp_paginate').show();
                }
            } else {
                $('#paymentHistoryCount').html('0');
                $('#tblPaymentHistoryBody').html(`<tr>
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

            $('#paymentHistoryCount').html('0');
            $('#tblPaymentHistoryBody').html(`<tr>
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
        }
    });
}

