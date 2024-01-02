using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using System;
using Sitecore.Mvc.Presentation;
using Sitecore.LayoutService.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class ServiceCarousalResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IServiceCarousal _serviceObj;
        public ServiceCarousalResolver(IServiceCarousal serviceObj)
        {
            this._serviceObj = serviceObj;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _serviceObj.GetServiceCarousalData(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceCarousalResolver throws Exception -> " + ex.Message);
            }
        }
    }
}