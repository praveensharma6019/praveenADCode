namespace Sitecore.Feature.Demo.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Analytics;
    using Sitecore.DependencyInjection;
    using Sitecore.ExperienceEditor.Utils;
    using Sitecore.ExperienceExplorer.Core.State;
    
    using Sitecore.Foundation.SitecoreExtensions.Attributes;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Mvc.Controllers;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Sites;

    [SkipAnalyticsTracking]
    public class PaymentController : SitecoreController
    {
        public PaymentController()
        {
            
        }

        [HttpPost]
        [ValidateRenderingId]
        public JsonResult GetCustomerCare(string itemId)
        {
            var Location = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId));
            string body = Location.Fields["Body"].Value;
            return Json(body);
        }

    }
}