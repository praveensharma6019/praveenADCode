using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;

namespace Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper
{
    public static class ItemExtensions
    {
        /// <summary>
        /// Determine if the Item inherits a specific template, by ID
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="templateId">The template id</param>
        /// <returns><c>True</c> if the item inherits from <paramref name="templateId"/></returns>
        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null)
            {
                return false;
            }

            return !templateId.IsNull && item.IsDerived(item.Database.GetItem(templateId, item.Language));
        }

        /// <summary>
        /// Determine if the Item inherits a specific template, by TemplateItem
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="inheritedTemplateItem">the templateItem</param>
        /// <returns><c>True</c> if the item inherits from <paramref name="inheritedTemplateItem"/></returns>
        public static bool IsDerived(this Item item, Item inheritedTemplateItem)
        {
            if (item == null
                || inheritedTemplateItem == null)
            {
                return false;
            }

            var itemTemplate = TemplateManager.GetTemplate(item);

            return itemTemplate != null && (itemTemplate.ID == inheritedTemplateItem.ID || itemTemplate.InheritsFrom(inheritedTemplateItem.ID));
        }
    }

   
}