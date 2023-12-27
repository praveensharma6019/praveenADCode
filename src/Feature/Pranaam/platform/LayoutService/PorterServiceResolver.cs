using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class PorterServiceResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IPorterService _porterInfo;
        public PorterServiceResolver(IPorterService porterInfo)
        {
            this._porterInfo = porterInfo;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            try
            {
                return _porterInfo.GetPorterCardDetails(rendering);
            }
            catch (Exception ex)
            {
                throw new Exception("PorterServiceResolver throws Exception -> " + ex.Message);
            }
        }
    }
}