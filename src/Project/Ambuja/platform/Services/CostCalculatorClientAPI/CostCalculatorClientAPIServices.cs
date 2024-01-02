using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.CostCalculatorClientAPI;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.CostCalculatorClientAPI
{
    public class CostCalculatorClientAPIServices : ICostCalculatorClientAPIServices
    {
        private readonly IMvcContext _mvcContext;
        public CostCalculatorClientAPIServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }
        public CostCalculatorClientAPIModel GetCostCalculatorClientAPIData(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getClientAPIModel = _mvcContext.GetDataSourceItem<CostCalculatorClientAPIModel>();
                return getClientAPIModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ClientAPIDropdownModel GetCostCalculatorClientAPIDropDownData(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getClientAPIDropDown = _mvcContext.GetDataSourceItem<ClientAPIDropdownModel>();
                return getClientAPIDropDown;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}