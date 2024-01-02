using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.Partnerships;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Common
{
    public class Partnerships : IPartnership
    {

        private readonly IMvcContext _mvcContext;

        public Partnerships(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }


        public TravelServices GetTravelServices(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<TravelServices>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }  
       

    }
}