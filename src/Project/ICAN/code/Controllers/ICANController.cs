using Microsoft.Azure.Storage.Blob;
using Newtonsoft.Json.Linq;
using RestSharp;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.ICAN.Website;
using Sitecore.ICAN.Website.Helper;
using Sitecore.ICAN.Website.Models;
using Sitecore.ICAN.Website.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using static Sitecore.Configuration.Settings;

namespace Sitecore.ICAN.Website.Controllers
{
    public class ICANController : Controller
    {
        ICANRepository repo = new ICANRepository();
        ICANRepositoryChallenge otprepo = new ICANRepositoryChallenge();
        BlobStorageService blobstorage = new BlobStorageService();
        public ICANController()
        {
        }
       
       
        [HttpGet]
        public ActionResult ICANSubmitYourIdea()
        {
            if (Session["SchoolCoordinateNumber"] == null) {
                return Redirect("/ican-challenge");
            }
            return base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", new IcanSubmitYourIdeaModel());
        }

        [HttpPost]
        public ActionResult ICANSubmitYourIdea(IcanSubmitYourIdeaModel m,string RegisterForm=null ,string SubmitForm=null)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert ICANSubmitYourIdea", "Start");
            bool flag = true;

            try
            {
              flag = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Info(string.Concat("Failed to validate auto script : ", exception.ToString()), "Failed");
            }
            
