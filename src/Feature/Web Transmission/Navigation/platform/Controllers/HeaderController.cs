using Adani.BAU.Transmission.Feature.Navigation.Platform.Models;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Sanitization;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Controllers
{
    public class HeaderController : Controller
    {
        // GET: Header
        public ActionResult Index()
        {


            return View();
        }
        public ActionResult HamburgerMenu()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            var model = new AirportHeader();
            if (dataSource != null && dataSource.Children != null && dataSource.Children.Count() > 0)
            {
                List<NavigationItems> navigationLinksLevel1 = new List<NavigationItems>();
                List<Item> folders = dataSource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkFolderTemplateID).ToList();
                if (folders != null && folders.Count > 0)
                {
                    foreach (Item folderLevel in folders)
                    {

                        List<Item> level1ItemList = folderLevel.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                        if (level1ItemList != null && level1ItemList.Count() > 0)
                        {
                            foreach (Item level1 in level1ItemList)
                            {
                                CheckboxField checkboxField = level1.Fields["IsActive"];
                                if (checkboxField != null && checkboxField.Checked)
                                {
                                    NavigationItems navigationLinks = new NavigationItems();
                                    string txt = string.Empty;
                                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(level1.Fields["LinkTitle"])))
                                    {
                                        navigationLinks.Title = txt;
                                        txt = string.Empty;
                                    }
                                    LinkField url = level1.Fields["LinkUrl"];
                                    if (url != null && url.Url != null)
                                    {
                                        navigationLinks.Link = url.Url;
                                        txt = string.Empty;
                                    }
                                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(level1.Fields["LeftIconClass"])))
                                    {
                                        navigationLinks.LeftIcon = txt;
                                        txt = string.Empty;
                                    }
                                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(level1.Fields["RightIconClass"])))
                                    {
                                        navigationLinks.LeftIcon = txt;
                                        txt = string.Empty;
                                    }

                                    if (level1.Children != null && level1.Children.Count() > 0)
                                    {
                                        List<NavigationItems2> navigationLinksLevel12 = new List<NavigationItems2>();
                                        List<Item> navigationLinksLevel2 = level1.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                                        foreach (Item item in navigationLinksLevel2)
                                        {
                                            CheckboxField isActive = item.Fields["IsActive"];
                                            if (isActive != null && isActive.Checked)
                                            {
                                                NavigationItems2 navigationLevel2 = new NavigationItems2();
                                                string text = string.Empty;
                                                if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LinkTitle"])))
                                                {
                                                    navigationLevel2.Title = text;
                                                    text = string.Empty;
                                                }
                                                LinkField url1 = item.Fields["LinkUrl"];
                                                if (url1 != null && url1.Url != null)
                                                {
                                                    navigationLevel2.Link = url.Url;
                                                    text = string.Empty;
                                                }
                                                if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LeftIconClass"])))
                                                {
                                                    navigationLevel2.LeftIcon = text;
                                                    text = string.Empty;
                                                }
                                                if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["RightIconClass"])))
                                                {
                                                    navigationLevel2.LeftIcon = text;
                                                    text = string.Empty;
                                                }

                                                if (item.Children != null && item.Children.Count() > 0)
                                                {
                                                    List<NavigationItems2> navigationLinksLevel13 = new List<NavigationItems2>();
                                                    List<Item> navigationLinksLevel3 = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                                                    foreach (Item item3 in navigationLinksLevel3)
                                                    {
                                                        CheckboxField isActiveField = item3.Fields["IsActive"];
                                                        if (isActiveField != null && isActiveField.Checked)
                                                        {
                                                            NavigationItems2 navigationLevel3 = new NavigationItems2();
                                                            string text3 = string.Empty;
                                                            if (!String.IsNullOrEmpty(text3 = GetSingleLineFieldValue(item3.Fields["LinkTitle"])))
                                                            {
                                                                navigationLevel3.Title = text3;
                                                                text3 = string.Empty;
                                                            }
                                                            LinkField url3 = item3.Fields["LinkUrl"];
                                                            if (url1 != null && url1.Url != null)
                                                            {
                                                                navigationLevel3.Link = url.Url;

                                                            }
                                                            if (!String.IsNullOrEmpty(text3 = GetSingleLineFieldValue(item3.Fields["LeftIconClass"])))
                                                            {
                                                                navigationLevel3.LeftIcon = text3;
                                                                text3 = string.Empty;
                                                            }
                                                            if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item3.Fields["RightIconClass"])))
                                                            {
                                                                navigationLevel3.LeftIcon = text3;
                                                                text3 = string.Empty;
                                                            }
                                                            navigationLinksLevel13.Add(navigationLevel3);
                                                        }


                                                    }
                                                    navigationLevel2.NavigationLevel3 = navigationLinksLevel13;


                                                }
                                                navigationLinksLevel12.Add(navigationLevel2);
                                            }
                                        }

                                        navigationLinks.NavigationLevel2 = navigationLinksLevel12;
                                    }
                                    navigationLinksLevel1.Add(navigationLinks);
                                }
                            }
                        }
                    }
                }
                model.NavigationLevel1 = navigationLinksLevel1;
            }
            return View("~/views/Header/HamburgerMenu.cshtml", model);
        }
        public ActionResult TopNavigationMenu()
        {

            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            var model = new AirportHeader();

            if (dataSource != null && dataSource.Children != null && dataSource.Children.Count() > 0)
            {
                List<NavigationItems> navigationLinksLevel1 = new List<NavigationItems>();
                List<Item> level1childrens = dataSource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                if (level1childrens != null && level1childrens.Count() > 0)
                {
                    foreach (var children in level1childrens)
                    {

                        CheckboxField checkboxField = children.Fields["IsActive"];
                        if (checkboxField != null && checkboxField.Checked)
                        {
                            NavigationItems navigationLinks = new NavigationItems();
                            string txt = string.Empty;
                            if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["LinkTitle"])))
                            {
                                navigationLinks.Title = txt;
                                txt = string.Empty;
                            }
                            LinkField url = children.Fields["LinkUrl"];
                            if (url != null && url.Url != null)
                            {
                                navigationLinks.Link = url.Url;
                                txt = string.Empty;
                            }
                            if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["LeftIconClass"])))
                            {
                                navigationLinks.LeftIcon = txt;
                                txt = string.Empty;
                            }
                            if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["RightIconClass"])))
                            {
                                navigationLinks.LeftIcon = txt;
                                txt = string.Empty;
                            }
                            if (children.Children != null && children.Children.Count() > 0)
                            {
                                List<NavigationItems2> navigationLinksLevel12 = new List<NavigationItems2>();
                                List<Item> navigationLinksLevel2 = children.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                                foreach (Item item in navigationLinksLevel2)
                                {

                                    CheckboxField isActive = item.Fields["IsActive"];
                                    if (isActive != null && isActive.Checked)
                                    {
                                        NavigationItems2 navigationLevel2 = new NavigationItems2();
                                        string text = string.Empty;
                                        if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LinkTitle"])))
                                        {
                                            navigationLevel2.Title = text;
                                            text = string.Empty;
                                        }
                                        LinkField url1 = item.Fields["LinkUrl"];
                                        if (url1 != null && url1.Url != null)
                                        {
                                            navigationLevel2.Link = url1.Url;
                                            text = string.Empty;
                                        }
                                        if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LeftIconClass"])))
                                        {
                                            navigationLevel2.LeftIcon = text;
                                            text = string.Empty;
                                        }
                                        if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["RightIconClass"])))
                                        {
                                            navigationLevel2.LeftIcon = text;
                                            text = string.Empty;
                                        }
                                        navigationLinksLevel12.Add(navigationLevel2);
                                    }
                                }
                                navigationLinks.NavigationLevel2 = navigationLinksLevel12;
                            }
                            navigationLinksLevel1.Add(navigationLinks);
                        }
                    }
                }
                model.NavigationLevel1 = navigationLinksLevel1;
            }

            return View("~/Views/Header/TopNavigation.cshtml", model);
        }
        public ActionResult HeaderPrimaryMenu()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            var model = new AirportHeader();

            if (dataSource != null && dataSource.Children != null && dataSource.Children.Count() > 0)
            {
                List<NavigationItems> navigationLinksLevel1 = new List<NavigationItems>();
                List<Item> level1childrens = dataSource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                if (level1childrens != null && level1childrens.Count() > 0)
                {
                    foreach (var children in level1childrens)
                    {

                        CheckboxField checkboxField = children.Fields["IsActive"];
                        if (checkboxField != null && checkboxField.Checked)
                        {
                            NavigationItems navigationLinks = new NavigationItems();
                            string txt = string.Empty;
                            if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["LinkTitle"])))
                            {
                                navigationLinks.Title = txt;
                                txt = string.Empty;
                            }
                            LinkField url = children.Fields["LinkUrl"];
                            if (url != null && url.Url != null)
                            {
                                navigationLinks.Link = url.Url;
                                txt = string.Empty;
                            }
                            if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["LeftIconClass"])))
                            {
                                navigationLinks.LeftIcon = txt;
                                txt = string.Empty;
                            }
                            if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["RightIconClass"])))
                            {
                                navigationLinks.LeftIcon = txt;
                                txt = string.Empty;
                            }
                            if (children.Children != null && children.Children.Count() > 0)
                            {
                                List<NavigationItems2> navigationLinksLevel12 = new List<NavigationItems2>();
                                List<Item> navigationLinksLevel2 = children.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                                foreach (Item item in navigationLinksLevel2)
                                {

                                    CheckboxField isActive = item.Fields["IsActive"];
                                    if (isActive != null && isActive.Checked)
                                    {
                                        NavigationItems2 navigationLevel2 = new NavigationItems2();
                                        string text = string.Empty;
                                        if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LinkTitle"])))
                                        {
                                            navigationLevel2.Title = text;
                                            text = string.Empty;
                                        }
                                        LinkField url1 = item.Fields["LinkUrl"];
                                        if (url1 != null && url1.Url != null)
                                        {
                                            navigationLevel2.Link = url1.Url;
                                            text = string.Empty;
                                        }
                                        if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LeftIconClass"])))
                                        {
                                            navigationLevel2.LeftIcon = text;
                                            text = string.Empty;
                                        }
                                        if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["RightIconClass"])))
                                        {
                                            navigationLevel2.LeftIcon = text;
                                            text = string.Empty;
                                        }
                                        navigationLinksLevel12.Add(navigationLevel2);
                                    }
                                }
                                navigationLinks.NavigationLevel2 = navigationLinksLevel12;
                            }
                            navigationLinksLevel1.Add(navigationLinks);
                        }
                    }
                }
                model.NavigationLevel1 = navigationLinksLevel1;
            }

            return View("~/Views/Header/HeaderPrimaryMenu.cshtml", model);
        }


        public ActionResult HeaderAccountLogin()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            var model = new HeaderMyAccountLogin();
            if (dataSource != null)
            {
                CheckboxField checkboxField = dataSource.Fields["IsActive"];
                if (checkboxField != null && checkboxField.Checked)
                {
                    NavigationItems navigationLinks = new NavigationItems();
                    string txt = string.Empty;
                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(dataSource.Fields["LinkTitle"])))
                    {
                        model.Title = txt;
                        txt = string.Empty;
                    }
                    LinkField url = dataSource.Fields["LinkUrl"];
                    if (url != null && url.Url != null)
                    {
                        model.Link = url.Url;
                    }
                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(dataSource.Fields["LeftIconClass"])))
                    {
                        model.LeftIcon = txt;
                        txt = string.Empty;
                    }
                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(dataSource.Fields["RightIconClass"])))
                    {
                        model.LeftIcon = txt;
                        txt = string.Empty;
                    }



                }

            }
            return View("~/Views/Header/HeaderHamburgerAccountView.cshtml", model);
        }



        public ActionResult HeaderHamburgerAccordian()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            var model = new CollapsableExpandale();
            CheckboxField checkbox = dataSource.Fields["isActive"];
            if (checkbox != null && checkbox.Checked)
            {
                if (!String.IsNullOrEmpty(dataSource.Parent.DisplayName))
                {
                    model.ParentTitle = dataSource.Parent.DisplayName;
                }
                else
                { model.ParentTitle = dataSource.Parent.Name; }
                string emptyText = string.Empty;
                if (!String.IsNullOrEmpty(emptyText = GetSingleLineFieldValue(dataSource.Fields["LinkTitle"])))
                {
                    model.Title = emptyText;
                    emptyText = string.Empty;
                }
                LinkField linkurl = dataSource.Fields["LinkUrl"];
                if (linkurl != null && linkurl.Url != null)
                {
                    model.Link = linkurl.Url;
                }
                if (!String.IsNullOrEmpty(emptyText = GetSingleLineFieldValue(dataSource.Fields["LeftIconClass"])))
                {
                    model.LeftIcon = emptyText;
                    emptyText = string.Empty;
                }
                if (!String.IsNullOrEmpty(emptyText = GetSingleLineFieldValue(dataSource.Fields["RightIconClass"])))
                {
                    model.LeftIcon = emptyText;
                    emptyText = string.Empty;
                }
                if (dataSource != null && dataSource.Children != null && dataSource.Children.Count() > 0)
                {
                    List<NavigationItems> navigationLinksLevel1 = new List<NavigationItems>();
                    List<Item> level1childrens = dataSource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();


                    if (level1childrens != null && level1childrens.Count() > 0)
                    {
                        foreach (var children in level1childrens)
                        {

                            CheckboxField checkboxField = children.Fields["IsActive"];
                            if (checkboxField != null && checkboxField.Checked)
                            {
                                NavigationItems navigationLinks = new NavigationItems();
                                string txt = string.Empty;
                                if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["LinkTitle"])))
                                {
                                    navigationLinks.Title = txt;
                                    txt = string.Empty;
                                }
                                LinkField url = children.Fields["LinkUrl"];
                                if (url != null && url.Url != null)
                                {
                                    navigationLinks.Link = url.Url;
                                    txt = string.Empty;
                                }
                                if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["LeftIconClass"])))
                                {
                                    navigationLinks.LeftIcon = txt;
                                    txt = string.Empty;
                                }
                                if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(children.Fields["RightIconClass"])))
                                {
                                    navigationLinks.LeftIcon = txt;
                                    txt = string.Empty;
                                }
                                if (children.Children != null && children.Children.Count() > 0)
                                {
                                    List<NavigationItems2> navigationLinksLevel12 = new List<NavigationItems2>();
                                    List<Item> navigationLinksLevel2 = children.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                                    foreach (Item item in navigationLinksLevel2)
                                    {

                                        CheckboxField isActive = item.Fields["IsActive"];
                                        if (isActive != null && isActive.Checked)
                                        {
                                            NavigationItems2 navigationLevel2 = new NavigationItems2();
                                            string text = string.Empty;
                                            if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LinkTitle"])))
                                            {
                                                navigationLevel2.Title = text;
                                                text = string.Empty;
                                            }
                                            LinkField url1 = item.Fields["LinkUrl"];
                                            if (url1 != null && url1.Url != null)
                                            {
                                                navigationLevel2.Link = url1.Url;
                                                text = string.Empty;
                                            }
                                            if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["LeftIconClass"])))
                                            {
                                                navigationLevel2.LeftIcon = text;
                                                text = string.Empty;
                                            }
                                            if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(item.Fields["RightIconClass"])))
                                            {
                                                navigationLevel2.LeftIcon = text;
                                                text = string.Empty;
                                            }
                                            navigationLinksLevel12.Add(navigationLevel2);
                                        }
                                    }
                                    navigationLinks.NavigationLevel2 = navigationLinksLevel12;
                                }
                                navigationLinksLevel1.Add(navigationLinks);
                            }
                        }
                    }
                    model.NavigationLevel1 = navigationLinksLevel1;
                }
            }
            return View("~/Views/Header/HeaderHamburgerCollapseableExpandaleView.cshtml", model);
        }
        public ActionResult HeaderHamburgerAccordianSingle()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            var model = new CollapsableExpandale();
            if (dataSource != null)
            {
                CheckboxField checkboxField = dataSource.Fields["IsActive"];
                if (checkboxField != null && checkboxField.Checked)
                {
                    NavigationItems navigationLinks = new NavigationItems();
                    string txt = string.Empty;
                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(dataSource.Fields["LinkTitle"])))
                    {
                        model.Title = txt;
                        txt = string.Empty;
                    }
                    LinkField url = dataSource.Fields["LinkUrl"];
                    if (url != null && url.Url != null)
                    {
                        model.Link = url.Url;
                    }
                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(dataSource.Fields["LeftIconClass"])))
                    {
                        model.LeftIcon = txt;
                        txt = string.Empty;
                    }
                    if (!String.IsNullOrEmpty(txt = GetSingleLineFieldValue(dataSource.Fields["RightIconClass"])))
                    {
                        model.LeftIcon = txt;
                        txt = string.Empty;
                    }
                    List<NavigationItems> navigationLinksLevel1 = new List<NavigationItems>();
                    List<Item> level1childrens = dataSource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                    if (level1childrens != null && level1childrens.Count() > 0)
                    {
                        foreach (var children in level1childrens)
                        {
                            CheckboxField checkbox = children.Fields["IsActive"];
                            if (checkbox != null && checkbox.Checked)
                            {

                                NavigationItems navigationLink1 = new NavigationItems();
                                string text = string.Empty;
                                if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(children.Fields["LinkTitle"])))
                                {
                                    navigationLink1.Title = text;
                                    text = string.Empty;
                                }
                                LinkField url1 = children.Fields["LinkUrl"];
                                if (url1 != null && url1.Url != null)
                                {
                                    navigationLink1.Link = url1.Url;
                                }
                                if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(children.Fields["LeftIconClass"])))
                                {
                                    navigationLink1.LeftIcon = text;
                                    text = string.Empty;
                                }
                                if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(children.Fields["RightIconClass"])))
                                {
                                    navigationLink1.LeftIcon = text;
                                    text = string.Empty;
                                }

                                navigationLinksLevel1.Add(navigationLink1);

                            }
                        }
                    }
                    model.NavigationLevel1 = navigationLinksLevel1;
                }
            }
            return View("~/Views/Header/HeaderHamburgerCollapseableExpandaleView.cshtml", model);
        }

        public ActionResult HeaderHamburgerOtherInfo()
        {
            var dataSourceId = RenderingContext.CurrentOrNull.Rendering.DataSource;
            var dataSource = Sitecore.Context.Database.GetItem(dataSourceId);
            var model = new CollapsableExpandale();
            if (dataSource != null)
            {
                if (dataSource.DisplayName != null && !String.IsNullOrEmpty(dataSource.DisplayName))
                {
                    model.ParentTitle = dataSource.DisplayName;
                }
                else { model.ParentTitle = dataSource.Name; }

                List<NavigationItems> navigationLinksLevel1 = new List<NavigationItems>();
                List<Item> level1childrens = dataSource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID).ToList();
                if (level1childrens != null && level1childrens.Count() > 0)
                {
                    foreach (var children in level1childrens)
                    {
                        CheckboxField checkbox = children.Fields["IsActive"];
                        if (checkbox != null && checkbox.Checked)
                        {

                            NavigationItems navigationLink1 = new NavigationItems();
                            string text = string.Empty;
                            if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(children.Fields["LinkTitle"])))
                            {
                                navigationLink1.Title = text;
                                text = string.Empty;
                            }
                            LinkField url1 = children.Fields["LinkUrl"];
                            if (url1 != null && url1.Url != null)
                            {
                                navigationLink1.Link = url1.Url;
                            }
                            if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(children.Fields["LeftIconClass"])))
                            {
                                navigationLink1.LeftIcon = text;
                                text = string.Empty;
                            }
                            if (!String.IsNullOrEmpty(text = GetSingleLineFieldValue(children.Fields["RightIconClass"])))
                            {
                                navigationLink1.LeftIcon = text;
                                text = string.Empty;
                            }

                            navigationLinksLevel1.Add(navigationLink1);

                        }
                    }
                }
                model.NavigationLevel1 = navigationLinksLevel1;

            }
            return View("~/Views/Header/HeaderHamburgerInformationView.cshtml", model);
        }

        public static string GetSingleLineFieldValue(Field field)
        {
            string text = string.Empty;
            if (field != null && field.Value != null)
            {
                text = field.Value;
            }
            text = StringSanitization.UseRegex(text);
            return text;

        }
    }
}