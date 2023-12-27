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
    public class PromotionOfferLocationTypeComputedField: AbstractComputedIndexField
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


            string PromotionCode = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.PromotionLocationTypeImageFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.PromotionLocationTypeImageFieldID].Value :
                                string.Empty;
            return PromotionCode.ToLower();
        }
    }
}