using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Mvc;
using Sitecore.AVRPL.Website.Models;

namespace Sitecore.AVRPL.Website.Controllers
{

    public class AVRPLController : Controller
    {
        [HttpGet]
        public ActionResult PhotoGalleryLoadMore(string count,string element)
        {
            AdaniRoadGallery AdaniRoadGalleryData = new AdaniRoadGallery();
            try
            {
                if (string.IsNullOrWhiteSpace(count) || string.IsNullOrWhiteSpace(element))
                {
                    return null;
                }
                int pageCount = int.Parse(count);
                var db = Sitecore.Configuration.Factory.GetDatabase("web");
                var mmyyItem = db.GetItem(element);
                if (mmyyItem != null)
                {
                    var mmyy = mmyyItem.GetChildren().OrderByDescending(x => x.Fields["__Created"].Value).ToList();
                    if (pageCount <= mmyy.Count)
                    {
                        AdaniRoadGalleryData.MonthYearName = mmyy[pageCount].Name;

                        foreach (Item items in mmyy[pageCount].GetChildren())
                        {
                            RoadImages AdaniRoadImagess = new RoadImages();
                            Sitecore.Data.Items.MediaItem sampleMedia = new Sitecore.Data.Items.MediaItem(items);
                            var urls = Sitecore.Resources.Media.MediaManager.GetMediaUrl(sampleMedia);
                            AdaniRoadImagess.ImageUrl = urls;
                            AdaniRoadGalleryData.AdaniRoadImages.Add(AdaniRoadImagess);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }            
            catch (Exception e)
            {
                Log.Error("Error", e.Message);
                return null;
            }
            return Json(AdaniRoadGalleryData, JsonRequestBehavior.AllowGet);

        }
    }
}
    
