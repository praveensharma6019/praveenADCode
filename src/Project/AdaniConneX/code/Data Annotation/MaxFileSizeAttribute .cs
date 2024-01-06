using Sitecore.AdaniConneX.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniConneX.Website
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "406";
        private readonly int _maxFileSize;        
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }        

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
           
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