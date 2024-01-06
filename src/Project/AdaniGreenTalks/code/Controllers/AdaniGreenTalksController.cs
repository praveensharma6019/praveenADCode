
using DocumentFormat.OpenXml.Office.CustomUI;
using Newtonsoft.Json;
using Sitecore.AdaniGreenTalks.Website.Models;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;
using System.Web.Mvc;
using Sitecore.AdaniGreenTalks.Website.Provider;
using Sitecore.AdaniGreenTalks.Website.Attribute;
using System.Web;
using Sitecore.Mvc.Extensions;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.AdaniGreenTalks.Website.Helpers;
using System.Runtime.InteropServices;

namespace Sitecore.AdaniGreenTalks.Website.Controllers
{
    public class AdaniGreenTalksController : Controller
    {
        private static readonly Regex alphaNumber = new Regex("^[a-z A-Z0-9 ]*$");
        private static readonly Regex filevalidregex = new Regex(@"^[a-z A-Z0-9 _ \- .\(\)&-]+$");
        
        //private static readonly Regex emailRegex = new Regex("^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*(\\.\\w{2,3})+$");
        private static readonly Regex NumRegex = new Regex("^(\\+\\d{1,3}[- ]?)?\\d{10}$");

        // GET: AdaniGreenTalks
        public ActionResult Index()
        {
            return View();
        }


