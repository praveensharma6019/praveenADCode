using Glass.Mapper.Sc.Web.Mvc;
using Project.AAHL.Website.Models.ContactUs;
using Project.AAHL.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils = Project.AAHL.Website.Helpers.Utils;

namespace Project.AAHL.Website.Services.Common
{
    public class ContactUsServices : IContactUsServices
    {

        private readonly IMvcContext _mvcContext;

        public ContactUsServices(IMvcContext mvcContext)
        {
            _mvcContext = mvcContext;
        }


        public Address GetAddress(Rendering rendering)
        {

            try
            {
                var datasource = Helpers.Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<Address>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public WriteToUsDetails GetWriteToUsDetails(Rendering rendering)
        {

            try
            {
                var datasource = Helpers.Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _mvcContext.GetDataSourceItem<WriteToUsDetails>();
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public WriteToUsForm GetWriteToUsForm(Rendering rendering)
        {
            WriteToUsForm writeToUsForm = new WriteToUsForm();
            writeToUsForm.FormFields = new List<FormFieldSection>();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetWriteToUsForm : Datasource is empty", this);
                return writeToUsForm;
            }

            try
            {
                var NameField = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.NameField;
                var MobileNumber = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.MobileNumber;
                var CountryCodeDropDown = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.CountryCodeDropDown;
                var Email = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.Email;
                var EnquiryType = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.EnquiryType;
                var Message = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.Message;
                var GoogleRecaptchaV3 = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.GoogleRecaptchaV3;
                var Submit = BaseTemplate.WriteToUsFormTemplate.TemplatesSection.Submit;

                var sectionItem = datasource.Children.FirstOrDefault()?.Children.FirstOrDefault();
                if (sectionItem != null)
                {
                    var ErrMsgfieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.ErrMsgfieldItem);
                    var ThankYouDatafieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.ThankYouDatafieldItem);
                    var ReCaptchaFieldfieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.ReCaptchaFieldfieldItem);
                    var FormGTMDatafieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.FormGTMDatafieldItem);
                    var ProgressDatafieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.ProgressDatafieldItem);
                    var FormFailDatafieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.FormFailDatafieldItem);
                    var ThemeDatafieldItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.ThemeDatafieldItem);
                    writeToUsForm.Theme = ThemeDatafieldItem.Fields[BaseTemplate.Fields.Theme].Value;
                    foreach (Item field in sectionItem.Children)
                    {
                        if (field.ID == MobileNumber || field.ID == NameField || field.ID == Email || field.ID == Message)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.PlaceholderText].Value;
                            formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Helpers.Utils.GetBoleanValue(field, BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.DefaultValue].Value;

