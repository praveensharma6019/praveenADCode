$(document).ready(function () {
    $("#calculate").click(function () {

        var avgRun = $("#averagerunning").val();
        var currentMileage = $("#mileage").val();
        var expectCNGMileage = 20;
        var cngRate = $("#locations").val();
        var selectedLocation = $("#locations option:selected").text();
        var typeOfCNG = $("input[name='cylinderSel']:checked").val();
        var fuelRate = "";
        switch (selectedLocation) {
            case "Ahmedabad":
                fuelRate = typeOfCNG == "petrol" ? 64 : 62;
                break;
            case "Baroda":
                fuelRate = typeOfCNG == "petrol" ? 64 : 62;
                break;
            case "Faridabad":
                fuelRate = typeOfCNG == "petrol" ? 61.95 : 60;
                break;
            case "Khurja":
                fuelRate = typeOfCNG == "petrol" ? 64 : 62;
                break;
            default: break;
        }

        //Cost calculator for CNG: -
        //Savings = ((avgRun/currentMileage)*fuelRate)-((avgRun/expectCNGMileage)*cngRate)
        var savings = ((avgRun / currentMileage) * fuelRate) - ((avgRun / expectCNGMileage) * cngRate);
        var estimateCost = ((avgRun / expectCNGMileage) * cngRate);

        $("#estimateCost").text(estimateCost.toFixed(2));
        $("#estimateSaving").text(savings.toFixed(2));
    });
});

function setAvgCost() {
    var avgCost = $("#locations").val();
    $("#Averagecost").text(avgCost);
}