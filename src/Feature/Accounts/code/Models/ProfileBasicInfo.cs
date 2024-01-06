namespace Sitecore.Feature.Accounts.Models
{
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;

    public class ProfileBasicInfo 
    {
        public string LoginName { get; set; }
        public string AccountNumber { get; set; }
        public string MasterAccountNumber { get; set; }

    }
}