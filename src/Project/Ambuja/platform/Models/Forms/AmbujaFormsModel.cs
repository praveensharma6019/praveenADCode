using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.Forms
{
    public class AmbujaFormsModel
    {
        public string AntiforgeryToken { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string SubmitButtonText { get; set; }
        public string CancelButtonText { get; set; }
        public List<FormFieldSection> FormFields { get; set; }
        public CheckField CheckboxField { get; set; }
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
        public List<FieldOption> FieldOptions { get; set; }
        public ErrorMessage ErrorMessages { get; set; }

    }

    public class CheckField
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldID { get; set; }
        public string Placeholder { get; set; }
        public bool Selected { get; set; }
        public bool Required { get; set; }
        public ErrorMessageCheckField ErrorMessages { get; set; }
    }

    public class ErrorMessageCheckField : RequiredField
    {
    }

    public class FieldOption
    {
        public string Label { get; set; }
        public string Id { get; set; }
        public List<FieldSubOption> subOptions { get; set; }
    }
    public class FieldSubOption
    {
        public string Label { get; set; }
        public string Id { get; set; }
    }

    public class ErrorMessage : RequiredField
    {
        public string MaxLengthErrorMessage { get; set; }
        public string MinLengthErrorMessage { get; set; }
        public string RegexErrorMessage { get; set; }
    }

    public class ErrorMessageReCaptchaField : RequiredField
    {
    }

    public class RequiredField
    {
        public string RequiredFieldErrorMessage { get; set; }
    }
}