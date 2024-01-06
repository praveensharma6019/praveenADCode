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
    public class ImageBannerController : Controller
    {
        Helper obj = new Helper();
        // GET: Banner
        public ActionResult ImageBannerData()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            ImageBannerList model = new ImageBannerList();
            try
            {
                if (dataSource != null && dataSource.HasChildren)
                {
                    model.imageBanners = ParseDatasourceImageBannerItems(dataSource.Children);
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }

            return View("~/views/ImageBanner/ImageBannerView.cshtml", model);
        }

        private List<ImageBanner> ParseDatasourceImageBannerItems(ChildList children)
        {
            List<ImageBanner> objBannerList = new List<ImageBanner>();
            ImageBanner objImageBanner = null;
            foreach (Sitecore.Data.Items.Item ImageBannerItem in children)
            {
                objImageBanner = new ImageBanner();
                objImageBanner.ImageAlt = obj.GetImageAlt(ImageBannerItem, "Image");
                objImageBanner.ImageUrl = obj.GetImageURL(ImageBannerItem, "Image");
                objBannerList.Add(objImageBanner);

            }
            return objBannerList;
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult InvestorMeetingView()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);

            InvestorMeetingModel model = new InvestorMeetingModel();
            try
            {
                if (dataSource != null && dataSource.HasChildren)
                {
                    model.InvestorList = ParseDatasourceBannerItems(dataSource.Children);
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }

            return View("~/Views/InvestorMeeting/InvestorMeetingView.cshtml", model);
            //return View();
        }
        private List<InvestorModel> ParseDatasourceBannerItems(ChildList children)
        {
            List<InvestorModel> objInvestorList = new List<InvestorModel>();
            InvestorModel objInvestor = null;
            foreach (Sitecore.Data.Items.Item InvertorItem in children)
            {
                objInvestor = new InvestorModel();
                objInvestor.Title = !string.IsNullOrEmpty(InvertorItem.Fields["Title"].Value) ? InvertorItem.Fields["Title"].Value : string.Empty;
                objInvestor.Description = InvertorItem.Fields["Description"].ToString(); //obj.ToString(InvertorItem, "Description");
                objInvestor.Date = Convert.ToString(InvertorItem.Fields["Date"]);
                objInvestorList.Add(objInvestor);

            }
            return objInvestorList;
        }
    }
}