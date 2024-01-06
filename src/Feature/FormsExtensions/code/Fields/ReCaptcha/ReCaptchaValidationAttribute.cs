using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Feature.FormsExtensions.Business.ReCaptcha;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.ContentSearch.Pipelines.ResolveBoost.ResolveItemBoost;
using Sitecore.DependencyInjection;
using Sitecore.Globalization;

namespace Feature.FormsExtensions.Fields.ReCaptcha
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReCaptchaValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly IReCaptchaService reCaptchaService = ServiceLocator.ServiceProvider.GetService<IReCaptchaService>();
        private readonly string secretKey;

        public ReCaptchaValidationAttribute()
        {
            secretKey = Sitecore.Configuration.Settings.GetSetting("GoogleCaptchaPrivateKey");
            reCaptchaService = new ReCaptchaService(secretKey);
        }
        public override bool IsValid(object value)
        {
            return reCaptchaService.VerifySync((string)value, secretKey);
        }
    
        public override string FormatErrorMessage(string name)
        {
            return Translate.Text(this.ErrorMessageString);
        }

        IEnumerable<ModelClientValidationRule> IClientValidatable.GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "required"
            };
            yield return rule;
        }

    }

}