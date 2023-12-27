using Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Constants;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.Upload;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Pipelines
{
    public class CheckImageSize : UploadProcessor
    {

        public List<string> ValidExtensions { get; set; }

        public CheckImageSize()
        {
            ValidExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".tiff", ".tif" };
        }

        public void Process(UploadArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));

            if (args.Destination == UploadDestination.File)
                return;
            //bool checkToken = false;

            foreach (string index in args.Files)
            {
                HttpPostedFile file = args.Files[index];
                if (!string.IsNullOrEmpty(file.FileName))
                {

                    if (file.FileName.Contains(Settings.GetSetting("Media.ForceUploadKey")))
                    {
                        break;

                    }
                    else if ((long)file.ContentLength > AllowedImageSize && ValidExtensions.Any(ext => ext.Equals(Path.GetExtension(file.FileName))))
                    {
                        string text = StringUtil.EscapeJavascriptString(file.FileName);
                        args.UiResponseHandlerEx.FileCannotBeUploaded(text, StringUtil.EscapeJavascriptString($"You must select an image smaller than {(object)MainUtil.FormatSize(AllowedImageSize)} in size with one of the following file extensions : .jpg, .png, .gif, .jpeg, .tif, .tiff "));
                        args.ErrorText = $"The file {(object)text} is too big to be uploaded. You must select an image smaller than {(object)MainUtil.FormatSize(AllowedImageSize)} in size with one of the following file extensions : .jpg, .png, .gif, .jpeg, .tif, .tiff.";
                        args.AbortPipeline();
                        break;
                    }
                    else if ((long)file.ContentLength > AllowedGifExtensionSize && Path.GetExtension(file.FileName) == ".gif")
                    {
                        string text = StringUtil.EscapeJavascriptString(file.FileName);
                        args.UiResponseHandlerEx.FileCannotBeUploaded(text, StringUtil.EscapeJavascriptString($"You must select an file smaller than {(object)MainUtil.FormatSize(AllowedGifExtensionSize)} in size with the following file extensions : .gif. "));
                        args.ErrorText = $"The file {(object)text} is too big to be uploaded. You must select an file smaller than {(object)MainUtil.FormatSize(AllowedGifExtensionSize)} in size with the following file extensions : .gif.";
                        args.AbortPipeline();
                        break;
                    }
                }
            }
        }

        public static long AllowedImageSize
        {

            get
            {
                var MaxVal = Settings.GetSetting(Constant.AllowedImageMaxSize);
                return Settings.GetLongSetting(Constant.AllowedImageSize, long.Parse(MaxVal));
            }
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