using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class WriteToUsDetailsContentResolver : RenderingContentsResolver
    {
        private readonly IContactUsServices rootResolver;

        public WriteToUsDetailsContentResolver(IContactUsServices dataResolver)
        {
            this.rootResolver = dataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetWriteToUsDetails(rendering);
        }
    }
}