using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Adani.BAU.Transmission.Feature.Media.Platform.Services;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Controllers
{
    public class RichTextController : Controller
    {
        Helper obj = new Helper();
        // GET: RichText
        public ActionResult RichTextComponent()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            BodyContentModel model = new BodyContentModel();
            try
            {
                if (dataSource != null )
                {
                    model.RichText = !string.IsNullOrEmpty(dataSource.Fields["RichText"].Value)? dataSource.Fields["RichText"].Value:string.Empty;
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }

            return View("~/views/RichText/BodyContentView.cshtml", model);
        }
    }
}