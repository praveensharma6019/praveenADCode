using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class ManageMobileAdaniGas
    {
        public string Message { get; set; }
        public bool IsError { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ManageMobileAdaniGas))]
        [RegularExpression(@"^(\d{10})$", ErrorMessageResourceName = nameof(InvalidMobileNumber), ErrorMessageResourceType = typeof(ManageMobileAdaniGas))]
        [DataType(DataType.PhoneNumber)]
        public string NewMobileNumber { set; get; }
        public List<MobileEntry> MobileNumbersList { set; get; }

        public ManageMobileAdaniGas()
        {
            IsError = false;
            MobileNumbersList = new List<MobileEntry>();
            Message = string.Empty;
            NewMobileNumber = string.Empty;
        }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string InvalidMobileNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile Number", "Please enter a valid Mobile Number");

    }

    public class MobileEntry
    {
        public string CustomerID { get; set; }
        public string MobileNo { get; set; }
        public string MessageFlag { get; set; }
        public string Message { get; set; }
        public bool Ischecked { get; set; }
    }
}