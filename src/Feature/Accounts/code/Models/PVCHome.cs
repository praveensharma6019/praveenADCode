using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class PVCHome
    {       
        public string LoginName { get; set; }
        public string AccountNumber { get; set; }
        public string BookNumber { get; set; }
        public string CycleNumber { get; set; }
        public string Zone { get; set; }
        public string Address { get; set; }
        public string BillMonth { get; set; }
        public string PaymentDueDate { get; set; }
        public string TariffSlab { get; set; }
        public string MeterNumber { get; set; }
        public string UnitsConsumed { get; set; }
        public string TotalCharges { get; set; }
        public string CurrentMonthsBills { get; set; }
        public string BroughtForward { get; set; }
        public string TotalBillAmount { get; set; }
        public string SecurityDeposit { get; set; }
        public string VDSAmount { get; set; }
        public string AmountPayable { get; set; }
        public int PaymentGateway { get; set; }
        public string Captcha { get; set; }
        public string OrderId { get; set; }
    }
}