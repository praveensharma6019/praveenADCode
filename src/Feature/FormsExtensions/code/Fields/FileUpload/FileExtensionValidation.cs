using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Sitecore.ExperienceForms.Mvc.Models.Validation;

namespace Feature.FormsExtensions.Fields.FileUpload
{
    public class FileExtensionValidation : ValidationElement<string>
    {

        public FileExtensionValidation(ValidationDataModel validationItem) : base(validationItem)
        {

        }

        public override void Initialize(object validationModel)
        {
            base.Initialize(validationModel);
            if (!(validationModel is FileUploadModel fileUploadModel))
                return;
        }

        public override ValidationResult Validate(object value)
        {
            var postedFile = (HttpPostedFileBase)value;
            List<string> supportedTypes = new List<string> { "pdf", "docx", "jpeg" };
            var postedFileExtension = System.IO.Path.GetExtension(postedFile.FileName).Substring(1);
            if (postedFile == null || supportedTypes.Contains(postedFileExtension))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("File Format not Allowed");
        }


        public override IEnumerable<ModelClientValidationRule> ClientValidationRules
        {
            get
            {
                var rule = new ModelClientValidationRule
                {
                    ErrorMessage = "File Format not Allowed",
                    ValidationType = "fileextensiontype"
                };
                yield return rule;
            }
        }
    }
}