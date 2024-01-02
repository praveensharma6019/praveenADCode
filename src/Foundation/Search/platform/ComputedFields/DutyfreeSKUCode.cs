using System;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class DutyfreeSKUCode : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }

        public object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            if (!item.TemplateID.Equals(Templates.DFCollection.ProductTemplateID))
            {
                return null;
            }

            string skuCode = item.Name;

            return (string.IsNullOrEmpty(skuCode)) ? string.Empty : skuCode;
        }
    }
}