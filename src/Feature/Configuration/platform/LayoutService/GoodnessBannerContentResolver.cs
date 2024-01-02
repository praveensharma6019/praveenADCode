using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.LayoutService
{
    public class GoodnessBannerContentResolver : RenderingContentsResolver
    {
        private readonly IWhyAdaniService GoodnessBanner;
        public GoodnessBannerContentResolver(IWhyAdaniService GoodnessBanner)
        {
            this.GoodnessBanner = GoodnessBanner;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return GoodnessBanner.GetGoodnessBanner(rendering);

        }
    }
}