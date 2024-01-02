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

namespace Adani.EV.Project.Helper
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
                    imageURL = GetSitecoreDomain() + MediaManager.GetMediaUrl(imgField.MediaItem);
                }
            }
            if (imageURL == GetSitecoreDomain())
            {
                imageURL = "";
            }
            return imageURL;
        }

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
                    imageURL = GetUrlDomain() + MediaManager.GetMediaUrl(imgField.MediaItem);
                }
            }
            return imageURL;
        }
        public static string GetSitecoreDomain()
        {
            string strSitedomain = string.Empty;

            var commonItem = Sitecore.Context.Database.GetItem("Commondata.ItemID");
            strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            return strSitedomain;
        }
        public static string GetUrlDomain()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                if (Uri.IsWellFormedUriString(HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute))
                {
                    var uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
                    return string.Format("{0}://{1}", uri.Scheme, uri.Host);
                }

            }
            return string.Empty;
        }

        public static string GetImageURLbyField(Field field)
        {
            string imageURL = string.Empty;
            if (field != null)
            {
                Sitecore.Data.Fields.ImageField imgField = field;
                if (imgField?.MediaItem != null)
                {
                    imageURL = GetSitecoreDomain() + MediaManager.GetMediaUrl(imgField.MediaItem);
                }
            }
            if (imageURL == GetSitecoreDomain())
            {
                imageURL = "";
            }
            return imageURL;
        }
        public static string GetRelativeImageURLbyField(Field field)
        {
            string imageURL = string.Empty;
            if (field != null)
            {
                Sitecore.Data.Fields.ImageField imgField = field;
                if (imgField?.MediaItem != null)
                {
                    imageURL = GetUrlDomain() + MediaManager.GetMediaUrl(imgField.MediaItem);
                }
            }
            if (imageURL == GetSitecoreDomain())
            {
                imageURL = "";
            }
            return imageURL;
        }

        public static string GetImageAltbyField(Field field)
        {
            string imageAlt = string.Empty;
            if (field != null)
            {
                ImageField imgField = field;
                if (imgField?.MediaItem != null)
                {
                    imageAlt = imgField.Alt;
                }
            }
            return imageAlt;
        }
        public static Item GetImageDetails(Item item, string fieldName, Item imageDetails = null)
        {
            if (!String.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)item.Fields[fieldName]);
                if (imgField.MediaItem != null)
                {
                    imageDetails = MediaManager.GetMedia(imgField.MediaItem).MediaData.MediaItem;
                }
            }
            return imageDetails;
        }
       
        public static Item GetLinkDetails(Item item, string fieldName, Item media = null)
        {
            if (!String.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                LinkField linkField = item.Fields[fieldName];

                if (linkField.LinkType != null && linkField.TargetItem != null)
                {
                    media = new MediaItem(linkField.TargetItem);
                }
            }
            return media;
        }

        public static string GetImageSource(Item item, string fieldName, Item imageDetails = null)
        {
            if (!String.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)item.Fields[fieldName]);
                if (imgField.MediaItem != null)
                {
                    imageDetails = Sitecore.Resources.Media.MediaManager.GetMedia(imgField.MediaItem).MediaData.MediaItem;
                }
            }
            string src = "";
            if (imageDetails != null)
            {
                src = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageDetails);
            }
            src = GetSitecoreDomain() + src;
            if (src == GetSitecoreDomain())
            {
                src = "";
            }
            return src;
        }
        //imageDetails = Sitecore.Resources.Media.MediaManager.GetMedia(imgField.MediaItem).MediaData.MediaItem
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
        public static string GetMedialLinkURL(Item item, Field field)
        {
            string linkURL = string.Empty;
            Sitecore.Data.Fields.LinkField linkField = field;
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
                        linkURL = GetSitecoreDomain() + Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media));
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                        break;
                }
            }
            return linkURL;
        }
        /*
         * Only for GalleryHighlightsContentResolver
         */
        public static string GetPropLinkURLbyField(Item item, Field field)
        {
            string linkURL = string.Empty;
            try
            {
                LinkField linkField = field;
                if (!String.IsNullOrEmpty(linkField.ToString()))
                {
                    switch (linkField.LinkType)
                    {
                        case "internal":
                        case "mailto":
                        case "anchor":
                        case "javascript":
                            linkURL = linkField.TargetItem != null ? GetUrlDomain() + LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                            break;
                        case "external":
                            linkURL = linkField.Url;
                            break;
                        case "media":
                            Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                            linkURL = GetSitecoreDomain() + MediaManager.GetMediaUrl(media);// Sitecore.StringUtil.EnsurePrefix("", GetUrlDomain() + MediaManager.GetMediaUrl(media));
                            break;
                        case "":
                            break;
                        default:
                            //logger
                            string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, new object());
            }

            return linkURL;
        }
        public static string GetLinkURLbyField(Item item, Field field)
        {
            string linkURL = string.Empty;

            bool flag = true;
            string strSitedomain = string.Empty;

            var commonItem = Sitecore.Context.Database.GetItem("Commondata.ItemID");
            strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;
            try
            {
                LinkField linkField = field;
                if (!String.IsNullOrEmpty(linkField.ToString()))
                {
                    switch (linkField.LinkType)
                    {
                        case "internal":
                            break;
                        case "mailto":
                            break;
                        case "anchor":
                            break;
                        case "javascript":
                            linkURL = linkField.TargetItem != null ? GetUrlDomain() + LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                            flag = false;
                            break;
                        case "external":
                            linkURL = linkField.Url;
                            break;
                        case "media":
                            Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                            linkURL = MediaManager.GetMediaUrl(media);// Sitecore.StringUtil.EnsurePrefix("", GetUrlDomain() + MediaManager.GetMediaUrl(media));
                            flag = false;
                            break;
                        case "":
                            break;
                        default:
                            //logger
                            string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                            break;
                    }

                    if (((Sitecore.Data.Fields.LinkField)field).Url.Contains("http"))
                    {
                        linkURL = ((Sitecore.Data.Fields.LinkField)field).Url.ToString();
                    }
                    else if (((Sitecore.Data.Fields.LinkField)field).Url.Contains("#"))
                    {
                        linkURL = ((Sitecore.Data.Fields.LinkField)field).Url.ToString();
                    }
                    else if (((Sitecore.Data.Fields.LinkField)field).Url.Contains("tel:"))
                    {
                        linkURL = ((Sitecore.Data.Fields.LinkField)field).Url.ToString();
                    }
                    else if (((Sitecore.Data.Fields.LinkField)field).Url.Contains("mailto:"))
                    {
                        linkURL = ((Sitecore.Data.Fields.LinkField)field).Url.ToString();
                    }
                    else if (((Sitecore.Data.Fields.LinkField)field).Url == null || ((Sitecore.Data.Fields.LinkField)field).Url == "")
                    {
                        linkURL = ((Sitecore.Data.Fields.LinkField)field).Url.ToString();
                    }
                    else
                    {
                        linkURL = flag ? strSitedomain + linkURL : linkURL;
                        //linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url;
                    }

                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, new object());
            }

            return linkURL;
        }

        public static string GetLinkURL(Sitecore.Data.Fields.LinkField lf)
        {
            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = false;

            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? LinkManager.GetItemUrl(lf.TargetItem, options) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    string URL = string.Empty;
                    if ((!String.IsNullOrEmpty(lf.Url)) && lf.Url != "" && lf.Url != null)
                    {
                        if (lf.Url.Contains("https://") || lf.Url.Contains("http://"))
                        {
                            URL = lf.Url;
                        }
                        else
                        {
                            URL = Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + lf.Url;
                        }
                    }
                    return URL;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    // return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                    return !string.IsNullOrEmpty(lf.Anchor) ? lf.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return lf.Url;
                case "javascript":
                    // Just return javascript
                    return lf.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return lf.Url;
            }
        }


        public static string GetLinkTargetID(Item item, string field)
        {
            string value = string.Empty;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    var fieldName = item.Fields[field];
                    if (fieldName != null && fieldName.Type == "General Link")
                    {
                        Sitecore.Data.Fields.LinkField link = fieldName;
                        value = link.TargetID.ToString();
                    }
                }
            }
            return value;
        }
        public static string GetLinkTargetwithID(Item item, ID id)
        {
            string value = string.Empty;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    var fieldName = item.Fields[id];
                    if (fieldName != null && fieldName.Type == "General Link")
                    {
                        Sitecore.Data.Fields.LinkField link = fieldName;
                        value = link.TargetID.ToString();
                    }
                }
            }
            return value;
        }

        public static string GetLinkTextbyField(Item item, Field field)
        {
            string linkURL = string.Empty;
            LinkField linkField = field;
            if (!String.IsNullOrEmpty(linkField.ToString()))
            {
                switch (linkField.LinkType)
                {
                    case "internal":
                    case "external":
                    case "mailto":
                    case "anchor":
                    case "javascript":
                        linkURL = linkField.Text;
                        break;
                    case "media":
                        Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                        linkURL = Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media));
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = String.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                        break;
                }
            }
            return linkURL;
        }

        public static string GetLinkDescription(Item item, string fieldName)
        {
            string value = string.Empty;
            if (item != null)
            {
                if (!string.IsNullOrEmpty(fieldName))
                {
                    var field = item.Fields[fieldName];
                    if (field != null && field.Type == "General Link")
                    {
                        Sitecore.Data.Fields.LinkField link = field;
                        value = link.Text;
                    }
                }
            }
            return value;
        }
        public static string GetLinkDescriptionByField(Field field)
        {
            string value = string.Empty;
            if (field != null)
            {
                if (field != null && field.Type == "General Link")
                {
                    LinkField link = field;
                    value = link.Text;
                }
            }
            return value;
        }
        public static string GetLinkStyle(Field field)
        {
            string value = string.Empty;
            if (field != null)
            {
                if (field != null && field.Type == "General Link")
                {
                    LinkField link = field;
                    value = link.Class;
                }
            }
            return value;
        }

       

        public static string GetImageUrlfromSitecore(string path)
        {
            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            Sitecore.Data.Items.Item sampleMedia = new MediaItem(webDB.GetItem(path));
            var domain = GetSitecoreDomain();
            return domain + Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(sampleMedia));
        }
        public static IEnumerable<Item> GetMultiListValueItem(this Item item, ID fieldID)
        {
            return new MultilistField(item.Fields[fieldID]).GetItems();
        }
        public static string GetSelectedItemFromDroplistField(Item item, ID fieldID)
        {
            Field field = item.Fields[fieldID];
            if (field == null || string.IsNullOrEmpty(field.Value))
            {
                return null;
            }
            return field.Value;
        }
        public static string GetSelectedItemFromDroplistFieldValue(Item item, ID fieldID)
        {
            Sitecore.Data.Fields.ReferenceField referenceField = item.Fields[fieldID];
            if (referenceField == null || string.IsNullOrEmpty(referenceField.Value))
            {
                return null;
            }
            return referenceField.TargetItem.Fields["Text"].Value;
        }
        public static bool GetCheckBoxSelection(Field field)
        {
            bool isChecked = false;
            if (field != null)
            {
                CheckboxField checkboxField = field;
                isChecked = checkboxField.Checked;
            }
            return isChecked;
        }
        public static string GetLinkURLTargetSpace(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            if (linkField != null && !String.IsNullOrEmpty(linkField.ToString()))
            {
                linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Target;
            }
            return linkURL;
        }
    }
}