using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class AddressContentResolver : RenderingContentsResolver
    {
        private readonly IContactUsServices rootResolver;

        public AddressContentResolver(IContactUsServices HomedataResolver)
        {
            this.rootResolver = HomedataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetAddress(rendering);
        }
    }
}