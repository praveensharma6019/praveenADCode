using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models.Home;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.Mining.Website.Services.Home
{
    public class SubscribeUsService : ISubscribeUsService
    {
       
        private readonly ISitecoreService _sitecoreService;
        public SubscribeUsService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public SubscribeUsModel GetSubscribeUs(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<SubscribeUsModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}