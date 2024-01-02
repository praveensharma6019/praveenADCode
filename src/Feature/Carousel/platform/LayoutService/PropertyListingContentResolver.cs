using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Realty.Feature.Carousel.Platform.Services;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Carousel.Platform.LayoutService
{
    public class PropertyListingContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IHeroCarouselService heroCarouselService;

        public PropertyListingContentResolver(IHeroCarouselService heroCarouselService)
        {
            this.heroCarouselService = heroCarouselService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string location = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            location = !string.IsNullOrEmpty(location) ? location.Replace("property-in-", "") : "";
            string type = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Type");
            string status = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Status");
            string Configuration = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Configuration");
            return heroCarouselService.GetPropertyList(rendering, location, type, status);
            //return heroCarouselService.GetPropertyList(rendering, location);

        }

    }
}