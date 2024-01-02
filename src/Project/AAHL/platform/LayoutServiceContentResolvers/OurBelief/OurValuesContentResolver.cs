using Project.AAHL.Website.Services.OurBelief;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers.OurBelief
{
    public class OurValuesContentResolver : RenderingContentsResolver
    {
        private readonly IOurBeliefService _rootResolver;

        public OurValuesContentResolver(IOurBeliefService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetOurValues(rendering);
        }
    }
}