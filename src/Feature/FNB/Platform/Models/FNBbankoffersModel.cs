using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Models
{
    public class FNBbankoffersModel
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public string DisplayID { get; set; }
        public string Information { get; set; }
        public string CTAlink { get; set; }
        public string ExpiryDate { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsApply { get; set; }
        public string PotentialEarnMessage { get; set; }
        public List<TermsAndConditionItem> TermsNConditions { get; set; }
      
    }
     public class TermsAndConditionItem
    {
        public string Text { get; set; }
    }
}