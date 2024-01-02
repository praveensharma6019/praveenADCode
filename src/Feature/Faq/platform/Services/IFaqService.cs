using Adani.SuperApp.Realty.Feature.Faq.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Faq.Platform.Services
{
    public interface IFaqService
    {
        FaqList GetFaqData(Sitecore.Mvc.Presentation.Rendering rendering);
        LocationFaqList LocationPageFaqList(Sitecore.Mvc.Presentation.Rendering rendering);
        LocationFaqList SeoPageFaqList(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}