using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.BannerModel;

namespace Project.Mining.Website.Banner
{
    public class BannerService : IBannerService
    {
        private readonly ISitecoreService _sitecoreService;
        public BannerService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public BannerModel GetBannerModel(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<BannerModel>(datasource);
            return ourAirportdata;
        }
        public PageBannerModel GetPageBannerModel(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var bannerdata = _sitecoreService.GetItem<PageBannerModel>(datasource);
            return bannerdata;
        }
    }
}