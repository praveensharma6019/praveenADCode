using Sitecore.Transmission.Website.Models;
using System;
using System.Web;

namespace Sitecore.Transmission.Website.SessionHelper
{
   
    public class UserSessionTransmission
    {
        


        public static TransmissionCostCalculatorModel CostCalculatorSesstion
        {
            get
            {
                return (TransmissionCostCalculatorModel)HttpContext.Current.Session["TransmissionCostCalculator"];
            }
            set
            {
                HttpContext.Current.Session["TransmissionCostCalculator"] = value;
            }
        }
        

        public UserSessionTransmission()
        {
        }
    }
}