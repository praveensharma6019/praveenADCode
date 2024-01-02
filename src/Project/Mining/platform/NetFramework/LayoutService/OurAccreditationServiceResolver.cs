using Project.Mining.Website.Services.Header;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using Project.Mining.Website.Services.OurAccreditation;

namespace Project.Mining.Website.LayoutService
{
    public class OurAccreditationServiceResolver : RenderingContentsResolver
    {
        private readonly IOurAccreditationService rootResolver;

        public OurAccreditationServiceResolver(IOurAccreditationService OurAccreditationdataResolver)
        {
            this.rootResolver = OurAccreditationdataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return rootResolver.GetOurAccreditation(rendering);
                    
        }
    }
}