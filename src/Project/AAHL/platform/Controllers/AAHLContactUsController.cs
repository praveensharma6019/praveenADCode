using Newtonsoft.Json;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.Forms;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Email.Model;
using Sitecore.Foundation.Email.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Project.AAHL.Website.Controllers
{
    public class AAHLContactUsController : Controller
    {
        [HttpPost]
        [Route("submitform")]
        public ActionResult Index(WriteToUsFormModel writeToUsFormModel)
        {
            try
            {
                Log.Info($"AAHL Write to Us model Received in Submitform Method: {JsonConvert.SerializeObject(writeToUsFormModel)}", writeToUsFormModel);
                // Fetch the user's IP address
                string ipAddress = Utils.GetClientIpAddress();
                string gCaptchaResponse = Request.Headers.Get("g-recaptcha-response");
                if (!Utils.IsReCaptchV2Valid(gCaptchaResponse))
                {
                    return Json(new { Result = "ReCaptcha validation failed." });
                }
                //Utils.ValidateAntiForgeryToken(HttpContext.Request);
                if (ModelState.IsValid)
                {
                    var errorResponse = new { FieldName = string.Empty, ErrorMessage = "" };
                    FormBaseModel formBaseModel = new FormBaseModel();
                    formBaseModel.Id = Guid.NewGuid();
                    formBaseModel.FormDefinationId = Guid.Parse("27A26A70-1D7D-4C94-97A1-C340C75DAFB8");
                    formBaseModel.Created = DateTime.Now;
                    formBaseModel.IsRedacted = false;

                    if (writeToUsFormModel.Name != null)
                    {
                        FormFieldData objNameFormField = new FormFieldData();
                        objNameFormField.FieldName = "Name";
                        objNameFormField.FormDefinationId = Guid.Parse("524F2B96-7D61-4DD4-BCCE-4020588BFD48");
                        objNameFormField.Value = writeToUsFormModel.Name;
                        objNameFormField.FormEntryId = formBaseModel.Id;
                        objNameFormField.ValueType = "System.String";
                        if (string.IsNullOrEmpty(writeToUsFormModel.Name))
                        {
                            errorResponse = new { FieldName = "Name", ErrorMessage = "Please enter name" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(writeToUsFormModel.Name) && (!Regex.IsMatch(writeToUsFormModel.Name, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                        {
                            errorResponse = new { FieldName = "Name", ErrorMessage = "Please enter valid name" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(writeToUsFormModel.Name) && (writeToUsFormModel.Name.Length > 128))
                        {
                            errorResponse = new { FieldName = "Name", ErrorMessage = "Maximum of 128 characters are allowed" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        formBaseModel.objlistformFieldDatas.Add(objNameFormField);
                    }

                    if (writeToUsFormModel.Email != null)
                    {
                        FormFieldData objEmailFormField = new FormFieldData();
                        objEmailFormField.FieldName = "Email";
                        objEmailFormField.FormDefinationId = Guid.Parse("83F090FB-AA85-46C8-AF17-390A664BAB0F");
                        objEmailFormField.Value = writeToUsFormModel.Email;
                        objEmailFormField.FormEntryId = formBaseModel.Id;
                        objEmailFormField.ValueType = "System.String";

                        if (string.IsNullOrEmpty(writeToUsFormModel.Email))
                        {
                            errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter email" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(writeToUsFormModel.Email) && (!Regex.IsMatch(writeToUsFormModel.Email, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
                        {
                            errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter valid email address" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        formBaseModel.objlistformFieldDatas.Add(objEmailFormField);
                    }

                    if (!string.IsNullOrEmpty(writeToUsFormModel.CountryCode))
                    {
                        FormFieldData objCountryCodeFormField = new FormFieldData();
                        objCountryCodeFormField.FieldName = "CountryCode";
                        objCountryCodeFormField.FormDefinationId = Guid.Parse("{77B1BB90-2F79-4532-8F98-314D402919BB}");
                        objCountryCodeFormField.Value = writeToUsFormModel.CountryCode;
                        objCountryCodeFormField.FormEntryId = formBaseModel.Id;
                        objCountryCodeFormField.ValueType = "System.String";

                        if (string.IsNullOrEmpty(writeToUsFormModel.CountryCode))
                        {
                            errorResponse = new { FieldName = "CountryCode", ErrorMessage = "Please select country code" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        formBaseModel.objlistformFieldDatas.Add(objCountryCodeFormField);

                        Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                        Item countryCodeFolderItem = db.GetItem(ID.Parse("{4591A302-2601-4B5E-B9CA-1723CC0AE107}"));

                        List<CountryDetails> countryCodes = new List<CountryDetails>();
                        foreach (Item countryDetail in countryCodeFolderItem.Children)
                        {
                            CountryDetails countryDetails = new CountryDetails();
                            countryDetails.CountryName = countryDetail.Fields["CountryName"].Value;
                            countryDetails.CountryCode = countryDetail.Fields["DialCode"].Value;
                            countryCodes.Add(countryDetails);
                        }

                        var isCountryCodePresentInSitecore = countryCodes.Where(c => c.CountryCode== writeToUsFormModel.CountryCode).Count();

                        if (isCountryCodePresentInSitecore == 0)
                        {
                            errorResponse = new { FieldName = "CountryCode", ErrorMessage = "Please select country code" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (writeToUsFormModel.MobileNumber != null)
                    {
                        FormFieldData objMobileNumberFormField = new FormFieldData();
                        objMobileNumberFormField.FieldName = "MobileNumber";
                        objMobileNumberFormField.FormDefinationId = Guid.Parse("6DF9E672-C4EF-4BF7-B85F-A96BC60EA306");
                        objMobileNumberFormField.Value = writeToUsFormModel.MobileNumber;
                        objMobileNumberFormField.FormEntryId = formBaseModel.Id;
                        objMobileNumberFormField.ValueType = "System.String";

                        if (string.IsNullOrEmpty(writeToUsFormModel.MobileNumber))
                        {
                            errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter mobile number" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(writeToUsFormModel.MobileNumber) && (!Regex.IsMatch(writeToUsFormModel.MobileNumber, (@"^\d+$"))))
                        {
                            errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter valid mobile number" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        formBaseModel.objlistformFieldDatas.Add(objMobileNumberFormField);
                    }

                    if (writeToUsFormModel.EnquiryType != null)
                    {
                        FormFieldData objEnquiryTypeFormField = new FormFieldData();
                        objEnquiryTypeFormField.FieldName = "EnquiryType";
                        objEnquiryTypeFormField.FormDefinationId = Guid.Parse("E03E53F3-88EE-469A-BB33-F4A0EFC63FB4");
                        objEnquiryTypeFormField.Value = writeToUsFormModel.EnquiryType;
                        objEnquiryTypeFormField.FormEntryId = formBaseModel.Id;
                        objEnquiryTypeFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                        if (string.IsNullOrEmpty(writeToUsFormModel.EnquiryType))
                        {
                            errorResponse = new { FieldName = "EnquiryType", ErrorMessage = "Please select enquiry type" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        formBaseModel.objlistformFieldDatas.Add(objEnquiryTypeFormField);

                        Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                        Item EnquiryTypeItem = db.GetItem("/sitecore/Forms/AAHL/write-to-us-form/Page/Section/EnquiryType/Settings/Datasource");

                        List<EnquiryType> enquiryType = new List<EnquiryType>();
                        foreach (Item EnquiryType in EnquiryTypeItem.Children)
                        {
                            EnquiryType queryType = new EnquiryType();
                            queryType.EnquiryTypeName = EnquiryType.DisplayName;
                            queryType.EnquiryTypeValue = System.Convert.ToInt32(EnquiryType.Fields["Value"].Value);
                            enquiryType.Add(queryType);
                        }

                        var enquiryTypeCount = enquiryType.Where(c => c.EnquiryTypeName == writeToUsFormModel.EnquiryType).Count();

                        if (enquiryTypeCount == 0)
                        {
                            errorResponse = new { FieldName = "EnquiryType", ErrorMessage = "Please select enquiry type" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                    }

                    if (writeToUsFormModel.Message != null)
                    {
                        FormFieldData objMessageFormField = new FormFieldData();
                        objMessageFormField.FieldName = "Message";
                        objMessageFormField.FormDefinationId = Guid.Parse("0A4188AB-D6FB-4B3E-90E8-77BD7D59184E");
                        objMessageFormField.Value = writeToUsFormModel.Message;
                        objMessageFormField.FormEntryId = formBaseModel.Id;
                        objMessageFormField.ValueType = "System.String";
                        if (string.IsNullOrEmpty(writeToUsFormModel.Message))
                        {
                            errorResponse = new { FieldName = "Message", ErrorMessage = "Please enter message" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(writeToUsFormModel.Message) && (!Regex.IsMatch(writeToUsFormModel.Message, (@"^[\w\s.,@:!?-]+$"))))
                        {
                            errorResponse = new { FieldName = "Message", ErrorMessage = "Please enter valid message" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        else if (!string.IsNullOrEmpty(writeToUsFormModel.Message) && (writeToUsFormModel.Name.Length > 256))
                        {
                            errorResponse = new { FieldName = "Message", ErrorMessage = "Maximum of 700 characters are allowed" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                        formBaseModel.objlistformFieldDatas.Add(objMessageFormField);
                    }

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        FormFieldData objIpFormField = new FormFieldData();
                        objIpFormField.FieldName = "IPAddress";
                        objIpFormField.FormDefinationId = Guid.Parse("D549FBD4-0613-454C-B124-1FC86111651C");
                        objIpFormField.Value = ipAddress;
                        objIpFormField.FormEntryId = formBaseModel.Id;
                        objIpFormField.ValueType = "System.String";
                        formBaseModel.objlistformFieldDatas.Add(objIpFormField);
                    }

                    var crmResponse = Utils.SendDataToCRM(writeToUsFormModel);
                    if (crmResponse != null)
                    {
                        Log.Info($"crmResponse received from CRM API: {JsonConvert.SerializeObject(crmResponse)}", crmResponse);
                        if (!string.IsNullOrEmpty(crmResponse.CaseNumber))
                        {
                            FormFieldData objIpFormField = new FormFieldData();
                            objIpFormField.FieldName = "CaseNumber";
                            objIpFormField.FormDefinationId = Guid.Parse("D549FBD4-0613-454C-B124-1FC86111651C");
                            objIpFormField.Value = crmResponse.CaseNumber;
                            objIpFormField.FormEntryId = formBaseModel.Id;
                            objIpFormField.ValueType = "System.String";
                            formBaseModel.objlistformFieldDatas.Add(objIpFormField);
                        }

                        if (!string.IsNullOrEmpty(crmResponse.Message))
                        {
                            FormFieldData objIpFormField = new FormFieldData();
                            objIpFormField.FieldName = "CRMMessage";
                            objIpFormField.FormDefinationId = Guid.Parse("D549FBD4-0613-454C-B124-1FC86111651C");
                            objIpFormField.Value = crmResponse.CaseNumber + "," + crmResponse.Message;
                            objIpFormField.FormEntryId = formBaseModel.Id;
                            objIpFormField.ValueType = "System.String";
                            formBaseModel.objlistformFieldDatas.Add(objIpFormField);
                        }
                    }
                    else 
                    {
                        Log.Info($"Received null response from CRM API", "");
                    }

                    bool isSave = Save(formBaseModel);

                    if (isSave)
                    {
                        var emailParametrs = Context.Database.GetItem("27A26A70-1D7D-4C94-97A1-C340C75DAFB8")?.Axes.GetDescendant("send email").Fields["Parameters"].Value;

                        if (emailParametrs == null)
                        {
                            return Json(new { Result = "Data saved but there's an issue with mail send configurations." });
                        }

                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(emailParametrs);
                        responseModel.Subject = ReplaceToken(writeToUsFormModel, responseModel.Subject);
                        responseModel.Body = ReplaceToken(writeToUsFormModel, responseModel.Body);
                        responseModel.To = writeToUsFormModel.Email;
                        var response = WebApiHelper.SentEmail(responseModel);

                        if (writeToUsFormModel.EnquiryType != null)
                        {
                            Item emailContext = Sitecore.Context.Database.GetItem("{C080D77C-575F-40E6-A3FB-8961C5365E30}");
                            Item enquiryTypeContext = Sitecore.Context.Database.GetItem("{B0C7B2AB-9B1D-473A-A76C-AE7A64B8BAA0}");
                            ResponseModel res = new ResponseModel();
                            //  res.Body = emailContext.Fields["Body"].Value;
                            string bodyText = string.Empty;
                            bodyText = emailContext.Fields["Body"].Value;

                            bodyText = bodyText.Replace("{Subject}", emailContext.Fields["SubjectName"].Value);
                            bodyText = bodyText.Replace("{Message}", writeToUsFormModel.Message.ToString());
                            bodyText = bodyText.Replace("{Name}", writeToUsFormModel.Name.ToString());
                            bodyText = bodyText.Replace("{Email}", writeToUsFormModel.Email.ToString());
                            bodyText = bodyText.Replace("{Mobile}", writeToUsFormModel.MobileNumber.ToString());
                            bodyText = bodyText.Replace("{EnquiryType}", writeToUsFormModel.EnquiryType.ToString());

                            res.Body = bodyText;
                            res.To = emailContext.Fields["MediaEmail"].Value;
                            res.From = emailContext.Fields["FromEmail"].Value;
                            res.Subject = emailContext.Fields["SubjectName"].Value;
                            if (writeToUsFormModel.EnquiryType == enquiryTypeContext.Fields["Media"].Value)
                            {
                                res.To = emailContext.Fields["MediaEmail"].Value;
                            }
                            if (writeToUsFormModel.EnquiryType == enquiryTypeContext.Fields["Career"].Value)
                            {
                                res.To = emailContext.Fields["CareerOpportunitiesEmail"].Value;
                            }
                            if (writeToUsFormModel.EnquiryType == enquiryTypeContext.Fields["Advertising"].Value)
                            {
                                res.To = emailContext.Fields["AdvertisingOpportunitiesEmail"].Value;
                            }
                            if (writeToUsFormModel.EnquiryType == enquiryTypeContext.Fields["Fraud"].Value)
                            {
                                res.To = emailContext.Fields["FraudMisconductEmail"].Value;
                            }
                            var mailres = WebApiHelper.SentEmail(res);
                        }

                        if (response != null && response.isSucess == false)
                        {
                            return Json(new { Result = response.ErrorMessage + "Your Form Saved Successfully But EnquiryTypeEmailSending Fail" });
                        }

                        return Json(new { Result = "Data Saved and Mail Sent Successfully" });
                        //  return Json(new { Result = " saved successfully" });
                    }
                    else
                    {
                        return Json(new { Result = "There is some issue while saving the data" });
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().ToString() == "System.Web.Mvc.HttpAntiForgeryException")
                {
                    return Json(new { Result = "Antiforgery validation failed." });
                }
            }
            return Json(ModelState.Where(i => i.Value.Errors.Count > 0).Select(x => new { Field = x.Key, ErrorMessage = x.Value.Errors[0].ErrorMessage }));
        }

        public bool Save(FormBaseModel objformBaseModel)
        {
            AAHLFormsDataContext dbContext = new AAHLFormsDataContext();
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

        private static string ReplaceToken(WriteToUsFormModel writeToUsFormModel, string message)
        {
            Type type = writeToUsFormModel.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                message = message.Replace($"[{property.Name}]", property.GetValue(writeToUsFormModel).ToString());
            }
            return message;
        }
    }
}