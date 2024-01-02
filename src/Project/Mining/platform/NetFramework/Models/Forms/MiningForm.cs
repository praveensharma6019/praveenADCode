using Project.Mining.Website.Models.Common;
using System.Collections.Generic;

namespace Project.Mining.Website.Models.Forms
{
    public class MiningForm
    {
        public string AntiforgeryToken { get; set; }
        public List<FormFieldSection> FormFields { get; set; }
        public SubmitButtonText SubmitButton { get; set; }
        public ReCaptchaField ReCaptchaField { get; set; }
        public FormThankyouData ThankYouData { get; set; }
        public FormProgressData ProgressData { get; set; }
        public FormFailedData FormFailData { get; set; }
    }

    public class FormFieldSection
    {
        public string Text { get; set; }
        public string HtmlTag { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public bool Required { get; set; }
        public bool IsClear { get; set; }
        public string Placeholder { get; set; }
        public int MaxAllowedLength { get; set; }
        public int MinRequiredLength { get; set; }
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
    }

    public class SubmitButtonText
    {
        public string ButtonText { get; set; }
    }

    public class ReCaptchaField
    {
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public string FieldID { get; set; }
        public ErrorMessageReCaptchaField ErrorMessages { get; set; }
        public bool Required { get; set; }
    }

    public class ErrorMessageReCaptchaField
    {
        public string RequiredFieldErrorMessage { get; set; }
    }

    public class FormProgressData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }
    public class FormThankyouData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string CtaText { get; set; }
        public string CtaLink { get; set; }
        public string Class { get; set; }
        public string Image { get; set; }
        public string MobileImage { get; set; }
        public string TabletImage { get; set; }
        public string ImageAlt { get; set; }

    }
    public class FormFailedData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }
}