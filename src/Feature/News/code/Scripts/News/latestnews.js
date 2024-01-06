
$(document).ready(function () {
    BindNewsDataTable("tblNews", "/api/News/LoadNews");
});

function BindNewsDataTable(tableId, actionURL) {
    var TableIdWithPrefix = $("#" + tableId);
    table = TableIdWithPrefix.DataTable({
        "pageLength": 10,

        "ajax": {
            "url": actionURL,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "Title", "name": "Title", "autoWidth": false },
            //{"data": "Center", "name": "Center", "autoWidth": false },
            //{"data": "Amount", "name": "Amount", "autoWidth": false },
            //{"data": "PaymentMode", "name": "PaymentMode", "autoWidth": false },
            //{"data": "Receipt", "name": "Receipt", "autoWidth": false }
        ],
        "bDestroy": true

    });
}

