using Sitecore.LayoutService.Configuration;
using System;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Feature.Master.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.LayoutService
{
    public class DomainListResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly IDomainListService domainListService;

        public DomainListResolver(IDomainListService domainListService)
        {
            this.domainListService = domainListService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return this.domainListService.GetDomainData(rendering);
        }
    }
}