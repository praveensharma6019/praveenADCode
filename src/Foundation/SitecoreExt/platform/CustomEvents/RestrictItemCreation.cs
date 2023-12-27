using Sitecore.Events;
using Sitecore.Web.UI.Sheer;
using System;
using System.Linq;


namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.CustomEvents
{
    public class RestrictItemCreation
    {
        public void OnItemAdded(object sender, EventArgs args)
        {
            // Extract the event arguments
            Sitecore.Data.Items.Item addedArgs = Event.ExtractParameter(args, 0) as Sitecore.Data.Items.Item;
            if (addedArgs != null)
            {
                Sitecore.Diagnostics.Assert.IsNotNull(addedArgs, "item");
                if (addedArgs != null)
                {
                    var item = addedArgs;
                    var MaxChildCount = item.Parent.Fields["{A2C9334E-E6C8-48DE-A992-C0AD017058B7}"];
                    // NOTE: you may want to do additional tests here to ensure that the item
                    // descends from /sitecore/content/home
                    if (MaxChildCount != null)
                    {
                        if (!string.IsNullOrEmpty(MaxChildCount.Value))
                        {
                            var childCount = double.Parse(MaxChildCount.Value);
                            if (item.Parent != null &&
                            item.Parent.Children.Count() > childCount && childCount > 0)
                            {
                                // Delete the item, warn user
                                SheerResponse.Alert(
                                    String.Format("Sorry, you cannot add more than {0} items to '{1}'.", MaxChildCount.Value, item.Parent.Name), new string[0]);
                                item.Delete();
                            }
                        }
                    }
                }
            }
        }

        public void OnItemCopied(object sender, EventArgs args)
        {
            Sitecore.Data.Items.Item itemRootCreatedByCopy = Event.ExtractParameter(args, 1) as Sitecore.Data.Items.Item;
            if (itemRootCreatedByCopy.Parent != null)
            {
                var MaxChildCount = itemRootCreatedByCopy.Parent.Fields["{A2C9334E-E6C8-48DE-A992-C0AD017058B7}"];
                if (MaxChildCount != null)
                {
                    if (!string.IsNullOrEmpty(MaxChildCount.Value))
                    {
                        var childCount = double.Parse(MaxChildCount.Value);
                        if (itemRootCreatedByCopy.Parent != null &&
                        itemRootCreatedByCopy.Parent.Children.Count() > childCount && childCount > 0)
                        {
                            // Delete the item, warn user
                            SheerResponse.Alert(
                                String.Format("Sorry, you cannot add more than {0} items to '{1}'.", MaxChildCount.Value, itemRootCreatedByCopy.Parent.Name), new string[0]);
                            itemRootCreatedByCopy.Delete();
                        }
                    }
                }
            }
        }
    }
}