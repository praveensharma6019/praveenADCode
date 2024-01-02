using Project.AmbujaCement.Website.Helpers;
using Project.AmbujaCement.Website.Models.Forms;
using Project.AmbujaCement.Website.Templates.Forms;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AmbujaCement.Website.Services.Forms
{
    public class AmbujaFormsService : IAmbujaFormsService
    {
        public GetInTouchFormModel GetGetInTouchFormModel(Rendering rendering)
        {
            var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
            if (datasource == null)
            {
                Sitecore.Diagnostics.Log.Info("GetInTouchFormModel : Datasource is empty", this);
                return null;
            }
            try
            {
                var getInTouchFormModel = new GetInTouchFormModel();
                getInTouchFormModel.getInTouchForm = GetAmbujaGetInTouchFormModel(datasource);
                getInTouchFormModel.getIntouchOtp = GetGetInTouchOtpModel();

                var getInTouchFormDetailsItem = GetSitecoreItem(AmbujaFormsTemplate.GetInTouchFormDetailsTemplate.GetInTouchFormDetailsItemId);
                getInTouchFormModel.successMessage = getInTouchFormDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchFormDetailsTemplate.SuccessMessageFieldId].Value;
                getInTouchFormModel.progressMessage = getInTouchFormDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchFormDetailsTemplate.ProgressMessageFieldId].Value;
                getInTouchFormModel.errorMessage = getInTouchFormDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchFormDetailsTemplate.ErrorMessageFieldId].Value;

                return getInTouchFormModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private GetInTouchOtpModel GetGetInTouchOtpModel()
        {
            try
            {
                var getInTouchOtpDetailsItem = GetSitecoreItem(AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.GetInTouchOtpDetailsItemId);

                var getInTouchOtpModel = new GetInTouchOtpModel();
                getInTouchOtpModel.heading = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.HeadingFieldId].Value;
                getInTouchOtpModel.wehavesentviaSMStoLabel = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.WehavesentviaSMStoLabelFieldId].Value;
                getInTouchOtpModel.editButtonLabel = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.EditButtonLabelFieldId].Value;
                getInTouchOtpModel.submitButtonText = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.SubmitButtonTextFieldId].Value;
                getInTouchOtpModel.resendButtonLabel = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.ResendButtonLabelFieldId].Value;
                getInTouchOtpModel.notReceivedOtpLabel = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.NotReceivedOtpLabelFieldId].Value;
                getInTouchOtpModel.youWillReceiveOtpLabel = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.YouWillReceiveOtpLabelFieldId].Value;
                getInTouchOtpModel.secondLabel = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.SecondLabelFieldId].Value;
                getInTouchOtpModel.editMobileNOTitle = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.EditButtonLabelFieldId].Value;
                getInTouchOtpModel.resendOtpTitle = getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.ResendOtpTitleFieldId].Value;
                getInTouchOtpModel.timer = Convert.ToInt32(getInTouchOtpDetailsItem.Fields[AmbujaFormsTemplate.GetInTouchOtpDetailsTemplate.TimerFieldId].Value);

                return getInTouchOtpModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private AmbujaFormsModel GetAmbujaGetInTouchFormModel(Item formsDataSourceItem)
        {
            if (formsDataSourceItem == null)
            {
                Sitecore.Diagnostics.Log.Info("GetAmbujaGetInTouchFormModel : Datasource is empty", this);
                return null;
            }
            try
            {
                var getInTouchForm = new AmbujaFormsModel();

                getInTouchForm.FormFields = new List<FormFieldSection>();
                getInTouchForm.AntiforgeryToken = Utils.GetAntiForgeryToken();

                var firstNameField = AmbujaFormsTemplate.GetInTouchFormTemplate.FirstName;
                var lastNameField = AmbujaFormsTemplate.GetInTouchFormTemplate.LastName;
                var mobileField = AmbujaFormsTemplate.GetInTouchFormTemplate.Mobile;
                var emailField = AmbujaFormsTemplate.GetInTouchFormTemplate.Email;
                var lookingForDropDownField = AmbujaFormsTemplate.GetInTouchFormTemplate.LookingForDropDown;
                var queryTypeDropDownField = AmbujaFormsTemplate.GetInTouchFormTemplate.QueryTypeDropDown;
                var statesDropdownField = AmbujaFormsTemplate.GetInTouchFormTemplate.StatesDropdown;
                var districtDropdownField = AmbujaFormsTemplate.GetInTouchFormTemplate.DistrictDropdown;
                var termsAndConditionsCheckboxField = AmbujaFormsTemplate.GetInTouchFormTemplate.TermsAndConditionsCheckbox;
                var submitButton = AmbujaFormsTemplate.GetInTouchFormTemplate.SubmitButton;

                var sectionItem = formsDataSourceItem.Children.FirstOrDefault()?.Children.FirstOrDefault();
                if (sectionItem != null)
                {
                    var ambujaFormDetailsItem = GetSitecoreItem(AmbujaFormsTemplate.AmbujaFormDetailsTemplate.AmbujaFormDetailsItemID);
                    
                    getInTouchForm.Heading = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.HeadingFieldID].Value;
                    getInTouchForm.Description = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DescriptionFieldID].Value;
                    getInTouchForm.SubmitButtonText = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.SubmitButtonTextFieldID].Value;
                    getInTouchForm.CancelButtonText = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.CancelButtonTextFieldID].Value;

                    foreach (Item field in sectionItem.Children)
                    {
                        if (field.ID == firstNameField || field.ID == lastNameField || field.ID == mobileField ||field.ID == emailField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();

                            formFieldSection.FieldType = field.Fields[AmbujaFormsTemplate.FormFieldsSection.DefaultValue].Value;
                            formFieldSection.FieldName = field.Fields[AmbujaFormsTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, AmbujaFormsTemplate.FormFieldsSection.Required);
                            formFieldSection.Placeholder = field.Fields[AmbujaFormsTemplate.FormFieldsSection.PlaceholderText].Value;
                            formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[AmbujaFormsTemplate.FormFieldsSection.minRequiredLength].Value);
                            formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[AmbujaFormsTemplate.FormFieldsSection.maxAllowedLength].Value);

                            if (field.ID == firstNameField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredNameFieldErrorMessageFieldID].Value;
                                errorMessage.MaxLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MaxLengthErrorMessageFieldID].Value;
                                errorMessage.MinLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MinLengthErrorMessageFieldID].Value;
                                errorMessage.RegexErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RegexErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == lastNameField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredNameFieldErrorMessageFieldID].Value;
                                errorMessage.MaxLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MaxLengthErrorMessageFieldID].Value;
                                errorMessage.MinLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MinLengthErrorMessageFieldID].Value;
                                errorMessage.RegexErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RegexErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == emailField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredEmailFieldErrorMessageFieldID].Value;
                                errorMessage.MaxLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MaxLengthErrorMessageFieldID].Value;
                                errorMessage.MinLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MinLengthErrorMessageFieldID].Value;
                                errorMessage.RegexErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RegexErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            if (field.ID == mobileField)
                            {
                                ErrorMessage errorMessage = new ErrorMessage();
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredMobileFieldErrorMessageFieldID].Value;
                                errorMessage.MaxLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MaxLengthErrorMessageFieldID].Value;
                                errorMessage.MinLengthErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.MinLengthErrorMessageFieldID].Value;
                                errorMessage.RegexErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RegexErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            getInTouchForm.FormFields.Add(formFieldSection);
                        }

                        if (field.ID == lookingForDropDownField || field.ID == queryTypeDropDownField || field.ID == statesDropdownField || field.ID == districtDropdownField)
                        {
                            FormFieldSection formFieldSection = new FormFieldSection();
                            formFieldSection.Placeholder = field.Fields[AmbujaFormsTemplate.FormFieldsSection.ValueProviderParameters].Value;
                            if (!string.IsNullOrEmpty(field.Fields[AmbujaFormsTemplate.FormFieldsSection.minRequiredLength].Value))
                            {
                                formFieldSection.MinRequiredLength = Convert.ToInt32(field.Fields[AmbujaFormsTemplate.FormFieldsSection.minRequiredLength].Value);
                            }
                            if (!string.IsNullOrEmpty(field.Fields[AmbujaFormsTemplate.FormFieldsSection.maxAllowedLength].Value))
                            {
                                formFieldSection.MaxAllowedLength = Convert.ToInt32(field.Fields[AmbujaFormsTemplate.FormFieldsSection.maxAllowedLength].Value);
                            }
                            formFieldSection.IsClear = true;
                            formFieldSection.Required = Utils.GetBoleanValue(field, AmbujaFormsTemplate.FormFieldsSection.Required);
                            formFieldSection.FieldName = field.Fields[AmbujaFormsTemplate.FormFieldsSection.Title].Value;
                            formFieldSection.FieldType = field.Fields[AmbujaFormsTemplate.FormFieldsSection.DefaultSelection].Value;
                            List<FieldOption> FieldOptionlist = new List<FieldOption>();
                            if (field.ID == lookingForDropDownField)
                            {
                                var lookingForDropDownId = 0;
                                var lookingForDropDownOptionsFolderItem = GetSitecoreItem(AmbujaFormsTemplate.AmbujaFormDetailsTemplate.LookingForOptionsFolderItemID);
                                if (lookingForDropDownOptionsFolderItem.Children.Any())
                                {
                                    foreach (Item lookingForOption in lookingForDropDownOptionsFolderItem.Children)
                                    {
                                        if (lookingForDropDownId != 0)
                                        {
                                            FieldOption fieldOption = new FieldOption();
                                            fieldOption.Label = lookingForOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionLabelFieldID].Value;
                                            fieldOption.Id = lookingForOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionValueFieldID].Value;
                                            fieldOption.subOptions = new List<FieldSubOption>();
                                            if (lookingForOption.Children.Any())
                                            {
                                                foreach (Item subOptions in lookingForOption.Children)
                                                {
                                                    var fieldSubOption = new FieldSubOption();
                                                    fieldSubOption.Label = subOptions.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionLabelFieldID].Value;
                                                    fieldSubOption.Id = subOptions.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionValueFieldID].Value;
                                                    fieldOption.subOptions.Add(fieldSubOption);
                                                }
                                            }
                                            //fieldOption.ParentId = lookingForOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionParentIdFieldID].Value;
                                            FieldOptionlist.Add(fieldOption);
                                        }
                                        lookingForDropDownId++;
                                    }
                                    formFieldSection.FieldOptions = FieldOptionlist;
                                }

                            }
                            #region
                            //if (field.ID == queryTypeDropDownField)
                            //{
                            //    var queryTypeDropDownId = 0;
                            //    var queryTypeDropDownOptionsFolderItem = GetSitecoreItem(AmbujaFormsTemplate.AmbujaFormDetailsTemplate.QueryTypeFolderItemID);
                            //    if (queryTypeDropDownOptionsFolderItem.Children.Any())
                            //    {
                            //        foreach (Item queryTypeOption in queryTypeDropDownOptionsFolderItem.Children)
                            //        {
                            //            if (queryTypeDropDownId != 0)
                            //            {
                            //                FieldOption fieldOption = new FieldOption();
                            //                fieldOption.Label = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionLabelFieldID].Value;
                            //                fieldOption.Id = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionValueFieldID].Value;
                            //                fieldOption.ParentId = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionParentIdFieldID].Value;
                            //                FieldOptionlist.Add(fieldOption);
                            //            }
                            //            queryTypeDropDownId++;
                            //        }
                            //        formFieldSection.FieldOptions = FieldOptionlist;
                            //    }

                            //}
                            #endregion
                            if (field.ID == statesDropdownField)
                            {
                                var statesDropdownId = 0;
                                var statesDropDownOptionsFolderItem = GetSitecoreItem(AmbujaFormsTemplate.AmbujaFormDetailsTemplate.StatesFolderItemID);
                                if (statesDropDownOptionsFolderItem.Children.Any())
                                {
                                    foreach (Item queryTypeOption in statesDropDownOptionsFolderItem.Children)
                                    {
                                        if (statesDropdownId != 0)
                                        {
                                            FieldOption fieldOption = new FieldOption();
                                            fieldOption.Label = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionLabelFieldID].Value;
                                            fieldOption.Id = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionValueFieldID].Value;
                                            fieldOption.subOptions = new List<FieldSubOption>();
                                            if (queryTypeOption.Children.Any())
                                            {
                                                foreach (Item subOptions in queryTypeOption.Children)
                                                {
                                                    var fieldSubOption = new FieldSubOption();
                                                    fieldSubOption.Label = subOptions.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionLabelFieldID].Value;
                                                    fieldSubOption.Id = subOptions.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionValueFieldID].Value;
                                                    fieldOption.subOptions.Add(fieldSubOption);
                                                }
                                            }
                                            //fieldOption.ParentId = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionParentIdFieldID].Value;
                                            FieldOptionlist.Add(fieldOption);
                                        }
                                        statesDropdownId++;
                                    }
                                    formFieldSection.FieldOptions = FieldOptionlist;
                                }

                            }
                            #region
                            ////if (field.ID == districtDropdownField)
                            ////{
                            ////    var districtDropdownId = 0;
                            ////    var districtDropDownOptionsFolderItem = GetSitecoreItem(AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DistrictFolderItemID);
                            ////    if (districtDropDownOptionsFolderItem.Children.Any())
                            ////    {
                            ////        foreach (Item queryTypeOption in districtDropDownOptionsFolderItem.Children)
                            ////        {
                            ////            if (districtDropdownId != 0)
                            ////            {
                            ////                FieldOption fieldOption = new FieldOption();
                            ////                fieldOption.Label = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionLabelFieldID].Value;
                            ////                fieldOption.Id = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionValueFieldID].Value;
                            ////                fieldOption.ParentId = queryTypeOption.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.DropdownOptionParentIdFieldID].Value;
                            ////                FieldOptionlist.Add(fieldOption);
                            ////            }
                            ////            districtDropdownId++;
                            ////        }
                            ////        formFieldSection.FieldOptions = FieldOptionlist;
                            ////    }

                            ////}
                            #endregion
                            ErrorMessage errorMessage = new ErrorMessage();
                            if (field.ID == lookingForDropDownField)
                            {
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredLookingForFieldErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            if (field.ID == queryTypeDropDownField)
                            {
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredQueryTypeFieldErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            if (field.ID == statesDropdownField)
                            {
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredStateFieldErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }
                            if (field.ID == districtDropdownField)
                            {
                                errorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredDistrictFieldErrorMessageFieldID].Value;
                                formFieldSection.ErrorMessages = errorMessage;
                            }

                            getInTouchForm.FormFields.Add(formFieldSection);
                        }

                        if (field.ID == termsAndConditionsCheckboxField)
                        {
                            CheckField checkFieldSection = new CheckField();
                            checkFieldSection.FieldType = "checkbox";

                            var termsAndConditionsCheckboxItem = GetSitecoreItem(field.ID);
                            checkFieldSection.FieldName = termsAndConditionsCheckboxItem.Name;

                            checkFieldSection.FieldID = "acceptTerms";
                            checkFieldSection.Placeholder = field.Fields[AmbujaFormsTemplate.FormFieldsSection.Title].Value;
                            checkFieldSection.Required = Utils.GetBoleanValue(field, AmbujaFormsTemplate.FormFieldsSection.Required);
                            checkFieldSection.Selected = Utils.GetBoleanValue(field, AmbujaFormsTemplate.FormFieldsSection.CheckboxDefaultValue);

                            ErrorMessageCheckField checkBoxErrorMessage = new ErrorMessageCheckField();
                            checkBoxErrorMessage.RequiredFieldErrorMessage = ambujaFormDetailsItem.Fields[AmbujaFormsTemplate.AmbujaFormDetailsTemplate.RequiredCheckboxFieldErrorMessageFieldID].Value;
                            checkFieldSection.ErrorMessages = checkBoxErrorMessage;
                            getInTouchForm.CheckboxField = checkFieldSection;
                        }
                    }
                    return getInTouchForm;
                }
                return null;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in GetRequestACallForm, exception:" + ex, ex);
                throw ex;
            }
        }

        private Item GetSitecoreItem(ID id)
        {
            return Sitecore.Context.Database.GetItem(id);
        }
    }
}