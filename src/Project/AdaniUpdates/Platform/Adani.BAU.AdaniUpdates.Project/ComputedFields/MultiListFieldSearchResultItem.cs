using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls.Presentation;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Adani.BAU.AdaniUpdates.Project.ComputedFields
{
    public class MultiListFieldSearchResultItem : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (SitecoreIndexableItem)indexable;
            if (indexableItem?.Item == null || indexableItem.Item.TemplateName != "MediaRelease")
            {
                return null;
            }

            var multilistField = (MultilistField)indexableItem.Item.Fields["Category"];
            if (multilistField != null)
            {
                var itemCategories = multilistField.GetItems();
                return itemCategories.Select(x => x.Fields["Key"]?.Value);
            }

            return null;
        }

        public string FieldName { get; set; }

        public string ReturnType { get; set; }
    }
}