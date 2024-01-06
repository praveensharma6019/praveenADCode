using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Transmission.Website.Models
{
    [Serializable]
    public class TransmissionsCostCalculator
    {
        public Guid Id { set; get; }
        public string RegistrationNumber { set; get; }
        public string CompanyName { set; get; }
        public string Login { set; get; }
        public string Mobile { set; get; }
        public string ElectricityBillConsumptionVal { set; get; }
        public string ElectricityConsumedatResidences { set; get; }
        public string TotalFamilyMembers { set; get; }
        public string TotalTrips { set; get; }
        public string MonthNames { set; get; }
        public string Years { set; get; }
        public string CNGUseds { set; get; }
        public string CNGUsedsVal { set; get; }
        public string LPGCylinders { set; get; }
        public string LPGCylindersVal { set; get; }
        public string DieselConsumptions { set; get; }
        public string DieselConsumptionsVal { set; get; }
        public string PetrolConsumptions { set; get; }
        public string PetrolConsumptionsVal { set; get; }
        public string AutoRikshaws { get; set; }
        public string AutoRikshawsVal { get; set; }
        public string Buses { get; set; }
        public string BusesVal { get; set; }
        public string reResponse { get; set; }
        public string PageInfo { get; set; }
        public string FormType { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string Trains { get; set; }
        public string TrainsVal { get; set; }
        public string EmployeeTotalemissionsperMonths { get; set; }
        public string AnnualCarbonFootprints { get; set; }
        public string TotalTransportationUses { get; set; }
        public string TotalDomesticUses { get; set; }
        public string NumberOfTreesNeeded { get; set; }
        public string LandNeeded { get; set; }
        public string AverageAnnualCarbonFootprints { get; set; }
        public string CarbonEmissionReducePercentage { get; set; }
        public string CarbonEmissionReviewYear { get; set; }
        public DateTime? CarbonEmissionReviewDate { get; set; }
        public string HistoryElectricityConsumedatResidences { get; set; }
        public string HistoryCNGUsed { get; set; }
        public string HistoryLPGUsed { get; set; }
        public string HistoryLPGUsedVal { get; set; }
        public string HistoryDieselConsumptions { get; set; }
        public string HistoryPetrolConsumptions { get; set; }
        public string HistoryAutoRikshaws { get; set; }
        public string HistoryBuses { get; set; }
        public string HistoryTrains { get; set; }
        public string HistoryTotalTrips { set; get; }

        public string HistoryAnnualCarbonFootprint { set; get; }
        public string NumberofTress { set; get; }
        public string ProjectName { set; get; }
        public string EstimatedCarbonAnnualizedValue { set; get; }
        public string DropdownAppliancesList { set; get; }
        public string DropdownAppliancesConsumptionVal { set; get; }
        public string DropdownAppliancesConsumption { set; get; }
        public string AirTripsDropdownList { set; get; }
        public string AirTripsValue { set; get; }
        public string AirTrips { set; get; }
    }

    public class ViewCarbonCalculator
    {
        public Guid Id { set; get; }
        public string RegistrationNumber { set; get; }
        public string FullName { set; get; }
        public string EmailID { set; get; }
        public string CompanyName { set; get; }
        public string Login { set; get; }
        public string Mobile { set; get; }
        public string ElectricityConsumedatResidences { set; get; }
        public string TotalFamilyMembers { set; get; }
        public string TotalTrips { set; get; }
        public string MonthNames { set; get; }
        public string Years { set; get; }
        public string CNGUseds { set; get; }
        public string LPGCylinders { set; get; }
        public string DieselConsumptions { set; get; }
        public string PetrolConsumptions { set; get; }
        public string AutoRikshaws { get; set; }
        public string Buses { get; set; }
        public string reResponse { get; set; }
        public string PageInfo { get; set; }
        public string FormType { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string Trains { get; set; }
        public string EmployeeTotalemissionsperMonths { get; set; }
        public string AnnualCarbonFootprints { get; set; }
        public string TotalTransportationUses { get; set; }
        public string TotalDomesticUses { get; set; }
        public string NumberOfTreesNeeded { get; set; }
        public string NumberOfPlantationProjectTreesNeeded { get; set; }
        public string LandNeeded { get; set; }
        public string AverageAnnualCarbonFootprints { get; set; }
        public string CarbonEmissionReducePercentage { get; set; }
        public string CarbonEmissionReviewYear { get; set; }
        public DateTime? CarbonEmissionReviewDate { get; set; }
        public string HistoryElectricityConsumedatResidences { get; set; }
        public string HistoryCNGUsed { get; set; }
        public string HistoryLPGUsed { get; set; }
        public string HistoryDieselConsumptions { get; set; }
        public string HistoryPetrolConsumptions { get; set; }
        public string HistoryAutoRikshaws { get; set; }
        public string HistoryBuses { get; set; }
        public string HistoryTrains { get; set; }
        public string HistoryTotalTrips { set; get; }
        public string TotalCarbonEmission { set; get; }
        public string HistoryAnnualCarbonFootprint { set; get; }
        public string NumberofTress { set; get; }
        public string ProjectName { set; get; }
        public string EstimatedCarbonAnnualizedValue { set; get; }
        public string RegistrationDate { set; get; }
        public string ElectricityBillConsumptionVal { set; get; }
        public string CNGUsedsVal { set; get; }
        public string LPGCylindersVal { set; get; }
        public string DieselConsumptionsVal { set; get; }
        public string PetrolConsumptionsVal { set; get; }
        public string AutoRikshawsVal { set; get; }
        public string BusesVal { set; get; }
        public string TrainsVal { set; get; }
        public string DropdownAppliancesList { set; get; }
        public string DropdownAppliancesConsumptionVal { set; get; }
        public string DropdownAppliancesConsumption { set; get; }
        public string AirTripsDropdownList { set; get; }
        public string AirTripsValue { set; get; }
        public string AirTrips { set; get; }
        public string CarbonEmissionReviewYearTemp { set; get; }

        //carbonoffset values
        public string PersonalTransport { get; set; }
        public string PublicTransport { set; get; }
        public string OnlineMeeting { set; get; }
        public string FiveStarAppliances { set; get; }
        public string OffsetNumberofTreesNeeded { set; get; }
        public string EmissionOffsetForTreePlantation { set; get; }
        public string FundNeededtoPlantTrees { set; get; }
        public string OffsetEmissionforFundingTrees { set; get; }
        public string OffsetEmissionfromDomesticUse { set; get; }
        public string OffsetEmissionfromTransportation { get; set; }
        public string OffsetEmissionfromAirTrips { set; get; }
        public string TotalOffsetCarbonEmission { set; get; }
        public string AverageOffsetEmissionperMonth { set; get; }
        public string OffsetAnnualCarbonFootprint { set; get; }
        public string CarbonAnnualCarbonFootprintDifference { set; get; }
        public string NumberOfTreesNeededVal { set; get; }
        public string NumberOfPlantationProjectTreesNeededVal { set; get; }
        public string FundNeededtoPlantTreesVal { set; get; }
        public Boolean IsPersonalTransport { set; get; }
        public Boolean IsPublicTransport { set; get; }
        public Boolean IsOnlineMeeting { set; get; }
        public Boolean IsFiveStarAppliances { set; get; }
        public Boolean IsNumberOfTreesNeeded { set; get; }
        public Boolean IsNumberOfPlantationProjectTreesNeeded { set; get; }
        public Boolean IsFundNeededtoPlantTrees { set; get; }
        public string PersonalTransportDropdownValue { set; get; }
        public string PersonalTransportValue { set; get; }
        public string PublicTransportValue { set; get; }
        public string OnlineMeetingValue { set; get; }
        public string FiveStarAppliancesValue { set; get; }
    }

    public class CarbonOffsetvalues
    {
        public Guid Id { set; get; }
        public string Login { set; get; }
        public string RegistrationNumber { set; get; }
        public string PersonalTransport { get; set; }
        public string PublicTransport { set; get; }
        public string OnlineMeeting { set; get; }
        public string FiveStarAppliances { set; get; }
        public string NumberofTreesNeeded { set; get; }
        public string NumberofTreesNeededValue { set; get; }
        public string NumberofPlantationProjectTreesNeeded { set; get; }
        public string NumberofPlantationProjectTreesNeededValue { set; get; }
        public string FundNeededtoPlantTrees { set; get; }
        public string FundNeededtoPlantTreesValue { set; get; }
        public string OffsetEmissionfromDomesticUse { set; get; }
        public string OffsetEmissionfromTransportation { get; set; }
        public string OffsetEmissionfromAirTrips { set; get; }
        public string TotalOffsetCarbonEmission { set; get; }
        public string AverageOffsetEmissionperMonth { set; get; }
        public string OffsetAnnualCarbonFootprint { set; get; }
        public string CarbonAnnualCarbonFootprintDifference { set; get; }
        public DateTime? FormSubmitOn { set; get; }
        public string MonthNames { set; get; }
        public string Years { set; get; }
        public string PersonalTransportDropdownValue { set; get; }
        public string PersonalTransportValue { set; get; }
        public string PublicTransportValue { set; get; }
        public string OnlineMeetingValue { set; get; }
        public string FiveStarAppliancesValue { set; get; }
    }
}