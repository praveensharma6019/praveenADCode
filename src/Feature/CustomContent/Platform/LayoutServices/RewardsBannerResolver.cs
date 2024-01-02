using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services;
using System.Web;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.LayoutServices
{
    public class RewardsBannerResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IResolveBannerJSON resolveBannerJSON;

        public RewardsBannerResolver(IResolveBannerJSON _resolveBannerJSON)
        {
            this.resolveBannerJSON = _resolveBannerJSON;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string storeType = "";
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].Trim().ToLower() : "";
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_storetype"]))
            {
                storeType = HttpContext.Current.Request.QueryString["sc_storetype"].Trim().ToLower();
            }
            return resolveBannerJSON.GetBanner(rendering, queryString, storeType);
        }
    }
}