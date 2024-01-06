namespace Sitecore.Feature.Navigation.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Navigation.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Mvc.Presentation;

    [Service(typeof(INavigationRepository), Lifetime = Lifetime.Transient)]
    public class NavigationRepository : INavigationRepository
    {
        public Item ContextItem => RenderingContext.Current?.ContextItem ?? Sitecore.Context.Item;

        public Item NavigationRoot { get; }

        public NavigationRepository()
        {
            this.NavigationRoot = this.GetNavigationRoot(this.ContextItem);
            if (this.NavigationRoot == null)
            {
                throw new InvalidOperationException($"Cannot determine navigation root from '{this.ContextItem.Paths.FullPath}'");
            }
        }

        public Item GetNavigationRoot(Item contextItem)
        {
            return contextItem.GetAncestorOrSelfOfTemplate(Templates.NavigationRoot.ID) ?? Context.Site.GetContextItem(Templates.NavigationRoot.ID);
        }

        public NavigationItems GetBreadcrumb()
        {
            var items = new NavigationItems
            {
                NavItems = this.GetNavigationHierarchy(true).Reverse().ToList()
            };

            for (var i = 0; i < items.NavItems.Count - 1; i++)
            {
                items.NavItems[i].Level = i;
                items.NavItems[i].IsActive = i == items.NavItems.Count - 1;
            }

            return items;
        }

        public NavigationItems GetPrimaryMenu()
        {
            var navItems = this.GetChildNavigationItems(this.NavigationRoot, 0, 1);

            this.AddRootToPrimaryMenu(navItems);
            return navItems;
        }

        private void AddRootToPrimaryMenu(NavigationItems navItems)
        {
            if (!this.IncludeInNavigation(this.NavigationRoot))
            {
                return;
            }

            var navigationItem = this.CreateNavigationItem(this.NavigationRoot, 0, 0);
            //Root navigation item is only active when we are actually on the root item
            navigationItem.IsActive = this.ContextItem.ID == this.NavigationRoot.ID;
            navItems?.NavItems?.Insert(0, navigationItem);
        }
		private bool IncludeResidentail(Item item, bool forceShowInMenu = false)
		{
			return item.HasContextLanguage() && item.IsDerived(Templates.RealtyResidentail.ID);
		}

		private bool IncludeCommercial(Item item, bool forceShowInMenu = false)
		{
			return item.HasContextLanguage() && item.IsDerived(Templates.RealtyCommercial.ID);
		}

		private bool IncludeInNavigation(Item item, bool forceShowInMenu = false)
        {
            return item.HasContextLanguage() && item.IsDerived(Templates.Navigable.ID) && (forceShowInMenu || MainUtil.GetBool(item[Templates.Navigable.Fields.ShowInNavigation], false));
        }

        public NavigationItem GetSecondaryMenuItem()
        {
            var rootItem = this.GetSecondaryMenuRoot();
            return rootItem == null ? null : this.CreateNavigationItem(rootItem, 0, 3);
        }

        public NavigationItems GetLinkMenuItems(Item menuRoot)
        {
            if (menuRoot == null)
            {
                throw new ArgumentNullException(nameof(menuRoot));
            }
            return this.GetChildNavigationItems(menuRoot, 0, 0);
        }

        public NavigationItems GetLinkMenuItems(Item menuRoot, int level, int maxLevel)
        {
            if (menuRoot == null)
            {
                throw new ArgumentNullException(nameof(menuRoot));
            }
            return this.GetChildNavigationItems(menuRoot, level, maxLevel);
        }

		public NavigationItems GetRealtyMenuItems()
		{
			NavigationItems navItem = new NavigationItems();
			List<NavigationItem> navItems = new List<NavigationItem>();

			Item parentItem = Sitecore.Context.Site.GetStartItem();
            //var childItems = parentItem.Children.Where(item => this.IncludeResidentail(item) || this.IncludeCommercial(item)).Select(i => this.CreateNavigationItem(i, 0, 0));
            var childItems = parentItem.Axes.GetDescendants().Where(item => item.TemplateID == Templates.RealtyResidentail.ID || item.TemplateID == Templates.RealtyCommercial.ID).Select(i => this.CreateNavigationItem(i, 0, 0));
            navItems.AddRange(childItems.ToList());

			//parentItem = Sitecore.Context.Database.GetItem(Templates.RealtyCommercialMenuRoot.ID);
			//childItems = parentItem.Children.Where(item => this.IncludeCommercial(item)).Select(i => this.CreateNavigationItem(i, 0, 0));
			//navItems.AddRange(childItems.ToList());

            navItem.NavItems = navItems.ToList();

			return navItem;
		}

		private Item GetSecondaryMenuRoot()
        {
            return this.FindActivePrimaryMenuItem();
        }

        private Item FindActivePrimaryMenuItem()
        {
            var primaryMenuItems = this.GetPrimaryMenu();
            //Find the active primary menu item
            var activePrimaryMenuItem = primaryMenuItems.NavItems.FirstOrDefault(i => i.Item.ID != this.NavigationRoot.ID && i.IsActive);
            return activePrimaryMenuItem?.Item;
        }

        private IEnumerable<NavigationItem> GetNavigationHierarchy(bool forceShowInMenu = false)
        {
            var item = this.ContextItem;
            while (item != null)
            {
                if (this.IncludeInNavigation(item, forceShowInMenu))
                {
                    yield return this.CreateNavigationItem(item, 0);
                }

                item = item.Parent;
            }
        }

        private NavigationItem CreateNavigationItem(Item item, int level, int maxLevel = -1)
        {
            var targetItem = item.IsDerived(Templates.Link.ID) ? item.TargetItem(Templates.Link.Fields.Link) : item;
            string url = item.IsDerived(Templates.Link.ID) ? item.LinkFieldUrl(Templates.Link.Fields.Link) : item.Url();
            if (!String.IsNullOrEmpty(item.Fields[Templates.LinkMenuItem.Fields.LinkToSection].ToString()))
                url = url + item.Fields[Templates.LinkMenuItem.Fields.LinkToSection].ToString();
            return new NavigationItem
            {
                Item = item,
                Url = url,
                IconCss = item.Fields[Templates.LinkMenuItem.Fields.Icon].ToString(),
                Target = item.IsDerived(Templates.Link.ID) ? item.LinkFieldTarget(Templates.Link.Fields.Link) : "",
                IsActive = this.IsItemActive(targetItem ?? item),
                Children = this.GetChildNavigationItems(item, level + 1, maxLevel),
                ShowChildren = !item.IsDerived(Templates.Navigable.ID) || item.Fields[Templates.Navigable.Fields.ShowChildren].IsChecked(),
                IsNewTab = item.Fields[Templates.Navigable.Fields.ShowInNewTab].IsChecked(),
                IsVisible = item.Fields[Templates.Navigable.Fields.IsVisible].IsChecked(),
                IsExternalLink = item.Fields[Templates.Navigable.Fields.IsExternalLink].IsChecked(),
                Link = item.LinkFieldUrl(Templates.Navigable.Fields.Link).ToString(),
                BGCSS = item.Fields[Templates.LinkMenuItem.Fields.BGCSS].ToString()
            };
        }

		private NavigationItems GetChildNavigationItems(Item parentItem, int level, int maxLevel)
        {
            if (level > maxLevel || !parentItem.HasChildren)
            {
                return null;
            }
            var childItems = parentItem.Children.Where(item => this.IncludeInNavigation(item)).Select(i => this.CreateNavigationItem(i, level, maxLevel));
            return new NavigationItems
            {
                NavItems = childItems.ToList()
            };
        }

        private bool IsItemActive(Item item)
        {
            return this.ContextItem.ID == item.ID || this.ContextItem.Axes.GetAncestors().Any(a => a.ID == item.ID);
        }

        public NavigationItems GetAboutUSNavigation()
        {
            var item = RenderingContext.Current.Rendering.Item;

            var childItem = item.Children;

            Database database = Sitecore.Context.Database;
            NavigationItems navItems = new NavigationItems();
            List<NavigationItem> navlist = new List<NavigationItem>();


            for (int i = 0; i < childItem.Count; i++)
            {
                var fld = database.GetItem(childItem[i].ID);
                fld.Fields.ReadAll();

                string value = fld.Fields["Show navigation in about us page"].Value.ToString();
                if (value != "")
                {
                    NavigationItem SummaryItem = new NavigationItem { Summary = fld.Fields["Summary"].Value.ToString(), IsMessage = fld.Fields["Is Message"].Value.ToString() };
                    navlist.Add(SummaryItem);
                }
            }


            return new NavigationItems
            {
                NavItems = navlist.ToList()
            };
        }




        public Sitecore.Collections.ChildList GetPaymentNavigationLinkMenu()
        {
            var item = RenderingContext.Current.Rendering.Item;
            var childItem = item.Children;
            return childItem;
        }

        public NavigationItems GetCustomerCare()
        {
            var item = RenderingContext.Current.Rendering.Item;
            var childItem = item.Children;
            //return childItem;

            Database database = Sitecore.Context.Database;
            NavigationItems navItems = new NavigationItems();
            List<NavigationItem> navlist = new List<NavigationItem>();

            for (int i = 0; i < childItem.Count; i++)
            {
                NavigationItem SummaryItem = new NavigationItem { Item = childItem[i] };
                navlist.Add(SummaryItem);
            }

            return new NavigationItems
            {
                NavItems = navlist.ToList()
            };


        }


    }
}