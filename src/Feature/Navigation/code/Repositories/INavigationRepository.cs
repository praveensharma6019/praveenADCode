namespace Sitecore.Feature.Navigation.Repositories
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Navigation.Models;

    public interface INavigationRepository
    {
        Item GetNavigationRoot(Item contextItem);
        NavigationItems GetAboutUSNavigation();
        NavigationItems GetBreadcrumb();
        NavigationItems GetPrimaryMenu();
        NavigationItem GetSecondaryMenuItem();
        NavigationItems GetLinkMenuItems(Item menuItem);

        NavigationItems GetLinkMenuItems(Item menuItem, int level, int maxLevel);

		NavigationItems GetRealtyMenuItems();

		Sitecore.Collections.ChildList GetPaymentNavigationLinkMenu();
        NavigationItems GetCustomerCare();
    }
}