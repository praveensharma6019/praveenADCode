using Project.AAHL.Website.Services.Common;
using Project.AAHL.Website.Services.OurBelief;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.Sustainability
{
    public class SustainabilityContentResolver : RenderingContentsResolver
    {
        private readonly ISustainability _rootResolver;

        public SustainabilityContentResolver(ISustainability rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetSustainability(rendering);
        }    
    }
}