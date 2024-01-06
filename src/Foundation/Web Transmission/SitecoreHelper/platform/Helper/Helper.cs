using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Sanitization;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;



namespace Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper
{
    public class Helper : IHelper
    {
        //public IEnumerable<Item> GetMultiListValueItems(this Item item, ID fieldId)
        //{
        //    return new MultilistField(item.Fields[fieldId]).GetItems();
        //}
        public string GetImageURL(Item item, string fieldName)
        {
            string imageURL = string.Empty;
            if (!String.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)item.Fields[fieldName]);
                if (imgField != null && imgField.MediaItem != null)
                {
                    imageURL = GetUrlDomain() + Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem);
                }


            }
            return imageURL;
        }
        public string GetUrlDomain()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                var uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri.ReplaceBlank());
                return string.Format("{0}://{1}", uri.Scheme, uri.Host);
            }
            return string.Empty;
        }
        public string GetImageAlt(Item item, string fieldName)
        {
            string imageAlt = string.Empty;
            if (!String.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                Sitecore.Data.Fields.ImageField imgField = ((ImageField)item.Fields[fieldName]);
                if (imgField.MediaItem != null)
                {
                    imageAlt = imgField.Alt;
                }
            }
            return imageAlt;
        }

        public string GetLinkURL(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            //LinkResolver linkResolver = new LinkResolver();

            return LinkUrl(linkField);
        }
        public string GetLinkTarget(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            //LinkResolver linkResolver = new LinkResolver();
            return linkField.Target;
        }

        public string GetProductItemUrl(Item Item)
        {
            string productURL = string.Empty;
            if (Item != null)
            {
                try
                {
                    productURL = Sitecore.Links.LinkManager.GetItemUrl(Item);
                }
                catch (Exception ex)
                {

                    productURL = string.Empty;
                }

            }
            return productURL;
        }

        public string GetImageUrlfromSitecore(string path)
        {
            Sitecore.Data.Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            Sitecore.Data.Items.Item sampleMedia = new Sitecore.Data.Items.MediaItem(webDB.GetItem(path));

            return Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(sampleMedia));
        }
        public string GetLinkText(Item item, string fieldName)
        {
            string linkText = string.Empty;
            LinkField linkField = item.Fields[fieldName];
            if (!String.IsNullOrEmpty(linkField.ToString()))
            {
                linkText = ((LinkField)item.Fields[fieldName]).Text;
            }
            return linkText;
        }
        public string GetImageURLByFieldId(Item item, string fieldId)
        {
            string imageURL = string.Empty;

            if (item != null && !string.IsNullOrEmpty(fieldId))
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
        public bool GetCheckboxOption(Field field)
        {
            CheckboxField checkboxField = field;
            if (checkboxField != null && checkboxField.Checked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetDropLinkValue(Field fieldName)
        {
            GroupedDroplinkField buttonVariant = fieldName;
            if (buttonVariant != null && buttonVariant.TargetItem != null)
            {
                return buttonVariant.TargetItem.Fields["name"].Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetDropListValue(Field fieldName)
        {
            GroupedDroplistField buttonVariant = fieldName;
            if (buttonVariant != null && buttonVariant.Value != null)
            {
                return buttonVariant.Value;
            }
            else
            {
                return string.Empty;
            }
        }

        public string Sanitize(string name)
        {
            return name.Replace(" ", "").Replace("&", "").Replace("'", "").Replace(".", "-");
        }

        public string GetImageURLbyField(Field field)
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
            return imageURL;
        }

        public string GetImageAltbyField(Field field)
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

        public string GetLinkURLbyField(Item item, Field field)
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
                        linkURL = GetUrlDomain() + LinkManager.GetItemUrl(linkField.TargetItem);
                        break;
                    case "media":
                        Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                        linkURL = Sitecore.StringUtil.EnsurePrefix('/', GetUrlDomain() + MediaManager.GetMediaUrl(media));
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

        public string GetLinkTextbyField(Item item, Field field)
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
                        linkURL = Sitecore.StringUtil.EnsurePrefix('/', GetUrlDomain() + MediaManager.GetMediaUrl(media));
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

        public string ToTitleCase(string title)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower());
        }

        public String LinkUrl(Sitecore.Data.Fields.LinkField lf)
        {
            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = false;
            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ?Sitecore.Links.LinkManager.GetItemUrl(lf.TargetItem,options) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? GetUrlDomain() + Sitecore.Resources.Media.MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
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

        public String LinkUrlText(Sitecore.Data.Fields.LinkField lf)
        {
            // Use LinkMananger for internal links, if link is not empty
            return lf.TargetItem != null ? lf.Text : string.Empty;

        }

        public String LinkUrlStyleclass(Sitecore.Data.Fields.LinkField lf)
        {
            // Use LinkMananger for internal links, if link is not empty
            return lf.TargetItem != null ? lf.Class : string.Empty;

        }

        public string GetImageURL(Sitecore.Data.Fields.ImageField imageField)
        {
            string imageURL = string.Empty;
            if (imageField != null && imageField.MediaItem != null)
            {
                Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
                imageURL = GetUrlDomain() + Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(image));


            }
            return imageURL;
        }

        public void LoadAssembly()
        {

            string binPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"\bin\libs");
            List<Assembly> allAssemblies = new List<Assembly>();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            try
            {
                foreach (string dll in Directory.GetFiles(binPath, "*.dll"))
                    allAssemblies.Add(Assembly.LoadFile(dll));
            }
            catch (Exception ex)
            {
            }
        }
    }
}