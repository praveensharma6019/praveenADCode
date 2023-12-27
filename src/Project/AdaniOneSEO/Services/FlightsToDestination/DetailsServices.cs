using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination.Banner;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination.Details;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination
{
    public class DetailsServices : IDetailsServices
    {
        private readonly IMvcContext _mvcContext;

        public DetailsServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }


        public DetailsModel GetDetails(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<DetailsModel>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}