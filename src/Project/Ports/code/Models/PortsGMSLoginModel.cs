using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class PortsGMSLoginModel
    {

        public string Email { set; get; }
        public string Mobile { set; get; }
        public string OTP { set; get; }
        public Guid RegistrationID { set; get; }
        public string UserType { set; get; }
        public string InCompleteGrievance { set; get; }
        public string Gcptchares { set; get; }

        public PortsGMSLoginModel()
        {
        }
    }
}