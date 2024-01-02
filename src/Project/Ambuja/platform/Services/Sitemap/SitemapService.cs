using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.SiteMap;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.Sitemap
{
    public class SitemapService : ISitemapService
    {
        private readonly IMvcContext _mvcContext;
        public SitemapService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public SitemapDataModel GetSitemapDataModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var sitemapDataModel = _mvcContext.GetDataSourceItem<SitemapDataModel>();
                return sitemapDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}