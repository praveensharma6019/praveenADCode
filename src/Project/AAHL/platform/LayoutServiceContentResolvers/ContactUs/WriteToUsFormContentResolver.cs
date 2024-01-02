using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class WriteToUsFormContentResolver : RenderingContentsResolver
    {
        private readonly IContactUsServices rootResolver;

        public WriteToUsFormContentResolver(IContactUsServices dataResolver)
        {
            this.rootResolver = dataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetWriteToUsForm(rendering);
        }
    }
}