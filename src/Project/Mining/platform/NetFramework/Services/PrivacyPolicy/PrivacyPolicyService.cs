using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Services;
using Project.Mining.Website.Services.Banner;
using Project.Mining.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.Mining.Website.Models.BannerModel;

namespace Project.Mining.Website.Services
{
    public class PrivacyPolicyService : IPrivacyPolicyService
    {
        private readonly ISitecoreService _sitecoreService;
        public PrivacyPolicyService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public PrivacyPolicy GetPrivacyDetail(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var privacydata = _sitecoreService.GetItem<PrivacyPolicy>(datasource);
            return privacydata;
        }
        public TermsandConditions GetTermsDetail(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var privacydata = _sitecoreService.GetItem<TermsandConditions>(datasource);
            return privacydata;
        }
    }
}