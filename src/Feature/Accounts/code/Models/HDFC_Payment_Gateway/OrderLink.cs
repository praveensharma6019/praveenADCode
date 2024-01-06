using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models.HDFC_Payment_Gateway
{
    public class OrderLink
    {
        public string PaymentLink { get; set; }
        public string Reason { get; set; }
        public bool IsSuccess { get; set; }
    }

    public class OrderInformation
    {
        public string ReferenceCode { get; set; }
        public string PaymentCode { get; set; }
        public string Amount { get; set; }
        public string PostDate { get; set; }
        public string TransactionCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }

    }

    public enum OrderStatus
    {
        NOTAVAILABLE,
        ACTIVE,
        FAILED,
        SUCCESS,
        ERROR,
        FLAGGED,
        PROCESSING
    }
}
