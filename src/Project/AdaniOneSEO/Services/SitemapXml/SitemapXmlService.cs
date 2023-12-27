using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.FlightsToChennai;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination.Banner;
using Project.AdaniOneSEO.Website.Models.SitemapXml;
using Project.AdaniOneSEO.Website.Models.VideoGallery;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AdaniOneSEO.Website.Services.SitemapXml
{
    public class SitemapXmlService : ISitemapXmlService
    {
        private readonly ISitecoreService _sitecoreService;
        public SitemapXmlService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public SitemapXmlModel GetSiteMapXML(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var Sitemapdata = _sitecoreService.GetItem<SitemapXmlModel>(datasource);
            return Sitemapdata;
        }

    }
}