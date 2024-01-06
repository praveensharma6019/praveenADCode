using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class PortsGMSOTP
    {
        public Guid Id { set; get; }
        public string OTPType { set; get; }
        public string OTPFor { set; get; }
        public string OTP { get; set; }
        public Boolean Status{get; set;}
        public Boolean IsMobile { get; set; }
        public string Gcptchares { set; get; }
    }
}