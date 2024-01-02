using Adani.SuperApp.Realty.Feature.Faq.Platform.Models;
using Sitecore.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.Services
{
    public interface IQuickLinksFaqService
    {
        QuickLinksFaqModel GetQuickLinksFaqData(Rendering rendering);
        
    }
}