using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public interface IResolveBannerJSON
    {
        WidgetModel GetBanner(Rendering rendering, string queryString, string storeType);
    }
}