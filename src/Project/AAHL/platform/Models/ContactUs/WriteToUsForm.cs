using System.Collections.Generic;

namespace Project.AAHL.Website.Models.ContactUs
{
    public class WriteToUsForm
    {
        public List<FormFieldSection> FormFields { get; set; }
        public ReCaptchaField ReCaptchaField { get; set; }
        public SubmitButton SubmitButton { get; set; }
        public FormGTMData FormGTMData { get; set; }
        public ThankYouData ThankYouData { get; set; }
        public ProgressData ProgressData { get; set; }
        public FormFailData FormFailData { get; set; }
        public string Theme { get; set; }
    }
    public class FormFieldSection
    {
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public bool IsClear { get; set; }
        public bool Required { get; set; }
        public string Placeholder { get; set; }
        public int MaxAllowedLength { get; set; }
        public int MinRequiredLength { get; set; }
        public ErrorMessages ErrorMessages { get; set; }
        public List<FieldOption> FieldOptions { get; set; }
        public List<CountryCodeOptions> CountryCodeOptions { get; set; }
    }

    public class ErrorMessages
    {
        public string RequiredFieldErrorMessage { get; set; }
        public string MaxLengthErrorMessage { get; set; }
        public string MinLengthErrorMessage { get; set; }
        public string RegexErrorMessage { get; set; }
    }

    public class FieldOption
    {
        public string Label { get; set; }
        public string Id { get; set; }
    }
    public class ReCaptchaField
    {
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public string FieldID { get; set; }
        public ErrorMessages ErrorMessages { get; set; }
        public bool Required { get; set; }
    }

    public class SubmitButton
    {
        public string ButtonText { get; set; }
    }

    public class FormGTMData
    {
        public string SubmitEvent { get; set; }
        public string GtmCategory { get; set; }
        public string GtmSubCategory { get; set; }
        public string PageType { get; set; }
        public string FailEvent { get; set; }
    }

    public class ThankYouData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
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

    public class CountryCodeOptions
    {
        public string countryName { get; set; }
        public string dialCode { get; set; }
        public string isO3 { get; set; }
        public string isO2 { get; set; }
        public string countryFlagImage { get; set; }
    }

}