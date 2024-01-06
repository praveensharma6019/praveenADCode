using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Power.Website.Models
{
    public class PowerCostCalculatorModel
    {
        public string ElectricityConsumedatResidences { set; get; }
        public string TotalFamilyMembers { set; get; }
        public string MonthNames { set; get; }
        public string Years{ set; get; }
        public string Email { set; get; }
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
        public string LandNeeded { get; set; }
        public string AverageAnnualCarbonFootprints { get; set; }
    }
}