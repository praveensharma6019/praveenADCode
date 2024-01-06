using Sitecore.Foundation.SitecoreExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.PageContent.Controllers
{
    [SkipAnalyticsTracking]
    public class PageContentController : Controller
    {
        // GET: PageContent
        public PageContentController()
        {

        }

        [HttpPost]
        [ValidateRenderingId]
        public JsonResult GetDTWiseRooftopSolar(string consumerNumber)
        {
           // var Location = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId));
           // string body = Location.Fields["Body"].Value;
            return Json(consumerNumber);
        }
    }
}