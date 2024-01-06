using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class DownloadViewBill : ProfileBasicInfo
    {
        public List<InvoiceRecord> InvoiceLines { get; set; }
        public string selectedMonth { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(EditProfile))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
    }

    public class InvoiceRecord
    {
        public string AccountNumber { get; set; }
        public string BillMonth { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceUrl { get; set; } //=> $"https://iss.adanielectricity.com/VAS/ProcessDownloadPDF.jsp?TXTCANO={AccountNumber}&INVOICENO={InvoiceNumber}";
    }
}