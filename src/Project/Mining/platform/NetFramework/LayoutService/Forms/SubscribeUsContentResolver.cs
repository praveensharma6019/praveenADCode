using Project.Mining.Website.Services.Forms;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService.Forms
{
    public class SubscribeUsContentResolver : RenderingContentsResolver
    {
        private readonly IMiningFormsService _miningFormsService;

        public SubscribeUsContentResolver(IMiningFormsService miningFormsService)
        {
            _miningFormsService = miningFormsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _miningFormsService.GetSubscribeForm(rendering);
        }
    }
}