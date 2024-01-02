using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.Sites;

namespace Adani.SuperApp.Realty.Foundation.Search.Platform.ComputedFields
{
    public class BlogImageComputedField : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            string imageUrl = string.Empty;
            try
            {
                Item item = indexable as SitecoreIndexableItem;
                if (item != null)
                {
                    ImageField imageField = item.Fields["Image"];
                    if (imageField != null && imageField.MediaItem != null)
                    {
                        MediaItem media = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
                        imageUrl = MediaManager.GetMediaUrl(media);
                        imageUrl = !string.IsNullOrWhiteSpace(imageUrl) && imageUrl.ToLower().Contains("sitecore/shell/") ? imageUrl.Replace("sitecore/shell/", "") : imageUrl;
                    }
                }
            }
            catch (Exception ex)
            {

               Log.Error("BlogImageComputedField : ", string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return imageUrl;
        }
        public string GetUrlDomain()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                var uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                return string.Format("{0}://{1}", uri.Scheme, uri.Host);
            }
            return string.Empty;
        }
    }

}