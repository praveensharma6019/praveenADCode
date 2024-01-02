using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Foundation.SitecoreHelper.Helper
{
    public static class Helper
    {
        public static string GetImageURL(Item item, string fieldName)
        {
            string imageURL = string.Empty;
            if (!String.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)item.Fields[fieldName]);
                if (imgField.MediaItem != null)
                {
                    imageURL = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem);
                }
            }
            return imageURL;
        }

        public static string GetLinkURL(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            if (!String.IsNullOrEmpty(linkField.ToString()))
            {
                switch (linkField.LinkType)
                {
                    case "internal":
                    case "external":
                    case "mailto":
                    case "anchor":
                    case "javascript":
                        linkURL = linkField.Url;
                        break;
                    case "media":
                        Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                        linkURL = Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(media));
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);                         
                        break;
                }
                linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url;
            }
            return linkURL;
        }

        public static string GetProductItemUrl(Item Item)
        {
            string productURL = string.Empty;
            if (Item!=null)
            {
                try
                {
                    productURL = Sitecore.Links.LinkManager.GetItemUrl(Item);
                }
                catch (Exception ex)
                {

                    productURL=string.Empty;
                }
                
            }
            return productURL;
        }

        public static string GetImageUrlfromSitecore(string path)
        {
            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            Sitecore.Data.Items.Item sampleMedia = new Sitecore.Data.Items.MediaItem(webDB.GetItem(path));

            return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(sampleMedia));
        }
    }
}