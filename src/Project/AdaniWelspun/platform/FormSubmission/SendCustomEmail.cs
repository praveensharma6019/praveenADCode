using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Mvc.Models.SubmitActions;
using Sitecore.ExperienceForms.Processing;
using Sitecore.Foundation.Email.Model;
using Sitecore.Foundation.Email.Utils;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.Data.Items;
using Adani.BAU.AdaniWelspunSXA.Project.Templates;

namespace Adani.BAU.AdaniWelspunSXA.Project.FormSubmission
{
    public class SendCustomEmail : Sitecore.ExperienceForms.Mvc.Processing.SubmitActions.SendEmail
    {
        private string _errorMessage;
        private static readonly Regex alphaNumber = new Regex("^[a-zA-Z ]*$");
        private static readonly Regex alphaNumberValid = new Regex("^[a-zA-Z ]+$");
        private static readonly Regex emailRegex = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");
        private static readonly Regex NumRegex = new Regex(@"^(\+\d{1,3}[- ]?)?\d{10}$");
        char ch = '.';
        int flag = 0;
        string[] contenttypeExtenstion = new string[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword" };
        //ListViewModel listView = new ListViewModel();
        /// <summary>
        /// Initializes a new instance of the <see cref="SendCustomEmail"/> class.
        /// </summary>
        /// <param name="submitActionData">submitActionData.</param>
        public SendCustomEmail(ISubmitActionData submitActionData)
            : base(submitActionData)
        {
        }

        /// <summary>
        /// Executes the action with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formSubmitContext">The form submit context.</param>
        /// <returns><c>true</c> if the action is executed correctly; otherwise <c>false</c>.</returns>
        protected override bool Execute(SendEmailData data, FormSubmitContext formSubmitContext)
        {
            try
            {
                Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

                if (!formSubmitContext.HasErrors)
                {
                    var FullName = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Full Name"));
                    if (FullName != null)
                    {
                        var propertyNameValid = FullName.GetType().GetProperty("Value");
                        string strFullNameValid = Convert.ToString(propertyNameValid.GetValue(FullName));
                        if (alphaNumber.IsMatch(strFullNameValid))
                        {
                            var property = FullName.GetType().GetProperty("Value");
                            var strFullName = Convert.ToString(property.GetValue(FullName));
                            data.Subject = data.Subject.Replace("[Full Name]", strFullName);
                            data.Body = data.Body.Replace("[Full Name]", strFullName);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct Name!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }



                    var Subject = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Subject"));
                    if (Subject != null)
                    {
                        var propertySubjectValid = Subject.GetType().GetProperty("Value");
                        string strFullSubjectValid = Convert.ToString(propertySubjectValid.GetValue(Subject));
                        if (alphaNumberValid.IsMatch(strFullSubjectValid))
                        {
                            var property = Subject.GetType().GetProperty("Value");
                            string strSubject = Convert.ToString(property.GetValue(Subject));
                            data.Subject = data.Subject.Replace("[Subject]", strSubject);
                            data.Body = data.Body.Replace("[Subject]", strSubject);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct Subject!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }




                    var EventName = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Event Name"));
                    if (EventName != null)
                    {
                        var property = EventName.GetType().GetProperty("Value");
                        string strEventName = Convert.ToString(property.GetValue(EventName));
                        data.Subject = data.Subject.Replace("[Event Name]", strEventName);
                        data.Body = data.Body.Replace("[Event Name]", strEventName);
                    }

                    var RefrenceNo = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Refrence No"));
                    if (RefrenceNo != null)
                    {
                        var property = RefrenceNo.GetType().GetProperty("Value");
                        string strRefrenceNo = Convert.ToString(property.GetValue(RefrenceNo));
                        data.Subject = data.Subject.Replace("[Refrence No]", strRefrenceNo);
                        data.Body = data.Body.Replace("[Refrence No]", strRefrenceNo);
                    }


                    var Email = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Email"));
                    if (Email != null)
                    {
                        var propertyEmailValid = Email.GetType().GetProperty("Value");
                        string strFullEmailValid = Convert.ToString(propertyEmailValid.GetValue(Email));

                        if (emailRegex.IsMatch(strFullEmailValid))
                        {
                            var property = FullName.GetType().GetProperty("Value");
                            string strEmail = Convert.ToString(property.GetValue(Email));
                            data.Subject = data.Subject.Replace("[Email]", strEmail);
                            data.Body = data.Body.Replace("[Email]", strEmail);
                            data.To = data.To + ";" + strEmail;

                        }
                        else
                        {
                            _errorMessage = "Please enter correct Email!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }



                    var Message = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Message"));
                    if (Message != null)
                    {
                        var propertyMessageValid = Message.GetType().GetProperty("Value");
                        string strFullMessageValid = Convert.ToString(propertyMessageValid.GetValue(Message));

                        if (alphaNumberValid.IsMatch(strFullMessageValid))
                        {
                            var property = Message.GetType().GetProperty("Value");
                            string strMessage = Convert.ToString(property.GetValue(Message));
                            data.Subject = data.Subject.Replace("[Message]", strMessage);
                            data.Body = data.Body.Replace("[Message]", strMessage);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct Message!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }

                    }



                    var MoblieNo = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Mobile No"));
                    if (MoblieNo != null)
                    {
                        var propertyMobileValid = MoblieNo.GetType().GetProperty("Value");
                        string strFullMobileValid = Convert.ToString(propertyMobileValid.GetValue(MoblieNo));

                        if (NumRegex.IsMatch(strFullMobileValid))
                        {
                            var property = MoblieNo.GetType().GetProperty("Value");
                            string strMoblieNo = Convert.ToString(property.GetValue(MoblieNo));
                            data.Subject = data.Subject.Replace("[Mobile No]", strMoblieNo);
                            data.Body = data.Body.Replace("[Mobile No]", strMoblieNo);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct Mobile Number!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }

                    }



                    var EnquiryType = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Enquiry Type"));
                    if (EnquiryType != null)
                    {
                        var OTPDataSource = Sitecore.Data.ID.Parse(Template.EnquiryDropdownItemId);
                        Item sourceItem = Sitecore.Context.Database.GetItem(OTPDataSource);
                        DropDownListViewModel dropdownField = EnquiryType as DropDownListViewModel;
                        var dropdownEnquiryList = dropdownField.Items.ToList();
                        string strEnquiryType = dropdownField.Value.FirstOrDefault();
                        foreach (var item in sourceItem.GetChildren().ToList())
                        {
                            var value = item.Fields["Value"].ToString();
                            if (strEnquiryType == value)
                            {
                                if (EnquiryType is DropDownListViewModel)
                                {
                                    data.Subject = data.Subject.Replace("[Enquiry Type]", strEnquiryType);
                                    data.Body = data.Body.Replace("[Enquiry Type]", strEnquiryType);
                                }
                                flag++;
                                break;
                            }

                        }
                        if (flag == 0)
                        {
                            _errorMessage = "Please choose correct Enquiry Type!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        

                    }



                    var objFileUrl = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("FileUrl"));

                    var objfileuploadfield = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("file-upload-field"));
                    if (objfileuploadfield != null)
                    {
                        FileUploadViewModel fileUploadViewModel = objfileuploadfield as FileUploadViewModel;
                        var FileUploadValue = fileUploadViewModel.Value.FirstOrDefault();
                        string str = FileUploadValue.FileName;
                        int occurance = str.Count(f => (f == ch));
                        if (FileUploadValue.FileId.ToString() != "{00000000-0000-0000-0000-000000000000}" && occurance == 1)
                        {

                            if (objfileuploadfield is FileUploadViewModel)
                            {
                                FileUploadViewModel files = objfileuploadfield as FileUploadViewModel;
                                var file = files.Files.FirstOrDefault();
                                if (file != null)
                                {
                                    var FileUploadmime = file.ContentType;
                                    if (!contenttypeExtenstion.Contains(FileUploadmime.ToLower()))
                                    {
                                        _errorMessage = "Please upload correct file!";
                                        formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                                        formSubmitContext.Abort();
                                        return false;
                                    }
                                    else
                                    {
                                        var model = (FileUploadViewModel)objfileuploadfield;
                                        if (model.Value.Count > 0)
                                        {
                                            string Url = Sitecore.Configuration.Settings.GetSetting("AzureBlobStorageFileStorageProvider.Url");

                                            int count = 1;
                                            string id = string.Empty;
                                            StringBuilder sb = new StringBuilder();
                                            StringBuilder sburls = new StringBuilder();
                                            foreach (var objmodel in model.Value)
                                            {

                                                if (objmodel != null)
                                                {

                                                    if (data.Body.Contains("[file-upload-field]"))
                                                    {
                                                        id = Convert.ToString((objmodel?.FileId.ToString()));
                                                        var url = string.Concat(new string[] { Url, id.ToString() });

                                                        sb.AppendLine("<a href=\"" + url + "\" target=\"_blank\">" + objmodel.FileName + "</a>" + "<br />");
                                                        if (count == 1)
                                                        {
                                                            sburls.AppendFormat("{0}:{1}", objmodel.FileName, url);
                                                        }
                                                        else
                                                        {
                                                            sburls.AppendFormat(", {0}:{1}", objmodel.FileName, url);
                                                        }
                                                        count = count + 1;
                                                    }

                                                }
                                            }

                                            data.Body = data.Body.Replace("[file-upload-field]", Convert.ToString(sb));
                                            if (objFileUrl != null)
                                            {
                                                AdaniWelspunEntityDataContext db = new AdaniWelspunEntityDataContext();
                                                var saveitem = db.FieldDatas.Where(c => c.Value.Contains(id)).FirstOrDefault();

                                                if (saveitem != null)
                                                {
                                                    var formentryid = saveitem.FormEntryId;
                                                    var result = db.FieldDatas.Where(c => c.FieldDefinitionId == Guid.Parse("3B1F7CBC-3E6A-4CF3-8DB7-D5867BFCAAC1") && c.FormEntryId == formentryid).FirstOrDefault();
                                                    if (result != null)
                                                    {
                                                        result.Value = Convert.ToString(sburls);
                                                        db.SubmitChanges();
                                                    }
                                                }
                                            }


                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            _errorMessage = "Please upload correct file!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    
                    ResponseModel responseModel = new ResponseModel();
                    responseModel.To = data.To;
                    responseModel.From = data.From;
                    responseModel.Cc = data.Cc;
                    responseModel.Bcc = data.Bcc;
                    responseModel.Subject = data.Subject;
                    responseModel.Body = data.Body;


                    var response = WebApiHelper.SentEmail(responseModel);


                    if (response != null && response.isSucess)
                    {
                        return true;
                    }
                    else
                    {
                        _errorMessage = "Email not sent!!";
                        formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                        formSubmitContext.Abort();
                        return false;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Sent Email Error:" + ex.Message, ex, this);
                return false;
            }
        }

    }
}