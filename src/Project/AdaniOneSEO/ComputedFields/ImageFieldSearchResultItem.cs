using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using static Project.AdaniOneSEO.Website.BaseTemplates;

namespace Project.AdaniOneSEO.Website.ComputedFields
{
    public class ImageFieldSearchResultItem : IComputedIndexField
    {
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
        public object ComputeFieldValue(IIndexable indexable)
        {
            //var indexableItem = (SitecoreIndexableItem)indexable;
            var indexableItem = (Item)(indexable as SitecoreIndexableItem);
            if (indexableItem == null || indexableItem.TemplateName != "Widget")
            {
                return null;
            }
            ImageField img = indexableItem.Fields[BannerWidgetTemplate.Image];
            var ImageItemUrl = "";
            //var field = (Sitecore.Data.Fields.ImageField)indexableItem.Fields["Image"];

            if (img != null && img.MediaItem != null)
            {
                ImageItemUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(img.MediaItem);
            }
            return ImageItemUrl.Replace("/sitecore/shell/", "/");
        }
    }
}