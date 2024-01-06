namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
  
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;

    
    public class GreenPowerOptInModels
    {

        public string Id { get; set; }
        public string AccountNumber { get; set; }

      
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }

        public string FacebookId { get; set; }
        public string TwitterId { get; set; }

        public bool IsIPledge { get; set; }
        public bool OptInFlagCurrentOrNextBilling { get; set; }
        public bool IsPicCaptured { get; set; }
        public string ImageName { get; set; }
        public string imageLinkContentType { get; set; }
        public string ImageLink { get; set; }
        public string OptInBillingFrom { get; set; }
        public string PercentageOptIn { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }



    }

}