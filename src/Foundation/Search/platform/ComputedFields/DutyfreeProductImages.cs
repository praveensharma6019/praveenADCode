using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class DutyfreeProductImages : IComputedIndexField
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
              //  Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }
            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            List<string> imagePath = new List<string>();
            string imageSitecorePath = string.Empty;
            string[] ImageFields = { "Image 1", "Image 2", "Image 3", "Image 4", "Image 5", "Image 6" };

            foreach(string image in ImageFields)
            {
                imageSitecorePath = !string.IsNullOrEmpty(item.Fields[image].Value) ? item.Fields[image].Value : string.Empty;

                if (!imageSitecorePath.Equals(string.Empty))
                {
                    Sitecore.Data.Items.Item sampleMedia = new Sitecore.Data.Items.MediaItem(webDB.GetItem(imageSitecorePath));

                    imagePath.Add(Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(sampleMedia)));
                }
            }

            return string.Join(",", imagePath.ToArray());
        }
    }
}