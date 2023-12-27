using Adani.SuperApp.Airport.Feature.Master.Platform.Services;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.LayoutService
{
    public class CountryMasterResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly ICountryMasterService countryMasterService;

        public CountryMasterResolver(ICountryMasterService countryMasterService)
        {
            this.countryMasterService = countryMasterService;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var datasource = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            return this.countryMasterService.GetCountryMasterData(datasource);
        }
    }
}