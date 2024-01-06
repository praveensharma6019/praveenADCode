using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class BBPSModel
    {
        public string TransactionId { get; set; }
        public string AccountNumber { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string OrderId { get; set; }
        public string ResponseStatus { get; set; }
        public string Responsecode { get; set; }
        public string AmountPayable { get; set; }
        public string Remark { get; set; }
        public string PaymentType { get; set; }
        public string TransactionDate { get; set; }
        public string Gateway { get; set; }
        public string PaymentMode { get; set; }
    }
}