using Newtonsoft.Json.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Mvc.Presentation;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;

namespace Project.Mining.Website.Helpers
{
    public static class Utils
    {
        public static string GetImageURL(Item item, string fieldName)
        {
            string imageURL = string.Empty;
            if (!string.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                ImageField imgField = (ImageField)item.Fields[fieldName];
                if (imgField.MediaItem != null)
                {
                    imageURL = MediaManager.GetMediaUrl(imgField.MediaItem, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false });
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
                    imageURL = MediaManager.GetMediaUrl(imgField.MediaItem, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false });
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
                ImageField imgField = field;
                if (imgField?.MediaItem != null)
                {
                    imageURL = MediaManager.GetMediaUrl(imgField.MediaItem, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false });
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
                ImageField imgField = field;
                if (imgField?.MediaItem != null)
                {
                    imageURL = MediaManager.GetMediaUrl(imgField.MediaItem, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false });
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
            if (!string.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                ImageField imgField = (ImageField)item.Fields[fieldName];
                if (imgField.MediaItem != null)
                {
                    imageDetails = MediaManager.GetMedia(imgField.MediaItem).MediaData.MediaItem;
                }
            }
            return imageDetails;
        }
        public static Item GetLinkDetails(Item item, string fieldName, Item media = null)
        {
            if (!string.IsNullOrEmpty(item.Fields[fieldName].ToString()))
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
            if (!string.IsNullOrEmpty(item.Fields[fieldName].ToString()))
            {
                ImageField imgField = (ImageField)item.Fields[fieldName];
                if (imgField.MediaItem != null)
                {
                    imageDetails = MediaManager.GetMedia(imgField.MediaItem).MediaData.MediaItem;
                }
            }
            string src = "";
            if (imageDetails != null)
            {
                src = MediaManager.GetMediaUrl(imageDetails, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false });
            }
            if (src == GetSitecoreDomain())
            {
                src = "";
            }
            return src;
        }
        public static string GetLinkURL(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            bool flag = true;
            string strSitedomain = string.Empty;

            var commonItem = Sitecore.Context.Database.GetItem("Commondata.ItemID");
            strSitedomain = commonItem != null ? commonItem.Fields["Site Domain"].Value : string.Empty;


            LinkField linkField = item.Fields[fieldName];
            if (!string.IsNullOrEmpty(linkField.ToString()))
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
                        MediaItem media = new MediaItem(linkField.TargetItem);
                        linkURL = Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false }));
                        flag = false;
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = string.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                        break;
                }

                if (((LinkField)item.Fields[fieldName]).Url.Contains("http"))
                {
                    linkURL = ((LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else if (((LinkField)item.Fields[fieldName]).Url.Contains("#"))
                {
                    linkURL = ((LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else if (((LinkField)item.Fields[fieldName]).Url.Contains("tel:"))
                {
                    linkURL = ((LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else if (((LinkField)item.Fields[fieldName]).Url.Contains("mailto:"))
                {
                    linkURL = ((LinkField)item.Fields[fieldName]).Url.ToString();
                }
                else
                {
                    linkURL = flag ? strSitedomain + ((LinkField)item.Fields[fieldName]).Url : ((LinkField)item.Fields[fieldName]).Url;
                    //linkURL = ((Sitecore.Data.Fields.LinkField)item.Fields[fieldName]).Url;
                }

            }
            return linkURL;
        }
        public static string GetMedialLinkURL(Item item, Field field)
        {
            string linkURL = string.Empty;
            LinkField linkField = field;
            if (!string.IsNullOrEmpty(linkField.ToString()))
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
                        MediaItem media = new MediaItem(linkField.TargetItem);
                        linkURL = Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false }));
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = string.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                        break;
                }
            }
            return linkURL;
        }
        public static string GetPropLinkURLbyField(Item item, Field field)
        {
            string linkURL = string.Empty;
            try
            {
                LinkField linkField = field;
                if (!string.IsNullOrEmpty(linkField.ToString()))
                {
                    switch (linkField.LinkType)
                    {
                        case "internal":
                        case "mailto":
                        case "anchor":
                        case "javascript":
                            linkURL = linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                            break;
                        case "external":
                            linkURL = linkField.Url;
                            break;
                        case "media":
                            MediaItem media = new MediaItem(linkField.TargetItem);
                            linkURL = MediaManager.GetMediaUrl(media, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false });// Sitecore.StringUtil.EnsurePrefix("", GetUrlDomain() + MediaManager.GetMediaUrl(media));
                            break;
                        case "":
                            break;
                        default:
                            //logger
                            string message = string.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
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
                if (!string.IsNullOrEmpty(linkField.ToString()))
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
                            linkURL = linkField.TargetItem != null ? LinkManager.GetItemUrl(linkField.TargetItem) : string.Empty;
                            flag = false;
                            break;
                        case "external":
                            linkURL = linkField.Url;
                            break;
                        case "media":
                            Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                            linkURL = MediaManager.GetMediaUrl(media, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false });// Sitecore.StringUtil.EnsurePrefix("", GetUrlDomain() + MediaManager.GetMediaUrl(media));
                            flag = false;
                            break;
                        case "":
                            break;
                        default:
                            //logger
                            string message = string.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
                            break;
                    }

                    if (((LinkField)field).Url.Contains("http"))
                    {
                        linkURL = ((LinkField)field).Url.ToString();
                    }
                    else if (((LinkField)field).Url.Contains("#"))
                    {
                        linkURL = ((LinkField)field).Url.ToString();
                    }
                    else if (((LinkField)field).Url.Contains("tel:"))
                    {
                        linkURL = ((LinkField)field).Url.ToString();
                    }
                    else if (((LinkField)field).Url.Contains("mailto:"))
                    {
                        linkURL = ((LinkField)field).Url.ToString();
                    }
                    else if (((LinkField)field).Url == null || ((LinkField)field).Url == "")
                    {
                        linkURL = ((LinkField)field).Url.ToString();
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
        public static string GetLinkURL(LinkField lf)
        {
            var options = LinkManager.GetDefaultUrlBuilderOptions();
            options.AlwaysIncludeServerUrl = false;

            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? LinkManager.GetItemUrl(lf.TargetItem, options) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? MediaManager.GetMediaUrl(lf.TargetItem, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false }) : string.Empty;
                case "external":
                    // Just return external links
                    string URL = string.Empty;
                    if (!string.IsNullOrEmpty(lf.Url) && lf.Url != "" && lf.Url != null)
                    {
                        if (lf.Url.Contains("https://") || lf.Url.Contains("http://"))
                        {
                            URL = lf.Url;
                        }
                        else
                        {
                            URL = Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + lf.Url;
                        }
                        return URL;
                    }
                    return lf.Text;
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
                        LinkField link = fieldName;
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
                        LinkField link = fieldName;
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
            if (!string.IsNullOrEmpty(linkField.ToString()))
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
                        MediaItem media = new MediaItem(linkField.TargetItem);
                        linkURL = Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false }));
                        break;
                    case "":
                        break;
                    default:
                        //logger
                        string message = string.Format("error : Unknown link type {0} in {1}", linkField.LinkType, item.Paths.FullPath);
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
                        LinkField link = field;
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
            Database webDB = Sitecore.Configuration.Factory.GetDatabase("web");
            Item sampleMedia = new MediaItem(webDB.GetItem(path));
            var domain = GetSitecoreDomain();
            return domain + Sitecore.StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(sampleMedia, new MediaUrlBuilderOptions() { AlwaysIncludeServerUrl = false }));
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
            ReferenceField referenceField = item.Fields[fieldID];
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
            LinkField linkField = item.Fields[fieldName];
            if (linkField != null && !string.IsNullOrEmpty(linkField.ToString()))
            {
                linkURL = ((LinkField)item.Fields[fieldName]).Target;
            }
            return linkURL;
        }
        public static string GetValue(Item item, ID fieldId, string defaultValue = "")
        {
            string fieldValue = item.Fields[fieldId]?.Value;
            return string.IsNullOrEmpty(fieldValue) ? defaultValue : fieldValue;
        }
        public static string GetGtmPageTypeValue(Item fieldId, string defaultValue = "")
        {
            string fieldValue = string.IsNullOrEmpty(fieldId.DisplayName) ? fieldId.DisplayName : fieldId.Name;
            return fieldValue;
        }
        public static bool GetBoleanValue(Item item, ID fieldId)
        {
            CheckboxField checkboxField = item.Fields[fieldId];
            if (checkboxField != null && checkboxField.Checked)
            {
                return checkboxField.Checked;
            }
            return false;
        }
        public static bool CompareIgnoreCase(string value1, string value2)
        {
            return string.Equals(value1, value2, StringComparison.CurrentCultureIgnoreCase);
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
        public static string GetDate(Item item, ID fieldId)
        {
            var dateISOformat = item.Fields[fieldId]?.Value;
            if (string.IsNullOrWhiteSpace(dateISOformat)) return string.Empty;

            var date = DateTimeOffset.ParseExact(dateISOformat, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
            return date.ToString("dd MMM yyyy");

        }
        public static Item GetRenderingDatasource(Rendering rendering)
        {
            if (string.IsNullOrEmpty(rendering.DataSource))
            {
                return null;
            }

            var datasourceItem = rendering.RenderingItem?.Database.GetItem(rendering.DataSource);
            if (datasourceItem == null)
            {
                Sitecore.Diagnostics.Log.Info($"{rendering.RenderingItem.Name} component's datasource is empty", rendering);
            }

            return datasourceItem;
        }
        public static string GetAntiForgeryToken()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }
        public static bool IsReCaptchV2Valid(string captchaResponse)
        {
            var secretKey = "6LcDScokAAAAADJ4kHlZna2x_kwiBdh9vaiCmtNS"; //ConfigurationManager.AppSettings["SecretKey"];
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    return jResponse.Value<bool>("success");
                }
            }
        }
        public static string Settings(string key)
        {
            var Item = Context.Database.GetItem(new ID("{2067414F-41BA-4BAC-A301-BCF4EE4CDE5B}"));
            return Item?.Children
                .FirstOrDefault(x => x.Fields["Key"].Value.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                ?.Fields["Phrase"]?.Value
                ?? string.Empty;
        }
        //V3 captcha method
        public static bool IsV3CaptchValid(string response, string secretKey)
        {
            HttpClient httpClient = new HttpClient();
            var captchaResponse = response;
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var res = httpClient.GetAsync(requestUri).Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5)
            {
                return false;
            }

            return true;
        }

        public static void ValidateAntiForgeryToken(HttpRequestBase request)
        {
            var antiforgeryToken = request.Headers.GetValues("AntiforgeryToken")?.FirstOrDefault();

            if (string.IsNullOrEmpty(antiforgeryToken))
            {
                AntiForgery.Validate("", "");
                return;
            }

            string[] tokens = antiforgeryToken.Split(':');
            if (tokens.Length == 2)
            {
                AntiForgery.Validate(tokens[0].Trim(), tokens[1].Trim());
            }
        }
    }
}