using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.AAHL.Website.Templates.BaseTemplate;
using static Project.AAHL.Website.Templates.BaseTemplate.TitleTemplate;

namespace Project.AAHL.Website.Services.Common
{
    public class HomeService : IHomeService
    {

        private readonly ISitecoreService _sitecoreService;

        public HomeService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public PageMetaData GetPageMetaData(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<PageMetaData>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Banner GetBanner(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<Banner>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public AirportStats GetAirportStats(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<AirportStats>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public BannerAdsModel GetBannerAdsModel(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<BannerAdsModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public AirportNews GetAirportNews(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<AirportNews>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public AboutAirport GetAboutAirport(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<AboutAirport>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AirportBusiness GetAirportBusiness(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<AirportBusiness>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}