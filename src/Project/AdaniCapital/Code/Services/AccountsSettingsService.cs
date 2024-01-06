using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Foundation.SitecoreExtensions.Extensions;

namespace Sitecore.AdaniCapital.Website.Services
{
    [Service(typeof(IAccountsSettingsService))]
    public class AccountsSettingsService : IAccountsSettingsService
    {
        public static readonly string PageNotFoundUrl = "/page-not-found";
        public virtual string GetPageLink(Item contextItem, ID fieldID)
        {
            var item = this.GetAccountsSettingsItem(contextItem);
            if (item == null)
            {
                throw new Exception("Page with accounts settings isn't specified");
            }

            InternalLinkField link = item.Fields[fieldID];
            if (link.TargetItem == null)
            {
                throw new Exception($"{link.InnerField.Name} link isn't set");
            }

            return link.TargetItem.Url();
        }
        public virtual string GetPageLinkOrDefault(Item contextItem, ID field, Item defaultItem = null)
        {
            try
            {
                return this.GetPageLink(contextItem, field);
            }
            catch (Exception ex)
            {
                Log.Warn(ex.Message, ex, this);
                return defaultItem?.Url() ?? PageNotFoundUrl;
            }
        }
        public virtual Item GetAccountsSettingsItem(Item contextItem)
        {
            Item item = null;

            if (contextItem != null)
            {
                item = contextItem.GetAncestorOrSelfOfTemplate(Templates.AccountsSettings.ID);
            }
            item = item ?? Context.Site.GetContextItem(Templates.AccountsSettings.ID);

            return item;
        }
    }    
}