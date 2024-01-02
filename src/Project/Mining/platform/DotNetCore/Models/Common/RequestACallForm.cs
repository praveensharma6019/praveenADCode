using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.Common
{
    public class RequestACallForm
    {
        [SitecoreComponentField]
        public virtual string? AntiforgeryToken { get; set; }
        [SitecoreComponentField]
        public virtual IEnumerable<FormFieldSection>? FormFields { get; set; }
        [SitecoreComponentField]
        public virtual SubmitButtonText? SubmitButton { get; set; }
        [SitecoreComponentField]
        public virtual ReCaptchaField? ReCaptchaField { get; set; }
        [SitecoreComponentField]
        public virtual FormThankyouData? ThankYouData { get; set; }
        [SitecoreComponentField]
        public virtual FormProgressData? ProgressData { get; set; }
        [SitecoreComponentField]
        public virtual FormFailedData? FormFailData { get; set; }
    }

    public class FormFieldSection
    {
        public virtual string? Text { get; set; }
        public virtual string? HtmlTag { get; set; }
        public virtual string? FieldName { get; set; }
        public virtual string? FieldType { get; set; }
        public virtual bool? Required { get; set; }
        public virtual bool? IsClear { get; set; }
        public virtual string? Placeholder { get; set; }
        public virtual int? MaxAllowedLength { get; set; }
        public virtual int? MinRequiredLength { get; set; }
        public virtual IEnumerable<FieldOption>? FieldOptions { get; set; }
        public virtual ErrorMessage? ErrorMessages { get; set; }

    }

    public class FieldOption
    {
        [SitecoreComponentField]
        public virtual string? Label { get; set; }
        [SitecoreComponentField]
        public virtual string? Id { get; set; }
    }

    public class ErrorMessage
    {
        [SitecoreComponentField]
        public virtual string? RequiredFieldErrorMessage { get; set; }
        [SitecoreComponentField]
        public virtual string? MaxLengthErrorMessage { get; set; }
        [SitecoreComponentField]
        public virtual string? MinLengthErrorMessage { get; set; }
        [SitecoreComponentField]
        public virtual string? RegexErrorMessage { get; set; }
    }

    public class SubmitButtonText
    {
        [SitecoreComponentField]
        public virtual string? ButtonText { get; set; }
    }

    public class ReCaptchaField
    {
        [SitecoreComponentField]
        public virtual string? FieldType { get; set; }
        [SitecoreComponentField]
        public virtual string? FieldName { get; set; }
        [SitecoreComponentField]
        public virtual string? FieldID { get; set; }
        [SitecoreComponentField]
        public virtual ErrorMessageReCaptchaField? ErrorMessages { get; set; }
        [SitecoreComponentField]
        public virtual bool? Required { get; set; }
    }

    public class ErrorMessageReCaptchaField
    {
        [SitecoreComponentField]
        public virtual string? RequiredFieldErrorMessage { get; set; }
    }

    public class FormProgressData
    {
        [SitecoreComponentField]
        public virtual string? Heading { get; set; }
        [SitecoreComponentField]
        public virtual string? Description { get; set; }
    }
    public class FormThankyouData
    {
        [SitecoreComponentField]
        public virtual string? Heading { get; set; }
        [SitecoreComponentField]
        public virtual string? Description { get; set; }
        [SitecoreComponentField]
        public virtual string? CtaText { get; set; }
        [SitecoreComponentField]
        public virtual string? CtaLink { get; set; }
        [SitecoreComponentField]
        public virtual string? Class { get; set; }
        [SitecoreComponentField]
        public virtual string? Image { get; set; }
        [SitecoreComponentField]
        public virtual string? MobileImage { get; set; }
        [SitecoreComponentField]
        public virtual string? TabletImage { get; set; }
        [SitecoreComponentField]
        public virtual string? ImageAlt { get; set; }

    }
    public class FormFailedData
    {
        [SitecoreComponentField]
        public virtual string? Heading { get; set; }
        [SitecoreComponentField]
        public virtual string? Description { get; set; }
    }
}
