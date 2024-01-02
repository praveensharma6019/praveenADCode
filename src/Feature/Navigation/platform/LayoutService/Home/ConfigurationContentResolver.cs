using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System.Linq;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;
namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.LayoutService.Home
{
    public class ConfigurationContentResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        protected readonly IFooterService RootResolver;

        public ConfigurationContentResolver(IFooterService rootResolver)
        {
            RootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return RootResolver.GetConfigurationData(rendering);

        }
    }
}