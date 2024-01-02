using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.Cargo;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Cargo
{
    public class CargoServices : ICargoServices
    {
        private readonly IMvcContext _mvcContext;
        public CargoServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public OurWayForwardModel GetOurWayForward(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var forwarddata = _mvcContext.GetDataSourceItem<OurWayForwardModel>();
                return forwarddata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public OperationalEfficiencyModel GetOperationalEfficiency(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var forwarddata = _mvcContext.GetDataSourceItem<OperationalEfficiencyModel>();
                return forwarddata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}