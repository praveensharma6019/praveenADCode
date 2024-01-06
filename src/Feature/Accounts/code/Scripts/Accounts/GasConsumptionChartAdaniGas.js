// Load the Visualization API and the piechart package.  
google.load('visualization', '1.0', { 'packages': ['corechart'] });

// Set a callback to run when the Google Visualization API is loaded.  
$(document).ready(function () {
    getConsumptionYears();

    getConsumptionValues($('#year').val());

    $('#year').change(function () {
        var year = $('#year').val();
        getConsumptionValues(year);
    });
});

function getConsumptionYears() {
    $.ajax(
        {
            type: "post",
            dataType: 'JSON',
            async: false,
            url: '/api/AdaniGas/GetGasConsumptionPattern_Years',
            success:
                function (response) {
                    if (response.length > 0) {
                        $('#year').html('');
                        var options = '';
                        for (var i = 0; i < response.length; i++) {
                            options += '<option value="' + response[i].Value + '">' + response[i].Text + '</option>';
                        }
                        $('#year').append(options);
                    }
                }
        });
}

function getConsumptionValues(year) {
    $.ajax(
        {
            type: "post",
            dataType: 'JSON',
            data: { 'year': year },
            url: '/api/AdaniGas/GetGasConsumptionPatternChartAdaniGas',
            success:
                function(response) {
                    drawGraph(response.SCMValues, 'scmgraph', "Date", "Consumption(SCM)", "Date", "SCM Value");
                    drawGraph(response.AmountValues, 'amountgraph', "Date", "Consumption(Amount)", "Date", "Amount Value");
                    drawGraph(response.MMBTUValues, 'mmbtugraph', "Date", "Consumption(MMBTU)", "Date", "MMBTU Value");

                },
            error:
                function (response) {
                    clearchart('scmgraph');
                    clearchart('amountgraph');
                    clearchart('mmbtugraph');
                    drawGraph(response.SCMValues, 'scmgraph', "Date", "Consumption(SCM)", "Date", "SCM Value");
                    drawGraph(response.AmountValues, 'amountgraph', "Date", "Consumption(Amount)", "Date", "Amount Value");
                    drawGraph(response.MMBTUValues, 'mmbtugraph', "Date", "Consumption(MMBTU)", "Date", "MMBTU Value");
                }
        });
}

function drawGraph(dataValues, elementId, xaxisTitle, yaxisTitle, xaxisLabel, yaxisLabel) {
    try {
        var options =
        {
            width: 400,
            height: 300,
            sliceVisibilityThreshold: 0,
            legend: { position: "top", alignment: "center" },
            hAxis:
            {
                title: xaxisTitle,
                titleTextStyle: { color: 'black' },
                count: -1,
                viewWindowMode: 'pretty',
                slantedText: true
            },
            vAxis: { title: yaxisTitle, minValue: 0 },
            bar: { groupWidth: "50%" }
        };
        var data = new google.visualization.DataTable();

        data.addColumn({ type: 'string', id: 'Date', label: xaxisLabel });
        data.addColumn({ type: 'number', id: 'Consumption', label: yaxisLabel });
        data.addColumn({ type: 'string', role: 'tooltip' });

        if (dataValues != undefined) {

            for (var i = 0; i < dataValues.length; i++) {
                var tooltip = "" + dataValues[i].Date + " \n " + yaxisLabel + "- " + dataValues[i].Consumption + "";
                data.addRow([dataValues[i].Date, parseFloat(dataValues[i].Consumption), tooltip]);
            }
        }

        //var view = new google.visualization.DataView(data);
        //view.setColumns([0, 1]);

        var chart = new google.visualization.ColumnChart(document.getElementById(elementId));
        chart.draw(data, options);
    }
    catch{
        //
    }
}

function clearchart(elementId) {
    var chart = new google.visualization.ColumnChart(document.getElementById(elementId));
    chart.clearChart(); 
}
