using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.DealerResult;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.DealerResult
{
    public class DealerResultService : IDealerResultService
    {
        private readonly IMvcContext _mvcContext;
        public DealerResultService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public DealerResultModel GetDealerResultModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getDealerResult = _mvcContext.GetDataSourceItem<DealerResultModel>();
                return getDealerResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}