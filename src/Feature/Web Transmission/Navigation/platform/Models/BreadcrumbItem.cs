using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Models
{
    public class BreadcrumbItem : CustomItem
    {
        public BreadcrumbItem(Item item)
                : base(item) { Assert.IsNotNull(item, "item"); }

        public string Title
        { get { return InnerItem["Title"]; } }

        public bool IsActive
        { get { return Sitecore.Context.Item.ID == InnerItem.ID; } }

        public string Url
        { get { return LinkManager.GetItemUrl(InnerItem); } }
    }

    public class BreadcrumbItemList : RenderingModel
    {
        public List<BreadcrumbItem> Breadcrumbs { get; set; }
        public override void Initialize(Rendering rendering)
        {
            Breadcrumbs = new List<BreadcrumbItem>();
            List<Item> items = GetBreadcrumbItems();
            foreach (Item item in items)
            {
                Breadcrumbs.Add(new BreadcrumbItem(item));
            }
            string homePath = Sitecore.Context.Site.StartPath;
            Item homeItem = Sitecore.Context.Database.GetItem(homePath);
            if (homeItem.ID != Sitecore.Context.Item.ID)
            {
                Breadcrumbs.Add(new BreadcrumbItem(Sitecore.Context.Item));
            }
        }

        private List<Sitecore.Data.Items.Item> GetBreadcrumbItems()
        {
            string homePath = Sitecore.Context.Site.StartPath;
            Item homeItem = Sitecore.Context.Database.GetItem(homePath);
            List<Item> items = Sitecore.Context.Item.Axes.GetAncestors()
              .SkipWhile(item => item.ID != homeItem.ID)
              .ToList();
            return items;
        }
    }


}