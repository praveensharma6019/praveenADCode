using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AGELPortal.Website.Attributes
{
    public sealed class RedirectResultNoBody : ActionResult
    {
        private readonly string location;
        public RedirectResultNoBody(string location)
        {
            this.location = location;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.StatusCode = 302;
            response.RedirectLocation = location;
            response.End();
        }
    }
}