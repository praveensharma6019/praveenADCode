using Sitecore.Pipelines.HttpRequest;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper
{
    public class ResolveItem : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            if (Convert.ToString(args.RequestUrl).Contains("/sitecore/api/layout/render"))
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri; 
                if (Convert.ToString(url).Contains("%20")) 
                {
                    HttpContext.Current.Response.Redirect("https://www.adanirealty.com/404");
                }
                var layout_API_Pages = Sitecore.Context.Database.GetItem("{A2953887-1736-42B0-ACAB-A62C2B57A989}");
                System.Collections.Specialized.NameValueCollection nameValueCollection = layout_API_Pages != null ? Sitecore.Web.WebUtil.ParseUrlParameters(layout_API_Pages["API"]) : null;
                if (nameValueCollection != null)
                {
                    foreach (var nv in nameValueCollection)
                    {
                        var queryItem = HttpUtility.ParseQueryString(args.RequestUrl.Query).Get("item");
                        queryItem = queryItem != null && queryItem.Contains("/") ? queryItem.Replace("/", "") : queryItem;
                        queryItem = queryItem != null && queryItem.Contains(".") ? queryItem.Replace(".", "_") : queryItem;

                        if (nv != null && Convert.ToString(nv).ToLower() == queryItem.ToLower())
                        {
                            var innerItemName = nameValueCollection[Convert.ToString(nv)];
                            var InnerItem = Sitecore.Context.Database.GetItem(innerItemName);
                            Sitecore.Context.Item = InnerItem;
                        }
                    }
                }
            }

        }
    }
}