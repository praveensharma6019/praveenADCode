namespace Sitecore.Electricity.Website.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ITSRLoginModel
    {
        public string userId { get; set; }
        public string UserRole { get; set; }
        public string SessionId { get; set; }
        public string UniqueId { get; set; }
    }


}