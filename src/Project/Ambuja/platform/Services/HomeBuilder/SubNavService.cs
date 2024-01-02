using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.HomeBuilder;
using Project.AmbujaCement.Website.Models.SiteMap;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.HomeBuilder
{
    public class SubNavService : ISubNavService
    {
        private readonly IMvcContext _mvcContext;
        public SubNavService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public SubNavModel GetSubNav(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var subnavDataModel = _mvcContext.GetDataSourceItem<SubNavModel>();
                return subnavDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public OverviewModel GetOverview(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var overDataModel = _mvcContext.GetDataSourceItem<OverviewModel>();
                return overDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public VectorCardModel GetVectorCard(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var vectorDataModel = _mvcContext.GetDataSourceItem<VectorCardModel>();
                return vectorDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public TwoColumnListModel GetColumnList(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var columnlistDataModel = _mvcContext.GetDataSourceItem<TwoColumnListModel>();
                return columnlistDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public CardListModel GetCardList(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var cardlistDataModel = _mvcContext.GetDataSourceItem<CardListModel>();
                return cardlistDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public SelectingTechModel GetSelectingTech(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var techDataModel = _mvcContext.GetDataSourceItem<SelectingTechModel>();
                return techDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public BisStandardsModel GetBisstandard(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var BisStandardsDataModel = _mvcContext.GetDataSourceItem<BisStandardsModel>();
                return BisStandardsDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public HomeBuilderModel GetHomeBuilderCard(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var data = _mvcContext.GetDataSourceItem<HomeBuilderModel>();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}