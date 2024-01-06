using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.KKRPL.Website.Models;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sitecore.KKRPL.Website.Controllers
{
    public class KKRPLController : Controller
    {
        [HttpGet]
        public ActionResult PhotoGalleryLoadMore(string count, string element)
        {
            AdaniRoadGallery adaniRoadGallery = new AdaniRoadGallery();
            try
            {
                if (string.IsNullOrWhiteSpace(count) || string.IsNullOrWhiteSpace(element))
                    return (ActionResult)null;
                int index = int.Parse(count);
                Item obj = Factory.GetDatabase("web").GetItem(element);
                if (obj == null)
                    return (ActionResult)null;
                List<Item> list = ((IEnumerable<Item>)((IEnumerable<Item>)obj.GetChildren()).OrderByDescending<Item, string>((Func<Item, string>)(x => ((BaseItem)x).Fields["__Created"].Value))).ToList<Item>();
                if (index > list.Count)
                    return (ActionResult)null;
                adaniRoadGallery.MonthYearName = list[index].Name;
                foreach (Item child in list[index].GetChildren())
                    adaniRoadGallery.AdaniRoadImages.Add(new RoadImages()
                    {
                        ImageUrl = MediaManager.GetMediaUrl(new MediaItem(child))
                    });
            }
            catch (Exception ex)
            {
                Log.Error("Error", (object)ex.Message);
                return (ActionResult)null;
            }
            return (ActionResult)this.Json((object)adaniRoadGallery, (JsonRequestBehavior)0);
        }
    }
}
