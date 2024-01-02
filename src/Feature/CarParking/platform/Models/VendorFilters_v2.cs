using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CarParking.Platform.Models
{
    public class VendorFiltersV2
    {
        public string language { get; set; }       
        public string airport { get; set; }      
        public bool isPreBooking { get; set; }
        public string TerminalCode { get; set; }
        public string parkingPackage { get; set; }
        public string facilityNumber { get; set; }
        public string productID { get; set; }

        public VendorFiltersV2()
        {
            language = "en";           
            airport = "";          
            isPreBooking = true;
            TerminalCode = "";
            parkingPackage = "";
            facilityNumber = "";
            productID = "";
        }
    }

}