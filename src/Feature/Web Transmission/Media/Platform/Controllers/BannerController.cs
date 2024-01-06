using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Collections;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Controllers
{
    public class BannerController : Controller
    {
        Helper obj = new Helper();
        // GET: Banner
        public ActionResult BannerData()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            BannerModelList model = new BannerModelList();
            try
            {
                if (dataSource != null && dataSource.HasChildren)
                {
                    model.bannerList = ParseDatasourceBannerItems(dataSource.Children);
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }

            return View("~/views/Banner/BannerView.cshtml", model);
        }

        private List<BannerModel> ParseDatasourceBannerItems(ChildList children)
        {
            List<BannerModel> objBannerList = new List<BannerModel>();
            BannerModel objBanner = null;
            foreach (Sitecore.Data.Items.Item BannerItem in children)
            {
                objBanner = new BannerModel();
                objBanner.Title = !string.IsNullOrEmpty(BannerItem.Fields["Title"].Value) ? BannerItem.Fields["Title"].Value : string.Empty;
                objBanner.ImageAlt = obj.GetImageAlt(BannerItem, "Image");
                objBanner.ImageSrc = obj.GetImageURL(BannerItem, "Image");
                objBanner.LinkURL = obj.GetLinkURL(BannerItem, "Link");
                objBannerList.Add(objBanner);

            }
            return objBannerList;
        }
    }
}