using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class BannerDetailResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly IBannerDetails _details;

        public BannerDetailResolver(IBannerDetails reasons)
        {
            this._details = reasons;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _details.GetBannerDetails(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["bannercode"]));
        }
    }
}