using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IMoreServicesService
    {
        WidgetModel GetMoreServicesService(Sitecore.Mvc.Presentation.Rendering rendering, string queryString);
    }
}