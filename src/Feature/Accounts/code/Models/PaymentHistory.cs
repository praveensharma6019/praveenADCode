using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class PaymentHistory
    {
        public List<PaymentHistoryRecord> PaymentHistoryList { get; set; }
    }

    public class PaymentHistoryRecord
    {
        public string PaymentDate { get; set; }
        public string Center { get; set; }
        public string Amount { get; set; }
        public string PaymentMode { get; set; }
        public string Receipt { get; set; }
    }
}