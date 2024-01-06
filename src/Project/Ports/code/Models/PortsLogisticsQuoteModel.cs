using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class PortsLogisticsQuoteModel
    {
        public string Name { set; get;}
        public string Organization { set; get; }
        public string Email { set; get; }
        public string ContactNumber { set; get; }
        public string Service { set; get; }
        public string Details { set; get; }
        public string FormType { set; get; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }
}