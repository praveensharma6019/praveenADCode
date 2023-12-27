using Sitecore.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.ErrorHandling.Platform.ItemNotFoundHandler
{
    public class NotFoundProcessor : Sitecore.Pipelines.HttpRequest.HttpRequestProcessor
    {
        public override void Process(Sitecore.Pipelines.HttpRequest.HttpRequestArgs args)
        {
            if (Sitecore.Context.Item != null || Sitecore.Context.Site == null || Sitecore.Context.Database == null)
            {
                return;
            }
            if (string.Equals(Sitecore.Context.Site.Properties["enableCustomErrors"], "true", StringComparison.OrdinalIgnoreCase) == false)
            {
                return;
            }

            if (Sitecore.Context.Item == null)
            {
                var notFoundItem =
                    Sitecore.Context.Database.GetItem(string.Concat(Sitecore.Context.Site.StartPath,
                        Settings.ItemNotFoundUrl));
                if (notFoundItem != null)
                {
                    CustomContext.PageNotFound = true;
                    Sitecore.Context.Item = notFoundItem;
                }
                else
                {
                    CustomContext.PageNotFound = false;
                }
            }
        }
    }
}