using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Models.Awards;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.MediaCenter;
using Project.AAHL.Website.Templates;
using Sitecore.ContentSearch.Linq.Nodes;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Shell.Framework.Commands.Shell;
using System;
using System.Collections.Generic;
using static Project.AAHL.Website.Templates.BaseTemplate;
using static Project.AAHL.Website.Templates.BaseTemplate.TitleTemplate;

namespace Project.AAHL.Website.Services.MediaCenter
{
    public class MediaCenterServices : IMediaCenterServices
    {

        private readonly ISitecoreService _sitecoreService;

        public MediaCenterServices(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public CenterRelease GetCenterRelease (Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ReleaseData = _sitecoreService.GetItem<CenterRelease>(datasource);
                return ReleaseData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CenterResources GetCenterResources(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ResourcesData = _sitecoreService.GetItem<CenterResources>(datasource);
                return ResourcesData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}