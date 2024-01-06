using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adani.Transmission.Feature.Media.Platform.Models;
using Adani.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Collections;
using Sitecore.Mvc.Presentation;


namespace Adani.Transmission.Feature.Media.Platform.Controllers
{
    public class InvestorController : Controller
    {
        Helper obj = new Helper();
        // GET: Investor
        [HttpGet]
        public ActionResult InvestorView()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            InvestorModelList model = new InvestorModelList();
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

            return View("~/views/Investor/InvestorView.cshtml", model);
        
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
                objInvestor.Date = Convert.ToDateTime(InvertorItem.Fields["Date"]);
                objInvestorList.Add(objInvestor);

            }
            return objInvestorList;
        }
    }
}