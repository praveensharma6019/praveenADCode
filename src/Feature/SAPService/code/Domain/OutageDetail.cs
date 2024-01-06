using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SapPiService.Domain
{
    public class OutageDetail
    {
        public string Message { get; set; }

        public List<OutageRecord> CurrentOutageDetails { get; set; }
        public List<OutageRecord> FutureOutageDetails { get; set; }

        public string Captcha { get; set; }
        public string Amount { get; set; }

        [Display(Name = nameof(AccountNoCaption), ResourceType = typeof(OutageDetail))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(OutageDetail))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(OutageDetail))]
        public string AccountNumber { get; set; }

        public static string Required => "Please enter a value for {0}";
        public static string AccountNoCaption => "Account No.";
        public static string InvalidAccountNumber => "Please enter a valid Account Number";
    }
}