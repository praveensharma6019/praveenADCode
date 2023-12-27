using Adani.SuperApp.Airport.Feature.FAQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FAQ.Interfaces
{
    public interface IFAQ
    {
        FAQData GetFaqData(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}