using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.Common
{
    public class FooterNew : IFooterNew
    {
        private readonly ISitecoreService _sitecoreService;

        public FooterNew(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public FooterNewModel GetFooterNew(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var footerModelData = _sitecoreService.GetItem<FooterNewModel>(datasource);
            return footerModelData;
        }
    }
}
