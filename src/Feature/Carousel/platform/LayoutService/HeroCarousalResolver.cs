using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class HeroCarousalResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IHeroCarouselService heroCarouselService;

        public HeroCarousalResolver(IHeroCarouselService heroCarouselService)
        {
            this.heroCarouselService = heroCarouselService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            string storeType = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_storetype"])? Sitecore.Context.Request.QueryString["sc_storetype"].Trim().ToLower() : "departure";
            string airport = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["sc_airport"]) ? Sitecore.Context.Request.QueryString["sc_airport"].Trim().ToLower() : "BOM";
            string queryString = !string.IsNullOrEmpty(Sitecore.Context.Request.QueryString["application"]) ? Sitecore.Context.Request.QueryString["application"].Trim().ToLower() : "";
            bool restricted = (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["sc_restricted"]) && System.Web.HttpContext.Current.Request.QueryString["sc_restricted"].Trim().ToLower().Equals("true"))? true : false;

            return heroCarouselService.GetHeroCarouseldata(rendering, queryString, storeType, airport, restricted);         
        }

    }
}