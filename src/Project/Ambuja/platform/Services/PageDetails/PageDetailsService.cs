using Glass.Mapper.Sc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AmbujaCement.Website.Services
{
    public class PageDetailsService : IPageDetailsService
    {
        private ISitecoreService _sitecoreService;

        public PageDetailsService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public PageDetailsModel GetDetails(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var PageDetailsModelData = _sitecoreService.GetItem<PageDetailsModel>(datasource);
                return PageDetailsModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}