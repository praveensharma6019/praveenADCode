using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class PayVDS
    {
        public string AccountNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PayVDS))]
        [RegularExpression(@"^[0-9]*$", ErrorMessageResourceName = nameof(InvalidAmount), ErrorMessageResourceType = typeof(PayVDS))]
        public decimal PaymentAmount { get; set; }
        public decimal AverageVDSAmount { get; set; }
        public string PANNo { get; set; }

        public string PaymentDueDate { get; set; }
        public int CurrentOutstanding { get; set; }

        public string PaymentMode { get; set; }

        //[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessageResourceName = nameof(InvalidLoginName), ErrorMessageResourceType = typeof(PayVDS))]
        public string LoginName { get; set; }
        public decimal MaxVDSAmount { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PayVDS))]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessageResourceName = nameof(InvalidMobile), ErrorMessageResourceType = typeof(PayVDS))]
        public string MobileNumber { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PayVDS))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(PayVDS))]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        public string AmountPayable { get; set; }

        public static string InvalidLoginName => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Login Name", "Please enter a Valid Login Name");

        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

        public static string InvalidMobile => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");

        public static string InvalidAmount => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Amount");

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }
}