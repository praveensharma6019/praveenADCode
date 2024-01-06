using Sitecore.ExperienceForms.Mvc.Models.Validation.Parameters;
using Sitecore.ExperienceForms.Mvc.Models.Validation;
using System;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Collections;
using Sitecore.Data.Items;
using System.Linq;
using Sitecore.Farmpik.Website.Models;

namespace Sitecore.Farmpik.Website.Data_Annotation
{
    public class SalutationValidator : ValidationElement<string>
    {

        public override IEnumerable<ModelClientValidationRule> ClientValidationRules
        {
            get
            {
                if (string.IsNullOrEmpty(this.Title))
                {
                    yield break;
                }
            }
        }

        protected virtual string Title { get; set; }
        protected virtual string DropDownID { get; set; }

        public SalutationValidator(ValidationDataModel validationItem) : base(validationItem)
        {
        }
        public override void Initialize(object validationModel)
        {


            if (this.ValidationItem.Name == "DropDownQueryValidator")
            {
                if (this.ValidationItem.Parameters.Length > 0)
                {
                    var parameter = string.IsNullOrEmpty(this.ValidationItem.Parameters) ? string.Empty : this.ValidationItem.Parameters;
                    DropDownParameterModel r = JsonConvert.DeserializeObject<DropDownParameterModel>(parameter);
                    if (r != null && !string.IsNullOrEmpty(r.DropDownID))
                    {
                        this.DropDownID = r.DropDownID;
                    }
                }

            }

        }
        public override ValidationResult Validate(object value)
        {
            if (value == null || string.IsNullOrEmpty(this.DropDownID))
            {
                return new ValidationResult("Invalid Input");// ValidationResult.Success;
            }
            var ddlValue = string.Empty;
            IList collection = (IList)value;
            if (collection != null && collection.Count > 0)
            {
                ddlValue = collection[0].ToString();
            }

            if (!string.IsNullOrEmpty(ddlValue))
            {
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                Item queryDropDownItem = db.GetItem(this.DropDownID);
                if (queryDropDownItem != null)
                {
                    var childlist = queryDropDownItem.GetChildren().Where(x => x.DisplayName == ddlValue).ToList();
                    if (childlist != null && childlist.Count > 0)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(this.FormatMessage(new object[] { this.Title }));
                    }
                }
                return new ValidationResult(this.FormatMessage(new object[] { this.Title }));
            }
            return new ValidationResult(this.FormatMessage(new object[] { this.Title }));
        }

    }
}