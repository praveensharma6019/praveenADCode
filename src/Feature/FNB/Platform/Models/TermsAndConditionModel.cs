using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.Models
{
    public class TermsAndConditionModel
    {
       
        public string Title { get; set; }
        public string OfferTitle { get; set; }

        public string Image { get; set; }
        public string LicenseText { get; set; }
        public string LicenseCode { get; set; }
        public List<TermsAndConditionItems> WebDescription { get; set; }
     
    }

    public class TermsAndConditionItems {

        public string list { get; set; }

    }




}