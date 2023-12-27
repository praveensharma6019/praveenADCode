using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface ILeftImageWithDetails
    {
        WidgetModel LeftImageWithDetailsData(Sitecore.Mvc.Presentation.Rendering rendering, Item datasource);
    }
}