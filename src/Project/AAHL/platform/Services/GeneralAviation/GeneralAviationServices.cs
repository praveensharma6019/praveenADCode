using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.GeneralAviation;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.GeneralAviation
{
    public class GeneralAviationServices : IGeneralAviationServices
    {
        private readonly IMvcContext _mvcContext;
        public GeneralAviationServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public ImageDetailModel GetGeneralAviation(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var aviationdata = _mvcContext.GetDataSourceItem<ImageDetailModel>();
                return aviationdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DetailsChildModel GetEfficiencyRedefined(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var efficiencydata = _mvcContext.GetDataSourceItem<DetailsChildModel>();
                return efficiencydata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}