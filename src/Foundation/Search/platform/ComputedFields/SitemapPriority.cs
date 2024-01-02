using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.InsertRenderings.Processors;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class SitemapPriority : IComputedIndexField
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

            if (!item.TemplateID.Equals(Templates.SitemapCollection.SeoPageTemplateID) && !item.TemplateID.Equals(Templates.SitemapCollection.SeolandingPageTemplateID))
            {
              //  Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }           

           
            var PriorityField = item.Fields["Priority"].Value;           
            string Priorityvalue=string.Empty;
            Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
            Sitecore.Data.Items.Item priorityItem = db.GetItem(PriorityField);
            if (priorityItem != null && priorityItem.Fields != null && priorityItem.Fields["Value"] != null)
            {
                Priorityvalue = float.Parse(priorityItem.Fields["Value"].Value).ToString("0.0");
            }

            return Priorityvalue;
        }
    }
}