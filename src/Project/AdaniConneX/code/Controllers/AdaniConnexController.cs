using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json.Linq;
using Sitecore.AdaniConneX.Website.Models;
using Sitecore.AdaniConneX.Website.Provider;
using Sitecore.AdaniConneX.Website.Providers;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using Sitecore.AdaniConneX.Website.Helpers;
using Sitecore.Foundation.Email.Models;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using DocumentFormat.OpenXml.Bibliography;
using System.Threading.Tasks;
using static Sitecore.ContentSearch.Linq.Extensions.ReflectionExtensions;

namespace Sitecore.AdaniConneX.Website.Controllers
{
    public class AdaniConnexController : Controller
    {

        private static readonly Regex alphaNumber = new Regex("^[a-z A-Z0-9 ]*$");
        private static readonly Regex emailRegex = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");
        private static readonly Regex NumRegex = new Regex(@"^(\+\d{1,3}[- ]?)?\d{10}$");

        string EncryptionKey = "Tl;jld@456763909QPwOeiRuTy873XY7";
        string EncryptionIV = "CEIVRAJWquG8iiMw";
        // GET: AdaniConnex
        public ActionResult Index()
        {
            return View();
        }

        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            string str = reResponse;
            string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Lcql9QZAAAAAOSIsZ-9gNRiZaVPrDmSrbKSe3y2");
            string str2 = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", str1, str);
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create(str2)).GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    flag = (JObject.Parse(streamReader.ReadToEnd()).Value<bool>("success") ? true : false);
                }
            }
            return flag;
        }

        [HttpPost]
        public async Task<ActionResult> Insertcontactdetail(AdaniConnex_ContactForm_Model m)
        {

            bool Validated = false;
            Log.Info("Insert ContactUsForm", "Start");
            var result = new { status = "1" };
            try
            {
                Validated = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                Log.Info(string.Concat("ContactUsForm Failed to validate auto script : ", ex.ToString()), this);
                result = new { status = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {

                if (Validated)
                {
                    if (string.IsNullOrEmpty(m.Name))
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.Name))
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (string.IsNullOrEmpty(m.Message))
                    {
                        result = new { status = "406" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (string.IsNullOrEmpty(m.Company))
                    {
                        result = new { status = "407" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (string.IsNullOrEmpty(m.Email))
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (!emailRegex.IsMatch(m.Email))
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (string.IsNullOrEmpty(m.Contact))
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (!NumRegex.IsMatch(m.Contact))
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }


                    AdaniConnexContactFormDataContext rdb = new AdaniConnexContactFormDataContext();

                    AdaniConnex_ContactForm r = new AdaniConnex_ContactForm();
                    r.Id = Guid.NewGuid();
                    r.Name = m.Name;
                    r.Email = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Email, EncryptionIV);
                    r.Contact = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Contact, EncryptionIV);
                    r.Company = m.Company;
                    r.Message = m.Message;
                    r.FormType = m.FormType;
                    r.FormUrl = m.FormUrl;
                    r.SubmittedDate = System.DateTime.Now;

                    #region Insert to DB
                    rdb.AdaniConnex_ContactForms.InsertOnSubmit(r);
                    rdb.SubmitChanges();

                    if (m.SendEmail == "true")
                    {
                        /*
                        #region Notification Email For User
                        Sitecore.Data.Items.Item ThankyouTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.ThankyouTemplate);
                        string messagebody = ThankyouTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                        messagebody = messagebody.Replace("[Name]", m.Name);
                     
                        var isEmailSent = await SendEmailToUser(m.Email, messagebody, ThankyouTemplate);
                        if (!System.Convert.ToBoolean(isEmailSent))
                        {
                            result = new { status = "409" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                        */
                        #region Lead Data Send To Business
                        Sitecore.Data.Items.Item LeadDataTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.LeadDataTemplate);
                        string leadDataBody = LeadDataTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                        leadDataBody = leadDataBody.Replace("[Name]", m.Name);
                        leadDataBody = leadDataBody.Replace("[Email]", m.Email);
                        leadDataBody = leadDataBody.Replace("[Contact]", m.Contact);
                        leadDataBody = leadDataBody.Replace("[Message]", m.Message);
                        leadDataBody = leadDataBody.Replace("[Company]", m.Company);
                        leadDataBody = leadDataBody.Replace("[FormType]", m.FormType);
                        leadDataBody = leadDataBody.Replace("[Country]", "");
                        leadDataBody = leadDataBody.Replace("[CV]", "");

                        var isEmailSent1 = await SendEmailToUser(m.Email, leadDataBody, LeadDataTemplate);
                        if (!System.Convert.ToBoolean(isEmailSent1))
                        {
                            result = new { status = "409" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                    }


                }
                else
                {
                    result = new { status = "408" };
                    return Json(result, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                result = new { status = "fail" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //WhitePaperFormInsertCode
        [HttpPost]
        public async Task<ActionResult> InsertWhitePaperFormdetail(AdaniConnex_WhitePaperForm_Model m)
        {

            bool Validated = false;
            Log.Info("Insert AdaniConnexWhitePaperForm", "Start");
            var result = new { status = "1" };
            try
            {
                Validated = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                Log.Info(string.Concat("ContactUsForm Failed to validate auto script : ", ex.ToString()), this);
                result = new { status = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (Validated)
                {

                    if (string.IsNullOrEmpty(m.Name))
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.Name))
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (string.IsNullOrEmpty(m.Company))
                    {
                        result = new { status = "406" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(m.Country))
                    {
                        result = new { status = "407" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (string.IsNullOrEmpty(m.Email))
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (!emailRegex.IsMatch(m.Email))
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (string.IsNullOrEmpty(m.Contact))
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    if (!NumRegex.IsMatch(m.Contact))
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    AdaniConnexContactFormDataContext rdb = new AdaniConnexContactFormDataContext();

                    AdaniConnex_WhitePaperForm r = new AdaniConnex_WhitePaperForm();
                    r.Id = Guid.NewGuid();
                    r.Name = m.Name;
                    r.Email = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Email, EncryptionIV);
                    r.Contact = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Contact, EncryptionIV);
                    r.Company = m.Company;
                    r.Country = m.Country;
                    r.SubmittedDate = System.DateTime.Now;

                    #region Insert to DB
                    rdb.AdaniConnex_WhitePaperForms.InsertOnSubmit(r);
                    rdb.SubmitChanges();

                    //Send Email to user
                    if (m.SendEmail == "true")
                    {
                        /*
                        #region Notification Email For User
                        Sitecore.Data.Items.Item ThankyouTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.ThankyouTemplate);
                        string messagebody = ThankyouTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                        messagebody = messagebody.Replace("[Name]", m.Name);

                        var isEmailSent = await SendEmailToUser(m.Email, messagebody, ThankyouTemplate);
                        if (!System.Convert.ToBoolean(isEmailSent))
                        {
                            result = new { status = "409" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                        */

                        #region Lead Data Send To Business
                        Sitecore.Data.Items.Item LeadDataTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.LeadDataTemplate);
                        string leadDataBody = LeadDataTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                        leadDataBody = leadDataBody.Replace("[Name]", m.Name);
                        leadDataBody = leadDataBody.Replace("[Email]", m.Email);
                        leadDataBody = leadDataBody.Replace("[Contact]", m.Contact);
                        leadDataBody = leadDataBody.Replace("[Country]", m.Country);
                        leadDataBody = leadDataBody.Replace("[Company]", m.Company);
                        leadDataBody = leadDataBody.Replace("[FormType]", m.FormType);
                        leadDataBody = leadDataBody.Replace("[Message]", "");
                        leadDataBody = leadDataBody.Replace("[CV]", "");

                        var isEmailSent1 = await SendEmailToUser(m.Email, leadDataBody, LeadDataTemplate);
                        if (!System.Convert.ToBoolean(isEmailSent1))
                        {
                            result = new { status = "409" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                        #endregion
                    }

                }
                else
                {
                    result = new { status = "408" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                result = new { status = "fail" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Take Tour Form Insert Code
        [HttpPost]
        public async Task<ActionResult> InsertTakeATourdetail(AdaniConnex_TakeAtourForm_Model m)
        {
            var result = new { status = "1" };
            try
            {
                if (string.IsNullOrEmpty(m.Name))
                {
                    result = new { status = "701" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.Name))
                {
                    result = new { status = "701" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.Email))
                {
                    result = new { status = "703" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.Email))
                {
                    result = new { status = "703" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.Contact))
                {
                    result = new { status = "705" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!NumRegex.IsMatch(m.Contact))
                {
                    result = new { status = "705" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.Company))
                {
                    result = new { status = "707" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                AdaniConnexContactFormDataContext rdb = new AdaniConnexContactFormDataContext();
                AdaniConnex_TakeAtourForm r = new AdaniConnex_TakeAtourForm();
                r.Id = Guid.NewGuid();
                r.Name = m.Name;
                r.Email = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Email, EncryptionIV);
                r.Contact = EncryptionServiceHelper.EncryptString(EncryptionKey, m.Contact, EncryptionIV);
                r.Company = m.Company;
                r.Location = m.Location;
                r.FormType = m.FormType;
                r.FormUrl = m.FormUrl;
                r.SubmittedDate = System.DateTime.Now;

                #region Insert to DB
                rdb.AdaniConnex_TakeAtourForms.InsertOnSubmit(r);
                rdb.SubmitChanges();

                //Send Email to user
                if (m.SendEmail == "true")
                {
                    /*
                    #region Notification Email For User
                    Sitecore.Data.Items.Item ThankyouTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.ThankyouTemplate);
                    string messagebody = ThankyouTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                    messagebody = messagebody.Replace("[Name]", m.Name);

                    var isEmailSent = await SendEmailToUser(m.Email, messagebody, ThankyouTemplate);
                    if (!System.Convert.ToBoolean(isEmailSent))
                    {
                        result = new { status = "409" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    #endregion
                    */

                    #region Lead Data Send To Business
                    Sitecore.Data.Items.Item LeadDataTemplate = Context.Database.GetItem(Templates.EmailTemplate.Datasource.LeadDataTemplate);
                    string leadDataBody = LeadDataTemplate.Fields[Templates.EmailTemplate.DatasourceFields.Body].Value;
                    leadDataBody = leadDataBody.Replace("[Name]", m.Name);
                    leadDataBody = leadDataBody.Replace("[Email]", m.Email);
                    leadDataBody = leadDataBody.Replace("[Contact]", m.Contact);
                    leadDataBody = leadDataBody.Replace("[Country]", m.Location);
                    leadDataBody = leadDataBody.Replace("[Company]", m.Company);
                    leadDataBody = leadDataBody.Replace("[FormType]", m.FormType);
                    leadDataBody = leadDataBody.Replace("[Message]", "");
                    leadDataBody = leadDataBody.Replace("[CV]", "");

                    var isEmailSent1 = await SendEmailToUser(m.Email, leadDataBody, LeadDataTemplate);
                    if (!System.Convert.ToBoolean(isEmailSent1))
                    {
                        result = new { status = "409" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                result = new { status = "fail" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //JoinUs AdaniConnex_JoinUsForm_Model m
        [HttpPost]
        public async Task<ActionResult> InsertJoinUsFormdetail(AdaniConnex_JoinUsForm_Model m)
        {
            bool Validated = false;
            var result = new { status = "1" };

            if (ModelState.IsValid)
            {
                Validated = this.IsReCaptchValid(m.reResponse);
                if (Validated)
                {
                    JoinUsFormProvider JoinUsProvider = new JoinUsFormProvider();
                    string JoinUsFormStatus = await JoinUsProvider.JoinUsForm(m);
                    if (JoinUsFormStatus != "1")
                    {
                        result = new { status = JoinUsFormStatus };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "408" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string errors = ModelState.SelectMany(state => state.Value.Errors).Aggregate("", (current, error) => current + (error.ErrorMessage));
                result = new { status = errors };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
        public async Task<bool> SendEmailToUser(string Email, string EmailBody,Sitecore.Data.Items.Item EmailTemplate)
        {
            //send email to user
            SendThankyouEmailService emailThankyouNotification = new SendThankyouEmailService();
            var emailServiceStatus = await emailThankyouNotification.ThankyouNotification(Email, EmailBody, EmailTemplate);
            if(emailServiceStatus)
             return true;

            return false;
        }
    }
}