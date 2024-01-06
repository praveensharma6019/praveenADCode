using Sitecore.Australia.Website.Models;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sitecore.Australia.Website.Controllers
{
    public class AustraliaController : Controller
    {
        // GET: Australia
        public ActionResult OurTeamList()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var DsOurTeam = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource;
            var parent = Sitecore.Context.Database.GetItem(DsOurTeam).Children;
            OurTeamList objList = new OurTeamList();
            foreach (var item in parent.InnerChildren)
            {
                OurTeams ObjTeam = new OurTeams();
                ObjTeam.Name = item["Name"];
                ObjTeam.Designation = item["Designation"];
                ObjTeam.Description = item["Description"];
                Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)item.Fields["Image"]);
                string url = Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem);
                ObjTeam.Image = url;
                objList.ListOurTeam.Add(ObjTeam);
            }
            return this.View(objList);
        }
    }
}