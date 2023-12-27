
using Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Constants;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines;
using Sitecore.Pipelines.Attach;
using Sitecore.Shell.Web.UI;
using Sitecore.Web.UI.XmlControls;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Represents a AttachPage2.
/// </summary>
/// 
namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Pipelines
{
    public class AttachPage2 : SecurePage
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            Control control = ControlFactory.GetControl("Attach");
            if (control != null)
            {
                Controls.Add(control);
            }
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!base.IsEvent && base.Request.Files.Count > 0)
            {
                string text = Upload();
                HttpContext.Current.Response.Write("<html><head><script type=\"text/JavaScript\" language=\"javascript\">window.top.scForm.getTopModalDialog().frames[0].scForm.postRequest(\"\", \"\", \"\", " + text + ")</script></head><body>Done</body></html>");
            }
        }

        /// <summary>
        /// Uploads this instance.
        /// </summary>
        /// <returns>The upload.</returns>
        /// <contract>
        ///   <ensures condition="not null" />
        /// </contract>
        private string Upload()
        {
            HttpPostedFile httpPostedFile = base.Request.Files["File"];
            if (httpPostedFile == null)
            {
                return "'ShowAlert(\"" + Translate.Text("File input not found.") + "\")'";
            }
            if (httpPostedFile.ContentLength == 0)
            {
                return "'ShowAlert(\"" + Translate.Text("The file does not contain any data. Maybe the file no longer exists.") + "\")'";
            }

            if ((httpPostedFile.ContentLength > AllowedImageSize || !AllowedExtension(httpPostedFile)) && !httpPostedFile.FileName.ToLower().Contains("gif"))
            {

                return "'ShowAlert(\"" + Translate.Text($"You must select an image smaller than {(object)MainUtil.FormatSize(AllowedImageSize)} in size with one of the following file extensions : .jpg, .png, .gif, .jpeg, .tif, .tiff ") + "\")'";

            }

            if (httpPostedFile.ContentLength > AllowedGifExtensionSize && Path.GetExtension(httpPostedFile.FileName) == Constant.GifExtension)
            {

                return "'ShowAlert(\"" + Translate.Text($"You must select an image smaller than {(object)MainUtil.FormatSize(AllowedGifExtensionSize)} in size with the following file extensions : .gif.") + "\")'";

            }
            string itemUri = String.Empty;
            if (Sitecore.Context.ClientPage != null)
            {
                itemUri = Sitecore.Context.ClientPage.ClientRequest.Form["ItemUri"];
            }
            MediaItem mediaItem = Database.GetItem(ItemUri.Parse(itemUri));
            if (mediaItem == null)
            {
                return "'ShowAlert(\"" + Translate.Text("The item does not exist. It may have been deleted by another user.") + "\")'";
            }
            try
            {
                Pipeline.Start("attachFile", new AttachArgs(httpPostedFile, mediaItem));
            }
            catch (Exception exception)
            {
                Log.Error("Upload error from Attach dialog.", exception, this);
                return "'ShowAlert(\"" + Translate.Text("The file you attached could be in the wrong format or has been corrupted.") + "\")'";
            }
            return "'EndUploading'";
        }

        public static long AllowedImageSize
        {
            get
            {
                var MaxVal = Settings.GetSetting(Constant.AllowedImageMaxSize);
                return Settings.GetLongSetting(Constant.AllowedImageSize, long.Parse(MaxVal));
            }
        }
        public bool AllowedExtension(HttpPostedFile file)
        {
            var validExtensions = Settings.GetSetting(Constant.AllowedExtension).Split(',').ToList();

            return validExtensions.Any(ext => ext.Equals(Path.GetExtension(file.FileName)));
        }

        public static long AllowedGifExtensionSize
        {
            get
            {
                var MaxVal = Settings.GetSetting(Constant.AllowedGIFExtensionMaxSize);

                return Settings.GetLongSetting(Constant.AllowedGIFExtensionSize, long.Parse(MaxVal));
            }
        }



    }

}