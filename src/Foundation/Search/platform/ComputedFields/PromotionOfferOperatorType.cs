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
    public class PromotionOfferOperatorType : AbstractComputedIndexField
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

            Sitecore.Data.Fields.MultilistField OperatorTypeMultilist = item.Fields[Templates.OfferCollection.PromotionOfferOperatorTypeFieldID] != null ?
                                                                     item.Fields[Templates.OfferCollection.PromotionOfferOperatorTypeFieldID] : null;
            return getOperatorTypeCode(OperatorTypeMultilist);
        }

        private string getOperatorTypeCode(Sitecore.Data.Fields.MultilistField OperatorTypeMultilist)
        {
            string itemTitleField = string.Empty;
            if (OperatorTypeMultilist != null && OperatorTypeMultilist.GetItems()!=null && OperatorTypeMultilist.GetItems().Count() > 0)
            {
                StringBuilder sbOperatorType = new StringBuilder();
                foreach (var item in OperatorTypeMultilist.GetItems())
                {
                    if (sbOperatorType.Length > 0)
                        sbOperatorType.Append(", " + item.Fields[Templates.OfferCollection.PromotionOfferLocationStoreFieldID].Value);
                    else
                        sbOperatorType.Append(item.Fields[Templates.OfferCollection.PromotionOfferLocationStoreFieldID].Value);
                    
                }
                itemTitleField = sbOperatorType.ToString();
            }
            
            return itemTitleField.ToLower();
        }
    }
}