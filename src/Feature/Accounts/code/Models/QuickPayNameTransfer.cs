using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class QuickPayNameTransfer
    {
        public string BuilderCaseNameTransferFees { get; set; }
        public string ResaleNameTransferFees { get; set; }
        public string DemiseNameTransferFees { get; set; }

        public bool IsCustomerIdURLRendered { get; set; }
        public string LoginName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PartnerNo { get; set; }
        public string OrderId { get; set; }
        public bool TermsConditions { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(QuickPayNameTransfer))]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Only Numbers allowed")]
        public double Amount { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(QuickPayNameTransfer))]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Only Numbers allowed")]
        public double AdvanceAmount { get; set; }

        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Captcha { get; set; }
        public string ResponseStatus { get; set; }
        public string TransactionId { get; set; }
        public string Responsecode { get; set; }
        public string Remark { get; set; }
        public string PaymentRef { get; set; }
        public string PaymentMode { get; set; }
        public string PaymentType { get; set; }
        public string UserType { get; set; }
        public string TransactionDate { get; set; }
        public int PaymentGateway { get; set; }

        public string Bill_No { get; set; }
        public string Bill_Date { get; set; }
        public string Due_Date { get; set; }
        public string Current_Outstanding_Amount { get; set; }

        public string Execution_time { get; set; }
        public string Partner_Type { get; set; }
        public string Message { get; set; }
        public string CustomerType { get; set; }
        public bool IsError { get; set; }
        public string MessageFlag { get; set; }
        public string IsS2S { get; set; }

        public string MaxLengthgas { get; set; }
    }
}