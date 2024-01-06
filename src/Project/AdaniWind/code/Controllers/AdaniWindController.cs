using Sitecore.AdaniWind.Website.Helper;
using Sitecore.AdaniWind.Website.Models;
using System.Web.Mvc;

namespace Sitecore.AdaniWind.Website.Controllers
{
    public class AdaniWindController : Controller
    {
        [HttpPost]
        public ActionResult ContactUsForm (ContactUs contactUs)
        {
            var result = new {status="0"};
            if (ModelState.IsValid)
            {
                ContactUsHelper contactUsHelper = new ContactUsHelper();
                result = new {status= contactUsHelper.SaveData(contactUs) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}