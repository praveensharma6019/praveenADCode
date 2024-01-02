using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public interface ITitleDescriptionWithLinkList
    {
        WidgetModel TitleDescriptionWithLinkListData(Sitecore.Mvc.Presentation.Rendering rendering, Item datasource);
    }
}