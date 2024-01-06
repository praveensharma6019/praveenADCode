using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;

namespace Sitecore.SportsLine.Website.Models
{
    public class BreadcrumbItem : CustomItem
    {
        public bool IsActive
        {
            get
            {
                bool d = Context.Item.ID == base.InnerItem.ID;
                return d;
            }
        }

        public string Title
        {
            get
            {
                return base.InnerItem["Title"];
            }
        }

        public string Url
        {
            get
            {
                return LinkManager.GetItemUrl(base.InnerItem);
            }
        }

        public BreadcrumbItem(Item item) : base(item)
        {
            Assert.IsNotNull(item, "item");
        }
    }
}