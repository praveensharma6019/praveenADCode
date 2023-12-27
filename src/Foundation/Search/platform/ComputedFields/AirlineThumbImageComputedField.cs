using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using System.Linq;

namespace Adani.SuperApp.Airport.Foundation.Search.Platform.ComputedFields
{
    public class AirlineThumbImageComputedField : IComputedIndexField
    {
        //public string Parameters { get; set; }
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        //string image;

        public object ComputeFieldValue(IIndexable indexable)
        {

            //Sitecore.Diagnostics.Log.Info("Logo Computed field entry started : " + System.DateTime.Now.ToString() , "computed field");
            Assert.ArgumentNotNull(indexable, "indexable");
            var item = (Item)(indexable as SitecoreIndexableItem);

            if (item == null)
            {
                Log.Warn(string.Format("{0} : unsupported IIndexable type : {1}", this, indexable.GetType()), this);
                return null;
            }

            if (!item.TemplateID.Equals(Templates.AirlineCollection.AirlineTemplateID))
            {
                Log.Warn(string.Format("{0} : unsupported Template type : {1}", this, indexable.GetType()), this);
                return null;
            }

            ImageField img = item.Fields[Templates.AirlineCollection.ThumbnailImageField];

            return (img == null || img.MediaItem == null) ? null : MediaManager.GetMediaUrl(img.MediaItem);
        }
    }
}