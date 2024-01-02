using Adani.SuperApp.Realty.Feature.Navigation.Platform.Models;
using Adani.SuperApp.Realty.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Http.Cors;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Models.SFDCWrapperModel;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Models.PlatinumRealtyModel;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Models.SamsaraVilasaEnquireNowModel;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Models.OGEnquireNowModel;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using Sitecore.Mvc.Common;
using Sitecore.Publishing.Explanations;
using Sitecore.Data;
using static Adani.SuperApp.Realty.Feature.Navigation.Platform.Templates;
using Newtonsoft.Json.Linq;
using Sitecore.StringExtensions;
using System.Runtime.Remoting;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Controllers
{
    public class HeaderController : Controller
    {
        AdaniRealityDataContext rdb = new AdaniRealityDataContext();
        private static readonly Regex alphaNumber = new Regex("^[a-z A-Z0-9]*$");
        private static readonly Regex emailRegex = new Regex("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$");
        private static readonly Regex NumRegex = new Regex("^[0-9]*$");

        private readonly ILogRepository _logRepository;
        // GET: Header
        AdaniRealityDataContext rgv = new AdaniRealityDataContext(); // reality global object
        public ActionResult Index()
        {


            return View();
        }

        public string GetData()
        {
            return "Test";
        }
        [HttpPost]
        public JsonResult POSTOTP(SfdcModel model)
        {
            var result = new { status = "0", message = string.Empty, errormessage = string.Empty, errorStatus = string.Empty };
            Item datasource = Sitecore.Context.Database.GetItem(EnquiryFormTemp.ItemID);
            Item SfdcItem = Sitecore.Context.Database.GetItem(Templates.SFDCStaticValue.ItemID);
            string customError = SfdcItem?.Fields[Templates.SFDCStaticValue.Fields.customerror].Value;
            string moreTryForOTP = SfdcItem?.Fields[Templates.SFDCStaticValue.Fields.moreTryForOTP].Value;
            try
            {
                var name = datasource?.Fields[Templates.EnquiryFormTemp.Fields.fullname].Value;
                var email = datasource?.Fields[Templates.EnquiryFormTemp.Fields.email].Value;
                var phoneNo = datasource?.Fields[Templates.EnquiryFormTemp.Fields.phoneNo].Value;
                Regex re = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                Regex rgx = new Regex("[^A-Za-z0-9]");
                Regex reMob = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";
                if (model.Name.Length > 2 && model.Name.Length < 50)
                {
                    foreach (var item in specialChar)
                    {
                        if (model.Name.Contains(item))
                        {
                            result = new { status = "0", message = "", errormessage = name, errorStatus = "Name" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    result = new { status = "0", message = "", errormessage = name, errorStatus = "Name" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var mobile = model.MobileNumber.Contains("+91") ? model.MobileNumber.Replace("+91", "").Trim() : model.MobileNumber.Trim();
                if (string.IsNullOrEmpty(model.Email) || !re.IsMatch(model.Email.ToLower()) || model.Email.Length < 2 || model.Email.Length > 50)
                {
                    result = new { status = "0", message = "", errormessage = email, errorStatus = "Email" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (model.MobileNumber.Length > 10 && model.MobileNumber.Substring(0, 3).Contains(" 91"))
                {
                    mobile = model.MobileNumber.Substring(0, 3).Contains("91") ? model.MobileNumber.Replace(" 91", "").Trim() : model.MobileNumber.Trim();
                    if (!reMob.IsMatch(mobile))
                    {
                        result = new { status = "0", message = "", errormessage = phoneNo, errorStatus = "mobileNo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                RealityOTPServices realityOTPServices = new RealityOTPServices();

                Reality__OTP reality__OTPObj = new Reality__OTP();
                var currDate = DateTime.Now;
                var beforeTime = currDate.AddMinutes(-60);
                Item sfdcItem = Sitecore.Context.Database.GetItem(Templates.SRDCServiceItemID);
                var mobileCatch = model.MobileNumber != "" && model.MobileNumber.Contains("+91") ? model.MobileNumber.Replace("+91", "91").Trim() : model.MobileNumber.Trim();
                var data = rgv.Reality__OTPs.Where(x => x.Mobile == mobileCatch && x.Session >= beforeTime && x.Session <= currDate).Count();
                var userEntry = rgv.Reality__OTPs.Where(x => x.Mobile == mobileCatch && x.Session >= beforeTime && x.Session <= currDate).FirstOrDefault();
                if (data >= 4)
                {
                    result = new { status = "503", message = moreTryForOTP, errormessage = "", errorStatus = "" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realityOTPServices.StoreGeneratedOtp(model);
                #endregion
                #region Api call to send SMS of OTP
                try
                {
                    var otpMessage = sfdcItem.Fields[Templates.SFDCService.Fields.OTPmessage].Value;
                    var apiurl = string.Format(otpMessage, model.MobileNumber, generatedotp);

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", this);
                        reality__OTPObj.Id = Guid.NewGuid();
                        reality__OTPObj.Mobile = model.MobileNumber.Contains("+91") ? model.MobileNumber.Replace("+91", "91").Trim() : model.MobileNumber.Trim();
                        reality__OTPObj.OTP = generatedotp;
                        reality__OTPObj.Session = DateTime.Now;
                        reality__OTPObj.Username = model.Name;
                        rgv.Reality__OTPs.InsertOnSubmit(reality__OTPObj);
                        rgv.SubmitChanges();
                        result = new { status = "1", message = "OTP send successfully", errormessage = "", errorStatus = "" };
                        string jsonStr = JsonConvert.SerializeObject(reality__OTPObj);
                        Log.Info("DataBase User Entry - jsonStr", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", this);
                        result = new { status = "0", message = "send OTP failed", errormessage = "", errorStatus = "" };
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
                result = new { status = "0", message = customError, errormessage = "", errorStatus = "" };
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult postSfdcWrapper(SfdcModel model)
        {
            var flag = false;
            var commonItem = Sitecore.Context.Database.GetItem(commonData.ItemID);

            var SfdcItem = Sitecore.Context.Database.GetItem(Templates.SFDCStaticValue.ItemID);
            var EnquirForm = Sitecore.Context.Database.GetItem(Templates.EnquiryFormTemp.ItemID);
            var customError = SfdcItem?.Fields[Templates.SFDCStaticValue.Fields.customerror].Value;
            var otMsg = SfdcItem?.Fields[Templates.SFDCStaticValue.Fields.OTPmsg].Value;
            var resubmit = SfdcItem?.Fields[Templates.SFDCStaticValue.Fields.resubmittheforms].Value;
            var invaliOt = SfdcItem?.Fields[Templates.SFDCStaticValue.Fields.invalidOTP].Value;
            var invalidInp = SfdcItem?.Fields[Templates.SFDCStaticValue.Fields.invalidinput].Value;
            var nameFlag = true;
            try
            {
                SandBoxModel sandBoxModel = new SandBoxModel();
                Regex re = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                Regex rgx = new Regex("[^A-Za-z0-9]");
                Regex reMob = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";
                if (model.Name.Length > 2 && model.Name.Length < 50)
                {
                    foreach (var item in specialChar)
                    {
                        if (model.Name.Contains(item))
                        {
                            nameFlag = false;
                            break;
                        }
                    }
                }
                else
                {
                    nameFlag = false;
                }
                var mobile = model.MobileNumber.Contains("+91") ? model.MobileNumber.Replace("+91", "").Trim() : model.MobileNumber;


                if (re.IsMatch(model.Email.ToLower()) && nameFlag && reMob.IsMatch(mobile))
                    flag = true;
                else
                {
                    if (re.IsMatch(model.Email.ToLower()) && nameFlag && model.OTP.Contains("00000"))
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            gotoPoint:
                if (flag)
                {

                    RealityOTPServices realityOTPServices = new RealityOTPServices();
                    LeadObject obj = new LeadObject();
                    if (string.IsNullOrEmpty(model.OTP))
                    {
                        sandBoxModel.OTPStatus = false;
                        sandBoxModel.Message = "Please provide OTP.";
                        Log.Info("JsonResult postSfdcWrapper OTP is null - ", this);
                        return Json(sandBoxModel, JsonRequestBehavior.AllowGet);
                    }
                    #region userEntry
                    obj.Firstname = model.Name;
                    obj.LastName = model.Name;
                    obj.Mobile = model.MobileNumber;
                    obj.Email = model.Email;

                    if (!string.IsNullOrEmpty(model.Comments))
                    {
                        if (model.Comments.Length < 141)
                        {
                            obj.Comments = model.Comments;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }

                    if (!string.IsNullOrEmpty(model.Type))
                    {
                        if (model.Type.Length > 2 && model.Type.Length < 50)
                        {
                            obj.Projectintrested = model.Type;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }
                    if (!string.IsNullOrEmpty(model.Configuration))
                    {
                        if (model.Configuration == "False") //club
                        {
                            obj.ProductType = null;
                        }
                        else if (model.Configuration.Length > 2 && model.Configuration.Length < 50)
                        {
                            obj.ProductType = model.Configuration;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }

                    if (!string.IsNullOrEmpty(model.Location))
                    {
                        if (model.Location.Length > 2 && model.Location.Length < 50)
                        {
                            obj.State = model.Location;
                            obj.AssignmentCity = model.Location;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }

                    if (model.PlanVisitDate != null && model.FormType.ToLower() == "plan a visit")
                    {
                        if (model.PlanVisitDate.ToString().Length > 2 && model.PlanVisitDate.ToString().Length < 50)
                        {
                            obj.ScheduleDate = model.PlanVisitDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }
                    else
                    {
                        obj.ScheduleDate = null;
                    }
                    if (model.FormType.ToLower() == "plan a visit")
                    {
                        if (!string.IsNullOrEmpty(model.timeSLot))
                        {
                            if (model.timeSLot.Length > 2 && model.timeSLot.Length < 50)
                            {
                                obj.TimeSlot = model.timeSLot;
                            }
                            else
                            {
                                flag = false;
                                goto gotoPoint;
                            }
                        }
                    }
                    else
                    {
                        obj.TimeSlot = "";
                    }
                    if (model.FormType.ToLower() == "send us your query")
                    {
                        if (!string.IsNullOrEmpty(model.purpose))
                        {
                            if (model.purpose.Length > 2 && model.purpose.Length < 50)
                            {
                                obj.purpose = model.purpose;
                            }
                            else
                            {
                                flag = false;
                                goto gotoPoint;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(model.FormType))
                    {
                        if (model.FormType.Length > 2 && model.FormType.Length < 50)
                        {
                            obj.FormType = model.FormType;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }
                    if (!string.IsNullOrEmpty(model.PageInfo))
                    {
                        if (model.PageInfo.Length >= 1 && model.PageInfo.Length < 200)
                        {
                            obj.PageInfo = model.PageInfo;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }
                    if (!string.IsNullOrEmpty(model.ProjectType))
                    {
                        if (model.ProjectType.Length > 2 && model.ProjectType.Length < 50)
                        {
                            obj.Project = model.ProjectType;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }

                    if (!string.IsNullOrEmpty(model.Country))
                    {
                        if (model.Country.Length > 2 && model.Country.Length < 50)
                        {
                            obj.Country = model.Country;
                        }
                        else
                        {
                            flag = false;
                            goto gotoPoint;
                        }
                    }
                    if (model.FormType.ToLower() == "plan a visit")
                    {
                        if (model.IsPreQualificationLeads == true || model.IsPreQualificationLeads == false)
                        {
                            obj.prequalification = "Yes";
                        }
                        else
                        {
                            flag = false;
                            obj.prequalification = "N/A";
                            goto gotoPoint;
                        }
                    }
                    else
                    {
                        obj.prequalification = "N/A";

                    }
                    if (model.IsHomeLoanRequired == true || model.IsHomeLoanRequired == false)
                    {
                        obj.IsHomeLoanRequired = model.IsHomeLoanRequired;
                    }
                    else
                    {
                        flag = false;
                        goto gotoPoint;
                    }
                    if (model.agreement == true || model.agreement == false)
                    {
                        obj.agreement = model.agreement;
                    }
                    else
                    {
                        flag = false;
                        goto gotoPoint;
                    }
                    if (model.FormType.ToLower() != "send us your query")
                    {
                        obj.RecordType = obj.Projectintrested == null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.RecordType].Value : commonItem.Fields[commonData.Fields.residentialID].Value.ToLower() == obj.Projectintrested.ToLower() ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.RecordType].Value : commonItem.Fields[commonData.Fields.commercialID].Value.ToLower() == obj.Projectintrested.ToLower() ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.commercialRecordType].Value : commonItem.Fields[commonData.Fields.ClubId].Value.ToLower() == obj.Projectintrested.ToLower() ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.clubRecordType].Value : SfdcItem.Fields[Templates.SFDCStaticValue.Fields.RecordType].Value;
                    }
                    else
                    {
                        obj.RecordType = obj.purpose.ToLower().Contains("residential") ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.RecordType].Value : obj.purpose.ToLower().Contains("commercial") ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.commercialRecordType].Value : obj.purpose.ToLower().Contains("club") ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.clubRecordType].Value : obj.purpose.ToLower().Contains("other") ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.OtherRecordType].Value : null;
                        //obj.RecordType = null;
                    }
                    obj.PostToSalesforce = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.PostToSalesforce] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.PostToSalesforce].Value : "";
                    obj.LeadSource = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.LeadSource] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.LeadSource].Value : "";
                    obj.UtmSource = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.UtmSource] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.UtmSource].Value : "";
                    obj.Budget = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Budget] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Budget].Value : "";
                    obj.Remarks = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Remarks] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Remarks].Value : ""; ;
                    System.Collections.Specialized.NameValueCollection nameValueCollection = SfdcItem != null ? Sitecore.Web.WebUtil.ParseUrlParameters(SfdcItem[Templates.SFDCStaticValue.Fields.MasterProjectIDs]) : null;

                    if (!string.IsNullOrEmpty(obj.Project))
                    {
                        //  System.Collections.Specialized.NameValueCollection nvc = nameValueCollection;

                        foreach (var nv in nameValueCollection)
                        {
                            var proj = obj.Project.Contains(" ") ? obj.Project.Replace(" ", "_") : obj.Project;
                            proj = proj.Replace("(", "").Replace(")", "");
                            proj = proj.Contains(".") ? proj.Replace(".", "_") : proj;
                            if (nv.ToString().ToLower().Equals(proj.ToLower()))
                            {
                                obj.MasterProjectID = nameValueCollection[nv.ToString()];
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(obj.MasterProjectID) && !string.IsNullOrEmpty(obj.Project))
                    {
                        obj.MasterProjectID = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.MasterProjectID] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.MasterProjectID].Value : "";
                    }
                        //ClubProjectID

                    System.Collections.Specialized.NameValueCollection clubValueCollection = SfdcItem != null ? Sitecore.Web.WebUtil.ParseUrlParameters(SfdcItem[Templates.SFDCStaticValue.Fields.ClubProjectIDs]) : null;

                    if (!string.IsNullOrEmpty(obj.Project))
                    {
                        //  System.Collections.Specialized.clubValueCollection cvc = clubValueCollection;

                        foreach (var nv in clubValueCollection)
                        {
                            var proj = obj.Project.Contains(" ") ? obj.Project.Replace(" ", "_") : obj.Project;
                            proj = proj.Replace("(", "").Replace(")", "");
                            proj = proj.Contains(".") ? proj.Replace(".", "_") : proj;
                            if (nv.ToString().ToLower().Equals(proj.ToLower()))
                            {
                                obj.ClubProject = clubValueCollection[nv.ToString()];
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(obj.ClubProject) && !string.IsNullOrEmpty(obj.Project))
                    {
                        obj.ClubProject = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.ClubProjectID] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.ClubProjectID].Value : "";
                    }

                    obj.Saletype = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Saletype] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Saletype].Value : "";
                    //obj.Comments = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Comments] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Comments].Value : "";
                    obj.Ads = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Ads] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.Ads].Value : "";
                    obj.UtmPlacement = SfdcItem.Fields[Templates.SFDCStaticValue.Fields.UtmPlacement] != null ? SfdcItem.Fields[Templates.SFDCStaticValue.Fields.UtmPlacement].Value : "";
                    #endregion
                    if (!string.IsNullOrEmpty(model.OTP) && model.OTP.Contains("00000"))
                    {
                        obj.OTPStatus = true;
                        Log.Info("JsonResult postSfdcWrapper OTP is valid - ", this);
                        SFDCWrapperModel objsfdc = new SFDCWrapperModel();
                        Random _random = new Random();
                        var result = objsfdc.GenerateLead(obj);
                        try
                        {
                            sandBoxModel.Firstname = obj.Firstname;
                            sandBoxModel.Mobile = obj.Mobile;
                            sandBoxModel.Email = obj.Email;
                            sandBoxModel.Project = obj.Project;
                            sandBoxModel.Projectintrested = obj.Projectintrested;
                            sandBoxModel.configuration = obj.ProductType;
                            sandBoxModel.LastName = obj.LastName;
                            sandBoxModel.timeSlot = obj.TimeSlot;
                            sandBoxModel.purpose = obj.purpose;
                            sandBoxModel.Country = obj.Country;
                            sandBoxModel.OTPStatus = obj.OTPStatus;
                            sandBoxModel.RecordType = obj.RecordType;
                            sandBoxModel.userblocked = false;
                            var resultresponse = JsonConvert.DeserializeObject<dynamic>(result);
                            sandBoxModel.Message = resultresponse.message;
                            realityOTPServices.DeleteOldOtp(model.MobileNumber);
                        }
                        catch (Exception ex)
                        {
                            sandBoxModel.Message = customError;
                            Log.Info("SFDC POST API exception for international number- " + ex.Message.ToString(), this);
                        }
                        return Json(sandBoxModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var status = realityOTPServices.ValidateOTP(model.MobileNumber, model.OTP);
                        //var status = true;
                        if (status == true)
                        {
                            obj.OTPStatus = true;
                            Log.Info("JsonResult postSfdcWrapper OTP is valid - ", this);
                            SFDCWrapperModel objsfdc = new SFDCWrapperModel();
                            Random _random = new Random();
                            var result = objsfdc.GenerateLead(obj);
                            try
                            {
                                sandBoxModel.Firstname = obj.Firstname;
                                sandBoxModel.Mobile = obj.Mobile;
                                sandBoxModel.Email = obj.Email;
                                sandBoxModel.Project = obj.Project;
                                sandBoxModel.Projectintrested = obj.Projectintrested;
                                sandBoxModel.configuration = obj.ProductType;
                                sandBoxModel.LastName = obj.LastName;
                                sandBoxModel.timeSlot = obj.TimeSlot;
                                sandBoxModel.purpose = obj.purpose;
                                sandBoxModel.Country = obj.Country;
                                sandBoxModel.OTPStatus = obj.OTPStatus;
                                sandBoxModel.userblocked = false;
                                sandBoxModel.RecordType = obj.RecordType;
                                var resultresponse = JsonConvert.DeserializeObject<dynamic>(result);
                                sandBoxModel.Message = resultresponse.message;
                                realityOTPServices.DeleteOldOtp(model.MobileNumber);
                            }
                            catch (Exception ex)
                            {
                                sandBoxModel.Message = customError;
                                Sitecore.Diagnostics.Log.Info("SFDC POST API exception at 250- " + ex.Message.ToString(), this);
                            }
                        }
                        else
                        {
                            var OtpFor = model.MobileNumber.Contains("+") ? model.MobileNumber.Replace("+91", "91").Trim() : model.MobileNumber.Trim();
                            var removeexistotp = rgv.Reality__OTPs.Where(x => x.Mobile == OtpFor).ToList().OrderByDescending(x => x.Session).FirstOrDefault();
                            if (removeexistotp.Attempt > 2)
                            {
                                sandBoxModel.OTPStatus = false;
                                sandBoxModel.userblocked = true;
                                sandBoxModel.Message = resubmit;
                                return Json(sandBoxModel, JsonRequestBehavior.AllowGet);
                            }
                            sandBoxModel.OTPStatus = false;
                            sandBoxModel.userblocked = false;
                            sandBoxModel.Message = invaliOt;
                            Log.Info("JsonResult postSfdcWrapper OTP is invalid - ", this);
                        }
                        return Json(sandBoxModel, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    sandBoxModel.OTPStatus = false;
                    sandBoxModel.userblocked = false;
                    sandBoxModel.Message = invalidInp;
                    Log.Info("JsonResult postSfdcWrapper data is invalid - ", this);
                    return Json(sandBoxModel, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                SandBoxModel sandBoxModel = new SandBoxModel();
                sandBoxModel.Message = customError;
                Log.Error($"{0}", ex, this);
                return Json(sandBoxModel, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AsterEnquiryNow(AsterEnquireModel m)
        {
            Log.Error("Aster Enquiry Now Start", "");
            var result = new { status = "0" };
            AdaniRealityDataContext rdb = new AdaniRealityDataContext();
            //PlatinumRealtyDataContext rdb1 = new PlatinumRealtyDataContext();
            String[] FormType = { "Contact us", "Enquire Now" };
            try
            {
                if (!Regex.IsMatch(m.full_name, (@"^([A-Za-z ]){2,100}$")))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.email, (@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[com]{2,9})$")))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.mobile, (@"^\d{10}$")))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!FormType.Contains(m.FormType))
                {
                    result = new { status = "406" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.PageInfo, (@"^[A-Za-z ]+$")))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.UTMSource, (@"^[A-Za-z _]+$")))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AsterEnquireNow r = new AsterEnquireNow();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("asterAdvertisementId");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("asterrecordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("asterPropertyCode");
                    #region Insert to DB

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    Log.Error("Aster Enquiry Now Lead Genration Start", "");
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {

                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Aster",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    if (leadResult == null)
                    {
                        result = new { status = "407" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    r.Comment = leadResult;
                    Log.Error("Aster Enquiry Now Lead Genration Result" + leadResult, "");
                    rdb.AsterEnquireNows.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult OysterGrandeEnquiryNow(OGEnquireNowModel m)
        {

            var result = new { status = "0" };

            try
            {
                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    OysterGrandePenthouseEnquireNow r = new OysterGrandePenthouseEnquireNow();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("OGheaderPropertyCode");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("OGrecordid");
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("OGAdvertisementId");
                    #region Insert to DB
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {

                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Oyster Grande",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.OysterGrandePenthouseEnquireNows.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SamsaraVilasaEnquiryNow(SamsaraVilasaEnquireNowModel m)
        {
            var result = new { status = "0" };
            try
            {
                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    SamsaraVilasaEnquireNow r = new SamsaraVilasaEnquireNow();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("SVAdvertisementId");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("SVrecordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("SVheaderPropertyCode");
                    #region Insert to DB
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {

                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Samsara Vilasa",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.SamsaraVilasaEnquireNows.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult NriCornerEnquiryNow(NriCornerEnquireNowModel m)
        {
            var result = new { status = "0" };
            Regex remail = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
            try
            {
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";

                if (string.IsNullOrEmpty(m.FullName))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (m.FullName.Length > 2 && m.FullName.Length < 50)
                {
                    foreach (var item in specialChar)
                    {
                        if (m.FullName.Contains(item))
                        {
                            result = new { status = "401" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.Email))
                {
                    Log.Info("Enquire now - Data added to DB Start email 1:" + m.Email, this);
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (remail.IsMatch(m.Email.ToLower()))
                {
                    if (m.Email.Length < 2 && m.Email.Length > 50)
                    {
                        Log.Info("Enquire now - Data added to DB Start email 2:" + m.Email, this);
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Log.Info("Enquire now - Data added to DB Start email 3:" + m.Email, this);
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }


                if (string.IsNullOrEmpty(m.Mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.Mobile))
                {
                    if (m.Mobile.Length < 2 && m.Mobile.Length > 18)
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.country))
                {
                    result = new { status = "country" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.country))
                {
                    if (m.country.Length > 2 && m.country.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.country.Contains(item))
                            {
                                result = new { status = "country" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "country" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageUrl))
                {
                    result = new { status = "m.PageUrl" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageUrl))
                {
                    if (m.PageUrl.Length < 2 && m.PageUrl.Length > 50)
                    {
                        result = new { status = "PageUrl" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.propertyType))
                {
                    result = new { status = "propertyType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.propertyType))
                {
                    if (m.propertyType.Length > 2 && m.propertyType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.propertyType.Contains(item))
                            {
                                result = new { status = "propertyType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "propertyType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormType))
                {
                    result = new { status = "FormType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormType))
                {
                    if (m.FormType.Length > 2 && m.FormType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "FormType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "FormType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(m.PageInfo))
                {
                    result = new { status = "PageInfo" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageInfo))
                {
                    if (m.PageInfo.Length > 2 && m.PageInfo.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "PageInfo" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PageInfo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrEmpty(m.PropertyLocation))
                {
                    if (m.PropertyLocation.Length > 2 && m.PropertyLocation.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.PropertyLocation.Contains(item))
                            {
                                result = new { status = "PropertyLocation" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "city" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (m.HomeLoan.ToString().ToLower() != "true" && m.HomeLoan.ToString().ToLower() != "false")
                {
                    result = new { status = "HomeLoan" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (m.TermsAndcondition.ToString().ToLower() != "true" && m.TermsAndcondition.ToString().ToLower() != "false")
                {
                    result = new { status = "TermsAndcondition" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                m.RecordType = Sitecore.Configuration.Settings.GetSetting("investrecordid");
                if (!string.IsNullOrEmpty(m.ProjectName))
                {
                    if (m.ProjectName.ToLower() == "ambrosia")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investAmbrosiaCode");
                    }
                    if (m.ProjectName.ToLower() == "ikaria")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investIkariaCode");
                    }
                    if (m.ProjectName.ToLower() == "green view")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investGreenViewCode");
                    }
                    if (m.ProjectName.ToLower() == "the storeys")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investTheStoreysCode");
                    }
                    if (m.ProjectName.ToLower() == "the north park")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investTheNorthParkCode");
                    }
                    if (m.ProjectName.ToLower() == "amogha")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investAmoghaCode");
                    }
                    if (m.ProjectName.ToLower() == "aster")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investAsterCode");
                    }
                    if (m.ProjectName.ToLower() == "paarijat")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investpaarijat");
                    }
                    if (m.ProjectName.ToLower() == "linkbay residences")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investLinkbayResidences");
                    }
                    if (m.ProjectName.ToLower() == "water lily")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investWaterLilyCode");
                    }
                    if (m.ProjectName.ToLower() == "atrius")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investArtiusCode");
                    }
                    if (m.ProjectName.ToLower() == "archway")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investArchwayCode");
                    }
                    if (m.ProjectName.ToLower() == "samsara vilasa")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investSamsaraVilasaCode");
                    }
                    if (m.ProjectName.ToLower() == "oyster grande")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investOysterGrandeCode");
                    }
                    if (m.ProjectName.ToLower() == "platinum tower")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investPlatinumTowerCode");
                    }
                    if (m.ProjectName.ToLower() == "aangan")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investAanganCode");
                    }
                    if (m.ProjectName.ToLower() == "samsara (m block)")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investSamsaraMBlockCode");
                    }
                    if (m.ProjectName.ToLower() == "the views")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investTheViewsCode");
                    }
                    if (m.ProjectName.ToLower() == "monte south")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investMonteSouthCode");
                    }
                    if (m.ProjectName.ToLower() == "atelier greens")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investAtelierGreensCode");
                    }
                    if (m.ProjectName.ToLower() == "ten bkc")
                    {
                        m.MasterProjectID = Sitecore.Configuration.Settings.GetSetting("investTenBkc");
                    }
                }
                if (m.isincludedquerystring == true)
                {
                    if (!string.IsNullOrEmpty(m.UTMSource) && !alphaNumber.IsMatch(m.UTMSource))
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (m.UTMSource.Length < 2 && m.UTMSource.Length > 50)
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (string.IsNullOrEmpty(m.UTMPlacement))
                    {
                        m.UTMPlacement = "";
                    }
                    if (!alphaNumber.IsMatch(m.UTMPlacement))
                    {
                        if (m.UTMPlacement.Length < 2 && m.UTMPlacement.Length > 50)
                        {
                            result = new { status = "UTMPlacement" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.AdvertisementID))
                    {
                        result = new { status = "AdvertisementId" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.AdvertisementID))
                    {
                        if (m.AdvertisementID.Length < 2 && m.AdvertisementID.Length > 50)
                        {
                            result = new { status = "AdvertisementId" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                if (m.isincludedquerystring == false)
                {
                    m.UTMSource = Sitecore.Configuration.Settings.GetSetting("investUTMSource");
                    m.UTMPlacement = Sitecore.Configuration.Settings.GetSetting("investUTMPlacement");
                    m.AdvertisementID = Sitecore.Configuration.Settings.GetSetting("investAdvertisementId");
                }
                NriCornerDataBase r = new NriCornerDataBase();
                r.FullName = m.FullName;
                r.Mobile = m.Mobile;
                r.Email = m.Email;
                r.RecordType = m.RecordType;
                r.MasterProjectID = m.MasterProjectID;
                r.PropertyType = m.propertyType;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.UTMSource = m.UTMSource;
                r.UTMPlacement = m.UTMPlacement;
                r.AdvertisementID = m.AdvertisementID;
                r.TermsAndCondition = m.TermsAndcondition;
                r.Projectname = m.ProjectName;
                r.HomeLoan = m.HomeLoan;
                r.city = m.PropertyLocation;
                r.country = m.country;
                r.PageUrl = m.PageUrl;
                r.Session = DateTime.Now;
                #region Insert to DB
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.FullName, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.Mobile, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.Email, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting ", this);
                SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                LeadObject1 obj = new LeadObject1
                {
                    LastName = m.FullName,
                    FormType = m.FormType,
                    Email = m.Email,
                    Mobile = m.Mobile,
                    Project = m.ProjectName,
                    AssignmentCity = m.PropertyLocation,
                    PageInfo = m.PageInfo,
                    MasterProjectID = m.MasterProjectID,
                    UtmSource = m.UTMSource,
                    UtmPlacement = m.UTMPlacement,
                    RecordType = m.RecordType,
                    LeadSource = "Digital",
                    Ads = m.AdvertisementID,
                    IsHomeLoanRequired = m.HomeLoan,
                    agreement = m.TermsAndcondition,
                    Country = m.country
                };
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                r.Comment = leadResult;
                rdb.NriCornerDataBases.InsertOnSubmit(r);
                rdb.SubmitChanges();
                result = new { status = "101" };
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertPlatinumRealtycontactdetail(PlatinumRealtyModel m)
        {
            var result = new { status = "0" };

            try
            {

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    platinumRealtyEnquireFormData r = new platinumRealtyEnquireFormData();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("PTAdvertisementId");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("PTrecordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("PTheaderPropertyCode");
                    #region Insert to DB
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {

                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Platinum Tower",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",

                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.platinumRealtyEnquireFormDatas.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool IsReCaptchValidV3(string reResponse)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6Le0pu0gAAAAAOgrah7pbbp55tiieAKO_0lFTNyV&response={reResponse}").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5)
            {
                return false;
            }

            return true;
        }

        [HttpPost]
        public ActionResult GoodLifeEnquiryNow(GoodLifeFestEnquireModel m)
        {
            var result = new { status = "0" };
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";
            try
            {
                var flag = this.IsReCaptchValidV3(m.reResponse);
                if (flag == false)
                {
                    result = new { status = "444" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.full_name))
                {
                    if (m.full_name.Length > 2 && m.full_name.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.full_name.Contains(item))
                            {
                                result = new { status = "401" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (emailRegex.IsMatch(m.email))
                {
                    if (m.email.Length < 2 && m.email.Length > 50)
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (NumRegex.IsMatch(m.mobile))
                {
                    if (m.mobile.Length < 2 && m.mobile.Length > 50)
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Projects_Interested__c))
                {
                    result = new { status = "m.Projects_Interested__c" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.Projects_Interested__c))
                {
                    if (m.Projects_Interested__c.Length > 2 && m.Projects_Interested__c.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.Projects_Interested__c.Contains(item))
                            {
                                result = new { status = "Projects_Interested__c" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "Projects_Interested__c" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PropertyLocation))
                {
                    result = new { status = "m.PropertyLocation" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PropertyLocation))
                {
                    if (m.PropertyLocation.Length > 2 && m.PropertyLocation.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.Projects_Interested__c.Contains(item))
                            {
                                result = new { status = "PropertyLocation" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PropertyLocation" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormType))
                {
                    result = new { status = "m.FormType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormType))
                {
                    if (m.FormType.Length > 2 && m.FormType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "FormType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "FormType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageInfo))
                {
                    result = new { status = "m.PageInfo" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageInfo))
                {
                    if (m.PageInfo.Length > 2 && m.PageInfo.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "PageInfo" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PageInfo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.projectname))
                {
                    result = new { status = "m.projectname" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.projectname))
                {
                    if (m.projectname.Length > 2 && m.projectname.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "projectname" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "projectname" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageUrl))
                {
                    result = new { status = "m.PageUrl" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageUrl))
                {
                    if (m.PageUrl.Length < 2 && m.PageUrl.Length > 50)
                    {
                        result = new { status = "PageUrl" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    result = new { status = "m.FormSubmitOn" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    if (m.FormSubmitOn.ToString().Length < 2 && m.FormSubmitOn.ToString().Length > 50)
                    {
                        result = new { status = "FormSubmitOn" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


                if (m.isincludedquerystring == true)
                {
                    if (!string.IsNullOrEmpty(m.UTMSource) && !alphaNumber.IsMatch(m.UTMSource))
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (m.UTMSource.Length < 2 && m.UTMSource.Length > 50)
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!string.IsNullOrEmpty(m.UTMPlacement) && !alphaNumber.IsMatch(m.UTMPlacement))
                    {
                        if (m.UTMPlacement.Length < 2 && m.UTMPlacement.Length > 50)
                        {
                            result = new { status = "UTMPlacement" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.RecordType))
                    {
                        result = new { status = "RecordType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.RecordType))
                    {
                        if (m.RecordType.Length < 2 && m.RecordType.Length > 50)
                        {
                            result = new { status = "RecordType" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (m.projectname.ToLower() == "archway")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glarchwaypropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glarchwayad");
                    }
                    if (m.projectname.ToLower() == "atrius")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glAtriuspropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glAtriusad");
                    }
                    if (m.projectname.ToLower() == "amogha")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glAmoghapropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glAmoghaad");
                    }
                    if (m.projectname.ToLower() == "aster")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glAsterpropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glAsterad");
                    }
                }
                if (m.isincludedquerystring == false)
                {
                    m.UTMSource = Sitecore.Configuration.Settings.GetSetting("goodlifefestUTMSource");
                    m.UTMPlacement = Sitecore.Configuration.Settings.GetSetting("goodlifefestUTMPlacement");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("goodlifefestrecordid");
                    if (m.projectname.ToLower() == "archway")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glarchwaypropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glarchwayad");
                    }
                    if (m.projectname.ToLower() == "atrius")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glAtriuspropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glAtriusad");
                    }
                    if (m.projectname.ToLower() == "amogha")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glAmoghapropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glAmoghaad");
                    }
                    if (m.projectname.ToLower() == "aster")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("glAsterpropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("glAsterad");
                    }
                }

                GoodLifeFestEnquireNow r = new GoodLifeFestEnquireNow();
                r.FullName = m.full_name;
                r.Mobile = m.mobile;
                r.Email = m.email;
                r.Project = m.projectname;
                r.PropertyType = m.Projects_Interested__c;
                r.PropertyLocation = m.PropertyLocation;
                r.FormType = m.FormType;
                r.PageUrl = m.PageUrl;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                r.UTMSource = m.UTMSource;
                r.Lead_Sub_Source = m.UTMPlacement;
                #region Insert to DB
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                LeadObject1 obj = new LeadObject1
                {

                    LastName = m.full_name,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    Email = m.email,
                    Mobile = m.mobile,
                    Project = m.projectname,
                    MasterProjectID = m.PropertyCode,
                    UtmSource = m.UTMSource,
                    AssignmentCity = m.PropertyLocation,
                    RecordType = m.RecordType,
                    LeadSource = "Digital",
                    Ads = m.AdvertisementId,
                    UtmPlacement = m.UTMPlacement,
                };

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                r.Comment = leadResult;
                rdb.GoodLifeFestEnquireNows.InsertOnSubmit(r);

                rdb.SubmitChanges();
                result = new { status = "101" };
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Sitecore.Diagnostics.Log.Error("Enquire now - Salesforce lead generation - GoodLifeEnquiryNow generate lead" + ex.ToString(), this);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ArchwayEnquiryNow(ArchwayEnquireNowModel m)
        {
            
            var RecordType = Sitecore.Configuration.Settings.GetSetting("archwayrecordid");
            var PropertyCode = Sitecore.Configuration.Settings.GetSetting("archwayPropertyCode");
            var result = new { status = "0" };
            try
            {
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";
                var flag = this.IsReCaptchValidV3(m.reResponse);
                if (flag == false)
                {
                    result = new { status = "444" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.full_name))
                {
                    if (m.full_name.Length > 2 && m.full_name.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.full_name.Contains(item))
                            {
                                result = new { status = "401" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (emailRegex.IsMatch(m.email))
                {
                    if (m.email.Length < 2 && m.email.Length > 50)
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (NumRegex.IsMatch(m.mobile))
                {
                    if (m.mobile.Length < 2 && m.mobile.Length > 50)
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Projects_Interested__c))
                {
                    result = new { status = "m.Projects_Interested__c" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.Projects_Interested__c))
                {
                    if (m.Projects_Interested__c.Length > 2 && m.Projects_Interested__c.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.Projects_Interested__c.Contains(item))
                            {
                                result = new { status = "Projects_Interested__c" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "Projects_Interested__c" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PropertyLocation))
                {
                    result = new { status = "m.PropertyLocation" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PropertyLocation))
                {
                    if (m.PropertyLocation.Length > 2 && m.PropertyLocation.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.PropertyLocation.Contains(item))
                            {
                                result = new { status = "PropertyLocation" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PropertyLocation" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormType))
                {
                    result = new { status = "m.FormType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormType))
                {
                    if (m.FormType.Length > 2 && m.FormType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "FormType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "FormType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageInfo))
                {
                    result = new { status = "m.PageInfo" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageInfo))
                {
                    if (m.PageInfo.Length > 2 && m.PageInfo.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.PageInfo.Contains(item))
                            {
                                result = new { status = "PageInfo" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PageInfo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrEmpty(m.ProjectName))
                {
                    if (m.ProjectName.Length > 2 && m.ProjectName.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "ProjectName" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "ProjectName" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!(m.ProjectName.ToLower() == "archway"))
                    {
                        result = new { status = "ProjectName" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageUrl) || !m.PageUrl.Contains(m.ProjectName.ToLower()))
                {
                    result = new { status = "m.PageUrl" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageUrl))
                {
                    if (m.PageUrl.Length < 2 && m.PageUrl.Length > 50)
                    {
                        result = new { status = "PageUrl" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    result = new { status = "m.FormSubmitOn" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    if (m.FormSubmitOn.ToString().Length < 2 && m.FormSubmitOn.ToString().Length > 50)
                    {
                        result = new { status = "FormSubmitOn" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (m.isincludedquerystring == true)
                {
                    if (!string.IsNullOrEmpty(m.UTMSource) && !alphaNumber.IsMatch(m.UTMSource))
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (m.UTMSource.Length < 2 && m.UTMSource.Length > 50)
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!string.IsNullOrEmpty(m.UTMPlacement) && !alphaNumber.IsMatch(m.UTMPlacement))
                    {
                        if (m.UTMPlacement.Length < 2 && m.UTMPlacement.Length > 50)
                        {
                            result = new { status = "UTMPlacement" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.RecordType))
                    {
                        result = new { status = "RecordType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.RecordType))
                    {
                        if (m.RecordType.Length < 2 && m.RecordType.Length > 50)
                        {
                            result = new { status = "RecordType" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.PropertyCode))
                    {
                        result = new { status = "PropertyCode" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.PropertyCode))
                    {
                        if (m.PropertyCode.Length < 2 && m.PropertyCode.Length > 50)
                        {
                            result = new { status = "PropertyCode" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.AdvertisementId))
                    {
                        result = new { status = "AdvertisementId" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.AdvertisementId))
                    {
                        if (m.AdvertisementId.Length < 2 && m.AdvertisementId.Length > 50)
                        {
                            result = new { status = "AdvertisementId" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    m.RecordType = RecordType != null ? RecordType : "";
                    m.PropertyCode = PropertyCode != null ? PropertyCode : "";
                }
                if (m.isincludedquerystring == false)
                {
                    m.UTMSource = Sitecore.Configuration.Settings.GetSetting("archwayUTMSource");
                    m.UTMPlacement = Sitecore.Configuration.Settings.GetSetting("archwayUTMPlacement");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("archwayrecordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("archwayPropertyCode");
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("archwayAdvertisementId");
                }
                RealityOTPServices realityOTPServices = new RealityOTPServices();
                var status = realityOTPServices.ValidateOTP(m.mobile, m.OTP);
                if (status == true)
                {
                    Log.Info("JsonResult postSfdcWrapper OTP is valid - ", this);
                    ArchwayEnquireNow r = new ArchwayEnquireNow();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.FormType = m.FormType;
                    r.Projectname = m.ProjectName;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    r.Lead_Sub_Source = m.UTMPlacement;
                    #region Insert to DB

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {
                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = m.ProjectName,
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        UtmPlacement = m.UTMPlacement,
                        Ads = m.AdvertisementId
                    };
                    realityOTPServices.DeleteOldOtp(m.mobile);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.ArchwayEnquireNows.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AtriusEnquiryNow(AtriusEnquireNowModel m)
        {

            var RecordType = Sitecore.Configuration.Settings.GetSetting("atriusrecordid");
            var PropertyCode = Sitecore.Configuration.Settings.GetSetting("atriusPropertyCode");
            var result = new { status = "0" };
            try
            {
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";
                var flag = this.IsReCaptchValidV3(m.reResponse);
                if (flag == false)
                {
                    result = new { status = "444" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.full_name))
                {
                    if (m.full_name.Length > 2 && m.full_name.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.full_name.Contains(item))
                            {
                                result = new { status = "401" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (emailRegex.IsMatch(m.email))
                {
                    if (m.email.Length < 2 && m.email.Length > 50)
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (NumRegex.IsMatch(m.mobile))
                {
                    if (m.mobile.Length < 2 && m.mobile.Length > 50)
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.Projects_Interested__c))
                {
                    result = new { status = "m.Projects_Interested__c" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.Projects_Interested__c))
                {
                    if (m.Projects_Interested__c.Length > 2 && m.Projects_Interested__c.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.Projects_Interested__c.Contains(item))
                            {
                                result = new { status = "Projects_Interested__c" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "Projects_Interested__c" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PropertyLocation))
                {
                    result = new { status = "m.PropertyLocation" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PropertyLocation))
                {
                    if (m.PropertyLocation.Length > 2 && m.PropertyLocation.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.PropertyLocation.Contains(item))
                            {
                                result = new { status = "PropertyLocation" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PropertyLocation" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormType))
                {
                    result = new { status = "m.FormType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormType))
                {
                    if (m.FormType.Length > 2 && m.FormType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "FormType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "FormType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageInfo))
                {
                    result = new { status = "m.PageInfo" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageInfo))
                {
                    if (m.PageInfo.Length > 2 && m.PageInfo.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.PageInfo.Contains(item))
                            {
                                result = new { status = "PageInfo" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PageInfo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrEmpty(m.ProjectName))
                {
                    if (m.ProjectName.Length > 2 && m.ProjectName.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "ProjectName" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "ProjectName" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!(m.ProjectName.ToLower() == "atrius"))
                    {
                        result = new { status = "ProjectName" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageUrl) || !m.PageUrl.Contains(m.ProjectName.ToLower()))
                {
                    result = new { status = "m.PageUrl" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageUrl))
                {
                    if (m.PageUrl.Length < 2 && m.PageUrl.Length > 50)
                    {
                        result = new { status = "PageUrl" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    result = new { status = "m.FormSubmitOn" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    if (m.FormSubmitOn.ToString().Length < 2 && m.FormSubmitOn.ToString().Length > 50)
                    {
                        result = new { status = "FormSubmitOn" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (m.isincludedquerystring == true)
                {
                    if (!string.IsNullOrEmpty(m.UTMSource) && !alphaNumber.IsMatch(m.UTMSource))
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (m.UTMSource.Length < 2 && m.UTMSource.Length > 50)
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!string.IsNullOrEmpty(m.UTMPlacement) && !alphaNumber.IsMatch(m.UTMPlacement))
                    {
                        if (m.UTMPlacement.Length < 2 && m.UTMPlacement.Length > 50)
                        {
                            result = new { status = "UTMPlacement" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.RecordType))
                    {
                        result = new { status = "RecordType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.RecordType))
                    {
                        if (m.RecordType.Length < 2 && m.RecordType.Length > 50)
                        {
                            result = new { status = "RecordType" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.PropertyCode))
                    {
                        result = new { status = "PropertyCode" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.PropertyCode))
                    {
                        if (m.PropertyCode.Length < 2 && m.PropertyCode.Length > 50)
                        {
                            result = new { status = "PropertyCode" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.AdvertisementId))
                    {
                        result = new { status = "AdvertisementId" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.AdvertisementId))
                    {
                        if (m.AdvertisementId.Length < 2 && m.AdvertisementId.Length > 50)
                        {
                            result = new { status = "AdvertisementId" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    m.RecordType = RecordType != null ? RecordType : "";
                    m.PropertyCode = PropertyCode != null ? PropertyCode : "";
                }

                if (m.isincludedquerystring == false)
                {
                    m.UTMSource = Sitecore.Configuration.Settings.GetSetting("atriusUTMSource");
                    m.UTMPlacement = Sitecore.Configuration.Settings.GetSetting("atriusUTMPlacement");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("atriusrecordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("atriusPropertyCode");
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("atriusAdvertisementId");
                }
                RealityOTPServices realityOTPServices = new RealityOTPServices();
                var status = realityOTPServices.ValidateOTP(m.mobile, m.OTP);
                if (status == true)
                {
                    Log.Info("JsonResult postSfdcWrapper OTP is valid - ", this);
                    AtriusEnquireNow r = new AtriusEnquireNow();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.PropertyType = m.Projects_Interested__c;
                    r.Projectname = m.ProjectName;
                    r.PropertyLocation = m.PropertyLocation;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    r.Lead_Sub_Source = m.UTMPlacement;

                    #region Insert to DB
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {
                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        MasterProjectID = m.PropertyCode,
                        Project = m.ProjectName,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        UtmPlacement = m.UTMPlacement,
                        Ads = m.AdvertisementId
                    };
                    realityOTPServices.DeleteOldOtp(m.mobile);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.AtriusEnquireNows.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ZeroEmiEnquiryNow(ZeroEmiEnquireNowModel m)
        {
            var result = new { status = "0" };
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";
            try
            {
                var flag = this.IsReCaptchValidV3(m.reResponse);
                if (flag == false)
                {
                    result = new { status = "444" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.full_name))
                {
                    if (m.full_name.Length > 2 && m.full_name.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.full_name.Contains(item))
                            {
                                result = new { status = "401" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (emailRegex.IsMatch(m.email))
                {
                    if (m.email.Length < 2 && m.email.Length > 50)
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (NumRegex.IsMatch(m.mobile))
                {
                    if (m.mobile.Length < 2 && m.mobile.Length > 50)
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(m.propertytype))
                {
                    result = new { status = "m.propertytype" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.propertytype))
                {
                    if (m.propertytype.Length > 2 && m.propertytype.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.propertytype.Contains(item))
                            {
                                result = new { status = "propertytype" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "propertytype" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PropertyLocation))
                {
                    result = new { status = "m.PropertyLocation" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PropertyLocation))
                {
                    if (m.PropertyLocation.Length > 2 && m.PropertyLocation.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.propertytype.Contains(item))
                            {
                                result = new { status = "PropertyLocation" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PropertyLocation" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormType))
                {
                    result = new { status = "m.FormType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormType))
                {
                    if (m.FormType.Length > 2 && m.FormType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "FormType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "FormType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageInfo))
                {
                    result = new { status = "m.PageInfo" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageInfo))
                {
                    if (m.PageInfo.Length > 2 && m.PageInfo.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "PageInfo" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PageInfo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.projectname))
                {
                    result = new { status = "m.projectname" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.projectname))
                {
                    if (m.projectname.Length > 2 && m.projectname.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "projectname" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "projectname" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageUrl))
                {
                    result = new { status = "m.PageUrl" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageUrl))
                {
                    if (m.PageUrl.Length < 2 && m.PageUrl.Length > 50)
                    {
                        result = new { status = "PageUrl" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    result = new { status = "m.FormSubmitOn" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    if (m.FormSubmitOn.ToString().Length < 2 && m.FormSubmitOn.ToString().Length > 50)
                    {
                        result = new { status = "FormSubmitOn" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }


                if (m.isincludedquerystring == true)
                {
                    if (!string.IsNullOrEmpty(m.UTMSource) && !alphaNumber.IsMatch(m.UTMSource))
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (m.UTMSource.Length < 2 && m.UTMSource.Length > 50)
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!string.IsNullOrEmpty(m.UTMPlacement) && !alphaNumber.IsMatch(m.UTMPlacement))
                    {
                        if (m.UTMPlacement.Length < 2 && m.UTMPlacement.Length > 50)
                        {
                            result = new { status = "UTMPlacement" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.RecordType))
                    {
                        result = new { status = "RecordType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.RecordType))
                    {
                        if (m.RecordType.Length < 2 && m.RecordType.Length > 50)
                        {
                            result = new { status = "RecordType" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                if (m.isincludedquerystring == false)
                {
                    m.UTMSource = Sitecore.Configuration.Settings.GetSetting("ZeroEmiUTMSource");
                    m.UTMPlacement = Sitecore.Configuration.Settings.GetSetting("ZeroEmiUTMPlacement");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("ZeroEmirecordid");
                    if (m.projectname.ToLower() == "archway")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("zearchwaypropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("zearchwayad");
                    }
                    if (m.projectname.ToLower() == "atrius")
                    {
                        m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("zeAtriuspropertycode");
                        m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("zeAtriusad");
                    }
                }

                ZeroEmiEnquireNow r = new ZeroEmiEnquireNow();
                r.FullName = m.full_name;
                r.Mobile = m.mobile;
                r.Email = m.email;
                r.Project = m.projectname;
                r.PropertyType = m.propertytype;
                r.PropertyLocation = m.PropertyLocation;
                r.RecordType = m.RecordType;
                r.MasterProjectID = m.PropertyCode;
                r.AdvertisementID = m.AdvertisementId;
                r.FormType = m.FormType;
                r.PageUrl = m.PageUrl;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                r.UTMSource = m.UTMSource;
                r.Lead_Sub_Source = m.UTMPlacement;
                #region Insert to DB
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                LeadObject1 obj = new LeadObject1
                {

                    LastName = m.full_name,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    Email = m.email,
                    Mobile = m.mobile,
                    Project = m.projectname,
                    MasterProjectID = m.PropertyCode,
                    UtmSource = m.UTMSource,
                    AssignmentCity = m.PropertyLocation,
                    RecordType = m.RecordType,
                    LeadSource = "Digital",
                    Ads = m.AdvertisementId,
                    UtmPlacement = m.UTMPlacement,
                };

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                r.Comment = leadResult;
                rdb.ZeroEmiEnquireNows.InsertOnSubmit(r);

                rdb.SubmitChanges();
                result = new { status = "101" };
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Sitecore.Diagnostics.Log.Error("Enquire now - Salesforce lead generation - GoodLifeEnquiryNow generate lead" + ex.ToString(), this);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SamsaraVilasa2EnquiryNow(SamsaraVilasa2EnquireNowModel m)
        {
            var result = new { status = "0" };
            try
            {
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,0123456789";

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.full_name))
                {
                    if (m.full_name.Length > 2 && m.full_name.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.full_name.Contains(item))
                            {
                                result = new { status = "401" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (emailRegex.IsMatch(m.email))
                {
                    if (m.email.Length < 2 && m.email.Length > 50)
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (NumRegex.IsMatch(m.mobile))
                {
                    if (m.mobile.Length < 2 && m.mobile.Length > 50)
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.PropertyLocation))
                {
                    result = new { status = "m.PropertyLocation" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PropertyLocation))
                {
                    if (m.PropertyLocation.Length > 2 && m.PropertyLocation.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.Projects_Interested__c.Contains(item))
                            {
                                result = new { status = "PropertyLocation" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PropertyLocation" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormType))
                {
                    result = new { status = "m.FormType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormType))
                {
                    if (m.FormType.Length > 2 && m.FormType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "FormType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "FormType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageInfo))
                {
                    result = new { status = "m.PageInfo" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageInfo))
                {
                    if (m.PageInfo.Length > 2 && m.PageInfo.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "PageInfo" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PageInfo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrEmpty(m.ProjectName))
                {
                    if (m.ProjectName.Length > 2 && m.ProjectName.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "ProjectName" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "ProjectName" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageUrl))
                {
                    result = new { status = "m.PageUrl" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageUrl))
                {
                    if (m.PageUrl.Length < 2 && m.PageUrl.Length > 50)
                    {
                        result = new { status = "PageUrl" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.Country))
                {
                    result = new { status = "m.Country" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.Country))
                {
                    if (m.Country.Length > 2 && m.Country.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.Projects_Interested__c.Contains(item))
                            {
                                result = new { status = "Country" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "Country" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    result = new { status = "m.FormSubmitOn" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    if (m.FormSubmitOn.ToString().Length < 2 && m.FormSubmitOn.ToString().Length > 50)
                    {
                        result = new { status = "FormSubmitOn" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (m.isincludedquerystring == true)
                {
                    if (!string.IsNullOrEmpty(m.UTMSource) && !alphaNumber.IsMatch(m.UTMSource))
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (m.UTMSource.Length < 2 && m.UTMSource.Length > 50)
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!string.IsNullOrEmpty(m.UTMPlacement) && !alphaNumber.IsMatch(m.UTMPlacement))
                    {
                        if (m.UTMPlacement.Length < 2 && m.UTMPlacement.Length > 50)
                        {
                            result = new { status = "UTMPlacement" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.RecordType))
                    {
                        result = new { status = "RecordType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.RecordType))
                    {
                        if (m.RecordType.Length < 2 && m.RecordType.Length > 50)
                        {
                            result = new { status = "RecordType" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.PropertyCode))
                    {
                        result = new { status = "PropertyCode" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.PropertyCode))
                    {
                        if (m.PropertyCode.Length < 2 && m.PropertyCode.Length > 50)
                        {
                            result = new { status = "PropertyCode" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.AdvertisementId))
                    {
                        result = new { status = "AdvertisementId" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.AdvertisementId))
                    {
                        if (m.AdvertisementId.Length < 2 && m.AdvertisementId.Length > 50)
                        {
                            result = new { status = "AdvertisementId" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("SV2recordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("SV2headerPropertyCode");
                }
                if (m.isincludedquerystring == false)
                {
                    m.UTMSource = Sitecore.Configuration.Settings.GetSetting("SV2UTMSource");
                    m.UTMPlacement = Sitecore.Configuration.Settings.GetSetting("SV2UTMPlacement");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("SV2recordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("SV2headerPropertyCode");
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("SV2AdvertisementId");
                }

                RealityOTPServices realityOTPServices = new RealityOTPServices();
                var status = realityOTPServices.ValidateOTP(m.mobile, m.OTP);
                //var status = true;
                if (status == true)
                {
                    Log.Info("JsonResult postSfdcWrapper OTP is valid - ", this);
                    SamsaraVilasa2EnquireNow r = new SamsaraVilasa2EnquireNow();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.PropertyType = m.Projects_Interested__c;
                    r.Project = m.ProjectName;
                    r.PropertyLocation = m.PropertyLocation;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.Country = m.Country;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    r.Lead_Sub_Source = m.UTMPlacement;
                    r.RecordType = m.RecordType;
                    r.MasterProjectID = m.PropertyCode;
                    r.AdvertisementID = m.AdvertisementId;


                    #region Insert to DB
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {
                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        MasterProjectID = m.PropertyCode,
                        Project = m.ProjectName,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        UtmPlacement = m.UTMPlacement,
                        Ads = m.AdvertisementId
                    };
                    realityOTPServices.DeleteOldOtp(m.mobile);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.SamsaraVilasa2EnquireNows.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InspireBusinessParkEnquireNow(SamsaraVilasa2EnquireNowModel m)
        {
            var result = new { status = "0" };
            try
            {
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.;'<>_,0123456789";

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.full_name))
                {
                    if (m.full_name.Length > 2 && m.full_name.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.full_name.Contains(item))
                            {
                                result = new { status = "401" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "401" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (emailRegex.IsMatch(m.email))
                {
                    if (m.email.Length < 2 && m.email.Length > 50)
                    {
                        result = new { status = "403" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (NumRegex.IsMatch(m.mobile))
                {
                    if (m.mobile.Length < 2 || m.mobile.Length > 15)
                    {
                        result = new { status = "405" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.PropertyLocation))
                {
                    result = new { status = "m.PropertyLocation" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PropertyLocation))
                {
                    if (m.PropertyLocation.Length > 2 && m.PropertyLocation.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.PropertyLocation.Contains(item))
                            {
                                result = new { status = "PropertyLocation" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PropertyLocation" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormType))
                {
                    result = new { status = "m.FormType" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormType))
                {
                    if (m.FormType.Length > 2 && m.FormType.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.FormType.Contains(item))
                            {
                                result = new { status = "FormType" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "FormType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageInfo))
                {
                    result = new { status = "m.PageInfo" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageInfo))
                {
                    if (m.PageInfo.Length > 2 && m.PageInfo.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.PageInfo.Contains(item))
                            {
                                result = new { status = "PageInfo" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "PageInfo" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (!string.IsNullOrEmpty(m.ProjectName))
                {
                    if (m.ProjectName.Length > 2 && m.ProjectName.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.ProjectName.Contains(item))
                            {
                                result = new { status = "ProjectName" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "ProjectName" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.PageUrl))
                {
                    result = new { status = "m.PageUrl" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.PageUrl))
                {
                    if (m.PageUrl.Length < 2 && m.PageUrl.Length > 50)
                    {
                        result = new { status = "PageUrl" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.Country))
                {
                    result = new { status = "m.Country" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.Country))
                {
                    if (m.Country.Length > 2 && m.Country.Length < 50)
                    {
                        foreach (var item in specialChar)
                        {
                            if (m.Country.Contains(item))
                            {
                                result = new { status = "Country" };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        result = new { status = "Country" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    result = new { status = "m.FormSubmitOn" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!string.IsNullOrEmpty(m.FormSubmitOn.ToString()))
                {
                    if (m.FormSubmitOn.ToString().Length < 2 && m.FormSubmitOn.ToString().Length > 50)
                    {
                        result = new { status = "FormSubmitOn" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                if (m.isincludedquerystring == true)
                {
                    if (!string.IsNullOrEmpty(m.UTMSource) && !alphaNumber.IsMatch(m.UTMSource))
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (m.UTMSource.Length < 2 && m.UTMSource.Length > 50)
                    {
                        result = new { status = "UTMSource" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!string.IsNullOrEmpty(m.UTMPlacement) && !alphaNumber.IsMatch(m.UTMPlacement))
                    {
                        if (m.UTMPlacement.Length < 2 && m.UTMPlacement.Length > 50)
                        {
                            result = new { status = "UTMPlacement" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.RecordType))
                    {
                        result = new { status = "RecordType" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.RecordType))
                    {
                        if (m.RecordType.Length < 2 && m.RecordType.Length > 50)
                        {
                            result = new { status = "RecordType" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.PropertyCode))
                    {
                        result = new { status = "PropertyCode" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.PropertyCode))
                    {
                        if (m.PropertyCode.Length < 2 && m.PropertyCode.Length > 50)
                        {
                            result = new { status = "PropertyCode" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (string.IsNullOrEmpty(m.AdvertisementId))
                    {
                        result = new { status = "AdvertisementId" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    if (!alphaNumber.IsMatch(m.AdvertisementId))
                    {
                        if (m.AdvertisementId.Length < 2 && m.AdvertisementId.Length > 50)
                        {
                            result = new { status = "AdvertisementId" };
                            return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("IBPrecordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("IBPheaderPropertyCode");
                }
                if (m.isincludedquerystring == false)
                {
                    m.UTMSource = Sitecore.Configuration.Settings.GetSetting("IBPUTMSource");
                    m.UTMPlacement = Sitecore.Configuration.Settings.GetSetting("IBPUTMPlacement");
                    m.RecordType = Sitecore.Configuration.Settings.GetSetting("IBPrecordid");
                    m.PropertyCode = Sitecore.Configuration.Settings.GetSetting("IBPheaderPropertyCode");
                    m.AdvertisementId = Sitecore.Configuration.Settings.GetSetting("IBPAdvertisementId");
                }

               
                    InspireBusinessParkEnquireNow r = new InspireBusinessParkEnquireNow();
                    r.FullName = m.full_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.PropertyType = m.Projects_Interested__c;
                    r.Project = m.ProjectName;
                    r.PropertyLocation = m.PropertyLocation;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.Country = m.Country;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    r.Lead_Sub_Source = m.UTMPlacement;
                    r.RecordType = m.RecordType;
                    r.MasterProjectID = m.PropertyCode;
                    r.AdvertisementID = m.AdvertisementId;


                    #region Insert to DB
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject1 obj = new LeadObject1
                    {
                        LastName = m.full_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        MasterProjectID = m.PropertyCode,
                        Project = m.ProjectName,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        UtmPlacement = m.UTMPlacement,
                        Ads = m.AdvertisementId
                    };
                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.InspireBusinessParkEnquireNows.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                    result = new { status = "101" };
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}