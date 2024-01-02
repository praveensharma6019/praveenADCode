using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Services.OurServices;
using Project.Mining.Website.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.OurServicesModel;

namespace Project.Mining.Website.Services.OurServices
{
    public class OurServices : IOurServices
    {
        private readonly ISitecoreService _sitecoreService;
        public OurServices(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }      

        public OurServicesModel GetOurServicesModel(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<OurServicesModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}