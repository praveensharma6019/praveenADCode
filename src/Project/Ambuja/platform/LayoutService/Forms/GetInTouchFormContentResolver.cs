using Project.AmbujaCement.Website.Services.Forms;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.AmbujaCement.Website.LayoutService.Forms
{
    public class GetInTouchFormContentResolver : RenderingContentsResolver
    {
        private readonly IAmbujaFormsService _ambujaFormsService;

        public GetInTouchFormContentResolver(IAmbujaFormsService ambujaFormsService)
        {
            _ambujaFormsService = ambujaFormsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _ambujaFormsService.GetGetInTouchFormModel(rendering);
        }
    }
}