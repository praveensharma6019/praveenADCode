using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Project.AmbujaCement.Website.Models.AboutUsPage;
using Project.AmbujaCement.Website.Models.ProductList;
using Project.AmbujaCement.Website.Models.ProjectDetails;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.AboutUs
{
    public class ProjectDetailsService : IProjectDetailsService
    {
        private readonly IMvcContext _mvcContext;
        public ProjectDetailsService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public BrochureDataModel GetBrochureDataModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getModelData = _mvcContext.GetDataSourceItem<BrochureDataModel>();
                return getModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public FeaturesModel GetFeaturesModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getModelData = _mvcContext.GetDataSourceItem<FeaturesModel>();
                return getModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public HeroBannerModel GetHeroBannerModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getModelData = _mvcContext.GetDataSourceItem<HeroBannerModel>();
                return getModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}