        //contact us form - created by Neeraj yadav
        [HttpGet]
        public ActionResult AdaniGreenTalksContactFormPage()
        {
            this.Session["contact_form_error"] = "";
            return (ActionResult)this.View();
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AdaniGreenTalksContactFormPage(AdaniGreenTalks_Contactus_Model m)
        {
            Session["contact_form_error"] = string.Empty;
            JsonResultModel model = new JsonResultModel();
            try
            {
                model = IsCheckValid(m, model);
                if (model.IsValid)
                {
                    AdaniGreentalksRcaptchaV3 rcaptchaV3 = new AdaniGreentalksRcaptchaV3();
                    bool captchaResult = rcaptchaV3.IsReCaptchValidV3(m.googleCaptchaToken);
                    if (!captchaResult)
                    {
                        Log.Error(Constant.recaptchaVerificationFailed, this);
                        model.errorModel = SetErrorMsg(Constant.recaptchaVerificationFailed);
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                    AdaniGreenTalksContactFormProvider cfp = new AdaniGreenTalksContactFormProvider();
                    string response = cfp.ContactusForm(m);
                    if (response == "successfully")
                    {
                        model.IsSuccess = true;
                    }
                    else
                    {
                        if (response != "failed")
                        {
                            model.errorModel = SetErrorMsg(Constant.errorOccurred);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.errorModel = SetErrorMsg(Constant.message);
                Log.Error(ex.Message, ex, typeof(AdaniGreenTalksController));
                return Json(model);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AdaniGreenTalksAttendFormPage(AdaniGreenTalks_Contactus_Model m)
        {
            Session["contact_form_error"] = string.Empty;
            JsonResultModel model = new JsonResultModel();
            try
            {
                model = IsCheckAttendValid(m, model);
                if (model.IsValid)
                {
                    AdaniGreentalksRcaptchaV3 rcaptchaV3 = new AdaniGreentalksRcaptchaV3();
                    bool captchaResult = rcaptchaV3.IsReCaptchValidV3(m.googleCaptchaToken);
                    if (!captchaResult)
                    {
                        Log.Error(Constant.recaptchaVerificationFailed, this);
                        model.errorModel = SetErrorMsg(Constant.recaptchaVerificationFailed);
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                    AdaniGreenTalksContactFormProvider cfp = new AdaniGreenTalksContactFormProvider();
                    string response = cfp.AttendForm(m);
                    if (response == "successfully")
                    {
                        model.IsSuccess = true;
                    }
                    else
                    {
                        if (response != "failed")
                        {
                            model.errorModel = SetErrorMsg(Constant.errorOccurred);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.errorModel = SetErrorMsg(Constant.message);
                Log.Error(ex.Message, ex, typeof(AdaniGreenTalksController));
                return Json(model);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AdaniGreenTalksContributeFormPage()
        {
            return (ActionResult)this.View("~/Views/AdaniGreenTalks/AdaniGreenTalksContributeFormPage.cshtml");
        }


        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AdaniGreenTalksContributeFormPage(AdaniGreenTalks_Contribute_Model m)
        {
            Session["contact_form_error"] = string.Empty;
            JsonResultModel model = new JsonResultModel();
            try
            {
                model = IsCheckValidContribute(m, model);
                if (model.IsValid)
                {
                    AdaniGreentalksRcaptchaV3 rcaptchaV3 = new AdaniGreentalksRcaptchaV3();
                    bool captchaResult = rcaptchaV3.IsReCaptchValidV3(m.googleCaptchaToken);
                    if (!captchaResult)
                    {
                        Log.Error(Constant.recaptchaVerificationFailed, this);
                        model.errorModel = SetErrorMsg(Constant.recaptchaVerificationFailed);
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                    AdaniGreenTalksContactFormProvider cfp = new AdaniGreenTalksContactFormProvider();
                    string response = cfp.ContributeForm(m);
                    if (response == "successfully")
                    {
                        model.IsSuccess = true;
                    }
                    else
                    {
                        if (response != "failed")
                        {
                            model.errorModel = SetErrorMsg(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.errorModel = SetErrorMsg(Constant.message);
                Log.Error(ex.Message, ex, typeof(AdaniGreenTalksController));
                return Json(model);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult AdaniGreenTalksSpeakFormPage(AdaniGreenTalks_Speak_Model m)
        {
            Session["contact_form_error"] = string.Empty;
            JsonResultModel model = new JsonResultModel();
            try
            {
                model = IsCheckValidSpeak(m, model);
                if (model.IsValid)
                {
                    AdaniGreentalksRcaptchaV3 rcaptchaV3 = new AdaniGreentalksRcaptchaV3();
                    bool captchaResult = rcaptchaV3.IsReCaptchValidV3(m.googleCaptchaToken);
                    if (!captchaResult)
                    {
                        Log.Error(Constant.recaptchaVerificationFailed, this);
                        model.errorModel = SetErrorMsg(Constant.recaptchaVerificationFailed);
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        // Checking for Internet Explorer      
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        // Get the complete folder path and store the file inside it.      

                        bool exists = System.IO.Directory.Exists(Server.MapPath("~/Uploads/AdaniGreenTalks/"));

                        if (!exists)
                            System.IO.Directory.CreateDirectory(Server.MapPath("~/Uploads/AdaniGreenTalks/"));
                        FileInfo fi = new FileInfo(fname);
                        var filepath = string.Format("{0}{1}-{2}{3}", "~/Uploads/AdaniGreenTalks/", Path.GetFileNameWithoutExtension(fi.Name), DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss"), fi.Extension);
                        string fileserverpath = Server.MapPath(filepath);
                        if (i == 0)
                        {
                            m.fileUploadPhoto = filepath;
                        }
                        if (i == 1)
                        {
                            m.fileUploadbiograph = filepath;
                        }
                        if (i == 2)
                        {
                            m.fileOriginalConcept = filepath;
                        }
                        fname = Path.Combine(fileserverpath);
                        file.SaveAs(fname);
                    }

                    AdaniGreenTalksContactFormProvider cfp = new AdaniGreenTalksContactFormProvider();
                    string response = cfp.SpeakForm(m);
                    if (response == "successfully")
                    {
                        model.IsSuccess = true;
                    }
                    else
                    {
                        if (response != "failed")
                        {
                            model.errorModel = SetErrorMsg(response);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                model.errorModel = SetErrorMsg(Constant.message);
                Log.Error(ex.Message, ex, typeof(AdaniGreenTalksController));
                return Json(model);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }






        [HttpGet]
        public ActionResult JoinUs_FormPage()
        {
            this.Session["Join-us-error"] = "";
            return View();
        }
        private void CreateValidationList(List<ContactValidation> contactvalidationlist, string StatusCode, string Msg)
        {
            ContactValidation obj = new ContactValidation
            {
                StatusCode = StatusCode,
                FieldErrorMessage = Msg
            };
            contactvalidationlist.Add(obj);
        }
        private ErrorMessage SetErrorMsg(string errorMsg)
        {
            ErrorMessage objError = new ErrorMessage
            {
                IsError = true,
                errorMessage = errorMsg,
            };
            return objError;
        }

        private JsonResultModel IsCheckValidContribute(AdaniGreenTalks_Contribute_Model contactUsModel, JsonResultModel model)
        {
            //
            model.IsValid = true;
            if (contactUsModel != null)
            {
                List<ContactValidation> contactvalidationlist = new List<ContactValidation>();
                if (string.IsNullOrEmpty(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Contribute.FirstName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Contribute.FirstName)));
                }
                else if (contactUsModel.FirstName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Contribute.FirstName)));
                }
                else if (hasSpecialChar(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Contribute.FirstName)));
                }

                if (string.IsNullOrEmpty(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Contribute.LastName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Contribute.LastName)));
                }
                else if (contactUsModel.LastName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Contribute.LastName)));
                }
                else if (hasSpecialChar(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Contribute.LastName)));
                }
                if (string.IsNullOrEmpty(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Contribute.Email)));
                }
                else if (contactUsModel.Email.Length > 120)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Contribute.Email)));
                }
                /*else if (!emailRegex.IsMatch(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Contribute.Email)));
                }*/
                else if (hasSpecialChar(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Contribute.Email)));
                }

                if (string.IsNullOrEmpty(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Contribute.ContactNumber)));
                }
                else if (contactUsModel.ContactNumber.Length > 10)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Contribute.ContactNumber)));
                }
                else if (!NumRegex.IsMatch(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Contribute.ContactNumber)));
                }

                if (string.IsNullOrEmpty(contactUsModel.City))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CityCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Contribute.City)));
                }
                else if (contactUsModel.City.Length > 50)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CityCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Contribute.City)));
                }
                else if (hasSpecialChar(contactUsModel.City))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Contribute.City)));
                }
                if (string.IsNullOrEmpty(contactUsModel.Goal))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.Goal, Constant.MsgNullCheck.Replace("$val$", nameof(model.Contribute.Goal)));
                }
                else if (contactUsModel.Goal.Length > 120)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.Goal, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Contribute.Goal)));
                }
                else
                {
                    var objGoal = ItemExtension.GetOptionsList(SitecoreConstant.Goals).Where(c => c.Text == contactUsModel.Goal).FirstOrDefault();
                    if (objGoal == null)
                    {
                        CreateValidationList(contactvalidationlist, MsgStatusCode.Goal, Constant.MsgSelectValidCheck.Replace("$val$", nameof(model.Contribute.Goal)));
                    }

                }
          
                if (contactvalidationlist.Count > 0)
                {
                    model.contactvalidationlist = contactvalidationlist;
                    model.IsValid = false;
                }

            }

            return model;
        }

        private JsonResultModel IsCheckValidSpeak(AdaniGreenTalks_Speak_Model contactUsModel, JsonResultModel model)
        {
            model.IsValid = true;
            if (contactUsModel != null)
            {
                char ch = '.';
                List<ContactValidation> contactvalidationlist = new List<ContactValidation>();
                if (string.IsNullOrEmpty(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.FirstName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.FirstName)));
                }
                else if (contactUsModel.FirstName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.FirstName)));
                }
                else if (hasSpecialChar(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.FirstName)));
                }
                
                if (string.IsNullOrEmpty(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.LastName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.LastName)));
                }
                else if (contactUsModel.LastName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.LastName)));
                }
                else if (hasSpecialChar(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.LastName)));
                }
                if (string.IsNullOrEmpty(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.Email)));
                }
                else if (contactUsModel.Email.Length > 120)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.Email)));
                }
                /*else if (!emailRegex.IsMatch(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.Email)));
                }*/
                else if (hasSpecialChar(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.Email)));
                }

                if (string.IsNullOrEmpty(contactUsModel.MobileNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.MobileNumber)));
                }
                else if (contactUsModel.MobileNumber.Length > 10)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.MobileNumber)));
                }
                else if (!NumRegex.IsMatch(contactUsModel.MobileNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.MobileNumber)));
                }


                if (string.IsNullOrEmpty(contactUsModel.NomineeFirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameNomineeCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.NomineeFirstName)));
                }
                else if (contactUsModel.NomineeFirstName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameNomineeCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.NomineeFirstName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.NomineeFirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameNomineeCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.NomineeFirstName)));
                }
                else if (hasSpecialChar(contactUsModel.NomineeFirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameNomineeCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.NomineeFirstName)));
                }
                if (string.IsNullOrEmpty(contactUsModel.NomineeLastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameNomineeCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.NomineeLastName)));
                }
                else if (contactUsModel.NomineeLastName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameNomineeCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.NomineeLastName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.NomineeLastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameNomineeCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.NomineeLastName)));
                }
                else if (hasSpecialChar(contactUsModel.NomineeLastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameNomineeCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.NomineeLastName)));
                }

                if (string.IsNullOrEmpty(contactUsModel.NomineeEmail))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailNomineeCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.NomineeEmail)));
                }
                else if (contactUsModel.NomineeEmail.Length > 120)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailNomineeCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.NomineeEmail)));
                }
                /*else if (!emailRegex.IsMatch(contactUsModel.NomineeEmail))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailNomineeCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.NomineeEmail)));
                }*/
                else if (hasSpecialChar(contactUsModel.NomineeEmail))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailNomineeCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.NomineeEmail)));
                }


                if (string.IsNullOrEmpty(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.ContactNumber)));
                }
                else if (contactUsModel.ContactNumber.Length > 10)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.ContactNumber)));
                }
                else if (!NumRegex.IsMatch(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.Speak.ContactNumber)));
                }

                if (string.IsNullOrEmpty(contactUsModel.City))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CityCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.City)));
                }
                else if (contactUsModel.City.Length > 50)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CityCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.City)));
                }
                else if (hasSpecialChar(contactUsModel.City))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CityCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.City)));
                }

                if (string.IsNullOrEmpty(contactUsModel.Country))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CountryCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.Country)));
                }
                else if (contactUsModel.Country.Length > 50)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CountryCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.Country)));
                }
                else if (hasSpecialChar(contactUsModel.Country))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.CountryCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.Country)));
                }
                if (string.IsNullOrEmpty(contactUsModel.Goal))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.Goal, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.Goal)));
                }
                else
                {
                    var objGoal = ItemExtension.GetOptionsList(SitecoreConstant.Goals).Where(c => c.Text == contactUsModel.Goal.Trim()).FirstOrDefault();
                    if (objGoal == null)
                    {
                        CreateValidationList(contactvalidationlist, MsgStatusCode.Goal, Constant.MsgSelectValidCheck.Replace("$val$", nameof(model.Speak.Goal)));
                    }
                }
                if (string.IsNullOrEmpty(contactUsModel.linkforarticle))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.linkforarticleCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.linkforarticle)));
                }
                else if (contactUsModel.linkforarticle.Length > 300)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.linkforarticleCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.linkforarticle)));
                }
                else if (hasSpecialChar(contactUsModel.linkforarticle))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.linkforarticleCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.linkforarticle)));
                }

                if (string.IsNullOrEmpty(contactUsModel.linkaudioorvideo))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.linkaudioorvideo, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.linkaudioorvideo)));
                }
                else if (contactUsModel.linkaudioorvideo.Length > 300)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.linkaudioorvideo, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.Speak.linkaudioorvideo)));
                }
                else if (hasSpecialChar(contactUsModel.linkaudioorvideo))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.linkaudioorvideo, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.Speak.linkaudioorvideo)));
                }

                if (string.IsNullOrEmpty(contactUsModel.fileUploadPhotoName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                }
                else  if (hasfileSpecialChar(contactUsModel.fileUploadPhotoName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgValidFileNameCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                }
                else if (!filevalidregex.IsMatch(contactUsModel.fileUploadPhotoName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgValidFileNameCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                }

                //else if (contactUsModel.fileUploadPhotoName.Count(f => (f == ch)) > 1)
                //{
                //    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgInValidFilePhotoextensionCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                //}
                else if (Request.Files.Count > 0)
                {
                    var httpPhotoPostedFileBase = Request.Files.Get(contactUsModel.fileUploadPhotoName);

                    if (httpPhotoPostedFileBase != null)
                    {
                        string extension1 = Path.GetExtension(httpPhotoPostedFileBase.FileName);
                        string[] allowedExtenstions1 = new string[] { ".png", ".jpeg", ".gif", ".jpg" };
                        if (!allowedExtenstions1.Contains(extension1))
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgValidFilePhotoCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                        }
                  
                        string str = httpPhotoPostedFileBase.FileName;
                      
                        int occurance = str.Count(f => (f == ch));
                        if (occurance > 1)
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgInValidFilePhotoextensionCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                        }

                        string contentType = String.IsNullOrEmpty(httpPhotoPostedFileBase.ContentType) ? GetMimeTypeByWindowsRegistry(extension1) : httpPhotoPostedFileBase.ContentType;
                        if (contentType != "")
                        {
                            if (contentType != "image/jpeg" && contentType != "image/png" && contentType != "image/jpg" && contentType != "image/gif")
                            {
                                CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgValidFilePhotoCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                            }
                        }
                        else
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgValidFilePhotoCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                        }

                        BinaryReader b = new BinaryReader(httpPhotoPostedFileBase.InputStream);
                        byte[] bindata = b.ReadBytes(httpPhotoPostedFileBase.ContentLength);
                        string filecontent = System.Convert.ToBase64String(bindata);
                        if (filecontent.StartsWith("iVBOR") == false && filecontent.StartsWith("/9j/4") == false && filecontent.ToUpper().StartsWith("R0LG") == false)
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgValidFilePhotoCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                        }
                        b.Close();
                    }
                    else
                    {
                        CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                    }
                }
                if (string.IsNullOrEmpty(contactUsModel.fileUploadbiographName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                }
                else if (hasfileSpecialChar(contactUsModel.fileUploadbiographName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgValidFilePhotoCheck.Replace("$val$", nameof(model.Speak.fileUploadbiographName)));
                }
                else if (!filevalidregex.IsMatch(contactUsModel.fileUploadbiographName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgValidFileNameCheck.Replace("$val$", nameof(model.Speak.fileUploadbiographName)));
                }
                else if (Request.Files.Count > 0)
                {
                    var httpPhotoPostedFileBase = Request.Files.Get(contactUsModel.fileUploadbiographName);

                    if (httpPhotoPostedFileBase != null)
                    {
                        string extension1 = Path.GetExtension(httpPhotoPostedFileBase.FileName);
                        string[] allowedExtenstions1 = new string[] { ".pdf" };
                        if (!allowedExtenstions1.Contains(extension1))
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                        }
                        string str = httpPhotoPostedFileBase.FileName;
                      //  char ch = '.';
                        int occurance = str.Count(f => (f == ch));
                        if (occurance > 1)
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgInValidFilepdfextensionCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                        }

                        string contentType = String.IsNullOrEmpty(httpPhotoPostedFileBase.ContentType) ? GetMimeTypeByWindowsRegistry(extension1) : httpPhotoPostedFileBase.ContentType;
                        if (contentType != "")
                        {
                            if (contentType != "application/pdf")
                            {
                                CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                            }
                        }
                        else
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                        }




                        BinaryReader b = new BinaryReader(httpPhotoPostedFileBase.InputStream);
                        byte[] bindata = b.ReadBytes(httpPhotoPostedFileBase.ContentLength);
                        string filecontent = System.Convert.ToBase64String(bindata);
                        if (filecontent.StartsWith("JVBER") == false)
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                        }
                        b.Close();

                    }
                    else
                    {
                        CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                    }


                }
                if (string.IsNullOrEmpty(contactUsModel.fileOriginalConceptName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                }
                else if (hasfileSpecialChar(contactUsModel.fileOriginalConceptName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgValidFileNameCheck.Replace("$val$", nameof(model.Speak.fileOriginalConceptName)));
                }
                else if (!filevalidregex.IsMatch(contactUsModel.fileOriginalConceptName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgValidFileNameCheck.Replace("$val$", nameof(model.Speak.fileOriginalConceptName)));
                }
                else if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase httpPhotoPostedFileBase;
                    var count = Request.Files.GetMultiple(contactUsModel.fileOriginalConceptName).Count();
                    if(count == 2)
                    {
                        httpPhotoPostedFileBase = Request.Files.GetMultiple(contactUsModel.fileOriginalConceptName).Last();

                    }
                    else
                    {
                        httpPhotoPostedFileBase = Request.Files.Get(contactUsModel.fileOriginalConceptName);
                    }

                    if (httpPhotoPostedFileBase != null)
                    {
                        string extension1 = Path.GetExtension(httpPhotoPostedFileBase.FileName);
                        string[] allowedExtenstions1 = new string[] { ".pdf" };
                        if (!allowedExtenstions1.Contains(extension1))
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                        }
                        string str = httpPhotoPostedFileBase.FileName;
                      //  char ch = '.';
                        int occurance = str.Count(f => (f == ch));
                        if (occurance > 1)
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgInValidFilepdfextensionCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                        }

                        string contentType = String.IsNullOrEmpty(httpPhotoPostedFileBase.ContentType) ? GetMimeTypeByWindowsRegistry(extension1) : httpPhotoPostedFileBase.ContentType;
                        if (contentType != "")
                        {
                            if (contentType != "application/pdf")
                            {
                                CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                            }
                        }
                        else
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                        }

                        BinaryReader b = new BinaryReader(httpPhotoPostedFileBase.InputStream);
                        byte[] bindata = b.ReadBytes(httpPhotoPostedFileBase.ContentLength);
                        string filecontent = System.Convert.ToBase64String(bindata);
                        if (filecontent.StartsWith("JVBER") == false)
                        {
                            CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgValidFilePDFCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                        }
                        b.Close();
                    }
                    else
                    {
                        CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                    }
                   

                }
               
                //if (Request.Files.Count == 0)
                //{
                //    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadPhotoCodeCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileUploadPhoto)));
                //    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                //    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));

                //}
                //else if (Request.Files.Count == 1)
                //{
                //    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadBiographyCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileUploadbiograph)));
                //    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));

                //}
                //else if (Request.Files.Count == 2)
                //{
                //    CreateValidationList(contactvalidationlist, MsgStatusCode.UploadoriginalconceptCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.Speak.fileOriginalConcept)));
                //}


                if (contactvalidationlist.Count > 0)
                {
                    model.contactvalidationlist = contactvalidationlist;
                    model.IsValid = false;
                }
                
            }
            
            return model;
        }

        private string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
            {
                return regKey.GetValue("Content Type").ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"\'<>&";
            foreach (var item in specialChar)
            {
                if (input.Contains(item) || item == '"') return true;
            }

            return false;
        }

        public static bool hasfileSpecialChar(string input)
        {
            string specialChar = @"\'<>";
            foreach (var item in specialChar)
            {
                if (input.Contains(item) || item == '"') return true;
            }

            return false;
        }
        private JsonResultModel IsCheckAttendValid(AdaniGreenTalks_Contactus_Model contactUsModel, JsonResultModel model)
        {
            model.IsValid = true;
            if (contactUsModel != null)
            {
                List<ContactValidation> contactvalidationlist = new List<ContactValidation>();
                if (string.IsNullOrEmpty(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }               
                else if (contactUsModel.FirstName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                else if (hasSpecialChar(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                if (string.IsNullOrEmpty(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                else if (contactUsModel.LastName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                else if (hasSpecialChar(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                if (string.IsNullOrEmpty(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }
                else if (contactUsModel.Email.Length > 120)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }
                /*else if (!emailRegex.IsMatch(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }*/
                else if (hasSpecialChar(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }

                if (string.IsNullOrEmpty(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.ContactNumber)));
                }
                else if (contactUsModel.ContactNumber.Length > 10)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.ContactNumber)));
                }
                else if (!NumRegex.IsMatch(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.ContactNumber)));
                }

                if (string.IsNullOrEmpty(contactUsModel.CustomerQuery))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.MessageCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.WriteAboutYourself)));
                }
                else if (contactUsModel.CustomerQuery.Length > 500)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.MessageCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.WriteAboutYourself)));
                }
                else if (hasSpecialChar(contactUsModel.CustomerQuery))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.MessageCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.CustomerQuery)));
                }
                if (contactvalidationlist.Count > 0)
                {
                    model.contactvalidationlist = contactvalidationlist;
                    model.IsValid = false;
                }

            }

            return model;
        }

        private JsonResultModel IsCheckValid(AdaniGreenTalks_Contactus_Model contactUsModel, JsonResultModel model)
        {
            model.IsValid = true;
            if (contactUsModel != null)
            {
                List<ContactValidation> contactvalidationlist = new List<ContactValidation>();
                if (string.IsNullOrEmpty(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                else if (contactUsModel.FirstName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                else if (hasSpecialChar(contactUsModel.FirstName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.FirstNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.FirstName)));
                }
                if (string.IsNullOrEmpty(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                else if (!alphaNumber.IsMatch(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                else if (contactUsModel.LastName.Length > 30)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                else if (hasSpecialChar(contactUsModel.LastName))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.LastNameCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.LastName)));
                }
                if (string.IsNullOrEmpty(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }
                else if (contactUsModel.Email.Length > 120)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }
                /*else if (!emailRegex.IsMatch(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }*/
                else if (hasSpecialChar(contactUsModel.Email))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.EmailCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.Email)));
                }
                if (string.IsNullOrEmpty(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.ContactNumber)));
                }
                else if (contactUsModel.ContactNumber.Length > 10)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.ContactNumber)));
                }
                else if (!NumRegex.IsMatch(contactUsModel.ContactNumber))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.ContactNoCode, Constant.MsgValidCheck.Replace("$val$", nameof(model.ContactUs.ContactNumber)));
                }

                if (string.IsNullOrEmpty(contactUsModel.CustomerQuery))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.MessageCode, Constant.MsgNullCheck.Replace("$val$", nameof(model.ContactUs.CustomerQuery)));
                }               
                else if (contactUsModel.CustomerQuery.Length > 500)
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.MessageCode, Constant.MsgValidLengthCheck.Replace("$val$", nameof(model.ContactUs.CustomerQuery)));
                }
                else if (hasSpecialChar(contactUsModel.CustomerQuery))
                {
                    CreateValidationList(contactvalidationlist, MsgStatusCode.MessageCode, Constant.MsgInvalidValidCheck.Replace("$val$", nameof(model.ContactUs.CustomerQuery)));
                }
                if (contactvalidationlist.Count > 0)
                {
                    model.contactvalidationlist = contactvalidationlist;
                    model.IsValid = false;
                }

            }

            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JoinUs_FormPage(AdaniGreenTalks_JoinUs_Model m)
        {
            if (ModelState.IsValid)
            {
                Session["join_form_error"] = string.Empty;
                AdaniGreentalksRcaptchaV3 rcaptchaV3 = new AdaniGreentalksRcaptchaV3();
                bool captchaResult = rcaptchaV3.IsReCaptchValidV3(m.googleCaptchaToken);
                if (!captchaResult)
                {
                    Log.Error("Join Us Capctha failed", this);
                    Session["join_form_error"] = "Invalid Captcha";
                    return View(m);
                }
                Log.Info("JoinUs_FormPage POST start" + m, this);
                AdaniGreenTalksJoinUsFormProvider joinUs = new AdaniGreenTalksJoinUsFormProvider();
                bool joinusResult = joinUs.JoinUsForm(m);
                if (joinusResult)
                {
                    Log.Info("JoinUs_FormPage POST End Successfully", this);
                    return Redirect("/thankyou");
                }
                else
                {
                    Log.Error("JoinUs_FormPage POST Failed", this);
                    return View();
                }
            }
            Log.Error("JoinUs_FormPage POST Model validation failed", this);
            return View(m);
        }

        //subscribe us form - created by Neeraj yadav
        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult SubscribeUsForm(AdaniGreenTalks_SubscribeUs_Model m)
        {
            var result = new { status = "0" };

            if (ModelState.IsValid)
            {
                AdaniGreentalksRcaptchaV3 rcaptchaV3 = new AdaniGreentalksRcaptchaV3();
                bool captchaResult = rcaptchaV3.IsReCaptchValidV3(m.googleCaptchaToken);
                if (!captchaResult)
                {
                    Log.Error("SubscribeUs Form Capctha failed", this);

                    result = new { status = "11" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                AdaniGreenTalksSubscribeFormProvider cfp = new AdaniGreenTalksSubscribeFormProvider();
                bool response = cfp.SubscribeForm(m);
                if (response)
                {
                    result = new { status = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FAQSearch(string searchData)
        {
            var result = new { status = 1, message = "Success" };

            try
            {
                if (!string.IsNullOrEmpty(searchData))
                {

                    Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("web");
                    var searchFolder = web.GetItem("{ADF26A55-6A38-42C4-9177-9B4AF7B3484B}");

                    string index = string.Format("sitecore_{0}_index", Sitecore.Context.Database.Name);
                    List<FAQSearchResultItem> query;
                    List<Item> matches = new List<Item>();

                    using (var context = ContentSearchManager.GetIndex(index).CreateSearchContext())
                    {
                        var predicate = PredicateBuilder.True<FAQSearchResultItem>();

                        // must have this (.and)
                        predicate = predicate.And(p => p.Paths.Contains(searchFolder.ID));

                        // must have this (.and)
                        predicate = predicate.And(
                            PredicateBuilder.False<FAQSearchResultItem>() // in any of these fields
                            .Or(p => p.Question.Contains(searchData))
                            .Or(p => p.Answer.Contains(searchData)));

                        query = context.GetQueryable<FAQSearchResultItem>().Where(predicate).ToList();
                        var qaPair = new List<KeyValuePair<string, string>>();
                        foreach (var q in query)
                        {
                            qaPair.Add(new KeyValuePair<string, string>(q.Question, q.Answer));
                        }
                        var resu = JsonConvert.SerializeObject(qaPair);
                        return Json(resu, JsonRequestBehavior.AllowGet);
                    }



                }
            }
            catch (Exception ex)
            {
                result = new { status = 0, message = "Exception occurred in FAQ search" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
    }
}
