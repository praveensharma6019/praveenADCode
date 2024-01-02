using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.GeneralAviation;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.GeneralAviation
{
    public class EfficiencyRedefinedResolver : RenderingContentsResolver
    {
        private readonly IGeneralAviationServices rootResolver;

        public EfficiencyRedefinedResolver(IGeneralAviationServices generalAviationService)
        {
            this.rootResolver = generalAviationService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetEfficiencyRedefined(rendering);
        }
    }
}