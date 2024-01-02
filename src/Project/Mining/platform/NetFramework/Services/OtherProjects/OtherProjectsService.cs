using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Models.ProjectDetails;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.HeaderModel;

namespace Project.Mining.Website.Services.Header
{
    public class OtherProjectsService : IOtherProjectsService
    {
        private readonly ISitecoreService _sitecoreService;
        public OtherProjectsService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public OtherProjectsModel GetOtherProject(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<OtherProjectsModel>(datasource);
            return ourAirportdata;
        }
    }
}