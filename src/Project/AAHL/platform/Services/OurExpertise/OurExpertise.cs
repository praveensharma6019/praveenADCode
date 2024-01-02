using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.LayoutServiceContentResolvers;
using Project.AAHL.Website.Models.Our_Expertise;
using Project.AAHL.Website.Models.OurAirports;
using Project.AAHL.Website.Models.OurExpertise;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Common
{
    public class OurExpertise : IOurExpertise
    {

        private readonly IMvcContext _mvcContext;

        public OurExpertise(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }



        public RetailDevelopmentModel GetRetailDevelopmentModel(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<RetailDevelopmentModel>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public VideoBannerModel GetRetailVideoBannerModel(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<VideoBannerModel>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}