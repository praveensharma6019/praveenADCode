using System.Collections.Generic;
using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Controllers
{
    public class WesternTransmissionFooterBottomController : Controller
    {
        Helper obj = new Helper();
        // GET: WesternTransmissionFooter
        public ActionResult FooterBottom()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            WesternTransmissionFooterBottom model = new WesternTransmissionFooterBottom();
            
            try
            {
                if (dataSource != null)
                {
                    model.Title = !string.IsNullOrEmpty(dataSource.Fields["Title"].Value) ? dataSource.Fields["Title"].Value : string.Empty;
                    model.FooterLinks = ParseFooterLinks((Sitecore.Data.Fields.MultilistField)dataSource.Fields["Footer Link"]);
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }
            

            return View("~/views/WesternTransmissionFooterBottom/WesternTransmissionFooterBottom.cshtml", model);
        }
        /// <summary>
        /// Parses the footer Links
        /// </summary>
        /// <param name="multilistField"></param>
        /// <returns></returns>
        private List<FooterLink> ParseFooterLinks(MultilistField multilistField)
        {
            List<FooterLink> objfooterLinkList = new List<FooterLink>();
            FooterLink Footerlnk = null;
            foreach (var FooterLink in multilistField.GetItems())
            {
                if (FooterLink != null)
                {
                    Footerlnk = new FooterLink();
                    Footerlnk.Title = !string.IsNullOrEmpty(FooterLink.Fields["Title"].Value) ? FooterLink.Fields["Title"].Value : string.Empty;
                    Footerlnk.LinkText = obj.GetLinkText(FooterLink, "Link");
                    Footerlnk.LinkUrl = obj.GetLinkURL(FooterLink, "Link");
                    objfooterLinkList.Add(Footerlnk);
                }
            }
            return objfooterLinkList;
        }

        
    }
}