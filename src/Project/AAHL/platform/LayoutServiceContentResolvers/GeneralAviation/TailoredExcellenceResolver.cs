using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.GeneralAviation;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.GeneralAviation
{
    public class TailoredExcellenceResolver : RenderingContentsResolver
    {
        private readonly IGeneralAviationServices rootResolver;

        public TailoredExcellenceResolver(IGeneralAviationServices generalAviationService)
        {
            this.rootResolver = generalAviationService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetGeneralAviation(rendering);
        }
    }
}