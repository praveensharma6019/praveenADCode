using Adani.SuperApp.Realty.Feature.Configuration.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.LayoutService
{
    public class CommunicationCornerResolver : RenderingContentsResolver
    {
        private readonly ICommunicationCornerService communicationService;
        public CommunicationCornerResolver(ICommunicationCornerService communicationService)
        {
            this.communicationService = communicationService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return communicationService.GetCommunicationCornerData(rendering);

        }   
    }
}