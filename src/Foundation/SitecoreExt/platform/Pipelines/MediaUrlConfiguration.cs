using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using Sitecore.Pipelines.RenderField;
using Sitecore;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sitecore.Resources.Media;
using System.Linq;
using Sitecore.Configuration;
using Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Constants;
using System.IO;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Pipelines
{
    public class MediaUrlConfiguration : GetImageFieldValue
    {
       
        public override void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull((object)args, nameof(args));

            if (this.ShouldExecute() && AllowedImageFields(args.FieldName))
            {
                base.Process(args); // generate "default" result
                if (args.Result.FirstPart != string.Empty )
                {
                    if (args.Result.FirstPart.Contains("<img"))
                    {   
                        args.Result.FirstPart = this.FixImageTag(args.Result.FirstPart);
                    }
                    
                }
                // alter results
                args.AbortPipeline(); // exit out
            }
        }


        public bool ShouldExecute()
        {
            if (!Context.PageMode.IsNormal)
                return false;
            if (Context.Site == null)
                return false;
            if (RenderingContext.Current?.Rendering?.RenderingItem?.InnerItem == null)
                return false;
            // note: you have access to RenderingContext.Current.Rendering at this point in time
            // you could check this for the current placeholder or type of rendering being processed
            // this may be useful if you want to avoid lazy loading images within your header, or
            // prevent lazy loading images within your hero renderings.

            return true;
        }

        public string FixImageTag(string tag)
        {
            var modifiedVal = tag;
            if (tag.Contains("<img"))
            {
                
                var imageDomain = Sitecore.Configuration.Settings.GetSetting("mediaDomainUrl").ToString();

                var img_src =  GetAttributeNameInHTMLString(tag, "src");
                if (img_src != null) {
                    imageDomain = imageDomain + img_src.First().ToString();
                }

                var existedURL = "src=\"" + img_src.First().ToString() + "\"";
                var modifiedURL = "src=\"" + imageDomain + "\"" + "  loading=\"lazy\"";

                if (tag.Contains(existedURL)) {
                modifiedVal=   tag.Replace(existedURL, modifiedURL);
                }

            }

            return modifiedVal;
        }

        public static List<string> GetAttributeNameInHTMLString(string htmlString, string attributeName)
        {
            List<string> attributeValues = new List<string>();
            string pattern = string.Format(@"(?<={0}="").*?(?="")", attributeName);

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);

            for (int i = 0, l = matches.Count; i < l; i++)
            {
                string d = matches[i].Value;
                attributeValues.Add(d);
            }
            return attributeValues;
        }

        public bool AllowedImageFields(string fieldName)
        {
            var validExtensions = Settings.GetSetting(Constant.AllowedImageFieldNames).Split(',').ToList();

            return validExtensions.Any(ext => ext.Equals(fieldName));
        }
    }
}
