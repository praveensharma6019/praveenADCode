using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.MetaData.Platform.Models;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Feature.MetaData.Platform;
using System.Text;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Controllers
{
    public class HeadDataController : Controller
    {
        // GET: HeadData
        public ActionResult GetCssLinks()
        {
            var model = new JSandCss();
            List<string> cssList = new List<string>();
            Item contextItem = Sitecore.Context.Item;
            string[] css = (contextItem.Fields[Templates.HeadDataCollection.css].Value).Split(',');
            int i;
            StringBuilder cssItems = new StringBuilder();
            for(i = 0; i < css.Length; i++)
            {
                cssItems.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" />", css[i]).AppendLine();
            }
            model.AssetItems = cssItems.ToString();
            return View("~/views/HeadData/RenderAssets.cshtml", model);
        }

        public ActionResult GetJavaScriptLinks()
        {
            var model = new JSandCss();
            Item contextItem = Sitecore.Context.Item;
            string[] javaScripts = (contextItem.Fields[Templates.HeadDataCollection.JavaScripts].Value).Split(',');
            int i;
            StringBuilder cssItems = new StringBuilder();
            for (i = 0; i < javaScripts.Length; i++)
            {
                cssItems.AppendFormat("<script src=\"{0}\"></script> ", javaScripts[i]).AppendLine();
            }
            model.AssetItems = cssItems.ToString();
            return View("~/views/HeadData/RenderAssets.cshtml", model);
        }
    }
}