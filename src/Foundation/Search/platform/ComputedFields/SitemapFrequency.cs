using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.InsertRenderings.Processors;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class SitemapFrequency : IComputedIndexField
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


            var frequencyField = item.Fields["ChangeFrequency"].Value;
            string frequencyvalue = string.Empty;
            Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
            Sitecore.Data.Items.Item frequencyItem = db.GetItem(frequencyField);
            if (frequencyItem != null && frequencyItem.Fields != null && frequencyItem.Fields["Value"] != null)
            {
                frequencyvalue = frequencyItem.Fields["Value"].Value.ToString();
            }

            return frequencyvalue;
        }
    }
}