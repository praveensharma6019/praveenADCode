using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniHousing.Website.Pipeline
{
    public class ValidateFormHandler : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var controller = controllerContext.HttpContext.Request.Form["fhController"];
            var action = controllerContext.HttpContext.Request.Form["fhAction"];

            return !string.IsNullOrWhiteSpace(controller)
                && !string.IsNullOrWhiteSpace(action)
                && controller == controllerContext.Controller.GetType().Name
                && methodInfo.Name == action;
        }
    }
}