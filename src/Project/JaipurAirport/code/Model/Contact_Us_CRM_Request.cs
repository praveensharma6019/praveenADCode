using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.JaipurAirport.Website.Model
{
    public class Contact_Us_CRM_Request
    {
        public string Name { get; set; }
        public string adl_mobilenumber { get; set; }
        public string adl_emailaddress { get; set; }
        public string adl_airportname = "JAI";
        public string adl_flightdate = "";
        public string adl_flightnumber = "";
        public int casetypecode { get; set; }
        public string adl_description { get; set; }
    }
}