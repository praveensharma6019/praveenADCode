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
    public class LOBPromotion : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                return null;
            }

            if (!item.TemplateID.Equals(Templates.OfferCollection.PromoOfferTemplateID) && !item.TemplateID.Equals(Templates.OfferCollection.CartOfferTemplateID))
            {
                return null;
            }
            
           
            string LOBPromotion = !string.IsNullOrEmpty(item.Fields[Templates.OfferCollection.LOBPromotionOfferFieldID].Value) ?
                                item.Fields[Templates.OfferCollection.LOBPromotionOfferFieldID].Value :
                                string.Empty;
            return LOBPromotion.ToLower();
        }
    }
}