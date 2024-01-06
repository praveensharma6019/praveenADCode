using System;
using System.Web.Mvc;

namespace Sitecore.MangaloreAirport.Website.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public ValidateModelAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewDataDictionary viewData = filterContext.Controller.ViewData;
            if (!viewData.ModelState.IsValid)
            {
                filterContext.Result = new ViewResult()
                {
                    ViewData = viewData,
                    TempData = filterContext.Controller.TempData
                };
            }
        }
    }
}