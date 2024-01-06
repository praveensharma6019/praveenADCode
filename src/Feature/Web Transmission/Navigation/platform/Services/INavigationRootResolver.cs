using Sitecore.Data.Items;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Services
{
    public interface INavigationRootResolver
    {
        Item GetNavigationRoot(Item contextItem);
    }
}
