using Project.AmbujaCement.Website.Models.Common;
using Sitecore.Mvc.Presentation;
using System;
using Glass.Mapper.Sc;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models;

namespace Project.AmbujaCement.Website.Services.Common
{
    public class ChecklistService : IChecklistService
    {
        private ISitecoreService _sitecoreService;
        public ChecklistService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public ChecklistModel GetChecklist(Rendering rendering)
        {
            try{
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;
                var Data = _sitecoreService.GetItem<ChecklistModel>(datasource);
                return Data;
            }
            catch (Exception ex){throw ex;}
        }
        public CommonItemModel GetItemData(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;
                var DataItem = _sitecoreService.GetItem<CommonItemModel>(datasource);
                return DataItem;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}