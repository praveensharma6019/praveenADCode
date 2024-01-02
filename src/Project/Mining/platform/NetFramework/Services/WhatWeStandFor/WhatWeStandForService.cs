using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Services.Banner;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.Banner
{
    public class WhatWeStandForService : IWhatWeStandForService
    {
        private readonly ISitecoreService _sitecoreService;
        public WhatWeStandForService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public WhatWeStandForModel GetWhatWeStandFor(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<WhatWeStandForModel>(datasource);
            return ourAirportdata;
        }
    }
}