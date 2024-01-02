using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;
using Sitecore.LayoutService.Configuration;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class HeroSliderResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IHeroSliderService data;
        public HeroSliderResolver(IHeroSliderService _faqService)
        {
            this.data = _faqService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            //Get the datasource for the item
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }

            return this.data.HeroSliderData(rendering, datasource);

        }
    }
}