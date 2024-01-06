using Sitecore.AdaniConneX.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.AdaniConneX.Website
{
    public class FileNameAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "406";           

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as HttpPostedFileBase;
            string[] allowedExtenstions = new string[] { ".doc", ".docx", ".pdf" };
            if (file == null)
            {
                return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
            }           
            
            else
            {
                //Regex alphaNumber = new Regex("^[a-zA-Z0-9-_ ]+$");
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
                if (!allowedExtenstions.Contains(fileExtension))
                {
                    return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
                }
                //file name              
                //if (!alphaNumber.IsMatch(fName))
                //{
                //    return new ValidationResult(ErrorMessage ?? DefaultErrorMessage);
                //}
                return ValidationResult.Success;

            }

        }
       
    }

}