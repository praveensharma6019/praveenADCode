using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Mvc.Presentation;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class BreadcrumbItem : CustomItem
    {
        public bool IsActive
        {
            get
            {
                bool d = Context.Item.ID == InnerItem.ID;
                return d;
            }
        }

        public string Title
        {
            get
            {
                return InnerItem["Title"];
            }
        }

        public string Url
        {
            get
            {
                return LinkManager.GetItemUrl(InnerItem);
            }
        }

        public BreadcrumbItem(Item item) : base(item)
        {
            Assert.IsNotNull(item, "item");
        }
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
            Breadcrumbs.Add(new BreadcrumbItem(Sitecore.Context.Item));
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