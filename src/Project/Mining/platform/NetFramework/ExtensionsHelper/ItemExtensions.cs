using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Xml.Xsl;

namespace Project.Mining.Website.ExtensionsHelper
{
    public static class ItemExtensions
    {
        public static string GetImageURLByFieldId(Item item, ID fieldId)
        {
            string imageURL = string.Empty;

            if (item != null)
            {
                ImageField imgField = item.Fields[fieldId];
                if (imgField != null && imgField.MediaItem != null)
                {
                    //temporary fix for full path
                    //TODO: Change into MedisAlwaysIncludeServerUrl as true in sitecore config
                    imageURL =  MediaManager.GetMediaUrl(imgField.MediaItem);
                }
            }
            return imageURL;
        }

        public static string GetLinkURL(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            bool flag = true;
            string strSitedomain = string.Empty;

            var commonItem = Sitecore.Context.Database.GetItem("Commondata.ItemID");
            strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;


            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            if (!String.IsNullOrEmpty(linkField.ToString()))
            {
                switch (linkField.LinkType)
                {
                    case "internal":
                        break;
                    case "external":
                        break;
                    case "mailto":
                        break;
                    case "anchor":
                        break;
                    case "javascript":
                        linkURL = linkField.Url;
                        flag = false;
                        break;
                    case "media":
                        Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                        linkURL = GetSitecoreDomain() + Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media));
                        flag = false;
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                        break;
                }

                if (((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.Contains("http"))
                {
                    linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else if (((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.Contains("#"))
                {
                    linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else if (((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.Contains("tel:"))
                {
                    linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else if (((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.Contains("mailto:"))
                {
                    linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else
                {
                    linkURL = flag ? strSitedomain + ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url : ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url;
                    //linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url;
                }

            }
            return linkURL;
        }

        public static string GetLinkURLTarget(Item item, string fieldName)
        {
            string linkType = string.Empty;
            bool flag = true;
            string strSitedomain = string.Empty;

            var commonItem = Sitecore.Context.Database.GetItem("Commondata.ItemID");
            strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;


            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            if (!String.IsNullOrEmpty(linkField.ToString()))
            {
                switch (linkField.LinkType)
                {
                    case "internal":
                        linkType = "_self";
                        break;
                    case "external":
                        linkType = "_blank";
                        break;
                    case "mailto":
                        linkType = "_blank";
                        break;
                    case "anchor":
                        linkType = linkField.Target;
                        break;
                    case "javascript":
                        linkType = linkField.Target;
                        flag = false;
                        break;
                    case "media":
                       // Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                        linkType = linkField.Target;
                        flag = false;
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                        break;
                }

            }
            return linkType;
        }
        public static string GetSitecoreDomain()
        {
            string strSitedomain = string.Empty;

            var commonItem = Sitecore.Context.Database.GetItem("Commondata.ItemID");
            strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            return strSitedomain;
        }
    }
}