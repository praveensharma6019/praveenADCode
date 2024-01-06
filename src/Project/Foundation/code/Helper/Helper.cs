using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using System.Text.RegularExpressions;

namespace Sitecore.Foundation.Website.Helper
{
    public static class Helper
    {
        public static string LinkUrl(this LinkField lf)
        {
            if (lf == null)
                return "";
            string lower = lf.LinkType.ToLower();
            if (lower == "internal")
                return lf.TargetItem != null ? LinkManager.GetItemUrl(lf.TargetItem) : string.Empty;
            if (lower == "media")
                return lf.TargetItem != null ? MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
            if (lower == "external")
                return lf.Url;
            if (lower == "anchor")
                return !string.IsNullOrEmpty(lf.Anchor) ? lf.Anchor : string.Empty;
            return lower == "mailto" || lower == "javascript" ? lf.Url : lf.Url;
        }

        public static string ImageUrl(ImageField imageField)
        {
            string str = "";
            if (imageField?.MediaItem != null)
                str = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(new MediaItem(imageField.MediaItem)));
            return str;
        }

        public static string ImageUrl(string imageFieldName, Item item)
        {
            if (item == null)
                return "";
            ImageField imageField = item.Fields[imageFieldName];
            return imageField != null ? Sitecore.Foundation.Website.Helper.Helper.ImageUrl(imageField) : "";
        }

        public static string RemoveHtml(string strSource) => !string.IsNullOrEmpty(strSource) ? Regex.Replace(strSource, "<(.|\n)*?>", "").Replace("&nbsp;", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Trim() : string.Empty;
    }
}
