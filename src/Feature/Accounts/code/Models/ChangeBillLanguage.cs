namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;
    public class ChangeBillLanguage : ProfileBasicInfo
    {
        [Required(ErrorMessage = "Bill Language Required")]
        [Display(Name = "Bill Language:")]
        public string BillLanguageSelected { get; set; }

        public List<string> BillLanguageList { get; set; }

        //[Required(ErrorMessageResourceName = nameof(CaptchaRequired), ErrorMessageResourceType = typeof(ChangeBillLanguage))]
        public string Captcha { get; set; }
        public static string CaptchaRequired => DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Please validate captcha to continue");
    }
}