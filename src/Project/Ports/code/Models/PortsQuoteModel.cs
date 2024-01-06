using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Ports.Website.Models
{
    public class PortsQuoteModel
    {
        public string Name { set; get; }
        public string Company { set; get; }
        public string Email { set; get; }
        public string DeliveryType { set; get; }
        public string Commodity { set; get; }
        public string CommodityType { set; get; }
        public string ExpectedQuantity { set; get; }
        public string ExpectedDate { set; get; }
        public string Message { set; get; }
        public string FormType { set; get; }
        public string PageInfo { get; set; }
        public DateTime FormSubmitOn { set; get; }
        public string OTP { get; set; }
    }
}