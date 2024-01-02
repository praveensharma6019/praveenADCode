using Adani.SuperApp.Airport.Feature.Navigation.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Services
{
    public class FooterDataList : IFooterDataList
    {
        private readonly ILogRepository _logRepository;
       
        private readonly IHelper _helper;
        public FooterDataList(ILogRepository logRepository,  IHelper helper)
        {
           
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public FooterData GetFooterData(Item datasource)
        {

            FooterData listFooterData = new FooterData();
            FooterDetails footerDetails = new FooterDetails();
            SeoContents seoContents = new SeoContents();        
            Payments payments1 = new Payments();
            List<FooterDetails> footerDetailList = new List<FooterDetails>();
            List<SeoContents> seoContentlist = new List<SeoContents>();
            List<MainNavigations> MainNavigationslists = new List<MainNavigations>();
            List<Routes> Routeslists = new List<Routes>();


            try
            {
                IEnumerable<Item> listItems = datasource.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkFolderTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                if (listItems != null && listItems.Count() > 0)
                {
                    foreach (Sitecore.Data.Items.Item item in listItems)
                    {

                        if (item.Name == Constant.seoContentItemName)
                        {
                            IEnumerable<Item> ChildlinkFolderItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkFolderTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkFolderItems != null && ChildlinkFolderItems.Count() > 0)
                            {
                                foreach (Sitecore.Data.Items.Item item1 in ChildlinkFolderItems)
                                {
                                    
                                    SeoContents MSeoContentsHeader = new SeoContents();
                                    List<LinkTitlelist> linkTitlelist = new List<LinkTitlelist>();
                                    MSeoContentsHeader.Heading = item1.DisplayName;
                                    IEnumerable<Item> ChildlinkItems = item1.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                                      
                                        if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                                        {
                                            try
                                            {
                                                
                                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                                {
                                                    if (childrenlink != null)
                                                    {
                                                        LinkTitlelist SerNav = new LinkTitlelist();                                                       
                                                        SerNav.LinkTitle = childrenlink.Fields[Constant.Link_Title].Value;
                                                        SerNav.LinkUrl = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                                    linkTitlelist.Add(SerNav);
                                                    }
                                                }
                                            MSeoContentsHeader.items = linkTitlelist;
                                            seoContentlist.Add(MSeoContentsHeader);
                                               
                                            }
                                            catch (Exception ex)
                                            {
                                                _logRepository.Error("ChildlinkFolder gives -> " + ex.Message);
                                            }
                                        }
                                   
                                }
                            }
                            
                            footerDetails.seoContents = seoContentlist;

                        }
                        else if (item.Name == Constant.MainNavigationItemName)
                        {
                            IEnumerable<Item> ChildlinkFolderItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkFolderTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkFolderItems != null && ChildlinkFolderItems.Count() > 0)
                            {
                                foreach (Sitecore.Data.Items.Item item1 in ChildlinkFolderItems)
                                {                                   
                                    MainNavigations MainNavigationsHeader = new MainNavigations();
                                        List<LinkTitlelist> linkTitlelist = new List<LinkTitlelist>();
                                    MainNavigationsHeader.Heading = item1.DisplayName; 
                                        IEnumerable<Item> ChildlinkItems = item1.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();

                                        if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                                        {
                                            try
                                            {
                                                foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                                {
                                                    if (childrenlink != null)
                                                    {
                                                        LinkTitlelist THNav = new LinkTitlelist();                                                       
                                                        THNav.LinkTitle = childrenlink.Fields[Constant.Link_Title].Value;
                                                        THNav.LinkUrl = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                                        THNav.target = _helper.LinkUrlTarget(childrenlink.Fields[Constant.Link_Url]);
                                                        THNav.isAgePopup = _helper.GetCheckboxOption(childrenlink.Fields[Constant.Link_isAgePopup]);
                                                    linkTitlelist.Add(THNav);
                                                    }
                                                }
                                            MainNavigationsHeader.items = linkTitlelist;
                                            MainNavigationslists.Add(MainNavigationsHeader);
                                            
                                           
                                        }
                                            catch (Exception ex)
                                            {
                                                _logRepository.Error("MainNavigation gives -> " + ex.Message);
                                            }
                                        }                                  
                                }
                                footerDetails.MainNavigations = MainNavigationslists;
                            }
                            
                        }
                        else if (item.Name == Constant.paymentItemName)
                        {
                            List<Payments> Paymentslists = new List<Payments>();
                            Payments PaymentsHeader = new Payments();
                            PaymentsHeader.Heading = item.DisplayName;
                            List<ImageLink> payments = new List<ImageLink>();
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.ImageWithLinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();

                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {
                                try
                                {
                                    foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                    {
                                        if (childrenlink != null)
                                        {
                                            ImageLink paymentNav = new ImageLink();
                                            paymentNav.Image = _helper.GetImageURL(childrenlink, "Image");
                                            paymentNav.Link = _helper.LinkUrl(childrenlink.Fields["Link"]);
                                            payments.Add(paymentNav);
                                        }
                                    }
                                    PaymentsHeader.items = payments;
                                    Paymentslists.Add(PaymentsHeader);
                                    footerDetails.Payments = Paymentslists;
                                }
                                catch (Exception ex)
                                {
                                    _logRepository.Error("payment gives -> " + ex.Message);
                                }
                            }

                        }
                        else if (item.Name == Constant.FooterSocialLinksItemName)
                        {
                            List<SocialLinks> SocialLinkslists = new List<SocialLinks>();
                            SocialLinks SocialLinksHeader = new SocialLinks();
                            SocialLinksHeader.Heading = item.DisplayName;
                            List<LinkURlIcon> socialLinks = new List<LinkURlIcon>();
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {
                                try
                                {
                                    foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                    {
                                        if (childrenlink != null)
                                        {
                                            LinkURlIcon SLNav = new LinkURlIcon();
                                            SLNav.LinkTitle = childrenlink.Fields[Constant.Link_Title].Value;
                                            SLNav.LinkUrl = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                            SLNav.itemicon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                            SLNav.Target = childrenlink.Fields[Constant.Link_Url] != null ? _helper.LinkUrlTarget(childrenlink.Fields[Constant.Link_Url]) : String.Empty;
                                            socialLinks.Add(SLNav);
                                        }
                                    }
                                    SocialLinksHeader.items = socialLinks;
                                    SocialLinkslists.Add(SocialLinksHeader);
                                    footerDetails.SocialLinks = SocialLinkslists;
                                }
                                catch (Exception ex)
                                {
                                    _logRepository.Error("FooterSocialLink gives -> " + ex.Message);
                                }
                            }

                        }
                        else if (item.Name == Constant.DownloadItemName)
                        {
                            List<Download> Downloadlists = new List<Download>();
                            Download DownloadHeader = new Download();
                            DownloadHeader.Heading = item.DisplayName;
                            List<ImageLink> downloads = new List<ImageLink>();
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.ImageWithLinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {
                                try
                                {
                                    foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                    {
                                        if (childrenlink != null)
                                        {
                                            ImageLink downloadNav = new ImageLink();                                         
                                            downloadNav.Image = _helper.GetImageURL(childrenlink, Constant.Image);
                                            downloadNav.Link = _helper.LinkUrl(childrenlink.Fields[Constant.Link]);
                                            downloadNav.ItemIcon = childrenlink.Fields[Constant.LogoClass].Value;
                                            downloads.Add(downloadNav);
                                        }
                                    }
                                    DownloadHeader.items = downloads;
                                    Downloadlists.Add(DownloadHeader);
                                    footerDetails.Download = Downloadlists;
                                }
                                catch (Exception ex)
                                {
                                    _logRepository.Error("DownloadItem gives -> " + ex.Message);
                                }
                            }

                        }
                        else if (item.Name == Constant.FooterCopyRightItemName)
                        {
                            List<copyright> copyrightlists = new List<copyright>();
                            copyright copyrightHeader = new copyright();
                            copyrightHeader.Heading = item.DisplayName;
                            List<LinkTitlelist> copyrights = new List<LinkTitlelist>();
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {
                                try
                                {
                                    foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                    {
                                        if (childrenlink != null)
                                        {
                                            LinkTitlelist copyrightNav = new LinkTitlelist();
                                            copyrightNav.LinkTitle = childrenlink.Fields[Constant.Link_Title].Value;
                                            copyrightNav.LinkUrl = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                            copyrights.Add(copyrightNav);
                                        }
                                    }

                                    copyrightHeader.items = copyrights;
                                    copyrightlists.Add(copyrightHeader);
                                    footerDetails.CopyRight = copyrightlists;
                                }
                                catch (Exception ex)
                                {
                                    _logRepository.Error("FooterCopyRight gives -> " + ex.Message);
                                }
                            }

                        }

                        else if (item.Name == Constant.FooterContactUsItemName)
                        {
                            List<contactus> copyrightlists = new List<contactus>();
                            contactus copyrightHeader = new contactus();
                            copyrightHeader.Heading = item.DisplayName;
                            List<LinkTitlelist> copyrights = new List<LinkTitlelist>();
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {
                                try
                                {
                                    foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                    {
                                        if (childrenlink != null)
                                        {
                                            LinkTitlelist copyrightNav = new LinkTitlelist();
                                            copyrightNav.LinkTitle = childrenlink.Fields[Constant.Link_Title].Value;
                                            copyrightNav.LinkUrl = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                            copyrights.Add(copyrightNav);
                                        }
                                    }

                                    copyrightHeader.items = copyrights;
                                    copyrightlists.Add(copyrightHeader);
                                    footerDetails.Contactus = copyrightlists;
                                }
                                catch (Exception ex)
                                {
                                    _logRepository.Error("Footer Contact us gives -> " + ex.Message);
                                }
                            }

                        }
                        else if (item.Name == Constant.BottomNavItemName)
                        {
                            List<BottomNav> BottomNavlists = new List<BottomNav>();
                            BottomNav bottomNav = new BottomNav();
                            bottomNav.Heading = item.DisplayName;
                            List<BottomNavFields> bottomNavList = new List<BottomNavFields>();
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.footerBottomNavID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {
                                try
                                {
                                    foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                    {
                                        if (childrenlink != null)
                                        {
                                            BottomNavFields bottomNav1 = new BottomNavFields();
                                            bottomNav1.Title = childrenlink.Fields[Constant.Title].Value;
                                            bottomNav1.Link = _helper.LinkUrl(childrenlink.Fields[Constant.Link]);
                                            bottomNav1.ActiveImage = _helper.GetImageURL(childrenlink, Constant.Active_Image);
                                            bottomNav1.ImagePath = childrenlink.Fields[Constant.Image_Path].Value;
                                            bottomNavList.Add(bottomNav1);
                                        }
                                    }
                                    bottomNav.items = bottomNavList;
                                    BottomNavlists.Add(bottomNav);
                                    footerDetails.BottomNav = BottomNavlists;
                                }
                                catch (Exception ex)
                                {
                                    _logRepository.Error("BottomNavItem gives -> " + ex.Message);
                                }
                            }

                        }
                        else if (item.Name == Constant.QuickLinksID)
                        {
                            List<QuickLinks> quickLinksList = new List<QuickLinks>();
                            IEnumerable<Item> ChildlinkItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"]) == true).ToList();
                            if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                            {
                                try
                                {
                                    foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                    {
                                        if (childrenlink != null)
                                        {
                                            QuickLinks quickLinks = new QuickLinks();
                                            quickLinks.LinkTitle = childrenlink.Fields[Constant.Link_Title].Value;
                                            quickLinks.Description = childrenlink.Fields[Constant.Link_rightIcon].Value;
                                            quickLinks.LinkUrl = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                            quickLinks.ItemIcon = childrenlink.Fields[Constant.Link_leftIcon].Value;
                                            quickLinks.Target = childrenlink.Fields[Constant.Link_Url] != null ? _helper.LinkUrlTarget(childrenlink.Fields[Constant.Link_Url]) : String.Empty;
                                            quickLinksList.Add(quickLinks);
                                        }
                                    }
                                    
                                    footerDetails.QuickLinks = quickLinksList;
                                }
                                catch (Exception ex)
                                {
                                    _logRepository.Error("QuickLinksItems gives -> " + ex.Message);
                                }
                            }

                        }
                        else if (item.Name == Constant.Routes)
                        {
                            IEnumerable<Item> ChildlinkFolderItems = item.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkFolderTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"])).ToList();
                            if (ChildlinkFolderItems != null && ChildlinkFolderItems.Count() > 0)
                            {
                                foreach (Sitecore.Data.Items.Item route in ChildlinkFolderItems)
                                {
                                    Routes RoutesHeader = new Routes();
                                    List<LinkTitlelist> linkTitlelist = new List<LinkTitlelist>();
                                    RoutesHeader.Heading = route.DisplayName;
                                    IEnumerable<Item> ChildlinkItems = route.Children.Where(x => x.TemplateID == Templates.AirportNavigation.LinkTemplateID && _helper.GetCheckboxOption(x.Fields["IsActive"])).ToList();

                                    if (ChildlinkItems != null && ChildlinkItems.Count() > 0)
                                    {
                                        try
                                        {
                                            foreach (Sitecore.Data.Items.Item childrenlink in ChildlinkItems)
                                            {
                                                if (childrenlink != null)
                                                {
                                                    LinkTitlelist RoutesNav = new LinkTitlelist();
                                                    RoutesNav.LinkTitle = childrenlink.Fields[Constant.Link_Title].Value;
                                                    RoutesNav.LinkUrl = _helper.LinkUrl(childrenlink.Fields[Constant.Link_Url]);
                                                    RoutesNav.target = _helper.LinkUrlTarget(childrenlink.Fields[Constant.Link_Url]);
                                                    RoutesNav.isAgePopup = _helper.GetCheckboxOption(childrenlink.Fields[Constant.Link_isAgePopup]);
                                                    linkTitlelist.Add(RoutesNav);
                                                }
                                            }
                                            RoutesHeader.items = linkTitlelist;
                                            Routeslists.Add(RoutesHeader);


                                        }
                                        catch (Exception ex)
                                        {
                                            _logRepository.Error("Routes gives -> " + ex.Message);
                                        }
                                    }
                                }
                                footerDetails.routes = Routeslists;
                            }

                        }

                    }
                }
                footerDetailList.Add(footerDetails);
                listFooterData.footerDetails = footerDetailList;
            }
            catch (Exception ex)
            {
                _logRepository.Error("ChildlinkFolder gives -> " + ex.Message);
            }
            return listFooterData;
        }
    }
}