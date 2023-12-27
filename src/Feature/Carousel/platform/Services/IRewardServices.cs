using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
   public interface IRewardServices
    {
        WidgetModel GetAdaniRewardsServices(Sitecore.Mvc.Presentation.Rendering rendering,string Location,string Type);
    }
}
