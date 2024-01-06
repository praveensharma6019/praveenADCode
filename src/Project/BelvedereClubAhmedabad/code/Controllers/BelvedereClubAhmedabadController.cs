using Sitecore.BelvedereClubAhmedabad.Website.Models;
using Sitecore.BelvedereClubAhmedabad.Website.Services;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.BelvedereClubAhmedabad.Website.Controllers
{
    public class BelvedereClubAhmedabadController : Controller
    {
        ClubAhmedabadRepository clubahmdRepo = new ClubAhmedabadRepository();
        // GET: BelvedereClubAhmedabad
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RoomAvailability()
        {
            return View("~/Views/BelvedereClubAhmedabad/Sublayouts/ClubAhmedabadHomePageForm.cshtml");
        }
        [HttpPost]
        public ActionResult RoomAvailability(AvailabilityFormModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            try
            {

                #region Delete Available otp from database for given mobile number

                clubahmdRepo.DeleteOldOtp(model.Mobile);
                #endregion

                #region Generate New Otp for given mobile number and save to database
                string generatedotp = clubahmdRepo.StoreGeneratedOtp(model);
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
        public ActionResult VerifyOTP(AvailabilityFormModel model)
        {
            var result = new { status = "0" };
            #region Verify OTP
            string generatedOTP = clubahmdRepo.GetOTP(model.Mobile);
            if (string.Equals(generatedOTP, model.OTP))
            {
                result = new { status = "1" };
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Insertcontactdetail(AvailabilityFormModel m)
        {
            Log.Error("Insertcontactdetail ","Start");
            var result = new { status = "1" };
            try
            {
                ClubAhmdRoomAvailabilityDataContext rdb = new ClubAhmdRoomAvailabilityDataContext();
                ClubAhmedabadRoomAvailability r = new ClubAhmedabadRoomAvailability();

                r.Name = m.Name;
                r.BookDateFrom = m.BookDateFrom;
                r.BookDateTo = m.BookDateTo;
                r.NoOfRoom = m.NoOfRoom;
                r.NoOfAdults = m.NoOfAdults;
                r.NoOfKids = m.NoOfKids;
                r.Email = m.Email;
                r.MobileNo = m.Mobile;
                r.FormSubmitOn = m.FormSubmitOn;

                #region Insert to DB
                rdb.ClubAhmedabadRoomAvailabilities.InsertOnSubmit(r);
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
                message = "Hello<br><br>Below are the Booking details:<br><br>Name: " + m.Name + "<br>Booking Date (From): " + m.BookDateFrom.ToShortDateString() + "<br>Booking Date (Till): " + m.BookDateTo.ToShortDateString();
                message = message + "<br> No. of Room Booked: " + m.NoOfRoom + "<br> Adults: " + m.NoOfAdults + "<br>Kids: " + m.NoOfKids;
                message = message + "<br>Email: "+m.Email+"<br>Contact Number: "+m.Mobile + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/RoomBook/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/RoomBook/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/RoomBook/EmailSubject", "");
                bool results = sendEmail(to, emailSubject, message, from);
                if (results)
                {
                    Log.Error("Email Sent- ","");
                }
            }
            catch (Exception ex)
            {
                result = new { status = "1" };
                Log.Error("Failed to sent Email",ex.ToString());
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
                Log.Error(ex.InnerException.ToString(),"sendEmail - ");
                return status;
            }
        }

        [HttpPost]
        public ActionResult InsertBrochuredetail(ClubAhmedabadEnquiryModel m)
        {
            Log.Error("Insertcontactdetail ", "Start");
            var result = new { status = "1" };
            try
            {
                ClubAhmedabadEnquiryDataContext rdb = new ClubAhmedabadEnquiryDataContext();
                ClubAhmedabadEnquiry r = new ClubAhmedabadEnquiry();

                r.Name = m.Name;
                r.Email = m.Email;
                r.City = m.City;
                r.Mobile = m.Mobile;
                r.FormType = m.FormType;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.ClubAhmedabadEnquiries.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Log.Error("Failed to sent Email", ex.ToString());

            }
            try
            { 
            string message = "";
                message = "Hello<br><br>Below are the details of customer Downloading the Brochure:<br><br>Name: " + m.Name;
                message = message + "<br>Email: " + m.Email + "<br>City: " + m.City + "<br>Contact No: "+m.Mobile+ "<br><br>Thanks";
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
                Log.Error("Failed to sent Email", ex.ToString());
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public ActionResult InsertEnquiryNowdetail(ClubAhmedabadEnquiryModel m)
        {
            Log.Error("Insertcontactdetail ", "Start");
            var result = new { status = "1" };
            try
            {
                ClubAhmedabadEnquiryDataContext rdb = new ClubAhmedabadEnquiryDataContext();
                ClubAhmedabadEnquiry r = new ClubAhmedabadEnquiry();

                r.Name = m.Name;
                r.Email = m.Email;
                r.InterestedIn = m.InterestedIn;
                r.City = m.City;
                r.Mobile = m.Mobile;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.ClubAhmedabadEnquiries.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try { 
            string message = "";
                message = "Hello<br><br>Below are the Enquiry-Now details of the Customer:<br><br>Name: " + m.Name;
                message = message + "<br>Email: " + m.Email + "<br>Interested In: "+m.InterestedIn+"<br>City: " + m.City + "<br>Contact No: " + m.Mobile + "<br>Message: "+m.Message+"<br><br>Thanks";
                string to= DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailTo", "");
                string from= DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailFrom", "");
                string emailSubject= DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailSubject", "");
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

        public ActionResult InsertMembershipEnquiryNowdetail(ClubAhmedabadEnquiryModel m)
        {
            Log.Error("Insertcontactdetail ", "Start");
            var result = new { status = "1" };
            try
            {
                ClubAhmedabadEnquiryDataContext rdb = new ClubAhmedabadEnquiryDataContext();
                ClubAhmedabadEnquiry r = new ClubAhmedabadEnquiry();

                r.Name = m.Name;
                r.Email = m.Email;
                r.City = m.City;
                r.Mobile = m.Mobile;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.ClubAhmedabadEnquiries.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try { 
            string message = "";
                message = "Hello<br><br>Below are the Enquiry-Now details of the Customer:<br><br>Name: " + m.Name;
                message = message + "<br>Email: " + m.Email + "<br>City: " + m.City + "<br>Contact No: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/MembershipEnquiryNow/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/MembershipEnquiryNow/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/MembershipEnquiryNow/EmailSubject", "");
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

    }
  
}