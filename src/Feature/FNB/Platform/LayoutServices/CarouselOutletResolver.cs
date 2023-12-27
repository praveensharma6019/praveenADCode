using Adani.SuperApp.Airport.Feature.FNB.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform.LayoutServices
{
    public class CarouselOutletResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {

        private readonly ICarouselOutlet _carouselData;

        public CarouselOutletResolver(ICarouselOutlet outletData)
        {
            this._carouselData = outletData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {

            return _carouselData.GetCarouselOutletData(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["sc_terminaltype"]),
                                                           Convert.ToString(Sitecore.Context.Request.QueryString["isApp"]));
        }
    }
}