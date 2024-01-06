using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Transmission.Website.Models
{
    [Serializable]
    public class TransmissionCostCalculatorModel
    {
        public string Email { set; get; }
        public string LoginId { set; get; }
        public string Mobile { set; get; }
        public string OTP { set; get; }
        public Guid RegistrationID { set; get; }
        public string UserType { set; get; }
        public string InCompleteCarbonCalcultor { set; get; }

        public TransmissionCostCalculatorModel()
        {
        }
    }
}