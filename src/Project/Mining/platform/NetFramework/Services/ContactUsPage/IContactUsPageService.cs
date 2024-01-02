using Project.Mining.Website.Models.ContactUsPage;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Mining.Website.Services.ContactUsPage
{
    public interface IContactUsPageService
    {
        ContactUsPageDetails GetContactUsPageDetails(Rendering rendering);
        ContactUsBanner GetContactUsBanner(Rendering rendering);
    }
}