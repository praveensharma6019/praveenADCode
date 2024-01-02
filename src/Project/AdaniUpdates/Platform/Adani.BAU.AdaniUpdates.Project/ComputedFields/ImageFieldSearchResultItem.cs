using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls.Presentation;
using System.Globalization;

namespace Adani.BAU.AdaniUpdates.Project.ComputedFields
{
    public class ImageFieldSearchResultItem : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = (SitecoreIndexableItem)indexable;
            if (indexableItem?.Item == null || indexableItem.Item.TemplateName != "MediaRelease")
            {
                return null;
            }
            var ImageItemUrl = "";
            var field = (Sitecore.Data.Fields.ImageField)indexableItem.Item.Fields["image"];
            if (field != null && field.MediaItem != null)
            {
                ImageItemUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(field.MediaItem);
            }
            return  ImageItemUrl.Replace("/sitecore/shell/","/");
        }

        public string FieldName { get; set; }

        public string ReturnType { get; set; }
    }
}