using System;
using System.Web.Http;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Fields;
using Adani.SuperApp.Airport.Feature.Hotels.Platform.Models;
using System.Web;
using Sitecore.Mvc.Extensions;
using Sitecore.Links;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform.Controllers
{
    public class HotelPoliciesController : ApiController
    {
        private ILogRepository logRepository;
        private readonly IHelper _helper;

        public HotelPoliciesController(ILogRepository _logRepository, IHelper helper)
        {
            this.logRepository = _logRepository;
            this._helper = helper;
        }

        [HttpGet]
        [Route("api/GetHotelPolicies")]
        public IHttpActionResult GetPolicies()
        {
            try
            {
                Response responseData = new Response();
                Result resultData = new Result();
                bool isConfirmationPage = HttpContext.Current.Request.QueryString["isConfirmationPage"] != null ? HttpContext.Current.Request.QueryString["isConfirmationPage"].ToBool() : false;

                Sitecore.Data.Database contextDB = Sitecore.Context.Database;
                Item policiesFolder = contextDB.GetItem(Constants.AllPoliciesFolderID);
                Item termsConditionsFolder = contextDB.GetItem(Constants.TermsConditionsFolderID.ToString());
                Item importantInformationFolder = contextDB.GetItem(Constants.ImpInformationFolderID);

                if(policiesFolder != null)
                    resultData.allPolicies = GetAllPoliciesData(policiesFolder);
                if(termsConditionsFolder != null)
                    resultData.termsAndConditions = GetTermsConditionsData(termsConditionsFolder);
                if(importantInformationFolder != null && isConfirmationPage)
                {
                    resultData.contactDetail = GetContactDetails(importantInformationFolder);
                    resultData.importantInfo = GetImportantInfo(importantInformationFolder);
                }

                if (resultData.allPolicies != null || resultData.termsAndConditions != null)
                {
                    responseData.status = true;
                    responseData.data = resultData;
                }

                return Json(responseData);
            }
            catch (Exception ex)
            {
                logRepository.Error(ex.Message);

                return null;
            }
        }

        private AllPolicies GetAllPoliciesData(Item policiesFolder)
        {
            AllPolicies allPolicies = new AllPolicies();
            allPolicies.title = policiesFolder.Fields[Constants.AutoId].Value;
            allPolicies.policies = GetPolicyData(policiesFolder);
            return allPolicies;
        }

        private List<Policies> GetPolicyData(Item policiesFolder)
        {
            List<Policies> policiesList = new List<Policies>();
            Policies policies = null;
            if(policiesFolder.HasChildren)
            {
                foreach (Item item in policiesFolder.GetChildren().Where(x => x.TemplateID == Constants.PoliciesTemplateID))
                {
                    policies = new Policies();
                    policies.subTitle = item.Fields[Constants.Title].Value;
                    policies.code = item.Fields[Constants.Name].Value;
                    if (item.HasChildren)
                        policies.policiesData = (from Item data in item.GetChildren() select data.Fields[Constants.Value].Value).ToList();
                    policiesList.Add(policies);
                }
            }

            return policiesList;
        }

        private TermsConditions GetTermsConditionsData(Item termsConditionsFolder)
        {
            TermsConditions termsConditions = new TermsConditions();
            termsConditions.title = termsConditionsFolder.Fields[Constants.Title].Value;
            if(termsConditionsFolder.Children != null)
            termsConditions.lines = (from Item item in termsConditionsFolder.GetChildren() select item.Fields[Constants.Value].Value).ToList();
            
            return termsConditions;
        }

        private ImportantInfo GetImportantInfo(Item item)
        {
            Item impInfoData = item.GetChildren().Where(x => x.TemplateID == Constants.ImportantInfoTemplateID).FirstOrDefault();
            ImportantInfo hotelImportantInfo = new ImportantInfo();
            hotelImportantInfo.title = impInfoData.Fields[Constants.Title].Value;
            if(impInfoData.HasChildren)
            {
                Information information = null;
                foreach (Item line in impInfoData.GetChildren())
                {
                    information = new Information();
                    information.title = line.Fields[Constants.Title] != null ? line.Fields[Constants.Title].ToString() : "";
                    information.description = line.Fields[Constants.Description] != null ? line.Fields[Constants.Description].Value.ToString() : "";
                    information.image = line.Fields[Constants.Image] != null ? _helper.GetImageURL(line.Fields[Constants.Image]) : "";
                    information.autoId = line.Fields[Constants.AutoId] != null ? line.Fields[Constants.AutoId].ToString() : "";
                    LineLinks links = null;
                    if(line.HasChildren)
                    {
                        foreach(Item link in line.GetChildren())
                        {
                            links = new LineLinks();
                            links.link = link.Name.Trim();
                            links.linkText = link.Fields[Constants.Title] != null ? link.Fields[Constants.Title].Value.ToString() : "";
                            links.linkURL = link.Fields[Constants.Link] != null ? _helper.LinkUrl(link.Fields[Constants.Link]) : "";
                            links.title = link.Fields[Constants.Title] != null ? link.Fields[Constants.Link].Value.ToString() : "";
                            links.description = link.Fields[Constants.Description] != null ? link.Fields[Constants.Description].Value.ToString() : "";
                            links.uniqueId = link.Fields[Constants.UniqueId] != null ? link.Fields[Constants.UniqueId].Value.ToString() : "";
                            links.image = link.Fields[Constants.Image] != null ? _helper.GetImageURL(link.Fields[Constants.Image]) : "";
                            information.links.Add(links);
                        }
                    }
                    hotelImportantInfo.lines.Add(information);
                }
            }
            return hotelImportantInfo;
        }

        private ContactDetailsItem GetContactDetails(Item contactItem)
        {
            ContactDetailsItem contactDetailsItem = null;
            if (contactItem.HasChildren)
            {
                contactDetailsItem = new ContactDetailsItem();
                var contactData = contactItem.GetChildren().Where(x => x.TemplateID == Constants.ContactDetailsTemplateID);
                foreach (Item contact in contactData)
                {
                    if (contact != null)
                    {
                        if (contact.Name == "Phone")
                        {
                            ContactDetail phone = new ContactDetail();
                            phone.name = contact.Fields[Constants.ReadMoreText].Value;
                            phone.title = contact.Fields[Constants.Title].Value;
                            phone.richText = contact.Fields[Constants.Description].Value;
                            contactDetailsItem.phone = phone;
                        }
                        if (contact.Name == "Email")
                        {
                            ContactDetail email = new ContactDetail();
                            email.name = contact.Fields[Constants.ReadMoreText].Value;
                            email.title = contact.Fields[Constants.Title].Value;
                            email.richText = contact.Fields[Constants.Description].Value;
                            contactDetailsItem.email = email;
                        }
                    }
                }
            }

            return contactDetailsItem;
        }
    }
}