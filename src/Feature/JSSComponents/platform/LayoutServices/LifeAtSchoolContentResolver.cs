using Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Services.Common;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.LayoutService
{
    public class LifeAtSchoolContentResolver : RenderingContentsResolver
    {
        private readonly ICommonComponents RootResolver;

        public LifeAtSchoolContentResolver(ICommonComponents lifeAtSchoolData)
        {
            this.RootResolver = lifeAtSchoolData;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetLifeAtSchool(rendering);
        }
    }
}