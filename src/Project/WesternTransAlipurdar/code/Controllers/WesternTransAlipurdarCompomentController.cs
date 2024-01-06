using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Resources.Media;
using Sitecore.WesternTransAlipurdar.Website.Models;
using Sitecore.WesternTransAlipurdar.Website.Providers;
using Sitecore.WesternTransAlipurdar.Website.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.WesternTransAlipurdar.Website.Controllers
{
    public class WesternTransAlipurdarCompomentController : Controller
    {
        Recaptchav2Provider captchav2 = new Recaptchav2Provider();
        WesternTransDataContext dbContext = new WesternTransDataContext();
        
        [HttpPost]
        public ActionResult ContactForm(ContactFormModel model)
        {        
            try
            {

                if (!captchav2.IsReCaptchValid(Request.Headers.Get("g-recaptcha-response")))
                {
                    return Json(new { Result = "ReCapta validation failed.", Status = false });
                }

                Helpers.Utils.ValidateAntiForgeryToken(HttpContext.Request);

                if (ModelState.IsValid)
                {
                    WesternTransDataContext dbContext = new WesternTransDataContext();
                    WebTransmissionContact dataTable = new WebTransmissionContact();
                    dataTable.CID = dbContext.WebTransmissionContacts.Where(x => x.Name != null).OrderByDescending(x => x.CID).Select(x => x.CID).ToList().FirstOrDefault() + 1;
                    dataTable.Name = model.Name;
                    dataTable.Enquiry = model.Inquiry;
                    dataTable.EmailID = model.Email;
                    dataTable.Message = model.Message;

                    dbContext.WebTransmissionContacts.InsertOnSubmit(dataTable);
                    dbContext.SubmitChanges();
                    return Json(new { Result = "Form Saved Successfully.", Status = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = ex.Message, Status = false });
            }
            string errors = ModelState.SelectMany(state => state.Value.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage));
            return Json(new { Result = errors, Status = false });
            }

        /*
         * GetDatasourceComponents used to get rendering and call respective functions
         * which fills the model for respective component
        */
        public void GetDatasourceComponents(Item sourceItem, object modelcontext)
        {            
            try
            {
                if (sourceItem != null)
                {
                    var allRenderingsReferences = sourceItem.Visualization.GetRenderings(Sitecore.Context.Device, false);
                    var renderingList = allRenderingsReferences.ToList();

                    foreach (var item in renderingList)
                    {
                        var renderingContext = Sitecore.Context.Database.GetItem(item.RenderingID);
                        var renderingControllerAction = renderingContext.Fields[BaseTemplate.Fields.ControllerAction].Value;
                        Item datasource = Sitecore.Context.Database.GetItem(item.Settings.DataSource);

                        if (renderingControllerAction != null && modelcontext != null)
                        {
                            Type type = this.GetType();
                            MethodInfo func = type.GetMethod(renderingControllerAction);
                            func.Invoke(this, new object[] { datasource, modelcontext });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #region API functions start
        public JsonResult Home()
        {
            try
            {
                Home homeData = new Home();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.Home);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, homeData);
                return Json(homeData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AboutUs()
        {
            try
            {
                AboutUs AboutUsData = new AboutUs();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.AboutUs);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, AboutUsData);
                return Json(AboutUsData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }   

        public JsonResult Business()
        {
            try
            {
                Business BusinessData = new Business();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.Business);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, BusinessData);
                return Json(BusinessData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ChairmanMesage()
        {
            try
            {
                ProfilePage profileData = new ProfilePage();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.AboutUsPages.ChairmanMessage);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, profileData);
                return Json(profileData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult OneVisionOneTeam()
        {
            try
            {
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.OneVisionOneTeam);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                OneVisionOneTeam visionData = new OneVisionOneTeam();

                var apiUrl = Request.Url.AbsoluteUri;
                Uri apiUri = new Uri(apiUrl);
                string queryParameter = HttpUtility.ParseQueryString(apiUri.Query).Get("Profile");
                if (!string.IsNullOrEmpty(queryParameter))
                {
                    queryParameter = Helpers.Utils.GetURLQueryParameter(queryParameter);
                    Item profileContext = sourceItem.Children.FirstOrDefault(x => Helpers.Utils.CompareIgnoreCase(x.Name, queryParameter));
                    ProfilePage profileData = new ProfilePage();
                    GetDatasourceComponents(profileContext, profileData);
                    return Json(profileData, JsonRequestBehavior.AllowGet);
                }
                GetDatasourceComponents(sourceItem, visionData);
                return Json(visionData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult LegalDisclaimer()
        {
            try
            {
                Legal legalDisclaimerData = new Legal();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.LegalDisclaimer);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, legalDisclaimerData);
                return Json(legalDisclaimerData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult TermsOfUse()
        {
            try
            {
                Legal termsConditionData = new Legal();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.TermsOfUse);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, termsConditionData);
                return Json(termsConditionData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult PrivacyPolicy()
        {
            try
            {
                Legal privacyData = new Legal();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.PrivacyPolicy);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, privacyData);
                return Json(privacyData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Investor()
        {
            try
            {
                Investors investorData = new Investors();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.Investor);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, investorData);
                return Json(investorData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult InvestorDownload()
        {
            try
            {
                InvestorDownload investorDownload = new InvestorDownload();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.InvestorDownload);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, investorDownload);
                return Json(investorDownload, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CorporateGovernance()
        {
            try
            {
                CorporateGovernance corporateGovernance = new CorporateGovernance();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.CorporateGovernance);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, corporateGovernance);
                return Json(corporateGovernance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ContactUs()
        {
            try
            {
                ContactUs contactUsData = new ContactUs();
                var dataSource = Sitecore.Data.ID.Parse(BaseTemplate.Pages.ContactUs);
                Item sourceItem = Sitecore.Context.Database.GetItem(dataSource);
                GetDatasourceComponents(sourceItem, contactUsData);
                return Json(contactUsData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region component rendering functions start
        public void GetBannerCarousel(Item datasource, object modelcontext)
        {
            GenericBannerCarousel genericCarouselData = new GenericBannerCarousel();
            List<BannerCarousel> listData = new List<BannerCarousel>();
            foreach (Item child in datasource.Children)
            {
                var itemsData = new BannerCarousel
                {
                    CTALink = Helpers.Utils.LinkUrl(child?.Fields[BaseTemplate.Fields.CTALink]),
                    CTAText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.CTAText),
                    isExternalLink = Helpers.Utils.GetCheckboxOption(child.Fields[BaseTemplate.Fields.isExternalLink]),
                    HTMLText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.HTMLText),
                    Image = Helpers.Utils.GetImageURLByFieldId(child, BaseTemplate.Fields.Image),
                    ImageAltText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.ImageAltText),
                };
                listData.Add(itemsData);
                genericCarouselData.Data = listData;

                modelcontext.GetType().GetProperty("BannerCarouselData").SetValue(modelcontext, genericCarouselData, null);
            }
            //return genericCarouselData;
        }

        public Home GetCommercialOperation(Item item, Home modelcontext)
        {
            HomeCommercialOperation operationData = new HomeCommercialOperation();
            if (item.HasChildren)
            {
                foreach (Item child in item.Children)
                {
                    operationData.Heading = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Heading);
                    operationData.CTAText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.CTAText);
                    operationData.CTALink = Helpers.Utils.LinkUrl(child?.Fields[BaseTemplate.Fields.CTALink]);
                    operationData.isExternalLink = Helpers.Utils.GetCheckboxOption(child.Fields[BaseTemplate.Fields.isExternalLink]);

                    List<HomeCommercialOperationItems> listData = new List<HomeCommercialOperationItems>();
                    foreach (Item subchild in child.Children)
                    {
                        var itemsData = new HomeCommercialOperationItems
                        {
                            Heading = Helpers.Utils.GetValue(subchild, BaseTemplate.Fields.Heading),
                            Image = Helpers.Utils.GetImageURLByFieldId(subchild, BaseTemplate.Fields.Image),
                            ImageAltText = Helpers.Utils.GetValue(subchild, BaseTemplate.Fields.ImageAltText),
                        };
                        listData.Add(itemsData);
                    }
                    operationData.Data = listData;
                    modelcontext.CommercialData = operationData;
                }
            }

            return modelcontext;
        }

        public Home GetGroupWebsite(Item item, Home modelcontext)
        {
            GroupWebsite websiteData = new GroupWebsite();
            if (item.HasChildren)
            {
                foreach (Item child in item.Children)
                {
                    websiteData.Heading = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Heading);

                    List<GroupWebsiteItems> listData = new List<GroupWebsiteItems>();
                    foreach (Item subchild in child.Children)
                    {
                        var itemsData = new GroupWebsiteItems
                        {
                            Heading = Helpers.Utils.GetValue(subchild, BaseTemplate.Fields.Heading),
                            Image = Helpers.Utils.GetImageURLByFieldId(subchild, BaseTemplate.Fields.Image),
                            ImageAltText = Helpers.Utils.GetValue(subchild, BaseTemplate.Fields.ImageAltText),
                            Link = Helpers.Utils.LinkUrl(subchild?.Fields[BaseTemplate.Fields.Link]),
                            isExternalLink = Helpers.Utils.GetCheckboxOption(child.Fields[BaseTemplate.Fields.isExternalLink])
                        };
                        listData.Add(itemsData);
                    }
                    websiteData.Data = listData;
                    modelcontext.GroupWebsiteData = websiteData;
                }
            }

            return modelcontext;
        }

        public void GetImageBanner(Item datasource, object modelcontext)
        {
            GenericImageBanner imageBannerData = new GenericImageBanner();

            imageBannerData.Image = Helpers.Utils.GetImageURLByFieldId(datasource, BaseTemplate.Fields.Image);
            imageBannerData.ImageAltText = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.ImageAltText);

            modelcontext.GetType().GetProperty("GenericImageBanner").SetValue(modelcontext, imageBannerData, null);

        }

        public void GetNavigationBreadcrumb(Item datasource, object modelcontext)
        {
            GenericBredcrumbNavigation breadcrumbNavigationData = new GenericBredcrumbNavigation();
            List<GenericBredcrumbNavigationItem> listData = new List<GenericBredcrumbNavigationItem>();

            while (datasource != null)
            {
                if (datasource.TemplateName == "Folder")
                {
                    continue;
                }

                var itemsData = new GenericBredcrumbNavigationItem
                {
                    Link = Helpers.Utils.LinkUrl(datasource?.Fields[BaseTemplate.Fields.Link]),
                    Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading)

                };

                listData.Add(itemsData);

                if (datasource.ID == BaseTemplate.Pages.Home)
                {
                    break;
                }
                else
                {
                    datasource = datasource.Parent;
                }
            }
            listData.Reverse();
            breadcrumbNavigationData.Data = listData;
            modelcontext.GetType().GetProperty("GenericBredcrumbNavigation").SetValue(modelcontext, breadcrumbNavigationData, null);
        }

        public void GetPageContent(Item datasource, object modelcontext)
        {
            PageContent pageContentData = new PageContent();
            pageContentData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
            pageContentData.HTMLText = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.HTMLText);

            modelcontext.GetType().GetProperty("PageContent").SetValue(modelcontext, pageContentData, null);
        }

        public void GetInThisSection(Item datasource, object modelcontext)
        {
            InThisSection inThisSectionData = new InThisSection();
            if (datasource != null)
            {
                inThisSectionData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
                if (datasource.HasChildren)
                {
                    List<InThisSectionItem> listData = new List<InThisSectionItem>();
                    foreach (Item child in datasource.GetChildren())
                    {
                        var itemsData = new InThisSectionItem
                        {
                            Heading = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Heading),
                            CTALink = Helpers.Utils.LinkUrl(child?.Fields[BaseTemplate.Fields.CTALink]),
                            CTAText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.CTAText),
                            isExternalLink = Helpers.Utils.GetCheckboxOption(child.Fields[BaseTemplate.Fields.isExternalLink]),
                            Image = Helpers.Utils.GetImageURLByFieldId(child, BaseTemplate.Fields.Image),
                            ImageAltText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.ImageAltText)
                        };
                        listData.Add(itemsData);
                    }
                    inThisSectionData.Data = listData;
                    modelcontext.GetType().GetProperty("InThisSection").SetValue(modelcontext, inThisSectionData, null);
                }
            }
        }

        public void GetBusinessCommercialOperation(Item datasource, object modelcontext)
        {
            BusinessCommercialOperation businessCommercialOperationData = new BusinessCommercialOperation();
            businessCommercialOperationData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
            businessCommercialOperationData.HTMLText = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.HTMLText);
            if (datasource.HasChildren)
            {
                List<BaseImage> listData = new List<BaseImage>();
                foreach (Item child in datasource.Children)
                {
                    var itemsData = new BaseImage
                    {
                        Image = Helpers.Utils.GetImageURLByFieldId(child, BaseTemplate.Fields.Image),
                        ImageAltText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.ImageAltText)
                    };
                    listData.Add(itemsData);
                }
                businessCommercialOperationData.Data = listData;
                modelcontext.GetType().GetProperty("BusinessCommercialOperation").SetValue(modelcontext, businessCommercialOperationData, null);
            }
        }

        //Using GetProfilePage() function for profile pages like chairman message and OVOT inner pages
        public void GetProfilePage(Item datasource, ProfilePage modelcontext)
        {
            if (datasource != null)
            {
                modelcontext.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
                modelcontext.HTMLText = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.HTMLText);
                modelcontext.Image = Helpers.Utils.GetImageURLByFieldId(datasource, BaseTemplate.Fields.Image);
                modelcontext.ImageAltText = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.ImageAltText);
                modelcontext.Designation = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Designation);
            }
        }

        public void GetOneVisionOneTeam(Item datasource, object modelcontext)
        {
            ProfileCard profileCardData = new ProfileCard();
            if (datasource != null)
            {
                profileCardData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
                if (datasource.HasChildren)
                {
                    List<ProfileCardItems> listData = new List<ProfileCardItems>();
                    foreach (Item child in datasource.Children)
                    {
                        if (child.TemplateID.ToString() == "{9AD1E509-A875-4144-A7DC-A2D923554996}")
                        {
                            var itemsData = new ProfileCardItems
                            {
                                Name = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Name),
                                Designation = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Designation),
                                Link = Helpers.Utils.LinkUrl(child?.Fields[BaseTemplate.Fields.Link]),
                                isExternalLink = Helpers.Utils.GetCheckboxOption(child.Fields[BaseTemplate.Fields.isExternalLink]),
                                Image = Helpers.Utils.GetImageURLByFieldId(child, BaseTemplate.Fields.Image),
                                ImageAltText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.ImageAltText)
                            };
                            listData.Add(itemsData);
                            profileCardData.Data = listData;

                            modelcontext.GetType().GetProperty("ProfileCard").SetValue(modelcontext, profileCardData, null);
                        }
                    }
                }
            }
        }

        public void GetInvestorTiles(Item datasource, object modelcontext)
        {
            InvestorTiles investorTileData = new InvestorTiles();
            if (datasource != null)
            {
                investorTileData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
                if (datasource.HasChildren)
                {
                    List<PageContent> listData = new List<PageContent>();
                    foreach (Item child in datasource.GetChildren())
                    {
                        var itemsData = new PageContent
                        {
                            Heading = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Heading),
                            HTMLText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.HTMLText)
                        };
                        listData.Add(itemsData);
                        investorTileData.Data = listData;
                    }
                    modelcontext.GetType().GetProperty("InvestorTitles").SetValue(modelcontext, investorTileData, null);
                }
            }

        }

        public void GetMediaLibraryDownloadSection(Item datasource, object modelcontext)
        {
            MediaLibrarySection mediaLibrarySectionData = new MediaLibrarySection();

            if (datasource != null)
            {
                mediaLibrarySectionData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);

                if (datasource.HasChildren)
                {
                    List<MediaFolders> MediaFoldersList = new List<MediaFolders>();

                    foreach (Item mediaFolder in datasource.Children)
                    {
                        var itemData = new MediaFolders
                        {
                            Heading = Helpers.Utils.GetValue(mediaFolder, BaseTemplate.Fields.Heading)
                        };

                        Sitecore.Data.Fields.LinkField mediaFolderPath = mediaFolder.Fields[BaseTemplate.Fields.Link];
                        Item mediaFolderContext = Context.Database.Items.GetItem(mediaFolderPath.TargetID);

                        if (mediaFolderContext != null)
                        {
                            if (mediaFolderContext.HasChildren)
                            {
                                MediaFolders mediaFolderData = new MediaFolders();
                                List<MediaFolderItems> mediaFolderCollection = new List<MediaFolderItems>();
                                foreach (Item mediaItems in mediaFolderContext.Children)
                                {
                                    var options = new MediaUrlOptions();
                                    options.RequestExtension = string.Empty;
                                    options.AlwaysIncludeServerUrl = false;
                                    var itemsData1 = new MediaFolderItems
                                    {
                                        Link = MediaManager.GetMediaUrl(mediaItems, options),
                                        Heading = Helpers.Utils.GetValue(mediaItems, BaseTemplate.Fields.MediaLibrary.Title)
                                    };
                                    mediaFolderCollection.Add(itemsData1);
                                    itemData.MediaFolderItems = mediaFolderCollection;
                                }
                            }
                        }
                        MediaFoldersList.Add(itemData);
                        mediaLibrarySectionData.MediaFolders = MediaFoldersList;
                    }
                    modelcontext.GetType().GetProperty("MediaLibrarySection").SetValue(modelcontext, mediaLibrarySectionData, null);
                }
            }
        }

        public void GetHeader(Item datasource, object modelcontext)
        {
            Header headerdata = new Header();

            Item rootItem = Sitecore.Context.Database.GetItem(BaseTemplate.Fields.RootItem);
            if (rootItem != null)
            {
                headerdata.Image = Helpers.Utils.GetImageURLByFieldId(rootItem, BaseTemplate.Fields.Logo);
                headerdata.ImageAltText = Helpers.Utils.GetImageAlt(rootItem, BaseTemplate.Fields.Logo);
              //   headerdata.Link = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                headerdata.Link = "/";
            }

            if (datasource != null)
            {
                if (datasource.HasChildren)
                {
                    List<Navigation> navigationData = new List<Navigation>();
                    foreach (Item parent in datasource.Children)
                    {
                        var itemsData = new Navigation
                        {
                            CTALink = Helpers.Utils.LinkUrl(parent?.Fields[BaseTemplate.Fields.CTALink]),
                            CTAText = Helpers.Utils.GetValue(parent, BaseTemplate.Fields.CTAText),
                            isExternalLink = Helpers.Utils.GetCheckboxOption(parent.Fields[BaseTemplate.Fields.isExternalLink]),
                            Image = Helpers.Utils.GetImageURLByFieldId(parent, BaseTemplate.Fields.Image),
                            ImageAltText = Helpers.Utils.GetValue(parent, BaseTemplate.Fields.ImageAltText),
                            Description = Helpers.Utils.GetValue(parent, BaseTemplate.Fields.Description)
                        };


                        if (parent.HasChildren)
                        {
                            Navigation navigation = new Navigation();
                            List<SubNavigation> subNavigationData = new List<SubNavigation>();
                            foreach (Item child in parent.Children)
                            {
                                var itemsData1 = new SubNavigation
                                {
                                    CTALink = Helpers.Utils.LinkUrl(child?.Fields[BaseTemplate.Fields.CTALink]),
                                    CTAText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.CTAText),
                                    isExternalLink = Helpers.Utils.GetCheckboxOption(child.Fields[BaseTemplate.Fields.isExternalLink]),
                                    Image = Helpers.Utils.GetImageURLByFieldId(child, BaseTemplate.Fields.Image),
                                    ImageAltText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.ImageAltText),
                                    Description = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Description)
                                };
                                subNavigationData.Add(itemsData1);
                                itemsData.SubNavigation = subNavigationData;
                            }
                        }

                        navigationData.Add(itemsData);
                        headerdata.Navigation = navigationData;
                    }
                    modelcontext.GetType().GetProperty("HeaderData").SetValue(modelcontext, headerdata, null);
                }
            }
        }

        public void GetCookies(Item datasource, object modelcontext)
        {
            Cookie cookieData = new Cookie();

            if (datasource != null)
            {
                cookieData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
                cookieData.Description = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Description);
                cookieData.CTALink = Helpers.Utils.LinkUrl(datasource?.Fields[BaseTemplate.Fields.CTALink]);
                cookieData.CTAText = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.CTAText);
                cookieData.isExternalLink = Helpers.Utils.GetCheckboxOption(datasource.Fields[BaseTemplate.Fields.isExternalLink]);
            }
            modelcontext.GetType().GetProperty("CookieData").SetValue(modelcontext, cookieData, null);
        }

        public void GetFooter(Item datasource, object modelcontext)
        {
            Footer footerData = new Footer();

            if (datasource != null)
            {
                Item footerNavigationContext = datasource.Children.FirstOrDefault(x => Helpers.Utils.CompareIgnoreCase(x.Name, "footer links"));
                Item footerSocialLinksContext = datasource.Children.FirstOrDefault(x => Helpers.Utils.CompareIgnoreCase(x.Name, "footer social links"));

                List<Navigation> navigationData = new List<Navigation>();
                foreach (Item parent in footerNavigationContext.Children)
                {
                    var itemsData = new Navigation
                    {
                        CTALink = Helpers.Utils.LinkUrl(parent?.Fields[BaseTemplate.Fields.CTALink]),
                        CTAText = Helpers.Utils.GetValue(parent, BaseTemplate.Fields.CTAText),
                        isExternalLink = Helpers.Utils.GetCheckboxOption(parent.Fields[BaseTemplate.Fields.isExternalLink]),
                        Image = Helpers.Utils.GetImageURLByFieldId(parent, BaseTemplate.Fields.Image),
                        ImageAltText = Helpers.Utils.GetValue(parent, BaseTemplate.Fields.ImageAltText),
                        Description = Helpers.Utils.GetValue(parent, BaseTemplate.Fields.Description)
                    };


                    if (parent.HasChildren)
                    {
                        Navigation navigation = new Navigation();
                        List<SubNavigation> subNavigationData = new List<SubNavigation>();
                        foreach (Item child in parent.Children)
                        {
                            var itemsData1 = new SubNavigation
                            {
                                CTALink = Helpers.Utils.LinkUrl(child?.Fields[BaseTemplate.Fields.CTALink]),
                                CTAText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.CTAText),
                                isExternalLink = Helpers.Utils.GetCheckboxOption(child.Fields[BaseTemplate.Fields.isExternalLink])
                            };
                            subNavigationData.Add(itemsData1);
                            itemsData.SubNavigation = subNavigationData;
                        }
                    }

                    navigationData.Add(itemsData);
                    footerData.Navigation = navigationData;
                }


                SocialLinks socialLinksObj = new SocialLinks();
                List <SocialLinksItems> SocialLinksItemsList = new List<SocialLinksItems>();
                
                var socialItemsData = new SocialLinks
                {
                    Heading = Helpers.Utils.GetValue(footerSocialLinksContext, BaseTemplate.Fields.Heading)
                };

                foreach (Item item in footerSocialLinksContext.Children)
                {
                    var itemsData = new SocialLinksItems
                    {
                        Link = Helpers.Utils.LinkUrl(item?.Fields[BaseTemplate.Fields.Link]),
                        Image = Helpers.Utils.GetImageURLByFieldId(item, BaseTemplate.Fields.Image),
                        ImageAltText = Helpers.Utils.GetValue(item, BaseTemplate.Fields.ImageAltText),
                        isExternalLink = Helpers.Utils.GetCheckboxOption(item.Fields[BaseTemplate.Fields.isExternalLink])
                    };
                    SocialLinksItemsList.Add(itemsData);
                }
                socialItemsData.SocialLinksItems = SocialLinksItemsList;

                footerData.SocialLinks = socialItemsData;

                modelcontext.GetType().GetProperty("FooterData").SetValue(modelcontext, footerData, null);
            }
        }

        public void GetCorporateGovernance(Item datasource, object modelcontext)
        {
            CorporateGoveranceProfiles corporateGoveranceProfilesData = new CorporateGoveranceProfiles();

            if (datasource != null)
            {
                corporateGoveranceProfilesData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);

                if (datasource.HasChildren)
                {
                    List<ProfileSections> ProfileSectionsList = new List<ProfileSections>();

                    foreach (Item child in datasource.Children)
                    {
                        var itemData = new ProfileSections
                        {
                            Heading = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Heading),
                            HTMLText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.HTMLText)
                        };

                        if (child.HasChildren)
                        {
                            ProfileSections ProfileSectionsData = new ProfileSections();
                            List<ProfileSectionItems> itemsCollection = new List<ProfileSectionItems>();
                            foreach (Item item in child.Children)
                            {
                                var itemsData1 = new ProfileSectionItems
                                {
                                    Heading = Helpers.Utils.GetValue(item, BaseTemplate.Fields.Heading),
                                    HTMLText = Helpers.Utils.GetValue(item, BaseTemplate.Fields.HTMLText),
                                    Image = Helpers.Utils.GetImageURLByFieldId(item, BaseTemplate.Fields.Image),
                                    ImageAltText = Helpers.Utils.GetValue(item, BaseTemplate.Fields.ImageAltText)
                                };
                                itemsCollection.Add(itemsData1);
                                itemData.ProfileSectionItems = itemsCollection;
                            }
                        }

                        ProfileSectionsList.Add(itemData);
                        corporateGoveranceProfilesData.ProfileSections = ProfileSectionsList;
                    }
                    modelcontext.GetType().GetProperty("CorporateGoveranceProfiles").SetValue(modelcontext, corporateGoveranceProfilesData, null);
                }
            }
        }

        public void GetContactPageContent(Item datasource, ContactUs modelcontext)
        {
            ContactDetail contactDetailData = new ContactDetail();

            if (datasource != null)
            {
                contactDetailData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.Heading);
                contactDetailData.HTMLText = Helpers.Utils.GetValue(datasource, BaseTemplate.Fields.HTMLText);
                Item cityContext = datasource.Children.FirstOrDefault(x => Helpers.Utils.CompareIgnoreCase(x.Name, "city"));

                List<CityDropDown> CityDropDownList = new List<CityDropDown>();
                var itemData = new CityDropDown
                {
                    Heading = Helpers.Utils.GetValue(cityContext, BaseTemplate.Fields.Heading),
                };

                Sitecore.Data.Fields.MultilistField cityDropdownContext = datasource?.Fields[BaseTemplate.Fields.ContactUs.ContactCityDropdown];

                if (cityDropdownContext != null)
                {
                    Sitecore.Data.Items.Item[] items = cityDropdownContext.GetItems();
                    if (items.Length > 0)
                    {
                        CityDropDown CityDropDownData = new CityDropDown();
                        List<CityDropDownItem> CityDropDownItemList = new List<CityDropDownItem>();
                        foreach (var city in items)
                        {
                            var itemData1 = new CityDropDownItem
                            {
                                Heading = Helpers.Utils.GetValue(city, BaseTemplate.Fields.Heading),
                                Image = Helpers.Utils.GetImageURLByFieldId(city, BaseTemplate.Fields.Image),
                                ImageAltText = Helpers.Utils.GetValue(city, BaseTemplate.Fields.ImageAltText)
                            };
                            CityDropDownItemList.Add(itemData1);
                            itemData.CityDropDownItem = CityDropDownItemList;
                        }
                    }
                }

                CityDropDownList.Add(itemData);
                contactDetailData.CityDropDown = CityDropDownList;

            }
            modelcontext.GetType().GetProperty("ContactDetailData").SetValue(modelcontext, contactDetailData, null);
        }

        public void GetCultureValues(Item datasource, object modelcontext)
        {

            CultureValues cultureValues = new CultureValues();
            List<CultureValuesData> cultureValuesList = new List<CultureValuesData>();
            if (datasource != null)
            {
                Item valuesContext = datasource.Children.FirstOrDefault(x => Helpers.Utils.CompareIgnoreCase(x.Name, "values"));
                var data = GetCultureValuesData(valuesContext);
                cultureValuesList.Add(data);

                Item cultureContext = datasource.Children.FirstOrDefault(x => Helpers.Utils.CompareIgnoreCase(x.Name, "culture"));
                data = GetCultureValuesData(cultureContext);
                cultureValuesList.Add(data);
                cultureValues.Data = cultureValuesList;
                modelcontext.GetType().GetProperty("CultureValues").SetValue(modelcontext, cultureValues, null);

            }
        }

        public CultureValuesData GetCultureValuesData(Item source)
        {
            if (source != null)
            {
                var itemsData = new CultureValuesData
                {
                    Heading = Helpers.Utils.GetValue(source, BaseTemplate.Fields.Heading)
                };

                if (source.HasChildren)
                {
                    List<CultureValuesDataItems> listData = new List<CultureValuesDataItems>();
                    foreach (Item child in source.Children)
                    {
                        var itemsData1 = new CultureValuesDataItems
                        {
                            Heading = Helpers.Utils.GetValue(child, BaseTemplate.Fields.Heading),
                            SubHeading = Helpers.Utils.GetValue(child, BaseTemplate.Fields.SubHeading),
                            Image = Helpers.Utils.GetImageURLByFieldId(child, BaseTemplate.Fields.Image),
                            ImageAltText = Helpers.Utils.GetValue(child, BaseTemplate.Fields.ImageAltText)
                        };
                        listData.Add(itemsData1);
                        itemsData.CultureValuesItems = listData;
                    }
                }
                return itemsData;
            }
            return null;
        }

        public void GetContactForm(Item datasource, object modelcontext)
        {
            ContactFormData contactFormData = new ContactFormData();
            if (datasource != null)
            {
                try
                {
                    contactFormData.AntiforgeryToken = Helpers.Utils.GetAntiForgeryToken();
                    contactFormData.Heading = Helpers.Utils.GetValue(datasource, BaseTemplate.ContactForm.FormFields.SectionHeadingField);

                    var Name = BaseTemplate.ContactForm.FormFields.NameField;
                    var Email = BaseTemplate.ContactForm.FormFields.EmailField;
                    var Message = BaseTemplate.ContactForm.FormFields.MessageField;
                    var InquiryDropdown = BaseTemplate.ContactForm.FormFields.InquiryDropDownField;
                    var Submit = BaseTemplate.ContactForm.FormFields.SubmitField;
                    var SectionHeading = BaseTemplate.ContactForm.FormFields.SectionHeadingField;
                    var Terms = BaseTemplate.ContactForm.FormFields.TermsField;
                    var ErrorMsgContextItem = Sitecore.Context.Database.GetItem(BaseTemplate.ContactForm.FormDataSources.ErrorMsgContextItem);

                    var formSection = datasource.Children.FirstOrDefault()?.Children.FirstOrDefault();
                    if (formSection != null)
                    {
                        List<FormFieldSection> FormFieldSectionData = new List<FormFieldSection>();

                        foreach (Item field in formSection.Children)
                        {
                            if (field.ID == SectionHeading)
                            {
                                contactFormData.Heading = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.Text].Value;
                            }
                            if (field.ID == Terms)
                            {
                                contactFormData.Terms = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.Text].Value;
                            }
                            if (field.ID == Name || field.ID == Email || field.ID == Message)
                            {
                                var itemsData = new FormFieldSection
                                {
                                    Placeholder = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.PlaceholderText].Value,
                                    MinAllowedLength = System.Convert.ToInt32(field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.MinAllowedLength].Value),
                                    MaxAllowedLength = System.Convert.ToInt32(field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.MaxAllowedLength].Value),
                                    IsClear = true,
                                    Required = Helpers.Utils.GetBoleanValue(field, BaseTemplate.ContactForm.FormDataTypeFields.Required),
                                    FieldName = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.LabelTitle].Value,
                                    FieldType = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.DefaultValue].Value
                                };

                                if (field.ID == Name)
                                {
                                    ErrorMessage errorMessage = new ErrorMessage();
                                    errorMessage.MaxLengthErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.maxLengthErrorMessageName].Value;
                                    errorMessage.MinLengthErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.minLengthErrorMessageName].Value;
                                    errorMessage.RegexErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.regexErrorMessageName].Value;
                                    errorMessage.RequiredFieldErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.requiredFieldErrorMessageName].Value;
                                    itemsData.ErrorMessages = errorMessage;
                                }
                                if (field.ID == Email)
                                {
                                    ErrorMessage errorMessage = new ErrorMessage();
                                    errorMessage.MaxLengthErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.maxLengthErrorMessageEmail].Value;
                                    errorMessage.MinLengthErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.minLengthErrorMessageEmail].Value;
                                    errorMessage.RegexErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.regexErrorMessageEmail].Value;
                                    errorMessage.RequiredFieldErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.requiredFieldErrorMessageEmail].Value;
                                    itemsData.ErrorMessages = errorMessage;
                                }
                                if (field.ID == Message)
                                {
                                    ErrorMessage errorMessage = new ErrorMessage();
                                    errorMessage.MaxLengthErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.maxLengthErrorMessageMessage].Value;
                                    errorMessage.MinLengthErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.minLengthErrorMessageMessage].Value;
                                    errorMessage.RegexErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.regexErrorMessageMessage].Value;
                                    errorMessage.RequiredFieldErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.requiredFieldErrorMessageMessage].Value;
                                    itemsData.ErrorMessages = errorMessage;
                                }

                                FormFieldSectionData.Add(itemsData);

                            }

                            if (field.ID == InquiryDropdown)
                            {

                                var itemsData = new FormFieldSection
                                {
                                    IsClear = true,
                                    Required = Helpers.Utils.GetBoleanValue(field, BaseTemplate.ContactForm.FormDataTypeFields.Required),
                                    FieldName = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.LabelTitle].Value,
                                    FieldType = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.DropDownDefaultSelection].Value
                                };

                                List<DropDownOptionFields> FieldOptionlist = new List<DropDownOptionFields>();

                                var positionid = 1;
                                var positionField = field.Children.FirstOrDefault()?.Children.FirstOrDefault();
                                foreach (Item position in positionField.Children)
                                {
                                    DropDownOptionFields fieldOption = new DropDownOptionFields();
                                    fieldOption.Label = position.Fields[BaseTemplate.ContactForm.FormDataTypeFields.DropDownOptionslabel].Value;
                                    fieldOption.Id = positionid.ToString();
                                    FieldOptionlist.Add(fieldOption);
                                    positionid++;
                                }
                                itemsData.FieldOptions = FieldOptionlist;

                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ErrorMsgContextItem.Fields[BaseTemplate.ContactForm.ErrorMessages.requiredFieldErrorMessageInquiry].Value;
                                itemsData.ErrorMessages = errorMessage;

                                FormFieldSectionData.Add(itemsData);
                            }

                            if (field.ID == Submit)
                            {
                                SubmitButtonText submitButtonText = new SubmitButtonText();
                                submitButtonText.ButtonText = field.Fields[BaseTemplate.ContactForm.FormDataTypeFields.LabelTitle].Value;
                                contactFormData.SubmitButton = submitButtonText;
                            }

                            if (field != null)
                            {
                                var thankYouDatafieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.ContactForm.FormDataSources.Thankyoupop);
                                ThankYouData thankYouData = new ThankYouData();
                                thankYouData.Heading = thankYouDatafieldItem.Fields[BaseTemplate.Fields.Heading].Value;
                                thankYouData.Description = thankYouDatafieldItem.Fields[BaseTemplate.Fields.Description].Value;
                                thankYouData.CTALink = Helpers.Utils.LinkUrl(thankYouDatafieldItem?.Fields[BaseTemplate.Fields.CTALink]);
                                thankYouData.CTAText = Helpers.Utils.GetValue(thankYouDatafieldItem, BaseTemplate.Fields.CTAText);
                                thankYouData.isExternalLink = Helpers.Utils.GetCheckboxOption(thankYouDatafieldItem.Fields[BaseTemplate.Fields.isExternalLink]);
                                contactFormData.ThankYouData = thankYouData;
                            }
                        }
                        contactFormData.FieldSectionData = FormFieldSectionData;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Exception detail: ", ex, this);
                }
            }
            modelcontext.GetType().GetProperty("ContactFormData").SetValue(modelcontext, contactFormData, null);
        }

        #endregion
    }
}