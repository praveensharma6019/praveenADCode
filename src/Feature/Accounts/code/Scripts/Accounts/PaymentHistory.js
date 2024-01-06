$(document).ready(function () {
    BindPaymentHistoryDataTable("tblPaymentHistory", "/api/Accounts/LoadData_PaymentHistory");
});

function BindPaymentHistoryDataTable(tableId, actionURL) {
    var TableIdWithPrefix = $("#" + tableId);
    table = TableIdWithPrefix.DataTable({
        "pageLength": 10,
        "ajax": {
            "url": actionURL,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "PaymentDate", "name": "PaymentDate", "autoWidth": false },
            { "data": "Center", "name": "Center", "autoWidth": false },
            { "data": "Amount", "name": "Amount", "autoWidth": false },
            { "data": "PaymentMode", "name": "PaymentMode", "autoWidth": false },
            { "data": "Receipt", "name": "Receipt", "autoWidth": false }           
        ],
        "order": [[0, "desc"]],
        "bDestroy": true

    });
}
