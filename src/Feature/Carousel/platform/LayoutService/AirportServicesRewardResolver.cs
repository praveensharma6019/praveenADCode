using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class AirportServicesRewardResolver: Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IRewardServices rewardServices;
        public AirportServicesRewardResolver(IRewardServices _rewardServices)
        {
            this.rewardServices = _rewardServices;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            if ((Sitecore.Context.Request.QueryString["sc_location"] == null ||
                string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]))) &&
                 (Sitecore.Context.Request.QueryString["sc_apptype"] == null ||
                    string.IsNullOrEmpty(Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"]))))
            {
                return null;
            }
            return rewardServices.GetAdaniRewardsServices(rendering, Convert.ToString(Sitecore.Context.Request.QueryString["sc_location"]),
                                                          Convert.ToString(Sitecore.Context.Request.QueryString["sc_apptype"]));
        }
    }
}