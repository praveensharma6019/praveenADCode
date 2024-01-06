using Sitecore.AdaniGreenTalks.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website
{
    public class FileNameAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "Upload file with correct format, size and name";
        private readonly string _fileName;
        public FileNameAttribute(string fileName)
        {
            _fileName = fileName;
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
            
            else
            {
                Regex alphaNumber = new Regex("^[a-zA-Z0-9-_ ]+$");
                //file mime type
                string fName = Path.GetFileNameWithoutExtension(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);
                char c = '.';
                int occurance = fName.Count(f => (f == c));
                if (occurance > 1)
                {
                    return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
                }
                //file extension               
                if (fileExtension.ToLower() != _fileName.ToLower())
                {
                    return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
                }
                //file name              
                if (!alphaNumber.IsMatch(fName))
                {
                    return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
                }
                return ValidationResult.Success;

            }

        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(_fileName.ToString());
        }
    }

}