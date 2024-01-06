using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Sanitization;
using Ganss.XSS;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Controllers
{
    public class ContactUsController : Controller
    {
        Helper obj = new Helper();
        // GET: SocialMedia
        public ActionResult ContactUs()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            ContactUsModel objContactUs = new ContactUsModel();
            try
            {
                if (dataSource != null)
                {
                    var richtext=!string.IsNullOrEmpty(dataSource.Fields["RichText"].Value) ? dataSource.Fields["RichText"].Value : string.Empty;
                    objContactUs.RichText = new HtmlString(richtext);

                    string imageURL = getFileUrl(dataSource);
                    objContactUs.FileURL = imageURL;
                    objContactUs.ImageAlt = obj.GetImageAlt(dataSource, "Image");
                    objContactUs.ImageSrc = obj.GetImageURL(dataSource, "Image");
                    Sitecore.Data.Fields.MultilistField multiselectField = dataSource.Fields["CityList"];
                    Sitecore.Data.Items.Item[] items = multiselectField.GetItems();

                    objContactUs.cityList = ParseDatasourceMediaItems(items);
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }

            return View("~/views/ContactUs/ContactUs.cshtml", objContactUs);
        }

        private string returnString(Sitecore.Data.Items.Item item, string columnName)
        {
            string val = !string.IsNullOrEmpty(item.Fields[columnName].Value) ? item.Fields[columnName].Value : string.Empty;
            return StringSanitization.UseRegex(val);

        }
        private List<NameValue> ParseDatasourceMediaItems(Sitecore.Data.Items.Item[] items)
        {
            List<NameValue> objNameValueList = new List<NameValue>();
            NameValue objNameValue = null;
            foreach (Sitecore.Data.Items.Item Item in items)
            {
                objNameValue = new NameValue();
                objNameValue.name = returnString(Item, "name");
                objNameValue.value = returnString(Item, "value");
                var richtext=!string.IsNullOrEmpty(Item.Fields["RichText"].Value) ? Item.Fields["RichText"].Value : string.Empty;
                var htmlsanitizer = new HtmlSanitizer();
                var sanitized=htmlsanitizer.Sanitize(richtext);
                objNameValue.RichText = new HtmlString(sanitized);
                objNameValue.ImageAlt = obj.GetImageAlt(Item, "Image");
                objNameValue.ImageSrc = obj.GetImageURL(Item, "Image");
                objNameValueList.Add(objNameValue);

            }
            return objNameValueList;
        }
        private string getFileUrl(Sitecore.Data.Items.Item dataSource)
        {
            string imageURL = string.Empty;
            Sitecore.Data.Fields.ImageField imageField = dataSource.Fields["File"];
            Sitecore.Resources.Media.MediaUrlOptions mediaOptions = new Sitecore.Resources.Media.MediaUrlOptions();
            mediaOptions.IncludeExtension = true;

            if (imageField != null && imageField.MediaItem != null)
            {
                Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
                //mediaOptions.RequestExtension = image.Extension;
                imageURL = Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(image, mediaOptions));
            }
            return imageURL;
        }
    }
}