using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.LayoutServices
{
    public class CustomContentJSONResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICustomContentJSON customContentJSON;

        public CustomContentJSONResolver(ICustomContentJSON _customContentJSON)
        {
            this.customContentJSON = _customContentJSON;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string storeType = string.Empty;
            string infoType = string.Empty;
            string insurance = string.Empty;
            string zeroCancellation = string.Empty;
            string status = string.Empty;
            string fareType = string.Empty;

            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_storetype"]))
            {
                storeType = System.Web.HttpContext.Current.Request.QueryString["sc_storetype"];
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["infotype"]))
            {
                infoType = System.Web.HttpContext.Current.Request.QueryString["infotype"];
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["insurance"]))
            {
                insurance = System.Web.HttpContext.Current.Request.QueryString["insurance"];
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["zeroCancellation"]))
            {
                zeroCancellation = System.Web.HttpContext.Current.Request.QueryString["zeroCancellation"];
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["status"]))
            {
                status = System.Web.HttpContext.Current.Request.QueryString["status"];
            }
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["fareType"]))
            {
                fareType = System.Web.HttpContext.Current.Request.QueryString["fareType"];
            }
            return this.customContentJSON.GetCustomContentJSONData(rendering, storeType.Trim().ToLower(), infoType.Trim().ToLower(), insurance.Trim().ToLower(), zeroCancellation.Trim().ToLower(), status.Trim().ToLower(), fareType.Trim().ToLower());
        }
    }
}