using Newtonsoft.Json;
using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Providers;
using Sitecore.Foundation.Email.Model;
using Sitecore.Foundation.Email.Utils;
using Sitecore.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Project.AdaniInternationalSchool.Website.Controllers
{
    public class AISContactUsController : Controller
    {
        aisFormsDataContext dbContext = new aisFormsDataContext();
        // GET: AISContactUs
        [HttpPost]
        public ActionResult Index(ContactUsModel contactUsModel)
        {
            try
            {
                if (!Utils.IsReCaptchV2Valid(Request.Form.Get("g-recaptcha-response")))
                {
                    return Json(new { Result = "ReCapta validation failed." });
                }

                Utils.ValidateAntiForgeryToken(HttpContext.Request);

                if (Request.Files.Count > 0)
                {
                    var fielname = contactUsModel.Resume.FileName;

                    if (!string.IsNullOrEmpty(fielname))
                    {

                        string extension = Path.GetExtension(fielname);
                        string[] allowedExtenstions = new string[] { ".pdf", ".doc", ".docx" };
                        var mime = contactUsModel.Resume.ContentType;
                        string[] allowedMimeTypes = new string[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword" };

                        if (!allowedExtenstions.Contains(extension) || !allowedMimeTypes.Contains(mime.ToLower()))
                        {
                            ModelState.AddModelError("Resume", $"Only .pdf, .doc and .docx files allowed.");
                        }

                        int maxFileSize = 1024 * 1024 * 3; //3 MB
                        if (contactUsModel.Resume.ContentLength > maxFileSize)
                        {
                            ModelState.AddModelError("Resume", $"Maximum allowed file size is 3MB");
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var errorResponse = new { FieldName = string.Empty, ErrorMessage = "" };
                    FormBaseModel formBaseModel = new FormBaseModel();
                    formBaseModel.Id = Guid.NewGuid();
                    formBaseModel.FormDefinationId = Guid.Parse("03E191A9-47FA-4342-910B-DDC197AE89A0");
                    formBaseModel.Created = DateTime.Now;
                    formBaseModel.IsRedacted = false;
                    FormFileStorage formFileStorage = new FormFileStorage();
                    var fileURL = string.Empty;
                    if (Request.Files.Count > 0)
                    {
                        
                        Stream fs = contactUsModel.Resume.InputStream;
                       byte[] UploadedFile = new byte[0];
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            UploadedFile = br.ReadBytes((Int32)fs.Length);

                            formFileStorage.Id = Guid.NewGuid();
                            formFileStorage.FileName = contactUsModel.Resume.FileName;
                            formFileStorage.Committed = true;
                            formFileStorage.Created = DateTime.Now;
                            formFileStorage.FileContent = UploadedFile;
                            formBaseModel.formFileStorage = formFileStorage;

                            //Upload FIle to Blob
                            AISBlobProvider blobProvider = new AISBlobProvider();
                            formFileStorage.FileName = blobProvider.UploadFileToBlob(UploadedFile, contactUsModel.Resume.FileName).ToString();
                            fileURL = formFileStorage.FileName;
                        }
                    }

                    FormFieldData objNameFormField = new FormFieldData();
                    objNameFormField.FieldName = "Name";
                    objNameFormField.FormDefinationId = Guid.Parse("4299957F-C7F6-4F59-B834-318B0356984F");
                    objNameFormField.Value = contactUsModel.Name;
                    objNameFormField.FormEntryId = formBaseModel.Id;
                    objNameFormField.ValueType = "System.String";
                    if (string.IsNullOrEmpty(contactUsModel.Name))
                    {
                        errorResponse = new { FieldName = "Name", ErrorMessage = "Please enter name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(contactUsModel.Name) && (!Regex.IsMatch(contactUsModel.Name, (@"^[a-zA-Z][a-zA-Z ]*$"))))
                    {
                        errorResponse = new { FieldName = "Name", ErrorMessage = "Please enter valid name" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(contactUsModel.Name) && (contactUsModel.Name.Length > 128))
                    {
                        errorResponse = new { FieldName = "Name", ErrorMessage = "!Only 50 characters are allowed" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    formBaseModel.objlistformFieldDatas.Add(objNameFormField);

                    FormFieldData objMobileNumberFormField = new FormFieldData();
                    objMobileNumberFormField.FieldName = "MobileNumber";
                    objMobileNumberFormField.FormDefinationId = Guid.Parse("326268C7-787F-4AEB-84A1-B86B22591A67");
                    objMobileNumberFormField.Value = contactUsModel.MobileNumber;
                    objMobileNumberFormField.FormEntryId = formBaseModel.Id;
                    objMobileNumberFormField.ValueType = "System.String";

                    formBaseModel.objlistformFieldDatas.Add(objMobileNumberFormField);
                    if (string.IsNullOrEmpty(contactUsModel.MobileNumber))
                    {
                        errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter mobile no" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(contactUsModel.MobileNumber) && (!Regex.IsMatch(contactUsModel.MobileNumber, (@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$"))))
                    {
                        errorResponse = new { FieldName = "MobileNumber", ErrorMessage = "Please enter valid mobile number" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                   
                    FormFieldData objEmailFormField = new FormFieldData();
                    objEmailFormField.FieldName = "Email";
                    objEmailFormField.FormDefinationId = Guid.Parse("695D46B6-3357-4CCC-BA7E-F3A6BD495A7F");
                    objEmailFormField.Value = contactUsModel.Email;
                    objEmailFormField.FormEntryId = formBaseModel.Id;
                    objEmailFormField.ValueType = "System.String";

                    formBaseModel.objlistformFieldDatas.Add(objEmailFormField);
                    if (string.IsNullOrEmpty(contactUsModel.Email))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter email" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(contactUsModel.Email) && (!Regex.IsMatch(contactUsModel.Email, (@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))))
                    {
                        errorResponse = new { FieldName = "Email", ErrorMessage = "Please enter valid email address" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    FormFieldData objPositionFormField = new FormFieldData();
                    objPositionFormField.FieldName = "Position";
                    objPositionFormField.FormDefinationId = Guid.Parse("383343A4-1C28-4878-8E26-6F6F4DE272ED");
                    objPositionFormField.Value = contactUsModel.Position;
                    objPositionFormField.FormEntryId = formBaseModel.Id;
                    objPositionFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                    formBaseModel.objlistformFieldDatas.Add(objPositionFormField);
                    if (string.IsNullOrEmpty(contactUsModel.Position))
                    {
                        errorResponse = new { FieldName = "Position", ErrorMessage = "Please select candidatePosition" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    FormFieldData objOrganisationFormField = new FormFieldData();
                    objOrganisationFormField.FieldName = "Organization";
                    objOrganisationFormField.FormDefinationId = Guid.Parse("A9ACD93B-52F9-444D-A384-91C9DC9EE102");
                    objOrganisationFormField.Value = contactUsModel.Organization;
                    objOrganisationFormField.FormEntryId = formBaseModel.Id;
                    objOrganisationFormField.ValueType = "System.String";

                    formBaseModel.objlistformFieldDatas.Add(objOrganisationFormField);
                    if (string.IsNullOrEmpty(contactUsModel.Organization))
                    {
                        errorResponse = new { FieldName = "Organization", ErrorMessage = "Please enter candidateOrg" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (!string.IsNullOrEmpty(contactUsModel.Organization) && (contactUsModel.Organization.Length > 256))
                    {
                        errorResponse = new { FieldName = "Organization", ErrorMessage = "!Only 256 characters are allowed" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    FormFieldData objExperienceFormField = new FormFieldData();
                    objExperienceFormField.FieldName = "Experience";
                    objExperienceFormField.FormDefinationId = Guid.Parse("AFCD6CAD-3A77-4EBC-B62B-419036AC60B8");
                    objExperienceFormField.Value = contactUsModel.Experience;
                    objExperienceFormField.FormEntryId = formBaseModel.Id;
                    objExperienceFormField.ValueType = "System.Collections.Generic.List`1[System.String]";

                    formBaseModel.objlistformFieldDatas.Add(objExperienceFormField);
                    if (string.IsNullOrEmpty(contactUsModel.Experience))
                    {
                        errorResponse = new { FieldName = "Experience", ErrorMessage = "Please select candidateExp" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    List<TotalExperience> TotExperience = new List<TotalExperience>
                  {
                       new TotalExperience { TotalExperienceValue=1, TotalExperienceName="0 - 11 Months"},
                       new TotalExperience { TotalExperienceValue=2, TotalExperienceName="1 Year - 3 Years"},
                       new TotalExperience { TotalExperienceValue=3, TotalExperienceName="3 Years - 5 Years"},
                       new TotalExperience { TotalExperienceValue=4, TotalExperienceName="5 Years - 10 Years"},
                       new TotalExperience { TotalExperienceValue=5, TotalExperienceName="10 Years - 15 Years"},
                       new TotalExperience { TotalExperienceValue=6, TotalExperienceName="15+ Years"},
                   };

                    List<PositionInterested> PosInterested = new List<PositionInterested>
                {
                   new PositionInterested { PositionInterestedValue=1, PositionInterestedName="Pre-Primary Home Room Teacher"},
                   new PositionInterested { PositionInterestedValue=2, PositionInterestedName="PYP Home Room Teacher"},
                   new PositionInterested { PositionInterestedValue=3, PositionInterestedName="PYP Hindi Teacher"},
                   new PositionInterested { PositionInterestedValue=4, PositionInterestedName="PYP Music Teacher"},
                   new PositionInterested { PositionInterestedValue=5, PositionInterestedName="PYP Dance Teacher"},
                   new PositionInterested { PositionInterestedValue=6, PositionInterestedName="PYP Art Teacher"},
                   new PositionInterested { PositionInterestedValue=7, PositionInterestedName="PYP Librarian"},
                   new PositionInterested { PositionInterestedValue=8, PositionInterestedName="Mathematics Teacher (Middle School/IGCSE/AS/A Level)"},
                   new PositionInterested { PositionInterestedValue=9, PositionInterestedName="Physics Teacher (Middle School/IGCSE/AS/A Level)"},
                   new PositionInterested { PositionInterestedValue=10, PositionInterestedName="English Teacher (Middle School/IGCSE/AS/A Level)"},
                   new PositionInterested { PositionInterestedValue=11, PositionInterestedName="Economics Teacher (Middle School/IGCSE/AS/A Level)"},
                   new PositionInterested { PositionInterestedValue=12, PositionInterestedName="Librarian (Middle School/IGCSE/AS/A Level)"},
                   new PositionInterested { PositionInterestedValue=13, PositionInterestedName="Special Educator (SEN)"},
                   new PositionInterested { PositionInterestedValue=14, PositionInterestedName="Geography Teacher (Middle School/IGCSE/AS/A Level)"},
                   new PositionInterested { PositionInterestedValue=15, PositionInterestedName="Dance Teacher (Middle School/IGCSE)"},
                   new PositionInterested { PositionInterestedValue=16, PositionInterestedName="Music Teacher (Middle School/IGCSE)"},
                   new PositionInterested { PositionInterestedValue=17, PositionInterestedName="Science Lab Assistant"},
                };

                    var PosInterestedCount = PosInterested.Where(c => c.PositionInterestedName == contactUsModel.Position).Count();
                    var TotExperienceCount = TotExperience.Where(c => c.TotalExperienceName == contactUsModel.Experience).Count();

                    if (PosInterestedCount == 0)
                    {
                        errorResponse = new { FieldName = "Position", ErrorMessage = "Please select candidatePosition" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    if (TotExperienceCount == 0)
                    {
                        errorResponse = new { FieldName = "Experience", ErrorMessage = "Please select candidateExp" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }

                    FormFieldData objResumeFormField = new FormFieldData();
                    objResumeFormField.FieldName = "Resume";
                    objResumeFormField.FormDefinationId = Guid.Parse("ABA7283E-89E1-4515-A759-B9B076261CC1");
                    objResumeFormField.Value = Convert.ToString(formFileStorage.Id);
                    objResumeFormField.FormEntryId = formBaseModel.Id;
                    objResumeFormField.ValueType = "System.Collections.Generic.List`1[Sitecore.ExperienceForms.Data.Entities.StoredFileInfo]";

                    formBaseModel.objlistformFieldDatas.Add(objResumeFormField);

                    FormFieldData objAgreementFormField = new FormFieldData();
                    objAgreementFormField.FieldName = "Agreement";
                    objAgreementFormField.FormDefinationId = Guid.Parse("59361CC6-264E-4A55-8770-4E1FC65731EA");
                    objAgreementFormField.Value = Convert.ToString(contactUsModel.Agreement);
                    objAgreementFormField.FormEntryId = formBaseModel.Id;
                    objAgreementFormField.ValueType = "System.Boolean";
                    if (!string.IsNullOrEmpty(Convert.ToString(contactUsModel.Agreement)) && (contactUsModel.Agreement == false))
                    {
                        errorResponse = new { FieldName = "Agreement", ErrorMessage = "Please accept the terms." };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    formBaseModel.objlistformFieldDatas.Add(objAgreementFormField);

                    bool isSave = Save(formBaseModel);

                    if (isSave)
                    {
                        var emailParametrs = Sitecore.Context.Database.GetItem("03E191A9-47FA-4342-910B-DDC197AE89A0")?.Axes.GetDescendant("send email ais").Fields["Parameters"].Value;
                        if (emailParametrs == null)
                        {
                            return Json(new { Result = "Data saved but there's an issue with mail send configurations." });
                        }

                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(emailParametrs);

                        responseModel.Subject = ReplaceToken(contactUsModel, responseModel.Subject);

                        responseModel.Body = ReplaceToken(contactUsModel, responseModel.Body);
                        responseModel.Body = responseModel.Body.Replace("$$ResumeURL$$", fileURL);
                        var response = WebApiHelper.SentEmail(responseModel);
                        if (response != null && response.isSucess == false)
                        {
                            return Json(new { Result = response.ErrorMessage + "Your Form Saved Successfully But EmailSending Fail" });
                        }

                        return Json(new { Result = "Form Saved and Mail Sent Successfully" });
                    }
                }
            }
            catch (Exception ex)
            {
                if(ex.GetType().ToString() == "System.Web.Mvc.HttpAntiForgeryException")
                {
                    return Json(new { Result = "Antiforgery validation failed." });
                }
            }

            return Json(ModelState.Where(i => i.Value.Errors.Count > 0).Select(x => new { Field = x.Key, ErrorMessage = x.Value.Errors[0].ErrorMessage }));
        }
        private static string ReplaceToken(ContactUsModel contactUsModel, string message)
        {
            Type type = contactUsModel.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                message = message.Replace($"[{property.Name}]", property.GetValue(contactUsModel).ToString());
            }
            return message;
        }

        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
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

                var formFileStorage = objformBaseModel.formFileStorage;

                FileStorage objfileStorage = new FileStorage();
                objfileStorage.Id = formFileStorage.Id;
                objfileStorage.FileName = formFileStorage.FileName;
                objfileStorage.Created = formFileStorage.Created;
                objfileStorage.FileContent = formFileStorage.FileContent;
                objfileStorage.Committed = formFileStorage.Committed;
                dbContext.FileStorages.InsertOnSubmit(objfileStorage);
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