                            if (field.ID == NameField)
                            {
                                ErrorMessages errorMessage = new ErrorMessages();
                                errorMessage.MaxLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RegexErrorMessage].Value;
                                errorMessage.RequiredFieldErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RequiredFieldErrorName].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            if (field.ID == MobileNumber)
                            {
                                ErrorMessages errorMessage = new ErrorMessages();
                                errorMessage.MaxLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RegexErrorMessage].Value;
                                errorMessage.RequiredFieldErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RequiredFieldErrorMobileNumber].Value;
                                formFieldSection.ErrorMessages = errorMessage;

                            }
                            if (field.ID == Email)
                            {
                                ErrorMessages errorMessage = new ErrorMessages();
                                errorMessage.MaxLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RegexErrorMessage].Value;
                                errorMessage.RequiredFieldErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RequiredFieldErrorEmail].Value;
                                formFieldSection.ErrorMessages = errorMessage;

                            }
                            if (field.ID == Message)
                            {
                                ErrorMessages errorMessage = new ErrorMessages();
                                errorMessage.MaxLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MaxLengthErrorMessage].Value;
                                errorMessage.MinLengthErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.MinLengthErrorMessage].Value;
                                errorMessage.RegexErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RegexErrorMessage].Value;
                                errorMessage.RequiredFieldErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RequiredFieldErrorMessageField].Value;
                                formFieldSection.ErrorMessages = errorMessage;

                            }
                            writeToUsForm.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == EnquiryType)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.ValueProviderParameters].Value;
                            if (!string.IsNullOrEmpty(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.minRequiredLength].Value))
                            {
                                formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            }
                            if (!string.IsNullOrEmpty(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.maxAllowedLength].Value))
                            {
                                formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            }
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Helpers.Utils.GetBoleanValue(field, BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.DefaultSelection].Value;
                            List<FieldOption> FieldOptionlist = new List<FieldOption>();
                            if (field.ID == EnquiryType)
                            {
                                var enquiryTypeid = 0;
                                var positionField = field.Children.FirstOrDefault()?.Children.FirstOrDefault();
                                foreach (Item position in positionField.Children)
                                {
                                    if (enquiryTypeid != 0)
                                    {
                                        FieldOption fieldOption = new FieldOption();
                                        fieldOption.Label = position.DisplayName;
                                        fieldOption.Id = enquiryTypeid.ToString();
                                        FieldOptionlist.Add(fieldOption);
                                    }
                                    enquiryTypeid++;
                                }
                                formFieldSection.FieldOptions = FieldOptionlist;
                            }
                            ErrorMessages errorMessage = new ErrorMessages();
                            if (field.ID == EnquiryType)
                            {
                                errorMessage.RequiredFieldErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RequiredFieldErrorEnquiryType].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            writeToUsForm.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == CountryCodeDropDown)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.ValueProviderParameters].Value;
                            if (!string.IsNullOrEmpty(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.minRequiredLength].Value))
                            {
                                formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.minRequiredLength].Value);
                            }
                            if (!string.IsNullOrEmpty(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.maxAllowedLength].Value))
                            {
                                formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.maxAllowedLength].Value);
                            }
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Helpers.Utils.GetBoleanValue(field, BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.DefaultSelection].Value;
                            List<CountryCodeOptions> countryCodeOptionList = new List<CountryCodeOptions>();
                            if (field.ID == CountryCodeDropDown)
                            {
                                var countryCodeFolderItem = Sitecore.Context.Database.GetItem(BaseTemplate.WriteToUsFormTemplate.TemplatesSection.CountryCodeDropdownFolderItem);
                                if (countryCodeFolderItem != null && countryCodeFolderItem.Children.Any())
                                {
                                    foreach (Item countryCodeDetails in countryCodeFolderItem.Children)
                                    {
                                        var countryCodeOption = new CountryCodeOptions();
                                        countryCodeOption.countryName = countryCodeDetails.Fields[BaseTemplate.WriteToUsFormTemplate.CountryCodeOptionDetails.CountryNameFieldId].Value;
                                        countryCodeOption.dialCode = countryCodeDetails.Fields[BaseTemplate.WriteToUsFormTemplate.CountryCodeOptionDetails.DialCodeFieldId].Value;
                                        countryCodeOption.isO3 = countryCodeDetails.Fields[BaseTemplate.WriteToUsFormTemplate.CountryCodeOptionDetails.IsO3FieldId].Value;
                                        countryCodeOption.isO2 = countryCodeDetails.Fields[BaseTemplate.WriteToUsFormTemplate.CountryCodeOptionDetails.IsO2FieldId].Value;
                                        countryCodeOption.countryFlagImage = Utils.GetImageURLbyField(countryCodeDetails.Fields[BaseTemplate.WriteToUsFormTemplate.CountryCodeOptionDetails.CountryFlagFieldId]);
                                        countryCodeOptionList.Add(countryCodeOption);
                                    }
                                    formFieldSection.CountryCodeOptions = countryCodeOptionList;
                                }
                            }
                            ErrorMessages errorMessage = new ErrorMessages();
                            if (field.ID == CountryCodeDropDown)
                            {
                                errorMessage.RequiredFieldErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RequiredFieldErrorCountryCode].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            writeToUsForm.FormFields.Add(formFieldSection);
                        }
                        if (field.ID == Submit)
                        {
                            SubmitButton submitButtonText = new SubmitButton();
                            submitButtonText.ButtonText = field.Fields[BaseTemplate.WriteToUsFormTemplate.FormFieldsSection.Title].Value;
                            writeToUsForm.SubmitButton = submitButtonText;
                        }
                        if (field != null)
                        {
                            ProgressData progressData = new ProgressData();
                            progressData.Heading = ProgressDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ThankYouDataSection.HeadingFieldId].Value;
                            progressData.Description = ProgressDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ThankYouDataSection.DescriptionFieldId].Value;
                            writeToUsForm.ProgressData = progressData;

                            FormFailData formFailData = new FormFailData();
                            formFailData.Heading = FormFailDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ThankYouDataSection.HeadingFieldId].Value;
                            formFailData.Description = FormFailDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ThankYouDataSection.DescriptionFieldId].Value;
                            writeToUsForm.FormFailData = formFailData;

                            ThankYouData thankYouData = new ThankYouData();
                            thankYouData.Heading = ThankYouDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ThankYouDataSection.HeadingFieldId].Value;
                            thankYouData.Description = ThankYouDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ThankYouDataSection.DescriptionFieldId].Value;
                            writeToUsForm.ThankYouData = thankYouData;


                            FormGTMData formGTMData = new FormGTMData();
                            formGTMData.SubmitEvent = FormGTMDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.FormGTMDataSection.GtmEvent].Value;
                            formGTMData.GtmCategory = FormGTMDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.FormGTMDataSection.GtmCategory].Value;
                            formGTMData.GtmSubCategory = FormGTMDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.FormGTMDataSection.GtmSubCategory].Value;
                            formGTMData.FailEvent = FormGTMDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.FormGTMDataSection.GtmTitle].Value;
                            formGTMData.PageType = FormGTMDatafieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.FormGTMDataSection.GtmpageType].Value;
                            writeToUsForm.FormGTMData = formGTMData;

                            ReCaptchaField reCaptchaField = new ReCaptchaField();
                            reCaptchaField.FieldType = ReCaptchaFieldfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ReCaptchaFieldSection.Title].Value;
                            reCaptchaField.FieldName = ReCaptchaFieldfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ReCaptchaFieldSection.Heading].Value;
                            reCaptchaField.FieldID = ReCaptchaFieldfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ReCaptchaFieldSection.SubTitle].Value;
                            ErrorMessages errorMessage = new ErrorMessages();
                            errorMessage.RequiredFieldErrorMessage = ErrMsgfieldItem.Fields[BaseTemplate.WriteToUsFormTemplate.ErrorMessagesSection.RequiredFieldErrorMessagereCaptcha].Value;
                            reCaptchaField.ErrorMessages = errorMessage;
                            reCaptchaField.Required = Helpers.Utils.GetBoleanValue(ReCaptchaFieldfieldItem, BaseTemplate.WriteToUsFormTemplate.ReCaptchaFieldSection.Active);
                            writeToUsForm.ReCaptchaField = reCaptchaField;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return writeToUsForm;
        }
    }
}