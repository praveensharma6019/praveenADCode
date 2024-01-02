using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class TitleDescImageDatasourceResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ITitleDescImageDatasourceService titleDescImageDatasourceService;

        public TitleDescImageDatasourceResolver(ITitleDescImageDatasourceService titleDescImageDatasourceService)
        {
            this.titleDescImageDatasourceService = titleDescImageDatasourceService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return titleDescImageDatasourceService.GetTitleDescImageDatasource(rendering);
        }
    }
}