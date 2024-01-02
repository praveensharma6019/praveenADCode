using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface ITitleDescImageDatasourceService
    {
        WidgetModel GetTitleDescImageDatasource(Sitecore.Mvc.Presentation.Rendering rendering);
    }
}