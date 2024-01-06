using InstamojoAPI;
using InstaMojoIntegration.Models;
using RestSharp;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Marathon.Website.Models;
using Sitecore.Marathon.Website.Services;
using Sitecore.Data.Items;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Sitecore.Marathon.Website;
using static Sitecore.Marathon.Website.CustomHelpers;
using Sitecore.Data;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Data;
using Sitecore.Project.Marathon;
using Sitecore.Marathon.Website.Validation;

namespace Sitecore.Marathon.Website.Controllers
{
    public class MarathonController : Controller
    {
        MarathonRepository repo = new MarathonRepository();
        // GET: Marathon
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckAffiliateCode(Donate m)
        {
            var result = new { status = "0" };

            try
            {
                Log.Error("CheckAffiliateCode ", "Start");

                if (!string.IsNullOrEmpty(m.AffiliateCode))
                {
                    try
                    {
                        using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                        {
                            if (objcontext.AhmedabadMarathonRegistrations.Where(x => x.Id.ToString() == m.AffiliateCode.ToString()).Any())
                            {
                                result = new { status = "1" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("CheckAffiliateCode ", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("CheckAffiliateCode ", ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult CheckCouponCode(RegistrationModel m)
        {
            var result = new { status = "0", extra = "", couponTitle = "", EmailDomain = "", EmployeeIDSource = "" };

            try
            {
                Log.Error("CheckCouponCode ", "Start");
                var extrafieldText = "";
                var coupon = "";
                var emailValidation = "";
                var employeeIDCollection = "";
                DateTime currentdate = DateTime.Parse(DateTime.Now.ToShortDateString()).Date;
                var item = Sitecore.Context.Database.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
                if (!string.IsNullOrEmpty(m.ReferenceCode))
                {
                    var filterdata = item.Children.Where(x => x.Fields["Coupon Title"].Value.ToLower() == (m.ReferenceCode.ToLower()));

                    if (filterdata.Count() > 0)
                    {

                        foreach (var element in filterdata.ToList())
                        {
                            //date validation

                            var startdate = (DateField)element.Fields["Start Date"];

                            var isoStartDate = Sitecore.DateUtil.IsoDateToServerTimeIsoDate(startdate.Value);

                            var serverTime = Sitecore.DateUtil.ToServerTime(startdate.DateTime);
                            var _xxx = Sitecore.DateUtil.FormatShortDateTime(serverTime);
                            var startdateformat = DateTimeOffset.ParseExact(startdate.Value, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
                            var enddate = (DateField)element.Fields["End Date"];
                            var enddateformat = DateTimeOffset.ParseExact(enddate.Value, "yyyyMMdd'T'HHmmss'Z'", CultureInfo.InvariantCulture);
                            var isoEndDate = Sitecore.DateUtil.IsoDateToServerTimeIsoDate(enddate.Value);
                            emailValidation = element.Fields["Email Domain"].ToString();

                            var empID = element.Fields["Employee ID Source"].Value;
                            if (empID != null)
                            {
                                var CouponEmpSor = Sitecore.Context.Database.GetItem(empID);
                                if (CouponEmpSor != null)
                                {
                                    employeeIDCollection = CouponEmpSor.Fields["Body"].Value.ToString();
                                }
                            }

                            if (startdate.DateTime.AddHours(5.5).Date <= currentdate.Date && enddate.DateTime.AddHours(5.5).Date >= currentdate.Date)
                            {
                                AhmedabadMarathonRegistrationDataContext adc = new AhmedabadMarathonRegistrationDataContext();
                                //var usagecount = (from rc in adc.AhmedabadMarathonRegistrations
                                //                  where ((rc.FormSubmitOn.Value.Date >= startdateformat.Date) && (rc.FormSubmitOn.Value.Date <= enddateformat.Date))
                                //                  select rc).Count();

                                var usagecount = (from rc in adc.AhmedabadMarathonRegistrations
                                                  where (rc.ReferenceCode.ToLower() == element.Fields["Coupon Title"].Value.ToLower())
                                                  select rc).Count();

                                var maxusage = element.Fields["Maximum Usage"].Value;

                                if (usagecount >= int.Parse(maxusage))
                                {
                                    result = new { status = "0", extra = extrafieldText, couponTitle = coupon, EmailDomain = emailValidation, EmployeeIDSource = employeeIDCollection };
                                    return Json(result, JsonRequestBehavior.AllowGet);
                                }

                                else
                                {
                                    var discount = element.Fields["Enter Discount Rate in Percentage"].Value;
                                    if (string.IsNullOrEmpty(discount) && (decimal.Parse(discount) <= 0 && (decimal.Parse(discount) > 100)))
                                    {
                                        result = new { status = "0", extra = extrafieldText, couponTitle = coupon, EmailDomain = emailValidation, EmployeeIDSource = employeeIDCollection };
                                        return Json(result, JsonRequestBehavior.AllowGet);
                                    }

                                    //end date validation
                                    var extrafieldId = element.Fields["Select Extra Fields"];
                                    coupon = element.Fields["Coupon Title"].Value;
                                    string[] IDarr = extrafieldId.ToString().Split('|');



                                    foreach (string id in IDarr)
                                    {
                                        if (id != null && id != "")
                                        {
                                            var items = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
                                            if (extrafieldText == "")
                                            {
                                                extrafieldText = items.Fields["Text"].Value;
                                            }
                                            else
                                            {
                                                extrafieldText = extrafieldText + "$" + items.Fields["Text"].Value;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                result = new { status = "0", extra = extrafieldText, couponTitle = coupon, EmailDomain = emailValidation, EmployeeIDSource = employeeIDCollection };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }

                        }
                        result = new { status = "1", extra = extrafieldText, couponTitle = coupon, EmailDomain = emailValidation, EmployeeIDSource = employeeIDCollection };
                    }
                    else
                    { result = new { status = "0", extra = extrafieldText, couponTitle = coupon, EmailDomain = emailValidation, EmployeeIDSource = employeeIDCollection }; }
                }
            }
            catch (Exception ex)
            {
                Log.Error("CheckCouponCode ", ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ValidateEmployeeID(RegistrationModel m)
        {
            var result = new { status = "0" };

            try
            {
                Log.Error("ValidateEmployeeID ", "Start");

                if (!string.IsNullOrEmpty(m.EmployeeID))
                {
                    try
                    {
                        using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                        {
                            if (objcontext.AhmedabadMarathonRegistrations.Where(x => x.EmployeeID.ToString() == m.EmployeeID.ToString() && x.ReferenceCode.ToString() == m.ReferenceCode.ToString()).Any())
                            {
                                result = new { status = "1" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("ValidateEmployeeID ", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("ValidateEmployeeID ", ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult CheckCouponCode(RegistrationModel m)
        //{
        //    Log.Error("CheckCouponCode ", "Start");
        //    var result = new { status = "1",extra="" };
        //    var extrafieldText = "";
        //    DateTime currentdate = DateTime.Parse(DateTime.Now.ToShortDateString());
        //    var item = Sitecore.Context.Database.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
        //    var filterdata = item.Children.Where(x => x.Fields["Coupon Title"].Value.ToLower()==(m.ReferenceCode.ToLower()) );
        //    if (filterdata.Count() > 0)
        //    {
        //        foreach (var element in filterdata.ToList())
        //        {
        //            var extrafieldId = element.Fields["Select Extra Fields"];
        //            string[] IDarr = extrafieldId.ToString().Split('|');



        //            foreach (string id in IDarr)
        //            {
        //                if (id != null && id != "")
        //                {
        //                    var items = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
        //                    if (extrafieldText == "")
        //                    {
        //                        extrafieldText = items.Fields["Text"].Value;
        //                    }
        //                    else
        //                    {
        //                        extrafieldText = extrafieldText + "$" + items.Fields["Text"].Value;
        //                    }
        //                }
        //            }

        //        }
        //        result = new { status = "1", extra=extrafieldText };
        //    }
        //    else
        //    { result = new { status = "0", extra = extrafieldText }; }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult MarathonRegisteration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarathonRegisteration(RegistrationModel m)
        {
            try
            {
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                AhmedabadMarathonRegistration r = new AhmedabadMarathonRegistration();
                //conversion from dd/mm/yyyy to mm/dd/yyyy
                var BIB_Number = repo.GenerateRandomBIBNumber(m.RaceDistance);
                DateTime getDateofBirth;
                getDateofBirth = DateTime.Parse(m.DateofBirth.ToString(), CultureInfo.GetCultureInfo("en-gb"));
                r.BIBNumber = BIB_Number;
                r.IsRaceDistanceChanged = false;
                r.RaceDistance = m.RaceDistance;
                r.RaceAmount = m.RaceAmount;
                r.ReferenceCode = m.ReferenceCode;
                r.FirstName = m.FirstName.ToUpper();
                r.LastName = m.LastName.ToUpper();
                r.DateofBirth = getDateofBirth;
                r.Email = m.Email;
                r.ContactNumber = m.ContactNumber;
                r.Nationality = m.Nationality;
                r.Gender = m.Gender;
                r.TShirtSize = m.TShirtSize;
                r.NamePreferredonBIB = m.NamePreferredonBIB;
                r.IdentityProofType = m.IdentityProofType;
                r.IdentityProofNumber = m.IdentityProofNumber;
                // r.IDCardAttachment = m.IDCardAttachment;
                r.Country = m.Country;
                r.State = m.State;
                r.City = m.City;
                r.Address = m.Address;
                r.Pincode = m.Pincode;
                r.EmergencyContactName = m.EmergencyContactName;
                r.EmergencyContactRelationship = m.EmergencyContactRelationship;
                r.EmergencyContactNumber = m.EmergencyContactNumber;
                r.BloodGroup = m.BloodGroup;
                r.ChronicIllness = m.ChronicIllness;
                r.HeartAilment = m.HeartAilment;
                r.AnyFaintingEpisodesinPast = m.AnyFaintingEpisodesinPast;
                r.AnyOtherAilment = m.AnyOtherAilment;
                r.AnyKnownAllergies = m.AnyKnownAllergies;
                r.PayrollCompany = m.PayrollCompany;
                r.EmployeeID = m.EmployeeID;
                r.UnitStation = m.UnitStation;
                //  r.ShantigramIdProof = m.ShantigramIdProof;
                r.NamePreferredonBIB = m.FirstName.ToUpper() + " " + m.LastName.ToUpper();
                r.DefencePersonnel = m.DefencePersonnel;
                //r.RaceCertificate = m.RaceCertificate;
                r.DetailsOfFullHalfMarathon = m.DetailsOfFullHalfMarathon;
                r.FinalAmount = m.RaceAmount;
                r.PaymentStatus = "pending";
                m.FinalAmount = m.RaceAmount;
                r.RegistrationStatus = "pending";
                r.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                m.Userid = Guid.NewGuid();
                r.UserId = m.Userid.ToString();

                DateTime currentDate = DateTime.Parse(DateTime.Now.ToString());

                TimeSpan timespan = currentDate.Subtract(getDateofBirth);
                r.Age = ((int)(timespan.TotalDays) / 365).ToString();
                m.Age = r.Age;

                if (m.IDCardAttachment != null)
                {
                    HttpPostedFileBase idCard = m.IDCardAttachment;
                    string fileName = idCard.FileName;
                    string path = Server.MapPath("/AhmedabadMarathonRegistration/UploadIDs/" + m.Userid + "-" + fileName);
                    idCard.SaveAs(path);
                    r.IDCardAttachment = "/AhmedabadMarathonRegistration/UploadIDs/" + m.Userid + "-" + fileName;
                }
                if (m.ShantigramIdProof != null)
                {
                    HttpPostedFileBase shantiId = m.ShantigramIdProof;
                    string fileName = shantiId.FileName;
                    string path = Server.MapPath("/AhmedabadMarathonRegistration/ShantigramIDs/" + m.Userid + "-" + fileName);
                    shantiId.SaveAs(path);
                    r.ShantigramIdProof = "/AhmedabadMarathonRegistration/ShantigramIDs/" + m.Userid + "-" + fileName;
                }
                if (m.RaceCertificate != null)
                {
                    HttpPostedFileBase shantiId = m.RaceCertificate;
                    string fileName = shantiId.FileName;
                    string path = Server.MapPath("/AhmedabadMarathonRegistration/UploadIDs/" + m.Userid + "-" + fileName);
                    shantiId.SaveAs(path);
                    r.RaceCertificate = "/AhmedabadMarathonRegistration/UploadIDs/" + m.Userid + "-" + fileName;
                }
                if (m.Rstat == "yes")
                {
                    var item = Sitecore.Context.Database.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
                    var filterdata = item.Children.Where(x => x.Fields["Coupon Title"].Value.ToLower() == (m.ReferenceCode.ToLower()));
                    if (filterdata.Count() > 0)
                    {
                        foreach (var element in filterdata.ToList())
                        {

                            var discount = element.Fields["Enter Discount Rate in Percentage"].Value;
                            if (!string.IsNullOrEmpty(discount) && (decimal.Parse(discount) >= 0 && (decimal.Parse(discount) <= 100)))
                            {
                                var pay_status = element.Fields["Pay remaining balance or not"].Value;
                                if (!string.IsNullOrEmpty(pay_status))
                                {
                                    if (pay_status.ToLower() == "yes")
                                    {
                                        var finalamt = m.RaceAmount - ((m.RaceAmount * decimal.Parse(discount)) / 100);
                                        r.PaymentStatus = "pending";
                                        r.DiscountRate = discount;
                                        r.FinalAmount = finalamt;
                                        m.FinalAmount = finalamt;
                                    }
                                    else if (pay_status.ToLower() == "complimentary")
                                    {
                                        r.PaymentStatus = "complementary";
                                        r.RegistrationStatus = "successful";
                                        r.AmountReceived = 0;
                                        var finalamt = m.RaceAmount - ((m.RaceAmount * decimal.Parse(discount)) / 100);
                                        r.DiscountRate = discount;
                                        r.FinalAmount = finalamt;
                                        m.FinalAmount = finalamt;
                                    }
                                    else
                                    {
                                        r.PaymentStatus = "no";
                                        r.RegistrationStatus = "successful";
                                        r.AmountReceived = 0;
                                        var finalamt = m.RaceAmount - ((m.RaceAmount * decimal.Parse(discount)) / 100);
                                        r.DiscountRate = discount;
                                        r.FinalAmount = finalamt;
                                        m.FinalAmount = finalamt;

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    r.ReferenceCode = "";
                }

                TempData["USerId"] = r.UserId.ToString();
                Session["uid"] = r.UserId.ToString();
                TempData.Keep();
                rdb.AhmedabadMarathonRegistrations.InsertOnSubmit(r);
                rdb.SubmitChanges();
                //sendEmail(m);
                // sendSMS(m, "Than you for your Registration in Ahmedabad Marathon ");

                if (r.PaymentStatus.ToLower() == "pending")
                {
                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                    {
                        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                        {
                            UserId = (m.Userid).ToString(),
                            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                            // msg = "ProjectId:" + model.projectid + "Project Name:" + model.ProjectName + ", BuildingId:" + model.buildingid + " FloorId:," + model.floorid + " UnitId," + model.unitid,
                            Id = Guid.NewGuid(),
                            Amount = System.Convert.ToString(m.FinalAmount),
                            Email = m.Email,
                            Mobile = m.ContactNumber,
                            UserType = "Guest",
                            GatewayType = "Insta-Mojo",
                            Created_Date = System.DateTime.Now,
                            RequestTime = System.DateTime.Now,
                            CreatedBy = m.FirstName + " " + m.LastName,
                            AccountNumber = m.ReferenceCode,
                            PaymentType = "Token Amount",
                            ResponseMsg = Request.Url.ToString()
                        };

                        rdb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                        rdb.SubmitChanges();
                        //   sendEmail(m, "AhmedabadMarathonPaymentHistories - pending");
                        PaymentService objPaymentService = new PaymentService();
                        ResultPayment Objresult = new ResultPayment();
                        Objresult = objPaymentService.Payment(m);
                        if (Objresult.IsSuccess)
                        {
                            //  sendEmail(m, "payment sucess1.->" + Objresult.Message);
                            return Redirect(Objresult.Message);

                        }
                        else
                        {
                            ViewBag.ErrorMessage = Objresult.Message;
                            return Redirect("/registration-failed");

                            //   sendEmail(m, Objresult.Message);
                        }
                        //   sendEmail(m, "final..return");
                        return View(m);
                    }
                }
                else
                {
                    m.Useridstring = m.Userid.ToString();
                    sendEmail(m);
                    sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");
                    return Redirect("/registration-thankyou");
                }
            }
            catch (Exception ex)
            {

                //  sendEmail(m, ex.Message);
                Console.WriteLine(ex);
            }

            return Redirect("MarathonRegisteration");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrationCheck(RegistrationCheck r, string btnSendOTP, string register, string validate)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Log.Info("Marathon RegistrationCheck Model validation pass" + r, this);
                    if (!Captcha.IsReCaptchValidV3(r.reResponse))
                    {
                        Log.Error("Marathon RegistrationCheck Catpcha validation failed", "");
                        Session["Regerrmsg"] = "Invalid Captcha";
                        Session["RegInfo"] = r;
                        return Redirect("/Register");
                    }
                    Session["OTPmsg"] = "";
                    Session["Regerrmsg"] = "";

                    if (!string.IsNullOrEmpty(r.RunMode))
                    {
                        Log.Info("Marathon RegistrationCheck RunMode check pass" + r.RunMode+" contactNumber"+r.PhoneNumber, this);
                        var Physical = Sitecore.Context.Database.GetItem("{6B0DCBFE-D405-424B-9A93-4AD52224109B}");
                        var Remote = Sitecore.Context.Database.GetItem("{1675BF07-AFA0-4325-A242-D58A01534E54}");
                        if (r.RunMode == "Physical")
                        {
                            var agecheck = (DateField)Physical.Fields["AgeCheckDate"];
                            DateTime PhysicalDate = Sitecore.DateUtil.ToServerTime(agecheck.DateTime);
                        }
                        else if (r.RunMode == "Remote")
                        {
                            var agecheck = (DateField)Remote.Fields["AgeCheckDate"];
                            var Phyagecheck = (DateField)Physical.Fields["AgeCheckDate"];
                            DateTime PhysicalDate = Sitecore.DateUtil.ToServerTime(Phyagecheck.DateTime);
                            DateTime RemoteDate = Sitecore.DateUtil.ToServerTime(agecheck.DateTime);
                        }
                    }

                    if ((string.IsNullOrEmpty(r.OTP) && !string.IsNullOrEmpty(validate)) || (!string.IsNullOrEmpty(btnSendOTP) && btnSendOTP == "Resend OTP"))
                    {
                        Log.Info("Marathon RegistrationCheck for ContactNumber" + r.PhoneNumber , this);
                        if (repo.ParticipantStatus(r.PhoneNumber) == string.Empty)
                        {
                            var currentSession = Guid.NewGuid().ToString();
                            var status = SendSMSOTP(r.PhoneNumber, currentSession);
                            if (status == "0")
                            {
                                Log.Error("Marathon RegistrationCheck ParticipantStatus is string.Empty  | OTP sent failed for ContactNumber" + r.PhoneNumber, this);
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/OTPNotSent", "OTP sent Failed.");
                                Session["RegInfo"] = r;
                                return Redirect("/Register");
                            }
                            else if (status == "4")
                            {
                                Log.Error("Marathon RegistrationCheck ParticipantStatus is string.Empty | exceeded OTP limit. Please try after 30 min for ContactNumber" + r.PhoneNumber, this);
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/OTPExceeded", "You have exceeded OTP limit. Please try after 30 min");
                                Session["RegInfo"] = r;
                                return Redirect("/Register");
                            }
                            else if (status == "3")
                            {
                                Log.Error("Marathon RegistrationCheck ParticipantStatus is string.Empty | Invalid Session! for ContactNumber" + r.PhoneNumber, this);
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/Invalid Session", "Invalid Session!");
                                Session["RegInfo"] = r;
                                return Redirect("/Register");
                            }
                            else
                            {
                                Log.Info("Marathon RegistrationCheck ParticipantStatus is string.Empty | OTP has been sent! for ContactNumber" + r.PhoneNumber, this);
                                Session["OTPmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/OTPSent", "OTP has been sent to: " + r.PhoneNumber);
                                Session["RegInfo"] = r;
                                Session["currentSession"] = currentSession;
                                return Redirect("/Register");
                            }
                        }
                        else if (repo.ParticipantStatus(r.PhoneNumber) == "old")
                        {
                            var currentSession = Guid.NewGuid().ToString();
                            var status = SendSMSOTP(r.PhoneNumber, currentSession);
                            if (status == "0")
                            {
                                Log.Error("Marathon RegistrationCheck ParticipantStatus is old | OTP sent Failed for ContactNumber" + r.PhoneNumber, this);
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/OTPNotSent", "OTP sent Failed");
                                Session["RegInfo"] = r;
                                return Redirect("/Register");
                            }
                            else if (status == "4")
                            {
                                Log.Error("Marathon RegistrationCheck ParticipantStatus is old | exceeded OTP limit. Please try after 30 min for ContactNumber" + r.PhoneNumber, this);
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/OTPExceeded", "You have exceeded OTP limit. Please try after 30 min");
                                Session["RegInfo"] = r;
                                return Redirect("/Register");
                            }
                            else if (status == "3")
                            {
                                Log.Error("Marathon RegistrationCheck ParticipantStatus is old | Invalid Session for ContactNumber" + r.PhoneNumber, this);
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/Invalid Session", "Invalid Session!");
                                Session["RegInfo"] = r;
                                return Redirect("/Register");
                            }
                            else
                            {
                                Log.Info("Marathon RegistrationCheck ParticipantStatus is old | OTP has been sent for ContactNumber" + r.PhoneNumber, this);
                                Session["OTPmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/OTPSent", "OTP has been sent to: " + r.PhoneNumber);
                                Session["RegInfo"] = r;
                                Session["currentSession"] = currentSession;
                                return Redirect("/Register");
                            }
                        }
                        else if (repo.ParticipantStatus(r.PhoneNumber) == "newPending")
                        {
                            Log.Info("Marathon RegistrationCheck ParticipantStatus is newPending | This Mobile Number is already registered. Kindly login to proceed for payment. for ContactNumber" + r.PhoneNumber, this);
                            Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/ErrorMsg/AlreadyExist", "This Mobile Number is already registered. Kindly login to proceed for payment.");
                            Session["RegInfo"] = r;
                            return Redirect("/Register");
                        }
                        else if (repo.ParticipantStatus(r.PhoneNumber) == "newRegistered")
                        {
                            Log.Info("Marathon RegistrationCheck ParticipantStatus is newRegistered | This Mobile Number is already registered. Kindly login to update details. for ContactNumber" + r.PhoneNumber, this);
                            Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/ErrorMsg/AlreadyExist", "This Mobile Number is already registered. Kindly login to update details.");
                            Session["RegInfo"] = r;
                            return Redirect("/Register");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(register))
                        {
                            if (!string.IsNullOrEmpty(r.OTP))
                            {
                                #region Verify OTP
                                if (Session["currentSession"] == null)
                                {
                                    Log.Error("Marathon RegistrationCheck Invalid Session! for PhoneNumber"+ r.PhoneNumber, "");
                                    Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/SessionError", "Invalid Session!");
                                    Session["RegInfo"] = r;
                                    return Redirect("/Register");
                                }
                                var CurrentSession = Session["currentSession"].ToString();
                                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                                string generatedOTP = repo.GetEmailOTP(r.PhoneNumber, CurrentSession);
                                if (generatedOTP == "InvalidAttempt")
                                {
                                    Log.Error("Marathon RegistrationCheck You have exceeded invalid attempts. Please send a new OTP for PhoneNumber"+r.PhoneNumber, "");
                                    Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/ExceededInvalidAttempts", "You have exceeded invalid attempts. Please send a new OTP");
                                    Session["RegInfo"] = r;
                                    return Redirect("/Register");
                                }
                                if (string.Equals(generatedOTP, r.OTP)) //true) 
                                {
                                    Log.Info("Marathon RegistrationCheck OTP verfied for PhoneNumber" + r.PhoneNumber, "");
                                    Session["BibCount"] = r.Count;
                                    if (repo.ParticipantStatus(r.PhoneNumber) == string.Empty)
                                    {
                                        Session["message"] = r;
                                        return Redirect("/MarathonRegisterationInfo");
                                    }
                                    else if (repo.ParticipantStatus(r.PhoneNumber) == "old")
                                    {
                                        Session["message"] = r;
                                        return Redirect("/MarathonRegisterationInfo");
                                    }
                                    else if (repo.ParticipantStatus(r.PhoneNumber) == "new")
                                    {
                                        Log.Info("Marathon RegistrationCheck | This Email is already registered. Kindly login to update details. for PhoneNumber" + r.PhoneNumber, "");
                                        Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/EmailExist", "This Email is already registered. Kindly login to update details.");
                                        Session["RegInfo"] = r;
                                        return Redirect("/Register");
                                    }
                                }
                                else
                                {
                                    Log.Info("Marathon RegistrationCheck Invalid OTP for PhoneNumber" + r.PhoneNumber, "");
                                    Session["OTPmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/InvalidOTp", "Invalid OTP");
                                    Session["RegInfo"] = r;
                                    return Redirect("/Register");
                                }
                                #endregion
                            }
                            else
                            {
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/EnterOTP", "Enter OTP sent to your mail id.");
                                Session["RegInfo"] = r;
                                return Redirect("/Register");
                            }
                        }
                    }
                    Log.Error("Marathon RegistrationCheck | There is some error, please try again later" + r.PhoneNumber, this);
                    Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/RegistrationCheckError", "There is some error, please try again later.");
                    Session["RegInfo"] = r;
                    return Redirect("/Register");
                }
                else
                {
                    Log.Error("Marathon RegistrationCheck model validation failed" + ModelState, this);
                    Session["RegInfo"] = r;
                    return Redirect("/Register");
                }
            }
            catch (Exception ex)
            {
                Log.Error("RegistrationCheck exception occured", ex.Message);
                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/RegistrationCheckError", "There is some error, please try again later.");
                Session["RegInfo"] = r;
                return Redirect("/Register");
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return Redirect("/Register");
        }

        [HttpPost]
        public ActionResult InsertRegistrationdetail(RegistrationModel m)
        {
            Log.Error("AhmedabadMarathonRegistration ", "Start");
            var result = new { status = "1" };
            try
            {
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                AhmedabadMarathonRegistration r = new AhmedabadMarathonRegistration();
                var BIB_Number = repo.GenerateRandomBIBNumber(m.RaceDistance);
                r.BIBNumber = BIB_Number;
                r.IsRaceDistanceChanged = false;
                r.ReferenceCode = m.ReferenceCode;
                r.FirstName = m.FirstName;
                r.LastName = m.LastName;
                //   r.DateofBirth = m.DateofBirth;
                r.Email = m.Email;
                r.ContactNumber = m.ContactNumber;
                r.Nationality = m.Nationality;
                r.Gender = m.Gender;
                r.TShirtSize = m.TShirtSize;
                r.NamePreferredonBIB = m.NamePreferredonBIB;
                r.IdentityProofType = m.IdentityProofType;
                r.IdentityProofNumber = m.IdentityProofNumber;
                // r.IDCardAttachment = m.IDCardAttachment;
                r.Country = m.Country;
                r.State = m.State;
                r.City = m.City;
                r.Address = m.Address;
                r.Pincode = m.Pincode;
                r.EmergencyContactName = m.EmergencyContactName;
                r.EmergencyContactRelationship = m.EmergencyContactRelationship;
                r.EmergencyContactNumber = m.EmergencyContactNumber;
                r.BloodGroup = m.BloodGroup;
                r.ChronicIllness = m.ChronicIllness;
                r.HeartAilment = m.HeartAilment;
                r.AnyFaintingEpisodesinPast = m.AnyFaintingEpisodesinPast;
                r.AnyOtherAilment = m.AnyOtherAilment;
                r.AnyKnownAllergies = m.AnyKnownAllergies;
                r.PayrollCompany = m.PayrollCompany;
                r.EmployeeID = m.EmployeeID;
                r.UnitStation = m.UnitStation;
                // r.ShantigramIdProof = m.ShantigramIdProof;
                // r.RaceDist = m.RaceDist;
                r.RaceAmount = m.RaceAmount;
                r.DiscountRate = m.DiscountRate;
                //  r.RemainingAmountStatus = m.RemainingAmountStatus;
                r.FormSubmitOn = m.FormSubmitOn;                // #region Insert to DB
                r.Updated = true;
                rdb.AhmedabadMarathonRegistrations.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertTrainingRegistration(RegistrationModel m)
        {
            var variable = new { status = "1" };
            try
            {


                AhmedabadMarathonRegistrationDataContext ahmedabadMarathonRegistrationDataContext = new AhmedabadMarathonRegistrationDataContext();

                if (ahmedabadMarathonRegistrationDataContext.AhmedabadMarathonTrainingRuns.Where(x => x.First_Name.ToString() == m.FirstName.ToString() &&
                 x.Last_Name.ToString() == m.LastName.ToString() &&
                 x.Contact_Number.ToString() == m.ContactNumber.ToString() &&
                 x.Email_Id.ToString() == m.Email.ToString()).Any())
                {
                    AhmedabadMarathonTrainingRun trainingRun = ahmedabadMarathonRegistrationDataContext.AhmedabadMarathonTrainingRuns.Where(x => x.First_Name.ToString() == m.FirstName.ToString() &&
                    x.Last_Name.ToString() == m.LastName.ToString() &&
                    x.Contact_Number.ToString() == m.ContactNumber.ToString() &&
                    x.Email_Id.ToString() == m.Email.ToString()).FirstOrDefault();

                    trainingRun.TrackingOption = m.Rstat;
                    trainingRun.FormSubmitOn = new DateTime?(m.FormSubmitOn);

                    ahmedabadMarathonRegistrationDataContext.SubmitChanges();


                    base.Session["runStatus"] = "true";
                }
                else
                {
                    AhmedabadMarathonTrainingRun ahmedabadMarathonTrainingRun = new AhmedabadMarathonTrainingRun()
                    {
                        Race_Category = m.RaceDistance,
                        TrackingOption = m.Rstat,
                        First_Name = m.FirstName,
                        Last_Name = m.LastName,
                        Contact_Number = m.ContactNumber,
                        Email_Id = m.Email,
                        FormSubmitOn = new DateTime?(m.FormSubmitOn)
                    };
                    ahmedabadMarathonRegistrationDataContext.AhmedabadMarathonTrainingRuns.InsertOnSubmit(ahmedabadMarathonTrainingRun);
                    ahmedabadMarathonRegistrationDataContext.SubmitChanges();
                    base.Session["runStatus"] = "true";
                }
                //   sendTrainingEmail(m);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                variable = new { status = "0" };
                Console.WriteLine(exception);
                Log.Error(string.Concat("Exception while inserting training registration details ", exception.Message), this);
            }
            // return base.Json(variable, JsonRequestBehavior.AllowGet);
            return Json(variable, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult InsertTrainingOption(RegistrationModel m)
        {
            var variable = new { status = "1" };
            try
            {


                AhmedabadMarathonRegistrationDataContext ahmedabadMarathonRegistrationDataContext = new AhmedabadMarathonRegistrationDataContext();

                if (ahmedabadMarathonRegistrationDataContext.AhmedabadMarathonTrainingOptions.Where(x => x.First_Name.ToString() == m.FirstName.ToString() &&
                 x.Last_Name.ToString() == m.LastName.ToString() && x.RegistrationId.ToString() == m.AffiliateCode.ToString() &&
                 x.Contact_Number.ToString() == m.ContactNumber.ToString() &&
                 x.Email_Id.ToString() == m.Email.ToString()).Any())
                {
                    AhmedabadMarathonTrainingOption trainingRun = ahmedabadMarathonRegistrationDataContext.AhmedabadMarathonTrainingOptions.Where(x => x.First_Name.ToString() == m.FirstName.ToString() &&
                    x.Last_Name.ToString() == m.LastName.ToString() && x.RegistrationId.ToString() == m.AffiliateCode.ToString() &&
                    x.Contact_Number.ToString() == m.ContactNumber.ToString() &&
                    x.Email_Id.ToString() == m.Email.ToString()).FirstOrDefault();

                    trainingRun.TrackingOption = m.Rstat;
                    trainingRun.FormSubmitOn = new DateTime?(m.FormSubmitOn);

                    ahmedabadMarathonRegistrationDataContext.SubmitChanges();


                    base.Session["runStatus"] = "true";
                }
                else
                {
                    AhmedabadMarathonTrainingOption ahmedabadMarathonTrainingRun = new AhmedabadMarathonTrainingOption()
                    {
                        Race_Category = m.RaceDistance,
                        TrackingOption = m.Rstat,
                        First_Name = m.FirstName,
                        Last_Name = m.LastName,
                        Contact_Number = m.ContactNumber,
                        Email_Id = m.Email,
                        RegistrationId = m.AffiliateCode,
                        FormSubmitOn = new DateTime?(m.FormSubmitOn)
                    };
                    ahmedabadMarathonRegistrationDataContext.AhmedabadMarathonTrainingOptions.InsertOnSubmit(ahmedabadMarathonTrainingRun);
                    ahmedabadMarathonRegistrationDataContext.SubmitChanges();
                    base.Session["runStatus"] = "true";
                }
                // sendTrainingEmail(m);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                variable = new { status = "0" };
                Console.WriteLine(exception);
                Log.Error(string.Concat("Exception while inserting training registration details ", exception.Message), this);
            }
            // return base.Json(variable, JsonRequestBehavior.AllowGet);
            return Json(variable, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public ActionResult CouponCodeCheck(RegistrationModel c)
        {
            var result = new { status = "1" };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Registration(RegistrationModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            try
            {

                #region Delete Available otp from database for given mobile number

                repo.DeleteOldOtp(model.ContactNumber);
                #endregion

                #region Generate New Otp for given mobile number and save to database
                string generatedotp = repo.StoreGeneratedOtp(model);
                #endregion

                #region Api call to send SMS of OTP
                try
                {
                    var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.ContactNumber, generatedotp);
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
        [ValidateAntiForgeryToken]
        public ActionResult CheckLoginCredential(MarathonLoginModel l, string login, string resendOTP)
        {
            try
            {
                Log.Info(" CheckLoginCredential Start", "");
                Session["errmsg"] = null;
                Session["LoginOTPmsg"] = null;
                Session["LoginData"] = null;
                Session["GroupCart"] = null;
                if (!Captcha.IsReCaptchValidV3(l.reResponse))
                {
                    Session["errmsg"] = "Invalid Captcha";
                    Session["LoginData"] = l;
                    return Redirect("/Register");
                }
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                var recordcheck = (from rc in rdb.AhmedabadMarathonRegistrations
                                   orderby rc.Id descending
                                   where (rc.ContactNumber.Equals(l.Contact))
                                   select rc);
                if (recordcheck.Count() > 0)
                {
                    IQueryable<AhmedabadMarathonRegistration> datarecord = null;
                    datarecord = (from rc in rdb.AhmedabadMarathonRegistrations
                                  orderby rc.Id
                                  where ((rc.ContactNumber == l.Contact) && (rc.Updated != null))
                                  select rc);
                    if (datarecord.Count() <= 0 || datarecord == null)
                    {
                        Log.Error(" CheckLoginCredential | This Email is not Registered, Kindly Register as a New User", "");
                        Session["errmsg"] = DictionaryPhraseRepository.Current.Get("/ErrorMsg/NotRegistered", "This Email is not Registered, Kindly Register as a New User");
                        Session["LoginData"] = l;
                        return Redirect("/Register");
                    }

                    if (datarecord.Count() > 0 && String.IsNullOrEmpty(l.OTP))
                    {
                        var status = SendSMSOTP(l.Contact, "currentSession");
                        if (status == "1")
                        {
                            Log.Info(" CheckLoginCredential | OTP has been sent to"+l.Contact, "");
                            Session["LoginOTPmsg"] = "OTP has been sent to :" + l.Contact;
                            Session["LoginData"] = l;
                            return Redirect("/Register");
                        }
                    }
                    if (datarecord.Count() > 0 && !String.IsNullOrEmpty(l.OTP))
                    {
                        string generatedOTP = repo.GetEmailOTP(l.Contact, "currentSession");
                        if (string.Equals(generatedOTP, l.OTP))
                        {
                            Log.Info(" CheckLoginCredential | OTP verified redirect to WelcomeRunner" , "");
                            Session["uid"] = datarecord.FirstOrDefault().UserId;
                            Session["errmsg"] = "";
                            return Redirect("/WelcomeRunner");
                        }
                        else
                        {
                            Log.Info(" CheckLoginCredential | Invalid OTP", "");
                            Session["LoginOTPmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/InvalidOTp", "Invalid OTP");
                            Session["LoginData"] = l;
                            return Redirect("/Register");
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(l.Email_id))
                        {
                            Log.Info(" CheckLoginCredential | Incorrect DOB/Contact Number", "");
                            Session["errmsg"] = "Incorrect DOB/Contact Number.";
                        }
                        Session["LoginData"] = l;
                        return Redirect("/Register");
                        //   return RedirectToAction("MarathonRegisteration");
                    }
                }
                else
                {
                    Log.Info(" CheckLoginCredential | Mobile Number is not Registered, Kindly Register as a New User", "");
                    Session["errmsg"] = DictionaryPhraseRepository.Current.Get("/ErrorMsg/NotRegistered", "This Mobile Number is not Registered, Kindly Register as a New User");
                    Session["LoginData"] = l;
                    return Redirect("/Register");
                }
            }
            catch(Exception ex)
            {
                Log.Info(" CheckLoginCredential Exception occured"+ex, "");
                Session["LoginData"] = l;
                return Redirect("/Register");
            }

        }
        public ActionResult CheckLoginCredential()
        {
            return View();
        }


        [HttpPost]
        public ActionResult VerifyOTP(RegistrationModel model)
        {
            var result = new { status = "0" };
            #region Verify OTP
            string generatedOTP = repo.GetOTP(model.ContactNumber);
            if (string.Equals(generatedOTP, model.OTP))
            {
                result = new { status = "1" };
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public string SendEmailOTP(string Email, string SessionID)
        {
            var result = new { status = "0" };
            if (SessionID == null)
            {
                if (Session["currentSession"] == null)
                {
                    result = new { status = "3" };
                    return result.status;
                }
                SessionID = Session["currentSession"].ToString();
            }
            bool flag = false;
            string AttmptCount = "0";
            using (AhmedabadMarathonRegistrationDataContext dbContext = new AhmedabadMarathonRegistrationDataContext())
            {
                AhmedabadMarathonOTP AHOTP = (
                        from x in dbContext.AhmedabadMarathonOTPs
                        where x.EmailId == Email
                        select x).FirstOrDefault<AhmedabadMarathonOTP>();
                if (AHOTP != null)
                {
                    AHOTP.Count = AHOTP.Count ?? "0";
                    if (Int64.Parse(AHOTP.Count) < 4)
                    {
                        flag = false;
                    }
                    else
                    {
                        DateTime value = AHOTP.Modified.Value;
                        flag = value.AddMinutes(30) > DateTime.Now;
                    }
                    if (!flag)
                    {
                        AttmptCount = AHOTP.Count;
                        if (AHOTP.Modified.Value.AddMinutes(30) <= DateTime.Now)
                        {
                            AttmptCount = "0";
                        }
                    }
                    else
                    {
                        result = new { status = "4" };
                        return result.status;
                    }
                }
            }
            Item MailItem = Sitecore.Context.Database.GetItem("{078E8C98-C96E-4CCB-AE1E-2A45C387FD78}");
            #region Send OTP

            string generatedOTP = repo.StoreEmailOTP(Email, SessionID, AttmptCount);
            string to = Email;
            string subject = MailItem.Fields["Subject"].Value;
            string body = MailItem.Fields["Message"].Value;
            body = body.Replace("$otp", generatedOTP);
            body = body.Replace("$email", Email);
            string from = MailItem.Fields["From"].Value;
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
                var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                result = new { status = "1" };
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Console.WriteLine(ex.Message, "sendOTPEmail - ");
                Log.Error(ex.Message, "sendOTPEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendOTPEmail - ");
            }
            #endregion
            return result.status;
        }

        [HttpPost]
        public ActionResult VerifyEmailOTP(string Email, string OTP)
        {
            var result = new { status = "0" };
            #region Verify OTP
            string generatedOTP = repo.GetEmailOTP(Email);
            if (string.Equals(generatedOTP, OTP))
            {
                if (repo.ParticipantStatus(Email) == string.Empty)
                {
                    result = new { status = "1" };
                }
                else if (repo.ParticipantStatus(Email) == "old")
                {
                    result = new { status = "2" };
                }
                else if (repo.ParticipantStatus(Email) == "new")
                {
                    result = new { status = "3" };
                }
            }
            else
            {
                result = new { status = "4" };
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DonationSavePaymentHistoy(string payment_id, string payment_status, string id, string transaction_id)
        {
            Log.Info("DonationSavePaymentHistoy Start | payment_status"+ payment_status + " for user id" + TempData["DonationUserId"], "");
            var Userid = TempData["DonationUserId"];
            ViewBag.uid = Userid;
            AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();
            Donate emp = new Donate();
            try
            {
                using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                {
                    if (objcontext.AhmedabadMarathonPaymentHistories.Where(x => x.UserId.ToString() == Userid.ToString() && x.OrderId == transaction_id.ToString()).Any())
                    {
                        AhmedabadMarathonPaymentHistory objToEdit = objcontext.AhmedabadMarathonPaymentHistories.Where(x => x.UserId.ToString() == Userid.ToString()).FirstOrDefault();
                        objToEdit.Status = payment_status;
                        objToEdit.PaymentRef = payment_id;
                        objToEdit.Modified_Date = System.DateTime.Now;
                        objcontext.SubmitChanges();
                    }

                    emp = objambd.AhmedabadMarathonDonations.Where(val => val.UserId == Userid.ToString()).Select(val => new Donate()
                    {
                        AffiliateCode = val.AffiliateCode,
                        Amount = val.Amount.ToString(),
                        CauseTitle = val.CauseTitle,
                        Name = val.Name,
                        PaymentStatus = val.PaymentStatus,
                        EmailId = val.EmailId,
                        Userid = new Guid(val.UserId),
                        MobileNumber = val.MobileNo,
                        TaxExemptionCause=val.TaxExemptionCause
                    }).SingleOrDefault();

                }

                if (payment_status != "Failed" && emp.PaymentStatus.ToLower() == "pending")
                {
                    //registration table update
                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                    {

                        AhmedabadMarathonDonation objToEdit = objcontext.AhmedabadMarathonDonations.Where(x => x.UserId.ToString() == Userid.ToString()).FirstOrDefault();
                        objToEdit.PaymentStatus = "successful";
                        //objToEdit.A = System.Convert.ToDecimal(emp.Amount);
                        objcontext.SubmitChanges();
                        //emp.PaymentStatus = "successful";
                    }
                    EmailService.SendMailDonationNow(emp);
                    sendDonationEmail(emp);
                    //sendSMS(emp, "");
                }
                else
                {
                    return Redirect("/registration-failed");
                }
            }
            catch (Exception ex)
            {
                Log.Error("Enable to sav Payment History:" + ex, this);
            }
            TempData.Remove("Registration");
            return View(emp);

        }




        public ActionResult DonationCounter()
        {

            AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();

            try
            {
                // List<Donate> donateList = new List<Donate>();
                //  List<AhmedabadMarathonRegistration> MarathonRegList = new List<AhmedabadMarathonRegistration>();

                var totalDonate = from rc in objambd.AhmedabadMarathonDonations
                                  where ((rc.PaymentStatus == "successful"))
                                  select rc;


                var MarathonRegList = from rc in objambd.AhmedabadMarathonRegistrations
                                      where ((rc.PaymentStatus == "successful" && rc.RegistrationStatus == "successful"))
                                      select rc;


                if (totalDonate.Count() > 0 && MarathonRegList.Count() > 0)
                {
                    try
                    {
                        decimal DonationAmount = 0;
                        decimal RegistrationAmount = 0;
                        var totalAmount = (from rc in totalDonate
                                           select rc.Amount).Sum();
                        if (totalAmount != null)
                        {
                            DonationAmount = (decimal)totalAmount;
                        }


                        var totalRegAmount = (from rc in MarathonRegList
                                              select rc.AmountReceived).Sum();
                        if (totalRegAmount != null)
                        {
                            RegistrationAmount = (decimal)totalRegAmount;
                        }
                        string amount = (DonationAmount + RegistrationAmount).ToString();
                        decimal parsedAmount = decimal.Parse(amount, CultureInfo.InvariantCulture);
                        CultureInfo hindi = new CultureInfo("hi-IN");
                        ViewBag.Message = string.Format(hindi, "{0:c}", parsedAmount);


                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = 0;
                        Sitecore.Diagnostics.Log.Error("Donate Counter:" + ex.Message, this);
                    }
                }
                else
                {
                    ViewBag.Message = 0;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = 0;
                Sitecore.Diagnostics.Log.Error("Donate Counter:" + ex.Message, this);
            }

            return View();

        }


        public ActionResult SavePaymentHistoy(string payment_id, string payment_status, string id, string transaction_id)
        {
            Log.Info("SavePaymentHistoy Start", "");
            DataTable dt = Session["GroupCart"] as DataTable;
            var Userid = Session["uid"].ToString();
            // var Userid = "224e3441-0c2d-491d-883c-bedebc92e44d";
            ViewBag.uid = Userid;
            AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();
            RegistrationModel emp = new RegistrationModel();
            //RegistrationModel m = (RegistrationModel)TempData["Registration"];
            try
            {
                using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                {
                    if (objcontext.AhmedabadMarathonPaymentHistories.Where(x => x.UserId.ToString() == Userid.ToString() && x.OrderId == transaction_id.ToString()).Any())
                    {
                        AhmedabadMarathonPaymentHistory objToEdit = objcontext.AhmedabadMarathonPaymentHistories.Where(x => x.UserId.ToString() == Userid.ToString()).FirstOrDefault();
                        objToEdit.Status = payment_status;
                        objToEdit.PaymentRef = payment_id;
                        objToEdit.Modified_Date = System.DateTime.Now;
                        objcontext.SubmitChanges();
                    }

                }
                if (dt!=null && Session["GroupRegisteration"].ToString() == "true")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][9].ToString() == "pending")
                        {
                            var empdata = objambd.AhmedabadMarathonRegistrations.Where(val => val.UserId == dt.Rows[i][6].ToString()).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();
                            if (empdata != null)
                            {
                                emp.RaceDistance = empdata.RaceDistance;
                                emp.ReferenceCode = empdata.ReferenceCode;
                                emp.FirstName = empdata.FirstName;
                                emp.LastName = empdata.LastName;
                                emp.Email = empdata.Email; 
                                decimal? d = empdata.FinalAmount;
                                emp.FinalAmount = d ?? 0;
                                emp.PaymentStatus = empdata.PaymentStatus;
                                emp.ContactNumber = empdata.ContactNumber;
                                emp.Age = empdata.Age;
                                emp.Gender = empdata.Gender;
                                emp.TShirtSize = empdata.TShirtSize;
                                emp.Useridstring = empdata.UserId;
                            }
                            if (payment_status != "Failed")
                            {
                                using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                                {
                                    AhmedabadMarathonRegistration objToEdit = objcontext.AhmedabadMarathonRegistrations.Where(x => x.UserId.ToString() == dt.Rows[i][6].ToString()).OrderByDescending(x => x.Id).FirstOrDefault();
                                    objToEdit.PaymentStatus = "successful";
                                    objToEdit.RegistrationStatus = "successful";
                                    objToEdit.AmountReceived = emp.FinalAmount;
                                    objcontext.SubmitChanges();
                                    emp.PaymentStatus = "successful";
                                }
                                sendEmail(emp);
                                sendSMS(emp, "");
                                /*if  charity bib then send mail*/
                                if(dt.Rows[i][10].ToString() == "Charity")
                                {
                                    EmailService.SendMailForCharityBib(empdata);
                                }

                                SMSOTP.registrationconfirmation(emp);
                            }
                            else
                            {
                                Session["GroupCart"] = null;
                                return Redirect("/registration-failed");
                            }
                        }                        
                    }
                    Session["GroupCart"] = null;
                }
                else { 
                    var empdata = objambd.AhmedabadMarathonRegistrations.Where(val => val.UserId == Userid.ToString()).OrderByDescending(x=>x.FormSubmitOn).FirstOrDefault();
                    if (empdata != null)
                    {
                        emp.RaceDistance = empdata.RaceDistance;
                        emp.ReferenceCode = empdata.ReferenceCode;
                        emp.FirstName = empdata.FirstName;
                        emp.LastName = empdata.LastName;
                        emp.Email = empdata.Email;
                        decimal? d = empdata.FinalAmount;
                        emp.FinalAmount = d ?? 0;
                        emp.PaymentStatus = empdata.PaymentStatus;
                        emp.ContactNumber = empdata.ContactNumber;
                        emp.Age = empdata.Age;
                        emp.Gender = empdata.Gender;
                        emp.TShirtSize = empdata.TShirtSize;
                        emp.Useridstring = empdata.UserId;
                    }

                    //if (payment_status != "Failed" && emp.PaymentStatus.ToLower() == "pending")
                    if (payment_status != "Failed")
                    {
                        //registration table update
                        using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                        {

                            AhmedabadMarathonRegistration objToEdit = objcontext.AhmedabadMarathonRegistrations.Where(x => x.UserId.ToString() == Userid.ToString()).OrderByDescending(x => x.Id).FirstOrDefault();
                            objToEdit.PaymentStatus = "successful";
                            objToEdit.RegistrationStatus = "successful";
                            objToEdit.AmountReceived = emp.FinalAmount;
                            objcontext.SubmitChanges();

                            emp.PaymentStatus = "successful";
                        }
                        sendEmail(emp);
                        sendSMS(emp, "");
                        SMSOTP.registrationconfirmation(emp);
                        if (empdata.RunType == "Charity")
                        {
                            EmailService.SendMailForCharityBib(empdata);
                        }
                    }
                    else
                    {
                        return Redirect("/registration-failed");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("SavePaymentHistoy exception occured" + ex, "");
                //ViewBag.FetchBillPayment = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                Log.Error("Enable to sav Payment History:" + ex.Message, this);
            }
            TempData.Remove("Registration");
            return View(emp);
            //return Redirect("properties?projectname=" + url);
        }
        public bool sendDonationEmail(Donate m, string msg = "")
        {
            bool status = false;
            try
            {
                Log.Error("AMD Marathon Donation send email start", "");
                AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();
                Donate emp = new Donate();

                //using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                //{
                //    emp = objambd.AhmedabadMarathonDonations.Where(val => val.UserId == m.Userid.ToString()).Select(val => new Donate()
                //    {
                //        Id = val.Id,
                //    }).SingleOrDefault();
                //}


                string message = "";
                string emailText = DictionaryPhraseRepository.Current.Get("/Registration/EmailText", "");
                string url = "https://" + Request.Url.Host;
                message = "<html><head><title>Thank you for the donation.</title><style>@font-face {font-family: 'Adani-Regular';font-style: normal;font-weight: 400;src: local('Montserrat-Regular'), url('/webfonts/Adani-Regular.woff') format('woff');}.table {font-family: 'Adani-Regular'; width: 100%; margin-bottom: 1rem; color: #212529;}.table thead th { vertical-align: middle;}.table td, .table th { padding: .75rem; vertical-align: top;}.table-striped tbody tr:nth-of-type(odd) { background-color: rgba(0,0,0,.05);}</style></head><body style=\" background: #f1f1f1;\">" +
                    "<table cellspacing=\"0\" class=\"table table-striped\" style=\"max-width: 680px;margin-left: auto;margin-right: auto;background: #fff;margin-top: 50px;margin-bottom: 50px;\"> <thead> <tr><th style=\"/* border-top: 5px solid #b93664; */border - bottom: 1px solid #ccc;background: #ffffff;\">" +
                    "<img src=\"" + url + "/-/media/8C32BA80FFB945DBA6468C03E1609895.ashx\"></th> </tr> <tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\">Hi " + m.Name + ",<br><br>Thank you for showing your solidarity towards the welfare of the Paraplegic Rehabilitation Centre beneficiaries. <br>Please find below your complete payment details:<br><p style=\"font-size:14px;font-weight:400; font-family: 'Segoe UI'; line-height:1.4; display:block; margin-top:0px;color:#3a3f4a;\"><b>PAYMENT DETAILS</b></p> <table style=\"width:100%;margin-bottom: 15px; margin-top: 0px;\" border=\"1\" cellspacing=\"0\" cellpadding=\"5\"><tbody><tr><td><p style=\"font-size:14px;font-weight:400; font-family: 'Segoe UI'; line-height:1.4; display:block; margin-top:0px;color:#3a3f4a;\"><b>Total Amount</b></p></td><td><p style=\"font-size:14px;font-weight:400; font-family: 'Segoe UI'; line-height:1.4; display:block; margin-top:0px;color:#3a3f4a;\">Rs. " + m.Amount + "</p></td></tr><tr><td><p style=\"font-size:14px;font-weight:400; font-family: 'Segoe UI'; line-height:1.4; display:block; margin-top:0px;color:#3a3f4a;\"><b>Paid for</b></p></td><td><p style=\"font-size:14px;font-weight:400; font-family: 'Segoe UI'; line-height:1.4; display:block; margin-top:0px;color:#3a3f4a;\">Instapay</p></td></tr></tbody></table><br>Thank You<br><br></th></tr></thead><tbody><tr></tr><tr><th colspan =\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\"><a href=\"https://www.facebook.com/AhmdMarathon\" style=\"color: #b93664;\">FACEBOOK</a> | <a href=\"https://www.instagram.com/AhmdMarathon\" style=\"color: #b93664;\">INSTAGRAM</a> | <a href=\"https://twitter.com/AhmdMarathon\" style=\"color: #b93664;\">TWITTER</a> | <a href=\"https://www.youtube.com/channel/UCE9B3d2_VpNZ2KupqxjyxQA\" style=\"color: #b93664;\">YouTube</a> | <a href=\"https://www.ahmedabadmarathon.com/FAQs\" style =\"color: #b93664;\">FAQs</a></th> </tr><tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\">For any queries regarding your registrations, feel free to contact us on <a href=\"mailto:ahmedabadmarathon@adani.com\" style=\"color: #b93664;\">ahmedabadmarathon@adani.com</a></th> </tr><tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\">Keep visiting <a href=\"\" style=\"color: #b93664;\">AhmedabadMarathon.com</a> for more information.</th> </tr> </tbody> </table></body></html>";

                // message = "Hello<br><br>" + emailText + "<br><br>Name: " + m.FirstName;
                //message = message + "<br>Mobile No: " + m.ContactNumber + "<br>Email Id: " + m.Email + "<br>Country: " + m.Country + "<br>State: " + m.State + "<br>Race Distance " + m.RaceDistance + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/Registration/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/Registration/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/Registration/DonationSubject", "");
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(m.EmailId);
                mail.Subject = emailSubject;
                mail.Body = message;
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
                Console.WriteLine("AMD Marathon Donation send email" + ex.Message, "sendEmail - ");
                Log.Error("AMD Marathon Donation send email"+ex, "sendEmail - ");
                Log.Error("AMD Marathon Donation send email" + ex.InnerException.ToString(), "sendEmail - ");
                return status;
            }
        }

        public bool sendEmail(RegistrationModel m, string msg = "")
        {
            bool status = false;
            try
            {
                Log.Info("AMD Marathon Send Email start", "");
                AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();
                RegistrationModel emp = new RegistrationModel();

                //using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                //{
                //    emp = objambd.AhmedabadMarathonRegistrations.Where(val => val.UserId == m.Useridstring.ToString()).Select(val => new RegistrationModel()
                //    {
                //        Id = val.Id,
                //        PaymentStatus = val.PaymentStatus,
                //        RegistrationStatus = val.RegistrationStatus,
                //        TimeSlot = val.TimeSlot,
                //        VaccinationCertificateName = val.VaccinationCertificate,
                //        RunDate = (DateTime.Parse(val.RunDate.ToString())).ToString("dd/MMM/yyyy")
                //    }).SingleOrDefault();
                //}

                var empList = objambd.AhmedabadMarathonRegistrations.Where(val => val.UserId == m.Useridstring.ToString()).FirstOrDefault();


                string message = "";
                string reportingTime = "";
                string BibExpo = "";
                if (empList.RunMode == "Remote")
                {
                    m.RunDate = "";
                    m.TimeSlot = "";
                    m.VaccinationCertificateName = "";
                    BibExpo = DictionaryPhraseRepository.Current.Get("/Registration/bibExpo", "Dates to be announced shortly");

                }
                else
                {
                    m.RunDate = empList.RunDate.ToString();
                    m.TimeSlot = empList.TimeSlot;
                    m.VaccinationCertificateName = empList.VaccinationCertificate;
                    reportingTime = DictionaryPhraseRepository.Current.Get("/Registration/reportingTime", "60 mins before reporting time");
                    BibExpo = DictionaryPhraseRepository.Current.Get("/Registration/bibExpo", "Dates to be announced shortly");
                }
                var AcknowledgeMail = Context.Database.GetItem(EMailTemplate.AcknowledgementMail);

                var TimeSlot = RegistrationFormValidation.FlagOffTimeTime(m.RaceDistance);
                reportingTime = RegistrationFormValidation.ReportingTime(m.RaceDistance);
                string emailText = DictionaryPhraseRepository.Current.Get("/Registration/EmailText", "");
                string url = "https://" + Request.Url.Host;

                var CharityMail = Context.Database.GetItem(EMailTemplate.CharityBib);
                message = AcknowledgeMail.Fields[EMailTemplate.Fields.Body].Value; ;
                message=message.Replace("[FirstName]", empList.FirstName);
                message=message.Replace("[Gender]", empList.Gender);
                message=message.Replace("[Age]", empList.Age);
                message=message.Replace("[Id]", empList.Id.ToString());
                message=message.Replace("[TShirtSize]", empList.TShirtSize);
                message=message.Replace("[ContactNumber]", empList.ContactNumber);
                message=message.Replace("[RaceDistance]", empList.RaceDistance);
                message=message.Replace("[TimeSlot]", TimeSlot);
                message=message.Replace("[reportingTime]", reportingTime);
                message=message.Replace("[BibExpo]", BibExpo);

                string to = DictionaryPhraseRepository.Current.Get("/Registration/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/Registration/EmailFrom", "");
                string emailSubject = AcknowledgeMail.Fields[EMailTemplate.Fields.Subject].Value;
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(empList.Email);
                mail.Subject = emailSubject;
                mail.Body = message;
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
                Log.Error("AMD Marathon Send Email exception"+ex, "");
                return status;
            }
        }


        public bool sendTrainingEmail(RegistrationModel m, string msg = "")
        {
            bool status = false;
            try
            {

                AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();
                RegistrationModel emp = new RegistrationModel();

                using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                {
                    emp = objambd.AhmedabadMarathonRegistrations.Where(val => val.UserId == m.Useridstring.ToString()).Select(val => new RegistrationModel()
                    {
                        Id = val.Id,
                    }).SingleOrDefault();
                }


                string message = "";
                string emailText = DictionaryPhraseRepository.Current.Get("/Registration/EmailText", "");
                string url = "https://" + Request.Url.Host;
                message = "<html><head><title>Thank you for the registrations.</title><style>@font-face {font-family: 'Adani-Regular';font-style: normal;font-weight: 400;src: local('Montserrat-Regular'), url('/webfonts/Adani-Regular.woff') format('woff');}.table {font-family: 'Adani-Regular'; width: 100%; margin-bottom: 1rem; color: #212529;}.table thead th { vertical-align: middle;}.table td, .table th { padding: .75rem; vertical-align: top;}.table-striped tbody tr:nth-of-type(odd) { background-color: rgba(0,0,0,.05);}</style></head><body style=\" background: #f1f1f1;\">" +
                    "<table cellspacing=\"0\" class=\"table table-striped\" style=\"max-width: 680px;margin-left: auto;margin-right: auto;background: #fff;margin-top: 50px;margin-bottom: 50px;\"> <thead> <tr><th style=\"/* border-top: 5px solid #b93664; */border - bottom: 1px solid #ccc;background: #ffffff;\">" +
                    "<img src=\"" + url + "/-/media/8C32BA80FFB945DBA6468C03E1609895.ashx\"></th> </tr> <tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\">Hi " + m.FirstName + ",<br><br>Your registration for our #Run4OurSoldiers is completed. 100% of the registration proceeds would be donated to Indian Armed Forces. <br><br>Thank You<br><br><p style=\"font-weight: 100;\">Your Registration details are as follows:</p></th> </tr> </thead> <tbody><tr><td colspan=\"2\" style=\"background:#fff;\"><table class=\"table\" cellspacing=\"0\" style=\"max-width: 600px;margin-left: auto;margin-right: auto;\"><thead><tr style=\"background: #fff;\"><th style=\"border: 1px solid #ccc;width: 40%;text-align: left;\">Name</th><th style=\"border: 1px solid #ccc;text-align: left;\">" + m.FirstName + "</th></tr></thead><tbody><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Race Category:</b></td><td style=\"border: 1px solid #ccc;\">" + m.RaceDistance + "</td></tr><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Gender:</b></td><td style=\"border: 1px solid #ccc;\">" + m.Gender + "</td></tr><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Age:</b></td><td style=\"border: 1px solid #ccc;\">" + m.Age + "</td></tr><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Registration ID/ Charity Runner ID:</b></td><td style=\"border: 1px solid #ccc;\">" + emp.Id + "</td></tr><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>T-shirt size:</b></td><td style=\"border: 1px solid #ccc;\">" + m.TShirtSize + "</td></tr><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Contact Number:</b></td><td style=\"border: 1px solid #ccc;\">" + m.ContactNumber + "</td></tr><tr style=\"background: #fff;\"><td colspan=\"2\" style=\"border: 1px solid #ccc;text-align:center;padding: 20px 0px;color: #166aac;\"><b>Event Details:</b></td></tr>" +
        "<tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Organizer:</b></td><td style=\"border: 1px solid #ccc;\">Adani Ahmedabad Marathon</td></tr><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Event Start Date:</b></td><td style=\"border: 1px solid #ccc;\">27<sup>th</sup> November 2022</td></tr><tr style=\"background: #fff;\"><td style=\"border: 1px solid #ccc;\"><b>Event Link:</b></td><td style=\"border: 1px solid #ccc;\"><a style=\"word-break: break-word;\" href=\"https://www.ahmedabadmarathon.com/\">https://www.ahmedabadmarathon.com</a></td></tr></tbody></table></td></tr><tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\"><b>NOTE:</b> You just need to log-in to make any changes to your registration data. </th> </tr><tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\"><a href=\"https://www.facebook.com/AhmdMarathon\" style=\"color: #b93664;\">FACEBOOK</a> | <a href=\"https://www.instagram.com/AhmdMarathon\" style=\"color: #b93664;\">INSTAGRAM</a> | <a href=\"https://twitter.com/AhmdMarathon\" style=\"color: #b93664;\">TWITTER</a> | <a href=\"https://www.youtube.com/channel/UCE9B3d2_VpNZ2KupqxjyxQA\" style=\"color: #b93664;\">YouTube</a> | <a href=\"https://www.ahmedabadmarathon.com/FAQs\" style =\"color: #b93664;\">FAQs</a></th> </tr><tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\">For any queries regarding your registrations, feel free to contact us on <a href=\"mailto:ahmedabadmarathon@adani.com\" style=\"color: #b93664;\">ahmedabadmarathon@adani.com</a></th> </tr><tr> <th colspan=\"2\" style=\"text-align: left;padding: 30px 15px;color: #b93664;\">Keep visiting <a href=\"\" style=\"color: #b93664;\">AhmedabadMarathon.com</a> for more information.</th> </tr> </tbody> </table></body></html>";

                // message = "Hello<br><br>" + emailText + "<br><br>Name: " + m.FirstName;
                //message = message + "<br>Mobile No: " + m.ContactNumber + "<br>Email Id: " + m.Email + "<br>Country: " + m.Country + "<br>State: " + m.State + "<br>Race Distance " + m.RaceDistance + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/Registration/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/Registration/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/Registration/EmailSubject", "");
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(m.Email);
                mail.Subject = emailSubject;
                mail.Body = message;
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


        public bool sendSMS(RegistrationModel m, string msg = "")
        {

            #region Api call to send SMS of OTP
            try
            {
                AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();
                RegistrationModel emp = new RegistrationModel();

                using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                {
                    emp = objambd.AhmedabadMarathonRegistrations.Where(val => val.UserId == m.Useridstring.ToString()).Select(val => new RegistrationModel()
                    {
                        Id = val.Id,
                    }).SingleOrDefault();
                }


                var client = new RestClient(DictionaryPhraseRepository.Current.Get("/SMS/URL", ""));
                string username = DictionaryPhraseRepository.Current.Get("/SMS/UserName", "");
                string password = DictionaryPhraseRepository.Current.Get("/SMS/Password", "");
                string from = DictionaryPhraseRepository.Current.Get("/SMS/from", "");


                msg = "Dear " + m.FirstName + ", Thank you for successfully registering for Adani Ahmedabad Marathon 2021 in " + m.RaceDistance + " Category. " + emp.Id + " is your registration id / charity runner id.";
                byte[] concatenated = System.Text.ASCIIEncoding.ASCII.GetBytes(username + ":" + password);
                string header = System.Convert.ToBase64String(concatenated);
                var request = new RestRequest(Method.POST);

                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Basic " + header);
                request.AddParameter("application/json", "{\"from\":\"" + from + "\", \"to\":\"91" + m.ContactNumber + "\",\"text\":\"" + msg + "\"}", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    Log.Error("OTP Api call success.", this);
                    return true;
                }
                else
                {
                    Log.Error("OTP Api call failed.", this);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return false;
            }
            #endregion

        }



        AhmedabadMarathonRegistrationDataContext marathonDb = new AhmedabadMarathonRegistrationDataContext();
        public ActionResult userinfo()
        {
            try {
                Log.Info("Marathon User info GET", "");
                if (Session["uid"] == null)
                {
                    Session["errmsg"] = "Session Expired! Please login again.";
                    return Redirect("/Register");
                }
                //User id
                var Racedetails = Sitecore.Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
                string uid = Session["uid"].ToString();
                RegistrationModel emp = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == uid).OrderByDescending(x => x.FormSubmitOn).Select(val => new RegistrationModel()
                {
                    BIBNumber = val.BIBNumber,
                    IsRaceDistanceChanged = val.IsRaceDistanceChanged == null ? false : val.IsRaceDistanceChanged,
                    RaceLocation = val.RaceLocation == null ? "Ahmedabad" : val.RaceLocation,//1

                    //ReceDetailsLists = MarathonHelper.GetRaceDistance(),
                    RegistrationStatus = val.RegistrationStatus,
                    Updated = bool.Parse(val.Updated.ToString()),
                    //  RaceDistance = val.RaceDistance.Trim(),//4
                    RaceDistance = val.RaceDistance != null ? val.RaceDistance.Trim() : "",
                    RaceAmount = (decimal)val.RaceAmount,
                    ReferenceCode = val.ReferenceCode,
                    FirstName = val.FirstName,
                    LastName = val.LastName,
                    DateofBirth = val.DateofBirth.ToString(),
                    Email = val.Email,
                    ContactNumber = val.ContactNumber,
                    Nationality = val.Nationality,
                    Gender = val.Gender.Trim(),
                    RunMode = val.RunMode,
                    // RunDate = (DateTime.Parse(val.RunDate.ToString())).ToString("dd/MMM/yyyy"),//3
                    RunDate = val.RunDate.ToString() != null ? (DateTime.Parse(val.RunDate.ToString())).ToString("dd/MMM/yyyy") : "",//3
                                                                                                                                     // TimeSlot = val.TimeSlot,//2
                    TimeSlot = val.TimeSlot != null ? val.TimeSlot.Trim() : "",
                    Vaccinationted = val.Vaccinationted,
                    VaccinationCertificateName = val.VaccinationCertificate,
                    TShirtSize = val.TShirtSize,
                    NamePreferredonBIB = val.NamePreferredonBIB,
                    IdentityProofType = val.IdentityProofType,
                    IdentityProofNumber = val.IdentityProofNumber,
                    // IDCardAttachment = val.IDCardAttachment,
                    Country = val.Country,
                    State = val.State,
                    City = val.City,
                    Address = val.Address,
                    Pincode = val.Pincode,
                    EmergencyContactName = val.EmergencyContactName,
                    EmergencyContactRelationship = val.EmergencyContactRelationship,
                    EmergencyContactNumber = val.EmergencyContactNumber,
                    BloodGroup = val.BloodGroup,
                    ChronicIllness = val.ChronicIllness,
                    HeartAilment = val.HeartAilment,
                    AnyFaintingEpisodesinPast = val.AnyFaintingEpisodesinPast,
                    AnyOtherAilment = val.AnyOtherAilment,
                    AnyKnownAllergies = val.AnyKnownAllergies,
                    PayrollCompany = val.PayrollCompany,
                    EmployeeID = val.EmployeeID,
                    UnitStation = val.UnitStation,
                    PaymentStatus = val.PaymentStatus,
                    IdCardFilename = val.IDCardAttachment,
                    ShantigramIdProofFilename = val.ShantigramIdProof,
                    DefencePersonnel = val.DefencePersonnel,
                    RaceCertificateName = val.RaceCertificate,
                    DetailsOfFullHalfMarathon = val.DetailsOfFullHalfMarathon,
                    RunType = val.RunType,
                    EmployeeEmailId = val.EmployeeEmailId,
                    DonationAmount = decimal.Parse(val.DonationAmount.ToString() ?? "0.00"),
                    FinalAmount = decimal.Parse(val.FinalAmount.ToString() ?? "0.00"),
                    PANNumber = val.PANNumber,
                    TaxExemptionCause = val.TaxExemptionCause,
                    TaxExemptionCertificate = bool.Parse(val.TaxExemptionCertificate)

                }).FirstOrDefault();
                //emp.RunDateList = new List<SelectListItem>();
                //emp.TimeSlotList = new List<SelectListItem>();
                if (emp.RunMode == "Physical")
                {
                    emp.ReceDetailsLists = MarathonHelper.GetRaceDistance();
                    if (!string.IsNullOrEmpty(emp.RaceDistance))
                    {
                        foreach (Item distance in Racedetails.GetChildren())
                        {
                            if (distance.Fields["Distance"].Value == emp.RaceDistance && distance.HasChildren)
                            {
                                emp.RunDateList = distance.GetChildren().ToList().Select(x => new SelectListItem()
                                {
                                    Value = (Sitecore.DateUtil.ToServerTime(((DateField)x.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy"),
                                    Text = x.Fields["RunDateText"].Value
                                }).ToList();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(emp.RunDate) && !string.IsNullOrEmpty(emp.RaceDistance))
                    {
                        foreach (Item distance in Racedetails.GetChildren())
                        {
                            if (distance.Fields["Distance"].Value == emp.RaceDistance && distance.HasChildren)
                            {
                                foreach (Item date in distance.GetChildren())
                                {
                                    if ((Sitecore.DateUtil.ToServerTime(((DateField)date.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy") == emp.RunDate)
                                    {
                                        emp.TimeSlotList = date.GetChildren().ToList().Select(x => new CustomSelectItem()
                                        {
                                            // Text = x.Fields["Text"].Value,
                                            Text = x.Fields["Text"].Value,
                                            Value = x.Fields["Value"].Value,
                                            MaxAllowedCount = Int32.Parse(x.Fields["Count"].Value ?? "250")
                                        }).ToList();
                                        if (emp.RunMode == "Physical")
                                        {
                                            AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                                            foreach (var times in emp.TimeSlotList)
                                            {
                                                var RunList = rdb.AhmedabadMarathonRegistrations.Where(x => x.RunMode == emp.RunMode && x.RaceDistance == emp.RaceDistance && x.RegistrationStatus == "successful" && x.Updated != null && x.RunDate == DateTime.Parse(emp.RunDate.ToString(), CultureInfo.GetCultureInfo("en-gb")) && x.TimeSlot == times.Value);
                                                if (RunList.Count() >= times.MaxAllowedCount)
                                                {
                                                    times.Disabled = true;
                                                    // times.Text = times.Text + " (slot full)";
                                                    times.Text = times.Value;
                                                    if (!string.IsNullOrEmpty(emp.TimeSlot) && emp.TimeSlot == times.Value)
                                                    {
                                                        if (emp.RegistrationStatus != "successful")
                                                        {
                                                            emp.TimeSlot = "";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //  times.Text = times.Text + " (slot left:" + (times.MaxAllowedCount - RunList.Count()) + ")";
                                                    times.Text = times.Value;
                                                }
                                            }
                                            //List<CustomSelectItem> tslot = new List<CustomSelectItem>();
                                            //foreach (var timeslot in emp.TimeSlotList)
                                            //{
                                            //    tslot.Add(new CustomSelectItem { Text = timeslot.Text, Value = timeslot.Value, Disabled = timeslot.Disabled });
                                            //}
                                            ViewData["TimeSlotList"] = emp.TimeSlotList;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (emp.RunMode == "Remote")
                {
                    emp.ReceDetailsLists = MarathonHelper.GetRemoteRaceDistance();
                }
                bindState();
                emp.StateList = ViewBag.state;
                return View(emp);
            }
            catch (Exception ex)
            {
                Log.Error("Marathon User info GET exception occured" + ex, "");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult userinfo(RegistrationModel m)
        {
            try {
                Log.Info("Marathon User info POST", "");
                bindState();
                m.StateList = ViewBag.state;
                if (Session["uid"] == null)
                {
                    Log.Info("Marathon User info POST | Invalid Session", "");
                    Session["errmsg"] = "Session Timeout! Please login again.";
                    return Redirect("/Register");
                }
                TempData["USerId"] = Session["uid"].ToString();
                TempData.Keep();
                AlreadyRegisterdUser alreadyRegisterdUser = new AlreadyRegisterdUser();
                var Url = alreadyRegisterdUser.UpdateRaceInfo(m, Session["uid"].ToString(), Request.Url.ToString());
                if (Url == "0")
                {
                    Log.Info("Marathon User info POST | URL is equal 0"+Url, "");
                    return View(m);
                }
                return Redirect(Url);
            }
            catch (Exception ex)
            {
                Log.Error("Marathon User info POST exception occured" + ex, "");
                return View();
            }
        }

        AhmedabadMarathonRegistrationDataContext MarathonDonation = new AhmedabadMarathonRegistrationDataContext();
        public ActionResult WelcomeRunnerUserInfo()
        {
            try {
                Log.Info("Marathon Welcome RunnerINfo Get start", "");
                if (Session["uid"] == null)
                {
                    Session["errmsg"] = "Invalid login attempt.";
                    return Redirect("/Register");
                }
                AlreadyRegisterdUser alreadyRegisterdUser = new AlreadyRegisterdUser();
                RegistrationModel lastYearDetail = alreadyRegisterdUser.LastYearDetail(Session["uid"].ToString());
                return base.View(lastYearDetail);
            }
            catch(Exception ex)
            {
                Log.Error("Marathon Welcome RunnerINfo Get exception occured"+ex, "");
                RegistrationModel lastYearDetail = new RegistrationModel();
                return base.View(lastYearDetail);
            }
        }


        [HttpPost]
        public JsonResult AddRunnerUserInfo(RegistrationModel m)
        {
            AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
            AhmedabadMarathonRegistration emp = new AhmedabadMarathonRegistration();
            var BIB_Number = repo.GenerateRandomBIBNumber(m.RaceDistance);
            if (Session["uid"] != null)
            {
                m.ReferenceCode = m.ReferenceCode;
                m.ContactNumber = m.ContactNumber.Trim();
                m.DateofBirth = m.DateofBirth.Trim();
                m.Email = m.Email.Trim();
                m.FirstName = m.FirstName.Trim();
                m.LastName = m.LastName.Trim();
                m.PaymentStatus = m.PaymentStatus;
                m.RegistrationStatus = m.RegistrationStatus;
                m.TShirtSize = m.TShirtSize.Trim();
                m.RaceAmount = m.RaceAmount;

                DateTime getDateOfBirth = DateTime.Now;
                string id = Session["uid"].ToString();
                emp = MarathonDonation.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == id).OrderBy(x => x.Id).FirstOrDefault();
                emp.UserId = id;
                try
                {
                    if (!string.IsNullOrEmpty(m.DateofBirth))
                    {
                        getDateOfBirth = DateTime.Parse(m.DateofBirth.ToString(), CultureInfo.GetCultureInfo("en-gb"));
                    }
                    emp.ReferenceCode = m.ReferenceCode;
                    emp.FirstName = m.FirstName.Trim();
                    emp.LastName = m.LastName.Trim();
                    emp.DateofBirth = getDateOfBirth;
                    DateTime currentDate = DateTime.Parse(DateTime.Now.ToString());
                    TimeSpan timespan = currentDate.Subtract(getDateOfBirth);
                    emp.Age = ((int)(timespan.TotalDays) / 365).ToString();
                    m.Age = emp.Age;
                    emp.Email = m.Email.Trim();
                    emp.ContactNumber = m.ContactNumber.Trim();
                    emp.TShirtSize = m.TShirtSize.Trim();
                    emp.RaceAmount = m.RaceAmount;

                    if (!string.IsNullOrEmpty(m.DetailsOfFullHalfMarathon))
                    {
                        emp.DetailsOfFullHalfMarathon = m.DetailsOfFullHalfMarathon;
                    }
                    if (m.RaceDistance != emp.RaceDistance && emp.IsRaceDistanceChanged==true)
                    {
                        emp.BIBNumber = repo.GenerateRandomBIBNumber(m.RaceDistance);
                    }
                    else
                    {
                        emp.BIBNumber = emp.BIBNumber;
                    }
                    //praveen
                    if (!string.IsNullOrEmpty(m.RunDate))
                    {
                        emp.RunDate = DateTime.Parse(m.RunDate.ToString());
                    }
                    decimal? d = emp.FinalAmount;
                    m.FinalAmount = d ?? 0;
                    emp.NamePreferredonBIB = m.FirstName.ToUpper().Trim() + " " + m.LastName.ToUpper().Trim();
                    emp.Updated = false;
                    emp.PaymentStatus = "pending";
                    emp.RegistrationStatus = "successful";
                    emp.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                    //ID raceLocationID = new ID("{969F5F5A-8F91-415E-BDB7-7712AD3A43D2}");
                    //var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                    //emp.RaceLocation = raceLocation.Fields["Name"].Value;
                    ID raceLocationID = new ID("{9D55601D-21DA-46AC-94B7-DCB0DE626927}");
                    var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                    emp.RaceLocation = raceLocation.Fields["Location"].Value;
                    //m.ReferenceCode = "aaabc12";
                    if (m.RunMode == "Remote Run")
                    {
                        m.FinalAmount = System.Convert.ToDecimal("499.00");
                        emp.RaceAmount = System.Convert.ToDecimal("499.00");
                        emp.FinalAmount = System.Convert.ToDecimal("499.00");
                        m.RaceAmount = System.Convert.ToDecimal("499.00");
                    }
                    else
                    {
                        Item RaceDetails = Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
                        emp.FinalAmount = System.Convert.ToDecimal(RaceDetails.GetChildren().Where(x => x.Fields["Distance"].Value == m.RaceDistance).FirstOrDefault().Fields["Amount"].Value);
                        m.FinalAmount = emp.FinalAmount ?? 0;
                        emp.RaceAmount = emp.FinalAmount ?? 0;
                        m.RaceAmount = emp.FinalAmount ?? 0;
                    }
                    if (m.ReferenceCode != null)
                    {
                        var item = Sitecore.Context.Database.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
                        var filterdata = item.Children.Where(x => x.Fields["Coupon Title"].Value.ToLower() == (m.ReferenceCode.ToLower()));
                        if (filterdata.Count() > 0)
                        {
                            foreach (var element in filterdata.ToList())
                            {

                                var discount = element.Fields["Enter Discount Rate in Percentage"].Value;
                                if (!string.IsNullOrEmpty(discount) && (decimal.Parse(discount) >= 0 && (decimal.Parse(discount) <= 100)))
                                {
                                    var pay_status = element.Fields["Pay remaining balance or not"].Value;
                                    if (!string.IsNullOrEmpty(pay_status))
                                    {
                                        if (pay_status.ToLower() == "yes")
                                        {
                                            var finalamt = m.RaceAmount - ((m.RaceAmount * decimal.Parse(discount)) / 100);
                                            emp.PaymentStatus = "pending";
                                            emp.DiscountRate = discount;
                                            emp.FinalAmount = finalamt;
                                            m.FinalAmount = finalamt;
                                        }
                                        else if (pay_status.ToLower() == "complimentary")
                                        {
                                            emp.PaymentStatus = "complementary";
                                            emp.RegistrationStatus = "successful";
                                            emp.AmountReceived = 0;
                                            var finalamt = m.RaceAmount - ((m.RaceAmount * decimal.Parse(discount)) / 100);
                                            emp.DiscountRate = discount;
                                            emp.FinalAmount = finalamt;
                                            m.FinalAmount = finalamt;
                                        }
                                        else
                                        {
                                            emp.PaymentStatus = "no";
                                            emp.RegistrationStatus = "successful";
                                            emp.AmountReceived = 0;
                                            var finalamt = m.RaceAmount - ((m.RaceAmount * decimal.Parse(discount)) / 100);
                                            emp.DiscountRate = discount;
                                            emp.FinalAmount = finalamt;
                                            m.FinalAmount = finalamt;

                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        emp.ReferenceCode = "";
                    }
                    //emp.ReferenceCode = "";
                    TempData["USerId"] = emp.UserId.ToString();
                    TempData.Keep();
                    //if (!string.IsNullOrWhiteSpace(emp.RaceDistance))
                    //{
                    //    rdb.AhmedabadMarathonRegistrations.InsertOnSubmit(emp);
                    //    rdb.SubmitChanges();
                    //}
                    //else
                    //{


                    emp.RaceLocation = raceLocation.Fields["Location"].Value;
                    emp.RaceDistance = m.RaceDistance;
                    emp.TimeSlot = m.TimeSlot;
                    emp.RunMode = m.RunMode;
                    if (m.RunDate == null)
                    {
                        emp.RunDate = DateTime.Parse("2022-11-19"); //Remote date
                    }
                    else
                    {
                        emp.RunDate = DateTime.Parse(m.RunDate.ToString());
                    }
                    emp.ReferenceCode = m.ReferenceCode;
                    MarathonDonation.SubmitChanges();
                    //}
                    //if (emp != null && emp.PaymentStatus.ToLower() == "pending")
                    //{
                    //    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                    //    {
                    //        m.OrderId = (Guid.NewGuid()).ToString();
                    //        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                    //        {
                    //            UserId = (emp.UserId).ToString(),
                    //            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                    //            Id = Guid.NewGuid(),
                    //            Amount = System.Convert.ToString(m.FinalAmount),
                    //            Email = m.Email.Trim(),
                    //            Mobile = m.ContactNumber.Trim(),
                    //            UserType = "Guest",
                    //            GatewayType = "Insta-Mojo",
                    //            Created_Date = System.DateTime.Now,
                    //            RequestTime = System.DateTime.Now,
                    //            CreatedBy = m.FirstName.Trim() + " " + m.LastName.Trim(),
                    //            AccountNumber = m.ReferenceCode,
                    //            OrderId = m.OrderId,
                    //            PaymentType = "Token Amount",
                    //            ResponseMsg = Request.Url.ToString()
                    //        };

                    //        marathonDb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                    //        marathonDb.SubmitChanges();
                    //        PaymentService objPaymentService = new PaymentService();
                    //        ResultPayment Objresult = new ResultPayment();
                    //        Objresult = objPaymentService.Payment(m);
                    //        Sitecore.Diagnostics.Log.Info("Objresult payment " + Objresult.Message, "");

                    //        if (Objresult.IsSuccess)
                    //        {
                    //            sendEmail(m);
                    //            return Json(new { result = "Redirect", url = Objresult.Message });
                    //        }
                    //        else
                    //        {
                    //            //ViewBag.ErrorMessage = Objresult.Message;
                    //            return Json(new { result = "Redirect", url = "/WelcomeRunner" });
                    //        }


                    //    }
                    //}
                    //else
                    //{
                    //    m.Useridstring = emp.UserId;
                    //    sendEmail(m);
                    //    sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");
                    //    return Json(new { result = "Redirect", url = "/registration-thankyou" });

                    //}
                    //return Json(new { result = "Redirect", url = "/WelcomeRunner" });
                }
                catch (Exception ex)
                {
                    Log.Error("Error in userinfo post controller", ex, this);
                }
            }
            //return Json(new { result = "Redirect", url = "/registration-thankyou" });
            return Json(new { result = "Redirect", url = "/WelcomeRunner" });
        }

        [HttpPost]
        public JsonResult PayNowUserInfo(RegistrationModel m)
        {
            Log.Info("PayNowUserInfo start", "");
            AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
            AhmedabadMarathonRegistration emp = new AhmedabadMarathonRegistration();

            if (Session["uid"] != null)
            {
                m.ContactNumber = m.ContactNumber.Trim();
                m.DateofBirth = m.DateofBirth.Trim();
                m.Email = m.Email.Trim();
                m.FirstName = m.FirstName.Trim();
                m.LastName = m.LastName.Trim();
                m.PaymentStatus = m.PaymentStatus.Trim();
                m.RegistrationStatus = m.RegistrationStatus.Trim();
                m.TShirtSize = m.TShirtSize.Trim();

                DateTime getDateOfBirth = DateTime.Now;
                string id = Session["uid"].ToString();
                emp = MarathonDonation.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == id).OrderBy(x => x.Id).FirstOrDefault();
                emp.UserId = id;
                try
                {
                    if (!string.IsNullOrEmpty(m.DateofBirth))
                    {
                        getDateOfBirth = DateTime.Parse(m.DateofBirth.ToString(), CultureInfo.GetCultureInfo("en-gb"));
                    }
                    if (m.RaceDistance != emp.RaceDistance)
                    {
                        emp.BIBNumber = repo.GenerateRandomBIBNumber(m.RaceDistance);
                    }
                    else
                    {
                        emp.BIBNumber = emp.BIBNumber;
                    }
                    emp.ReferenceCode = m.ReferenceCode;
                    emp.FirstName = m.FirstName.Trim();
                    emp.LastName = m.LastName.Trim();
                    emp.DateofBirth = getDateOfBirth;
                    DateTime currentDate = DateTime.Parse(DateTime.Now.ToString());
                    TimeSpan timespan = currentDate.Subtract(getDateOfBirth);
                    emp.Age = ((int)(timespan.TotalDays) / 365).ToString();
                    m.Age = emp.Age;
                    emp.Email = m.Email.Trim();
                    emp.ContactNumber = m.ContactNumber.Trim();
                    emp.TShirtSize = m.TShirtSize.Trim();

                    if (!string.IsNullOrEmpty(m.DetailsOfFullHalfMarathon))
                    {
                        emp.DetailsOfFullHalfMarathon = m.DetailsOfFullHalfMarathon;
                    }
                    decimal? d = emp.FinalAmount;
                    m.FinalAmount = d ?? 0;
                    emp.NamePreferredonBIB = m.FirstName.ToUpper().Trim() + " " + m.LastName.ToUpper().Trim();
                    emp.Updated = false;
                    emp.PaymentStatus = "pending";
                    emp.RegistrationStatus = "successful";
                    emp.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                    //ID raceLocationID = new ID("{969F5F5A-8F91-415E-BDB7-7712AD3A43D2}");
                    //var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                    //emp.RaceLocation = raceLocation.Fields["Name"].Value;
                    ID raceLocationID = new ID("{9D55601D-21DA-46AC-94B7-DCB0DE626927}");
                    var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                    emp.RaceLocation = raceLocation.Fields["Location"].Value;
                    var location = emp.RaceLocation = raceLocation.Fields["Location"].Value;
                    emp.ReferenceCode = "";
                    TempData["USerId"] = emp.UserId.ToString();
                    TempData.Keep();
                    //rdb.AhmedabadMarathonRegistrations.InsertOnSubmit(emp);
                    //rdb.SubmitChanges();
                    if (emp.PaymentStatus.ToLower() == "pending")
                    {
                        Log.Info("PayNowUserInfo PaymentStatus is pending", "");
                        using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                        {
                            m.OrderId = (Guid.NewGuid()).ToString();
                            AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                            {
                                UserId = (emp.UserId).ToString(),
                                TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                                Id = Guid.NewGuid(),
                                Amount = System.Convert.ToString(m.FinalAmount),
                                Email = m.Email.Trim(),
                                Mobile = m.ContactNumber.Trim(),
                                UserType = "Guest",
                                GatewayType = "Insta-Mojo",
                                Created_Date = System.DateTime.Now,
                                RequestTime = System.DateTime.Now,
                                CreatedBy = m.FirstName.Trim() + " " + m.LastName.Trim(),
                                AccountNumber = m.ReferenceCode,
                                OrderId = m.OrderId,
                                PaymentType = "Token Amount",
                                ResponseMsg = Request.Url.ToString()
                            };

                            //marathonDb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                            //marathonDb.SubmitChanges();
                            PaymentService objPaymentService = new PaymentService();
                            ResultPayment Objresult = new ResultPayment();
                            Objresult = objPaymentService.Payment(m);
                            Sitecore.Diagnostics.Log.Info("Objresult payment " + Objresult.Message, "");
                            Session["GroupRegisteration"] = "false";

                            if (Objresult.IsSuccess)
                            {
                                Log.Info("PayNowUserInfo Objresult is Success", "");
                                sendEmail(m);
                                return Json(new { result = "Redirect", url = Objresult.Message });
                            }
                            else
                            {
                                Log.Info("PayNowUserInfo Objresult is faile", "");
                                ViewBag.ErrorMessage = Objresult.Message;
                                return Json(new { result = "Redirect", url = "/registration-failed" });
                            }

                        }
                    }
                    else
                    {
                        Log.Info("PayNowUserInfo PaymentStatus is not pending", "");
                        m.Useridstring = emp.UserId;
                        sendEmail(m);
                        sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");
                        return Json(new { result = "Redirect", url = "/registration-thankyou" });
                    }

                }
                catch (Exception ex)
                {
                    Log.Error("PayNowUserInfo exception", ex, this);
                }
            }
            Log.Info("PayNowUserInfo Session is null", "");
            return Json(new { result = "Redirect", url = "/registration-thankyou" });
        }

        AhmedabadMarathonRegistrationDataContext DonateInfo = new AhmedabadMarathonRegistrationDataContext();

        //  Start Remove the data 
        [HttpPost]
        public JsonResult RemoveUserInfo(string location)
        {
            var flagHideRaceInfo = "false";
            if (!string.IsNullOrEmpty(Session["uid"].ToString()))
            {
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                AhmedabadMarathonRegistration emp = new AhmedabadMarathonRegistration();

                string id = Session["uid"].ToString();
                var removecount = MarathonDonation.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == id).ToList().Count();
                emp = MarathonDonation.AhmedabadMarathonRegistrations.SingleOrDefault(val => val.UserId.ToString() == id && val.PaymentStatus == "pending");
                //val.RaceLocation == location

                if (emp != null)
                {
                    if (emp.PaymentStatus == "Pending")
                    {
                        emp.RaceLocation = null;
                        emp.TimeSlot = null;
                        emp.RunDate = null;
                        emp.RaceDistance = null;
                        emp.ReferenceCode = null;
                        MarathonDonation.SubmitChanges();
                        flagHideRaceInfo = "true";
                    }


                    if (removecount > 1)
                    {

                        rdb.AhmedabadMarathonRegistrations.Attach(emp);
                        rdb.AhmedabadMarathonRegistrations.DeleteOnSubmit(emp);
                        rdb.SubmitChanges();

                    }
                    else
                    {
                        emp.RaceLocation = null;
                        emp.TimeSlot = null;
                        emp.RunDate = null;
                        emp.RaceDistance = null;
                        emp.ReferenceCode = null;
                        MarathonDonation.SubmitChanges();
                        flagHideRaceInfo = "true";
                    }

                }
                else if (emp == null)
                {
                    flagHideRaceInfo = "true";
                }
            }

            return Json(new { result = "Redirect", url = "/WelcomeRunner", flag = flagHideRaceInfo });
        }

        // AhmedabadMarathonRegistrationDataContext DonateInfo = new AhmedabadMarathonRegistrationDataContext();
        //Remove Data 

        public ActionResult WelcomeRunnerAmountInfo()
        {

            if (Session["uid"] == null)
            {
                Session["errmsg"] = "Some thing has been wrong, Please try again.";
                return Redirect("/Register");
            }

            try
            {
                WelcomeRunnerDonate WelcomeRunnerLists = new WelcomeRunnerDonate();

                List<Donate> donateList = new List<Donate>();
                string Userid = Session["id"].ToString();

                var TotalCurrentYearRecord = from rc in DonateInfo.AhmedabadMarathonDonations
                                             where ((rc.PaymentStatus == "successful") && (rc.AffiliateCode == Userid.ToString()))
                                             select rc;
                if (TotalCurrentYearRecord.Count() > 0)
                {
                    try
                    {
                        var Todaytotalcollectionrecord = (from rc in TotalCurrentYearRecord
                                                          select rc.Amount).Sum();
                        if (Todaytotalcollectionrecord != null)
                        {
                            ViewBag.Message = Todaytotalcollectionrecord;
                        }



                        foreach (var item in TotalCurrentYearRecord)
                        {
                            Donate obj = new Donate();
                            obj.Name = item.Name;
                            obj.AffiliateCode = item.AffiliateCode;
                            obj.EmailId = item.EmailId;
                            obj.Amount = item.Amount.ToString();
                            donateList.Add(obj);

                        }
                        WelcomeRunnerLists.WelcomeRunnerList = donateList;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    // var data = stdntcntxt.student;
                    return View(WelcomeRunnerLists.WelcomeRunnerList.ToList());
                }
                else
                {
                    ViewBag.Message = 0;
                    WelcomeRunnerLists.WelcomeRunnerList = donateList;
                    return View(WelcomeRunnerLists.WelcomeRunnerList);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
            //return null;

        }

        private void GenrateShareImage(string uid)
        {
            try
            {
                AhmedabadMarathonRegistration emp = MarathonDonation.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == uid).FirstOrDefault();

                var filePath = Server.MapPath("/images/Marathon/Donation" + uid + ".jpg");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                Item parentItem = Sitecore.Context.Database.GetItem(new Data.ID("{9D0890E9-F4EB-47E9-BE82-73F11413500D}"));
                var itemInfo = parentItem.Children;

                Bitmap bitMapImage = new System.Drawing.Bitmap(Server.MapPath((@"/images/Marathon/DonationTemplate.png")));
                Graphics graphicImage = Graphics.FromImage(bitMapImage);

                Color StringColor = System.Drawing.ColorTranslator.FromHtml("#446aa8");
                foreach (var item in itemInfo.ToList())
                {
                    var Title = item.Fields["Title"].Value.ToString();
                    var Body = item.Fields["Body"].Value.ToString();
                    var summery = item.Fields["Summary"].Value.ToString();


                    if (Title.IndexOf("<ID>") > 0)
                    {
                        Title = Title.Replace("<ID>", emp.Id.ToString());
                    }
                    if (Title.IndexOf("<KM>") > 0)
                    {
                        Title = Title.Replace("<KM>", emp.RaceDistance.ToString());
                    }
                    graphicImage.DrawString(Title,
                    new Font("Arial", 20, FontStyle.Bold),
                    new SolidBrush(StringColor), new Point(int.Parse(Body), int.Parse(summery)));
                }


                bitMapImage.Save(Server.MapPath(@"/images/Marathon/Donation" + uid + ".jpg"), ImageFormat.Jpeg);
                graphicImage.Dispose();
                bitMapImage.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public ActionResult DonateSubmit()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DonateSubmit(Donate d)
        {
            try
            {
                Guid UserId = Guid.NewGuid();
                TempData["DonationUserId"] = UserId.ToString();
                TempData.Keep();
                var Url = Donation.DonationSubmit(d, UserId, Request.Url.ToString());
                if(Url.Equals("0"))
                {
                    d.Amount = "0";
                    return View(d);
                }
                if (Url.Equals("1"))
                {
                    d.reResponse = "Invalid Captcha";
                    return View(d);
                }
                return Redirect(Url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Redirect("/");
        }



        public ActionResult GetUserInfo(string Email, string OTP, string distance)
        {
            try
            {
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                string generatedOTP = repo.GetEmailOTP(Email);
                if (string.Equals(generatedOTP, OTP))
                {
                    if (repo.ParticipantStatus(Email) == "old")
                    {
                        RegistrationModel m = new RegistrationModel();
                        Session["uid"] = rdb.AhmedabadMarathonRegistrations.Where(a => a.Email.ToLower() == Email.ToLower() && a.RegistrationStatus.ToLower() == "successful" && a.Updated == null).FirstOrDefault().UserId;
                        Session["errmsg"] = "";

                        Session["distance"] = distance;
                        return Json(new { result = "Redirect", url = Url.Action("MarathonRegisteredInfo", "Marathon") });
                        //return Redirect("/MarathonRegisteredInfo");
                    }
                    else
                    {
                        Session["errmsg"] = "";
                        return Json(new { result = "new" });
                    }
                }
                return Json(new { result = "new" });
            }
            catch (Exception ex)
            {
                Log.Error("ParticipantStatus ", ex.Message);
                Session["errmsg"] = "";
                return Json(new { result = "new" });
            }
        }

        public ActionResult MarathonRegisterationInfo()
        {
            try
            {
                Log.Info("MarathonRegisterationInfo Get Start" , "");
                bindState();
                Session["GroupCart"] = null;
                Session["CurrentBibCount"] = 1;
                if (Session["message"] == null)
                {
                    Log.Info("MarathonRegisterationInfo Session message is null", "");
                    return Redirect("/Register");
                }

                RegistrationCheck info = (RegistrationCheck)Session["message"];
                Session["uid"] = null;
                if (info.Email == null)
                {
                    info.Email = "";
                }
                if (info.PhoneNumber == null)
                {
                    info.PhoneNumber = "";
                }
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                var datarecord = rdb.AhmedabadMarathonRegistrations.Where(a => (a.Email.ToLower() == info.Email.ToLower() || a.ContactNumber == info.PhoneNumber) && a.RegistrationStatus.ToLower() == "successful" && a.Updated == null).FirstOrDefault();

                RegistrationModel emp = new RegistrationModel();
                emp.StateList = ViewBag.state;
                //emp.RunDateList = new List<SelectListItem>();
                //emp.TimeSlotList = new List<SelectListItem>();
                //ID raceLocationID = new ID("{969F5F5A-8F91-415E-BDB7-7712AD3A43D2}");
                //var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                //emp.RaceLocation = raceLocation.Fields["Name"].Value;
                ID raceLocationID = new ID("{9D55601D-21DA-46AC-94B7-DCB0DE626927}");
                var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                emp.RaceLocation = raceLocation.Fields["Location"].Value;
                var location = emp.RaceLocation = raceLocation.Fields["Location"].Value;
                //emp.RaceLocation = m.RaceLocation;
                string RaceAmt = string.Empty;
                if (info.RunMode == "Physical")
                {
                    RaceAmt = MarathonHelper.GetRaceAmount(info.distance);
                }
                else if (info.RunMode == "Remote")
                {
                    RaceAmt = MarathonHelper.GetRemoteRaceAmount(info.distance);
                }

                if (datarecord != null)
                {
                    Session["uid"] = datarecord.UserId;
                    emp = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == datarecord.UserId).Select(val => new RegistrationModel()
                    {
                        //RaceLocation=val.RaceLocation,
                        //ReceDetailsLists = MarathonHelper.GetRaceDistance(),
                        BIBNumber = val.BIBNumber,
                        IsRaceDistanceChanged = val.IsRaceDistanceChanged == null ? false : val.IsRaceDistanceChanged,
                        RunMode = info.RunMode,
                        RaceDistance = info.distance,
                        RaceAmount = 0,
                        //ReferenceCode = val.ReferenceCode,
                        FirstName = val.FirstName,
                        LastName = val.LastName,
                        DateofBirth = val.DateofBirth.ToString(),
                        Email = info.Email,
                        ContactNumber = val.ContactNumber,
                        Nationality = val.Nationality,
                        Gender = val.Gender,
                        TShirtSize = val.TShirtSize,
                        NamePreferredonBIB = val.NamePreferredonBIB,
                        IdentityProofType = val.IdentityProofType,
                        IdentityProofNumber = val.IdentityProofNumber,

                        // IDCardAttachment = val.IDCardAttachment,
                        Country = val.Country,
                        State = val.State,
                        City = val.City,
                        Address = val.Address,
                        Pincode = val.Pincode,
                        EmergencyContactName = val.EmergencyContactName,
                        EmergencyContactRelationship = val.EmergencyContactRelationship,
                        EmergencyContactNumber = val.EmergencyContactNumber,
                        BloodGroup = val.BloodGroup,
                        ChronicIllness = val.ChronicIllness,
                        HeartAilment = val.HeartAilment,
                        AnyFaintingEpisodesinPast = val.AnyFaintingEpisodesinPast,
                        AnyOtherAilment = val.AnyOtherAilment,
                        AnyKnownAllergies = val.AnyKnownAllergies,
                        PayrollCompany = val.PayrollCompany,
                        EmployeeID = val.EmployeeID,
                        UnitStation = val.UnitStation,
                        PaymentStatus = val.PaymentStatus,
                        IdCardFilename = val.IDCardAttachment,
                        ShantigramIdProofFilename = val.ShantigramIdProof,
                        DefencePersonnel = val.DefencePersonnel,
                        RaceCertificateName = val.RaceCertificate,
                        DetailsOfFullHalfMarathon = val.DetailsOfFullHalfMarathon,
                        Updated = val.Updated == null ? false : true
                    }).FirstOrDefault();
                }
                else
                {
                    //emp.ReceDetailsLists = MarathonHelper.GetRaceDistance();
                    emp.Email = info.Email;
                    emp.RaceAmount = 0;
                    emp.RaceDistance = info.distance;
                    emp.DateofBirth = info.DateofBirth;
                    emp.ContactNumber = info.PhoneNumber;
                    emp.RunMode = info.RunMode;
                    emp.Updated = true;
                }

                Session["usermail"] = info.Email;
                Session["Emp"] = emp;
                //var DateTimeSlot = Sitecore.Context.Database.GetItem("{DD30F955-9DA8-43A7-A3C8-5B6AE6AE67F4}");
                Item RaceDetails = Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");

                //emp.RunDateList = DateTimeSlot.GetChildren().ToList().Select(x => new SelectListItem()
                //{
                //    Text = (Sitecore.DateUtil.ToServerTime(((DateField)x.Fields["RunDateText"]).DateTime)).ToString("dd/MM/yyyy"),
                //    Value = (Sitecore.DateUtil.ToServerTime(((DateField)x.Fields["RunDateText"]).DateTime)).ToString("dd/MM/yyyy")
                //}).ToList();
                //emp.TimeSlotList = DateTimeSlot.GetChildren().ToList().Select(x => new SelectListItem()
                //{
                //    Text = x.Fields["Time"].Value,
                //    Value = x.Fields["Text"].Value
                //}).ToList();
                if (emp.RunMode == "Physical")
                {
                    emp.ReceDetailsLists = MarathonHelper.GetRaceDistance();
                    if (!string.IsNullOrEmpty(emp.RaceDistance))
                    {
                        foreach (Item distance in RaceDetails.GetChildren())
                        {
                            if (distance.Fields["Distance"].Value == emp.RaceDistance && distance.HasChildren)
                            {
                                emp.RunDateList = distance.GetChildren().ToList().Select(x => new SelectListItem()
                                {
                                    Value = (Sitecore.DateUtil.ToServerTime(((DateField)x.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy"),
                                    Text = x.Fields["RunDateText"].Value
                                }).ToList();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(emp.RunDate) && !string.IsNullOrEmpty(emp.RaceDistance))
                    {
                        foreach (Item distance in RaceDetails.GetChildren())
                        {
                            if (distance.Fields["Distance"].Value == emp.RaceDistance && distance.HasChildren)
                            {
                                foreach (Item date in distance.GetChildren())
                                {
                                    if ((Sitecore.DateUtil.ToServerTime(((DateField)date.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy") == emp.RunDate)
                                    {
                                        emp.TimeSlotList = date.GetChildren().ToList().Select(x => new CustomSelectItem()
                                        {
                                            Text = x.Fields["Text"].Value,
                                            Value = x.Fields["Value"].Value,
                                            MaxAllowedCount = Int32.Parse(x.Fields["Count"].Value ?? "250")
                                        }).ToList();
                                        if (emp.RunMode == "Physical")
                                        {
                                            foreach (var times in emp.TimeSlotList)
                                            {
                                                var RunList = rdb.AhmedabadMarathonRegistrations.Where(x => x.RunMode == emp.RunMode && x.RaceDistance == emp.RaceDistance && x.RegistrationStatus == "successful" && x.Updated != null && x.RunDate == DateTime.Parse(emp.RunDate.ToString(), CultureInfo.GetCultureInfo("en-gb")) && x.TimeSlot == times.Value);
                                                if (RunList.Count() >= times.MaxAllowedCount)
                                                {
                                                    times.Disabled = true;
                                                    times.Text = times.Value;
                                                    //  times.Text = times.Text + " (slot full)";
                                                }
                                                else
                                                {
                                                    times.Text = times.Value;
                                                    // times.Text = times.Text + " (slot left:" + (times.MaxAllowedCount - RunList.Count()) + ")";
                                                }
                                            }
                                            //List<CustomSelectItem> tslot = new List<CustomSelectItem>();
                                            //foreach (var timeslot in emp.TimeSlotList)
                                            //{
                                            //    tslot.Add(new CustomSelectItem { Text = timeslot.Text, Value = timeslot.Value, Disabled = timeslot.Disabled });
                                            //}
                                            ViewData["TimeSlotList"] = emp.TimeSlotList;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (info.RunMode == "Remote")
                {
                    emp.ReceDetailsLists = MarathonHelper.GetRemoteRaceDistance();
                }
                //foreach (Item DateList in DateTimeSlot.GetChildren())
                //{
                //    SelectListItem RunDateList = new SelectListItem();
                //    var DateSlot = (DateField)DateList.Fields["RunDateText"];
                //    DateTime DateValue = Sitecore.DateUtil.ToServerTime(DateSlot.DateTime);
                //    RunDateList.Text = DateValue.Date.ToString("dd/MM/yyyy");
                //    RunDateList.Value = DateValue.Date.ToString("dd/MM/yyyy");
                //    emp.RunDateList.Add(RunDateList);

                //    SelectListItem TimeSlotList = new SelectListItem();
                //    TimeSlotList.Text = DateList.Fields["Time"].Value;
                //    TimeSlotList.Value = DateList.Fields["Text"].Value;
                //    emp.TimeSlotList.Add(TimeSlotList);
                //}
                bindState();
                emp.StateList = ViewBag.state;
                return View(emp);
            }
            catch (Exception ex)
            {
                Log.Error("MarathonRegisterationInfo Get exception" + ex, "");
                return Redirect("/Register");
            }
        }
        public void bindState()
        {
            AhmedabadMarathonRegistrationDataContext stateTable = new AhmedabadMarathonRegistrationDataContext();
            var state = stateTable.MarathonStates.ToList();
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select State", Value = "0" });

            foreach (var m in state)
            {
                li.Add(new SelectListItem { Text = m.statename, Value = m.stateid.ToString(), Selected = true });
                ViewBag.state = li;
            }
        }
        [HttpPost]
        public JsonResult getCity(int id)
        {
            AhmedabadMarathonRegistrationDataContext cityTable = new AhmedabadMarathonRegistrationDataContext();
            var ddlCity = cityTable.MarathonCities.Where(x => x.stateid == id).ToList();
            List<SelectListItem> licities = new List<SelectListItem>();

            licities.Add(new SelectListItem { Text = "Select City", Value = "0" });
            if (ddlCity != null)
            {
                foreach (var x in ddlCity)
                {
                    licities.Add(new SelectListItem { Text = x.CityName, Value = x.Cityid.ToString(), Selected = true });
                }
            }
            return Json(new SelectList(licities, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarathonRegisterationInfo(RegistrationModel m)
        {
            try
            {
                Log.Info("MarathonRegisterationInfo Post Start", "");
                bindState();
                m.StateList = ViewBag.state;
                RegistrationCheck registrationModel = Session["message"] as RegistrationCheck;
                Item RaceDetails = Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
                if (m.RunMode == "Physical")
                {
                    Log.Info("MarathonRegisterationInfo Post m.RunMode is Physical", "");

                    m.ReceDetailsLists = MarathonHelper.GetRaceDistance();
                    if (!string.IsNullOrEmpty(m.RaceDistance))
                    {
                        foreach (Item distance in RaceDetails.GetChildren())
                        {
                            if (distance.Fields["Distance"].Value == m.RaceDistance)
                            {
                                m.RunDateList = distance.GetChildren().ToList().Select(x => new SelectListItem()
                                {
                                    Value = (Sitecore.DateUtil.ToServerTime(((DateField)x.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy"),
                                    Text = x.Fields["RunDateText"].Value
                                }).ToList();
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(m.RunDate) && !string.IsNullOrEmpty(m.RaceDistance))
                    {
                        foreach (Item distance in RaceDetails.GetChildren())
                        {
                            if (distance.Fields["Distance"].Value == m.RaceDistance)
                            {
                                foreach (Item date in distance.GetChildren())
                                {
                                    if ((Sitecore.DateUtil.ToServerTime(((DateField)date.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy") == m.RunDate)
                                    {
                                        m.TimeSlotList = date.GetChildren().ToList().Select(x => new CustomSelectItem()
                                        {
                                            Text = x.Fields["Text"].Value,
                                            Value = x.Fields["Value"].Value,
                                            MaxAllowedCount = Int32.Parse(x.Fields["Count"].Value ?? "250")
                                        }).ToList();
                                        if (m.RunMode == "Physical")
                                        {
                                            AhmedabadMarathonRegistrationDataContext AAMrdb = new AhmedabadMarathonRegistrationDataContext();
                                            foreach (var times in m.TimeSlotList)
                                            {
                                                var RunList = AAMrdb.AhmedabadMarathonRegistrations.Where(x => x.RunMode == m.RunMode && x.RaceDistance == m.RaceDistance && x.RegistrationStatus == "successful" && x.Updated != null && x.RunDate == DateTime.Parse(m.RunDate.ToString(), CultureInfo.GetCultureInfo("en-gb")) && x.TimeSlot == times.Value);
                                                if (RunList.Count() >= times.MaxAllowedCount)
                                                {
                                                    times.Disabled = true;
                                                    times.Text = times.Value;
                                                    //    times.Text = times.Text + " (slot full)";
                                                }
                                                else
                                                {
                                                    //   times.Text = times.Text + " (slot left:" + (times.MaxAllowedCount - RunList.Count()) + ")";
                                                    times.Text = times.Value;
                                                }
                                            }
                                            //List<CustomSelectItem> tslot = new List<CustomSelectItem>();
                                            //foreach (var timeslot in m.TimeSlotList)
                                            //{
                                            //    tslot.Add(new CustomSelectItem { Text = timeslot.Text, Value = timeslot.Value, Disabled = timeslot.Disabled });
                                            //}
                                            ViewData["TimeSlotList"] = m.TimeSlotList;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(m.RunMode))
                {
                    Log.Info("MarathonRegisterationInfo Post m.RunMode is not equal to null", "");

                    var Physical = Sitecore.Context.Database.GetItem("{6B0DCBFE-D405-424B-9A93-4AD52224109B}");
                    var Remote = Sitecore.Context.Database.GetItem("{1675BF07-AFA0-4325-A242-D58A01534E54}");
                    if (m.RunMode == "Physical")
                    {
                        Log.Info("MarathonRegisterationInfo Post m.RunMode is Physical", "");
                        var agecheck = (DateField)Physical.Fields["AgeCheckDate"];
                        DateTime PhysicalDate = Sitecore.DateUtil.ToServerTime(agecheck.DateTime);
                        DateTime getDOB = DateTime.ParseExact(m.DateofBirth?.ToString(), "dd/MM/yyyy", CultureInfo.GetCultureInfo("en-gb"));

                        if (getDOB > PhysicalDate)
                        {
                            Log.Error("MarathonRegisterationInfo Post | Age should be"+ Physical.Fields["AgeLimit"].Value + "+ on " + PhysicalDate.Date.AddYears(18).ToString("dd/MM/yyyy"), "");
                            Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/ErrorDOB", "Age should be " + Physical.Fields["AgeLimit"].Value + "+ on " + PhysicalDate.Date.AddYears(18).ToString("dd/MM/yyyy"));
                            //Session["RegInfo"] = m;
                            return View(m);
                        }
                        AhmedabadMarathonRegistrationDataContext AAMrdb = new AhmedabadMarathonRegistrationDataContext();
                        //if (!m.TimeSlotList.Any(x => x.Value == m.TimeSlot))
                        //{
                        //    Session["Regerrmsg1"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/Invalid Slot", "Invalid time slot");
                        //    //Session["RegInfo"] = m;
                        //    return View(m);
                        //}
                        //foreach (var times in m.TimeSlotList)
                        //{
                        //    if (times.Value == m.TimeSlot)
                        //    {
                        //        var RunList = AAMrdb.AhmedabadMarathonRegistrations.Where(x => x.RunMode == m.RunMode && x.RaceDistance == m.RaceDistance && x.RegistrationStatus == "successful" && x.Updated != null && x.RunDate == DateTime.Parse(m.RunDate.ToString(), CultureInfo.GetCultureInfo("en-gb")) && x.TimeSlot == times.Value);
                        //        if (RunList.Count() >= times.MaxAllowedCount)
                        //        {
                        //            Session["Regerrmsg1"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/full Slot", "Selected slot is full, please select any available time slot");
                        //            m.TimeSlot = "";
                        //            return View(m);
                        //        }
                        //    }
                        //}
                    }
                    else if (m.RunMode == "Remote")
                    {
                        Log.Info("MarathonRegisterationInfo Post m.RunMode is Remote", "");
                        var agecheck = (DateField)Remote.Fields["AgeCheckDate"];
                        var Phyagecheck = (DateField)Physical.Fields["AgeCheckDate"];
                        DateTime PhysicalDate = Sitecore.DateUtil.ToServerTime(Phyagecheck.DateTime);
                        DateTime RemoteDate = Sitecore.DateUtil.ToServerTime(agecheck.DateTime);
                        DateTime getDOB = DateTime.Parse(m.DateofBirth.ToString(), CultureInfo.GetCultureInfo("en-gb"));
                        if (m.RaceDistance != null && (m.RaceDistance == "42.195KM" || m.RaceDistance == "21.097KM"))
                        {
                            if (getDOB > PhysicalDate)
                            {
                                Log.Error("MarathonRegisterationInfo Post | 42KM and 21KM | Age should be " + Physical.Fields["AgeLimit"].Value + "+ on " + PhysicalDate.Date.AddYears(18).ToString("dd/MM/yyyy"),"");
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/ErrorDOB", "Age should be " + Physical.Fields["AgeLimit"].Value + "+ on " + PhysicalDate.Date.AddYears(18).ToString("dd/MM/yyyy"));
                                Session["RegInfo"] = m;
                                return Redirect("/Register");
                            }
                        }
                        else if (m.RaceDistance != null && m.RaceDistance == "10KM")
                        {
                            if (getDOB > RemoteDate.AddYears(-2))
                            {
                                Log.Error("MarathonRegisterationInfo Post | 10KM | Age should be 12 on"+RemoteDate.Date.AddYears(10).ToString("dd/MM/yyyy"), "");
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/ErrorDOB", "Age should be " + "12" + "+ on " + RemoteDate.Date.AddYears(10).ToString("dd/MM/yyyy"));
                                Session["RegInfo"] = m;
                                return Redirect("/Register");
                            }
                        }
                        else
                        {
                            if (getDOB > RemoteDate)
                            {
                                Log.Error("MarathonRegisterationInfo Post | 5KM | Age should be" + Remote.Fields["AgeLimit"].Value + "+ on " + RemoteDate.Date.AddYears(10).ToString("dd/MM/yyyy"), "");
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/ErrorDOB", "Age should be " + Remote.Fields["AgeLimit"].Value + "+ on " + RemoteDate.Date.AddYears(10).ToString("dd/MM/yyyy"));
                                Session["RegInfo"] = m;
                                return Redirect("/Register");
                            }
                        }
                    }
                }
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
                AhmedabadMarathonRegistration emp = new AhmedabadMarathonRegistration();

                // m.Email = Session["usermail"].ToString();
                if (m.RunMode == "Physical")
                {
                    m.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(m.RaceDistance));
                }
                else if (m.RunMode == "Remote")
                {
                    m.RaceAmount = decimal.Parse(MarathonHelper.GetRemoteRaceAmount(m.RaceDistance));
                }
                DateTime getDateOfBirth = DateTime.Now;
                if (Session["uid"] != null)
                {
                    Log.Info("MarathonRegisterationInfo Post Session is not equal to null", "");

                    if (ModelState.IsValid)
                    {
                        Log.Info("MarathonRegisterationInfo Post Session is not equal to null. Model Validation Pass", "");
                        if (m.RunType == "Charity")
                        {
                            m.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(m.RaceDistance));
                            m.DonationAmount = m.DonationAmount - m.RaceAmount;
                        }
                        else
                        {
                            m.DonationAmount = 0;
                        }
                        string id = Session["uid"].ToString();
                        emp = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == id).Single<AhmedabadMarathonRegistration>();
                        try
                        {
                            if (!string.IsNullOrEmpty(m.DateofBirth))
                            {
                                getDateOfBirth = DateTime.Parse(m.DateofBirth.ToString(), CultureInfo.GetCultureInfo("en-gb"));
                            }
                            if (m.RaceDistance != emp.RaceDistance)
                            {
                                emp.IsRaceDistanceChanged = true;
                                emp.BIBNumber = repo.GenerateRandomBIBNumber(m.RaceDistance);
                            }
                            else
                            {
                                emp.IsRaceDistanceChanged = false;
                                emp.BIBNumber = emp.BIBNumber;
                            }
                            emp.ReferenceCode = m.ReferenceCode;
                            emp.FirstName = m.FirstName;
                            emp.LastName = m.LastName;
                            emp.RaceDistance = m.RaceDistance;
                            emp.RunType = m.RunType;
                            emp.RaceAmount = m.RaceAmount;
                            emp.DateofBirth = getDateOfBirth;
                            DateTime currentDate = DateTime.Parse(DateTime.Now.ToString());
                            TimeSpan timespan = currentDate.Subtract(getDateOfBirth);
                            emp.Age = ((int)(timespan.TotalDays) / 365).ToString();
                            m.Age = emp.Age;
                            emp.Email = m.Email;
                            emp.ContactNumber = m.ContactNumber;
                            emp.Nationality = m.Nationality;
                            emp.Gender = m.Gender;
                            emp.TShirtSize = m.TShirtSize;
                            emp.NamePreferredonBIB = m.NamePreferredonBIB;
                            emp.IdentityProofType = m.IdentityProofType;
                            emp.IdentityProofNumber = m.IdentityProofNumber;
                            emp.Country = m.Country;
                            if (m.State != null && !string.IsNullOrEmpty(m.State))
                            {
                                AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                                var state = StateTable.MarathonStates.Where(x => x.stateid.ToString() == m.State.ToString()).FirstOrDefault();
                                emp.State = state.statename.ToString();
                            }
                            if (m.City != null && !string.IsNullOrEmpty(m.City))
                            {
                                AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                                var city = StateTable.MarathonCities.Where(x => x.Cityid.ToString() == m.City.ToString()).FirstOrDefault();
                                emp.City = city.CityName.ToString();
                            }
                            emp.Address = m.Address;
                            emp.Pincode = m.Pincode;
                            emp.EmergencyContactName = m.EmergencyContactName;
                            emp.EmergencyContactRelationship = m.EmergencyContactRelationship;
                            emp.EmergencyContactNumber = m.EmergencyContactNumber;
                            emp.BloodGroup = m.BloodGroup;
                            emp.ChronicIllness = m.ChronicIllness;
                            emp.HeartAilment = m.HeartAilment;
                            emp.AnyFaintingEpisodesinPast = m.AnyFaintingEpisodesinPast;
                            emp.AnyOtherAilment = m.AnyOtherAilment;
                            emp.AnyKnownAllergies = m.AnyKnownAllergies;
                            emp.PayrollCompany = m.PayrollCompany;
                            emp.EmployeeID = m.EmployeeID;
                            emp.UnitStation = m.UnitStation;
                            emp.FinalAmount = m.RaceAmount;
                            emp.DonationAmount = m.DonationAmount;
                            emp.PANNumber = m.PANNumber;
                            emp.TaxExemptionCause = m.TaxExemptionCause;
                            emp.TaxExemptionCertificate = m.TaxExemptionCertificate.ToString();
                            emp.RunMode = m.RunMode;
                            //emp.RunDate = DateTime.Parse(m.RunDate.ToString(), CultureInfo.GetCultureInfo("en-gb"));
                            emp.TimeSlot = m.TimeSlot;
                            if (!string.IsNullOrEmpty(m.DetailsOfFullHalfMarathon))
                            {
                                emp.DetailsOfFullHalfMarathon = m.DetailsOfFullHalfMarathon;
                            }
                            emp.EmployeeID = m.EmployeeID;
                            emp.EmployeeEmailId = m.EmployeeEmailId;
                            emp.Vaccinationted = m.Vaccinationted;
                            emp.DefencePersonnel = m.DefencePersonnel;
                            m.FinalAmount = m.RaceAmount;
                            emp.NamePreferredonBIB = m.FirstName.ToUpper() + " " + m.LastName.ToUpper();
                            emp.Updated = false;
                            emp.PaymentStatus = "pending";
                            emp.RegistrationStatus = "Pending";
                            emp.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                            //ID raceLocationID = new ID("{969F5F5A-8F91-415E-BDB7-7712AD3A43D2}");
                            //var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                            //emp.RaceLocation = raceLocation.Fields["Name"].Value;
                            ID raceLocationID = new ID("{969F5F5A-8F91-415E-BDB7-7712AD3A43D2}");
                            var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                            emp.RaceLocation = raceLocation.Fields["Name"].Value;
                            //emp.RaceLocation = m.RaceLocation;



                            if (m.VaccinationCertificate != null)
                            {
                                BlobAPIService blobAPIService = new BlobAPIService();
                                emp.IDCardAttachment = blobAPIService.BlobAPI(m.VaccinationCertificate);
                            }
                            if (m.IDCardAttachment != null)
                            {
                                BlobAPIService blobAPIService = new BlobAPIService();
                                emp.IDCardAttachment = blobAPIService.BlobAPI(m.IDCardAttachment);
                            }
                            if (m.ShantigramIdProof != null)
                            {
                                BlobAPIService blobAPIService = new BlobAPIService();
                                emp.IDCardAttachment = blobAPIService.BlobAPI(m.ShantigramIdProof);
                            }
                            if (!string.IsNullOrEmpty(m.ReferenceCode))
                            {
                                ApplyCodeResponse codeResponse = RegistrationFormValidation.ApplyCouponCode(m.ReferenceCode, m.RaceAmount, m.RunType, m.EmployeeID);
                                emp.PaymentStatus = codeResponse.PaymentStatus;
                                emp.RegistrationStatus = codeResponse.RegistrationStatus;
                                emp.DiscountRate = codeResponse.DiscountRate;
                                emp.AmountReceived = codeResponse.AmountReceived;
                                emp.FinalAmount = codeResponse.FinalAmount;
                                m.FinalAmount = codeResponse.FinalAmount;
                            }
                            else
                            {
                                emp.ReferenceCode = "";
                            }
                            emp.FinalAmount = emp.FinalAmount + emp.DonationAmount;
                            m.FinalAmount = decimal.Parse(emp.FinalAmount.ToString());

                            TempData["USerId"] = emp.UserId.ToString();
                            Session["uid"] = emp.UserId.ToString();
                            TempData.Keep();
                            marathonDb.SubmitChanges();
                            //sendEmail(m);
                            sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");

                            if (registrationModel.Count == "1")
                            {
                                if (emp.PaymentStatus.ToLower() == "pending")
                                {
                                    Log.Info("MarathonRegisterationInfo Post Session is not equal to null. PaymentStatus is pending", "");
                                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                                    {
                                        m.OrderId = (Guid.NewGuid()).ToString();
                                        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                                        {
                                            UserId = (emp.UserId).ToString(),
                                            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                                            Id = Guid.NewGuid(),
                                            Amount = System.Convert.ToString(emp.FinalAmount),
                                            Email = m.Email,
                                            Mobile = m.ContactNumber,
                                            UserType = "Guest",
                                            GatewayType = "Insta-Mojo",
                                            Created_Date = System.DateTime.Now,
                                            RequestTime = System.DateTime.Now,
                                            CreatedBy = m.FirstName + " " + m.LastName,
                                            AccountNumber = m.ReferenceCode,
                                            OrderId = m.OrderId,
                                            PaymentType = "Token Amount",
                                            ResponseMsg = Request.Url.ToString()
                                        };

                                        marathonDb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                                        marathonDb.SubmitChanges();
                                        //   sendEmail(m, "AhmedabadMarathonPaymentHistories - pending");
                                        PaymentService objPaymentService = new PaymentService();
                                        ResultPayment Objresult = new ResultPayment();
                                        Objresult = objPaymentService.Payment(m);
                                        if (Objresult.IsSuccess)
                                        {
                                            Log.Info("MarathonRegisterationInfo Post Session is not equal to null. PymentPass Objresult is success", "");
                                            //  sendEmail(m, "payment sucess1.->" + Objresult.Message);
                                            return Redirect(Objresult.Message);
                                        }
                                        else
                                        {
                                            Log.Info("MarathonRegisterationInfo Post Session is not equal to null. PymentPass Objresult is failed", "");
                                            ViewBag.ErrorMessage = Objresult.Message;
                                            return Redirect("/registration-failed");
                                            //   sendEmail(m, Objresult.Message);
                                        }
                                        //   sendEmail(m, "final..return");
                                        return View(m);
                                    }
                                }
                                else
                                {
                                    Log.Info("MarathonRegisterationInfo Post Session is not equal to null. PaymentStatus is not pending", "");
                                    //updated code to send email
                                    m.Useridstring = emp.UserId;
                                    sendEmail(m);
                                    sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");
                                    SMSOTP.registrationconfirmation(m);
                                    return Redirect("/registration-thankyou");
                                }
                            }
                            var currentCount = int.Parse(Session["CurrentBibCount"].ToString());
                            Session["CurrentBibCount"] = currentCount++;
                            DataTable dt = new DataTable();
                            if (Session["GroupCart"] != null)
                            {
                                dt = Session["GroupCart"] as DataTable;
                            }
                            else
                            {
                                dt.Columns.Add("RaceDistance");
                                dt.Columns.Add("FirstName");
                                dt.Columns.Add("LastName");
                                dt.Columns.Add("Email");
                                dt.Columns.Add("ContactNumber");
                                dt.Columns.Add("FinalAmount");
                                dt.Columns.Add("Userid");
                                dt.Columns.Add("RaceAmount");
                                dt.Columns.Add("DiscountRate");
                                dt.Columns.Add("PaymentStatus");
                                dt.Columns.Add("RaceType");
                                dt.Columns.Add("DonationAmount");
                            }

                            DataRow dr = dt.NewRow();

                            dr[0] = emp.RaceDistance;
                            dr[1] = emp.FirstName;
                            dr[2] = emp.LastName;
                            dr[3] = emp.Email;
                            dr[4] = emp.ContactNumber;
                            dr[5] = emp.FinalAmount;
                            dr[6] = emp.UserId;
                            dr[7] = emp.RaceAmount;
                            dr[8] = emp.DiscountRate;
                            dr[9] = emp.PaymentStatus;
                            dr[10] = m.RunType;
                            dr[11] = m.DonationAmount;
                            dt.Rows.Add(dr);
                            dt.NewRow();
                            Session["PageRefresh"] = "false";
                            Session["GroupCart"] = dt;
                            return Redirect("/GroupRegister");
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error in MarathonRegisterationInfo Session is not equal to null post controller", ex, this);
                        }

                    }
                    else
                    {
                        Log.Info("MarathonRegisterationInfo Post Session is not equal to null. Model Validation Failed", "");
                        return View(m);
                    }
                }
                else
                {
                    Log.Info("MarathonRegisterationInfo Post Session is  equal to null", "");
                    if (ModelState.IsValid)
                    {
                        Log.Info("MarathonRegisterationInfo Post Session is  equal to null. Model Validation Pass.", "");
                        try
                        {
                            var existingrecord = rdb.AhmedabadMarathonRegistrations.Where(a => a.ContactNumber == m.ContactNumber && a.Updated != null).FirstOrDefault();
                            if (existingrecord != null)
                            {
                                Log.Info("MarathonRegisterationInfo Post Session is  equal to null.You are already registered. Kindly login to update details.", "");
                                Session.Clear();
                                Session["Regerrmsg"] = DictionaryPhraseRepository.Current.Get("/Marathon/Register/Recorded", "You are already registered. Kindly login to update details.");
                                return Redirect("/Register");
                            }
                            if (!string.IsNullOrEmpty(m.DateofBirth))
                            {
                                getDateOfBirth = DateTime.Parse(m.DateofBirth.ToString(), CultureInfo.GetCultureInfo("en-gb"));
                            }
                            if (m.RunType == "Charity")
                            {
                                m.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(m.RaceDistance));
                                m.DonationAmount = m.DonationAmount - m.RaceAmount;
                            }
                            else
                            {
                                m.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(m.RaceDistance));
                                m.DonationAmount = 0;
                            }
                            emp.BIBNumber = repo.GenerateRandomBIBNumber(m.RaceDistance);
                            emp.IsRaceDistanceChanged = false;
                            emp.ReferenceCode = m.ReferenceCode;
                            emp.FirstName = m.FirstName;
                            emp.LastName = m.LastName;
                            emp.RaceDistance = m.RaceDistance;
                            emp.RunType = m.RunType;
                            emp.RaceAmount = m.RaceAmount;
                            emp.DateofBirth = getDateOfBirth;
                            emp.Email = m.Email;
                            emp.ContactNumber = m.ContactNumber;
                            emp.Nationality = m.Nationality;
                            emp.Gender = m.Gender;
                            emp.TShirtSize = m.TShirtSize;
                            emp.NamePreferredonBIB = m.NamePreferredonBIB;
                            emp.IdentityProofType = m.IdentityProofType;
                            emp.IdentityProofNumber = m.IdentityProofNumber;
                            emp.Country = m.Country;
                            if (m.State != null && !string.IsNullOrEmpty(m.State))
                            {
                                AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                                var state = StateTable.MarathonStates.Where(x => x.stateid.ToString() == m.State.ToString()).FirstOrDefault();
                                emp.State = state.statename.ToString();
                            }
                            if (m.City != null && !string.IsNullOrEmpty(m.City))
                            {
                                AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                                var city = StateTable.MarathonCities.Where(x => x.Cityid.ToString() == m.City.ToString()).FirstOrDefault();
                                emp.City = city.CityName.ToString();
                            }
                            emp.Address = m.Address;
                            emp.Pincode = m.Pincode;
                            emp.EmergencyContactName = m.EmergencyContactName;
                            emp.EmergencyContactRelationship = m.EmergencyContactRelationship;
                            emp.EmergencyContactNumber = m.EmergencyContactNumber;
                            emp.BloodGroup = m.BloodGroup;
                            emp.ChronicIllness = m.ChronicIllness;
                            emp.HeartAilment = m.HeartAilment;
                            emp.AnyFaintingEpisodesinPast = m.AnyFaintingEpisodesinPast;
                            emp.AnyOtherAilment = m.AnyOtherAilment;
                            emp.AnyKnownAllergies = m.AnyKnownAllergies;
                            emp.PayrollCompany = m.PayrollCompany;
                            emp.EmployeeID = m.EmployeeID;
                            emp.EmployeeEmailId = m.EmployeeEmailId;
                            emp.UnitStation = m.UnitStation;
                            emp.FinalAmount = m.RaceAmount;
                            emp.RunMode = m.RunMode;
                            if (!string.IsNullOrEmpty(m.DetailsOfFullHalfMarathon))
                            {
                                emp.DetailsOfFullHalfMarathon = m.DetailsOfFullHalfMarathon;
                            }
                            emp.TimeSlot = m.TimeSlot;
                            emp.Vaccinationted = m.Vaccinationted;
                            emp.NamePreferredonBIB = m.FirstName.ToUpper() + " " + m.LastName.ToUpper();
                            emp.Updated = true;
                            emp.DefencePersonnel = m.DefencePersonnel;
                            emp.PaymentStatus = "pending";
                            m.FinalAmount = m.RaceAmount;
                            emp.DonationAmount = m.DonationAmount;
                            emp.PANNumber = m.PANNumber;
                            emp.TaxExemptionCause = m.TaxExemptionCause;
                            emp.TaxExemptionCertificate = m.TaxExemptionCertificate.ToString();
                            emp.RegistrationStatus = "Successful";
                            DateTime currentDate = DateTime.Parse(DateTime.Now.ToString());
                            TimeSpan timespan = currentDate.Subtract(getDateOfBirth);
                            emp.Age = ((int)(timespan.TotalDays) / 365).ToString();
                            m.Age = emp.Age;
                            emp.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                            m.Userid = Guid.NewGuid();
                            emp.UserId = m.Userid.ToString();
                            emp.RaceLocation = m.RaceLocation;
                            emp.EmployeeID = m.EmployeeID;
                            if (m.VaccinationCertificate != null)
                            {
                                BlobAPIService blobAPIService = new BlobAPIService();
                                emp.IDCardAttachment = blobAPIService.BlobAPI(m.VaccinationCertificate);
                            }
                            if (m.IDCardAttachment != null)
                            {
                                BlobAPIService blobAPIService = new BlobAPIService();
                                emp.IDCardAttachment = blobAPIService.BlobAPI(m.IDCardAttachment);
                            }
                            if (m.ShantigramIdProof != null)
                            {
                                BlobAPIService blobAPIService = new BlobAPIService();
                                emp.IDCardAttachment = blobAPIService.BlobAPI(m.ShantigramIdProof);
                            }
                            if (m.RaceCertificate != null)
                            {
                                BlobAPIService blobAPIService = new BlobAPIService();
                                emp.IDCardAttachment = blobAPIService.BlobAPI(m.RaceCertificate);
                            }
                            #region payment
                            if (!string.IsNullOrEmpty(m.ReferenceCode))
                            {
                                ApplyCodeResponse codeResponse = RegistrationFormValidation.ApplyCouponCode(m.ReferenceCode, m.RaceAmount, m.RunType, m.EmployeeID);
                                emp.PaymentStatus = codeResponse.PaymentStatus;
                                emp.RegistrationStatus = codeResponse.RegistrationStatus;
                                emp.DiscountRate = codeResponse.DiscountRate;
                                emp.AmountReceived = codeResponse.AmountReceived;
                                emp.FinalAmount = codeResponse.FinalAmount;
                                m.FinalAmount = codeResponse.FinalAmount;
                            }
                            else
                            {
                                emp.ReferenceCode = "";
                            }
                            #endregion
                            emp.FinalAmount = emp.FinalAmount + emp.DonationAmount;
                            m.FinalAmount = decimal.Parse(emp.FinalAmount.ToString());
                            TempData["USerId"] = emp.UserId.ToString();
                            Session["uid"] = emp.UserId.ToString();
                            TempData.Keep();
                            marathonDb.AhmedabadMarathonRegistrations.InsertOnSubmit(emp);
                            marathonDb.SubmitChanges();

                            //sendEmail(m);
                            // sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");

                            if (registrationModel.Count == "1")
                            {
                                if (emp.PaymentStatus.ToLower() == "pending")
                                {
                                    Log.Info("MarathonRegisterationInfo Post Session is  equal to null. PaymentStatus is pending", "");
                                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                                    {
                                        m.OrderId = (Guid.NewGuid()).ToString();
                                        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                                        {
                                            UserId = (m.Userid).ToString(),
                                            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),

                                            Id = Guid.NewGuid(),
                                            Amount = System.Convert.ToString(emp.FinalAmount),
                                            Email = m.Email,
                                            Mobile = m.ContactNumber,
                                            UserType = "Guest",
                                            GatewayType = "Insta-Mojo",
                                            Created_Date = System.DateTime.Now,
                                            RequestTime = System.DateTime.Now,
                                            CreatedBy = m.FirstName + " " + m.LastName,
                                            AccountNumber = m.ReferenceCode,
                                            OrderId = m.OrderId,
                                            PaymentType = "Token Amount",
                                            ResponseMsg = Request.Url.ToString()
                                        };

                                        marathonDb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                                        marathonDb.SubmitChanges();
                                        //   sendEmail(m, "AhmedabadMarathonPaymentHistories - pending");
                                        PaymentService objPaymentService = new PaymentService();
                                        ResultPayment Objresult = new ResultPayment();
                                        Objresult = objPaymentService.Payment(m);
                                        if (Objresult.IsSuccess)
                                        {
                                            Log.Info("MarathonRegisterationInfo Post Session is  equal to null. Payment Object is Success", "");
                                            //  sendEmail(m, "payment sucess1.->" + Objresult.Message);
                                            return Redirect(Objresult.Message);
                                        }
                                        else
                                        {
                                            Log.Info("MarathonRegisterationInfo Post Session is  equal to null. Payment Object is not success", "");
                                            ViewBag.ErrorMessage = Objresult.Message;
                                            return Redirect("/registration-failed");
                                            //   sendEmail(m, Objresult.Message);
                                        }
                                        //   sendEmail(m, "final..return");
                                    }
                                }
                                else
                                {
                                    Log.Info("MarathonRegisterationInfo Post Session is  equal to null. PaymentStatus is not pending", "");
                                    //updated code to send email
                                    m.Useridstring = emp.UserId;
                                    sendEmail(m);
                                    sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");
                                    SMSOTP.registrationconfirmation(m);
                                    return Redirect("/registration-thankyou");
                                }
                            }
                            var currentCount = int.Parse(Session["CurrentBibCount"].ToString());
                            Session["CurrentBibCount"] = currentCount++;
                            DataTable dt = new DataTable();
                            if (Session["GroupCart"] != null)
                            {
                                dt = Session["GroupCart"] as DataTable;
                            }
                            else
                            {
                                dt.Columns.Add("RaceDistance");
                                dt.Columns.Add("FirstName");
                                dt.Columns.Add("LastName");
                                dt.Columns.Add("Email");
                                dt.Columns.Add("ContactNumber");
                                dt.Columns.Add("FinalAmount");
                                dt.Columns.Add("Userid");
                                dt.Columns.Add("RaceAmount");
                                dt.Columns.Add("DiscountRate");
                                dt.Columns.Add("PaymentStatus");
                                dt.Columns.Add("RaceType");
                                dt.Columns.Add("DonationAmount");
                            }

                            DataRow dr = dt.NewRow();

                            dr[0] = emp.RaceDistance;
                            dr[1] = emp.FirstName;
                            dr[2] = emp.LastName;
                            dr[3] = emp.Email;
                            dr[4] = emp.ContactNumber;
                            dr[5] = emp.FinalAmount;
                            dr[6] = emp.UserId;
                            dr[7] = emp.RaceAmount;
                            dr[8] = emp.DiscountRate;
                            dr[9] = emp.PaymentStatus;
                            dr[10] = m.RunType;
                            dr[11] = m.DonationAmount;
                            dt.Rows.Add(dr);
                            dt.NewRow();
                            Session["PageRefresh"] = "false";
                            Session["GroupCart"] = dt;
                            return Redirect("/GroupRegister");

                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error in MarathonRegisterationInfo Session is equal to null post controller", ex, this);
                        }

                    }
                    else
                    {
                        Log.Info("MarathonRegisterationInfo Post Session is  equal to null. Model Validation Failed.", "");
                        return View(m);
                    }
                }
                Session.Clear();
                Log.Error("MarathonRegisterationInfo Post | Something has been wrong. Please try again later", "");
                Session["Regerrmsg"] = "Something has been wrong. Please try again later.";
                return Redirect("/Register");
  
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Log.Error("MarathonRegisterationInfo Post exception" + ex, "");
                return Redirect("/Register");
            }
        }

        public JsonResult GETRunDateList(string Distance)
        {
            List<SelectListItem> RunDateList = new List<SelectListItem>();
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                Item RaceDetails = Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
                foreach (Item distance in RaceDetails.GetChildren())
                {
                    if (distance.Fields["Distance"].Value == Distance)
                    {
                        RunDateList = distance.GetChildren().ToList().Select(x => new SelectListItem()
                        {
                            Value = (Sitecore.DateUtil.ToServerTime(((DateField)x.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy"),
                            Text = x.Fields["RunDateText"].Value

                        }).ToList();

                        return Json(RunDateList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("GETRunDateList api call error in marathon: " + e.Message, this);
            }
            return Json(RunDateList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GETRunTimeList(string RunMode, string Distance, string RunDate)
        {
            List<TimeSlotList> RunTimeList = new List<TimeSlotList>();
            try
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                Item RaceDetails = Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
                foreach (Item distance in RaceDetails.GetChildren())
                {
                    if (distance.Fields["Distance"].Value == Distance)
                    {
                        foreach (Item date in distance.GetChildren())
                        {
                            if ((Sitecore.DateUtil.ToServerTime(((DateField)date.Fields["RunDateValue"]).DateTime)).ToString("dd/MMM/yyyy") == RunDate)
                            {
                                RunTimeList = date.GetChildren().ToList().Select(x => new TimeSlotList()
                                {
                                    Text = x.Fields["Text"].Value,
                                    Value = x.Fields["Value"].Value,
                                    MaxAllowedCount = Int32.Parse(x.Fields["Count"].Value ?? "250")
                                }).ToList();
                                if (RunMode == "Physical")
                                {
                                    AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();

                                    foreach (var times in RunTimeList)
                                    {
                                        var RunList = rdb.AhmedabadMarathonRegistrations.Where(x => x.RunMode == RunMode && x.RaceDistance == Distance && x.RegistrationStatus == "successful" && x.Updated != null && x.RunDate == DateTime.Parse(RunDate.ToString(), CultureInfo.GetCultureInfo("en-gb")) && x.TimeSlot == times.Value);
                                        if (RunList.Count() >= times.MaxAllowedCount)
                                        {
                                            times.Disabled = true;
                                            times.Text = times.Value;
                                            //times.Text = times.Text + " (slot full)";
                                        }
                                        else
                                        {
                                            times.Text = times.Value;
                                            // times.Text = times.Text + " (slot left:" + (times.MaxAllowedCount - RunList.Count()) + ")";
                                        }
                                    }
                                }
                            }
                        }
                        return Json(RunTimeList, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("GETRunDateList api call error in marathon: " + e.Message, this);
            }
            return Json(RunTimeList, JsonRequestBehavior.AllowGet); ;
        }
        public bool FileMIMEisValid(HttpPostedFileBase attchment)
        {
            {
                var mime = attchment.ContentType;

                if (mime == "application/pdf" || mime == "image/jpeg" || mime == "image/png")
                {
                    // upload the File because file is valid  
                    return true;
                }
                else
                {
                    //  file is Invalid  
                    return false;

                }
            }
        }

        AhmedabadMarathonRegistrationDataContext welcomeDB = new AhmedabadMarathonRegistrationDataContext();
        public ActionResult WelcomeRunnerConfirmationEmail()
        {
            ActionResult actionResult;
            string msg = "";
            var result = new { status = "0" };

            if (Session["uid"] == null)
            {
                result = new { status = "2" };
                actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            }
            //User id
            try
            {
                var Racedetails = Sitecore.Context.Database.GetItem("{0FAABDDF-9C8E-4283-B499-9853674E97D4}");
                string uid = Session["uid"].ToString();

                RegistrationModel emp = welcomeDB.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == uid && val.Updated != null).Select(val => new RegistrationModel()
                {
                    //ReceDetailsLists = MarathonHelper.GetRaceDistance(),
                    RegistrationStatus = val.RegistrationStatus,
                    Updated = bool.Parse(val.Updated.ToString()),
                    RaceDistance = val.RaceDistance.Trim(),
                    RaceAmount = (decimal)val.RaceAmount,
                    ReferenceCode = val.ReferenceCode,
                    FirstName = val.FirstName,
                    LastName = val.LastName,
                    DateofBirth = val.DateofBirth.ToString(),
                    Email = val.Email,
                    ContactNumber = val.ContactNumber,
                    Nationality = val.Nationality,
                    Gender = val.Gender,
                    RunMode = val.RunMode,
                    RunDate = (DateTime.Parse(val.RunDate.ToString())).ToString("dd/MMM/yyyy"),
                    TimeSlot = val.TimeSlot,
                    Vaccinationted = val.Vaccinationted,
                    VaccinationCertificateName = val.VaccinationCertificate,
                    TShirtSize = val.TShirtSize,
                    NamePreferredonBIB = val.NamePreferredonBIB,
                    IdentityProofType = val.IdentityProofType,
                    IdentityProofNumber = val.IdentityProofNumber,
                    // IDCardAttachment = val.IDCardAttachment,
                    Country = val.Country,
                    State = val.State,
                    City = val.City,
                    Address = val.Address,
                    Pincode = val.Pincode,
                    EmergencyContactName = val.EmergencyContactName,
                    EmergencyContactRelationship = val.EmergencyContactRelationship,
                    EmergencyContactNumber = val.EmergencyContactNumber,
                    BloodGroup = val.BloodGroup,
                    ChronicIllness = val.ChronicIllness,
                    HeartAilment = val.HeartAilment,
                    AnyFaintingEpisodesinPast = val.AnyFaintingEpisodesinPast,
                    AnyOtherAilment = val.AnyOtherAilment,
                    AnyKnownAllergies = val.AnyKnownAllergies,
                    PayrollCompany = val.PayrollCompany,
                    EmployeeID = val.EmployeeID,
                    UnitStation = val.UnitStation,
                    PaymentStatus = val.PaymentStatus,
                    IdCardFilename = val.IDCardAttachment,
                    ShantigramIdProofFilename = val.ShantigramIdProof,
                    DefencePersonnel = val.DefencePersonnel,
                    RaceCertificateName = val.RaceCertificate,
                    DetailsOfFullHalfMarathon = val.DetailsOfFullHalfMarathon,

                }).FirstOrDefault();
                //calling sendEmail
                sendEmail(emp, msg);

                result = new { status = "1" };
                actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                Log.Error(string.Format("{0}", 0), ex, this);
                actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            }
            return actionResult;

        }


        //API Code For Mobile App 
        public ActionResult GetRegisterdata(string test)
        {
            var strMessage = new { Message = string.Empty, MessageFlag = string.Empty };
            try
            {

                string contactno = Request.QueryString["contactno"] != null ? Request.QueryString["contactno"].ToString() : string.Empty;
                string email = Request.QueryString["email"] != null ? Request.QueryString["email"].ToString() : string.Empty;
                string dateofbirth = Request.QueryString["dateofbirth"] != null ? Request.QueryString["dateofbirth"].ToString() : string.Empty;

                if (string.IsNullOrEmpty(contactno))
                {
                    strMessage = new { Message = "Please enter valid Contact Number", MessageFlag = "F" };
                    return Json(new { HttpStatusCode.BadRequest, strMessage }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(email))
                {
                    strMessage = new { Message = "Please enter valid Email", MessageFlag = "F" };
                    return Json(new { HttpStatusCode.BadRequest, strMessage }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(dateofbirth))
                {
                    strMessage = new { Message = "Please enter valid D.O.B", MessageFlag = "F" };
                    return Json(new { HttpStatusCode.BadRequest, strMessage }, JsonRequestBehavior.AllowGet);
                }


                using (AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext())
                {
                    var RegData = dataContext.AhmedabadMarathonRegistrations.Where
                        (x => x.ContactNumber == contactno && x.Email == email && x.DateofBirth.ToString() == dateofbirth).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();

                    if (RegData != null)
                    {

                        strMessage = new { Message = " Registered Data Retrieved Successfully.", MessageFlag = "S" };

                        var RegisterData = new
                        {
                            BIBNumber = RegData.BIBNumber,
                            Name = RegData.FirstName + RegData.LastName,
                            ContactNumber = RegData.ContactNumber,
                            email = RegData.Email,
                            Address = RegData.Address,
                            CityCountry = RegData.City + ", " + RegData.Country,
                            RaceDetails = RegData.RunDate.ToString(),
                            RaceDistace = RegData.RaceDistance,
                            Tshirt = RegData.TShirtSize,
                            RegisterDetails = RegData.RegistrationStatus + ", " + RegData.FinalAmount,
                            Nationality = RegData.Gender + "," + RegData.Nationality,
                            PaymentDetails = RegData.PaymentStatus

                        };
                        return Json(new { HttpStatusCode.OK, RegisterData, strMessage }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        strMessage = new { Message = "Registered Data Not Found.", MessageFlag = "F" };
                        return Json(new { HttpStatusCode.NotFound, strMessage }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                strMessage = new { Message = "Please contact to backend administrators for this Error: " + ex.Message, MessageFlag = "F" };
                return Json(new { strMessage }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult Getpaymentdata(string test)
        {
            var strMessage = new { Message = string.Empty, MessageFlag = string.Empty };
            try
            {
                string contactno = Request.QueryString["contactno"] != null ? Request.QueryString["contactno"].ToString() : string.Empty;
                string email = Request.QueryString["email"] != null ? Request.QueryString["email"].ToString() : string.Empty;


                if (string.IsNullOrEmpty(contactno))
                {
                    strMessage = new { Message = "Please enter valid Contact Number", MessageFlag = "F" };
                    return Json(new { HttpStatusCode.BadRequest, strMessage }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(email))
                {
                    strMessage = new { Message = "Please enter valid Email", MessageFlag = "F" };
                    return Json(new { HttpStatusCode.BadRequest, strMessage }, JsonRequestBehavior.AllowGet);
                }


                if (!string.IsNullOrEmpty(contactno) && !string.IsNullOrEmpty(email))
                {
                    using (AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext())
                    {
                        var RegisterData = dataContext.AhmedabadMarathonRegistrations.Where(x => x.ContactNumber == contactno && x.Email == email).OrderByDescending(y => y.FormSubmitOn).FirstOrDefault();


                        if (RegisterData != null)
                        {
                            var RegisterPaymentData = dataContext.AhmedabadMarathonPaymentHistories.Where(y => y.Email == RegisterData.Email && y.UserId == RegisterData.UserId).OrderByDescending(y => y.Modified_Date).FirstOrDefault();

                            if (RegisterPaymentData != null)
                            {
                                var PaymentData = new
                                {
                                    RegisterEmail = RegisterPaymentData.Email,
                                    RegisterMobileNumber = RegisterPaymentData.Mobile,
                                    RegisterCreatedDate = RegisterPaymentData.Created_Date.ToString(),
                                    RegisterModifiedDate = RegisterPaymentData.Modified_Date.ToString(),
                                    PaymentAmount = RegisterPaymentData.Amount,
                                    Order = RegisterPaymentData.OrderId,
                                    TransactionId = RegisterPaymentData.TransactionId,
                                    PaymentStatus = RegisterData.PaymentStatus

                                };

                                strMessage = new { Message = " Payment Data Retrieved Successfully.", MessageFlag = "S" };

                                return Json(new { HttpStatusCode.OK, PaymentData, strMessage }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                strMessage = new { Message = "Payment Data Not Found.", MessageFlag = "F" };
                                return Json(new { HttpStatusCode.NotFound, strMessage }, JsonRequestBehavior.AllowGet);
                            }

                        }
                        else
                        {
                            strMessage = new { Message = "Registered Data Not Found.", MessageFlag = "F" };
                            return Json(new { HttpStatusCode.NotFound, strMessage }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
                else
                {
                    strMessage = new { Message = "Registered Data Not Found.", MessageFlag = "F" };
                    return Json(new { HttpStatusCode.NotFound, strMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                strMessage = new { Message = "Please contact to backend administrators for this Error: " + ex.Message, MessageFlag = "F" };
                return Json(new { strMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        /*July New Code*/
        public string SendSMSOTP(string Email, string SessionID)
        {
            try
            {
                Log.Info("Marathon SendSMSOTP Start", this);
                var result = new { status = "0" };
                if (SessionID == null)
                {
                    if (Session["currentSession"] == null)
                    {
                        result = new { status = "3" };
                        return result.status;
                    }
                    SessionID = Session["currentSession"].ToString();
                }
                return SMSOTP.SendSMSOTP(Email, SessionID);
            }
            catch (Exception ex)
            {
                Log.Error("Marathon SendSMSOTP exception occured"+ex.Message, this);
                var result = new { status = "0" };
                return result.status;
            }
        }

        public ActionResult GroupRegisteration()
        {
            try
            {
                if(Session["uid"]!=null)
                {
                    if (Session["PageRefresh"]==null)
                    {
                        Session["PageRefresh"] = "true";
                    }
                    if (Session["CurrentBibCount"]!=null  && Session["PageRefresh"].ToString()=="false")
                    {
                        var currentCount = int.Parse(Session["CurrentBibCount"].ToString());
                        Session["CurrentBibCount"] = currentCount+1;
                        Session["PageRefresh"] = "true";
                    }                   
                    else if (Session["CurrentBibCount"]==null)
                    {
                        Session["CurrentBibCount"] = 1;
                    }
                    Log.Info("Marathon GroupRegisteration Get", this);
                    RegistrationModel registration = new RegistrationModel();
                    bindState();
                    registration.StateList = ViewBag.state;
                    ID raceLocationID = new ID("{9D55601D-21DA-46AC-94B7-DCB0DE626927}");
                    var raceLocation = Sitecore.Context.Database.GetItem(raceLocationID);
                    registration.RaceLocation = raceLocation.Fields["Location"].Value;
                    return View(registration);
                }
                Log.Info("Marathon GroupRegisteration User is not logged in ", this);
                return Redirect("/Register");
            }
            catch (Exception ex)
            {
                Log.Error("Marathon GroupRegisteration exception occured"+ex.Message, this);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupRegisteration(RegistrationModel groupModel)
        {
            try
            {
                if (Session["uid"] != null)
                {
                    bindState();
                    groupModel.StateList = ViewBag.state;
                    if (ModelState.IsValid)
                    {
                        var SessionId = Session["uid"].ToString();
                        var UserId = Guid.NewGuid().ToString();
                        GroupRegistration groupRegistrationObj = new GroupRegistration();
                        groupRegistrationObj.GroupRegisteration(groupModel,SessionId,UserId);
                        Session["PageRefresh"] = "false";
                        return Redirect("/GroupRegister");
                    }
                    else
                    {
                        Log.Error("Marathon GroupRegisteration Model validation failed" + groupModel, this);
                        return View(groupModel);
                    }                   
                }
                Log.Info("Marathon GroupRegisteration add participant: User is not logged in ", this);
                return Redirect("/Register");
            }
            catch (Exception ex)
            {
                Log.Error("Marathon GroupRegisteration add participant exception occured" + ex.Message, this);
                return Redirect("/GroupRegister");
            }


        }
        [HttpPost]
        public ActionResult SaveGroupRegistration()
        {
            Log.Info("Marathon SaveGroupRegistration start", this);
            Session["GroupRegisteration"] = "true";

            DataTable dt = Session["GroupCart"] as DataTable;
            decimal finalAmount = 0;
            try
            {
                if (Session["uid"] != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if(dt.Rows[i][9].ToString()== "pending")
                        {
                            finalAmount = finalAmount + Decimal.Parse(dt.Rows[i][5].ToString(), NumberStyles.AllowDecimalPoint);
                        }
                    }
                    var redirectionURL = GroupPayment(finalAmount);
                    return Json(new { result = "Redirect", url = redirectionURL });
                }
                Log.Info("Marathon SaveGroupRegistration: User is not logged in ", this);
                return Redirect("/Register");
            }

            catch (Exception ex)
            {
                Log.Error("Marathon SaveGroupRegistration exception occured", ex.Message);
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResendOTPSMS(string PhoneNumber, string formName, string reResponse)
        {
            var result = new { status = "0" };
            var currentSession = "currentSession";
            try
            {
                if (!Captcha.IsReCaptchValidV3(reResponse))
                {
                    result = new { status = "10" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(PhoneNumber, "^[6-9][0-9]{9}$"))
                {
                    result = new { status = "9" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                AhmedabadMarathonRegistrationDataContext dbContext = new AhmedabadMarathonRegistrationDataContext();
                Log.Info("Marathon ResendOTPSMS : for mobile Number" + PhoneNumber + "and Form " + formName, this);
                if(formName== "new_participant")
                {
                    var NewParticipant = dbContext.AhmedabadMarathonRegistrations.Where(x => x.ContactNumber == PhoneNumber).Any();
                    if(NewParticipant)
                    {
                        result = new { status = "6" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                }
                if (formName == "login_form")
                {
                    var NewParticipant = dbContext.AhmedabadMarathonRegistrations.Where(x => x.ContactNumber == PhoneNumber).Any();
                    if (!NewParticipant)
                    {
                        result = new { status = "7" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if(Session["currentSession"]!=null)
                {
                    currentSession = Session["currentSession"].ToString();
                }
                var status = SendSMSOTP(PhoneNumber, currentSession);
                result = new { status = status };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Marathon ResendOTPSMS: failed exception occured", ex.Message);
                return Json(result, JsonRequestBehavior.AllowGet);

            }

        }

        public string GroupPayment(decimal finalAmount)
        {
            try
            {
                if (Session["uid"] != null)
                {
                    var UserSessionId = Session["uid"].ToString();
                    var RequestUrl = Request.Url.ToString();
                    if(finalAmount>=decimal.Parse("10.00"))
                    {
                        Log.Info("Gorup Registration payment", "");
                        GroupRegistration groupRegistration = new GroupRegistration();
                        return groupRegistration.GroupPayment(UserSessionId, finalAmount, RequestUrl);
                    }
                    else
                    {
                        Log.Info("Gorup Registration payment is less than 10.00", "");
                        DataTable dt = Session["GroupCart"] as DataTable;
                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i][9].ToString() != "pending")
                                {
                                    var userData = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId == dt.Rows[i][6].ToString()).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();
                                    RegistrationModel user = new RegistrationModel();
                                    if (userData != null)
                                    {
                                        user.RaceDistance = userData.RaceDistance;
                                        user.ReferenceCode = userData.ReferenceCode;
                                        user.FirstName = userData.FirstName;
                                        user.LastName = userData.LastName;
                                        user.Email = userData.Email;
                                        decimal? d = userData.FinalAmount;
                                        user.FinalAmount = d ?? 0;
                                        user.PaymentStatus = userData.PaymentStatus;
                                        user.ContactNumber = userData.ContactNumber;
                                        user.Age = userData.Age;
                                        user.Gender = userData.Gender;
                                        user.TShirtSize = userData.TShirtSize;
                                        user.Useridstring = userData.UserId;
                                    }
                                    sendEmail(user);
                                    sendSMS(user, "");
                                    SMSOTP.registrationconfirmation(user);
                                }

                            }
                        }
                        return "/registration-thankyou";
                    }
                }
                Log.Info("Marathon GroupPayment: User is not logged in ", this);
                return "/Register";
            }
            catch(Exception ex)
            {
                Log.Error("Marathon GroupPayment  function failed exception occured", ex.Message);
                return "/registration-failed";
            }
        }


        public ActionResult DeleteGroupParticipant(int id)
        {
            var result = new { status = "0" };
            try
            {
                DataTable dt = Session["GroupCart"] as DataTable;
                dt.Rows.RemoveAt(id);
                Session["GroupCart"] = dt;
                if (dt.Rows.Count <= 0)
                {
                    Session["GroupCart"] = null;
                }
                result = new { status = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("Marathon DeleteGroupParticipant: failed exception occured", ex.Message);
                return Json(result, JsonRequestBehavior.AllowGet);
            }         
        }

        [HttpPost]
        public ActionResult AgeValidator(string DateofBirth, string RaceCategory)
        {
            var result = new { status = "0" };
            result = new { status = RegistrationFormValidation.CheckAge(DateofBirth, RaceCategory) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CheckCouponCodeValidation(string ReferenceCode)
        {
            return Json(JsonConvert.SerializeObject(RegistrationFormValidation.CouponCodeValidator(ReferenceCode)), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CheckEmployeeCode(string EmployeeID)
        {
            var result = new { status = "0" };
            result = new { status = RegistrationFormValidation.CheckEmployeeCode(EmployeeID) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult MinimumDonationAmount(string DonationAmount, string RaceDistance)
        {
            var result = new { minimumAMount = "0" ,minRaceAmount="0"};
            result = new { minimumAMount = RegistrationFormValidation.MinDonationAmount(DonationAmount, RaceDistance), minRaceAmount= RegistrationFormValidation.MinRaceAmount( RaceDistance) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AlreadyRegisteredUser(RegisteredUserRegistration registeredUser)
        {
            var result = new { redirect = "0" ,redirectUrl=""};
            if (ModelState.IsValid)
            {
                AlreadyRegisterdUser alreadyRegisterdUser = new AlreadyRegisterdUser();
                result = new { redirect = "1", redirectUrl = alreadyRegisterdUser.ALreadyRegisteredUserRegistration(registeredUser, Session["uid"].ToString(), Request.Url.ToString()) };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Log.Info("AlreadyRegisteredUser Model Validation Failed", "");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PayNowAndCompleteRegistration(string reResponse)
        {
            var result = new { redirect = "0", redirectUrl = "" };
            AlreadyRegisterdUser alreadyRegisterdUser = new AlreadyRegisterdUser();
            result = new { redirect = "1", redirectUrl = alreadyRegisterdUser.PayAndCompleteRegistration(Session["uid"].ToString(), Request.Url.ToString(), reResponse) };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }


}

