using Project.AAHL.Website.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AAHL.Website.LayoutServiceContentResolvers
{
    public class FooterNewContentResolver : RenderingContentsResolver
    {
        private readonly IFooterNew _footerNew;

        public FooterNewContentResolver(IFooterNew footernew)
        {
            this._footerNew = footernew;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _footerNew.GetFooterNew(rendering);
        }
    }
}