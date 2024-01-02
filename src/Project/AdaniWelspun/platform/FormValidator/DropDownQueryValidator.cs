using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adani.BAU.AdaniWelspunSXA.Project.Model;
using Newtonsoft.Json;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Mvc.Models.Validation;
using Sitecore.StringExtensions;

namespace Adani.BAU.AdaniWelspunSXA.Project.FormValidator
{
    public class DropDownQueryValidator : ValidationElement<string>
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

        public DropDownQueryValidator(ValidationDataModel validationItem) : base(validationItem)
        {

        }

        public override void Initialize(object validationModel)
        {
          
            if (validationModel is DropDownListViewModel stringInputViewModel)
                this.Title = stringInputViewModel.Title;

            if (this.ValidationItem.Name == "DropDownQueryValidator")
            {
                if(this.ValidationItem.Parameters.Length > 0)
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
                    var childlist = queryDropDownItem.GetChildren().Where(x => x.Name == ddlValue).ToList();
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