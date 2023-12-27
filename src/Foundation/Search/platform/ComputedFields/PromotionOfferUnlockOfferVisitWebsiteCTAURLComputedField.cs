using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class PromotionOfferUnlockOfferVisitWebsiteCTAURLComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                return null;
            }

            if (!item.TemplateID.Equals(Templates.OfferCollection.PromoOfferTemplateID))
            {
                return null;
            }


            Sitecore.Data.Fields.LinkField OfferUnlockOfferVisitWebsiteCTALink = item.Fields[Templates.OfferCollection.UnlockOfferCTAVisitWesiteLinkFieldID];

            return OfferUnlockOfferVisitWebsiteCTALink.Url;
        }
    }
}