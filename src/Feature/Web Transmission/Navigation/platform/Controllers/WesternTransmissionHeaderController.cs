using System.Web.Mvc;
using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Sitecore.Mvc.Presentation;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using System.Collections.Generic;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Controllers
{
    public class WesternTransmissionHeaderController : Controller
    {
        // GET: WesternTransmissionHeader
        public ActionResult Header()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            WesternTransmissionHeaderModel model = new WesternTransmissionHeaderModel();
            Helper obj = new Helper();
            try
            {
          
                if (dataSource != null)
                {
                
                    model.LogoImageSrc = obj.GetImageURL(dataSource, "Image");
                    model.LogoImageAlt = obj.GetImageAlt(dataSource, "Image");
                    var MenuList =(Sitecore.Data.Fields.MultilistField)dataSource.Fields["Menu Links"];
                    model.MenuItem= parseMenuData(MenuList);
                    
                }
            }
            catch (System.Exception ex)
            {

                string msg = ex.Message;
            }
            return View("~/views/WesternTransmissionHeader/WesternTransmissionHeader.cshtml", model);
        }

        private List<MenuLink> parseMenuData(Sitecore.Data.Fields.MultilistField menuList)
        {
            List<MenuLink> objList = new List<MenuLink>();
            MenuLink Menulnk = null;
            Helper obj = new Helper();
            foreach (var MenuLink in menuList.GetItems())
            {
                if (MenuLink != null)
                {
                    Menulnk = new MenuLink();
                    Menulnk.Title =!string.IsNullOrEmpty(MenuLink.Fields["Title"].Value)? MenuLink.Fields["Title"].Value:string.Empty;
                    Menulnk.LinkText = obj.GetLinkText(MenuLink, "Link");
                    Menulnk.LinkUrl = obj.GetLinkURL(MenuLink, "Link");
                    Menulnk.subMenuLinks=parseSubMenuItem((Sitecore.Data.Fields.MultilistField)MenuLink.Fields["SubMenu"]);
                    objList.Add(Menulnk);
                }
            }
            return objList;
        }

        private List<subMenuLink> parseSubMenuItem(Sitecore.Data.Fields.MultilistField multilistField)
        {
            List<subMenuLink> objsubmenuLinkList = new List<subMenuLink>();
            subMenuLink SubMenulnk = null;
            Helper obj = new Helper();
            foreach (var SubMenuLink in multilistField.GetItems())
            {
                if (SubMenuLink != null)
                {
                    SubMenulnk = new subMenuLink();
                    SubMenulnk.Title = !string.IsNullOrEmpty(SubMenuLink.Fields["Title"].Value) ? SubMenuLink.Fields["Title"].Value : string.Empty;
                    SubMenulnk.LinkText = obj.GetLinkText(SubMenuLink, "Link");
                    SubMenulnk.LinkUrl = obj.GetLinkURL(SubMenuLink, "Link");
                    SubMenulnk.ImageAlt = obj.GetImageAlt(SubMenuLink, "Image");
                    SubMenulnk.ImageSrc = obj.GetImageURL(SubMenuLink, "Image");
                    
                    objsubmenuLinkList.Add(SubMenulnk);
                }
            }
            return objsubmenuLinkList;
        }
    }
}