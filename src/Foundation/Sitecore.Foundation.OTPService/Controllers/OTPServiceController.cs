using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.OTPService.Models;
using Sitecore.Foundation.OTPService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Sitecore.Data;
namespace Sitecore.Foundation.OTPService.Controllers
{
    public class OTPServiceController : Controller
    {
        OTPServiceDataContext otpDataContext = new OTPServiceDataContext(); // OTP Service global object
        // GET: OTPService
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetOTP(OTPServiceModel model)
        {
            var result = new { status = "0", message = string.Empty, errormessage = string.Empty, errorStatus = string.Empty, OTP = string.Empty };
            var OTPDataSource = Sitecore.Data.ID.Parse(model.OTPSitecoreDatasource);
            Item datasource = Sitecore.Context.Database.GetItem(OTPDataSource);
            string customError = datasource?.Fields[Templates.Template.SitecoreOTPMessages.OTPCustomErrorMessage].Value;
            string moreTryForOTP = datasource?.Fields[Templates.Template.SitecoreOTPMessages.OTPAttemptExceedErrorMessage].Value;

            if (datasource == null)
            {
                result = new { status = "0", message = "OTP datasource item not found", errormessage = "OTP datasource item not found", errorStatus = "", OTP = string.Empty };              
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var nameErrorMessage = datasource?.Fields[Templates.Template.SitecoreOTPMessages.NameErrorMessage].Value;
                var emailErrorMessage = datasource?.Fields[Templates.Template.SitecoreOTPMessages.EmailErrorMessage].Value;
                var mobileErrorMessage = datasource?.Fields[Templates.Template.SitecoreOTPMessages.MobileErrorMessage].Value;

                Regex re = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                Regex rgx = new Regex("[^A-Za-z0-9]");
                Regex reMob = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";

                #region validating Name, Mobile and Email
                if (model.Name.Length > 2 && model.Name.Length < 50)
                {
                    foreach (var item in specialChar)
                    {
                        if (model.Name.Contains(item))
                        {
                            result = new { status = "0", message = "", errormessage = nameErrorMessage, errorStatus = "Name", OTP = string.Empty };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    result = new { status = "0", message = "", errormessage = nameErrorMessage, errorStatus = "Name", OTP = string.Empty };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
               
                
                var mobileno = model.MobileNumber.Contains("+91") ? model.MobileNumber.Replace("+91", "").Trim() : model.MobileNumber.Trim();
                if (string.IsNullOrEmpty(model.Email) || !re.IsMatch(model.Email.ToLower()))
                {
                    result = new { status = "0", message = "", errormessage = emailErrorMessage, errorStatus = "Email", OTP = string.Empty };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (model.MobileNumber.Length > 10 && model.MobileNumber.Substring(0, 3).Contains(" 91"))
                {
                    mobileno = model.MobileNumber.Substring(0, 3).Contains("91") ? model.MobileNumber.Replace(" 91", "").Trim() : model.MobileNumber.Trim();
                    if (!reMob.IsMatch(mobileno))
                    {
                        result = new { status = "0", message = "", errormessage = mobileErrorMessage, errorStatus = "mobileNo", OTP = string.Empty };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion
               
                OTPCRUDService otpServiceOBJ = new OTPCRUDService();
                OTPService otpTableOBJ = new OTPService();

                var currDate = DateTime.Now;
                var beforeTime = currDate.AddMinutes(-60);
                var mobileCatch = model.MobileNumber != "" && model.MobileNumber.Contains("+91") ? model.MobileNumber.Replace("+91", "91").Trim() : model.MobileNumber.Trim();
                var data = otpDataContext.OTPServices.Where(x => x.Mobile == mobileCatch && x.Session >= beforeTime && x.Session <= currDate).Count();
                var userEntry = otpDataContext.OTPServices.Where(x => x.Mobile == mobileCatch && x.Session >= beforeTime && x.Session <= currDate).FirstOrDefault();
                if (data >= 4)
                {
                    result = new { status = "503", message = moreTryForOTP, errormessage = "", errorStatus = "", OTP = string.Empty };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                #region Generate New Otp for given mobile number and save to database
                string generatedotp = otpServiceOBJ.StoreGeneratedOtp(model);
                #endregion

                #region Api call to send SMS of OTP
                try
                {
                    var otpMessage = datasource?.Fields[Templates.Template.SitecoreOTPFields.OTPMessageBody].Value;
                    var apiurl = string.Format(otpMessage, model.MobileNumber, generatedotp);

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", this);
                        otpTableOBJ.ID = Guid.NewGuid();
                        otpTableOBJ.Mobile = model.MobileNumber.Contains("+91") ? model.MobileNumber.Replace("+91", "91").Trim() : model.MobileNumber.Trim();
                        otpTableOBJ.OTP = generatedotp;
                        otpTableOBJ.Session = DateTime.Now;
                        otpTableOBJ.Username = model.Name;
                        otpDataContext.OTPServices.InsertOnSubmit(otpTableOBJ);
                        otpDataContext.SubmitChanges();
                        result = new { status = "1", message = "OTP send successfully", errormessage = "", errorStatus = "", OTP = generatedotp };
                        string jsonStr = JsonConvert.SerializeObject(otpTableOBJ);
                        Log.Info("DataBase User Entry - jsonStr", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", this);
                        result = new { status = "0", message = "send OTP failed", errormessage = "", errorStatus = "", OTP = string.Empty };
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                return Json(result, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                result = new { status = "0", message = customError, errormessage = "", errorStatus = "", OTP = string.Empty };
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult VerifyOTP(OTPServiceModel model)
        {
            var result = new { status = "0", message="Invalid OTP"};
            try
            {
                OTPCRUDService otpServiceOBJ = new OTPCRUDService();
                var status = otpServiceOBJ.ValidateOTP(model.MobileNumber, model.OTP);
                if(status)
                {
                    result = new { status = "1", message = "OTP verfication successful." };
                }
                #region Return Response with Mobile Number and Generated otp
                return Json(result, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                result = new { status = "0", message = System.Convert.ToString(ex)};
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}