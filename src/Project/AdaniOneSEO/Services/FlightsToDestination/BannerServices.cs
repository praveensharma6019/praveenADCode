using Glass.Mapper.Sc;
﻿using Glass.Mapper.Sc.Web.Mvc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination.Banner;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination
{
    public class BannerServices : IBannerServices
    {
        private readonly IMvcContext _mvcContext;

        public BannerServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }


        public BannerModel GetBanner(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<BannerModel>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}