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
    public class PromotionSEOFriendlyPath : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null && !item.TemplateID.Equals(Templates.OfferCollection.PromoOfferTemplateID))
            {
                return null;
            }

            string SeofriendlyPath = string.Empty;
            if (!string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionOfferUniqueFieldID].Value))
            {
                SeofriendlyPath = item.Parent.Name + "/" + (!string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionOfferUniqueFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionOfferUniqueFieldID].Value :
                                string.Empty);
            }
            return SeofriendlyPath.ToLower();
        }
    }
}