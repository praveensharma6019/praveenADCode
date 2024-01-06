using Sitecore;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


namespace Sitecore.AdaniHousing.Website.Helper
{
    public static class Helper
    {
        public static string ImageUrl(ImageField imageField)
        {
            object mediaItem;
            string str = "";
            if (imageField != null)
            {
                mediaItem = imageField.MediaItem;
            }
            else
            {
                mediaItem = null;
            }
            if (mediaItem != null)
            {
                MediaItem mediaItem1 = new MediaItem(imageField.MediaItem);
                str = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(mediaItem1));
            }
            return str;
        }

        public static string ImageUrl(string imageFieldName, Item item)
        {
            string str;
            if (item == null)
            {
                str = "";
            }
            else
            {
                ImageField imageField = item.Fields[imageFieldName];
                str = (imageField != null ? Helper.ImageUrl(imageField) : "");
            }
            return str;
        }

        public static string ImageTagUrl(ImageField imageField)
        {
            string str = "";
            if (imageField != null && imageField.MediaItem != null)
            {
                Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);

                string src = Sitecore.StringUtil.EnsurePrefix('/',

                Sitecore.Resources.Media.MediaManager.GetMediaUrl(image));

                str = String.Format(@"<img srcset="""" src=""{0}"" alt=""{1}"" />", src, image.Alt);
            }
            return str;
        }

        public static string ImageTagUrl(string imageFieldName, Item item)
        {
            string str;
            if (item == null)
            {
                str = "";
            }
            else
            {
                ImageField imageField = item.Fields[imageFieldName];
                str = (imageField != null ? Helper.ImageTagUrl(imageField) : "");
            }
            return str;
        }

        public static string LinkUrl(this LinkField lf)
        {
            string url;
            if (lf == null)
            {
                url = "";
            }
            else
            {
                string lower = lf.LinkType.ToLower();
                if (lower == "internal")
                {
                    url = (lf.TargetItem != null ? LinkManager.GetItemUrl(lf.TargetItem) : string.Empty);
                }
                else if (lower == "media")
                {
                    url = (lf.TargetItem != null ? MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty);
                }
                else if (lower == "external")
                {
                    url = lf.Url;
                }
                else if (lower == "anchor")
                {
                    url = (!string.IsNullOrEmpty(lf.Anchor) ? lf.Anchor : string.Empty);
                }
                else if (lower == "mailto")
                {
                    url = lf.Url;
                }
                else
                {
                    url = (lower == "javascript" ? lf.Url : lf.Url);
                }
            }
            return url;
        }

        public static string RemoveHtml(string strSource)
        {
            return (!string.IsNullOrEmpty(strSource) ? Regex.Replace(strSource, "<(.|\n)*?>", "").Replace("&nbsp;", string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Trim() : string.Empty);
        }
    }
}