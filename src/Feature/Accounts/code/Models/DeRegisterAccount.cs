namespace Sitecore.Feature.Accounts.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class DeRegisterAccount : ProfileBasicInfo 
    {
        public List<KeyValuePair<string, string>> AccountList { get; set; }
        public string CurrentAccountNumber { get; set; }
        public string CurrentAccountItemID { get; set; }
        public string CurrentAccountName { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }
}