          if(flag == true)
            { 
            Log.Info("Insert ICANSubmitYourIdea Captcha Validated", "Start");

                SubmitIdeaHelper helper = new SubmitIdeaHelper();
                
                try
                {
                    
                        ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                    bool isValid = true;
                    string errorMessage = "Invalid field values:";
                    //validate TitleForProjects
                    if (string.IsNullOrEmpty(m.TitleForProject))
                    {
                        errorMessage = errorMessage + "Title For Project";
                        isValid = false;
                    }
                    //validate TitleForProjects
                    if (string.IsNullOrEmpty(m.TweetLengthIntro))
                    {
                        errorMessage = errorMessage + "Tweet Length Intro";
                        isValid = false;
                    }
                    //validate TitleForProjects
                    if (string.IsNullOrEmpty(m.ExplainatoryText))
                    {
                        errorMessage = errorMessage + "Explainatory Text";
                        isValid = false;
                    }
                    if (!isValid)
                    {
                        ViewBag.Message = errorMessage;
                        return base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", m);
                    }


                    m.RegistrationNo = helper.GetUniqueRegNo();
                        while (true)
                        {
                            if ((
                                from a in Ican.ICANSubmitYourIdeas
                                where a.RegistrationNumber == m.RegistrationNo
                                select a).FirstOrDefault<ICANSubmitYourIdea>() == null)
                            {
                                break;
                            }
                            m.RegistrationNo = helper.GetUniqueRegNo();
                        }
                        m.Id = Guid.NewGuid();

                    ICANSubmitYourIdea IcanForm = new ICANSubmitYourIdea()

                    {

                        Id = m.Id,
                        RegistrationNumber = m.RegistrationNo,
                        Student1Name = m.FirstName_1 + " " + m.LastName_1,
                        Student2Name = m.FirstName_2 + " " + m.LastName_2,
                        Student3Name = m.FirstName_3 + " " + m.LastName_3,
                        Student4Name = m.FirstName_4 + " " + m.LastName_4,
                        Student1DOB = m.DOB_1,
                        GroupName = m.GroupName,
                        Student1Grade = m.Grade_1,
                        Student2DOB = m.DOB_2,
                        Student2Grade = m.Grade_2,
                        Student3DOB = m.DOB_3,
                        Student3Grade = m.Grade_3,
                        Student4DOB = m.DOB_4,
                        Student4Grade = m.Grade_4,

                        TitleForProject = "\"" + m.TitleForProject + "\"",
                        TweetLengthIntro = "\"" + m.TweetLengthIntro + "\"",
                        ExplainatoryText = "\"" + m.ExplainatoryText + "\"",
                        YLink=m.YLink,
                        CreatedBy = m.CreatedBy,
                        CreatedDate = new DateTime?(DateTime.Now),
                        SchoolCoordinateNumber = Session["SchoolCoordinateNumber"].ToString(),

                        };

                        if (m.Image1 != null&& m.Image1.ContentLength > 0)
                        {

                            HttpPostedFileBase Image1 = m.Image1;
                            string fileName = Image1.FileName;
                            Image1.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images01/", m.RegistrationNo, "-", m.MobileNo, "-", fileName })));
                            IcanForm.Image1 = string.Concat(new string[] { "/ICANSubmit/Images01/",m.RegistrationNo, "-", m.MobileNo, "-", fileName });
                        }
                        if (m.Image2 != null)
                        {
                            HttpPostedFileBase Image2 = m.Image2;
                            string fileNames = Image2.FileName;
                            Image2.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images02/", m.RegistrationNo, "-", m.MobileNo, "-", fileNames })));
                            IcanForm.Image2 = string.Concat(new string[] { "/ICANSubmit/Images02/", m.RegistrationNo, "-", m.MobileNo, "-", fileNames });
                        }
                        if (m.Image3 != null)
                        {
                            HttpPostedFileBase Images3 = m.Image3;
                            string Image2 = Images3.FileName;
                            Images3.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images03/", m.RegistrationNo, "-", m.MobileNo, "-", Image2 })));
                            IcanForm.Image3 = string.Concat(new string[] { "/ICANSubmit/Images03/", m.RegistrationNo, "-", m.MobileNo, "-", Image2 });
                        }
                        if (m.Image4 != null)
                        {
                            HttpPostedFileBase Images4 = m.Image4;
                            string Image4 = Images4.FileName;
                            Images4.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images04/", m.RegistrationNo, "-", m.MobileNo, "-", Image4 })));
                            IcanForm.Image4 = string.Concat(new string[] { "/ICANSubmit/Images04/", m.RegistrationNo, "-", m.MobileNo, "-", Image4 });
                        }
                        if (m.Image5 != null)
                        {
                            HttpPostedFileBase Images5 = m.Image5;
                            string Image5 = Images5.FileName;
                            Images5.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images05/", m.RegistrationNo, "-", m.MobileNo, "-", Image5 })));
                            IcanForm.Image5 = string.Concat(new string[] { "/ICANSubmit/Images05/", m.RegistrationNo, "-", m.MobileNo, "-", Image5 });
                        }
                        if (m.Image6 != null)
                        {
                            HttpPostedFileBase Images6 = m.Image6;
                            string Image6 = Images6.FileName;
                            Images6.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images06/", m.RegistrationNo, "-", m.MobileNo, "-", Image6 })));
                            IcanForm.Image6 = string.Concat(new string[] { "/ICANSubmit/Images06/", m.RegistrationNo, "-", m.MobileNo, "-", Image6 });
                        }

                        if (m.Image7 != null)
                        {
                            HttpPostedFileBase Images7 = m.Image7;
                            string Image7 = Images7.FileName;
                            Images7.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images06/", m.RegistrationNo, "-", m.MobileNo, "-", Image7 })));
                            IcanForm.Image7 = string.Concat(new string[] { "/ICANSubmit/Images06/", m.RegistrationNo, "-", m.MobileNo, "-", Image7 });
                        }


                        if (m.VideoLink != null)
                        {
                            HttpPostedFileBase Attachments = m.VideoLink;
                            string fileNam = Attachments.FileName;
                            CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer();
                            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileNam);
                            blob.UploadFromStream(Attachments.InputStream);
                         IcanForm.VideoLink = string.Concat(new string[] { "https://adanistagesa.blob.core.windows.net/ican/",fileNam});
                    }
                    ////if (m.YLink != null)
                    ////{
                    ////    HttpPostedFileBase YLink = m.YLink;
                    ////    string YLinkk = YLink.FileName;
                    ////    CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer();
                    ////    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(YLinkk);
                    ////    blob.UploadFromStream(YLink.InputStream);
                        
                    ////}



                    Ican.ICANSubmitYourIdeas.InsertOnSubmit(IcanForm);
                        Ican.SubmitChanges();
                        Log.Info("Form data saved into db successfully: ", this);
                    
                    SendMailforVendorEnroll(m);
                    return this.Redirect("/thankyou");
                    
                }

                


                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", m);
                }
            }


            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/ICAN/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", m);
            }




            return base.View("/thankyou#msform");

        }
        [HttpGet]
        public ActionResult ICANChallenge()
        {
            return base.View("/Views/ICAN/Sublayouts/ICANChallenge.cshtml");
        }

        [HttpPost]
        public ActionResult ICANChallenge(IcanSignINForm m, string RegisterForm = null, string SubmitForm = null)
        {
            ActionResult actionResult;
            var result = new { status = "1" };
            bool flag = true;
            bool isValid = true;
            string errorMessage = "Invalid field values:";
            if (string.IsNullOrEmpty(m.SchoolCoordinateNumber))
            {
                errorMessage = "Please enter the Mobile Number.";
                isValid = false;
                flag = false;
                if (!isValid)
                {
                    ViewBag.SchoolBoard = errorMessage;
                    return base.View("/Views/ICAN/Sublayouts/ICANChallenge.cshtml", m);
                }
            }
            // validate Mobile
            if (string.IsNullOrEmpty(m.SchoolCoordinateNumber) || !Regex.IsMatch(m.SchoolCoordinateNumber, @"^[0-9]{10,10}$"))
            {
                errorMessage = "Invalid Mobile Number";
                isValid = false;
                flag = false;
                if (!isValid)
                {
                    ViewBag.Message = errorMessage;
                    return base.View("/Views/ICAN/Sublayouts/ICANChallenge.cshtml", m);
                }
            }

            if (flag == true)
            {
                ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                SubmitIdeaHelper helper = new SubmitIdeaHelper();
                try
                {
                    if (!Ican.ICANChallengeSignUps.Where(u => u.TeamCoordinatorMobileNumber == m.SchoolCoordinateNumber).Any())
                    {
                        ViewBag.Message += "Please SignUp ! User does not exist.";
                        ViewBag.MessageBoxTitle = "Message";
                        ViewBag.MessageBoxButtonText = "Ok";
                        return base.View("/Views/ICAN/Sublayouts/ICANChallenge.cshtml", m);
                        //return this.Redirect("/ican-challenge-signup");
                    }

                    else
                    {

                        ICANChallengeSignIn IcanForm = new ICANChallengeSignIn()

                        {

                            ID = m.Id,
                            SchoolCoordinateNumber = m.SchoolCoordinateNumber
                        };
                        Ican.ICANChallengeSignIns.InsertOnSubmit(IcanForm);
                        Ican.SubmitChanges();
                        Log.Info("Form data saved into db successfully: ", this);


                        var ICANDashboard = Ican.ICANSubmitYourIdeas.FirstOrDefault();
                        var ICANDashboardcheck = Ican.ICANSubmitYourIdeas.Where(x => x.SchoolCoordinateNumber == m.SchoolCoordinateNumber).FirstOrDefault();
                        Session["SchoolCoordinateNumber"] = m.SchoolCoordinateNumber;
                        //SendMailforVendorEnroll(m);
                        if (ICANDashboardcheck != null)
                        {
                            return this.Redirect("/ican-challenge-dashboard");
                        }
                        else
                        {
                            return this.Redirect("/ICANSubmitIdea");
                        }

                    }
                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                }
            }


            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/ICAN/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", m);
            }




            return base.View("/ICANSubmitIdea");

        }



        [HttpGet]
        public ActionResult ICANChallengeSignUp()
        {
            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml");
        }

        [HttpPost]
        public ActionResult ICANChallengeSignUp(IcanSignUpForm m, string RegisterForm = null, string SubmitForm = null)
        {


            var result = new { status = "1" };

            Log.Info("Insert ICANSubmitYourIdea", "Start");
            bool flag = true;


            if (flag == true)
            {
                try
                {
                    ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                    bool isValid = true;
                    string errorMessage = "Invalid field values:";
                    //validate TeamCoordinatorMobileNumber
                    if (m.TeamCoordinatorMobileNumber == null || m.TeamCoordinatorMobileNumber == "")
                    {
                        errorMessage = "Please enter Mobile Number.";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.TeamCoordinatorMobileNumber = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }
                    //validate OTP
                    if (m.OTP != null)
                    {
                        string getOTP = otprepo.GetOTP(m.TeamCoordinatorMobileNumber);
                        if (m.OTP != getOTP)
                        {
                            errorMessage = "Invalid OTP";
                            isValid = false;
                            if (!isValid)
                            {
                                ViewBag.Message = errorMessage;
                                return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                            }
                        }
                    }
                    //validate OTP
                    if (m.OTP == null)
                    {
                        errorMessage = "First Enter OTP";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.Message = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }
                    //validate SchoolUDISENumber
                    if (string.IsNullOrEmpty(m.SchoolUDISENumber))
                    {
                        errorMessage = errorMessage + "SchoolUDISENumber";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.SchoolUDISENumber = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }

                    //validate SchoolAddress
                    if (string.IsNullOrEmpty(m.SchoolAddress))
                    {
                        errorMessage = errorMessage + "SchoolAddress";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.SchoolAddress = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }
                    //validate PrincipalName
                    if (string.IsNullOrEmpty(m.PrincipalName))
                    {
                        errorMessage = errorMessage + "PrincipalName";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.PrincipalName = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }
                    //validate PricipalEmail
                    if (string.IsNullOrEmpty(m.PrincipalEmailID) || !Regex.IsMatch(m.PrincipalEmailID, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        errorMessage = errorMessage + "Email";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.PrincipalEmailID = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }
                    //validate Schoolboard
                    if (string.IsNullOrEmpty(m.SchoolBoard))
                    {
                        errorMessage = errorMessage + "SchoolBoard";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.SchoolBoard = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }
                    //validate Principal Email
                    if (string.IsNullOrEmpty(m.TeamCoordinatorEmailID) || !Regex.IsMatch(m.TeamCoordinatorEmailID, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        errorMessage = errorMessage + "Email";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.TeamCoordinatorEmailID = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }
                    // validate Mobile
                    if (string.IsNullOrEmpty(m.TeamCoordinatorMobileNumber) || !Regex.IsMatch(m.TeamCoordinatorMobileNumber, @"^[0-9]{10,10}$"))
                    {
                        errorMessage = errorMessage + "Mobile";
                        isValid = false;
                        if (!isValid)
                        {
                            ViewBag.TeamCoordinatorMobileNumber = errorMessage;
                            return base.View("/Views/ICAN/Sublayouts/ICANChallengeSignUp.cshtml", m);
                        }
                    }

                    ICANChallengeSignUp Icanform = new ICANChallengeSignUp()
                    {
                        //STUID = m.Id,
                        SchoolUDISENumber = m.SchoolUDISENumber,
                        SchoolAddress = m.SchoolAddress,
                        PrincipalName = m.PrincipalName,
                        PrincipalEmailID = m.PrincipalEmailID,
                        SchoolBoard = m.SchoolBoard,
                        TeamCoordinatorName = m.TeamCoordinatorName,
                        TeamCoordinatorEmailID = m.TeamCoordinatorEmailID,
                        TeamCoordinatorMobileNumber = m.TeamCoordinatorMobileNumber,
                    };
                    Ican.ICANChallengeSignUps.InsertOnSubmit(Icanform);
                    Ican.SubmitChanges();
                    UserWelcomeEmail(m);
                    Log.Info("Form data saved into db successfully: ", this);
                    Session["SchoolCoordinateNumber"] = m.TeamCoordinatorMobileNumber;
                    return Redirect("/IcanSubmitIdea");

                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                }
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/ICAN/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
            }

            return base.View("/thankyou");

        }
        [HttpGet]
        public ActionResult ICANSubmitYourIdeaEDIT(string CoordinatorNumber)
        {
            ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
            IcanSubmitYourIdeaModel icanSubmitYourIdea = new IcanSubmitYourIdeaModel();
            Session["Number"] = CoordinatorNumber;
            var StudentList = Ican.ICANSubmitYourIdeas.Where(i => i.SchoolCoordinateNumber.Contains(CoordinatorNumber)).FirstOrDefault();
            if(StudentList != null)
            {
                var FirstName1 = StudentList.Student1Name.Split(' ');
                icanSubmitYourIdea.FirstName_1 = FirstName1[0];
                icanSubmitYourIdea.LastName_1 = FirstName1[1];
                var FirstName2 = StudentList.Student2Name.Split(' ');
                icanSubmitYourIdea.FirstName_2 = FirstName2[0];
                icanSubmitYourIdea.LastName_2 = FirstName2[1];
                var FirstName3 = StudentList.Student3Name.Split(' ');
                icanSubmitYourIdea.FirstName_3 = FirstName3[0];
                icanSubmitYourIdea.LastName_3 = FirstName3[1];
                var FirstName4 = StudentList.Student4Name.Split(' ');
                icanSubmitYourIdea.FirstName_4 = FirstName4[0];
                icanSubmitYourIdea.LastName_4= FirstName4[1];
                icanSubmitYourIdea.GroupName = StudentList.GroupName;
                 icanSubmitYourIdea.DOB_1 = StudentList.Student1DOB;
                icanSubmitYourIdea.DOB_2 = StudentList.Student2DOB;
                icanSubmitYourIdea.DOB_3 = StudentList.Student3DOB;
                icanSubmitYourIdea.DOB_4 = StudentList.Student4DOB;
                icanSubmitYourIdea.Grade_1 = StudentList.Student1Grade;
                icanSubmitYourIdea.Grade_2 = StudentList.Student2Grade;
                icanSubmitYourIdea.Grade_3 = StudentList.Student3Grade;
                icanSubmitYourIdea.Grade_4 = StudentList.Student4Grade;
                icanSubmitYourIdea.TitleForProject = StudentList.TitleForProject;
                icanSubmitYourIdea.TweetLengthIntro = StudentList.TweetLengthIntro;
                icanSubmitYourIdea.ExplainatoryText = StudentList.ExplainatoryText;
                icanSubmitYourIdea.TitleForProject = StudentList.TitleForProject;
                
                return View("/Views/ICAN/Sublayouts/ICANSubmitIdeaEDIT.cshtml", icanSubmitYourIdea);
            }
            return base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", new IcanSubmitYourIdeaModel());
        }

        [HttpPost]
        public ActionResult ICANSubmitYourIdeaEDIT(IcanSubmitYourIdeaModel m)
        {
            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert ICANSubmitYourIdea", "Start");
            bool flag = true;

            try
            {
                flag = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Info(string.Concat("Failed to validate auto script : ", exception.ToString()), "Failed");
            }

            if (flag == true)
            {
                Log.Info("Insert ICANSubmitYourIdea Captcha Validated", "Start");

                SubmitIdeaHelper helper = new SubmitIdeaHelper();

                try
                {

                    ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                    bool isValid = true;
                    string errorMessage = "Invalid field values:";
                    //validate TitleForProjects
                    if (string.IsNullOrEmpty(m.TitleForProject))
                    {
                        errorMessage = errorMessage + "Title For Project";
                        isValid = false;
                    }
                    //validate TitleForProjects
                    if (string.IsNullOrEmpty(m.TweetLengthIntro))
                    {
                        errorMessage = errorMessage + "Tweet Length Intro";
                        isValid = false;
                    }
                    //validate TitleForProjects
                    if (string.IsNullOrEmpty(m.ExplainatoryText))
                    {
                        errorMessage = errorMessage + "Explainatory Text";
                        isValid = false;
                    }
                    if (!isValid)
                    {
                        ViewBag.Message = errorMessage;
                        return base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", m);
                    }
                    var iedas = Ican.ICANSubmitYourIdeas.Where(x => x.SchoolCoordinateNumber == Session["Number"].ToString()).FirstOrDefault();
                    
                    iedas.Student1Name = m.FirstName_1 + " " + m.LastName_1;
                    iedas.Student2Name = m.FirstName_2 + " " + m.LastName_2;
                    iedas.Student3Name = m.FirstName_3 + " " + m.LastName_3;
                    iedas.Student4Name = m.FirstName_4 + " " + m.LastName_4;
                    iedas.Student1DOB = m.DOB_1;
                    iedas.GroupName = m.GroupName;
                    iedas.Student1Grade = m.Grade_1;
                    iedas.Student2DOB = m.DOB_2;
                    iedas.Student2Grade = m.Grade_2;
                    iedas.Student3DOB = m.DOB_3;
                    iedas.Student3Grade = m.Grade_3;
                    iedas.Student4DOB = m.DOB_4;
                    iedas.Student4Grade = m.Grade_4;

                    iedas.TitleForProject = "\"" + m.TitleForProject + "\"";
                    iedas.TweetLengthIntro = "\"" + m.TweetLengthIntro + "\"";
                    iedas.ExplainatoryText = "\"" + m.ExplainatoryText + "\"";



                    if (m.Image1 != null && m.Image1.ContentLength > 0)
                    {

                        HttpPostedFileBase Image1 = m.Image1;
                        string fileName = Image1.FileName;
                        Image1.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images01/", m.RegistrationNo, "-", m.MobileNo, "-", fileName })));
                        iedas.Image1 = string.Concat(new string[] { "/ICANSubmit/Images01/", m.RegistrationNo, "-", m.MobileNo, "-", fileName });
                    }
                    if (m.Image2 != null)
                    {
                        HttpPostedFileBase Image2 = m.Image2;
                        string fileNames = Image2.FileName;
                        Image2.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images02/", m.RegistrationNo, "-", m.MobileNo, "-", fileNames })));
                        iedas.Image2 = string.Concat(new string[] { "/ICANSubmit/Images02/", m.RegistrationNo, "-", m.MobileNo, "-", fileNames });
                    }
                    if (m.Image3 != null)
                    {
                        HttpPostedFileBase Images3 = m.Image3;
                        string Image2 = Images3.FileName;
                        Images3.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images03/", m.RegistrationNo, "-", m.MobileNo, "-", Image2 })));
                        iedas.Image3 = string.Concat(new string[] { "/ICANSubmit/Images03/", m.RegistrationNo, "-", m.MobileNo, "-", Image2 });
                    }
                    if (m.Image4 != null)
                    {
                        HttpPostedFileBase Images4 = m.Image4;
                        string Image4 = Images4.FileName;
                        Images4.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images04/", m.RegistrationNo, "-", m.MobileNo, "-", Image4 })));
                        iedas.Image4 = string.Concat(new string[] { "/ICANSubmit/Images04/", m.RegistrationNo, "-", m.MobileNo, "-", Image4 });
                    }
                    if (m.Image5 != null)
                    {
                        HttpPostedFileBase Images5 = m.Image5;
                        string Image5 = Images5.FileName;
                        Images5.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images05/", m.RegistrationNo, "-", m.MobileNo, "-", Image5 })));
                        iedas.Image5 = string.Concat(new string[] { "/ICANSubmit/Images05/", m.RegistrationNo, "-", m.MobileNo, "-", Image5 });
                    }
                    if (m.Image6 != null)
                    {
                        HttpPostedFileBase Images6 = m.Image6;
                        string Image6 = Images6.FileName;
                        Images6.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images06/", m.RegistrationNo, "-", m.MobileNo, "-", Image6 })));
                        iedas.Image6 = string.Concat(new string[] { "/ICANSubmit/Images06/", m.RegistrationNo, "-", m.MobileNo, "-", Image6 });
                    }

                    if (m.Image7 != null)
                    {
                        HttpPostedFileBase Images7 = m.Image7;
                        string Image7 = Images7.FileName;
                        Images7.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/ICANSubmit/Images06/", m.RegistrationNo, "-", m.MobileNo, "-", Image7 })));
                        iedas.Image7 = string.Concat(new string[] { "/ICANSubmit/Images07/", m.RegistrationNo, "-", m.MobileNo, "-", Image7 });
                    }


                    if (m.VideoLink != null)
                    {
                        HttpPostedFileBase Attachments = m.VideoLink;
                        string fileNam = Attachments.FileName;
                        CloudBlobContainer blobContainer = blobstorage.GetCloudBlobContainer();
                        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileNam);
                        blob.UploadFromStream(Attachments.InputStream);
                        iedas.VideoLink = string.Concat(new string[] { "https://adanistagesa.blob.core.windows.net/ican/", fileNam });
                    }



                  
                    Ican.SubmitChanges();
                    Log.Info("Form data saved into db successfully: ", this);

                    SendMailforVendorEnroll(m);
                    return this.Redirect("/thankyou");

                }




                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", m);
                }
            }


            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/ICAN/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/ICAN/Sublayouts/IcanSubmitIdea.cshtml", m);
            }




            return base.View("/thankyou");

        }


        

        public ActionResult Logout()
        {
            Session["SchoolCoordinateNumber"] = "";
            return Redirect("/ican-challenge");
        }
       
        [HttpGet]
        public ActionResult ICANChallengeDashboard()
        {
            if (Session["SchoolCoordinateNumber"] == null)
            {
                return Redirect("/ican-challenge");
            }
            var SchoolNumber = Session["SchoolCoordinateNumber"].ToString();
            IcanSubmitYourIdeaModel icanSubmitYour = new IcanSubmitYourIdeaModel();
            ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
            icanSubmitYour.ideas = Ican.ICANSubmitYourIdeas.Where(x => x.SchoolCoordinateNumber == SchoolNumber).ToList();
            return base.View("/Views/ICAN/Sublayouts/ICANSubmitIdeaDashboard.cshtml", icanSubmitYour);
        }

        [HttpGet]
        public ActionResult ICANJuryDashboard()
        {
            if (Session["JuryMobileNumber"] == null)
            {
                return Redirect("/ICANJuryLogin");
            }
            else
            {
                ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                var JuryNumber = Session["JuryMobileNumber"].ToString();
                var JuryName = Ican.ICANJuryLogins.Where(u => u.JuryMobileNumber == JuryNumber).Select(x => x.Name).FirstOrDefault();
                ViewBag.JuryName = JuryName;
                   
                IcanSignUpForm icanlistModel = new IcanSignUpForm();
                var icanlist1 = (from s in Ican.ICANChallengeSignUps
                                 join r in Ican.ICANSubmitYourIdeas on s.TeamCoordinatorMobileNumber equals r.SchoolCoordinateNumber
                                 orderby s.STUID
                                 select new
                                 {
                                     s.SchoolUDISENumber,
                                     s.SchoolBoard,
                                     s.SchoolAddress,
                                     s.STUID,
                                     r.CreatedDate,
                                     r.GroupName,
                                     r.RegistrationNumber
                                 }).ToList();
                foreach (var item in icanlist1)
                {
                    IcanSignUpForm icanlist = new IcanSignUpForm();
                    icanlist.GroupName = item.GroupName;
                    icanlist.SchoolAddress = item.SchoolAddress;
                    icanlist.SchoolBoard = item.SchoolBoard;
                    icanlist.SchoolUDISENumber = item.SchoolUDISENumber;
                    icanlist.ApplicationDate = item.CreatedDate;
                    icanlist.RegistrationNo = item.RegistrationNumber;
                    int ReviewedScoreCount = Ican.ICANJuryScores.Where(x => x.GroupName == item.GroupName && x.SchoolUDISENumber == item.SchoolUDISENumber && x.JuryPhoneNumber == JuryNumber && x.RegistrationNumber==item.RegistrationNumber).Count();
                    icanlist.ReviewedCount = ReviewedScoreCount;
                    icanlistModel.jury.Add(icanlist);
                }

                return base.View("/Views/ICAN/Sublayouts/IcanJuryDashboard.cshtml", icanlistModel);
            }
        }

        

        [HttpGet]
        public ActionResult ICANJuryScore(string GroupName, string UDISENumber, DateTime? DateOfApplication, string RegistrationNo)
        {
            if (Session["JuryMobileNumber"] == null)
            {
                return Redirect("/ICANJuryLogin");
            }
            else
            {
                ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                var JuryNumber = Session["JuryMobileNumber"].ToString();
                var JuryName = Ican.ICANJuryLogins.Where(u => u.JuryMobileNumber == JuryNumber).Select(x => x.Name).FirstOrDefault();
                ViewBag.JuryName = JuryName;
                int ReviewedScoreCount = Ican.ICANJuryScores.Where(x => x.GroupName == GroupName && x.SchoolUDISENumber == UDISENumber && x.JuryPhoneNumber == JuryNumber && x.RegistrationNumber==RegistrationNo).Count();
                ViewBag.Count = ReviewedScoreCount;
                ICANJuryScoreModel ICANJuryScore = new ICANJuryScoreModel();
                var icanlist1 = (from s in Ican.ICANChallengeSignUps
                                 join r in Ican.ICANSubmitYourIdeas on s.TeamCoordinatorMobileNumber equals r.SchoolCoordinateNumber
                                 orderby s.STUID
                                 select new
                                 {
                                     s.SchoolAddress,
                                     s.SchoolUDISENumber,
                                     s.PrincipalName,
                                     s.PrincipalEmailID,
                                     s.SchoolBoard,
                                     s.TeamCoordinatorName,
                                     s.TeamCoordinatorEmailID,
                                     s.TeamCoordinatorMobileNumber,
                                     r.CreatedDate,
                                     r.GroupName,
                                     r.TitleForProject,
                                     r.ExplainatoryText,
                                     r.Image1,
                                     r.Image2,
                                     r.Image7,
                                     r.VideoLink,
                                     r.YLink,
                                     r.Student1Name, r.Student1DOB, r.Student1Grade,
                                     r.Student2Name,
                                     r.Student2DOB,
                                     r.Student2Grade,
                                     r.Student3Name,
                                     r.Student3DOB,
                                     r.Student3Grade,
                                     r.Student4Name,
                                     r.Student4DOB,
                                     r.Student4Grade,
                                     r.RegistrationNumber
                                 }).Where(x=>x.GroupName==GroupName && x.SchoolUDISENumber==UDISENumber && x.RegistrationNumber==RegistrationNo).ToList();
                foreach (var item in icanlist1)
                {
                    ICANJuryScoreModel icanscore = new ICANJuryScoreModel();
                    icanscore.SchoolAddress = item.SchoolAddress;
                    icanscore.SchoolUDISENumber = item.SchoolUDISENumber;
                    icanscore.PrincipalName = item.PrincipalName;
                    icanscore.PrincipalEmailID = item.PrincipalEmailID;
                    icanscore.SchoolBoard = item.SchoolBoard;
                    icanscore.TeamCoordinatorName = item.TeamCoordinatorName;
                    icanscore.TeamCoordinatorEmailID = item.TeamCoordinatorEmailID;
                    icanscore.TeamCoordinatorMobileNumber = item.TeamCoordinatorMobileNumber;
                    icanscore.GroupName = item.GroupName;
                    icanscore.ProjectTitle = item.TitleForProject;
                    icanscore.ExplanatoryText = item.ExplainatoryText;
                    icanscore.Image1 = item.Image1;
                    icanscore.Image2 = item.Image2;
                    icanscore.PDF = item.Image7;
                    icanscore.Video = item.VideoLink;
                    icanscore.YouTubeLink = item.YLink;
                    icanscore.DateOfApplication = item.CreatedDate;
                    icanscore.StudentName1 = item.Student1Name;
                    icanscore.StudentDOB1 = item.Student1DOB;
                    icanscore.StudentGrade1 = item.Student1Grade;
                    icanscore.StudentName2 = item.Student2Name;
                    icanscore.StudentDOB2 = item.Student2DOB;
                    icanscore.StudentGrade2 = item.Student2Grade;
                    icanscore.StudentName3 = item.Student3Name;
                    icanscore.StudentDOB3 = item.Student3DOB;
                    icanscore.StudentGrade3 = item.Student3Grade;
                    icanscore.StudentName4 = item.Student4Name;
                    icanscore.StudentDOB4 = item.Student4DOB;
                    icanscore.StudentGrade4 = item.Student4Grade;
                    icanscore.RegistrationNo = item.RegistrationNumber;
                    ICANJuryScore.SchoolUDISENumber = item.SchoolUDISENumber;
                    ICANJuryScore.JuryMobileNumber = JuryNumber;
                    ICANJuryScore.GroupName = item.GroupName;
                    ICANJuryScore.RegistrationNo = item.RegistrationNumber;
                    ICANJuryScore.juryScore.Add(icanscore);
                }
               
                return base.View("/Views/ICAN/Sublayouts/ICANJuryScore.cshtml", ICANJuryScore);
            }
        }


        [HttpPost]
        public ActionResult ICANJuryScore(ICANJuryScoreModel m, string RegisterForm = null, string SubmitForm = null)
        {
            var result = new { status = "1" };

            Log.Info("Insert ICANJuryScore", "Start");
            bool flag = true;
            bool isValid = true;
            string errorMessage = "Invalid field values:";
            if (string.IsNullOrEmpty(m.Notes))
            {
                errorMessage = "Please enter the notes.";
                isValid = false;
                flag = false;
                if (!isValid)
                {
                    ViewBag.SchoolBoard = errorMessage;
                    return base.View("/Views/ICAN/Sublayouts/ICANJuryScore.cshtml", m);
                }
            }
            if (flag == true)
            {
                try
                {
                    ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                    ICANJuryScore IcanJuyScoreform = new ICANJuryScore()
                    {
                        //STUID = m.Id,
                        CreativityScore = m.CreativityScore,
                        DetailingScore = m.DetailingScore,
                        ScientificScore = m.ScientificScore,
                        PermanencyScore = m.PermanencyScore,
                        ClimateScore = m.ClimateScore,
                        OverallPresentaionScore = m.OverallPresentationScore,
                        TotalScore = m.TotalScore,
                        Notes = m.Notes,
                        CreatedDate = new DateTime?(DateTime.Now),
                        GroupName = m.GroupName,
                        JuryPhoneNumber = m.JuryMobileNumber,
                        SchoolUDISENumber = m.SchoolUDISENumber,
                        RegistrationNumber = m.RegistrationNo
                    };
                    Ican.ICANJuryScores.InsertOnSubmit(IcanJuyScoreform);
                    Ican.SubmitChanges();
                    Log.Info("Form data saved into db successfully: ", this);
                    return Redirect("/ican-jury-dashboard");

                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                }
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/ICAN/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
            }

            return base.View("/thankyou");

        }

        [HttpGet]
        public ActionResult ICANFinalistDashboard()
        {
            if (Session["JuryMobileNumber"] == null)
            {
                return Redirect("/ICANJuryLogin");
            }
            else
            {
                ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                var JuryNumber = Session["JuryMobileNumber"].ToString();
                var JuryName = Ican.ICANJuryLogins.Where(u => u.JuryMobileNumber == JuryNumber).Select(x => x.Name).FirstOrDefault();
                ViewBag.JuryName = JuryName;
                ICANFinaistModel icanlistModel = new ICANFinaistModel();
                var icanlist1= Ican.ICANFinalistDashboardNews.ToList();
                foreach (var item in icanlist1)
                {
                    ICANFinaistModel icanlist = new ICANFinaistModel();
                    icanlist.Id = item.ID;
                    icanlist.SchoolName = item.SchoolName;
                    icanlist.ProjectName = item.ProjectName;
                    icanlist.SchoolUDISENumber = item.UDISE;
                    icanlist.CoordinateName = item.CoordinateName;
                    icanlistModel.ICANFinalist.Add(icanlist);
                }

                return base.View("/Views/ICAN/Sublayouts/ICANFinalistDashboard.cshtml", icanlistModel);
            }
        }

        [HttpGet]
        public ActionResult ICANFinalistReview(int ID)
        {
            if (Session["JuryMobileNumber"] == null)
            {
                return Redirect("/ICANJuryLogin");
            }
            else
            {
                ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
                var JuryNumber = Session["JuryMobileNumber"].ToString();
                var JuryName = Ican.ICANJuryLogins.Where(u => u.JuryMobileNumber == JuryNumber).Select(x => x.Name).FirstOrDefault();
                ViewBag.JuryName = JuryName;
                ICANFinaistModel ICANFinalistList = new ICANFinaistModel();
                var icanlist1 = (from s in Ican.ICANFinalistDashboardNews
                                 join r in Ican.ICANFinalistReviews on s.ID equals r.SrNo
                                 select new
                                 {
                                     s.ID,
                                     s.SchoolName,
                                     s.ProjectName,
                                     s.UDISE,
                                     s.CoordinateName,
                                     r.School_details,
                                     r.Project_Name,
                                     r.CreatedDate,
                                     r.GroupName,
                                     r.TitleForProject,
                                     r.ExplainatoryText,
                                     r.Image1,
                                     r.Image2,
                                     r.Image7,
                                     r.VideoLink,
                                     r.YLink,
                                     r.Student1Name,
                                     r.Student1DOB,
                                     r.Student1Grade,
                                     r.Student2Name,
                                     r.Student2DOB,
                                     r.Student2Grade,
                                     r.Student3Name,
                                     r.Student3DOB,
                                     r.Student3Grade,
                                     r.Student4Name,
                                     r.Student4DOB,
                                     r.Student4Grade,
                                     r.RegistrationNumber,
                                     r.SchoolCoordinateNumber
                                 }).Where(x =>x.ID==ID).ToList();
                foreach (var item in icanlist1)
                {
                    ICANFinaistModel icanfinalist = new ICANFinaistModel();
                    icanfinalist.SchoolName = item.SchoolName;
                    icanfinalist.ProjectName = item.ProjectName;
                    icanfinalist.SchoolUDISENumber = item.UDISE;
                    icanfinalist.CoordinateName = item.CoordinateName;
                    icanfinalist.GroupName = item.GroupName;
                    icanfinalist.TeamCoordinatorMobileNumber = item.SchoolCoordinateNumber;
                    icanfinalist.ProjectTitle = item.TitleForProject;
                    icanfinalist.ExplanatoryText = item.ExplainatoryText;
                    icanfinalist.Image1 = item.Image1;
                    icanfinalist.Image2 = item.Image2;
                    icanfinalist.PDF = item.Image7;
                    icanfinalist.Video = item.VideoLink;
                    icanfinalist.YouTubeLink = item.YLink;
                    icanfinalist.DateOfApplication = item.CreatedDate;
                    icanfinalist.StudentName1 = item.Student1Name;
                    icanfinalist.Student1DOB = item.Student1DOB;
                    icanfinalist.StudentGrade1 = item.Student1Grade;
                    icanfinalist.StudentName2 = item.Student2Name;
                    icanfinalist.Student2DOB = item.Student2DOB;
                    icanfinalist.StudentGrade2 = item.Student2Grade;
                    icanfinalist.StudentName3 = item.Student3Name;
                    icanfinalist.Student3DOB = item.Student3DOB;
                    icanfinalist.StudentGrade3 = item.Student3Grade;
                    icanfinalist.StudentName4 = item.Student4Name;
                    icanfinalist.Student4DOB = item.Student4DOB;
                    icanfinalist.StudentGrade4 = item.Student4Grade;
                    ICANFinalistList.JuryMobileNumber = JuryNumber;
                    ICANFinalistList.GroupName = item.GroupName;
                    ICANFinalistList.ICANFinalist.Add(icanfinalist);
                }

                return base.View("/Views/ICAN/Sublayouts/ICANFinalistReview.cshtml", ICANFinalistList);
            }
        }


        [HttpGet]
        public ActionResult ICANJuryLogin()
        {
            return base.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml");
        }

        [HttpPost]
        public ActionResult ICANJuryLogin(IcanJurySignINForm m)
        {
            ICANSubmitIdeaDataContext Ican = new ICANSubmitIdeaDataContext();
            var result = new { status = "1" };
            Log.Info("Insert ICANSubmitYourIdea", "Start");
            bool flag = true;
            bool isValid = true;
            string errorMessage = "Invalid field values:";
            if(m.JuryMobileNumber==null || m.JuryMobileNumber=="")
            {
                errorMessage = "Please enter Mobile Number.";
                isValid = false;
                flag = false;
                return this.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml", m);
            }
            if (string.IsNullOrEmpty(m.JuryMobileNumber) || !Regex.IsMatch(m.JuryMobileNumber, @"^[0-9]{10,10}$"))
            {
                errorMessage = "Invalid Mobile Number.";
                isValid = false;
                flag = false;
                return this.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml", m);
            }
            if (!Ican.ICANJuryLogins.Where(u => u.JuryMobileNumber == m.JuryMobileNumber).Any())
            {
                errorMessage = "Entered Mobile Number is not Registered.";
                isValid = false;
                flag = false;
                return this.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml", m);
            }
            string getOTP = "";
            if (m.OTP != null)
            {
                 getOTP = otprepo.GetOTP(m.JuryMobileNumber);
            }
            if (m.OTP == null)
            {
                errorMessage = "First Pls Enter OTP";
                isValid = false;
                flag = false;
                return this.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml", m);
            }
            if (getOTP != null && getOTP!="")
            {
                if (m.OTP != getOTP)
                {
                    errorMessage = "Invalid OTP";
                    isValid = false;
                    flag = false;
                    return this.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml", m);
                }
            }
            
            if (!isValid)
            {
                ViewBag.Message = errorMessage;
                return base.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml", m);
            }
            if (flag == true)
            {
                try
                {
                    Session["JuryMobileNumber"] = m.JuryMobileNumber;
                    if (Session["JuryMobileNumber"] == null)
                    {
                        return Redirect("/ICANJuryLogin");
                    }
                    var JuryNumber = Session["JuryMobileNumber"].ToString();
                    if (JuryNumber != null)
                    {
                        return this.Redirect("/ican-jury-dashboard");
                    }
                    else
                    {
                        return this.Redirect("/ICANSubmitIdea");
                    }
                  

                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Contact Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                }
            }
            else
            {
                errorMessage = "Please Enter the valid OTP or Mobile Nummber";
                ViewBag.Message = errorMessage;
                return base.View("/Views/ICAN/Sublayouts/ICANJuryLogin.cshtml", m);
            }

            return base.View("/thankyou");

        }

        public ActionResult LogoutJury()
        {
            Session["JuryMobileNumber"] = "";
            return Redirect("/ICANJuryLogin");
        }
        [HttpPost]
        public ActionResult ICANSendOtp(ICANChallengeSignUp model)
        {
            //EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };
            ICANSubmitIdeaDataContext rdb = new ICANSubmitIdeaDataContext();
            ICANOtpHistory r = new ICANOtpHistory();
            try
            {
                bool flag = false;
                bool isValid = false;
                string errorMessage = "Invalid field values:";
                if (!string.IsNullOrEmpty(model.TeamCoordinatorMobileNumber) || !Regex.IsMatch(model.TeamCoordinatorMobileNumber, @"^[0-9]{10,10}$"))
                {
                    isValid = true;
                    flag = true;
                }
                else
                {
                    var result1 = new { status = "0", msg = "Invalid Mobile Number" };
                    return Json(result1, JsonRequestBehavior.AllowGet);
                }
                if (flag == true)
                {
                    var currDate = DateTime.Now;
                    #region Delete Available otp from database for given mobile number

                    otprepo.DeleteOldOtp(model.TeamCoordinatorMobileNumber);
                    #endregion
                    #region Generate New Otp for given mobile number and save to database
                    string generatedotp = otprepo.StoreGeneratedOtp(model);
                    #endregion
                    #region Api call to send SMS of OTP
                    try
                    {
                        //var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);
                        var apiurl = string.Format("https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADNCOM&password=adani321&type=TEXT&sender=ADNCOM&mobile={0}&message=Dear%20Member%2C%0A%7B%23{1}%23%7D%20is%20your%20one%20time%20password%20%28OTP%29%20to%20register%20with%20iCan.%20Please%20enter%20the%20OTP%20to%20proceed.%0AAdani%20iCan%0A&PEID=1101537230000028798&HeaderId=1105159548146149232&templateId=1107161899896365307", model.TeamCoordinatorMobileNumber, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Error("OTP Api call success. https://bulksms.analyticsmantra.com/", this);
                            result = new { status = "1" };
                            //r.MobileNumber = model.TeamCoordinatorMobileNumber;
                            r.otp = generatedotp;
                            r.status = false;
                            r.count = 0;
                            r.date = DateTime.Now;
                            rdb.ICANOtpHistories.InsertOnSubmit(r);

                            rdb.SubmitChanges();
                            var result1 = new { status = "1", msg1 = "OTP is sent to the registered Mobile Number" };
                            return Json(result1, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Log.Error("OTP Api call failed. https://bulksms.analyticsmantra.com/", this);
                            result = new { status = "0" };
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{0}", ex, this);
                    }
                    #endregion

                    #region Return Response with Mobile Number and Generated otp
                    //result = new { status = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);


                    #endregion

                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult ICANSendJuryOTP(IcanJurySignINForm model)
        {
            //EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };
            ICANSubmitIdeaDataContext rdb = new ICANSubmitIdeaDataContext();
            ICANOtpHistory r = new ICANOtpHistory();
            try
            {
                bool flag = false;
                bool isValid = false;
                string errorMessage = "Invalid field values:";
                if (rdb.ICANJuryLogins.Where(u => u.JuryMobileNumber == model.JuryMobileNumber).Any())
                {
                    isValid = true;
                    flag = true;
                }
                else {
                   var result1 = new { status = "0",msg= "Entered Mobile Number is not Registered" };
                    errorMessage = "Entered Mobile Number is not Registered.";
                    return Json(result1, JsonRequestBehavior.AllowGet);
                }
                if (flag == true)
                {
                    var currDate = DateTime.Now;
                    #region Delete Available otp from database for given mobile number

                    otprepo.DeleteOldOtp(model.JuryMobileNumber);
                    #endregion
                    #region Generate New Otp for given mobile number and save to database
                    string generatedotp = otprepo.StoreGeneratedJuryOtp(model);
                    #endregion
                    #region Api call to send SMS of OTP
                    try
                    {
                        //var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);
                        var apiurl = string.Format("https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADNCOM&password=adani321&type=TEXT&sender=ADNCOM&mobile={0}&message=Dear%20Member%2C%0A%7B%23{1}%23%7D%20is%20your%20one%20time%20password%20%28OTP%29%20to%20register%20with%20iCan.%20Please%20enter%20the%20OTP%20to%20proceed.%0AAdani%20iCan%0A&PEID=1101537230000028798&HeaderId=1105159548146149232&templateId=1107161899896365307", model.JuryMobileNumber, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Error("OTP Api call success. https://bulksms.analyticsmantra.com/", this);
                            result = new { status = "1" };
                            r.MobileNumber = model.JuryMobileNumber;
                            r.otp = generatedotp;
                            r.status = false;
                            r.count = 0;
                            r.date = DateTime.Now;
                            rdb.ICANOtpHistories.InsertOnSubmit(r);

                            rdb.SubmitChanges();
                            var result1 = new { status = "1", msg1 = "OTP is sent to the registered Mobile Number" };
                            return Json(result1, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            Log.Error("OTP Api call failed. https://bulksms.analyticsmantra.com/", this);
                            result = new { status = "0" };
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"{0}", ex, this);
                    }
                    #endregion

                    #region Return Response with Mobile Number and Generated otp
                    //result = new { status = "1" };
                    return Json(result, JsonRequestBehavior.AllowGet);


                    #endregion

                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            string str = reResponse;
            string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
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

       
        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool flag;
            bool status = false;
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(from)
                };
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                ContentType ct = new ContentType("application/pdf");
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                status = true;
                flag = status;
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                flag = status;
            }
            return flag;
        }

public void SendMailforVendorEnroll(IcanSubmitYourIdeaModel model)
        {
            try
            {
                Item mailconfig = Context.Database.GetItem(Templates.MailConfiguration.MailConfigurationItemID);
                Log.Info("Payment Success mail sending to client", this);
                string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                string CustomerTo = model.EmailID;
                string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                string CustomerMailBody = CustomerMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SuccessMessage].Value;

                string OfficialsFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_MailFrom].Value;
                string OfficialsTo = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_RecipientMail].Value;
                string OfficialsMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_Message].Value;
                string OfficialsSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_SubjectName].Value;

                using (ICANSubmitIdeaDataContext dbcontext = new ICANSubmitIdeaDataContext())
                {
                    ICANSubmitYourIdea ctx = dbcontext.ICANSubmitYourIdeas.Where(x => x.Id == model.Id && x.RegistrationNumber == model.RegistrationNo).FirstOrDefault();
                    CustomerMailBody = CustomerMailBody.Replace("$name1", ctx.Student1Name);
                    

                    CustomerMailBody = CustomerMailBody.Replace("$DateOfBirth", ctx.Student1DOB.ToString());
                  
                    CustomerMailBody = CustomerMailBody.Replace("$IntriguingTitle", ctx.TitleForProject);
                    CustomerMailBody = CustomerMailBody.Replace("$TweetIntro", ctx.TweetLengthIntro);
                    CustomerMailBody = CustomerMailBody.Replace("$ExplanatoryText", ctx.ExplainatoryText);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", ctx.RegistrationNumber);
                    //officials mail body
                    OfficialsMailBody = OfficialsMailBody.Replace("$name1", ctx.Student1Name);
                    OfficialsMailBody = OfficialsMailBody.Replace("$DateOfBirth", ctx.Student1DOB.ToString());

                    OfficialsMailBody = OfficialsMailBody.Replace("$IntriguingTitle", ctx.TitleForProject);
                    OfficialsMailBody = OfficialsMailBody.Replace("$TweetIntro", ctx.TweetLengthIntro);
                    OfficialsMailBody = OfficialsMailBody.Replace("$ExplanatoryText", ctx.ExplainatoryText);

                    //OfficialsMailBody = OfficialsMailBody.Replace("$teleno", ctx.TelephoneNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$registrationno", ctx.RegistrationNumber);

                }
                var mailSendingCust = sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom);
                if (mailSendingCust == true)
                {
                    Log.Info("Sending mail to customer is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to customer is Failed", this);
                }
                var mailSendingOfc = sendEmail(OfficialsTo, OfficialsSubject, OfficialsMailBody, OfficialsFrom);
                if (mailSendingOfc == true)
                {
                    Log.Info("Sending mail to Officials is Successfull", this);
                }
                else
                {
                    Log.Info("Sending mail to Officials is Failed", this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SendMailforVendorEnroll Form : " + ex.Message, this);
            }

        }

        private void UserWelcomeEmail(IcanSignUpForm m)
        {
            MailMessage mail = null;
            



            try
            {
                var settingsItem = Context.Database.GetItem("{EF45F23F-3354-40E1-A306-2E57F28F77D1}");





                var mailTemplateItem = settingsItem;
                //From field
                var fromMail = mailTemplateItem.Fields["{8605948C-60FB-46B8-8AAA-4C52561B53BC}"];



                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                //body field
                var body = mailTemplateItem.Fields["{1519CCAD-ED26-4F60-82CA-22079AF44D16}"];
                // subject field
                var subject = mailTemplateItem.Fields["{0F45DF05-546F-462D-97C0-BA4FB2B02564}"];



                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }



            try
            {
                mail.To.Add(m.TeamCoordinatorEmailID);
                mail.Body = mail.Body.Replace("#NAME#", m.TeamCoordinatorName);
                mail.Body = mail.Body.Replace("#UDISE#", m.SchoolUDISENumber);
                mail.Body = mail.Body.Replace("#SAD#", m.SchoolAddress);
                mail.Body = mail.Body.Replace("#PNAME#", m.PrincipalName);
                mail.Body = mail.Body.Replace("#PEMAIL#", m.PrincipalEmailID);
                mail.Body = mail.Body.Replace("#SB#", m.SchoolBoard);
                mail.Body = mail.Body.Replace("#TNAME#", m.TeamCoordinatorName);
                mail.Body = mail.Body.Replace("#TEMAIL#", m.TeamCoordinatorEmailID);
                mail.Body = mail.Body.Replace("#TM#", m.TeamCoordinatorMobileNumber);



                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + m.TeamCoordinatorEmailID + " - Error - " + ex.Message + "", ex, this);
            }
        }






    }
}


