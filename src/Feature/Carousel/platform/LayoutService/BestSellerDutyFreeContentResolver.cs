using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class BestSellerDutyFreeContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IBestSellerDutyFreeService bestSellerDutyFreeService;

        public BestSellerDutyFreeContentResolver(IBestSellerDutyFreeService bestDutyFreeService)
        {
            this.bestSellerDutyFreeService = bestDutyFreeService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].ToLower() : "";
            return bestSellerDutyFreeService.GetBestSellerDutyFreeService(rendering, queryString);
        }
    }
}