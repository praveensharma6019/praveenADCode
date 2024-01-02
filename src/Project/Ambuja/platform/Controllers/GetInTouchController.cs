using Newtonsoft.Json;
using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.Forms;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Project.AmbujaCement.Website.Controllers
{
    public class GetInTouchController : Controller
    {
        private IList<GetInTouchPostFormModel> _getInTouchFormData = new List<GetInTouchPostFormModel>();
        private string getInTouchFormFilePath = System.Web.HttpContext.Current.Server.MapPath("/App_Data/OtpData/GetInTouchFormDataFile.json");

        public GetInTouchController()
        {
            if (!System.IO.File.Exists(getInTouchFormFilePath))
            {
                var filestream = System.IO.File.Create(getInTouchFormFilePath);
                filestream.Close();
            }
            else
            {
                _getInTouchFormData = System.IO.File.ReadAllLines(getInTouchFormFilePath).Select(x => JsonConvert.DeserializeObject<GetInTouchPostFormModel>(x)).ToList();
            }
        }

        [HttpPost]
        [Route("submitform")]
        public ActionResult Index(GetInTouchPostFormModel getInTouchForm)
        {
            try
            {
                //string gCaptchaResponse = Request.Form.Get("g-recaptcha-response");
                //if (!Utils.IsReCaptchV2Valid(gCaptchaResponse))
                //{
                //    return Json(new { Result = "ReCaptcha validation failed." });
                //}
                //Utils.ValidateAntiForgeryToken(HttpContext.Request);
                if (ModelState.IsValid)
                {
                    var errorResponse = new { FieldName = string.Empty, ErrorMessage = "" };
                    if (getInTouchForm.firstName != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.firstName))
                        {
                            errorResponse = new { FieldName = "firstName", ErrorMessage = "Please enter First Name" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(getInTouchForm.firstName) && (!Regex.IsMatch(getInTouchForm.firstName, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                        {
                            errorResponse = new { FieldName = "firstName", ErrorMessage = "Please enter valid First Name" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(getInTouchForm.firstName) && (getInTouchForm.firstName.Length > 128))
                        {
                            errorResponse = new { FieldName = "firstName", ErrorMessage = "Maximum of 128 characters are allowed" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (getInTouchForm.lastName != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.lastName))
                        {
                            errorResponse = new { FieldName = "lastName", ErrorMessage = "Please enter Last Name" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(getInTouchForm.lastName) && (!Regex.IsMatch(getInTouchForm.lastName, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                        {
                            errorResponse = new { FieldName = "lastName", ErrorMessage = "Please enter valid Last Name" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(getInTouchForm.lastName) && (getInTouchForm.lastName.Length > 128))
                        {
                            errorResponse = new { FieldName = "lastName", ErrorMessage = "Maximum of 128 characters are allowed" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (getInTouchForm.phoneNo != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.phoneNo))
                        {
                            errorResponse = new { FieldName = "phoneNo", ErrorMessage = "Please enter mobile no" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(getInTouchForm.phoneNo) && (!Regex.IsMatch(getInTouchForm.phoneNo, (@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))))
                        {
                            errorResponse = new { FieldName = "phoneNo", ErrorMessage = "Please enter valid mobile number" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (getInTouchForm.email != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.email))
                        {
                            errorResponse = new { FieldName = "email", ErrorMessage = "Please enter email" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(getInTouchForm.email) && (!Regex.IsMatch(getInTouchForm.email, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
                        {
                            errorResponse = new { FieldName = "email", ErrorMessage = "Please enter valid email address" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (getInTouchForm.lookingFor != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.lookingFor))
                        {
                            errorResponse = new { FieldName = "lookingFor", ErrorMessage = "Please select what you are Looking for" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                        Sitecore.Data.Items.Item LookingTypeItem = db.GetItem("/sitecore/content/Ambuja/Global/Forms/LookingFor");

                        List<lookingFor> enquiryType = new List<lookingFor>();
                        foreach (Sitecore.Data.Items.Item LookinType in LookingTypeItem.Children)
                        {
                            lookingFor quiryType = new lookingFor();
                            quiryType.LookingforName = LookinType.Fields["Value"].Value;
                            enquiryType.Add(quiryType);
                        }

                        var enquiryTypeCount = enquiryType.Count(c => c.LookingforName.Equals(getInTouchForm.lookingFor, StringComparison.OrdinalIgnoreCase));
                        if (enquiryTypeCount == 0)
                        {
                            errorResponse = new { FieldName = "lookingFor", ErrorMessage = "Please select SolutionType" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (getInTouchForm.queryType != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.queryType))
                        {
                            errorResponse = new { FieldName = "queryType", ErrorMessage = "Please select Query" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                        Sitecore.Data.Items.Item QueryTypeItem = db.GetItem("/sitecore/content/Ambuja/Global/Forms/QueryType");

                        List<queryType> enquiryType = new List<queryType>();
                        foreach (Sitecore.Data.Items.Item QueryType in QueryTypeItem.Children)
                        {
                            queryType quiryType = new queryType();
                            quiryType.QuerytypeName = QueryType.Fields["Value"].Value;
                            enquiryType.Add(quiryType);
                        }

                        var enquiryTypeCount = enquiryType.Count(c => c.QuerytypeName.Equals(getInTouchForm.queryType, StringComparison.OrdinalIgnoreCase));
                        if (enquiryTypeCount == 0)
                        {
                            errorResponse = new { FieldName = "queryType", ErrorMessage = "Please select QueryType" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (getInTouchForm.state != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.state))
                        {
                            errorResponse = new { FieldName = "state", ErrorMessage = "Please select State" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                        Sitecore.Data.Items.Item StateTypeItem = db.GetItem("/sitecore/content/Ambuja/Global/Forms/State");

                        List<state> enquiryType = new List<state>();
                        foreach (Sitecore.Data.Items.Item StateType in StateTypeItem.Children)
                        {
                            state quiryType = new state();
                            quiryType.StateName = StateType.Fields["Value"].Value;
                            enquiryType.Add(quiryType);
                        }

                        var enquiryTypeCount = enquiryType.Count(c => c.StateName.Equals(getInTouchForm.state, StringComparison.OrdinalIgnoreCase));
                        if (enquiryTypeCount == 0)
                        {
                            errorResponse = new { FieldName = "state", ErrorMessage = "Please select State" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (getInTouchForm.district != null)
                    {
                        if (string.IsNullOrEmpty(getInTouchForm.district))
                        {
                            errorResponse = new { FieldName = "district", ErrorMessage = "Please select District" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                        Sitecore.Data.Items.Item StateTypeItem = db.GetItem("/sitecore/content/Ambuja/Global/Forms/State");

                        List<district> enquiryType = new List<district>();
                        foreach (Sitecore.Data.Items.Item stateItem in StateTypeItem.Children)
                        {
                            foreach (Sitecore.Data.Items.Item districtItem in stateItem.Children)
                            {
                                district quiryType = new district();
                                quiryType.CityName = districtItem.Fields["Value"].Value;
                                enquiryType.Add(quiryType);
                            }
                        }

                        var enquiryTypeCount = enquiryType.Count(c => c.CityName.Equals(getInTouchForm.district, StringComparison.OrdinalIgnoreCase));
                        if (enquiryTypeCount == 0)
                        {
                            errorResponse = new { FieldName = "district", ErrorMessage = "Please select District" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    getInTouchForm.ExpireAt = DateTimeOffset.Now.AddMinutes(20);

                    if (IsUserAllowedToSendSMS(getInTouchForm.phoneNo))
                    {
                        if (SaveToFileStorage(getInTouchForm))
                        {
                            string otpResult = Utils.SendOTP(getInTouchForm.phoneNo);
                            if (otpResult != "true")
                            {
                                return Json(new { Result = "OTP sending failed!" });
                            }
                            else
                            {
                                return Json(new { Result = "OTP Sent!" });
                            }
                        }
                        else
                        {
                            return Json(new { Result = "Some error occured, please contact your administrator" });
                        }
                    }
                    else
                    {
                        return Json(new { Result = $"Unable to send OTP, please try again after 30 mins" });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info($"Exception in Submit form method: {ex}", ex);
                return Json(new { Result = $"Exception in Submit form method: {ex}" });
            }
            return Json(ModelState.Where(i => i.Value.Errors.Count > 0).Select(x => new { Field = x.Key, ErrorMessage = x.Value.Errors[0].ErrorMessage }));
        }

        [HttpPost]
        [Route("verifyotp")]
        public ActionResult VerifyOtp(VerifyOtpModel verifyOtpModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var status = 0;
                    var getInTouchForm = FetchStoredGetInTouchFormModel(verifyOtpModel.PhoneNo);
                    Log.Info($"Stored GetInTouchForm Model: {getInTouchForm}", verifyOtpModel);
                    if (getInTouchForm != null)
                    {
                        FormBaseModel formBaseModel = new FormBaseModel();
                        formBaseModel.Id = Guid.NewGuid();
                        formBaseModel.FormDefinationId = Guid.Parse("E4AF56FF-F85F-490A-9DEC-E0DFCFE6EA8C");
                        formBaseModel.Created = DateTime.Now;
                        formBaseModel.IsRedacted = false;

                        if (getInTouchForm.firstName != null)
                        {
                            FormFieldData objfNameFormField = new FormFieldData();
                            objfNameFormField.FieldName = "firstName";
                            objfNameFormField.FormDefinationId = Guid.Parse("4683C374-6A29-48BD-BCAD-92F2C6BC7BE9");
                            objfNameFormField.Value = getInTouchForm.firstName;
                            objfNameFormField.FormEntryId = formBaseModel.Id;
                            objfNameFormField.ValueType = "System.String";

                            formBaseModel.objlistformFieldDatas.Add(objfNameFormField);
                        }

                        if (getInTouchForm.lastName != null)
                        {
                            FormFieldData objlNameFormField = new FormFieldData();
                            objlNameFormField.FieldName = "lastName";
                            objlNameFormField.FormDefinationId = Guid.Parse("74488B9A-6BB4-49E1-BD72-D01C904205CC");
                            objlNameFormField.Value = getInTouchForm.lastName;
                            objlNameFormField.FormEntryId = formBaseModel.Id;
                            objlNameFormField.ValueType = "System.String";

                            formBaseModel.objlistformFieldDatas.Add(objlNameFormField);
                        }
                        if (getInTouchForm.phoneNo != null)
                        {
                            FormFieldData objMobileNumberFormField = new FormFieldData();
                            objMobileNumberFormField.FieldName = "phoneNo";
                            objMobileNumberFormField.FormDefinationId = Guid.Parse("B317FA4D-5D7F-441E-BE36-A3DA2ED28046");
                            objMobileNumberFormField.Value = getInTouchForm.phoneNo;
                            objMobileNumberFormField.FormEntryId = formBaseModel.Id;
                            objMobileNumberFormField.ValueType = "System.String";

                            formBaseModel.objlistformFieldDatas.Add(objMobileNumberFormField);
                        }

                        if (getInTouchForm.email != null)
                        {
                            FormFieldData objEmailFormField = new FormFieldData();
                            objEmailFormField.FieldName = "email";
                            objEmailFormField.FormDefinationId = Guid.Parse("4C267183-4DCD-4DC3-B5F5-10F6AEF103D2");
                            objEmailFormField.Value = getInTouchForm.email;
                            objEmailFormField.FormEntryId = formBaseModel.Id;
                            objEmailFormField.ValueType = "System.String";

                            formBaseModel.objlistformFieldDatas.Add(objEmailFormField);
                        }

                        if (getInTouchForm.lookingFor != null)
                        {
                            FormFieldData objLookingFormField = new FormFieldData();
                            objLookingFormField.FieldName = "lookingFor";
                            objLookingFormField.FormDefinationId = Guid.Parse("8711F999-8365-449E-974C-136D12FA56DE");
                            objLookingFormField.Value = getInTouchForm.lookingFor;
                            objLookingFormField.FormEntryId = formBaseModel.Id;
                            objLookingFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                            formBaseModel.objlistformFieldDatas.Add(objLookingFormField);
                        }
                        if (getInTouchForm.queryType != null)
                        {
                            FormFieldData objQueryFormField = new FormFieldData();
                            objQueryFormField.FieldName = "queryType";
                            objQueryFormField.FormDefinationId = Guid.Parse("29CF7598-805B-4C28-A2B3-B986E0288CA2");
                            objQueryFormField.Value = getInTouchForm.queryType;
                            objQueryFormField.FormEntryId = formBaseModel.Id;
                            objQueryFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                            formBaseModel.objlistformFieldDatas.Add(objQueryFormField);
                        }

                        if (getInTouchForm.state != null)
                        {
                            FormFieldData objStateFormField = new FormFieldData();
                            objStateFormField.FieldName = "state";
                            objStateFormField.FormDefinationId = Guid.Parse("E43211C8-B5AC-4C3D-A2A1-05515BDF7045");
                            objStateFormField.Value = getInTouchForm.state;
                            objStateFormField.FormEntryId = formBaseModel.Id;
                            objStateFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                            formBaseModel.objlistformFieldDatas.Add(objStateFormField);
                        }

                        if (getInTouchForm.district != null)
                        {
                            FormFieldData objDistrictFormField = new FormFieldData();
                            objDistrictFormField.FieldName = "district";
                            objDistrictFormField.FormDefinationId = Guid.Parse("71392F2F-C0A0-4F73-862F-7D09353A2E38");
                            objDistrictFormField.Value = getInTouchForm.district;
                            objDistrictFormField.FormEntryId = formBaseModel.Id;
                            objDistrictFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                            formBaseModel.objlistformFieldDatas.Add(objDistrictFormField);
                        }

                        getInTouchForm.otp = verifyOtpModel.Otp;

                        //Calling Verify OTP API
                        string otpVerify = Utils.VerifyOTP(getInTouchForm);
                        Log.Info($"Verify OTP service response in GetInTouchController: {otpVerify}", otpVerify);
                        if (otpVerify == "success")
                        {
                            status = 1;
                            var isValid = getInTouchForm != null && getInTouchForm.phoneNo == verifyOtpModel.PhoneNo && getInTouchForm.ExpireAt > DateTimeOffset.Now;
                            if (isValid)
                            {
                                Delete(getInTouchForm.phoneNo);
                            }
                        }
                        else
                        {
                            return Json(new { Status = status, Result = "Seems like you entered wrong OTP." });
                        }
                        //Calling AdaniOneOnboarding API
                        //string UserOnboarding = Utils.UserOnboard(getInTouchForm);
                        //if (UserOnboarding != "success")
                        //{
                        //    return Json(new { Result = "Adani One User Onboarding Failed!!!!!" });
                        //}
                        //Calling FreshDeskAPI
                        string ticket = Utils.FreshdeskAPI(getInTouchForm);
                        if (!string.IsNullOrEmpty(ticket))
                        {
                            FormFieldData objticketFormField = new FormFieldData();
                            objticketFormField.FieldName = "ticket_id";
                            objticketFormField.FormDefinationId = Guid.Parse("B65D0390-0A98-43B0-A65C-78EC1FC27374");
                            objticketFormField.Value = ticket;
                            objticketFormField.FormEntryId = formBaseModel.Id;
                            objticketFormField.ValueType = "System.String";
                            formBaseModel.objlistformFieldDatas.Add(objticketFormField);

                            bool isSave = Save(formBaseModel);
                            bool containsNumber = ticket.Any(char.IsDigit);
                            if (isSave)
                            {
                                if (containsNumber)
                                {
                                    return Json(new { Status = status, Result = "Data saved successfully and Ticket Created!!", TicketId = ticket });
                                }
                                else
                                {
                                    return Json(new { Status = status, Result = "Data saved successfully but Ticket Not Created!!" });
                                }
                            }
                            else
                            {
                                status = 0;
                                return Json(new { Status = status, Result = $"There is some issue while saving the data but Ticket Created Successfully - {ticket}", TicketId = ticket });
                            }
                        }
                        else
                        {
                            // Ticket creation failed, save the data without the ticket information
                            bool isSave = Save(formBaseModel);

                            if (isSave)
                            {
                                status = 1;
                                return Json(new { Status = status, Result = "Data saved successfully but Ticket could not be created" });
                            }
                            else
                            {
                                status = 0;
                                return Json(new { Status = status, Result = "There is some issue while saving the data and Ticket could not be created" });
                            }
                        }
                    }
                    else
                    {
                        status = 0;
                        return Json(new { Status = status, Result = $"Unable to verify OTP!" });
                    }
                }
                return Json(ModelState.Where(i => i.Value.Errors.Count > 0).Select(x => new { Field = x.Key, ErrorMessage = x.Value.Errors[0].ErrorMessage }));
            }
            catch (Exception ex)
            {
                Log.Info($"Exception raised in VerifyOtp Method: {ex}", ex);
                return Json(new { Status = 0, Result = $"Exception in VerifyOtp Method: {ex}" });
            }
        }

        public bool Save(FormBaseModel objformBaseModel)
        {
            AmbujaGetInTouchFormsDataContext dbContext = new AmbujaGetInTouchFormsDataContext();
            try
            {
                FormEntry objformEntry = new FormEntry();
                objformEntry.Id = objformBaseModel.Id;
                objformEntry.IsRedacted = objformBaseModel.IsRedacted;
                objformEntry.Created = objformBaseModel.Created;
                objformEntry.FormDefinitionId = objformBaseModel.FormDefinationId;

                dbContext.FormEntries.InsertOnSubmit(objformEntry);
                dbContext.SubmitChanges();

                foreach (var item in objformBaseModel.objlistformFieldDatas)
                {
                    FieldData objfieldData = new FieldData();
                    objfieldData.ValueType = item.ValueType;
                    objfieldData.Value = item.Value;
                    objfieldData.FieldDefinitionId = item.FormDefinationId;
                    objfieldData.Id = Guid.NewGuid();
                    objfieldData.FormEntry = objformEntry;
                    objfieldData.FieldName = item.FieldName;
                    dbContext.FieldDatas.InsertOnSubmit(objfieldData);
                    dbContext.SubmitChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveToFileStorage(GetInTouchPostFormModel getInTouchPostFormModel)
        {
            try
            {
                //var getInTouchPostFormData = _getInTouchFormData.FirstOrDefault(x => x.phoneNo == getInTouchPostFormModel.phoneNo);
                //if (getInTouchPostFormData != null)
                //{
                //    getInTouchPostFormData.phoneNo = getInTouchPostFormModel.phoneNo;
                //    getInTouchPostFormData.ExpireAt = getInTouchPostFormModel.ExpireAt.DateTime;
                //}
                //else
                //{
                //    getInTouchPostFormData = new GetInTouchPostFormModel()
                //    {
                //        firstName = getInTouchPostFormModel.firstName,
                //        lastName = getInTouchPostFormModel.lastName,
                //        phoneNo = getInTouchPostFormModel.phoneNo,
                //        email = getInTouchPostFormModel.email,
                //        lookingFor = getInTouchPostFormModel.lookingFor,
                //        queryType = getInTouchPostFormModel.queryType,
                //        state = getInTouchPostFormModel.state,
                //        district = getInTouchPostFormModel.district,
                //        termsAndConditions = getInTouchPostFormModel.termsAndConditions,
                //        ExpireAt = getInTouchPostFormModel.ExpireAt.DateTime
                //    };
                //    _getInTouchFormData.Add(getInTouchPostFormData);
                //}

                var getInTouchPostFormData = new GetInTouchPostFormModel()
                {
                    firstName = getInTouchPostFormModel.firstName,
                    lastName = getInTouchPostFormModel.lastName,
                    phoneNo = getInTouchPostFormModel.phoneNo,
                    email = getInTouchPostFormModel.email,
                    lookingFor = getInTouchPostFormModel.lookingFor,
                    queryType = getInTouchPostFormModel.queryType,
                    state = getInTouchPostFormModel.state,
                    district = getInTouchPostFormModel.district,
                    termsAndConditions = getInTouchPostFormModel.termsAndConditions,
                    ExpireAt = getInTouchPostFormModel.ExpireAt.DateTime
                };
                _getInTouchFormData.Add(getInTouchPostFormData);
                System.IO.File.WriteAllLines(getInTouchFormFilePath, _getInTouchFormData.Select(x => JsonConvert.SerializeObject(x)));
                return true;
            }
            catch (Exception ex)
            {
                Log.Info($"Exception in SaveToFileStorage Method: {ex.InnerException}", ex);
                return false;
            }
        }

        public GetInTouchPostFormModel FetchStoredGetInTouchFormModel(string phoneNo)
        {
            try
            {
                var latestGetInTouchPostFormModelData = _getInTouchFormData
                    .Where(x => x.phoneNo == phoneNo)
                    .OrderByDescending(x => x.ExpireAt)
                    .FirstOrDefault();

                if (latestGetInTouchPostFormModelData != null)
                {
                    return new GetInTouchPostFormModel
                    {
                        firstName = latestGetInTouchPostFormModelData.firstName,
                        lastName = latestGetInTouchPostFormModelData.lastName,
                        phoneNo = latestGetInTouchPostFormModelData.phoneNo,
                        email = latestGetInTouchPostFormModelData.email,
                        lookingFor = latestGetInTouchPostFormModelData.lookingFor,
                        queryType = latestGetInTouchPostFormModelData.queryType,
                        state = latestGetInTouchPostFormModelData.state,
                        district = latestGetInTouchPostFormModelData.district,
                        termsAndConditions = latestGetInTouchPostFormModelData.termsAndConditions,
                        ExpireAt = latestGetInTouchPostFormModelData.ExpireAt.DateTime
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Info($"Exception in FetchStoredGetInTouchFormModel {ex.InnerException}", ex);
            }

            return null;
        }

        public bool Delete(string key)
        {
            var getInTouchFormData = _getInTouchFormData
                    .Where(x => x.phoneNo == key).ToList();

            foreach (var item in getInTouchFormData)
            {
                _getInTouchFormData.Remove(item);
            }
            System.IO.File.WriteAllLines(getInTouchFormFilePath, _getInTouchFormData.Select(x => JsonConvert.SerializeObject(x)));
            return true;
        }

        public bool IsUserAllowedToSendSMS(string phoneNo)
        {
            bool isAllowedToSendSMS = false;
            double timedifference = 0;
            var User = _getInTouchFormData
                    .Where(x => x.phoneNo == phoneNo)
                    .OrderByDescending(x => x.ExpireAt)
                    .FirstOrDefault();
            var userSMSSendingHistoryList = _getInTouchFormData
                    .Where(x => x.phoneNo == phoneNo && x.ExpireAt > DateTime.Now.AddMinutes(-30))
                    .OrderByDescending(x => x.ExpireAt)
                    .Take(3)
                    .ToList();

            int checktimediffer = 30;

            if (userSMSSendingHistoryList != null)
            {
                if (userSMSSendingHistoryList.Count() == 3)
                {
                    var objloginhistoryfirst = userSMSSendingHistoryList.OrderByDescending(c => c.ExpireAt).First();
                    var objloginhistorylast = userSMSSendingHistoryList.OrderByDescending(c => c.ExpireAt).Last();
                    timedifference = Math.Round(objloginhistoryfirst.ExpireAt.DateTime.Subtract(objloginhistorylast.ExpireAt.DateTime).TotalMinutes);
                    if (timedifference > checktimediffer)
                    {
                        isAllowedToSendSMS = true;
                    }
                    else
                    {
                        isAllowedToSendSMS = false;
                    }
                }
                else if (userSMSSendingHistoryList.Count() < 3)
                {
                    isAllowedToSendSMS = true;
                }
            }
            else
            {
                isAllowedToSendSMS = true;
            }

            return isAllowedToSendSMS;
        }
    }
}