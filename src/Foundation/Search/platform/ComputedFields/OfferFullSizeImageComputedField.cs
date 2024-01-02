using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class OfferFullSizeImageComputedField : AbstractComputedIndexField
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

            ImageField img = item.Fields[Templates.OfferCollection.PromotionOfferFullScreenImageFieldID];

            return (img == null || img.MediaItem == null) ? null : MediaManager.GetMediaUrl(img.MediaItem);
        }
    }
}
