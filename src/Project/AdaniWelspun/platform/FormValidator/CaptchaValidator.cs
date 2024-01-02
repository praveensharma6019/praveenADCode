using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Mvc;
using Sitecore.ExperienceForms.Mvc.Models.Validation;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.Data;
using System.Net.Http;
using Sitecore.ExperienceForms.Mvc.Extensions;
using System.Web.Optimization;

namespace Adani.BAU.AdaniWelspunSXA.Project.FormValidator
{
    public class CaptchaValidator : ValidationElement<string>
    {
        public override IEnumerable<ModelClientValidationRule> ClientValidationRules
        {
            get
            {
                if (string.IsNullOrEmpty(this.ApiKey) || string.IsNullOrEmpty(this.SecretKey))
                {
                    yield break;
                }
            }
        }

        protected virtual string ApiKey { get; set; }
        protected virtual string SecretKey { get; set; }
        protected virtual string Title { get; set; }
        protected virtual string FieldName { get; set; }

        public CaptchaValidator(ValidationDataModel validationItem) : base(validationItem)
        {

        }


        public override void Initialize(object validationModel)
        {
            base.Initialize(validationModel);
            StringInputViewModel stringInputViewModel = validationModel as StringInputViewModel;
            if (stringInputViewModel != null)
            {
                var fieldItem = Sitecore.Context.Database.GetItem(ID.Parse(stringInputViewModel.ItemId));
                if (fieldItem != null)
                {
                    this.ApiKey = fieldItem["Api Key"];
                    this.SecretKey = fieldItem["Secret Key"];
                }
                this.Title = stringInputViewModel.Title;
                this.FieldName = stringInputViewModel.Name;
            }
        }

        public override ValidationResult Validate(object value)
        {
            if (value == null)
            {
                return new ValidationResult("Captcha is Inavlid.");// ValidationResult.Success;
            }
           
            var isCaptchaValid = ValidateCaptcha((string)value, this.SecretKey);
            if (!isCaptchaValid)
            {
                return new ValidationResult(this.FormatMessage(new object[] { this.Title }));
            }
            return ValidationResult.Success;
        }
        public static bool ValidateCaptcha(string response, string secret)
        {
            HttpClient httpClient = new HttpClient();

            var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=" + secret + "&response=" + response + "").Result;

            if (res.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }
            
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);

            if (JSONdata.success != "true" || JSONdata.score <= 0.5)
            {
                return false;
            }

            return true;
        }
    }
}