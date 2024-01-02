using Project.Mining.Website.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService
{
    public class TermsandConditionsServiceResolver : RenderingContentsResolver
    {
        private readonly IPrivacyPolicyService _rootResolver;

        public TermsandConditionsServiceResolver(IPrivacyPolicyService rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
          return _rootResolver.GetTermsDetail(rendering);
                    
        }
    }
}