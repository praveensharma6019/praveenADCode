using Sitecore.Data.Templates;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Mvc.Models.SubmitActions;
using Sitecore.ExperienceForms.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using Sitecore.Data.Items;
using Project.Mining.Website.Models.Forms;
using Sitecore.Foundation.Email.Utils;
using Sitecore.Foundation.Email.Model;
using Project.Mining.Website.Templates;
using Sitecore.ExperienceForms.Mvc.Pipelines.ValidateSubmit;
using Sitecore.Helper;
using Sitecore.Mvc.Extensions;
using System.IO;
using Microsoft.Ajax.Utilities;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web.UI;

namespace Project.Mining.Website.SitecoreForms
{
    public class FormSubmission : Sitecore.ExperienceForms.Mvc.Processing.SubmitActions.SendEmail
    {
        private string _errorMessage;
        private static readonly Regex alphaNumber = new Regex("^[a-zA-Z ]*$");
        private static readonly Regex alphaNumberValid = new Regex("^[a-zA-Z ]+$");
        private static readonly Regex emailRegex = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");
        private static readonly Regex NumRegex = new Regex(@"^(\+\d{1,3}[- ]?)?\d{10}$");
        private static readonly Regex MultilineRegex = new Regex(@"^[\w\s.,@:!?-]+$");
        char ch = '.';
        int flag = 0;
        string[] contenttypeExtenstion = new string[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword" };

        public FormSubmission(ISubmitActionData submitActionData) : base(submitActionData)
        {

        }
        protected override bool Execute(SendEmailData data, FormSubmitContext formSubmitContext)
        {
            try
            {
                Sitecore.Data.Database DBContext = Sitecore.Configuration.Factory.GetDatabase("web");
                Item mailBodyContext = null;
                Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
                string dropdownValue = string.Empty;
                string url = string.Empty;

                if (!formSubmitContext.HasErrors)
                {
                    #region creating mail body
                    if (formSubmitContext.FormId == MiningFormTemplate.Form.RequestACall)
                    {
                        mailBodyContext = DBContext.GetItem(MiningFormTemplate.Form.RequestACallMailBody);
                    }
                    if (formSubmitContext.FormId == MiningFormTemplate.Form.Career)
                    {
                        mailBodyContext = DBContext.GetItem(MiningFormTemplate.Form.CareerMailBody);
                    }
                    if (formSubmitContext.FormId == MiningFormTemplate.Form.Contact)
                    {
                        mailBodyContext = DBContext.GetItem(MiningFormTemplate.Form.ContactMailBody);
                    }
                    if (formSubmitContext.FormId == MiningFormTemplate.Form.Enquiry)
                    {
                        mailBodyContext = DBContext.GetItem(MiningFormTemplate.Form.EnquiryMailBody);
                    }
                    if (formSubmitContext.FormId == MiningFormTemplate.Form.Brochure)
                    {
                        mailBodyContext = DBContext.GetItem(MiningFormTemplate.Form.BrochureMailBody);
                    }
                    if (formSubmitContext.FormId == MiningFormTemplate.Form.Subscribe)
                    {
                        mailBodyContext = DBContext.GetItem(MiningFormTemplate.Form.SubscribeMailBody);
                    }
                    if (mailBodyContext == null)
                    {
                        _errorMessage = "Email body context not found";
                        formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                        formSubmitContext.Abort();
                        return false;
                    }    
                    data.Body = mailBodyContext.Fields["Description"].Value;
                    #endregion

                    #region FirstName
                    var FirstName = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("First Name"));
                    if (FirstName != null)
                    {
                        var propertyNameValid = FirstName.GetType().GetProperty("Value");
                        string strFirstNameValid = Convert.ToString(propertyNameValid.GetValue(FirstName));

                        if (strFirstNameValid.Length == 0)
                        {
                            _errorMessage = "Please enter your first name";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (alphaNumber.IsMatch(strFirstNameValid))
                        {
                            var property = FirstName.GetType().GetProperty("Value");
                            var strFirstName = Convert.ToString(property.GetValue(FirstName));
                            data.Subject = data.Subject.Replace("[First Name]", strFirstName);
                            data.Body = data.Body.Replace("[First Name]", strFirstName);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct Name!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    else
                    {
                        data.Subject = data.Subject.Replace("[First Name]", "");
                        data.Body = data.Body.Replace("[First Name]", "");
                    }
                    #endregion

                    #region LastName
                    var LastName = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Last Name"));
                    if (LastName != null)
                    {
                        var propertyNameValid = LastName.GetType().GetProperty("Value");
                        string strLastNameValid = Convert.ToString(propertyNameValid.GetValue(LastName));

                        if (strLastNameValid.Length == 0)
                        {
                            _errorMessage = "Please enter your last name";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (alphaNumber.IsMatch(strLastNameValid))
                        {
                            var property = LastName.GetType().GetProperty("Value");
                            var strLastName = Convert.ToString(property.GetValue(LastName));
                            data.Subject = data.Subject.Replace("[Last Name]", strLastName);
                            data.Body = data.Body.Replace("[Last Name]", strLastName);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct last name!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    else
                    {
                        data.Subject = data.Subject.Replace("[Last Name]", "");
                        data.Body = data.Body.Replace("[Last Name]", "");
                    }
                    #endregion

                    #region Email
                    var Email = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Email"));
                    if (Email != null)
                    {
                        var propertyEmailValid = Email.GetType().GetProperty("Value");
                        string strFullEmailValid = Convert.ToString(propertyEmailValid.GetValue(Email));
                        if (strFullEmailValid.Length == 0)
                        {
                            _errorMessage = "Please enter your email address";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (emailRegex.IsMatch(strFullEmailValid))
                        {
                            var property = Email.GetType().GetProperty("Value");
                            string strEmail = Convert.ToString(property.GetValue(Email));
                            data.Subject = data.Subject.Replace("[Email]", strEmail);
                            data.Body = data.Body.Replace("[Email]", strEmail);
                            data.To = data.To+";";
                        }
                        else
                        {
                            _errorMessage = "Please enter correct email address";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    #endregion

                    #region MobileNumber
                    var MoblieNo = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Mobile No"));
                    if (MoblieNo != null)
                    {
                        var property = MoblieNo.GetType().GetProperty("Value");
                        string strMoblieNo = Convert.ToString(property.GetValue(MoblieNo));
                        if (strMoblieNo.Length == 0)
                        {
                            data.Subject = data.Subject.Replace("[Mobile No]", "");
                            data.Body = data.Body.Replace("[Mobile No]", "");
                        }
                        else
                        {

                            strMoblieNo = "+91 " + strMoblieNo;
                            data.Subject = data.Subject.Replace("[Mobile No]", strMoblieNo);
                            data.Body = data.Body.Replace("[Mobile No]", strMoblieNo);
                        }                        
                    }                   
                    #endregion

                    #region SolutionTypeDropdown
                    var SolutionTypeDropDown = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("SolutionType"));
                    if (SolutionTypeDropDown != null)
                    {
                        var dropdownContext = Sitecore.Data.ID.Parse(MiningFormTemplate.RequestACallFormTemplate.SolutionTypeDropDownList);
                        Item sourceItem = Sitecore.Context.Database.GetItem(dropdownContext);
                        DropDownListViewModel dropdownField = SolutionTypeDropDown as DropDownListViewModel;
                        string strSolutionTypeDropDown = dropdownField.Value.FirstOrDefault();
                        dropdownValue = strSolutionTypeDropDown;
                        Item DefaultItemContext = sourceItem.GetChildren().ToList().First();
                        string DeafultValue = DefaultItemContext.Fields["Value"].Value.ToString();
                        foreach (var item in sourceItem.GetChildren().ToList())
                        {
                            var value = item.Fields["Value"].ToString();
                            if (strSolutionTypeDropDown == value)
                            {
                                if (strSolutionTypeDropDown.ToLower() == DeafultValue.ToLower())
                                {
                                    _errorMessage = "Please choose correct solution type!";
                                    formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                                    formSubmitContext.Abort();
                                    return false;
                                }

                                if (SolutionTypeDropDown is DropDownListViewModel)
                                {
                                    data.Subject = data.Subject.Replace("[SolutionType]", strSolutionTypeDropDown);
                                    data.Body = data.Body.Replace("[SolutionType]", strSolutionTypeDropDown);
                                }
                                flag++;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            _errorMessage = "Please choose correct solution type!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    #endregion

                    #region LookingForDropdown
                    var LookingForDropDown = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("IAmLookingFor"));
                    if (LookingForDropDown != null)
                    {
                        var dropdownContext = Sitecore.Data.ID.Parse(MiningFormTemplate.EnquiryFormTemplate.LookingForDropDownList);
                        Item sourceItem = Sitecore.Context.Database.GetItem(dropdownContext);
                        DropDownListViewModel dropdownField = LookingForDropDown as DropDownListViewModel;
                        string strLookingForDropDown = dropdownField.Value.FirstOrDefault();
                        dropdownValue = strLookingForDropDown;
                        Item DefaultItemContext = sourceItem.GetChildren().ToList().First();
                        string DeafultValue = DefaultItemContext.Fields["Value"].Value.ToString();
                        foreach (var item in sourceItem.GetChildren().ToList())
                        {
                            var value = item.Fields["Value"].ToString();
                            if (strLookingForDropDown == value)
                            {
                                if (strLookingForDropDown.ToLower() == DeafultValue.ToLower())
                                {
                                    _errorMessage = "Please choose correct solution type!";
                                    formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                                    formSubmitContext.Abort();
                                    return false;
                                }

                                if (LookingForDropDown is DropDownListViewModel)
                                {
                                    data.Subject = data.Subject.Replace("[IAmLookingFor]", strLookingForDropDown);
                                    data.Body = data.Body.Replace("[IAmLookingFor]", strLookingForDropDown);
                                }
                                flag++;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            _errorMessage = "Please choose correct solution type!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    #endregion

                    #region HelpTypeDropdown
                    var HelpTypeDropdown = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("HelpType-Dropdown"));
                    if (HelpTypeDropdown != null)
                    {
                        var dropdownContext = Sitecore.Data.ID.Parse(MiningFormTemplate.ContactFormTemplate.HelpTypeDropDownList);
                        Item sourceItem = Sitecore.Context.Database.GetItem(dropdownContext);
                        DropDownListViewModel dropdownField = HelpTypeDropdown as DropDownListViewModel;
                        string strHelpTypeDropDown = dropdownField.Value.FirstOrDefault();
                        dropdownValue = strHelpTypeDropDown;
                        Item DefaultItemContext = sourceItem.GetChildren().ToList().First();
                        string DeafultValue = DefaultItemContext.Fields["Value"].Value.ToString();
                        foreach (var item in sourceItem.GetChildren().ToList())
                        {
                            var value = item.Fields["Value"].ToString();
                            if (strHelpTypeDropDown == value)
                            {
                                if (strHelpTypeDropDown.ToLower() == DeafultValue.ToLower())
                                {
                                    _errorMessage = "Please choose correct solution type!";
                                    formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                                    formSubmitContext.Abort();
                                    return false;
                                }

                                if (HelpTypeDropdown is DropDownListViewModel)
                                {
                                    data.Subject = data.Subject.Replace("[HelpType-Dropdown]", strHelpTypeDropDown);
                                    data.Body = data.Body.Replace("[HelpType-Dropdown]", strHelpTypeDropDown);
                                }
                                flag++;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            _errorMessage = "Please choose correct help type!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    #endregion

                    #region Others
                    var Others = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Others"));
                    if (Others != null && dropdownValue.ToLower() == "Other".ToLower())
                    {

                        var propertyOthersValid = Others.GetType().GetProperty("Value");
                        string strOthersValid = Convert.ToString(propertyOthersValid.GetValue(Others));
                        if (strOthersValid.Length > 255)
                        {
                            _errorMessage = "Character length should be less than 255";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (MultilineRegex.IsMatch(strOthersValid))
                        {
                            var property = Others.GetType().GetProperty("Value");
                            string strOthers = Convert.ToString(property.GetValue(Others));
                            data.Subject = data.Subject.Replace("[Others]", strOthers);
                            data.Body = data.Body.Replace("[Others]", strOthers);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct input!";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    else
                    {
                        data.Subject = data.Subject.Replace("[Others]", "");
                        data.Body = data.Body.Replace("[Others]", "");
                    }
                    #endregion

                    #region Message
                    var Description = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Description"));
                    if (Description != null)
                    {
                        var propertyMessageValid = Description.GetType().GetProperty("Value");
                        string strFullMessageValid = Convert.ToString(propertyMessageValid.GetValue(Description));
                        if (strFullMessageValid.Length > 255)
                        {
                            _errorMessage = "Character length should be less than 255";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (MultilineRegex.IsMatch(strFullMessageValid))
                        {
                            var property = Description.GetType().GetProperty("Value");
                            string strMessage = Convert.ToString(property.GetValue(Description));
                            data.Subject = data.Subject.Replace("[Description]", strMessage);
                            data.Body = data.Body.Replace("[Description]", strMessage);

                        }
                        else
                        {
                            _errorMessage = "Please enter correct message";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    #endregion

                    #region CheckBox
                    var CheckBox = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("TCCheckbox"));
                    if (CheckBox != null)
                    {
                        var propertyCheckBoxValid = CheckBox.GetType().GetProperty("Value");
                        string strCheckBoxValid = Convert.ToString(propertyCheckBoxValid.GetValue(CheckBox));

                        if (strCheckBoxValid == "True")
                        {
                            var property = CheckBox.GetType().GetProperty("Value");
                            string strCheckBox = Convert.ToString(property.GetValue(CheckBox));
                            data.Subject = data.Subject.Replace("[TCCheckbox]", strCheckBox);
                            data.Body = data.Body.Replace("[TCCheckbox]", strCheckBox);

                        }
                        else
                        {
                            _errorMessage = "Please accpet the T&C and privacy policy";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    #endregion

                    #region Upload File

                    var objfileuploadfield = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("File Upload"));
                    if (objfileuploadfield != null)
                    {
                        FileUploadViewModel fileUploadViewModel = objfileuploadfield as FileUploadViewModel;
                        var FileUploadValue = fileUploadViewModel.Value.FirstOrDefault();
                        string str = FileUploadValue.FileName;
                        int occurance = str.Count(f => (f == ch));
                        if (FileUploadValue.FileId.ToString() == "{00000000-0000-0000-0000-000000000001}")
                        {
                            _errorMessage = "Please enter a file less than 3 MB";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (FileUploadValue.FileId.ToString() == "{00000000-0000-0000-0000-000000000000}")
                        {
                            _errorMessage = "Please select the correct PDF or DOC file";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (FileUploadValue.FileId.ToString() == "{00000000-0000-0000-0000-000000000002}")
                        {
                            _errorMessage = "Please upload the file with single extension";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                        if (FileUploadValue.FileId.ToString() != "{00000000-0000-0000-0000-000000000000}" && occurance == 1)
                        {

                            if (objfileuploadfield is FileUploadViewModel)
                            {
                                FileUploadViewModel files = objfileuploadfield as FileUploadViewModel;
                                var file = files.Files.FirstOrDefault();
                                if (file != null)
                                {
                                    string extension = Path.GetExtension(file.FileName);
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

                                                    id = Convert.ToString((objmodel?.FileId.ToString()));
                                                    //url = string.Concat(new string[] { Url, id + extension });
                                                    url = string.Concat(new string[] { Url, id});

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
                                            data.Body = data.Body.Replace("[File Upload]", Convert.ToString(sb));
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            _errorMessage = "Please upload file with single extension";
                            formSubmitContext.Errors.Add(new FormActionError { ErrorMessage = _errorMessage });
                            formSubmitContext.Abort();
                            return false;
                        }
                    }
                    #endregion

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