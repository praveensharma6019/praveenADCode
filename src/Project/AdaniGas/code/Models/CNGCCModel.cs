using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniGas.Website.Models
{
    [Serializable]
    public class CNGCCModel
    {
        public List<SelectListItem> LocationWiseCNGRateList { get; set; }
        public string selectedLocationForCNGRate { get; set; }
        public List<SelectListItem> FuelTypeList { get; set; }
        public string selectedFuelType { get; set; }
        public string LocationWiseCNGRate { get; set; }
        public double Averagecost { get; set; }
        public double TotalCost { get; set; }
        public double TotalSaving { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Average Run must be between 1 and 100000")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Average Run must be a number")]
        public double AverageRun { get; set; }

        [Required]
        [Range(1, 25, ErrorMessage = "Current Mileage must be between 1 and 25")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Current Mileage must be a number")]
        public double CurrentMileage { get; set; }
    }
    public class CityWisePrice
    {
        public string City { get; set; }
        public string Product { get; set; }
        public string Eff_date { get; set; }
        public string MMBTU_Rate { get; set; }
    }
}