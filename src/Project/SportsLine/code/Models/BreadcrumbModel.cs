using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sitecore.SportsLine.Website.Models
{
    public class BreadcrumbModel : RenderingModel
    {
        public List<BreadcrumbItem> Breadcrumbs
        {
            get;
            set;
        }

        public BreadcrumbModel()
        {
        }

        private List<Item> GetBreadcrumbItems()
        {
            string startPath = Context.Site.StartPath;
            Item item1 = Context.Database.GetItem(startPath);
            List<Item> list = Context.Item.Axes.GetAncestors().SkipWhile<Item>((Item item) => item.ID != item1.ID).ToList<Item>();
            return list;
        }

        public override void Initialize(Rendering rendering)
        {
            this.Breadcrumbs = new List<BreadcrumbItem>();
            foreach (Item breadcrumbItem in this.GetBreadcrumbItems())
            {
                this.Breadcrumbs.Add(new BreadcrumbItem(breadcrumbItem));
            }
            this.Breadcrumbs.Add(new BreadcrumbItem(Context.Item));
        }
    }
}