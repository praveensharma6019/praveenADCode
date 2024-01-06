using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Transmission.Website.Models;
using System;
using System.Collections.Generic;
using Sitecore.Transmission.Website.Helper;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Sitecore.Transmission.Website.Services;
using System.Net.Http;
using System.Net.Http.Headers;

using Sitecore.Exceptions;
using System.Threading.Tasks;
using System.Globalization;
using Sitecore.Transmission.Website.SessionHelper;


namespace Sitecore.Transmission.Website.Controllers
{
    public class TransmissionController : Controller
    {
        private TransmissionRepository transmissionrepos = new TransmissionRepository();

        // GET: Transmission
        public ActionResult Index()
        {
            return View();
        }

        public bool IsReCaptchValid(string reResponse)
        {
            var result = false;
            // var captchaResponse = Request.Form["g-recaptcha-response"];
            var captchaResponse = reResponse;
            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
            //var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            // var secretKey = "6LdkC64UAAAAAJiii15Up9xETgsLuPQnQ1BKZft8";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        [HttpGet]
        public ActionResult TransmissionVendorInquiry()
        {
            return base.View("/Views/Transmission/Sublayouts/TransmissionVendorInquiry.cshtml", new TransmissionVendorModel());
        }

        [HttpPost]
        public ActionResult TransmissionVendorInquiry(TransmissionVendorModel m, string SubmitApplication)
        {
            ActionResult actionResult;
            Log.Info("Insert TransmissionVendorEnrollmentForm", "Start");
            bool validationStatus = true;
            try
            {
                validationStatus = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Info(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            if (validationStatus)
            {
                Log.Info("Insert Transmission Enquiry Form Captcha Validated", "Start");
                TransmissionVendorEnqiryHelper transmissionvendorenqiryhelper = new TransmissionVendorEnqiryHelper();
                string value = "";
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (base.ModelState.IsValid)
                        {
                            TransmissionContactFormRecordDataContext transmissionVendorFormDataContext = new TransmissionContactFormRecordDataContext();
                            m.RegistrationNo = transmissionvendorenqiryhelper.GetUniqueRegNo();
                            string newsid;
                            if (m.RegistrationNo != null)
                            {
                                string newid = m.MessageType.Substring(0, 3);
                                string newids = newid.ToUpper();
                                newsid = newids + m.RegistrationNo;
                                m.RegistrationNo = newsid;
                            }
                            while (true)
                            {
                                if ((
                                    from a in transmissionVendorFormDataContext.TransmissionVendorEnquiryForms
                                    where a.RegistrationNo == m.RegistrationNo
                                    select a).FirstOrDefault<TransmissionVendorEnquiryForm>() == null)
                                {
                                    break;
                                }
                                m.RegistrationNo = transmissionvendorenqiryhelper.GetUniqueRegNo();

                            }
                            m.Id = Guid.NewGuid();
                            TransmissionVendorEnquiryForm transmissionvendorenquiry = new TransmissionVendorEnquiryForm()
                            {
                                Id = m.Id,
                                RegistrationNo = m.RegistrationNo,
                                Name = m.Name,
                                CompanyName = m.CompanyName,
                                Email = m.EmailID,
                                Message = m.Message,
                                MessageType = m.MessageType,
                                MobileNo = m.MobileNo,
                                PageInfo = m.PageInfo,
                                FormType = m.FormName,
                                SubmittedBy = m.SubmittedBy,
                                FormSubmitOn = new DateTime?(DateTime.Now),
                                CreatedOn = new DateTime?(DateTime.Now),
                                InqTicketStatus = "Open",
                                UpdateCount = 0,
                                CurrentStatusPhase1 = "Pending for Review"
                            };
                            transmissionVendorFormDataContext.TransmissionVendorEnquiryForms.InsertOnSubmit(transmissionvendorenquiry);
                            transmissionVendorFormDataContext.SubmitChanges();
                            Log.Info("Form Transmission Vendor Form data saved into db successfully: ", this);
                        }

                        else
                        {
                            actionResult = base.View("/Views/Transmission/Sublayouts/TransmissionVendorInquiry.cshtml", m);
                            return actionResult;
                        }
                    }


                    this.SendMailforVendorEnroll(m);
                    actionResult = this.Redirect("/vendorregistration-thankyou");
                }

                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    Log.Error(string.Concat("Error at Saving Form Defence Vendor: ", ex.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/Transmission/Sublayouts/TransmissionVendorInquiry.cshtml", m);
                }
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/Transmission/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/Transmission/Sublayouts/TransmissionVendorInquiry.cshtml", m);
            }
            return actionResult;
        }

        [HttpGet]
        public ActionResult VendorInquiryStatus()
        {
            return base.View("/Views/Transmission/Sublayouts/TransmissionVendorInquiryStatusCheck.cshtml", new TransmissionVendorModel());
        }
        [HttpPost]
        public ActionResult VendorInquiryStatus(string RegistrationNo, string reResponse)
        {
            TransmissionVendorModel model = new TransmissionVendorModel();
            TransmissionContactFormRecordDataContext VendorDataContext = new TransmissionContactFormRecordDataContext();
            bool validationStatus = true;
            try
            {
                validationStatus = this.IsReCaptchValid(reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Info(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }
            if (validationStatus)
            {
                try
                {
                    model.RegistrationNo = RegistrationNo;
                    if (string.IsNullOrEmpty(RegistrationNo))
                    {
                        ModelState.AddModelError(nameof(model.RegistrationNo), "Please Enter Inquiry Registration No.");
                        return View("/Views/Transmission/Sublayouts/TransmissionVendorInquiryStatusCheck.cshtml", model);
                    }
                    var vendorData = (from a in VendorDataContext.TransmissionVendorEnquiryForms
                                      where a.RegistrationNo == RegistrationNo
                                      select a).FirstOrDefault();
                    if (vendorData == null)
                    {
                        ModelState.AddModelError(nameof(model.RegistrationNo), "Inquiry Registration Number Not Found");
                        return View("/Views/Transmission/Sublayouts/TransmissionVendorInquiryStatusCheck.cshtml", model);
                    }
                    if (vendorData.CurrentStatusPhase2 != null)
                    {
                        model.CurrentStatus = vendorData.CurrentStatusPhase2;
                    }
                    else
                    {
                        model.CurrentStatus = vendorData.CurrentStatusPhase1;
                    }
                    return View("/Views/Transmission/Sublayouts/TransmissionVendorInquiryStatusCheck.cshtml", model);
                }
                catch (Exception ex)
                {
                    Log.Error(string.Concat("Error at Saving Form Defence Vendor: ", ex.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    return View("/Views/Transmission/Sublayouts/TransmissionVendorInquiryStatusCheck.cshtml", model);
                }
            }
            else
            {
                ModelState.AddModelError(nameof(model.reResponse), DictionaryPhraseRepository.Current.Get("/Transmission/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                return View("/Views/Transmission/Sublayouts/TransmissionVendorInquiryStatusCheck.cshtml", model);
            }

        }
        public void SendMailforVendorEnroll(TransmissionVendorModel model)
        {
            try
            {
                Item mailconfig = Context.Database.GetItem(Templates.MailConfiguration.MailConfigurationItemID);
                Log.Info("Success mail sending to client", this);
                string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
                string CustomerTo = model.EmailID;
                string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
                string value = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SuccessMessage].Value;
                string CustomerMailBody = value;
                CustomerMailBody = value;
                string OfficialsFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_MailFrom].Value;
                string OfficialsTo = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_RecipientMail].Value;
                string OfficialsMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_Message].Value;
                string OfficialsSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_SubjectName].Value;
                using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                {
                    TransmissionVendorEnquiryForm ctx = (
                        from x in dbcontext.TransmissionVendorEnquiryForms
                        where (x.Id == model.Id) && x.RegistrationNo == model.RegistrationNo
                        select x).FirstOrDefault<TransmissionVendorEnquiryForm>();

                    CustomerMailBody = CustomerMailBody.Replace("$name", ctx.Name);
                    CustomerMailBody = CustomerMailBody.Replace("$companyname", ctx.CompanyName);
                    CustomerMailBody = CustomerMailBody.Replace("$mobile", ctx.MobileNo);
                    CustomerMailBody = CustomerMailBody.Replace("$mail", ctx.Email);
                    CustomerMailBody = CustomerMailBody.Replace("$registrationno", ctx.RegistrationNo);
                    CustomerMailBody = CustomerMailBody.Replace("$message", ctx.Message);
                    OfficialsMailBody = OfficialsMailBody.Replace("$name", ctx.Name);
                    OfficialsMailBody = OfficialsMailBody.Replace("$companyname", ctx.CompanyName);
                    OfficialsMailBody = OfficialsMailBody.Replace("$mobile", ctx.MobileNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$mail", ctx.Email);
                    OfficialsMailBody = OfficialsMailBody.Replace("$registrationno", ctx.RegistrationNo);
                    OfficialsMailBody = OfficialsMailBody.Replace("$message", ctx.Message);

                }
                if (!this.sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom))
                {
                    Log.Info("Sending mail to customer is Failed", this);
                }
                else
                {
                    Log.Info("Sending mail to customer is Successfull", this);
                }
                if (!this.sendEmail(OfficialsTo, OfficialsSubject, OfficialsMailBody, OfficialsFrom))
                {
                    Log.Info("Sending mail to Officials is Failed", this);
                }
                else
                {
                    Log.Info("Sending mail to Officials is Successfull", this);
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Error at SendMailforVendorEnroll Form Defence Vendor: ", ex.Message), this);
            }
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


        [HttpPost]
        public ActionResult InsertContactFormdetail(TransmissionContactModel m)
        {
            bool validationStatus = false;
            var result = new { status = "1" };
            Log.Error("Validating Transmission ContactForm to stop auto script ", "Start");

            try
            {
                validationStatus = IsReCaptchValid(m.reResponse);
                //if (Request.Cookies["SIDCC"]!=null)
                //{
                //    if (Session["validate"] == null)
                //    {
                //        validationStatus = true;
                //    }
                //    else
                //    {
                //        if (Session["validate"].ToString() != Request.Cookies["SIDCC"].Value)
                //        {
                //            validationStatus = true;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }

            if (validationStatus == true)
            {
                Log.Error("InsertTransmissionContactFormRecord ", "Start");
                var getEmailTo = "";

                try
                {
                    TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                    TransmissionContactFormRecord r = new TransmissionContactFormRecord();
                    bool isValid = true;
                    string errorMessage = "Invalid field values:";

                    if (string.IsNullOrEmpty(m.Name) || !Regex.IsMatch(m.Name, "^[a-zA-Z][a-zA-Z ]*$"))
                    {
                        errorMessage = errorMessage + " Name";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(m.Email) || !Regex.IsMatch(m.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        errorMessage = errorMessage + " Email";
                        isValid = false;
                    }
                    if (string.IsNullOrEmpty(m.Mobile) || !Regex.IsMatch(m.Mobile, @"^[0-9]{10,10}$"))
                    {
                        errorMessage = errorMessage + "Mobile";
                        isValid = false;
                    }

                    //validate MessageType
                    if (string.IsNullOrEmpty(m.MessageType))
                    {
                        errorMessage = errorMessage + " MessageType";
                        isValid = false;
                    }
                    //validate message
                    if (string.IsNullOrEmpty(m.Message))
                    {
                        errorMessage = errorMessage + " Message";
                        isValid = false;
                    }

                    if (!isValid)
                    {
                        result = new { status = "4" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    r.Name = m.Name;
                    r.Email = m.Email;
                    r.Mobile = m.Mobile;
                    r.MessageType = m.MessageType;
                    r.Message = m.Message;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = m.FormSubmitOn;



                    #region Insert to DB
                    rdb.TransmissionContactFormRecords.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                    //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                }
                catch (Exception ex)
                {
                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }

                try
                {
                    var msgTpye = Sitecore.Context.Database.GetItem("{9C66A307-02B8-4494-9DC6-1723E26686F5}");
                    var getfilteredItem = msgTpye.Children.Where(x => x.Fields["SubjectName"].Value.ToLower() == m.MessageType.ToLower());

                    foreach (var itemData in getfilteredItem.ToList())
                    {
                        getEmailTo = itemData.Fields["EmailTo"].Value;
                    }
                }
                catch (Exception ex)
                {
                    result = new { status = "1" };
                    Log.Error("Failed to get subject specific Email", ex.ToString());
                }

                try
                {
                    string emailText = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailText", "");
                    string message = "";
                    message = "Hello<br><br>" + emailText + "<br><br>";
                    message = message + "<br>Name: " + m.Name + "<br>Email-Id: " + m.Email + "<br>Subject of Message: " + m.MessageType + "<br>Contact Number: " + m.Mobile + "<br>Message: " + m.Message + "<br><br>Thanks";
                    //string to = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailTo", "");
                    string from = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailFrom", "");
                    string emailSubject = DictionaryPhraseRepository.Current.Get("/ContactFrom/EmailSubject", "");
                    bool results = sendEmail(getEmailTo, emailSubject, message, from);

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
            }
            else
            {
                result = new { status = "2" };

            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidateMonth(string Month, string Year)
        {
            JsonResult actionResult;
            var result = new { status = "0" };
            string Login = "";
            TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext();
            if (Session["TransmissionCostCalculator"] != null)
            {
                Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
            }

            var MonthCarbonCalculationCount = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == Month && x.Year == Year && x.Login == Login).Count();
            if (MonthCarbonCalculationCount > 0)
            {
                result = new { status = "1" };
            }
            actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            return actionResult;
        }

        [HttpGet]
        public ActionResult InsertCostCalculator(string RegistrationNo)
        {
            TransmissionsCostCalculator model = new TransmissionsCostCalculator();
            if (string.IsNullOrEmpty(RegistrationNo))
            {
                try
                {
                    if (Session["TransmissionCostCalculator"] != null)
                    {
                        TransmissionContactFormRecordDataContext dbcontext1 = new TransmissionContactFormRecordDataContext();
                        var obj1 = dbcontext1.TransmissionInsertCostCalculators.Where(X => X.Login == UserSessionTransmission.CostCalculatorSesstion.LoginId && X.CarbonEmissionReducePercentage != "" && (X.CarbonEmissionReviewYear != "" || X.CarbonEmissionReviewDate != null)).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();


                        if (obj1 != null)
                        {
                            Session["CarbonEmissionReducePercentage"] = obj1.CarbonEmissionReducePercentage;
                            if (obj1.CarbonEmissionReviewDate != null)
                            {
                                Session["CarbonEmissionReviewDate"] = DateTime.Parse(obj1.CarbonEmissionReviewDate.ToString()).Date.ToString("mm/dd/yyyy");
                            }


                            Session["CarbonEmissionReviewYear"] = obj1.CarbonEmissionReviewYear;

                        }
                        using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                        {
                            var objs = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == UserSessionTransmission.CostCalculatorSesstion.LoginId).ToList();
                            TransmissionRepository mode = new TransmissionRepository();
                            model.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
                            string[] Monthsname = {"January", "February", "March", "April", "May", "June",
  "July", "August", "September", "October", "November", "December"};
                            List<string> month = new List<string>();
                            if (objs.Count >= 1)
                            {
                                var Month_Name = dbcontext.TransmissionInsertCostCalculators.Where(X => X.Login == model.Login).Select(x => x.MonthName).ToList();
                                foreach (var item in Month_Name)
                                {
                                    if (Monthsname.Contains(item))
                                    {
                                        month.Add(item);
                                    }
                                }
                                string[] Finalmonth = Monthsname.Except(month.ToArray()).ToArray();
                                var obj = dbcontext.TransmissionInsertCostCalculators.Where(X => X.Login == model.Login).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();
                                if (obj != null)
                                {
                                    model.MonthNames = Finalmonth[0].Length < 1 ? Monthsname[0] : Finalmonth[0];
                                    ViewBag.FinalMonthname = Finalmonth[0].Length < 1 ? Monthsname[0] : Finalmonth[0];
                                    TempData["FinalMonthname"] = Finalmonth[0].Length < 1 ? Monthsname[0] : Finalmonth[0];
                                    model.HistoryElectricityConsumedatResidences = obj.ElectricityConsumedAtResidence;
                                    model.HistoryCNGUsed = obj.CNGUsed;
                                    model.HistoryDieselConsumptions = obj.DieselConsumption;
                                    model.HistoryPetrolConsumptions = obj.PetrolConsumption;
                                    model.HistoryAutoRikshaws = obj.CNGAutoRickshaw;
                                    model.HistoryBuses = obj.BusUse;
                                    model.HistoryLPGUsed = obj.LPGUsed;
                                    model.HistoryTrains = obj.TrainUse;
                                    model.HistoryTotalTrips = (Decimal.Parse(obj.NumberofTrips) * 1000).ToString("0.####");
                                    //model.HistoryTotalTrips = obj.NumberofTrips ;
                                    model.HistoryAnnualCarbonFootprint = obj.AnnualCarbonFootprint;
                                    return View(model);
                                }
                            }
                        }
                    }
                    else
                    {
                        return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                    }
                }
                catch
                {

                }
            }
            else
            {
                ViewCarbonCalculator modal = new ViewCarbonCalculator();
                //Session["EditTransmissionCostCalculator"] = null;
                try
                {
                    if (Session["TransmissionCostCalculator"] != null)
                    {
                        TransmissionContactFormRecordDataContext dbcontext1 = new TransmissionContactFormRecordDataContext();
                        var obj1 = dbcontext1.TransmissionInsertCostCalculators.Where(X => X.Login == UserSessionTransmission.CostCalculatorSesstion.LoginId && X.RegistartionNumber == RegistrationNo && X.CarbonEmissionReducePercentage != "" && (X.CarbonEmissionReviewYear != "" || X.CarbonEmissionReviewDate != null)).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();
                        if (obj1 != null)
                        {
                            Session["CarbonEmissionReducePercentage"] = obj1.CarbonEmissionReducePercentage;
                            if (obj1.CarbonEmissionReviewDate != null)
                            {
                                Session["CarbonEmissionReviewDate"] = DateTime.Parse(obj1.CarbonEmissionReviewDate.ToString()).Date.ToString("mm/dd/yyyy");
                            }
                            Session["CarbonEmissionReviewYear"] = obj1.CarbonEmissionReviewYear;
                        }
                        using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                        {
                            var CarbonCalculationRecord = (from rc in dbcontext.TransmissionInsertCostCalculators
                                                           where (rc.RegistartionNumber == RegistrationNo)
                                                           select rc).FirstOrDefault();
                            if (CarbonCalculationRecord != null)
                            {
                                var OffsetCarbonCalculationRecord = (from rc in dbcontext.TransmissionCarbonOffsetValues
                                                                     where (rc.RegistartionNumber == RegistrationNo)
                                                                     select rc).FirstOrDefault();
                                var userRegistrationDetail = dbcontext.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == CarbonCalculationRecord.Login || x.MobileNumber == CarbonCalculationRecord.Login).FirstOrDefault();
                                modal.TotalFamilyMembers = obj1.TotalFamilyMembers;
                                modal.ElectricityBillConsumptionVal = obj1.ElectricityBillConsumption_Val;
                                modal.CNGUsedsVal = obj1.CNGUsed_Val;
                                modal.LPGCylindersVal = obj1.LPGUsed_Val;
                                modal.DieselConsumptionsVal = obj1.DieselConsumption_Val;
                                modal.PetrolConsumptionsVal = obj1.PetrolConsumption_Val;
                                modal.AutoRikshawsVal = obj1.CNGAutoRickshaw_Val;
                                modal.BusesVal = obj1.BusUse_Val;
                                modal.TrainsVal = obj1.TrainUse_Val;
                                modal.ElectricityConsumedatResidences = obj1.ElectricityConsumedAtResidence;
                                modal.CNGUseds = obj1.CNGUsed;
                                modal.LPGCylinders = obj1.LPGUsed;
                                modal.DieselConsumptions = obj1.DieselConsumption;
                                modal.PetrolConsumptions = obj1.PetrolConsumption;
                                modal.AutoRikshaws = obj1.CNGAutoRickshaw;
                                modal.Buses = obj1.BusUse;
                                modal.Trains = obj1.TrainUse;
                                modal.RegistrationNumber = obj1.RegistartionNumber;
                                modal.MonthNames = obj1.MonthName;
                                modal.Years = obj1.Year;
                                modal.DropdownAppliancesList = obj1.ElectricityAppliancesListValues;
                                modal.DropdownAppliancesConsumptionVal = obj1.ElectricityAppliancesConsumption_Val;
                                modal.DropdownAppliancesConsumption = obj1.ElectricityAppliancesConsumption;
                                modal.AirTripsDropdownList = obj1.AirTripsDropdownList;
                                modal.AirTripsValue = obj1.AirTripsValue;
                                modal.AirTrips = obj1.AirTrips;
                                string domestic = CarbonCalculationRecord.EmissionfromDomesticUse;
                                modal.CarbonEmissionReducePercentage = obj1.CarbonEmissionReducePercentage;
                                modal.CarbonEmissionReviewYear = obj1.CarbonEmissionReviewYear;
                                modal.CarbonEmissionReviewDate = obj1.CarbonEmissionReviewDate;
                                modal.TotalDomesticUses = CarbonCalculationRecord.EmissionfromDomesticUse;
                                modal.TotalTransportationUses = CarbonCalculationRecord.EmissionfromTransportation;
                                var totlaemission = Decimal.Parse(CarbonCalculationRecord.EmissionfromDomesticUse) + Decimal.Parse(CarbonCalculationRecord.EmissionfromTransportation) + Decimal.Parse(CarbonCalculationRecord.NumberofTrips);
                                modal.TotalCarbonEmission = totlaemission.ToString();
                                modal.EmployeeTotalemissionsperMonths = CarbonCalculationRecord.AverageEmissionperMonth;
                                modal.LandNeeded = CarbonCalculationRecord.LandNeededtoPlantTrees;
                                modal.NumberOfTreesNeeded = CarbonCalculationRecord.NumberofTreesNeeded;
                                modal.AverageAnnualCarbonFootprints = CarbonCalculationRecord.AverageAnnualCarbonFootprint;
                                modal.AnnualCarbonFootprints = CarbonCalculationRecord.AnnualCarbonFootprint;
                                modal.OffsetEmissionfromDomesticUse = OffsetCarbonCalculationRecord.OffsetEmissionfromDomesticUse;
                                modal.OffsetEmissionfromTransportation = OffsetCarbonCalculationRecord.OffsetEmissionfromTransportation;
                                modal.OffsetEmissionfromAirTrips = OffsetCarbonCalculationRecord.OffsetEmissionfromAirTrips;
                                modal.TotalOffsetCarbonEmission = OffsetCarbonCalculationRecord.TotalOffsetCarbonEmission;
                                modal.AverageOffsetEmissionperMonth = OffsetCarbonCalculationRecord.AverageOffsetEmissionperMonth;
                                modal.OffsetAnnualCarbonFootprint = OffsetCarbonCalculationRecord.OffsetAnnualCarbonFootprint;
                                var treeneeded = !string.IsNullOrEmpty(OffsetCarbonCalculationRecord.NumberofTreesNeeded) ? Double.Parse(OffsetCarbonCalculationRecord.NumberofTreesNeeded) / 1.81 : 0.00;
                                var fundfortrees = !string.IsNullOrEmpty(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees) ? Double.Parse(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees) : 0.00;
                                var EmissionOffsetForTreePlantation = !string.IsNullOrEmpty(OffsetCarbonCalculationRecord.NumberofTreesNeeded) ? Double.Parse(OffsetCarbonCalculationRecord.NumberofTreesNeeded).ToString("0.##") : "0.00";
                                modal.EmissionOffsetForTreePlantation = EmissionOffsetForTreePlantation;
                                var OffsetEmissionforFundingTrees = !string.IsNullOrEmpty(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees) ? Double.Parse(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees).ToString("0.##") : "";
                                modal.OffsetEmissionforFundingTrees = OffsetEmissionforFundingTrees;
                                Double trees = (fundfortrees / 1.81) * 1000;
                                modal.OffsetNumberofTreesNeeded = treeneeded.ToString();
                                modal.FundNeededtoPlantTrees = trees.ToString();

                                modal.PersonalTransportDropdownValue = OffsetCarbonCalculationRecord.PersonalTransportDropdownValue;
                                modal.PersonalTransportDropdownValue = OffsetCarbonCalculationRecord.PersonalTransportDropdownValue;
                                modal.PersonalTransportDropdownValue = OffsetCarbonCalculationRecord.PersonalTransportDropdownValue;

                                if (!string.IsNullOrEmpty(OffsetCarbonCalculationRecord.PersonalTransport))
                                {
                                    modal.IsPersonalTransport = true;
                                    modal.PersonalTransportDropdownValue = OffsetCarbonCalculationRecord.PersonalTransportDropdownValue;
                                    modal.PersonalTransportValue = OffsetCarbonCalculationRecord.PersonalTransportValue;
                                    modal.PersonalTransport = OffsetCarbonCalculationRecord.PersonalTransport;
                                }
                                if (!string.IsNullOrEmpty(OffsetCarbonCalculationRecord.PublicTransport))
                                {
                                    modal.IsPublicTransport = true;
                                    modal.PublicTransportValue = OffsetCarbonCalculationRecord.PublicTransportValue;
                                    modal.PublicTransport = OffsetCarbonCalculationRecord.PublicTransport;
                                }
                                if (!string.IsNullOrEmpty(OffsetCarbonCalculationRecord.OnlineMeeting))
                                {
                                    modal.IsOnlineMeeting = true;
                                    modal.OnlineMeeting = OffsetCarbonCalculationRecord.OnlineMeeting;
                                    modal.OnlineMeetingValue = OffsetCarbonCalculationRecord.OnlineMeetingValue;
                                }
                                if (!string.IsNullOrEmpty(OffsetCarbonCalculationRecord.FiveStarAppliances))
                                {
                                    modal.IsFiveStarAppliances = true;
                                    modal.FiveStarAppliances = OffsetCarbonCalculationRecord.FiveStarAppliances;
                                    modal.FiveStarAppliancesValue = OffsetCarbonCalculationRecord.FiveStarAppliancesValue;
                                }
                                if (!string.IsNullOrEmpty(OffsetCarbonCalculationRecord.NumberofTreesNeeded))
                                {
                                    modal.IsNumberOfTreesNeeded = true;
                                    modal.NumberOfTreesNeededVal = OffsetCarbonCalculationRecord.NoOfTreesNeededValue;
                                    modal.NumberofTress = OffsetCarbonCalculationRecord.NumberofTreesNeeded;
                                }
                                if (!string.IsNullOrEmpty(OffsetCarbonCalculationRecord.PlantationTreesNeeded))
                                {
                                    modal.IsNumberOfPlantationProjectTreesNeeded = true;
                                    modal.NumberOfPlantationProjectTreesNeededVal = OffsetCarbonCalculationRecord.PlantationTreesNeededValue;
                                    modal.NumberOfPlantationProjectTreesNeeded = OffsetCarbonCalculationRecord.PlantationTreesNeeded;
                                }
                                if (!string.IsNullOrEmpty(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees))
                                {
                                    modal.IsFundNeededtoPlantTrees = true;
                                    modal.FundNeededtoPlantTreesVal = OffsetCarbonCalculationRecord.FundNeededToPlantTreesValue;
                                    modal.FundNeededtoPlantTrees = OffsetCarbonCalculationRecord.FundNeededtoPlantTrees;
                                }
                                Session["RegistrationNo"] = CarbonCalculationRecord.RegistartionNumber;
                                return View("EditCostCalculator", modal);
                            }
                        }
                    }
                    else
                    {
                        return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                    }
                }
                catch
                {

                }
                return View("EditCostCalculator", modal);
            }

            return View(model);

        }

        [HttpPost]
        public ActionResult InsertCostCalculator(TransmissionsCostCalculator model)
        {

            bool validationStatus = true;
            var result = new { status = "1", Result = "" };
            Log.Error("Validating Transmission Cost Calculator to stop auto script ", "Start");
            try
            {
            }
            catch (Exception ex)
            {
                result = new { status = "2", Result = "" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }
            if (Session["TransmissionCostCalculator"] != null)
            {
                bool status = true;
                model.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
                string company = "";
                if (validationStatus == true)
                {
                    Log.Error("InsertCostCalculatorRecord", "Start");

                    try
                    {
                        using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                        {
                            var objs = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == UserSessionTransmission.CostCalculatorSesstion.LoginId).ToList();
                            model.Id = Guid.NewGuid();
                            TransmissionVendorEnqiryHelper helper = new TransmissionVendorEnqiryHelper();
                            model.RegistrationNumber = helper.GetUniqueRegNo();
                            Session["Number"] = model.RegistrationNumber;
                            if (UserSessionTransmission.CostCalculatorSesstion.LoginId != null)
                            {
                                var userCompanyName = dbcontext.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == UserSessionTransmission.CostCalculatorSesstion.LoginId || x.MobileNumber == UserSessionTransmission.CostCalculatorSesstion.LoginId).FirstOrDefault();
                                Session["CompanyName"] = userCompanyName.Company;
                                company = userCompanyName.Company;
                            }
                            var MonthCarbonCalculationCount = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login).Count();
                            TransmissionInsertCostCalculator ObjRecord = new TransmissionInsertCostCalculator();
                            ObjRecord = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login).FirstOrDefault();
                            var ComparisionParameter = "";
                            if (MonthCarbonCalculationCount > 0)
                            {
                                var xxx = model.AverageAnnualCarbonFootprints;
                                var yyy = ObjRecord.AverageAnnualCarbonFootprint;
                                string ModelAverageAnnualCarbonFootprints = xxx.Replace("Tonnes", "");
                                string ObjRecordAverageAnnualCarbonFootprints = yyy.Replace("Tonnes", "");
                                if (Decimal.Parse(model.ElectricityConsumedatResidences) > Decimal.Parse(ObjRecord.ElectricityConsumedAtResidence))
                                {
                                    ObjRecord.ElectricityConsumedAtResidence = model.ElectricityConsumedatResidences;
                                }
                                else
                                {
                                    ComparisionParameter = "Electricity Consumed at Residence";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.CNGUseds) > Decimal.Parse(ObjRecord.CNGUsed))
                                {
                                    ObjRecord.CNGUsed = model.CNGUseds;
                                }
                                else
                                {
                                    ComparisionParameter = "PNG Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.LPGCylinders) > Decimal.Parse(ObjRecord.LPGUsed))
                                {
                                    ObjRecord.LPGUsed = model.LPGCylinders;
                                }
                                else
                                {
                                    ComparisionParameter = "LPG Cylinder Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.DieselConsumptions) > Decimal.Parse(ObjRecord.DieselConsumption))
                                {
                                    ObjRecord.DieselConsumption = model.DieselConsumptions;
                                }
                                else
                                {
                                    ComparisionParameter = "Diesel Consumptions";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.PetrolConsumptions) > Decimal.Parse(ObjRecord.PetrolConsumption))
                                {
                                    ObjRecord.PetrolConsumption = model.PetrolConsumptions;
                                }
                                else
                                {
                                    ComparisionParameter = "Petrol Consumptions";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.Buses) > Decimal.Parse(ObjRecord.BusUse))
                                {
                                    ObjRecord.BusUse = model.Buses;
                                }
                                else
                                {
                                    ComparisionParameter = "Bus Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.Trains) > Decimal.Parse(ObjRecord.TrainUse))
                                {
                                    ObjRecord.TrainUse = model.Trains;
                                }
                                else
                                {
                                    ComparisionParameter = "Train Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.AutoRikshaws) > Decimal.Parse(ObjRecord.CNGAutoRickshaw))
                                {
                                    ObjRecord.CNGAutoRickshaw = model.AutoRikshaws;
                                }
                                else
                                {
                                    ComparisionParameter = "CNG Auto Rikshaw/Taxi Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.TotalTrips) > Decimal.Parse(ObjRecord.NumberofTrips))
                                {
                                    ObjRecord.NumberofTrips = model.TotalTrips;
                                }
                                else
                                {
                                    ComparisionParameter = "Air Trips";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (status == true)
                                {
                                    dbcontext.SubmitChanges();

                                    // code to save the history of all the insert cost calculator
                                    TransmissionInsertCostCalculatorHistory history = new TransmissionInsertCostCalculatorHistory();
                                    history.ElectricityConsumedAtResidence = model.ElectricityConsumedatResidences;
                                    history.CNGUsed = model.CNGUseds;
                                    history.MonthName = model.MonthNames;
                                    history.Year = model.Years;
                                    history.Id = model.Id;
                                    history.CostCalculatorID = ObjRecord.Id.ToString(); ;
                                    history.RegistartionNumber = model.RegistrationNumber;
                                    history.TotalFamilyMembers = model.TotalFamilyMembers;
                                    history.LPGUsed = model.LPGCylinders;
                                    history.LPGUsed_Val = model.LPGCylindersVal;
                                    history.DieselConsumption = model.DieselConsumptions;
                                    history.PetrolConsumption = model.PetrolConsumptions;
                                    history.Login = model.Login;
                                    history.CNGAutoRickshaw = model.AutoRikshaws;
                                    history.NumberofTrips = model.TotalTrips;
                                    history.BusUse = model.Buses;
                                    history.TrainUse = model.Trains;
                                    history.FormType = model.FormType;
                                    history.PageInfo = model.PageInfo;
                                    history.FormSubmitOn = model.FormSubmitOn;
                                    history.EmissionfromDomesticUse = model.TotalDomesticUses;
                                    history.EmissionfromTransportation = model.TotalTransportationUses;
                                    history.AverageEmissionperMonth = model.EmployeeTotalemissionsperMonths;
                                    history.AnnualCarbonFootprint = model.AnnualCarbonFootprints;
                                    history.AverageAnnualCarbonFootprint = model.AverageAnnualCarbonFootprints;
                                    history.NumberofTreesNeeded = model.NumberOfTreesNeeded;
                                    history.LandNeededtoPlantTrees = model.LandNeeded;
                                    dbcontext.TransmissionInsertCostCalculatorHistories.InsertOnSubmit(history);
                                    dbcontext.SubmitChanges();
                                    Log.Info("Cost Calculator history Form data saved into db successfully: ", this);
                                }
                            }
                            else
                            {
                                TransmissionInsertCostCalculator ObjReg = new TransmissionInsertCostCalculator();
                                ObjReg.ElectricityConsumedAtResidence = model.ElectricityConsumedatResidences;
                                ObjReg.CNGUsed = model.CNGUseds;
                                ObjReg.MonthName = model.MonthNames;
                                ObjReg.CompanyName = company;
                                ObjReg.Year = model.Years;
                                ObjReg.Id = model.Id;
                                ObjReg.RegistartionNumber = model.RegistrationNumber;
                                ObjReg.TotalFamilyMembers = model.TotalFamilyMembers;
                                ObjReg.LPGUsed = model.LPGCylinders;
                                ObjReg.LPGUsed_Val = model.LPGCylindersVal;
                                ObjReg.DieselConsumption = model.DieselConsumptions;
                                ObjReg.PetrolConsumption = model.PetrolConsumptions;
                                ObjReg.Login = model.Login;
                                ObjReg.CNGAutoRickshaw = model.AutoRikshaws;
                                ObjReg.NumberofTrips = model.TotalTrips;
                                ObjReg.BusUse = model.Buses;
                                ObjReg.TrainUse = model.Trains;
                                ObjReg.FormType = model.FormType;
                                ObjReg.PageInfo = model.PageInfo;
                                ObjReg.FormSubmitOn = model.FormSubmitOn;
                                ObjReg.FormUpdatedOn = model.FormSubmitOn;
                                ObjReg.EmissionfromDomesticUse = model.TotalDomesticUses;
                                ObjReg.EmissionfromTransportation = model.TotalTransportationUses;
                                ObjReg.AverageEmissionperMonth = model.EmployeeTotalemissionsperMonths;
                                ObjReg.AnnualCarbonFootprint = model.AnnualCarbonFootprints;
                                ObjReg.AverageAnnualCarbonFootprint = model.AverageAnnualCarbonFootprints;
                                ObjReg.NumberofTreesNeeded = model.NumberOfTreesNeeded;
                                ObjReg.LandNeededtoPlantTrees = model.LandNeeded;
                                ObjReg.CNGUsed_Val = model.CNGUsedsVal;
                                ObjReg.DieselConsumption_Val = model.DieselConsumptionsVal;
                                ObjReg.PetrolConsumption_Val = model.PetrolConsumptionsVal;
                                ObjReg.CNGAutoRickshaw_Val = model.AutoRikshawsVal;
                                ObjReg.BusUse_Val = model.BusesVal;
                                ObjReg.TrainUse_Val = model.TrainsVal;
                                ObjReg.ElectricityBillConsumption_Val = model.ElectricityBillConsumptionVal;
                                ObjReg.ElectricityAppliancesListValues = string.IsNullOrEmpty(model.DropdownAppliancesList) ? "" : model.DropdownAppliancesList.ToString();
                                ObjReg.ElectricityAppliancesConsumption_Val = string.IsNullOrEmpty(model.DropdownAppliancesConsumptionVal) ? "" : model.DropdownAppliancesConsumptionVal.ToString();
                                ObjReg.ElectricityAppliancesConsumption = string.IsNullOrEmpty(model.DropdownAppliancesConsumption) ? "" : model.DropdownAppliancesConsumption.ToString();
                                ObjReg.AirTripsDropdownList = string.IsNullOrEmpty(model.AirTripsDropdownList) ? "" : model.AirTripsDropdownList.ToString();
                                ObjReg.AirTripsValue = string.IsNullOrEmpty(model.AirTripsValue) ? "" : model.AirTripsValue.ToString();
                                ObjReg.AirTrips = string.IsNullOrEmpty(model.AirTrips) ? "" : model.AirTrips.ToString();
                                //new added
                                string[] TotalTrips = !string.IsNullOrEmpty(model.AirTrips) ? model.AirTrips.Split(',') : null;
                                var totalTripsInt = 0.00;
                                if (TotalTrips != null)
                                {
                                    foreach (var ii in TotalTrips)
                                    {
                                        totalTripsInt = totalTripsInt + double.Parse(ii);
                                    }
                                }
                                if (totalTripsInt != 0.00)
                                {
                                    ObjReg.EmissiontotalTripsValue = (totalTripsInt / 1000.00).ToString();
                                }

                                dbcontext.TransmissionInsertCostCalculators.InsertOnSubmit(ObjReg);
                                dbcontext.SubmitChanges();
                                Log.Info("Cost Calculator Form data saved into db successfully: ", this);

                                // code to save the history of all the insert cost calculator
                                TransmissionInsertCostCalculatorHistory history = new TransmissionInsertCostCalculatorHistory();
                                history.ElectricityConsumedAtResidence = model.ElectricityConsumedatResidences;
                                history.CNGUsed = model.CNGUseds;
                                history.MonthName = model.MonthNames;
                                history.Year = model.Years;
                                history.Id = model.Id;
                                history.CostCalculatorID = ObjReg.Id.ToString(); ;
                                history.RegistartionNumber = model.RegistrationNumber;
                                history.TotalFamilyMembers = model.TotalFamilyMembers;
                                history.LPGUsed = model.LPGCylinders;
                                history.LPGUsed_Val = model.LPGCylindersVal;
                                history.DieselConsumption = model.DieselConsumptions;
                                history.PetrolConsumption = model.PetrolConsumptions;
                                history.Login = model.Login;
                                history.CNGAutoRickshaw = model.AutoRikshaws;
                                history.NumberofTrips = model.TotalTrips;
                                history.BusUse = model.Buses;
                                history.TrainUse = model.Trains;
                                history.FormType = model.FormType;
                                history.PageInfo = model.PageInfo;
                                history.FormSubmitOn = model.FormSubmitOn;
                                history.EmissionfromDomesticUse = model.TotalDomesticUses;
                                history.EmissionfromTransportation = model.TotalTransportationUses;
                                history.AverageEmissionperMonth = model.EmployeeTotalemissionsperMonths;
                                history.AnnualCarbonFootprint = model.AnnualCarbonFootprints;
                                history.AverageAnnualCarbonFootprint = model.AverageAnnualCarbonFootprints;
                                history.NumberofTreesNeeded = model.NumberOfTreesNeeded;
                                history.LandNeededtoPlantTrees = model.LandNeeded;
                                dbcontext.TransmissionInsertCostCalculatorHistories.InsertOnSubmit(history);
                                dbcontext.SubmitChanges();
                                Log.Info("Cost Calculator history Form data saved into db successfully: ", this);


                                // Code to calculate Estimated Annualized Carbon Value
                                string[] months1 = { "January", "February", "March" };
                                string[] months2 = { "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                string[] months3 = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                List<string> month = new List<string>();
                                int count = 0;
                                int altYear = 0;
                                List<TransmissionInsertCostCalculator> Data = null;
                                string FY = "FY ";
                                if (months1.Contains(model.MonthNames))
                                {
                                    int strlen = months1.Length;
                                    altYear = Int32.Parse(model.Years) - 1;
                                    FY = FY + altYear + "-" + model.Years;
                                    Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == model.Years && months1.Contains(x.MonthName)) || (x.Year == altYear.ToString() && months2.Contains(x.MonthName)))).ToList();

                                }
                                if (months2.Contains(model.MonthNames))
                                {
                                    int strlen = months2.Length;
                                    altYear = Int32.Parse(model.Years) + 1;
                                    FY = FY + model.Years + "-" + altYear;
                                    Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == altYear.ToString() && months1.Contains(x.MonthName)) || (x.Year == model.Years && months2.Contains(x.MonthName)))).ToList();

                                }
                                decimal sum = 0.00m;
                                var count2 = 0;
                                if (Data.Count > 0)
                                {
                                    foreach (var item in Data)
                                    {
                                        decimal AverageEmissionperMonth = Math.Round(decimal.Parse(item.AverageEmissionperMonth), 2);
                                        sum += AverageEmissionperMonth;
                                        count2++;
                                    }
                                }
                                var EstimatedAnnualizedCarbonFootprint = (sum / count2 * 12).ToString();
                                TransmissionInsertCostCalculator carbon = new TransmissionInsertCostCalculator();
                                carbon = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && x.Year == model.Years && months3.Contains(x.MonthName) && x.Id == model.Id).FirstOrDefault();
                                if (carbon != null)
                                {
                                    carbon.EstimatedCarbonAnnualizedValue = Math.Round(decimal.Parse(EstimatedAnnualizedCarbonFootprint), 2).ToString();
                                    carbon.FinancialYear = FY;
                                    dbcontext.SubmitChanges();
                                }
                            }
                            // End

                            var obj = dbcontext.TransmissionInsertCostCalculators.Where(X => X.Login == model.Login && X.CarbonEmissionReducePercentage != "" && (X.CarbonEmissionReviewYear != "" || X.CarbonEmissionReviewDate != null)).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();


                            if (obj != null)
                            {
                                Session["CarbonEmissionReducePercentage"] = obj.CarbonEmissionReducePercentage;

                                Session["CarbonEmissionReviewYear"] = obj.CarbonEmissionReviewYear;
                                if (obj.CarbonEmissionReviewDate != null && obj.CarbonEmissionReviewDate.ToString() != "")
                                {
                                    Session["CarbonEmissionReviewDate"] = DateTime.Parse(obj.CarbonEmissionReviewDate.ToString()).Date.ToString("mm/dd/yyyy");
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        result = new { status = "0", Result = "" };
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                result = new { status = "2", Result = "" };

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditCostCalculator(TransmissionsCostCalculator model)
        {

            bool validationStatus = true;
            var result = new { status = "1", Result = "" };
            Log.Error("Validating Transmission Cost Calculator to stop auto script ", "Start");
            try
            {
            }
            catch (Exception ex)
            {
                result = new { status = "2", Result = "" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }
            if (Session["TransmissionCostCalculator"] != null)
            {
                bool status = true;
                model.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
                string company = "";
                if (validationStatus == true)
                {
                    Log.Error("InsertCostCalculatorRecord", "Start");

                    try
                    {
                        using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                        {
                            var objs = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == UserSessionTransmission.CostCalculatorSesstion.LoginId && x.RegistartionNumber == model.RegistrationNumber).FirstOrDefault();
                            Session["Number"] = model.RegistrationNumber;
                            if (UserSessionTransmission.CostCalculatorSesstion.LoginId != null)
                            {
                                var userCompanyName = dbcontext.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == UserSessionTransmission.CostCalculatorSesstion.LoginId || x.MobileNumber == UserSessionTransmission.CostCalculatorSesstion.LoginId).FirstOrDefault();
                                Session["CompanyName"] = userCompanyName.Company;
                                company = userCompanyName.Company;
                            }
                            var MonthCarbonCalculationCount = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login && x.RegistartionNumber == model.RegistrationNumber).Count();
                            TransmissionInsertCostCalculator ObjRecord = new TransmissionInsertCostCalculator();
                            ObjRecord = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login && x.RegistartionNumber == model.RegistrationNumber).FirstOrDefault();
                            TransmissionCarbonOffsetValue transmissionCarbonOffsetValue = new TransmissionCarbonOffsetValue();
                            var trans = dbcontext.TransmissionCarbonOffsetValues.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login && x.RegistartionNumber == model.RegistrationNumber).FirstOrDefault();
                            transmissionCarbonOffsetValue = dbcontext.TransmissionCarbonOffsetValues.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login && x.RegistartionNumber == model.RegistrationNumber).FirstOrDefault();

                            var ComparisionParameter = "";
                            if (MonthCarbonCalculationCount == 1 && transmissionCarbonOffsetValue != null)
                            {
                                var xxx = model.AverageAnnualCarbonFootprints;
                                var yyy = ObjRecord.AverageAnnualCarbonFootprint;
                                string ModelAverageAnnualCarbonFootprints = xxx.Replace("Tonnes", "");
                                string ObjRecordAverageAnnualCarbonFootprints = yyy.Replace("Tonnes", "");
                                if (Decimal.Parse(model.ElectricityConsumedatResidences) >= Decimal.Parse(ObjRecord.ElectricityConsumedAtResidence))
                                {
                                    ObjRecord.ElectricityConsumedAtResidence = model.ElectricityConsumedatResidences;
                                }
                                else
                                {
                                    ComparisionParameter = "Electricity Consumed at Residence";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.CNGUseds) >= Decimal.Parse(ObjRecord.CNGUsed))
                                {
                                    ObjRecord.CNGUsed = model.CNGUseds;
                                }
                                else
                                {
                                    ComparisionParameter = "PNG Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.LPGCylinders) >= Decimal.Parse(ObjRecord.LPGUsed))
                                {
                                    ObjRecord.LPGUsed = model.LPGCylinders;
                                }
                                else
                                {
                                    ComparisionParameter = "LPG Cylinder Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.DieselConsumptions) >= Decimal.Parse(ObjRecord.DieselConsumption))
                                {
                                    ObjRecord.DieselConsumption = model.DieselConsumptions;
                                }
                                else
                                {
                                    ComparisionParameter = "Diesel Consumptions ";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.PetrolConsumptions) >= Decimal.Parse(ObjRecord.PetrolConsumption))
                                {
                                    ObjRecord.PetrolConsumption = model.PetrolConsumptions;
                                }
                                else
                                {
                                    ComparisionParameter = "Petrol Consumptions";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.Buses) >= Decimal.Parse(ObjRecord.BusUse))
                                {
                                    ObjRecord.BusUse = model.Buses;
                                }
                                else
                                {
                                    ComparisionParameter = "Bus Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.Trains) >= Decimal.Parse(ObjRecord.TrainUse))
                                {
                                    ObjRecord.TrainUse = model.Trains;
                                }
                                else
                                {
                                    ComparisionParameter = "Train Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (Decimal.Parse(model.AutoRikshaws) >= Decimal.Parse(ObjRecord.CNGAutoRickshaw))
                                {
                                    ObjRecord.CNGAutoRickshaw = model.AutoRikshaws;
                                }
                                else
                                {
                                    ComparisionParameter = "CNG Auto Rikshaw/Taxi Usage";
                                    result = new { status = "3", Result = ComparisionParameter };
                                    status = false;
                                }
                                if (model.AirTrips != null && ObjRecord.AirTrips != null)
                                {
                                    var cou = 0;
                                    string[] splitModel = model.AirTripsValue.Split(',');
                                    string[] splitObj = ObjRecord.AirTripsValue.Split(',');
                                    foreach (var i in splitObj)
                                    {
                                        if (decimal.Parse(splitModel[cou]) >= decimal.Parse(i)) { splitObj[cou] = i; }
                                        else
                                        {
                                            ComparisionParameter = "Air Trips";
                                            result = new { status = "3", Result = ComparisionParameter };
                                            status = false;
                                        }
                                        cou++;
                                    }
                                }
                                if (model.DropdownAppliancesConsumptionVal != null && ObjRecord.ElectricityAppliancesConsumption_Val != null)
                                {
                                    var cou = 0;
                                    string[] splitModel = model.DropdownAppliancesConsumptionVal.Split(',');
                                    string[] splitObj = ObjRecord.ElectricityAppliancesConsumption_Val.Split(',');
                                    foreach (var i in splitObj)
                                    {
                                        if (decimal.Parse(splitModel[cou]) >= decimal.Parse(i)) { splitObj[cou] = i; }
                                        else
                                        {
                                            ComparisionParameter = "Electricity Appliances";
                                            result = new { status = "3", Result = ComparisionParameter };
                                            status = false;
                                        }
                                        cou++;
                                    }
                                }
                                if (status == true)
                                {
                                    ObjRecord.TotalFamilyMembers = model.TotalFamilyMembers;
                                    ObjRecord.FormUpdatedOn = model.FormSubmitOn;
                                    ObjRecord.ElectricityBillConsumption_Val = model.ElectricityBillConsumptionVal;
                                    ObjRecord.ElectricityConsumedAtResidence = model.ElectricityConsumedatResidences;
                                    ObjRecord.ElectricityAppliancesListValues = model.DropdownAppliancesList;
                                    ObjRecord.ElectricityAppliancesConsumption_Val = model.DropdownAppliancesConsumptionVal;
                                    ObjRecord.ElectricityAppliancesConsumption = model.DropdownAppliancesConsumption;
                                    ObjRecord.CNGUsed = model.CNGUseds;
                                    ObjRecord.CNGUsed_Val = model.CNGUsedsVal;
                                    ObjRecord.LPGUsed = model.LPGCylinders;
                                    ObjRecord.LPGUsed_Val = model.LPGCylindersVal;
                                    ObjRecord.DieselConsumption = model.DieselConsumptions;
                                    ObjRecord.DieselConsumption_Val = model.DieselConsumptionsVal;
                                    ObjRecord.PetrolConsumption = model.PetrolConsumptions;
                                    ObjRecord.PetrolConsumption_Val = model.PetrolConsumptionsVal;
                                    ObjRecord.CNGAutoRickshaw = model.AutoRikshaws;
                                    ObjRecord.CNGAutoRickshaw_Val = model.AutoRikshawsVal;
                                    ObjRecord.BusUse = model.Buses;
                                    ObjRecord.BusUse_Val = model.BusesVal;
                                    ObjRecord.TrainUse = model.Trains;
                                    ObjRecord.TrainUse_Val = model.TrainsVal;
                                    ObjRecord.AirTripsDropdownList = model.AirTripsDropdownList;
                                    ObjRecord.AirTripsValue = model.AirTripsValue;
                                    ObjRecord.AirTrips = model.AirTrips;
                                    ObjRecord.NumberofTrips = model.TotalTrips;
                                    ObjRecord.EmissionfromDomesticUse = model.TotalDomesticUses;
                                    ObjRecord.EmissionfromTransportation = model.TotalTransportationUses;
                                    ObjRecord.AverageEmissionperMonth = model.EmployeeTotalemissionsperMonths;
                                    ObjRecord.LandNeededtoPlantTrees = model.LandNeeded;
                                    ObjRecord.NumberofTreesNeeded = model.NumberOfTreesNeeded;
                                    ObjRecord.AverageAnnualCarbonFootprint = model.AverageAnnualCarbonFootprints;
                                    ObjRecord.AnnualCarbonFootprint = model.AnnualCarbonFootprints;
                                    string[] TotalTrips = !string.IsNullOrEmpty(model.AirTrips) ? model.AirTrips.Split(',') : null;
                                    var totalTripsInt = 0.00;
                                    if (TotalTrips != null)
                                    {
                                        foreach (var ii in TotalTrips)
                                        {
                                            totalTripsInt = totalTripsInt + double.Parse(ii);
                                        }
                                    }
                                    if (totalTripsInt != 0.00)
                                    {
                                        ObjRecord.EmissiontotalTripsValue = (totalTripsInt / 1000.00).ToString();
                                    }
                                    ObjRecord.EstimatedCarbonAnnualizedValue = model.EstimatedCarbonAnnualizedValue;
                                    ObjRecord.NumberofTrips = (totalTripsInt / 1000.00).ToString();
                                    dbcontext.SubmitChanges();
                                }
                                // Code to calculate Estimated Annualized Carbon Value
                                string[] months1 = { "January", "February", "March" };
                                string[] months2 = { "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                string[] months3 = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                List<string> month = new List<string>();
                                int count = 0;
                                int altYear = 0;
                                List<TransmissionInsertCostCalculator> Data = null;
                                string FY = "FY ";
                                if (months1.Contains(model.MonthNames))
                                {
                                    int strlen = months1.Length;
                                    altYear = Int32.Parse(model.Years) - 1;
                                    FY = FY + altYear + "-" + model.Years;
                                    Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == model.Years && months1.Contains(x.MonthName)) || (x.Year == altYear.ToString() && months2.Contains(x.MonthName)))).ToList();
                                }
                                if (months2.Contains(model.MonthNames))
                                {
                                    int strlen = months2.Length;
                                    altYear = Int32.Parse(model.Years) + 1;
                                    FY = FY + model.Years + "-" + altYear;
                                    Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == altYear.ToString() && months1.Contains(x.MonthName)) || (x.Year == model.Years && months2.Contains(x.MonthName)))).ToList();
                                }
                                decimal sum = 0.00m;
                                var count2 = 0;
                                if (Data.Count > 0)
                                {
                                    foreach (var item in Data)
                                    {
                                        decimal AverageEmissionperMonth = Math.Round(decimal.Parse(item.AverageEmissionperMonth), 2);
                                        sum += AverageEmissionperMonth;
                                        count2++;
                                    }
                                }
                                var EstimatedAnnualizedCarbonFootprint = (sum / count2 * 12).ToString();
                                TransmissionInsertCostCalculator carbon = new TransmissionInsertCostCalculator();
                                carbon = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && x.Year == model.Years && months3.Contains(x.MonthName) && x.RegistartionNumber == model.RegistrationNumber).FirstOrDefault();
                                if (carbon != null)
                                {
                                    carbon.EstimatedCarbonAnnualizedValue = Math.Round(decimal.Parse(EstimatedAnnualizedCarbonFootprint), 2).ToString();
                                    carbon.FinancialYear = FY;
                                    dbcontext.SubmitChanges();
                                }
                            }
                            // End

                            var obj = dbcontext.TransmissionInsertCostCalculators.Where(X => X.Login == model.Login && X.CarbonEmissionReducePercentage != "" && (X.CarbonEmissionReviewYear != "" || X.CarbonEmissionReviewDate != null)).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();


                            if (obj != null)
                            {
                                Session["CarbonEmissionReducePercentage"] = obj.CarbonEmissionReducePercentage;

                                Session["CarbonEmissionReviewYear"] = obj.CarbonEmissionReviewYear;
                                if (obj.CarbonEmissionReviewDate != null && obj.CarbonEmissionReviewDate.ToString() != "")
                                {
                                    Session["CarbonEmissionReviewDate"] = DateTime.Parse(obj.CarbonEmissionReviewDate.ToString()).Date.ToString("mm/dd/yyyy");
                                }

                            }

                        }
                    }

                    catch (Exception ex)
                    {
                        result = new { status = "0", Result = "" };
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                result = new { status = "2", Result = "" };

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertCostCalculatorDate(string CarbonEmissionReducePercentage, string CarbonEmissionReviewYear, string CarbonEmissionReviewDate, string NumberofTress, string ProjectName, string RegNo = "")
        {
            string regNo = RegNo;

            TransmissionsCostCalculator model = new TransmissionsCostCalculator();
            bool validationStatus = true;
            var result = new { status = "1" };

            Log.Error("Validating Transmission Cost Calculator to stop auto script ", "Start");
            try
            {
            }
            catch (Exception ex)
            {
                result = new { status = "2" };
                Log.Error("Failed to validate auto script : " + ex.ToString(), "Failed");
            }
            if (Session["TransmissionCostCalculator"] != null)
            {
                model.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;

                if (validationStatus == true)
                {
                    Log.Error("InsertCostCalculatorRecord", "Start");

                    try
                    {



                        using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                        {

                            var MonthCarbonCalculationCount = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == model.MonthNames && x.Year == CarbonEmissionReviewYear).Count();
                            if (MonthCarbonCalculationCount > 1)
                            {
                                result = new { status = "3" };
                                Session["ReviewMessage"] = "";
                            }
                            else
                            {
                                var ObjReg = new List<TransmissionInsertCostCalculator>();
                                if (regNo != "")
                                {
                                    ObjReg = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && x.RegistartionNumber == regNo).ToList();
                                }
                                else
                                {
                                    ObjReg = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login).ToList();
                                }



                                if (ObjReg != null)
                                {
                                    foreach (var i in ObjReg)
                                    {
                                        i.CarbonEmissionReducePercentage = CarbonEmissionReducePercentage;
                                        i.CarbonEmissionReviewYear = CarbonEmissionReviewYear;
                                        if (CarbonEmissionReviewDate != "undefined" && CarbonEmissionReviewDate != null && CarbonEmissionReviewDate != "")
                                        {
                                            i.CarbonEmissionReviewDate = DateTime.Parse(CarbonEmissionReviewDate);
                                        }

                                        i.NumberofTress = NumberofTress;
                                        i.ProjectName = ProjectName;
                                    }

                                    dbcontext.SubmitChanges();
                                    Log.Info("Cost Calculator Form data saved into db successfully: ", this);
                                    Session["ReviewMessage"] = "You have successfully submitted your carbon footprint data.";
                                    //Sending Email for successful registration

                                    Data.Items.Item settingsItem;
                                    settingsItem = Context.Database.GetItem(Templates.MailConfiguration.ReviewDate.ReviewDateID);

                                    var mailTemplateItem = settingsItem;
                                    var fromMail = mailTemplateItem.Fields[Templates.MailConfiguration.ReviewDate.Fields.From];
                                    var body = mailTemplateItem.Fields[Templates.MailConfiguration.ReviewDate.Fields.Body];
                                    var subject = mailTemplateItem.Fields[Templates.MailConfiguration.ReviewDate.Fields.Subject];

                                    string bodyText = body.Value.Replace("[CarbonEmissionReducePercentage]", CarbonEmissionReducePercentage);

                                    if (CarbonEmissionReviewYear == "" || CarbonEmissionReviewYear == null)
                                    {
                                        bodyText = bodyText.Replace("[CarbonEmissionReviewYear]", "NA");
                                    }
                                    else
                                    {
                                        bodyText = bodyText.Replace("[CarbonEmissionReviewYear]", CarbonEmissionReviewYear);
                                    }
                                    if (CarbonEmissionReviewDate.ToString() == "" || CarbonEmissionReviewDate == null)
                                    {
                                        bodyText = bodyText.Replace("[CarbonEmissionReviewDate]", "NA");
                                    }
                                    else
                                    {
                                        bodyText = bodyText.Replace("[CarbonEmissionReviewDate]", CarbonEmissionReviewDate.ToString());
                                    }


                                    MailMessage mail = new MailMessage
                                    {
                                        From = new MailAddress(fromMail.Value),
                                        Body = bodyText,
                                        Subject = subject.Value,
                                        IsBodyHtml = true,

                                    };
                                    var loginUserDetail = dbcontext.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == model.Login || x.MobileNumber == model.Login).FirstOrDefault();
                                    mail.To.Add(loginUserDetail.EmailId);

                                    MainUtil.SendMail(mail);



                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        result = new { status = "0" };
                        Console.WriteLine(ex);
                    }

                }

            }
            else
            {
                result = new { status = "2" };

            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewCostCalculatorDetails(CarbonOffsetvalues model)
        {
            if (model.RegistrationNumber == null || model.RegistrationNumber == "")
            {
                bool validationStatus = true;
                var result = new { status = "1", Result = "" };
                Log.Error("Validating Transmission Offset Cost Calculator to stop auto script ", "Start");
                if (Session["TransmissionCostCalculator"] != null)
                {
                    model.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
                    if (validationStatus == true)
                    {
                        Log.Error("InsertCostCalculatorRecord", "Start");
                        try
                        {
                            using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                            {
                                var objs = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login).FirstOrDefault();
                                if (objs != null)
                                {
                                    model.Id = Guid.NewGuid();
                                    model.Login = objs.Login;
                                    model.RegistrationNumber = objs.RegistartionNumber;
                                    //code to save the average offset emission per month in TransmissionInsertCostCalculatorTable
                                    objs.AverageOffsetEmissionperMonth = Math.Round(decimal.Parse(model.AverageOffsetEmissionperMonth), 2).ToString();
                                    // code to save the carbon offset values
                                    TransmissionCarbonOffsetValue offset = new TransmissionCarbonOffsetValue();
                                    offset.Id = model.Id;
                                    offset.CostCalculatorID = objs.Id.ToString();
                                    offset.Login = model.Login;
                                    offset.RegistartionNumber = model.RegistrationNumber;
                                    offset.MonthName = model.MonthNames;
                                    offset.Year = model.Years;
                                    offset.PersonalTransport = model.PersonalTransport;
                                    offset.PublicTransport = model.PublicTransport;
                                    offset.OnlineMeeting = model.OnlineMeeting;
                                    offset.FiveStarAppliances = model.FiveStarAppliances;
                                    offset.NumberofTreesNeeded = model.NumberofTreesNeeded;
                                    offset.PlantationTreesNeeded = model.NumberofPlantationProjectTreesNeeded;
                                    offset.FundNeededtoPlantTrees = model.FundNeededtoPlantTrees;
                                    offset.OffsetEmissionfromDomesticUse = model.OffsetEmissionfromDomesticUse;
                                    offset.OffsetEmissionfromTransportation = model.OffsetEmissionfromTransportation;
                                    offset.OffsetEmissionfromAirTrips = model.OffsetEmissionfromAirTrips;
                                    offset.TotalOffsetCarbonEmission = model.TotalOffsetCarbonEmission;
                                    offset.AverageOffsetEmissionperMonth = model.AverageOffsetEmissionperMonth;
                                    offset.OffsetAnnualCarbonFootprint = model.OffsetAnnualCarbonFootprint;
                                    offset.DifferenceofAnnualCarbonFootprint = model.CarbonAnnualCarbonFootprintDifference;
                                    offset.FormSubmitOn = model.FormSubmitOn;
                                    offset.NoOfTreesNeededValue = model.NumberofTreesNeededValue;
                                    offset.PlantationTreesNeededValue = model.NumberofPlantationProjectTreesNeededValue;
                                    offset.FundNeededToPlantTreesValue = model.FundNeededtoPlantTreesValue;
                                    offset.PersonalTransportDropdownValue = model.PersonalTransportDropdownValue;
                                    offset.PersonalTransportValue = model.PersonalTransportValue;
                                    offset.PublicTransportValue = model.PublicTransportValue;
                                    offset.OnlineMeetingValue = model.OnlineMeetingValue;
                                    offset.FiveStarAppliancesValue = model.FiveStarAppliancesValue;
                                    if (model.PersonalTransportDropdownValue != null)
                                    {
                                        switch (model.PersonalTransportDropdownValue)
                                        {
                                            case "0.12075":
                                                offset.OffSetWithCarPooling = model.PersonalTransport;
                                                break;
                                            case "0.1288":
                                                offset.OffSetwithCNG = model.PersonalTransport;
                                                break;
                                            case "0.10626":
                                                offset.OffSetwithElectricVehicle = model.PersonalTransport;
                                                break;
                                            case "0.161":
                                                offset.OffSetwithCycle = model.PersonalTransport;
                                                break;
                                            case "0.145839":
                                                offset.OffSetWithPublicTransport = model.PersonalTransport;
                                                break;
                                        }
                                    }
                                    dbcontext.TransmissionCarbonOffsetValues.InsertOnSubmit(offset);
                                    // Code to calculate Estimated Annualized Offset Emission Value
                                    string[] months1 = { "January", "February", "March" };
                                    string[] months2 = { "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                    string[] months3 = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                    int altYear = 0;
                                    List<TransmissionInsertCostCalculator> Data = null;
                                    if (months1.Contains(model.MonthNames))
                                    {
                                        altYear = Int32.Parse(model.Years) - 1;
                                        Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == model.Years && months1.Contains(x.MonthName)) || (x.Year == altYear.ToString() && months2.Contains(x.MonthName)))).ToList();
                                    }
                                    if (months2.Contains(model.MonthNames))
                                    {
                                        altYear = Int32.Parse(model.Years) + 1;
                                        Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == altYear.ToString() && months1.Contains(x.MonthName)) || (x.Year == model.Years && months2.Contains(x.MonthName)))).ToList();
                                    }
                                    decimal sum = 0.00m;
                                    if (Data.Count > 0)
                                    {
                                        foreach (var item in Data)
                                        {
                                            decimal AverageOffsetEmissionperMonth = Math.Round(decimal.Parse(item.AverageOffsetEmissionperMonth), 2);
                                            sum += AverageOffsetEmissionperMonth;
                                        }
                                    }
                                    var EstimatedAnnualizedOffsetEmission = sum.ToString();
                                    objs.AnnualFYOffsetEmission = Math.Round(decimal.Parse(EstimatedAnnualizedOffsetEmission), 2).ToString();
                                    dbcontext.SubmitChanges();
                                    Log.Info("Offset Cost Calculator value saved into db successfully: ", this);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result = new { status = "0", Result = "" };
                            Console.WriteLine(ex);
                        }
                    }
                }
                else
                {
                    result = new { status = "2", Result = "" };
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool validationStatus = true;
                var result = new { status = "1", Result = "" };
                Log.Error("Validating Transmission Offset Cost Calculator to stop auto script ", "Start");
                if (Session["TransmissionCostCalculator"] != null)
                {
                    model.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
                    if (validationStatus == true)
                    {
                        Log.Error("InsertCostCalculatorRecord", "Start");
                        try
                        {
                            using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                            {
                                var objs = dbcontext.TransmissionInsertCostCalculators.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login && x.RegistartionNumber == model.RegistrationNumber).FirstOrDefault();
                                var objoffset = dbcontext.TransmissionCarbonOffsetValues.Where(x => x.MonthName == model.MonthNames && x.Year == model.Years && x.Login == model.Login && x.RegistartionNumber == model.RegistrationNumber).FirstOrDefault();
                                if (objs != null && objoffset != null)
                                {
                                    //TransmissionCarbonOffsetValue ObjRecord = new TransmissionCarbonOffsetValue();
                                    objs.AverageOffsetEmissionperMonth = Math.Round(decimal.Parse(model.AverageOffsetEmissionperMonth), 2).ToString();
                                    objoffset.PersonalTransport = model.PersonalTransport;
                                    objoffset.PersonalTransportDropdownValue = model.PersonalTransportDropdownValue;
                                    objoffset.PersonalTransportValue = model.PersonalTransportValue;
                                    objoffset.PublicTransport = model.PublicTransport;
                                    objoffset.PublicTransportValue = model.PublicTransportValue;
                                    objoffset.OnlineMeeting = model.OnlineMeeting;
                                    objoffset.OnlineMeetingValue = model.OnlineMeetingValue;
                                    objoffset.FiveStarAppliances = model.FiveStarAppliances;
                                    objoffset.FiveStarAppliancesValue = model.FiveStarAppliancesValue;
                                    objoffset.NumberofTreesNeeded = model.NumberofTreesNeeded;
                                    objoffset.PlantationTreesNeeded = model.NumberofPlantationProjectTreesNeeded;
                                    objoffset.NoOfTreesNeededValue = model.NumberofTreesNeededValue;
                                    objoffset.PlantationTreesNeededValue = model.NumberofPlantationProjectTreesNeededValue;
                                    objoffset.FundNeededtoPlantTrees = model.FundNeededtoPlantTrees;
                                    objoffset.FundNeededToPlantTreesValue = model.FundNeededtoPlantTreesValue;
                                    objoffset.OffsetEmissionfromDomesticUse = model.OffsetEmissionfromDomesticUse;
                                    objoffset.OffsetEmissionfromTransportation = model.OffsetEmissionfromTransportation;
                                    objoffset.OffsetEmissionfromAirTrips = model.OffsetEmissionfromAirTrips;
                                    objoffset.TotalOffsetCarbonEmission = model.TotalOffsetCarbonEmission;
                                    objoffset.AverageOffsetEmissionperMonth = model.AverageOffsetEmissionperMonth;
                                    objoffset.OffsetAnnualCarbonFootprint = model.OffsetAnnualCarbonFootprint;
                                    if (model.PersonalTransportDropdownValue != null)
                                    {
                                        objoffset.OffSetWithCarPooling = null;
                                        objoffset.OffSetwithCNG = null;
                                        objoffset.OffSetwithElectricVehicle = null;
                                        objoffset.OffSetwithCycle = null;
                                        objoffset.OffSetWithPublicTransport = null;
                                        switch (model.PersonalTransportDropdownValue)
                                        {
                                            case "0.12075":
                                                objoffset.OffSetWithCarPooling = model.PersonalTransport;
                                                break;
                                            case "0.1288":
                                                objoffset.OffSetwithCNG = model.PersonalTransport;
                                                break;
                                            case "0.10626":
                                                objoffset.OffSetwithElectricVehicle = model.PersonalTransport;
                                                break;
                                            case "0.161":
                                                objoffset.OffSetwithCycle = model.PersonalTransport;
                                                break;
                                            case "0.145839":
                                                objoffset.OffSetWithPublicTransport = model.PersonalTransport;
                                                break;
                                        }
                                    }
                                    // Code to calculate Estimated Annualized Offset Emission Value
                                    string[] months1 = { "January", "February", "March" };
                                    string[] months2 = { "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                    string[] months3 = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                                    int altYear = 0;
                                    List<TransmissionInsertCostCalculator> Data = null;
                                    if (months1.Contains(model.MonthNames))
                                    {
                                        altYear = Int32.Parse(model.Years) - 1;
                                        Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == model.Years && months1.Contains(x.MonthName)) || (x.Year == altYear.ToString() && months2.Contains(x.MonthName)))).ToList();
                                    }
                                    if (months2.Contains(model.MonthNames))
                                    {
                                        altYear = Int32.Parse(model.Years) + 1;
                                        Data = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == model.Login && ((x.Year == altYear.ToString() && months1.Contains(x.MonthName)) || (x.Year == model.Years && months2.Contains(x.MonthName)))).ToList();
                                    }
                                    decimal sum = 0.00m;
                                    if (Data.Count > 0)
                                    {
                                        foreach (var item in Data)
                                        {
                                            decimal AverageOffsetEmissionperMonth = Math.Round(decimal.Parse(item.AverageOffsetEmissionperMonth), 2);
                                            sum += AverageOffsetEmissionperMonth;
                                        }
                                    }
                                    var EstimatedAnnualizedOffsetEmission = sum.ToString();
                                    objs.AnnualFYOffsetEmission = Math.Round(decimal.Parse(EstimatedAnnualizedOffsetEmission), 2).ToString();
                                    dbcontext.SubmitChanges();
                                    Log.Info("Offset Cost Calculator value successfully updated into db", this);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result = new { status = "0", Result = "" };
                            Console.WriteLine(ex);
                        }
                    }
                }
                else
                {
                    result = new { status = "2", Result = "" };
                }
                return Json(result, JsonRequestBehavior.AllowGet);

            }
        }



        [HttpPost]
        public ActionResult InsertSubscribeUsFormdetail(TransmissionContactModel m)
        {


            Log.Error("InsertSubscribeUsFormdetail", "Start");
            var result = new { status = "1" };
            try
            {
                TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                TransmissionContactFormRecord r = new TransmissionContactFormRecord();

                r.Name = m.Name;
                r.Email = m.Email;
                r.Mobile = m.Mobile;
                r.Message = m.Message;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.TransmissionContactFormRecords.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }



            try
            {
                string emailText = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailText", "");
                string message = "";
                message = "Hello<br><br>" + emailText + "<br><br>";
                message = message + "<br>Email-Id: " + m.Email + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/SubscribeForm/EmailSubject", "");
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

        public ActionResult CarbonCalculatorGenerateOTP(TransmissionOTPModel model)
        {
            ActionResult actionResult;
            string msg;
            var result = new { status = "0" };
            try
            {
                var currdate = DateTime.Now;
                var befortime = currdate.AddMinutes(-60);
                TransmissionContactFormRecordDataContext pgd = new TransmissionContactFormRecordDataContext();
                var data = pgd.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == model.OTPFor && x.CreatedDate >= befortime && x.CreatedDate <= currdate).Count();

                if (data >= 4)
                {
                    result = new { status = "3" };
                    actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
                    return actionResult;
                }

                string generatedotp = this.transmissionrepos.TransmissionStoreGeneratedOtp(model);
                TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                if (model.IsMobile)
                {
                    try
                    {

                        if (rdb.Transmission_CarbonCalculator_RegistrationForms.Any(x => x.MobileNumber == model.OTPFor))
                        {

                        }
                        else
                        {
                            msg = "User not registered";
                            Session["ErrorMsg"] = msg;
                            result = new { status = "2" };
                            actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
                            return actionResult;
                        }

                        var API = DictionaryPhraseRepository.Current.Get("/SMS/API", "");
                        var apiurl = string.Format(API, model.OTPFor, generatedotp);                    

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Error("OTP Api call success. https://vknr1.api.infobip.com/", this);
                            result = new { status = "1" };
                            Session["Mobilenumber"] = model.OTPFor;
                            Session["Counter"] = ((int)Session["Counter"]) + 1;
                        }
                        else
                        {
                            Log.Error("OTP Api call failed. https://vknr1.api.infobip.com/", this);
                            result = new { status = "0" };
                        }
                    }
                    catch (Exception exception)
                    {
                        Exception ex = exception;
                        Log.Error(string.Format("{0}", 0), ex, this);
                    }
                }
                else
                {
                    if (rdb.Transmission_CarbonCalculator_RegistrationForms.Any(x => x.EmailId == model.OTPFor))
                    {

                    }
                    else
                    {
                        msg = "User not registered";
                        Session["ErrorMsg"] = msg;
                        result = new { status = "4" };
                        actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
                        return actionResult;
                    }


                    SendEmailCarbonCalculatorOTP(model.OTPFor, null, model.OTPType, generatedotp);
                }


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

        public ActionResult CarbonCalculatorRegistrationGenerateOTP(TransmissionOTPModel model)
        {
            ActionResult actionResult;
            string msg;
            var result = new { status = "0" };
            try
            {

                var currdate = DateTime.Now;
                var befortime = currdate.AddMinutes(-60);
                TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                var data = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == model.OTPFor && x.CreatedDate >= befortime && x.CreatedDate <= currdate && x.OTPType == model.OTPType).Count();

                if (data >= 4)
                {
                    result = new { status = "2" };
                    actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
                    return actionResult;
                }

                string generatedotp = this.transmissionrepos.TransmissionStoreGeneratedOtp(model);

                if (model.IsMobile)
                {
                    try
                    {

                        var API = DictionaryPhraseRepository.Current.Get("/SMS/API", "");
                        var apiurl = string.Format(API, model.OTPFor, generatedotp);

                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Error("OTP Api call success. https://vknr1.api.infobip.com/", this);
                            result = new { status = "1" };
                            Session["Mobilenumber"] = model.OTPFor;
                            Session["Counter"] = ((int)Session["Counter"]) + 1;
                        }
                        else
                        {
                            Log.Error("OTP Api call failed. https://vknr1.api.infobip.com/", this);
                            result = new { status = "0" };
                        }
                    }
                    catch (Exception exception)
                    {
                        Exception ex = exception;
                        Log.Error(string.Format("{0}", 0), ex, this);
                    }
                }
                else
                {


                    SendEmailCarbonCalculatorOTP(model.OTPFor, null, model.OTPType, generatedotp);
                }


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
        public ActionResult CarbonCalculatorVerifyOTP(TransmissionOTPModel model)
        {
            ActionResult actionResult;
            TransmissionContactFormRecordDataContext pgd = new TransmissionContactFormRecordDataContext();
            var result = new { status = "0" };
            try
            {

                if (!string.IsNullOrEmpty(model.OTPFor))
                {
                    var data = pgd.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == model.OTPFor && x.OTP == model.OTP && x.OTPType == model.OTPType).FirstOrDefault();
                    result = new { status = data.OTP };
                    if (string.Equals(data.OTP, model.OTP))
                    {
                        result = new { status = "1" };
                        data.Status = true;
                        pgd.SubmitChanges();


                    }
                }



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


        [HttpGet]
        public ActionResult TransmissionCarbonCalculatorLogin()
        {
            //var check = !string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["Login"].ToString()) ? Session["Login"].ToString() : (System.Web.HttpContext.Current.Session["Login"] = 1234567890);

            // Session.Abandon();
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", ""));
            if (Request.QueryString["gid"] != null)
            {
                Session["LoginDeatils"] = Request.QueryString["gid"].ToString();
            }
            else
            {
                Session["LoginDeatils"] = null;
            }
            return View();

        }


        public ActionResult TransmissionCalculatorHistory(TransmissionsCostCalculator pm)
        {
            Session["Message"] = null;

            try
            {
                TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                Transmission_CarbonCalculator_RegistrationForm pgr = new Transmission_CarbonCalculator_RegistrationForm();
                TransmissionRepository model = new TransmissionRepository();


                if (UserSessionTransmission.CostCalculatorSesstion == null)
                {
                    return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                }
                var objs = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == UserSessionTransmission.CostCalculatorSesstion.LoginId || x.MobileNumber == UserSessionTransmission.CostCalculatorSesstion.LoginId).FirstOrDefault();

                Session["UserName"] = objs.FullName;
                Session["CompanyName"] = objs.Company;
                List<TransmissionInsertCostCalculator> FinalList = new List<TransmissionInsertCostCalculator>();
                if (Session["TransmissionCostCalculator"] != null)
                {
                    pm.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
                    model.FinalHistoryLists = model.HistoryList(pm.Login);
                    if (model.FinalHistoryLists.Count > 0)
                    {
                        var yearlist = model.FinalHistoryLists.Select(o => o.FinancialYear).Distinct().ToList();
                        var ListofModel = new List<FYModel>();


                        if (yearlist != null && yearlist.Count > 0)
                        {
                            foreach (var item in yearlist)
                            {
                                var FYModelOBJ = new FYModel();
                                FYModelOBJ.year = item;
                                FYModelOBJ.value = model.FinalHistoryLists.Where(x => x.FinancialYear == item).OrderByDescending(y => y.FormUpdatedOn).FirstOrDefault().EstimatedCarbonAnnualizedValue;
                                FYModelOBJ.offsetValue = model.FinalHistoryLists.Where(x => x.FinancialYear == item).OrderByDescending(y => y.FormUpdatedOn).FirstOrDefault().AnnualFYOffsetEmission;
                                ListofModel.Add(FYModelOBJ);
                            }
                            model.ListofModel = ListofModel;
                            ViewBag.totalEstimated = model.ListofModel;
                        }


                        return View(model);
                    }
                    else
                    {
                        Session["Message"] = "No results found";
                        return View();
                    }
                }
                else
                {
                    return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult TransmissionAuditTrailHistory(TransmissionsCostCalculator pm)
        {
            Session["Message"] = null;

            try
            {
                TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                Transmission_CarbonCalculator_RegistrationForm pgr = new Transmission_CarbonCalculator_RegistrationForm();
                TransmissionRepository model = new TransmissionRepository();


                var objs = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == UserSessionTransmission.CostCalculatorSesstion.LoginId || x.MobileNumber == UserSessionTransmission.CostCalculatorSesstion.LoginId).FirstOrDefault();

                Session["UserName"] = objs.FullName;
                Session["CompanyName"] = objs.Company;
                List<TransmissionInsertCostCalculator> FinalList = new List<TransmissionInsertCostCalculator>();
                if (Session["TransmissionCostCalculator"] != null)
                {
                    pm.Login = UserSessionTransmission.CostCalculatorSesstion.LoginId;
                    var models = model.HistoryList(pm.Login);
                    if (models.Count > 0)
                    {
                        return View(models);
                    }
                    else
                    {
                        Session["Message"] = "No results found";
                        return View();
                    }
                }
                else
                {
                    return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                }
            }
            catch
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult TransmissionCarbonCalculatorLogin(TransmissionCostCalculatorModel pm)
        {
            string msg = "";
            try
            {

                TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                Transmission_CarbonCalculator_RegistrationForm pgr = new Transmission_CarbonCalculator_RegistrationForm();


                //var data1 = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == pm.LoginId && x.Isactive == 1).FirstOrDefault();

                //if (data1 != null)
                //{
                //    msg = "User is already logged in somewhere. Please logout and try again";
                //    Session["ErrorMsg"] = msg;
                //    return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                //}



                if (rdb.Transmission_CarbonCalculator_RegistrationForms.Any(x => x.MobileNumber == pm.LoginId || x.EmailId == pm.LoginId))
                {
                    if (pm.LoginId != null)
                    {

                        var currdate = DateTime.Now;
                        var befortime = currdate.AddMinutes(-60);

                        var data2 = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == pm.LoginId && x.Isactive > 0 && x.LastAttemptdate >= befortime && x.LastAttemptdate <= currdate).FirstOrDefault();
                        if (data2 != null)
                        {
                            if (data2.Isactive > 3)
                            {

                                msg = "Wrong Attempt Exceeds 4 times.Try after sometime";
                                Session["ErrorMsg"] = msg;
                                return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                            }
                        }

                        var Mobile = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == pm.LoginId && x.OTP == pm.OTP && x.OTPType == "login").FirstOrDefault();
                        if (Mobile != null)
                        {

                            Mobile.Status = true;
                            rdb.SubmitChanges();
                            var registerUser = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.MobileNumber == pm.LoginId || x.EmailId == pm.LoginId && x.Status == true).FirstOrDefault();

                            Transmission_CarbonCalculator_LoginHistory entity = new Transmission_CarbonCalculator_LoginHistory()
                            {
                                Id = Guid.NewGuid(),
                                LoginId = pm.LoginId,
                                Created_Date = DateTime.Now.Date,

                            };
                            rdb.Transmission_CarbonCalculator_LoginHistories.InsertOnSubmit(entity);
                            rdb.SubmitChanges();
                            Session["TransmissionCostCalculator"] = pm;
                            Session["Login"] = pm.LoginId;

                            //TransmissionContactFormRecordDataContext pgd = new TransmissionContactFormRecordDataContext();
                            //var data = pgd.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == pm.LoginId).FirstOrDefault();

                            //if (data != null)
                            //{
                            //    data.Isactive = 1;
                            //}

                            //pgd.SubmitChanges();
                            Markblankattempt(pm.LoginId);
                            return this.Redirect("/Carbon-Footprint-Calculator/History?LoginID=" + pm.LoginId + "");
                        }
                        else
                        {

                            msg = "Entered Mobile no. or OTP is invalid.";
                            Session["ErrorMsg"] = msg;
                            Transmission_CarbonCalculator_LoginHistory entity = new Transmission_CarbonCalculator_LoginHistory()
                            {
                                Id = Guid.NewGuid(),
                                LoginId = pm.LoginId,
                                Created_Date = DateTime.Now.Date,
                                Status = false

                            };
                            rdb.Transmission_CarbonCalculator_LoginHistories.InsertOnSubmit(entity);
                            rdb.SubmitChanges();
                            
                            Markwrongattempt(pm.LoginId);

                            return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                        }
                    }

                }
                else
                {
                    msg = "Entered Mobile no. or OTP is invalid.";
                    Session["ErrorMsg"] = msg;
                  
                    var loginhistories = rdb.Transmission_CarbonCalculator_LoginHistories.Where(x => x.LoginId == (pm.LoginId) && x.Created_Date.GetValueOrDefault().Date == DateTime.Now.Date && x.Status == false).Count();
                    if (loginhistories <= 2)
                    {
                        Transmission_CarbonCalculator_LoginHistory entity = new Transmission_CarbonCalculator_LoginHistory()
                        {
                            Id = Guid.NewGuid(),
                            LoginId = (pm.LoginId),
                            Created_Date = DateTime.Now.Date,
                            Status = false

                        };
                        rdb.Transmission_CarbonCalculator_LoginHistories.InsertOnSubmit(entity);
                        rdb.SubmitChanges();
                        return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                    }
                    else
                    {

                        msg = "You have exceeded the maximum Login attempts for Today";
                        Session["ErrorMsg"] = msg;
                    }
                    msg = "Invalid Login Credentials. Please enter the Credentials carefully";
                    Session["ErrorMsg"] = msg;
                }

            }
            catch (Exception ex)
            {
                msg = "Something went wrong please try again.";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }

            return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
        }

        private void Markwrongattempt(string LoginId)
        {

            TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
            var data1 = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == LoginId).FirstOrDefault();

            if (data1.Isactive == null)
            {
                data1.Isactive = 0;
               
            }
            data1.Isactive = data1.Isactive + 1;
            data1.LastAttemptdate = DateTime.Now;
            rdb.SubmitChanges();
        }
        private void Markblankattempt(string LoginId)
        {
            TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
            var data1 = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == LoginId).ToList();
            data1.ForEach(a => a.Isactive = 0);
            rdb.SubmitChanges();
        }


        [HttpGet]
        public ActionResult CarbonCalculatorRegistration()
        {
            if (Request.QueryString["gid"] != null)
            {
                Session["CarbonCalculatorLogin"] = Request.QueryString["gid"].ToString();
            }
            else
            {
                Session["CarbonCalculatorLogin"] = null;
            }
            return View();
        }


        public bool sendEmailCarbonCalculatorRegistration(string to, string subject, string body, string from)
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
                //ContentType ct = new ContentType("application/pdf");
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


        [HttpPost]
        public ActionResult CarbonCalculatorRegistration(CarbonCalculatorRegistrationModel pm)
        {
            string msg = "";
            string url = "";
            try
            {
                TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
                Transmission_CarbonCalculator_RegistrationForm pgr = new Transmission_CarbonCalculator_RegistrationForm();
                Transmission_Carbon_CalculatorOTPHistory userOtpHistory = new Transmission_Carbon_CalculatorOTPHistory();
                if (rdb.Transmission_CarbonCalculator_RegistrationForms.Any(x => x.MobileNumber == pm.MobileNumber || x.EmailId == pm.EmailId))
                {
                    Session["ErrorMsg"] = "User Already Registered";
                    return this.Redirect("/Carbon-Footprint-Calculator/User-Registration");


                }
                else
                {
                    var regUserMobileOTP = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == pm.MobileNumber).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    var regUserEmailOTP = rdb.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.OTPFor == pm.EmailId).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                    if (regUserMobileOTP != null && regUserEmailOTP != null)
                    {
                        if (regUserMobileOTP.OTP == pm.OTP && regUserEmailOTP.OTP == pm.EmailOTP)
                        {
                            pm.Id = Guid.NewGuid();
                            pgr.Id = pm.Id;
                            pgr.FullName = pm.FullName;
                            pgr.EmailId = pm.EmailId;
                            pgr.MobileNumber = pm.MobileNumber;
                            pgr.Company = pm.Company;
                            pgr.Status = true;
                            pgr.Created_Date = System.DateTime.Now; ;
                            pgr.Modified_Date = System.DateTime.Now; ;

                            #region Insert to DB
                            rdb.Transmission_CarbonCalculator_RegistrationForms.InsertOnSubmit(pgr);
                            rdb.SubmitChanges();


                            //Sending Email for successful registration
                            Data.Items.Item settingsItem;
                            settingsItem = Context.Database.GetItem(Templates.MailConfiguration.costCalculator.costCalculatorRegistration);

                            var mailTemplateItem = settingsItem;
                            var fromMail = mailTemplateItem.Fields[Templates.MailConfiguration.costCalculator.Fields.From];
                            var body = mailTemplateItem.Fields[Templates.MailConfiguration.costCalculator.Fields.Body];
                            var subject = mailTemplateItem.Fields[Templates.MailConfiguration.costCalculator.Fields.Subject];

                            string bodyText = body.Value.Replace("[FullName]", pm.FullName);
                            bodyText = bodyText.Replace("[EmailId]", pm.EmailId);
                            bodyText = bodyText.Replace("[MobileNumber]", pm.MobileNumber);
                            bodyText = bodyText.Replace("[Company]", pm.Company);


                            MailMessage mail = new MailMessage
                            {
                                From = new MailAddress(fromMail.Value),
                                Body = bodyText,
                                Subject = subject.Value,
                                IsBodyHtml = true,

                            };
                            mail.To.Add(pm.EmailId);
                            MainUtil.SendMail(mail);
                            Session["RegSuccessMsg"] = "User successfully Registered";


                            Session["CarbonCalculatorLogin"] = "User registration is successful. Kindly use the registered login id for login";
                            return this.Redirect("/Carbon-Footprint-Calculator/User-Login");


                        }
                        #endregion
                        else
                        {
                            Session["ErrorMsg"] = "Please enter a valid OTP";

                        }
                    }
                  
                }


            }
            catch (Exception ex)
            {
                msg = "Something went wrong please try again after sometime.";
                Session["ErrorMsg"] = msg;
                url = "/Carbon-Footprint-Calculator/User-Registration";
                Console.WriteLine(ex);
            }
            return Redirect("/Carbon-Footprint-Calculator/User-Registration");

        }

