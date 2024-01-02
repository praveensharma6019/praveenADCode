using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.DiscoverMiningBrochureModel;

namespace Project.Mining.Website.Services.OurProjects
{
    public class DiscoverMiningBrochureServices : IDiscoverMiningBrochureServices
    {
        private readonly ISitecoreService _sitecoreService;
        public DiscoverMiningBrochureServices(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public DiscoverMiningBrochureModel GetDiscoverMiningBrochure(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<DiscoverMiningBrochureModel>(datasource);
            return ourAirportdata;
        }
    }
}