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
    public class BlogDetailTitleComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, "indexable");

            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                return null;
            }

            if (!item.TemplateID.Equals(Templates.BlogCollection.BlogDetailTemplateID))
            {
                return null;
            }
            
           
            string blogDetailTitle = !string.IsNullOrEmpty(item.Fields[Templates.BlogCollection.BlogDetailTitleFieldID].Value) ?
                                item.Fields[Templates.BlogCollection.BlogDetailTitleFieldID].Value :
                                string.Empty;
            return blogDetailTitle;
        }
    }
}