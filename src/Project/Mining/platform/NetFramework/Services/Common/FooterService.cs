using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models.Common;
using Project.Mining.Website.Models.ProjectListing;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Services.Common
{
    public class FooterService : IFooterService
    {
        private readonly ISitecoreService _sitecoreService;
        public FooterService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public FooterServiceModel GetFooterServiceComponent(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<FooterServiceModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}