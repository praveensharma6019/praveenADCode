using Sitecore.AdaniWind.Website.Helper;
using Sitecore.AdaniWind.Website.Models;
using Sitecore.Diagnostics;
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
                Log.Info("Adani Wind | Contact us Validation Pass", "");
                ContactUsHelper contactUsHelper = new ContactUsHelper();
                result = new {status= contactUsHelper.SaveData(contactUs) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Log.Error("Adani Wind | Contact us Validation Failed", "");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}