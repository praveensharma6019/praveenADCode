using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using static Project.AdaniInternationalSchool.Website.Templates.CareerFormRootSection.Fields;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class CareerFormData
    {
        public string AntiforgeryToken { get; set; }
        public string SectionID { get; set; }
        public string SectionHeading { get; set; }
        public string Theme { get; set; }
        public List<FormFieldSection> FormFields { get; set; }
        public SubmitButtonText SubmitButton { get; set; }
        public CheckField CheckboxField { get; set; }
        public ReCaptchaField ReCaptchaField { get; set; }
        public FormGTMData FormGTMData { get; set; }
        public ThankYouData ThankYouData { get; set; }
        public ProgressData ProgressData { get; set; }
        public FormFailData FormFailData { get; set; }
    }
    public class ProgressData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }
    public class FormFailData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }
    public class FormGTMData
    {
        public string SubmitEvent { get; set; }
        public string GtmCategory { get; set; }
        public string GtmSubCategory { get; set; }
        public string PageType { get; set; }
        public string FailEvent { get; set; }
    }

    public class ReCaptchaField
    {
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public string FieldID { get; set; }
        public ErrorMessageReCaptchaField ErrorMessages { get; set; }
        public bool Required { get; set; }
    }

    public class ThankYouData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }
    public class CheckField
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldID { get; set; }
        public string Url { get; set; }
        public string Placeholder { get; set; }
        public string Target { get; set; }
        public bool Selected { get; set; }
        public bool Required { get; set; }
        public ErrorMessageCheckField ErrorMessages { get; set; }
    }
    public class FormFieldSection
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldDescription { get; set; }
        public bool Required { get; set; }
        public bool IsClear { get; set; }
        public string Placeholder { get; set; }
        public int MaxAllowedLength { get; set; }
        public int MinRequiredLength { get; set; }
        public int MaxAllowedFileSize { get; set; }
        public double MinRequiredFileSize { get; set; }

        public List<FieldOption> FieldOptions { get; set; }
        public ErrorMessage ErrorMessages { get; set; }

    }
    public class FieldOption
    {
        public string Label { get; set; }
        public string Id { get; set; }
    }

    public class ErrorMessage
    {
        public string RequiredFieldErrorMessage { get; set; }
        public string MaxLengthErrorMessage { get; set; }
        public string MinLengthErrorMessage { get; set; }
        public string RegexErrorMessage { get; set; }
        public string MaxFileSizeErrorMessage { get; set; }
        public string MinFileSizeErrorMessage { get; set; }
    }
    public class ErrorMessageReCaptchaField
    {
        public string RequiredFieldErrorMessage { get; set; }
    } 
    public class ErrorMessageCheckField
    {
        public string RequiredFieldErrorMessage { get; set; }
    }
    public class SubmitButtonText
    {
        public string ButtonText { get; set; }
    }

}