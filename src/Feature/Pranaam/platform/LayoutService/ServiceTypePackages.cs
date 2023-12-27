using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using System;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class ServiceTypePackages : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IServiceTypePackages _servicePckg;
        public ServiceTypePackages(IServiceTypePackages servicePckg)
        {
            this._servicePckg = servicePckg;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                var airportCode = Sitecore.Context.Request.QueryString["airportcode"];
                var serviceTypeId = Sitecore.Context.Request.QueryString["servicetypeid"];
                var travelSectorId = Sitecore.Context.Request.QueryString["travelsectorid"];
                return _servicePckg.GetServiceTypePackage(serviceTypeId,travelSectorId, airportCode);
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceTypePackages throws Exception -> " + ex.Message);
            }
        }
    }
}