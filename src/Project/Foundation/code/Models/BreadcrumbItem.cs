using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace Sitecore.Foundation.Website.Models
{
    public class BreadcrumbItem : CustomItem
    {
        public BreadcrumbItem(Item item)
          : base(item)
        {
            Assert.IsNotNull((object)item, nameof(item));
        }

        public string Title => ((BaseItem)((CustomItemBase)this).InnerItem)[nameof(Title)];

        public bool IsActive
        {
            get
            {
                bool d = Context.Item.ID == InnerItem.ID;
                return d;
            }
        }

        public string Url => LinkManager.GetItemUrl(((CustomItemBase)this).InnerItem);


    }
}
