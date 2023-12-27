using Glass.Mapper.Sc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.FlightsToChennai;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniOneSEO.Website.Services
{
    public class PageMetaData : IPageMetaData
    {
        private readonly ISitecoreService _sitecoreService;
        public PageMetaData(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public PageMetaDataModel GetPageMetaData(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var data = _sitecoreService.GetItem<PageMetaDataModel>(datasource);
            return data;
        }
    }
}