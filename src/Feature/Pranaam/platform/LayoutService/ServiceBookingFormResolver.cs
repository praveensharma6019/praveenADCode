using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using System;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class ServiceBookingFormResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IServiceBookingForm _bookingForm;
        public ServiceBookingFormResolver(IServiceBookingForm bookingForm)
        {
            this._bookingForm = bookingForm;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _bookingForm.GetServiceBookingData(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceBookingFormResolver throws Exception -> " + ex.Message);
            }
        }
    }
}