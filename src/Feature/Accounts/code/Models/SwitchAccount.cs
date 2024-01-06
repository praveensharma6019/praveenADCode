namespace Sitecore.Feature.Accounts.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class SwitchAccount : ProfileBasicInfo 
    {
        public List<KeyValuePair<string, string>> AccountList { get; set; }
        public List<SwitchAccountComponent> SwitchAccountList { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }

    public class SwitchAccountComponent
    {
        public string AccountItemId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
    }
}