        public ActionResult Logout()
        {
            Session["TransmissionCostCalculator"] = null;
            string _loginid = string.Empty;
            if (Session["Login"] != null)
            {
                _loginid = Session["Login"].ToString();
            }

            TransmissionContactFormRecordDataContext pgd = new TransmissionContactFormRecordDataContext();
            var data = pgd.Transmission_Carbon_CalculatorOTPHistories.Where(x => x.Isactive == 1).ToList();
            data.ForEach(a => a.Isactive = 0);


            pgd.SubmitChanges();
            return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
        }



        [HttpPost]
        public ActionResult DisplayCarbonFootPrintReviewSection()
        {

            TransmissionsCostCalculator model = new TransmissionsCostCalculator();
            try
            {
                if (Session["TransmissionCostCalculator"] != null)
                {
                    using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                    {
                        var dbObjs = dbcontext.TransmissionInsertCostCalculators.Where(x => x.Login == UserSessionTransmission.CostCalculatorSesstion.LoginId).ToList();

                        //TransmissionRepository mode = new TransmissionRepository();
                        //model.Login = UserSession.CostCalculatorSesstion.LoginId;

                        if (dbObjs.Count >= 1)
                        {
                            var obj = dbcontext.TransmissionInsertCostCalculators.Where(X => X.Login == model.Login && X.CarbonEmissionReducePercentage != null && (X.CarbonEmissionReviewYear != null || X.CarbonEmissionReviewDate != null)).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();


                            if (obj != null)
                            {
                                TempData["CarbonEmissionReducePercentage"] = obj.CarbonEmissionReducePercentage;
                                TempData["CarbonEmissionReviewDate"] = obj.CarbonEmissionReviewDate;
                                TempData["CarbonEmissionReviewYear"] = obj.CarbonEmissionReviewYear;

                                return View();
                            }
                        }
                    }

                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return base.View("/Views/Transmission/Sublayouts/TransmissionVendorInquiry.cshtml", new TransmissionVendorModel());
        }

        private void SendEmailCarbonCalculatorOTP(string Email = null, string Name = null, string Type = null, string OTP = null)
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            var settingsItem = Context.Database.GetItem("{BD3F2B66-5B39-4FE5-9F05-6C5542463BFE}");
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailConfiguration.costCalculator.Fields.From];

            try
            {

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[Templates.MailConfiguration.costCalculator.Fields.Body];
                var subject = mailTemplateItem.Fields[Templates.MailConfiguration.costCalculator.Fields.Subject];

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
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", Name);
                mail.Body = mail.Body.Replace("#OTP#", OTP);
                mail.Subject = mail.Subject.Replace("#TYPE", Type);
                MainUtil.SendMail(mail);


            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }

        [HttpGet]
        public ActionResult ViewCarbonFootprint(string RegistrationNo)
        {
            ViewCarbonCalculator model = new ViewCarbonCalculator();
            try
            {
                if (Session["TransmissionCostCalculator"] != null)
                {
                    TransmissionContactFormRecordDataContext dbcontext1 = new TransmissionContactFormRecordDataContext();
                    var obj1 = dbcontext1.TransmissionInsertCostCalculators.Where(X => X.Login == UserSessionTransmission.CostCalculatorSesstion.LoginId && X.RegistartionNumber == RegistrationNo && X.CarbonEmissionReducePercentage != "" && (X.CarbonEmissionReviewYear != "" || X.CarbonEmissionReviewDate != null)).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();
                    if (obj1 != null)
                    {
                        Session["CarbonEmissionReducePercentage"] = obj1.CarbonEmissionReducePercentage;
                        if (obj1.CarbonEmissionReviewDate != null)
                        {
                            Session["CarbonEmissionReviewDate"] = DateTime.Parse(obj1.CarbonEmissionReviewDate.ToString()).Date.ToString("mm/dd/yyyy");
                        }
                        Session["CarbonEmissionReviewYear"] = obj1.CarbonEmissionReviewYear;
                    }
                    using (TransmissionContactFormRecordDataContext dbcontext = new TransmissionContactFormRecordDataContext())
                    {
                        var CarbonCalculationRecord = (from rc in dbcontext.TransmissionInsertCostCalculatorHistories
                                                       where (rc.RegistartionNumber == RegistrationNo)
                                                       select rc).FirstOrDefault();
                        if (CarbonCalculationRecord != null)
                        {
                            var userRegistrationDetail = dbcontext.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == CarbonCalculationRecord.Login || x.MobileNumber == CarbonCalculationRecord.Login).FirstOrDefault();

                            model.FullName = userRegistrationDetail.FullName;
                            model.CompanyName = userRegistrationDetail.Company;
                            model.EmailID = userRegistrationDetail.EmailId;
                            model.Login = userRegistrationDetail.MobileNumber;
                            model.RegistrationDate = userRegistrationDetail.Created_Date.ToString();
                            model.RegistrationNumber = CarbonCalculationRecord.RegistartionNumber;
                            model.MonthNames = CarbonCalculationRecord.MonthName;
                            model.Years = CarbonCalculationRecord.Year;
                            model.TotalFamilyMembers = CarbonCalculationRecord.TotalFamilyMembers;
                            model.ElectricityConsumedatResidences = CarbonCalculationRecord.ElectricityConsumedAtResidence + " kg CO2eq";
                            model.CNGUseds = CarbonCalculationRecord.CNGUsed + " kg CO2eq";
                            model.LPGCylinders = CarbonCalculationRecord.LPGUsed + " kg CO2eq";
                            model.DieselConsumptions = CarbonCalculationRecord.DieselConsumption + " kg CO2eq";
                            model.PetrolConsumptions = CarbonCalculationRecord.PetrolConsumption + " kg CO2eq";
                            model.AutoRikshaws = CarbonCalculationRecord.CNGAutoRickshaw + " kg CO2eq";
                            model.Buses = CarbonCalculationRecord.BusUse + " kg CO2eq";
                            model.Trains = CarbonCalculationRecord.TrainUse + " kg CO2eq";
                            model.TotalTrips = CarbonCalculationRecord.NumberofTrips;
                            IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
                            string domestic = CarbonCalculationRecord.EmissionfromDomesticUse;
                            model.TotalDomesticUses = CarbonCalculationRecord.EmissionfromDomesticUse;
                            model.TotalTransportationUses = CarbonCalculationRecord.EmissionfromTransportation;
                            var totlaemission = Decimal.Parse(CarbonCalculationRecord.EmissionfromDomesticUse) + Decimal.Parse(CarbonCalculationRecord.EmissionfromTransportation) + Decimal.Parse(CarbonCalculationRecord.NumberofTrips);
                            model.TotalCarbonEmission = totlaemission.ToString();
                            model.EmployeeTotalemissionsperMonths = CarbonCalculationRecord.AverageEmissionperMonth;
                            model.LandNeeded = CarbonCalculationRecord.LandNeededtoPlantTrees + " Hectares";
                            model.NumberOfTreesNeeded = CarbonCalculationRecord.NumberofTreesNeeded + " Nos";
                            model.AverageAnnualCarbonFootprints = CarbonCalculationRecord.AverageAnnualCarbonFootprint;
                            model.AnnualCarbonFootprints = CarbonCalculationRecord.AnnualCarbonFootprint + " Tonnes";

                            model.FormSubmitOn = CarbonCalculationRecord.FormSubmitOn ?? DateTime.Now;
                            var OffsetCarbonCalculationRecord = (from rc in dbcontext.TransmissionCarbonOffsetValues
                                                                 where (rc.RegistartionNumber == RegistrationNo)
                                                                 select rc).FirstOrDefault();
                            model.PersonalTransport = OffsetCarbonCalculationRecord.PersonalTransport + " kg CO2eq";
                            model.PublicTransport = OffsetCarbonCalculationRecord.PublicTransport + " kg CO2eq";
                            model.OnlineMeeting = OffsetCarbonCalculationRecord.OnlineMeeting + " kg CO2eq";
                            model.FiveStarAppliances = OffsetCarbonCalculationRecord.FiveStarAppliances + " kg CO2eq";
                            model.OffsetNumberofTreesNeeded = OffsetCarbonCalculationRecord.NumberofTreesNeeded;
                            model.NumberOfPlantationProjectTreesNeeded = OffsetCarbonCalculationRecord.PlantationTreesNeeded;
                            model.FundNeededtoPlantTrees = OffsetCarbonCalculationRecord.FundNeededtoPlantTrees + " INR";
                            model.OffsetEmissionfromDomesticUse = OffsetCarbonCalculationRecord.OffsetEmissionfromDomesticUse + " Tonnes";
                            model.OffsetEmissionfromTransportation = OffsetCarbonCalculationRecord.OffsetEmissionfromTransportation + " Tonnes";
                            model.OffsetEmissionfromAirTrips = OffsetCarbonCalculationRecord.OffsetEmissionfromAirTrips + " Tonnes";
                            model.TotalOffsetCarbonEmission = OffsetCarbonCalculationRecord.TotalOffsetCarbonEmission + " Tonnes";
                            model.AverageOffsetEmissionperMonth = OffsetCarbonCalculationRecord.AverageOffsetEmissionperMonth + " Tonnes";
                            model.OffsetAnnualCarbonFootprint = OffsetCarbonCalculationRecord.OffsetAnnualCarbonFootprint + " Tonnes";
                            var treeneeded = Double.Parse(OffsetCarbonCalculationRecord.NumberofTreesNeeded) / 1.81;
                            var fundfortrees = Double.Parse(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees);
                            var EmissionOffsetForTreePlantation = Double.Parse(OffsetCarbonCalculationRecord.NumberofTreesNeeded).ToString("0.##");
                            model.EmissionOffsetForTreePlantation = EmissionOffsetForTreePlantation + " Tonnes";
                            var OffsetEmissionforFundingTrees = Double.Parse(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees).ToString("0.##");
                            model.OffsetEmissionforFundingTrees = OffsetEmissionforFundingTrees + " Tonnes";
                            Double trees = (fundfortrees / 1.81) * 1000;
                            model.OffsetNumberofTreesNeeded = treeneeded.ToString();
                            model.FundNeededtoPlantTrees = trees.ToString() + " INR";
                            Session["RegistrationNo"] = CarbonCalculationRecord.RegistartionNumber;
                            return View(model);
                        }
                    }
                }
                else
                {
                    return this.Redirect("/Carbon-Footprint-Calculator/User-Login");
                }
            }
            catch
            {

            }
            return View(model);

        }

    }
}