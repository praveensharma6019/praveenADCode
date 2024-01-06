using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AhmedabadAirport.Website.Model
{
    public class Contact_Us_CRM_Request
    {
        public string Name { get; set; }
        public string adl_mobilenumber { get; set; }
        public string adl_emailaddress { get; set; }
        public string adl_airportname = "AMD";
        public string adl_flightdate = "";
        public string adl_flightnumber = "";
        public int casetypecode { get; set; }
        public string adl_description { get; set; }
    }
}