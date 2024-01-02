using Glass.Mapper.Sc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services
{
    public class BannerService : IBannerService
    {
        private ISitecoreService _sitecoreService;

        public BannerService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public BannerModel GetBanner(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var BannerModelData = _sitecoreService.GetItem<BannerModel>(datasource);
                return BannerModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}