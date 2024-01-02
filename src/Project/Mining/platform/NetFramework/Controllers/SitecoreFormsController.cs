using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models.Forms;
using Project.Mining.Website.Providers;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Project.Mining.Website.Controllers
{
    public class SitecoreFormsController : Controller
    {
        MiningFormDataContext dbContext = new MiningFormDataContext();
        Recaptchav2Provider captchav2 = new Recaptchav2Provider();
        // Post: SitecoreForms
        [HttpPost]
        [Route("requestcallbackform")]
        public ActionResult GetRequestCallBackForm(SitecoreFormsModel sitecoreFormsModel)
        {
            try
            {
                //if (!captchav2.IsReCaptchValidV3(Request.Headers.Get("g-recaptcha-response")))
                //{
                //    return Json(new { Result = "ReCapta validation failed.", Status = false });
                //}

                //Utils.ValidateAntiForgeryToken(HttpContext.Request);

                if (ModelState.IsValid)
                {
                    var errorResponse = new { FieldName = string.Empty, ErrorMessage = "" };
                    FormBaseModel formBaseModel = new FormBaseModel();
                    formBaseModel.Id = Guid.NewGuid();
                    formBaseModel.FormDefinationId = Guid.Parse("628EF1CD-BE8E-4A7F-B846-A5C2D443980C");
                    formBaseModel.Created = DateTime.Now;
                    formBaseModel.IsRedacted = false;

                    FormFieldData objNameFormField = new FormFieldData();
                    objNameFormField.FieldName = "Name";
                    objNameFormField.FormDefinationId = Guid.Parse("A4074F7C-E488-4222-AB7B-0926810AB61E");
                    objNameFormField.Value = sitecoreFormsModel.Name;
                    objNameFormField.FormEntryId = formBaseModel.Id;
                    objNameFormField.ValueType = "System.String";
                    if (string.IsNullOrEmpty(sitecoreFormsModel.Name))
                    {
                        errorResponse = new { FieldName = "Name", ErrorMessage = "Please enter name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(sitecoreFormsModel.Name) && (!Regex.IsMatch(sitecoreFormsModel.Name, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                    {
                        errorResponse = new { FieldName = "Name", ErrorMessage = "Please enter valid name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(sitecoreFormsModel.Name) && (sitecoreFormsModel.Name.Length > 128))
                    {
                        errorResponse = new { FieldName = "Name", ErrorMessage = "Maximum of 128 characters are allowed" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objNameFormField);

                    FormFieldData objMobileNumberFormField = new FormFieldData();
                    objMobileNumberFormField.FieldName = "MobileNumber";
                    objMobileNumberFormField.FormDefinationId = Guid.Parse("16750A13-D1D7-4E0A-A5DD-1C4CB106E9A5");
                    objMobileNumberFormField.Value = sitecoreFormsModel.MobileNumber;
                    objMobileNumberFormField.FormEntryId = formBaseModel.Id;
                    objMobileNumberFormField.ValueType = "System.String";

                    if (string.IsNullOrEmpty(sitecoreFormsModel.MobileNumber))
                    {
                        errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter mobile no" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(sitecoreFormsModel.MobileNumber) && (!Regex.IsMatch(sitecoreFormsModel.MobileNumber, (@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))))
                    {
                        errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter valid mobile number" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objMobileNumberFormField);

                    FormFieldData objEmailFormField = new FormFieldData();
                    objEmailFormField.FieldName = "Email";
                    objEmailFormField.FormDefinationId = Guid.Parse("D0C8FA3E-A6A0-4C7B-A683-841FDB68AEAB");
                    objEmailFormField.Value = sitecoreFormsModel.Email;
                    objEmailFormField.FormEntryId = formBaseModel.Id;
                    objEmailFormField.ValueType = "System.String";

                    if (string.IsNullOrEmpty(sitecoreFormsModel.Email))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter email" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(sitecoreFormsModel.Email) && (!Regex.IsMatch(sitecoreFormsModel.Email, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter valid email address" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objEmailFormField);



                    FormFieldData objPositionFormField = new FormFieldData();
                    objPositionFormField.FieldName = "Position";
                    objPositionFormField.FormDefinationId = Guid.Parse("326BEB66-6A41-4A01-A633-7270567C910D");
                    objPositionFormField.Value = sitecoreFormsModel.SolutionType;
                    objPositionFormField.FormEntryId = formBaseModel.Id;
                    objPositionFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                    if (string.IsNullOrEmpty(sitecoreFormsModel.SolutionType))
                    {
                        errorResponse = new { FieldName = "Position", ErrorMessage = "Please select candidatePosition" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    formBaseModel.objlistformFieldDatas.Add(objPositionFormField);
                    Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                    Sitecore.Data.Items.Item EnquiryTypeItem = db.GetItem("/sitecore/Forms/Mining/mining-request-a-call-form/Page/Section/Dropdown-List/Settings/Datasource");

                    List<SolutionType> solutionType = new List<SolutionType>();
                    foreach (Item EnquiryType in EnquiryTypeItem.Children)
                    {
                        SolutionType solutionTypes = new SolutionType();
                        solutionTypes.SolutionTypeName = EnquiryType.Fields["Value"].Value;
                        solutionType.Add(solutionTypes);
                    }

                    var solutionTypeCount = solutionType.Where(c => c.SolutionTypeName == sitecoreFormsModel.SolutionType).Count();

                    if (solutionTypeCount == 0)
                    {
                        errorResponse = new { FieldName = "SolutionType", ErrorMessage = "Please select SolutionType" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }


                    FormFieldData objMessageFormField = new FormFieldData();
                    objMessageFormField.FieldName = "Message";
                    objMessageFormField.FormDefinationId = Guid.Parse("7F8DA833-BFC8-4772-9F8D-E97900A52A78");
                    objMessageFormField.Value = sitecoreFormsModel.Message;
                    objMessageFormField.FormEntryId = formBaseModel.Id;
                    objMessageFormField.ValueType = "System.String";
                    if (string.IsNullOrEmpty(sitecoreFormsModel.Message))
                    {
                        errorResponse = new { FieldName = "Message", ErrorMessage = "Please enter name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(sitecoreFormsModel.Message) && (!Regex.IsMatch(sitecoreFormsModel.Message, (@"^[\w\s.,@:!?-]+$"))))
                    {
                        errorResponse = new { FieldName = "Message", ErrorMessage = "Please enter valid name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(sitecoreFormsModel.Message) && (sitecoreFormsModel.Message.Length > 700))
                    {
                        errorResponse = new { FieldName = "Message", ErrorMessage = "Maximum of 700 characters are allowed" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objMessageFormField);



                    bool isSave = Save(formBaseModel);

                    if (isSave)
                    {
                        return Json(new { Result = "Data saved successfully" });
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


        [HttpPost]
        [Route("ContactUsForm")]
        public ActionResult GetContactForm(ContactUsSitecoreFormsModel ContactUsSitecoreFormsModel)
        {
            try
            {
                //if (!captchav2.IsReCaptchValidV3(Request.Headers.Get("g-recaptcha-response")))
                //{
                //    return Json(new { Result = "ReCapta validation failed.", Status = false });
                //}

             //   Utils.ValidateAntiForgeryToken(HttpContext.Request);

                if (ModelState.IsValid)
                {
                    var errorResponse = new { FieldName = string.Empty, ErrorMessage = "" };
                    FormBaseModel formBaseModel = new FormBaseModel();
                    formBaseModel.Id = Guid.NewGuid();
                    formBaseModel.FormDefinationId = Guid.Parse("52387A2D-C29E-4741-A144-4AB0F03580DE");
                    formBaseModel.Created = DateTime.Now;
                    formBaseModel.IsRedacted = false;

                    FormFieldData objFirstNameFormField = new FormFieldData();
                    objFirstNameFormField.FieldName = "FirstName";
                    objFirstNameFormField.FormDefinationId = Guid.Parse("13CD047D-05D9-40A2-9837-FFADFE4DC3F5");
                    objFirstNameFormField.Value = ContactUsSitecoreFormsModel.FirstName;
                    objFirstNameFormField.FormEntryId = formBaseModel.Id;
                    objFirstNameFormField.ValueType = "System.String";
                    if (string.IsNullOrEmpty(ContactUsSitecoreFormsModel.FirstName))
                    {
                        errorResponse = new { FieldName = "FirstName", ErrorMessage = "Please enter FirstName" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.FirstName) && (!Regex.IsMatch(ContactUsSitecoreFormsModel.FirstName, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                    {
                        errorResponse = new { FieldName = "FirstName", ErrorMessage = "Please enter valid FirstName" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.FirstName) && (ContactUsSitecoreFormsModel.FirstName.Length > 128))
                    {
                        errorResponse = new { FieldName = "FirstName", ErrorMessage = "Maximum of 128 characters are allowed" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objFirstNameFormField);

                    FormFieldData objLastNameFormField = new FormFieldData();
                    objLastNameFormField.FieldName = "LastName";
                    objLastNameFormField.FormDefinationId = Guid.Parse("BC1F1432-C104-4B5B-9E26-61BF9878BFAB");
                    objLastNameFormField.Value = ContactUsSitecoreFormsModel.LastName;
                    objLastNameFormField.FormEntryId = formBaseModel.Id;
                    objLastNameFormField.ValueType = "System.String";
                    if (string.IsNullOrEmpty(ContactUsSitecoreFormsModel.LastName))
                    {
                        errorResponse = new { FieldName = "LastName", ErrorMessage = "Please enter LastName" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.LastName) && (!Regex.IsMatch(ContactUsSitecoreFormsModel.LastName, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                    {
                        errorResponse = new { FieldName = "LastName", ErrorMessage = "Please enter valid LastName" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.LastName) && (ContactUsSitecoreFormsModel.LastName.Length > 128))
                    {
                        errorResponse = new { FieldName = "LastName", ErrorMessage = "Maximum of 128 characters are allowed" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objLastNameFormField);


                    FormFieldData objMobileNumberFormField = new FormFieldData();
                    objMobileNumberFormField.FieldName = "MobileNumber";
                    objMobileNumberFormField.FormDefinationId = Guid.Parse("19B96F3C-F650-4D35-B629-856CD46C9E1A");
                    objMobileNumberFormField.Value = ContactUsSitecoreFormsModel.MobileNumber;
                    objMobileNumberFormField.FormEntryId = formBaseModel.Id;
                    objMobileNumberFormField.ValueType = "System.String";

                    if (string.IsNullOrEmpty(ContactUsSitecoreFormsModel.MobileNumber))
                    {
                        errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter mobile no" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.MobileNumber) && (!Regex.IsMatch(ContactUsSitecoreFormsModel.MobileNumber, (@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$"))))
                    {
                        errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter valid mobile number" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objMobileNumberFormField);



                    FormFieldData objEmailFormField = new FormFieldData();
                    objEmailFormField.FieldName = "Email";
                    objEmailFormField.FormDefinationId = Guid.Parse("3579E7DA-ECEF-43EC-85B8-52718DEDA4CC");
                    objEmailFormField.Value = ContactUsSitecoreFormsModel.Email;
                    objEmailFormField.FormEntryId = formBaseModel.Id;
                    objEmailFormField.ValueType = "System.String";

                    if (string.IsNullOrEmpty(ContactUsSitecoreFormsModel.Email))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter email" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.Email) && (!Regex.IsMatch(ContactUsSitecoreFormsModel.Email, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter valid email address" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objEmailFormField);



                    FormFieldData objPositionFormField = new FormFieldData();
                    objPositionFormField.FieldName = "Help Type";
                    objPositionFormField.FormDefinationId = Guid.Parse("B57D654C-8594-4606-B6A4-F68B495E0D10");
                    objPositionFormField.Value = ContactUsSitecoreFormsModel.HelpTypeDropdown;
                    objPositionFormField.FormEntryId = formBaseModel.Id;
                    objPositionFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                    if (string.IsNullOrEmpty(ContactUsSitecoreFormsModel.HelpTypeDropdown))
                    {
                        errorResponse = new { FieldName = "Position", ErrorMessage = "Please select What can we help you with?" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objPositionFormField);
                    Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                    Sitecore.Data.Items.Item MessageTypeItem = db.GetItem("/sitecore/Forms/Mining/mining-contact-us-form/Page/Section/HelpType-Dropdown/Settings/Datasource");

                    List<HelpTypeDropdown> helpType = new List<HelpTypeDropdown>();
                    foreach (Item EnquiryType in MessageTypeItem.Children)
                    {
                        HelpTypeDropdown HelpFieldType = new HelpTypeDropdown();
                        HelpFieldType.HelpTypeDropdownName = EnquiryType.Fields["Value"].Value;
                        helpType.Add(HelpFieldType);
                    }
                    var HelpTypeDropdownCount = helpType.Where(c => c.HelpTypeDropdownName == ContactUsSitecoreFormsModel.HelpTypeDropdown).Count();

                    if (HelpTypeDropdownCount == 0)
                    {
                        errorResponse = new { FieldName = "HelpType", ErrorMessage = "Please select HelpType" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }


                    FormFieldData objMessageFormField = new FormFieldData();
                    objMessageFormField.FieldName = "Message";
                    objMessageFormField.FormDefinationId = Guid.Parse("ED8A4A2B-28A7-488C-98B5-16577D964A55");
                    objMessageFormField.Value = ContactUsSitecoreFormsModel.Message;
                    objMessageFormField.FormEntryId = formBaseModel.Id;
                    objMessageFormField.ValueType = "System.String";
                    if (string.IsNullOrEmpty(ContactUsSitecoreFormsModel.Message))
                    {
                        errorResponse = new { FieldName = "Message", ErrorMessage = "Please enter name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.Message) && (!Regex.IsMatch(ContactUsSitecoreFormsModel.Message, (@"^[\w\s.,@:!?-]+$"))))
                    {
                        errorResponse = new { FieldName = "Message", ErrorMessage = "Please enter valid name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(ContactUsSitecoreFormsModel.Message) && (ContactUsSitecoreFormsModel.Message.Length > 700))
                    {
                        errorResponse = new { FieldName = "Message", ErrorMessage = "Maximum of 700 characters are allowed" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objMessageFormField);



                    bool isSave = Save(formBaseModel);

                    if (isSave)
                    {
                        return Json(new { Result = "Data saved successfully" });
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

        [HttpPost]
        [Route("Subscribeform")]
        public ActionResult GetSubscribeForm(SubscibeUsFormModel SubscibeUsFormModel)
        {
            try
            {
                //if (!captchav2.IsReCaptchValidV3(Request.Headers.Get("g-recaptcha-response")))
                //{
                //    return Json(new { Result = "ReCapta validation failed.", Status = false });
                //}

                //Utils.ValidateAntiForgeryToken(HttpContext.Request);

                if (ModelState.IsValid)
                {
                    var errorResponse = new { FieldName = string.Empty, ErrorMessage = "" };
                    FormBaseModel formBaseModel = new FormBaseModel();
                    formBaseModel.Id = Guid.NewGuid();
                    formBaseModel.FormDefinationId = Guid.Parse("5294DC01-664F-4B78-9E20-651F9D691572");
                    formBaseModel.Created = DateTime.Now;
                    formBaseModel.IsRedacted = false;


                    FormFieldData objEmailFormField = new FormFieldData();
                    objEmailFormField.FieldName = "Email";
                    objEmailFormField.FormDefinationId = Guid.Parse("1FF4FC21-A9BB-4638-82FD-711423EA2F20");
                    objEmailFormField.Value = SubscibeUsFormModel.Email;
                    objEmailFormField.FormEntryId = formBaseModel.Id;
                    objEmailFormField.ValueType = "System.String";

                    if (string.IsNullOrEmpty(SubscibeUsFormModel.Email))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter email" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(SubscibeUsFormModel.Email) && (!Regex.IsMatch(SubscibeUsFormModel.Email, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter valid email address" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objEmailFormField);


                    bool isSave = Save(formBaseModel);

                    if (isSave)
                    {
                        return Json(new { Result = "Data saved successfully" });
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
    }
}