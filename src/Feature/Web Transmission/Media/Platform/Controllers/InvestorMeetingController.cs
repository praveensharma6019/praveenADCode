using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Controllers
{
    public class InvestorMeetingController : Controller
    {
        List<InvestorMeetingModel> listinvestorMeetingModels = new List<InvestorMeetingModel>();
        // GET: InvestorMeeting
        [HttpGet]
        public ActionResult InvestorMeetingView()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);

            Sitecore.Data.Items.Item targetItem = Sitecore.Context.Database.GetItem(dataSourceId);
            
            StringBuilder data = new StringBuilder();

            //var item = Sitecore.Context.Database.GetItem("/sitecore/content/AdaniTransmission/Home/Investor/InvestorDownload");

            try
            {
                
                if (targetItem != null && targetItem.HasChildren)
                {
                    listinvestorMeetingModels = ParseDatasourceBannerItems(targetItem.GetChildren().Where(c => c.TemplateID == new Sitecore.Data.ID("{91F92BDC-1A33-4701-A5D1-62393FBEB347}")).ToList());
                }

         
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;

            }

            //return View("~/Views/InvestorMeeting/InvestorMeetingView.cshtml", model);
            return View(listinvestorMeetingModels);
        }

        private List<InvestorMeetingModel> ParseDatasourceBannerItems(List<Item> items)
        {
            List<InvestorMeetingModel> objInvestorMeetingList = new List<InvestorMeetingModel>();

            foreach (Sitecore.Data.Items.Item InvertorItem in items)
            {
                InvestorMeetingModel objInvestorMeetingModel = new InvestorMeetingModel();
             //   Sitecore.Data.Items.Item targetInvertorItem = Sitecore.Context.Database.GetItem(InvertorItem.ID);
                objInvestorMeetingModel.HeaderTitle =new HtmlString(FieldRenderer.Render(InvertorItem, "Title"));
                var listInvestorModel = new List<InvestorModel>();

                if (InvertorItem != null && InvertorItem.HasChildren)
                {
                    InvestorModel objInvestor = new InvestorModel();
                    objInvestorMeetingModel.InvestorList = ParseDatasourceGetInvestorItems(InvertorItem.GetChildren().Where(c => c.TemplateID == new Sitecore.Data.ID("{039011BF-6781-488F-9C6C-ACABDB21A3AB}")).ToList());

                }
                objInvestorMeetingList.Add(objInvestorMeetingModel);


            }
            return objInvestorMeetingList;
        }

        private List<InvestorModel> ParseDatasourceGetInvestorItems(List<Item> items)
        {
            List<InvestorModel> objInvestorList = new List<InvestorModel>();

            InvestorModel objInvestor = null;
            foreach (Sitecore.Data.Items.Item InvertorItem in items)
            {

                objInvestor = new InvestorModel();
                //objInvestor.Title = InvertorItem.Fields["Title"].Value;
                objInvestor.Description = InvertorItem.Fields["DocumnetDescription"].Value;//  Convert.ToString(InvertorItem.Fields["Date"].Value); 

                Sitecore.Data.Fields.DateField dateTimeField = InvertorItem.Fields["Date"];

                string dateTimeString = dateTimeField.Value;

                DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                objInvestor.Date = dateTimeStruct.ToString("dd/MM/yyyy");
                objInvestor.filePath = helper.GetImageURL(InvertorItem);
                objInvestorList.Add(objInvestor);

            }
            return objInvestorList;
        }

     
    }

    public static class helper
    {
        public static string GetImageURL(Item currentItem)
        {
            string imageURL = string.Empty;
            Sitecore.Data.Fields.ImageField imageField = currentItem.Fields["InvestorsDocuments"];
            if (imageField != null && imageField.MediaItem != null)
            {
                Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
                imageURL = Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(image));
            }
            return imageURL;
        }
    }

    static class DateTimeExtensions
    {
        public static string ToMonthName(this DateTime dateTime)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static string ToShortMonthName(this DateTime dateTime)
        {
            return System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }
    }
}