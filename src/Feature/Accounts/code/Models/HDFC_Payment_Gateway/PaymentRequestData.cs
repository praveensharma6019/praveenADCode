using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models.HDFC_Payment_Gateway
{
    public class PaymentRequestData
    {
        public string AppId { get; set; }
        public string OrderId { get; set; }
        public string ClientCode { get; set; }
        public decimal OrderAmount { get; set; }
        public string OrderCurrency { get; set; }
        public string OrderNote { get; set; }
        public string CustomerReference { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string NotifyUrl { get; set; }
        public string ReturnUrl { get; set; }
        public string ErrorUrl { get; set; }
    }
}