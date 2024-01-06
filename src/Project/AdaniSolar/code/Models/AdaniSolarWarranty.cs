using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniSolar.Website.Models
{
    public class AdaniSolarWarranty : IValidatableObject
    {
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "401")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter Invoice Number")]
        public string AdaniInvoiceNumber { get; set; }

        [Required(ErrorMessage = "Please enter the invoice date")]
        public DateTime AdaniInvoiceDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Custom validation logic goes here
            if (string.IsNullOrEmpty(AdaniInvoiceNumber))
            {
                yield return new ValidationResult("Invoice Number is required", new[] { "AdaniInvoiceNumber" });
            }

            if (AdaniInvoiceDate == null)
            {
                yield return new ValidationResult("Invoice Date is required", new[] { "AdaniInvoiceDate" });
            }
        }

        //[Required(ErrorMessage = "Captcha is required.")]
        public string reResponse { get; set; }

        public string SectionType { get; set; }

    }

    public class Invoice
    {
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "401")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter Invoice Number")]
        public string Invoice_No { get; set; }

        [Required(ErrorMessage = "Please enter the invoice date")]
        public string Invoice_No_Date { get; set; }
    }

    public class AdaniSolarWarrantyInvoiceRoot
    {
        public Invoice Record { get; set; }
    }

    public class WarrantyCertificate
    {
        public CerData[] cerDatas { get; set; }
    }

    public class CerData
    {
        public string palletId { get; set; }
        public string moduleSerialNumber { get; set; }
        public string performanceWarrantyTill { get; set; }
    }
    public class OtpData
    {
        public string customerId { get; set; }
        public string destinationAddress { get; set; }
        public string message { get; set; }
        public string sourceAddress { get; set; }
        public string messageType { get; set; }
        public string dltTemplateId { get; set; }
        public string entityId { get; set; }
    }

    public class MobileOtp
    {
        public OtpData OtpData { get; set; }
    }
}