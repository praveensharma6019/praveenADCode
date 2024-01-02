using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Services;
using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.BannerModel;

namespace Project.Mining.Website.Banner
{
    public class WhyMtcsService : IWhyMtcsService
    {
        private readonly ISitecoreService _sitecoreService;
        public WhyMtcsService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public WhyMtcsModel GetWhyMtcsModel(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<WhyMtcsModel>(datasource);
            return ourAirportdata;
        }
    }
}