$(document).ready(function () {
    $("#calculate").click(function () {
        //Total kgs of LPG gas consumed = number of LPG cylinders * Size of the LPG cylinder(in kgs)
        var numberOfLPGcylinders = $("#NumberOfCylinder").val();
        var sizeoftheLPGcylinder = $("input[name='cylinderSel']:checked").val();
        var totalkgsofLPGgasconsumed = numberOfLPGcylinders * sizeoftheLPGcylinder;
        var averagecostperLPGcylinder = $("#costPerCyl").val();

        //Cost of LPG gas per kg = Average cost per LPG cylinder / Size of the LPG cylinder(in kgs)
        var costofLPGgasperkg = averagecostperLPGcylinder / sizeoftheLPGcylinder;

        //Annual LPG Cost = Total kgs of LPG gas consumed * Cost of LPG gas per kg
        var annualLPGCost = totalkgsofLPGgasconsumed * costofLPGgasperkg;

        //Equivalent MMBTU = (Total kgs of LPG gas consumed * 11500 * 3.968321) / 1000000
        var equivalentMMBTU = (totalkgsofLPGgasconsumed * 11500 * 3.968321) / 1000000;


        //Estimated Cost for PNG gas = Equivalent MMBTU * Current PNG Gas Rate
        var currentPNGGasRate = $("#locations").val();
        var estimatedCostforPNGgas = equivalentMMBTU * currentPNGGasRate;

        //Savings = Annual LPG Cost - Estimated Cost for PNG gas
        var savings = annualLPGCost - estimatedCostforPNGgas;

        $("#estimateCost").text(estimatedCostforPNGgas.toFixed(2));
        $("#estimateSaving").text(savings.toFixed(2));
        $("#mmbtu").text(equivalentMMBTU.toFixed(1));
    });
});

function setAvgCost() {
    var avgCost = $("#locations").val();
    $("#Averagecost").text(avgCost);
}