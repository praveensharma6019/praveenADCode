using Sitecore.AdaniGreenTalks.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Max 5 mb size file upload is allowed";
        private readonly int _maxFileSize;        
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            AdaniGreenTalks_JoinUs_Model loginModel = (AdaniGreenTalks_JoinUs_Model)validationContext.ObjectInstance;
            var file1 = loginModel.Doc_UploadProject as HttpPostedFileBase;

            var file = value as HttpPostedFileBase;
            if (file1 == null)
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
            }
            if (file == null && loginModel.Participate_AdaniPrizes == null)
            {
                return ValidationResult.Success;
            }
            if (file == null && loginModel.Participate_AdaniPrizes == "check")
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
            }
            if(!(file.ContentLength <= _maxFileSize))
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);

            }
            return ValidationResult.Success;

        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_maxFileSize.ToString());
        }
    }
}