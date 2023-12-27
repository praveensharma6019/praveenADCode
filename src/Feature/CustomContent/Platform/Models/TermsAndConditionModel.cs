using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Pipelines.Rules.Taxonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models
{
    public class TermsAndConditionModel
    {
       
        public string Title { get; set; }
        public string OfferTitle { get; set; }
        public string SubTitle { get; set; }
        public string PromoCodeTitle { get; set; }
        public string PromoCodeValue { get; set; }
        public string Description { get; set; }
        public string SubDescription { get; set; }
        public string ValidityPeriod { get; set; }
        public string MinimumTransaction { get; set; }
        public List<TermsAndConditionItems> WebDescription { get; set; }
     
     
    }

    public class TermsAndConditionItems {

        public string list { get; set; }

    }




}