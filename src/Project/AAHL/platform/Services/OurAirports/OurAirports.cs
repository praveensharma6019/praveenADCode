using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.OurAirports;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Common
{
    public class OurAirports : IOurAirports
    {

        private readonly IMvcContext _mvcContext;

        public OurAirports(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }


        public Details GetDetails(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<Details>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public TravelReadyAirports GetTravelReadyAirports(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<TravelReadyAirports>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public OurAirportMap GetAirportMap(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelMapData = _mvcContext.GetDataSourceItem<OurAirportMap>();
                return ModelMapData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}