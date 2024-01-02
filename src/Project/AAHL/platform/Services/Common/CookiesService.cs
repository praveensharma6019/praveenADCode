using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Models.Partnerships;
using Sitecore.Mvc.Presentation;
using System;
using System.Net;

namespace Project.AAHL.Website.Services.Common
{
    public class CookiesService : ICookiesService
    {
        private readonly IMvcContext _mvcContext;

        public CookiesService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public CookiesModel GetCookies(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var cookieModelData = _mvcContext.GetDataSourceItem<CookiesModel>();
            return cookieModelData;
        }
    }
}
