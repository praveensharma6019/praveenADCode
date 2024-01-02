using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Collections.Specialized;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class ProductCategory : AbstractComputedIndexField
    {
        
        public override object ComputeFieldValue(IIndexable indexable)
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
                Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }

            GroupedDroplistField CategoryField = item.Fields[Templates.DFCollection.CategoryFieldId];

            GroupedDroplistField MaterialGroupField = item.Fields[Templates.DFCollection.MaterialGroupFieldId];

            GroupedDroplistField BrandField = item.Fields[Templates.DFCollection.BrandFieldId];

            SitecoreItemDataField SubCategoryField = item.Fields[Templates.DFCollection.SubCategoryFieldId];

            StringCollection categories = new StringCollection
            {
                MaterialGroupField?.Value?.ToLower(),
                CategoryField?.Value?.ToLower(),
                BrandField?.Value?.ToLower(),
                SubCategoryField?.Value?.ToString()?.ToLower()
            };
            string.Join(" ", categories);

            return categories;
        }
    }
}