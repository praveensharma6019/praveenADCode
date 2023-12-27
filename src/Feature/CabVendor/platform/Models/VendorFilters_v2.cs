using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models
{
    public class VendorFiltersV2
    {
        public string language { get; set; }
        public string vendorCode { get; set; }
        public string airport { get; set; }
        public string tripType { get; set; }
        public string cabSchedule { get; set; }
        public string cabBookingType { get; set; }
        public bool isPreBooking { get; set; }
        public string cancellationCode { get; set; }
        public VendorFiltersV2()
        {
            language = "en";
            vendorCode = "";
            airport = "";
            tripType = "";
            cabSchedule = "";
            cabBookingType = "";
            isPreBooking = true;
        }
    }

}