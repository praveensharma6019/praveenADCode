using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniInstitute.Website.Controllers
{
    public class AdaniInstituteController : Controller
    {
        // GET: AdaniInstitute
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetSearchResult(string itemId)
        {

            var homeItem = Sitecore.Context.Database.GetItem("/sitecore/content/AdaniInstitute/Home");
            var item = homeItem.Axes.GetDescendants().Where(p => p.Name.ToLower().Contains(itemId)).ToList();

            var list = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < item.Count(); i++)
            {
                list.Add(new KeyValuePair<string, string>(item[i].Name, item[i].Paths.ContentPath));
            }

            return Json(list);
        }

    }
}