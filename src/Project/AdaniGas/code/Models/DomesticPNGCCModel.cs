using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniGas.Website.Models
{
    [Serializable]
    public class DomesticPNGCCModel
    {
        public List<SelectListItem> LocationWiseGasRateList { get; set; }
        public string LocationWiseGasRates { get; set; }
        public List<SelectListItem> CylinderSizesList { get; set; }

        public string selectedLocationForGasRate { get; set; }
        public string selectedCylinderSize { get; set; }

        public double Averagecost { get; set; }
        public double TotalCost { get; set; }

        public double TotalSaving { get; set; }

        public double EquivalentMMBTU { get; set; }

        [Required]
        [Range(1, 40, ErrorMessage = "Number of cylinders consumed in a year must be between 1 and 40")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Number of cylinders must be a number")]
        public double NumberOfCylinders { get; set; }

        [Required]
        [Range(1, 5000, ErrorMessage = "Average cost per cylinder must be between 1 and 5000")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Number of cylinders must be a number")]
        public double AverageCostPerCylinder { get; set; }

        public string Date { get; set; }
    }
}