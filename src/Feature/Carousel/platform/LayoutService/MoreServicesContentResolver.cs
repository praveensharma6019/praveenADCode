using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class MoreServicesContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IMoreServicesService moreServicesService;

        public MoreServicesContentResolver(IMoreServicesService moreServicesServices)
        {
            this.moreServicesService = moreServicesServices;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].ToLower() : "";
            return moreServicesService.GetMoreServicesService(rendering,queryString);
        }
    }
}