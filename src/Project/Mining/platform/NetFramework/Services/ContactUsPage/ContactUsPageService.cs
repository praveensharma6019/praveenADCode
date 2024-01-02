using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models.ContactUsPage;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Services.ContactUsPage
{
    public class ContactUsPageService : IContactUsPageService
    {
        private readonly ISitecoreService _sitecoreService;
        public ContactUsPageService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public ContactUsBanner GetContactUsBanner(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var contactUsBannerdata = _sitecoreService.GetItem<ContactUsBanner>(datasource);
            return contactUsBannerdata;
        }
        public ContactUsPageDetails GetContactUsPageDetails(Rendering rendering)
        {
            var datasource = Utils.GetRenderingDatasource(rendering);
            if (datasource == null) return null;

            var contactUsPagedata = _sitecoreService.GetItem<ContactUsPageDetails>(datasource);
            return contactUsPagedata;
        }
    }
   
}