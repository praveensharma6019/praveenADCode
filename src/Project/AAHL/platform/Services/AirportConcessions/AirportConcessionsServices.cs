using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.AirportConcessions;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.AirportConcessions
{
    public class AirportConcessionsServices : IAirportConcessionsServices
    {
        private readonly IMvcContext _mvcContext;
        public AirportConcessionsServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public BeyondAirportsModel GetConcessions(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var concessionsdata = _mvcContext.GetDataSourceItem<BeyondAirportsModel>();
                return concessionsdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartnerWithUsModel GetPartnerWithUs(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var partnerWithdata = _mvcContext.GetDataSourceItem<PartnerWithUsModel>();
                return partnerWithdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}