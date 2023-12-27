using Project.AdaniOneSEO.Website.Services.FlightsToDestination.Filter_Options;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;


namespace Project.AdaniOneSEO.Website.LayoutServices.FlightsToDestination
{
    public class FilterOptionsLayoutService : RenderingContentsResolver
    {
        private readonly IFilterOptions _rootResolver;

        public FilterOptionsLayoutService(IFilterOptions rootResolver)
        {
            _rootResolver = rootResolver;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _rootResolver.GetFilterOptions(rendering);

        }
    }
}