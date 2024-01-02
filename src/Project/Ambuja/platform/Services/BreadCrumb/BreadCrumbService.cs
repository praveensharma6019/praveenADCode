using Glass.Mapper.Sc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.BreadCrumb;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services
{
    public class BreadCrumbService : IBreadCrumbService
    {
        private ISitecoreService _sitecoreService;

        public BreadCrumbService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public BreadCrumbModel GetBreadcrumb(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var BreadCrumbModelData = _sitecoreService.GetItem<BreadCrumbModel>(datasource);
                return BreadCrumbModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}