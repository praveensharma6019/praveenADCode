$(document).ready(function () {

    //BindConsumptionHistoryDataTable("tblConsumptionHistory", "/api/Accounts/LoadData_ConsumptionHistory");
});

function BindConsumptionHistoryDataTable(tableId, actionURL) {

    $.ajax({
        url: actionURL,
        type: 'GET',
        dataType: 'json',
        success: function (data, textStatus, xhr) {
            $("#meterInfo").html();
            $("#meterNumber").html();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
        }
    }); 


    var TableIdWithPrefix = $("#" + tableId);
    table = TableIdWithPrefix.DataTable({
        "pageLength": 10,
        "ajax": {
            "url": actionURL,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "ConsumptionDate", "name": "ConsumptionDate", "autoWidth": false },
            { "data": "Status", "name": "Status", "autoWidth": false },
            { "data": "Reading", "name": "Reading", "autoWidth": false },
            { "data": "UnitsConsumed", "name": "UnitsConsumed", "autoWidth": false },
        ],
        "bDestroy": true,
        "bFilter": false
    });
}