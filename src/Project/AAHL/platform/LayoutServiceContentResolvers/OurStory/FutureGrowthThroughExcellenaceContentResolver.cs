using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class FutureGrowthThroughExcellenaceContentResolver : RenderingContentsResolver
    {
        private readonly IOurStory rootResolver;

        public FutureGrowthThroughExcellenaceContentResolver(IOurStory HomedataResolver)
        {
            this.rootResolver = HomedataResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return rootResolver.GetFutureGrowthThroughExcellenace(rendering);
        }
    }
}