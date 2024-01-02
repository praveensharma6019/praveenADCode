using Glass.Mapper.Sc.Web.Mvc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services.SitemapXML
{
    public class SitemapXMLService : ISitemapXMLService
    {
        private readonly IMvcContext _mvcContext;
        public SitemapXMLService(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }

        public SiteMapXML GetSiteMapXML(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var getSitemapXMLModel = _mvcContext.GetDataSourceItem<SiteMapXML>();
                return getSitemapXMLModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}