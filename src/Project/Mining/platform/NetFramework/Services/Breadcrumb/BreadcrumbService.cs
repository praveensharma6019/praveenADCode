using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.HeaderModel;

namespace Project.Mining.Website.Services.Header
{
    public class BreadcrumbService : IBreadcrumbService
    {
        private readonly ISitecoreService _sitecoreService;
        public BreadcrumbService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public BreadcrumbModel GetBreadcrum(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<BreadcrumbModel>(datasource);
            return ourAirportdata;
        }
    }
}