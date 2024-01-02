using Glass.Mapper.Sc.Web.Mvc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models.Sitemap;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.Mining.Website.Services.Sitemap
{
    public class SitemapService : ISitemapService
    {
        private readonly IMvcContext _mvcContext;
        public SitemapService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public SitemapModel GetSitemapData(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var sitemapDataModel = _mvcContext.GetDataSourceItem<SitemapModel>();
                return sitemapDataModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}