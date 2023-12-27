using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class OfferRedeemStepResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IRedeemOffer redeemServices;
        public OfferRedeemStepResolver(IRedeemOffer _redeemServices)
        {
            this.redeemServices = _redeemServices;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
           return redeemServices.RedeemOfferSteps(rendering);
        }
    }
}