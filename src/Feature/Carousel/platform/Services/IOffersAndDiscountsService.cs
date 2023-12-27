using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;


namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface IOffersAndDiscountsService
    {
        WidgetModel OffersAndDiscountsData(Sitecore.Mvc.Presentation.Rendering rendering, Item datasource, string queryString, string cityqueryString);
    }
}
