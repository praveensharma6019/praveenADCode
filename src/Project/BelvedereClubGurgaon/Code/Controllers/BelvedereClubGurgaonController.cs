using Sitecore.BelvedereClubGurgaon.Website.Models;
using Sitecore.BelvedereClubGurgaon.Website.Services;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.BelvedereClubGurgaon.Website.Controllers
{
    public class BelvedereClubGurgaonController : Controller
    {
        ClubGurgaonRepository clubgurgaonRepo = new ClubGurgaonRepository();

        // GET: BelvedereClubGurgaon
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateOTP(ClubGurgaonEnquiryModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            try
            {

                #region Delete Available otp from database for given mobile number

                clubgurgaonRepo.DeleteOldOtp(model.Mobile);
                #endregion

                #region Generate New Otp for given mobile number and save to database
                string generatedotp = clubgurgaonRepo.StoreGeneratedOtp(model);
                #endregion

                #region Api call to send SMS of OTP
                try
                {
                    var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.Mobile, generatedotp);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // HttpResponseMessage response = 
                    Task<HttpResponseMessage> x = client.GetAsync(apiurl);
                    HttpResponseMessage response = x.Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://otp2.maccesssmspush.com", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://otp2.maccesssmspush.com", this);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                result = new { status = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);

                //return new JsonResult() { Data = new { MobileNumber = model.Mobile,otp= generatedotp, message = "" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult VerifyOTP(ClubGurgaonEnquiryModel model)
        {
            var result = new { status = "0" };
            #region Verify OTP
            string generatedOTP = clubgurgaonRepo.GetOTP(model.Mobile);
            if (string.Equals(generatedOTP, model.OTP))
            {
                result = new { status = "1" };
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool status = false;
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                //mail.From = new MailAddress(Sitecore.Configuration.Settings.MailServerUserName);
                mail.From = new MailAddress(from);
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, ct);
                // attachment.ContentDisposition.FileName = fileName;
                // mail.Attachments.Add(attachment);
                MainUtil.SendMail(mail);
                status = true;
                return status;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                return status;
            }
        }
        [HttpPost]
        public ActionResult InsertEnquiryNowdetail(ClubGurgaonEnquiryModel m)
        {


            Log.Error("Insertcontactdetail ", "Start");
            var result = new { status = "1" };
            try
            {
                ClubGurgaonEnquiryDataContext rdb = new ClubGurgaonEnquiryDataContext();
                ClubGurgaonEnquiry r = new ClubGurgaonEnquiry();

                r.Name = m.Name;
                r.Email = m.Email;
                r.InterestedIn = m.InterestedIn;
                r.City = m.City;
                r.Mobile = m.Mobile;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.ClubGurgaonEnquiries.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try
            {
                string message = "";
                message = "Hello<br><br>Below are the Enquiry details of the Customer:<br><br>Name: " + m.Name;
                message = message + "<br>Email: " + m.Email + "<br>Interested In: " + m.InterestedIn + "<br>City: " + m.City + "<br>Contact No: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailSubject", "");
                bool results = sendEmail(to, emailSubject, message, from);

                if (results)
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception ex)
            {
                result = new { status = "1" };
                Log.Error("Failed to sent Email", ex.ToString());
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertBrochuredetail(ClubGurgaonEnquiryModel m)
        {
            Log.Error("Insertcontactdetail ", "Start");
            var result = new { status = "1" };
            try
            {
                ClubGurgaonEnquiryDataContext rdb = new ClubGurgaonEnquiryDataContext();
                ClubGurgaonEnquiry r = new ClubGurgaonEnquiry();

                r.Name = m.Name;
                r.Email = m.Email;
                r.Mobile = m.Mobile;
                r.FormType = m.FormType;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.ClubGurgaonEnquiries.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try
            {
                string message = "";
                message = "Hello<br><br>Below are the details of customer Downloading the Brochure:<br><br>Name: " + m.Name;
                message = message + "<br>Email: " + m.Email + "<br>Contact No: " + m.Mobile + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/DownloadBrochure/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/DownloadBrochure/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/DownloadBrochure/EmailSubject", "");
                bool results = sendEmail(to, emailSubject, message, from);
                if (results)
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception ex)
            {
                result = new { status = "1" };
                Console.WriteLine(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogin(Login model)
        {
            try
            {
                var item = Context.Database.GetItem(Templates.LeadGeneration.LeadCreation);
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                using (ClubGurgaonEnquiryDataContext dbcontext = new ClubGurgaonEnquiryDataContext())
                {
                    var registerUser = dbcontext.Registrations.Where(x => x.UserId == model.LoginName && x.Password == model.Password && x.UserType.ToLower() == "clubgurgaon" && x.status == true).FirstOrDefault();
                    if (registerUser == null)
                    {
                        ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Realty/UserLogin/Login User Error", "User and password is not valid. Please enter valid credential."));
                        return this.View(model);
                    }

                    UserSession.UserSessionContext = new LoginModel
                    {
                        userId = registerUser.UserId,
                        leadCity = registerUser.City,
                        UserType = registerUser.UserType
                    };
                    var url = item.Url();
                    return this.Redirect(url);
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at UserLoginLogin Post:" + ex.Message, this);
                ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Realty/UserLogin/Login Technical Error", "There is technical problem. Please try after sometime."));
                return this.View(model);
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var item = Context.Database.GetItem(Templates.LeadGeneration.LogOut);
            if (UserSession.UserSessionContext != null && !string.IsNullOrEmpty(UserSession.UserSessionContext.userId))
            {
                using (ClubGurgaonEnquiryDataContext dbcontext = new ClubGurgaonEnquiryDataContext())
                {
                    var result = (from user in dbcontext.Registrations where user.UserId == UserSession.UserSessionContext.userId select user).Single();
                    result.Modified_Date = System.DateTime.Now;
                    result.ModifiedBy = UserSession.UserSessionContext.userId;
                    dbcontext.SubmitChanges();
                }
            }
            this.Session["UserLogin"] = null;
            UserSession.UserSessionContext = null;
            return this.Redirect(item.Url());
        }

        [HttpGet]
        public ActionResult LeadGeneration()
        {
            if (UserSession.UserSessionContext == null || string.IsNullOrEmpty(UserSession.UserSessionContext.userId))
            {
                this.Session["UserLogin"] = null;
                UserSession.UserSessionContext = null;
                var item = Context.Database.GetItem(Templates.LeadGeneration.LogOut);
                return this.Redirect(item.Url());
            }
            LeadGenerationModel m = new LeadGenerationModel();

            m.LeadSource = new List<SelectListItem>();
            m.LeadSource.Add(new SelectListItem { Text = "Social Media", Value = "Social Media" });
            m.LeadSource.Add(new SelectListItem { Text = "Print Advt. ", Value = "Print Advt." });
            m.LeadSource.Add(new SelectListItem { Text = "Tele - marketing", Value = "Tele - marketing" });
            m.LeadSource.Add(new SelectListItem { Text = "Events", Value = "Events" });

            m.LeadSource.Add(new SelectListItem
            {
                Text = "Outdoor Marketing Campaigns",
                Value = "Outdoor Marketing Campaigns",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Existing Member Reference",
                Value = "Existing Member Reference",
            });
           
            m.LeadStatus = new List<SelectListItem>();
            m.LeadStatus.Add(new SelectListItem { Text = "Open", Value = "Open" });
            m.LeadStatus.Add(new SelectListItem { Text = "Contacted", Value = "Contacted" });

            m.Lead_OwnersList = new List<SelectListItem>();
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Bhoopendra Mali", Value = "Bhoopendra Mali" });
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Darshna Chauhan", Value = "Darshna Chauhan" });
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Kapil Lalvani", Value = "Kapil Lalvani" });
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Jay Raval", Value = "Jay Raval" });
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Neeraj Kushwa", Value = "Neeraj Kushwa" });
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Parth Shah", Value = "Parth Shah" });
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Person-1", Value = "Person-1" });
            m.Lead_OwnersList.Add(new SelectListItem { Text = "Person-2", Value = "Person-2" });

            m.InterestedInList = new List<SelectListItem>();
            m.InterestedInList.Add(new SelectListItem { Text = "Yearly Membership", Value = "Yearly Membership" });
            m.InterestedInList.Add(new SelectListItem { Text = "10 Years Non-refundable Membership", Value = "10 Years Non-refundable Membership" });
            m.InterestedInList.Add(new SelectListItem { Text = "25 Years Refundable Membership", Value = "25 Years Refundable Membership" });
            m.InterestedInList.Add(new SelectListItem { Text = "Corporate Membership", Value = "Corporate Membership" });

            m.Gender = new List<SelectListItem>();
            m.Gender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            m.Gender.Add(new SelectListItem { Text = "Female", Value = "Female" });

            return View(m);
        }

        [HttpPost]
        public ActionResult LeadGeneration(LeadGenerationModel m)
        {
            try
            {
                
                if (!ModelState.IsValid)
                {
                    m.LeadSource = new List<SelectListItem>();
                    m.LeadSource.Add(new SelectListItem { Text = "Social Media", Value = "Social Media" });
                    m.LeadSource.Add(new SelectListItem { Text = "Print Advt. ", Value = "Print Advt." });
                    m.LeadSource.Add(new SelectListItem { Text = "Tele - marketing", Value = "Tele - marketing" });
                    m.LeadSource.Add(new SelectListItem { Text = "Events", Value = "Events" });

                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Outdoor Marketing Campaigns",
                        Value = "Outdoor Marketing Campaigns",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Existing Member Reference",
                        Value = "Existing Member Reference",
                    });
                   
                    m.LeadStatus = new List<SelectListItem>();
                    m.LeadStatus.Add(new SelectListItem { Text = "Open", Value = "Open" });
                    m.LeadStatus.Add(new SelectListItem { Text = "Contacted", Value = "Contacted" });

                    m.Lead_OwnersList = new List<SelectListItem>();
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Bhoopendra Mali", Value = "Bhoopendra Mali" });
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Darshna Chauhan", Value = "Darshna Chauhan" });
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Kapil Lalvani", Value = "Kapil Lalvani" });
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Jay Raval", Value = "Jay Raval" });
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Neeraj Kushwa", Value = "Neeraj Kushwa" });
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Parth Shah", Value = "Parth Shah" });
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Person-1", Value = "Person-1" });
                    m.Lead_OwnersList.Add(new SelectListItem { Text = "Person-2", Value = "Person-2" });

                    m.InterestedInList = new List<SelectListItem>();
                    m.InterestedInList.Add(new SelectListItem { Text = "Yearly Membership", Value = "Yearly Membership" });
                    m.InterestedInList.Add(new SelectListItem { Text = "10 Years Non-refundable Membership", Value = "10 Years Non-refundable Membership" });
                    m.InterestedInList.Add(new SelectListItem { Text = "25 Years Refundable Membership", Value = "25 Years Refundable Membership" });
                    m.InterestedInList.Add(new SelectListItem { Text = "Corporate Membership", Value = "Corporate Membership" });

                    m.Gender = new List<SelectListItem>();
                    m.Gender.Add(new SelectListItem { Text = "Male", Value = "Male" });
                    m.Gender.Add(new SelectListItem { Text = "Female", Value = "Female" });
                    return this.View(m);
                }

                ClubGurgaonEnquiryDataContext rdb = new ClubGurgaonEnquiryDataContext();
                ClubGurgaonEnquiry r = new ClubGurgaonEnquiry();

                r.Name = m.FirstName;
                r.Last_Name = m.LastName;
                r.Mobile = m.Mobile;
                r.Email = m.Email;
                r.City = m.City;
                r.Gender = m.SelectedGender;
                r.Lead_Source = m.SelectedLeadSource;
                r.Lead_Status = m.SelectedLeadStatus;
                r.Lead_Owner = m.Lead_Owner;
                r.Remarks = m.Remarks;
                r.FormSubmitOn = DateTime.Now;
                r.FormSubmitBy = UserSession.UserSessionContext.userId;
                r.FormType = "Lead Creation page - By Admin";
                r.InterestedIn = m.InterestedIn;
                r.MemberShipDetails = m.Membership;
                r.Profession = m.Profession;
                r.Age = m.Age;


                rdb.ClubGurgaonEnquiries.InsertOnSubmit(r);

                rdb.SubmitChanges();
                Log.Info("Lead created successfully:" + DateTime.Now, this);
                var item = Context.Database.GetItem(Templates.LeadGeneration.ThankYou);
                var url = item.Url();
                return this.Redirect(url);
            }
            catch (Exception ex)
            {
                Log.Error("Error at LeadGeneration POST:" + ex.Message, this);
                var item = Context.Database.GetItem(Templates.LeadGeneration.LogOut);
                return this.Redirect(item.Url());
            }

        }

    }
}