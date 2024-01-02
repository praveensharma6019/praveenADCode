using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using static Adani.SuperApp.Realty.Feature.Configuration.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public class QuickLinksService : IQuickLinksService
    {
        private readonly ILogRepository _logRepository;
        public QuickLinksService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public QuickLinksModel GetQuickLinksData(Rendering rendering)
        {
            QuickLinksModel quickLinksModel = new QuickLinksModel();
            try
            {
                List<object> quickLinks = new List<object>();
                Item item = rendering?.Item;
                quickLinks.Add(GetDisclaimerData(item));
                quickLinks.Add(GetTermsConditionData(item));
                quickLinks.Add(GetPrivacyPolicyData(item));
                quickLinks.Add(GetCookiePolicyData(item));
                quickLinks.Add(GetFaqListData(item));
                quickLinksModel.PageSpecificBreadCrumbs = new List<PageSpecificBreadCrumb>();
                quickLinksModel.PageSpecificBreadCrumbs = GetPageSpecificBreadCrumb(item.Fields[FaqTemplate.Fields.PageSpecificBreadCrumb]);
                quickLinksModel.PageSpecificSEOdata = GetPageSpecificSEOData(item.Fields[FaqTemplate.Fields.PageSpecificBreadCrumb]);
                quickLinksModel.QuickLinks = quickLinks;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return quickLinksModel;
        }

        private List<SEOData> GetPageSpecificSEOData(Field field)
        {
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            var domain = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.SiteDomain].Value) ? commonItem.Fields[Templates.commonData.Fields.SiteDomain].Value : "";
            List<SEOData> getPageSpecificSEOData = new List<SEOData>();
            try
            {
                MultilistField treeListField = field;
                foreach (Item item in treeListField?.GetItems())
                {
                    SEOData pageSeo = new SEOData();

                    pageSeo.MetaTitle = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.metatitleID].Value : "";
                    pageSeo.MetaKeywords = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaKeywords].Value : "";
                    pageSeo.metaDescription = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.MetaDescription].Value : "";
                    pageSeo.pageTitle = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value : item.Name;
                    pageSeo.ogTitle = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.OgTitle].Value : "";
                    pageSeo.robotsTags = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.RobotsTags].Value : "";
                    pageSeo.browserTitle = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.BrowserTitle].Value : "";
                    pageSeo.ogDescription = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.OgDescription].Value : "";
                    pageSeo.ogImage = Helper.GetImageSource(item, SEODataTemplate.PageTemplateFields.OgImage.ToString()) != null ? Helper.GetImageSource(item, SEODataTemplate.PageTemplateFields.OgImage.ToString()) : "";
                    pageSeo.ogKeyword = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.OgKeyword].Value : "";
                    pageSeo.canonicalUrl = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value) ? domain + item.Fields[Templates.SEODataTemplate.PageTemplateFields.canonicalUrl].Value : "";
                    pageSeo.googleSiteVerification = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.googleSiteVerification].Value : "";
                    pageSeo.msValidate = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.msValidate].Value : "";
                    orgSchema orgSchema = new orgSchema();
                    orgSchema.telephone = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.telephone].Value : "";
                    orgSchema.contactType = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.contactType].Value : "";
                    orgSchema.areaServed = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.areaServed].Value : "";
                    orgSchema.streetAddress = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.streetAddress].Value : "";
                    orgSchema.addressLocality = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.addressLocality].Value : "";
                    orgSchema.addressRegion = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.addressRegion].Value : "";
                    orgSchema.postalCode = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.postalCode].Value : "";
                    orgSchema.contactOption = !string.IsNullOrEmpty(item.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value) ? item.Fields[Templates.SEODataTemplate.PageTemplateFields.ContactOption].Value : "";
                    orgSchema.logo = Helper.GetImageSource(item, SEODataTemplate.PageTemplateFields.Logo.ToString()) != null ? Helper.GetImageSource(item, SEODataTemplate.PageTemplateFields.Logo.ToString()) : "";
                    orgSchema.url = pageSeo.canonicalUrl;
                    var childern = Helper.GetMultiListValueItem(item, Templates.SEODataTemplate.PageTemplateFields.sameASTemplateID);
                    if (childern != null)
                    {
                        foreach (var nestedItem in childern)
                        {
                            var linkFeild = Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) != null ?
                                                 Helper.GetLinkURL(nestedItem, Templates.SEODataTemplate.PageTemplateFields.Fields.navLink.ToString()) : "";
                            orgSchema.sameAs.Add(linkFeild);
                        }
                    }
                    pageSeo.orgSchema = orgSchema;

                    getPageSpecificSEOData?.Add(pageSeo);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return getPageSpecificSEOData;
        }

        private List<PageSpecificBreadCrumb> GetPageSpecificBreadCrumb(Field field)
        {
            List<PageSpecificBreadCrumb> pageSpecificBreadCrumbs = new List<PageSpecificBreadCrumb>();
            try
            {
                MultilistField treeListField = field;
                foreach (Item item in treeListField?.GetItems())
                {
                    PageSpecificBreadCrumb pageSpecificBreadCrumb = new PageSpecificBreadCrumb();
                    pageSpecificBreadCrumb.Heading = Helper.GetLinkTextbyField(item, item?.Fields[FaqTemplate.Fields.Link]);
                    pageSpecificBreadCrumb.Link = Helper.GetLinkURLbyField(item, item?.Fields[FaqTemplate.Fields.Link]);
                    pageSpecificBreadCrumbs?.Add(pageSpecificBreadCrumb);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return pageSpecificBreadCrumbs;
        }

        public FaqSection GetFaqListData(Item item)
        {
            FaqSection faqCategories = new FaqSection();
            Category category = new Category();
            category.QuickLinksFaq = new List<QuickLinksFaq>();
            try
            {
                MultilistField multilistField = item?.Fields[QuickLinksTemplate.FaqFields.Faq];
                if (multilistField != null)
                {
                    foreach (Item innerItem in multilistField.GetItems())
                    {
                        QuickLinksFaq quickLinksFaq = new QuickLinksFaq();
                        quickLinksFaq.Id = innerItem?.Fields[QuickLinsFaqTemplate.CategoryFields.Id]?.Value?.ToLower();
                        quickLinksFaq.Category = innerItem?.Fields[QuickLinsFaqTemplate.CategoryFields.Title]?.Value;
                        quickLinksFaq.Heading = innerItem?.Fields[QuickLinsFaqTemplate.CategoryFields.Heading]?.Value;
                        quickLinksFaq.Links = Helper.GetLinkURL(innerItem, Templates.QuickLinsFaqTemplate.CategoryFields.Links.ToString()) != null ? Helper.GetLinkURL(innerItem, Templates.QuickLinsFaqTemplate.CategoryFields.Links.ToString()) : "";
                        quickLinksFaq.FaqItems = GetFaqData(innerItem);
                        category?.QuickLinksFaq?.Add(quickLinksFaq);
                    }
                    category.Heading = item?.Fields[QuickLinksTemplate.FaqFields.Heading]?.Value;
                    category.Key = item.Fields[QuickLinksTemplate.FaqFields.Key]?.Value;
                    faqCategories.Faqs = category;

                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return faqCategories;
        }

        private List<Data> GetFaqData(Item item)
        {
            List<Data> dataList = new List<Data>();
            try
            {
                foreach (Item innerItem in item.GetChildren())
                {
                    Data data = new Data();
                    data.Title = innerItem?.Fields[Templates.FaqTemplate.Fields.Title].Value;
                    data.Body = innerItem?.Fields[Templates.FaqTemplate.Fields.Content].Value;
                    dataList?.Add(data);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return dataList;
        }


        private DisclaimerObject GetDisclaimerData(Item item)
        {
            DisclaimerObject disclaimerObject = new DisclaimerObject();
            QuickLink quickLink = new QuickLink();
            try
            {
                quickLink.Description = item?.Fields[QuickLinksTemplate.DisclaimerFields.Description]?.Value;
                quickLink.Heading = item?.Fields[QuickLinksTemplate.DisclaimerFields.Heading]?.Value;
                quickLink.Key = item?.Fields[QuickLinksTemplate.DisclaimerFields.Key]?.Value;
                disclaimerObject.Disclaimer = new QuickLink();
                disclaimerObject.Disclaimer = quickLink;

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return disclaimerObject;
        }
        private TermsConditionObject GetTermsConditionData(Item item)
        {
            TermsConditionObject quickLinkObject = new TermsConditionObject();
            QuickLink quickLink = new QuickLink();
            try
            {
                quickLink.Description = item?.Fields[QuickLinksTemplate.TermsConditionFields.Description]?.Value;
                quickLink.Heading = item?.Fields[QuickLinksTemplate.TermsConditionFields.Heading]?.Value;
                quickLink.Key = item?.Fields[QuickLinksTemplate.TermsConditionFields.Key]?.Value;
                quickLinkObject.TermsandCondition = quickLink;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return quickLinkObject;
        }
        private PrivacyPolicyObject GetPrivacyPolicyData(Item item)
        {
            PrivacyPolicyObject privacyPolicyObject = new PrivacyPolicyObject();
            QuickLink quickLink = new QuickLink();
            try
            {
                quickLink.Description = item?.Fields[QuickLinksTemplate.PrivacyPolicyFields.Description]?.Value;
                quickLink.Heading = item?.Fields[QuickLinksTemplate.PrivacyPolicyFields.Heading]?.Value;
                quickLink.Key = item?.Fields[QuickLinksTemplate.PrivacyPolicyFields.Key]?.Value;
                privacyPolicyObject.PrivacyPolicy = quickLink;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return privacyPolicyObject;
        }
        private CookiePolicyObject GetCookiePolicyData(Item item)
        {
            CookiePolicyObject cookiePolicyObject = new CookiePolicyObject();
            QuickLink quickLink = new QuickLink();
            try
            {
                quickLink.Description = item?.Fields[QuickLinksTemplate.CookiePolicyFields.Description]?.Value;
                quickLink.Heading = item?.Fields[QuickLinksTemplate.CookiePolicyFields.Heading]?.Value;
                quickLink.Key = item?.Fields[QuickLinksTemplate.CookiePolicyFields.Key]?.Value;
                cookiePolicyObject.CookiePolicy = quickLink;
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return cookiePolicyObject;
        }
    }
}