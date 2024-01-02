using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using System;

namespace Adani.BAU.AdaniWelspunSXA.Project.FormValidator.FormFields
{
    [Serializable]
    public class GoogleRecaptchaV3 : StringInputViewModel
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
     
        public string ErrorMessage { get; set; }

        protected override void InitItemProperties(Item item)
        {
            base.InitItemProperties(item);

            ApiKey = Sitecore.StringUtil.GetString(item.Fields["Api Key"]);
            SecretKey = Sitecore.StringUtil.GetString(item.Fields["Secret Key"]);
     
            ErrorMessage = Sitecore.StringUtil.GetString(item.Fields["Error Message"]);
        }

        protected override void UpdateItemFields(Item item)
        {
            base.UpdateItemFields(item);

            item.Fields["Api Key"].SetValue(ApiKey, true);
            item.Fields["Secret Key"].SetValue(SecretKey, true);
            item.Fields["Error Message"].SetValue(ErrorMessage, true);
        }
    }
}