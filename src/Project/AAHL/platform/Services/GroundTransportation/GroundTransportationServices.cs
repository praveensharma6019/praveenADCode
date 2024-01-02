using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.GroundTransportation;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.GroundTransportation
{
    public class GroundTransportationServices : IGroundTransportationServices
    {
        private readonly IMvcContext _mvcContext;
        public GroundTransportationServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public TravelExperience GetTransportation(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var aviationdata = _mvcContext.GetDataSourceItem<TravelExperience>();
                return aviationdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}