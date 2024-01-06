using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Electricity.Website.Controllers
{
    public class MoneySaverController : Controller
    {
        // GET: MoneySaver
        public ActionResult Survey()
        {
            return View();
        }
    }
}