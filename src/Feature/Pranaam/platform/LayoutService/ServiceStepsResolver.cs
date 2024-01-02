using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class ServiceStepsResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IBookingSteps _bookingSteps;
        public ServiceStepsResolver(IBookingSteps bookingSteps)
        {
            this._bookingSteps = bookingSteps;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _bookingSteps.GetPranaamSteps(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceStepsResolver throws Exception -> " + ex.Message);
            }
        }
    }
}