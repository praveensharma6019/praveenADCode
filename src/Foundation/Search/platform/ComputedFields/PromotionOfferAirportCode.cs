using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class PromotionOfferAirportCode : AbstractComputedIndexField
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

            Sitecore.Data.Fields.MultilistField storeTypeMultilist = item.Fields[Templates.OfferCollection.PromotionOfferLocationStoreTypeFieldID] != null ?
                                                                     item.Fields[Templates.OfferCollection.PromotionOfferLocationStoreTypeFieldID] : null;
            return getLocationCode(storeTypeMultilist);
        }

        private string getLocationCode(Sitecore.Data.Fields.MultilistField storeTypeMultilist)
        {
            string itemTitleField = string.Empty;
            if (storeTypeMultilist != null && storeTypeMultilist.GetItems() != null && storeTypeMultilist.GetItems().Count() > 0)
            {
                StringBuilder sbStoreType = new StringBuilder();
                foreach (var item in storeTypeMultilist.GetItems())
                {
                    if (sbStoreType.Length > 0)
                        sbStoreType.Append(", " + item.Fields[Templates.OfferCollection.PromotionOfferLocationStoreFieldID].Value);
                    else
                        sbStoreType.Append(item.Fields[Templates.OfferCollection.PromotionOfferLocationStoreFieldID].Value);
                }
                if(!string.IsNullOrEmpty(sbStoreType.ToString()))
                    itemTitleField = Convert.ToString(sbStoreType);
            }

            return itemTitleField.ToLower();
        }
    }
}