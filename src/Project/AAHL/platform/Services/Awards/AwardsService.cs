using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Models.Awards;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Templates;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell.Framework.Commands.Shell;
using System;
using System.Collections.Generic;
using static Project.AAHL.Website.Templates.BaseTemplate;
using static Project.AAHL.Website.Templates.BaseTemplate.TitleTemplate;

namespace Project.AAHL.Website.Services.Common
{
    public class AwardsService : IAwardsService
    {

        private readonly ISitecoreService _sitecoreService;

        public AwardsService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public AwardsAccolades GetAwardsAccolades(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<AwardsAccolades>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public SearchModel GetSearch(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<SearchModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      
    }
}