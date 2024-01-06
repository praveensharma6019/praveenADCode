using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class ContactFormData
    {
        public string AntiforgeryToken { get; set; }
        public string Heading { get; set; }
        public List<FormFieldSection> FieldSectionData { get; set; }
        public string Terms { get; set; }
        public SubmitButtonText SubmitButton { get; set; }
        public ThankYouData ThankYouData { get; set; }
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
        public int MinAllowedLength { get; set; }
        public List<DropDownOptionFields> FieldOptions { get; set; }
        public ErrorMessage ErrorMessages { get; set; }
    }

    public class DropDownOptionFields
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

    public class ThankYouData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string CTAText { get; set; }
        public string CTALink { get; set; }
        public bool isExternalLink { get; set; }
    }
}