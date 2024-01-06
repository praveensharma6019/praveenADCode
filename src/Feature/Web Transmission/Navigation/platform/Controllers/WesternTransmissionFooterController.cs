using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Controllers
{
    public class WesternTransmissionFooterController : Controller
    {
        Helper obj = null;
        // GET: WesternTransmissionFooter
        public ActionResult Footer()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            WesternTransmissionFooter model = new WesternTransmissionFooter();

            try
            {
                if (dataSource != null)
                {
                   model.mainFooterLinks = ParseFooterLinks((Sitecore.Data.Fields.MultilistField)dataSource.Fields["Footer Menu List"]);
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }


            return View("~/views/WesternTransmissionFooter/WesternTransmissionFooter.cshtml", model);
        }

        private List<MainFooterLinks> ParseFooterLinks(MultilistField multilistField)
        {
            List<MainFooterLinks> objfooterLinkList = new List<MainFooterLinks>();
            MainFooterLinks MainFooterlnk = null;
            obj = new Helper();
            foreach (var MainFooterLink in multilistField.GetItems())
            {
                if (MainFooterLink != null)
                {
                    MainFooterlnk = new MainFooterLinks();
                    MainFooterlnk.LinkText = obj.GetLinkText(MainFooterLink, "Link");
                    MainFooterlnk.LinkUrl = obj.GetLinkURL(MainFooterLink, "Link");
                    MainFooterlnk.subMenuFooterLinks = parseSubMenuLink((Sitecore.Data.Fields.MultilistField)MainFooterLink.Fields["SubMenu"]);
                    objfooterLinkList.Add(MainFooterlnk);
                }
            }
            return objfooterLinkList;
        }

        private List<SubMenuFooterLink> parseSubMenuLink(MultilistField multilistField)
        {
            List<SubMenuFooterLink> objFootersubmenuLinkList = new List<SubMenuFooterLink>();
            SubMenuFooterLink SubMenuFooterlnk = null;
            Helper obj = new Helper();
            foreach (var FooterSubMenuLink in multilistField.GetItems())
            {
                if (FooterSubMenuLink != null)
                {
                    SubMenuFooterlnk = new SubMenuFooterLink();
                    SubMenuFooterlnk.Title = !string.IsNullOrEmpty(FooterSubMenuLink.Fields["Title"].Value) ? FooterSubMenuLink.Fields["Title"].Value : string.Empty;
                    SubMenuFooterlnk.LinkText = obj.GetLinkText(FooterSubMenuLink, "Link");
                    SubMenuFooterlnk.LinkUrl = obj.GetLinkURL(FooterSubMenuLink, "Link");
                    SubMenuFooterlnk.ImageAlt = obj.GetImageAlt(FooterSubMenuLink, "Image");
                    SubMenuFooterlnk.ImageSrc = obj.GetImageURL(FooterSubMenuLink, "Image");

                    objFootersubmenuLinkList.Add(SubMenuFooterlnk);
                }
            }
            return objFootersubmenuLinkList;
        }
        public ActionResult FooterContactUs()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            obj = new Helper();
            FooterLink model = new FooterLink();

            try
            {
                if (dataSource != null)
                {
                    model.Title = !string.IsNullOrEmpty(dataSource.Fields["Title"].Value) ? dataSource.Fields["Title"].Value : string.Empty;
                    model.LinkText = obj.GetLinkText(dataSource, "Link");
                    model.LinkUrl = obj.GetLinkURL(dataSource, "Link");
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }


            return View("~/views/WesternTransmissionFooter/ContactUsFooterLink.cshtml", model);
        }
    }
}