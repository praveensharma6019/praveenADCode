using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.DealerResult;
using Project.AmbujaCement.Website.Models.FAQ;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.FAQ
{
    public class FAQService : IFAQService
    {
        private readonly IMvcContext _mvcContext;
        public FAQService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public FAQModel GetFAQModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getFAQDetails = _mvcContext.GetDataSourceItem<FAQModel>();
                return getFAQDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}