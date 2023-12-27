using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
   
    public class OfferDiscountContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IOfferDiscount offerDiscountService;

        public OfferDiscountContentResolver(IOfferDiscount _offerDiscountService)
        {
            this.offerDiscountService = _offerDiscountService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            if (Sitecore.Context.Request.QueryString["sc_apptype"] == null && !string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"])))
            {
                return null;
            } 
            else

                return offerDiscountService.GetOfferList(rendering, true, Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                            Convert.ToString(Sitecore.Context.Request.QueryString["sc_storetype"]),
                                                            Convert.ToString(Sitecore.Context.Request.QueryString["sc_iste"]),
                                                            Convert.ToString(Sitecore.Context.Request.QueryString["sc_module"]),
                                                            Convert.ToString(Sitecore.Context.Request.QueryString["sc_isod"]),
                                                            Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"]));
        }
    }
}