using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Services
{
    public interface INavigationRootResolver
    {
        Item GetNavigationRoot(Item contextItem);
    }
}
