using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using Project.Mining.Website.Services.OurServices;

namespace Project.Mining.Website.LayoutService
{
    public class OurServicesServiceResolver : RenderingContentsResolver
    {
        private readonly IOurServices rootResolver;

        public OurServicesServiceResolver(IOurServices OurServicesdataResolver)
        {
            this.rootResolver = OurServicesdataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return rootResolver.GetOurServicesModel(rendering);
                    
        }
    }
}