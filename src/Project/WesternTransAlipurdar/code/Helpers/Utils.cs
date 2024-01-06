using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Links;
using Sitecore.Resources.Media;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;

namespace Sitecore.WesternTransAlipurdar.Website.Helpers
{
    public class Utils
    {

        public static string GetUrlDomain()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                var uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri.Replace("%20", " "));
                return string.Format("{0}://{1}", uri.Scheme, uri.Host);
            }
            return string.Empty;
        }

        public static bool CompareIgnoreCase(string value1, string value2)
        {
            return string.Equals(value1, value2, StringComparison.CurrentCultureIgnoreCase);
        }
        public static string GetLinkURL(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            //LinkResolver linkResolver = new LinkResolver();

            return LinkUrl(linkField);
        }
        public static string GetLinkTarget(Item item, string fieldName)
        {
            string linkURL = string.Empty;
            Sitecore.Data.Fields.LinkField linkField = item.Fields[fieldName];
            //LinkResolver linkResolver = new LinkResolver();
            return linkField.Target;
        }

        public static string GetImageURLByFieldId(Item item, ID fieldId)
        {
            string imageURL = string.Empty;

            if (item != null)
            {
                ImageField imgField = item.Fields[fieldId];
                if (imgField != null && imgField.MediaItem != null)
                {
                    imageURL = MediaManager.GetMediaUrl(imgField.MediaItem,new MediaUrlOptions() { AlwaysIncludeServerUrl=false});
                }
            }
            return imageURL;
        }

        public static string GetImageAlt(Item item, ID fieldId)
        {
            string imageAlt = string.Empty;
            if (item != null)
            {
                Sitecore.Data.Fields.ImageField imgField = ((ImageField)item.Fields[fieldId]);
                if (imgField.MediaItem != null)
                {
                    imageAlt = imgField.Alt;
                }
            }
            return imageAlt;
        }

        public static bool GetCheckboxOption(Field field)
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

        public static string GetDropLinkValue(Field fieldName)
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

        public static string GetDropListValue(Field fieldName)
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

        public static string Sanitize(string name)
        {
            return name.Replace(" ", "").Replace("&", "").Replace("'", "").Replace(".", "-");
        }

        public static string GetLinkURLbyField(Item item, Field field)
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

        public static String LinkUrl(Sitecore.Data.Fields.LinkField lf)
        {
            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = false;
            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(lf.TargetItem, options) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? GetUrlDomain() + Sitecore.Resources.Media.MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    string URL = string.Empty;
                    if ((!String.IsNullOrEmpty(lf.Url)) && lf.Url != "" && lf.Url != null)
                    {
                        URL = lf.Url;
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
        public static string GetValue(Item item, ID fieldId, string defaultValue = "")
        {
            string fieldValue = item.Fields[fieldId]?.Value;
            return string.IsNullOrEmpty(fieldValue) ? defaultValue : fieldValue;
        }

        public static string GetURLQueryParameter(string value)
        {
            value = value.Replace("%20", " ");
            value = value.Replace("%2D", "-");
            return value;
        }
        public static string GetAntiForgeryToken()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }
        public static bool GetBoleanValue(Item item, ID fieldId)
        {
            return item.Fields[fieldId]?.Value == "1";
        }
        public static bool IsReCaptchV2Valid(string captchaResponse)
        {
            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
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