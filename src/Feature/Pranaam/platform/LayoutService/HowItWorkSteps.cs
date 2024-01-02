using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class HowItWorkSteps : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IHowItWorkSteps _obj;
        public HowItWorkSteps(IHowItWorkSteps obj)
        {
            this._obj = obj;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _obj.GetStepDetails(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("HowItWorkSteps Resolver throws Exception -> " + ex.Message);
            }
        }
    }
}