using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models.Forms;
using Project.Mining.Website.Templates;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Mining.Website.Services.Forms
{
    public class MiningFormsService: IMiningFormsService
    {
        public MiningForm GetSubscribeForm(Rendering rendering) 
        {
            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetSubscribeForm : Datasource is empty", this);
                return null;
            }
            try 
            {
                var subscribeUsForm = new MiningForm();

                subscribeUsForm.FormFields = new List<FormFieldSection>();
                subscribeUsForm.AntiforgeryToken = Utils.GetAntiForgeryToken();

                var newsLetterHeadingField = MiningFormTemplate.SubscribeUsFormTemplate.NewsLetter;
                var textHeadingField = MiningFormTemplate.SubscribeUsFormTemplate.Text;
                var emailField = MiningFormTemplate.SubscribeUsFormTemplate.Email;
                var googleV3CaptchaField = MiningFormTemplate.SubscribeUsFormTemplate.GoogleV3Captcha;
                var sectionItem = datasource.Children.FirstOrDefault()?.Children.FirstOrDefault();
                if (sectionItem != null)
                {
                    var subscribeUsErrorMessageItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.SubscribeUsErrorMessagesItem);

                    foreach (Item field in sectionItem.Children)
                    {
                        if (field.ID == newsLetterHeadingField || field.ID == textHeadingField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Text = field.Fields[MiningFormTemplate.TextTemplate.Text].Value;
                            
                            ReferenceField htmlTagDropList = field.Fields[MiningFormTemplate.TextTemplate.HtmlTag];
                            if (htmlTagDropList != null)
                            {
                                formFieldSection.HtmlTag = htmlTagDropList.Value;
                            }

                            formFieldSection.FieldName = field.Name;
                            formFieldSection.FieldType = "label";
                            formFieldSection.Required = true;
                            subscribeUsForm.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == emailField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[MiningFormTemplate.FormFieldsSection.PlaceholderText].Value;
                            formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, MiningFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[MiningFormTemplate.FormFieldsSection.DefaultValue].Value;
                            
                            ErrorMessage errorMessage = new ErrorMessage();
                            errorMessage.MaxLengthErrorMessage = subscribeUsErrorMessageItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MaxLengthErrorMessageEmail].Value;
                            errorMessage.MinLengthErrorMessage = subscribeUsErrorMessageItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MinLengthErrorMessageEmail].Value;
                            errorMessage.RegexErrorMessage = subscribeUsErrorMessageItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RegexErrorMessageEmail].Value;
                            errorMessage.RequiredFieldErrorMessage = subscribeUsErrorMessageItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RequiredFieldErrorMessageEmail].Value;
                            formFieldSection.ErrorMessages = errorMessage;
                            
                            subscribeUsForm.FormFields.Add(formFieldSection);
                        }

                        {
                            SubmitButtonText submitButtonText = new SubmitButtonText();
                            submitButtonText.ButtonText = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            subscribeUsForm.SubmitButton = submitButtonText;
                        }
                        if (field != null)
                        {
                            var progressDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.ProgressMessagesItem);
                            var progressData = new FormProgressData();
                            progressData.Heading = progressDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            progressData.Description = progressDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            subscribeUsForm.ProgressData = progressData;
                           
                            var formFailDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.FormSubmissionFailedMessagesItem);
                            var formfailedData = new FormFailedData();
                            formfailedData.Heading = formFailDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            formfailedData.Description = formFailDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            subscribeUsForm.FormFailData = formfailedData;
                            
                            var thankYouDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.ThankyouMessagesItem);
                            var thankyouData = new FormThankyouData();
                            thankyouData.Heading = thankYouDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            thankyouData.Description = thankYouDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            subscribeUsForm.ThankYouData = thankyouData;

                            var reCaptchaFieldfieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.RecaptchaErrorMessagesItem);
                            ReCaptchaField reCaptchaField = new ReCaptchaField();
                            reCaptchaField.FieldType = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.Title].Value;
                            reCaptchaField.FieldName = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            reCaptchaField.FieldID = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.SubHeading].Value;

                            ErrorMessageReCaptchaField errorMessageReCaptchaField = new ErrorMessageReCaptchaField();

                            errorMessageReCaptchaField.RequiredFieldErrorMessage = reCaptchaFieldfieldItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RecaptchaErrorMessageFieldId].Value;
                            reCaptchaField.ErrorMessages = errorMessageReCaptchaField;
                            reCaptchaField.Required = Utils.GetBoleanValue(reCaptchaFieldfieldItem, MiningFormTemplate.MiningFormErrorMessages.IsCaptchaRequiredFieldId);
                            subscribeUsForm.ReCaptchaField = reCaptchaField;
                        }
                    }
                    return subscribeUsForm;
                }
                return null;
            }
            catch(Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in GetSubscribeForm, exception:"+ ex, ex);
                throw ex;
            }
        }
        public MiningForm GetRequestACallForm(Rendering rendering)
        {
            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetRequestACallForm : Datasource is empty", this);
                return null;
            }
            try
            {
                var requestACallForm = new MiningForm();

                requestACallForm.FormFields = new List<FormFieldSection>();
                requestACallForm.AntiforgeryToken = Utils.GetAntiForgeryToken();

                var requestACallHeading = MiningFormTemplate.RequestACallFormTemplate.RequestACallHeading;
                var nameField = MiningFormTemplate.RequestACallFormTemplate.Name;
                var emailField = MiningFormTemplate.RequestACallFormTemplate.Email;
                var mobileField = MiningFormTemplate.RequestACallFormTemplate.Mobile;
                var messageField = MiningFormTemplate.RequestACallFormTemplate.Message;
                var selectSolutionTypeDropdownField = MiningFormTemplate.RequestACallFormTemplate.SelectSolutionType;
                var googleV3CaptchaField = MiningFormTemplate.RequestACallFormTemplate.GoogleV3Captcha;
                var submitField = MiningFormTemplate.RequestACallFormTemplate.SubmitButton;


                var sectionItem = datasource.Children.FirstOrDefault()?.Children.FirstOrDefault();
                if (sectionItem != null)
                {
                    var requestACallFormErrorMessagesItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.RequestACallFormErrorMessagesItem);

                    foreach (Item field in sectionItem.Children)
                    {
                        if (field.ID == requestACallHeading)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Text = field.Fields[MiningFormTemplate.TextTemplate.Text].Value;

                            ReferenceField htmlTagDropList = field.Fields[MiningFormTemplate.TextTemplate.HtmlTag];
                            if (htmlTagDropList != null)
                            {
                                formFieldSection.HtmlTag = htmlTagDropList.Value;
                            }

                            formFieldSection.FieldName = field.Name;
                            formFieldSection.FieldType = "label";
                            formFieldSection.Required = true;
                            requestACallForm.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == nameField || field.ID == emailField || field.ID == mobileField || field.ID == messageField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[MiningFormTemplate.FormFieldsSection.PlaceholderText].Value;
                            formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, MiningFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[MiningFormTemplate.FormFieldsSection.DefaultValue].Value;
                            
                            if (field.ID == nameField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RequiredFieldErrorMessageName].Value;
                                errorMessage.MaxLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MaxLengthErrorMessageName].Value;
                                errorMessage.MinLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MinLengthErrorMessageName].Value;
                                errorMessage.RegexErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RegexErrorMessageName].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == emailField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RequiredFieldErrorMessageEmail].Value;
                                errorMessage.MaxLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MaxLengthErrorMessageEmail].Value;
                                errorMessage.MinLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MinLengthErrorMessageEmail].Value;
                                errorMessage.RegexErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RegexErrorMessageEmail].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == mobileField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RequiredFieldErrorMessageMobile].Value;
                                errorMessage.MaxLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MaxLengthErrorMessageMobile].Value;
                                errorMessage.MinLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MinLengthErrorMessageMobile].Value;
                                errorMessage.RegexErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RegexErrorMessageMobile].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == messageField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RequiredFieldErrorMessageMessage].Value;
                                errorMessage.MaxLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MaxLengthErrorMessageMessage].Value;
                                errorMessage.MinLengthErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.MinLengthErrorMessageMessage].Value;
                                errorMessage.RegexErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RegexErrorMessageMessage].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            requestACallForm.FormFields.Add(formFieldSection);
                        }

                        if (field.ID == selectSolutionTypeDropdownField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[MiningFormTemplate.FormFieldsSection.ValueProviderParameters].Value;
                            if (!string.IsNullOrEmpty(field.Fields[MiningFormTemplate.FormFieldsSection.minRequiredLength].Value))
                            {
                                formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            }
                            if (!string.IsNullOrEmpty(field.Fields[MiningFormTemplate.FormFieldsSection.maxAllowedLength].Value))
                            {
                                formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            }
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, MiningFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[MiningFormTemplate.FormFieldsSection.DefaultSelection].Value;
                            List<FieldOption> FieldOptionlist = new List<FieldOption>();
                            if (field.ID == selectSolutionTypeDropdownField)
                            {
                                var selectSolutionTypeId = 0;
                                var selectSolutionTypeField = field.Children.FirstOrDefault()?.Children.FirstOrDefault();
                                foreach (Item position in selectSolutionTypeField.Children)
                                {
                                    if (selectSolutionTypeId != 0)
                                    {
                                        FieldOption fieldOption = new FieldOption();
                                        fieldOption.Label = position.Fields[MiningFormTemplate.FormFieldsSection.fieldOptionslabel].Value;
                                        fieldOption.Id = selectSolutionTypeId.ToString();
                                        FieldOptionlist.Add(fieldOption);
                                    }
                                    selectSolutionTypeId++;
                                }
                                formFieldSection.FieldOptions = FieldOptionlist;
                            }
                            
                            ErrorMessage errorMessage = new ErrorMessage();
                            errorMessage.RequiredFieldErrorMessage = requestACallFormErrorMessagesItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RequiredFieldErrorMessageSelectSolutionType].Value;
                            formFieldSection.ErrorMessages = errorMessage;
                            requestACallForm.FormFields.Add(formFieldSection);
                        }

                        if (field.ID == submitField)
                        {
                            SubmitButtonText submitButtonText = new SubmitButtonText();
                            submitButtonText.ButtonText = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            requestACallForm.SubmitButton = submitButtonText;
                        }
                        if (field != null)
                        {
                            var progressDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.ProgressMessagesItem);
                            var progressData = new FormProgressData();
                            progressData.Heading = progressDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            progressData.Description = progressDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            requestACallForm.ProgressData = progressData;

                            var formFailDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.FormSubmissionFailedMessagesItem);
                            var formfailedData = new FormFailedData();
                            formfailedData.Heading = formFailDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            formfailedData.Description = formFailDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            requestACallForm.FormFailData = formfailedData;

                            var thankYouDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.ThankyouMessagesItem);
                            var thankyouData = new FormThankyouData();
                            thankyouData.Heading = thankYouDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            thankyouData.Description = thankYouDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            requestACallForm.ThankYouData = thankyouData;

                            var reCaptchaFieldfieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.RecaptchaErrorMessagesItem);
                            ReCaptchaField reCaptchaField = new ReCaptchaField();
                            reCaptchaField.FieldType = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.Title].Value;
                            reCaptchaField.FieldName = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            reCaptchaField.FieldID = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.SubHeading].Value;

                            ErrorMessageReCaptchaField errorMessageReCaptchaField = new ErrorMessageReCaptchaField();

                            errorMessageReCaptchaField.RequiredFieldErrorMessage = reCaptchaFieldfieldItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RecaptchaErrorMessageFieldId].Value;
                            reCaptchaField.ErrorMessages = errorMessageReCaptchaField;
                            reCaptchaField.Required = Utils.GetBoleanValue(reCaptchaFieldfieldItem, MiningFormTemplate.MiningFormErrorMessages.IsCaptchaRequiredFieldId);
                            requestACallForm.ReCaptchaField = reCaptchaField;
                        }
                    }
                    return requestACallForm;
                }
                return null;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in GetRequestACallForm, exception:" + ex, ex);
                throw ex;
            }
        }

        public MiningForm GetContactForm(Rendering rendering)
        {
            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetContactForm : Datasource is empty", this);
                return null;
            }
            try
            {
                var ContactForm = new MiningForm();

                ContactForm.FormFields = new List<FormFieldSection>();
                ContactForm.AntiforgeryToken = Utils.GetAntiForgeryToken();

                var HelpText = MiningFormTemplate.ContactFormTemplate.Helptext;
                var FeedbackText = MiningFormTemplate.ContactFormTemplate.Feedbacktext;
                var firstNameField = MiningFormTemplate.ContactFormTemplate.FirstName;
                var lastNameField = MiningFormTemplate.ContactFormTemplate.LastName;
                var emailField = MiningFormTemplate.ContactFormTemplate.Email;
                var mobileField = MiningFormTemplate.ContactFormTemplate.Mobile;
                var messageField = MiningFormTemplate.ContactFormTemplate.Message;
                var selecthelpTypeDropdownField = MiningFormTemplate.ContactFormTemplate.SelectHelpType;
                var googleV3CaptchaField = MiningFormTemplate.ContactFormTemplate.GoogleV3Captcha;
                var submitField = MiningFormTemplate.ContactFormTemplate.SubmitButton;


                var sectionItem = datasource.Children.FirstOrDefault()?.Children.FirstOrDefault();
                if (sectionItem != null)
                {
                    var ContactFormErrorMessagesItem = GetSitecoreItem(MiningFormTemplate.MiningContactFormErrorMessages.ContactUsFormErrorMessagesItem);

                    foreach (Item field in sectionItem.Children)
                    {
                        if (field.ID == HelpText || field.ID == FeedbackText)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Text = field.Fields[MiningFormTemplate.TextTemplate.Text].Value;

                            ReferenceField htmlTagDropList = field.Fields[MiningFormTemplate.TextTemplate.HtmlTag];
                            if (htmlTagDropList != null)
                            {
                                formFieldSection.HtmlTag = htmlTagDropList.Value;
                            }

                            formFieldSection.FieldName = field.Name;
                            formFieldSection.FieldType = "label";
                            formFieldSection.Required = true;
                            ContactForm.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == firstNameField || field.ID == lastNameField || field.ID == emailField || field.ID == mobileField || field.ID == messageField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[MiningFormTemplate.FormFieldsSection.PlaceholderText].Value;
                            formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, MiningFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[MiningFormTemplate.FormFieldsSection.DefaultValue].Value;

                            if (field.ID == firstNameField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RequiredFieldErrorMessageFirstName].Value;
                                errorMessage.MaxLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RegexErrorMessage].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            if (field.ID == lastNameField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RequiredFieldErrorMessageLastName].Value;
                                errorMessage.MaxLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RegexErrorMessage].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == emailField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RequiredFieldErrorMessageEmail].Value;
                                errorMessage.MaxLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RegexErrorMessage].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == mobileField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RequiredFieldErrorMessageMobile].Value;
                                errorMessage.MaxLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RegexErrorMessage].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == messageField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RequiredFieldErrorMessageField].Value;
                                errorMessage.MaxLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RegexErrorMessage].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            ContactForm.FormFields.Add(formFieldSection);
                        }

                        if (field.ID == selecthelpTypeDropdownField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[MiningFormTemplate.FormFieldsSection.ValueProviderParameters].Value;
                            if (!string.IsNullOrEmpty(field.Fields[MiningFormTemplate.FormFieldsSection.minRequiredLength].Value))
                            {
                                formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            }
                            if (!string.IsNullOrEmpty(field.Fields[MiningFormTemplate.FormFieldsSection.maxAllowedLength].Value))
                            {
                                formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[MiningFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            }
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, MiningFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[MiningFormTemplate.FormFieldsSection.DefaultSelection].Value;
                            List<FieldOption> FieldOptionlist = new List<FieldOption>();
                            if (field.ID == selecthelpTypeDropdownField)
                            {
                                var selectHelpTypeId = 0;
                                var selectHelpTypeField = field.Children.FirstOrDefault()?.Children.FirstOrDefault();
                                foreach (Item position in selectHelpTypeField.Children)
                                {
                                    if (selectHelpTypeId != 0)
                                    {
                                        FieldOption fieldOption = new FieldOption();
                                        fieldOption.Label = position.Fields[MiningFormTemplate.FormFieldsSection.fieldOptionslabel].Value;
                                        fieldOption.Id = selectHelpTypeId.ToString();
                                        FieldOptionlist.Add(fieldOption);
                                    }
                                    selectHelpTypeId++;
                                }
                                formFieldSection.FieldOptions = FieldOptionlist;
                            }

                            ErrorMessage errorMessage = new ErrorMessage();
                            errorMessage.RequiredFieldErrorMessage = ContactFormErrorMessagesItem.Fields[MiningFormTemplate.MiningContactFormErrorMessages.RequiredFieldErrorSelectHelpDropdown].Value;
                            formFieldSection.ErrorMessages = errorMessage;
                            ContactForm.FormFields.Add(formFieldSection);
                        }

                        if (field.ID == submitField)
                        {
                            SubmitButtonText submitButtonText = new SubmitButtonText();
                            submitButtonText.ButtonText = field.Fields[MiningFormTemplate.FormFieldsSection.Title].Value;
                            ContactForm.SubmitButton = submitButtonText;
                        }
                        if (field != null)
                        {
                            var progressDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningContactFormErrorMessages.ProgressMessagesItem);
                            var progressData = new FormProgressData();
                            progressData.Heading = progressDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            progressData.Description = progressDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            ContactForm.ProgressData = progressData;

                            var formFailDatafieldItem = GetSitecoreItem(MiningFormTemplate.MiningContactFormErrorMessages.FormSubmissionFailedMessagesItem);
                            var formfailedData = new FormFailedData();
                            formfailedData.Heading = formFailDatafieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            formfailedData.Description = formFailDatafieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            ContactForm.FormFailData = formfailedData;

                            var thankYouModelFieldItem = GetSitecoreItem(MiningFormTemplate.MiningContactFormErrorMessages.ThankyouModelMessagesItem);
                            var thankyouData = new FormThankyouData();
                            thankyouData.Heading = thankYouModelFieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            thankyouData.Description = thankYouModelFieldItem.Fields[BaseTemplates.Fields.Description].Value;
                            thankyouData.CtaLink = Utils.GetLinkURL(thankYouModelFieldItem.Fields[BaseTemplates.Fields.CtaLink]);
                            thankyouData.Class = thankYouModelFieldItem.Fields[BaseTemplates.Fields.Class].Value;
                            thankyouData.CtaText = thankYouModelFieldItem.Fields[BaseTemplates.Fields.CtaText].Value;
                            thankyouData.Image = Utils.GetImageURLByFieldId(thankYouModelFieldItem, BaseTemplates.Fields.Image);
                            thankyouData.MobileImage = Utils.GetImageURLByFieldId(thankYouModelFieldItem, BaseTemplates.Fields.MobileImage);
                            thankyouData.TabletImage = Utils.GetImageURLByFieldId(thankYouModelFieldItem, BaseTemplates.Fields.TabletImage);
                            thankyouData.ImageAlt = thankYouModelFieldItem.Fields[BaseTemplates.Fields.ImageAltText].Value;

                            ContactForm.ThankYouData = thankyouData;

                            var reCaptchaFieldfieldItem = GetSitecoreItem(MiningFormTemplate.MiningFormErrorMessages.RecaptchaErrorMessagesItem);
                            ReCaptchaField reCaptchaField = new ReCaptchaField();
                            reCaptchaField.FieldType = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.Title].Value;
                            reCaptchaField.FieldName = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.Heading].Value;
                            reCaptchaField.FieldID = reCaptchaFieldfieldItem.Fields[BaseTemplates.Fields.SubHeading].Value;

                            ErrorMessageReCaptchaField errorMessageReCaptchaField = new ErrorMessageReCaptchaField();

                            errorMessageReCaptchaField.RequiredFieldErrorMessage = reCaptchaFieldfieldItem.Fields[MiningFormTemplate.MiningFormErrorMessages.RecaptchaErrorMessageFieldId].Value;
                            reCaptchaField.ErrorMessages = errorMessageReCaptchaField;
                            reCaptchaField.Required = Utils.GetBoleanValue(reCaptchaFieldfieldItem, MiningFormTemplate.MiningFormErrorMessages.IsCaptchaRequiredFieldId);
                            ContactForm.ReCaptchaField = reCaptchaField;
                        }
                    }
                    return ContactForm;
                }
                return null;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in GetContactForm, exception:" + ex, ex);
                throw ex;
            }
        }


        public Item GetSitecoreItem(ID id)
        {
            return Sitecore.Context.Database.GetItem(id);
        }
    }
}