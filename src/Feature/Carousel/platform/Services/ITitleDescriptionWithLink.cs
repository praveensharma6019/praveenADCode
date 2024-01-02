using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface ITitleDescriptionWithLink
    {
        WidgetModel AboutAirport(Sitecore.Mvc.Presentation.Rendering rendering, Item datasource);
    }
}
