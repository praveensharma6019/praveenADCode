using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;
using Sitecore.Data.Items;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Services
{
    public class NavigationRootResolver : INavigationRootResolver
    {
        public Item GetNavigationRoot(Item contextItem)
        {
            if (contextItem == null)
            {
                return null;
            }
            return contextItem.IsDerived(Templates.NavigationRoot.Id)
                ? contextItem
                : contextItem.Axes.GetAncestors().LastOrDefault(x => x.IsDerived(Templates.NavigationRoot.Id));
        }
    }

  
    
}

