namespace Sitecore.Feature.PageContent.Repositories
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.PageContent.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Mvc.Presentation;
    using System;

    [Service(typeof(IQuickAccessFuncRepository), Lifetime = Lifetime.Transient)]
    public class QuickAccessFuncRepository : IQuickAccessFuncRepository
    {

        public Item ContextItem => RenderingContext.Current?.ContextItem ?? Sitecore.Context.Item;

        public Item NavigationRoot { get; }

        public QuickAccessFuncRepository()
        {
            this.NavigationRoot = this.GetNavigationRoot(this.ContextItem);
            if (this.NavigationRoot == null)
            {
                throw new InvalidOperationException($"Cannot determine navigation root from '{this.ContextItem.Paths.FullPath}'");
            }
        }
        public Item GetNavigationRoot(Item contextItem)
        {
            return contextItem.GetAncestorOrSelfOfTemplate(Templates.HasPageContent.ID) ?? Context.Site.GetContextItem(Templates.HasPageContent.ID);
        }

        //public QuickAccessFuncItems GetQuickAccessFun(Item menuRoot)
        //{
        //    if (menuRoot == null)
        //    {
        //        throw new ArgumentNullException(nameof(menuRoot));
        //    }
        //    return this.GetChildNavigationItems(menuRoot, 0, 0);
        //}

        //private QuickAccessFuncItems GetChildNavigationItems(Item parentItem, int level, int maxLevel)
        //{
        //    if (level > maxLevel || !parentItem.HasChildren)
        //    {
        //        return null;
        //    }
        //    var childItems = parentItem.Children;
        //    return new QuickAccessFuncItems
        //    {
        //        NavItems = childItems.ToList()
        //    };
        //}
    }
}