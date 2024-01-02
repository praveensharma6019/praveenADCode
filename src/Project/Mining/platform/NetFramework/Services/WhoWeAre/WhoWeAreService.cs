using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Services;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.BannerModel;
using static Project.Mining.Website.Models.WhoWeAreModel;

namespace Project.Mining.Website.Home
{
    public class WhoWeAreService : IWhoWeAreService
    {
        private readonly ISitecoreService _sitecoreService;
        public WhoWeAreService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public WhoWeAreModel GetWhoWeAreModel(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<WhoWeAreModel>(datasource);
            return ourAirportdata;
        }
    }
}