using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class ServiceDemoResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IServiceDemo _demo;
        public ServiceDemoResolver(IServiceDemo demo)
        {
            this._demo = demo;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _demo.GetServiceDemoData(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("ServiceDemoResolver throws Exception -> " + ex.Message);
            }
        }
    }
}