using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.OurAccreditationModel;

namespace Project.Mining.Website.Services.OurAccreditation
{
    public class OurAccreditationService : IOurAccreditationService
    {
        private readonly ISitecoreService _sitecoreService;
        public OurAccreditationService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public OurAccreditationModel GetOurAccreditation(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var ourAirportdata = _sitecoreService.GetItem<OurAccreditationModel>(datasource);
            return ourAirportdata;
        }
    }
}