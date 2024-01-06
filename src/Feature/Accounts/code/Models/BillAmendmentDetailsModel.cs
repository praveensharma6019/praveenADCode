namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class BillAmendmentDetailsModel
    {
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Entered Account no is invalid. Please enter a valid 9 digit Account No.");

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(BillAmendmentDetailsModel))]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(BillAmendmentDetailsModel))]
        public string AccountNo { get; set; }

        public string Language { get; set; }
        public string Captcha { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }

}