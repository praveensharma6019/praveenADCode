using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class SliderWithImageAndDetailResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ISliderWithImageAndDetailService sliderWithImageAndDetailService;

        public SliderWithImageAndDetailResolver(ISliderWithImageAndDetailService sliderWithImageAndDetailService)
        {
            this.sliderWithImageAndDetailService = sliderWithImageAndDetailService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].ToLower() : "";
            string cityqueryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["citycode"]) ? Sitecore.Context.Request.QueryString["citycode"].ToLower() : "";
            return sliderWithImageAndDetailService.GetSliderWithImageAndDetail(rendering,queryString, cityqueryString);
        }
    }
}